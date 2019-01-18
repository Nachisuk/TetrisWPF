using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TetrisWPF.Properties;
using TetrisWPF;

public abstract class FactoryCreator
{
    public abstract Tetromino FactoryMethod();
}

public class TetrominoFactory : FactoryCreator
{
    public override Tetromino FactoryMethod()
    {
        Random rnd = new Random();
        int typeId = rnd.Next(1, 7);
        switch (typeId)
        {
            case 1:
                return new Tetromino_I();
            case 2:
                return new Tetromino_J();
            case 3:
                return new Tetromino_L();
            case 4:
                return new Tetromino_O();
            case 5:
                return new Tetromino_S();
            case 6:
                return new Tetromino_T();
            case 7:
                return new Tetromino_Z();
            default:
                throw new ArgumentException("Poza zakresem", "zakres");
        }
    }
}
public abstract class Tetromino
{
    public abstract int[,] shape { get; set; }
    public abstract String shapeName { get; }
    public abstract int rotated { get; set; }
    public abstract Color tetriminoColor { get; set; }
    public List<int[]> lokacja = new List<int[]>();

    public bool Stwórz()
    {
        for (int i = 0; i < shape.GetLength(0); i++)
        {
            for (int j = 0; j < shape.GetLength(1); j++)
            {
                if (shape[i, j] == 1)
                {
                    lokacja.Add(new int[] { i, (10 - shape.GetLength(1)) / 2 + j });
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (GameBoard.lokacjaOstatniegoTetrisaGrid[this.lokacja[i][0], this.lokacja[i][1]] == 1)
            {
                return false;
            }
        }

        Aktualizuj();
        return true;
    }

    public static void Landslide()
    {
        Random rnd = new Random();
        int x_position = 0;
        int y_position = rnd.Next(0, 9);
        bool czyPonizej = false;
        int[] rndtable = new int[] { 1, 1, 1, 1, 1, 2, 2, 2 };
        int range = rndtable[rnd.Next(0, rndtable.Length)];
        while (!czyPonizej)
        {

            if (x_position + range >= GameBoard.Instance.GameBoard1.RowDefinitions.Count)
            {
                GameBoard.lokacjaOstatniegoTetrisaGrid[x_position, y_position] = 1;
                GameBoard.tetrisColorGrid[x_position, y_position] = 8;
                czyPonizej = true;
            }
            else if (x_position + range < GameBoard.Instance.GameBoard1.RowDefinitions.Count)
            {
                if (GameBoard.lokacjaOstatniegoTetrisaGrid[x_position + range, y_position] == 1)
                {
                    GameBoard.lokacjaOstatniegoTetrisaGrid[x_position, y_position] = 1;
                    GameBoard.tetrisColorGrid[x_position, y_position] = 8;
                    czyPonizej = true;
                }
            }
            x_position++;
        }
        GameBoard.Rysuj();
    }

    public void Opadaj()
    {
        Random rnd = new Random();

        //TODO nie skończone wersja TESTOWA
        if (czyJestCosPonizej())
        {
            for (int i = 0; i < 4; i++)
            {
                if (lokacja[i][0] >= 0)
                {
                    GameBoard.lokacjaOstatniegoTetrisaGrid[lokacja[i][0], lokacja[i][1]] = 1;
                    GameBoard.tetrisColorGrid[lokacja[i][0], lokacja[i][1]] = GameBoard.aktualnyKolor;
                }
            }
            GameBoard.czyOpadł = true;
        }
        else
        {
            for (int przesunięcie = 0; przesunięcie < 4; przesunięcie++)
            {
                lokacja[przesunięcie][0] += 1;
                //if (lokacja[przesunięcie][0] + 1 >= GameBoard.TetrisBoardHeight + 1) GameBoard.czyNaDole = true;
            }
            Aktualizuj();
        }
    }
    public bool czyJestCosPonizej()
    {
        for (int i = 0; i < 4; i++)
        {
            if (lokacja[i][0] + 1 >= GameBoard.Instance.GameBoard1.RowDefinitions.Count) return true;
            if (lokacja[i][0] + 1 < GameBoard.Instance.GameBoard1.RowDefinitions.Count)
            {
                if (lokacja[i][0] >= 0)
                    if (GameBoard.lokacjaOstatniegoTetrisaGrid[lokacja[i][0] + 1, lokacja[i][1]] == 1)
                    {
                        return true;
                    }
            }
        }
        return false;
    }

    public bool czyJestCosZLewa()
    {
        for (int i = 0; i < 4; i++)
        {
            if (lokacja[i][1] == 0)
            {
                return true;
            }
            else if (lokacja[i][0] >= 0 && GameBoard.lokacjaOstatniegoTetrisaGrid[lokacja[i][0], lokacja[i][1] - 1] == 1)
            {
                return true;
            }
        }
        return false;
    }

    public bool czyJestCosZPrawa()
    {
        for (int i = 0; i < 4; i++)
        {
            if (lokacja[i][1] == GameBoard.Instance.GameBoard1.ColumnDefinitions.Count - 1)
            {
                return true;
            }
            else if (lokacja[i][0] >= 0 && GameBoard.lokacjaOstatniegoTetrisaGrid[lokacja[i][0], lokacja[i][1] + 1] == 1)
            {
                return true;
            }
        }
        return false;
    }

    public void Aktualizuj()
    {
        for (int i = 0; i < GameBoard.Instance.GameBoard1.RowDefinitions.Count; i++)
        {
            for (int j = 0; j < GameBoard.Instance.GameBoard1.ColumnDefinitions.Count; j++)
            {
                GameBoard.grid[i, j] = 0;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (lokacja[i][0] >= 0)
                GameBoard.grid[lokacja[i][0], lokacja[i][1]] = 1;
        }
        GameBoard.Rysuj();
    }
    private void ObrocKsztalt()
    {
        //utworzenie nowej tablicy o odwrotnych wymiarach niz aktualna:
        int[,] tmpKształt = new int[this.shape.GetLength(1), this.shape.GetLength(0)];
        //transportowanie kolejnych "wartosci" pod polami:
        for (int i = 0; i < this.shape.GetLength(1); i++)
        {
            for (int j = 0; j < this.shape.GetLength(0); j++)
            {
                tmpKształt[i, j] = this.shape[this.shape.GetLength(0) - 1 - j, i];
            }
        }
        this.shape = tmpKształt;
    }

    public void Obroc()
    {

        //zrobienie buckupu w razie gdyby sie okazalo ze obrocony nachodzi na jakies juz w gridzie: (nie mozna zwyłym =this, bo tworzy kopię płytką)
        int[,] BU_kształt = this.shape.Clone() as int[,];
        List<int[]> BU_lokacja = new List<int[]>();
        for (int i = 0; i < 4; i++)
            BU_lokacja.Add(this.lokacja[i].Clone() as int[]);
        int BU_stopienObrocenia = this.rotated;

        switch (shapeName)
        {
            case "I":
                //sprawdzamy który z stopien obrocenia jest 0 - lezy czy 1 - pionowy
                switch (rotated)
                {
                    case 0:
                        //przekształcenie aby było w stopniu 1
                        ObrocKsztalt();
                        rotated++;
                        lokacja[3] = lokacja[1];
                        lokacja[0] = new int[] { lokacja[3][0] - 3, lokacja[3][1] };
                        lokacja[1] = new int[] { lokacja[3][0] - 2, lokacja[3][1] };
                        lokacja[2] = new int[] { lokacja[3][0] - 1, lokacja[3][1] };
                        break;

                    case 1:
                        ObrocKsztalt();
                        rotated = 0;
                        lokacja[1] = lokacja[3];
                        lokacja[0] = new int[] { lokacja[3][0], lokacja[3][1] - 1 };
                        lokacja[2] = new int[] { lokacja[3][0], lokacja[3][1] + 1 };
                        lokacja[3] = new int[] { lokacja[3][0], lokacja[3][1] + 2 };
                        break;
                }
                break;

            case "T":
                switch (rotated)
                {
                    case 0:
                        //przekształcenie aby było w stopniu 1
                        ObrocKsztalt();
                        rotated++;
                        lokacja[3] = lokacja[2];
                        lokacja[0] = new int[] { lokacja[3][0] - 2, lokacja[3][1] };
                        lokacja[1] = new int[] { lokacja[3][0] - 1, lokacja[3][1] };
                        lokacja[2] = new int[] { lokacja[3][0] - 1, lokacja[3][1] + 1 };
                        break;

                    case 1:
                        ObrocKsztalt();
                        rotated++;
                        lokacja[0] = new int[] { lokacja[1][0], lokacja[1][1] - 1 };
                        break;

                    case 2:
                        ObrocKsztalt();
                        rotated++;
                        lokacja[2] = lokacja[1];
                        lokacja[1] = lokacja[0];
                        lokacja[0] = new int[] { lokacja[2][0] - 1, lokacja[2][1] };
                        break;

                    case 3:
                        ObrocKsztalt();
                        rotated = 0;
                        lokacja[0] = lokacja[2];
                        lokacja[2] = lokacja[3];
                        lokacja[1] = new int[] { lokacja[2][0], lokacja[2][1] - 1 };
                        lokacja[3] = new int[] { lokacja[2][0], lokacja[2][1] + 1 };
                        break;
                }
                break;

            case "O":
                return;
                break;

            case "S":
                switch (rotated)
                {
                    case 0:
                        //przekształcenie aby było w stopniu 1
                        ObrocKsztalt();
                        rotated++;
                        lokacja[2] = lokacja[0];
                        lokacja[1] = new int[] { lokacja[2][0], lokacja[2][1] - 1 };
                        lokacja[0] = new int[] { lokacja[1][0] - 1, lokacja[1][1] };
                        break;

                    case 1:
                        ObrocKsztalt();
                        rotated = 0;
                        lokacja[0] = lokacja[2];
                        lokacja[2] = new int[] { lokacja[3][0], lokacja[3][1] - 1 };
                        lokacja[1] = new int[] { lokacja[0][0], lokacja[0][1] + 1 };
                        break;
                }
                break;

            case "Z":
                switch (rotated)
                {
                    case 0:
                        //przekształcenie aby było w stopniu 1
                        ObrocKsztalt();
                        rotated++;
                        lokacja[3] = lokacja[2];
                        lokacja[2] = new int[] { lokacja[1][0], lokacja[1][1] + 1 };
                        lokacja[0] = new int[] { lokacja[2][0] - 1, lokacja[2][1] };
                        break;

                    case 1:
                        ObrocKsztalt();
                        rotated = 0;
                        lokacja[2] = lokacja[3];
                        lokacja[0] = new int[] { lokacja[1][0], lokacja[1][1] - 1 };
                        lokacja[3] = new int[] { lokacja[2][0], lokacja[2][1] + 1 };
                        break;
                }
                break;

            case "J":
                switch (rotated)
                {
                    case 0:
                        //przekształcenie aby było w stopniu 1
                        ObrocKsztalt();
                        rotated++;
                        lokacja[3] = lokacja[2];
                        lokacja[2] = new int[] { lokacja[3][0] - 1, lokacja[3][1] };
                        lokacja[0] = new int[] { lokacja[2][0] - 1, lokacja[2][1] };
                        lokacja[1] = new int[] { lokacja[0][0], lokacja[0][1] + 1 };
                        break;

                    case 1:
                        ObrocKsztalt();
                        rotated++;
                        lokacja[1] = lokacja[2];
                        lokacja[2] = new int[] { lokacja[1][0], lokacja[1][1] + 1 };
                        lokacja[0] = new int[] { lokacja[1][0], lokacja[1][1] - 1 };
                        lokacja[3] = new int[] { lokacja[2][0] + 1, lokacja[2][1] };
                        break;

                    case 2:
                        ObrocKsztalt();
                        rotated++;
                        lokacja[0] = new int[] { lokacja[1][0] - 1, lokacja[1][1] };
                        lokacja[3] = new int[] { lokacja[1][0] + 1, lokacja[1][1] };
                        lokacja[2] = new int[] { lokacja[3][0], lokacja[3][1] - 1 };
                        break;

                    case 3:
                        ObrocKsztalt();
                        rotated = 0;
                        lokacja[1] = lokacja[2];
                        lokacja[2] = lokacja[3];
                        lokacja[0] = new int[] { lokacja[1][0] - 1, lokacja[1][1] };
                        lokacja[3] = new int[] { lokacja[2][0], lokacja[2][1] + 1 };
                        break;
                }
                break;

            case "L":
                switch (rotated)
                {
                    case 0:
                        //przekształcenie aby było w stopniu 1
                        ObrocKsztalt();
                        rotated++;
                        lokacja[1] = new int[] { lokacja[2][0] - 1, lokacja[2][1] };
                        lokacja[0] = new int[] { lokacja[1][0] - 1, lokacja[1][1] };
                        break;

                    case 1:
                        ObrocKsztalt();
                        rotated++;
                        lokacja[3] = new int[] { lokacja[2][0], lokacja[2][1] - 1 };
                        lokacja[0] = new int[] { lokacja[1][0], lokacja[1][1] - 1 };
                        lokacja[2] = new int[] { lokacja[1][0], lokacja[1][1] + 1 };
                        break;

                    case 2:
                        ObrocKsztalt();
                        rotated++;
                        lokacja[1] = new int[] { lokacja[2][0] - 1, lokacja[2][1] };
                        lokacja[0] = new int[] { lokacja[1][0], lokacja[1][1] - 1 };
                        lokacja[3] = new int[] { lokacja[2][0] + 1, lokacja[2][1] };
                        break;

                    case 3:
                        ObrocKsztalt();
                        rotated = 0;
                        lokacja[0] = lokacja[2];
                        lokacja[2] = new int[] { lokacja[3][0], lokacja[3][1] - 1 };
                        lokacja[1] = new int[] { lokacja[3][0], lokacja[3][1] - 2 };
                        break;
                }
                break;
        }
        //korekta
        //GameBoard.lokacjaOstatniegoTetrisaGrid
        bool poza = true;
        while (poza == true)
        {
            int i;
            for (i = 0; i < 4; i++)
            {
                if (lokacja[i][1] < 0) //wychodzi za lewą granice
                    Przesun(1, 0);
                else if (lokacja[i][1] >= GameBoard.Instance.GameBoard1.ColumnDefinitions.Count)
                    Przesun(-1, 0);
            }
            if (i == 4)
                poza = false;
        }

        //sprawdzamy czy któryś z "nowych" "poprawionych" pozycji klocka nie wchodzi na nic
        bool nachodzi = false;
        for (int i = 0; i < 4; i++)
        {
            if (lokacja[i][0] >= 0)
                if (GameBoard.lokacjaOstatniegoTetrisaGrid[lokacja[i][0], lokacja[i][1]] == 1)
                {
                    nachodzi = true;
                    break;
                }
        }
        //zrobic aby probowalo sie przesunac
        if (nachodzi)
        {
            //przywroc z backupu
            this.shape = BU_kształt;
            this.lokacja = BU_lokacja;
            this.rotated = BU_stopienObrocenia;
        }

        //pewnie jakies sprawdzenie czy nowy wytwor i jego wspolrzedne z lokacja nie wchodza na cos. Jesli wchodza przywroc buckUp przypisujac go do this;
        Aktualizuj();
    }

    private void Przesun(int d_x, int d_y)
    {
        for (int i = 0; i < 4; i++)
        {
            this.lokacja[i][0] += d_y;
            this.lokacja[i][1] += d_x;
        }
    }

    public bool CzyWystaje()
    {
        for (int i = 0; i < 4; i++)
            if (lokacja[i][0] < 0)
                return true;
        return false;
    }
}


public class Tetromino_I : Tetromino
{
    public override int[,] shape
    {
        get
        {
            return new int[1, 4] { { 1, 1, 1, 1 } };
        }

        set
        {
            this.shape = value;
        }
    }

    public override String shapeName
    {
        get
        {
            return "I";
        }
    }

    public override int rotated
    {
        get
        {
            return this.rotated;
        }

        set
        {
            this.rotated = value;
        }
    }

    public override Color tetriminoColor
    {
        get
        {
            return this.tetriminoColor;
        }

        set
        {
            this.tetriminoColor = value;
        }
    }

}

public class Tetromino_O : Tetromino
{
    public override int[,] shape
    {
        get
        {
            return new int[2, 2] { { 1, 1 }, { 1, 1 } };
        }
        set
        {
            this.shape = value;
        }
    }

    public override String shapeName
    {
        get
        {
            return "O";
        }
    }

    public override int rotated
    {
        get
        {
            return this.rotated;
        }

        set
        {
            this.rotated = value;
        }
    }

    public override Color tetriminoColor
    {
        get
        {
            return this.tetriminoColor;
        }

        set
        {
            this.tetriminoColor = value;
        }
    }

}

public class Tetromino_T : Tetromino
{
    public override int[,] shape
    {
        get
        {
            return new int[2, 3] { { 0, 1, 0 }, { 1, 1, 1 } };
        }
        set
        {
            this.shape = value;
        }
    }

    public override String shapeName
    {
        get
        {
            return "T";
        }
    }

    public override int rotated
    {
        get
        {
            return this.rotated;
        }

        set
        {
            this.rotated = value;
        }
    }

    public override Color tetriminoColor
    {
        get
        {
            return this.tetriminoColor;
        }

        set
        {
            this.tetriminoColor = value;
        }
    }

}

public class Tetromino_Z : Tetromino
{
    public override int[,] shape
    {
        get
        {
            return new int[2, 3] { { 1, 1, 0 }, { 0, 1, 1 } };
        }
        set
        {
            this.shape = value;
        }
    }

    public override String shapeName
    {
        get
        {
            return "Z";
        }
    }

    public override int rotated
    {
        get
        {
            return this.rotated;
        }

        set
        {
            this.rotated = value;
        }
    }

    public override Color tetriminoColor
    {
        get
        {
            return this.tetriminoColor;
        }

        set
        {
            this.tetriminoColor = value;
        }
    }

}

public class Tetromino_S : Tetromino
{
    public override int[,] shape
    {
        get
        {
            return new int[2, 3] { { 0, 1, 1 }, { 1, 1, 0 } };
        }
        set
        {
            this.shape = value;
        }
    }

    public override String shapeName
    {
        get
        {
            return "S";
        }
    }

    public override int rotated
    {
        get
        {
            return this.rotated;
        }

        set
        {
            this.rotated = value;
        }
    }

    public override Color tetriminoColor
    {
        get
        {
            return this.tetriminoColor;
        }

        set
        {
            this.tetriminoColor = value;
        }
    }

}

public class Tetromino_J : Tetromino
{
    public override int[,] shape
    {
        get
        {
            return new int[2, 3] { { 1, 0, 0 }, { 1, 1, 1 } };
        }
        set
        {
            this.shape = value;
        }
    }

    public override String shapeName
    {
        get
        {
            return "J";
        }
    }

    public override int rotated
    {
        get
        {
            return this.rotated;
        }

        set
        {
            this.rotated = value;
        }
    }

    public override Color tetriminoColor
    {
        get
        {
            return this.tetriminoColor;
        }

        set
        {
            this.tetriminoColor = value;
        }
    }

}

public class Tetromino_L : Tetromino
{
    public override int[,] shape
    {
        get
        {
            return new int[2, 3] { { 0, 0, 1 }, { 1, 1, 1 } };
        }
        set
        {
            this.shape = value;
        }
    }

    public override String shapeName
    {
        get
        {
            return "L";
        }
    }

    public override int rotated
    {
        get
        {
            return this.rotated;
        }

        set
        {
            this.rotated = value;
        }
    }

    public override Color tetriminoColor
    {
        get
        {
            return this.tetriminoColor;
        }

        set
        {
            this.tetriminoColor = value;
        }
    }

}

public class Tetromino_Single : Tetromino
{
    public override int[,] shape
    {
        get
        {
            return new int[1, 1] { { 1 } };
        }
        set
        {
            this.shape = value;
        }
    }

    public override String shapeName
    {
        get
        {
            return "o";
        }
    }

    public override int rotated
    {
        get
        {
            return this.rotated;
        }

        set
        {
            this.rotated = value;
        }
    }

    public override Color tetriminoColor
    {
        get
        {
            return this.tetriminoColor;
        }

        set
        {
            this.tetriminoColor = value;
        }
    }

}
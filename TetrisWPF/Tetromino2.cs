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
        switch (6)
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
    public int[,] shape;
    public String shapeName;
    public int rotated = 0;
    public Color tetriminoColor;
    public List<int[]> lokacja = new List<int[]>();

    public bool Stwórz()
    {
        for (int i = 0; i < this.shape.GetLength(0); i++)
        {
            for (int j = 0; j < this.shape.GetLength(1); j++)
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

        this.rotated = 0;
        Aktualizuj();
        return true;
    }

    public int[,] getKształt()
    {
        return this.shape;
    }

    public static void Landslide()
    {
        Random rnd = new Random();
        int x_position = 0;
        int y_position = rnd.Next(0, 9);
        bool czyPonizej = false;
        int[] rndtable = new int[] { 1, 1, 1, 1, 1, 1, 1, 2 };
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

    public void Obroc(int i)
    {
        if(i == 1)
        {
            RotateAbstract rotate = new RotateRight();

            var tmp_shape = this.shape;
            var tmp_location = this.lokacja;
            var tmp_rotation = this.rotated;
            rotate.RotateMethod(ref tmp_shape, ref tmp_location,shapeName,ref tmp_rotation);

            this.shape = tmp_shape;
            this.lokacja = tmp_location;
            this.rotated = tmp_rotation;
        }
        if(i == 2)
        {
            RotateAbstract rotate = new RotateLeft();

            var tmp_shape = this.shape;
            var tmp_location = this.lokacja;
            var tmp_rotation = this.rotated;
            rotate.RotateMethod(ref tmp_shape, ref tmp_location, shapeName, ref tmp_rotation);

            this.shape = tmp_shape;
            this.lokacja = tmp_location;
            this.rotated = tmp_rotation;
        }
        Aktualizuj();
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
    public Tetromino_I()
    {
        this.shape = new int[1, 4] { { 1, 1, 1, 1 } };
        this.shapeName = "I";
    }
}

public class Tetromino_O : Tetromino
{
    public Tetromino_O()
    {
        this.shape = new int[2, 2] { { 1, 1 }, { 1, 1 } };
        this.shapeName = "O";
    }

}

public class Tetromino_T : Tetromino
{
    public Tetromino_T()
    {
        this.shape = new int[2, 3] { { 0, 1, 0 }, { 1, 1, 1 } };
        this.shapeName = "T";
    }
}

public class Tetromino_Z : Tetromino
{
    public Tetromino_Z()
    {
        this.shape = new int[2, 3] { { 1, 1, 0 }, { 0, 1, 1 } };
        this.shapeName = "Z";
    }

}

public class Tetromino_S : Tetromino
{
    public Tetromino_S()
    {
        this.shape = new int[2, 3] { { 0, 1, 1 }, { 1, 1, 0 } };
        this.shapeName = "S";
    }

}

public class Tetromino_J : Tetromino
{

    public Tetromino_J()
    {
        this.shape = new int[2, 3] { { 1, 0, 0 }, { 1, 1, 1 } };
        this.shapeName = "J";
    }
}

public class Tetromino_L : Tetromino
{
    public Tetromino_L()
    {
        this.shape = new int[2, 3] { { 0, 0, 1 }, { 1, 1, 1 } };
        this.shapeName = "L";
    }


}

public class Tetromino_Single : Tetromino
{
    public Tetromino_Single()
    {
        this.shape = new int[1, 1] { { 1 } };
        this.shapeName = "o";
    }
}
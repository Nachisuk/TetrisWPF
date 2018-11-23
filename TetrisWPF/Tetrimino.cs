using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisWPF
{
    public static class Tetrimino_I
    {
        private static int[,] tablicaTetrimo = new int[1, 4] { { 1, 1, 1, 1 } };
        private static string typ = "I";

        public static int[,] getTablicaTetrimo()
        {
            return tablicaTetrimo;
        }

        public static String getTypTetrimo()
        {
            return typ;
        }
    }

    public static class Tetrimino_O
    {
        private static int[,] tablicaTetrimo = new int[2, 2] { { 1, 1 }, { 1, 1 } };
        private static string typ = "O";

        public static int[,] getTablicaTetrimo()
        {
            return tablicaTetrimo;
        }
        public static String getTypTetrimo()
        {
            return typ;
        }
    }

    public static class Tetrimino_T
    {
        private static int[,] tablicaTetrimo = new int[2, 3] { { 0, 1, 0 }, { 1, 1, 1 } };
        private static string typ = "T";

        public static int[,] getTablicaTetrimo()
        {
            return tablicaTetrimo;
        }
        public static String getTypTetrimo()
        {
            return typ;
        }
    }

    public static class Tetrimino_S
    {
        private static int[,] tablicaTetrimo = new int[2, 3] { { 0, 1, 1 }, { 1, 1, 0 } };
        private static string typ = "S";

        public static int[,] getTablicaTetrimo()
        {
            return tablicaTetrimo;
        }
        public static String getTypTetrimo()
        {
            return typ;
        }
    }

    public static class Tetrimino_Z
    {
        private static int[,] tablicaTetrimo = new int[2, 3] { { 1, 1, 0 }, { 0, 1, 1 } };
        private static string typ = "Z";

        public static int[,] getTablicaTetrimo()
        {
            return tablicaTetrimo;
        }
        public static String getTypTetrimo()
        {
            return typ;
        }
    }

    public static class Tetrimino_J
    {
        private static int[,] tablicaTetrimo = new int[2, 3] { { 1, 0, 0 }, { 1, 1, 1 } };
        private static string typ = "J";

        public static int[,] getTablicaTetrimo()
        {
            return tablicaTetrimo;
        }
        public static String getTypTetrimo()
        {
            return typ;
        }
    }

    public static class Tetrimino_L
    {
        private static int[,] tablicaTetrimo = new int[2, 3] { { 0, 0, 1 }, { 1, 1, 1 } };
        private static string typ = "L";

        public static int[,] getTablicaTetrimo()
        {
            return tablicaTetrimo;
        }
        public static String getTypTetrimo()
        {
            return typ;
        }
    }
    public static class Tetrimino_Element
    {
        private static int[,] tablicaTetrimo = new int[1, 1] { { 1 } };
        private static string typ = "o";

        public static int[,] getTablicaTetrimo()
        {
            return tablicaTetrimo;
        }
        public static String getTypTetrimo()
        {
            return typ;
        }
    }



    public class Tetrimo
    {
        private int[,] kształt;
        private string nazwaKształtu;
        public List<int[]> lokacja = new List<int[]>();
        public int[,] grid = new int[23, 10];
        public int stopienObrocenia = 0;

        public Tetrimo()
        {
            Random rnd = new Random();
            int rodzaj = rnd.Next(1, 7);
            switch (rodzaj)
            {
                case 1:
                    this.nazwaKształtu = Tetrimino_I.getTypTetrimo();
                    this.kształt = Tetrimino_I.getTablicaTetrimo();
                    break;
                case 2:
                    this.nazwaKształtu = Tetrimino_J.getTypTetrimo();
                    this.kształt = Tetrimino_J.getTablicaTetrimo();
                    break;
                case 3:
                    this.nazwaKształtu = Tetrimino_L.getTypTetrimo();
                    this.kształt = Tetrimino_L.getTablicaTetrimo();
                    break;
                case 4:
                    this.nazwaKształtu = Tetrimino_O.getTypTetrimo();
                    this.kształt = Tetrimino_O.getTablicaTetrimo();
                    break;
                case 5:
                    this.nazwaKształtu = Tetrimino_S.getTypTetrimo();
                    this.kształt = Tetrimino_S.getTablicaTetrimo();
                    break;
                case 6:
                    this.nazwaKształtu = Tetrimino_T.getTypTetrimo();
                    this.kształt = Tetrimino_T.getTablicaTetrimo();
                    break;
                case 7:
                    this.nazwaKształtu = Tetrimino_Z.getTypTetrimo();
                    this.kształt = Tetrimino_Z.getTablicaTetrimo();
                    break;
                default:
                    Console.WriteLine("Error 1");
                    break;
            }
        }

        public int[,] getKształt()
        {
            return this.kształt;
        }

        public bool Stwórz()
        {
            for (int i = 0; i < kształt.GetLength(0); i++)
            {
                for (int j = 0; j < kształt.GetLength(1); j++)
                {
                    if (kształt[i, j] == 1)
                    {
                        lokacja.Add(new int[] { i, (10 - kształt.GetLength(1)) / 2 + j });
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (GameBoard.lokacjaOstatniegoTetrisaGrid[this.lokacja[i][0], this.lokacja[i][1]] == 1)
                {
                    //GameBoard.czyGameOver = true;
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
                if (lokacja[i][1] == GameBoard.Instance.GameBoard1.ColumnDefinitions.Count-1)
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
            int[,] tmpKształt = new int[this.kształt.GetLength(1), this.kształt.GetLength(0)];
            //transportowanie kolejnych "wartosci" pod polami:
            for (int i = 0; i < this.kształt.GetLength(1); i++)
            {
                for (int j = 0; j < this.kształt.GetLength(0); j++)
                {
                    tmpKształt[i, j] = this.kształt[this.kształt.GetLength(0) - 1 - j, i];
                }
            }
            this.kształt = tmpKształt;
        }

        public void Obroc()
        {

            //zrobienie buckupu w razie gdyby sie okazalo ze obrocony nachodzi na jakies juz w gridzie: (nie mozna zwyłym =this, bo tworzy kopię płytką)
            int[,] BU_kształt = this.kształt.Clone() as int[,];
            List<int[]> BU_lokacja = new List<int[]>();
            for (int i = 0; i < 4; i++)
                BU_lokacja.Add(this.lokacja[i].Clone() as int[]);
            int BU_stopienObrocenia = this.stopienObrocenia;

            switch (nazwaKształtu)
            {
                case "I":
                    //sprawdzamy który z stopien obrocenia jest 0 - lezy czy 1 - pionowy
                    switch (stopienObrocenia)
                    {
                        case 0:
                            //przekształcenie aby było w stopniu 1
                            ObrocKsztalt();
                            stopienObrocenia++;
                            lokacja[3] = lokacja[1];
                            lokacja[0] = new int[] { lokacja[3][0] - 3, lokacja[3][1] };
                            lokacja[1] = new int[] { lokacja[3][0] - 2, lokacja[3][1] };
                            lokacja[2] = new int[] { lokacja[3][0] - 1, lokacja[3][1] };
                            break;

                        case 1:
                            ObrocKsztalt();
                            stopienObrocenia = 0;
                            lokacja[1] = lokacja[3];
                            lokacja[0] = new int[] { lokacja[3][0], lokacja[3][1] - 1 };
                            lokacja[2] = new int[] { lokacja[3][0], lokacja[3][1] + 1 };
                            lokacja[3] = new int[] { lokacja[3][0], lokacja[3][1] + 2 };
                            break;
                    }
                    break;

                case "T":
                    switch (stopienObrocenia)
                    {
                        case 0:
                            //przekształcenie aby było w stopniu 1
                            ObrocKsztalt();
                            stopienObrocenia++;
                            lokacja[3] = lokacja[2];
                            lokacja[0] = new int[] { lokacja[3][0] - 2, lokacja[3][1] };
                            lokacja[1] = new int[] { lokacja[3][0] - 1, lokacja[3][1] };
                            lokacja[2] = new int[] { lokacja[3][0] - 1, lokacja[3][1] + 1 };
                            break;

                        case 1:
                            ObrocKsztalt();
                            stopienObrocenia++;
                            lokacja[0] = new int[] { lokacja[1][0], lokacja[1][1] - 1 };
                            break;

                        case 2:
                            ObrocKsztalt();
                            stopienObrocenia++;
                            lokacja[2] = lokacja[1];
                            lokacja[1] = lokacja[0];
                            lokacja[0] = new int[] { lokacja[2][0] - 1, lokacja[2][1] };
                            break;

                        case 3:
                            ObrocKsztalt();
                            stopienObrocenia = 0;
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
                    switch (stopienObrocenia)
                    {
                        case 0:
                            //przekształcenie aby było w stopniu 1
                            ObrocKsztalt();
                            stopienObrocenia++;
                            lokacja[2] = lokacja[0];
                            lokacja[1] = new int[] { lokacja[2][0], lokacja[2][1] - 1 };
                            lokacja[0] = new int[] { lokacja[1][0] - 1, lokacja[1][1] };
                            break;

                        case 1:
                            ObrocKsztalt();
                            stopienObrocenia = 0;
                            lokacja[0] = lokacja[2];
                            lokacja[2] = new int[] { lokacja[3][0], lokacja[3][1] - 1 };
                            lokacja[1] = new int[] { lokacja[0][0], lokacja[0][1] + 1 };
                            break;
                    }
                    break;

                case "Z":
                    switch (stopienObrocenia)
                    {
                        case 0:
                            //przekształcenie aby było w stopniu 1
                            ObrocKsztalt();
                            stopienObrocenia++;
                            lokacja[3] = lokacja[2];
                            lokacja[2] = new int[] { lokacja[1][0], lokacja[1][1] + 1 };
                            lokacja[0] = new int[] { lokacja[2][0] - 1, lokacja[2][1] };
                            break;

                        case 1:
                            ObrocKsztalt();
                            stopienObrocenia = 0;
                            lokacja[2] = lokacja[3];
                            lokacja[0] = new int[] { lokacja[1][0], lokacja[1][1] - 1 };
                            lokacja[3] = new int[] { lokacja[2][0], lokacja[2][1] + 1 };
                            break;
                    }
                    break;

                case "J":
                    switch (stopienObrocenia)
                    {
                        case 0:
                            //przekształcenie aby było w stopniu 1
                            ObrocKsztalt();
                            stopienObrocenia++;
                            lokacja[3] = lokacja[2];
                            lokacja[2] = new int[] { lokacja[3][0] - 1, lokacja[3][1] };
                            lokacja[0] = new int[] { lokacja[2][0] - 1, lokacja[2][1] };
                            lokacja[1] = new int[] { lokacja[0][0], lokacja[0][1] + 1 };
                            break;

                        case 1:
                            ObrocKsztalt();
                            stopienObrocenia++;
                            lokacja[1] = lokacja[2];
                            lokacja[2] = new int[] { lokacja[1][0], lokacja[1][1] + 1 };
                            lokacja[0] = new int[] { lokacja[1][0], lokacja[1][1] - 1 };
                            lokacja[3] = new int[] { lokacja[2][0] + 1, lokacja[2][1] };
                            break;

                        case 2:
                            ObrocKsztalt();
                            stopienObrocenia++;
                            lokacja[0] = new int[] { lokacja[1][0] - 1, lokacja[1][1] };
                            lokacja[3] = new int[] { lokacja[1][0] + 1, lokacja[1][1] };
                            lokacja[2] = new int[] { lokacja[3][0], lokacja[3][1] - 1 };
                            break;

                        case 3:
                            ObrocKsztalt();
                            stopienObrocenia = 0;
                            lokacja[1] = lokacja[2];
                            lokacja[2] = lokacja[3];
                            lokacja[0] = new int[] { lokacja[1][0] - 1, lokacja[1][1] };
                            lokacja[3] = new int[] { lokacja[2][0], lokacja[2][1] + 1 };
                            break;
                    }
                    break;

                case "L":
                    switch (stopienObrocenia)
                    {
                        case 0:
                            //przekształcenie aby było w stopniu 1
                            ObrocKsztalt();
                            stopienObrocenia++;
                            lokacja[1] = new int[] { lokacja[2][0] - 1, lokacja[2][1] };
                            lokacja[0] = new int[] { lokacja[1][0] - 1, lokacja[1][1] };
                            break;

                        case 1:
                            ObrocKsztalt();
                            stopienObrocenia++;
                            lokacja[3] = new int[] { lokacja[2][0], lokacja[2][1] - 1 };
                            lokacja[0] = new int[] { lokacja[1][0], lokacja[1][1] - 1 };
                            lokacja[2] = new int[] { lokacja[1][0], lokacja[1][1] + 1 };
                            break;

                        case 2:
                            ObrocKsztalt();
                            stopienObrocenia++;
                            lokacja[1] = new int[] { lokacja[2][0] - 1, lokacja[2][1] };
                            lokacja[0] = new int[] { lokacja[1][0], lokacja[1][1] - 1 };
                            lokacja[3] = new int[] { lokacja[2][0] + 1, lokacja[2][1] };
                            break;

                        case 3:
                            ObrocKsztalt();
                            stopienObrocenia = 0;
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
                this.kształt = BU_kształt;
                this.lokacja = BU_lokacja;
                this.stopienObrocenia = BU_stopienObrocenia;
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
}

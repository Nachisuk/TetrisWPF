using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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

namespace TetrisWPF
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int boardHeight = 24;
        public const int Padding = 1;
        public const int boardWidth = 10;
        public const int Size = 31;

        public static int[,] grid;
        public static int[,] lokacjaOstatniegoTetrisaGrid;
        public static int[,] tetrisColorGrid;

        //pisze testowy komentarz
        //Zmienne stoperów potrzebne do kontroli opadania, odczekania wciśnięcia przyisku itp.
        public static Stopwatch timer;
        public static Stopwatch dropTimer;
        public static Stopwatch inputTimer;
        public static int dropTime, dropRate, inputTime, playTime;

        //Zmienna mówiąca czy dany klocek tetrisa już opadł
        public static bool czyOpadł;

        //zmienna przesuwają miejsce wypisywania granic tetrisa
        static int Position_X = 30;
        static int Position_Y = 0;

        //zmienne zmieniające wielkość tetrisa
        public static int TetrisBoardHeight = 23;
        public static int TetrisBoardWidth = 10;

        //zmmienne do sterowania przyciskami
        public static ConsoleKeyInfo key;
        public static bool czyNacisnieto = false;
        public static bool czyCosZLewej = false;
        public static bool czyCosZPrawej = false;
        bool isLineCleared;

        //zmmienne dotyczące klocków tetrisa
        public static Tetrimo tetris;
        public static Tetrimo następnyTetris;

        //zmienna do losowanek
        public static Random rnd;

        public static int aktualnyKolor;
        public static int nastepnyKolor;

        public static string element;

        //zmienne punktów,linni,combo

        public static int punkty, wyczyszczoneLinie, combo, poziom;

        //zmienna okreslająca czy to game over
        public static bool czyGameOver;
        public static bool czyZapauzowane;

        //zmienna określająca GameMode

        public static String actualGameMode;
        public static bool czyPokazywać;
        public static bool isHaunted;


        //zmienna BazyDanych
        public static BazaWynikow bazaWynikow;

        public DispatcherTimer mainTimer;

        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            InitializeVariables();
            //DrawGameBoardStart();
            DrawGameBoard2();
            //Test1
            Rysuj();


            tetris = new Tetrimo();
            następnyTetris = new Tetrimo();
            tetris.Stwórz();
            RysujNastepny(następnyTetris.getKształt());

            TimerStart();
            // Uruchom();

        }

        public void TimerStart()
        {
            mainTimer = new DispatcherTimer();
            mainTimer.Interval = new TimeSpan(0, 0, 0,0,100);
            mainTimer.Tick += Callback;
            mainTimer.Start();
        }

        public void InitializeVariables()
        {
            grid = new int[boardHeight, boardWidth];
            lokacjaOstatniegoTetrisaGrid = new int[boardHeight, boardWidth];
            tetrisColorGrid = new int[boardHeight,boardWidth];

            timer = new Stopwatch();
            timer.Start();
            dropTimer = new Stopwatch();
            dropTimer.Start();
            inputTimer = new Stopwatch();
            dropRate = 300;
            inputTime = 10;
            czyOpadł = false;
            czyNacisnieto = false;
            Random rnd = new Random();
            aktualnyKolor = rnd.Next(1, 7);
            nastepnyKolor = rnd.Next(1, 7);
            czyCosZLewej = false;
            punkty = 0; wyczyszczoneLinie = 0; combo = 0;
            poziom = 1;
            element = Encoding.ASCII.GetString(new byte[] { 65 });
            bool isLineCleared = false;
            czyZapauzowane = false;
            czyGameOver = false;
            playTime = 0;
            czyPokazywać = false;
            isHaunted = false;
            // grid[10, 5] = 1;
        }

        public static Color ColorAssign(int rodzaj)
        {
            switch (rodzaj)
            {
                case 1:
                    return Colors.Aqua;
                case 2:
                    return Colors.Blue;
                case 3:
                    return Colors.Yellow;
                case 4:
                    return Colors.Green;
                case 5:
                    return Colors.Magenta;
                case 6:
                    return Colors.Red;
                case 7:
                    return Colors.DarkGreen;
                default:
                    return Colors.Black;
            }
        }


        public void DrawGameBoard2()
        {
            int Rows = GameBoard.RowDefinitions.Count;
            int Columns = GameBoard.ColumnDefinitions.Count;

            Label[,] BlockControls = new Label[Columns, Rows];
            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    Border border = new Border();
                    

                    double width = GameBoard.ColumnDefinitions[0].ActualWidth;

                    border.Width = 30 - Padding * 2;
                    border.Height = 30 - Padding * 2;

                    Grid.SetRow(border, j);
                    Grid.SetColumn(border, i);

                    border.Background = new SolidColorBrush(Colors.Beige);
                    border.Padding = new Thickness(0);

                    GameBoard.Children.Add(border);
                }
            }
        }
       

        public static void Rysuj()
        {
            Instance.GameBoard.Children.Clear();
            for (int i = 0; i < Instance.GameBoard.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < Instance.GameBoard.ColumnDefinitions.Count; j++)
                {
                    if (grid[i, j] == 1 || lokacjaOstatniegoTetrisaGrid[i, j] == 1)
                    {
                        Border border = new Border();
                        border.Width = Size - Padding * 2;
                        border.Height = Size - Padding * 2;
                        Instance.GameBoard.Children.Add(border);

                        if (tetrisColorGrid[i, j] < 1 || tetrisColorGrid[i, j] > 8)
                        {
                            border.Background = new SolidColorBrush(ColorAssign(aktualnyKolor));
                            border.Padding = new Thickness(0);
                            //Console.ForegroundColor = Color(aktualnyKolor);
                        }

                        else
                        {
                            border.Background = new SolidColorBrush(ColorAssign(tetrisColorGrid[i,j]));
                            border.Padding = new Thickness(0);
                            //  Console.ForegroundColor = Color(tetrisColorGrid[i, j]);
                        }

                        if (actualGameMode == "  Haunted  ")
                        {
                            if (czyPokazywać || grid[i, j] == 1)
                            {
                                Grid.SetRow(border, i);
                                Grid.SetColumn(border, j);
                            }
                            else
                            {
                                border.Background = new SolidColorBrush(Colors.Beige);
                                border.Padding = new Thickness(0);

                                Grid.SetRow(border, i);
                                Grid.SetColumn(border, j);
                            }
                        }
                        else
                        {
                            Grid.SetRow(border, i);
                            Grid.SetColumn(border, j);
                        }
                        
                    }
                    else
                    {
                        Border border = new Border();

                        border.Width = 30 - Padding * 2;
                        border.Height = 30 - Padding * 2;

                        Grid.SetRow(border, i);
                        Grid.SetColumn(border, j);

                        border.Background = new SolidColorBrush(Colors.Beige);
                        border.Padding = new Thickness(0);
                        Instance.GameBoard.Children.Add(border);
                    }

                }
            }
        }

        public void RysujNastepny(int[,] kształt)
        {
            NextTetris.Children.Clear();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Border border = new Border();
                    NextTetris.Children.Add(border);
                    border.Width = 31 - Padding * 2;
                    border.Height = 31 - Padding * 2;

                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);

                    border.Background = new SolidColorBrush(Colors.Beige);
                    border.Padding = new Thickness(0);
                }
            }

           for (int i=0; i < kształt.GetLength(0); i++)
            {
                for(int j=0;j<kształt.GetLength(1);j++)
                {
                    Border border = new Border();
                    NextTetris.Children.Add(border);
                    border.Width = 31 - Padding * 2;
                    border.Height = 31 - Padding * 2;

                    if (kształt[i,j] == 1)
                    {
                        Grid.SetRow(border, i);
                        Grid.SetColumn(border, j);

                        border.Background = new SolidColorBrush(ColorAssign(nastepnyKolor));
                        border.Padding = new Thickness(0);
                    }                    
                }
            }
        }

        private void Callback(object sender, EventArgs e)
        {
                Debug.WriteLine("tick");

                dropTime = (int)dropTimer.ElapsedMilliseconds;
            Debug.WriteLine(dropTime.ToString());

            if (dropTime > dropRate)
            {
                dropTime = 0;
                dropTimer.Restart();
                tetris.Opadaj();
            }

            if (czyOpadł)
            {
                if (tetris.CzyWystaje())
                {
                    czyGameOver = true;
                }

                else
                {
                    if (actualGameMode == "  Haunted  " && !isHaunted)
                    {
                        czyPokazywać = true;
                        isHaunted = true;
                        timer.Restart();
                    }
                    Random rnd = new Random();

                    tetris = następnyTetris;
                    następnyTetris = new Tetrimo();

                    aktualnyKolor = nastepnyKolor;
                    nastepnyKolor = rnd.Next(1, 7);

                    RysujNastepny(następnyTetris.getKształt());

                    if (!tetris.Stwórz())
                    {
                        czyGameOver = true;

                    }

                    czyOpadł = false;
                }

                if (czyGameOver == true)
                {

                    //Interface.GameOverPopUp();
                    //Interface.GameOver_ChangeLightedOption(false);
                    bool ifLeftPressed = true;
                    while (true)
                    {
                        ConsoleKey choice;
                        if (Console.KeyAvailable)
                        {
                            choice = Console.ReadKey(true).Key;
                            switch (choice)
                            {
                                case ConsoleKey.LeftArrow:
                                    if (!ifLeftPressed)
                                    {
                                        //Interface.GameOver_ChangeLightedOption(ifLeftPressed);
                                        ifLeftPressed = !ifLeftPressed;
                                    }
                                    //Restart();
                                    break;
                                case ConsoleKey.RightArrow:
                                    if (ifLeftPressed)
                                    {
                                        //Interface.GameOver_ChangeLightedOption(ifLeftPressed);
                                        ifLeftPressed = !ifLeftPressed;
                                    }
                                    //Podsumowanie();
                                    break;
                                case ConsoleKey.R:
                                    Restart();
                                    break;
                                case ConsoleKey.Escape:
                                    //Interface.MainMenu(MenuOptions.ZwrocOpcje());
                                    break;
                                case ConsoleKey.Enter:
                                    if (ifLeftPressed)
                                        Restart();
                                    else
                                        Podsumowanie();
                                    break;
                            }
                        }
                    }
                }
            }
            if (czyZapauzowane)
            {
                //Interface.PausePopUp();

            }
            switch (actualGameMode)
            {
                case "  Maraton ":

                    if (poziom == 15)
                    {
                        czyGameOver = true;
                    }
                    break;
                case "  Endless ":

                    break;
                case "   Ultra  ":
                    if (poziom < 5) poziom = 5;
                    playTime = (int)timer.ElapsedMilliseconds / 1000;
                    Console.SetCursorPosition(5, 4);
                    Console.Write("Pozotały czas: ");
                    Console.SetCursorPosition(21, 4);
                    float czas = 180 - playTime;

                    Console.Write("" + czas + " ");
                    if (playTime > 180)
                    {
                        czyGameOver = true;
                    }

                    break;
                case " LandSlide ":
                    playTime = (int)timer.ElapsedMilliseconds / 1000;

                    Console.SetCursorPosition(5, 4);
                    Console.Write("Następny osuw za: ");
                    Console.SetCursorPosition(23, 4);

                    float czas1 = 16 - playTime;
                    Console.Write("" + czas1 + " ");

                    if (playTime > 15)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Tetrimo.Landslide();
                        }
                        timer.Restart();
                    }
                    break;
                case "  Haunted  ":
                    if (isHaunted)
                    {
                        playTime = (int)timer.ElapsedMilliseconds;
                        if (playTime > 800)
                        {
                            isHaunted = false;
                            czyPokazywać = false;
                        }
                    }
                    break;

            }
            WyczyscLinie();

        }

        public void Sterowanie2(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Left:
                    if (!tetris.czyJestCosZLewa())
                    {
                        for (int i = 0; i < 4; i++)
                        {

                            tetris.lokacja[i][1] -= 1;

                        }
                        tetris.Aktualizuj();
                    }
                    break;
                case Key.Right:
                    if (!tetris.czyJestCosZPrawa())
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            tetris.lokacja[i][1] += 1;
                        }
                        tetris.Aktualizuj();
                    }
                    break;
                case Key.Down:
                    tetris.Opadaj();
                    break;
                case Key.Up:
                   while(tetris.czyJestCosPonizej() != true)
                    {
                        tetris.Opadaj();
                    }
                    break;
                case Key.Space:
                    tetris.Obroc();
                    break;
                case Key.R:
                     Restart();
                     break;
                case Key.P:
                    Pause();
                    break;
                case Key.Escape:
                    break;
            }
        }

        public void WyczyscLinie()
        {
            int combo = 0;
            for (int i = 0; i < Instance.GameBoard.RowDefinitions.Count; i++)
            {
                int j;
                for (j = 0; j < Instance.GameBoard.ColumnDefinitions.Count; j++)
                {
                    if (lokacjaOstatniegoTetrisaGrid[i, j] == 0) break;
                }

                if (j == 10)
                {
                    wyczyszczoneLinie++;
                    combo++;
                    isLineCleared = true;
                    //TODO lines clear + combo
                    //czyszczenie pełnej linii
                    for (j = 0; j < 10; j++)
                    {
                        lokacjaOstatniegoTetrisaGrid[i, j] = 0;
                    }

                    //tworzenie nowego gridu po usunięciu linii
                    int[,] nowaTablicaZrzuconychTetrisow = new int[TetrisBoardHeight + 1, TetrisBoardWidth];
                    int[,] nowaTablicaKolorów = new int[TetrisBoardHeight + 1, TetrisBoardWidth];
                    //przesuwanie w dół wszystkich elementów po usunięciu linii
                    for (int z = 1; z < i; z++)
                    {
                        for (int x = 0; x < 10; x++)
                        {
                            nowaTablicaZrzuconychTetrisow[z + 1, x] = lokacjaOstatniegoTetrisaGrid[z, x];
                            nowaTablicaKolorów[z + 1, x] = tetrisColorGrid[z, x];
                            tetrisColorGrid[z, x] = 10;
                            lokacjaOstatniegoTetrisaGrid[z, x] = 0;
                        }
                    }

                    //przekazanie przesuniętych elementów do aktualnej tablicy przechowującej nasze klocki

                    for (int z = 0; z < Instance.GameBoard.RowDefinitions.Count; z++)
                    {
                        for (int x = 0; x < Instance.GameBoard.ColumnDefinitions.Count; x++)
                        {
                            if (nowaTablicaZrzuconychTetrisow[z, x] == 1)
                            {
                                lokacjaOstatniegoTetrisaGrid[z, x] = 1;
                            }

                            if (lokacjaOstatniegoTetrisaGrid[z, x] == 1 && !(tetrisColorGrid[z, x] == 10))
                            {
                                nowaTablicaKolorów[z, x] = tetrisColorGrid[z, x];

                            }
                        }
                    }

                    tetrisColorGrid = nowaTablicaKolorów;
                    Rysuj();
                }
            }
            if (combo == 1)
                punkty += 30 * poziom;
            else if (combo == 2)
                punkty += 60 * poziom;
            else if (combo == 3)
                punkty += 180 * poziom;
            else if (combo == 4)
                punkty += 250 * poziom;
            else if (combo > 4)
                punkty += 350 * poziom;

            if (wyczyszczoneLinie % 10 == 0 && wyczyszczoneLinie > 0 && isLineCleared)
            {
                poziom++;
                if (poziom <= 10) dropRate = dropRate - 22;
                isLineCleared = false;
            }

            if (combo > 0)
            {
                PoziomLabel.Text = poziom.ToString();
                PunktyLabel.Text = punkty.ToString();
                WyczyszczoneLinieLabel.Text = wyczyszczoneLinie.ToString();
            }

        }
        

        public void Restart()
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.InitializeVariables();
        }

        public void Podsumowanie()
        {
            
           
        }

        public void Pause()
        {
            if (mainTimer.IsEnabled) mainTimer.Stop();
            else mainTimer.Start();
        }
    }
}

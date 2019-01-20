using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace TetrisWPF
{
    public abstract partial class GameBoard : Page//component
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
        protected static int Position_X = 30;
        protected static int Position_Y = 0;

        //zmienne zmieniające wielkość tetrisa
        public static int TetrisBoardHeight = 23;
        public static int TetrisBoardWidth = 10;

        //zmmienne do sterowania przyciskami
        public static ConsoleKeyInfo key;
        public static bool czyNacisnieto = false;
        public static bool czyCosZLewej = false;
        public static bool czyCosZPrawej = false;
        protected bool isLineCleared;

        //zmmienne dotyczące klocków tetrisa
        public static string filenameTetrisa = "../../Images/";
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

        //public static String actualGameMode;
        public static bool czyPokazywać;

        public static KeyEventHandler keyEventHandler;
        public static EventHandler eventHandlerTick;

        //zmienna BazyDanych
        //private DataOperator bazaWynikow;

        public DispatcherTimer mainTimer;

        public static GameBoard Instance { get; protected set; }



        public abstract void InitializeVariables();
        public abstract void UstawTlo();
        public abstract void DrawGameBoard2();
        public abstract void Rysuj();

        //public abstract void Callback(object sender, EventArgs e);
        public abstract void FallingAction();
        public abstract void AdditionalAction();

        public abstract GameBoard getNew();



        public GameBoard InitializeGameBoard()
        {

            Instance = this;
            InitializeComponent();
            UstawTlo();
            InitializeVariables();
            
            DrawGameBoard2();
            Rysuj();

            keyEventHandler = new KeyEventHandler(Sterowanie2);
            Application.Current.MainWindow.KeyDown += keyEventHandler;
            Console.Out.Write("Dodaje kontroler");

            tetris = new Tetrimo();
            następnyTetris = new Tetrimo();
            tetris.Stwórz(this);
            RysujNastepny(następnyTetris.getKształt());

            TimerStart();
            return this;
        }

        public  void Restart()
        {
            mainTimer.Tick -= eventHandlerTick;
            Application.Current.MainWindow.KeyDown -= keyEventHandler;
            App.Current.MainWindow.Content = getNew().InitializeGameBoard();
        }

        public static ImageBrush ImgAssign(int rodzaj)
        {

            string fullFileName = filenameTetrisa + "tetrisElement/";
            switch (rodzaj)
            {
                case 1:
                    fullFileName += "Aqua";
                    break;
                case 2:
                    fullFileName += "Blue";
                    break;
                case 3:
                    fullFileName += "Yellow";
                    break;
                case 4:
                    fullFileName += "Green";
                    break;
                case 5:
                    fullFileName += "Magenta";
                    break;
                case 6:
                    fullFileName += "Red";
                    break;
                case 7:
                    fullFileName += "DarkGreen";
                    break;
                default:
                    fullFileName += "Black";
                    break;
            }

            fullFileName += ".png";

            BitmapImage img = new BitmapImage(new Uri(fullFileName, UriKind.Relative));
            ImageBrush image = new ImageBrush();
            image.ImageSource = img;
            return image;
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

        public void TimerStart()
        {
            Console.Out.Write("TimeStart()");
            mainTimer = new DispatcherTimer();
            mainTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            eventHandlerTick = Callback;
            mainTimer.Tick += eventHandlerTick;
            mainTimer.Start();
        }

        public void RysujNastepny(int[,] kształt )
        {
            Instance.NextTetris.Children.Clear();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Border border = new Border();
                    Instance.NextTetris.Children.Add(border);
                    border.Width = 31 - Padding * 2;
                    border.Height = 31 - Padding * 2;

                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);

                    border.Background = new SolidColorBrush(Color.FromArgb(127, 15, 21, 33));
                    border.Padding = new Thickness(0);
                }
            }

            for (int i = 0; i < kształt.GetLength(0); i++)
            {
                for (int j = 0; j < kształt.GetLength(1); j++)
                {
                    Border border = new Border();
                    Instance.NextTetris.Children.Add(border);
                    border.Width = 31 - Padding * 2;
                    border.Height = 31 - Padding * 2;

                    if (kształt[i, j] == 1)
                    {
                        Grid.SetRow(border, i);
                        Grid.SetColumn(border, j);
                        border.Background = ImgAssign(nastepnyKolor);
                        border.Padding = new Thickness(0);
                    }
                }
            }
        }

        public void Callback(object sender, EventArgs e)
        {
            Debug.WriteLine("tick");

            dropTime = (int)dropTimer.ElapsedMilliseconds;
            Debug.WriteLine(dropTime.ToString());

            if (dropTime > dropRate)
            {
                dropTime = 0;
                dropTimer.Restart();
                tetris.Opadaj(this);
            }

            if (czyOpadł)
            {
                if (tetris.CzyWystaje())
                {
                    czyGameOver = true;
                }

                else
                {
                    FallingAction();
                }
            }

            if (czyGameOver == true)
            {
                mainTimer.Stop();
                czyZapauzowane = true;
                GameOverPopUp dlg = new GameOverPopUp();
                dlg.Owner = App.Current.MainWindow;

                dlg.ShowDialog();
                if (dlg.DialogResult == true) //wybrano przejscie do podsumowania
                {
                    Podsumowanie();
                }
                else //wybrano opcje restartu
                {
                    Restart();
                }
            }

            if (czyZapauzowane)
            {

            }

            AdditionalAction();

            WyczyscLinie();

        }

        public void Sterowanie2(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    Debug.WriteLine("KliknietoLewo");
                    if (!tetris.czyJestCosZLewa() && !czyZapauzowane)
                    {
                        for (int i = 0; i < 4; i++)
                        {

                            tetris.lokacja[i][1] -= 1;

                        }
                        tetris.Aktualizuj(this);
                    }
                    break;
                case Key.Right:
                    Debug.WriteLine("KliknietoPrawo");
                    if (!tetris.czyJestCosZPrawa() && !czyZapauzowane)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            tetris.lokacja[i][1] += 1;
                        }
                        tetris.Aktualizuj(this);
                    }
                    break;
                case Key.Down:
                    Debug.WriteLine("KliknietoDol");
                    if (!czyZapauzowane)
                        tetris.Opadaj(this);
                    break;
                case Key.Up:
                    Debug.WriteLine("KliknietoGora");
                    while (tetris.czyJestCosPonizej() != true && !czyZapauzowane)
                    {
                        tetris.Opadaj(this);
                    }
                    break;
                case Key.Space:
                    Debug.WriteLine("KliknietoSpacja");
                    if (!czyZapauzowane)
                        tetris.Obroc(this);
                    break;
                case Key.R:
                    Debug.WriteLine("KliknietoR");
                    Restart();
                    break;
                case Key.P:
                    Debug.WriteLine("KliknietoP");
                    Pause();
                    break;
                case Key.Escape:
                    Debug.WriteLine("KliknietoEsc");
                    mainTimer.Tick -= eventHandlerTick;
                    Application.Current.MainWindow.KeyDown -= keyEventHandler;
                    MainMenu main = new MainMenu();
                    App.Current.MainWindow.Close();
                    App.Current.MainWindow = main;
                    App.Current.MainWindow.Show();
                    break;
            }
        }

        public void WyczyscLinie()
        {
            int combo = 0;
            for (int i = 0; i < Instance.GameBoard1.RowDefinitions.Count; i++)
            {
                int j;
                for (j = 0; j < Instance.GameBoard1.ColumnDefinitions.Count; j++)
                {
                    if (lokacjaOstatniegoTetrisaGrid[i, j] == 0) break;
                }

                if (j == 10)
                {
                    wyczyszczoneLinie++;
                    combo++;
                    isLineCleared = true;
                    for (j = 0; j < 10; j++)//czyszczenie pełnej linii
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

                    for (int z = 0; z < Instance.GameBoard1.RowDefinitions.Count; z++)
                    {
                        for (int x = 0; x < Instance.GameBoard1.ColumnDefinitions.Count; x++)
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

        
        
        public void Podsumowanie()
        {
            mainTimer.Tick -= eventHandlerTick;
            Application.Current.MainWindow.KeyDown -= keyEventHandler;
            App.Current.MainWindow.Content = new Summary(this);
            App.Current.MainWindow.Height = 450;
            App.Current.MainWindow.Width = 800;

            //centrowanie okna gry
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = App.Current.MainWindow.Width;
            double windowHeight = App.Current.MainWindow.Height;
            App.Current.MainWindow.Left = (screenWidth / 2) - (windowWidth / 2);
            App.Current.MainWindow.Top = (screenHeight / 2) - (windowHeight / 2);

        }

        public void Pause()
        {
            if (mainTimer.IsEnabled)
            {
                mainTimer.Stop();
                czyZapauzowane = true;
                PausePopUp dlg = new PausePopUp();
                dlg.Owner = App.Current.MainWindow;

                dlg.ShowDialog();
                if (dlg.DialogResult == true)
                {
                    mainTimer.Start();
                    czyZapauzowane = false;
                }
            }

        }
    }

    public abstract class GameBoardDecorator : GameBoard
    {
        protected GameBoard gameBoard;

        public GameBoardDecorator(GameBoard _gameBoard)
        {
            //mainTimer.Tick -= eventHandlerTick;
            //Application.Current.MainWindow.KeyDown -= keyEventHandler;
            this.gameBoard = _gameBoard;
            
        }

        public override void InitializeVariables()
        {
            gameBoard.InitializeVariables();
        }

        public override void UstawTlo()
        {
            gameBoard.UstawTlo();
        }

        public override void DrawGameBoard2()
        {
            gameBoard.DrawGameBoard2();
        }

        public override void Rysuj()
        {
            gameBoard.Rysuj();
        }

        public override void FallingAction()
        {
            gameBoard.FallingAction();
        }

        public override void AdditionalAction()
        {
            gameBoard.AdditionalAction();
        }

    }

    public class LandSlideDecorator : GameBoardDecorator
    {


        public LandSlideDecorator(GameBoard gameBoard)
            :base(gameBoard)
        {
        }
        public override void DrawGameBoard2()
        {
            base.DrawGameBoard2();
            Instance.TrybGry.Text = "Osuwisko za: ";
        }

        public override GameBoard getNew()
        {
            return new LandSlideDecorator(gameBoard.getNew());
        }

        public override void AdditionalAction()
        {
            gameBoard.AdditionalAction();
            playTime = (int)timer.ElapsedMilliseconds / 1000;

            float czas1 = 16 - playTime;
            Instance.Czas.Text = czas1.ToString();

            if (playTime > 15)
            {
                for (int i = 0; i < 5; i++)
                {
                    Tetrimo.Landslide(this);
                }
                timer.Restart();
            }
            
        }
    }

    public class HauntedDecorator : GameBoardDecorator
    {
        public static bool isHaunted;

        public HauntedDecorator(GameBoard gameBoard)
            : base(gameBoard)
        {
        }

        public override GameBoard getNew()
        {
            return new HauntedDecorator(gameBoard.getNew());
        }

        public override void InitializeVariables()
        {
            base.InitializeVariables();
            isHaunted = false;
        }

        public override void Rysuj()
        {
            Instance.GameBoard1.Children.Clear();
            for (int i = 0; i < Instance.GameBoard1.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < Instance.GameBoard1.ColumnDefinitions.Count; j++)
                {
                    if (grid[i, j] == 1 || lokacjaOstatniegoTetrisaGrid[i, j] == 1)
                    {
                        Border border = new Border();
                        border.Width = Size - Padding * 2;
                        border.Height = Size - Padding * 2;
                        Instance.GameBoard1.Children.Add(border);

                        if (tetrisColorGrid[i, j] < 1 || tetrisColorGrid[i, j] > 8)
                        {
                            border.Background = ImgAssign(aktualnyKolor);
                            border.Padding = new Thickness(0);
                        }

                        else
                        {
                            border.Background = ImgAssign(tetrisColorGrid[i, j]);
                            border.Padding = new Thickness(0);
                        }

                        
                        if (czyPokazywać || grid[i, j] == 1)
                        {
                            Grid.SetRow(border, i);
                            Grid.SetColumn(border, j);
                        }
                        else
                        {
                            border.Background = new SolidColorBrush(Color.FromArgb(127, 15, 21, 33));
                            border.Padding = new Thickness(0);

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

                        border.Background = new SolidColorBrush(Color.FromArgb(127, 15, 21, 33));
                        border.Padding = new Thickness(0);
                        Instance.GameBoard1.Children.Add(border);
                    }

                }
            }
        }

        public override void FallingAction()
        {
            if (!isHaunted)
            {
                czyPokazywać = true;
                isHaunted = true;
                timer.Restart();
            }
            gameBoard.FallingAction();
        }

        public override void AdditionalAction()
        {
            gameBoard.AdditionalAction();
            if (isHaunted)
            {
                playTime = (int)timer.ElapsedMilliseconds;
                if (playTime > 800)
                {
                    isHaunted = false;
                    czyPokazywać = false;
                }
            }
        }

    }

    public class LevelLimitDecorator : GameBoardDecorator
    {
        public LevelLimitDecorator(GameBoard gameBoard)
            : base(gameBoard)
        {
        }

        public override GameBoard getNew()
        {
            return new LevelLimitDecorator(gameBoard.getNew());
        }

        public override void AdditionalAction()
        {
            gameBoard.AdditionalAction();
            if (poziom == 15)
            {
                czyGameOver = true;
            }
        }

    }

    public class TimeLimitDecorator : GameBoardDecorator
    {


        public TimeLimitDecorator(GameBoard gameBoard)
            : base(gameBoard)
        {
        }

        public override GameBoard getNew()
        {
            return new TimeLimitDecorator(gameBoard.getNew());
        }

        public override void DrawGameBoard2()
        {
            base.DrawGameBoard2();
            Instance.TrybGry.Text = "Pozostały czas: ";
        }

        public override void AdditionalAction()
        {
            if (poziom < 5) poziom = 5;
            playTime = (int)timer.ElapsedMilliseconds / 1000;
            //Console.SetCursorPosition(5, 4);
            // Console.Write("Pozotały czas: ");
            // Console.SetCursorPosition(21, 4);
            float czas = 180 - playTime;
            Instance.Czas.Text = czas.ToString();
            //  Console.Write("" + czas + " ");
            if (playTime > 180)
            {
                czyGameOver = true;
            }
            gameBoard.AdditionalAction();
        }

    }

    public class RegularGameBoard : GameBoard//component
    {
        
        

        public RegularGameBoard()
        {
            //InitializeGameBoard();
        }



        public override void UstawTlo()
        {
            ImageBrush tlo = new ImageBrush();
            Image obrazek = new Image();
            var filename = "../../Images/dark-background.jpg";
            obrazek.Source = new BitmapImage(new Uri(filename, UriKind.Relative));
            tlo.ImageSource = obrazek.Source;
            Instance.Background = tlo;
        }

        

        public override void InitializeVariables()
        {
            grid = new int[boardHeight, boardWidth];
            lokacjaOstatniegoTetrisaGrid = new int[boardHeight, boardWidth];
            tetrisColorGrid = new int[boardHeight, boardWidth];

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
            isLineCleared = false;
            czyZapauzowane = false;
            czyGameOver = false;
            playTime = 0;
            czyPokazywać = false;
        }

        


        public override void DrawGameBoard2()
        {
            int Rows = Instance.GameBoard1.RowDefinitions.Count;
            int Columns = Instance.GameBoard1.ColumnDefinitions.Count;

            Label[,] BlockControls = new Label[Columns, Rows];
            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    Border border = new Border();


                    double width = Instance.GameBoard1.ColumnDefinitions[0].ActualWidth;

                    border.Width = 30 - Padding * 2;
                    border.Height = 30 - Padding * 2;

                    Grid.SetRow(border, j);
                    Grid.SetColumn(border, i);


                    border.Background = new SolidColorBrush(Color.FromArgb(127, 15, 21, 33));//#7F0F1521
                    border.Padding = new Thickness(0);

                    Instance.GameBoard1.Children.Add(border);
                }
            }
        }

        public override void Rysuj()
        {
            Instance.GameBoard1.Children.Clear();
            for (int i = 0; i < Instance.GameBoard1.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < Instance.GameBoard1.ColumnDefinitions.Count; j++)
                {
                    if (grid[i, j] == 1 || lokacjaOstatniegoTetrisaGrid[i, j] == 1)
                    {
                        Border border = new Border();
                        border.Width = Size - Padding * 2;
                        border.Height = Size - Padding * 2;
                        Instance.GameBoard1.Children.Add(border);

                        if (tetrisColorGrid[i, j] < 1 || tetrisColorGrid[i, j] > 8)
                        {
                            border.Background = ImgAssign(aktualnyKolor);
                            border.Padding = new Thickness(0);
                        }

                        else
                        {
                            border.Background = ImgAssign(tetrisColorGrid[i, j]);
                            border.Padding = new Thickness(0);
                        }

                        Grid.SetRow(border, i);
                        Grid.SetColumn(border, j);

                    }
                    else
                    {
                        Border border = new Border();

                        border.Width = 30 - Padding * 2;
                        border.Height = 30 - Padding * 2;

                        Grid.SetRow(border, i);
                        Grid.SetColumn(border, j);

                        border.Background = new SolidColorBrush(Color.FromArgb(127, 15, 21, 33));
                        border.Padding = new Thickness(0);
                        Instance.GameBoard1.Children.Add(border);
                    }

                }
            }
        }

        public override void FallingAction()
        {
            Random rnd = new Random();

            tetris = następnyTetris;
            następnyTetris = new Tetrimo();

            aktualnyKolor = nastepnyKolor;
            nastepnyKolor = rnd.Next(1, 7);

            RysujNastepny(następnyTetris.getKształt());

            if (!tetris.Stwórz(this))
            {
                czyGameOver = true;

            }

            czyOpadł = false;
        }

        public override void AdditionalAction()
        {
        }

        public override GameBoard getNew()
        {
            return new RegularGameBoard();
        }

        
    }
    /*
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Logika interakcji dla klasy GameBoard.xaml
    /// </summary>
    public  partial class GameBoard : Page
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
        public static string filenameTetrisa = "../../Images/";
        public static Tetromino tetris;
        public static Tetromino następnyTetris;

        public static TetrominoFactory fabryka;

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

        public static KeyEventHandler keyEventHandler;
        public static EventHandler eventHandlerTick ;

        //zmienna BazyDanych
        //private DataOperator bazaWynikow;

        public DispatcherTimer mainTimer;

        public static GameBoard Instance { get; private set; }

        public GameBoard(string GameMode)
        {
            
            Instance = this;
            InitializeComponent();
            UstawTlo();
            InitializeVariables();
            //DrawGameBoardStart();
            DrawGameBoard2();
            //Test1
            Rysuj();

            keyEventHandler = new KeyEventHandler(Sterowanie2);
            Application.Current.MainWindow.KeyDown += keyEventHandler;
            

            actualGameMode = GameMode;

            if(GameMode == "   Ultra  ")
            {
                TrybGry.Text = "Pozostały czas: ";
            }
            if(GameMode == " LandSlide ")
            {
                TrybGry.Text = "Osuwisko za: ";
            }

            fabryka = new TetrominoFactory();

            tetris = fabryka.FactoryMethod();
            następnyTetris = fabryka.FactoryMethod();
            tetris.Stwórz();
            RysujNastepny(następnyTetris.getKształt());

            TimerStart();

            DataOperator bazaWynikow = DataOperator.getInstance();
            //bazaWynikow.InicjalizujBazeWynikow();

            // Uruchom();
        }

        

        public void UstawTlo()
        {
            ImageBrush tlo = new ImageBrush();
            Image obrazek = new Image();
            var filename = "../../Images/dark-background.jpg";
            obrazek.Source = new BitmapImage(new Uri(filename, UriKind.Relative));
            tlo.ImageSource = obrazek.Source;
            this.Background = tlo;
        }

        public void TimerStart()
        {
            mainTimer = new DispatcherTimer();
            mainTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            eventHandlerTick = Callback;
            mainTimer.Tick += eventHandlerTick;
            //mainTimer.Tick += Callback;
            mainTimer.Start();
        }

        public void InitializeVariables()
        {
            grid = new int[boardHeight, boardWidth];
            lokacjaOstatniegoTetrisaGrid = new int[boardHeight, boardWidth];
            tetrisColorGrid = new int[boardHeight, boardWidth];

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
            isLineCleared = false;
            czyZapauzowane = false;
            czyGameOver = false;
            playTime = 0;
            czyPokazywać = false;
            isHaunted = false;
            actualGameMode = " LandSlide ";
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
            int Rows = GameBoard1.RowDefinitions.Count;
            int Columns = GameBoard1.ColumnDefinitions.Count;

            Label[,] BlockControls = new Label[Columns, Rows];
            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    Border border = new Border();


                    double width = GameBoard1.ColumnDefinitions[0].ActualWidth;

                    border.Width = 30 - Padding * 2;
                    border.Height = 30 - Padding * 2;

                    Grid.SetRow(border, j);
                    Grid.SetColumn(border, i);

                    
                    border.Background = new SolidColorBrush(Color.FromArgb(127, 15, 21, 33));//#7F0F1521
                    border.Padding = new Thickness(0);

                    GameBoard1.Children.Add(border);
                }
            }
        }


        public static ImageBrush ImgAssign(int rodzaj)
        {
            
            string fullFileName = filenameTetrisa+"tetrisElement/";
            switch (rodzaj)
            {
                case 1:
                    fullFileName+="Aqua";
                    break;
                case 2:
                    fullFileName += "Blue";
                    break;
                case 3:
                    fullFileName += "Yellow";
                    break;
                case 4:
                    fullFileName += "Green";
                    break;
                case 5:
                    fullFileName += "Magenta";
                    break;
                case 6:
                    fullFileName += "Red";
                    break;
                case 7:
                    fullFileName += "DarkGreen";
                    break;
                default:
                    fullFileName += "Black";
                    break;
            }

            fullFileName +=".png";

            BitmapImage img = new BitmapImage(new Uri(fullFileName, UriKind.Relative));
            ImageBrush image = new ImageBrush();
            image.ImageSource = img;
            return image;
        }


        public static void Rysuj()
        {
            Instance.GameBoard1.Children.Clear();
            for (int i = 0; i < Instance.GameBoard1.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < Instance.GameBoard1.ColumnDefinitions.Count; j++)
                {
                    if (grid[i, j] == 1 || lokacjaOstatniegoTetrisaGrid[i, j] == 1)
                    {
                        Border border = new Border();
                        border.Width = Size - Padding * 2;
                        border.Height = Size - Padding * 2;
                        Instance.GameBoard1.Children.Add(border);

                        if (tetrisColorGrid[i, j] < 1 || tetrisColorGrid[i, j] > 8)
                        {
                            border.Background = ImgAssign(aktualnyKolor);
                            //border.Background = new SolidColorBrush(ColorAssign(aktualnyKolor));
                            border.Padding = new Thickness(0);
                            //Console.ForegroundColor = Color(aktualnyKolor);
                        }

                        else
                        {
                            border.Background = ImgAssign(tetrisColorGrid[i, j]);
                            //border.Background = new SolidColorBrush(ColorAssign(tetrisColorGrid[i, j]));
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
                                border.Background = new SolidColorBrush(Color.FromArgb(127, 15, 21, 33));
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

                        border.Background = new SolidColorBrush(Color.FromArgb(127, 15, 21, 33));
                        border.Padding = new Thickness(0);
                        Instance.GameBoard1.Children.Add(border);
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

                    border.Background = new SolidColorBrush(Color.FromArgb(127, 15, 21, 33));
                    border.Padding = new Thickness(0);
                }
            }

            for (int i = 0; i < kształt.GetLength(0); i++)
            {
                for (int j = 0; j < kształt.GetLength(1); j++)
                {
                    Border border = new Border();
                    NextTetris.Children.Add(border);
                    border.Width = 31 - Padding * 2;
                    border.Height = 31 - Padding * 2;

                    if (kształt[i, j] == 1)
                    {
                        Grid.SetRow(border, i);
                        Grid.SetColumn(border, j);
                        border.Background = ImgAssign(nastepnyKolor);
                        //border.Background = new SolidColorBrush(ColorAssign(nastepnyKolor));
                        border.Padding = new Thickness(0);
                    }
                }
            }
        }

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {

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
                    następnyTetris = fabryka.FactoryMethod();

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
                    mainTimer.Stop();
                    czyZapauzowane = true;
                    GameOverPopUp dlg = new GameOverPopUp();
                    dlg.Owner = App.Current.MainWindow;

                    dlg.ShowDialog();
                    if (dlg.DialogResult == true) //wybrano przejscie do podsumowania
                    {
                        Podsumowanie();
                    }
                    else //wybrano opcje restartu
                    {
                        Restart();
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
                    //Console.SetCursorPosition(5, 4);
                   // Console.Write("Pozotały czas: ");
                   // Console.SetCursorPosition(21, 4);
                    float czas = 180 - playTime;
                    Czas.Text = czas.ToString();
                  //  Console.Write("" + czas + " ");
                    if (playTime > 180)
                    {
                        czyGameOver = true;
                    }

                    break;
                case " LandSlide ":
                    playTime = (int)timer.ElapsedMilliseconds / 1000;

                    //Console.SetCursorPosition(5, 4);
                    //Console.Write("Następny osuw za: ");
                    //Console.SetCursorPosition(23, 4);

                    float czas1 = 16 - playTime;
                    Czas.Text = czas1.ToString();

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
            switch (e.Key)
            {
                case Key.Left:
                    Debug.WriteLine("KliknietoLewo");
                    if (!tetris.czyJestCosZLewa() && !czyZapauzowane)
                    {
                        for (int i = 0; i < 4; i++)
                        {

                            tetris.lokacja[i][1] -= 1;

                        }
                        tetris.Aktualizuj();
                    }
                    break;
                case Key.Right:
                    Debug.WriteLine("KliknietoPrawo");
                    if (!tetris.czyJestCosZPrawa() && !czyZapauzowane)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            tetris.lokacja[i][1] += 1;
                        }
                        tetris.Aktualizuj();
                    }
                    break;
                case Key.Down:
                    Debug.WriteLine("KliknietoDol");
                    if (!czyZapauzowane)
                        tetris.Opadaj();
                    break;
                case Key.Up:
                    Debug.WriteLine("KliknietoGora");
                    while (tetris.czyJestCosPonizej() != true && !czyZapauzowane)
                    {
                        tetris.Opadaj();
                    }
                    break;
                case Key.Space:
                    Debug.WriteLine("KliknietoSpacja");
                    if (!czyZapauzowane)
                        tetris.Obroc(1);
                    break;
                case Key.Z:
                    Debug.WriteLine("KliknietoLAlt");
                    if (!czyZapauzowane)
                        tetris.Obroc(2);
                    break;
                case Key.R:
                    Debug.WriteLine("KliknietoR");
                    Restart();
                    break;
                case Key.P:
                    Debug.WriteLine("KliknietoP");
                    Pause();
                    break;
                case Key.Escape:
                    Debug.WriteLine("KliknietoEsc");
                    mainTimer.Tick -= eventHandlerTick;
                    Application.Current.MainWindow.KeyDown -= keyEventHandler;
                    MainMenu main = new MainMenu();
                    App.Current.MainWindow.Close();
                    App.Current.MainWindow = main;
                    App.Current.MainWindow.Show();
                    break;
            }
        }

        public void WyczyscLinie()
        {
            int combo = 0;
            for (int i = 0; i < Instance.GameBoard1.RowDefinitions.Count; i++)
            {
                int j;
                for (j = 0; j < Instance.GameBoard1.ColumnDefinitions.Count; j++)
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

                    for (int z = 0; z < Instance.GameBoard1.RowDefinitions.Count; z++)
                    {
                        for (int x = 0; x < Instance.GameBoard1.ColumnDefinitions.Count; x++)
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
            mainTimer.Tick -= eventHandlerTick;
            Application.Current.MainWindow.KeyDown -= keyEventHandler;
            App.Current.MainWindow.Content = new GameBoard(actualGameMode);
        }

        public void Podsumowanie()
        {
            mainTimer.Tick -= eventHandlerTick;
            Application.Current.MainWindow.KeyDown -= keyEventHandler;
            App.Current.MainWindow.Content = new Summary();
            App.Current.MainWindow.Height = 450;
            App.Current.MainWindow.Width = 800;

            //centrowanie okna gry
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = App.Current.MainWindow.Width;
            double windowHeight = App.Current.MainWindow.Height;
            App.Current.MainWindow.Left = (screenWidth / 2) - (windowWidth / 2);
            App.Current.MainWindow.Top = (screenHeight / 2) - (windowHeight / 2);

        }

        public void Pause()
        {
            if (mainTimer.IsEnabled)
            {
                mainTimer.Stop();
                czyZapauzowane = true;
                PausePopUp dlg = new PausePopUp();
                dlg.Owner = App.Current.MainWindow;
                
                dlg.ShowDialog();
                if (dlg.DialogResult == true)
                {
                    mainTimer.Start();
                    czyZapauzowane = false;
                }
            }
            
        }
    }*/
}


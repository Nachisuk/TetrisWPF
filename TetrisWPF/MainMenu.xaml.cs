using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace TetrisWPF.Properties
{
    /// <summary>
    /// Logika interakcji dla klasy MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        public static MainMenu Instance { get; private set; }
        public static bool czyTryby,czyZmienic;
        public int i;
        public KeyEventHandler keyEventHandler;
        //public List<Label> labellist;
        //public static BazaWynikow bazaWynikow;
        MenuState state;

        public MainMenu()
        {
            InitializeComponent();
            Instance = this;
            //this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            UstawTlo();


            state = new MenuStateOptions(this);

            keyEventHandler = new KeyEventHandler(SterowanieMenu);
            Application.Current.MainWindow.KeyDown += keyEventHandler;

            WriteLabels();
        }

        public void setState(MenuState current)
        {
            state = current;
        }

        public void WriteLabels()
        {
            List<String> tmpList = state.GetCurrent3Options();
            Opcja0.Content = tmpList[0];
            Opcja1.Content = tmpList[1];
            Opcja2.Content = tmpList[2];
        }

        public void UstawTlo()
        {
            ImageBrush tlo = new ImageBrush();
            Image obrazek = new Image();
            var filename = "../../Images/tetrisMainMenu_chilled.jpg";
            //var filename = "../../Images/tetrisMainMenu.jpg";
            obrazek.Source = new BitmapImage(new Uri(filename, UriKind.Relative));
            tlo.ImageSource = obrazek.Source;
            this.Background = tlo;

            ImageBrush napis = new ImageBrush();
            Image obrazek1 = new Image();
            var filename1 = "../../Images/mainmenutextV2.png";
            obrazek1.Source = new BitmapImage(new Uri(filename1, UriKind.Relative));
            napis.ImageSource = obrazek1.Source;
            NapisTytulowy.Background = napis;
        }

        
        public void SterowanieMenu(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Left:
                    Console.Out.Write("Wcisnieto lewy menu");
                    state.LeftPressed();
                    WriteLabels();
                    break;

                case Key.Right:
                    state.RightPressed();
                    WriteLabels();
                    break;

                case Key.Enter:
                    state.EnterMiddle(this);
                    WriteLabels();
                    break;
                case Key.Escape:
                    state.GoBack(this);
                    WriteLabels();
                    break;
            }
        }

        public static void GameModeStart(string gamemode)
        {
            GameBoard main = new RegularGameBoard();

            switch (gamemode)
            {
                case "  Maraton ":
                    main = new LevelLimitDecorator(main);
                    break;
                case "  Endless ":

                    break;
                case "   Ultra  ":
                    main = new TimeLimitDecorator(main);
                    break;
                case " LandSlide ":
                    main = new LandSlideDecorator(main);
                    break;
                case "  Haunted  ":
                    main = new HauntedDecorator(main);
                    break;
            }

            

            App.Current.MainWindow.Content = main.InitializeGameBoard();


            
            App.Current.MainWindow.Height = 1000;
            App.Current.MainWindow.Width = 850;

            //centrowanie okna gry
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = App.Current.MainWindow.Width;
            double windowHeight = App.Current.MainWindow.Height;
            App.Current.MainWindow.Left = (screenWidth / 2) - (windowWidth / 2);
            App.Current.MainWindow.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private void Opcja0_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var key = Key.Left;
            var target = this;
            var routedEvent = Keyboard.KeyDownEvent;

            target.RaiseEvent(
                new KeyEventArgs(
                    Keyboard.PrimaryDevice,
                    PresentationSource.FromVisual(target),
                    0,
                    key)
                { RoutedEvent = routedEvent }
                );
        }

        private void Opcja2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var key = Key.Right;
            var target = this;
            var routedEvent = Keyboard.KeyDownEvent;

            target.RaiseEvent(
                new KeyEventArgs(
                    Keyboard.PrimaryDevice,
                    PresentationSource.FromVisual(target),
                    0,
                    key)
                { RoutedEvent = routedEvent }
                );
        }

        private void Opcja1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var key = Key.Enter;
            var target = this;
            var routedEvent = Keyboard.KeyDownEvent;

            target.RaiseEvent(
                new KeyEventArgs(
                    Keyboard.PrimaryDevice,
                    PresentationSource.FromVisual(target),
                    0,
                    key)
                { RoutedEvent = routedEvent }
                );
        }

        public void ScoreBoard()
        {
            Application.Current.MainWindow.KeyDown -= keyEventHandler;
            App.Current.MainWindow.Content = new VM1();
        }
    }

    

    public abstract class MenuState
    {
        public int indexOfCurrentMiddle = 1;
        protected List<MainMenuOptions> listOfOptions;

        
        public abstract void LeftPressed();
        public abstract void RightPressed();
        public abstract void EnterMiddle(MainMenu context);
        public abstract void GoBack(MainMenu context);

        public List<String> GetCurrent3Options()
        {
            List<String> result = new List<String>();
            int tmp = mod((indexOfCurrentMiddle - 1) ,listOfOptions.Count());
            result.Add(listOfOptions[mod((indexOfCurrentMiddle - 1), listOfOptions.Count())].zwrocNazwe());
            result.Add(listOfOptions[mod(indexOfCurrentMiddle, listOfOptions.Count())].zwrocNazwe());
            result.Add(listOfOptions[mod((indexOfCurrentMiddle+1), listOfOptions.Count())].zwrocNazwe());
            return result;
        }

        public int mod(int x, int m)
        {
            return (x % m + m) % m;
        }
    }

    public class MenuStateOptions : MenuState
    {
        public MenuStateOptions(MainMenu context)
        {
            listOfOptions = new List<MainMenuOptions>();
            listOfOptions.Add(new MainMenu_ClassicTet());
            listOfOptions.Add(new MainMenu_Scoreboard(context));
            listOfOptions.Add(new MainMenu_Statystyki());
            listOfOptions.Add(new MainMenu_ExitGame());

            indexOfCurrentMiddle = 1;
        }



        public override void LeftPressed()
        {
            indexOfCurrentMiddle--;
            indexOfCurrentMiddle = mod(indexOfCurrentMiddle, listOfOptions.Count());
        }

        public override void RightPressed()
        {
            indexOfCurrentMiddle++;
            indexOfCurrentMiddle = mod(indexOfCurrentMiddle, listOfOptions.Count());
        }

        public override void EnterMiddle(MainMenu context)
        {
            if (listOfOptions[indexOfCurrentMiddle].zwrocNazwe().Trim(' ').ToLower()=="graj")
            {
                context.setState(new MenuStateGameModes());
                return;
            }
            else
            {
                Application.Current.MainWindow.KeyDown -= context.keyEventHandler;
                listOfOptions[indexOfCurrentMiddle].FunkcjaOpcji();
            }
                

        }

        public override void GoBack(MainMenu context)
        {
            Environment.Exit(0);
        }
    }

    public class MenuStateGameModes : MenuState
    {
        public MenuStateGameModes()
        {
            listOfOptions = new List<MainMenuOptions>();
            listOfOptions.Add(new GameMode_Marathon());
            listOfOptions.Add(new GameMode_Endless());
            listOfOptions.Add(new GameMode_Ultra());
            listOfOptions.Add(new GameMode_LandSlide());
            listOfOptions.Add(new GameMode_Haunted());

            indexOfCurrentMiddle = 1;
        }



        public override void LeftPressed()
        {
            indexOfCurrentMiddle--;
            indexOfCurrentMiddle = mod(indexOfCurrentMiddle ,listOfOptions.Count());
        }

        public override void RightPressed()
        {
            indexOfCurrentMiddle++;
            indexOfCurrentMiddle = mod(indexOfCurrentMiddle, listOfOptions.Count());
        }

        public override void EnterMiddle(MainMenu context)
        {
            Application.Current.MainWindow.KeyDown -= context.keyEventHandler;
            listOfOptions[indexOfCurrentMiddle].FunkcjaOpcji();
        }

        public override void GoBack(MainMenu context)
        {
            context.setState(new MenuStateOptions(context));
        }
    }

}

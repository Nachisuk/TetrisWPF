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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TetrisWPF.Properties;

namespace TetrisWPF
{
    /// <summary>
    /// Logika interakcji dla klasy StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public  StartWindow ThisWindow;
        public  int MainWindowHeight = 700;
        public  int MainWindowWidth = 1100;
        public  int GameWindowHeight = 1000;
        public  int GameWindowWidth = 850;

        public WindowContent currentContent;

        public StartWindow()
        {
            InitializeComponent();
            ThisWindow = this;
            App.Current.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SetContent(new StartUpContent(ThisWindow));
            ResumeContent();
        }

        public void SetContent(WindowContent content)
        {
            if (currentContent != null)
                currentContent.RemoveHandler();
            currentContent = content;
        }

        public void ResumeContent ()
        {
            ThisWindow.Content = currentContent.getPage();
            
        }

        public void RemoveCurrentHandler()
        {
            currentContent.RemoveHandler();
        }

        public void ChangeWindowResolution(int height, int width)
        {
            ThisWindow.Height = height;
            ThisWindow.Width = width;
            //centrowanie okna gry
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = ThisWindow.Width;
            double windowHeight = ThisWindow.Height;
            ThisWindow.Left = (screenWidth / 2) - (windowWidth / 2);
            ThisWindow.Top = (screenHeight / 2) - (windowHeight / 2);
        }

    }

    public abstract class WindowContent
    {
        public KeyEventHandler keyEventHandler;
        public StartWindow requestingWindow;
        
        public WindowContent(StartWindow _window)
        {
            requestingWindow = _window;        
        }

        public  void RemoveHandler()
        {
            requestingWindow.KeyDown -= keyEventHandler;
        }

        public abstract Page getPage();
        


    }

    public class StartUpContent : WindowContent
    {
        public MainMenuStart page;

        public StartUpContent(StartWindow _window) 
            : base(_window)
        {
        }

        public override Page getPage()
        {
            page = new MainMenuStart(requestingWindow);
            keyEventHandler = new KeyEventHandler(page.Sterowanie);
            requestingWindow.KeyDown += keyEventHandler;
            requestingWindow.ChangeWindowResolution(requestingWindow.MainWindowHeight, requestingWindow.MainWindowWidth);
            return page;
        }

    }

    public class MainMenuContent : WindowContent
    {
        public MainMenu page;

        public MainMenuContent(StartWindow _window)
            : base(_window)
        {
        }

        public override Page getPage()
        {
            page = new MainMenu(requestingWindow);
            keyEventHandler = new KeyEventHandler(page.Sterowanie);
            requestingWindow.KeyDown += keyEventHandler;
            requestingWindow.ChangeWindowResolution(requestingWindow.MainWindowHeight, requestingWindow.MainWindowWidth);
            return page;
        }

    }

    public class ScoreBoardContent : WindowContent
    {
        public ScoreBoard page;

        public ScoreBoardContent(StartWindow _window)
            : base(_window)
        {
        }

        public override Page getPage()
        {
            page = new ScoreBoard(requestingWindow);
            keyEventHandler = new KeyEventHandler(page.Sterowanie);
            requestingWindow.KeyDown += keyEventHandler;
            requestingWindow.ChangeWindowResolution(requestingWindow.MainWindowHeight, requestingWindow.MainWindowWidth);
            return page;
        }

    }

    public class GameContent : WindowContent
    {
        public GameBoard page;
        public String mode;
        
        public GameContent(StartWindow _window, String _mode)
            : base(_window)
        {
            mode = _mode;
        }

        public GameContent(StartWindow _window, GameBoard _pageDest)
            : base(_window)
        {
            page = _pageDest;
        }

        public override Page getPage()
        {
            if(page == null)
            {
                page = new RegularGameBoard(requestingWindow);

                switch (mode)
                {
                    case "  Maraton ":
                        page = new LevelLimitDecorator(page);
                        break;
                    case "  Endless ":

                        break;
                    case "   Ultra  ":
                        page = new TimeLimitDecorator(page);
                        break;
                    case " LandSlide ":
                        page = new LandSlideDecorator(page);
                        break;
                    case "  Haunted  ":
                        page = new HauntedDecorator(page);
                        break;
                }
            }
            
            keyEventHandler = new KeyEventHandler(page.Sterowanie);
            requestingWindow.KeyDown += keyEventHandler;
            requestingWindow.ChangeWindowResolution(requestingWindow.GameWindowHeight, requestingWindow.GameWindowWidth);
            return page.InitializeGameBoard();
        }

    }

    public class SummaryContent : WindowContent
    {
        public Summary page;
        public GameBoard gameboard;


        public SummaryContent(GameBoard _gameboard, StartWindow _window)
            : base(_window)
        {
            gameboard = _gameboard;
        }

        public override Page getPage()
        {
            page = new Summary(gameboard, requestingWindow);
            keyEventHandler = new KeyEventHandler(page.Sterowanie);
            requestingWindow.KeyDown += keyEventHandler;
            requestingWindow.ChangeWindowResolution(requestingWindow.MainWindowHeight, requestingWindow.MainWindowWidth);
            return page;
        }

    }

}

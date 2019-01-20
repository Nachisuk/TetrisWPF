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
using TetrisWPF.Properties;

namespace TetrisWPF
{
    /// <summary>
    /// Logika interakcji dla klasy Summary.xaml
    /// </summary>
    public partial class Summary : Page
    {
        private String gamemode;
        public StartWindow requestingWindow;

        public Summary(GameBoard _gameboard, StartWindow _window)
        {
            InitializeComponent();
            requestingWindow = _window;
            gamemode = NameOfMode(_gameboard);
            UstawTlo();
            UstawWyniki();
            textBox.Focus();
        }

        private String NameOfMode(GameBoard gameBoard)
        {
            String res = "";
            if (!(gameBoard is GameBoardDecorator))
                res = "  Endless ";
            else
            {
                if (gameBoard is HauntedDecorator)
                    res = "  Haunted  ";
                else if (gameBoard is LandSlideDecorator)
                    res = " LandSlide ";
                else if (gameBoard is TimeLimitDecorator)
                    res = "   Ultra  ";
                else if (gameBoard is LevelLimitDecorator)
                    res = "  Maraton ";
            }
            return res;
        }

        public void UstawTlo()
        {
            ImageBrush tlo = new ImageBrush();
            Image obrazek = new Image();
            var filename = "../../Images/scoreBoardBackgroundV2.jpg";
            obrazek.Source = new BitmapImage(new Uri(filename, UriKind.Relative));
            tlo.ImageSource = obrazek.Source;
            this.Background = tlo;

            ImageBrush napis = new ImageBrush();
            Image obrazek1 = new Image();
            var filename1 = "../../Images/podsumowanieTextV2.png";
            obrazek1.Source = new BitmapImage(new Uri(filename1, UriKind.Relative));
            napis.ImageSource = obrazek1.Source;
            napisGrid.Background = napis;
        }

        public void UstawWyniki()
        {
            string outstring = "";
            outstring += "Poziom:\t" + GameBoard.poziom;
            outstring += "\nPunkty:\t" + GameBoard.punkty;
            outstring += "\nLinie:\t" + GameBoard.wyczyszczoneLinie;
            textBlock.Text = outstring;
        }

        public void Sterowanie(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                
                case Key.Enter:
                    
                    Debug.WriteLine("EntrWcisniety");
                    

                    string imie = textBox.Text.Replace(" ","");
                    if (String.IsNullOrEmpty(imie))
                    {
                        imie = "Anonim";
                    }



                    DataOperator.getInstance().TryZapisacDanyWynik(GameBoard.punkty, gamemode, imie);

                    requestingWindow.SetContent(new MainMenuContent(requestingWindow));
                    requestingWindow.ResumeContent();
                    


                    break;
            }
        }

        

    }
}

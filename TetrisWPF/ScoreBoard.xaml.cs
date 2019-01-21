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

namespace TetrisWPF
{
    /// <summary>
    /// Logika interakcji dla klasy ScoreBoard.xaml
    /// </summary>
    public partial class ScoreBoard : Page
    {
        private DataOperator bazaWynikow;
        private List<String> lista;
        private int iloscOpcji;
        private int aktualna;
        public StartWindow requestingWindow;

        public ScoreBoard(StartWindow _window)
        {
            InitializeComponent();
            requestingWindow = _window;
            bazaWynikow = DataOperator.getInstance();
            UstawTlo();

            lista = new List<String>();
            lista.Add("maraton");
            lista.Add("endless");
            lista.Add("ultra");
            lista.Add("landslide");
            lista.Add("haunted");

            iloscOpcji = lista.Count;
            aktualna = 0;

            WypiszWyniki(lista[aktualna]);
        }

        public void Sterowanie(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    Debug.WriteLine("LewyWcisniety");
                    aktualna -= 1;
                    if (aktualna < 0)
                    {
                        aktualna = iloscOpcji + aktualna;
                    }
                    WypiszWyniki(lista[aktualna]);
                    break;

                case Key.Right:
                    Debug.WriteLine("PrawyWcisniety");
                    aktualna += 1;
                    aktualna = aktualna % iloscOpcji;
                    WypiszWyniki(lista[aktualna]);
                    break;
                case Key.Escape:
                    Debug.WriteLine("EscWcisniety");
                    requestingWindow.SetContent(new MainMenuContent(requestingWindow));
                    requestingWindow.ResumeContent();
                    break;
            }
        }

        public void UstawTlo()
        {
            ImageBrush tlo = new ImageBrush();
            Image obrazek = new Image();
            var filename = "../../Images/scoreBoardBackgroundV2.jpg";
            obrazek.Source = new BitmapImage(new Uri(filename, UriKind.Relative));
            tlo.ImageSource = obrazek.Source;
            this.Background = tlo;
        }

        public string WynikiTrybuText(string tryb)
        {
            string wynik = "";
            int i = 1;
            foreach (var pozycja in bazaWynikow.getScores(tryb))
            {
                wynik = wynik + "\t" + i + ".\t" + pozycja.Value + " - " + pozycja.Key + " pkt.\n";
                i++;
            }
            return wynik;
        }

        public void WypiszWyniki(string tryb)
        {
            var filename = "../../Images/headers/";
            filename += tryb + ".png";

            textBlock.Text = WynikiTrybuText(tryb);

            ImageBrush napis = new ImageBrush();
            Image obrazek1 = new Image();
            obrazek1.Source = new BitmapImage(new Uri(filename, UriKind.Relative));
            napis.ImageSource = obrazek1.Source;
            napisGrid.Background = napis;
        }
    }
}

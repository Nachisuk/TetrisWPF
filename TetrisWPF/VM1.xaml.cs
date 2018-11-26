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
    /// Logika interakcji dla klasy VM1.xaml
    /// </summary>
    public partial class VM1 : Page
    {
        private BazaWynikow bazaWynikow;
        private List<String> lista;
        private int iloscOpcji;
        private int aktualna;
        private KeyEventHandler keyEventHandler;
        public VM1()
        {
            InitializeComponent();
            keyEventHandler = new KeyEventHandler(SterowanieMenu2);
            Application.Current.MainWindow.KeyDown += keyEventHandler;
            bazaWynikow = MainMenu.bazaWynikow; 
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

        public void SterowanieMenu2(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    Console.Out.WriteLine("LewyWcisniety");
                    aktualna -= 1;
                    if (aktualna < 0)
                    {
                        aktualna = iloscOpcji + aktualna;
                    }
                    WypiszWyniki(lista[aktualna]);
                    break;

                case Key.Right:
                    Console.Out.WriteLine("PrawyWcisniety");
                    aktualna += 1;
                    aktualna = aktualna%iloscOpcji;
                    WypiszWyniki(lista[aktualna]);
                    break;
                case Key.Escape:
                    Console.Out.WriteLine("EscWcisniety");
                    Application.Current.MainWindow.KeyDown -= keyEventHandler;
                    MainMenu main = new MainMenu();
                    App.Current.MainWindow.Close();
                    App.Current.MainWindow = main;
                    App.Current.MainWindow.Show();
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
            foreach (var pozycja in bazaWynikow.WynikiTrybowAktualne[tryb])
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

            textBlock.Text = WynikiTrybuText("maraton");
          
            ImageBrush napis = new ImageBrush();
            Image obrazek1 = new Image();
            obrazek1.Source = new BitmapImage(new Uri(filename, UriKind.Relative));
            napis.ImageSource = obrazek1.Source;
            napisGrid.Background = napis;
        }

    }
}

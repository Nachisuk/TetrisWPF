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
    public partial class MainMenu : Window
    {
        public static MainMenu Instance { get; private set; }
        public static List<MainMenuOptions> listaopcji;
        public static List<MainMenuOptions> listatrybow;
        public static bool czyTryby;
        public int i;
        public List<Label> labellist;
        public static BazaWynikow bazaWynikow;

        public MainMenu()
        {
            InitializeComponent();
            Instance = this;
            UstawTlo();

            listaopcji = MenuOptions.ZwrocOpcje();
            listatrybow = GameMenuOptions.ZwrocTryby();
            czyTryby = false;
            i = 1;

            labellist = new List<Label>();

            bazaWynikow = new BazaWynikow();
            bazaWynikow.InicjalizujBazeWynikow();
            
            labellist.Add(Opcja0);
            labellist.Add(Opcja1);
            labellist.Add(Opcja2);

            MainMenuOptionsInitialize();
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


        public void MainMenuOptionsInitialize()
        {
            
        }

        public void SterowanieMenu(object sender, KeyEventArgs e)
        {
            List<MainMenuOptions> lista;
            if (!czyTryby) lista = listaopcji;
            else lista = listatrybow;

            int liczbaOpcji = lista.Count;
            switch(e.Key)
            {
                case Key.Left:
                    for(int j=2;j >=0;j--)
                    {
                        int z = (i - j) % liczbaOpcji;

                        if(z== (-1))
                        {
                            labellist[j].Content = lista[liczbaOpcji - 1].zwrocNazwe();
                        }
                        else if(z ==(-2))
                        {
                            labellist[j].Content = lista[liczbaOpcji - 2].zwrocNazwe();
                            i = liczbaOpcji;
                        }
                        else
                        {
                            labellist[j].Content = lista[z].zwrocNazwe();
                        }                 
                    }
                    i = (i - 1) % liczbaOpcji;
                    break;

                case Key.Right:
                    for(int j=0; j<3;j++)
                    {
                        labellist[j].Content = lista[(i+j) % liczbaOpcji].zwrocNazwe();
                    }
                    i = (i + 1) % liczbaOpcji;
                    break;

                case Key.Enter:
                    lista[i].FunkcjaOpcji();
                    break;
                case Key.Escape:
                    czyTryby = false;
                    break;
            }
        }

        public static void GameModeStart(string gamemode)
        {
            GameBoard main = new GameBoard(gamemode);
            Instance.Content = main;
            App.Current.MainWindow.Height = 1000;
            App.Current.MainWindow.Width = 850;
        }

        public static void ScoreBoard()
        {

           // WypiszWyniki("maraton");

            List<String> lista = new List<String>();
            lista.Add("maraton");
            lista.Add("endless");
            lista.Add("ultra");
            lista.Add("landslide");
            lista.Add("haunted");
            int iloscOpcji = lista.Count;
            int i = 0;

            Instance.Content = new VM1();
        }



        public static void WypiszWyniki(string gamemode)
        {
           // Console.Clear();

           // Console.WriteAscii("        " + String.Format("" + gamemode[0].ToString().ToUpper() + gamemode.Substring(1)));
            int i = 1;
            foreach (var wynikZNazwami in bazaWynikow.WynikiTrybowAktualne[gamemode])
            {
               // Console.WriteLine("\t" + i + ".\t" + wynikZNazwami.Value + " - " + wynikZNazwami.Key + " pkt.");
               // i++;
            }
        }
    }
}

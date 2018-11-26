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
        public static List<MainMenuOptions> lista;
        public static bool czyTryby,czyZmienic;
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
            lista = listaopcji;
            czyTryby = false;
            i = 1;
            czyZmienic = false;
            labellist = new List<Label>();

            bazaWynikow = new BazaWynikow();
            bazaWynikow.InicjalizujBazeWynikow();
            
            labellist.Add(Opcja0);
            labellist.Add(Opcja1);
            labellist.Add(Opcja2);


            Opcja0.Content = lista[0].zwrocNazwe();
            Opcja1.Content = lista[1].zwrocNazwe();
            Opcja2.Content = lista[2].zwrocNazwe();
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

        public static void MainMenuSwitch()
        {
            czyTryby = true;
            lista = listatrybow;
            Instance.Opcja0.Content = lista[0].zwrocNazwe();
            Instance.Opcja1.Content = lista[1].zwrocNazwe();
            Instance.Opcja2.Content = lista[2].zwrocNazwe();
        }

        public void SterowanieMenu(object sender, KeyEventArgs e)
        {
            if (czyTryby)
            {
                lista = listatrybow;
                if(czyZmienic)
                {
                    czyZmienic = false;
                    Opcja0.Content = lista[0].zwrocNazwe();
                    Opcja1.Content = lista[1].zwrocNazwe();
                    Opcja2.Content = lista[2].zwrocNazwe();
                }
            }
            else
            {
                lista = listaopcji;
                if(!czyZmienic)
                {
                    czyZmienic = true;
                    Opcja0.Content = lista[0].zwrocNazwe();
                    Opcja1.Content = lista[1].zwrocNazwe();
                    Opcja2.Content = lista[2].zwrocNazwe();
                }
            }

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

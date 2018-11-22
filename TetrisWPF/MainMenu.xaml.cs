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
        public static List<MainMenuOptions> listaopcji;
        public static List<MainMenuOptions> listatrybow;
        public static bool czyTryby;
        public int i;
        public List<Label> labellist;

        public MainMenu()
        {
            InitializeComponent();
            listaopcji = MenuOptions.ZwrocOpcje();
            listatrybow = GameMenuOptions.ZwrocTryby();
            czyTryby = false;
            i = 1;

            labellist = new List<Label>();

            labellist.Add(Opcja0);
            labellist.Add(Opcja1);
            labellist.Add(Opcja2);

            MainMenuOptionsInitialize();
        }

        public void MainMenuOptionsInitialize()
        {
            Opcja0.Content = "Elson 1";
            Opcja1.Content = "Elson 2";
            Opcja2.Content = "Elson 3";
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
            MainWindow main = new MainWindow(gamemode);
            App.Current.MainWindow.Close();
            App.Current.MainWindow = main;            
            main.Show();
        }

        public static void ScoreBoard()
        {

        }
    }
}

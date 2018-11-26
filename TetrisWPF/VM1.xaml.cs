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

namespace TetrisWPF
{
    /// <summary>
    /// Logika interakcji dla klasy VM1.xaml
    /// </summary>
    public partial class VM1 : Page
    {
        public VM1()
        {
            InitializeComponent();
            UstawTlo();
            WypiszWyniki("maraton");
            //GameBoard.bazaWynikow.WynikiTrybowAktualne
            /*
             WypiszWyniki("maraton");
            
            List<String> lista = new List<String>();
            lista.Add("maraton");
            lista.Add("endless");
            lista.Add("ultra");
            lista.Add("landslide");
            lista.Add("haunted");
            int iloscOpcji = lista.Count;
            int i = 0;
            while (true)
            {
                ConsoleKey key = ConsoleKey.B;
                if (Console.KeyAvailable)
                {
                    key = Console.ReadKey(true).Key;
                }
                switch (key)
                {
                    case ConsoleKey.Escape:
                        MainMenu(MenuOptions.ZwrocOpcje());
                        break;
                    case ConsoleKey.LeftArrow:
                        if (i == 0) i = iloscOpcji - 1;
                        else i = i - 1;
                        WypiszWyniki(lista[i]);
                        break;
                    case ConsoleKey.RightArrow:
                        if (i == iloscOpcji - 1) i = 0;
                        else i = i + 1;
                        WypiszWyniki(lista[i]);
                        break;

                }
            }*/

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

        public void WypiszWyniki(string tryb)
        {

        }

    }
}

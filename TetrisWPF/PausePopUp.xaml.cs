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

namespace TetrisWPF
{
    /// <summary>
    /// Logika interakcji dla klasy PausePopUp.xaml
    /// </summary>
    public partial class PausePopUp : Window
    {
        public PausePopUp()
        {
            InitializeComponent();
            UstawTlo();
        }

        public void UstawTlo()
        {
            ImageBrush tlo = new ImageBrush();
            Image obrazek = new Image();
            var filename = "../../Images/dark-background_PopUp.jpg";
            obrazek.Source = new BitmapImage(new Uri(filename, UriKind.Relative));
            tlo.ImageSource = obrazek.Source;
            this.Background = tlo;

            ImageBrush napis = new ImageBrush();
            Image obrazek1 = new Image();
            var filename1 = "../../Images/pauseMenuText.png";
            obrazek1.Source = new BitmapImage(new Uri(filename1, UriKind.Relative));
            napis.ImageSource = obrazek1.Source;
            NapisTytulowy.Background = napis;

            ImageBrush podpis = new ImageBrush();
            Image obrazek2 = new Image();
            var filename2 = "../../Images/pauseMenuSubtextV2.png";
            obrazek2.Source = new BitmapImage(new Uri(filename2, UriKind.Relative));
            podpis.ImageSource = obrazek2.Source;
            PodpisTytulowy.Background = podpis;
        }

        public void Sterowanie(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.P:
                    this.Close();
                    break;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

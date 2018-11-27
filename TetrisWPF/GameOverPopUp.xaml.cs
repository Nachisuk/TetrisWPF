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
    /// Logika interakcji dla klasy GameOverPopUp.xaml
    /// </summary>
    public partial class GameOverPopUp : Window
    {
        private bool chosenOptionPodsumowanie;
        public GameOverPopUp()
        {
            InitializeComponent();
            chosenOptionPodsumowanie = true;
            UstawTlo();
            restartBtn.Focus();
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
            var filename1 = "../../Images/gameOverPopUpTextV2.png";
            obrazek1.Source = new BitmapImage(new Uri(filename1, UriKind.Relative));
            napis.ImageSource = obrazek1.Source;
            NapisTytulowy.Background = napis;
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            chosenOptionPodsumowanie = false;
            this.Close();
        }

        private void PodsumowanieButton_Click(object sender, RoutedEventArgs e)
        {
            chosenOptionPodsumowanie = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (chosenOptionPodsumowanie)
                this.DialogResult = true;
            else
                this.DialogResult = false;
        }
    }
}
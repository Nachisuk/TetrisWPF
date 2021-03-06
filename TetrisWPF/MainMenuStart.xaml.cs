﻿using System;
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
    /// Logika interakcji dla klasy MainMenuStart.xaml
    /// </summary>
    public partial class MainMenuStart : Window
    {
        public MainMenuStart()
        {
            InitializeComponent();
            App.Current.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            UstawTlo();
        }

        public void UstawTlo()
        {
            ImageBrush tlo = new ImageBrush();
            Image obrazek = new Image();
            var filename = "../../Images/tetrisMainMenu_chilled.jpg";
            //var filename = "../../Images/tetrisMainMenu.jpg";
            obrazek.Source = new BitmapImage(new Uri(filename,UriKind.Relative));
            tlo.ImageSource = obrazek.Source;
            TetrisStart.Background = tlo;

            ImageBrush napis = new ImageBrush();
            Image obrazek1 = new Image();
            var filename1 = "../../Images/mainmenutextV2.png";
            obrazek1.Source = new BitmapImage(new Uri(filename1, UriKind.Relative));
            napis.ImageSource = obrazek1.Source;
            NapisTytulowy.Background = napis;

            ImageBrush podpis = new ImageBrush();
            Image obrazek2 = new Image();
            var filename2 = "../../Images/mainmenusubtextV2.png";
            obrazek2.Source = new BitmapImage(new Uri(filename2, UriKind.Relative));
            podpis.ImageSource = obrazek2.Source;
            PodpisTytulowy.Background = podpis;
        }

        public void Przejdz(object sender, KeyEventArgs e)
        {
            if(e.Key != Key.None)
            {
                MainMenu main = new MainMenu();
                App.Current.MainWindow = main;
                this.Close();
                main.Show();
            }
        }
    }
}

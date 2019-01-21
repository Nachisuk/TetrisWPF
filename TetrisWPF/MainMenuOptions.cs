using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisWPF.Properties;

namespace TetrisWPF
{
    public interface MainMenuOptions
    {
        void FunkcjaOpcji();
        string zwrocNazwe();
        string zwrocOpis();
        
    }

    public class MainMenu_ClassicTet : MainMenuOptions
    {
        
        public string nazwa = "      Graj       ";
        public void FunkcjaOpcji()
        {
            //redirect to subMenu
        }

        public string zwrocNazwe()
        {
            return this.nazwa;
        }
        public string zwrocOpis() { return ""; }
    }

    public class MainMenu_Scoreboard : MainMenuOptions
    {
        public string nazwa = " Tablice wyników ";
        public MainMenu mainMenu;

        public MainMenu_Scoreboard()
        {

        }

        public MainMenu_Scoreboard (MainMenu mainMenu)
        {
            this.mainMenu = mainMenu;
        }

        public void FunkcjaOpcji()
        {
            mainMenu.ScoreBoard();
        }

        public string zwrocNazwe()
        {
            return this.nazwa;
        }
        public string zwrocOpis() { return ""; }
    }

    public class MainMenu_ExitGame : MainMenuOptions
    {
        public string nazwa = "   Wyjdz z gry   ";
        public void FunkcjaOpcji()
        {
            Environment.Exit(0);
        }

        public string zwrocNazwe()
        {
            return this.nazwa;
        }
        public string zwrocOpis() { return ""; }
    }

    public class GameMode_Marathon : MainMenuOptions
    {
        public string nazwa = "  Maraton ";
        public void FunkcjaOpcji()
        {
            MainMenu.GameModeStart(nazwa);
        }

        public string zwrocNazwe()
        {
            return this.nazwa;
        }
        public string zwrocOpis() { return "Graj do 15 poziomu i zdobądź jak najwięcej punktów"; }
    }
    public class GameMode_Endless : MainMenuOptions
    {
        public string nazwa = "  Endless ";
        public void FunkcjaOpcji()
        {
            MainMenu.GameModeStart(nazwa);
        }

        public string zwrocNazwe()
        {
            return this.nazwa;
        }
        public string zwrocOpis() { return "        Graj bez ograniczeń"; }
    }

    public class GameMode_Ultra : MainMenuOptions
    {
        public string nazwa = "   Ultra  ";
        public void FunkcjaOpcji()
        {
            MainMenu.GameModeStart(nazwa);
        }

        public string zwrocNazwe()
        {
            return this.nazwa;
        }
        public string zwrocOpis() { return "Zdobądź jak najwięcej punktów w 3 minuty"; }
    }

    public class GameMode_LandSlide : MainMenuOptions
    {
        public string nazwa = " LandSlide ";
        public void FunkcjaOpcji()
        {
            MainMenu.GameModeStart(nazwa);
        }

        public string zwrocNazwe()
        {
            return this.nazwa;
        }
        public string zwrocOpis() { return "         Uważaj na osuwiska"; }
    }

    public class GameMode_Haunted : MainMenuOptions
    {
        public string nazwa = "  Haunted  ";
        public void FunkcjaOpcji()
        {
            MainMenu.GameModeStart(nazwa);
        }

        public string zwrocNazwe()
        {
            return this.nazwa;
        }

        public string zwrocOpis() { return "         Przetestuj swoją pamięć"; }
    }

    
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisWPF
{
    class DataOperator
    {
        private static DataOperator instance = null;
        private static readonly object padlock = new object();

        private String filename = "bazaWynikow.txt";
        private Dictionary<String, SortedDictionary<int, String>> scores;


        private DataOperator()
        {

            InicjalizujBazeWynikow();
        }

        public static DataOperator getInstance()
        {
            if (instance==null)
            {
                lock (padlock)
                {
                    if(instance == null)
                    {
                        instance = new DataOperator();
                    }
                }
            }
            return instance;
        }

        class MojComparer : IComparer<int> //komperer do sortowania slownika kluczy od najwiekszego do najmniejszego, nie odwrotnie, jak jest domyslnie
        {
            public int Compare(int x, int y)
            {
                return -(x.CompareTo(y));
            }
        }

        public Dictionary<String, SortedDictionary<int, String>> getScores()
        {
            return scores;
        }

        public SortedDictionary<int, String> getScores(String mode)
        {
            return scores[mode];
        }

        public void InicjalizujBazeWynikow()
        {
            if (File.Exists(filename))
            {
                PobierzPlik();
            }
            else
            {
                GenerujPlikZero();
            }
        }

        private void GenerujPlikZero()
        {
            scores = new Dictionary<String, SortedDictionary<int, String>>();
            //WynikiTrybowStartowe = new Dictionary<String, SortedDictionary<int, String>>();

            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (var mode in new String[] { "maraton", "endless", "ultra", "landslide", "haunted" })
                {
                    int tmpWynik = 50;
                    String tmpNazwaAnonima = "Anonim";

                    sw.WriteLine(mode);
                    scores.Add(mode, new SortedDictionary<int, String>(new MojComparer()));
                    //WynikiTrybowStartowe.Add(mode, new SortedDictionary<int, String>(new MojComparer()));


                    for (int x = tmpWynik; x <= tmpWynik * (Math.Pow(2, 9)); x = x * 2)  //może być i i=0 poki i<10 przy i++
                    {
                        sw.WriteLine(x + " " + tmpNazwaAnonima);
                        scores[mode].Add(x, tmpNazwaAnonima);
                        //WynikiTrybowStartowe[mode].Add(x, tmpNazwaAnonima);
                    }

                    sw.WriteLine(";;");
                }
            }

        }

        private void PobierzPlik()
        {
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    scores = new Dictionary<String, SortedDictionary<int, String>>();
                    //WynikiTrybowStartowe = new Dictionary<String, SortedDictionary<int, String>>();

                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        scores.Add(line, new SortedDictionary<int, String>(new MojComparer()));
                        //WynikiTrybowStartowe.Add(line, new SortedDictionary<int, String>(new MojComparer()));
                        String mode = line;

                        while ((line = sr.ReadLine()) != ";;")
                        {
                            String[] lineTab = line.Split(' ');
                            int tmpWynikUsera;
                            int.TryParse(lineTab[0], out tmpWynikUsera);

                            scores[mode].Add(tmpWynikUsera, lineTab[1]);
                            //WynikiTrybowStartowe[mode].Add(tmpWynikUsera, lineTab[1]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                GenerujPlikZero();
                return;
            }
        }

        

        public void TryZapisacDanyWynik(int iloscPunktow, string gamemode, string nazwaUzytkownika)
        {

            // String nazwaUzytkownika = "TestowyUkonczony";
            String kluczOfMode;

            //dla pewnosci
            switch (gamemode)
            {
                case "  Maraton ":
                    kluczOfMode = "maraton";
                    break;
                case "  Endless ":
                    kluczOfMode = "endless";
                    break;
                case "   Ultra  ":
                    kluczOfMode = "ultra";
                    break;
                case " LandSlide ":
                    kluczOfMode = "landslide";
                    break;
                case "  Haunted  ":
                    kluczOfMode = "haunted";
                    break;
                default:
                    kluczOfMode = gamemode.Trim(' ').ToLower();
                    break;
            }



            if (scores[kluczOfMode].ContainsKey(iloscPunktow)) //jezeli byl juz taki wynik to nadpisz go nowa nazwa uzytkownika(bo siwierzszy gracz)
            {
                scores[kluczOfMode][iloscPunktow] = nazwaUzytkownika;
                ZapiszDoPlikuAktualne(); // TODO: do zmiany jesli bedziemy chcieli po wyłączeniu okna dopiero zapisywać, czy po jakimś przycisku zapisz
            }
            else if (!(scores[kluczOfMode].Last().Key > iloscPunktow)) //jezeli nasz nowy wynik nie jest mniejszy niz najmniejszy
            {
                scores[kluczOfMode].Add(iloscPunktow, nazwaUzytkownika); //dodaje nowy
                scores[kluczOfMode].Remove(scores[kluczOfMode].Last().Key); //usuwa najmniejszy (czyt pierwszy w kolejnosci)
                ZapiszDoPlikuAktualne(); // TODO: do zmiany jesli bedziemy chcieli po wyłączeniu okna dopiero zapisywać, czy po jakimś przycisku zapisz
            }
            //else ignoruj - nie poczeba zmian

        }

        public void ZapiszDoPlikuAktualne() //plus aktualizowanie WynikiTrybowStartowego
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                //WynikiTrybowStartowe = new Dictionary<string, SortedDictionary<int, string>>();

                foreach (var mode in scores.Keys)
                {
                    sw.WriteLine(mode);
                    //WynikiTrybowStartowe.Add(mode, new SortedDictionary<int, string>(new MojComparer()));
                    foreach (var wynik in scores[mode])
                    {
                        sw.WriteLine(wynik.Key + " " + wynik.Value);
                        //WynikiTrybowStartowe[mode].Add(wynik.Key, wynik.Value);
                    }

                    sw.WriteLine(";;");
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace projekt2
{
    public class Transakcja
    {
        public static int wartoscTransakcji;
        public static int liczbaSprzedanych;
        public static int liczbaTransakcji;

        // przechowuje adres ścieżki bazy danych Produkty.txt
        public static string sciezka;

        // przechowuje wybrany nr handlowca/kod produktu
        public static int id = 1;

        // potrzebne do wyświetlania odpowiedniego wykresu i wyglądu formatki
        public static int id_wejscia;

        // można ustawić zakres z jakiego losować ilość transakcji
        public static int ilosc_transakcji_min = 100;
        public static int ilosc_transakcji_max = 1000;

        // przechowuje informacje czy zaczytano bazę do programu
        public static bool loaded = false;

        public int KodHandlowca { get; set; }
        public int NumerMiesiaca { get; set; }
        public int KodProduktu { get; set; }
        public int LiczbaSprzedanych { get; set; }
        public int WartoscSprzedanych { get; set; }
        public int KodWojewodztwa { get; set; }

        static Transakcja()
        {
            wartoscTransakcji = 0;
            liczbaSprzedanych = 0;
            liczbaTransakcji = 0;
        }
    }
}

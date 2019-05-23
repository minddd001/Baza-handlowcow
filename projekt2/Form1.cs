using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;


namespace projekt2
{
    public partial class Form1 : Form
    {
        Random rnd = new Random(); // losowanie
        private DataTable dane_tab = null; // tworzymy puste DataTable, które posłuży do wypełnienia datagridview
        public List<Transakcja> Transakcje; // tworzymy nową listę gdzie będziemy przechowywać wszystkie transakcje

        // metoda, który służy do wypełnienia pliku Produkty.txt losowymi danymi
        private void Losuj_baze()
        {
                  //Transakcje.Clear(); // czyści listę
            try
                {
                    // pobiera adres lokalizacji pliku bazy danych Produkty.txt na komputerze i wyświetla w programie
                    Transakcja.sciezka = @"Produkty.txt";
                    Transakcja.sciezka = Path.GetFullPath(Transakcja.sciezka); 
                    Directory.CreateDirectory(Path.GetDirectoryName(Transakcja.sciezka));
                    textBox1.Text = Transakcja.sciezka;
                    File.Delete(Transakcja.sciezka); // zanim wylosujemy nową bazę trzeba usunąć Produkty.txt (o ile istnieje), jeżeli tego nie zrobimy w rzadkim przypadku może wystąpić problem z ilością transakcji w Produkty.txt

                    // wypełniamy bazę danych Produkty.txt wylosowanymi danymi
                    using (StreamWriter writer = new StreamWriter(Transakcja.sciezka, true))
                            {
                                string temp = "";
                                for (int i = 1; i <= (new Random().Next(Transakcja.ilosc_transakcji_min, Transakcja.ilosc_transakcji_max)); i++)
                                {
                                    temp = Convert.ToString(rnd.Next(1, 9)) + "," + Convert.ToString(rnd.Next(1, 13)) + "," + Convert.ToString(rnd.Next(1, 51)) + "," + Convert.ToString(rnd.Next(1, 101)) + "," + Convert.ToString(rnd.Next(1000, 10001)) + "," + Convert.ToString(rnd.Next(1, 17));
                                    writer.WriteLine(temp);

                                }
                                Transakcja.loaded = true; // zapisujemy informację o tym, że pomyślnie załadowano/stworzono bazę
                }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Convert.ToString(ex));
                }
                
                Zaladuj_transakcje(); // automatycznie wywołujemy metodę, która wczytuje dane
        }

        // metoda tworzy obiekty klasy Transakcja i zapisuje je w liście
        private void Zaladuj_transakcje()
        {
            Transakcje.Clear(); // czyścimy listę z transakcji, żeby nie dublować transakcji (jeżeli np. losujemy bazę kilka razy)
            Transakcja.wartoscTransakcji = 0;
            Transakcja.liczbaSprzedanych = 0;
            Transakcja.liczbaTransakcji = 0;
            try
            {
                    // odczytujemy bazę danych linia po linii
                    List<string> linie = File.ReadAllLines(Transakcja.sciezka).ToList();
                    foreach (var linia in linie)
                    {
                        string[] dane = linia.Split(','); // poszczególne dane oddzielone są przecinkami, program musi to wiedzieć
                        Transakcja nowyHandl = new Transakcja // tworzymy nową transakcję
                        {
                            KodHandlowca = Convert.ToInt32(dane[0]),
                            NumerMiesiaca = Convert.ToInt32(dane[1]),
                            KodProduktu = Convert.ToInt32(dane[2]),
                            LiczbaSprzedanych = Convert.ToInt32(dane[3]),
                            WartoscSprzedanych = Convert.ToInt32(dane[4]),
                            KodWojewodztwa = Convert.ToInt32(dane[5])
                        };

                        // zliczamy ilość transakcji, ilość sprzedanych produktów itd.
                        Transakcja.wartoscTransakcji += nowyHandl.WartoscSprzedanych;
                        Transakcja.liczbaSprzedanych += nowyHandl.LiczbaSprzedanych;
                        Transakcja.liczbaTransakcji++;
                        Transakcje.Add(nowyHandl);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }
            Wypelnij_tabele(); // przechodzimy do wypełniania tabeli
        }

        // metoda wypełnia datagridview danymi z listy
        private void Wypelnij_tabele()
        {
            if (Transakcje != null) // jeżeli nasza lista z transakcjami jest pusta to nie wypełnimy datagridview
            {
                dane_tab = new DataTable("Baza danych");
                DataColumn Kod_h = new DataColumn("Kod handlowca", typeof(int));
                DataColumn Mc = new DataColumn("Miesiąc",typeof(int));
                DataColumn Kod_p = new DataColumn("Kod produktu", typeof(int));
                DataColumn Ls = new DataColumn("Liczba sprzedanych", typeof(int));
                DataColumn Ws = new DataColumn("Wartość transakcji", typeof(int));
                DataColumn Kw = new DataColumn("Kod województwa", typeof(int));

                dane_tab.Columns.Add(Kod_h);
                dane_tab.Columns.Add(Mc);
                dane_tab.Columns.Add(Kod_p);
                dane_tab.Columns.Add(Ls);
                dane_tab.Columns.Add(Ws);
                dane_tab.Columns.Add(Kw);

                foreach (Transakcja h in Transakcje)
                {
                    DataRow wiersz;
                    wiersz = dane_tab.NewRow();
                    wiersz["Kod handlowca"] = h.KodHandlowca;
                    wiersz["Miesiąc"] = h.NumerMiesiaca;
                    wiersz["Kod produktu"] = h.KodProduktu;
                    wiersz["Liczba sprzedanych"] = h.LiczbaSprzedanych;
                    wiersz["Wartość transakcji"] = h.WartoscSprzedanych;
                    wiersz["Kod województwa"] = h.KodWojewodztwa;
                    dane_tab.Rows.Add(wiersz);

                }
                dataGridView1.DataSource = dane_tab;

                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                label2.Text = Convert.ToString(Transakcja.liczbaTransakcji);
                label4.Text = Convert.ToString(Transakcja.wartoscTransakcji + " zł");
                label6.Text = Convert.ToString(Transakcja.liczbaSprzedanych);
            }
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Transakcje = new List<Transakcja>();
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 d = new Form2();
            d.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Losuj_baze();
        }

        private void raportyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 d = new Form2();
            d.ShowDialog();
        }

        private void zakończToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void otwórzBazęToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.button1.PerformClick();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.SafeFileName == "Produkty.txt") // akceptujemy tylko plik o nazwie Produkty.txt
                {
                    // pobieramy ścieżkę pliku bazy danych, zapisujemy ją w zmiennej
                    textBox1.Text = ofd.FileName;
                    Transakcja.sciezka = ofd.FileName;
                    Transakcja.loaded = true;
                    Zaladuj_transakcje();
                }
                else
                {
                    MessageBox.Show("Wybrano zły plik. Wybierz plik Produkty.txt", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.button1.PerformClick(); // klikamy button1, żeby kontynuować wybieranie pliku
                }
            }
                
            
        }

        private void oProgramieToolStripMenuItem_Click(object sender, EventArgs e)
        {
                MessageBox.Show("Jakub Piotrowski");
        }
    }
}
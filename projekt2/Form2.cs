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
using System.Diagnostics;

namespace projekt2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // wypełniamy combobox'y liczbami
            for(int i = 1; i <= 50; i++) comboBox1.Items.Add(i);
            for(int i = 1; i <= 8; i++) comboBox2.Items.Add(i);
            for(int i = 1; i <= 8; i++) comboBox3.Items.Add(i);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Wybierz kod produktu!");
                return; // jeżeli nie wybrano kodu produktu to nie wykonujemy dalszych instrukcji
            }
            if (Transakcja.loaded == false) MessageBox.Show("Nie znaleziono pliku bazy Produkty.txt", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            if (Transakcja.loaded == true)
            {
                Transakcja.id = Convert.ToInt32(comboBox1.Text);
                Transakcja.id_wejscia = 1;
                Form3 d = new Form3();
                d.ShowDialog(); // otwiera formatkę Form3
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("Wybierz kod handlowca!");
                return; // // jeżeli nie wybrano kodu handlowca to nie wykonujemy dalszych instrukcji
            }
            // jeżeli nie załadowaliśmy bazy to nie przechodzimy dalej
            if (Transakcja.loaded == false) MessageBox.Show("Nie znaleziono pliku bazy Produkty.txt", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            if (Transakcja.loaded == true)
            {
                Transakcja.id = Convert.ToInt32(comboBox2.Text);
                try
                {
                    // zdaję sobie sprawę, że można użyć listy z danymi utworzonej w metodzie Zaladuj_transakcje() w Form1 ale postanowiłem znowu odczytywać
                    // plik bazy danych i od nowa zapisać listę
                    List<Transakcja> Transakcje = new List<Transakcja>();
                    List<string> linie = File.ReadAllLines(Transakcja.sciezka).ToList();
                    try
                    {
                        using (StreamWriter writer = new StreamWriter("Handlowiec.txt"))
                        {
                            string temp = "";
                            writer.WriteLine("Handlowiec nr " + Transakcja.id);
                            foreach (var linia in linie) // odczytujemy linię po linii
                            {
                                string[] dane = linia.Split(',');
                                if (Transakcja.id == Convert.ToInt32(dane[0])) // czytamy tylko te linie które dotyczą handlowca/produktu, które ID wybraliśmy
                                {
                                    // poniższe if, else if i else istnieją po to, żeby tekst w Handlowiec.txt był ładnie wyrównany tzn. żeby kolumny się nie rozjeżdzały
                                    if (Convert.ToInt32(dane[2]) < 10) temp = "Miesiąc: " + Convert.ToString(dane[1]) + (char)9 + "Kod produktu: " + Convert.ToString(dane[2]) + (char)9 + (char)9 + "Liczba sprzedanych: " + Convert.ToString(dane[3]) + (char)9 + (char)9 + "Wartość sprzedanych: " + Convert.ToString(dane[4]) + (char)9 + "Kod województwa: " + Convert.ToString(dane[5]) + (char)13;
                                    else if (Convert.ToInt32(dane[4]) < 100) temp = "Miesiąc: " + Convert.ToString(dane[1]) + (char)9 + "Kod produktu: " + Convert.ToString(dane[2]) + (char)9 + "Liczba sprzedanych: " + Convert.ToString(dane[3]) + (char)9 + (char)9 + "Wartość sprzedanych: " + Convert.ToString(dane[4]) + (char)9 + (char)9 + "Kod województwa: " + Convert.ToString(dane[5]) + (char)13;
                                    else temp = "Miesiąc: " + Convert.ToString(dane[1]) + (char)9 + "Kod produktu: " + Convert.ToString(dane[2]) + (char)9 + "Liczba sprzedanych: " + Convert.ToString(dane[3]) + (char)9 + (char)9 + "Wartość sprzedanych: " + Convert.ToString(dane[4]) + (char)9 + "Kod województwa: " + Convert.ToString(dane[5]) + (char)13;
                                    writer.WriteLine(temp); // zapisujemy nową linię w raporcie
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(Convert.ToString(ex));
                    }
                    Process.Start("notepad.exe", "Handlowiec.txt"); // uruchamiamy notatnik z nowo utworzonym plikiem
                    Transakcje.Clear();
                }
                catch (Exception)
                {
                    MessageBox.Show("Nie znaleziono pliku bazy Produkty.txt", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
 
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text == "")
            {
                MessageBox.Show("Wybierz kod handlowca!");
                return;
            }
            if (Transakcja.loaded == false) MessageBox.Show("Nie znaleziono pliku bazy Produkty.txt", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            if (Transakcja.loaded == true)
            {
                Transakcja.id = Convert.ToInt32(comboBox3.Text);
                Transakcja.id_wejscia = 2;
                Form3 d = new Form3();
                d.ShowDialog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

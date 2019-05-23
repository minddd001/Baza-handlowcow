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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
            switch (Transakcja.id_wejscia) // switch jest potrzebny, żeby wiedzieć jak sformatować wykres i które dane odczytać z pliku
            {
                case 1:
                    {
                        columnHeader1.Width = 130;
                        columnHeader2.Width = 130;
                        button2.Visible = false;
                        label4.Text = "Raport ze sprzedaży produktu o kodzie " + Transakcja.id;
                        try
                        {
                            List<Transakcja> Transakcje = new List<Transakcja>();
                            List<string> linie = File.ReadAllLines(Transakcja.sciezka).ToList();
                            int[] tab = new int[16];  // dane z pliku zapisujemy w tabeli
                            foreach (var linia in linie)
                            {
                                string[] dane = linia.Split(',');
                                if (Transakcja.id == Convert.ToInt32(dane[2]))
                                {
                                    int i = Convert.ToInt32(dane[5]) - 1;
                                    tab[i] = tab[i] + Convert.ToInt32(dane[4]);
                                }
                            }
                            for (int i = 0; i <= 15; i++)
                            {
                                // dodajemy dane do ListView korzystając z danych z tabeli
                                ListViewItem a = new ListViewItem(Convert.ToString(i + 1));
                                a.SubItems.Add(Convert.ToString(tab[i]) + " zł");
                                listView1.Items.Add(a);
                            }

                            chart1.Series["Wartość sprzedaży"].Points.Clear();

                            for (int i = 0; i <= 15; i++)
                            {
                                chart1.Series["Wartość sprzedaży"].Points.AddXY(i + 1, tab[i]); // dodajemy dane do wykresu korzystając z danych z tabeli
                            }
                            Transakcje.Clear();
                        }
                        catch(Exception)
                        {
                            MessageBox.Show("Nie znaleziono pliku bazy Produkty.txt", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        break;
                    }
                case 2:
                    {
                        // modyfikujemy nazwy kolumn w listview
                        listView1.Columns.Remove(columnHeader1);
                        listView1.Columns.Remove(columnHeader2);
                        
                        listView1.Columns.Add(columnHeader1);
                        columnHeader1.Text = "Nr miesiąca";

                        listView1.Columns.Add(columnHeader2);
                        columnHeader2.Text = "Sum. wart. sprzedaży";
                        
                        columnHeader1.Width = 130;
                        columnHeader2.Width = 130;

                        listView1.Height = 232;
                        label4.Text = "Raport sum. wart. sprzedaży dla handlowca nr " + Transakcja.id;
                        button2.Visible = true;
                        int[] T = new int[12]; // dane z pliku zapisujemy w tabeli
                        try
                        {
                            List<Transakcja> Transakcje = new List<Transakcja>();
                            List<string> linie = File.ReadAllLines(Transakcja.sciezka).ToList();
                            foreach (var linia in linie)
                            {
                                string[] dane = linia.Split(',');
                                if (Transakcja.id == Convert.ToInt32(dane[0]))
                                {
                                    T[Convert.ToInt32(dane[1]) - 1] += Convert.ToInt32(dane[4]);
                                }
                            }
                            try
                            {
                                // tworzymy raport Wyroby.txt i wykorzystujemy do niego dane z tabeli
                                using (StreamWriter writer = new StreamWriter("Wyroby.txt"))
                                {
                                    writer.WriteLine("Handlowiec nr " + Transakcja.id);
                                    writer.WriteLine("Miesięczna wartość sprzedaży:");
                                    for (int i = 0; i <= 11; i++)
                                    {
                                        int j = i + 1;
                                        writer.WriteLine("Miesiąc nr " + j + ": " + T[i] + " zł");
                                    }
                                }

                                for (int i = 0; i <= 11; i++)
                                {
                                    ListViewItem a = new ListViewItem(Convert.ToString(i + 1));
                                    a.SubItems.Add(Convert.ToString(T[i]) + " zł");
                                    listView1.Items.Add(a);
                                }
                                chart1.Series["Wartość sprzedaży"].Points.Clear();

                                for (int i = 0; i <= 11; i++)
                                {
                                    chart1.Series["Wartość sprzedaży"].Points.AddXY(i + 1, T[i]);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(Convert.ToString(ex));
                            }
                            Transakcje.Clear();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Nie znaleziono pliku bazy Produkty.txt", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        break;
                    }
                default:
                {
                    break;
                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // zmieniamy wygląd wykresu na tryb 3D
            if (checkBox1.Checked == true) chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            else chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "Wyroby.txt"); // otwieramy notatnik z raportem Wyroby.txt
        }
    }
}

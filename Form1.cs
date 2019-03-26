using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Vjezba_24_insertovanje_podataka
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static String employeeID;
        public static String konekcioniString = "Server=localhost; Port=3306; " +
            "Database=hr; Uid=root; Pwd=zehe";

        private void buttonPrijava_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            String korisnickoIme = textBoxKorisnickoIme.Text;
            String sifra = textBoxSifra.Text;

            String query = "SELECT passwd, CONCAT(first_name, ' ', last_name), " +
                " employee_id, login_status FROM employees WHERE username ='" + korisnickoIme + "' ";

            try
            {
                MySqlConnection konekcija = new MySqlConnection(konekcioniString);

                konekcija.Open();

                MySqlCommand cmd = new MySqlCommand(query, konekcija);

                MySqlDataReader reader;
                reader = cmd.ExecuteReader();
                reader.Read();

                if (!reader.HasRows)
                {
                    errorProvider1.SetError(textBoxKorisnickoIme, "Pogrešno korisničko ime !!!");
                }
                else
                {
                    String passwd = reader[0].ToString();
                    String imePrez = reader[1].ToString();
                    employeeID = reader[2].ToString();
                    String loginStatus = reader[3].ToString();

                    if (loginStatus == "1")
                    {
                        errorProvider1.SetError(buttonPrijava, "Korisnik je već logovan!!!");
                    }
                    else if (sifra == passwd)
                    {
                        MessageBox.Show("Uspješno ste logovani " + imePrez);
                        PostaviStatusLogin();
                        Form2 fr2 = new Form2();
                        this.Hide();
                        fr2.Show();
                    }
                    else
                    {
                        errorProvider1.SetError(textBoxSifra, "Pogrešan password !!!");
                    }
                }

                reader.Close();
                konekcija.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PostaviStatusLogin()
        {
            String upit = "UPDATE employees SET login_status=1 " +
                    " WHERE employee_id='" + employeeID + "' ";

            try
            {
                MySqlConnection konekcija = new MySqlConnection(konekcioniString);
                konekcija.Open();

                MySqlCommand cmd = new MySqlCommand(upit, konekcija);

                // Funkcija ExecuteNonQuery() se koristi kada ne očekujemo nikakve 
                // podatke da nam budu vraćeni na osnovu proslijeđenog upita bazi. 
                // Ona se koristi kada izvršavamo INSERT, UPDATE, DELETE upite.
                cmd.ExecuteNonQuery();

                konekcija.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void PostaviStatusLogout()
        {
            String upit = "UPDATE employees SET login_status=0 " +
                    " WHERE employee_id='" + employeeID + "' ";

            try
            {
                MySqlConnection konekcija = new MySqlConnection(konekcioniString);
                konekcija.Open();

                MySqlCommand cmd = new MySqlCommand(upit, konekcija);

                cmd.ExecuteNonQuery();
                konekcija.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}

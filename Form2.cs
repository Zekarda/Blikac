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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            PrikazPodataka();
            IspuniComboBox();
        }

        String empID = Form1.employeeID;
        String konekStr = Form1.konekcioniString;

        private void buttonOdjava_Click(object sender, EventArgs e)
        {
            this.Hide();

            Form1 fr1 = new Form1();
            fr1.PostaviStatusLogout();
            fr1.Show();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 fr1 = new Form1();
            fr1.PostaviStatusLogout();

            Application.Exit();
        }

        private void buttonIzmjeni_Click(object sender, EventArgs e)
        {
            AzuriranjePodataka();
        }

        private void buttonPrikazi_Click(object sender, EventArgs e)
        {
            PrikazPodataka();
        }

        private void buttonDodaj_Click(object sender, EventArgs e)
        {
            DodavanjePodataka();
        }

        private void PrikazPodataka()
        {
            String upit = "SELECT first_name, last_name, email, phone_number, salary " +
                " FROM employees WHERE employee_id ='" + empID + "' ";

            try
            {
                MySqlConnection konekcija = new MySqlConnection(konekStr);
                konekcija.Open();

                MySqlCommand cmd = new MySqlCommand(upit, konekcija);

                MySqlDataReader reader;
                reader = cmd.ExecuteReader();
                reader.Read();

                String ime = reader[0].ToString();
                String prezime = reader[1].ToString();
                String mail = reader[2].ToString();
                String telefon = reader[3].ToString();
                String plata = reader[4].ToString();

                textBoxID.Text = empID;
                textBoxID.Text = empID;
                textBoxIme.Text = ime;
                textBoxPrezime.Text = prezime;
                textBoxMail.Text = mail;
                textBoxTelefon.Text = telefon;
                textBoxPlata.Text = plata;

                reader.Close();
                konekcija.Close();
            }
            catch (Exception greska)
            {
                MessageBox.Show(greska.Message);
            }
        }

        private void AzuriranjePodataka()
        {
            try
            {
                Double plata = System.Convert.ToDouble(textBoxPlata.Text);

                String upit = "UPDATE employees SET " +
                    " first_name='" + textBoxIme.Text + "', " +
                    " last_name='" + textBoxPrezime.Text + "', " +
                    " email='" + textBoxMail.Text + "', " +
                    " phone_number='" + textBoxTelefon.Text + "', " +
                    " salary='" + plata + "' " +
                    " WHERE employee_id='" + empID + "' ";

                MySqlConnection konekcija = new MySqlConnection(konekStr);
                konekcija.Open();

                MySqlCommand cmd = new MySqlCommand(upit, konekcija);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Podaci za korisnika ID=" + empID + " su ažurirani!!!");

                konekcija.Close();
            }
            catch (Exception greska)
            {
                MessageBox.Show(greska.Message);
            }
        }

        private void DodavanjePodataka()
        {
            try
            {

                String upit = "INSERT INTO employees(first_name, last_name, email, phone_number, " +
                    " hire_date, job_id, username, passwd, login_status) VALUES " +
                    " ('" + textBoxImeInsr.Text + "', '" + textBoxPrezimeInsr.Text + "', '" + textBoxMailInsr.Text + "', " +
                    " '" + textBoxTelefonInsr.Text + "', '" + dateTimePickerDatumInsr.Value.ToString("yyyy-MM-dd") + "', " +
                    " '" + comboBoxPosaoInsr.Text + "', '" + textBoxKorImeInsr.Text + "', " +
                    " '" + textBoxSifraInsr.Text + "', 0) ";

                MySqlConnection konekcija = new MySqlConnection(konekStr);
                konekcija.Open();

                MySqlCommand cmd = new MySqlCommand(upit, konekcija);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Dodan novi korisnik !!!");

                konekcija.Close();
            }
            catch (Exception greska)
            {
                MessageBox.Show(greska.Message);
            }
        }

        private void IspuniComboBox()
        {
            try
            {
                String upit = "select job_id from jobs";

                MySqlConnection konekcija = new MySqlConnection(konekStr);
                konekcija.Open();

                MySqlCommand cmd = new MySqlCommand(upit, konekcija);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    comboBoxPosaoInsr.Items.Add(reader["job_id"].ToString());
                }

                konekcija.Close();
            }
            catch (Exception greska)
            {
                MessageBox.Show(greska.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                String upit = "DELETE FROM employees WHERE employee_id = '" + textBox1.Text + "'";
                MySqlConnection konekcija = new MySqlConnection(konekStr);
                konekcija.Open();

                MySqlCommand cmd = new MySqlCommand(upit, konekcija);
                cmd.ExecuteNonQuery();
                konekcija.Close();
                MessageBox.Show("Uspješno ste deletedovali momčića s ID-om " + textBox1.Text + "!");
            }
            catch(Exception ex){
                MessageBox.Show(ex.Message);
            }
        }
    }
}

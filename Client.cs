using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace iTruck
{
    public partial class Client : Form
    {
        Form1 f1;

        static string con_string = "Host=localhost:5432;Username=postgres;Password=65adf4gs65d4fb4s6dfg4;Database=gruzoperevozki";
        NpgsqlConnection con = new NpgsqlConnection(con_string);

        public Client(Form1 f)
        {
            f1 = f;
            InitializeComponent();

            con.Open();
            NpgsqlCommand cmd = new NpgsqlCommand($"select фамилия, имя, отчество from клиент where Ид_клиента = {f1.acc_id}", con);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    label1.Text = reader.GetString(0) + " " + reader.GetString(1) + " " + reader.GetString(2);
                }
            }
            con.Close();


            //loadImage();
        }

        /*
        private void loadImage()
        {
            con.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("select image from услуги where Ид_услуги = 1", con);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {

                }
            }
             con.Close();
        }
        */

        private void Client_FormClosed(object sender, FormClosedEventArgs e)
        {
            f1.Show();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

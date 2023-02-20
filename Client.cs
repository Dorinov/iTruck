using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Npgsql;

namespace iTruck
{
    public partial class Client : Form
    {
        Form1 f1;

        int maxId = 0;
        int selId = 1;

        Image img;

        static string con_string = DataSafe.con_string;
        NpgsqlConnection con = new NpgsqlConnection(con_string);

        public Client(Form1 f)
        {
            f1 = f;
            InitializeComponent();
            img = pictureBox1.Image;

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
        }

        private void Client_Load(object sender, EventArgs e)
        {
            getMaxId();
        }







        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти из аккаунта?", "Выход", MessageBoxButtons.YesNo)
                == DialogResult.Yes)
            {
                Close();
            }
        }

        private void Client_FormClosed(object sender, FormClosedEventArgs e)
        {
            f1.Show();
        }




        private void setUslugiData(int id)
        {
            con.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("select * from услуги where Ид_услуги = " + id, con);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                name_uslugi.Text = reader.GetString(3);
                max_massa.Text = $"Максимальная масса: {reader.GetInt32(1)} килограмм.";
                max_objem.Text = $"Максимальный объем: {reader.GetInt32(2)} кубометров.";
            }
            con.Close();
        }
        private void loadImage(int id)
        {
            con.Open();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("select image from услуги where Ид_услуги = " + id, con);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Byte[] byteBLOBData = new Byte[0];
                    byteBLOBData = (Byte[])(ds.Tables[0].Rows[0][0]);
                    MemoryStream stmBLOBData = new MemoryStream(byteBLOBData);
                    pictureBox1.Image = Image.FromStream(stmBLOBData);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Не удалось загрузить изображение!\r\n" + ex.Message, "Ошибка", MessageBoxButtons.OK);
                pictureBox1.Image = img;
            }
            con.Close();
        }

        private bool isRowExists(int id)
        {
            bool exists = false;

            con.Open();
            NpgsqlCommand cmd1 = new NpgsqlCommand($"select exists(select наименование from услуги where Ид_услуги = {id})", con);
            var reader = cmd1.ExecuteReader();
            if (reader.Read())
            {
                exists = reader.GetBoolean(0);
            }
            con.Close();

            return exists;
        }


        private void getMaxId()
        {
            con.Open();
            NpgsqlCommand cmd1 = new NpgsqlCommand($"select Ид_услуги from услуги", con);
            using (var reader = cmd1.ExecuteReader())
            {
                while (reader.Read())
                {
                    maxId = reader.GetInt32(0);
                }
            }
            con.Close();
        }

        private void menu2uslugi(bool value)
        {
            if (value)
            {
                panel3.Location = new Point(12, 378);
                panel_uslug.Location = new Point(11, 30);
            }
            else
            {
                panel3.Location = new Point(11, 30);
                panel_uslug.Location = new Point(12, 378); 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            menu2uslugi(true);
            setUslugiData(selId);
            loadImage(selId);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            menu2uslugi(false);
            selId = 1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (selId != 1)
            {
                selId += -1;
                if (isRowExists(selId))
                {
                    setUslugiData(selId);
                    loadImage(selId);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (selId != maxId)
            {
                selId += 1;
                if (isRowExists(selId))
                {
                    setUslugiData(selId);
                    loadImage(selId);
                }
            }
        }

        private void menu2zakaz(bool value)
        {
            if (value)
            {
                panel3.Location = new Point(12, 378);
                panel_zakaza.Location = new Point(11, 30);
            }
            else
            {
                panel3.Location = new Point(11, 30);
                panel_zakaza.Location = new Point(12, 378);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            menu2zakaz(true);
            setUslugaText(selId);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            menu2zakaz(false);
            selId = 1;
            clearData();
        }


        private void setUslugaText(int id)
        {
            con.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("select наименование from услуги where Ид_услуги = " + id, con);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                label4.Text = reader.GetString(0) + $" ({id})";
            }
            con.Close();
        }

        private void clearData()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }


        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {
                con.Open();

                string txt = $"insert into заказ values({maxId+1}, null," + $"{f1.acc_id}, {selId}," +
                $"'{textBox1.Text}', '{textBox2.Text}', '{dateTimePicker1.Value.Date}'," +
                $"'{dateTimePicker1.Value.TimeOfDay}', '{textBox3.Text}', false)";

                try
                {
                    string zakaz = $"Услуга: {label4.Text} ({selId})\r\n" +
                                    $"Пункт А: {textBox1.Text}\r\n" +
                                    $"Пункт Б: {textBox2.Text}\r\n" +
                                    $"Дата и время: {dateTimePicker1.Value.Date} - {dateTimePicker1.Value.TimeOfDay}\r\n" +
                                    $"Комментарий: {textBox3.Text}";
                    if (MessageBox.Show("Уверены, что хотите сделать заказ?\r\n" + zakaz, "Заказ", MessageBoxButtons.YesNo)
                        == DialogResult.Yes)
                    {
                        NpgsqlCommand cmd = new NpgsqlCommand(txt, con);
                        cmd.ExecuteNonQuery();
                        menu2zakaz(false);
                        selId = 1;
                        clearData();
                        MessageBox.Show("Заказ создан!", "Заказ", MessageBoxButtons.OK);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось создать заказ.\r\n" + ex.Message, "Ошибка", MessageBoxButtons.OK);
                }

                con.Close();
            }
            else
            {
                MessageBox.Show("Сначала заполните все обязательные поля!", "Заказ", MessageBoxButtons.OK);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (selId != 1)
            {
                selId += -1;
                if (isRowExists(selId))
                {
                    setUslugaText(selId);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (selId != maxId)
            {
                selId += 1;
                if (isRowExists(selId))
                {
                    setUslugaText(selId);
                }
            }
        }
    }
}

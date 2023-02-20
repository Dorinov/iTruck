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
    public partial class Form1 : Form
    {
        public static string con_string = DataSafe.con_string;
        NpgsqlConnection con = new NpgsqlConnection(con_string);

        Timer timer = new Timer();
        int sec = 15;
        int try_counter = 0;

        public int acc_id;

        public Form1()
        {
            InitializeComponent();
        }

        private void timer_tick(object s, EventArgs e)
        {
            sec += -1;
            LogIn.Text = $"Вход ({sec} с.)";
            if (sec == 0)
            {
                sec = 15;
                LogIn.Enabled = true;
                LogUp.Enabled = true;
                LogIn.Text = "Вход";
                timer.Stop();
            }
        }

        public void startTimer()
        {
            LogIn.Text = "Вход (15 с.)";
            LogIn.Enabled = false;
            LogUp.Enabled = false;
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_tick);
            timer.Start();
        }

        private int checkLoginPassword(string id_c_name, string table_name)
        {
            int p_accept = 0;
            int id = 0;
            con.Open();
            NpgsqlCommand cmd = new NpgsqlCommand($"select {id_c_name}, номер_телефона from {table_name}", con);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string login = reader.GetString(1);
                    if (login == tb_login.Text)
                    {
                        id = reader.GetInt32(0);
                        break;
                    }
                }
            }
            if (id != 0)
            {
                NpgsqlCommand cmd1 = new NpgsqlCommand($"select пароль from {table_name} where {id_c_name} = " + id, con);
                var reader = cmd1.ExecuteReader();
                if (reader.Read())
                {
                    string pass = reader.GetString(0);
                    if (pass == tb_password.Text)
                    {
                        tb_login.Text = "";
                        tb_password.Text = "";
                        try_counter = 0;
                        acc_id = id;
                        p_accept = 2;
                    }
                    else
                    {
                        p_accept = 1;
                    }
                }
                reader.Close();
            }
            con.Close();
            return p_accept;
        }

        private void tryLogin()
        {
            if (tb_login.Text != "" && tb_password.Text != "")
            {
                try_counter++;

                int s = checkLoginPassword("Ид_сотрудника", "сотрудник");
                if (s == 0)
                {
                    int c = checkLoginPassword("Ид_клиента", "клиент");
                    if (c == 0)
                    {
                        if (!checkTryCounter())
                            MessageBox.Show("Аккаунт не существует.", "Ошибка входа", MessageBoxButtons.OK);
                    }
                    else if (c == 1)
                    {
                        if (!checkTryCounter())
                            MessageBox.Show("Неправильный пароль.", "Ошибка входа", MessageBoxButtons.OK);
                    }
                    else if (c == 2)
                    {
                        Client cl = new Client(this);
                        cl.Show();
                        this.Hide();
                    }
                }
                else if (s == 1)
                {
                    if (!checkTryCounter())
                        MessageBox.Show("Неправильный пароль.", "Ошибка входа", MessageBoxButtons.OK);
                }
                else if (s == 2)
                {
                    Sotrudnik sotr = new Sotrudnik(this);
                    sotr.Show();
                    this.Hide();
                }
            }
        }

        private bool checkTryCounter()
        {
            bool limit = false;
            if (try_counter == 3)
            {
                limit = true;
                try_counter = 0;
                Captcha ct = new Captcha(this);
                ct.ShowDialog();
            }
            return limit;
        }

        private void LogIn_Click(object sender, EventArgs e)
        {
            try { tryLogin(); }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка входа", MessageBoxButtons.OK);
            }
        }

        private void LogUp_Click(object sender, EventArgs e)
        {
            Reg r = new Reg(this);
            r.Show();
            this.Hide();
        }

        private void tb_login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                tb_password.Focus();
        }

        private void tb_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try { tryLogin(); }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка входа", MessageBoxButtons.OK);
                }
            }
        }

        public void regComplete()
        {
            Client cl = new Client(this);
            cl.Show();
            this.Hide();
        }
    }
}

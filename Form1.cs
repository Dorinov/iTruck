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

        private void tryLogin()
        {
            if (tb_login.Text != "" && tb_password.Text != "")
            {
                try_counter++;

                string id = "";
                string t_login = tb_login.Text;
                string t_pass = tb_password.Text;

                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("select Ид_сотрудника, номер_телефона from сотрудник", con);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string login = reader["номер_телефона"].ToString();
                        if (login == t_login)
                        {
                            id = reader["Ид_сотрудника"].ToString();
                            break;
                        }
                    }
                }

                if (id != "")
                {
                    bool p_accept = false;
                    NpgsqlCommand cmd1 = new NpgsqlCommand("select пароль from сотрудник where Ид_сотрудника = " + id, con);
                    using (var reader = cmd1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string pass = reader["пароль"].ToString();
                            if (pass == t_pass)
                            {
                                acc_id = Convert.ToInt32(id);
                                Sotrudnik sotr = new Sotrudnik(this);
                                tb_login.Text = "";
                                tb_password.Text = "";
                                sotr.Show();
                                try_counter = 0;
                                this.Hide();
                                p_accept = true;
                                break;
                            }
                        }
                    }
                    if (!p_accept)
                    {
                        if (try_counter == 3)
                        {
                            try_counter = 0;
                            Captcha ct = new Captcha(this);
                            ct.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Неправильный пароль.", "Ошибка входа", MessageBoxButtons.OK);
                        }
                    }
                }
                else
                {
                    NpgsqlCommand cmd1 = new NpgsqlCommand("select Ид_клиента, номер_телефона from клиент", con);
                    using (var reader = cmd1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string login = reader["номер_телефона"].ToString();
                            if (login == t_login)
                            {
                                id = reader["Ид_клиента"].ToString();
                                break;
                            }
                        }
                    }

                    if (id != "")
                    {
                        bool p_accept = false;
                        NpgsqlCommand cmd2 = new NpgsqlCommand("select пароль from клиент where Ид_клиента = " + id, con);
                        using (var reader = cmd2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string pass = reader["пароль"].ToString();
                                if (pass == t_pass)
                                {
                                    acc_id = Convert.ToInt32(id);
                                    Client cl = new Client(this);
                                    tb_login.Text = "";
                                    tb_password.Text = "";
                                    cl.Show();
                                    try_counter = 0;
                                    this.Hide();
                                    p_accept = true;
                                    break;
                                }
                            }
                        }
                        if (!p_accept)
                        {
                            if (try_counter == 3)
                            {
                                try_counter = 0;
                                Captcha ct = new Captcha(this);
                                ct.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("Неправильный пароль.", "Ошибка входа", MessageBoxButtons.OK);
                            }
                        }
                    }
                    else
                    {
                        if (try_counter == 3)
                        {
                            try_counter = 0;
                            Captcha ct = new Captcha(this);
                            ct.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Аккаунт не существует.", "Ошибка входа", MessageBoxButtons.OK);
                        }
                    }
                }
                con.Close();
            }
        }

        private void LogIn_Click(object sender, EventArgs e)
        {
            tryLogin();
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
            {
                tb_password.Focus();
            }
        }

        private void tb_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tryLogin();
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

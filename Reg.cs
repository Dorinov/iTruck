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
    public partial class Reg : Form
    {
        Form1 f1;

        static string con_string = "Host=localhost:5432;Username=postgres;Password=65adf4gs65d4fb4s6dfg4;Database=gruzoperevozki";
        NpgsqlConnection con = new NpgsqlConnection(con_string);

        public Reg(Form1 f)
        {
            f1 = f;
            InitializeComponent();
        }

        private void LogUp_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void tryReg()
        {
            if (tb_login.Text != "" && tb_password.Text != "")
            {
                string t_login = tb_login.Text;
                string t_pass = tb_password.Text;

                bool haveAcc = false;

                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("select номер_телефона from сотрудник", con);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string login = reader["номер_телефона"].ToString();
                        if (login == t_login)
                        {
                            haveAcc = true;
                            break;
                        }
                    }
                }

                if (!haveAcc)
                {
                    int maxId = 0;

                    NpgsqlCommand cmd1 = new NpgsqlCommand("select Ид_клиента, номер_телефона from клиент", con);
                    using (var reader = cmd1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            maxId = Convert.ToInt32(reader["Ид_клиента"]);
                            string login = reader["номер_телефона"].ToString();
                            if (login == t_login)
                            {
                                haveAcc = true;
                                break;
                            }
                        }
                    }
                    maxId++;

                    if (!haveAcc)
                    {
                        NpgsqlCommand cmd2 = new NpgsqlCommand($"insert into клиент values({maxId},null,null,null,'{t_login}',null,'{t_pass}')", con);
                        cmd2.ExecuteNonQuery();
                        f1.acc_id = maxId;
                        f1.regComplete();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Такой аккаунт уже существует.", "Ошибка регистрации", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("Такой аккаунт уже существует.", "Ошибка регистрации", MessageBoxButtons.OK);
                }
                con.Close();
            }
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
                tryReg();
            }
        }

        private void LogIn_Click(object sender, EventArgs e)
        {
            tryReg();
        }
    }
}

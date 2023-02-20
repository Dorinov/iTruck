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

        static string con_string = DataSafe.con_string;
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

        private bool isLoginAlreadyUsed(string table_name)
        {
            bool used = false;
            NpgsqlCommand cmd = new NpgsqlCommand($"select exists(select пароль from {table_name} where номер_телефона = {tb_login.Text})", con);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                used = reader.GetBoolean(0);
            }
            reader.Close();
            return used;
        }

        private int getMaxId()
        {
            int id = 0;
            NpgsqlCommand cmd = new NpgsqlCommand("select Ид_клиента from клиент", con);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                }
            }
            id++;
            return id;
        }

        private void tryReg()
        {
            if (tb_login.Text != "" && tb_password.Text != "")
            {
                con.Open();
                if (!isLoginAlreadyUsed("сотрудник") && !isLoginAlreadyUsed("клиент"))
                {
                    int id = getMaxId();
                    NpgsqlCommand cmd = new NpgsqlCommand($"insert into клиент values({id},null,null,null,'{tb_login.Text}',null,'{tb_password.Text}')", con);
                    cmd.ExecuteNonQuery();
                    f1.acc_id = id;
                    f1.regComplete();
                    this.Close();
                }
                else
                    MessageBox.Show("Такой аккаунт уже существует.", "Ошибка регистрации", MessageBoxButtons.OK);
                con.Close();
            }
        }

        private void tb_login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                tb_password.Focus();
        }

        private void tb_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                tryReg();
        }

        private void LogIn_Click(object sender, EventArgs e)
        {
            tryReg();
        }
    }
}

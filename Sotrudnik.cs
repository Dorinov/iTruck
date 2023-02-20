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
    public partial class Sotrudnik : Form
    {
        Form1 f1;
        int id_d;
        string s_d;

        int last_value = 1;

        bool edit_mode = false;

        string[] names = new string[5] { "должность", "сотрудник", "клиент", "заказ", "услуги" };
        string[] ids = new string[5] { "Ид_должности", "Ид_сотрудника", "Ид_клиента", "Ид_заказа", "Ид_услуги" };
        int sel_tab = 0;

        static string con_string = DataSafe.con_string;
        NpgsqlConnection con = new NpgsqlConnection(con_string);

        public Sotrudnik(Form1 f)
        {
            f1 = f;
            InitializeComponent();

            con.Open();
            NpgsqlCommand cmd = new NpgsqlCommand($"select ид_должности, фамилия, имя, отчество, наименование from сотрудник inner join должность on должность.Ид_должности = сотрудник.ид_должности where Ид_сотрудника = {f1.acc_id}", con);
            using(var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    id_d = reader.GetInt32(0);
                    label27.Text = reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3);
                    s_d = reader.GetString(4);
                }
            }
            con.Close();

            label1.Text = $"ID: {f1.acc_id}   |   {s_d}";
        }

        private void Sotrudnik_FormClosed(object sender, FormClosedEventArgs e)
        {
            f1.Show();
        }

        private void changePanel(int arg, int val = 1)
        {
            sel_tab = arg;
            numericUpDown1.Value = val;

            panel_dolzh.Location = new Point(12, 298);
            panel_sotr.Location = new Point(12, 298);
            panel_client.Location = new Point(12, 298);
            panel_zakaz.Location = new Point(12, 298);
            panel_usl.Location = new Point(12, 298);

            switch (arg)
            {
                case 1:
                    panel_dolzh.Location = new Point(137, 61);
                    break;
                case 2:
                    panel_sotr.Location = new Point(137, 61);
                    break;
                case 3:
                    panel_client.Location = new Point(137, 61);
                    break;
                case 4:
                    panel_zakaz.Location = new Point(137, 61);
                    break;
                case 5:
                    panel_usl.Location = new Point(137, 61);
                    break;
                default:
                    break;
            }

            if (arg != 0)
            {
                label3.Text = $"Данные ({names[arg - 1]})";
                button_dataViewer.Enabled = true;
                int maxId = 0;
                con.Open();
                NpgsqlCommand cmd1 = new NpgsqlCommand($"select {ids[arg-1]} from {names[arg-1]}", con);
                using (var reader = cmd1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        maxId = reader.GetInt32(0);
                    }
                }
                con.Close();
                getNewData(val);

                numericUpDown1.Enabled = true;
                numericUpDown1.Maximum = maxId;

                if (id_d == 1)
                {
                    button_edit.Enabled = true;
                    button_add.Enabled = true;
                    button_delete.Enabled = true;
                }
                else if (id_d == 2)
                    button_add.Enabled = true;
            }
            else
            {
                label3.Text = "Данные";
                numericUpDown1.Enabled = false;
                button_edit.Enabled = false;
                button_add.Enabled = false;
                button_delete.Enabled = false;
            }
        }

        private void button_dolzh_Click(object sender, EventArgs e)
        {
            changePanel(1);
        }

        private void button_sotr_Click(object sender, EventArgs e)
        {
            changePanel(2);
        }

        private void button_client_Click(object sender, EventArgs e)
        {
            changePanel(3);
        }

        private void button_zakaz_Click(object sender, EventArgs e)
        {
            changePanel(4);
        }

        private void button_usl_Click(object sender, EventArgs e)
        {
            changePanel(5);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value - last_value == 1)
                getNewData(Convert.ToInt32(numericUpDown1.Value));
            else
                getNewData(Convert.ToInt32(numericUpDown1.Value), false);
            last_value = Convert.ToInt32(numericUpDown1.Value);
        }

        
        private void getNewData(int _id, bool kuda = true)
        {
            con.Open();
            bool have = false;
            NpgsqlCommand cmd;
            while (!have)
            {
                cmd = new NpgsqlCommand($"SELECT EXISTS(select * from {names[sel_tab-1]} where {ids[sel_tab - 1]} = {_id})", con);
                var reader1 = cmd.ExecuteReader();
                if (reader1.Read())
                    have = reader1.GetBoolean(0);
                reader1.Close();

                if (!have)
                {
                    if (kuda)
                        _id++;
                    else
                        _id--;
                }
            }

            NpgsqlCommand cmd1 = new NpgsqlCommand($"select * from {names[sel_tab - 1]} where {ids[sel_tab - 1]} = {_id}", con);
            var reader = cmd1.ExecuteReader();
            if (reader.Read())
            {
                switch (sel_tab)
                {
                    case 1:
                        textBox4.Text = reader.GetString(1);
                        textBox5.Text = Convert.ToString(reader.GetInt32(2)) + " RUB";
                        break;
                    case 2:
                        textBox6.Text = reader.GetString(1);
                        textBox7.Text = reader.GetString(2);
                        textBox8.Text = reader.GetString(3);
                        textBox9.Text = reader.GetString(4);
                        numericUpDown5.Value = reader.GetInt32(5);
                        textBox10.Text = reader.GetString(6);
                        textBox10.PasswordChar = '*';
                        break;
                    case 3:
                        textBox15.Text = reader.GetString(1);
                        textBox14.Text = reader.GetString(2);
                        textBox13.Text = reader.GetString(3);
                        textBox12.Text = reader.GetString(4);
                        textBox11.Text = reader.GetString(5);
                        textBox11.PasswordChar = '*';
                        break;
                    case 4:
                        if (reader.GetValue(1).ToString() != "")
                            numericUpDown2.Value = reader.GetInt32(1);
                        else
                            numericUpDown2.Value = 0;
                        numericUpDown3.Value = reader.GetInt32(2);
                        numericUpDown4.Value = reader.GetInt32(3);
                        textBox1.Text = reader.GetString(4);
                        textBox2.Text = reader.GetString(5);
                        string date = Convert.ToString(reader.GetDateTime(6)).Substring(0, 10);
                        string time = Convert.ToString(reader.GetTimeSpan(7));
                        dateTimePicker1.Value = DateTime.Parse(date + " " + time);
                        textBox3.Text = reader.GetString(8);
                        checkBox1.Checked = reader.GetBoolean(9);
                        break;
                    case 5:
                        textBox17.Text = Convert.ToString(reader.GetInt32(1)) + " кг";
                        textBox18.Text = Convert.ToString(reader.GetInt32(2)) + " м3";
                        textBox16.Text = reader.GetString(3);
                        break;
                    default:
                        break;
                }
            }
            reader.Close();
            con.Close();
            numericUpDown1.Value = _id;
        }

        private void button_edit_Click(object sender, EventArgs e)
        {
            if (edit_mode)
            {
                int id = Convert.ToInt32(numericUpDown1.Value);
                string txt = "";
                con.Open();
                switch (sel_tab)
                {
                    case 1:
                        txt = $"update должность set наименование = '{textBox4.Text}', оклад = {textBox5.Text} where Ид_должности = {id}";
                        break;
                    case 2:
                        txt = $"update сотрудник set фамилия = '{textBox6.Text}', имя = '{textBox7.Text}', отчество = '{textBox8.Text}', номер_телефона = '{textBox9.Text}', ид_должности = {Convert.ToInt32(numericUpDown5.Value)}, пароль = '{textBox10.Text}' where Ид_сотрудника = {id}";
                        break;
                    case 3:
                        txt = $"update клиент set фамилия = '{textBox15.Text}', имя = '{textBox14.Text}', отчество = '{textBox13.Text}', номер_телефона = '{textBox12.Text}', пароль = '{textBox11.Text}' where Ид_клиента = {id}";
                        break;
                    case 4:
                        txt = $"update заказ set ид_сотрудника = {Convert.ToInt32(numericUpDown2.Value)}, ид_клиента = {Convert.ToInt32(numericUpDown3.Value)}, ид_услуги = {Convert.ToInt32(numericUpDown4.Value)}, пункт_а = '{textBox1.Text}', пункт_б = '{textBox2.Text}', дата = '{dateTimePicker1.Value.Date}', время = '{dateTimePicker1.Value.TimeOfDay}', комментарий_заказчика = '{textBox3.Text}', выполнен = {Convert.ToString(checkBox1.Checked)} where Ид_заказа = {id};";
                        break;
                    case 5:
                        txt = $"update услуги set макс_вес = {textBox17.Text}, макс_объем = {textBox18.Text}, наименование = '{textBox16.Text}' where Ид_услуги = {id}";
                        break;
                    default:
                        break;
                }

                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(txt, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    changeButtonsMode(1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Ошибка запроса", MessageBoxButtons.OK);
                    con.Close();
                }
            }
            else
                changeButtonsMode(4);
        }

        /*  operationId:
         *      1 - завершение редактирования
         *      2 - завершение добавления
         *      3 - отмена
         * 
         *      4 - начало редактирования
         *      5 - начало добавления */
        private void changeButtonsMode(int operationId, int maxid = 0)
        {
            if (operationId <= 3)
            {
                numericUpDown1.Enabled = true;
                button_add.Enabled = true;
                button_edit.Enabled = true;
                button_edit.Text = "Редактировать";
                button_delete.Enabled = true;
                button_dataViewer.Enabled = true;
                unlockPanels(false);
                getNewData(Convert.ToInt32(numericUpDown1.Value));
                button_dolzh.Enabled = true;
                button_sotr.Enabled = true;
                button_client.Enabled = true;
                button_zakaz.Enabled = true;
                button_usl.Enabled = true;
                button1.Visible = false;
                edit_mode = false;
                if (operationId == 2)
                {
                    numericUpDown1.Maximum = maxid;
                    numericUpDown1.Value = maxid;
                }
            }
            else
            {
                edit_mode = true;
                button1.Visible = true;
                button_dolzh.Enabled = false;
                button_sotr.Enabled = false;
                button_client.Enabled = false;
                button_zakaz.Enabled = false;
                button_usl.Enabled = false;
                button_dataViewer.Enabled = false;
                numericUpDown1.Enabled = false;
                button_delete.Enabled = false;
                unlockPanels(true);
                if (operationId == 4)
                {
                    button_add.Enabled = false;
                    button_edit.Text = "Применить";
                }
                if (operationId == 5)
                    button_edit.Enabled = false;
            }
        }

        private void unlockPanels(bool i)
        {
            if (i)
            {
                textBox10.PasswordChar = '0';
                textBox11.PasswordChar = '0';
            }
            else
            {
                textBox10.PasswordChar = '*';
                textBox11.PasswordChar = '*';
            }

            switch (sel_tab)
            {
                case 1:
                    textBox4.Enabled = i;
                    textBox5.Enabled = i;
                    if (i)
                        textBox5.Text = textBox5.Text.Substring(0, textBox5.Text.Length - 4); ;
                    break;
                case 2:
                    textBox6.Enabled = i;
                    textBox7.Enabled = i;
                    textBox8.Enabled = i;
                    textBox9.Enabled = i;
                    numericUpDown5.Enabled = i;
                    textBox10.Enabled = i;
                    break;
                case 3:
                    textBox15.Enabled = i;
                    textBox14.Enabled = i;
                    textBox13.Enabled = i;
                    textBox12.Enabled = i;
                    textBox11.Enabled = i;
                    break;
                case 4:
                    numericUpDown2.Enabled = i;
                    numericUpDown3.Enabled = i;
                    numericUpDown4.Enabled = i;
                    textBox1.Enabled = i;
                    textBox2.Enabled = i;
                    dateTimePicker1.Enabled = i;
                    textBox3.Enabled = i;
                    checkBox1.Enabled = i;
                    break;
                case 5:
                    textBox17.Enabled = i;
                    textBox18.Enabled = i;
                    textBox16.Enabled = i;
                    if (i)
                    {
                        textBox17.Text = textBox17.Text.Substring(0, textBox17.Text.Length - 3);
                        textBox18.Text = textBox18.Text.Substring(0, textBox18.Text.Length - 3);
                    }
                    break;
                default:
                    break;
            }
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            if (edit_mode)
            {
                int maxId = Convert.ToInt32(numericUpDown1.Maximum) + 1;
                string txt = "";
                con.Open();
                switch (sel_tab)
                {
                    case 1:
                        txt = $"insert into должность values({maxId}, '{textBox4.Text}', {textBox5.Text})";
                        break;
                    case 2:
                        txt = $"insert into сотрудник values({maxId}, '{textBox6.Text}', '{textBox7.Text}', '{textBox8.Text}', '{textBox9.Text}', {Convert.ToInt32(numericUpDown5.Value)}, '{textBox10.Text}')";
                        break;
                    case 3:
                        txt = $"insert into клиент values({maxId}, '{textBox15.Text}', '{textBox14.Text}', '{textBox13.Text}', '{textBox12.Text}', '{textBox11.Text}')";
                        break;
                    case 4:
                        txt = $"insert into заказ values({maxId}, {Convert.ToInt32(numericUpDown2.Value)}, {Convert.ToInt32(numericUpDown3.Value)}, {Convert.ToInt32(numericUpDown4.Value)}, '{textBox1.Text}', '{textBox2.Text}', '{dateTimePicker1.Value.Date}', '{dateTimePicker1.Value.TimeOfDay}', '{textBox3.Text}', {Convert.ToString(checkBox1.Checked)})";
                        break;
                    case 5:
                        txt = $"insert into услуги values({maxId}, {textBox17.Text}, {textBox18.Text}, '{textBox16.Text}')";
                        break;
                    default:
                        break;
                }

                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(txt, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    changeButtonsMode(2, maxId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Ошибка запроса", MessageBoxButtons.OK);
                    con.Close();
                }
            }
            else
            {
                changeButtonsMode(5);
                switch (sel_tab)
                {
                    case 1:
                        textBox4.Text = "";
                        textBox5.Text = "";
                        break;
                    case 2:
                        textBox6.Text = "";
                        textBox7.Text = "";
                        textBox8.Text = "";
                        textBox9.Text = "";
                        numericUpDown5.Minimum = id_d;
                        textBox10.Text = "";
                        break;
                    case 3:
                        textBox15.Text = "";
                        textBox14.Text = "";
                        textBox13.Text = "";
                        textBox12.Text = "";
                        textBox11.Text = "";
                        break;
                    case 4:
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        checkBox1.Checked = false;
                        break;
                    case 5:
                        textBox17.Text = "";
                        textBox18.Text = "";
                        textBox16.Text = "";
                        break;
                    default:
                        break;
                }
            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show($"Вы уверены, что хотите удалить эту запись?\r\nid: {Convert.ToInt32(numericUpDown1.Value)}, table: {names[sel_tab-1]}", "Удаление", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                con.Open();
                NpgsqlCommand cmdDel = new NpgsqlCommand($"delete from {names[sel_tab-1]} where {ids[sel_tab-1]} = {Convert.ToInt32(numericUpDown1.Value)}", con);
                cmdDel.ExecuteNonQuery();
                con.Close();
                changePanel(sel_tab);
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (edit_mode)
                changeButtonsMode(3);
        }

        private void button_dataViewer_Click(object sender, EventArgs e)
        {
            dataViewer dv = new dataViewer();
            dv.sel_tab = names[sel_tab - 1];
            dv.firstColumnName = ids[sel_tab - 1];
            dv.id_dolzh = id_d;
            dv.ShowDialog();
        }
    }
}

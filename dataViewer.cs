using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using Npgsql;

namespace iTruck
{
    public partial class dataViewer : Form
    {
        public string sel_tab;
        public string firstColumnName;
        public int id_dolzh;

        bool editRows = false;

        private BindingSource bindingSource1 = new BindingSource();
        private NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter();

        static string con_string = DataSafe.con_string;
        NpgsqlConnection con = new NpgsqlConnection(con_string);

        public dataViewer()
        {
            InitializeComponent();
        }

        private void dataViewer_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = bindingSource1;
            GetData($"select * from {sel_tab} order by {firstColumnName}");

            label1.Text = "Таблица: " + sel_tab;
            setAccess();
        }

        private void setAccess()
        {
            switch (id_dolzh)
            {
                case 1:
                    dataGridView1.ReadOnly = false;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    editRows = true;
                    break;
                case 2:
                    dataGridView1.ReadOnly = false;
                    button2.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        private void GetData(string cmd)
        {
            try
            {
                dataAdapter = new NpgsqlDataAdapter(cmd, con_string);

                NpgsqlCommandBuilder commandBuilder = new NpgsqlCommandBuilder(dataAdapter);

                DataTable table = new DataTable
                {
                    Locale = CultureInfo.InvariantCulture
                };
                dataAdapter.Fill(table);
                bindingSource1.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //GetData(dataAdapter.SelectCommand.CommandText); //
            GetData($"select * from {sel_tab} order by {firstColumnName}");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                dataAdapter.Update((DataTable)bindingSource1.DataSource);
                GetData($"select * from {sel_tab} order by {firstColumnName}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int y = dataGridView1.CurrentCellAddress.Y;
                int id = Convert.ToInt32(dataGridView1[0, y].Value);

                if (MessageBox.Show($"Вы уверены, что хотите удалить строку?\r\ntable: {sel_tab}, id: {id}", "Удаление", MessageBoxButtons.YesNo)
                    == DialogResult.Yes)
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand($"delete from {sel_tab} where {firstColumnName} = {id}", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                GetData($"select * from {sel_tab} order by {firstColumnName}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }






        private void печатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                printDocument1.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка печати", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                Bitmap bmp = new Bitmap(dataGridView1.Size.Width + 10, dataGridView1.Size.Height + 30);
                dataGridView1.DrawToBitmap(bmp, dataGridView1.Bounds);
                e.Graphics.DrawImage(bmp, 0, 0);
                e.Graphics.DrawString(sel_tab,
                             new Font("Arial", 12),
                             Brushes.Black,
                             new PointF((dataGridView1.Size.Width + 10) / 2, 5));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка печати", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void отправитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            genImage();
            SendMail sm = new SendMail();
            sm.ShowDialog();
        }


        private void genImage()
        {
            Bitmap result = new Bitmap(dataGridView1.Size.Width + 10, dataGridView1.Size.Height + 30);
            Graphics g = Graphics.FromImage((Image)result);
            g.Clear(Color.White);
            dataGridView1.DrawToBitmap(result, dataGridView1.Bounds);
            g.DrawImage(result, 0, 0);
            g.DrawString(sel_tab,
                         new Font("Arial", 12),
                         Brushes.Black,
                         new PointF((dataGridView1.Size.Width + 10) / 2, 5));

            result.Save("mail.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!editRows)
                ((DataGridView)sender).CancelEdit();
        }
    }
}

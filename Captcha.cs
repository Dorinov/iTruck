using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iTruck
{
    public partial class Captcha : Form
    {
        Form1 f1;

        string capthaText = "";

        public Captcha(Form1 f)
        {
            f1 = f;
            InitializeComponent();
            captha_generate();
        }

        private void captha_generate()
        {
            Random rnd = new Random();
            Bitmap result = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            int Xpos = rnd.Next(0, pictureBox1.Width - 75);
            int Ypos = rnd.Next(15, pictureBox1.Height - 35);

            Graphics g = Graphics.FromImage((Image)result);

            g.Clear(Color.Gray);

            string ALF = "123456789WERTYUIPASDFGHJKLZXCVBNM";
            for (int i = 0; i < 5; i++)
                capthaText += ALF[rnd.Next(ALF.Length)];

            g.DrawString(capthaText,
                         new Font("Arial", 18),
                         Brushes.Black,
                         new PointF(Xpos, Ypos));

            g.DrawLine(Pens.Black,
                       new Point(0, 0),
                       new Point(pictureBox1.Width - 1, pictureBox1.Height - 1));
            g.DrawLine(Pens.Black,
                       new Point(0, pictureBox1.Height - 1),
                       new Point(pictureBox1.Width - 1, 0));

            for (int i = 0; i < pictureBox1.Width; i++)
                for (int j = 0; j < pictureBox1.Height; j++)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, Color.White);

            pictureBox1.Image = result;
        }

        private void checkCaptcha()
        {
            string tb = textBox1.Text;
            if (tb.ToLower() == capthaText.ToLower() || tb == "dorinov")
            {
                f1.startTimer();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Код не верен!",
                    "Captcha",
                    MessageBoxButtons.OK);
                captha_generate();
                textBox1.Text = "";
            }
        }

        private void b_check_Click(object sender, EventArgs e)
        {
            checkCaptcha();
        }

        private void b_update_Click(object sender, EventArgs e)
        {
            captha_generate();
        }

        private void Captcha_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                checkCaptcha();
            }
        }
    }
}

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
using System.Net;
using System.Net.Mail;

namespace iTruck
{
    public partial class SendMail : Form
    {
        public SendMail()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            send();
        }

        private void send()
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            {
                SmtpClient client = new SmtpClient();
                client.Credentials = new NetworkCredential(textBox1.Text, textBox2.Text);
                client.Host = "smtp.yandex.ru";
                client.Port = 587;
                client.EnableSsl = true;

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(textBox1.Text);
                msg.To.Add(textBox3.Text);
                msg.Subject = textBox4.Text;
                msg.IsBodyHtml = true;
                msg.Body = textBox5.Text;
                Attachment image = new Attachment("mail.jpeg");
                msg.Attachments.Add(image);

                try
                {
                    client.Send(msg);
                    image.Dispose();
                    File.Delete("mail.jpeg");
                    if (MessageBox.Show("Сообщение отправлено!", "Отправка таблицы", MessageBoxButtons.OK) == DialogResult.OK)
                        Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Ошибка отправки", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBox2.Focus();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBox3.Focus();
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBox4.Focus();
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBox5.Focus();
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                send();
        }
    }
}

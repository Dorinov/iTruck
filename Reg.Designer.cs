﻿namespace iTruck
{
    partial class Reg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LogUp = new System.Windows.Forms.Button();
            this.LogIn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tb_login = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Пароль";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.tb_password);
            this.panel2.Location = new System.Drawing.Point(44, 55);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(231, 40);
            this.panel2.TabIndex = 9;
            // 
            // tb_password
            // 
            this.tb_password.Location = new System.Drawing.Point(9, 9);
            this.tb_password.Name = "tb_password";
            this.tb_password.Size = new System.Drawing.Size(208, 20);
            this.tb_password.TabIndex = 5;
            this.tb_password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_password_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Номер телефона";
            // 
            // LogUp
            // 
            this.LogUp.Location = new System.Drawing.Point(44, 134);
            this.LogUp.Name = "LogUp";
            this.LogUp.Size = new System.Drawing.Size(231, 23);
            this.LogUp.TabIndex = 8;
            this.LogUp.Text = "Обратно ко входу";
            this.LogUp.UseVisualStyleBackColor = true;
            this.LogUp.Click += new System.EventHandler(this.LogUp_Click);
            // 
            // LogIn
            // 
            this.LogIn.Location = new System.Drawing.Point(44, 105);
            this.LogIn.Name = "LogIn";
            this.LogIn.Size = new System.Drawing.Size(231, 23);
            this.LogIn.TabIndex = 5;
            this.LogIn.Text = "Регистрация";
            this.LogIn.UseVisualStyleBackColor = true;
            this.LogIn.Click += new System.EventHandler(this.LogIn_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.tb_login);
            this.panel1.Location = new System.Drawing.Point(44, 14);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(231, 81);
            this.panel1.TabIndex = 6;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(141, 90);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 4;
            // 
            // tb_login
            // 
            this.tb_login.Location = new System.Drawing.Point(9, 9);
            this.tb_login.Name = "tb_login";
            this.tb_login.Size = new System.Drawing.Size(208, 20);
            this.tb_login.TabIndex = 3;
            this.tb_login.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_login_KeyDown);
            // 
            // Reg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 167);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LogUp);
            this.Controls.Add(this.LogIn);
            this.Controls.Add(this.panel1);
            this.MaximumSize = new System.Drawing.Size(336, 206);
            this.MinimumSize = new System.Drawing.Size(336, 206);
            this.Name = "Reg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "iTruck | Регистрация";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tb_password;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button LogUp;
        private System.Windows.Forms.Button LogIn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox tb_login;
    }
}
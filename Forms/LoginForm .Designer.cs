using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project3.Forms
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer? components = null;
        private ComboBox? languageComboBox;
        private Label? titleLabel;
        private TextBox? loginTextBox;
        private TextBox? passwordTextBox;
        private Button? loginButton;
        private LinkLabel? createAccountLink;
        private CheckBox? showPasswordCheckBox;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            languageComboBox = new ComboBox();
            titleLabel = new Label();
            loginTextBox = new TextBox();
            passwordTextBox = new TextBox();
            loginButton = new Button();
            createAccountLink = new LinkLabel();
            showPasswordCheckBox = new CheckBox();
            SuspendLayout();
            // 
            // languageComboBox
            // 
            languageComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            languageComboBox.Font = new Font("Microsoft Sans Serif", 12F);
            languageComboBox.Location = new Point(30, 38);
            languageComboBox.Margin = new Padding(4, 5, 4, 5);
            languageComboBox.Name = "languageComboBox";
            languageComboBox.Size = new Size(174, 37);
            languageComboBox.TabIndex = 0;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Microsoft Sans Serif", 36F, FontStyle.Bold);
            titleLabel.ForeColor = Color.White;
            titleLabel.Location = new Point(438, 273);
            titleLabel.Margin = new Padding(4, 0, 4, 0);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(481, 82);
            titleLabel.TabIndex = 1;
            titleLabel.Text = "Авторизация";
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // loginTextBox
            // 
            loginTextBox.Font = new Font("Microsoft Sans Serif", 14F);
            loginTextBox.ForeColor = Color.Gray;
            loginTextBox.Location = new Point(465, 461);
            loginTextBox.Margin = new Padding(4, 5, 4, 5);
            loginTextBox.Name = "loginTextBox";
            loginTextBox.Size = new Size(399, 39);
            loginTextBox.TabIndex = 2;
            loginTextBox.Text = "Логин";
            // 
            // passwordTextBox
            // 
            passwordTextBox.Font = new Font("Microsoft Sans Serif", 14F);
            passwordTextBox.ForeColor = Color.Gray;
            passwordTextBox.Location = new Point(465, 586);
            passwordTextBox.Margin = new Padding(4, 5, 4, 5);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.Size = new Size(399, 39);
            passwordTextBox.TabIndex = 3;
            passwordTextBox.Text = "Пароль";
            // 
            // loginButton
            // 
            loginButton.BackColor = Color.White;
            loginButton.Font = new Font("Microsoft Sans Serif", 14F);
            loginButton.Location = new Point(519, 750);
            loginButton.Margin = new Padding(4, 5, 4, 5);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(292, 78);
            loginButton.TabIndex = 5;
            loginButton.Text = "Войти";
            loginButton.UseVisualStyleBackColor = false;
            // 
            // createAccountLink
            // 
            createAccountLink.AutoSize = true;
            createAccountLink.Font = new Font("Microsoft Sans Serif", 12F);
            createAccountLink.LinkColor = Color.White;
            createAccountLink.Location = new Point(544, 886);
            createAccountLink.Margin = new Padding(4, 0, 4, 0);
            createAccountLink.Name = "createAccountLink";
            createAccountLink.Size = new Size(203, 29);
            createAccountLink.TabIndex = 6;
            createAccountLink.TabStop = true;
            createAccountLink.Text = "Создать аккаунт";
            createAccountLink.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // showPasswordCheckBox
            // 
            showPasswordCheckBox.AutoSize = true;
            showPasswordCheckBox.Location = new Point(831, 598);
            showPasswordCheckBox.Margin = new Padding(4, 5, 4, 5);
            showPasswordCheckBox.Name = "showPasswordCheckBox";
            showPasswordCheckBox.Size = new Size(22, 21);
            showPasswordCheckBox.TabIndex = 4;
            showPasswordCheckBox.UseVisualStyleBackColor = true;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(123, 104, 238);
            ClientSize = new Size(1330, 1064);
            Controls.Add(createAccountLink);
            Controls.Add(loginButton);
            Controls.Add(showPasswordCheckBox);
            Controls.Add(passwordTextBox);
            Controls.Add(loginTextBox);
            Controls.Add(titleLabel);
            Controls.Add(languageComboBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Авторизация - Первая форма";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}

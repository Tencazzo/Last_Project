using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project3.Forms
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer? components = null;
        private ComboBox? languageComboBox;
        private Label? titleLabel;
        private TextBox? loginTextBox;
        private TextBox? emailTextBox;
        private TextBox? passwordTextBox;
        private TextBox? confirmPasswordTextBox;
        private Button? registerButton;
        private LinkLabel? authorizationLink;
        private CheckBox? showPasswordCheckBox;
        private CheckBox? showConfirmPasswordCheckBox;

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
            emailTextBox = new TextBox();
            passwordTextBox = new TextBox();
            confirmPasswordTextBox = new TextBox();
            registerButton = new Button();
            authorizationLink = new LinkLabel();
            showPasswordCheckBox = new CheckBox();
            showConfirmPasswordCheckBox = new CheckBox();
            SuspendLayout();
            // 
            // languageComboBox
            // 
            languageComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            languageComboBox.Font = new Font("Microsoft Sans Serif", 12F);
            languageComboBox.Location = new Point(31, 25);
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
            titleLabel.Location = new Point(448, 234);
            titleLabel.Margin = new Padding(4, 0, 4, 0);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(471, 82);
            titleLabel.TabIndex = 1;
            titleLabel.Text = "Регистрация";
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // loginTextBox
            // 
            loginTextBox.Font = new Font("Microsoft Sans Serif", 14F);
            loginTextBox.ForeColor = Color.Gray;
            loginTextBox.Location = new Point(465, 386);
            loginTextBox.Margin = new Padding(4, 5, 4, 5);
            loginTextBox.Name = "loginTextBox";
            loginTextBox.Size = new Size(399, 39);
            loginTextBox.TabIndex = 2;
            loginTextBox.Text = "Логин";
            // 
            // emailTextBox
            // 
            emailTextBox.Font = new Font("Microsoft Sans Serif", 14F);
            emailTextBox.ForeColor = Color.Gray;
            emailTextBox.Location = new Point(465, 495);
            emailTextBox.Margin = new Padding(4, 5, 4, 5);
            emailTextBox.Name = "emailTextBox";
            emailTextBox.Size = new Size(399, 39);
            emailTextBox.TabIndex = 3;
            emailTextBox.Text = "Почта";
            // 
            // passwordTextBox
            // 
            passwordTextBox.Font = new Font("Microsoft Sans Serif", 14F);
            passwordTextBox.ForeColor = Color.Gray;
            passwordTextBox.Location = new Point(465, 605);
            passwordTextBox.Margin = new Padding(4, 5, 4, 5);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.Size = new Size(399, 39);
            passwordTextBox.TabIndex = 4;
            passwordTextBox.Text = "Пароль";
            // 
            // confirmPasswordTextBox
            // 
            confirmPasswordTextBox.Font = new Font("Microsoft Sans Serif", 14F);
            confirmPasswordTextBox.ForeColor = Color.Gray;
            confirmPasswordTextBox.Location = new Point(465, 714);
            confirmPasswordTextBox.Margin = new Padding(4, 5, 4, 5);
            confirmPasswordTextBox.Name = "confirmPasswordTextBox";
            confirmPasswordTextBox.Size = new Size(399, 39);
            confirmPasswordTextBox.TabIndex = 6;
            confirmPasswordTextBox.Text = "Повторите пароль";
            // 
            // registerButton
            // 
            registerButton.BackColor = Color.White;
            registerButton.Font = new Font("Microsoft Sans Serif", 14F);
            registerButton.Location = new Point(519, 848);
            registerButton.Margin = new Padding(4, 5, 4, 5);
            registerButton.Name = "registerButton";
            registerButton.Size = new Size(292, 78);
            registerButton.TabIndex = 8;
            registerButton.Text = "Войти";
            registerButton.UseVisualStyleBackColor = false;
            // 
            // authorizationLink
            // 
            authorizationLink.AutoSize = true;
            authorizationLink.Font = new Font("Microsoft Sans Serif", 12F);
            authorizationLink.LinkColor = Color.White;
            authorizationLink.Location = new Point(570, 977);
            authorizationLink.Margin = new Padding(4, 0, 4, 0);
            authorizationLink.Name = "authorizationLink";
            authorizationLink.Size = new Size(166, 29);
            authorizationLink.TabIndex = 9;
            authorizationLink.TabStop = true;
            authorizationLink.Text = "Авторизация";
            authorizationLink.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // showPasswordCheckBox
            // 
            showPasswordCheckBox.AutoSize = true;
            showPasswordCheckBox.Location = new Point(830, 617);
            showPasswordCheckBox.Margin = new Padding(4, 5, 4, 5);
            showPasswordCheckBox.Name = "showPasswordCheckBox";
            showPasswordCheckBox.Size = new Size(22, 21);
            showPasswordCheckBox.TabIndex = 5;
            showPasswordCheckBox.UseVisualStyleBackColor = true;
            // 
            // showConfirmPasswordCheckBox
            // 
            showConfirmPasswordCheckBox.AutoSize = true;
            showConfirmPasswordCheckBox.Location = new Point(830, 726);
            showConfirmPasswordCheckBox.Margin = new Padding(4, 5, 4, 5);
            showConfirmPasswordCheckBox.Name = "showConfirmPasswordCheckBox";
            showConfirmPasswordCheckBox.Size = new Size(22, 21);
            showConfirmPasswordCheckBox.TabIndex = 7;
            showConfirmPasswordCheckBox.UseVisualStyleBackColor = true;
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoValidate = AutoValidate.EnablePreventFocusChange;
            BackColor = Color.RoyalBlue;
            ClientSize = new Size(1330, 1064);
            Controls.Add(authorizationLink);
            Controls.Add(registerButton);
            Controls.Add(showConfirmPasswordCheckBox);
            Controls.Add(confirmPasswordTextBox);
            Controls.Add(showPasswordCheckBox);
            Controls.Add(passwordTextBox);
            Controls.Add(emailTextBox);
            Controls.Add(loginTextBox);
            Controls.Add(titleLabel);
            Controls.Add(languageComboBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "RegisterForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Регистрация";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}

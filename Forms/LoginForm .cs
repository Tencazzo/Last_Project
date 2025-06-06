using System;
using System.Drawing;
using System.Windows.Forms;
using Project3.Services;
using Project3.Infrastructure;

namespace Project3.Forms
{
    public partial class LoginForm : Form
    {
        private readonly ILocalizationService _localizationService;
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public LoginForm(ILocalizationService localizationService, IUserService userService, ILogger logger)
        {
            _localizationService = localizationService;
            _userService = userService;
            _logger = logger;

            InitializeComponent();
            InitializeLanguageComboBox();
            SetupEventHandlers();
            UpdateLocalization();
        }

        private void InitializeLanguageComboBox()
        {
            languageComboBox?.Items.Add("Русский");
            languageComboBox?.Items.Add("English");
            if (languageComboBox != null)
                languageComboBox.SelectedIndex = 0;
        }

        private void SetupEventHandlers()
        {
            if (languageComboBox != null)
                languageComboBox.SelectedIndexChanged += LanguageComboBox_SelectedIndexChanged;
            if (loginTextBox != null)
            {
                loginTextBox.Enter += LoginTextBox_Enter;
                loginTextBox.Leave += LoginTextBox_Leave;
            }
            if (passwordTextBox != null)
            {
                passwordTextBox.Enter += PasswordTextBox_Enter;
                passwordTextBox.Leave += PasswordTextBox_Leave;
            }
            if (showPasswordCheckBox != null)
                showPasswordCheckBox.CheckedChanged += ShowPasswordCheckBox_CheckedChanged;
            if (loginButton != null)
                loginButton.Click += LoginButton_Click;
            if (createAccountLink != null)
                createAccountLink.LinkClicked += CreateAccountLink_LinkClicked;
        }

        private void LanguageComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (languageComboBox != null)
            {
                string language = languageComboBox.SelectedIndex == 0 ? "ru" : "en";
                _localizationService.SetLanguage(language);
                UpdateLocalization();
            }
        }

        private void UpdateLocalization()
        {
            if (titleLabel != null)
                titleLabel.Text = _localizationService.GetString("Authorization");

            if (loginTextBox?.ForeColor == Color.Gray)
                loginTextBox.Text = _localizationService.GetString("Login");

            if (passwordTextBox?.ForeColor == Color.Gray)
                passwordTextBox.Text = _localizationService.GetString("Password");

            if (loginButton != null)
                loginButton.Text = _localizationService.GetString("Enter");
            if (createAccountLink != null)
                createAccountLink.Text = _localizationService.GetString("CreateAccount");
            if (showPasswordLabel != null)
                showPasswordLabel.Text = _localizationService.GetString("ShowPassword");
        }

        private void LoginTextBox_Enter(object? sender, EventArgs e)
        {
            if (loginTextBox?.ForeColor == Color.Gray)
            {
                loginTextBox.Text = "";
                loginTextBox.ForeColor = Color.Black;
            }
        }

        private void LoginTextBox_Leave(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(loginTextBox?.Text))
            {
                if (loginTextBox != null)
                {
                    loginTextBox.Text = _localizationService.GetString("Login");
                    loginTextBox.ForeColor = Color.Gray;
                }
            }
        }

        private void PasswordTextBox_Enter(object? sender, EventArgs e)
        {
            if (passwordTextBox?.ForeColor == Color.Gray)
            {
                passwordTextBox.Text = "";
                passwordTextBox.ForeColor = Color.Black;
                passwordTextBox.UseSystemPasswordChar = true;
            }
        }

        private void PasswordTextBox_Leave(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(passwordTextBox?.Text))
            {
                if (passwordTextBox != null)
                {
                    passwordTextBox.Text = _localizationService.GetString("Password");
                    passwordTextBox.ForeColor = Color.Gray;
                    passwordTextBox.UseSystemPasswordChar = false;
                }
            }
        }

        private void ShowPasswordCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (passwordTextBox?.ForeColor != Color.Gray && passwordTextBox != null && showPasswordCheckBox != null)
            {
                passwordTextBox.UseSystemPasswordChar = !showPasswordCheckBox.Checked;
            }
        }

        private void LoginButton_Click(object? sender, EventArgs e)
        {
            if (loginTextBox?.ForeColor == Color.Gray ||
                passwordTextBox?.ForeColor == Color.Gray)
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (loginTextBox != null && passwordTextBox != null &&
                _userService.Login(loginTextBox.Text, passwordTextBox.Text))
            {
                _logger.LogInfo($"Successful login for user: {loginTextBox.Text}");

                var mainForm = DIContainer.Resolve<MainForm>();
                mainForm.Show();
                this.Hide(); // Change from Close() to Hide()
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка авторизации",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateAccountLink_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            var registerForm = DIContainer.Resolve<RegisterForm>();
            registerForm.Show();
            this.Hide(); // Change from Close() to Hide()
        }
    }
}

using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Project3.Infrastructure;
using Project3.Models;
using Project3.Services;

namespace Project3.Forms
{
    public partial class RegisterForm : Form
    {
        private readonly ILocalizationService _localizationService;
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public RegisterForm(ILocalizationService localizationService, IUserService userService, ILogger logger)
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
                languageComboBox.SelectedIndex = _localizationService.CurrentLanguage == "ru" ? 0 : 1;
        }

        private void SetupEventHandlers()
        {
            if (languageComboBox != null)
                languageComboBox.SelectedIndexChanged += LanguageComboBox_SelectedIndexChanged;
            if (loginTextBox != null)
            {
                loginTextBox.Enter += (s, e) => HandleTextBoxEnter(loginTextBox, "Login");
                loginTextBox.Leave += (s, e) => HandleTextBoxLeave(loginTextBox, "Login");
            }
            if (emailTextBox != null)
            {
                emailTextBox.Enter += (s, e) => HandleTextBoxEnter(emailTextBox, "Email");
                emailTextBox.Leave += (s, e) => HandleTextBoxLeave(emailTextBox, "Email");
            }
            if (passwordTextBox != null)
            {
                passwordTextBox.Enter += (s, e) => HandlePasswordEnter(passwordTextBox, "Password");
                passwordTextBox.Leave += (s, e) => HandlePasswordLeave(passwordTextBox, "Password");
            }
            if (confirmPasswordTextBox != null)
            {
                confirmPasswordTextBox.Enter += (s, e) => HandlePasswordEnter(confirmPasswordTextBox, "RepeatPassword");
                confirmPasswordTextBox.Leave += (s, e) => HandlePasswordLeave(confirmPasswordTextBox, "RepeatPassword");
            }
            if (showPasswordCheckBox != null)
                showPasswordCheckBox.CheckedChanged += (s, e) => TogglePasswordVisibility(passwordTextBox, showPasswordCheckBox);
            if (showConfirmPasswordCheckBox != null)
                showConfirmPasswordCheckBox.CheckedChanged += (s, e) => TogglePasswordVisibility(confirmPasswordTextBox, showConfirmPasswordCheckBox);
            if (registerButton != null)
                registerButton.Click += RegisterButton_Click;
            if (authorizationLink != null)
                authorizationLink.LinkClicked += AuthorizationLink_LinkClicked;
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
                titleLabel.Text = _localizationService.GetString("Registration");
            this.Text = _localizationService.GetString("RegisterFormTitle");

            if (loginTextBox?.ForeColor == Color.Gray)
                loginTextBox.Text = _localizationService.GetString("Login");

            if (emailTextBox?.ForeColor == Color.Gray)
                emailTextBox.Text = _localizationService.GetString("Email");

            if (passwordTextBox?.ForeColor == Color.Gray)
                passwordTextBox.Text = _localizationService.GetString("Password");

            if (confirmPasswordTextBox?.ForeColor == Color.Gray)
                confirmPasswordTextBox.Text = _localizationService.GetString("RepeatPassword");

            if (registerButton != null)
                registerButton.Text = _localizationService.GetString("Enter");
            if (authorizationLink != null)
                authorizationLink.Text = _localizationService.GetString("Authorization");
        }

        private void HandleTextBoxEnter(TextBox? textBox, string key)
        {
            if (textBox?.ForeColor == Color.Gray)
            {
                textBox.Text = "";
                textBox.ForeColor = Color.Black;
            }
        }

        private void HandleTextBoxLeave(TextBox? textBox, string key)
        {
            if (string.IsNullOrWhiteSpace(textBox?.Text))
            {
                if (textBox != null)
                {
                    textBox.Text = _localizationService.GetString(key);
                    textBox.ForeColor = Color.Gray;
                }
            }
        }

        private void HandlePasswordEnter(TextBox? textBox, string key)
        {
            if (textBox?.ForeColor == Color.Gray)
            {
                textBox.Text = "";
                textBox.ForeColor = Color.Black;
                textBox.UseSystemPasswordChar = true;
            }
        }

        private void HandlePasswordLeave(TextBox? textBox, string key)
        {
            if (string.IsNullOrWhiteSpace(textBox?.Text))
            {
                if (textBox != null)
                {
                    textBox.Text = _localizationService.GetString(key);
                    textBox.ForeColor = Color.Gray;
                    textBox.UseSystemPasswordChar = false;
                }
            }
        }

        private void TogglePasswordVisibility(TextBox? textBox, CheckBox? checkBox)
        {
            if (textBox?.ForeColor != Color.Gray && textBox != null && checkBox != null)
            {
                textBox.UseSystemPasswordChar = !checkBox.Checked;
            }
        }

        private void RegisterButton_Click(object? sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            var user = new User
            {
                Login = loginTextBox?.Text ?? "",
                Email = emailTextBox?.Text ?? "",
                Password = passwordTextBox?.Text ?? ""
            };

            if (_userService.Register(user))
            {
                _logger.LogInfo($"User {user.Login} registered successfully");
                MessageBox.Show("Регистрация прошла успешно!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                var loginForm = DIContainer.Resolve<LoginForm>();
                loginForm.Show();
                this.Hide(); 
            }
            else
            {
                MessageBox.Show("Ошибка при регистрации. Возможно, пользователь уже существует.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateForm()
        {
            if (loginTextBox?.ForeColor == Color.Gray ||
                emailTextBox?.ForeColor == Color.Gray ||
                passwordTextBox?.ForeColor == Color.Gray ||
                confirmPasswordTextBox?.ForeColor == Color.Gray)
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!IsValidEmail(emailTextBox?.Text ?? ""))
            {
                MessageBox.Show("Введите корректный email адрес", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (passwordTextBox?.Text != confirmPasswordTextBox?.Text)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if ((passwordTextBox?.Text?.Length ?? 0) < 6)
            {
                MessageBox.Show("Пароль должен содержать минимум 6 символов", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        private void AuthorizationLink_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            var loginForm = DIContainer.Resolve<LoginForm>();
            loginForm.Show();
            this.Hide(); 
        }
    }
}

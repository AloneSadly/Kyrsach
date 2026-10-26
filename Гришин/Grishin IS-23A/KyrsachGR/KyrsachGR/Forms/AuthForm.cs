using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KyrsachGR
{
    public partial class AuthForm : Form
    {
        private CaptchaGenerator captchaGenerator = new CaptchaGenerator();
        private int failedAttempts = 0;
        private bool isPasswordVisible = false;

        // ============================================================
        // ХАРДКОД-ПОЛЬЗОВАТЕЛИ (для тестирования без БД)
        // Формат: логин -> (пароль, роль)
        // ============================================================
        private readonly Dictionary<string, (string Password, string Role)> testUsers =
            new Dictionary<string, (string, string)>(StringComparer.OrdinalIgnoreCase)
        {
            { "admin",   ("admin",   "admin")   },
            { "manager", ("manager", "manager") },
            { "user",    ("user",    "seller")  }  // "user" привязан к роли "seller"
        };

        public AuthForm()
        {
            InitializeComponent();
        }

        private void AuthForm_Load(object sender, EventArgs e)
        {
            try
            {
                txtPassword.PasswordChar = '*';

                // 1. Добавляем тестовых пользователей в ComboBox
                comboBoxLogin.Items.Clear();
                comboBoxLogin.Items.AddRange(new object[] { "admin", "manager", "user" });

                // 2. Пытаемся загрузить реальных пользователей из БД (если БД доступна)
                try
                {
                    string[] dbLogins = DbManager.GetLogins();
                    foreach (string login in dbLogins)
                    {
                        // Не добавляем дубликаты
                        if (!comboBoxLogin.Items.Contains(login))
                        {
                            comboBoxLogin.Items.Add(login);
                        }
                    }
                }
                catch
                {
                    // Если БД недоступна, просто игнорируем ошибку
                    // (тестовые пользователи уже добавлены)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBoxLogin.Text))
            {
                MessageBox.Show("Выберите логин!");
                return;
            }

            // Проверка капчи
            if (pbCaptcha.Visible)
            {
                if (txtCaptcha.Text.Trim().ToUpper() != captchaGenerator.Text)
                {
                    MessageBox.Show("Неверный код с картинки!");
                    RefreshCaptcha();
                    return;
                }
            }

            string login = comboBoxLogin.Text.Trim();
            string password = txtPassword.Text;

            // ============================================================
            // 1. СНАЧАЛА ПРОВЕРЯЕМ ХАРДКОД-ПОЛЬЗОВАТЕЛЕЙ
            // ============================================================
            if (testUsers.ContainsKey(login))
            {
                var testUser = testUsers[login];
                if (testUser.Password == password)
                {
                    // Успех через хардкод
                    OpenFormByRole(testUser.Role, login);
                    return;
                }
                else
                {
                    // Неверный пароль для тестового пользователя
                    HandleFailedLogin();
                    return;
                }
            }

            // ============================================================
            // 2. ЕСЛИ ЛОГИН НЕ НАЙДЕН В ХАРДКОДЕ - ИЩЕМ В БД
            // ============================================================
            try
            {
                string role = DbManager.Authenticate(login, password);

                if (role != null)
                {
                    OpenFormByRole(role, login);
                }
                else
                {
                    HandleFailedLogin();
                }
            }
            catch (Exception ex)
            {
                // Если БД недоступна, показываем понятное сообщение
                MessageBox.Show("Ошибка подключения к БД: " + ex.Message +
                    "\n\nПопробуйте войти как:\n" +
                    "admin / admin\n" +
                    "manager / manager\n" +
                    "user / user");
            }
        }

        private void OpenFormByRole(string role, string login) // Добавили параметр login
        {
            Form nextForm = null;

            switch (role.ToLower())
            {
                case "admin":
                    nextForm = new WelcomeAdmForm(login); // Передаем логин
                    break;
                case "manager":
                    nextForm = new WelcomeMerchForm(login); // Передаем логин
                    break;
                case "seller":
                case "user":
                    nextForm = new WelcomeUserForm(login); // Передаем логин
                    break;
                default:
                    MessageBox.Show($"Неизвестная роль: {role}. Обратитесь к администратору.");
                    return;
            }

            this.Hide();
            nextForm.ShowDialog();
            this.Close();
        }

        private void HandleFailedLogin()
        {
            failedAttempts++;
            MessageBox.Show($"Неверный логин или пароль. Попытка {failedAttempts}");

            if (failedAttempts == 1)
            {
                ShowCaptcha(true);
                RefreshCaptcha();
            }
            else if (failedAttempts >= 2)
            {
                BlockInterface();
            }
        }

        private void BlockInterface()
        {
            MessageBox.Show("Слишком много неудачных попыток. Подождите 10 секунд.");

            comboBoxLogin.Enabled = false;
            txtPassword.Enabled = false;
            btnLogin.Enabled = false;
            btnExit.Enabled = false;
            txtCaptcha.Enabled = false;
            btnRefreshCaptcha.Enabled = false;

            timerBlock.Start();
        }

        private void timerBlock_Tick(object sender, EventArgs e)
        {
            timerBlock.Stop();

            comboBoxLogin.Enabled = true;
            txtPassword.Enabled = true;
            btnLogin.Enabled = true;
            btnExit.Enabled = true;
            txtCaptcha.Enabled = true;
            btnRefreshCaptcha.Enabled = true;

            RefreshCaptcha();
        }

        private void ShowCaptcha(bool show)
        {
            pbCaptcha.Visible = show;
            txtCaptcha.Visible = show;
            lblCaptcha.Visible = show;
            btnRefreshCaptcha.Visible = show;
            txtCaptcha.Text = "";
        }

        private void RefreshCaptcha()
        {
            pbCaptcha.Image = captchaGenerator.Generate(pbCaptcha.Width, pbCaptcha.Height);
            txtCaptcha.Text = "";
            txtCaptcha.Focus();
        }

        private void btnRefreshCaptcha_Click(object sender, EventArgs e)
        {
            RefreshCaptcha();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btnTogglePass_Click(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                txtPassword.PasswordChar = '\0';
                btnTogglePass.Text = "🙈"; // Закрытый глаз
            }
            else
            {
                txtPassword.PasswordChar = '*';
                btnTogglePass.Text = "👁"; // Открытый глаз
            }
        }
    }
}
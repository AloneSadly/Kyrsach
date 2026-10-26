using KyrsachGR.Scripts;
using System;
using System.Data;
using System.Windows.Forms;

namespace KyrsachGR
{
    public partial class WelcomeAdmForm : Form
    {
        private string currentLogin;

        // Измененный конструктор
        public WelcomeAdmForm(string login)
        {
            InitializeComponent();
            this.currentLogin = login;
            LoadUserData();
        }

        private void LoadUserData()
        {
            try
            {
                DataTable dt = DbManager.GetUserInfo(currentLogin);
                if (dt.Rows.Count > 0)
                {
                    string fullName = dt.Rows[0]["full_name"].ToString();
                    string phone = dt.Rows[0]["phone"].ToString();

                    // Применяем форматирование
                    label1.Text = $"Добро пожаловать, \"{FormatHelper.ShortenName(fullName)}\"!";
                    label3.Text = $"Номер телефона: {FormatHelper.MaskPhone(phone)}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            AccountingForm acc = new AccountingForm();
            acc.ShowDialog();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            AllOrdersForm allOrders = new AllOrdersForm();
            allOrders.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            ReferenceForm referenceForm = new ReferenceForm();
            referenceForm.ShowDialog();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            UsersForm usersForm = new UsersForm();
            usersForm.ShowDialog();
        }
        private void WelcomeAdmForm_FormClosed(object sender, FormClosedEventArgs e) { }
    }
}
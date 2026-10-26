using KyrsachGR.Scripts;
using System;
using System.Data;
using System.Windows.Forms;

namespace KyrsachGR
{
    public partial class WelcomeMerchForm : Form
    {
        private string currentLogin;

        public WelcomeMerchForm(string login)
        {
            InitializeComponent();
            this.currentLogin = login;
        }

        private void WelcomeMerchForm_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = DbManager.GetUserInfo(currentLogin);
                if (dt.Rows.Count > 0)
                {
                    string fullName = dt.Rows[0]["full_name"].ToString();
                    string phone = dt.Rows[0]["phone"].ToString();

                    label1.Text = $"Добро пожаловать, \"{FormatHelper.ShortenName(fullName)}\"!";
                    label3.Text = $"Номер телефона: {FormatHelper.MaskPhone(phone)}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AccountingForm acc = new AccountingForm();
            acc.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0); 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AllOrdersForm allOrdersForm = new AllOrdersForm();
            allOrdersForm.Show();
        }
    }
}
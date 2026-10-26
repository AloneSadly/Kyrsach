using KyrsachGR.Scripts;
using System;
using System.Data;
using System.Windows.Forms;

namespace KyrsachGR
{
    public partial class WelcomeUserForm : Form
    {
        private string currentLogin;

        public WelcomeUserForm(string login)
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
        private void button4_Click(object sender, EventArgs e)
        {
            Order_create inv = new Order_create();
            inv.Show();
        }
    }
}
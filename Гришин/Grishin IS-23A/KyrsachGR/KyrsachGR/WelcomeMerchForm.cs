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
    public partial class WelcomeMerchForm : Form
    {
        public WelcomeMerchForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AccountingForm acc = new AccountingForm();
            acc.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AuthForm auth = new AuthForm();
            this.Close();
            auth.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AllOrdersForm allOrdersForm = new AllOrdersForm();
            allOrdersForm.Show();
        }

        private void WelcomeMerchForm_Load(object sender, EventArgs e)
        {

        }
    }
}

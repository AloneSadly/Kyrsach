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
    public partial class WelcomeUserForm : Form
    {
        public WelcomeUserForm()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            AuthForm auth = new AuthForm();
            this.Close();
            auth.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Order_create inv = new Order_create();
            inv.Show();
        }
    }
}

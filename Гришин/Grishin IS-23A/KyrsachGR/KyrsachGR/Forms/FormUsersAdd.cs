using System;
using System.Windows.Forms;

namespace KyrsachGR
{
    public partial class FormUsersAdd : Form
    {
        public FormUsersAdd()
        {
            InitializeComponent();
        }

        private void FormUsersAdd_Load(object sender, EventArgs e)
        {
            cmbRole.Items.Clear();
            // Важно: роли должны строго совпадать с ENUM в БД!
            cmbRole.Items.AddRange(new object[] { "admin", "manager", "seller" });
            cmbRole.SelectedIndex = 2; // По умолчанию seller
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Заполните логин, пароль и роль!");
                return;
            }

            try
            {
                DbManager.AddUser(
                    txtLogin.Text,
                    txtFullName.Text,
                    txtEmail.Text,
                    txtPhone.Text,
                    txtPassword.Text,
                    cmbRole.SelectedItem.ToString()
                );

                MessageBox.Show("Сотрудник успешно добавлен!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
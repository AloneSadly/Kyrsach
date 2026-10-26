using System;
using System.Data;
using System.Windows.Forms;

namespace KyrsachGR
{
    public partial class FormUsersEdit : Form
    {
        private int currentUserId = 0;

        // Конструктор с ID
        public FormUsersEdit(int userId)
        {
            InitializeComponent();
            this.currentUserId = userId;

            // Заполняем ComboBox ролями сразу
            cmbRole.Items.Clear();
            cmbRole.Items.AddRange(new object[] { "admin", "manager", "seller" });

            // ВЫЗЫВАЕМ ЗАГРУЗКУ ПРЯМО ЗДЕСЬ (самый надежный способ)
            LoadUserData();
        }

        // Пустой конструктор (на всякий случай)
        public FormUsersEdit()
        {
            InitializeComponent();
        }

        private void LoadUserData()
        {
            if (currentUserId <= 0) return;

            try
            {
                DataTable dt = DbManager.GetUserById(currentUserId);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    txtLogin.Text = row["login"].ToString();
                    txtFullName.Text = row["full_name"].ToString();
                    txtEmail.Text = row["email"].ToString();
                    txtPhone.Text = row["phone"].ToString();
                    txtPassword.Text = row["pass"].ToString();

                    // Устанавливаем роль
                    string role = row["role"].ToString();
                    if (cmbRole.Items.Contains(role))
                    {
                        cmbRole.SelectedItem = role;
                    }
                    else
                    {
                        cmbRole.SelectedIndex = 2; // seller
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        // Кнопка Сохранить
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (currentUserId == 0) return;

            try
            {
                DbManager.UpdateUser(
                    currentUserId,
                    txtFullName.Text,
                    txtEmail.Text,
                    txtPhone.Text,
                    txtLogin.Text,
                    txtPassword.Text,
                    cmbRole.SelectedItem?.ToString() ?? "seller"
                );

                MessageBox.Show("Данные пользователя сохранены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
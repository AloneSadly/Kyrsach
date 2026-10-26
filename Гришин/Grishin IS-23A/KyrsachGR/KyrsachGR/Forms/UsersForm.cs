using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace KyrsachGR
{
    public partial class UsersForm : Form
    {
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem deleteMenuItem;

        public UsersForm()
        {
            InitializeComponent();
            SetupContextMenu(); // Инициализируем меню ПКМ
        }

        private void UsersForm_Load(object sender, EventArgs e)
        {
            SetupDataGridView();

            cmbProduct.Items.Clear();
            cmbProduct.Items.AddRange(new object[] { "Все", "admin", "manager", "seller" });
            cmbProduct.SelectedIndex = 0;

            LoadUsers();
        }

        private void SetupDataGridView()
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.MultiSelect = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.ColumnAdded += (s, ev) =>
            {
                if (ev.Column.Name == "user_id" || ev.Column.Name == "pass")
                    ev.Column.Visible = false;
            };
        }

        // ============================================================
        // НАСТРОЙКА ПКМ (УДАЛЕНИЕ)
        // ============================================================
        private void SetupContextMenu()
        {
            contextMenu = new ContextMenuStrip();
            deleteMenuItem = new ToolStripMenuItem("Удалить пользователя");
            deleteMenuItem.Click += DeleteMenuItem_Click;
            contextMenu.Items.Add(deleteMenuItem);

            dataGridView1.ContextMenuStrip = contextMenu;
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            DataGridViewRow row = dataGridView1.SelectedRows[0];
            int userId = Convert.ToInt32(row.Cells["user_id"].Value);
            string userName = row.Cells["full_name"].Value.ToString();
            string userLogin = row.Cells["login"].Value.ToString();

            // Запрещаем удалять самого себя (если в системе есть текущий пользователь)
            // Если у вас нет глобальной переменной текущего юзера, можно захардкодить "admin"
            if (userLogin.ToLower() == "admin")
            {
                MessageBox.Show("Нельзя удалить главного администратора!", "Запрещено", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить пользователя '{userName}'?\nЭто действие нельзя отменить.",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DbManager.DeleteUser(userId);
                    MessageBox.Show("Пользователь удален!");
                    LoadUsers(); // Обновляем таблицу
                }
                catch (Exception ex)
                {
                    // Если пользователь связан с заказами (Foreign Key), вылетит ошибка
                    if (ex.Message.Contains("foreign key"))
                        MessageBox.Show("Невозможно удалить пользователя, так как он имеет связанные заказы (чеки) в системе!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        // ============================================================
        // ЗАГРУЗКА И ФИЛЬТРЫ
        // ============================================================
        private void LoadUsers()
        {
            try
            {
                string search = txtSearch.Text.Trim();
                string role = cmbProduct.SelectedItem?.ToString() ?? "Все";

                DataTable dt = DbManager.GetUsers(search, role);
                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns["full_name"] != null) dataGridView1.Columns["full_name"].HeaderText = "ФИО";
                if (dataGridView1.Columns["email"] != null) dataGridView1.Columns["email"].HeaderText = "Email";
                if (dataGridView1.Columns["phone"] != null) dataGridView1.Columns["phone"].HeaderText = "Телефон";
                if (dataGridView1.Columns["login"] != null) dataGridView1.Columns["login"].HeaderText = "Логин";
                if (dataGridView1.Columns["role"] != null) dataGridView1.Columns["role"].HeaderText = "Роль";

                ApplyConditionalFormatting();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ApplyConditionalFormatting()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["role"].Value == null) continue;
                string role = row.Cells["role"].Value.ToString().ToLower();

                switch (role)
                {
                    case "admin":
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                        row.DefaultCellStyle.ForeColor = Color.White;
                        row.DefaultCellStyle.Font = new Font("Verdana", 9.75F, FontStyle.Bold);
                        break;
                    case "manager":
                        row.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                        break;
                    case "seller":
                        row.DefaultCellStyle.BackColor = Color.LightCyan;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                        break;
                    default:
                        row.DefaultCellStyle.BackColor = Color.White;
                        break;
                }
            }
        }

        // Поиск
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadUsers();
        }

        // Сортировка/Фильтр по роли
        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUsers();
        }

        // ============================================================
        // КНОПКИ
        // ============================================================
        private void button1_Click(object sender, EventArgs e) // ДОБАВИТЬ
        {
            using (FormUsersAdd addForm = new FormUsersAdd())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя!");
                return;
            }

            // Берем ID из скрытой колонки user_id
            int userId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["user_id"].Value);

            // ПЕРЕДАЕМ ID В КОНСТРУКТОР!
            using (FormUsersEdit editForm = new FormUsersEdit(userId))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers(); // Обновить таблицу после сохранения
                }
            }
        }

        private void button3_Click(object sender, EventArgs e) // ВЫХОД
        {
            this.Close();
        }
    }
}
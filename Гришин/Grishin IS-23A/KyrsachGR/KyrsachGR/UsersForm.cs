using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace KyrsachGR
{
    public partial class UsersForm : Form
    {
        public UsersForm()
        {
            InitializeComponent();
        }

        private void UsersForm_Load(object sender, EventArgs e)
        {
            // Настройка таблицы
            SetupDataGridView();

            // Заполняем ComboBox ролей
            cmbProduct.Items.Clear();
            cmbProduct.Items.AddRange(new object[] { "Все", "admin", "manager", "seller" });
            cmbProduct.SelectedIndex = 0; // "Все"

            // Загружаем данные
            LoadUsers();
        }

        /// <summary>
        /// Настройка внешнего вида таблицы
        /// </summary>
        private void SetupDataGridView()
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.MultiSelect = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Скрываем лишние колонки
            dataGridView1.ColumnAdded += (s, ev) =>
            {
                if (ev.Column.Name == "user_id" || ev.Column.Name == "pass")
                    ev.Column.Visible = false;
            };
        }

        /// <summary>
        /// Загрузка пользователей с учетом поиска и фильтра по роли
        /// </summary>
        private void LoadUsers()
        {
            try
            {
                string search = txtSearch.Text.Trim();
                string role = cmbProduct.SelectedItem?.ToString() ?? "Все";

                DataTable dt = DbManager.GetUsers(search, role);
                dataGridView1.DataSource = dt;

                // Красивые заголовки для колонок
                if (dataGridView1.Columns["full_name"] != null)
                    dataGridView1.Columns["full_name"].HeaderText = "ФИО";
                if (dataGridView1.Columns["email"] != null)
                    dataGridView1.Columns["email"].HeaderText = "Email";
                if (dataGridView1.Columns["phone"] != null)
                    dataGridView1.Columns["phone"].HeaderText = "Телефон";
                if (dataGridView1.Columns["login"] != null)
                    dataGridView1.Columns["login"].HeaderText = "Логин";
                if (dataGridView1.Columns["role"] != null)
                    dataGridView1.Columns["role"].HeaderText = "Роль";

                // Применяем условное форматирование
                ApplyConditionalFormatting();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Условное форматирование по ролям
        /// </summary>
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

        // ============================================================
        // ПОИСК И ФИЛЬТРЫ
        // ============================================================

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUsers();
        }

        // ============================================================
        // КНОПКИ
        // ============================================================

        /// <summary>
        /// Кнопка "Добавить"
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            // Открываем форму AuthForm? Нет, нужно что-то другое.
            // Вариант 1: Открыть форму редактирования пользователя (если создадите UserEditForm).
            // Вариант 2: Показать сообщение, что добавление через эту форму недоступно.

            MessageBox.Show("Для добавления пользователя откройте форму редактирования (UserEditForm).",
                "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Если у вас есть UserEditForm:
            // var editForm = new UserEditForm(0); // 0 = новый пользователь
            // if (editForm.ShowDialog() == DialogResult.OK) LoadUsers();
        }

        /// <summary>
        /// Кнопка "Изменить"
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя!");
                return;
            }

            int userId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["user_id"].Value);

            MessageBox.Show($"Выбран пользователь с ID = {userId}.",
                "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Если есть UserEditForm:
            // var editForm = new UserEditForm(userId);
            // if (editForm.ShowDialog() == DialogResult.OK) LoadUsers();
        }

        /// <summary>
        /// Кнопка "Выход"
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
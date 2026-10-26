using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace KyrsachGR
{
    public partial class AccountingForm : Form
    {
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalPages = 0;

        // Для контекстного меню (удаления)
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem deleteMenuItem;

        public AccountingForm()
        {
            InitializeComponent();
            SetupContextMenu(); // Инициализируем ПКМ
        }

        private void AccountingForm_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.MultiSelect = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new object[] { "По дате", "По имени", "По цене" });
            comboBox1.SelectedIndex = 0;

            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(new object[] { "Все", "В наличии", "Нет в наличии" });
            comboBox2.SelectedIndex = 0;

            LoadProducts();
        }

        // ============================================================
        // НАСТРОЙКА ПКМ (УДАЛЕНИЕ)
        // ============================================================
        private void SetupContextMenu()
        {
            contextMenu = new ContextMenuStrip();
            deleteMenuItem = new ToolStripMenuItem("Удалить товар");
            deleteMenuItem.Click += DeleteMenuItem_Click;
            contextMenu.Items.Add(deleteMenuItem);

            dataGridView1.ContextMenuStrip = contextMenu;
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            DataGridViewRow row = dataGridView1.SelectedRows[0];
            int productId = Convert.ToInt32(row.Cells["id"].Value);
            string productName = row.Cells["name"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить товар '{productName}'?\nЭто действие нельзя отменить.",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DbManager.DeleteProduct(productId);
                    MessageBox.Show("Товар успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts(); // Обновляем таблицу
                }
                catch (Exception ex)
                {
                    // Если товар уже продан (есть в чеке), БД выдаст ошибку внешнего ключа
                    if (ex.Message.Contains("foreign key"))
                        MessageBox.Show("Невозможно удалить товар, так как он присутствует в оформленных заказах (чеках)!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Ошибка при удалении: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ============================================================
        // ЗАГРУЗКА И ОТОБРАЖЕНИЕ
        // ============================================================
        private void LoadProducts()
        {
            try
            {
                string search = textBox2.Text.Trim();
                string status = comboBox2.SelectedItem?.ToString() ?? "Все";
                string sort = comboBox1.SelectedItem?.ToString() ?? "По дате";

                int totalRecords = DbManager.GetProductsCount(search, status);
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                if (totalPages < 1) totalPages = 1;
                if (currentPage > totalPages) currentPage = totalPages;

                DataTable dt = DbManager.GetProducts(currentPage, pageSize, search, status, sort);
                dataGridView1.DataSource = dt;

                // Скрываем технические колонки ID
                if (dataGridView1.Columns["id"] != null) dataGridView1.Columns["id"].Visible = false;
                if (dataGridView1.Columns["color_color_id"] != null) dataGridView1.Columns["color_color_id"].Visible = false;
                if (dataGridView1.Columns["country_country_id"] != null) dataGridView1.Columns["country_country_id"].Visible = false;
                // Скрываем колонку "category", если она дублируется с категориями
                if (dataGridView1.Columns["category"] != null) dataGridView1.Columns["category"].Visible = false;

                SetHeader("name", "Название");
                SetHeader("article", "Артикул");
                SetHeader("price", "Цена");
                SetHeader("stock_quantity", "Остаток");
                SetHeader("color", "Цвет");
                SetHeader("country", "Страна");

                ApplyConditionalFormatting();
                UpdatePagination();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetHeader(string col, string header)
        {
            if (dataGridView1.Columns[col] != null)
                dataGridView1.Columns[col].HeaderText = header;
        }

        private void ApplyConditionalFormatting()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["stock_quantity"].Value == null) continue;
                int stock = Convert.ToInt32(row.Cells["stock_quantity"].Value);

                if (stock == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                else if (stock < 10)
                {
                    row.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void UpdatePagination()
        {
            Button[] buttons = { button8, button9, button10, button11, button12 };

            for (int i = 0; i < buttons.Length; i++)
            {
                int pageNum = i + 1;

                if (pageNum <= totalPages)
                {
                    buttons[i].Text = pageNum.ToString();
                    buttons[i].Visible = true;
                    buttons[i].Enabled = pageNum != currentPage;
                    buttons[i].BackColor = (pageNum == currentPage) ? Color.LightBlue : SystemColors.Control;
                }
                else
                {
                    buttons[i].Visible = false;
                }
            }
        }

        // --- ФИЛЬТРЫ ---
        private void textBox2_TextChanged(object sender, EventArgs e) { currentPage = 1; LoadProducts(); }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { currentPage = 1; LoadProducts(); }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { LoadProducts(); }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            comboBox2.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;
            dateTimePicker1.Value = DateTime.Now;
            currentPage = 1;
            LoadProducts();
        }

        // --- ПАГИНАЦИЯ ---
        private void button8_Click(object sender, EventArgs e) { currentPage = 1; LoadProducts(); }
        private void button9_Click(object sender, EventArgs e) { currentPage = 2; LoadProducts(); }
        private void button10_Click(object sender, EventArgs e) { currentPage = 3; LoadProducts(); }
        private void button11_Click(object sender, EventArgs e) { currentPage = 4; LoadProducts(); }
        private void button12_Click(object sender, EventArgs e) { currentPage = 5; LoadProducts(); }

        // --- ДЕЙСТВИЯ ---
        private void button1_Click(object sender, EventArgs e) { this.Close(); }

        // ДОБАВЛЕНИЕ ТОВАРА
        private void button13_Click(object sender, EventArgs e)
        {
            using (AddOrderForm addForm = new AddOrderForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }

        // ИЗМЕНЕНИЕ ТОВАРА
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для изменения!");
                return;
            }

            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);

            using (FormProductsEdit editForm = new FormProductsEdit(id))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }

        // ЭКСПОРТ CSV
        private void button15_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = "Товары_" + DateTime.Now.ToString("yyyy-MM-dd") + ".csv"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExportToCsv(dataGridView1, sfd.FileName);
                    MessageBox.Show("Экспорт выполнен успешно!");
                }
                catch (Exception ex) { MessageBox.Show("Ошибка экспорта: " + ex.Message); }
            }
        }

        // ИМПОРТ CSV
        private void button14_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "CSV files (*.csv)|*.csv" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string[] lines = File.ReadAllLines(ofd.FileName, Encoding.UTF8);
                    int importedCount = 0;

                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] parts = lines[i].Split(';');
                        if (parts.Length >= 3)
                        {
                            string name = parts[0];
                            string article = parts[1];
                            decimal price = Convert.ToDecimal(parts[2]);
                            int stock = parts.Length > 3 ? Convert.ToInt32(parts[3]) : 0;

                            DbManager.AddProduct(name, "", article, "", 0, 0, price, stock, 1, 1, 1, 1, 1);
                            importedCount++;
                        }
                    }
                    MessageBox.Show($"Импортировано записей: {importedCount}");
                    LoadProducts();
                }
                catch (Exception ex) { MessageBox.Show("Ошибка импорта: " + ex.Message); }
            }
        }

        private void ExportToCsv(DataGridView dgv, string filePath)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                if (!dgv.Columns[i].Visible) continue;
                sb.Append(dgv.Columns[i].HeaderText);
                if (i < dgv.Columns.Count - 1) sb.Append(";");
            }
            sb.AppendLine();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    if (!dgv.Columns[i].Visible) continue;
                    object value = row.Cells[i].Value;
                    string text = value == null ? "" : value.ToString().Replace(";", ",");
                    sb.Append(text);
                    if (i < dgv.Columns.Count - 1) sb.Append(";");
                }
                sb.AppendLine();
            }
            File.WriteAllText(filePath, sb.ToString(), new UTF8Encoding(true));
        }
    }
}
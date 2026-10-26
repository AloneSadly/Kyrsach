using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace KyrsachGR
{
    public partial class AllOrdersForm : Form
    {
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalPages = 0;

        public AllOrdersForm()
        {
            InitializeComponent();
        }

        private void AllOrdersForm_Load(object sender, EventArgs e)
        {
            SetupDataGridView();

            // Заполняем фильтр статусов
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(new string[] { "Все", "Обычный", "Крупный" });
            comboBox2.SelectedIndex = 0;

            // Заполняем сортировку (если она есть на форме)
            if (comboBox1 != null)
            {
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(new string[] { "По дате", "По сумме", "По клиенту" });
                comboBox1.SelectedIndex = 0;
                comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            }

            LoadOrders();
        }

        private void SetupDataGridView()
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadOrders()
        {
            try
            {
                string search = textBox2.Text.Trim();
                string status = comboBox2.SelectedItem?.ToString() ?? "Все";
                string sort = comboBox1 != null ? comboBox1.SelectedItem?.ToString() ?? "По дате" : "По дате";

                int totalRecords = DbManager.GetOrdersCount(search, status);
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                if (totalPages < 1) totalPages = 1;

                if (currentPage > totalPages) currentPage = totalPages;
                if (currentPage < 1) currentPage = 1;

                DataTable dt = DbManager.GetOrders(currentPage, pageSize, search, status, sort);
                dataGridView1.DataSource = dt;

                ApplyConditionalFormatting();
                UpdatePaginationButtons();
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
                if (row.Cells["Сумма"].Value != null)
                {
                    decimal sum = Convert.ToDecimal(row.Cells["Сумма"].Value);
                    if (sum > 10000)
                    {
                        row.DefaultCellStyle.BackColor = Color.MistyRose;
                        row.DefaultCellStyle.ForeColor = Color.DarkRed;
                    }
                }

                if (row.Cells["Продавец"].Value == DBNull.Value || string.IsNullOrEmpty(row.Cells["Продавец"].Value.ToString()))
                {
                    row.Cells["Продавец"].Style.BackColor = Color.LightGray;
                }
            }
        }

        private void UpdatePaginationButtons()
        {
            Button[] pageButtons = { button8, button9, button10, button11, button12 };

            for (int i = 0; i < pageButtons.Length; i++)
            {
                int pageNum = i + 1;
                if (pageNum <= totalPages)
                {
                    pageButtons[i].Text = pageNum.ToString();
                    pageButtons[i].Visible = true;
                    pageButtons[i].Enabled = (pageNum != currentPage);
                }
                else
                {
                    pageButtons[i].Visible = false;
                }
            }
        }

        // --- ПАГИНАЦИЯ ---
        private void button8_Click(object sender, EventArgs e) { currentPage = 1; LoadOrders(); }
        private void button9_Click(object sender, EventArgs e) { currentPage = 2; LoadOrders(); }
        private void button10_Click(object sender, EventArgs e) { currentPage = 3; LoadOrders(); }
        private void button11_Click(object sender, EventArgs e) { currentPage = 4; LoadOrders(); }
        private void button12_Click(object sender, EventArgs e) { currentPage = 5; LoadOrders(); }

        // --- ФИЛЬТРЫ ---
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadOrders();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadOrders();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadOrders();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            comboBox2.SelectedIndex = 0;
            if (comboBox1 != null) comboBox1.SelectedIndex = 0;
            dateTimePicker1.Value = DateTime.Now;
            currentPage = 1;
            LoadOrders();
        }

        // --- КНОПКИ ДЕЙСТВИЙ ---
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ФОРМИРОВАНИЕ ЧЕКА (ПОДРОБНОЕ С ПОЗИЦИЯМИ)
        private void button14_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заказ из таблицы для формирования чека!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var row = dataGridView1.SelectedRows[0];
                int receiptId = Convert.ToInt32(row.Cells["№ Чека"].Value);
                string date = row.Cells["Дата"].Value?.ToString();
                string seller = row.Cells["Продавец"].Value?.ToString();
                string customer = row.Cells["Клиент"].Value?.ToString();
                decimal total = Convert.ToDecimal(row.Cells["Сумма"].Value);

                // Получаем список товаров в этом чеке
                DataTable items = DbManager.GetOrderItems(receiptId);

                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Text files (*.txt)|*.txt",
                    FileName = $"Чек_№{receiptId}_{DateTime.Now:yyyyMMdd}.txt"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("=========================================");
                    sb.AppendLine("              МАГАЗИН ОБОЕВ              ");
                    sb.AppendLine("=========================================");
                    sb.AppendLine($"Чек №: {receiptId}");
                    sb.AppendLine($"Дата: {date}");
                    sb.AppendLine($"Продавец: {seller}");
                    sb.AppendLine($"Покупатель: {customer}");
                    sb.AppendLine("-----------------------------------------");
                    sb.AppendLine("Товар\t\tКол-во\tЦена\tСумма");
                    sb.AppendLine("-----------------------------------------");

                    foreach (DataRow item in items.Rows)
                    {
                        sb.AppendLine($"{item["Товар"]}\t{item["Кол-во"]}\t{item["Цена"]}\t{item["Сумма"]}");
                    }

                    sb.AppendLine("-----------------------------------------");
                    sb.AppendLine($"ИТОГО К ОПЛАТЕ: {total} руб.");
                    sb.AppendLine("=========================================");
                    sb.AppendLine("       Спасибо за покупку!       ");

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Чек успешно сформирован и сохранен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании чека: " + ex.Message);
            }
        }

        // ФОРМИРОВАНИЕ ОТЧЕТА ЗА ПЕРИОД (РЕАЛЬНЫЙ SQL-ЗАПРОС)
        private void button15_Click(object sender, EventArgs e)
        {
            DateTime startDate = dateTimePicker2.Value.Date;
            DateTime endDate = dateTimePicker3.Value.Date.AddDays(1).AddSeconds(-1); // Конец дня

            if (startDate > endDate)
            {
                MessageBox.Show("Дата 'С' не может быть больше даты 'По'!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Получаем данные за период из БД
                DataTable reportData = DbManager.GetOrdersForReport(startDate, endDate);

                if (reportData.Rows.Count == 0)
                {
                    MessageBox.Show("За выбранный период заказов не найдено.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv",
                    FileName = $"Отчет_с_{startDate:dd.MM.yyyy}_по_{endDate:dd.MM.yyyy}.csv"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"Отчет о продажах за период с {startDate:dd.MM.yyyy} по {dateTimePicker3.Value:dd.MM.yyyy}");
                    sb.AppendLine("--------------------------------------------------");

                    // Заголовки
                    sb.AppendLine("№ Чека;Дата;Продавец;Клиент;Сумма");

                    // Данные
                    foreach (DataRow row in reportData.Rows)
                    {
                        sb.AppendLine($"{row["№ Чека"]};{row["Дата"]};{row["Продавец"]};{row["Клиент"]};{row["Сумма"]}");
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), new UTF8Encoding(true)); // UTF8 с BOM для Excel
                    MessageBox.Show($"Отчет успешно сформирован! Найдено {reportData.Rows.Count} заказов.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании отчета: " + ex.Message);
            }
        }
    }
}
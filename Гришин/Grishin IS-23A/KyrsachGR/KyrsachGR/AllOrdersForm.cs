using System;
using System.Data;
using System.Drawing;
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

            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(new string[] { "Все", "Обычный", "Крупный" });
            comboBox2.SelectedIndex = 0;

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

                int totalRecords = DbManager.GetOrdersCount(search, status);
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                if (totalPages < 1) totalPages = 1;

                if (currentPage > totalPages) currentPage = totalPages;
                if (currentPage < 1) currentPage = 1;

                DataTable dt = DbManager.GetOrders(currentPage, pageSize, search, status);
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

        private void button8_Click(object sender, EventArgs e) { currentPage = 1; LoadOrders(); }
        private void button9_Click(object sender, EventArgs e) { currentPage = 2; LoadOrders(); }
        private void button10_Click(object sender, EventArgs e) { currentPage = 3; LoadOrders(); }
        private void button11_Click(object sender, EventArgs e) { currentPage = 4; LoadOrders(); }
        private void button12_Click(object sender, EventArgs e) { currentPage = 5; LoadOrders(); }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadOrders();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            comboBox2.SelectedIndex = 0;
            dateTimePicker1.Value = DateTime.Now;
            currentPage = 1;
            LoadOrders();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadOrders();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
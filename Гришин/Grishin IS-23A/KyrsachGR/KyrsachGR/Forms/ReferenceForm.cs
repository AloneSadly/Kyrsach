using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace KyrsachGR
{
    public partial class ReferenceForm : Form
    {
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem deleteMenuItem;

        public ReferenceForm()
        {
            InitializeComponent();
            SetupContextMenu();
        }

        private void ReferenceForm_Load(object sender, EventArgs e)
        {
            LoadDataForTab(tpCountry);
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataForTab(tabControl.SelectedTab);
        }

        private void SetupContextMenu()
        {
            contextMenu = new ContextMenuStrip();
            deleteMenuItem = new ToolStripMenuItem("Удалить запись");
            deleteMenuItem.Click += DeleteMenuItem_Click;
            contextMenu.Items.Add(deleteMenuItem);

            dgvCountry.ContextMenuStrip = contextMenu;
            dataGridView1.ContextMenuStrip = contextMenu;
            dataGridView2.ContextMenuStrip = contextMenu;
            dataGridView3.ContextMenuStrip = contextMenu;
            dataGridView4.ContextMenuStrip = contextMenu;
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView activeDgv = null;
            TabPage activeTab = tabControl.SelectedTab;

            if (activeTab == tpCountry) activeDgv = dgvCountry;
            else if (activeTab == tpMaterial) activeDgv = dataGridView1;
            else if (activeTab == tpCategory) activeDgv = dataGridView2;
            else if (activeTab == tpColor) activeDgv = dataGridView3;
            else if (activeTab == tpPattern) activeDgv = dataGridView4;

            if (activeDgv == null || activeDgv.SelectedRows.Count == 0) return;

            DataGridViewRow row = activeDgv.SelectedRows[0];
            string itemName = row.Cells["name"].Value?.ToString() ?? "эту запись";
            int itemId = 0;

            string tableName = "";
            string idColumn = "";

            if (activeTab == tpCountry) { tableName = "country"; idColumn = "country_id"; }
            else if (activeTab == tpMaterial) { tableName = "material"; idColumn = "material_id"; }
            else if (activeTab == tpCategory) { tableName = "categories"; idColumn = "categ_id"; }
            else if (activeTab == tpColor) { tableName = "color"; idColumn = "color_id"; }
            else if (activeTab == tpPattern) { tableName = "pattern"; idColumn = "pattern_id"; }

            try
            {
                itemId = Convert.ToInt32(row.Cells[idColumn].Value);
            }
            catch
            {
                MessageBox.Show("Не удалось получить ID записи.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить '{itemName}'?\nЭто действие нельзя отменить.",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DbManager.DeleteReferenceItem(tableName, idColumn, itemId);
                    MessageBox.Show("Запись успешно удалена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataForTab(activeTab);
                }
                catch (Exception ex)
                {
                    // Обработка ошибки внешнего ключа (если запись используется в товарах)
                    if (ex.Message.Contains("foreign key"))
                        MessageBox.Show("Невозможно удалить запись, так как она используется в карточках товаров!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Ошибка при удалении: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadDataForTab(TabPage tab)
        {
            try
            {
                if (tab == tpCountry)
                {
                    dgvCountry.DataSource = DbManager.GetReferenceData("country", "name", "country_id", "");
                    SetupGridColumns(dgvCountry, "country_id");
                }
                else if (tab == tpMaterial)
                {
                    dataGridView1.DataSource = DbManager.GetReferenceData("material", "name", "material_id", "");
                    SetupGridColumns(dataGridView1, "material_id");
                    ApplyConditionalFormatting(dataGridView1);
                }
                else if (tab == tpCategory)
                {
                    dataGridView2.DataSource = DbManager.GetReferenceData("categories", "name", "categ_id", "");
                    SetupGridColumns(dataGridView2, "categ_id");
                    ApplyConditionalFormatting(dataGridView2);
                }
                else if (tab == tpColor)
                {
                    dataGridView3.DataSource = DbManager.GetReferenceData("color", "name", "color_id", "");
                    SetupGridColumns(dataGridView3, "color_id");
                    ApplyConditionalFormatting(dataGridView3);
                }
                else if (tab == tpPattern)
                {
                    dataGridView4.DataSource = DbManager.GetReferenceData("pattern", "name", "pattern_id", "");
                    SetupGridColumns(dataGridView4, "pattern_id");
                    ApplyConditionalFormatting(dataGridView4);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Вспомогательный метод для настройки колонок
        private void SetupGridColumns(DataGridView dgv, string idColumnName)
        {
            // Скрываем колонку с ID
            if (dgv.Columns[idColumnName] != null)
            {
                dgv.Columns[idColumnName].Visible = false;
            }

            // Переименовываем колонку name
            if (dgv.Columns["name"] != null)
            {
                dgv.Columns["name"].HeaderText = "Название";
                dgv.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Растянуть на всю ширину
            }

            // Настройка внешнего вида
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void ApplyConditionalFormatting(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells["name"] != null && (row.Cells["name"].Value == DBNull.Value || string.IsNullOrEmpty(row.Cells["name"].Value.ToString())))
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.ForeColor = Color.DarkGray;
                }
                else
                {
                    if (row.Index % 2 == 0) row.DefaultCellStyle.BackColor = Color.White;
                    else row.DefaultCellStyle.BackColor = Color.WhiteSmoke;
                }
            }
        }

        // ============================================================
        // ВКЛАДКА "СТРАНЫ"
        // ============================================================
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCountryName.Text)) return;
            try
            {
                DbManager.AddReferenceItem("country", "name", txtCountryName.Text);
                txtCountryName.Text = "";
                LoadDataForTab(tpCountry);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (dgvCountry.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(txtCountryName.Text)) return;
            try
            {
                int id = Convert.ToInt32(dgvCountry.SelectedRows[0].Cells["country_id"].Value);
                DbManager.UpdateReferenceItem("country", "country_id", id, "name", txtCountryName.Text);
                LoadDataForTab(tpCountry);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtCountryName.Text = "";
            LoadDataForTab(tpCountry);
        }

        // ============================================================
        // ВКЛАДКА "МАТЕРИАЛЫ"
        // ============================================================
        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text)) return;
            try
            {
                DbManager.AddReferenceItem("material", "name", textBox1.Text);
                textBox1.Text = "";
                LoadDataForTab(tpMaterial);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(textBox1.Text)) return;
            try
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["material_id"].Value);
                DbManager.UpdateReferenceItem("material", "material_id", id, "name", textBox1.Text);
                LoadDataForTab(tpMaterial);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            LoadDataForTab(tpMaterial);
        }

        // ============================================================
        // ВКЛАДКА "КАТЕГОРИИ"
        // ============================================================
        private void button7_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text)) return;
            try
            {
                DbManager.AddReferenceItem("categories", "name", textBox3.Text);
                textBox3.Text = "";
                LoadDataForTab(tpCategory);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(textBox3.Text)) return;
            try
            {
                int id = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["categ_id"].Value);
                DbManager.UpdateReferenceItem("categories", "categ_id", id, "name", textBox3.Text);
                LoadDataForTab(tpCategory);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            LoadDataForTab(tpCategory);
        }

        // ============================================================
        // ВКЛАДКА "ЦВЕТА"
        // ============================================================
        private void button10_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox4.Text)) return;
            try
            {
                DbManager.AddReferenceItem("color", "name", textBox4.Text);
                textBox4.Text = "";
                LoadDataForTab(tpColor);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(textBox4.Text)) return;
            try
            {
                int id = Convert.ToInt32(dataGridView3.SelectedRows[0].Cells["color_id"].Value);
                DbManager.UpdateReferenceItem("color", "color_id", id, "name", textBox4.Text);
                LoadDataForTab(tpColor);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
            LoadDataForTab(tpColor);
        }

        // ============================================================
        // ВКЛАДКА "ШАБЛОНЫ"
        // ============================================================
        private void button13_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox5.Text)) return;
            try
            {
                DbManager.AddReferenceItem("pattern", "name", textBox5.Text);
                textBox5.Text = "";
                LoadDataForTab(tpPattern);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (dataGridView4.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(textBox5.Text)) return;
            try
            {
                int id = Convert.ToInt32(dataGridView4.SelectedRows[0].Cells["pattern_id"].Value);
                DbManager.UpdateReferenceItem("pattern", "pattern_id", id, "name", textBox5.Text);
                LoadDataForTab(tpPattern);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            LoadDataForTab(tpPattern);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
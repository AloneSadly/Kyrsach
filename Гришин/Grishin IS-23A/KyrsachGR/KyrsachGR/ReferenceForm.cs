using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace KyrsachGR
{
    public partial class ReferenceForm : Form
    {
        private TabPage currentTab;

        public ReferenceForm()
        {
            InitializeComponent();
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
        private void LoadDataForTab(TabPage tab)
        {
            currentTab = tab;
            try
            {
                if (tab == tpCountry)
                {

                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("Название", typeof(string));
                    dt.Rows.Add(1, "Россия");
                    dt.Rows.Add(2, "Китай");
                    dgvCountry.DataSource = dt;
                }
                else if (tab == tpMaterial)
                {
                    dataGridView1.DataSource = DbManager.GetReferenceData("material", "name", textBox1.Text);
                    ApplyConditionalFormatting(dataGridView1);
                }
                else if (tab == tpCategory)
                {
                    dataGridView2.DataSource = DbManager.GetReferenceData("categories", "name", textBox3.Text);
                    ApplyConditionalFormatting(dataGridView2);
                }
                else if (tab == tpColor)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("Название", typeof(string));
                    dt.Rows.Add(1, "Красный");
                    dt.Rows.Add(2, "Синий");
                    dataGridView3.DataSource = dt;
                }
                else if (tab == tpPattern)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("Название", typeof(string));
                    dt.Rows.Add(1, "Клетка");
                    dt.Rows.Add(2, "Полоска");
                    dataGridView4.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ApplyConditionalFormatting(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells["name"] != null &&
                    (row.Cells["name"].Value == DBNull.Value ||
                     string.IsNullOrEmpty(row.Cells["name"].Value.ToString())))
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.ForeColor = Color.DarkGray;
                }
                else
                {
                    if (row.Index % 2 == 0)
                        row.DefaultCellStyle.BackColor = Color.White;
                    else
                        row.DefaultCellStyle.BackColor = Color.WhiteSmoke;
                }
            }
        }


        private void button2_Click(object sender, EventArgs e) 
        {
            if (string.IsNullOrWhiteSpace(txtCountryName.Text))
            {
                MessageBox.Show("Введите название!");
                return;
            }

            try
            {
                MessageBox.Show($"Страна '{txtCountryName.Text}' добавлена (демо)");
                txtCountryName.Text = "";
                LoadDataForTab(tpCountry);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button15_Click(object sender, EventArgs e) 
        {
            if (dgvCountry.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите строку для изменения!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCountryName.Text))
            {
                MessageBox.Show("Введите новое название!");
                return;
            }

            try
            {
                MessageBox.Show("Изменено (демо)");
                LoadDataForTab(tpCountry);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtCountryName.Text = "";
            LoadDataForTab(tpCountry);
        }


        private void button4_Click(object sender, EventArgs e) 
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Введите название материала!");
                return;
            }

            try
            {
                DbManager.AddReferenceItem("material", "name", textBox1.Text);
                MessageBox.Show("Материал добавлен!");
                textBox1.Text = "";
                LoadDataForTab(tpMaterial);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e) 
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите материал!");
                return;
            }

            try
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["material_id"].Value);
                DbManager.UpdateReferenceItem("material", "material_id", id, "name", textBox1.Text);
                MessageBox.Show("Изменено!");
                LoadDataForTab(tpMaterial);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e) 
        {
            textBox1.Text = "";
            LoadDataForTab(tpMaterial);
        }


        private void button7_Click(object sender, EventArgs e) 
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Введите название категории!");
                return;
            }

            try
            {
                DbManager.AddReferenceItem("categories", "name", textBox3.Text);
                MessageBox.Show("Категория добавлена!");
                textBox3.Text = "";
                LoadDataForTab(tpCategory);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e) 
        {
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите категорию!");
                return;
            }

            try
            {
                int id = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["categ_id"].Value);
                DbManager.UpdateReferenceItem("categories", "categ_id", id, "name", textBox3.Text);
                MessageBox.Show("Изменено!");
                LoadDataForTab(tpCategory);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e) 
        {
            textBox3.Text = "";
            LoadDataForTab(tpCategory);
        }


        private void txtCountryName_TextChanged(object sender, EventArgs e)
        {
            LoadDataForTab(tpCountry);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadDataForTab(tpMaterial);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            LoadDataForTab(tpCategory);
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            LoadDataForTab(tpPattern);
        }
    }
}
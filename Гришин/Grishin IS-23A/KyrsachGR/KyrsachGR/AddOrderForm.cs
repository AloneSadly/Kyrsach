using System;
using System.Windows.Forms;

namespace KyrsachGR
{
    public partial class AddOrderForm : Form
    {
        public AddOrderForm()
        {
            InitializeComponent();
        }

        private void AddOrderForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Загружаем категории в ComboBox
                cmbCategory.DataSource = DbManager.GetCategories();
                cmbCategory.DisplayMember = "name";
                cmbCategory.ValueMember = "categ_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки категорий: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text) || cmbCategory.SelectedValue == null)
            {
                MessageBox.Show("Заполните обязательные поля (Название, Цена, Категория)!");
                return;
            }

            try
            {
                decimal price = Convert.ToDecimal(txtPrice.Text);
                int stock = string.IsNullOrEmpty(txtStockQuantity.Text) ? 0 : Convert.ToInt32(txtStockQuantity.Text);
                int catId = Convert.ToInt32(cmbCategory.SelectedValue);

                // Парсим числа (если пустые, ставим 0)
                decimal w = string.IsNullOrEmpty(txtWidth.Text) ? 0 : Convert.ToDecimal(txtWidth.Text);
                decimal l = string.IsNullOrEmpty(txtLength.Text) ? 0 : Convert.ToDecimal(txtLength.Text);

                DbManager.AddProduct(
                    txtName.Text, txtDescription.Text, txtArticle.Text, txtManufacturer.Text, txtCountry.Text,
                    w, l, txtPattern.Text, txtColor.Text, price, stock, catId
                );

                MessageBox.Show("Товар успешно добавлен!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Проверьте правильность ввода чисел (цена, ширина, длина).");
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
using System;
using System.Data;
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
                // Загружаем Категории
                cmbCategory.DataSource = DbManager.GetCategories();
                cmbCategory.DisplayMember = "name";
                cmbCategory.ValueMember = "categ_id";

                // Загружаем Страны
                cmbCountry.DataSource = DbManager.GetCountries();
                cmbCountry.DisplayMember = "name";
                cmbCountry.ValueMember = "country_id";

                // Загружаем Материалы
                cmbMaterial.DataSource = DbManager.GetMaterials();
                cmbMaterial.DisplayMember = "name";
                cmbMaterial.ValueMember = "material_id";

                // Загружаем Цвета
                cmbColor.DataSource = DbManager.GetColors();
                cmbColor.DisplayMember = "name";
                cmbColor.ValueMember = "color_id";

                // Загружаем Шаблоны/Рисунки
                cmbPattern.DataSource = DbManager.GetPatterns();
                cmbPattern.DisplayMember = "name";
                cmbPattern.ValueMember = "pattern_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки справочников: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text) || cmbCategory.SelectedValue == null)
            {
                MessageBox.Show("Заполните обязательные поля (Название, Цена, Категория)!");
                return;
            }

            try
            {
                decimal price = Convert.ToDecimal(txtPrice.Text);
                int stock = string.IsNullOrEmpty(txtStockQuantity.Text) ? 0 : Convert.ToInt32(txtStockQuantity.Text);

                // Берем ID из ComboBox
                int catId = Convert.ToInt32(cmbCategory.SelectedValue);
                int countryId = Convert.ToInt32(cmbCountry.SelectedValue);
                int materialId = Convert.ToInt32(cmbMaterial.SelectedValue);
                int colorId = Convert.ToInt32(cmbColor.SelectedValue);
                int patternId = Convert.ToInt32(cmbPattern.SelectedValue);

                decimal w = string.IsNullOrEmpty(txtWidth.Text) ? 0 : Convert.ToDecimal(txtWidth.Text);
                decimal l = string.IsNullOrEmpty(txtLength.Text) ? 0 : Convert.ToDecimal(txtLength.Text);

                // Вызываем метод (убедитесь, что порядок аргументов совпадает с методом в DbManager)
                DbManager.AddProduct(
                    txtName.Text, txtDescription.Text, txtArticle.Text, txtManufacturer.Text,
                    w, l, price, stock, catId, materialId, colorId, countryId, patternId
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
using System;
using System.Data;
using System.Windows.Forms;

namespace KyrsachGR
{
    public partial class FormProductsEdit : Form
    {
        private int productId;

        public FormProductsEdit(int id)
        {
            InitializeComponent();
            this.productId = id;

            // Загружаем данные сразу в конструкторе (как для пользователей)
            LoadComboBoxes();
            LoadProductData();
        }

        // Загрузка справочников в ComboBox
        private void LoadComboBoxes()
        {
            try
            {
                cmbCategory.DataSource = DbManager.GetCategories();
                cmbCategory.DisplayMember = "name";
                cmbCategory.ValueMember = "categ_id";

                txtCountry.DataSource = DbManager.GetCountries();
                txtCountry.DisplayMember = "name";
                txtCountry.ValueMember = "country_id";

                txtMaterial.DataSource = DbManager.GetMaterials();
                txtMaterial.DisplayMember = "name";
                txtMaterial.ValueMember = "material_id";

                txtColor.DataSource = DbManager.GetColors();
                txtColor.DisplayMember = "name";
                txtColor.ValueMember = "color_id";

                txtPattern.DataSource = DbManager.GetPatterns();
                txtPattern.DisplayMember = "name";
                txtPattern.ValueMember = "pattern_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки справочников: " + ex.Message);
            }
        }

        // Загрузка данных конкретного товара
        private void LoadProductData()
        {
            try
            {
                DataTable dt = DbManager.GetProductById(productId);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    txtName.Text = row["name"].ToString();
                    txtDescription.Text = row["description"].ToString();
                    txtArticle.Text = row["article"].ToString();
                    txtManufacturer.Text = row["manufacturer"].ToString();
                    txtWidth.Text = row["width"].ToString();
                    txtLength.Text = row["length"].ToString();
                    txtPrice.Text = row["price"].ToString();
                    txtStockQuantity.Text = row["stock_quantity"].ToString();

                    // Безопасная установка значений в ComboBox
                    SetComboValue(cmbCategory, row["category_id"]);
                    SetComboValue(txtCountry, row["country_country_id"]);
                    SetComboValue(txtMaterial, row["material_material_id"]);
                    SetComboValue(txtColor, row["color_color_id"]);
                    SetComboValue(txtPattern, row["pattern_pattern_id"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных товара: " + ex.Message);
            }
        }

        // Вспомогательный метод для безопасной установки значения в ComboBox
        private void SetComboValue(ComboBox cmb, object value)
        {
            if (value != DBNull.Value && value != null)
            {
                try
                {
                    cmb.SelectedValue = Convert.ToInt32(value);
                }
                catch
                {
                    // Если не удалось установить, оставляем первое значение по умолчанию
                }
            }
        }

        // Кнопка СОХРАНИТЬ
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Заполните хотя бы Название и Цену!");
                return;
            }

            try
            {
                decimal price = Convert.ToDecimal(txtPrice.Text);
                int stock = string.IsNullOrEmpty(txtStockQuantity.Text) ? 0 : Convert.ToInt32(txtStockQuantity.Text);
                decimal w = string.IsNullOrEmpty(txtWidth.Text) ? 0 : Convert.ToDecimal(txtWidth.Text);
                decimal l = string.IsNullOrEmpty(txtLength.Text) ? 0 : Convert.ToDecimal(txtLength.Text);

                // Берем ID из ComboBox
                int catId = Convert.ToInt32(cmbCategory.SelectedValue);
                int countryId = Convert.ToInt32(txtCountry.SelectedValue);
                int materialId = Convert.ToInt32(txtMaterial.SelectedValue);
                int colorId = Convert.ToInt32(txtColor.SelectedValue);
                int patternId = Convert.ToInt32(txtPattern.SelectedValue);

                DbManager.UpdateProduct(
                    productId,
                    txtName.Text, txtDescription.Text, txtArticle.Text, txtManufacturer.Text,
                    w, l, price, stock, catId, materialId, colorId, countryId, patternId
                );

                MessageBox.Show("Товар успешно обновлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Проверьте правильность ввода чисел (цена, ширина, длина)!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }

        // Кнопка ОТМЕНА
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
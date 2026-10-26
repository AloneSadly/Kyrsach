using System;
using System.Data;
using System.Windows.Forms;

namespace KyrsachGR
{
    public partial class Order_create : Form
    {
        private DataTable cartTable;
        private int currentSellerId = 3; // Заглушка, в идеале передавать ID продавца

        public Order_create()
        {
            InitializeComponent();
        }

        private void Order_create_Load(object sender, EventArgs e)
        {
            // Настройка корзины
            cartTable = new DataTable();
            cartTable.Columns.Add("id", typeof(int));
            cartTable.Columns.Add("Название", typeof(string));
            cartTable.Columns.Add("quantity", typeof(int));
            cartTable.Columns.Add("price", typeof(decimal));
            cartTable.Columns.Add("Сумма", typeof(decimal));
            dgvItems.DataSource = cartTable;

            // Заполнение ComboBox "Категория" из БД
            try
            {
                DataTable categories = DbManager.GetCategories();
                cmbCategoryFilter.DataSource = categories;
                cmbCategoryFilter.DisplayMember = "name";
                cmbCategoryFilter.ValueMember = "categ_id";
            }
            catch (Exception ex) { MessageBox.Show("Ошибка загрузки категорий: " + ex.Message); }

            // Заполнение ComboBox "Сортировка"
            cmbSortBy.Items.Clear();
            cmbSortBy.Items.AddRange(new string[] { "Без сортировки", "По цене (возр.)", "По цене (убыв.)" });
            cmbSortBy.SelectedIndex = 0;

            // Счетчик количества (вместо DomainUpDown лучше NumericUpDown, но оставим ваш)
            domainUpDown1.Text = "1";

            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                string search = txtSearch.Text.Trim();
                string sort = cmbSortBy.SelectedItem?.ToString() ?? "Без сортировки";

                // Получаем ID категории (если выбрано "Все", то 0)
                int catId = 0;
                if (cmbCategoryFilter.SelectedValue != null && cmbCategoryFilter.SelectedValue is int)
                {
                    catId = (int)cmbCategoryFilter.SelectedValue;
                }

                DataTable dt = DbManager.GetProducts(1, 1000, search, "Все", sort, catId);
                dgvProducts.DataSource = dt;

                // Скрываем технические колонки
                if (dgvProducts.Columns["id"] != null) dgvProducts.Columns["id"].Visible = false;
                if (dgvProducts.Columns["color"] != null) dgvProducts.Columns["color"].Visible = false;
                if (dgvProducts.Columns["country"] != null) dgvProducts.Columns["country"].Visible = false;
                if (dgvProducts.Columns["category"] != null) dgvProducts.Columns["category"].Visible = false;

                if (dgvProducts.Columns["name"] != null) dgvProducts.Columns["name"].HeaderText = "Название";
                if (dgvProducts.Columns["article"] != null) dgvProducts.Columns["article"].HeaderText = "Артикул";
                if (dgvProducts.Columns["price"] != null) dgvProducts.Columns["price"].HeaderText = "Цена";
                if (dgvProducts.Columns["stock_quantity"] != null) dgvProducts.Columns["stock_quantity"].HeaderText = "Остаток";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // Поиск и фильтры
        private void txtSearch_TextChanged(object sender, EventArgs e) { LoadProducts(); }
        private void cmbCategoryFilter_SelectedIndexChanged(object sender, EventArgs e) { LoadProducts(); }
        private void cmbSortBy_SelectedIndexChanged(object sender, EventArgs e) { LoadProducts(); }

        // Кнопка "Сброс"
        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            cmbCategoryFilter.SelectedIndex = 0;
            cmbSortBy.SelectedIndex = 0;
            LoadProducts();
        }

        // Добавление товара в корзину
        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар из списка!");
                return;
            }

            DataGridViewRow row = dgvProducts.SelectedRows[0];
            int prodId = Convert.ToInt32(row.Cells["id"].Value);
            string prodName = row.Cells["name"].Value.ToString();
            decimal price = Convert.ToDecimal(row.Cells["price"].Value);

            // Читаем количество (исправленный баг)
            int qtyToAdd = 1;
            if (!int.TryParse(domainUpDown1.Text, out qtyToAdd) || qtyToAdd <= 0)
            {
                MessageBox.Show("Укажите корректное количество больше нуля!");
                return;
            }

            // Проверяем, есть ли уже такой товар в корзине
            DataRow existingRow = null;
            foreach (DataRow r in cartTable.Rows)
            {
                if (Convert.ToInt32(r["id"]) == prodId)
                {
                    existingRow = r;
                    break;
                }
            }

            if (existingRow != null)
            {
                existingRow["quantity"] = Convert.ToInt32(existingRow["quantity"]) + qtyToAdd;
                existingRow["Сумма"] = Convert.ToDecimal(existingRow["quantity"]) * price;
            }
            else
            {
                cartTable.Rows.Add(prodId, prodName, qtyToAdd, price, qtyToAdd * price);
            }

            UpdateTotals();
            domainUpDown1.Text = "1"; // Сбрасываем счетчик
        }

        // Удаление из корзины
        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvItems.SelectedRows.Count == 0) return;
            int index = dgvItems.SelectedRows[0].Index;
            cartTable.Rows.RemoveAt(index);
            UpdateTotals();
        }

        // Пересчет итогов и скидок
        private void UpdateTotals()
        {
            decimal subtotal = 0;
            foreach (DataRow row in cartTable.Rows)
            {
                subtotal += Convert.ToDecimal(row["Сумма"]);
            }

            // Логика скидки по заданию
            decimal discountPercent = 0;
            if (subtotal >= 70000) discountPercent = 15;
            else if (subtotal >= 50000) discountPercent = 10;
            else if (subtotal >= 30000) discountPercent = 5;

            decimal discountSum = subtotal * (discountPercent / 100);
            decimal total = subtotal - discountSum;

            lblSubtotalAmount.Text = subtotal.ToString("F2") + " руб.";
            lblDiscountPercent.Text = discountPercent.ToString() + " %";
            lblDiscountSum.Text = discountSum.ToString("F2") + " руб.";
            lblTotalAmount.Text = total.ToString("F2") + " руб.";
        }

        // Создание заказа
        private void btnCreateReceipt_Click(object sender, EventArgs e)
        {
            if (cartTable.Rows.Count == 0)
            {
                MessageBox.Show("Корзина пуста!");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                MessageBox.Show("Введите ФИО покупателя!");
                return;
            }

            try
            {
                // Извлекаем итоговую сумму из Label (убираем текст " руб.")
                string totalStr = lblTotalAmount.Text.Replace(" руб.", "").Trim();
                decimal totalAmount = Convert.ToDecimal(totalStr);

                int receiptId = DbManager.CreateReceipt(
                    DateTime.Now,
                    totalAmount,
                    currentSellerId,
                    txtCustomerName.Text,
                    txtCustomerPhone.Text,
                    cartTable
                );

                MessageBox.Show($"Заказ успешно создан! Номер чека: {receiptId}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
using System;
using System.Data;
using System.Windows.Forms;

namespace KyrsachGR
{
    public partial class Order_create : Form
    {
        // Таблица, которая будет выступать в роли "корзины"
        private DataTable cartTable;
        // ID текущего авторизованного пользователя (для seller_id)
        private int currentSellerId = 3; // По умолчанию (Сидоров). В реальном проекте передавайте ID из AuthForm.

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

            // Настройка ComboBox
            cmbCategoryFilter.Items.Add("Все");
            cmbCategoryFilter.Items.Add("Одежда"); // В идеале заполнять из БД
            cmbCategoryFilter.SelectedIndex = 0;

            cmbSortBy.Items.AddRange(new string[] { "По имени", "По цене" });
            cmbSortBy.SelectedIndex = 0;
            cmbSortOrder.Items.AddRange(new string[] { "По возрастанию", "По убыванию" });
            cmbSortOrder.SelectedIndex = 0;

            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                string search = txtSearch.Text.Trim();
                // Загружаем товары (используем метод из DbManager, который мы писали ранее)
                // Передаем пустую строку вместо статуса, чтобы показать все товары
                DataTable dt = DbManager.GetProducts(1, 100, search, "Все", cmbSortBy.Text);
                dgvProducts.DataSource = dt;

                if (dgvProducts.Columns["id"] != null) dgvProducts.Columns["id"].Visible = false;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // Поиск
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
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

            // Получаем данные выбранного товара
            DataGridViewRow row = dgvProducts.SelectedRows[0];
            int prodId = Convert.ToInt32(row.Cells["id"].Value);
            string prodName = row.Cells["name"].Value.ToString();
            decimal price = Convert.ToDecimal(row.Cells["price"].Value);
            int qtyToAdd = Convert.ToInt32(domainUpDown1.Text); 

            if (qtyToAdd <= 0)
            {
                MessageBox.Show("Укажите количество больше нуля!");
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
                // Если есть, увеличиваем количество
                existingRow["quantity"] = Convert.ToInt32(existingRow["quantity"]) + qtyToAdd;
                existingRow["Сумма"] = Convert.ToDecimal(existingRow["quantity"]) * price;
            }
            else
            {
                // Если нет, добавляем новую строку
                cartTable.Rows.Add(prodId, prodName, qtyToAdd, price, qtyToAdd * price);
            }

            UpdateTotals();
            Convert.ToInt32(domainUpDown1.Text = "0"); // Сбрасываем счетчик
        }

        // Удаление из корзины
        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvItems.SelectedRows.Count == 0) return;

            int index = dgvItems.SelectedRows[0].Index;
            cartTable.Rows.RemoveAt(index);
            UpdateTotals();
        }

        // Пересчет итогов
        private void UpdateTotals()
        {
            decimal subtotal = 0;
            foreach (DataRow row in cartTable.Rows)
            {
                subtotal += Convert.ToDecimal(row["Сумма"]);
            }

            // Скидка 10% если сумма > 15000
            decimal discountPercent = subtotal > 15000 ? 10 : 0;
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
                MessageBox.Show("Введите имя покупателя!");
                return;
            }

            try
            {
                decimal totalAmount = Convert.ToDecimal(lblTotalAmount.Text.Split(' ')[0]);

                // Вызываем метод сохранения в БД
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
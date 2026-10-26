using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace KyrsachGR
{
    public class DbManager
    {
        private static string connStr = "server=localhost;user=root;database=kyrsach;password=root;";
        public static string[] GetLogins()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "SELECT login FROM users";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    List<string> logins = new List<string>();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            logins.Add(reader["login"].ToString());
                        }
                    }
                    return logins.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка загрузки логинов: " + ex.Message);
            }
        }
        public static string Authenticate(string login, string password)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "SELECT role FROM users WHERE login = @login AND pass = @pass";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@pass", password);

                    object result = cmd.ExecuteScalar();
                    return result?.ToString(); 
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка авторизации: " + ex.Message);
            }
        }
        public static DataTable GetOrders(int page, int pageSize, string searchText = "", string status = "", string sortOrder = "По дате")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = @"
                SELECT 
                    r.receipts_id AS '№ Чека',
                    r.sale_date AS 'Дата',
                    u.full_name AS 'Продавец',
                    r.customer_name AS 'Клиент',
                    r.total_amount AS 'Сумма',
                    CASE 
                        WHEN r.total_amount > 10000 THEN 'Крупный'
                        ELSE 'Обычный'
                    END AS 'Статус'
                FROM receipts r
                LEFT JOIN users u ON r.seller_id = u.user_id
                WHERE 1=1 ";

                    if (!string.IsNullOrEmpty(searchText))
                        sql += " AND (r.customer_name LIKE @search OR u.full_name LIKE @search) ";

                    if (!string.IsNullOrEmpty(status) && status != "Все")
                        sql += " AND CASE WHEN r.total_amount > 10000 THEN 'Крупный' ELSE 'Обычный' END = @status ";

                    switch (sortOrder)
                    {
                        case "По сумме": sql += " ORDER BY r.total_amount DESC "; break;
                        case "По клиенту": sql += " ORDER BY r.customer_name ASC "; break;
                        default: sql += " ORDER BY r.sale_date DESC "; break; // По дате
                    }

                    sql += " LIMIT @offset, @pageSize";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@offset", (page - 1) * pageSize);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex) { throw new Exception("Ошибка загрузки заказов: " + ex.Message); }
        }
        public static DataTable GetOrderItems(int receiptId)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = @"
            SELECT 
                p.name AS 'Товар', 
                ri.quantity AS 'Кол-во', 
                ri.price_per_unit AS 'Цена', 
                (ri.quantity * ri.price_per_unit) AS 'Сумма'
            FROM receipt_items ri
            JOIN products p ON ri.product_id = p.id
            WHERE ri.receipt_id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", receiptId);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public static DataTable GetOrdersForReport(DateTime start, DateTime end)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = @"
            SELECT 
                r.receipts_id AS '№ Чека',
                r.sale_date AS 'Дата',
                u.full_name AS 'Продавец',
                r.customer_name AS 'Клиент',
                r.total_amount AS 'Сумма'
            FROM receipts r
            LEFT JOIN users u ON r.seller_id = u.user_id
            WHERE r.sale_date BETWEEN @start AND @end
            ORDER BY r.sale_date";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", end);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public static int GetOrdersCount(string searchText = "", string status = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "SELECT COUNT(*) FROM receipts r LEFT JOIN users u ON r.seller_id = u.user_id WHERE 1=1 ";

                    if (!string.IsNullOrEmpty(searchText))
                        sql += " AND (r.customer_name LIKE @search OR u.full_name LIKE @search) ";
                    if (!string.IsNullOrEmpty(status) && status != "Все")
                        sql += " AND CASE WHEN r.total_amount > 10000 THEN 'Крупный' ELSE 'Обычный' END = @status ";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                    cmd.Parameters.AddWithValue("@status", status);

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch
            {
                return 0;
            }
        }
        public static DataTable GetReferenceData(string tableName, string nameColumn, string idColumn, string searchText = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string[] allowedTables = { "categories", "material", "color", "pattern", "country" };
                    if (Array.IndexOf(allowedTables, tableName) < 0)
                        throw new Exception("Недопустимая таблица");

                    string sql = $"SELECT {idColumn}, {nameColumn} FROM {tableName}";

                    if (!string.IsNullOrEmpty(searchText))
                        sql += $" WHERE {nameColumn} LIKE @search";

                    sql += " ORDER BY " + nameColumn;

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    if (!string.IsNullOrEmpty(searchText))
                        cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка загрузки {tableName}: " + ex.Message);
            }
        }
        public static void AddReferenceItem(string tableName, string nameColumn, string nameValue)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = $"INSERT INTO {tableName} ({nameColumn}) VALUES (@name)";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", nameValue);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка добавления: " + ex.Message);
            }
        }
        public static void UpdateReferenceItem(string tableName, string idColumn, int id, string nameColumn, string nameValue)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = $"UPDATE {tableName} SET {nameColumn} = @name WHERE {idColumn} = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", nameValue);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка обновления: " + ex.Message);
            }
        }
        public static void DeleteReferenceItem(string tableName, string idColumn, int id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = $"DELETE FROM {tableName} WHERE {idColumn} = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка удаления: " + ex.Message);
            }
        }
        public static DataTable GetUsers(string searchText = "", string role = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string sql = "SELECT user_id, full_name, email, phone, login, pass, role FROM users WHERE 1=1 ";

                    if (!string.IsNullOrEmpty(searchText))
                        sql += " AND full_name LIKE @search ";

                    if (!string.IsNullOrEmpty(role) && role != "Все")
                        sql += " AND role = @role ";

                    sql += " ORDER BY user_id";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    if (!string.IsNullOrEmpty(searchText))
                        cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                    if (!string.IsNullOrEmpty(role) && role != "Все")
                        cmd.Parameters.AddWithValue("@role", role);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка загрузки пользователей: " + ex.Message);
            }
        }
        public static void DeleteUser(int userId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "DELETE FROM users WHERE user_id = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка удаления пользователя: " + ex.Message);
            }
        }
        public static DataTable GetCategories()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT categ_id, name FROM categories", conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public static void AddProduct(string name, string desc, string article, string manufacturer, string country,
            decimal width, decimal length, string pattern, string color, decimal price, int stock, int categoryId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = @"INSERT INTO products (name, description, article, manufacturer, country, width, length, pattern, color, price, stock_quantity, category_id) 
                           VALUES (@name, @desc, @art, @man, @country, @w, @l, @pat, @col, @price, @stock, @cat)";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@desc", desc);
                    cmd.Parameters.AddWithValue("@art", article);
                    cmd.Parameters.AddWithValue("@man", manufacturer);
                    cmd.Parameters.AddWithValue("@country", country);
                    cmd.Parameters.AddWithValue("@w", width);
                    cmd.Parameters.AddWithValue("@l", length);
                    cmd.Parameters.AddWithValue("@pat", pattern);
                    cmd.Parameters.AddWithValue("@col", color);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@stock", stock);
                    cmd.Parameters.AddWithValue("@cat", categoryId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { throw new Exception("Ошибка добавления товара: " + ex.Message); }
        }
        public static void AddUser(string login, string fullName, string email, string phone, string pass, string role)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = @"INSERT INTO users (login, full_name, email, phone, pass, role) 
                           VALUES (@login, @fio, @email, @phone, @pass, @role)";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@fio", fullName);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@pass", pass);
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { throw new Exception("Ошибка добавления сотрудника: " + ex.Message); }
        }
        public static int CreateReceipt(DateTime date, decimal total, int sellerId, string custName, string custPhone, DataTable items)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlTransaction transaction = conn.BeginTransaction();
                    try
                    {
                        string sqlReceipt = @"INSERT INTO receipts (sale_date, total_amount, seller_id, customer_name, customer_phone) 
                                      VALUES (@date, @total, @seller, @name, @phone);
                                      SELECT LAST_INSERT_ID();";
                        MySqlCommand cmd1 = new MySqlCommand(sqlReceipt, conn, transaction);
                        cmd1.Parameters.AddWithValue("@date", date);
                        cmd1.Parameters.AddWithValue("@total", total);
                        cmd1.Parameters.AddWithValue("@seller", sellerId);
                        cmd1.Parameters.AddWithValue("@name", custName);
                        cmd1.Parameters.AddWithValue("@phone", custPhone);
                        int receiptId = Convert.ToInt32(cmd1.ExecuteScalar());

                        foreach (DataRow row in items.Rows)
                        {
                            int prodId = Convert.ToInt32(row["id"]);
                            int qty = Convert.ToInt32(row["quantity"]);
                            decimal price = Convert.ToDecimal(row["price"]);

                            string sqlItem = "INSERT INTO receipt_items (receipt_id, product_id, quantity, price_per_unit) VALUES (@rid, @pid, @qty, @price)";
                            MySqlCommand cmdItem = new MySqlCommand(sqlItem, conn, transaction);
                            cmdItem.Parameters.AddWithValue("@rid", receiptId);
                            cmdItem.Parameters.AddWithValue("@pid", prodId);
                            cmdItem.Parameters.AddWithValue("@qty", qty);
                            cmdItem.Parameters.AddWithValue("@price", price);
                            cmdItem.ExecuteNonQuery();

                            string sqlStock = "UPDATE products SET stock_quantity = stock_quantity - @qty WHERE id = @pid";
                            MySqlCommand cmdStock = new MySqlCommand(sqlStock, conn, transaction);
                            cmdStock.Parameters.AddWithValue("@qty", qty);
                            cmdStock.Parameters.AddWithValue("@pid", prodId);
                            cmdStock.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return receiptId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex) { throw new Exception("Ошибка создания заказа: " + ex.Message); }
        }
        public static DataTable GetProducts(int page, int pageSize, string searchText = "", string status = "", string sortOrder = "Без сортировки", int categoryId = 0)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = @"
                SELECT 
                    p.id, 
                    p.name, 
                    p.article, 
                    p.price, 
                    p.stock_quantity, 
                    c.name AS color, 
                    cnt.name AS country,
                    cat.name AS category
                FROM products p
                LEFT JOIN color c ON p.color_color_id = c.color_id
                LEFT JOIN country cnt ON p.country_country_id = cnt.country_id
                LEFT JOIN categories cat ON p.category_id = cat.categ_id
                WHERE 1=1 ";

                    if (!string.IsNullOrEmpty(searchText))
                        sql += " AND p.name LIKE @search ";

                    if (status == "В наличии")
                        sql += " AND p.stock_quantity > 0 ";
                    else if (status == "Нет в наличии")
                        sql += " AND p.stock_quantity = 0 ";

                    // Фильтр по категории (если передано ID > 0)
                    if (categoryId > 0)
                        sql += " AND p.category_id = @catId ";

                    // Сортировка
                    switch (sortOrder)
                    {
                        case "По цене (возр.)": sql += " ORDER BY p.price ASC "; break;
                        case "По цене (убыв.)": sql += " ORDER BY p.price DESC "; break;
                        default: sql += " ORDER BY p.id ASC "; break; // По умолчанию (без сортировки)
                    }

                    sql += " LIMIT @offset, @pageSize";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    if (!string.IsNullOrEmpty(searchText))
                        cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                    if (categoryId > 0)
                        cmd.Parameters.AddWithValue("@catId", categoryId);

                    cmd.Parameters.AddWithValue("@offset", (page - 1) * pageSize);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex) { throw new Exception("Ошибка загрузки товаров: " + ex.Message); }
        }
        public static int GetProductsCount(string searchText = "", string status = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "SELECT COUNT(*) FROM products p WHERE 1=1 ";

                    if (!string.IsNullOrEmpty(searchText))
                        sql += " AND p.name LIKE @search ";
                    if (status == "В наличии")
                        sql += " AND p.stock_quantity > 0 ";
                    else if (status == "Нет в наличии")
                        sql += " AND p.stock_quantity = 0 ";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    if (!string.IsNullOrEmpty(searchText))
                        cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch { return 0; }
        }
        public static DataTable GetProductById(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT * FROM products WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public static void UpdateProduct(int id, string name, string desc, string article, string manufacturer,
            decimal width, decimal length, decimal price, int stock, int categoryId, int materialId, int colorId, int countryId, int patternId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = @"UPDATE products SET 
                name=@name, description=@desc, article=@art, manufacturer=@man, 
                width=@w, length=@l, price=@price, stock_quantity=@stock, 
                category_id=@cat, material_material_id=@mat, 
                color_color_id=@col, country_country_id=@cnt, pattern_pattern_id=@pat
                WHERE id=@id";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@desc", desc);
                    cmd.Parameters.AddWithValue("@art", article);
                    cmd.Parameters.AddWithValue("@man", manufacturer);
                    cmd.Parameters.AddWithValue("@w", width);
                    cmd.Parameters.AddWithValue("@l", length);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@stock", stock);
                    cmd.Parameters.AddWithValue("@cat", categoryId);
                    cmd.Parameters.AddWithValue("@mat", materialId);
                    cmd.Parameters.AddWithValue("@col", colorId);
                    cmd.Parameters.AddWithValue("@cnt", countryId);
                    cmd.Parameters.AddWithValue("@pat", patternId);
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { throw new Exception("Ошибка обновления товара: " + ex.Message); }
        }
        public static void AddProduct(string name, string desc, string article, string manufacturer,
            decimal width, decimal length, decimal price, int stock, int categoryId, int materialId, int colorId, int countryId, int patternId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = @"INSERT INTO products (name, description, article, manufacturer, width, length, price, stock_quantity, category_id, material_material_id, color_color_id, country_country_id, pattern_pattern_id) 
                           VALUES (@name, @desc, @art, @man, @w, @l, @price, @stock, @cat, @mat, @col, @cnt, @pat)";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@desc", desc);
                    cmd.Parameters.AddWithValue("@art", article);
                    cmd.Parameters.AddWithValue("@man", manufacturer);
                    cmd.Parameters.AddWithValue("@w", width);
                    cmd.Parameters.AddWithValue("@l", length);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@stock", stock);
                    cmd.Parameters.AddWithValue("@cat", categoryId);
                    cmd.Parameters.AddWithValue("@mat", materialId);
                    cmd.Parameters.AddWithValue("@col", colorId);
                    cmd.Parameters.AddWithValue("@cnt", countryId);
                    cmd.Parameters.AddWithValue("@pat", patternId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { throw new Exception("Ошибка добавления товара: " + ex.Message); }
        }
        public static DataTable GetCountries()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT country_id, name FROM country", conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public static DataTable GetMaterials()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT material_id, name FROM material", conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public static DataTable GetColors()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT color_id, name FROM color", conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public static DataTable GetPatterns()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT pattern_id, name FROM pattern", conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public static DataTable GetUserInfo(string login)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT full_name, phone FROM users WHERE login = @login";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@login", login);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public static DataTable GetUserById(int userId)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT user_id, full_name, email, phone, login, pass, role FROM users WHERE user_id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", userId);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public static void UpdateUser(int userId, string fullName, string email, string phone, string login, string pass, string role)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = @"UPDATE users SET 
                            full_name = @fio, 
                            email = @email, 
                            phone = @phone, 
                            login = @login, 
                            pass = @pass, 
                            role = @role 
                           WHERE user_id = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@fio", fullName);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@pass", pass);
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { throw new Exception("Ошибка обновления пользователя: " + ex.Message); }
        }
        public static void DeleteProduct(int productId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "DELETE FROM products WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", productId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка удаления товара: " + ex.Message);
            }
        }
    }
}
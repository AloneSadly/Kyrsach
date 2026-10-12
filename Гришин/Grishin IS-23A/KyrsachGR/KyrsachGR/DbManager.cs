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
        public static DataTable GetOrders(int page, int pageSize, string searchText = "", string status = "")
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

                    sql += " ORDER BY r.sale_date DESC LIMIT @offset, @pageSize";

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
            catch (Exception ex)
            {
                throw new Exception("Ошибка загрузки заказов: " + ex.Message);
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
        public static DataTable GetReferenceData(string tableName, string searchColumn, string searchText = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string[] allowedTables = { "categories", "material", "colors", "patterns", "countries" };
                    if (Array.IndexOf(allowedTables, tableName) < 0)
                        throw new Exception("Недопустимая таблица");

                    string sql = $"SELECT * FROM {tableName}";

                    if (!string.IsNullOrEmpty(searchText))
                        sql += $" WHERE {searchColumn} LIKE @search";

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

        public static void AddReferenceItem(string tableName, string nameColumn, string nameValue,
            string descColumn = null, string descValue = null)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string sql;
                    MySqlCommand cmd;

                    if (descColumn != null)
                    {
                        sql = $"INSERT INTO {tableName} ({nameColumn}, {descColumn}) VALUES (@name, @desc)";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@name", nameValue);
                        cmd.Parameters.AddWithValue("@desc", descValue ?? "");
                    }
                    else
                    {
                        sql = $"INSERT INTO {tableName} ({nameColumn}) VALUES (@name)";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@name", nameValue);
                    }

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка добавления: " + ex.Message);
            }
        }

        public static void UpdateReferenceItem(string tableName, string idColumn, int id,
            string nameColumn, string nameValue, string descColumn = null, string descValue = null)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string sql;
                    MySqlCommand cmd;

                    if (descColumn != null)
                    {
                        sql = $"UPDATE {tableName} SET {nameColumn} = @name, {descColumn} = @desc WHERE {idColumn} = @id";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@name", nameValue);
                        cmd.Parameters.AddWithValue("@desc", descValue ?? "");
                        cmd.Parameters.AddWithValue("@id", id);
                    }
                    else
                    {
                        sql = $"UPDATE {tableName} SET {nameColumn} = @name WHERE {idColumn} = @id";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@name", nameValue);
                        cmd.Parameters.AddWithValue("@id", id);
                    }

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
        public static DataTable GetProducts(int page, int pageSize, string searchText = "",
                                    string status = "", string sortOrder = "По дате")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = @"SELECT id, name, article, price, stock_quantity, color, country 
                           FROM products WHERE 1=1 ";

                    if (!string.IsNullOrEmpty(searchText))
                        sql += " AND name LIKE @search ";

                    if (status == "В наличии")
                        sql += " AND stock_quantity > 0 ";
                    else if (status == "Нет в наличии")
                        sql += " AND stock_quantity = 0 ";

                    switch (sortOrder)
                    {
                        case "По имени":
                            sql += " ORDER BY name ASC ";
                            break;
                        case "По цене":
                            sql += " ORDER BY price DESC ";
                            break;
                        default: 
                            sql += " ORDER BY id DESC ";
                            break;
                    }

                    sql += " LIMIT @offset, @pageSize";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    if (!string.IsNullOrEmpty(searchText))
                        cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                    cmd.Parameters.AddWithValue("@offset", (page - 1) * pageSize);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка загрузки товаров: " + ex.Message);
            }
        }

        public static int GetProductsCount(string searchText = "", string status = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "SELECT COUNT(*) FROM products WHERE 1=1 ";

                    if (!string.IsNullOrEmpty(searchText))
                        sql += " AND name LIKE @search ";
                    if (status == "В наличии")
                        sql += " AND stock_quantity > 0 ";
                    else if (status == "Нет в наличии")
                        sql += " AND stock_quantity = 0 ";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    if (!string.IsNullOrEmpty(searchText))
                        cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
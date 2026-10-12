cReate database kyrsach;
use kyrsach;

create table Users(
	user_id INT auto_increment primary key,
    full_name varchar(150) NOT NULL,
    email varchar(100),
    phone varchar(20) NOT NULL,
    login varchar(25) NOT NULL,
    pass varchar(50) NOT NULL,
    role ENUM('admin', 'seller','merchandiser')
);
CREATE table categories (
	categ_id INT auto_increment primary key,
    `name` varchar(100) NOT NULL,
    descript TEXT
);

CREATE TABLE products (
    id INT AUTO_INCREMENT PRIMARY KEY,
    `name` VARCHAR(200) NOT NULL,
    `description` TEXT,
    article VARCHAR(50),
    manufacturer VARCHAR(100),
    country VARCHAR(50),
    width DECIMAL(5,2),
    length DECIMAL(5,2),
    material VARCHAR(50),
    pattern VARCHAR(100),
    color VARCHAR(50),
    price DECIMAL(10,2) NOT NULL,
    stock_quantity INT NOT NULL DEFAULT 0,
    category_id INT NOT NULL,
    FOREIGN KEY (category_id) REFERENCES categories(categ_id)
        ON UPDATE CASCADE ON DELETE RESTRICT
);

CREATE TABLE receipts (
	receipts_id INT AUTO_INCREMENT PRIMARY KEY,
    sale_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    total_amount DECIMAL(10,2) NOT NULL DEFAULT 0,
    discount DECIMAL(5,2) NOT NULL DEFAULT 0,
    seller_id INT NOT NULL,
    customer_name VARCHAR(100) NOT NULL,
    customer_phone VARCHAR(20) NOT NULL,
    FOREIGN KEY (seller_id) REFERENCES users(user_id)
        ON UPDATE CASCADE ON DELETE RESTRICT
); 
CREATE TABLE receipt_items (
    receipt_items_id INT AUTO_INCREMENT PRIMARY KEY,
    receipt_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL,
    price_per_unit DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (receipt_id) REFERENCES receipts(receipts_id)
        ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES products(id)
        ON UPDATE CASCADE ON DELETE RESTRICT
); 
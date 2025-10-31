CREATE DATABASE WebBanLapTop;
GO

USE WebBanLapTop;
GO

CREATE TABLE category (
    id INT IDENTITY(1,1) PRIMARY KEY,
    category_name NVARCHAR(255) NOT NULL,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
);

CREATE TABLE [user] (
    id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(50) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    email NVARCHAR(100) NOT NULL UNIQUE,
    role NVARCHAR(20) NOT NULL DEFAULT 'customer' CHECK (role IN ('admin','customer')),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
);

CREATE TABLE product (
    id INT IDENTITY(1,1) PRIMARY KEY,
    category_id INT NOT NULL,
    name NVARCHAR(255) NOT NULL,
    description NVARCHAR(MAX) NULL,
    price DECIMAL(10,2) NOT NULL,
    stock INT DEFAULT 0,
    image_url NVARCHAR(500) NULL, -- Đường dẫn hình ảnh
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_product_category FOREIGN KEY (category_id)
        REFERENCES category(id) ON DELETE CASCADE
);

CREATE TABLE [order] (
    id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    total_amount DECIMAL(10,2) NOT NULL,
    status NVARCHAR(20) NOT NULL DEFAULT 'pending' 
        CHECK (status IN ('pending','paid','shipped','cancelled')),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_order_user FOREIGN KEY (user_id)
        REFERENCES [user](id) ON DELETE CASCADE
);

CREATE TABLE order_item (
    id INT IDENTITY(1,1) PRIMARY KEY,
    order_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL DEFAULT 1,
    price DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_orderitem_order FOREIGN KEY (order_id)
        REFERENCES [order](id) ON DELETE CASCADE,
    CONSTRAINT FK_orderitem_product FOREIGN KEY (product_id)
        REFERENCES product(id) ON DELETE CASCADE
);

-- Category
INSERT INTO category (category_name) VALUES
('Gaming Laptop'),
('Ultrabook'),
('Business Laptop');

-- User
INSERT INTO [user] (username, password, email, role) VALUES
('admin1', 'admin123', 'admin1@example.com', 'admin'),
('customer1', 'cust123', 'customer1@example.com', 'customer');

-- Product
INSERT INTO product (category_id, name, description, price, stock, image_url) VALUES
(1, N'Asus ROG Strix G15', N'Laptop gaming hiệu năng cao', 2500000, 10, '/images/asus_rog_g15.jpg'),
(1, N'MSI Katana GF66', N'Laptop gaming tầm trung', 1800000, 15, '/images/msi_katana_gf66.jpg'),
(2, N'Dell XPS 13', N'Ultrabook mỏng nhẹ cao cấp', 2200000, 8, '/images/dell_xps13.jpg'),
(2, N'HP Spectre x360', N'Ultrabook 2-in-1 tiện dụng', 2100000, 5, '/images/hp_spectre_x360.jpg'),
(3, N'Lenovo ThinkPad X1 Carbon', N'Laptop doanh nhân bền bỉ', 2400000, 7, '/images/thinkpad_x1_carbon.jpg');

-- Order
INSERT INTO [order] (user_id, total_amount, status) VALUES
(2, 4300000.00, 'paid'),    -- đơn hàng 1 của customer1: 2200000 + 2100000
(2, 1800000.00, 'pending'); -- đơn hàng 2 của customer1: 1800000

-- Order Item
INSERT INTO order_item (order_id, product_id, quantity, price) VALUES
-- Đơn hàng 1 (order_id = 1)
(3, 13, 1, 2200000.00),   -- Dell XPS 13
(3, 14, 1, 2100000.00),   -- HP Spectre x360

-- Đơn hàng 2 (order_id = 2)
(4, 12, 1, 1800000.00);   -- MSI Katana GF66





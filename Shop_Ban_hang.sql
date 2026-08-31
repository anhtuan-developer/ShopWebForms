CREATE DATABASE ShopWebForms;
GO

USE ShopWebForms;
GO
CREATE TABLE DanhMuc
(
    MaDanhMuc INT IDENTITY(1,1) PRIMARY KEY,

    TenDanhMuc NVARCHAR(100) NOT NULL,

    MoTa NVARCHAR(500),

    TrangThai BIT NOT NULL DEFAULT 1,

    NgayTao DATETIME NOT NULL DEFAULT GETDATE()
);
GO
CREATE TABLE SanPham
(
    MaSanPham INT IDENTITY(1,1) PRIMARY KEY,

    MaDanhMuc INT NOT NULL,

    TenSanPham NVARCHAR(200) NOT NULL,

    MoTa NVARCHAR(MAX),

    Gia DECIMAL(18,2) NOT NULL,

    SoLuong INT NOT NULL DEFAULT 0,

    HinhAnh NVARCHAR(500),

    TrangThai BIT NOT NULL DEFAULT 1,

    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_SanPham_DanhMuc
        FOREIGN KEY (MaDanhMuc)
        REFERENCES DanhMuc(MaDanhMuc)
);
GO

INSERT INTO DanhMuc
(
    TenDanhMuc,
    MoTa
)
VALUES
(
    N'Điện thoại',
    N'Các loại điện thoại và smartphone'
),
(
    N'Laptop',
    N'Laptop phục vụ học tập và công việc'
),
(
    N'Phụ kiện',
    N'Các phụ kiện công nghệ'
),
(
    N'Đồng hồ',
    N'Đồng hồ thông minh và đồng hồ thời trang'
);
GO
INSERT INTO SanPham
(
    MaDanhMuc,
    TenSanPham,
    MoTa,
    Gia,
    SoLuong,
    HinhAnh
)
VALUES
(
    1,
    N'iPhone 15 Pro',
    N'iPhone 15 Pro chính hãng',
    24990000,
    20,
    N'iphone-15-pro.jpg'
),
(
    1,
    N'Samsung Galaxy S25',
    N'Smartphone Samsung Galaxy S25',
    19990000,
    15,
    N'samsung-s25.jpg'
),
(
    2,
    N'ASUS Vivobook 15',
    N'Laptop ASUS Vivobook 15',
    15990000,
    10,
    N'asus-vivobook-15.jpg'
),
(
    2,
    N'Dell Inspiron 15',
    N'Laptop Dell Inspiron 15',
    14990000,
    12,
    N'dell-inspiron-15.jpg'
),
(
    3,
    N'Tai nghe Bluetooth',
    N'Tai nghe Bluetooth không dây',
    1290000,
    30,
    N'tai-nghe-bluetooth.jpg'
),
(
    3,
    N'Chuột Logitech',
    N'Chuột Logitech không dây',
    590000,
    25,
    N'chuot-logitech.jpg'
),
(
    4,
    N'Apple Watch',
    N'Apple Watch chính hãng',
    8490000,
    8,
    N'apple-watch.jpg'
),
(
    4,
    N'Samsung Galaxy Watch',
    N'Samsung Galaxy Watch',
    5990000,
    10,
    N'samsung-watch.jpg'
);
GO

CREATE TABLE KhachHang
(
    MaKhachHang INT IDENTITY(1,1) PRIMARY KEY,

    HoTen NVARCHAR(100) NOT NULL,

    Email VARCHAR(150) NOT NULL UNIQUE,

    MatKhau VARCHAR(255) NOT NULL,

    SoDienThoai VARCHAR(20) NULL,

    DiaChi NVARCHAR(255) NULL,

    NgayTao DATETIME NOT NULL
        DEFAULT GETDATE()
);
GO

CREATE TABLE DonHang
(
    MaDonHang INT IDENTITY(1,1) PRIMARY KEY,

    MaKhachHang INT NULL,

    HoTenNguoiNhan NVARCHAR(100) NOT NULL,

    SoDienThoai VARCHAR(20) NOT NULL,

    DiaChiGiaoHang NVARCHAR(255) NOT NULL,

    TongTien DECIMAL(18,2) NOT NULL,

    TrangThai NVARCHAR(50) NOT NULL
        DEFAULT N'Chờ xử lý',

    NgayDat DATETIME NOT NULL
        DEFAULT GETDATE(),

    CONSTRAINT FK_DonHang_KhachHang
        FOREIGN KEY (MaKhachHang)
        REFERENCES KhachHang(MaKhachHang)
);
GO

CREATE TABLE ChiTietDonHang
(
    MaChiTiet INT IDENTITY(1,1) PRIMARY KEY,

    MaDonHang INT NOT NULL,

    MaSanPham INT NOT NULL,

    SoLuong INT NOT NULL,

    DonGia DECIMAL(18,2) NOT NULL,

    ThanhTien AS (SoLuong * DonGia),

    CONSTRAINT FK_ChiTietDonHang_DonHang
        FOREIGN KEY (MaDonHang)
        REFERENCES DonHang(MaDonHang),

    CONSTRAINT FK_ChiTietDonHang_SanPham
        FOREIGN KEY (MaSanPham)
        REFERENCES SanPham(MaSanPham)
);
GO

CREATE TABLE Admin
(
    MaAdmin INT IDENTITY(1,1) PRIMARY KEY,

    HoTen NVARCHAR(100) NOT NULL,

    Email VARCHAR(150) NOT NULL UNIQUE,

    MatKhau VARCHAR(255) NOT NULL,

    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),

    TrangThai BIT NOT NULL DEFAULT 1
);
GO

INSERT INTO Admin
(
    HoTen,
    Email,
    MatKhau,
    TrangThai
)
VALUES
(
    N'Quản trị viên',
    'admin@gmail.com',
    '123456',
    1
);
GO

CREATE TABLE DanhGia
    (
        MaDanhGia INT IDENTITY(1,1) NOT NULL,

        MaSanPham INT NOT NULL,

        MaKhachHang INT NOT NULL,

        NoiDung NVARCHAR(2000) NOT NULL,

        SoSao INT NOT NULL,

        NgayDanhGia DATETIME NOT NULL
            CONSTRAINT DF_DanhGia_NgayDanhGia
            DEFAULT GETDATE(),

        TrangThai BIT NOT NULL
            CONSTRAINT DF_DanhGia_TrangThai
            DEFAULT 1,

        CONSTRAINT PK_DanhGia
            PRIMARY KEY (MaDanhGia),

        CONSTRAINT CK_DanhGia_SoSao
            CHECK (SoSao BETWEEN 1 AND 5),

        CONSTRAINT FK_DanhGia_SanPham
            FOREIGN KEY (MaSanPham)
            REFERENCES SanPham(MaSanPham),

        CONSTRAINT FK_DanhGia_KhachHang
            FOREIGN KEY (MaKhachHang)
            REFERENCES KhachHang(MaKhachHang)
    );
GO

CREATE TABLE TinTuc
(
    MaTinTuc INT IDENTITY(1,1) PRIMARY KEY,

    TieuDe NVARCHAR(250) NOT NULL,

    NoiDung NVARCHAR(MAX) NOT NULL,

    HinhAnh NVARCHAR(500) NULL,

    TrangThai BIT NOT NULL DEFAULT 1,

    NgayTao DATETIME NOT NULL DEFAULT GETDATE()
);
GO

INSERT INTO TinTuc
(
    TieuDe,
    NoiDung,
    HinhAnh,
    TrangThai
)
VALUES
(
    N'Chào mừng đến với SHOP 5 ANH EM',
    N'Website bán hàng trực tuyến cung cấp các sản phẩm công nghệ và phụ kiện. Đây là bài viết mẫu để kiểm tra hệ thống tin tức.',
    N'about.jpg',
    1
),
(
    N'Cập nhật sản phẩm và ưu đãi mới',
    N'Thường xuyên theo dõi mục Tin tức để cập nhật sản phẩm mới, chương trình ưu đãi và các thông tin từ cửa hàng.',
    N'carousel-1.jpg',
    1
);
GO

CREATE TABLE BinhLuan
(
    MaBinhLuan INT IDENTITY(1,1) PRIMARY KEY,

    MaTinTuc INT NOT NULL,

    MaKhachHang INT NOT NULL,

    NoiDung NVARCHAR(1000) NOT NULL,

    TrangThai BIT NOT NULL DEFAULT 1,

    NgayBinhLuan DATETIME NOT NULL
        DEFAULT GETDATE(),

    CONSTRAINT FK_BinhLuan_TinTuc
        FOREIGN KEY (MaTinTuc)
        REFERENCES TinTuc(MaTinTuc),

    CONSTRAINT FK_BinhLuan_KhachHang
        FOREIGN KEY (MaKhachHang)
        REFERENCES KhachHang(MaKhachHang)
);
GO

CREATE TABLE LienHe
    (
        MaLienHe INT IDENTITY(1,1) PRIMARY KEY,

        HoTen NVARCHAR(100) NOT NULL,

        Email VARCHAR(150) NOT NULL,

        TieuDe NVARCHAR(250) NOT NULL,

        NoiDung NVARCHAR(MAX) NOT NULL,

        NgayGui DATETIME NOT NULL
            CONSTRAINT DF_LienHe_NgayGui
            DEFAULT GETDATE(),

        TrangThai BIT NOT NULL
            CONSTRAINT DF_LienHe_TrangThai
            DEFAULT 0
    );
GO


CREATE TABLE dbo.PasswordResetToken
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    MaKhachHang INT NOT NULL,

    TokenHash VARCHAR(64) NOT NULL,

    ExpiresAt DATETIME NOT NULL,

    Used BIT NOT NULL
        DEFAULT 0,

    CreatedAt DATETIME NOT NULL
        DEFAULT GETDATE(),


    CONSTRAINT FK_PasswordResetToken_KhachHang

        FOREIGN KEY (MaKhachHang)

        REFERENCES dbo.KhachHang(MaKhachHang)
);


CREATE INDEX
    IX_PasswordResetToken_TokenHash

ON dbo.PasswordResetToken(TokenHash);

GO

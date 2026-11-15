/*
    Fresh installation script for StudyCafeManagement.
    Run the whole file in SSMS, connected to the SQL Server instance used by App.config.
    This script creates a new database and deliberately stops if it already exists,
    so it cannot overwrite an existing database or its data.

    Sample accounts (all passwords are 123456):
      admin / 123456
      thungan01 / 123456
      phache01 / 123456
*/
USE [master];
GO

IF DB_ID(N'QuanLyCaPheHocTap') IS NOT NULL
    THROW 50000, N'Database QuanLyCaPheHocTap already exists. This fresh-install script did not modify it.', 1;
GO

CREATE DATABASE [QuanLyCaPheHocTap] COLLATE Vietnamese_CI_AS;
GO

USE [QuanLyCaPheHocTap];
GO

CREATE TABLE dbo.VAITRO
(
    MaNhom varchar(20) NOT NULL,
    TenNhom nvarchar(50) NOT NULL,
    CONSTRAINT PK_VaiTro PRIMARY KEY (MaNhom),
    CONSTRAINT CK_VaiTro_TenNhom CHECK (TenNhom IN (N'Admin', N'Thu ngân', N'Pha chế'))
);
GO

CREATE TABLE dbo.NGUOIDUNG
(
    MaNV varchar(20) NOT NULL,
    TenDangNhap varchar(50) NOT NULL,
    MatKhau varchar(256) NOT NULL,
    MaNhomQuyen varchar(20) NOT NULL,
    TrangThai nvarchar(30) NOT NULL CONSTRAINT DF_NGUOIDUNG_TrangThai DEFAULT N'Đang hoạt động',
    CONSTRAINT PK_NguoiDung PRIMARY KEY (MaNV),
    CONSTRAINT UQ_NguoiDung_TenDangNhap UNIQUE (TenDangNhap),
    CONSTRAINT FK_NguoiDung_VaiTro FOREIGN KEY (MaNhomQuyen) REFERENCES dbo.VAITRO(MaNhom),
    CONSTRAINT CK_NguoiDung_TrangThai CHECK (TrangThai IN (N'Đang hoạt động', N'Đã khóa'))
);
GO

CREATE TABLE dbo.DANHMUCMON
(
    MaDanhMuc varchar(20) NOT NULL,
    TenDanhMuc nvarchar(100) NOT NULL,
    CONSTRAINT PK_DanhMucMon PRIMARY KEY (MaDanhMuc)
);
GO

CREATE TABLE dbo.SANPHAM
(
    MaMon varchar(20) NOT NULL,
    TenMon nvarchar(100) NOT NULL,
    MaDanhMuc varchar(20) NOT NULL,
    DonGia decimal(18, 2) NOT NULL,
    HinhAnh nvarchar(255) NULL,
    TrangThai nvarchar(30) NOT NULL CONSTRAINT DF_SANPHAM_TrangThai DEFAULT N'Còn',
    CONSTRAINT PK_SanPham PRIMARY KEY (MaMon),
    CONSTRAINT FK_SanPham_DanhMuc FOREIGN KEY (MaDanhMuc) REFERENCES dbo.DANHMUCMON(MaDanhMuc),
    CONSTRAINT CK_SanPham_DonGia CHECK (DonGia > 0),
    CONSTRAINT CK_SanPham_TrangThai CHECK (TrangThai IN (N'Còn', N'Hết', N'Ngưng bán'))
);
GO

CREATE TABLE dbo.BAN
(
    MaBan varchar(20) NOT NULL,
    TenBan nvarchar(50) NOT NULL,
    DonGiaGio decimal(18, 2) NOT NULL CONSTRAINT DF_BAN_DonGiaGio DEFAULT 15000,
    TrangThai nvarchar(30) NOT NULL CONSTRAINT DF_BAN_TrangThai DEFAULT N'Trống',
    CONSTRAINT PK_Ban PRIMARY KEY (MaBan),
    CONSTRAINT CK_Ban_DonGiaGio CHECK (DonGiaGio > 0),
    CONSTRAINT CK_Ban_TrangThai CHECK (TrangThai IN (N'Trống', N'Đang phục vụ'))
);
GO

CREATE TABLE dbo.KHACHHANG
(
    MaKH varchar(20) NOT NULL,
    HoTen nvarchar(100) NOT NULL,
    SDT varchar(15) NOT NULL,
    DiemTichLuy int NOT NULL CONSTRAINT DF_KHACHHANG_DiemTichLuy DEFAULT 0,
    HangKhach nvarchar(30) NOT NULL CONSTRAINT DF_KHACHHANG_HangKhach DEFAULT N'Thường',
    CONSTRAINT PK_KhachHang PRIMARY KEY (MaKH),
    CONSTRAINT UQ_KhachHang_SDT UNIQUE (SDT),
    CONSTRAINT CK_KhachHang_DiemTichLuy CHECK (DiemTichLuy >= 0),
    CONSTRAINT CK_KhachHang_HangKhach CHECK (HangKhach IN (N'Thường', N'Thân thiết'))
);
GO

CREATE TABLE dbo.NGUYENLIEU
(
    MaNL varchar(20) NOT NULL,
    TenNL nvarchar(100) NOT NULL,
    SoLuongTon int NOT NULL CONSTRAINT DF_NGUYENLIEU_SoLuongTon DEFAULT 0,
    DonViTinh nvarchar(30) NOT NULL,
    CONSTRAINT PK_NguyenLieu PRIMARY KEY (MaNL),
    CONSTRAINT CK_NguyenLieu_SoLuongTon CHECK (SoLuongTon >= 0)
);
GO

CREATE TABLE dbo.CONGTHUC
(
    MaMon varchar(20) NOT NULL,
    MaNL varchar(20) NOT NULL,
    SoLuongCanDung int NOT NULL,
    CONSTRAINT PK_CongThuc PRIMARY KEY (MaMon, MaNL),
    CONSTRAINT FK_CongThuc_SanPham FOREIGN KEY (MaMon) REFERENCES dbo.SANPHAM(MaMon),
    CONSTRAINT FK_CongThuc_NguyenLieu FOREIGN KEY (MaNL) REFERENCES dbo.NGUYENLIEU(MaNL),
    CONSTRAINT CK_CongThuc_SoLuongCanDung CHECK (SoLuongCanDung > 0)
);
GO

CREATE TABLE dbo.HOADON
(
    MaHD varchar(20) NOT NULL,
    MaBan varchar(20) NULL,
    MaNV varchar(20) NOT NULL,
    MaKH varchar(20) NULL,
    NgayLap datetime NOT NULL CONSTRAINT DF_HOADON_NgayLap DEFAULT GETDATE(),
    SoGioSuDung decimal(5, 2) NOT NULL CONSTRAINT DF_HOADON_SoGioSuDung DEFAULT 0,
    DonGiaGioApDung decimal(18, 2) NOT NULL CONSTRAINT DF_HOADON_DonGiaGioApDung DEFAULT 0,
    TongTienTruocGiam decimal(18, 2) NOT NULL CONSTRAINT DF_HOADON_TongTienTruocGiam DEFAULT 0,
    TienGiamGia decimal(18, 2) NOT NULL CONSTRAINT DF_HOADON_TienGiamGia DEFAULT 0,
    TongThanhToan decimal(18, 2) NOT NULL CONSTRAINT DF_HOADON_TongThanhToan DEFAULT 0,
    TrangThai nvarchar(30) NOT NULL CONSTRAINT DF_HOADON_TrangThai DEFAULT N'Đang order',
    CONSTRAINT PK_HoaDon PRIMARY KEY (MaHD),
    CONSTRAINT FK_HoaDon_Ban FOREIGN KEY (MaBan) REFERENCES dbo.BAN(MaBan),
    CONSTRAINT FK_HoaDon_NguoiDung FOREIGN KEY (MaNV) REFERENCES dbo.NGUOIDUNG(MaNV),
    CONSTRAINT FK_HoaDon_KhachHang FOREIGN KEY (MaKH) REFERENCES dbo.KHACHHANG(MaKH),
    CONSTRAINT CK_HoaDon_TrangThai CHECK (TrangThai IN (N'Đang order', N'Đã thanh toán', N'Đã hủy')),
    CONSTRAINT CK_HoaDon_HopLeBanVaGio CHECK
    (
        (MaBan IS NULL AND SoGioSuDung = 0 AND DonGiaGioApDung = 0)
        OR (MaBan IS NOT NULL AND SoGioSuDung >= 0 AND DonGiaGioApDung >= 0)
    )
);
GO

CREATE TABLE dbo.CHITIETHOADON
(
    MaHD varchar(20) NOT NULL,
    MaMon varchar(20) NOT NULL,
    SoLuong int NOT NULL,
    DonGiaBan decimal(18, 2) NOT NULL,
    GhiChu nvarchar(255) NULL,
    CONSTRAINT PK_ChiTietHoaDon PRIMARY KEY (MaHD, MaMon),
    CONSTRAINT FK_CTHD_HoaDon FOREIGN KEY (MaHD) REFERENCES dbo.HOADON(MaHD),
    CONSTRAINT FK_CTHD_SanPham FOREIGN KEY (MaMon) REFERENCES dbo.SANPHAM(MaMon),
    CONSTRAINT CK_CTHD_SoLuong CHECK (SoLuong >= 1),
    CONSTRAINT CK_CTHD_DonGiaBan CHECK (DonGiaBan > 0)
);
GO

CREATE TRIGGER dbo.trg_HoaDon_QuanLyMoBan
ON dbo.HOADON
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        JOIN dbo.BAN b ON i.MaBan = b.MaBan
        WHERE i.MaBan IS NOT NULL
          AND i.TrangThai = N'Đang order'
          AND
          (
              b.TrangThai = N'Đang phục vụ'
              OR EXISTS
              (
                  SELECT 1
                  FROM dbo.HOADON h
                  WHERE h.MaBan = i.MaBan
                    AND h.MaHD <> i.MaHD
                    AND h.TrangThai = N'Đang order'
              )
          )
    )
    BEGIN
        RAISERROR (N'Bàn này đang ở trạng thái Đang phục vụ, không được mở thêm hóa đơn mới!', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    UPDATE b
    SET b.TrangThai = N'Đang phục vụ'
    FROM dbo.BAN b
    JOIN inserted i ON b.MaBan = i.MaBan
    WHERE i.MaBan IS NOT NULL
      AND i.TrangThai = N'Đang order';

    UPDATE h
    SET h.DonGiaGioApDung = b.DonGiaGio
    FROM dbo.HOADON h
    JOIN inserted i ON h.MaHD = i.MaHD
    JOIN dbo.BAN b ON i.MaBan = b.MaBan
    WHERE i.MaBan IS NOT NULL
      AND h.DonGiaGioApDung = 0;
END;
GO

CREATE TRIGGER dbo.trg_ChiTietHoaDon_KiemTraKhoVaKhoaSuaDoi
ON dbo.CHITIETHOADON
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM (SELECT MaHD FROM inserted UNION SELECT MaHD FROM deleted) changed
        JOIN dbo.HOADON h ON changed.MaHD = h.MaHD
        WHERE h.TrangThai = N'Đã thanh toán'
    )
    BEGIN
        RAISERROR (N'Không được phép chỉnh sửa hoặc xóa chi tiết của hóa đơn Đã thanh toán!', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM inserted)
    BEGIN
        IF EXISTS
        (
            SELECT 1
            FROM
            (
                SELECT ct.MaNL, SUM((i.SoLuong - ISNULL(d.SoLuong, 0)) * ct.SoLuongCanDung) AS LuongCanDung
                FROM inserted i
                LEFT JOIN deleted d ON i.MaHD = d.MaHD AND i.MaMon = d.MaMon
                JOIN dbo.CONGTHUC ct ON i.MaMon = ct.MaMon
                GROUP BY ct.MaNL
            ) YeuCau
            JOIN dbo.NGUYENLIEU nl ON YeuCau.MaNL = nl.MaNL
            WHERE YeuCau.LuongCanDung > 0
              AND nl.SoLuongTon < YeuCau.LuongCanDung
        )
        BEGIN
            RAISERROR (N'Hết nguyên liệu! Số lượng tồn kho không đủ để chế biến món này.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
    END
END;
GO

CREATE TRIGGER dbo.trg_HoaDon_ThanhToanVaKhoaHoaDon
ON dbo.HOADON
AFTER UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM deleted WHERE TrangThai = N'Đã thanh toán')
    BEGIN
        RAISERROR (N'Hóa đơn Đã thanh toán bị khóa vĩnh viễn, không được phép chỉnh sửa hoặc xóa!', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        JOIN deleted d ON i.MaHD = d.MaHD
        WHERE d.TrangThai = N'Đang order'
          AND i.TrangThai = N'Đã thanh toán'
    )
    BEGIN
        IF EXISTS
        (
            SELECT 1
            FROM inserted i
            WHERE i.TrangThai = N'Đã thanh toán'
              AND i.MaBan IS NULL
              AND NOT EXISTS
              (
                  SELECT 1
                  FROM dbo.CHITIETHOADON ct
                  WHERE ct.MaHD = i.MaHD
              )
        )
        BEGIN
            RAISERROR (N'Hóa đơn mua mang đi bắt buộc phải có ít nhất 1 món!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        IF EXISTS
        (
            SELECT 1
            FROM inserted i
            WHERE i.TrangThai = N'Đã thanh toán'
              AND i.MaBan IS NOT NULL
              AND i.SoGioSuDung <= 0
        )
        BEGIN
            RAISERROR (N'Hóa đơn thuê bàn học khi thanh toán bắt buộc phải có số giờ sử dụng lớn hơn 0!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        IF EXISTS
        (
            SELECT 1
            FROM
            (
                SELECT ct.MaNL, SUM(cthd.SoLuong * ct.SoLuongCanDung) AS TongCanTru
                FROM inserted i
                JOIN deleted d ON i.MaHD = d.MaHD
                JOIN dbo.CHITIETHOADON cthd ON i.MaHD = cthd.MaHD
                JOIN dbo.CONGTHUC ct ON cthd.MaMon = ct.MaMon
                WHERE d.TrangThai = N'Đang order'
                  AND i.TrangThai = N'Đã thanh toán'
                GROUP BY ct.MaNL
            ) XuatKho
            JOIN dbo.NGUYENLIEU nl ON XuatKho.MaNL = nl.MaNL
            WHERE nl.SoLuongTon < XuatKho.TongCanTru
        )
        BEGIN
            RAISERROR (N'Hết nguyên liệu trong kho khi thực hiện thanh toán!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        UPDATE nl
        SET nl.SoLuongTon = nl.SoLuongTon - XuatKho.TongCanTru
        FROM dbo.NGUYENLIEU nl
        JOIN
        (
            SELECT ct.MaNL, SUM(cthd.SoLuong * ct.SoLuongCanDung) AS TongCanTru
            FROM inserted i
            JOIN deleted d ON i.MaHD = d.MaHD
            JOIN dbo.CHITIETHOADON cthd ON i.MaHD = cthd.MaHD
            JOIN dbo.CONGTHUC ct ON cthd.MaMon = ct.MaMon
            WHERE d.TrangThai = N'Đang order'
              AND i.TrangThai = N'Đã thanh toán'
            GROUP BY ct.MaNL
        ) XuatKho ON nl.MaNL = XuatKho.MaNL;

        UPDATE h
        SET TongTienTruocGiam = (h.SoGioSuDung * h.DonGiaGioApDung) + ISNULL(TienMon.TongMon, 0),
            TienGiamGia = CASE
                WHEN kh.HangKhach = N'Thân thiết'
                THEN 0.10 * ((h.SoGioSuDung * h.DonGiaGioApDung) + ISNULL(TienMon.TongMon, 0))
                ELSE 0
            END,
            TongThanhToan = CASE
                WHEN kh.HangKhach = N'Thân thiết'
                THEN 0.90 * ((h.SoGioSuDung * h.DonGiaGioApDung) + ISNULL(TienMon.TongMon, 0))
                ELSE ((h.SoGioSuDung * h.DonGiaGioApDung) + ISNULL(TienMon.TongMon, 0))
            END
        FROM dbo.HOADON h
        JOIN inserted i ON h.MaHD = i.MaHD
        JOIN deleted d ON i.MaHD = d.MaHD
        LEFT JOIN dbo.KHACHHANG kh ON h.MaKH = kh.MaKH
        OUTER APPLY
        (
            SELECT SUM(SoLuong * DonGiaBan) AS TongMon
            FROM dbo.CHITIETHOADON cthd
            WHERE cthd.MaHD = h.MaHD
        ) TienMon
        WHERE d.TrangThai = N'Đang order'
          AND i.TrangThai = N'Đã thanh toán';

        UPDATE kh
        SET DiemTichLuy = kh.DiemTichLuy + FLOOR(h.TongThanhToan / 10000.0),
            HangKhach = CASE
                WHEN kh.DiemTichLuy + FLOOR(h.TongThanhToan / 10000.0) >= 100
                THEN N'Thân thiết'
                ELSE kh.HangKhach
            END
        FROM dbo.KHACHHANG kh
        JOIN dbo.HOADON h ON kh.MaKH = h.MaKH
        JOIN inserted i ON h.MaHD = i.MaHD
        JOIN deleted d ON i.MaHD = d.MaHD
        WHERE d.TrangThai = N'Đang order'
          AND i.TrangThai = N'Đã thanh toán';

        UPDATE b
        SET TrangThai = N'Trống'
        FROM dbo.BAN b
        JOIN inserted i ON b.MaBan = i.MaBan
        JOIN deleted d ON i.MaHD = d.MaHD
        WHERE d.TrangThai = N'Đang order'
          AND i.TrangThai = N'Đã thanh toán';
    END

    UPDATE b
    SET TrangThai = N'Trống'
    FROM dbo.BAN b
    JOIN inserted i ON b.MaBan = i.MaBan
    JOIN deleted d ON i.MaHD = d.MaHD
    WHERE d.TrangThai = N'Đang order'
      AND i.TrangThai = N'Đã hủy';
END;
GO

INSERT INTO dbo.VAITRO (MaNhom, TenNhom) VALUES
('ROLE_ADMIN', N'Admin'),
('ROLE_CASHIER', N'Thu ngân'),
('ROLE_BARISTA', N'Pha chế');

INSERT INTO dbo.NGUOIDUNG (MaNV, TenDangNhap, MatKhau, MaNhomQuyen, TrangThai) VALUES
('NV01', 'admin', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'ROLE_ADMIN', N'Đang hoạt động'),
('NV02', 'thungan01', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'ROLE_CASHIER', N'Đang hoạt động'),
('NV03', 'phache01', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'ROLE_BARISTA', N'Đang hoạt động');

INSERT INTO dbo.DANHMUCMON (MaDanhMuc, TenDanhMuc) VALUES
('DM_NUOC', N'Nước'),
('DM_BANH', N'Bánh'),
('DM_DOAN', N'Đồ ăn');

INSERT INTO dbo.SANPHAM (MaMon, TenMon, MaDanhMuc, DonGia, HinhAnh, TrangThai) VALUES
('MON01', N'Cà phê sữa đá', 'DM_NUOC', 29000, N'caphesuada.jpg', N'Còn'),
('MON02', N'Trà sữa trân châu đường đen', 'DM_NUOC', 35000, N'trasua.jpg', N'Còn'),
('MON03', N'Nước ngọt Coca-Cola', 'DM_NUOC', 18000, N'cocacola.jpg', N'Còn'),
('MON04', N'Snack khoai tây', 'DM_BANH', 15000, N'snack_khoaitay.jpg', N'Còn'),
('MON05', N'Bánh sừng bò Croissant', 'DM_BANH', 28000, N'croissant.jpg', N'Còn'),
('MON06', N'Mỳ trộn xúc xích trứng lòng đào', 'DM_DOAN', 42000, N'mytron.jpg', N'Còn'),
('MON07', N'Mỳ trộn bò bằm sốt cay', 'DM_DOAN', 48000, N'mytron_bo.jpg', N'Còn');

INSERT INTO dbo.NGUYENLIEU (MaNL, TenNL, SoLuongTon, DonViTinh) VALUES
('NL01', N'Hạt cà phê rang xay', 5000, N'gram'),
('NL02', N'Sữa đặc', 3000, N'ml'),
('NL03', N'Cốt trà đen', 4000, N'ml'),
('NL04', N'Trân châu đường đen', 2000, N'gram'),
('NL05', N'Lon Coca-Cola', 100, N'lon'),
('NL06', N'Gói Snack khoai tây', 80, N'gói'),
('NL07', N'Cốt bánh Croissant cấp đông', 50, N'cái'),
('NL08', N'Vắt mỳ khô', 120, N'vắt'),
('NL09', N'Xúc xích tiệt trùng', 150, N'cây'),
('NL10', N'Trứng gà tươi', 100, N'quả'),
('NL11', N'Thịt bò bằm ướp sẵn', 3000, N'gram');

INSERT INTO dbo.CONGTHUC (MaMon, MaNL, SoLuongCanDung) VALUES
('MON01', 'NL01', 25),
('MON01', 'NL02', 30),
('MON02', 'NL03', 150),
('MON02', 'NL04', 50),
('MON03', 'NL05', 1),
('MON04', 'NL06', 1),
('MON05', 'NL07', 1),
('MON06', 'NL08', 1),
('MON06', 'NL09', 1),
('MON06', 'NL10', 1),
('MON07', 'NL08', 1),
('MON07', 'NL11', 80);

INSERT INTO dbo.KHACHHANG (MaKH, HoTen, SDT, DiemTichLuy, HangKhach) VALUES
('KH01', N'Nguyễn Minh Huy', '0901234567', 120, N'Thân thiết'),
('KH02', N'Trần Ngọc Minh', '0912345678', 45, N'Thường');
GO

MERGE dbo.BAN AS target
USING (VALUES
    ('T1-D01', N'Bàn đơn 01', 7500),
    ('T1-D02', N'Bàn đơn 02', 7500),
    ('T1-D03', N'Bàn đơn 03', 7500),
    ('T1-D04', N'Bàn đơn 04', 7500),
    ('T1-D05', N'Bàn đơn 05', 7500),
    ('T1-D06', N'Bàn đơn 06', 7500),
    ('T1-D07', N'Bàn đơn 07', 7500),
    ('T1-D08', N'Bàn đơn 08', 7500),
    ('T1-D09', N'Bàn đơn 09', 7500),
    ('T1-D10', N'Bàn đơn 10', 7500),
    ('T1-D11', N'Bàn đơn 11', 7500),
    ('T1-D12', N'Bàn đơn 12', 7500),
    ('T1-P01', N'Bàn đôi 01', 18000),
    ('T1-P02', N'Bàn đôi 02', 18000),
    ('T1-P03', N'Bàn đôi 03', 18000),
    ('T1-P04', N'Bàn đôi 04', 18000),
    ('T1-P05', N'Bàn đôi 05', 18000),
    ('T1-P06', N'Bàn đôi 06', 18000),
    ('T1-P07', N'Bàn đôi 07', 18000),
    ('T1-P08', N'Bàn đôi 08', 18000),
    ('T1-P09', N'Bàn đôi 09', 18000),
    ('T1-P10', N'Bàn đôi 10', 18000),
    ('T1-P11', N'Bàn đôi 11', 18000),
    ('T1-P12', N'Bàn đôi 12', 18000),
    ('T1-R01', N'Bàn tròn 01', 40000),
    ('T1-R02', N'Bàn tròn 02', 40000),
    ('T1-R03', N'Bàn tròn 03', 40000),
    ('T1-R04', N'Bàn tròn 04', 40000),
    ('T1-R05', N'Bàn tròn 05', 40000),
    ('T1-R06', N'Bàn tròn 06', 40000),
    ('T2-R01', N'Bàn tròn 07', 40000),
    ('T2-R02', N'Bàn tròn 08', 40000),
    ('T2-R03', N'Bàn tròn 09', 40000),
    ('T2-R04', N'Bàn tròn 10', 40000),
    ('T2-R05', N'Bàn tròn 11', 40000),
    ('T2-R06', N'Bàn tròn 12', 40000),
    ('T2-C01', N'Bàn 10-20 người số 1', 100000),
    ('T2-C02', N'Bàn 10-20 người số 2', 100000),
    ('T2-C03', N'Bàn 10-20 người số 3', 100000),
    ('T2-C04', N'Bàn 10-20 người số 4', 100000),
    ('T2-L01', N'Bàn 50 người', 250000)
) AS source (MaBan, TenBan, DonGiaGio)
ON target.MaBan = source.MaBan
WHEN MATCHED THEN
    UPDATE SET TenBan = source.TenBan, DonGiaGio = source.DonGiaGio
WHEN NOT MATCHED THEN
    INSERT (MaBan, TenBan, DonGiaGio, TrangThai)
    VALUES (source.MaBan, source.TenBan, source.DonGiaGio, N'Trống');
GO

SELECT N'Database QuanLyCaPheHocTap created.' AS Result;
SELECT N'BAN' AS TableName, COUNT(*) AS RecordCount FROM dbo.BAN
UNION ALL SELECT N'VAITRO', COUNT(*) FROM dbo.VAITRO
UNION ALL SELECT N'NGUOIDUNG', COUNT(*) FROM dbo.NGUOIDUNG
UNION ALL SELECT N'DANHMUCMON', COUNT(*) FROM dbo.DANHMUCMON
UNION ALL SELECT N'SANPHAM', COUNT(*) FROM dbo.SANPHAM
UNION ALL SELECT N'NGUYENLIEU', COUNT(*) FROM dbo.NGUYENLIEU
UNION ALL SELECT N'CONGTHUC', COUNT(*) FROM dbo.CONGTHUC
UNION ALL SELECT N'KHACHHANG', COUNT(*) FROM dbo.KHACHHANG
ORDER BY TableName;
GO

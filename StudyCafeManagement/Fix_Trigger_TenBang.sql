USE [QuanLyCaPheHocTap];
SET NOCOUNT ON;

IF OBJECT_ID(N'dbo.CHITIETHOADON', N'U') IS NULL
    THROW 50001, N'Không tìm thấy bảng dbo.CHITIETHOADON; hãy tạo/khôi phục CSDL trước.', 1;

DECLARE @hoaDonTriggerId int = OBJECT_ID(N'dbo.trg_HoaDon_ThanhToanVaKhoaHoaDon', N'TR');
DECLARE @chiTietTriggerId int = OBJECT_ID(N'dbo.trg_ChiTietHoaDon_KiemTraKhoVaKhoaSuaDoi', N'TR');
DECLARE @definition nvarchar(max);
DECLARE @sql nvarchar(max);
DECLARE @alterIndex int;
DECLARE @updated TABLE (ObjectName sysname NOT NULL, Result nvarchar(20) NOT NULL);

IF @hoaDonTriggerId IS NULL OR @chiTietTriggerId IS NULL
    THROW 50002, N'Không tìm thấy trigger cần sửa; hãy kiểm tra cấu trúc CSDL.', 1;

SELECT N'dbo.trg_HoaDon_ThanhToanVaKhoaHoaDon' AS ObjectName, definition AS OriginalDefinition
FROM sys.sql_modules WHERE object_id = @hoaDonTriggerId
UNION ALL
SELECT N'dbo.trg_ChiTietHoaDon_KiemTraKhoVaKhoaSuaDoi', definition
FROM sys.sql_modules WHERE object_id = @chiTietTriggerId;

SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;

    SET @definition = OBJECT_DEFINITION(@hoaDonTriggerId);
    IF CHARINDEX(N'ChiTietHoaDon', @definition COLLATE Latin1_General_100_BIN2) > 0
    BEGIN
        SET @sql = REPLACE(@definition COLLATE Latin1_General_100_BIN2,
                           N'ChiTietHoaDon', N'dbo.CHITIETHOADON');
        SET @alterIndex = CHARINDEX(N'CREATE TRIGGER', @sql COLLATE Latin1_General_100_BIN2);
        IF @alterIndex = 0
            THROW 50003, N'Không nhận dạng được định nghĩa trigger thanh toán.', 1;

        SET @sql = STUFF(@sql, @alterIndex, LEN(N'CREATE TRIGGER'), N'ALTER TRIGGER');
        EXEC sys.sp_executesql @sql;
        INSERT INTO @updated VALUES (N'dbo.trg_HoaDon_ThanhToanVaKhoaHoaDon', N'Fixed');
    END
    ELSE IF CHARINDEX(N'dbo.CHITIETHOADON', @definition COLLATE Latin1_General_100_BIN2) > 0
        INSERT INTO @updated VALUES (N'dbo.trg_HoaDon_ThanhToanVaKhoaHoaDon', N'Already correct');
    ELSE
        THROW 50004, N'Trigger thanh toán không tham chiếu dbo.CHITIETHOADON.', 1;

    SET @definition = OBJECT_DEFINITION(@chiTietTriggerId);
    IF CHARINDEX(N'ON ChiTietHoaDon', @definition COLLATE Latin1_General_100_BIN2) > 0
    BEGIN
        SET @sql = REPLACE(@definition COLLATE Latin1_General_100_BIN2,
                           N'ON ChiTietHoaDon', N'ON dbo.CHITIETHOADON');
        SET @alterIndex = CHARINDEX(N'CREATE TRIGGER', @sql COLLATE Latin1_General_100_BIN2);
        IF @alterIndex = 0
            THROW 50005, N'Không nhận dạng được định nghĩa trigger chi tiết hóa đơn.', 1;

        SET @sql = STUFF(@sql, @alterIndex, LEN(N'CREATE TRIGGER'), N'ALTER TRIGGER');
        EXEC sys.sp_executesql @sql;
        INSERT INTO @updated VALUES (N'dbo.trg_ChiTietHoaDon_KiemTraKhoVaKhoaSuaDoi', N'Fixed');
    END
    ELSE IF CHARINDEX(N'ON dbo.CHITIETHOADON', @definition COLLATE Latin1_General_100_BIN2) > 0
        INSERT INTO @updated VALUES (N'dbo.trg_ChiTietHoaDon_KiemTraKhoVaKhoaSuaDoi', N'Already correct');
    ELSE
        THROW 50006, N'Trigger chi tiết không gắn với dbo.CHITIETHOADON.', 1;

    COMMIT TRANSACTION;
    SELECT ObjectName, Result FROM @updated;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
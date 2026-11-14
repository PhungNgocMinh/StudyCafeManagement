/* Nạp 41 bàn của quán (tầng 1: 30 bàn, tầng 2: 11 bàn). Chạy lại nhiều lần vẫn an toàn.
   MaBan theo quy ước T{tầng}-{loại}{số}: D = bàn đơn, P = bàn đôi, R = bàn tròn 3-8 người,
   C = bàn 10-20 người, L = bàn 50 người. FrmOrder dựa vào quy ước này để xếp bàn đúng tầng/khu. */
USE QuanLyCaPheHocTap;
GO
/* Bước 1: xóa ĐÚNG 4 bàn thử cũ (Bàn đơn 01, Bàn đơn 02, Bàn nhóm 01, Phòng họp kín 01).
   Chỉ xóa bàn có mã KHÔNG theo quy ước T{tầng}-{loại}{số} và CHƯA có hóa đơn nào dùng,
   nên không đụng tới 41 bàn mới hay bất kỳ bàn/hóa đơn nào khác. */
DELETE FROM BAN
WHERE TenBan IN (N'Bàn đơn 01', N'Bàn đơn 02', N'Bàn nhóm 01', N'Phòng họp kín 01')
  AND MaBan NOT LIKE N'T[0-9]-[DPRCL][0-9]%'
  AND NOT EXISTS (SELECT 1 FROM HOADON h WHERE h.MaBan = BAN.MaBan);
GO

/* Bước 2: nạp 41 bàn chuẩn. */
MERGE BAN AS t
USING (VALUES
    (N'T1-D01', N'Bàn đơn 01', 7500),
    (N'T1-D02', N'Bàn đơn 02', 7500),
    (N'T1-D03', N'Bàn đơn 03', 7500),
    (N'T1-D04', N'Bàn đơn 04', 7500),
    (N'T1-D05', N'Bàn đơn 05', 7500),
    (N'T1-D06', N'Bàn đơn 06', 7500),
    (N'T1-D07', N'Bàn đơn 07', 7500),
    (N'T1-D08', N'Bàn đơn 08', 7500),
    (N'T1-D09', N'Bàn đơn 09', 7500),
    (N'T1-D10', N'Bàn đơn 10', 7500),
    (N'T1-D11', N'Bàn đơn 11', 7500),
    (N'T1-D12', N'Bàn đơn 12', 7500),
    (N'T1-P01', N'Bàn đôi 01', 18000),
    (N'T1-P02', N'Bàn đôi 02', 18000),
    (N'T1-P03', N'Bàn đôi 03', 18000),
    (N'T1-P04', N'Bàn đôi 04', 18000),
    (N'T1-P05', N'Bàn đôi 05', 18000),
    (N'T1-P06', N'Bàn đôi 06', 18000),
    (N'T1-P07', N'Bàn đôi 07', 18000),
    (N'T1-P08', N'Bàn đôi 08', 18000),
    (N'T1-P09', N'Bàn đôi 09', 18000),
    (N'T1-P10', N'Bàn đôi 10', 18000),
    (N'T1-P11', N'Bàn đôi 11', 18000),
    (N'T1-P12', N'Bàn đôi 12', 18000),
    (N'T1-R01', N'Bàn tròn 01', 40000),
    (N'T1-R02', N'Bàn tròn 02', 40000),
    (N'T1-R03', N'Bàn tròn 03', 40000),
    (N'T1-R04', N'Bàn tròn 04', 40000),
    (N'T1-R05', N'Bàn tròn 05', 40000),
    (N'T1-R06', N'Bàn tròn 06', 40000),
    (N'T2-R01', N'Bàn tròn 07', 40000),
    (N'T2-R02', N'Bàn tròn 08', 40000),
    (N'T2-R03', N'Bàn tròn 09', 40000),
    (N'T2-R04', N'Bàn tròn 10', 40000),
    (N'T2-R05', N'Bàn tròn 11', 40000),
    (N'T2-R06', N'Bàn tròn 12', 40000),
    (N'T2-C01', N'Bàn 10-20 người số 1', 100000),
    (N'T2-C02', N'Bàn 10-20 người số 2', 100000),
    (N'T2-C03', N'Bàn 10-20 người số 3', 100000),
    (N'T2-C04', N'Bàn 10-20 người số 4', 100000),
    (N'T2-L01', N'Bàn 50 người', 250000)
) AS s (MaBan, TenBan, DonGiaGio)
ON t.MaBan = s.MaBan
WHEN MATCHED THEN UPDATE SET t.TenBan = s.TenBan, t.DonGiaGio = s.DonGiaGio
WHEN NOT MATCHED THEN INSERT (MaBan, TenBan, DonGiaGio, TrangThai) VALUES (s.MaBan, s.TenBan, s.DonGiaGio, N'Trống');
GO
SELECT COUNT(*) AS SoBan FROM BAN;
-- Bàn thử cũ còn sót lại (nếu có) là do đã có hóa đơn tham chiếu nên được giữ lại:
SELECT MaBan, TenBan FROM BAN WHERE MaBan NOT LIKE N'T[0-9]-[DPRCL][0-9]%';

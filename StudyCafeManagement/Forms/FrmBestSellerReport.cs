using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace StudyCafeManagement.Forms
{
    public class FrmBestSellerReport : Form
    {
        private const string ConnectionString =
            "Data Source=.;Initial Catalog=QuanLyCaPheHocTap;Integrated Security=True;TrustServerCertificate=True";
        private static readonly Color[] SliceColors =
        {
            Color.FromArgb(200, 138, 88), Color.FromArgb(100, 120, 100),
            Color.FromArgb(180, 80, 80), Color.FromArgb(91, 128, 164), Color.FromArgb(205, 178, 98)
        };
        private readonly DateTimePicker fromDate = new DateTimePicker();
        private readonly DateTimePicker toDate = new DateTimePicker();
        private readonly DataGridView grid = new DataGridView();
        private readonly Panel chart = new Panel();
        private readonly List<KeyValuePair<string, int>> products = new List<KeyValuePair<string, int>>();

        public FrmBestSellerReport()
        {
            Text = "Top 5 mặt hàng bán chạy";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(800, 550);
            Size = new Size(1000, 700);

            FlowLayoutPanel filters = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 48,
                Padding = new Padding(12, 8, 8, 4),
                WrapContents = false
            };
            fromDate.Format = DateTimePickerFormat.Short;
            fromDate.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            toDate.Format = DateTimePickerFormat.Short;
            toDate.Value = DateTime.Today;
            Button refresh = new Button { Text = "Thống kê", AutoSize = true };
            refresh.Click += delegate { LoadReport(); };
            filters.Controls.Add(new Label { Text = "Từ ngày:", AutoSize = true, Padding = new Padding(0, 7, 0, 0) });
            filters.Controls.Add(fromDate);
            filters.Controls.Add(new Label { Text = "Đến ngày:", AutoSize = true, Padding = new Padding(8, 7, 0, 0) });
            filters.Controls.Add(toDate);
            filters.Controls.Add(refresh);

            chart.Dock = DockStyle.Top;
            chart.Height = 330;
            chart.BackColor = Color.FromArgb(255, 253, 249);
            chart.Paint += DrawChart;
            grid.Dock = DockStyle.Fill;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.RowHeadersVisible = false;

            Controls.Add(grid);
            Controls.Add(chart);
            Controls.Add(filters);
            CafeTheme.Apply(this);
            LoadReport();
        }

        private void LoadReport()
        {
            if (fromDate.Value.Date > toDate.Value.Date)
            {
                MessageBox.Show(this, "Ngày bắt đầu phải trước hoặc bằng ngày kết thúc.",
                    "Khoảng thời gian không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            const string query = @"
SELECT TOP 5 s.MaMon, s.TenMon, SUM(c.SoLuong) AS SoLuongBan,
       SUM(c.SoLuong * c.DonGiaBan) AS DoanhThu
FROM dbo.CHITIETHOADON c
JOIN dbo.HOADON h ON h.MaHD = c.MaHD
JOIN dbo.SANPHAM s ON s.MaMon = c.MaMon
WHERE h.TrangThai = N'Đã thanh toán'
  AND h.NgayLap >= @FromDate AND h.NgayLap < @ToDate
GROUP BY s.MaMon, s.TenMon
ORDER BY SUM(c.SoLuong) DESC, s.TenMon;";
            DataTable table = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate.Value.Date;
                    command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate.Value.Date.AddDays(1);
                    adapter.Fill(table);
                }

                products.Clear();
                foreach (DataRow row in table.Rows)
                    products.Add(new KeyValuePair<string, int>(
                        Convert.ToString(row["TenMon"]), Convert.ToInt32(row["SoLuongBan"])));
                grid.DataSource = table;
                if (grid.Columns.Contains("MaMon"))
                    grid.Columns["MaMon"].HeaderText = "Mã món";
                if (grid.Columns.Contains("TenMon"))
                    grid.Columns["TenMon"].HeaderText = "Tên mặt hàng";
                if (grid.Columns.Contains("SoLuongBan"))
                    grid.Columns["SoLuongBan"].HeaderText = "Số lượng bán";
                if (grid.Columns.Contains("DoanhThu"))
                {
                    grid.Columns["DoanhThu"].HeaderText = "Doanh thu";
                    grid.Columns["DoanhThu"].DefaultCellStyle.Format = "N0";
                }
                chart.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Không thể tải báo cáo mặt hàng bán chạy: " + ex.Message,
                    "Lỗi báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DrawChart(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.Clear(chart.BackColor);
            if (products.Count == 0)
            {
                TextRenderer.DrawText(graphics, "Không có mặt hàng đã thanh toán trong khoảng thời gian này.",
                    Font, chart.ClientRectangle, Color.FromArgb(100, 100, 100),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                return;
            }

            int total = 0;
            foreach (KeyValuePair<string, int> item in products)
                total += item.Value;
            Rectangle pie = new Rectangle(24, 20, 270, 270);
            float startAngle = -90;
            for (int i = 0; i < products.Count; i++)
            {
                float sweep = (float)products[i].Value / total * 360F;
                using (SolidBrush brush = new SolidBrush(SliceColors[i % SliceColors.Length]))
                    graphics.FillPie(brush, pie, startAngle, sweep);
                startAngle += sweep;

                int legendY = 38 + i * 48;
                using (SolidBrush brush = new SolidBrush(SliceColors[i % SliceColors.Length]))
                    graphics.FillRectangle(brush, 330, legendY, 18, 18);
                string legend = string.Format(CultureInfo.CurrentCulture, "{0} — {1} món ({2:P0})",
                    products[i].Key, products[i].Value, (double)products[i].Value / total);
                graphics.DrawString(legend, Font, Brushes.DimGray, 358, legendY - 1);
            }
        }
    }
}

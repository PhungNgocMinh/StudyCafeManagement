using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace StudyCafeManagement.Forms
{
    public class FrmRevenueReport : Form
    {
        private const string ConnectionString =
            "Data Source=.;Initial Catalog=QuanLyCaPheHocTap;Integrated Security=True;TrustServerCertificate=True";
        private readonly DateTimePicker fromDate = new DateTimePicker();
        private readonly DateTimePicker toDate = new DateTimePicker();
        private readonly DataGridView grid = new DataGridView();
        private readonly Panel chart = new Panel();
        private readonly Label summary = new Label();
        private readonly List<KeyValuePair<DateTime, decimal>> revenues =
            new List<KeyValuePair<DateTime, decimal>>();

        public FrmRevenueReport()
        {
            Text = "Báo cáo doanh thu";
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

            summary.Dock = DockStyle.Top;
            summary.Height = 32;
            summary.Padding = new Padding(12, 6, 0, 0);
            summary.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            chart.Dock = DockStyle.Top;
            chart.Height = 280;
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
            Controls.Add(summary);
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
SELECT CONVERT(date, NgayLap) AS Ngay, SUM(TongThanhToan) AS DoanhThu
FROM dbo.HOADON
WHERE TrangThai = N'Đã thanh toán'
  AND NgayLap >= @FromDate AND NgayLap < @ToDate
GROUP BY CONVERT(date, NgayLap)
ORDER BY Ngay;";
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

                revenues.Clear();
                decimal total = 0;
                foreach (DataRow row in table.Rows)
                {
                    DateTime date = Convert.ToDateTime(row["Ngay"], CultureInfo.InvariantCulture);
                    decimal amount = Convert.ToDecimal(row["DoanhThu"], CultureInfo.InvariantCulture);
                    revenues.Add(new KeyValuePair<DateTime, decimal>(date, amount));
                    total += amount;
                }
                grid.DataSource = table;
                if (grid.Columns.Contains("Ngay"))
                    grid.Columns["Ngay"].HeaderText = "Ngày";
                if (grid.Columns.Contains("DoanhThu"))
                {
                    grid.Columns["DoanhThu"].HeaderText = "Doanh thu";
                    grid.Columns["DoanhThu"].DefaultCellStyle.Format = "N0";
                }
                summary.Text = "Tổng doanh thu: " + total.ToString("N0") + " đ";
                chart.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Không thể tải báo cáo doanh thu: " + ex.Message,
                    "Lỗi báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DrawChart(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.Clear(chart.BackColor);
            Rectangle bounds = chart.ClientRectangle;
            if (revenues.Count == 0)
            {
                TextRenderer.DrawText(graphics, "Không có doanh thu trong khoảng thời gian này.",
                    Font, bounds, Color.FromArgb(100, 100, 100),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                return;
            }

            int left = 60, right = 20, top = 20, bottom = 55;
            int chartWidth = Math.Max(1, bounds.Width - left - right);
            int chartHeight = Math.Max(1, bounds.Height - top - bottom);
            decimal max = 0;
            foreach (KeyValuePair<DateTime, decimal> point in revenues)
                if (point.Value > max) max = point.Value;
            if (max == 0) max = 1;

            using (Pen axis = new Pen(Color.FromArgb(190, 180, 170)))
            using (SolidBrush bar = new SolidBrush(Color.FromArgb(200, 138, 88)))
            using (Font labelFont = new Font("Segoe UI", 8F))
            {
                graphics.DrawLine(axis, left, top, left, top + chartHeight);
                graphics.DrawLine(axis, left, top + chartHeight, left + chartWidth, top + chartHeight);
                int slot = Math.Max(1, chartWidth / revenues.Count);
                int barWidth = Math.Max(4, Math.Min(36, slot - 8));
                for (int i = 0; i < revenues.Count; i++)
                {
                    KeyValuePair<DateTime, decimal> point = revenues[i];
                    int height = (int)(point.Value / max * (chartHeight - 8));
                    int x = left + i * chartWidth / revenues.Count + (slot - barWidth) / 2;
                    int y = top + chartHeight - height;
                    graphics.FillRectangle(bar, x, y, barWidth, height);
                    if (revenues.Count <= 14)
                    {
                        string label = point.Key.ToString("dd/MM", CultureInfo.InvariantCulture);
                        graphics.DrawString(label, labelFont, Brushes.DimGray, x - 8, top + chartHeight + 6);
                    }
                }
                graphics.DrawString(max.ToString("N0"), labelFont, Brushes.DimGray, 4, top);
            }
        }
    }
}

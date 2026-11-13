namespace StudyCafeManagement
{
    partial class FrmPayment
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle moneyStyle = new System.Windows.Forms.DataGridViewCellStyle();

            this.lblMaHD = new System.Windows.Forms.Label();
            this.lblBan = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.lblElapsed = new System.Windows.Forms.Label();
            this.lblCapSDT = new System.Windows.Forms.Label();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.btnFindKH = new System.Windows.Forms.Button();
            this.btnClearKH = new System.Windows.Forms.Button();
            this.lblKhachHang = new System.Windows.Forms.Label();
            this.lblCapGio = new System.Windows.Forms.Label();
            this.nudHours = new System.Windows.Forms.NumericUpDown();
            this.lblRate = new System.Windows.Forms.Label();
            this.lblCapBan = new System.Windows.Forms.Label();
            this.lblValBan = new System.Windows.Forms.Label();
            this.lblCapMon = new System.Windows.Forms.Label();
            this.lblValMon = new System.Windows.Forms.Label();
            this.lblCapTam = new System.Windows.Forms.Label();
            this.lblValTam = new System.Windows.Forms.Label();
            this.lblCapGiam = new System.Windows.Forms.Label();
            this.lblValGiam = new System.Windows.Forms.Label();
            this.lblCapTong = new System.Windows.Forms.Label();
            this.lblValTong = new System.Windows.Forms.Label();
            this.lblDiem = new System.Windows.Forms.Label();
            this.pnlSum = new System.Windows.Forms.Panel();
            this.btnPay = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.colTenMon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGhiChu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.nudHours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.pnlSum.SuspendLayout();
            this.SuspendLayout();
            // lblMaHD
            this.lblMaHD.Location = new System.Drawing.Point(16, 12);
            this.lblMaHD.Name = "lblMaHD";
            this.lblMaHD.Size = new System.Drawing.Size(310, 22);
            this.lblMaHD.Text = "Hóa đơn:";
            this.lblMaHD.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            // lblBan
            this.lblBan.Location = new System.Drawing.Point(16, 36);
            this.lblBan.Name = "lblBan";
            this.lblBan.Size = new System.Drawing.Size(310, 22);
            this.lblBan.Text = "Bàn:";
            this.lblBan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            // lblStart
            this.lblStart.Location = new System.Drawing.Point(340, 12);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(320, 22);
            this.lblStart.Text = "Bắt đầu:";
            // lblElapsed
            this.lblElapsed.Location = new System.Drawing.Point(340, 36);
            this.lblElapsed.Name = "lblElapsed";
            this.lblElapsed.Size = new System.Drawing.Size(320, 22);
            this.lblElapsed.Text = "Đã sử dụng:";
            // lblCapSDT
            this.lblCapSDT.Location = new System.Drawing.Point(16, 74);
            this.lblCapSDT.Name = "lblCapSDT";
            this.lblCapSDT.Size = new System.Drawing.Size(72, 22);
            this.lblCapSDT.Text = "SĐT khách:";
            // txtSDT
            this.txtSDT.Location = new System.Drawing.Point(92, 70);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.Size = new System.Drawing.Size(140, 25);
            this.txtSDT.MaxLength = 15;
            this.txtSDT.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSDT_KeyDown);
            // btnFindKH
            this.btnFindKH.Location = new System.Drawing.Point(238, 68);
            this.btnFindKH.Name = "btnFindKH";
            this.btnFindKH.Size = new System.Drawing.Size(70, 28);
            this.btnFindKH.Text = "Tìm";
            this.btnFindKH.UseVisualStyleBackColor = true;
            this.btnFindKH.Click += new System.EventHandler(this.btnFindKH_Click);
            // btnClearKH
            this.btnClearKH.Location = new System.Drawing.Point(312, 68);
            this.btnClearKH.Name = "btnClearKH";
            this.btnClearKH.Size = new System.Drawing.Size(70, 28);
            this.btnClearKH.Text = "Bỏ khách";
            this.btnClearKH.UseVisualStyleBackColor = true;
            this.btnClearKH.Click += new System.EventHandler(this.btnClearKH_Click);
            // lblKhachHang
            this.lblKhachHang.Location = new System.Drawing.Point(390, 70);
            this.lblKhachHang.Name = "lblKhachHang";
            this.lblKhachHang.Size = new System.Drawing.Size(274, 26);
            this.lblKhachHang.Text = "Khách vãng lai";
            this.lblKhachHang.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // lblCapGio
            this.lblCapGio.Location = new System.Drawing.Point(16, 112);
            this.lblCapGio.Name = "lblCapGio";
            this.lblCapGio.Size = new System.Drawing.Size(120, 22);
            this.lblCapGio.Text = "Số giờ sử dụng:";
            // nudHours
            this.nudHours.Location = new System.Drawing.Point(140, 108);
            this.nudHours.Name = "nudHours";
            this.nudHours.Size = new System.Drawing.Size(80, 25);
            this.nudHours.DecimalPlaces = 2;
            this.nudHours.Increment = new decimal(new int[] { 25, 0, 0, 131072 });
            this.nudHours.Maximum = new decimal(new int[] { 240, 0, 0, 0 });
            this.nudHours.ValueChanged += new System.EventHandler(this.Recalc);
            // lblRate
            this.lblRate.Location = new System.Drawing.Point(230, 112);
            this.lblRate.Name = "lblRate";
            this.lblRate.Size = new System.Drawing.Size(430, 22);
            // dgvItems
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AllowUserToDeleteRows = false;
            this.dgvItems.AllowUserToResizeRows = false;
            this.dgvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItems.AutoGenerateColumns = false;
            this.dgvItems.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTenMon, this.colSoLuong, this.colDonGia, this.colThanhTien, this.colGhiChu});
            this.dgvItems.Location = new System.Drawing.Point(16, 144);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.ReadOnly = true;
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItems.Size = new System.Drawing.Size(648, 200);
            moneyStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            moneyStyle.Format = "N0";
            this.colTenMon.DataPropertyName = "TenMon";
            this.colTenMon.HeaderText = "Món";
            this.colTenMon.Name = "colTenMon";
            this.colTenMon.Width = 230;
            this.colSoLuong.DataPropertyName = "SoLuong";
            this.colSoLuong.HeaderText = "SL";
            this.colSoLuong.Name = "colSoLuong";
            this.colSoLuong.Width = 40;
            this.colDonGia.DataPropertyName = "DonGia";
            this.colDonGia.DefaultCellStyle = moneyStyle;
            this.colDonGia.HeaderText = "Đơn giá";
            this.colDonGia.Name = "colDonGia";
            this.colDonGia.Width = 90;
            this.colThanhTien.DataPropertyName = "ThanhTien";
            this.colThanhTien.DefaultCellStyle = moneyStyle;
            this.colThanhTien.HeaderText = "Thành tiền";
            this.colThanhTien.Name = "colThanhTien";
            this.colThanhTien.Width = 100;
            this.colGhiChu.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGhiChu.DataPropertyName = "GhiChu";
            this.colGhiChu.HeaderText = "Ghi chú";
            this.colGhiChu.Name = "colGhiChu";
            // pnlSum
            this.pnlSum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSum.Location = new System.Drawing.Point(16, 352);
            this.pnlSum.Name = "pnlSum";
            this.pnlSum.Size = new System.Drawing.Size(648, 188);
            this.lblCapBan.Location = new System.Drawing.Point(10, 6);
            this.lblCapBan.Name = "lblCapBan";
            this.lblCapBan.Size = new System.Drawing.Size(380, 22);
            this.lblCapBan.Text = "Tiền bàn:";
            this.lblValBan.Location = new System.Drawing.Point(396, 6);
            this.lblValBan.Name = "lblValBan";
            this.lblValBan.Size = new System.Drawing.Size(240, 22);
            this.lblValBan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.pnlSum.Controls.Add(this.lblCapBan);
            this.pnlSum.Controls.Add(this.lblValBan);
            this.lblCapMon.Location = new System.Drawing.Point(10, 32);
            this.lblCapMon.Name = "lblCapMon";
            this.lblCapMon.Size = new System.Drawing.Size(380, 22);
            this.lblCapMon.Text = "Tiền món:";
            this.lblValMon.Location = new System.Drawing.Point(396, 32);
            this.lblValMon.Name = "lblValMon";
            this.lblValMon.Size = new System.Drawing.Size(240, 22);
            this.lblValMon.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.pnlSum.Controls.Add(this.lblCapMon);
            this.pnlSum.Controls.Add(this.lblValMon);
            this.lblCapTam.Location = new System.Drawing.Point(10, 58);
            this.lblCapTam.Name = "lblCapTam";
            this.lblCapTam.Size = new System.Drawing.Size(380, 22);
            this.lblCapTam.Text = "Tạm tính:";
            this.lblValTam.Location = new System.Drawing.Point(396, 58);
            this.lblValTam.Name = "lblValTam";
            this.lblValTam.Size = new System.Drawing.Size(240, 22);
            this.lblValTam.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.pnlSum.Controls.Add(this.lblCapTam);
            this.pnlSum.Controls.Add(this.lblValTam);
            this.lblCapGiam.Location = new System.Drawing.Point(10, 84);
            this.lblCapGiam.Name = "lblCapGiam";
            this.lblCapGiam.Size = new System.Drawing.Size(380, 22);
            this.lblCapGiam.Text = "Giảm giá khách Thân thiết (10%):";
            this.lblValGiam.Location = new System.Drawing.Point(396, 84);
            this.lblValGiam.Name = "lblValGiam";
            this.lblValGiam.Size = new System.Drawing.Size(240, 22);
            this.lblValGiam.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.pnlSum.Controls.Add(this.lblCapGiam);
            this.pnlSum.Controls.Add(this.lblValGiam);
            this.lblCapTong.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblCapTong.Location = new System.Drawing.Point(10, 116);
            this.lblCapTong.Name = "lblCapTong";
            this.lblCapTong.Size = new System.Drawing.Size(300, 32);
            this.lblCapTong.Text = "TỔNG THANH TOÁN:";
            this.lblCapTong.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblValTong.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblValTong.ForeColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.lblValTong.Location = new System.Drawing.Point(316, 114);
            this.lblValTong.Name = "lblValTong";
            this.lblValTong.Size = new System.Drawing.Size(320, 36);
            this.lblValTong.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDiem.Location = new System.Drawing.Point(10, 156);
            this.lblDiem.Name = "lblDiem";
            this.lblDiem.Size = new System.Drawing.Size(626, 22);
            this.pnlSum.Controls.Add(this.lblCapTong);
            this.pnlSum.Controls.Add(this.lblValTong);
            this.pnlSum.Controls.Add(this.lblDiem);
            // btnPay
            this.btnPay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPay.BackColor = System.Drawing.Color.FromArgb(46, 125, 50);
            this.btnPay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPay.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnPay.ForeColor = System.Drawing.Color.White;
            this.btnPay.Location = new System.Drawing.Point(444, 552);
            this.btnPay.Name = "btnPay";
            this.btnPay.Size = new System.Drawing.Size(220, 44);
            this.btnPay.Text = "XÁC NHẬN THANH TOÁN";
            this.btnPay.UseVisualStyleBackColor = false;
            this.btnPay.Click += new System.EventHandler(this.btnPay_Click);
            // btnClose
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(318, 552);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 44);
            this.btnClose.Text = "Quay lại";
            this.btnClose.UseVisualStyleBackColor = true;
            // FrmPayment
            this.AcceptButton = null;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(680, 612);
            this.Controls.Add(this.lblMaHD);
            this.Controls.Add(this.lblBan);
            this.Controls.Add(this.lblStart);
            this.Controls.Add(this.lblElapsed);
            this.Controls.Add(this.lblCapSDT);
            this.Controls.Add(this.txtSDT);
            this.Controls.Add(this.btnFindKH);
            this.Controls.Add(this.btnClearKH);
            this.Controls.Add(this.lblKhachHang);
            this.Controls.Add(this.lblCapGio);
            this.Controls.Add(this.nudHours);
            this.Controls.Add(this.lblRate);
            this.Controls.Add(this.dgvItems);
            this.Controls.Add(this.pnlSum);
            this.Controls.Add(this.btnPay);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(696, 650);
            this.Name = "FrmPayment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thanh toán hóa đơn";
            this.Load += new System.EventHandler(this.FrmPayment_Load);
            this.Shown += new System.EventHandler(this.FrmPayment_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.nudHours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.pnlSum.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblMaHD;
        private System.Windows.Forms.Label lblBan;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.Label lblElapsed;
        private System.Windows.Forms.Label lblCapSDT;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.Button btnFindKH;
        private System.Windows.Forms.Button btnClearKH;
        private System.Windows.Forms.Label lblKhachHang;
        private System.Windows.Forms.Label lblCapGio;
        private System.Windows.Forms.NumericUpDown nudHours;
        private System.Windows.Forms.Label lblRate;
        private System.Windows.Forms.Label lblCapBan;
        private System.Windows.Forms.Label lblValBan;
        private System.Windows.Forms.Label lblCapMon;
        private System.Windows.Forms.Label lblValMon;
        private System.Windows.Forms.Label lblCapTam;
        private System.Windows.Forms.Label lblValTam;
        private System.Windows.Forms.Label lblCapGiam;
        private System.Windows.Forms.Label lblValGiam;
        private System.Windows.Forms.Label lblCapTong;
        private System.Windows.Forms.Label lblValTong;
        private System.Windows.Forms.Label lblDiem;
        private System.Windows.Forms.Panel pnlSum;
        private System.Windows.Forms.Button btnPay;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenMon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThanhTien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGhiChu;
    }
}

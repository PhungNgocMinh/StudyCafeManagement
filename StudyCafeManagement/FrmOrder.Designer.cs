namespace StudyCafeManagement
{
    partial class FrmOrder
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle moneyStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblNhanVien = new System.Windows.Forms.Label();
            this.lblLegendBusy = new System.Windows.Forms.Label();
            this.lblLegendFree = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cboTakeAway = new System.Windows.Forms.ComboBox();
            this.btnTakeAway = new System.Windows.Forms.Button();
            this.tabFloors = new System.Windows.Forms.TabControl();
            this.panelRight = new System.Windows.Forms.Panel();
            this.btnPay = new System.Windows.Forms.Button();
            this.btnCancelInvoice = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.dgvOrder = new System.Windows.Forms.DataGridView();
            this.colTenMon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGhiChu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpMenu = new System.Windows.Forms.GroupBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.nudQty = new System.Windows.Forms.NumericUpDown();
            this.cboProduct = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.cboCategory = new System.Windows.Forms.ComboBox();
            this.grpInvoice = new System.Windows.Forms.GroupBox();
            this.lblKhachHang = new System.Windows.Forms.Label();
            this.btnFindKH = new System.Windows.Forms.Button();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.lblInvoiceInfo = new System.Windows.Forms.Label();
            this.lblSelected = new System.Windows.Forms.Label();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.panelTop.SuspendLayout();
            this.panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrder)).BeginInit();
            this.grpMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQty)).BeginInit();
            this.grpInvoice.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblNhanVien);
            this.panelTop.Controls.Add(this.lblLegendBusy);
            this.panelTop.Controls.Add(this.lblLegendFree);
            this.panelTop.Controls.Add(this.btnRefresh);
            this.panelTop.Controls.Add(this.cboTakeAway);
            this.panelTop.Controls.Add(this.btnTakeAway);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1240, 52);
            // 
            // btnTakeAway
            // 
            this.btnTakeAway.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnTakeAway.Location = new System.Drawing.Point(10, 8);
            this.btnTakeAway.Name = "btnTakeAway";
            this.btnTakeAway.Size = new System.Drawing.Size(150, 36);
            this.btnTakeAway.Text = "Mua mang đi";
            this.btnTakeAway.UseVisualStyleBackColor = true;
            this.btnTakeAway.Click += new System.EventHandler(this.btnTakeAway_Click);
            // 
            // cboTakeAway
            // 
            this.cboTakeAway.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTakeAway.Location = new System.Drawing.Point(168, 14);
            this.cboTakeAway.Name = "cboTakeAway";
            this.cboTakeAway.Size = new System.Drawing.Size(230, 25);
            this.cboTakeAway.SelectionChangeCommitted += new System.EventHandler(this.cboTakeAway_SelectionChangeCommitted);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(408, 8);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 36);
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblLegendFree
            // 
            this.lblLegendFree.BackColor = System.Drawing.Color.FromArgb(76, 175, 80);
            this.lblLegendFree.ForeColor = System.Drawing.Color.White;
            this.lblLegendFree.Location = new System.Drawing.Point(528, 14);
            this.lblLegendFree.Name = "lblLegendFree";
            this.lblLegendFree.Size = new System.Drawing.Size(70, 24);
            this.lblLegendFree.Text = "Trống";
            this.lblLegendFree.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLegendBusy
            // 
            this.lblLegendBusy.BackColor = System.Drawing.Color.FromArgb(229, 57, 53);
            this.lblLegendBusy.ForeColor = System.Drawing.Color.White;
            this.lblLegendBusy.Location = new System.Drawing.Point(604, 14);
            this.lblLegendBusy.Name = "lblLegendBusy";
            this.lblLegendBusy.Size = new System.Drawing.Size(100, 24);
            this.lblLegendBusy.Text = "Đang phục vụ";
            this.lblLegendBusy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNhanVien
            // 
            this.lblNhanVien.Location = new System.Drawing.Point(730, 14);
            this.lblNhanVien.Name = "lblNhanVien";
            this.lblNhanVien.Size = new System.Drawing.Size(300, 24);
            this.lblNhanVien.Text = "Nhân viên:";
            this.lblNhanVien.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabFloors
            // 
            this.tabFloors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFloors.Name = "tabFloors";
            this.tabFloors.Padding = new System.Drawing.Point(14, 6);
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.btnPay);
            this.panelRight.Controls.Add(this.btnCancelInvoice);
            this.panelRight.Controls.Add(this.btnRemove);
            this.panelRight.Controls.Add(this.lblTongTien);
            this.panelRight.Controls.Add(this.dgvOrder);
            this.panelRight.Controls.Add(this.grpMenu);
            this.panelRight.Controls.Add(this.grpInvoice);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(480, 648);
            // 
            // grpInvoice
            // 
            this.grpInvoice.Controls.Add(this.lblKhachHang);
            this.grpInvoice.Controls.Add(this.btnFindKH);
            this.grpInvoice.Controls.Add(this.txtSDT);
            this.grpInvoice.Controls.Add(this.lblInvoiceInfo);
            this.grpInvoice.Controls.Add(this.lblSelected);
            this.grpInvoice.Location = new System.Drawing.Point(8, 4);
            this.grpInvoice.Name = "grpInvoice";
            this.grpInvoice.Size = new System.Drawing.Size(462, 148);
            this.grpInvoice.Text = "Hóa đơn hiện tại";
            // 
            // lblSelected
            // 
            this.lblSelected.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSelected.Location = new System.Drawing.Point(10, 20);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(442, 26);
            this.lblSelected.Text = "Chưa chọn bàn";
            // 
            // lblInvoiceInfo
            // 
            this.lblInvoiceInfo.Location = new System.Drawing.Point(10, 48);
            this.lblInvoiceInfo.Name = "lblInvoiceInfo";
            this.lblInvoiceInfo.Size = new System.Drawing.Size(442, 56);
            // 
            // txtSDT
            // 
            this.txtSDT.Location = new System.Drawing.Point(10, 112);
            this.txtSDT.MaxLength = 15;
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.Size = new System.Drawing.Size(130, 25);
            this.txtSDT.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSDT_KeyDown);
            // 
            // btnFindKH
            // 
            this.btnFindKH.Location = new System.Drawing.Point(146, 110);
            this.btnFindKH.Name = "btnFindKH";
            this.btnFindKH.Size = new System.Drawing.Size(100, 28);
            this.btnFindKH.Text = "Gắn khách (SĐT)";
            this.btnFindKH.UseVisualStyleBackColor = true;
            this.btnFindKH.Click += new System.EventHandler(this.btnFindKH_Click);
            // 
            // lblKhachHang
            // 
            this.lblKhachHang.Location = new System.Drawing.Point(252, 108);
            this.lblKhachHang.Name = "lblKhachHang";
            this.lblKhachHang.Size = new System.Drawing.Size(204, 34);
            this.lblKhachHang.Text = "Khách vãng lai";
            this.lblKhachHang.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpMenu
            // 
            this.grpMenu.Controls.Add(this.btnAdd);
            this.grpMenu.Controls.Add(this.txtNote);
            this.grpMenu.Controls.Add(this.nudQty);
            this.grpMenu.Controls.Add(this.cboProduct);
            this.grpMenu.Controls.Add(this.txtSearch);
            this.grpMenu.Controls.Add(this.cboCategory);
            this.grpMenu.Location = new System.Drawing.Point(8, 158);
            this.grpMenu.Name = "grpMenu";
            this.grpMenu.Size = new System.Drawing.Size(462, 128);
            this.grpMenu.Text = "Gọi món";
            // 
            // cboCategory
            // 
            this.cboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCategory.Location = new System.Drawing.Point(10, 22);
            this.cboCategory.Name = "cboCategory";
            this.cboCategory.Size = new System.Drawing.Size(150, 25);
            this.cboCategory.SelectedIndexChanged += new System.EventHandler(this.FilterChanged);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(166, 22);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(286, 25);
            this.txtSearch.TextChanged += new System.EventHandler(this.FilterChanged);
            // 
            // cboProduct
            // 
            this.cboProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProduct.Location = new System.Drawing.Point(10, 54);
            this.cboProduct.Name = "cboProduct";
            this.cboProduct.Size = new System.Drawing.Size(442, 25);
            // 
            // nudQty
            // 
            this.nudQty.Location = new System.Drawing.Point(10, 90);
            this.nudQty.Maximum = new decimal(new int[] { 99, 0, 0, 0 });
            this.nudQty.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudQty.Name = "nudQty";
            this.nudQty.Size = new System.Drawing.Size(60, 25);
            this.nudQty.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // txtNote
            // 
            this.txtNote.Location = new System.Drawing.Point(76, 90);
            this.txtNote.MaxLength = 255;
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(270, 25);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(352, 88);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 29);
            this.btnAdd.Text = "Thêm món";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgvOrder
            // 
            this.dgvOrder.AllowUserToAddRows = false;
            this.dgvOrder.AllowUserToDeleteRows = false;
            this.dgvOrder.AllowUserToResizeRows = false;
            this.dgvOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOrder.AutoGenerateColumns = false;
            this.dgvOrder.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrder.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTenMon, this.colSoLuong, this.colDonGia, this.colThanhTien, this.colGhiChu});
            this.dgvOrder.Location = new System.Drawing.Point(8, 292);
            this.dgvOrder.MultiSelect = false;
            this.dgvOrder.Name = "dgvOrder";
            this.dgvOrder.ReadOnly = true;
            this.dgvOrder.RowHeadersVisible = false;
            this.dgvOrder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrder.Size = new System.Drawing.Size(462, 248);
            // 
            // columns
            // 
            moneyStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            moneyStyle.Format = "N0";
            this.colTenMon.DataPropertyName = "TenMon";
            this.colTenMon.HeaderText = "Món";
            this.colTenMon.Name = "colTenMon";
            this.colTenMon.Width = 140;
            this.colSoLuong.DataPropertyName = "SoLuong";
            this.colSoLuong.HeaderText = "SL";
            this.colSoLuong.Name = "colSoLuong";
            this.colSoLuong.Width = 36;
            this.colDonGia.DataPropertyName = "DonGia";
            this.colDonGia.DefaultCellStyle = moneyStyle;
            this.colDonGia.HeaderText = "Đơn giá";
            this.colDonGia.Name = "colDonGia";
            this.colDonGia.Width = 70;
            this.colThanhTien.DataPropertyName = "ThanhTien";
            this.colThanhTien.DefaultCellStyle = moneyStyle;
            this.colThanhTien.HeaderText = "Thành tiền";
            this.colThanhTien.Name = "colThanhTien";
            this.colThanhTien.Width = 80;
            this.colGhiChu.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGhiChu.DataPropertyName = "GhiChu";
            this.colGhiChu.HeaderText = "Ghi chú";
            this.colGhiChu.Name = "colGhiChu";
            // 
            // lblTongTien
            // 
            this.lblTongTien.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTongTien.Location = new System.Drawing.Point(8, 546);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(462, 40);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.Location = new System.Drawing.Point(8, 596);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(100, 40);
            this.btnRemove.Text = "Xóa món";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnCancelInvoice
            // 
            this.btnCancelInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancelInvoice.Location = new System.Drawing.Point(114, 596);
            this.btnCancelInvoice.Name = "btnCancelInvoice";
            this.btnCancelInvoice.Size = new System.Drawing.Size(120, 40);
            this.btnCancelInvoice.Text = "Hủy hóa đơn";
            this.btnCancelInvoice.UseVisualStyleBackColor = true;
            this.btnCancelInvoice.Click += new System.EventHandler(this.btnCancelInvoice_Click);
            // 
            // btnPay
            // 
            this.btnPay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPay.BackColor = System.Drawing.Color.FromArgb(46, 125, 50);
            this.btnPay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPay.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnPay.ForeColor = System.Drawing.Color.White;
            this.btnPay.Location = new System.Drawing.Point(300, 596);
            this.btnPay.Name = "btnPay";
            this.btnPay.Size = new System.Drawing.Size(170, 40);
            this.btnPay.Text = "Thanh toán";
            this.btnPay.UseVisualStyleBackColor = false;
            this.btnPay.Click += new System.EventHandler(this.btnPay_Click);
            // 
            // timerRefresh
            // 
            this.timerRefresh.Interval = 30000;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // FrmOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1240, 700);
            this.Controls.Add(this.tabFloors);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1100, 640);
            this.Name = "FrmOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gọi món & Sơ đồ bàn";
            this.Load += new System.EventHandler(this.FrmOrder_Load);
            this.panelTop.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrder)).EndInit();
            this.grpMenu.ResumeLayout(false);
            this.grpMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQty)).EndInit();
            this.grpInvoice.ResumeLayout(false);
            this.grpInvoice.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblNhanVien;
        private System.Windows.Forms.Label lblLegendBusy;
        private System.Windows.Forms.Label lblLegendFree;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ComboBox cboTakeAway;
        private System.Windows.Forms.Button btnTakeAway;
        private System.Windows.Forms.TabControl tabFloors;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Button btnPay;
        private System.Windows.Forms.Button btnCancelInvoice;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.DataGridView dgvOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenMon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThanhTien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGhiChu;
        private System.Windows.Forms.GroupBox grpMenu;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.NumericUpDown nudQty;
        private System.Windows.Forms.ComboBox cboProduct;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cboCategory;
        private System.Windows.Forms.GroupBox grpInvoice;
        private System.Windows.Forms.Label lblKhachHang;
        private System.Windows.Forms.Button btnFindKH;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.Label lblInvoiceInfo;
        private System.Windows.Forms.Label lblSelected;
        private System.Windows.Forms.Timer timerRefresh;
    }
}

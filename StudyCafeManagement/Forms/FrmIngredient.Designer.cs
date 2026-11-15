    namespace StudyCafeManagement.Forms
    {
        partial class FrmIngredient
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
                this.lblTitle = new System.Windows.Forms.Label();
                this.grpInput = new System.Windows.Forms.GroupBox();
                this.lblIngredientName = new System.Windows.Forms.Label();
                this.txtIngredientName = new System.Windows.Forms.TextBox();
                this.lblStockQuantity = new System.Windows.Forms.Label();
                this.nudStockQuantity = new System.Windows.Forms.NumericUpDown();
                this.lblUnit = new System.Windows.Forms.Label();
                this.txtUnit = new System.Windows.Forms.TextBox();
                this.btnSave = new System.Windows.Forms.Button();
                this.btnDelete = new System.Windows.Forms.Button();
                this.btnReset = new System.Windows.Forms.Button();
                this.lblSearch = new System.Windows.Forms.Label();
                this.txtSearch = new System.Windows.Forms.TextBox();
                this.dgvIngredient = new System.Windows.Forms.DataGridView();
                this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
                this.grpInput.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.nudStockQuantity)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.dgvIngredient)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
                this.SuspendLayout();
                // 
                // lblTitle
                // 
                this.lblTitle.AutoSize = true;
                this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
                this.lblTitle.ForeColor = System.Drawing.Color.MidnightBlue;
                this.lblTitle.Location = new System.Drawing.Point(165, 15);
                this.lblTitle.Name = "lblTitle";
                this.lblTitle.Size = new System.Drawing.Size(326, 37);
                this.lblTitle.TabIndex = 0;
                this.lblTitle.Text = "QUẢN LÝ NGUYÊN LIỆU";
                // 
                // grpInput
                // 
                this.grpInput.Controls.Add(this.txtUnit);
                this.grpInput.Controls.Add(this.lblUnit);
                this.grpInput.Controls.Add(this.nudStockQuantity);
                this.grpInput.Controls.Add(this.lblStockQuantity);
                this.grpInput.Controls.Add(this.txtIngredientName);
                this.grpInput.Controls.Add(this.lblIngredientName);
                this.grpInput.Font = new System.Drawing.Font("Segoe UI", 10F);
                this.grpInput.Location = new System.Drawing.Point(25, 60);
                this.grpInput.Name = "grpInput";
                this.grpInput.Size = new System.Drawing.Size(630, 125);
                this.grpInput.TabIndex = 1;
                this.grpInput.TabStop = false;
                this.grpInput.Text = "Thông tin nguyên liệu";
                // 
                // lblIngredientName
                // 
                this.lblIngredientName.AutoSize = true;
                this.lblIngredientName.Location = new System.Drawing.Point(20, 35);
                this.lblIngredientName.Name = "lblIngredientName";
                this.lblIngredientName.Size = new System.Drawing.Size(68, 23);
                this.lblIngredientName.TabIndex = 0;
                this.lblIngredientName.Text = "Tên NL:";
                // 
                // txtIngredientName
                // 
                this.txtIngredientName.Location = new System.Drawing.Point(120, 32);
                this.txtIngredientName.Name = "txtIngredientName";
                this.txtIngredientName.Size = new System.Drawing.Size(470, 30);
                this.txtIngredientName.TabIndex = 1;
                // 
                // lblStockQuantity
                // 
                this.lblStockQuantity.AutoSize = true;
                this.lblStockQuantity.Location = new System.Drawing.Point(20, 75);
                this.lblStockQuantity.Name = "lblStockQuantity";
                this.lblStockQuantity.Size = new System.Drawing.Size(77, 23);
                this.lblStockQuantity.TabIndex = 2;
                this.lblStockQuantity.Text = "Tồn kho:";
                // 
                // nudStockQuantity
                // 
                this.nudStockQuantity.DecimalPlaces = 2;
                this.nudStockQuantity.Location = new System.Drawing.Point(120, 72);
                this.nudStockQuantity.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
                this.nudStockQuantity.Name = "nudStockQuantity";
                this.nudStockQuantity.Size = new System.Drawing.Size(180, 30);
                this.nudStockQuantity.TabIndex = 3;
                // 
                // lblUnit
                // 
                this.lblUnit.AutoSize = true;
                this.lblUnit.Location = new System.Drawing.Point(320, 75);
                this.lblUnit.Name = "lblUnit";
                this.lblUnit.Size = new System.Drawing.Size(98, 23);
                this.lblUnit.TabIndex = 4;
                this.lblUnit.Text = "Đơn vị tính:";
                // 
                // txtUnit
                // 
                this.txtUnit.Location = new System.Drawing.Point(410, 72);
                this.txtUnit.Name = "txtUnit";
                this.txtUnit.Size = new System.Drawing.Size(180, 30);
                this.txtUnit.TabIndex = 5;
                // 
                // btnSave
                // 
                this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F);
                this.btnSave.Location = new System.Drawing.Point(165, 200);
                this.btnSave.Name = "btnSave";
                this.btnSave.Size = new System.Drawing.Size(110, 38);
                this.btnSave.TabIndex = 2;
                this.btnSave.Text = "Lưu";
                this.btnSave.UseVisualStyleBackColor = true;
                this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
                // 
                // btnDelete
                // 
                this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 10F);
                this.btnDelete.Location = new System.Drawing.Point(290, 200);
                this.btnDelete.Name = "btnDelete";
                this.btnDelete.Size = new System.Drawing.Size(110, 38);
                this.btnDelete.TabIndex = 3;
                this.btnDelete.Text = "Xóa";
                this.btnDelete.UseVisualStyleBackColor = true;
                this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
                // 
                // btnReset
                // 
                this.btnReset.Font = new System.Drawing.Font("Segoe UI", 10F);
                this.btnReset.Location = new System.Drawing.Point(415, 200);
                this.btnReset.Name = "btnReset";
                this.btnReset.Size = new System.Drawing.Size(110, 38);
                this.btnReset.TabIndex = 4;
                this.btnReset.Text = "Làm mới";
                this.btnReset.UseVisualStyleBackColor = true;
                this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
                // 
                // lblSearch
                // 
                this.lblSearch.AutoSize = true;
                this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
                this.lblSearch.Location = new System.Drawing.Point(25, 255);
                this.lblSearch.Name = "lblSearch";
                this.lblSearch.Size = new System.Drawing.Size(83, 23);
                this.lblSearch.TabIndex = 5;
                this.lblSearch.Text = "Tìm kiếm:";
                // 
                // txtSearch
                // 
                this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
                this.txtSearch.Location = new System.Drawing.Point(115, 252);
                this.txtSearch.Name = "txtSearch";
                this.txtSearch.Size = new System.Drawing.Size(540, 30);
                this.txtSearch.TabIndex = 6;
                this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
                // 
                // dgvIngredient
                // 
                this.dgvIngredient.AllowUserToAddRows = false;
                this.dgvIngredient.AllowUserToDeleteRows = false;
                this.dgvIngredient.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                this.dgvIngredient.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                this.dgvIngredient.Location = new System.Drawing.Point(25, 295);
                this.dgvIngredient.MultiSelect = false;
                this.dgvIngredient.Name = "dgvIngredient";
                this.dgvIngredient.ReadOnly = true;
                this.dgvIngredient.RowHeadersWidth = 51;
                this.dgvIngredient.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
                this.dgvIngredient.Size = new System.Drawing.Size(630, 220);
                this.dgvIngredient.TabIndex = 7;
                this.dgvIngredient.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvIngredient_CellClick);
                // 
                // errorProvider1
                // 
                this.errorProvider1.ContainerControl = this;
                // 
                // FrmIngredient
                // 
                this.ClientSize = new System.Drawing.Size(682, 535);
                this.Controls.Add(this.dgvIngredient);
                this.Controls.Add(this.txtSearch);
                this.Controls.Add(this.lblSearch);
                this.Controls.Add(this.btnReset);
                this.Controls.Add(this.btnDelete);
                this.Controls.Add(this.btnSave);
                this.Controls.Add(this.grpInput);
                this.Controls.Add(this.lblTitle);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                this.MaximizeBox = false;
                this.Name = "FrmIngredient";
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                this.Text = "Quản Lý Nguyên Liệu Kho";
                this.Load += new System.EventHandler(this.FrmIngredient_Load);
                this.grpInput.ResumeLayout(false);
                this.grpInput.PerformLayout();
                ((System.ComponentModel.ISupportInitialize)(this.nudStockQuantity)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.dgvIngredient)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
                this.ResumeLayout(false);
                this.PerformLayout();
            }

            #endregion

            private System.Windows.Forms.Label lblTitle;
            private System.Windows.Forms.GroupBox grpInput;
            private System.Windows.Forms.Label lblIngredientName;
            private System.Windows.Forms.TextBox txtIngredientName;
            private System.Windows.Forms.Label lblStockQuantity;
            private System.Windows.Forms.NumericUpDown nudStockQuantity;
            private System.Windows.Forms.Label lblUnit;
            private System.Windows.Forms.TextBox txtUnit;
            private System.Windows.Forms.Button btnSave;
            private System.Windows.Forms.Button btnDelete;
            private System.Windows.Forms.Button btnReset;
            private System.Windows.Forms.Label lblSearch;
            private System.Windows.Forms.TextBox txtSearch;
            private System.Windows.Forms.DataGridView dgvIngredient;
            private System.Windows.Forms.ErrorProvider errorProvider1;
        }
    }
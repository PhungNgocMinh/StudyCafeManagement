namespace StudyCafeManagement.Forms
{
    partial class Form1
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
            this.pnlTopHeader = new System.Windows.Forms.Panel();
            this.lblShopName = new System.Windows.Forms.Label();
            this.pnlRightMenu = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnRecipe = new System.Windows.Forms.Button();
            this.btnIngredient = new System.Windows.Forms.Button();
            this.btnProduct = new System.Windows.Forms.Button();
            this.btnCategory = new System.Windows.Forms.Button();
            this.btnCustomer = new System.Windows.Forms.Button();
            this.btnTable = new System.Windows.Forms.Button();
            this.pnlDesktop = new System.Windows.Forms.Panel();
            this.pnlTopHeader.SuspendLayout();
            this.pnlRightMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopHeader
            // 
            this.pnlTopHeader.BackColor = System.Drawing.Color.MidnightBlue;
            this.pnlTopHeader.Controls.Add(this.lblShopName);
            this.pnlTopHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlTopHeader.Name = "pnlTopHeader";
            this.pnlTopHeader.Size = new System.Drawing.Size(1100, 70);
            this.pnlTopHeader.TabIndex = 0;
            // 
            // lblShopName
            // 
            this.lblShopName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblShopName.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblShopName.ForeColor = System.Drawing.Color.White;
            this.lblShopName.Location = new System.Drawing.Point(0, 0);
            this.lblShopName.Name = "lblShopName";
            this.lblShopName.Size = new System.Drawing.Size(1100, 70);
            this.lblShopName.TabIndex = 0;
            this.lblShopName.Text = "STUDY CAFE MANAGEMENT SYSTEM";
            this.lblShopName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlRightMenu
            // 
            this.pnlRightMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(53)))), ((int)(((byte)(65)))));
            this.pnlRightMenu.Controls.Add(this.btnExit);
            this.pnlRightMenu.Controls.Add(this.btnRecipe);
            this.pnlRightMenu.Controls.Add(this.btnIngredient);
            this.pnlRightMenu.Controls.Add(this.btnProduct);
            this.pnlRightMenu.Controls.Add(this.btnCategory);
            this.pnlRightMenu.Controls.Add(this.btnCustomer);
            this.pnlRightMenu.Controls.Add(this.btnTable);
            this.pnlRightMenu.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRightMenu.Location = new System.Drawing.Point(880, 70);
            this.pnlRightMenu.Name = "pnlRightMenu";
            this.pnlRightMenu.Size = new System.Drawing.Size(220, 630);
            this.pnlRightMenu.TabIndex = 1;
            // 
            // btnTable
            // 
            this.btnTable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTable.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnTable.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnTable.Location = new System.Drawing.Point(10, 20);
            this.btnTable.Name = "btnTable";
            this.btnTable.Size = new System.Drawing.Size(200, 50);
            this.btnTable.TabIndex = 0;
            this.btnTable.Text = "Quản Lý Bàn";
            this.btnTable.UseVisualStyleBackColor = true;
            this.btnTable.Click += new System.EventHandler(this.btnTable_Click);
            // 
            // btnCustomer
            // 
            this.btnCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomer.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCustomer.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnCustomer.Location = new System.Drawing.Point(10, 80);
            this.btnCustomer.Name = "btnCustomer";
            this.btnCustomer.Size = new System.Drawing.Size(200, 50);
            this.btnCustomer.TabIndex = 1;
            this.btnCustomer.Text = "Khách Hàng";
            this.btnCustomer.UseVisualStyleBackColor = true;
            this.btnCustomer.Click += new System.EventHandler(this.btnCustomer_Click);
            // 
            // btnCategory
            // 
            this.btnCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCategory.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCategory.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnCategory.Location = new System.Drawing.Point(10, 140);
            this.btnCategory.Name = "btnCategory";
            this.btnCategory.Size = new System.Drawing.Size(200, 50);
            this.btnCategory.TabIndex = 2;
            this.btnCategory.Text = "Danh Mục Món";
            this.btnCategory.UseVisualStyleBackColor = true;
            this.btnCategory.Click += new System.EventHandler(this.btnCategory_Click);
            // 
            // btnProduct
            // 
            this.btnProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProduct.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnProduct.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnProduct.Location = new System.Drawing.Point(10, 200);
            this.btnProduct.Name = "btnProduct";
            this.btnProduct.Size = new System.Drawing.Size(200, 50);
            this.btnProduct.TabIndex = 3;
            this.btnProduct.Text = "Sản Phẩm / Món";
            this.btnProduct.UseVisualStyleBackColor = true;
            this.btnProduct.Click += new System.EventHandler(this.btnProduct_Click);
            // 
            // btnIngredient
            // 
            this.btnIngredient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIngredient.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnIngredient.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnIngredient.Location = new System.Drawing.Point(10, 260);
            this.btnIngredient.Name = "btnIngredient";
            this.btnIngredient.Size = new System.Drawing.Size(200, 50);
            this.btnIngredient.TabIndex = 4;
            this.btnIngredient.Text = "Nguyên Liệu Kho";
            this.btnIngredient.UseVisualStyleBackColor = true;
            this.btnIngredient.Click += new System.EventHandler(this.btnIngredient_Click);
            // 
            // btnRecipe
            // 
            this.btnRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecipe.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnRecipe.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnRecipe.Location = new System.Drawing.Point(10, 320);
            this.btnRecipe.Name = "btnRecipe";
            this.btnRecipe.Size = new System.Drawing.Size(200, 50);
            this.btnRecipe.TabIndex = 5;
            this.btnRecipe.Text = "Định Mức Công Thức";
            this.btnRecipe.UseVisualStyleBackColor = true;
            this.btnRecipe.Click += new System.EventHandler(this.btnRecipe_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Crimson;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(10, 550);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(200, 50);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Thoát Hệ Thống";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // pnlDesktop
            // 
            this.pnlDesktop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlDesktop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDesktop.Location = new System.Drawing.Point(0, 70);
            this.pnlDesktop.Name = "pnlDesktop";
            this.pnlDesktop.Size = new System.Drawing.Size(880, 630);
            this.pnlDesktop.TabIndex = 2;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this.pnlDesktop);
            this.Controls.Add(this.pnlRightMenu);
            this.Controls.Add(this.pnlTopHeader);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ Thống Quản Lý Study Cafe";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlTopHeader.ResumeLayout(false);
            this.pnlRightMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTopHeader;
        private System.Windows.Forms.Label lblShopName;
        private System.Windows.Forms.Panel pnlRightMenu;
        private System.Windows.Forms.Button btnTable;
        private System.Windows.Forms.Button btnCustomer;
        private System.Windows.Forms.Button btnCategory;
        private System.Windows.Forms.Button btnProduct;
        private System.Windows.Forms.Button btnIngredient;
        private System.Windows.Forms.Button btnRecipe;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel pnlDesktop;
    }
}
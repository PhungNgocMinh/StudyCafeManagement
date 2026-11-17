using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudyCafeManagement.Forms
{
    public partial class FrmMain : Form
    {
        private string currentUser;
        private string currentRole;
        private string currentEmployeeId;
        private readonly ToolStripMenuItem mnuManagement = new ToolStripMenuItem("Quản lý danh mục");
        private readonly ToolStripMenuItem mnuSystem = new ToolStripMenuItem("Hệ thống");
        private readonly ToolStripMenuItem mnuOperations = new ToolStripMenuItem("Nghiệp vụ");
        private readonly ToolStripStatusLabel sessionStatus = new ToolStripStatusLabel();

        public FrmMain(string username, string role) : this(username, role, null)
        {
        }

        public FrmMain(string username, string role, string employeeId)
        {
            currentUser = username;
            currentRole = role;
            currentEmployeeId = employeeId;
            InitializeComponent();
            ConfigureShell();
            BuildMenus();
        }

        private void ConfigureShell()
        {
            IsMdiContainer = true;
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(900, 600);
            BackColor = Color.FromArgb(247, 244, 239);
            menuStrip1.BackColor = Color.FromArgb(60, 42, 33);
            menuStrip1.ForeColor = Color.White;
            menuStrip1.Font = new Font("Segoe UI", 10F);
            statusStrip1.BackColor = Color.FromArgb(60, 42, 33);
            statusStrip1.ForeColor = Color.FromArgb(247, 244, 239);
            sessionStatus.Spring = true;
            sessionStatus.TextAlign = ContentAlignment.MiddleRight;
            statusStrip1.Items.Add(sessionStatus);
            UpdateSessionStatus();
        }

        private void UpdateSessionStatus()
        {
            Text = string.Format("Study Coffee Shop Manager • {0} [{1}]", currentUser, currentRole);
            sessionStatus.Text = string.Format("Phiên làm việc: {0} | Vai trò: {1}", currentUser, currentRole);
        }

        private void BuildMenus()
        {
            menuStrip1.Items.Clear();
            menuStrip1.Items.Add(mnuOperations);
            menuStrip1.Items.Add(mnuManagement);
            menuStrip1.Items.Add(mnuSystem);

            mnuOperations.DropDownItems.Clear();
            AddMenuItem(mnuOperations, "Sơ đồ bàn & gọi món", delegate { OpenChildForm(new FrmOrder(currentEmployeeId)); });
            AddMenuItem(mnuOperations, "Doanh thu", delegate { OpenChildForm(new FrmRevenueReport()); });
            AddMenuItem(mnuOperations, "Mặt hàng bán chạy", delegate { OpenChildForm(new FrmBestSellerReport()); });

            mnuManagement.DropDownItems.Clear();
            AddMenuItem(mnuManagement, "Danh mục món", delegate { OpenChildForm(new FrmCategory()); });
            AddMenuItem(mnuManagement, "Sản phẩm", delegate { OpenChildForm(new FrmProduct()); });
            AddMenuItem(mnuManagement, "Bàn học", delegate { OpenChildForm(new FrmTable()); });
            AddMenuItem(mnuManagement, "Khách hàng", delegate { OpenChildForm(new FrmCustomer()); });
            AddMenuItem(mnuManagement, "Nguyên liệu", delegate { OpenChildForm(new FrmIngredient()); });
            AddMenuItem(mnuManagement, "Công thức", delegate { OpenChildForm(new FrmRecipe()); });

            mnuSystem.DropDownItems.Clear();
            AddMenuItem(mnuSystem, "Tài khoản nhân viên", delegate { OpenChildForm(new FrmUser()); });
            AddMenuItem(mnuSystem, "Nhóm quyền", delegate { OpenChildForm(new FrmRole()); });
            AddMenuItem(mnuSystem, "Đổi mật khẩu", delegate
            {
                using (FrmChangePassword form = new FrmChangePassword(currentUser))
                    form.ShowDialog(this);
            });
            mnuSystem.DropDownItems.Add(new ToolStripSeparator());
            AddMenuItem(mnuSystem, "Đăng xuất", Logout);

            ApplyRolePermissions();
        }

        private static void AddMenuItem(ToolStripMenuItem parent, string text, Action action)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(text);
            item.Click += delegate { action(); };
            parent.DropDownItems.Add(item);
        }

        private void ApplyRolePermissions()
        {
            bool isAdmin = string.Equals(currentRole, "Admin", StringComparison.OrdinalIgnoreCase);
            bool isCashier = string.Equals(currentRole, "Thu ngân", StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(currentRole, "ThuNgân", StringComparison.OrdinalIgnoreCase);
            bool isBarista = string.Equals(currentRole, "Pha chế", StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(currentRole, "PhaChế", StringComparison.OrdinalIgnoreCase);

            mnuManagement.Visible = isAdmin;
            mnuSystem.Visible = true;
            foreach (ToolStripItem item in mnuSystem.DropDownItems)
            {
                if (item is ToolStripMenuItem && item.Text != "Đổi mật khẩu" && item.Text != "Đăng xuất")
                    item.Visible = isAdmin;
            }

            mnuOperations.DropDownItems[0].Visible = isAdmin || isCashier || isBarista;
            mnuOperations.DropDownItems[1].Visible = isAdmin || isCashier;
            mnuOperations.DropDownItems[2].Visible = isAdmin || isCashier;
        }

        private void OpenChildForm(Form childForm)
        {
            foreach (Form form in MdiChildren)
            {
                if (form.GetType() == childForm.GetType())
                {
                    childForm.Dispose();
                    form.Activate();
                    return;
                }
            }

            childForm.MdiParent = this;
            childForm.Show();
        }

        private void Logout()
        {
            Hide();
            using (FrmLogin login = new FrmLogin())
            {
                if (login.ShowDialog() == DialogResult.OK)
                {
                    Form[] openChildren = MdiChildren;
                    foreach (Form child in openChildren)
                        child.Close();
                    currentUser = login.LoggedInUser;
                    currentRole = login.LoggedInRole;
                    currentEmployeeId = login.LoggedInEmployeeId;
                    UpdateSessionStatus();
                    BuildMenus();
                    Show();
                    return;
                }
            }

            Close();
        }

        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            statusStrip1 = new StatusStrip();
            SuspendLayout();
            menuStrip1.Dock = DockStyle.Top;
            menuStrip1.Name = "menuStrip1";
            menuStrip1.TabIndex = 0;
            statusStrip1.Dock = DockStyle.Bottom;
            statusStrip1.Name = "statusStrip1";
            statusStrip1.TabIndex = 1;
            ClientSize = new Size(1100, 700);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "FrmMain";
            ResumeLayout(false);
            PerformLayout();
        }

        private StatusStrip statusStrip1;
        private MenuStrip menuStrip1;
    }
}

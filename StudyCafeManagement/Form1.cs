using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudyCafeManagement.Forms
{
    public partial class Form1 : Form
    {
        private Form activeForm = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Khởi tạo mặc định
        }

        // Hàm hỗ trợ nhúng Form con vào panel làm việc chính (pnlDesktop)
        private void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pnlDesktop.Controls.Add(childForm);
            pnlDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        #region Sự kiện Click các nút Menu bên phải

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmCustomer());
        }

        private void btnIngredient_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmIngredient());
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmProduct());
        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmTable());
        }

        private void btnRecipe_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmRecipe());
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmCategory());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát chương trình?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        #endregion
    }
}
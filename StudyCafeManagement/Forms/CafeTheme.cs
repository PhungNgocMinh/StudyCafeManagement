using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudyCafeManagement.Forms
{
    internal static class CafeTheme
    {
        private static readonly Color Oatmeal = Color.FromArgb(247, 244, 239);
        private static readonly Color Espresso = Color.FromArgb(60, 42, 33);
        private static readonly Color Caramel = Color.FromArgb(200, 138, 88);
        internal static readonly Color Sage = Color.FromArgb(100, 120, 100);
        internal static readonly Color Terracotta = Color.FromArgb(180, 80, 80);
        private static readonly Color Cream = Color.FromArgb(255, 253, 249);
        private static readonly Color Border = Color.FromArgb(230, 225, 218);
        private static readonly Font FormFont = new Font("Segoe UI", 9F);
        private static readonly Font InputFont = new Font("Segoe UI", 9.5F);
        private static readonly Font ButtonFont = new Font("Segoe UI", 9F, FontStyle.Bold);
        private static readonly Font GridHeaderFont = new Font("Segoe UI", 9F, FontStyle.Bold);
        private static readonly Font TitleFont = new Font("Segoe UI", 16F, FontStyle.Bold);

        internal static void Apply(Form form)
        {
            form.BackColor = Oatmeal;
            form.ForeColor = Espresso;
            form.Font = FormFont;
            ApplyToControls(form.Controls);
        }

        internal static void StyleSurface(Control surface)
        {
            surface.BackColor = Cream;
            surface.ForeColor = Espresso;
        }

        private static void ApplyToControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is DataGridView grid)
                {
                    StyleGrid(grid);
                }
                else if (control is Button button)
                {
                    StyleButton(button);
                }
                else if (control is TextBox || control is ComboBox || control is NumericUpDown)
                {
                    control.BackColor = Color.White;
                    control.ForeColor = Espresso;
                    control.Font = InputFont;
                }
                else if (control is GroupBox)
                {
                    StyleSurface(control);
                }
                else if (control is Panel || control is TabPage || control is TabControl)
                {
                    StyleSurface(control);
                }
                else if (control is PictureBox pictureBox)
                {
                    pictureBox.BackColor = Color.White;
                    pictureBox.BorderStyle = BorderStyle.FixedSingle;
                }
                else if (control is Label label)
                {
                    label.ForeColor = Espresso;
                    if (label.Name == "lblTitle")
                    {
                        label.Font = TitleFont;
                        label.ForeColor = Caramel;
                    }
                    else if (label.Name == "lblValTong")
                    {
                        label.ForeColor = Caramel;
                    }
                    else if (label.Name == "lblLegendFree")
                    {
                        label.BackColor = Sage;
                        label.ForeColor = Color.White;
                    }
                    else if (label.Name == "lblLegendBusy")
                    {
                        label.BackColor = Terracotta;
                        label.ForeColor = Color.White;
                    }
                }

                if (control.HasChildren)
                    ApplyToControls(control.Controls);
            }
        }

        private static void StyleButton(Button button)
        {
            string identity = (button.Name + " " + button.Text).ToLowerInvariant();
            Color backColor = Color.FromArgb(230, 225, 218);
            Color foreColor = Espresso;

            if (identity.Contains("delete") || identity.Contains("remove") ||
                identity.Contains("cancel") || identity.Contains("xóa") || identity.Contains("hủy"))
            {
                backColor = Terracotta;
                foreColor = Color.White;
            }
            else if (identity.Contains("save") || identity.Contains("add") ||
                     identity.Contains("pay") || identity.Contains("lưu") ||
                     identity.Contains("thêm") || identity.Contains("thanh toán"))
            {
                backColor = Caramel;
                foreColor = Color.White;
            }
            else if (identity.Contains("find") || identity.Contains("search") ||
                     identity.Contains("tìm") || identity.Contains("browse"))
            {
                backColor = Sage;
                foreColor = Color.White;
            }

            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.UseVisualStyleBackColor = false;
            button.Cursor = Cursors.Hand;
            button.Font = ButtonFont;
        }

        private static void StyleGrid(DataGridView grid)
        {
            grid.BackgroundColor = Cream;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = Border;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Espresso;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = GridHeaderFont;
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = Espresso;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(238, 216, 194);
            grid.DefaultCellStyle.SelectionForeColor = Espresso;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Oatmeal;
            grid.RowHeadersDefaultCellStyle.BackColor = Espresso;
            grid.RowHeadersDefaultCellStyle.ForeColor = Color.White;
        }
    }
}

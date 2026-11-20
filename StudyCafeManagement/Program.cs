using System;
using System.Windows.Forms;
using StudyCafeManagement.Forms;

namespace StudyCafeManagement
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (FrmLogin login = new FrmLogin())
            {
                if (login.ShowDialog() != DialogResult.OK)
                    return;

                Application.Run(new FrmMain(login.LoggedInUser, login.LoggedInRole, login.LoggedInEmployeeId));
            }
        }
    }
}
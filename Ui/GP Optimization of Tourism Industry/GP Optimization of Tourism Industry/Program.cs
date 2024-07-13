using System;
using System.Windows.Forms;

namespace GP_Optimization_of_Tourism_Industry
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LoginForm loadingForm = new LoginForm();
            Application.Run(loadingForm);
        }
    }
}
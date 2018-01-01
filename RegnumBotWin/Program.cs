using Dependencias;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegnumBotWin
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
            var kernel = new StandardKernel(new NinjectRoModule());
            var form = kernel.Get<Form1>();
            Application.Run(form);
        }
    }
}

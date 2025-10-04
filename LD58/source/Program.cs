using ChaosFramework.Components;
using System;
using System.Windows.Forms;

namespace LD58
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BaseGame().window);
        }
    }
}

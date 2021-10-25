using System;
using Gtk;

namespace Greending
{
    class MainClass
    {
        public static string[] arguments;
        public static void Main(string[] args)
        {
            arguments = args;
            Application.Init();
            MainWindow win = new MainWindow();
            win.Show();
            Application.Run();
        }
    }
}

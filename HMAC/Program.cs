using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SHA1;
using System.Text;

namespace HMAC
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
            var digester = new Digester();
            var a = "072d90b20945c01d1034dbe0bf24b28a7d372b34";
            var b = Encoding.UTF8.GetBytes(a);
            var c = digester.Digest(b);
            Console.WriteLine(c);
            Application.Run(new Form1());
        }
    }
}

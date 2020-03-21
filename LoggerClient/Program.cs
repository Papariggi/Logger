using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoggerClient
{
    static class Program
    {
        [STAThread]
        static void Main(String[] args)
        {
            Client.run("192.168.1.48", 3001);
        }
    }
}

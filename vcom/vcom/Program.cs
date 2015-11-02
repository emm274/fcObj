using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using vyserlibLib;

namespace vcom
{
    class Program
    {
        static void Main(string[] args)
        {
            IVirtualSerialLibrary lib = new VirtualSerialLibrary();

            if (lib != null) {

                Console.WriteLine("VirtualSerialLibrary:");

                IVirtualSerialDevice port1 = lib.createDevice("COM5");

                if (port1 != null)
                {
                    Console.WriteLine("port1[COM5] open.");
                }

                Console.ReadKey();
            }
        }
    }
}

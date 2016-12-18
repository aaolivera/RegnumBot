using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Servicios;

namespace RegnumBot
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                var a = new RegnumReader();


                a.TomarDatos();
                Thread.Sleep(2000);
            }
            
        }
    }
}

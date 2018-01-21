using Dependencias;
using Dominio;
using Ninject;
using Ninject.Extensions.Logging;
using Servicios.RegnumProviders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RegnumBotCartero
{
    class Program
    {
        private static readonly IKernel kernel = new StandardKernel(new NinjectRoModule());
        private static readonly ILogger log = kernel.Get<ILoggerFactory>().GetCurrentClassLogger();
        private static readonly AventuraProvider aventuraProvider = kernel.Get<AventuraProvider>();

        private static readonly MapaProvider mapaProvider = kernel.Get<MapaProvider>();

        static void Main(string[] args)
        {
            //var aventura = ObtenerAventura();
            
            mapaProvider.Mover(new Point(980,810));

            Console.ReadLine();
        }

        static Aventura ObtenerAventura()
        {
            log.Info("Esperando aventura");
            Aventura aventura = null;
            while (aventura == null)
            {
                aventura = aventuraProvider.Obtener();
                Thread.Sleep(500);
            }
            log.Info($"Aventura: {aventura}");
            return aventura;
        }
    }
}

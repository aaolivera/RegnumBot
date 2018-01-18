
using Ninject.Extensions.Logging;

namespace Servicios.RegnumProviders
{
    public class MoverPjProvider : RegnumProvider
    {
        public MoverPjProvider(ILogger log) : base(null, null, log)
        {
            
        }

        public void Girar(bool derecha, decimal rad)
        {
            _log.Debug($"Girar direccion {derecha} cantidad {rad}");
        }

        public void Avanzar(double distancia)
        {
            _log.Debug($"Avanzar {distancia}");
        }
    }
}

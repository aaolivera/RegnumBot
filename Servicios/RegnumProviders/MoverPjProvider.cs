
using Ninject.Extensions.Logging;
using Servicios.InternalProviders;

namespace Servicios.RegnumProviders
{
    public class MoverPjProvider : RegnumProvider
    {
        private readonly KeyProvider keyProvider;

        public MoverPjProvider(KeyProvider keyProvider, ILogger log) : base(null, null, log)
        {
            this.keyProvider = keyProvider;
        }

        public void Girar(decimal rad)
        {
            var comando = string.Empty;
            for(var i = 0; i < (rad / 0.05m); i++)
            {
                keyProvider.KeyPress("q");
            }
            
            _log.Debug($"Girar {rad}");
        }

        public void Avanzar(double distancia)
        {
            _log.Debug($"Avanzar {distancia}");
        }
    }
}

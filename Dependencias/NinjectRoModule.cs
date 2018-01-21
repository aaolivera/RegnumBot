using Ninject;
using Ninject.Modules;
using Servicios.InternalProviders;
using Servicios.RegnumProviders;
using System;

namespace Dependencias
{
    public class NinjectRoModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(FrameProvider)).To(typeof(FrameProvider)).InSingletonScope().WithConstructorArgument("processName", "ROClientGame");
            Bind(typeof(MouseProvider)).To(typeof(MouseProvider)).InSingletonScope().WithConstructorArgument("processName", "ROClientGame");
            Bind(typeof(KeyProvider)).To(typeof(KeyProvider)).InSingletonScope().WithConstructorArgument("processName", "ROClientGame");
            Bind(typeof(ColorProvider)).To(typeof(ColorProvider)).InSingletonScope();

            Bind(typeof(CoordenadasProvider)).To(typeof(CoordenadasProvider)).InSingletonScope();
            Bind(typeof(ObjetivoProvider)).To(typeof(ObjetivoProvider)).InSingletonScope();
            Bind(typeof(PiedraProvider)).To(typeof(PiedraProvider)).InSingletonScope();
            Bind(typeof(StatsProvider)).To(typeof(StatsProvider)).InSingletonScope();
            Bind(typeof(AventuraProvider)).To(typeof(AventuraProvider)).InSingletonScope();
            Bind(typeof(MapaProvider)).To(typeof(MapaProvider)).InSingletonScope();
        }
    }
}
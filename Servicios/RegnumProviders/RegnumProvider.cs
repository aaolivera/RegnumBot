using System.Collections.Generic;
using Dominio.Handlers;
using Servicios.InternalProviders;

namespace Servicios.RegnumProviders
{
    public class RegnumProvider
    {
        protected readonly FrameProvider _frameProvider;
        protected readonly MouseProvider _mouseProvider;
        protected readonly Dictionary<EventType, List<IFrameEventHandler>> _frameEventHandlers;
        
        protected RegnumProvider(FrameProvider frameProvider, MouseProvider mouseProvider)
        {
            this._frameProvider = frameProvider;
            this._mouseProvider = mouseProvider;
            this._frameEventHandlers = new Dictionary<EventType, List<IFrameEventHandler>>();
        }
        
        //EVENTOS
        protected void EjecutarEvento(object obj, EventType name)
        {
            if (_frameEventHandlers.ContainsKey(name) && _frameEventHandlers[name] != null)
            {
                _frameEventHandlers[name].ForEach(x => x.Ejecutar(obj));
            }
        }

        public void RegistrarHandler(EventType name, IFrameEventHandler frameEventHandler)
        {
            if (_frameEventHandlers.ContainsKey(name))
            {
                _frameEventHandlers[name].Add(frameEventHandler);
            }
            else
            {
                _frameEventHandlers.Add(name, new List<IFrameEventHandler> { frameEventHandler });
            }
        }
    }

}

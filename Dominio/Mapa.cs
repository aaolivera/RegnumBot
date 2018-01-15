using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Mapa
    {
        private List<Nodo> Nodos = new List<Nodo>();
        public Nodo ObtenerNodo(int x, int y)
        {
            for (var i = 0; i < Nodos.Count; i++)
            {
                if (Nodos[i].HayInterseccion(x, y))
                {
                    return Nodos[i];
                }
            }
            return null;
        }

        public Nodo AgregarNodo (Nodo n)
        {
            var nodo = ObtenerNodo(n.X, n.Y);
            if (nodo == null)
            {
                nodo = new Nodo(n.X, n.Y);
                Nodos.Add(nodo);
            }
            if(n.NodosAsociados != null)
            {
                foreach (var na in n.NodosAsociados)
                {
                    var nodoAgregado = AgregarNodo(na);
                    nodo.AgregarAsociado(nodoAgregado);
                    nodoAgregado.AgregarAsociado(nodo);
                }
            }
            
            return nodo;
        }
    }
}

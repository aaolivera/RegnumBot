using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Nodo
    {
        public Nodo()
        {

        }

        public Nodo(int x, int y)
        {
            X = x;
            Y = y;
            NodosAsociados = new List<Nodo>();
        }

        public int Radio { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public List<Nodo> NodosAsociados { get; set; }

        public bool HayInterseccion(int x, int y)
        {
            var distancia = Math.Sqrt((X - x) * (X - x) + (Y - y) * (Y - y));
            return distancia <= Radio * 2;
        }

        public void AgregarAsociado(Nodo nodo)
        {
            if (!NodosAsociados.Contains(nodo)) NodosAsociados.Add(nodo);
        }
        //public bool Seleccionado { get; set; }
        //public decimal Direccion { get; set; }
        //public decimal Direccion { get; set; }
    }
}

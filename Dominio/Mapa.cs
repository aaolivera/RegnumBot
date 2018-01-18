using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Mapa
    {
        private List<Nodo> Nodos = new List<Nodo>();
        public Nodo ObtenerNodo(Point pos)
        {
            for (var i = 0; i < Nodos.Count; i++)
            {
                if (Nodos[i].HayInterseccion(pos))
                {
                    return Nodos[i];
                }
            }
            return null;
        }

        public Nodo AgregarNodo (Nodo n)
        {
            var nodo = ObtenerNodo(new Point(n.X, n.Y));
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

        public void AgregarNodoAsociadoAlCercano(Nodo n)
        {
            Nodo nodoCercano = null;
            double distancia = 99999;
            foreach (var na in Nodos)
            {
                var nuevaDistancia = na.Distancia(n);
                if (nuevaDistancia < distancia)
                {
                    nodoCercano = na;
                    distancia = nuevaDistancia;
                }
            }

            n.AgregarAsociado(nodoCercano);
            nodoCercano.AgregarAsociado(n);
            Nodos.Add(n);
        }

        public List<Nodo> DefinirCamino(Point desde, Point destino)
        {
            foreach(var n in Nodos)
            {
                n.Peso = 99999;
                n.Anterior = null;
                n.Marcado = false;
            }

            var nodoInicial = ObtenerNodo(desde);
            var nodoFinal = ObtenerNodo(destino);
            nodoInicial.Peso = 0;
            var stack = new Queue<Nodo>();
            stack.Enqueue(nodoInicial);
            RecorrerGrafo(stack);

            return Recorrido(nodoFinal);
        }

        private void RecorrerGrafo(Queue<Nodo> stack)
        {
            var nodoActual = stack.Dequeue();

            nodoActual.Marcado = true;
            foreach(var n in nodoActual.NodosAsociados.Where(x => !x.Marcado))
            {
                var nuevoPeso = nodoActual.Peso + n.Distancia(nodoActual);
                if(nuevoPeso < n.Peso)
                {
                    n.Peso = nuevoPeso;
                    n.Anterior = nodoActual;
                }
                stack.Enqueue(n);
            }
            if(stack.Count > 0) RecorrerGrafo(stack);
        }

        private List<Nodo> Recorrido(Nodo nodo)
        {
            if(nodo.Anterior == null)
            {
                return new List<Nodo>() { nodo };
            }
            var lista = Recorrido(nodo.Anterior);
            lista.Add(nodo);
            return lista;
        }
    }
}

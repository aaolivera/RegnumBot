function Nodo(x, y) {
    var self = this;
    self.Radio = 3;
    self.X = x;
    self.Y = y;
    self.HayInterseccion = function (x, y) {
        var distancia = Math.sqrt((self.X - x) * (self.X - x) + (self.Y - y) * (self.Y - y));
        return distancia <= self.Radio * 2 - 1;
    }
    self.Seleccionado = false;
    self.NodosAsociados = [];
}

function Mapa() {
    var self = this;
    self.Nodos = [];

    self.OrdenarNodos = function () {
        self.Nodos.sort(function (a, b) {
            if (a.X === b.X) return (a.Y < b.Y) ? -1 : (a.Y > b.Y) ? 1 : 0;
            return a.X < b.Y ? -1 : 0;
        });
    }

    self.SeleccionarNodo = function (x, y) {
        for (var i = 0; i < self.Nodos.length; i++) {
            if (self.Nodos[i].HayInterseccion(x, y)) {
                return self.Nodos[i];
            }
        }
        return null;
    }

    self.ObtenerNodo = function (x, y) {
        var nodo = self.SeleccionarNodo(x, y);
        if (nodo === null) {
            nodo = new Nodo(x, y);
            self.Nodos.push(nodo);
        }
        self.OrdenarNodos();
        return nodo;
    }

}

$(document).ready(function () {
    var mapa = new Mapa();
    var canvas = document.getElementById('myCanvas');
    var ctx = canvas.getContext('2d');
    drawCanvas(ctx, canvas, mapa);


    var mouseIsDown = false;
    var nodoSeleccionado = null;
    canvas.onmousedown = function (e) {
        if (event.which === 1 || event.which === 3) {
            mouseIsDown = true;
            nodoSeleccionado = onclick(ctx, e, mapa, nodoSeleccionado);
            drawCanvas(ctx, canvas, mapa);
        }
    }

    canvas.onmouseup = function (e) {
        mouseIsDown = false;
        if (nodoSeleccionado !== null) {
            nodoSeleccionado.Seleccionado = false;
        }
        
        if (event.which === 1) {
            onUpLeftClick(ctx, e, mapa, nodoSeleccionado);
        }

        if (nodoSeleccionado !== null || event.which === 1) {
            drawCanvas(ctx, canvas, mapa);
        }
        
        return false;
    }

    canvas.onmousemove = function (e) {
        if (!mouseIsDown) return;
        if(event.which === 3) {
            onMoveRigthClick(ctx, e, mapa, nodoSeleccionado);
            drawCanvas(ctx, canvas, mapa);
        }
        return false;
    }
    document.addEventListener('contextmenu', event => event.preventDefault());

});

function drawCanvas(ctx, canvas, mapa) {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.stroke();
    for (var i = 0; i < mapa.Nodos.length; i++) {
        var nodo = mapa.Nodos[i];
        ctx.beginPath();
        ctx.arc(nodo.X, nodo.Y, nodo.Radio, 0, 2 * Math.PI);
        ctx.fillStyle = nodo.Seleccionado ? 'red' : 'orange';
        ctx.fill();
        ctx.stroke();
    }
    for (var i = 0; i < mapa.Nodos.length; i++) {
        var nodo = mapa.Nodos[i];
        for (var j = 0; j < nodo.NodosAsociados.length; j++) {
            ctx.beginPath();
            ctx.moveTo(nodo.X, nodo.Y);
            ctx.lineTo(nodo.NodosAsociados[j].X, nodo.NodosAsociados[j].Y);
            ctx.stroke();
        }
    }
}

function onclick(ctx, e, mapa, nodoSeleccionado) {
    var x = e.offsetX;
    var y = e.offsetY;
    if (nodoSeleccionado !== null) {
        nodoSeleccionado.Seleccionado = false;
    }
    escribirEnConsola('Selecciona: ' + x + '/' + y);
    var nodo = mapa.ObtenerNodo(x, y);
    nodo.Seleccionado = true;
    return nodo;
}

function onMoveRigthClick(ctx, e, mapa, nodoSeleccionado) {
    var x = e.offsetX;
    var y = e.offsetY;

    nodoSeleccionado.X = x;
    nodoSeleccionado.Y = y;
}

function onUpLeftClick(ctx, e, mapa, nodoSeleccionado) {
    var x = e.offsetX;
    var y = e.offsetY;

    var nodo = mapa.ObtenerNodo(x, y);

    if (nodoSeleccionado !== nodo) {
        nodoSeleccionado.NodosAsociados.push(nodo);
        nodo.NodosAsociados.push(nodoSeleccionado);
    }
}

function escribirEnConsola(texto) {
    $("#consola").html(">" + texto + "<br />" + $("#consola").html());
}

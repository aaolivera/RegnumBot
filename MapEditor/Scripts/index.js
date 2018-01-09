var ancho = 4;

$(document).ready(function () {
    var mapa = inicializarMapa();
    var canvas = document.getElementById('myCanvas');
    var ctx = canvas.getContext('2d');
    drawCanvas(ctx, mapa);


    var mouseIsDown = false;

    canvas.onmousedown = function (e) {
        mouseIsDown = true;
        drawCelda(ctx, e, mapa);
    }
    canvas.onmouseup = function (e) {
        mouseIsDown = false;
        return false;
    }

    canvas.onmousemove = function (e) {
        if (!mouseIsDown) return;
        drawCelda(ctx, e, mapa);

        return false;
    }
    document.addEventListener('contextmenu', event => event.preventDefault());
});

function inicializarMapa() {
    var filas = [];
    dimension = 3000;
    for (var i = 0; i < dimension; ++i) {
        var fila = [];
        for (var j = 0; j < dimension; ++j) {
            fila.push(0);
        }
        filas.push(fila);
    }

    return filas;
}

function drawCanvas(ctx, mapa) {
    var imageObj = new Image();

    imageObj.onload = function () {
        ctx.drawImage(imageObj, 0, 0, imageObj.width, imageObj.height);

        for (var i = 0; i < mapa.length; i++) {
            var fila = mapa[i];
            for (var j = 0; j < fila.length; j++) {
                ctx.lineWidth = 0.3;
                if (fila[j] != 0) {
                    primero = false;
                    ctx.fillStyle = "blue";
                    ctx.fillRect(j * ancho, i * ancho, ancho, ancho);
                } else {
                    ctx.rect(j * ancho, i * ancho, ancho, ancho);
                }
            }
        }
    };

    imageObj.src = 'http://localhost:56745/Img/Mapa.png';
}

function drawCelda(ctx, e, mapa) {
    var x = e.offsetX - e.offsetX % ancho;
    var y = e.offsetY - e.offsetY % ancho;
    
    if (event.which === 1) {
        escribirEnConsola('Activa: ' + Math.round(x / 3) + '/' + Math.round(y / 3));
        ctx.lineWidth = 0.3;
        ctx.fillStyle = "blue";
        ctx.fillRect(x, y, ancho, ancho);
        mapa[y][x] = 1;
    }
    else if (event.which === 3) {
        escribirEnConsola('Desactiva: ' + Math.round(x / 3) + '/' + Math.round(y / 3));
        ctx.lineWidth = 0.3;
        ctx.fillStyle = "red";
        ctx.fillRect(x, y, ancho, ancho);
        mapa[y][x] = 0;
    }
}

function escribirEnConsola(texto) {
    $("#consola").html(">" + texto + "<br />" + $("#consola").html());
}

var ancho = 1;
$(document).ready(function () {
    var mapa = inicializarMapa();
    var canvas = document.getElementById('myCanvas');
    var ctx = canvas.getContext('2d');
    drawCanvas(ctx, mapa);


    var mouseIsDown = false;

    canvas.onmousedown = function (e) {
        mouseIsDown = true;
        onclick(ctx, e, mapa);
    }
    canvas.onmouseup = function (e) {
        mouseIsDown = false;
        return false;
    }

    canvas.onmousemove = function (e) {
        if (!mouseIsDown) return;
        onclick(ctx, e, mapa);

        return false;
    }
    document.addEventListener('contextmenu', event => event.preventDefault());

    $("#guardar").click(function () {
        guardar(mapa);
    });

    $("#abrir").click(function () {
        document.getElementById('abrirOculto').click();
    });
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
        //Dibujo imagen
        ctx.drawImage(imageObj, 0, 0, imageObj.width / 3, imageObj.height / 3);

        //Dibujo los caminos
        for (var i = 0; i < mapa.length; i++) {
            var fila = mapa[i];
            for (var j = 0; j < fila.length; j++) {
                drawCelda(ctx,j , i, fila[j]);
            }
        }
    };

    imageObj.src = mapaUrl;
}

function drawCelda(ctx, x , y, color) {
    ctx.lineWidth = 0.3;
    if (color == 1) {
        ctx.fillStyle = "blue";
        ctx.fillRect(x, y, ancho, ancho);
    }
    if (color == 3) {
        ctx.fillStyle = "red";
        ctx.fillRect(x, y, ancho, ancho);
    }
}

function onclick(ctx, e, mapa) {
    var x = Math.round((e.offsetX - e.offsetX % ancho ));
    var y = Math.round((e.offsetY - e.offsetY % ancho ));

    escribirEnConsola((event.which === 1 ? 'Activa: ' : 'Desactiva: ') + x + '/' + y);
    drawCelda(ctx, x, y, event.which);

    if (event.which === 1) {
        mapa[y][x] = 1;
    }
    else if (event.which === 3) {
        mapa[y][x] = 0;
    }
}

function escribirEnConsola(texto) {
    $("#consola").html(">" + texto + "<br />" + $("#consola").html());
}

function guardar(mapa) {
    var texto = "";
    var arrayLength = mapa.length;
    for (var i = 0; i < arrayLength; i++) {
        texto += mapa[i].join() + "\n";
    }
    var blob = new Blob([texto], { type: "text/plain;charset=utf-8" });
    saveAs(blob, "mapa.txt");
}

function abrir(event) {
    $.blockUI();
    var input = event.target;
    var resultado = "";
    var reader = new FileReader();
    reader.onload = function () {
        resultado = reader.result;
        console.log(reader.result.substring(0, 200));

        var filas = resultado.split('\n');
        var mapaAux = [];
        var arrayLength = filas.length;
        for (var i = 0; i < arrayLength; i++) {
            mapaAux.push(filas[i].split(','));
        }

        var canvas = document.getElementById('myCanvas');
        var ctx = canvas.getContext('2d');
        drawCanvas(ctx, mapaAux);
        $.unblockUI();
    };
    reader.readAsText(input.files[0]);    
}
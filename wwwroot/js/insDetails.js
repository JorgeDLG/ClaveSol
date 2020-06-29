////////////////// MEDIA ////////////////////

var escenario = $("#escenario");
var plano = $("#plano");
var plano2 = $("#2plano");
var plano3 = $("#3plano");
var plano4 = $("#4plano");


var mediaEsc = $(".multimedia").find("#escenario");

//escenario.hover(addControls);
ClickEvToPlanos();

function swap(plano) {
    /*1. Coge media contenida MEDIAescenario & MEDIAplano
        2. haz MEDIAplano.replaceWith(MEDIAescenario);
        3. same VICEVERSA
        4. Si han perdido atributos añadelos. 
            (IF video esta en escenario add controls)
    */
    //1 

    var mediaPlano = $(".multimedia").find("#" + plano);
    //mediaPlano.css("border","1px solid red");

    var objPlano = $(mediaPlano).find("video");
    if (objPlano.length == 0) {
        objPlano = $(mediaPlano).find("img");
        objPlano.length == 0 ? console.log("Plano no contiene img ni vid") : console.log("Plano contiene img")
    }

    var objEsc = $(mediaEsc).find("video");
    if (objEsc.length == 0) {
        objEsc = $(mediaEsc).find("img");
        objEsc.length == 0 ? console.log("Escenario no contiene img ni vid") : console.log("Escenario contiene img")
    }

    objEsc.replaceWith(objPlano);
    mediaPlano.append(objEsc);

}

function addControls() {
    //AMPLIAR ELEMENTO + boton escape
    mediaEsc.css("border", "1px solid red");
    console.log(mediaEsc);

    video = mediaEsc.find("video");
    if (video.length == 1) {
        mediaEsc.css("border", "1px solid blue");
        video.prop("controls");
    }
}

function ClickEvToPlanos() {
    for (let i = 1; i <= 4; i++) {
        let plaNo, tagPlano;
        i == 1 ? (plaNo = "plano", tagPlano = "plano") : (plaNo = "plano" + i, tagPlano = i + "plano")

        eval(plaNo).on("click", () => swap(tagPlano));
    }
}

////////////////// ATTRIBUTES ////////////////////

var colorContainer = $("#Color");

try {
    var colorsOptions = colorContainer.find("div");

    colorsOptions.each(function() {
        $(this).click(select);
    });

} catch (error) {
    console.log(error);
}

function select() {
    colorContainer.find("input").removeAttr("checked");
    colorContainer.find("div").removeClass("selected");

    $(this).addClass("selected");
    $(this).find("input").attr("checked", "checked");
}

//SAVE COMMENTS
$('#myModal').on('shown.bs.modal', function() {
    $('#myInput').trigger('focus')
})

function afterSaveComment() {
    var url = window.location.href;
    var insId = url.slice(url.lastIndexOf("/") + 1);
    var title = $('#title').val(); //coge el valor 
    var body = $('#body').val(); //coge el valor 

    console.log(insId)

    $.ajax({
        url: `/Comments/createComment?insId=${insId}&title=${title}&body=${body}`

    }).done(function(data) { //NO SE RECUPERAN BIEN LOS DATOS
        console.log("data is: " + data);
        window.location.reload();
    });
}

//ADD TO CART W/ ATTRIBUTES
function addToCartwAttrs() {

    var url = window.location.href;
    var insId = url.slice(url.lastIndexOf("/") + 1);

    var color = $("div#Color").find("input[checked*='checked']").attr("value");

    //debugger;
    var restoAttrbsValues = getRestAttributesValues();
    var attrbsValuesStr = color == null ? restoAttrbsValues : color + ',' + restoAttrbsValues
    console.log("Attributes:" + attrbsValuesStr);
    console.log("Instrument ID" + insId, "tipo: " + typeof insId);

    // if (attrbsValuesStr == "") { attrbsValuesStr = null }

    $.ajax({
        url: `/Cart/addToCart?id=${insId}&attribsValues=${attrbsValuesStr}`

    }).done(function(data) {
        console.log("Ajax response OK, count Lines on Cart:" +
            data);

        //Contador
        $(".badge").empty();
        $(".badge").append(data);
    });
}

function getRestAttributesValues() {

    var values = new Array();
    $(".selectAttribs").each(function() {
        values.push($(this).prop("selectedOptions")[0].value);
    });

    return values.join();

}
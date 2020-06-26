// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function testScriptAccess() {
    alert("Script site.js accessed");
}
refreshCounter();

function refreshCounter() {
    $.ajax({
        url: `/Cart/getNlinesCart/`
    }).done(function(data) {
        $(".badge").empty();
        $(".badge").append(data);
    });
}

var linksDeleteLine = $(".deleteLineOrderlink");
linksDeleteLine.each(function() {
    let tdId = $(this).parent().siblings(".lineIds");
    let lineId = tdId[0].innerText;
    $(this).click(() => ajaxDeleteLine(lineId));
});

function ajaxDeleteLine(lineOrderId) {
    $.ajax({
        url: `/Cart/deleteLine/${lineOrderId}`

    }).done(function(data) {
        //alert("Nlines in cart:",String(data)); //why NULL?
        location.reload();
    });
}

//+ & - QUANTITY BUTTONS
var quantityTds = $(".quantity");

quantityTds.each(function() {
    let tdId = $(this).siblings(".lineIds");
    let lineId = tdId[0].innerText;

    $(this).find(".svgLess").click(() => ajaxQuantityBy1(lineId, -1));
    $(this).find(".svgMore").click(() => ajaxQuantityBy1(lineId, 1));

    //   let htmlBlock = less[0] + $(this)[0].innerHTML + more[0];
    //    $(this)[0].innerHTML = htmlBlock;
});

function ajaxQuantityBy1(id, operation) {
    $.ajax({
        url: `/Cart/quantityBy1?id=${id}&operation=${operation}`

    }).done(function(data) {
        console.log(data, typeof data);

        location.reload();
    });
}

/*///////////////// ATTRIBUTES ////////////////////

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
*/
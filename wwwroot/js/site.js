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
    }).done(function (data) {
        $(".badge").empty();
        $(".badge").append(data);
    });
}

var linksDeleteLine = $(".deleteLineOrderlink");
linksDeleteLine.each(function () {
    //NO PILLA BIEN ID
  let tdId = $(this).parent().siblings(".lineIds");
  let lineId = tdId[0].innerText;
  $(this).click(() => ajaxDeleteLine(lineId));
});

function ajaxDeleteLine(lineOrderId) {
    $.ajax({
        url: `/Cart/deleteLine/${lineOrderId}`

    }).done(function (data){
        //why can't render data? 
        alert("Nlines in cart:",String(data));
        location.reload();
    });
}
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
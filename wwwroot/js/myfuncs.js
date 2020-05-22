console.log("loaded");

var elements = $('body,nav,p,svg,.card-body').not('.bi-bucket');

if (localStorage.getItem('darkMode') == null) {
    localStorage.setItem('darkMode','0');
}

var inDarkMode = localStorage.getItem('darkMode');   

if (inDarkMode == 1) {
    elements.addClass("bg-dark text-white")
}

function switchDarkMode() {
    inDarkMode = localStorage.getItem('darkMode');
    elements.toggleClass("bg-dark text-white");

    if (inDarkMode == '0') {
        localStorage.setItem('darkMode','1'); //NO CAMBIA
    }else{
        localStorage.setItem('darkMode','0');
    } 
}
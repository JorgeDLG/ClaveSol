console.log("loaded myfuncs.js");

var elements = $('body,nav,p,svg,.card-body,ol,div.card,li').not('.bi-cart4');
var tables = $('table');

if (localStorage.getItem('darkMode') == null) { //init rutine (inizialice darkMode to false)
    localStorage.setItem('darkMode','0');
}

var inDarkMode = localStorage.getItem('darkMode');  //retrieve darkmode variable 

if (inDarkMode == 1) {
    elements.addClass("bg-dark text-white")
    tables.addClass("table-dark")
}

function switchDarkMode() { //User input Event
    inDarkMode = localStorage.getItem('darkMode');
    elements.toggleClass("bg-dark text-white");
    tables.toggleClass("table-dark");

    if (inDarkMode == '0') { //Variable switcher (update state)
        localStorage.setItem('darkMode','1');
    }else{
        localStorage.setItem('darkMode','0');
    } 
}
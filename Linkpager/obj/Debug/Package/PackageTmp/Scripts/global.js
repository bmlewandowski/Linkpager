function toggle_generalpanel(d) {
    if (document.getElementById(d).style.display != "block") {
        document.getElementById(d).style.display = "block";
    }
    else {
        document.getElementById(d).style.display = "none";
    }
}

function toggle_more(d, i) {
    if (document.getElementById(d).style.display != "block") {
        document.getElementById(d).style.display = "block";
        document.getElementById(i).src = "/Images/interface/less.gif";
    }
    else {
        document.getElementById(d).style.display = "none";
        document.getElementById(i).src = "/Images/interface/more.gif";
    }
}

function toggle_loginpanel(d, i) {
    if (document.getElementById(d).style.display != "block") {
        document.getElementById(d).style.display = "block";
        document.getElementById(i).src = "/Images/interface/login_less.png";
    }
    else {
        document.getElementById(d).style.display = "none";
        document.getElementById(i).src = "/Images/interface/login_more.png";
    }
}

function toggle_folderpanel(d, i) {

    if (document.getElementById(d).style.display != "block") {
        document.getElementById(d).style.display = "block";
        document.getElementById(i).src = "/Images/interface/folder_less.gif";
    }
    else {
        document.getElementById(d).style.display = "none";
        document.getElementById(i).src = "/Images/interface/folder_more.gif";
    }
}


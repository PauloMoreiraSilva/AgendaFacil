
function openCenterWin(url, name, theWidth, theHeight)
{
    var theTop = (screen.height/2)-(theHeight/2);
    var theLeft = (screen.width/2)-(theWidth/2);
    var features = 'height='+theHeight+',width='+theWidth+',top='+theTop+',left='+theLeft+",scrollbars=yes,toolbar=no,location=no,directories=no,status=no,resizable=no";
    var w = window.open(url, name, features);
}

function openCenterWin2(url, name, theWidth, theHeight, features) {
    var theTop = (screen.height / 2) - (theHeight / 2);
    var theLeft = (screen.width / 2) - (theWidth / 2);
    var fullfeatures = 'height=' + theHeight + ',width=' + theWidth + ',top=' + theTop + ',left=' + theLeft + "," + features;
    var w = window.open(url, name, fullfeatures);
}

function ValidaTextArea(objCampo, label, spanMsg, maxSize) {
    if (objCampo.value.length > maxSize) {
        objCampo.innerText = objCampo.innerText.substr(0, maxSize);
        alert('O campo ' + label + ' comporta somente ' + maxSize + ' caracteres! (incluindo quebra de linhas)');
        objCampo.focus();
        window.event.returnValue = false;
        return false;
    }
    document.getElementById(spanMsg).innerHTML = maxSize - parseInt(objCampo.value.length);
}
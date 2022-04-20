
function AjaxFileUpload_change_text() {
    Sys.Extended.UI.Resources.AjaxFileUpload_SelectFile = "Selecionar arquivo(s)"; //"Select File";
    Sys.Extended.UI.Resources.AjaxFileUpload_DropFiles = "Arraste o(s) arquivo(s) aqui"; //"Drop files here";
    Sys.Extended.UI.Resources.AjaxFileUpload_Pending = "Pendente"; //"pending";
    Sys.Extended.UI.Resources.AjaxFileUpload_Remove = "Remover"; //"Remove";
    Sys.Extended.UI.Resources.AjaxFileUpload_Upload = "Enviar"; //"Upload";
    Sys.Extended.UI.Resources.AjaxFileUpload_Uploaded = "Enviado"; //"Uploaded";
    Sys.Extended.UI.Resources.AjaxFileUpload_UploadedPercentage = "Enviado {0} %"; //"uploaded {0} %";
    Sys.Extended.UI.Resources.AjaxFileUpload_Uploading = "Enviando"; //"Uploading";
    Sys.Extended.UI.Resources.AjaxFileUpload_FileInQueue = "{0} arquivo(s) na fila."; //"{0} file(s) in queue.";
    Sys.Extended.UI.Resources.AjaxFileUpload_AllFilesUploaded = "Todos os arquivos foram enviados."; //"All Files Uploaded.";
    Sys.Extended.UI.Resources.AjaxFileUpload_FileList = "Lista dos arquivos enviados:"; //"List of Uploaded files:";
    Sys.Extended.UI.Resources.AjaxFileUpload_SelectFileToUpload = "Selecione o(s) arquivo(s) para envio."; //"Please select file(s) to upload.";
    Sys.Extended.UI.Resources.AjaxFileUpload_Cancelling = "Cancelando..."; //"Cancelling...";
    Sys.Extended.UI.Resources.AjaxFileUpload_UploadError = "Um erro ocorreu durante o envio do arquivo"; //"An Error occured during file upload.";
    Sys.Extended.UI.Resources.AjaxFileUpload_CancellingUpload = "Cancelando o envio..."; //"Cancelling upload...";
    Sys.Extended.UI.Resources.AjaxFileUpload_UploadingInputFile = "Enviando o arquivo: {0}."; //"Uploading file: {0}.";
    Sys.Extended.UI.Resources.AjaxFileUpload_Cancel = "Cancelar"; //"Cancel";
    Sys.Extended.UI.Resources.AjaxFileUpload_Canceled = "Cancelado"; //"cancelled";
    Sys.Extended.UI.Resources.AjaxFileUpload_UploadCanceled = "Envio do arquivo cancelado"; //"File upload cancelled";
    Sys.Extended.UI.Resources.AjaxFileUpload_DefaultError = "Erro no envio do arquivo"; //"File upload error";
    Sys.Extended.UI.Resources.AjaxFileUpload_UploadingHtml5File = "Enviando arquivo: {0} de {1} bytes."; //"Uploading file: {0} of size {1} bytes.";
    Sys.Extended.UI.Resources.AjaxFileUpload_error = "Erro"; //"error";
}

window.onload = function () {
    $(".ajax__fileupload_dropzone").bind("drop", function () {
        checkfilesize();
    });
    $(".ajax__fileupload_queueContainer").bind("click", function () {
        checkfilesize();
    });
    $(".ajax__fileupload_uploadbutton").bind("mouseenter", function () {
        checkfilesize();
    });
}

function checkfilesize() {
    var total_filesize_num = 0;
    var myElements = $(".filesize");
    if (myElements.length == 0) {
        $(".ajax__fileupload_uploadbutton").css("visibility", "hidden");
        return;
    }
    for (var i = 0; i < myElements.length; i++) {
        var filesize = myElements.eq(i).html(); //$(".filesize").html();     
        var filesize_num = filesize_tonum(filesize);
        if (filesize_num > 50) {
            $(".ajax__fileupload_uploadbutton").css("visibility", "hidden");
            alert('Você selecionou um arquivo de ' + filesize + '. O máximo permitido é de 50 MB. Por favor remova este arquivo e selecione outro.');
            break;
        } else {
            $(".ajax__fileupload_uploadbutton").css("visibility", "visible");
        }
        //total_filesize_num = total_filesize_num + filesize_tonum(filesize);
    }
    /*
    if (total_filesize_num > 5) {
        $(".ajax__fileupload_uploadbutton").css("visibility", "hidden");
        alert('Maximum file size is 5MB only! Please select another one.');
        return;
    } else {
        $(".ajax__fileupload_uploadbutton").css("visibility", "visible");
    }
    */
}

function filesize_tonum(filesize) {
    var filesize_num = 0;
    if (filesize.indexOf("kb") > 0) {
        var space = filesize.lastIndexOf(" ");
        filesize_num = parseFloat("0." + filesize.substr(0, filesize.length - space + 1));
    }
    else if (filesize.indexOf("MB") > 0) {
        var space = filesize.lastIndexOf(" ");
        filesize_num = parseFloat(filesize.substr(0, filesize.length - space + 1));
    }
    else if (filesize.indexOf("GB") > 0) {
        var space = filesize.lastIndexOf(" ");
        filesize_num = (parseFloat(filesize.substr(0, filesize.length - space + 1)) * 1024);
    }
    return filesize_num;
}
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="PI4Sem.AgendaFacil.Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Templates/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div style="font-size: 9pt; padding: 50px 10px 50px 10px;">
            <table border="0" cellpadding="5" cellspacing="5" width="100%">
                <tr>
                    <td>
                        <asp:Image ID="imgError" runat="server" ImageUrl="~/Images/Erro250x208.png" /></td>
                    <td width="100%">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
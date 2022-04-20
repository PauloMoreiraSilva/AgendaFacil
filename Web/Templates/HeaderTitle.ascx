<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HeaderTitle.ascx.cs" Inherits="Templates_HeaderTitle" %>

<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/header-title-left.png" /></td>
        <td width="100%" style="BACKGROUND-IMAGE: url('<%Response.Write(ImagePath);%>/header-title-middle.png'); BACKGROUND-REPEAT: repeat-x;">
            <div style="vertical-align:middle; margin-bottom:5px;">
                <asp:Label ID="lblTitle" runat="server" Text="Label" CssClass="TextoCabecalho1"></asp:Label>
            </div>
        </td>
        <td><asp:Image ID="Image2" runat="server" ImageUrl="~/Images/header-title-right.png" /></td>
    </tr>
</table>
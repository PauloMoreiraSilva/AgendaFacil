<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccessDenied.aspx.cs" Inherits="PI4Sem.AgendaFacil.AccessDenied" MasterPageFile="~/MenuMain.master" %>

<asp:Content ID="ContentPage" ContentPlaceHolderID="ContentBody" runat="Server">
    <br />
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Access Denied.gif" /></td>
            <td style="width: 100%;">
                <asp:Label ID="lblMsg" runat="server" Text="" Font-Size="12pt" CssClass="TextoCabecalho1"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
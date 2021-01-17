<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeatureForm.ascx.cs" Inherits="NikSoft.ContentManager.Web.Panel.FeatureForm" %>
<div class="row">
    <asp:PlaceHolder ID="PlcTextBox" runat="server"></asp:PlaceHolder>
    <asp:PlaceHolder ID="PlcDropDown" runat="server"></asp:PlaceHolder>
</div>
<div class="row">
    <asp:PlaceHolder ID="PlcCheckBoxList" runat="server"></asp:PlaceHolder>
    <asp:PlaceHolder ID="PlcTextArea" runat="server"></asp:PlaceHolder>
</div>
<div class="text-center">
    <asp:Button ID="BtnSave" runat="server" CssClass="btn btn-save-nik" Text="ذخیره" />
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cu_VisualLink.ascx.cs" Inherits="NikSoft.ContentManager.Web.Panel.cu_VisualLink" %>
<div class="row">
    <div class="col-sm-6">
        <div class="form-group">
            <label class="control-label" for="txtTitle">عنوان</label>
            <Nik:NikTextBox BoxTitle="Title" ID="txtTitle" runat="server" ClientIDMode="Static" CssClass="form-control reqfield"></Nik:NikTextBox>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-sm-12">
        <div class="form-group">
            <label class="control-label" for="TxtDesc">توضیحات</label>
            <Nik:NikTextBox ID="TxtDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></Nik:NikTextBox>
        </div>
    </div>
</div>
<hr />
<div class="row">
    <div class="col-md-12 text-center">
        <Nik:NikButton ID="btnSave" runat="server" Text="ذخیره" SettingValue="SaveButton" CssClass="btn btn-default btn-save-nik" />
    </div>
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cu_GeneralMenus.ascx.cs" Inherits="NikSoft.ContentManager.Web.Panel.cu_GeneralMenus" %>
<div class="row">
    <div class="col-sm-4">
        <div class="form-group">
            <label class="control-label" for="txtTitle">عنوان</label>
            <Nik:NikTextBox BoxTitle="Title" ID="txtTitle" runat="server" ClientIDMode="Static" CssClass="form-control reqfield"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-md-4">
        <div class="checkbox">
            <div class="label-null"></div>
            <label>
                <asp:CheckBox ID="chbEnbaled" Checked="true" runat="server" ClientIDMode="Static" />
                فعال؟
            </label>
        </div>
    </div>
    <div class="col-md-4">
        <div class="checkbox">
            <div class="label-null"></div>
            <label>
                <asp:CheckBox ID="chbLogin" Checked="true" runat="server" ClientIDMode="Static" />
                نیاز به لاگین دارد؟
            </label>
        </div>
    </div>
    <div class="col-sm-12">
        <div class="form-group">
            <label class="control-label" for="txtTitle">توضیحات</label>
            <Nik:NikTextBox BoxTitle="Title" ID="txtDesc" runat="server" TextMode="MultiLine" Rows="3" ClientIDMode="Static" CssClass="form-control reqfield"></Nik:NikTextBox>
        </div>
    </div>
</div>
<hr />
<div class="row">
    <div class="col-md-12 text-center">
        <Nik:NikButton ID="btnSave" runat="server" Text="ذخیره" SettingValue="SaveButton" CssClass="btn btn-default btn-save-nik" />
    </div>
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cu_GeneralMenuItem.ascx.cs" Inherits="NikSoft.ContentManager.Web.Panel.cu_GeneralMenuItem" %>
<div class="row">
    <div class="col-sm-4">
        <div class="form-group">
            <label class="control-label" for="txtTitle">عنوان</label>
            <Nik:NikTextBox BoxTitle="Title" ID="txtTitle" runat="server" ClientIDMode="Static" CssClass="form-control reqfield"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-4">
        <div class="form-group">
            <label class="control-label" for="ddlParentMenu">منوی بالاتر</label>
            <asp:DropDownList ID="ddlParentMenu" runat="server" CssClass="form-control select2" ClientIDMode="Static"></asp:DropDownList>
        </div>
    </div>
    <div class="col-sm-4">
        <div class="form-group">
            <label class="control-label" for="TxtLink">لینک</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtLink" runat="server" ClientIDMode="Static" CssClass="form-control reqfield"></Nik:NikTextBox>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-sm-4">
        <div class="form-group">
            <label class="control-label" for="TxtClassName">کلاس</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtClassName" runat="server" ClientIDMode="Static" CssClass="form-control reqfield"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-3">
        <div class="form-group">
            <label class="control-label" for="TxtFontIcon">فونت آیکن</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtFontIcon" runat="server" ClientIDMode="Static" CssClass="form-control reqfield"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-2">
        <div class="form-group">
            <label class="control-label" for="fuIcon">آیکن</label>
            <div>
                <label for="fuIcon" class="btn btn-default btn-file-nik">
                    <asp:FileUpload ID="fuIcon" runat="server" CssClass="hidden" ClientIDMode="Static" />
                    انتخاب آیکن
                </label>
            </div>
            <div class="image-icon-panel">
                <asp:Image ID="MenuImg" runat="server" CssClass="image-icon-nik" />
                <div class="file-remover-wrapper">
                    <div class="remove-icon-holder">
                        <i class="fa fa-trash" aria-hidden="true"></i>
                        <asp:Button ID="BtnRemoveImg" runat="server" Text="" CssClass="btn-file-remover" OnClick="BtnRemoveImg_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-sm-1">
        <div class="checkbox">
            <div class="label-null"></div>
            <label>
                <asp:CheckBox ID="chbEnbaled" Checked="true" runat="server" ClientIDMode="Static" />
                فعال؟
            </label>
        </div>
    </div>
    <div class="col-sm-2">
        <div class="checkbox">
            <div class="label-null"></div>
            <label>
                <asp:CheckBox ID="chbLogin" runat="server" ClientIDMode="Static" />
                نیاز به لاگین دارد؟
            </label>
        </div>
    </div>
</div>
<div class="row">
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
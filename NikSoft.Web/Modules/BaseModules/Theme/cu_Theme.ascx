<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cu_Theme.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Theme.cu_Theme" %>
<div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <label class="control-label" for="txtTitle">عنوان</label>
            <Nik:NikTextBox BoxTitle="Title" ID="txtTitle" runat="server" ClientIDMode="Static" CssClass="form-control reqfield" MaxLength="200" MinLength="3" data-validation="required length" data-validation-length="3-200"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-md-6" runat="server" id="colentitle">
        <div class="form-group">
            <label class="control-label" for="ddlParent">نام پوشه(انگلیسی )</label>
            <Nik:NikTextBox BoxTitle="Key Title" ID="txtEnTitle" runat="server" ClientIDMode="Static" CssClass="form-control reqfield" MaxLength="200" MinLength="3" data-validation="required length" data-validation-length="3-200"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-md-6" runat="server" id="fileupload">
        <div class="form-group">
            <label class="control-label" for="fuIcon">آپلود فایل</label>
            <div>
                <label for="fuFile" class="btn btn-default btn-file-nik" title="if have file">
                    <asp:FileUpload CssClass="hidden" ID="fuFile" runat="server" ClientIDMode="Static" />
                    فایل پوسته
                </label>
            </div>
        </div>
    </div>
    <div class="col-md-6">
        <div class="form-group">
            <label class="control-label" for="fuIcon">تصویر</label>
            <div>
                <label for="fuIcon" class="btn btn-default btn-file-nik">
                    <asp:FileUpload ID="fuIcon" runat="server" CssClass="hidden" ClientIDMode="Static" />
                    انتخاب تصویر
                </label>
            </div>
            <div class="image-icon-panel">
                <asp:Image ID="ThemImage" runat="server" CssClass="image-icon-nik" />
                <div class="file-remover-wrapper">
                    <div class="remove-icon-holder">
                        <i class="fa fa-trash" aria-hidden="true"></i>
                        <asp:Button ID="BtnRemoveImg" runat="server" Text="" CssClass="btn-file-remover" OnClick="BtnRemoveImg_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<hr />
<div class="row">
    <div class="col-md-12 text-center">
        <Nik:NikButton ID="btnSave" runat="server" Text="ذخیره" SettingValue="SaveButton" CssClass="btn btn-default btn-save-nik" />
    </div>
</div>
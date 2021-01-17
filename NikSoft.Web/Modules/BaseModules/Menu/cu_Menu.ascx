<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cu_Menu.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Menu.cu_Menu" %>
<div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <label class="control-label" for="txtTitle">عنوان</label>
            <Nik:NikTextBox BoxTitle="Title" ID="txtTitle" runat="server" ClientIDMode="Static" CssClass="form-control reqfield" MaxLength="200" MinLength="3" data-validation="required length" data-validation-length="3-200"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-md-6">
        <div class="form-group">
            <label class="control-label" for="ddlParent">منوی بالاتر</label>
            <asp:DropDownList ID="ddlParent" runat="server" CssClass="form-control select2" ClientIDMode="Static"></asp:DropDownList>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <label class="control-label" for="txtLink">لینک</label>
            <div class="input-group">
                <Nik:NikTextBox ID="txtLink" runat="server" ClientIDMode="Static" CssClass="form-control reqfield" MaxLength="500" data-validation="required length" data-validation-length="max500"></Nik:NikTextBox>
                <span class="input-group-btn">
                    <a href="#" class="btn btn-default slm" data-control-target="txtLink" data-control-hidden="hfLink"><span class="glyphicon glyphicon-th-list"></span></a>
                </span>
            </div>
            <asp:HiddenField ID="hfLink" runat="server" ClientIDMode="Static" />

        </div>
    </div>
    <div class="col-md-6">
        <div class="form-group">
            <label class="control-label" for="txtOrder">ترتیب</label>
            <Nik:NikTextBox BoxTitle="Oredr Number" ID="txtOrder" runat="server" ClientIDMode="Static" CssClass="form-control reqfield" data-validation="required number" data-validation-allowing="range[1;100]"></Nik:NikTextBox>
        </div>
    </div>
</div>
<div class="row">

    <div class="col-md-6">
        <div class="form-group">
            <label class="control-label" for="fuImgDashboard">آیکن</label>
            <asp:TextBox ID="txtAweSome" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
        </div>
    </div>

    <div class="col-md-6">
        <div class="form-group">
            <label class="control-label" for="fuIcon">تصویر</label>
            <div class="">
                <label for="fuIcon" class="btn btn-default btn-file-nik">
                    <asp:FileUpload ID="fuIcon" runat="server" CssClass="hidden" ClientIDMode="Static" />
                    انتخاب تصویر
                </label>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-6">
        <div class="checkbox">
            <label>
                <asp:CheckBox ID="chbEnbaled" Checked="true" runat="server" ClientIDMode="Static" />
                فعال؟
            </label>
        </div>
    </div>
    <div class="col-md-6">
        <div class="checkbox">
            <label>
                <asp:CheckBox ID="chbShowInDashboard" runat="server" ClientIDMode="Static" />
                نمایش در پنل؟
            </label>
        </div>
    </div>
</div>
<hr />
<div class="row">
    <div class="col-md-12 text-center">
        <Nik:NikButton ID="btnSave" runat="server" Text="ذخیره" SettingValue="SaveButton" CssClass="btn btn-default btn-save-nik" />
    </div>
</div>
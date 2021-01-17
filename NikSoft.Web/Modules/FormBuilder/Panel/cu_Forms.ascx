<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cu_Forms.ascx.cs" Inherits="NikSoft.FormBuilder.Web.Panel.cu_Forms" %>
<div class="row">
    <div class="col-sm-4">
        <div class="form-group">
            <label class="control-label" for="txtTitle">عنوان</label>
            <Nik:NikTextBox BoxTitle="Title" ID="txtTitle" runat="server" CssClass="form-control"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-4">
        <div class="form-group">
            <label class="control-label" for="ddlParent">فرم ارشد</label>
            <asp:DropDownList ID="ddlParent" runat="server" CssClass="form-control select2" ClientIDMode="Static"></asp:DropDownList>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-3">
        <div class="checkbox">
            <div class="label-null"></div>
            <label>
                <asp:CheckBox ID="chbEnbaled" Checked="true" runat="server" ClientIDMode="Static" />
                فعال؟
            </label>
        </div>
    </div>
    <div class="col-md-3">
        <div class="checkbox">
            <div class="label-null"></div>
            <label>
                <asp:CheckBox ID="chbLogin" runat="server" ClientIDMode="Static" />
                نیاز به لاگین دارد؟
            </label>
        </div>
    </div>
    <div class="col-md-3">
        <div class="checkbox">
            <div class="label-null"></div>
            <label>
                <asp:CheckBox ID="chbIpRecord" runat="server" ClientIDMode="Static" />
                IP کاربر دخیره شود؟
            </label>
        </div>
    </div>
    <div class="col-sm-12">
        <div class="form-group">
            <label class="control-label" for="TxtDesc">توضیحات</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtDesc" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-12">
        <div class="form-group">
            <label class="control-label" for="TxtMessage">متن پیام بعد از تکمیل فرم</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtMessage" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control"></Nik:NikTextBox>
        </div>
    </div>
</div>
<hr />
<div class="row">
    <div class="col-md-12 text-center">
        <Nik:NikButton ID="btnSave" runat="server" Text="ذخیره" SettingValue="SaveButton" CssClass="btn btn-default btn-save-nik" />
    </div>
</div>
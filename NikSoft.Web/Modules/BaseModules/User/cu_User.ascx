<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cu_User.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.User.cu_User" %>
<div class="callout callout-danger" runat="server" id="logoutmsg">
	<h4>هشدار</h4>
	<p>در هنگام ویرایش سیستم ممکن است شما از وضعیت ورود خارج نماید.</p>
</div>

<div class="row" runat="server" id="portalcont">
    <div class="col-md-6">
        <div class="form-group">
            <label for="ddlPortal">پرتال</label>
            <asp:DropDownList ID="ddlPortal" runat="server" CssClass="form-control reqfield" data-validation="dropdown"></asp:DropDownList>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <label for="txtFirstName">نام</label>
            <Nik:NikTextBox BoxTitle="first name" MinLength="2" MaxLength="200" ID="txtFirstName" runat="server" CssClass="form-control reqfield" data-validation="require length" data-validation-length="2-200" />
        </div>
    </div>
    <div class="col-md-6">
        <div class="form-group">
            <label for="txtLastName">نام خانوادگی</label>
            <Nik:NikTextBox BoxTitle="last name" MinLength="2" MaxLength="200" ID="txtLastName" runat="server" CssClass="form-control reqfield" data-validation="require length" data-validation-length="2-200" />
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <label for="txtUsername">نام کاربری</label>
            <div class="input-group">
                <Nik:NikTextBox ID="txtUsername" BoxTitle="username" CssClass="form-control reqfield" runat="server" MinLength="4" MaxLength="20" ClientIDMode="Static"></Nik:NikTextBox>
                <span class="input-group-btn">
                    <Nik:NikButton ID="btnCheckuser" runat="server" Text="معتبر" CssClass="btn btn-default btn-save" CausesValidation="False" OnClick="btnCheckuser_Click" />
                </span>
            </div>
        </div>
    </div>
    <div class="col-md-6">
        <div class="form-group">
            <label for="txtEmail">پست الکترونیک</label>
            <Nik:NikTextBox BoxTitle="email" ID="txtEmail" runat="server" CssClass="form-control reqfield" data-validation="email"></Nik:NikTextBox>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <label for="txtPassword">رمز عبور</label>
            <Nik:NikTextBox BoxTitle="password" ID="txtPassword" runat="server" CssClass="form-control reqfield" TextMode="Password"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-md-6">
        <div class="form-group">
            <label for="txtConfirmPassword">تکرار رمز عبور</label>
            <Nik:NikTextBox BoxTitle="confirme password" ID="txtConfirmPassword" runat="server" CssClass="form-control reqfield" data-validation="confirmation" data-validation-confirm="txtPassword" TextMode="Password"></Nik:NikTextBox>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <label for="dateExpire">تاریخ انقضاء</label>
            <Nik:NikTextBox ID="dateExpire" runat="server" CssClass="form-control"></Nik:NikTextBox>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-6">
        <div class="checkbox">
            <label>
                <asp:CheckBox ID="chbUnExpire" runat="server" />
                بدون تاریخ انقضاء
            </label>
        </div>
    </div>
    <div class="col-md-6">
        <div class="checkbox">
            <label>
                <asp:CheckBox ID="chbIsLock" runat="server" />
                عدم ورود؟
            </label>
        </div>
    </div>
</div>
<hr />
<div class="row">
    <div class="col-md-12 text-center">
        <Nik:NikButton ID="btnSubmit" CssClass="btn btn-default btn-save-nik" Text="ثبت نام" runat="server" SettingValue="SaveButton" />
    </div>
</div>


<script type="text/javascript">

    $.validate({
        modules: 'security'
    });

</script>
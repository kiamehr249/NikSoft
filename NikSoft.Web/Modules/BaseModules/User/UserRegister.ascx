<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserRegister.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.User.UserRegister" %>
<div class="page-header">
    <h3>ثبت نام در سامانه</h3>
</div>
<div class="row">
    <div class="col-sm-3">

    </div>
    <div class="col-sm-6">
        <div class="form-group">
            <label for="TxtFname">نام</label>
            <Nik:NikTextBox BoxTitle="نام" MinLength="2" MaxLength="200" ID="TxtFname" runat="server" CssClass="form-control reqfield" data-validation="require length" data-validation-length="2-200" />
        </div>
        <div class="form-group">
            <label for="TxtLname">نام خانوادگی</label>
            <Nik:NikTextBox BoxTitle="نام خانوادگی" MinLength="2" MaxLength="200" ID="TxtLname" runat="server" CssClass="form-control reqfield" data-validation="require length" data-validation-length="2-200" />
        </div>
        <div class="form-group">
            <label for="TxtUsername">نام کاربری</label>
            <Nik:NikTextBox BoxTitle="نام کاربری" MinLength="2" MaxLength="200" ID="TxtUsername" runat="server" CssClass="form-control reqfield" data-validation="require length" data-validation-length="2-200" />
            <% if (UnIsEmpty)
                { %>
            <div class="error">
                <p>نام کاربری را وارد کنید.</p>
            </div>
            <%} %>

             <% if (UnNotEngilsh)
                { %>
            <div class="error">
                <p>نام کاربری را با حروف انگلیسی و بدون فاصله وارد کنید.</p>
            </div>
            <%} %>

            <% if (ExistUser)
                { %>
            <div class="error">
                <p>این نام کاربری قبلا ثبت نام کرده است.</p>
            </div>
            <%} %>
        </div>
        <div class="form-group">
            <label for="TxtPass">رمز عبور</label>
            <Nik:NikTextBox TextMode="Password" BoxTitle="رمز عبور" MinLength="2" MaxLength="200" ID="TxtPass" runat="server" CssClass="form-control reqfield" data-validation="require length" data-validation-length="2-200" />
            <% if (PsEmpty)
                { %>
            <div class="error">
                <p>رمز عبور را وارد کنید.</p>
            </div>
            <%} %>
        </div>
         <div class="form-group">
            <label for="TxtPassConf">تکرار رمز عبور</label>
            <Nik:NikTextBox TextMode="Password" MinLength="2" MaxLength="200" ID="TxtPassConf" runat="server" CssClass="form-control reqfield" data-validation="require length" data-validation-length="2-200" />
            <% if (PsEmpty)
                { %>
            <div class="error">
                <p>تکرار رمز عبور را وارد کنید.</p>
            </div>
            <%} %>

            <% if (PsNotMatch)
                { %>
            <div class="error">
                <p>تکرار رمز عبور را با رمز عبور وارد شده تفاوت دارد.</p>
            </div>
            <%} %>
        </div>
        <hr />
        <div class="form-group text-center">
            <asp:Button ID="BtnSave" runat="server" Text="ثبت نام" CssClass="btn btn-success" OnClick="BtnSave_Click" />
        </div>
    </div>
    <div class="col-sm-3">

    </div>
</div>


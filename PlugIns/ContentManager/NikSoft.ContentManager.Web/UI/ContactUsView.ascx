<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactUsView.ascx.cs" Inherits="NikSoft.ContentManager.Web.UI.ContactUsView" %>
<asp:HiddenField ID="HfSuccess" runat="server" Value="پیام شما ارسال شد" />
<div class="page-header">
    <h4>تماس با ما</h4>
</div>
<div class="row">
    <div class="col-sm-6">
        <div class="form-group">
            <label for="TxtFname">نام</label>
            <Nik:NikTextBox ID="TxtFname" CssClass="form-control reqfield" runat="server" EmptyTextIsValid="false" EmptyMessage="نام نمی تواند خالی باشد"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-6">
        <div class="form-group">
            <label for="txtFName">نام خانوادگی</label>
            <Nik:NikTextBox ID="TxtLname" CssClass="form-control reqfield" runat="server"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-6">
        <div class="form-group">
            <label for="TxtPhone">شماره تماس</label>
            <Nik:NikTextBox ID="TxtPhone" CssClass="form-control reqfield" runat="server" MinLength="8" MaxLength="15" MinLengthMessage="شماره تماس باید بیشتر از 7 کارکتر باشد" MaxLengthMessage="شماره تلفن باید کمتر از 16 کارکتر باشد" PublicMessage="شماره تماس را به عدد وارد کنید"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-6">
        <div class="form-group">
            <label for="TxtEmail">آدرس ایمیل</label>
            <Nik:NikTextBox ID="TxtEmail" CssClass="form-control reqfield" runat="server" MinLength="5" MaxLength="50" MinLengthMessage="ایمیل باید بیشتر از 5 کارکتر باشد" MaxLengthMessage="ایمیل باید کمتر از 20 کارکتر باشد" PublicMessage="ایمیل وارد شه صحیح نیست"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-6">
        <div class="form-group">
            <label for="TxtCompany">شرکت / سازمان</label>
            <Nik:NikTextBox ID="TxtCompany" CssClass="form-control reqfield" runat="server"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-12">
        <div class="form-group">
            <label for="TxtMessage">موضوع</label>
            <Nik:NikTextBox ID="TxtSubject" CssClass="form-control reqfield" runat="server"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-12">
        <div class="form-group">
            <label for="TxtMessage">متن پیام</label>
            <Nik:NikTextBox ID="TxtMessage" CssClass="form-control reqfield" runat="server" TextMode="MultiLine" Rows="4" MinLength="5" MaxLength="2000" MinLengthMessage="متن پیام باید بیشتر از 5 کارکتر باشد" MaxLengthMessage="متن پیام باید کمتر از 2000 کارکتر باشد" EmptyTextIsValid="false" EmptyMessage="متن نمی تواند خالی باشد"></Nik:NikTextBox>
        </div>
    </div>
</div>
<hr />
<div class="form-group text-center">
    <asp:Button ID="BtnSend" runat="server" CssClass="btn btn-success" Text="ارسال" OnClick="BtnSend_Click" />
</div>
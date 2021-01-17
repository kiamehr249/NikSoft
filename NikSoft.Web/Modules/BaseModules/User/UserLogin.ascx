<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserLogin.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.User.UserLogin" %>
<div class="login-page">
    <div class="row">
        <div class="col-sm-6">
            <div class="row">
                <div class="col-sm-12">
                    <div class="page-header">
                        <h3>ورود</h3>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-user"></span></span>
                            <asp:TextBox ID="txtUserName" runat="server" class="form-control popoverbutton" placeholder="نام کاربری" data-content="نام کاربری خود را وارد کنید" data-placement="left" data-trigger="focus"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <div class="input-group">
                            <span class="input-group-addon tooltiper"><span class="glyphicon glyphicon-lock"></span></span>
                            <asp:TextBox Font-Names="tahoma" ID="txtPassword" runat="server" MaxLength="20" CssClass="form-control popoverbutton" TextMode="Password" autocomplete="off" placeholder="رمز عبور" data-content="رمز عبور را وارد کنید" data-placement="left" data-trigger="focus"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <div class="input-group">
                            <span class="input-group-addon">
                                <asp:CheckBox ID="chbRememberMe" runat="server" ClientIDMode="Static" />
                            </span>
                            <span class="input-group-addon">مرا به یاد داشته باش؟</span>
                        </div>
                    </div>
                </div>
                <div class="col-sm-12">
                    <asp:Button ID="btnlogin" runat="server" OnClick="btnlogin_Click" CssClass="btn btn-default btn-sm" Text="ورود" />
                </div>
                <div class="col-sm-12">
                    <br />
                    <div class="callout callout-danger" id="msg" runat="server" visible="false">
                        <h4>
                            <asp:Label runat="server">نام کاربری یا رمز عبور اشتباه می باشد.</asp:Label></h4>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-6">
            <div class="page-header">
                <h3>ثبت نام</h3>
            </div>
            <div class="register-text">
                <p>اگر تاکنون در سامانه عضو نشده اید می تواند با کلیک روی ثبت نام این کار را انجام دهید.</p>
            </div>
            <div class="register-btns">
                <a href="<%= "/" + Level + "/UserRegister" %>" class="btn btn-success">ثبت نام</a>
            </div>
        </div>
    </div>
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PublicUserProfile.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.User.PublicUserProfile" %>
<div class="page-header">
    <h3>پروفایل کاربری</h3>
</div>
<div class="row">
    <div class="col-sm-4">
        <div class="user-details">
            <span>نام: </span><span><%= myUser.FirstName %></span>
        </div>
    </div>
    <div class="col-sm-4">
        <div class="user-details">
            <span>نام خانوادگی: </span><span><%= myUser.LastName %></span>
        </div>
    </div>
    <div class="col-sm-4">
        <div class="user-details">
            <span>پست الکترونیک: </span><span><%= myUser.Email %></span>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-sm-4">
        <div class="user-details">
            <span>نام کاربری: </span><span><%= myUser.UserName %></span>
        </div>
    </div>
    <div class="col-sm-4">
        <div class="user-details">
            <span>آخرین ورود: </span><span><%= myUser.LastLogin %></span>
        </div>
    </div>
    <div class="col-sm-4">
        <div class="user-details">
            <span>تاریخ اعتبار: </span><span><%= myUser.PassExpireDate %></span>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-sm-4">
        <div class="form-group">
            <a href="<%= "/" + Level + "/LogOut" %>" class="btn btn-danger">خروج</a>
        </div>
    </div>
    <div class="col-sm-4">
        <div class="user-details">
            <span>کد پستی: </span><span><%= myProfile != null ? myProfile.ZipCode : " خالی " %></span>
        </div>
    </div>
   <div class="col-sm-12">
        <div class="user-details">
            <span>آدرس: </span><span><%= myProfile != null ? myProfile.Address : " خالی " %></span>
        </div>
    </div>
</div>

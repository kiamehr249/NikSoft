<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="W_UserDetailMenu.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Widgets.W_UserDetailMenu" %>
<a href="#" class="dropdown-toggle pr-0" data-toggle="dropdown">
    <span class="user-online-status"><%= myUser != null ? myUser.FirstName + " " + myUser.LastName : "" %></span>
    <asp:Image ID="ImgUser" runat="server" ImageUrl="~/images/user-none-image.png" CssClass="user-auth-img img-circle" alt="user_auth" />
</a>
<ul class="dropdown-menu user-auth-dropdown" data-dropdown-in="flipInX" data-dropdown-out="flipOutX">
    <li>
        <a href="/panel/UserProfile"><i class="fa fa-user" aria-hidden="true"></i><span>پروفایل</span></a>
    </li>
    <li class="divider"></li>
    <li>
        <a href="/panel/LogOut"><i class="fa fa-power-off" aria-hidden="true"></i><span>خروج</span></a>
    </li>
</ul>
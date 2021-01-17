<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserPerCategories.ascx.cs" Inherits="NikSoft.ContentManager.Web.Panel.UserPerCategories" %>
<div class="page-header">
    <h4>دسته های محتوایی</h4>
    <p><%= MyUser.FirstName + " " + MyUser.LastName %></p>
</div>
<div class="row">
    <asp:Repeater ID="RepCategories" runat="server" ItemType="NikSoft.ContentManager.Service.ContentCategory">
        <ItemTemplate>
            <div class="col-sm-3">
                <a href="<%# "/panel/rd_UserPublicContent/" + Item.ID  %>" class="full-btn-box"><%# Item.Title %></a>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
<div class="row">
    <div class="col-md-12">
        <asp:PlaceHolder ID="pl1" runat="server" EnableViewState="False" ViewStateMode="Disabled"></asp:PlaceHolder>
    </div>
</div>
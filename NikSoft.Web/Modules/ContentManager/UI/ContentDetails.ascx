<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContentDetails.ascx.cs" Inherits="NikSoft.ContentManager.Web.UI.ContentDetails" %>
<div class="content-wrap">
    <div class="title-content">
        <h3><%= myConetnt.Title  %></h3>
    </div>
    <div class="img">
        <img src="<%= "/" + myConetnt.ImgUrl %>" />
    </div>
    <div class="mini-desc">
        <%= myConetnt.MiniDesc  %>
    </div>
    <div class="date-time">
         <%= NikSoft.Utilities.Utilities.ShowPersianDate(myConetnt.CreateDate.Value, 4)  %>
    </div>
    <div class="full-desc">
        <%= myConetnt.FullText  %>
    </div>
    <div class="slider owl-carousel">
        <asp:Repeater ID="RepAlbums" runat="server" ItemType="NikSoft.ContentManager.Service.ContentFile">
            <ItemTemplate>
                <div class="file-item">
                    <img src="<%# "/" + Item.FileUrl  %>" />
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <div class="last-contents">
        <asp:Repeater ID="RepLast" runat="server" ItemType="NikSoft.ContentManager.Service.PublicContent">
            <ItemTemplate>
                <li class="clearfix">
                    <a href="<%# "/" + Level + "/NewsDetails/" + Item.ID %>">
                        <img src="<%# "/" + Item.ImgUrl %>" alt="" class="widget-posts-img">
                    </a>
                    <div class="widget-posts-descr">
                        <a href="<%# "/" + Level + "/NewsDetails/" + Item.ID %>" title=""><%# Item.Title %></a>
                        <div><%# Item.CreateDate %></div>
                    </div>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>

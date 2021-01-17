<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContentGroupView.ascx.cs" Inherits="NikSoft.ContentManager.Web.UI.ContentGroupView" %>
<div class="row">
    <asp:Repeater ID="RepGroup" runat="server" ItemType="NikSoft.ContentManager.Service.ContentGroup">
        <ItemTemplate>
            <div class="col-sm-3">
                <div class="content-title">
                    <a href="<%# "/" + Level + "/ContentCategory/" + Item.ID  %>"><%# Item.Title %></a>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
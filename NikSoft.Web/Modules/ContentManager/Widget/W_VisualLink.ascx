<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="W_VisualLink.ascx.cs" Inherits="NikSoft.ContentManager.Web.Widget.W_VisualLink" %>
<div class="row">
    <asp:Repeater ID="RepContents" runat="server" ItemType="NikSoft.ContentManager.Service.VisualLinkItem">
        <ItemTemplate>
            <div class="col-sm-3">
                <div class="content-img">
                    <img src="<%# "/" + Item.Img1 %>" class="img-responsive" />
                </div>
                <div class="content-title">
                    <h4><%# Item.Title %></h4>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Repeater ID="RepTow" runat="server" ItemType="NikSoft.ContentManager.Service.VisualLinkItem">
        <ItemTemplate>
        </ItemTemplate>
    </asp:Repeater>
</div>
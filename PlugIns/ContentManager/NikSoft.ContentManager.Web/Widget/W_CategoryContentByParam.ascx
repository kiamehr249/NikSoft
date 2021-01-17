<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="W_CategoryContentByParam.ascx.cs" Inherits="NikSoft.ContentManager.Web.Widget.W_CategoryContentByParam" %>

<div class="row">
    <asp:Repeater ID="RepParam" runat="server" ItemType="NikSoft.ContentManager.Service.PublicContent">
        <ItemTemplate>
            <div class="col-sm-12">
                <div class="content-img">
                    <img src="<%# "/" + Item.ImgUrl %>" class="img-responsive" />
                </div>
                <div class="content-title">
                    <h4><%# NikSoft.Utilities.Utilities.GetTextLimited(Item.Title, TitleLength) %></h4>
                </div>
                <div class="content-lead">
                    <%# NikSoft.Utilities.Utilities.GetTextLimited(Item.MiniDesc, LeadLength) %>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
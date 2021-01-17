<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContentInCategory.ascx.cs" Inherits="NikSoft.ContentManager.Web.UI.ContentInCategory" %>
<div class="search-bar">
    <asp:TextBox ID="TxtSearch" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
    <asp:Button ID="BtnSearch" runat="server" CssClass="btn btn-default" Text="جستجو" OnClick="BtnSearch_Click" />
</div>
<div class="row">
    <asp:Repeater ID="RepContents" runat="server" ItemType="NikSoft.ContentManager.Service.PublicContent">
        <ItemTemplate>
            <div class="col-sm-3">
                <div class="img">
                    <a href="<%# "/" + Level + "/ContentDetaile/" + Item.ID  %>">
                        <img src="<%# "/" + Item.ImgUrl %>" class="img-responsive" />
                    </a>
                </div>
                <div class="content-title">
                    <a href="<%# "/" + Level + "/ContentDetaile/" + Item.ID  %>"><%# Item.Title %></a>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>

<div class="paging">
    <ul>
        <asp:Repeater ID="RepPaging" runat="server" ItemType="NikSoft.ContentManager.Service.Pagination">
            <ItemTemplate>
                <li>
                    <a><%# Item.Id + 1 %></a>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</div>

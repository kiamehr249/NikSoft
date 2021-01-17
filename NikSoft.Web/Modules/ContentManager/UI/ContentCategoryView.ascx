<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContentCategoryView.ascx.cs" Inherits="NikSoft.ContentManager.Web.UI.ContentCategoryView" %>
<div class="search-bar">
    <asp:TextBox ID="TxtSearch" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
    <asp:Button ID="BtnSearch" runat="server" CssClass="btn btn-default" Text="جستجو" OnClick="BtnSearch_Click" />
</div>
<div class="row">
    <asp:Repeater ID="RepGroup" runat="server" ItemType="NikSoft.ContentManager.Service.ContentCategory">
        <ItemTemplate>
            <div class="col-sm-12">
                <div class="row">
                    <div class="col-sm-3">
                        <div class="img">
                            <img src="<%# Item.ImgUrl %>" class="img-responsive" />
                        </div>
                        <div class="content-title">
                            <a href="<%# "/" + Level + "/Contents/" + Item.ID  %>"><%# Item.Title %></a>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <asp:Repeater DataSource="<%# Item.Childs.ToList() %>" ID="RepChilds" runat="server" ItemType="NikSoft.ContentManager.Service.ContentCategory">
                        <ItemTemplate>
                            <div class="col-sm-3">
                                <a href="<%# "/" + Level + "/Contents/" + Item.ID  %>"><%# Item.Title %></a>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="W_PanelMenu.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Widgets.W_PanelMenu" %>
<ul class="nav navbar-nav side-nav">
    <asp:Repeater ID="RepMains" ItemType="NikSoft.Web.Modules.BaseModules.Widgets.MenuModel" runat="server">
        <ItemTemplate>
            <li>
                <a class="parent-menu" href='#dashboard_<%# Item.MenuItem.ID %>' data-toggle="collapse">
                    <div class="pull-right"><%# Item.MenuItem.AweSomeFontClass %><span class="right-nav-text"><%# Item.MenuItem.Title %></span></div>
                    <div class="pull-left caret-menu"><i class="fa fa-caret-down" aria-hidden="true" visible='<%# Item.ItemChilds.Count < 1 ? "false" : "true" %>'></i></div>
                    <div class="clearfix"></div>
                </a>
                <ul id='dashboard_<%# Item.MenuItem.ID %>' class="sub-menu-hoder collapse close-m" visible='<%# Item.ItemChilds.Count < 1 ? "false" : "true" %>'>
                    <asp:Repeater ID="RepChilds" ItemType="NikSoft.Web.Modules.BaseModules.Widgets.MenuModel" runat="server">
                        <ItemTemplate>
                            <li>
                                <a class="" href='<%# GetLink(Item.MenuItem.Link) %>'><%# Item.MenuItem.Title %></a>
                                <asp:HiddenField ID="HidSubitemID" Value='<%# Item.MenuItem.ID %>' runat="server" />
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:HiddenField ID="HidItemID" Value='<%# Item.MenuItem.ID %>' runat="server" />
                </ul>
            </li>
        </ItemTemplate>
    </asp:Repeater>
</ul>
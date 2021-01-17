<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="W_ClientMenu.ascx.cs" Inherits="NikSoft.ContentManager.Web.Widget.W_ClientMenu" %>
<nav class="menu-wrapper">
    <ul class="nik-menu">
        <asp:Repeater ID="RepMains" runat="server" ItemType="NikSoft.ContentManager.Service.GeneralMenuItem">
            <ItemTemplate>
                <li class="main-menu-item">
                    <a href='<%# Item.Link == "" ? "javascript:void();" : "/" + Level + Item.Link %>' class="menu-title">
                        <%# Item.Title %>
                    </a>
                    <ul class="sub-menu-list" runat="server" visible='<%# Item.Childs.Count > 0 ? true : false %>'>
                        <asp:Repeater DataSource="<%# Item.Childs.OrderBy(x => x.Ordering).ToList() %>" ID="RepChild" runat="server" ItemType="NikSoft.ContentManager.Service.GeneralMenuItem">
                            <ItemTemplate>
                                <li class="sub-menu-item">
                                    <a href='<%# Item.Link == "" ? "javascript:void();" : "/" + Level + Item.Link %>' class="sub-menu-title">
                                        <%# Item.Title %>
                                    </a>
                                    <ul class="last-menu-list" runat="server" visible='<%# Item.Childs.Count > 0 ? true : false %>'>
                                        <asp:Repeater DataSource="<%# Item.Childs.OrderBy(x => x.Ordering).ToList() %>" ID="RepSecond" runat="server" ItemType="NikSoft.ContentManager.Service.GeneralMenuItem">
                                            <ItemTemplate>
                                                <li class="last-menu-item">
                                                    <a href='<%# Item.Link == "" ? "javascript:void();" : "/" + Level + Item.Link %>' class="last-menu-title">
                                                        <%# Item.Title %>
                                                    </a>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</nav>
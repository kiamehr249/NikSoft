<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="W_NikPanelMenu.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Widgets.W_NikPanelMenu" %>
<nav class="navbar navbar-default">
    <div class="navbar-header">
        <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1" aria-expanded="false">
            <span class="sr-only">Toggle navigation</span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
        </button>
        <a class="navbar-brand" href="/panel/page/default">Nikmehr</a>
    </div>
    <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
        <ul class="nav navbar-nav">
            <asp:Literal ID="ltrMenu" runat="server"></asp:Literal>
            <li class="logout-panel">
                <a class="btn btn-log" href="/panel/logout">
                    <i class="fa fa-sign-out" aria-hidden="true"></i>
                    <span>Sign Out</span>            
                </a>
            </li>
        </ul>
    </div>
</nav>
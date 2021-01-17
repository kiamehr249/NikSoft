<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PanelWidgetContainer.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Template.PanelWidgetContainer" %>
<div class="portlet">
    <div class="portlet-header">
        <asp:UpdatePanel ID="WidgetHeaderUpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-12">
                        <div class="dropdown">
                            <button id="dsett" type="button" data-toggle="dropdown" class="btn" aria-haspopup="true" aria-expanded="false">
								<asp:Literal ID="ltTitle" runat="server"></asp:Literal>
                                <span class="caret"></span>
                            </button>
                            <ul id="menu1" class="dropdown-menu" aria-labelledby="dsett">
                                <li>
                                    <asp:LinkButton ID="ss" ForeColor="Black" runat="Server" Text="" OnClick="ShowSetting_Click"><span class="glyphicon glyphicon-cog"></span><asp:Literal ID="lt_panel" runat="server"></asp:Literal></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lb1" runat="Server" OnClick="ShowTitle_Click"><span class="glyphicon glyphicon-ok"></span><asp:Literal ID="lt_title" runat="server"></asp:Literal> </asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lb_delWid" runat="Server" OnClick="lb_delWid_Click"><span style="color:red;" class="glyphicon glyphicon-ban-circle"></span><asp:Literal ID="lt_Pub" runat="server"></asp:Literal> </asp:LinkButton>
                                </li>
                                <li>
                                    <asp:HyperLink runat="Server" ID="widgetEdit" Target="_blank"><span style="color:red;" class="glyphicon glyphicon-pencil"></span>Edite Widget</asp:HyperLink>
                                </li>
                                <li role="separator" class="divider"></li>
                                <li>
                                    <a href="#" data-wdid='<%= WidgetInstance.Widget.ID %>' class="widgetremove"><span style="color:red;" class="glyphicon glyphicon-trash"></span>Remove</a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:LinkButton CssClass="widgettitle" ID="WidgetTitle" runat="Server" Text="Widget Title" OnClick="WidgetTitle_Click" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:TextBox ID="WidgetTitleTextBox" runat="Server" Visible="False" CssClass="form-control" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Literal ID="lw1" Visible="false" runat="server" Text="<br />لینک پیوند" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:TextBox ID="widget_tlink" runat="Server" CssClass="form-control" Visible="False" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="SaveWidgetTitle" runat="Server" CssClass="btn btn-default btn-save-nik" OnClick="SaveWidgetTitle_Click" Visible="False" Text="save" />
                    </div>
                </div>
                <asp:PlaceHolder ID="plcWid" runat="server"></asp:PlaceHolder>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="10" AssociatedUpdatePanelID="WidgetHeaderUpdatePanel">
        <ProgressTemplate>
            <div class="uprogress">
                waiting to load
                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/loading.gif" style="max-width: 50px;" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="WidgetBodyUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="portlet-content">
                <asp:PlaceHolder ID="WidgetBodyPanel" runat="Server"></asp:PlaceHolder>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="5" AssociatedUpdatePanelID="WidgetBodyUpdatePanel">
        <ProgressTemplate>
            <div class="uprogress">
                waiting to load
				<asp:Image ID="Image2" runat="server" ImageUrl="~/images/loading.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>
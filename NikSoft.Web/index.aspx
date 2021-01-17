<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="index.aspx.cs" Inherits="NikSoft.Web.index" %>

<%@ Register Src="~/Modules/BaseModules/EngineBase/TemplateEngine.ascx" TagName="UCThem" TagPrefix="Nik" %>
<%@ Register Src="~/Modules/BaseModules/Notification/NotificationBar.ascx" TagPrefix="Nik" TagName="NotificationBar" %>

<!DOCTYPE html>

<html id="phtml" runat="server" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="h1">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title></title>
</head>
<body>
    <asp:Literal ID="litScriptTop" runat="server"></asp:Literal>
    <form id="mainForm" runat="server">
        <div class="notification-load">
            <Nik:NotificationBar runat="server" ID="nb" />
        </div>
        <div class="body-container">
            <Nik:UCThem ID="UCTemLoad" runat="server" />
        </div>
    </form>
    <asp:Literal ID="litScriptBottom" runat="server"></asp:Literal>
</body>
</html>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NotificationBar.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Notification.NotificationBar" %>
<link href="../../contents/noty/noty.css" rel="stylesheet" />
<script src="../../contents/noty/noty.min.js" type="text/javascript"></script>

<script type="text/javascript">
    $(document).ready(function () {

        var notiftext = '<%=finalMessage %>';
        if ($.trim(notiftext) != '') {
            var entery = notiftext;
            new Noty({
	            text: entery,
	            type: '<%= type %>',
                layout: 'topRight',
                timeout: parseInt('<%= timeOut %>')
	        }).show();
		}
	});
</script>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditNikWidget.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.WidgetEdit.EditNikWidget" %>
<link rel="stylesheet" href='<%= ResolveUrl("~/contents/codemirror5/lib/codemirror.css") %>'>
<link rel="stylesheet" href='<%= ResolveUrl("~/contents/codemirror5/theme/material.css") %>'>
<script src='<%= ResolveUrl("~/contents/codemirror5/lib/codemirror.js") %>'></script>
<script src='<%= ResolveUrl("~/contents/codemirror5/addon/fold/xml-fold.js") %>'></script>
<script src='<%= ResolveUrl("~/contents/codemirror5/addon/edit/matchtags.js") %>'></script>
<script src='<%= ResolveUrl("~/contents/codemirror5/mode/xml/xml.js") %>'></script>
<script src='<%= ResolveUrl("~/contents/codemirror5/mode/css/css.js") %>'></script>
<script src='<%= ResolveUrl("~/contents/codemirror5/mode/htmlmixed/htmlmixed.js") %>'></script>
<script src='<%= ResolveUrl("~/contents/codemirror5/mode/htmlembedded/htmlembedded.js") %>'></script>

<div class="row">
	<div class="col-md-12">
		<asp:TextBox ID="txtCode" runat="server" TextMode="MultiLine"></asp:TextBox>
	</div>
</div>
<hr />
<div class="row">
	<div class="col-md-12 text-center">
		<asp:Button ID="btnSave" runat="server" CssClass="btn btn-default btn-save-nik" Text="Save" OnClick="btnSave_Click" />
		<a href='<%= ResolveUrl("~/dashboard/allmodulesview/" + ModuleParameters) %>' class="btn btn-default btn-back">Back To List</a>
	</div>
</div>

<script>
	var editor = CodeMirror.fromTextArea(document.getElementById('<%= txtCode.ClientID %>'), {
		lineNumbers: true,
		extraKeys: { "Ctrl-Space": "autocomplete" },
		styleActiveLine: true,
		matchBrackets: true,
		mode: '<%= Mode %>',
		indentUnit: 4,
		indentWithTabs: true,
		theme: 'material',
		matchTags: { bothTags: true },
		extraKeys: { "Ctrl-J": "toMatchingTag" }
	});
</script>
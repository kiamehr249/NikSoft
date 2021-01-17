<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cu_UserTypeGroup.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.User.cu_UserTypeGroup" %>
<div class="row">
	<div class="col-md-6">
		<div class="form-group">
			<label for="ddlUserType">User Group</label>
			<asp:DropDownList ID="ddlUserType" runat="server" CssClass="form-control reqfield" ClientIDMode="Static" data-validation="dropdown"></asp:DropDownList>
		</div>
	</div>
	<div class="col-md-6">
		<div class="form-group">
			<label for="txtTitle">Title</label>
			<Nik:NikTextBox BoxTitle="Title" ID="txtTitle" runat="server" ClientIDMode="Static" CssClass="form-control reqfield" data-validation="required length" data-validation-length="3-200" MaxLength="200" MinLength="3" />
		</div>
	</div>
</div>
<hr />
<div class="row">
	<div class="col-md-12 text-center">
		<Nik:NikButton ID="btnSubmit" CssClass="btn btn-default btn-save-nik" Text="save" runat="server" ClientIDMode="Static" SettingValue="SaveButton" />
	</div>
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cu_UserType.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.User.cu_UserType" %>
<div class="row">
	<div class="col-md-6">
		<div class="form-group">
			<label for="txtTitle">Title</label>
			<Nik:NikTextBox ID="txtTitle" data-validation="required length" data-validation-length="3-200" BoxTitle="Title" runat="server" CssClass="form-control reqfield" MinLength="3" MaxLength="200" ClientIDMode="Static"></Nik:NikTextBox>
		</div>
	</div>
</div>
<hr />
<div class="row">
	<div class="col-md-12 text-center">
		<Nik:NikButton ID="btnSubmit" CssClass="btn btn-default btn-save-nik" Text="save" runat="server" ClientIDMode="Static" SettingValue="SaveButton" />
	</div>
</div>
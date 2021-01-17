﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserGroupManagement.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.User.UserGroupManagement" %>
<div class="row">
    <div class="col-sm-3 col-xs-4">
        <h4 class="">نام: <%= ThisName %></h4>
    </div>
    <div class="col-sm-3 col-xs-4">
        <h4 class="">نام خانوادگی: <%= ThisLastname %></h4>
    </div>
    <div class="col-sm-3 col-xs-4">
        <h4 class="">نام کاربری: <%= ThisUsername %></h4>
    </div>
</div>
<div class="row">
	<div class="col-md-6">
		<div class="form-group">
			<label for="ddlUserType">دسته کاربری</label>
			<asp:DropDownList ID="ddlUserType" runat="server" CssClass="form-control reqfield" ClientIDMode="Static"></asp:DropDownList>
		</div>
	</div>
    <div class="col-sm-16">
        <div class="label-null"></div>
        <Nik:NikButton ID="btnSave" runat="server" Text="ذخیره" CssClass="btn btn-default btn-save-nik" ClientIDMode="Static" SettingValue="SaveButton" />
    </div>
</div>
<hr />
<div class="row">
	<div class="col-md-6">
		<div class="input-group">
			<label class="input-group-addon" for="txtCount">تعداد:</label>
			<asp:TextBox ClientIDMode="Static" ID="txtCount" runat="server" ForeColor="Red" Enabled="false" CssClass="countlabel form-control"></asp:TextBox>
		</div>
	</div>
</div>
<div class="row">
	<div class="col-md-12">
		<Nik:NikGridView ID="GV1" runat="server">
			<EmptyDataTemplate>
				<div class="noinfo">هیچ آیتمی وجود ندارد</div>
			</EmptyDataTemplate>
			<Columns>
				<asp:TemplateField HeaderText="ردیف" ItemStyle-Width="2%">
					<ItemTemplate>
						<Nik:ListCounter ID="ListCounter2" runat="server" IndexFormat="{0}."></Nik:ListCounter>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="" ItemStyle-Width="2%">
					<ItemTemplate>
						<input type="checkbox" name='ch1' value='<%# Eval("ID") %>'>
					</ItemTemplate>
					<HeaderTemplate>
						<input type="checkbox" id="chan" title="همه">
					</HeaderTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="دسته کاربری">
					<ItemTemplate>
						<%# Eval("Title") %>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</Nik:NikGridView>
	</div>
</div>
<div class="row">
	<div class="col-md-12">
		<asp:PlaceHolder ID="pl1" runat="server" EnableViewState="False" ViewStateMode="Disabled"></asp:PlaceHolder>
	</div>
</div>
<div class="row">
	<div class="col-md-12">
		<Nik:NikButton ID="bd" runat="server" CssClass="btn btn-default btn-remove" Text="حذف" OnClick="bd_Click" ClientIDMode="Static" />
		<Nik:NikHyperLink runat="server" NavigateUrl="~/panel/rd_user/view" CssClass="btn btn-default btn-back" SettingValue="BackButton">بازگشت</Nik:NikHyperLink>
		<Nik:NikHyperLink ID="hlut" runat="server" CssClass="btn btn-default btn-edite" SettingValue="BackButton">گروه کاربری</Nik:NikHyperLink>
	</div>
</div>
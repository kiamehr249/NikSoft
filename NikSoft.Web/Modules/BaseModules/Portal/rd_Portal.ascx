﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rd_Portal.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Portal.rd_Portal" %>
<Nik:ModalSearch runat="server">
<div class="row">
	<div class="col-md-6">
		<div class="form-group">
			<label for="txtTitle">عنوان</label>
			<asp:TextBox ID="txtTitle" CssClass="form-control" runat="server" ClientIDMode="Static" />
		</div>
	</div>
</div>
<div class="row">
	<div class="col-md-6">
		<div class="btn-group">
            <Nik:NikLinkButton ID="breset" runat="server" Text="همه" OnClick="breset_Click" CssClass="btn btn-default btn-back btn-sm" SettingValue="ResetButton"></Nik:NikLinkButton>
			<Nik:NikButton ID="btnSearch" runat="server" Text="جستجو" OnClick="btnSearch_Click" CssClass="btn btn-default btn-search btn-sm" ClientIDMode="Static" SettingValue="SearchButton" />	
		</div>
	</div>
	<div class="col-md-6">
		<div class="input-group">
			<label class="input-group-addon" for="txtCount">تعداد:</label>
			<asp:TextBox ClientIDMode="Static" ID="txtCount" runat="server" ForeColor="Red" Enabled="false" CssClass="countlabel form-control"></asp:TextBox>
		</div>
	</div>
</div>
</Nik:ModalSearch>
<div class="row">
	<div class="col-md-12 table-responsive">
		<Nik:NikGridView ID="GV1" runat="server" ItemType="NikSoft.NikModel.Portal">
			<EmptyDataTemplate>
				<div class="noinfo">هیچ آیتمی وجود ندارد.</div>
			</EmptyDataTemplate>
			<Columns>
				<asp:TemplateField HeaderText="ردیف" ItemStyle-Width="2%">
					<ItemTemplate>
						<Nik:ListCounter ID="ListCounter2" runat="server" IndexFormat="{0}."></Nik:ListCounter>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="" ItemStyle-Width="2%">
					<ItemTemplate>
						<input type="checkbox" name='ch1' value='<%# Item.ID %>'>
					</ItemTemplate>
					<HeaderTemplate>
						<input type="checkbox" id="chan" title="همه">
					</HeaderTemplate>
				</asp:TemplateField>
				<asp:BoundField HeaderText="عنوان" DataField="Title" />
				<asp:BoundField HeaderText="Alias" DataField="Alias" />
				<asp:BoundField HeaderText="ماکسیمم حجم" DataField="MaxVol" />
				<asp:TemplateField HeaderText="دامنه ها">
					<ItemTemplate>
						<asp:HyperLink runat="server" Text="" CssClass="btn btn-default btn-sm btn-info-nik" NavigateUrl='<%# "~/"+ Level +"/PortalDomain/" + Item.ID  %>'>
							دامنه ها <span class="badge"><%# Item.PortalAddresses.Count %></span>
						</asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</Nik:NikGridView>
	</div>
	<div class="col-md-12">
		<asp:PlaceHolder ID="pl1" runat="server" EnableViewState="False" ViewStateMode="Disabled"></asp:PlaceHolder>
	</div>
</div>
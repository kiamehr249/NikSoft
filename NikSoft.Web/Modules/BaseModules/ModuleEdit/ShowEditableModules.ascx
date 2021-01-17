<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShowEditableModules.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.ModuleEdit.ShowEditableModules" %>
<div class="row">
	<div class="col-md-5">
		<div class="form-group">
			<label class="control-label">دسته ماژول</label>
			<asp:DropDownList ID="ddlModules" runat="server" CssClass="form-control" AutoPostBack="True" Width="100%" OnSelectedIndexChanged="ddlModules_SelectedIndexChanged"></asp:DropDownList>
		</div>
	</div>
	<div class="col-md-12">
		<Nik:NikGridView ID="GV1" runat="server" ItemType="NikSoft.NikModel.NikModule">
			<EmptyDataTemplate>
				<div class="noinfo">Nothing to show</div>
			</EmptyDataTemplate>
			<Columns>
				<asp:TemplateField HeaderText="ردیف" ItemStyle-Width="2%">
					<ItemTemplate>
						<Nik:ListCounter ID="lc2" runat="server" IndexFormat="{0}."></Nik:ListCounter>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="عنوان">
					<ItemTemplate>
						<%# 
							Item.Title + "<br />"+ Item.ModuleKey
						%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:HyperLink ID="h1" runat="server" NavigateUrl='<%# ResolveUrl("~/panel/EditModule/" + Item.ModuleKey) %>' CssClass="btn btn-default btn-edite btn-sm">ویرایش</asp:HyperLink>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</Nik:NikGridView>
	</div>
</div>
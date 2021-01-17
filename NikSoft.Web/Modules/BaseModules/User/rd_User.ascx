<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rd_User.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.User.rd_User" %>
<Nik:ModalSearch runat="server">
	<div class="row">
		<div class="col-md-6">
			<div class="form-group">
				<label for="txtUsername">نام کاربری</label>
				<asp:TextBox ID="txtUsername" CssClass="form-control" runat="server" placeholder="نام کاربری" ClientIDMode="Static" />
			</div>
		</div>
		<div class="col-md-6">
			<div class="form-group">
				<label for="txtName">نام/ نام خوانوادگی</label>
				<asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="نام/ نام خوانوادگی" ClientIDMode="Static" />
			</div>
		</div>
	</div>
	<div class="row">
		<div class="col-md-6">
			<div class="form-group">
				<label for="ddlLock">وضعیت ورود</label>
				<asp:DropDownList ID="ddlLock" runat="server" CssClass="form-control" ClientIDMode="Static">
					<asp:ListItem Value="0">انتخاب کنید</asp:ListItem>
					<asp:ListItem Value="1">منع</asp:ListItem>
					<asp:ListItem Value="2">مجاز</asp:ListItem>
				</asp:DropDownList>
			</div>
		</div>
		<div class="col-md-6">
			<div class="form-group">
				<label for="ddlStatus">فعال</label>
				<asp:DropDownList ID="ddlStatus" CssClass="form-control" runat="server" ClientIDMode="Static">
					<asp:ListItem Value="0">انتخاب کنید</asp:ListItem>
					<asp:ListItem Value="1">فعال</asp:ListItem>
					<asp:ListItem Value="2">غیرفعال</asp:ListItem>
				</asp:DropDownList>
			</div>
		</div>
	</div>
	<div class="row">
		<div class="col-md-6">
			<div class="btn-group">				
				<Nik:NikLinkButton ID="breset" runat="server" Text="همه" OnClick="breset_Click" CssClass="btn btn-default btn-back" SettingValue="ResetButton"></Nik:NikLinkButton>
                <Nik:NikButton ID="btnSearch" runat="server" Text="جستجو" OnClick="btnSearch_Click" CssClass="btn btn-default btn-search" ClientIDMode="Static" SettingValue="SearchButton" />
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
		<Nik:NikGridView ID="GV1" runat="server" ItemType="NikSoft.NikModel.User">
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
				<asp:TemplateField HeaderText="نام کاربری">
					<ItemTemplate>
						<%# Item.UserName %>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="نام">
					<ItemTemplate>
						<%# Item.FirstName %>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="نام خانوادگی">
					<ItemTemplate>
						<%# Item.LastName %>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="پرتال">
					<ItemTemplate>
						<%# Item.Portal.Title %>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						<div class="btn-group-vertical">
							<asp:HyperLink runat="server" NavigateUrl='<%# ResolveUrl(GetUrlUserType(Item.ID)) %>' CssClass="btn btn-default btn-edite btn-sm">دسته کاربری</asp:HyperLink>
							<asp:HyperLink runat="server" NavigateUrl='<%# ResolveUrl(GetUrlUserGroup(Item.ID))%>' CssClass="btn btn-default btn-edite btn-sm">گروه کاربری</asp:HyperLink>
						</div>
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
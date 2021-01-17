<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cu_Portal.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Portal.cu_Portal" %>
<div class="row">
	<div class="col-sm-4">
		<div class="form-group">
			<label class="control-label" for="txtTitle">عنوان</label>
			<Nik:NikTextBox ID="txtTitle" CssClass="form-control reqfield" runat="server" MinLength="6" MaxLength="200" BoxTitle="Title" ClientIDMode="Static" data-validation="required length" data-validation-length="6-200"></Nik:NikTextBox>
		</div>
	</div>
	<div class="col-sm-4">
		<div class="form-group">
			<label class="control-label" for="txtAlias">Alias</label>
			<Nik:NikTextBox ID="txtAlias" runat="server" CssClass="form-control reqfield" MinLength="2" MaxLength="25" ClientIDMode="Static" data-validation="required length" data-validation-length="2-20"></Nik:NikTextBox>
		</div>
	</div>
    <div class="col-md-4">
		<div class="form-group">
			<label class="control-label" for="txtTitle">توضیحات متا</label>
			<Nik:NikTextBox ID="txtMeta" CssClass="form-control reqfield" runat="server" MinLength="6" MaxLength="200" ClientIDMode="Static" data-validation="required length" data-validation-length="6-200"></Nik:NikTextBox>
		</div>
	</div>
</div>
<div class="row">
	<div class="col-md-4">
		<div class="form-group">
			<label class="control-label" for="txtAlias">نام پوشه</label>
			<Nik:NikTextBox ID="txtFolderAlias" runat="server" CssClass="form-control reqfield" MinLength="2" MaxLength="25" ClientIDMode="Static" data-validation="required length" data-validation-length="2-20"></Nik:NikTextBox>
		</div>
	</div>
	<div class="col-md-4">
		<div class="form-group">
			<label class="control-label" for="ddlPortalParent">پورتال بالاتر</label>
			<asp:DropDownList ID="ddlPortalParent" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
		</div>
	</div>
	<div class="col-md-4">
		<div class="form-group">
			<label class="control-label" for="ddlPortalParent">جهت</label>
			<asp:DropDownList ID="ddlDirection" runat="server" ClientIDMode="Static" CssClass="form-control reqfield">
				<asp:ListItem Value="0">انتخاب کنید</asp:ListItem>
                <asp:ListItem Value="ltr">چپ به راست</asp:ListItem>
				<asp:ListItem Value="rtl">راست به چپ</asp:ListItem>
			</asp:DropDownList>
		</div>
	</div>
</div>
<div class="row">
	<div class="col-md-4">
		<div class="form-group">
			<label class="control-label" for="txtDomain">حداکثر حجم</label>
			<div class="input-group">
				<asp:TextBox ID="txtQouta" runat="server" CssClass="form-control reqfiled" ClientIDMode="Static" Text="500"></asp:TextBox>
				<span class="input-group-addon" id="basic-addon2">MG</span>
			</div>
		</div>
	</div>
	<div class="col-md-4">
		<div class="form-group">
			<label class="control-label" for="ddlLanguage">زبان</label>
			<asp:DropDownList ID="ddlLanguage" runat="server" ClientIDMode="Static" CssClass="form-control reqfield">
				<asp:ListItem Value="0" Text="انتخاب کنید"></asp:ListItem>
				<asp:ListItem Value="1" Text="انگلیسی"></asp:ListItem>
                <asp:ListItem Value="2" Text="فارسی"></asp:ListItem>
                <asp:ListItem Value="3" Text="آلمانی"></asp:ListItem>
                <asp:ListItem Value="4" Text="فرانسه"></asp:ListItem>
                <asp:ListItem Value="5" Text="ایتالیایی"></asp:ListItem>
                <asp:ListItem Value="6" Text="ترکی"></asp:ListItem>
			</asp:DropDownList>
		</div>
	</div>
    <div class="col-md-4">
		<div class="checkbox">
            <div class="label-null"></div>
			<label>
                <asp:CheckBox ID="chbHerOwnLogo" Checked="false" runat="server" />
				نمایش لگوی پرتال؟
			</label>
		</div>
	</div>
</div>
<div class="row">
	<div class="col-md-6">
        <label for="fuFav">آیکن پرتال(Fav Icon)</label>
		<div class="form-group">
			<label for="fuFav" class="btn btn-default btn-file-nik">
                انتخاب تصویر
                <asp:FileUpload runat="server" ClientIDMode="Static" ID="fuFav" CssClass="hidden" />
			</label>		
		</div>
	</div>
	<div class="col-md-6">
		<div class="form-group">
			<asp:Image runat="server" ID="ImgFav" CssClass="img-responsive" />
		</div>
	</div>
</div>
<div class="row">
	<div class="col-md-12">
		<div class="form-group">
			<label class="control-label" for="txtDesc">توضیحات</label>
			<Nik:NikTextBox TextMode="MultiLine" Rows="5" ID="txtDesc" runat="server" CssClass="form-control" MaxLength="20" BoxTitle="Description" ClientIDMode="Static" data-validation="length" data-validation-length="max1000"></Nik:NikTextBox>
		</div>
	</div>
</div>
<hr />
<div class="row">
	<div class="col-md-12 text-center">
		<Nik:NikButton ID="btnSave" runat="server" Text="ذخیره" ClientIDMode="Static" class="btn btn-default btn-save-nik" SettingValue="SaveButton" />
	</div>
</div>
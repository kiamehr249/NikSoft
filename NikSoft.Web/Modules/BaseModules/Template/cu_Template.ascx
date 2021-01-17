<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cu_Template.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Template.cu_Template" %>
<div class="row">
	<div class="col-md-6">
		<div class="form-group">
			<label class="control-label" for="txtTitle">عنوان</label>
			<Nik:NikTextBox runat="server" ID="txtTitle" ClientIDMode="Static" BoxTitle="Title" MinLength="3" MaxLength="100" CssClass="reqfield form-control"></Nik:NikTextBox>
		</div>
	</div>
	<div class="col-md-6">
		<div class="form-group">
			<label class="control-label" for="txtTitle">نوع</label>
			<asp:DropDownList ID="ddlTemplateType" runat="server" ClientIDMode="Static" CssClass="reqfield form-control"></asp:DropDownList>
		</div>
	</div>
</div>
<div class="row">
	<div class="col-md-12">
		<div class="form-group">
			<label class="control-label" for="txtDesc">توضیحات</label>
			<Nik:NikTextBox runat="server" ID="txtDesc" ClientIDMode="Static" BoxTitle="Desciptions" MaxLength="225" CssClass="form-control" TextMode="MultiLine" Rows="3"></Nik:NikTextBox>
		</div>
	</div>
</div>
<div class="row">
	<div class="col-md-4">
		<div class="form-group">
			<label class="control-label" for="txtTitle">پوسته</label>
			<asp:DropDownList ID="ddlUI" runat="server" ClientIDMode="Static" CssClass="reqfield form-control"></asp:DropDownList>
		</div>
	</div>
	<div class="col-md-4" id="selectmodule">
		<div class="form-group">
			<label class="control-label" for="txtTitle">ماژول انتخابی</label>
			<asp:DropDownList ID="ddlModule" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
		</div>
	</div>
    <div class="col-md-4" id="divparameter">
		<div class="form-group">
			<label class="control-label" for="txtTitle">ماژول پارامتر</label>
			<asp:TextBox ID="txtModuleParameter" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
		</div>
	</div>
</div>
<hr />
<div class="row">
	<div class="col-md-12 text-center">
		<Nik:NikButton ID="btnSave" runat="server" Text="ذخیره" ClientIDMode="Static" class="btn btn-default btn-save-nik" SettingValue="SaveButton" />
	</div>
</div>
<script type="text/javascript">
	var checkPageType = function () {
		var a = $('#ddlTemplateType').val();
		if (a === '2') {
			$('#selectmodule,#divparameter').show();
		} else {
			$('#selectmodule,#divparameter').hide();
		}
	}

	$(document).ready(function () {
		$('#ddlTemplateType').change(checkPageType);
		checkPageType();
	});
</script>
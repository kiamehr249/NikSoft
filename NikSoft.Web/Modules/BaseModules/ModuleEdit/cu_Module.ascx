<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cu_Module.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.ModuleEdit.cu_Module" %>
<div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <label class="control-label" for="txtTitle">عنوان</label>
            <Nik:NikTextBox BoxTitle="Title" ID="txtTitle" runat="server" ClientIDMode="Static" CssClass="form-control reqfield" MaxLength="200" MinLength="3" data-validation="required length" data-validation-length="3-200"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-md-6">
        <div class="form-group">
            <label class="control-label" for="TxtModuleKey">کلید(انگلیسی)</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtModuleKey" runat="server" ClientIDMode="Static" CssClass="form-control reqfield" MaxLength="200" MinLength="3" data-validation="required length" data-validation-length="3-200"></Nik:NikTextBox>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <label class="control-label" for="ddlCategory">دسته بندی</label>
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control select2" ClientIDMode="Static"></asp:DropDownList>
        </div>
    </div>
    <div class="col-sm-6">
        <div class="form-group">
            <label class="control-label" for="ddlEditable">ماژول پایه</label>
            <asp:DropDownList ID="ddlEditable" runat="server" CssClass="form-control select2" ClientIDMode="Static"></asp:DropDownList>
        </div>
    </div>
</div>
<hr />
<div class="row">
    <div class="col-md-12 text-center">
        <Nik:NikButton ID="btnSave" runat="server" Text="ذخیره" SettingValue="SaveButton" CssClass="btn btn-default btn-save-nik" />
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        FirstOnChange('#ddlCategory', '#ddlEditable', GetModulesByCategory);
		<%= SelectData1 %>

        function GetModulesByCategory(gID, jID) {
            var ddlGroups = $('#ddlEditable');
            var data = "{'id' : '" + gID + "'}";
            var url = '<%=ResolveUrl("../../../WebService/PortalWebService.asmx/GetModulesByCategory") %>';
            CallWebServices(url, data, ddlGroups, jID);
        }
    });
</script>
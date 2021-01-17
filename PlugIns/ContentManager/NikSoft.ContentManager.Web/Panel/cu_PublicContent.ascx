<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cu_PublicContent.ascx.cs" Inherits="NikSoft.ContentManager.Web.Panel.cu_PublicContent" %>
<asp:HiddenField ID="hfEditor" Value="false" runat="server" />
<div class="row">
    <div class="col-sm-6">
        <div class="form-group">
            <label class="control-label" for="txtTitle">عنوان</label>
            <Nik:NikTextBox BoxTitle="Title" ID="txtTitle" runat="server" ClientIDMode="Static" CssClass="form-control reqfield"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-6">
        <div class="form-group">
            <label class="control-label" for="txtTitle">آیکن فونت</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtIconFont" runat="server" ClientIDMode="Static" CssClass="form-control reqfield"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-6">
        <div class="form-group">
            <label class="control-label" for="ddlContentGroup">گروه محتوایی</label>
            <asp:DropDownList ID="ddlContentGroup" runat="server" CssClass="form-control select2" ClientIDMode="Static"></asp:DropDownList>
        </div>
    </div>
    <div class="col-sm-6">
        <div class="form-group">
            <label class="control-label" for="ddlContTitle">دسته محتوایی</label>
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control select2" ClientIDMode="Static"></asp:DropDownList>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-sm-12">
        <div class="form-group">
            <label class="control-label" for="TxtMiniDesc">لید</label>
            <Nik:NikTextBox ID="TxtMiniDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></Nik:NikTextBox>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-sm-6">
        <label class="control-label" for="TxtFullText">متن</label>
    </div>
    <div class="col-sm-6 text-left">
        <asp:Button ID="BtnTextEditor" CssClass="btn btn-edite btn-sm" runat="server" Text="ادیتور" OnClick="BtnTextEditor_Click" />
    </div>
    <div id="TextAreaText" runat="server" class="col-sm-12">
        <div class="form-group">
            <Nik:NikTextBox ID="TxtFullText" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6"></Nik:NikTextBox>
        </div>
    </div>
    <div id="EditorText" runat="server" visible="false" class="col-sm-12">
        <div class="form-group">
            <Nik:TinyMCE ID="EditFullText" CssClass="form-control" Height="200" ShowEditor="true" runat="server" ToolbarMode="Full"></Nik:TinyMCE>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-2">
        <div class="checkbox">
            <div class="label-null"></div>
            <label>
                <asp:CheckBox ID="chbEnbaled" Checked="true" runat="server" ClientIDMode="Static" />
                فعال؟
            </label>
        </div>
    </div>
    <div class="col-md-2">
        <div class="checkbox">
            <div class="label-null"></div>
            <label>
                <asp:CheckBox ID="chbIsStore"  runat="server" ClientIDMode="Static" />
                فروشگاهی؟
            </label>
        </div>
    </div>
    <div class="col-sm-2">
        <div class="form-group">
            <label class="control-label" for="FuImg">تصویر</label>
            <div class="select-img">
                <label for="FuImg" class="btn btn-default btn-file-nik">
                    <asp:FileUpload ID="FuImg" runat="server" CssClass="hidden" ClientIDMode="Static" />
                    انتخاب تصویر
                </label>
            </div>
            <div class="image-icon-panel">
                <asp:Image ID="ImgContent" runat="server" CssClass="image-icon-nik" />
                <div class="file-remover-wrapper">
                    <div class="remove-icon-holder">
                        <i class="fa fa-trash" aria-hidden="true"></i>
                        <asp:Button ID="BtnRemoveImg" runat="server" Text="" CssClass="btn-file-remover" OnClick="BtnRemoveImg_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-sm-2">
        <div class="form-group">
            <label class="control-label" for="fuIcon">آیکن</label>
            <div class="select-img">
                <label for="fuIcon" class="btn btn-default btn-file-nik">
                    <asp:FileUpload ID="fuIcon" runat="server" CssClass="hidden" ClientIDMode="Static" />
                    انتخاب آیکن
                </label>
            </div>
            <div class="image-icon-panel">
                <asp:Image ID="ImgIcon" runat="server" CssClass="image-icon-nik" />
                <div class="file-remover-wrapper">
                    <div class="remove-icon-holder">
                        <i class="fa fa-trash" aria-hidden="true"></i>
                        <asp:Button ID="BtnRemoveIcon" runat="server" Text="" CssClass="btn-file-remover" OnClick="BtnRemoveIcon_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-sm-6">
        <div class="form-group">
            <label class="control-label" for="fuIcon">فایل ضمیمه</label>
            <div class="select-file">
                <label for="FuAttachFile" class="btn btn-file-nik">
                    <asp:FileUpload ID="FuAttachFile" runat="server" CssClass="hidden" ClientIDMode="Static" />
                    انتخاب فایل
                </label>
            </div>
            <div class="file-details-panel">
                <i class="fa fa-trash" aria-hidden="true"><asp:Button ID="BtnRemovFile" CssClass="btn-file-remover" runat="server" OnClick="BtnRemovFile_Click" /></i>
                <asp:Label ID="LblSelectFile" runat="server"></asp:Label>
            </div>
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
        FirstOnChange('#ddlContentGroup', '#ddlCategory', GetContentCategory);
		<%= SelectData1 %>

        function GetContentCategory(gID, jID) {
            var ddlGroups = $('#ddlCategory');
            var data = "{'GroupID' : '" + gID + "'}";
            var url = '<%=ResolveUrl("../WebService/ContentWebService.asmx/GetContentCategory") %>';
            CallWebServices(url, data, ddlGroups, jID);
        }
    });
</script>
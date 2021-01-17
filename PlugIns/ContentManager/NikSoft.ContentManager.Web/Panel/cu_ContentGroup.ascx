<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cu_ContentGroup.ascx.cs" Inherits="NikSoft.ContentManager.Web.Panel.cu_ContentGroup" %>
<div class="row">
    <div class="col-sm-4">
        <div class="form-group">
            <label class="control-label" for="txtTitle">عنوان</label>
            <Nik:NikTextBox BoxTitle="Title" ID="txtTitle" runat="server" CssClass="form-control reqfield"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-4">
        <div class="form-group">
            <label class="control-label" for="TxtFontIcon">فونت</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtFontIcon" runat="server" CssClass="form-control"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-2">
        <div class="form-group">
            <label class="control-label" for="fuIcon">تصویر</label>
            <div class="select-img">
                <label for="fuIcon" class="btn btn-default btn-file-nik">
                    <asp:FileUpload ID="fuIcon" runat="server" CssClass="hidden" ClientIDMode="Static" />
                    انتخاب تصویر
                </label>
                <div class="file-detail" id="fdcw1" style="display: none;">
                    <p id="fdc1"></p>
                </div>
            </div>
            <div class="image-icon-panel">
                <asp:Image ID="CatImage" runat="server" CssClass="image-icon-nik" />
                <div class="file-remover-wrapper">
                    <div class="remove-icon-holder">
                        <i class="fa fa-trash" aria-hidden="true"></i>
                        <asp:Button ID="BtnRemoveImg" runat="server" Text="" CssClass="btn-file-remover" OnClick="BtnRemoveImg_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-md-2">
        <div class="checkbox">
            <div class="label-null"></div>
            <label>
                <asp:CheckBox ID="chbEnbaled" Checked="true" runat="server" ClientIDMode="Static" />
                فعال؟
            </label>
        </div>
    </div>
    <div class="col-sm-12">
        <div class="form-group">
            <label class="control-label" for="TxtDesc">توضیحات</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtDesc" runat="server" TextMode="MultiLine" Rows="3"  CssClass="form-control"></Nik:NikTextBox>
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
        $('#fuIcon').change(function () {
            console.log($(this).val());
            var fnv = $(this).val();
            $('#fdc1').text(fnv);
            $('#fdcw1').fadeIn();
        });
    });
</script>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cu_VisualLinkItem.ascx.cs" Inherits="NikSoft.ContentManager.Web.Panel.cu_VisualLinkItem" %>
<div class="row">
    <div class="col-sm-6">
        <div class="form-group">
            <label class="control-label" for="txtTitle">عنوان</label>
            <Nik:NikTextBox BoxTitle="Title" ID="txtTitle" runat="server" ClientIDMode="Static" CssClass="form-control reqfield"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-6">
        <div class="form-group">
            <label class="control-label" for="ddlContTitle">گروه پیوند</label>
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control select2" ClientIDMode="Static"></asp:DropDownList>
        </div>
    </div>
    <div class="col-sm-6">
        <div class="form-group">
            <label class="control-label" for="TxtLink1">لینک 1</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtLink1" runat="server" ClientIDMode="Static" CssClass="form-control reqfield"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-6">
        <div class="form-group">
            <label class="control-label" for="TxtLink2">لینک 2</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtLink2" runat="server" ClientIDMode="Static" CssClass="form-control reqfield"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-6">
        <div class="form-group">
            <label class="control-label" for="TxtBtnText1">عنوان لینک 1</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtBtnText1" runat="server" ClientIDMode="Static" CssClass="form-control reqfield"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-6">
        <div class="form-group">
            <label class="control-label" for="TxtBtnText2">عنوان لینک 2</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtBtnText2" runat="server" ClientIDMode="Static" CssClass="form-control reqfield"></Nik:NikTextBox>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-sm-12">
        <div class="form-group">
            <label class="control-label" for="TxtDesc">توضیحات</label>
            <Nik:NikTextBox ID="TxtDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></Nik:NikTextBox>
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
    <div class="col-sm-2">
        <div class="form-group">
            <label class="control-label" for="FuImg">تصیر کاور</label>
            <div class="select-img">
                <label for="FuImg1" class="btn btn-default btn-file-nik">
                    <asp:FileUpload ID="FuImg1" runat="server" CssClass="hidden" ClientIDMode="Static" />
                    انتخاب تصویر
                </label>
            </div>
            <div class="image-icon-panel">
                <asp:Image ID="Img1Prev" runat="server" CssClass="image-icon-nik" />
                <div class="file-remover-wrapper">
                    <div class="remove-icon-holder">
                        <i class="fa fa-trash" aria-hidden="true"></i>
                        <asp:Button ID="BtnRemoveImg1" runat="server" Text="" CssClass="btn-file-remover" OnClick="BtnRemoveImg1_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-sm-2">
        <div class="form-group">
            <label class="control-label" for="FuImg">تصویر 2</label>
            <div class="select-img">
                <label for="FuImg2" class="btn btn-default btn-file-nik">
                    <asp:FileUpload ID="FuImg2" runat="server" CssClass="hidden" ClientIDMode="Static" />
                    انتخاب تصویر
                </label>
            </div>
            <div class="image-icon-panel">
                <asp:Image ID="Img2Prev" runat="server" CssClass="image-icon-nik" />
                <div class="file-remover-wrapper">
                    <div class="remove-icon-holder">
                        <i class="fa fa-trash" aria-hidden="true"></i>
                        <asp:Button ID="BtnRemoveImg2" runat="server" Text="" CssClass="btn-file-remover" OnClick="BtnRemoveImg2_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-sm-2">
        <div class="form-group">
            <label class="control-label" for="FuImg">تصویر 3</label>
            <div class="select-img">
                <label for="FuImg1" class="btn btn-default btn-file-nik">
                    <asp:FileUpload ID="FuImg3" runat="server" CssClass="hidden" ClientIDMode="Static" />
                    انتخاب تصویر
                </label>
            </div>
            <div class="image-icon-panel">
                <asp:Image ID="Img3Prev" runat="server" CssClass="image-icon-nik" />
                <div class="file-remover-wrapper">
                    <div class="remove-icon-holder">
                        <i class="fa fa-trash" aria-hidden="true"></i>
                        <asp:Button ID="BtnRemoveImg3" runat="server" Text="" CssClass="btn-file-remover" OnClick="BtnRemoveImg3_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-sm-2">
        <div class="form-group">
            <label class="control-label" for="FuImg">تصویر 4</label>
            <div class="select-img">
                <label for="FuImg4" class="btn btn-default btn-file-nik">
                    <asp:FileUpload ID="FuImg4" runat="server" CssClass="hidden" ClientIDMode="Static" />
                    انتخاب تصویر
                </label>
            </div>
            <div class="image-icon-panel">
                <asp:Image ID="Img4Prev" runat="server" CssClass="image-icon-nik" />
                <div class="file-remover-wrapper">
                    <div class="remove-icon-holder">
                        <i class="fa fa-trash" aria-hidden="true"></i>
                        <asp:Button ID="BtnRemoveImg4" runat="server" Text="" CssClass="btn-file-remover" OnClick="BtnRemoveImg4_Click" />
                    </div>
                </div>
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
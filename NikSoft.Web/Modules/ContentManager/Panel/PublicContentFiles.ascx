<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PublicContentFiles.ascx.cs" Inherits="NikSoft.ContentManager.Web.Panel.PublicContentFiles" %>
<%@ Import Namespace="NikSoft.ContentManager.Service" %>

<asp:HiddenField ID="hfEditItemID" Value="0" runat="server" />
<div id="SearchModal" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">جستجو</h4>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="control-label" for="txtTitle">عنوان</label>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="control-label" for="ddlSearchType">نوع</label>
                            <asp:DropDownList ID="ddlSearchType" runat="server" CssClass="form-control select2" ClientIDMode="Static">
                                <asp:ListItem Text="انتخاب کنید" Value="0"></asp:ListItem>
                                <asp:ListItem Text="تصویر" Value="1"></asp:ListItem>
                                <asp:ListItem Text="ویدیو" Value="2"></asp:ListItem>
                                <asp:ListItem Text="صدا" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="input-group">
                                <label class="input-group-addon" for="txtCount">تعداد:</label>
                                <asp:TextBox ClientIDMode="Static" ID="txtCount" runat="server" ForeColor="Red" Enabled="false" CssClass="countlabel form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="btn-group">
                                <Nik:NikLinkButton ID="breset" runat="server" Text="همه" OnClick="breset_Click" CssClass="btn btn-default btn-back btn-sm" SettingValue="ResetButton">همه</Nik:NikLinkButton>
                                <Nik:NikButton ID="btnSearch" runat="server" Text="جستجو" OnClick="btnSearch_Click" CssClass="btn btn-default btn-search btn-sm" ClientIDMode="Static" SettingValue="SearchButton" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<div class="row">
    <div class="col-sm-4">
        <div class="form-group">
            <label class="control-label" for="txtTitle">عنوان</label>
            <asp:TextBox ID="TxtItemTitle" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
        </div>
    </div>
    <div class="col-sm-4">
        <div class="form-group">
            <label class="control-label" for="ddlItemType">نوع</label>
            <asp:DropDownList ID="ddlItemType" runat="server" CssClass="form-control select2" ClientIDMode="Static">
                <asp:ListItem Text="انتخاب کنید" Value="0"></asp:ListItem>
                <asp:ListItem Text="تصویر" Value="1"></asp:ListItem>
                <asp:ListItem Text="ویدیو" Value="2"></asp:ListItem>
                <asp:ListItem Text="صدا" Value="3"></asp:ListItem>
                <asp:ListItem Text="فایل" Value="4"></asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-sm-2">
        <div class="form-group">
            <label class="control-label" for="FuFile">فایل</label>
            <div class="select-img">
                <label for="FuFile" class="btn btn-default btn-file-nik">
                    <asp:FileUpload ID="FuFile" runat="server" CssClass="hidden" ClientIDMode="Static" />
                    انتخاب فایل
                </label>
                <div class="file-detail" id="fdcw1" style="display: none;">
                    <p id="fdc1"></p>
                </div>
            </div>
            <div class="image-icon-panel" id="fileImg" runat="server" visible="false">
                <asp:Image ID="ImgFile" runat="server" CssClass="image-icon-nik" />
            </div>
        </div>
    </div>
    <div class="col-md-1">
        <div class="checkbox">
            <div class="label-null"></div>
            <label>
                <asp:CheckBox ID="chbEnbaled" Checked="true" runat="server" ClientIDMode="Static" />
                فعال؟
            </label>
        </div>
    </div>
    <div class="col-md-1">
        <div class="checkbox" id="coverwrap" style="display: none;">
            <div class="label-null"></div>
            <label>
                <asp:CheckBox ID="chbIsCover" runat="server" ClientIDMode="Static" />
                کاور؟
            </label>
        </div>
    </div>
    <div class="col-sm-12">
        <div class="form-group">
            <label class="control-label" for="txtTitle">توضیحات</label>
            <asp:TextBox ID="TxtDesc" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
        </div>
    </div>
</div>
<hr />
<div class="row">
    <div class="col-sm-12 text-center">
        <div class="form-group">
            <asp:HyperLink ID="HypBackToList" CssClass="btn btn-default btn-back-nik" runat="server">بازگشت</asp:HyperLink>
            <asp:Button ID="BtnSave" CssClass="btn btn-save-nik" runat="server" Text="ذخیره" OnClick="BtnSave_Click" />
            <asp:HyperLink ID="HypCancel" CssClass="btn btn-back-nik" runat="server" Text="لغو" Visible="false"></asp:HyperLink>
            <asp:Button ID="BtnUpdate" CssClass="btn btn-save-nik" runat="server" Text="ذخیره" Visible="false" OnClick="BtnUpdate_Click" />
        </div>
    </div>
</div>
<hr />
<br />
<div class="row">
    <div class="col-sm-12 text-left">
        <button type="button" class="btn btn-search btn-sm" data-toggle="modal" data-target="#SearchModal"><i class="fa fa-search" aria-hidden="true"></i> جستجو</button>
    </div>
</div>
<div class="row">
    <div class="col-md-12 table-responsive">
        <Nik:NikGridView ID="GV1" PageSize="15" runat="server" ItemType="NikSoft.ContentManager.Service.ContentFile" OnRowCommand="GV1_RowCommand">
            <EmptyDataTemplate>
                <div class="noinfo">هیج موردی وجود ندارد.</div>
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
                <asp:TemplateField HeaderText="عنوان">
                    <ItemTemplate>
                        <%# Item.Title %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="تصویر">
                    <ItemTemplate>
                        <img src='<%# "/" + Item.FileUrl %>' class="grid-item-img" style="max-height: 70px;" runat="server" visible="<%# Item.ItemType == FileType.Image ? true : false %>" />
                        <img src='<%# "/" + Item.CoverImage %>' class="grid-item-img" style="max-height: 70px;" runat="server" visible="<%# Item.ItemType == FileType.Video ? true : false %>" />
                        <img src="/images/EmptySound.jpg" class="grid-item-img" style="max-height: 70px;" runat="server" visible="<%# Item.ItemType == FileType.Sound ? true : false %>" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="نوع فایل">
                    <ItemTemplate>
                        <%# GetFileType(Item.ItemType) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="کاور">
                    <ItemTemplate>
                        <div class="text-center">
                            <Nik:NikConfirmLinkButton Visible='<%# Item.ItemType == FileType.Image ? true : false %>' CssClass='<%# Item.IsCover ? "btn btn-enable btn-sm" : "btn btn-disable btn-sm" %>' ID="isCovered" runat="server" CommandArgument='<%# Item.ID %>' MessageText="آیا اطمینان دارید؟" CommandName="iscover">
							    <%# NikSoft.Utilities.Utilities.GetEnabledImage(Item.IsCover) %>
                            </Nik:NikConfirmLinkButton>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="تنظیمات">
                    <ItemTemplate>
                        <div class="form-group text-center">
                            <div class="btn-group-vertical">
                                <Nik:NikLinkButton CssClass="btn btn-edite btn-sm" ID="EditItem" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="EditMe">
                                    <i class="fa fa-pencil-square-o" aria-hidden="true"></i> ویرایش
                                </Nik:NikLinkButton>
                                <Nik:NikConfirmLinkButton CssClass="btn btn-delete btn-sm" ID="DeleteItem" runat="server" CommandArgument='<%# Item.ID %>' MessageText="آیا اطمینان دارید؟" CommandName="DeleteMe">
                                    <i class="fa fa-trash-o" aria-hidden="true"></i> حذف
                                </Nik:NikConfirmLinkButton>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="فعال">
                    <ItemTemplate>
                        <div class="text-center">
                            <Nik:NikConfirmLinkButton CssClass='<%# Item.Enabled ? "btn btn-enable btn-sm" : "btn btn-disable btn-sm" %>' ID="clenabled" runat="server" CommandArgument='<%# Item.ID %>' MessageText="آیا اطمینان دارید؟" CommandName="enabled">
							    <%# NikSoft.Utilities.Utilities.GetEnabledImage(Item.Enabled) %>
                            </Nik:NikConfirmLinkButton>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="مرتب سازی">
                    <ItemTemplate>
                        <div class="text-center">
                            <div class="btn-group">
                                <asp:LinkButton CssClass="btn btn-edite btn-sm" runat="server" ID="bMUp" CommandArgument='<%# Eval("ID") %>' CommandName="MoveUp">
								    <span class="glyphicon glyphicon-chevron-up"></span>
                                </asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-edite btn-sm" runat="server" ID="bMDown" CommandArgument='<%# Eval("ID") %>' CommandName="MoveDown">			
                                    <span class="glyphicon glyphicon-chevron-down"></span>
                                </asp:LinkButton>
                            </div>
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
<script type="text/javascript">
    $(document).ready(function () {
        $('#FuFile').change(function () {
            console.log($(this).val());
            var fnv = $(this).val();
            $('#fdc1').text(fnv);
            $('#fdcw1').fadeIn();
            setTimeout(function () {
                $('#fdcw1').fadeOut();
            }, 3000);
        });
        $('#ddlItemType').change(function () {
            if ($(this).find("option:selected").index() != 1) {
                $("#coverwrap").fadeOut();
            } else {
                $("#coverwrap").fadeIn();
            }
        });
    });
</script>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateFeatureFrom.ascx.cs" Inherits="NikSoft.ContentManager.Web.Panel.CreateFeatureFrom" %>
<asp:HiddenField ID="hfEditItemID" Value="0" runat="server" />

<Nik:ModalSearch runat="server">
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="control-label" for="txtTitle">عنوان</label>
                <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="control-label" for="ddlCategory">دسته بندی</label>
                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control select2" ClientIDMode="Static"></asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <div class="btn-group">
                    <Nik:NikLinkButton ID="breset" runat="server" OnClick="breset_Click" CssClass="btn btn-back btn-sm" SettingValue="ResetButton">همه</Nik:NikLinkButton>
                    <Nik:NikButton ID="btnSearch" runat="server" Text="جستجو" OnClick="btnSearch_Click" CssClass="btn btn-search btn-sm" ClientIDMode="Static" SettingValue="SearchButton" />
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <div class="input-group">
                    <label class="input-group-addon" for="txtCount">تعداد:</label>
                    <asp:TextBox ClientIDMode="Static" ID="txtCount" runat="server" ForeColor="Red" Enabled="false" CssClass="countlabel form-control"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
</Nik:ModalSearch>

<div class="form-wrap">
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="control-label" for="txtTitle">عنوان</label>
                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-2">
            <div class="checkbox">
                <div class="label-null"></div>
                <label for="chbEnbaled">
                    <asp:CheckBox ID="chbEnbaled" Checked="true" runat="server" ClientIDMode="Static" />
                    فعال؟
                </label>
            </div>
        </div>
        <div class="col-md-12">
            <div class="form-group">
                <label class="control-label" for="TxtDesc">توضیحات</label>
                <asp:TextBox ID="TxtDesc" runat="server" CssClass="form-control" ClientIDMode="Static" TextMode="MultiLine" Rows="3"></asp:TextBox>
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
</div>

<div class="row">
    <div class="col-md-12 table-responsive">
        <Nik:NikGridView ID="GV1" PageSize="15" runat="server" ItemType="NikSoft.ContentManager.Service.FeatureForm" OnRowCommand="GV1_RowCommand">
            <EmptyDataTemplate>
                <div class="noinfo">هیج موردی وجود ندارد.</div>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="ردیف" ItemStyle-Width="2%">
                    <ItemTemplate>
                        <Nik:ListCounter ID="ListCounter2" runat="server" IndexFormat="{0}."></Nik:ListCounter>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="عنوان">
                    <ItemTemplate>
                        <%# Item.Title %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="تنظیمات">
                    <ItemTemplate>
                        <div class="form-group text-center">
                            <div class="btn-group-vertical">
                                <asp:HyperLink CssClass="btn btn-search btn-sm" ID="HypItems" runat="server" NavigateUrl='<%# "/panel/FeatureItems/" + Item.ID %>'>
                                    <i class="fa fa-sitemap" aria-hidden="true"></i> آیتم ها
                                </asp:HyperLink>
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
            </Columns>
        </Nik:NikGridView>
    </div>
</div>

<div class="row">
    <div class="col-md-12">
        <asp:PlaceHolder ID="pl1" runat="server" EnableViewState="False" ViewStateMode="Disabled"></asp:PlaceHolder>
    </div>
</div>
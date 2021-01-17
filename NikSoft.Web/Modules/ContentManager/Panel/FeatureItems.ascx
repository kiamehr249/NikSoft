<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeatureItems.ascx.cs" Inherits="NikSoft.ContentManager.Web.Panel.FeatureItems" %>
<%@ Import Namespace="NikSoft.ContentManager.Service" %>
<asp:HiddenField ID="HfEdit" Value="0" runat="server" />
<div class="call-out call-out-info">
    <h3><span>فرم: </span> <span><%= FeatureName %></span></h3>
</div>
<div class="row">
    <div class="col-md-4">
        <div class="form-group">
            <label class="control-label" for="txtTitle">عنوان</label>
            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
        </div>
    </div>
    <div class="col-md-4">
        <div class="form-group">
            <label class="control-label" for="TxtFeatureKey">کلید(انگلیسی)</label>
            <asp:TextBox ID="TxtFeatureKey" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
        </div>
    </div>
    <div class="col-md-4">
        <div class="form-group">
            <label class="control-label" for="txtTitle">نوع</label>
            <asp:DropDownList ID="DdlType" runat="server" CssClass="form-control select2">
                <asp:ListItem Text="انتخاب کنید" Value="0"></asp:ListItem>
                <asp:ListItem Text="کادر متنی" Value="1"></asp:ListItem>
                <asp:ListItem Text="کادر متنی بزرگ" Value="2"></asp:ListItem>
                <asp:ListItem Text="لیست آبشاری" Value="3"></asp:ListItem>
                <asp:ListItem Text="لیست انتخابی" Value="4"></asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-md-12">
        <div class="form-group">
            <label class="control-label" for="txtTitle">توضیحات</label>
            <asp:TextBox ID="TxtDesc" runat="server" CssClass="form-control" TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>
        </div>
    </div>
</div>
<div class="btn-row text-center">
    <asp:HyperLink ID="HpBack" CssClass="btn btn-back-nik" runat="server">بازگشت</asp:HyperLink>
    <asp:HyperLink ID="HpCancel" CssClass="btn btn-back-nik" runat="server" Visible="false">لغو</asp:HyperLink>
    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-save-nik" Text="ذخیره" OnClick="btnSave_Click" />
    <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-save-nik" Text="بروز رسانی" Visible="false" OnClick="btnUpdate_Click" />
</div>
<hr />

<div class="row">
    <div class="col-md-12 table-responsive">
        <Nik:NikGridView ID="GV1" PageSize="15" runat="server" ItemType="NikSoft.ContentManager.Service.FeatuerModel" OnRowCommand="GV1_RowCommand">
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
                <asp:TemplateField HeaderText="نوع">
                    <ItemTemplate>
                        <%# GetTpye(Item.Type) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="تنظیمات">
                    <ItemTemplate>
                        <div class="form-group text-center">
                            <div class="btn-group-vertical">
                                <asp:HyperLink ID="hpItems" runat="server" NavigateUrl='<%# "/panel/FeatureSubItem/" + Item.FormID + "?type=" + Convert.ToInt32(Item.Type) + "&parent=" + Item.ItemID %>' CssClass="btn btn-search btn-sm" Visible="<%# (Item.Type == FeatureType.DropDown) || Item.Type == FeatureType.CheckBoxList ? true : false %>">آیتم ها</asp:HyperLink>
                                <Nik:NikLinkButton CssClass="btn btn-edite btn-sm" ID="EditItem" runat="server" CommandArgument='<%# Item.ItemID %>' CommandName="EditMe">
                                    <i class="fa fa-pencil-square-o" aria-hidden="true"></i> ویرایش
                                </Nik:NikLinkButton>
                                <Nik:NikConfirmLinkButton CssClass="btn btn-delete btn-sm" ID="DeleteItem" runat="server" CommandArgument='<%# Item.ItemID %>' MessageText="آیا اطمینان دارید؟" CommandName="DeleteMe">
                                    <i class="fa fa-trash-o" aria-hidden="true"></i> حذف
                                </Nik:NikConfirmLinkButton>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="فعال">
                    <ItemTemplate>
                        <div class="text-center">
                            <Nik:NikConfirmLinkButton CssClass='<%# Item.Enabled ? "btn btn-enable btn-sm" : "btn btn-disable btn-sm" %>' ID="clenabled" runat="server" CommandArgument='<%# Item.ItemID %>' MessageText="آیا اطمینان دارید؟" CommandName="enabled">
							    <%# NikSoft.Utilities.Utilities.GetEnabledImage(Item.Enabled) %>
                            </Nik:NikConfirmLinkButton>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="مرتب سازی">
                    <ItemTemplate>
                        <div class="text-center">
                            <div class="btn-group">
                                <asp:LinkButton CssClass="btn btn-edite btn-sm" runat="server" ID="bMUp" CommandArgument='<%# Item.ItemID %>' CommandName="MoveUp">
								    <span class="glyphicon glyphicon-chevron-up"></span>
                                </asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-edite btn-sm" runat="server" ID="bMDown" CommandArgument='<%# Item.ItemID %>' CommandName="MoveDown">			
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
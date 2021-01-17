<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeatureSubItem.ascx.cs" Inherits="NikSoft.ContentManager.Web.Panel.FeatureSubItem" %>

<div class="call-out call-out-info">
    <h3><span>آیتم: </span> <span><%= FeatureItemName %></span></h3>
</div>

<asp:HiddenField ID="HfEdit" Value="0" runat="server" />
<div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <label class="control-label" for="txtTitle">عنوان</label>
            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
        </div>
    </div>
    <div class="col-md-6" id="keyWrap" runat="server">
        <div class="form-group">
            <label class="control-label" for="TxtKeyFeature">کلید(انگلیسی)</label>
            <asp:TextBox ID="TxtKeyFeature" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
        </div>
    </div>
    <div class="col-md-12" id="DescWrap" runat="server">
        <div class="form-group">
            <label class="control-label" for="TxtKeyFeature">توضیحات</label>
            <asp:TextBox ID="TxtDesc" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
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
        <Nik:NikGridView ID="GV1" PageSize="15" runat="server" ItemType="NikSoft.ContentManager.Service.FeatureListModel" OnRowCommand="GV1_RowCommand">
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
                                <Nik:NikLinkButton CssClass="btn btn-edite btn-sm" ID="EditItem" runat="server" CommandArgument='<%# Item.ID %>' CommandName="EditMe">
                                    <i class="fa fa-pencil-square-o" aria-hidden="true"></i> ویرایش
                                </Nik:NikLinkButton>
                                <asp:HyperLink NavigateUrl='<%# "/panel/FeatureSubItem/" + Item.FeatureFormID + "?type=3&parent=" + Item.ID %>' ID="HpDropItem" runat="server" CssClass="btn btn-search btn-sm" Visible="<%# TypeID == 3 ? true : false %>">آیتم ها</asp:HyperLink>
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
                                <asp:LinkButton CssClass="btn btn-edite btn-sm" runat="server" ID="bMUp" CommandArgument='<%# Item.ID %>' CommandName="MoveUp">
								    <span class="glyphicon glyphicon-chevron-up"></span>
                                </asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-edite btn-sm" runat="server" ID="bMDown" CommandArgument='<%# Item.ID %>' CommandName="MoveDown">			
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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rd_Template.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Template.rd_Template" %>
<Nik:ModalSearch runat="server">
    <div class="row">
        <div class="col-md-4">
            <div class="form-group">
                <label for="txtName">عنوان</label>
                <asp:TextBox ID="txtName" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-4">
            <div class="form-group">
                <label for="ddlTemplateType">نوع تمپلیت</label>
                <asp:DropDownList ID="ddlTemplateType" runat="server" ClientIDMode="Static" CssClass="form-control select2">
                    <asp:ListItem Value="0">انتخاب کنید</asp:ListItem>
                    <asp:ListItem Value="1">صفحه اصلی</asp:ListItem>
                    <asp:ListItem Value="2">صفحه داخلی</asp:ListItem>
                    <asp:ListItem Value="3">صفحه اصلی پنل</asp:ListItem>
                    <asp:ListItem Value="4">صفحه داخلی پنل</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-md-4">
            <div class="form-group">
                <label for="ddlPortal">پرتال</label>
                <asp:DropDownList ID="ddlPortal" runat="server" ClientIDMode="Static" CssClass="form-control select2"></asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="btn-group">
                <Nik:NikLinkButton ID="breset" runat="server" CssClass="btn btn-default btn-back" Text="همه" OnClick="breset_Click" SettingValue="ResetButton"></Nik:NikLinkButton>
                <Nik:NikButton ID="btnSearch" runat="server" Text="جستجو" OnClick="btnSearch_Click" CssClass="btn btn-default btn-search" ClientIDMode="Static" SettingValue="SearchButton" />
            </div>
        </div>
        <div class="col-md-6">
            <div class="input-group">
                <label class="input-group-addon" for="txtCount">تعداد:</label>
                <asp:TextBox ClientIDMode="Static" ID="txtCount" runat="server" ForeColor="Red" Enabled="false" CssClass="countlabel form-control"></asp:TextBox>
            </div>
        </div>
    </div>
</Nik:ModalSearch>
<div class="row">
    <div class="col-md-12 table-responsive">
        <Nik:NikGridView ID="GV1" runat="server" ItemType="NikSoft.NikModel.Template" OnRowCommand="GV1_RowCommand">
            <EmptyDataTemplate>
                <div class="noinfo">هیچ آتمی وجود ندارد</div>
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
                <asp:TemplateField HeaderText="پرتال">
                    <ItemTemplate>
                        <%# Item.Portal.Title %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="نوع">
                    <ItemTemplate>
                        <%# GetPageTemplateType(Item.Type) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="فعال">
                    <ItemTemplate>
                        <div class="text-center" style="margin-top: 15px;">
                            <Nik:NikConfirmLinkButton CssClass='<%# Item.IsSelected ? "btn btn-success btn-sm" : "btn btn-danger btn-sm" %>' Enabled='<%# Item.IsSelected ? false: true %>' ID="clenabled" runat="server" CommandArgument='<%# Item.ID %>' MessageText="Are you ready?" CommandName="pubunPub">
							    <%# NikSoft.Utilities.Utilities.GetEnabledImage(Item.IsSelected) %>
                            </Nik:NikConfirmLinkButton>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="‌ویجت" ItemStyle-Width="12%">
                    <ItemTemplate>
                        <div class="btn-group-vertical">
                            <Nik:NikHyperLink runat="server" CssClass="btn btn-default btn-sm btn-edite" NavigateUrl='<%# ResolveUrl("~/panel/TemplateWidgets/" + Item.ID) %>'>ویجت ها</Nik:NikHyperLink>
                            <Nik:NikConfirmLinkButton runat="server" CommandArgument='<%# Item.ID %>' CommandName="removeall" CssClass="btn btn-default btn-sm  btn-remove" Text="حذف ویجت ها" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="تنظیمات" ItemStyle-Width="12%">
                    <ItemTemplate>
                        <div class="btn-group-vertical">
                            <Nik:NikHyperLink runat="server" CssClass="btn btn-default btn-sm btn-search" NavigateUrl='<%# ResolveUrl("~/?templateID=" + Item.ID) %>' Target="_blank">مشاهده</Nik:NikHyperLink>
                            <Nik:NikConfirmLinkButton runat="server" CommandArgument='<%# Item.ID %>' CommandName="copyto" CssClass="btn btn-default btn-sm btn-edite" Text="کپی گرفتن" MessageText="آیا اطمینان دارید؟" />
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
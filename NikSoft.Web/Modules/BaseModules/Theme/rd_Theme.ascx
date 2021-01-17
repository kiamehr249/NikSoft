<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rd_Theme.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Theme.rd_Theme" %>
<Nik:ModalSearch runat="server">
    <div class="row">
        <div class="col-md-6">
            <div class="form-check">
                <asp:CheckBox ID="chbAllPortals" CssClass="nik-checkbox" Checked="false" runat="server" ClientIDMode="Static" />
                <label for="chbAllPortals" class="lable-control">مشاهده همه؟</label>
            </div>
        </div>
        <div class="col-md-6 text-left">
            <asp:Button runat="server" ID="btnSearch" Text="جستجو" CssClass="btn btn-default btn-sm btn-search" OnClick="btnSearch_Click" />
        </div>
    </div>
</Nik:ModalSearch>

<div class="row">
    <div class="col-md-12 table-responsive">
        <Nik:NikGridView ID="GV1" runat="server" ItemType="NikSoft.NikModel.Theme" OnDataBound="GV1_DataBound">
            <EmptyDataTemplate>
                <div class="noinfo">هیچ موردی وجود ندارد.</div>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="ردیف" ItemStyle-Width="2%">
                    <ItemTemplate>
                        <Nik:ListCounter runat="server" IndexFormat="{0}."></Nik:ListCounter>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="انتخاب" ItemStyle-Width="2%">
                    <ItemTemplate>
                        <input type="checkbox" name='<%# Item.PortalID == PortalUser.PortalID ? "ch1" : "ndo" %>' value='<%# Item.ID %>' style='<%# Item.PortalID == PortalUser.PortalID ? "": "display:none;" %>'>
                        <asp:HiddenField ID="hf_iId" runat="server" Value='<%# Item.PortalID %>' />
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
                        <a class="fancybox-thumbs" data-fancybox-group="thumb" target="_blank" href='<%# ResolveClientUrl("~/" +  Item.ThemeImg) %>'>
                            <asp:Image ID="im3" ToolTip='<%# Item.Title %>' ImageUrl='<%# "/" + Item.ThemeImg %>' runat="server" Style="max-height: 120px;" />
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="تنظیمات">
                    <ItemTemplate>
                        <div class="btn-group-vertical">
                            <a href='<%# ResolveUrl("~/panel/ThemeCreator/" + Item.ID) %>' class="btn btn-default btn-edite btn-sm">آیتم ها</a>
                            <a href='<%# ResolveUrl("~/panel/CopyThemepart/" + Item.ID) %>' class="btn btn-default btn-search btn-sm">کپی گرفتن</a>
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

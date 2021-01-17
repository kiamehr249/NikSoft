<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rd_Module.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.ModuleEdit.rd_Module" %>
<Nik:ModalSearch runat="server">
    <div class="row">
        <div class="col-md-4">
            <div class="form-group">
                <label class="control-label" for="txtTitle">عنوان</label>
                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-4">
            <div class="form-group">
                <label class="control-label" for="TxtModuleKey">کلید ماژول</label>
                <asp:TextBox ID="TxtModuleKey" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-4">
            <div class="form-group">
                <label class="control-label" for="ddlGroup">دسته بندی</label>
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

<div class="row">
    <div class="col-md-12 table-responsive">
        <Nik:NikGridView ID="GV1" PageSize="15" runat="server" ItemType="NikSoft.NikModel.NikModule">
            <EmptyDataTemplate>
                <div class="noinfo">هیچ موردی وجود ندارد.</div>
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
                <asp:TemplateField HeaderText="کلید">
                    <ItemTemplate>
                        <%# Item.ModuleKey %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="دسته">
                    <ItemTemplate>
                        <%# Item.NikModuleDefinition.Title %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="مسیر">
                    <ItemTemplate>
                        <%# Item.ModuleFile %>
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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PortalDomain.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Portal.PortalDomain" %>
<div class="row">
    <div class="col-sm-4">
        <h4>پرتال: <%= portalTitle %></h4>
    </div>
</div>
<div class="row">
    <div class="col-sm-6">
        <div class="form-group">
            <label for="txtFirstName" class="control-label">بدون www.</label>
            <Nik:NikTextBox ID="txtDomainAddress" runat="server" placeholder="دامنه" data-validation="length" data-validation-length="5-30" CssClass="form-control" />
        </div>
    </div>
    <div class="col-sm-6">
        <div class="form-group">
            <label for="TxtDesc" class="control-label">توضیحات</label>
            <Nik:NikTextBox ID="TxtDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="متن توضیحات"></Nik:NikTextBox>          
        </div>
    </div>
</div>
<hr />
<div class="row">
    <div class="col-sm-12 text-center">
        <div class="form-group">
            <Nik:NikButton ID="btnSave" runat="server" Text="ذخیره" CssClass="btn btn-default btn-save-nik" ClientIDMode="Static" OnClick="btnSave_Click" SettingValue="SaveButton" />
        </div>
    </div>
</div>
<hr />

<div class="row">
    <div class="col-md-12">
        <Nik:NikGridView ID="GV1" runat="server" ItemType="NikSoft.NikModel.PortalAddress">
            <EmptyDataTemplate>
                <div class="noinfo">هیچ آیتمی وجود ندارد</div>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="ردیف" ItemStyle-Width="2%">
                    <ItemTemplate>
                        <Nik:ListCounter ID="lc1" runat="server" IndexFormat="{0}."></Nik:ListCounter>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                    <ItemTemplate>
                        <input type="checkbox" name='ch1' value='<%# Eval("ID") %>'>
                    </ItemTemplate>
                    <HeaderTemplate>
                        <input type="checkbox" id="chan" title="همه">
                    </HeaderTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="دامنه">
                    <ItemTemplate>
                        <a href="<%# "http://" +  Item.DomainAddress %>" target="_blank"><%# Item.DomainAddress %></a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </Nik:NikGridView>
    </div>
</div>
<div class="row">
    <div class="col-md-12">
        <Nik:NikButton ID="bd" runat="server" CssClass="btn btn-default btn-remove" Text="حذف" OnClick="bd_Click" ClientIDMode="Static" />
        <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default btn-back" Text="بازگشت" OnClick="btnBack_Click" />
    </div>
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rd_Menu.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Menu.rd_Menu" %>
<Nik:ModalSearch runat="server">
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="control-label" for="txtTitle">عنوان</label>
                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="control-label" for="ddlParent">منوی بالاتر</label>
                <asp:DropDownList ID="ddlParent" runat="server" CssClass="form-control select2" ClientIDMode="Static"></asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <div class="btn-group">
                    <Nik:NikLinkButton ID="breset" runat="server" Text="همه" OnClick="breset_Click" CssClass="btn btn-default btn-sm" SettingValue="ResetButton">همه</Nik:NikLinkButton>
                    <Nik:NikButton ID="btnSearch" runat="server" Text="جستجو" OnClick="btnSearch_Click" CssClass="btn btn-default btn-sm" ClientIDMode="Static" SettingValue="SearchButton" />
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
        <Nik:NikGridView ID="GV1" PageSize="15" runat="server" ItemType="NikSoft.NikModel.NikMenu" OnRowCommand="GV1_RowCommand">
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
                <asp:TemplateField HeaderText="منوی بالاتر">
                    <ItemTemplate>
                        <%# Item.ParentID != null ? Item.Parent.Title : string.Empty %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="فعال">
                    <ItemTemplate>
                        <div class="text-center">
                            <Nik:NikConfirmLinkButton CssClass='<%# Item.Enabled ? "btn btn-enable btn-sm" : "btn btn-disable btn-sm" %>' ID="clenabled" runat="server" CommandArgument='<%# Item.ID %>' MessageText="آیا از تغییر اطمینان دارید؟" CommandName="enabled">
							    <%# NikSoft.Utilities.Utilities.GetEnabledImage(Item.Enabled) %>
                            </Nik:NikConfirmLinkButton>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="مشاهده">
                    <ItemTemplate>
                        <%# NikSoft.Utilities.Utilities.GetEnabledImage(Item.ShowInPanel) %>
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
    <div class="col-md-12 text-center">
        <asp:PlaceHolder ID="pl1" runat="server" EnableViewState="False" ViewStateMode="Disabled"></asp:PlaceHolder>
    </div>
</div>

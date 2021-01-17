<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContentAccess.ascx.cs" Inherits="NikSoft.ContentManager.Web.Panel.ContentAccess" %>
<div class="row">
    <div class="col-sm-4">
        <div class="form-group">
            <label class="control-label" for="txtTitle">کاربر</label>
            <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-control select2"></asp:DropDownList>
        </div>
    </div>
    <div class="col-sm-4">
        <div class="form-group">
            <label class="control-label" for="txtTitle">گروه محتوا</label>
            <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-control select2" ClientIDMode="Static"></asp:DropDownList>
        </div>
    </div>
    <div class="col-sm-4">
        <div class="form-group">
            <label class="control-label" for="txtTitle">دسته محتوا</label>
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control select2" ClientIDMode="Static"></asp:DropDownList>
        </div>
    </div>
</div>
<hr />
<div class="row">
    <div class="col-sm-12 text-center">
        <asp:Button ID="btnSave" CssClass="btn btn-save-nik" runat="server" Text="ذخیره" OnClick="btnSave_Click" />
    </div>
</div>
<hr />
<div class="page-header">
    <h4>گروه های محتوایی</h4>
</div>
<div class="row">
    <div class="col-md-12 table-responsive">
        <Nik:NikGridView ID="GV1" PageSize="15" runat="server" ItemType="NikSoft.ContentManager.Service.GroupPermission" OnRowCommand="GV1_RowCommand">
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
                <asp:TemplateField HeaderText="نام کاربر">
                    <ItemTemplate>
                        <%# GetUserName(Item.UserID) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="گروه محتوایی">
                    <ItemTemplate>
                        <%# Item.ContentGroup.Title %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="جزئیات">
                    <ItemTemplate>
                        <p>ساخته شده توسط: <%# GetUserName(Item.CreatorID) %></p>
                        <p>تاریخ ساخت: <%# Item.CreateDate %></p>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="تنظیمات">
                    <ItemTemplate>
                        <div class="form-group text-center">
                            <div class="btn-group-vertical">
                                <Nik:NikConfirmLinkButton CssClass="btn btn-delete btn-sm" ID="DeleteItem" runat="server" CommandArgument='<%# Item.ID %>' MessageText="آیا اطمینان دارید؟" CommandName="DeleteMe">
                                    <i class="fa fa-trash-o" aria-hidden="true"></i> حذف
                                </Nik:NikConfirmLinkButton>
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
<hr />
<div class="page-header">
    <h4>دسته های محتوایی</h4>
</div>
<div class="row">
    <div class="col-md-12 table-responsive">
        <Nik:NikGridView ID="GV2" PageSize="15" runat="server" ItemType="NikSoft.ContentManager.Service.CategoryPermission" OnRowCommand="GV2_RowCommand">
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
                <asp:TemplateField HeaderText="نام کاربر">
                    <ItemTemplate>
                        <%# GetUserName(Item.UserID) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="دسته های محتوا">
                    <ItemTemplate>
                        <%# Item.ContentCategory.Title %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="جزئیات">
                    <ItemTemplate>
                        <p>ساخته شده توسط: <%# GetUserName(Item.CreatorID) %></p>
                        <p>تاریخ ساخت: <%# Item.CreateDate %></p>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="تنظیمات">
                    <ItemTemplate>
                         <div class="form-group text-center">
                            <div class="btn-group-vertical">
                                <Nik:NikConfirmLinkButton CssClass="btn btn-delete btn-sm" ID="DeleteItem" runat="server" CommandArgument='<%# Item.ID %>' MessageText="آیا اطمینان دارید؟" CommandName="DeleteMe">
                                    <i class="fa fa-trash-o" aria-hidden="true"></i> حذف
                                </Nik:NikConfirmLinkButton>
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
        <asp:PlaceHolder ID="pl2" runat="server" EnableViewState="False" ViewStateMode="Disabled"></asp:PlaceHolder>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        FirstOnChange('#ddlGroup', '#ddlCategory', GetContentCategory2);
		<%= SelectData1 %>

        function GetContentCategory2(gID, jID) {
            var ddlGroups = $('#ddlCategory');
            var data = "{'GroupID':'" + gID + "'}";
            var url = '<%=ResolveUrl("../WebService/ContentWebService.asmx/GetContentCategory2") %>';
            CallWebServices(url, data, ddlGroups, jID);
        }
    });
</script>

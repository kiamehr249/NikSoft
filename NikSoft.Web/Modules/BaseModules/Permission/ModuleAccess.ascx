<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleAccess.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Permission.ModuleAccess" %>
<div class="row">
    <div class="col-sm-6">
        <div class="form-group">
            <div class="input-group">
                <label for="DDLUserTypeCat" class="input-group-addon">گروه کاربری</label>
                <asp:DropDownList ID="DDLUserTypeCat" AutoPostBack="true" CssClass="form-control select2" runat="server" OnSelectedIndexChanged="DDLUserTypeCat_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="col-sm-6">
        <div class="form-group">
            <div class="input-group">
                <label for="DDLModulsCategory" class="input-group-addon">دسته ماژول</label>
                <asp:DropDownList ID="DDLModulsCategory" AutoPostBack="true" CssClass="form-control select2" runat="server" OnSelectedIndexChanged="DDLModulsCategory_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-sm-12">
        <div class="page-header">
            <h4>موارد منع دسترسی</h4>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-sm-12">
        <div class="form-group text-center">
            <asp:Button ID="BtnAddAccess" CssClass="btn btn-default btn-save-nik" runat="server" Text="دادن دسترسی" OnClick="BtnAddAccess_Click" />
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-12 table-responsive">
        <Nik:NikGridView ID="GVDeAccess" AutoGenerateColumns="false" CssClass="table table-bordered table-striped table-hover" runat="server" ItemType="NikSoft.NikModel.NikModule">
            <EmptyDataTemplate>
                <div class="noinfo">هیج موردی موجود نیست!</div>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="Row" ItemStyle-Width="2%">
                    <ItemTemplate>
                        <Nik:ListCounter ID="ListCounter2" runat="server" IndexFormat="{0}."></Nik:ListCounter>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                    <ItemTemplate>
                        <input type="checkbox" name='chAcc' value='<%# Item.ID %>'>
                    </ItemTemplate>
                    <HeaderTemplate>
                        <input type="checkbox" id="chAllA" title="همه">
                    </HeaderTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="عنوان">
                    <ItemTemplate>
                        <%# Item.Title %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="دسته ماژول">
                    <ItemTemplate>
                        <%# Item.NikModuleDefinition.Title != null ? Item.NikModuleDefinition.Title : string.Empty %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </Nik:NikGridView>
    </div>
</div>
<br />
<div class="row">
    <div class="col-sm-12">
        <div class="page-header">
            <h4>موارد داری دسترسی</h4>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-sm-12">
        <div class="form-group text-center">
            <asp:Button ID="BtnRemoveAccess" runat="server" CssClass="btn btn-default btn-remove" Text="گرفتن دسترسی" OnClick="BtnRemoveAccess_Click" />
        </div>
    </div>
</div>
<br />
<div class="row">
    <div class="col-sm-12">
        <Nik:NikGridView ID="GVAccess" AutoGenerateColumns="false" ItemType="NikSoft.NikModel.UserRoleModule" CssClass="table table-bordered table-striped table-hover" runat="server">
            <Columns>
                <asp:TemplateField HeaderText="Row" ItemStyle-Width="2%">
                    <ItemTemplate>
                        <Nik:ListCounter ID="ListCounter2" runat="server" IndexFormat="{0}."></Nik:ListCounter>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                    <ItemTemplate>
                        <input type="checkbox" name='chDAcc' value='<%# Item.NikModuleID %>'>
                    </ItemTemplate>
                    <HeaderTemplate>
                        <input type="checkbox" id="chAllDa" title="همه">
                    </HeaderTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="عنوان">
                    <ItemTemplate>
                        <%# Item.NikModule.Title %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="دسته ماژول">
                    <ItemTemplate>
                        <%# Item.NikModule.NikModuleDefinition.Title %>
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

<script type="text/javascript">
    $(document).ready(function () {

        $("input[id*=chAllA]").click(function () {
            var id = $(this).attr('id').substring(6);
            $('input[name="chAcc' + id + '"]').not("input[id*=chAllA]").prop("checked", $(this).prop('checked'));
            deletebuttonchecker("chAcc", "chAllA");
        });

        $('input[name*="chAcc"]').click(function () {
            deletebuttonchecker("chAcc", "chAllA");
        });

        $("input[id*=chAllDa]").click(function () {
            var id = $(this).attr('id').substring(7);
            $('input[name="chDAcc' + id + '"]').not("input[id*=chAllDa]").prop("checked", $(this).prop('checked'));
            deletebuttonchecker("chDAcc", "chAllDa");
        });

        $('input[name*="chDAcc"]').click(function () {
            deletebuttonchecker("chDAcc", "chAllDa");
        });
    });
</script>
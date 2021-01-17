<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ThemeCreator.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Theme.ThemeCreator" %>
<div class="row">
    <div class="col-md-12 table-responsive">
        <fieldset>
            <legend>پوسته</legend>
            <Nik:NikGridView ID="GVSkin" runat="server" ItemType="NikSoft.UILayer.NikSkinTemplate" OnRowCommand="GV_RowCommand">
                <EmptyDataTemplate>
                    <div class="noinfo">هیچ آیتمی وجود تدارد.</div>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="ردیف" ItemStyle-Width="2%">
                        <ItemTemplate>
                            <Nik:ListCounter ID="ListCounter2" runat="server" IndexFormat="{0}."></Nik:ListCounter>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="عنوان">
                        <ItemTemplate>
                            <%# GetSkinTitle(Item) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="فایل">
                        <ItemTemplate>
                            <%# System.IO.Path.GetFileNameWithoutExtension(Item.AppRelativeVirtualPath) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href='<%# ResolveUrl("~/panel/EditTheme/" + themeID + "?t=1&f=" + System.IO.Path.GetFileNameWithoutExtension(Item.AppRelativeVirtualPath)) %>' class="btn btn-default btn-sm btn-edite">ویرایش</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Nik:NikGridView>
        </fieldset>
    </div>
</div>
<div class="row">
    <div class="col-md-12 table-responsive">
        <fieldset>
            <legend>قاب</legend>
            <Nik:NikGridView ID="GVBlock" runat="server" ItemType="NikSoft.UILayer.BlockTemplate" OnRowCommand="GV_RowCommand">
                <EmptyDataTemplate>
                    <div class="noinfo">هیچ آیتمی وجود ندارد.</div>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="ردیف" ItemStyle-Width="2%">
                        <ItemTemplate>
                            <Nik:ListCounter ID="ListCounter1" runat="server" IndexFormat="{0}."></Nik:ListCounter>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="عنوان">
                        <ItemTemplate>
                            <%# GetSkinTitle(Item) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="فایل">
                        <ItemTemplate>
                            <%# System.IO.Path.GetFileNameWithoutExtension(Item.AppRelativeVirtualPath) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href='<%# ResolveUrl("~/panel/EditTheme/" + themeID + "?t=2&f=" + System.IO.Path.GetFileNameWithoutExtension(Item.AppRelativeVirtualPath)) %>' class="btn btn-default btn-sm btn-edite">ویرایش</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Nik:NikGridView>
        </fieldset>
    </div>
</div>
<div class="row">
    <div class="col-md-12 table-responsive">
        <fieldset>
            <legend>CSS</legend>
            <Nik:NikGridView ID="GVCss" runat="server" ItemType="System.IO.FileInfo" OnRowCommand="GV_RowCommand">
                <EmptyDataTemplate>
                    <div class="noinfo">هیچ آیتمی وجود ندارد.</div>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="ردیف" ItemStyle-Width="2%">
                        <ItemTemplate>
                            <Nik:ListCounter ID="ListCounter3" runat="server" IndexFormat="{0}."></Nik:ListCounter>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="عنوان">
                        <ItemTemplate>
                            <%# Item.Name %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href='<%# ResolveUrl("~/panel/EditTheme/" + themeID + "?t=3&f=" + System.IO.Path.GetFileNameWithoutExtension(Item.FullName)) %>' class="btn btn-default btn-sm btn-edite">ویرایش</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Nik:NikGridView>
        </fieldset>
    </div>
</div>


<div class="row">
    <div class="col-md-12 table-responsive">
        <fieldset>
            <legend>Java Script</legend>
            <Nik:NikGridView ID="GvJS" runat="server" ItemType="System.IO.FileInfo" OnRowCommand="GV_RowCommand">
                <EmptyDataTemplate>
                    <div class="noinfo">هیچ آیتمی وجود ندارد.</div>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="ردیف" ItemStyle-Width="2%">
                        <ItemTemplate>
                            <Nik:ListCounter ID="ListCounter4" runat="server" IndexFormat="{0}."></Nik:ListCounter>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="عنوان">
                        <ItemTemplate>
                            <%# Item.Name %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href='<%# ResolveUrl("~/Panel/EditTheme/" + themeID + "?t=4&f=" + System.IO.Path.GetFileNameWithoutExtension(Item.FullName)) %>' class="btn btn-default btn-sm btn-edite">ویرایش</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Nik:NikGridView>
        </fieldset>
    </div>
</div>

<div id="newFilePanel" runat="server">
    <div class="row">
        <label class="checkbox-inline">
            <asp:RadioButton ID="rbtUploadFile" runat="server" class="rbtfile" GroupName="file" Checked="true" />
            فایل موجود
        </label>
        <label class="checkbox-inline">
            <asp:RadioButton ID="rbtCreateFile" runat="server" class="rbtfile" GroupName="file" />
            ساخت فایل
        </label>
        <div class="col-md-12">
            <div class="row" id="fileuploadcontainer">
                <div class="col-md-12">
                    <br />
                    <div class="form-group">
                        <label for="fuSkin" class="btn btn-default btn-file-nik">
                            <asp:FileUpload ID="fuSkin" runat="server" CssClass="hidden" />
                            انتخاب فایل
                        </label>
                    </div>
                </div>
            </div>

            <div class="row" id="filecreatecontainer">
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="txtTitle">عنوان</label>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="txtName">نام انگلیسی</label>
                        <asp:TextBox ID="txtFileName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="ddlFileType">نوع فایل</label>
                        <asp:DropDownList ID="ddlFileType" runat="server" CssClass="form-control notsearchable">
                            <asp:ListItem Value="0">انتخاب کنید</asp:ListItem>
                            <asp:ListItem Value="1">پوسته</asp:ListItem>
                            <asp:ListItem Value="2">فاب</asp:ListItem>
                            <asp:ListItem Value="3">CSS</asp:ListItem>
                            <asp:ListItem Value="4">Java Script</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <hr />
            <div class="form-group text-center">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-default btn-save-nik" Text="ذخیره" OnClick="btnSave_Click" />
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        CheckFileRbt();
        $('.rbtfile').change(CheckFileRbt);
    });

    function CheckFileRbt() {
        if ($('#<%= rbtUploadFile.ClientID %>').is(':checked')) {
            $('#fileuploadcontainer').show();
            $('#filecreatecontainer').hide();
        }
        else if ($('#<%= rbtCreateFile.ClientID %>').is(':checked')) {
            $('#fileuploadcontainer').hide();
            $('#filecreatecontainer').show();
        }
    }
</script>

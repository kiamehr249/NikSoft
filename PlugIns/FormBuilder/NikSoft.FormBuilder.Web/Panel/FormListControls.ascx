<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormListControls.ascx.cs" Inherits="NikSoft.FormBuilder.Web.Panel.FormListControls" %>
<style>
    .out {
        display: none;
    }
</style>
<div class="back-to-form">
    <nav class="navbar navbar-inverse">
        <ul class="nav navbar-nav">
            <li class="active"><a href="<%= "/" + Level + "/rd_forms"  %>">بازگشت</a></li>
            <li class="dropdown">
                <a class="dropdown-toggle" data-toggle="dropdown" href="#">سایر کنترل ها
                    <span class="caret"></span></a>
                <ul class="dropdown-menu">
                    <li><a href="<%= "/" + Level + "/TextBoxControl/" + thisForm.ID %>">کادر متنی</a></li>
                    <li><a href="<%= "/" + Level + "/FormListControls/" + thisForm.ID %>">لیست کنترل ها</a></li>
                    <li><a href="<%= "/" + Level + "/CheckBoxControl/" + thisForm.ID %>">گزینه انتخابی</a></li>
                    <li><a href="<%= "/" + Level + "/FileUploadControl/" + thisForm.ID %>">فایل</a></li>
                </ul>
            </li>
        </ul>
    </nav>
</div>
<div class="page-header">
    <h3 class="text-center">لیست کنترل ها</h3>
    <p><%= thisForm.Title %></p>
</div>
<asp:HiddenField ID="HfEdit" Value="0" runat="server" />
<div class="row">
    <div class="col-sm-3">
        <div class="form-group">
            <label class="control-label" for="ddlControlType">نوع کنترل</label>
            <asp:DropDownList ID="ddlControlType" runat="server" CssClass="form-control select2" ClientIDMode="Static">
                <asp:ListItem Value="0">انتخاب کنید</asp:ListItem>
                <asp:ListItem Value="1">Drop Down List</asp:ListItem>
                <asp:ListItem Value="2">Check Box List</asp:ListItem>
                <asp:ListItem Value="3">Radio Button List</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-sm-3 dropdown out">
        <div class="form-group">
            <label class="control-label" for="ddlParents">لیست بالاتر</label>
            <asp:DropDownList ID="ddlParent" runat="server" CssClass="form-control select2" ClientIDMode="Static">
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-md-3">
        <div class="checkbox">
            <div class="label-null"></div>
            <label>
                <asp:CheckBox ID="chbIsNullable" runat="server" ClientIDMode="Static" />
                پرکردن آن ضروری می باشد؟
            </label>
        </div>
    </div>
    <div class="col-sm-12">
        <div class="form-group">
            <label class="control-label" for="TxtTitle">عنوان</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtTitle" runat="server" CssClass="form-control"></Nik:NikTextBox>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-sm-3">
        <div class="form-group">
            <label class="control-label" for="TxtClass">Class</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtClass" runat="server" CssClass="form-control"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-3">
        <div class="form-group">
            <label class="control-label" for="TxtIdentityValue">ID</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtIdentityValue" runat="server" CssClass="form-control"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-3">
        <div class="form-group">
            <label class="control-label" for="TxtKeyWord">کلید واژه انگلیسی</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtKeyWord" runat="server" CssClass="form-control"></Nik:NikTextBox>
        </div>
    </div>
    <div class="col-sm-3">
        <div class="form-group">
            <label class="control-label" for="TxtPosition">شماره جایگاه</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtPosition" runat="server" CssClass="form-control"></Nik:NikTextBox>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-sm-12">
        <div class="form-group">
            <label class="control-label" for="TxtPosition">پیام خطا</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtMessage" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></Nik:NikTextBox>
        </div>
    </div>
</div>
<br />
<div class="text-center">
    <asp:Button ID="btnSave" CssClass="btn btn-default btn-create" runat="server" Text="ذخیره" OnClick="btnSave_Click" />
</div>

<br />
<div class="text-box-list">
    <div class="page-header">
        <h4>لیست کنترل ها</h4>
    </div>
    <div class="row">
        <div class="col-md-12 table-responsive">
            <Nik:NikGridView ID="GV1" PageSize="15" runat="server" ItemType="NikSoft.FormBuilder.Service.ListControlModel" OnRowCommand="GV1_RowCommand">
                <EmptyDataTemplate>
                    <div class="noinfo">هیچ موردی وجود ندارد.</div>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="ردیف" ItemStyle-Width="2%">
                        <ItemTemplate>
                            <Nik:ListCounter ID="ListCounter2" runat="server" IndexFormat="{0}."></Nik:ListCounter>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="نوع">
                        <ItemTemplate>
                            <%# GetControlType(Item.ControlType) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="عنوان">
                        <ItemTemplate>
                            <%# Item.Title %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ضروری">
                        <ItemTemplate>
                            <%# Item.IsNullable ? "ضروری" : "غیر ضروری" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="پیام خطا">
                        <ItemTemplate>
                            <%# Item.Message %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="آیتم ها" ItemStyle-Width="5%">
                        <ItemTemplate>
                            <div class="text-center">
                                <div class="btn-group">
                                    <a href="<%# "/" + Level + "/FormListControlItems/" + Item.ID  %>" class="btn btn-primary btn-sm">آیتم های لیست</a>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="عملیات" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <div class="text-center">
                                <div class="btn-group">
                                    <asp:LinkButton CssClass="btn btn-edite btn-sm" runat="server" ID="editebtn" CommandArgument='<%# Eval("ID") %>' CommandName="editme">
								    <span class="glyphicon glyphicon-pencil"></span>
                                    </asp:LinkButton>
                                    <Nik:NikConfirmLinkButton CssClass="btn btn-danger btn-sm" ID="deletebtn" runat="server" CommandArgument='<%# Item.ID %>' MessageText="Are you ready?" CommandName="deleteme">
							       <span class="glyphicon glyphicon-remove"></span>
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
</div>


<script type="text/javascript">
    $(document).ready(function () {
        //FirstOnChange('#ddlParentList', '#ddlParentItems', getListItems);
		//<%= SelectData1 %>

        initFirstLoad();

        $('#ddlControlType').on('change', function () {
            var num = $("#ddlControlType option:selected").val();
            showRelativeControls({
                current: num,
                trusteds: ['1'],
                cls: ['.dropdown']
            });
        });

    });

    function getListItems(gID, jID) {
        var ddlparent = $('#ddlParentItems');
        var data = "{'listControlId' : '" + gID + "'}";
        var url = '<%=ResolveUrl("../Api/FormBuilderApi.asmx/GetControlListItems") %>';
        CallWebServices(url, data, ddlparent, jID);
    }

    function showRelativeControls(objs) {
        if (objs.trusteds.includes(objs.current)) {
            $.each(objs.cls, function (id, val) {
                showControls(val);
            });

        } else {
            $.each(objs.cls, function (id, val) {
                hideControls(val);
            });
        }
    }

    function showControls(cls) {
        $(cls).removeClass('out');
    }

    function hideControls(cls) {
        $(cls).addClass('out');
    }


    function initFirstLoad() {
        var num = '<%= ControlType %>';
        showRelativeControls({
            current: num,
            trusteds: ['1'],
            cls: ['.dropdown']
        });
    }

</script>

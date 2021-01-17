<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormComponents.ascx.cs" Inherits="NikSoft.FormBuilder.Web.Panel.FormComponents" %>
<style>
    .out {
        display: none;
    }
</style>

<div class="row">
    <div class="col-sm-3">
        <div class="form-group">
            <label class="control-label" for="ddlControlType">نوع کنترل</label>
            <asp:DropDownList ID="ddlControlType" runat="server" CssClass="form-control select2" ClientIDMode="Static">
                <asp:ListItem Value="0">انتخاب کنید</asp:ListItem>
                <asp:ListItem Value="1">Text Box</asp:ListItem>
                <asp:ListItem Value="2">Text Area</asp:ListItem>
                <asp:ListItem Value="3">Check Box</asp:ListItem>
                <asp:ListItem Value="4">Drop Down List</asp:ListItem>
                <asp:ListItem Value="5">Check Box List</asp:ListItem>
                <asp:ListItem Value="6">Radio Button List</asp:ListItem>
                <asp:ListItem Value="7">File Upload</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-sm-9">
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

<asp:HiddenField ID="HfTextBox" Value="0" runat="server" />
<div class="text-box out">
    <div class="page-header">
        <h4>کادر متنی</h4>
    </div>
    <div class="row">
        <div class="col-sm-3 text-mode-1 out">
            <div class="form-group">
                <label class="control-label" for="ddlDataType">نوع داده</label>
                <asp:DropDownList ID="ddlDataType" runat="server" CssClass="form-control select2" ClientIDMode="Static">
                    <asp:ListItem Value="0">انتخاب کنید</asp:ListItem>
                    <asp:ListItem Value="1">Integer</asp:ListItem>
                    <asp:ListItem Value="2">Boolian</asp:ListItem>
                    <asp:ListItem Value="3">String</asp:ListItem>
                    <asp:ListItem Value="4">Longe</asp:ListItem>
                    <asp:ListItem Value="5">DateTime</asp:ListItem>
                    <asp:ListItem Value="6">Time</asp:ListItem>
                    <asp:ListItem Value="7">Date</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-sm-3 text-mode-1 out">
            <div class="form-group">
                <label class="control-label" for="ddlValueType">مقدار ورودی</label>
                <asp:DropDownList ID="ddlValueType" runat="server" CssClass="form-control select2" ClientIDMode="Static">
                    <asp:ListItem Value="0">انتخاب کنید</asp:ListItem>
                    <asp:ListItem Value="1">عددی</asp:ListItem>
                    <asp:ListItem Value="2">متن</asp:ListItem>
                    <asp:ListItem Value="3">ایمیل</asp:ListItem>
                    <asp:ListItem Value="4">پسورد</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-sm-3">
            <div class="form-group">
                <label class="control-label" for="TxtPlacehoder">مقدار نمونه(Placehoder)</label>
                <Nik:NikTextBox BoxTitle="Title" ID="TxtPlacehoder" runat="server" CssClass="form-control"></Nik:NikTextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3">
            <div class="form-group">
                <label class="control-label" for="TxtLength">طول کارکتر ورودی</label>
                <Nik:NikTextBox BoxTitle="Title" ID="TxtLength" runat="server" CssClass="form-control"></Nik:NikTextBox>
            </div>
        </div>

        <div class="col-sm-3 text-mode-2 out">
            <div class="form-group">
                <label class="control-label" for="TxtRows">تعداد سطر</label>
                <Nik:NikTextBox BoxTitle="Title" ID="TxtRows" runat="server" CssClass="form-control"></Nik:NikTextBox>
            </div>
        </div>
    </div>
    <br />
    <div class="text-center">
        <asp:Button ID="btnSaveTextbox" CssClass="btn btn-default btn-create" runat="server" Text="ذخیره" OnClick="btnSaveTextbox_Click" />
    </div>
</div>



<br />
<div class="text-box-list">
    <div class="page-header">
        <h4>لیست کارهای متنی</h4>
    </div>
    <div class="row">
        <div class="col-md-12 table-responsive">
            <Nik:NikGridView ID="GV1" PageSize="15" runat="server" ItemType="NikSoft.FormBuilder.Service.TextBoxModel" OnRowCommand="GV1_RowCommand">
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
                            <%# Item.TextMode == NikSoft.FormBuilder.Service.TextBoxType.TextBox ? "Text Box" : "Text Area" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="عنوان">
                        <ItemTemplate>
                            <%# Item.Title %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="مقدار ورودی">
                        <ItemTemplate>
                            <%# GetValueType(Item.ValueType) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="پیام خطا">
                        <ItemTemplate>
                            <%# Item.Message %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="عملیات">
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
        initFirstLoad();

        $('#ddlControlType').on('change', function () {
            var num = $("#ddlControlType option:selected").val();
            showRelativeControls({
                current: num,
                trusteds: ['1', '2'],
                cls: ['.text-box']
            });

            showRelativeControls({
                current: num,
                trusteds: ['1'],
                cls: ['.text-mode-1']
            });

            showRelativeControls({
                current: num,
                trusteds: ['2'],
                cls: ['.text-mode-2']
            });
        });

    });


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
            trusteds: ['1', '2'],
            cls: ['.text-box']
        });

        showRelativeControls({
            current: num,
            trusteds: ['1'],
            cls: ['.text-mode-1']
        });

        showRelativeControls({
            current: num,
            trusteds: ['2'],
            cls: ['.text-mode-2']
        });
    }

</script>

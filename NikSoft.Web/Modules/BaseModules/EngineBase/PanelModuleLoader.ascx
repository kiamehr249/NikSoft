<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PanelModuleLoader.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.EngineBase.PanelModuleLoader" %>
<div class="panel panel-default panel-nik">
    <div class="panel-heading">
        <div class="row">
            <div class="col-sm-3 module-title-panel">
                <h4><%= PageHeadingText %></h4>
            </div>
            <div class="col-sm-9" id="options">
                <div class="panel-options text-left">
                    <Nik:NikLinkButton ID="imgBtnList" data-toggle="tooltip" data-placement="bottom" title="View and Delete Items" runat="server" CssClass="btn btn-default btn-sm btn-back-nik" OnClick="imgBtnList_Click1" EnableViewState="false" CausesValidation="False" ClientIDMode="Static">بازگشت</Nik:NikLinkButton>
                    <Nik:NikLinkButton ID="imgBtnNew" data-toggle="tooltip" data-placement="bottom" data-trigger="hover" title="Create new Item" runat="server" CssClass="btn btn-default btn-sm btn-create" OnClick="imgBtnNew_Click" EnableViewState="false" ClientIDMode="Static">ایجاد</Nik:NikLinkButton>
                    <asp:LinkButton runat="server" ID="be" ClientIDMode="Static" CssClass="btn btn-default btn-sm btn-edite" OnClick="btnEdit_Click">ویرایش</asp:LinkButton>
                    <asp:LinkButton runat="server" ID="bd" ClientIDMode="Static" CssClass="btn btn-default btn-sm btn-remove">حذف</asp:LinkButton>
                    <a data-toggle="modal" id="searchbtn" runat="server" clientidmode="Static" data-target="#searchmodal" title="Search Items" class="btn btn-default btn-sm btn-search">جستجو <i class="fa fa-search" aria-hidden="true"></i></a>
                </div>
                <div class="clear"></div>
            </div>
        </div>
    </div>
    <div class="panel-body">
        <asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder>
    </div>
    <div class="panel-footer">
        <div class="text-center">
            <h5>نیک سافت: 1.0.1</h5>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(function () {
        $(".btn-remove").click(function () {
            return confirm('آیا از حذف آیتم/آیتم ها اطمینان دارید؟');
        });
    });
</script>
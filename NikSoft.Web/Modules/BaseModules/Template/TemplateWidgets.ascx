<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplateWidgets.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Template.TemplateWidgets" %>
<%@ Reference Control="~/Modules/BaseModules/Template/PanelWidgetContainer.ascx" %>

<link href="/css/thememanager.css" rel="stylesheet" />
<link href="../../../contents/jquery-ui-1.12.1.custom/jquery-ui.min.css" rel="stylesheet" />
<script src="../../../contents/jquery-ui-1.12.1.custom/jquery-ui.min.js"></script>
<script type="text/javascript">
    $(function () {
        $("[id*=showdetail_]").click(function () {
            var ar = $(this).attr("id").split("_");
            var title = $("#iconwidget" + ar[1]).attr("title");
            var Desc = $("#iconwidget" + ar[1]).attr("data-desc");
            var imgSrc = "../../../contents/jquery-ui-1.12.1.custom/" + $("#iconwidget" + ar[1]).attr("data-img");
            $('#lblTitle').html(title);
            $('#lblDesc').html(Desc);
            $('#imgwidget').attr("src", imgSrc);
        });
    });
</script>

<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="exampleModalLabel">
                    <asp:Label ID="lblTitle" ClientIDMode="Static" runat="server"></asp:Label>
                </h4>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-md-12">
                        <asp:Label ID="lblDesc" runat="server" ClientIDMode="Static"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <img src="/" id="imgwidget" class="img-responsive" />
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="closebtn btn btn-primary" data-dismiss="modal">بستن</button>
            </div>
        </div>
    </div>
</div>

<div class="row">
    <div class="col-sm-3">
    </div>
    <div class="col-md-6 col-sm-6">
        <div class="input-group">
            <span class="input-group-addon" id="basic-addon1">جستجو</span>
            <input id="txtSearchWidget" type="text" maxlength="255" class="form-control" placeholder="جستجوی ویجت" />
            <span class="input-group-addon cursor-pointer btn-remove" id="btnClearSearch">
                <span class="glyphicon glyphicon-remove"></span>
            </span>
        </div>
    </div>
    <div class="col-sm-3">
    </div>
</div>
<hr />
<div class="row">
    <div class="col-md-12">
        <div class="widgetlistcontainer">
            <div class="widgetlist">
                <asp:PlaceHolder ID="plcWidgets" runat="server"></asp:PlaceHolder>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-1"></div>
    <div class="col-md-6">
        <div class="form-group">
            <label class="control-label" for="ddlTemp">کپی از</label>
            <asp:DropDownList ID="ddlTemp" runat="server" ClientIDMode="Static" CssClass="reqfield form-control"></asp:DropDownList>
        </div>
    </div>
    <div class="col-md-3">
        <div class="label-null"></div>
        <asp:Button ID="btnCopy" CssClass="btn btn-defualt btn-save-nik" runat="server" Text="ذخیره" OnClick="btnCopy_Click" />
    </div>
    <div class="col-md-1"></div>
</div>
<asp:UpdatePanel runat="server" ID="upST">
    <ContentTemplate>
        <div class="row">
            <div class="col-md-12 modulemain">
                <asp:PlaceHolder ID="plc" runat="server"></asp:PlaceHolder>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<hr />
<div class="row">
    <div class="col-md-12 text-center">
        <asp:Button ID="btnBack" CssClass="btn btn-back" runat="server" Text="بازگشت" OnClick="btnBack_Click" />
        <a href="#" id="savewidget" class="btn btn-defualt btn-save-nik">ذخیره</a>
    </div>
</div>
<script type="text/javascript">
    var a = 1;
    var b = 0;
    function pageLoad(sender, args) {
        Init();
    }

    function Init() {
        $(".godown,.goup,#btnClearSearch,#txtSearchWidget,#savewidget,.widgetremove").unbind();
        $(".widgetlistcontainer .arrows .list-group-item").hover(
            function () {
                $(this).addClass("active");
            },
            function () {
                $(this).removeClass("active");
            });

        $('#btnClearSearch').click(function () {
            a = 1;
            b = 0;
            $("#txtSearchWidget").val('');
            $("#txtSearchWidget").change();
        });
        var lastValue = new String('');
        $("#txtSearchWidget").on('change keyup paste mouseup', function () {
            if ($.trim($(this).val()) != lastValue) {
                if ($.trim($(this).val()) === '') {
                    $('.widgetlist a').css('display', '');
                    return;
                }
                lastValue = new String($.trim($(this).val()));
                $('.widgetlist a:first').css('margin-top', '');
                $('.widgetlist a').each(function (index, value) {
                    var $val = new String($(value).html());
                    if ($val.toLowerCase().indexOf(lastValue.toLowerCase()) != -1) {
                        $(value).css('display', '');
                    } else {
                        $(value).css('display', 'none');
                    }
                });
            }
        });

        $('.widgetremove').click(function () {
            var wdid = $(this).data('wdid');
            $.ajax({
                type: "POST",
                async: false,
                url: "../../../WebService/PortalWebService.asmx/RemoveWidget",
                data: "{'widgetID':'" + wdid + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (msg) {
                    if (msg.d === 'error') {
                        //show some message to user
                    }
                    var upST = '<%= upST.ClientID %>';
                    __doPostBack(upST, '');
                    return false;
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.responseText);
                    alert(thrownError);
                }
            });
            return false;
        });
        $('#savewidget').click(function () {
            var res = '';
            $('.modulerep[data-mc]').each(function (index) {
                var t = $(this).data("mc");
                res += t + "-";
                var x = $(this).find(".widgetclass").map(function () {
                    return $(this).attr("wid");
                }).get().join(',');
                res += x;
                res += "_";
            });
            $.ajax({
                type: "POST",
                async: false,
                url: "../../../WebService/PortalWebService.asmx/SaveWidgetPosition",
                data: "{'result':'" + res + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (msg) {
                    if (msg.d === 'error') {
                        //show some message to user
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.responseText);
                    alert(thrownError);
                }
            });
            return false;
        });
        $(".modulemain [data-mc]").addClass('modulerep');
        $(".modulerep[data-mc]").sortable({
            connectWith: ".modulerep[data-mc]",
            stop: function (event, ui) {
                if (ui.item.get(0).tagName === 'A') {
                    var wid = ui.item.data('wid');
                    var panelNo = $(this).data('mc');
                    $.ajax({
                        type: "POST",
                        async: false,
                        url: "../../../WebService/PortalWebService.asmx/SaveNewWidget",
                        data: "{'widgetID':'" + wid + "','panelNo':'" + panelNo + "','pageID':'<%= PageID %>'}",
                        contentType: "application/json; charset=utf-8",
                        success: function (msg) {
                            var upST = '<%= upST.ClientID %>';
                            __doPostBack(upST, '');
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.responseText);
                            alert(thrownError);
                        }
                    });
                }
            },
            cursorAt: { top: 10, right: 10 },
            helper: function (event, ui) {
                var helper = ui.clone();
                helper.css({ 'width': '150px' });
                return helper;
            },
            cursor: "move",
            receive: function (event, ui) {
                event.preventDefault();
            }
        });

        $('.widgetlist a').draggable({
            connectToSortable: ".modulerep[data-mc]",
            revert: true,
            cursorAt: { top: 10, right: 10 },
            cursor: "move",
            helper: function (event) {
                var helper = $(event.target).clone();
                helper.css({ 'width': '150px' }, 50);
                return helper;
            },
            revertDuration: 0
        });

        $('span.ui-icon-minusthick,span.ui-icon-plusthick').remove();

        $(".portlet").addClass("ui-widget ui-widget-content ui-helper-clearfix ui-corner-all")
            .find(".portlet-header")
            .addClass("ui-widget-header ui-corner-all")
            .end();
    }
</script>
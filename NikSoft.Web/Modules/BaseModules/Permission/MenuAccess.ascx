<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuAccess.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Permission.MenuAccess" %>
<div class="row">
    <div class="col-sm-6">
        <div class="form-group">
            <asp:DropDownList ID="ddlUserGroups" CssClass="form-control user-group-ddl select2" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlUserGroups_SelectedIndexChanged"></asp:DropDownList>
        </div>
    </div>
    <div class="col-sm-12">
        <div class="row">
            <asp:Repeater ID="RepMains" ItemType="NikSoft.Web.Modules.BaseModules.Permission.MenuModel" runat="server">
                <ItemTemplate>
                    <div class="col-sm-3">
                        <div class="menu-cell">
                            <button type="button" class="main-title btn btn-default form-control" data-toggle="modal" data-target='<%# "#menu" + Item.MenuItem.ID %>'>
                                <%# Item.MenuItem.Title %>
                            </button>
                            <div class="menu-select">
                                <div class="form-group">
                                    <div class="check-nik">
                                        <asp:CheckBox ID="chMenu" CssClass="menu-check" runat="server" />
                                        <span class="chlabl">انتخاب</span>
                                        <div class="item-id hidden">
                                            <asp:HiddenField ID="HidItemID" Value='<%# Item.MenuItem.ID %>' runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id='<%# "menu" + Item.MenuItem.ID %>' class="modal fade" role="dialog">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                                        <h4 class="modal-title"><%# Item.MenuItem.Title %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <asp:Repeater ID="RepChilds" ItemType="NikSoft.Web.Modules.BaseModules.Permission.MenuModel" runat="server">
                                            <ItemTemplate>
                                                <div class="child-cell">
                                                    <div class="check-nik">
                                                        <asp:CheckBox ID="chSubMenu" CssClass="menu-check" runat="server" />
                                                        <span class="chlabl"><%# Item.MenuItem.Title %></span>            
                                                        <div class="item-id hidden">
                                                            <asp:HiddenField ID="HidSubitemID" Value='<%# Item.MenuItem.ID %>' runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">بستن</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $('.menu-check input').change(function () {
            var parentDiv = $($(this).parents('.check-nik')[0]);
            var itemID = parentDiv.find('.item-id input').attr('value');
            var UserGroupID = $('.user-group-ddl').val();
            if (UserGroupID != 0) {
                if ($(this).is(':checked')) {
                    AddMenuAccess(itemID, UserGroupID);
                } else {
                    RemoveMenuAccess(itemID, UserGroupID);
                }
            }
        });
    });

    function AddMenuAccess(MenuID, UserGroupID) {
        var dataParameters = "{MenuID:" + MenuID + ", UserGroupID:" + UserGroupID + "}";
        $.ajax({
            type: "POST",
            async: false,
            url: "../../../WebService/PortalWebService.asmx/AddMenuAccess",
            data: dataParameters,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                if (msg.d === 0) {
                    alert('the menu is accessed before');
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.responseText);
                alert(thrownError);
            }
        });
    }

    function RemoveMenuAccess(MenuID, UserGroupID) {
        var dataParameters = "{MenuID:" + MenuID + ", UserGroupID:" + UserGroupID + "}";
        $.ajax({
            type: "POST",
            async: false,
            url: "../../../WebService/PortalWebService.asmx/RemoveMenuAccess",
            data: dataParameters,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                if (msg.d === 0) {
                    alert('the menu not assessed before');
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.responseText);
                alert(thrownError);
            }
        });
    }
</script>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DBManger.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Permission.DBManger" %>
<style>
    #sqlquery textarea, #sqlquery table {
        direction: ltr !important;
    }
</style>
<div id="sqlquery">
    <asp:PlaceHolder ID="plcSql" runat="server" Visible="false">
        <div class="row">
            <div class="col-sm-12">
                <div class="page-header">
                    <h4>Query خود را بنویسید</h4>
                </div>
            </div>
            <div class="col-sm-12">
                <div id="messageBox" runat="server" class="callout callout-danger" visible="false">
                    <h4><asp:Literal ID="LtrErrors" runat="server"></asp:Literal></h4>
                </div>
            </div>
            <div class="col-md-12">
                <asp:TextBox ID="txt" runat="server" Columns="100" Rows="10" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <hr />
        <div class="row">
            <div class="col-md-12 text-center">
                <asp:Button ID="btn" CssClass="btn btn-default btn-save-nik" runat="server" Text="اجرا" OnClick="btn_Click" />
            </div>
        </div>
        <hr />
        <div class="row">
            <div class="col-sm-12">
                <div class="page-header">
                    <h4>نتایج Query</h4>
                </div>
            </div>
            <div class="col-md-12 table-responsive">
                <asp:DataGrid ID="dl" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive">
                </asp:DataGrid>
            </div>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="plcLoing" runat="server">
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <div class="input-group">
                        <span class="input-group-addon"><span class="glyphicon glyphicon-user"></span></span>
                        <asp:TextBox ID="txtUname" runat="server" ClientIDMode="Static" CssClass="form-control" />
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <div class="input-group">
                        <span class="input-group-addon tooltiper"><span class="glyphicon glyphicon-lock"></span></span>
                        <asp:TextBox ID="txtPass" TextMode="Password" runat="server" ClientIDMode="Static" CssClass="form-control" />
                    </div>
                </div>
            </div>
        </div>
        <hr />
        <div class="row">
            <div class="col-md-12 text-center">
                <asp:Button runat="server" CssClass="btn btn-default btn-save-nik" ID="btnLogin" OnClick="btnLogin_Click" Text="ورود" />
            </div>
        </div>
    </asp:PlaceHolder>
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SystemSettingView.ascx.cs" Inherits="NikSoft.Web.Modules.BaseModules.Setting.SystemSettingView" %>
<asp:Panel runat="server" ID="pan">
    <asp:Panel runat="server" ID="pantxt">
        <div class="setting-header-box" runat="server" id="divint">
            <div class="callout callout-danger">
                <div class="row">
                    <div class="col-md-4">
                        <label class="title-label" runat="server" id="lbl_int_Title"></label>
                    </div>
                    <div class="col-md-3">
                        <label class="">حداقل مجاز</label>
                        <label class="" runat="server" id="lblmin"></label>
                    </div>
                    <div class="col-md-3">
                        <label class="">حداکثر مجاز</label>
                        <label class="" runat="server" id="lblMax"></label>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
			<div class="col-md-12">
				<div class="form-group">
					<label class="control-label" for="txt_Value" runat="server" id="l2">مقدار</label>
					<Nik:NikTextBox ID="txt_Value" runat="server" CssClass="reqfield form-control"></Nik:NikTextBox>
					<Nik:TinyMCE ID="TinyMCE1" CssClass="form-control" ShowEditor="true" runat="server" ToolbarMode="Full" />
				</div>
			</div>
		</div>
    </asp:Panel>
    <asp:Panel runat="server" ID="panchk">
        <div class="row">
            <div class="col-md-12">
                <div class="form-group">
                    <label class="control-label" for="txt_Value" runat="server" id="l1">مقدار</label>
                    <asp:CheckBox runat="server" ID="chkValue" CssClass="reqfield" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlRange">
        <div class="row">
            <div class="col-md-12">
                <label class="label label-success" runat="server" id="lbl_title"></label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="control-label" for="txtStart" runat="server" id="Label1">از</label>
                    <Nik:NikTextBox ID="txtStart" runat="server" CssClass="reqfield form-control"></Nik:NikTextBox>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label class="control-label" for="txtEnd" runat="server" id="Label2">تا</label>
                    <Nik:NikTextBox ID="txtEnd" runat="server" CssClass="reqfield form-control"></Nik:NikTextBox>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPhoto">
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label for="fuPhoto">تصویر</label>
                    <label class="control-label" for="fuPhoto" runat="server" id="Label3">
                        انتخاب تصویر
                        <asp:FileUpload ID="fuPhoto" runat="server" ClientIDMode="Static" />
                    </label>
                    <asp:Image ID="imgPhoto" Width="80" Height="80" runat="server" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <hr />
    <div class="row">
        <div class="col-md-12 text-center">
            <asp:HyperLink ID="HypCancel" runat="server" class="btn btn-back-nik">لغو</asp:HyperLink>
            <Nik:NikButton ID="btnSave" runat="server" Text="ذخیره" OnClick="BtnSave_Click" class="btn btn-default btn-save-nik" SettingValue="SaveButton" />
        </div>
    </div>
    <hr />
</asp:Panel>

<div class="row">
    <div class="col-sm-12 text-right">
        <asp:Button ID="btnRebuildCache" CssClass="btn btn-default btn-remove" runat="server" Text="پاک کردن کش سرور" OnClick="btnRebuildCache_Click" />
        <br />
    </div>
    <div class="col-sm-12">
        <div class="input-group">
            <label class="input-group-addon" for="txtCount">تعداد:</label>
            <asp:TextBox ClientIDMode="Static" ID="txtCount" runat="server" ForeColor="Red" Enabled="false" CssClass="countlabel form-control"></asp:TextBox>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-12">
        <div class="grid table-responsive">
            <Nik:NikGridView ID="GV1" runat="server" OnRowCommand="GV1_RowCommand" ItemType="NikSoft.NikModel.NikSetting">
                <EmptyDataTemplate>
                    هیچ آیتمی وجود ندارد
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="ردیف" ItemStyle-Width="2%" ItemStyle-CssClass="first" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <Nik:ListCounter ID="ListCounter2" runat="server" IndexFormat="{0}."></Nik:ListCounter>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="SettingLabel" HeaderText="برچسب"></asp:BoundField>
                    <asp:TemplateField HeaderText="مقدار">
                        <ItemTemplate>
                            <asp:Literal Text='<%# GetValue(Item) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="مقدار سرور">
                        <ItemTemplate>
                            <asp:Literal Text='<%# GetRealServerSeenValue(Item.SettingName, Item.SettingModule) %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="تنظیمات">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="btnEdit" Text="ویرایش" CommandArgument="<%# Item.ID %>" CssClass="btn btn-default btn-edite"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Nik:NikGridView>
        </div>
    </div>
</div>
<hr />
<div class="row">
    <div class="col-md-12">
        <asp:PlaceHolder ID="pl1" runat="server" EnableViewState="False" ViewStateMode="Disabled"></asp:PlaceHolder>
    </div>
</div>
<asp:HiddenField ID="hid" runat="server" />

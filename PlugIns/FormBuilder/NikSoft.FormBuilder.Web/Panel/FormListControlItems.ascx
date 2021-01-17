﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormListControlItems.ascx.cs" Inherits="NikSoft.FormBuilder.Web.Panel.FormListControlItems" %>
<div class="back-to-form">
    <nav class="navbar navbar-inverse">
        <ul class="nav navbar-nav">
            <li class="active"><a href="<%= "/" + Level + "/FormListControls/" + thisForm.ID  %>">بازگشت</a></li>
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
    <p>فرم: <%= thisForm.Title %></p>
    <p><%= GetControlType(thisListControl.ControlType) + ": " + thisListControl.Title %></p>
</div>
<asp:HiddenField ID="HfEdit" Value="0" runat="server" />
<div class="row">
    <div class="col-sm-3" id="childCell" runat="server" visible="false">
        <div class="form-group">
            <label class="control-label" for="ddlParentItems">آیتم های ارشد</label>
            <asp:DropDownList ID="ddlParentItems" runat="server" CssClass="form-control select2" ClientIDMode="Static">
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-sm-3">
        <div class="form-group">
            <label class="control-label" for="ddlParents">لیست کنترل ها</label>
            <asp:DropDownList ID="ddlControl" runat="server" CssClass="form-control select2" ClientIDMode="Static" Enabled="false">
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-sm-6">
        <div class="form-group">
            <label class="control-label" for="TxtTitle">عنوان</label>
            <Nik:NikTextBox BoxTitle="Title" ID="TxtTitle" runat="server" CssClass="form-control"></Nik:NikTextBox>
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
            <Nik:NikGridView ID="GV1" PageSize="15" runat="server" ItemType="NikSoft.FormBuilder.Service.ListControlItemModel" OnRowCommand="GV1_RowCommand">
                <EmptyDataTemplate>
                    <div class="noinfo">هیچ موردی وجود ندارد.</div>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="ردیف" ItemStyle-Width="2%">
                        <ItemTemplate>
                            <Nik:ListCounter ID="ListCounter2" runat="server" IndexFormat="{0}."></Nik:ListCounter>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="عنوان">
                        <ItemTemplate>
                            <%# Item.Title %>
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
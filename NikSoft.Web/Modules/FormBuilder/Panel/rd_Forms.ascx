<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rd_Forms.ascx.cs" Inherits="NikSoft.FormBuilder.Web.Panel.rd_Forms" %>
<Nik:ModalSearch runat="server">
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="control-label" for="txtTitle">عنوان</label>
                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="control-label" for="ddlParent">فرم ارشد</label>
                <asp:DropDownList ID="ddlParent" runat="server" CssClass="form-control select2" ClientIDMode="Static"></asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <div class="btn-group">
                    <Nik:NikLinkButton ID="breset" runat="server" OnClick="breset_Click" CssClass="btn btn-back btn-sm" SettingValue="ResetButton">همه</Nik:NikLinkButton>
                    <Nik:NikButton ID="btnSearch" runat="server" Text="جستجو" OnClick="btnSearch_Click" CssClass="btn btn-search btn-sm" ClientIDMode="Static" SettingValue="SearchButton" />
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <div class="input-group">
                    <label class="input-group-addon" for="txtCount">تعداد:</label>
                    <asp:TextBox ClientIDMode="Static" ID="txtCount" runat="server" ForeColor="Red" Enabled="false" CssClass="countlabel form-control"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
</Nik:ModalSearch>

<div class="row">
    <div class="col-md-12 table-responsive">
        <Nik:NikGridView ID="GV1" PageSize="15" runat="server" ItemType="NikSoft.FormBuilder.Service.FormModel" OnRowCommand="GV1_RowCommand">
            <EmptyDataTemplate>
                <div class="noinfo">هیچ موردی وجود ندارد.</div>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="ردیف" ItemStyle-Width="2%">
                    <ItemTemplate>
                        <Nik:ListCounter ID="ListCounter2" runat="server" IndexFormat="{0}."></Nik:ListCounter>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                    <ItemTemplate>
                        <input type="checkbox" name='ch1' value='<%# Item.ID %>'>
                    </ItemTemplate>
                    <HeaderTemplate>
                        <input type="checkbox" id="chan" title="همه">
                    </HeaderTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="عنوان">
                    <ItemTemplate>
                        <%# Item.Title %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="توضیحات">
                    <ItemTemplate>
                        <%# Item.Description %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="حریم">
                    <ItemTemplate>
                        <%# Item.LoginRequired ? "خصوصی" : "عمومی" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ثبت IP کاربر">
                    <ItemTemplate>
                        <%# Item.RecordIP ? "بلی" : "خیر" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="پیام ارسال">
                    <ItemTemplate>
                        <%# Item.Message %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="فرم ارشد">
                    <ItemTemplate>
                        <%# Item.ParentID == null ? "ندارد" : "دارد" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="عملیات">
                    <ItemTemplate>
                        <div class="btn-group">
                            <a href="<%# "/panel/FormTemplate/" + Item.ID %>" class="btn btn-default btn-sm btn-edite">تمپلیت</a>
                            <a href="#" class="btn btn-info btn-sm" data-toggle="modal" data-target="<%# "#modal" + Item.ID %>">کنترل ها <span><%# GetItemCount(Item.ID) %></span></a>
                        </div>
                        <div id="<%# "modal" + Item.ID %>" class="modal modal-md fade" role="dialog">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                                        <h4 class="modal-title">کنترل های فرم</h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="row">
                                            <div class="col-sm-3"></div>
                                            <div class="col-sm-6 text-center">
                                                <div class="btn-group-vertical">
                                                    <a href="<%# "/panel/TextBoxControl/" + Item.ID %>" class="btn btn-info">کادرهای متنی</a>
                                                    <a href="<%# "/panel/FormListControls/" + Item.ID %>" class="btn btn-info">لیست کنترل ها</a>
                                                    <a href="<%# "/panel/TextBoxControl/" + Item.ID %>" class="btn btn-info">گزینه انتخابی</a>
                                                    <a href="<%# "/panel/FileUploadControl/" + Item.ID %>" class="btn btn-info">فایل</a>
                                                </div>
                                            </div>
                                            <div class="col-sm-3"></div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="فعال">
                    <ItemTemplate>
                        <div class="text-center">
                            <Nik:NikConfirmLinkButton CssClass='<%# Item.Enabled ? "btn btn-enable btn-sm" : "btn btn-disable btn-sm" %>' ID="clenabled" runat="server" CommandArgument='<%# Item.ID %>' MessageText="Are you ready?" CommandName="enabled">
							    <%# NikSoft.Utilities.Utilities.GetEnabledImage(Item.Enabled) %>
                            </Nik:NikConfirmLinkButton>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="مرتب سازی">
                    <ItemTemplate>
                        <div class="text-center">
                            <div class="btn-group">
                                <asp:LinkButton CssClass="btn btn-edite btn-sm" runat="server" ID="bMUp" CommandArgument='<%# Eval("ID") %>' CommandName="MoveUp">
								    <span class="glyphicon glyphicon-chevron-up"></span>
                                </asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-edite btn-sm" runat="server" ID="bMDown" CommandArgument='<%# Eval("ID") %>' CommandName="MoveDown">			
                                    <span class="glyphicon glyphicon-chevron-down"></span>
                                </asp:LinkButton>
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

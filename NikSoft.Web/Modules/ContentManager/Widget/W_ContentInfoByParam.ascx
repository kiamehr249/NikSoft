<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="W_ContentInfoByParam.ascx.cs" Inherits="NikSoft.ContentManager.Web.Widget.W_ContentInfoByParam" %>

<h4><%= thisGroup != null ? thisGroup.Title : "" %></h4>

<h4><%= thisCat != null ? thisCat.Title : "" %></h4>

<h4><%= thisConetnt != null ? thisConetnt.Title : "" %></h4>
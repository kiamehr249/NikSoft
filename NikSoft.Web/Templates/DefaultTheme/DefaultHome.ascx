﻿<%@ Control Language="C#" Inherits="NikSoft.UILayer.NikSkinTemplate" %>
<Nik:SkinTitle runat="server" Title="DefaultHome"></Nik:SkinTitle>
<div class="header">
    <div class="container">
        <div class="row">
            <div class="col-sm-12" data-mc="1" runat="server">
            </div>			
        </div>
    </div>
</div>
<div class="middel-wrapper">
	<div class="slider-wrapper" data-mc="2" runat="server">
	</div>
	<div class="general-contents m-bt-60">
		<div class="container" data-mc="3" runat="server">
		</div>
	</div>
	<div class="links-wrapper">
		<div class="container">
			<div class="row">
				<div class="col-sm-12" data-mc="4" runat="server">
				</div>
			</div>
		</div>
	</div>
</div>

<Nik:ResourceLink runat="server" ResourceLinkType="Style" FilePath="/contents/bootstrap/css/bootstrap.min.css" />
<Nik:ResourceLink runat="server" ResourceLinkType="Style" FilePath="/contents/bootstrap/css/bootstrap-theme.min.css" />
<Nik:ResourceLink runat="server" ResourceLinkType="Style" FilePath="/contents/font-awesome/font-awesome-4.7.0/css/font-awesome.css" />
<Nik:ResourceLink runat="server" ResourceLinkType="Style" FilePath="/contents/select2/css/select2.min.css" />
<Nik:ResourceLink runat="server" ResourceLinkType="Style" FilePath="/contents/bootstrap/css/bootstrap-rtl.css" />
<Nik:ResourceLink runat="server" ResourceLinkType="Style" FilePath="animate.css" />
<Nik:ResourceLink runat="server" ResourceLinkType="Style" FilePath="owl.carousel.min.css" />
<Nik:ResourceLink runat="server" ResourceLinkType="Style" FilePath="UiStyles.css" ExplodesAdmin="true" />

<Nik:ResourceLink runat="server" ResourceLinkType="Script" FilePath="/contents/js/jquery-1.10.2.min.js" IsTop="true" />
<Nik:ResourceLink runat="server" ResourceLinkType="Script" FilePath="/contents/bootstrap/js/bootstrap.min.js" IsTop="true" />
<Nik:ResourceLink runat="server" ResourceLinkType="Script" FilePath="/contents/select2/js/select2.full.min.js" IsTop="true" />
<Nik:ResourceLink runat="server" ResourceLinkType="Script" FilePath="/contents/js/EngineScript.js" IsTop="true" />

<Nik:ResourceLink runat="server" ResourceLinkType="Script" FilePath="owl.carousel.min.js" />
<Nik:ResourceLink runat="server" ResourceLinkType="Script" FilePath="UiScript.js" />
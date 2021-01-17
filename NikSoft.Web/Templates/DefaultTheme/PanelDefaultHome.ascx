<%@ Control Language="C#" AutoEventWireup="true" Inherits="NikSoft.UILayer.NikSkinTemplate" %>
<Nik:SkinTitle runat="server" Title="PanelDefaultHome"></Nik:SkinTitle>
	
<div id="wrapper" runat="server" class="wrapper">
	<nav class="navbar navbar-inverse navbar-fixed-top">
		<div class="mobile-only-brand pull-right">
			<div class="nav-header pull-right">
				<div class="logo-wrap">
					<a href="/">
						<img class="brand-img" src="/images/nik-logo-gold-w.png" alt="brand" />
					</a>
				</div>
			</div>
			<a id="toggle_nav_btn" class="toggle-left-nav-btn pull-right" href="javascript:void(0);"><i class="fa fa-bars" aria-hidden="true"></i></a>
			<a id="toggle_mobile_nav" class="mobile-only-view" href="javascript:void(0);"><i class="fa fa-ellipsis-h" aria-hidden="true"></i></a>
		</div>
		<div id="mobile_only_nav" class="mobile-only-nav pull-left">
			<ul class="nav navbar-right top-nav pull-right">
				<!--<li class="dropdown language-drp" data-mc="4" runat="server">
				</li>-->
				<li class="dropdown auth-drp" data-mc="3" runat="server">
				</li>
			</ul>
		</div>
	</nav>
	
	<div class="fixed-sidebar-left nicescroll-bar" data-mc="1" runat="server">
		
	</div>
	
	<div class="page-wrapper">
        <div class="container-fluid pt-25">
			<div class="row">
				<div class="col-sm-6" data-mc="2" runat="server">
					
				</div>
			</div>
		</div>
		<footer class="footer pl-30 pr-30 text-center">
			<div class="row">
				<div class="col-sm-12">
					<p>2017 &copy; نیک سافت</p>
				</div>
			</div>
		</footer>
	</div>
	
</div>
<Nik:ResourceLink runat="server" ResourceLinkType="Style" FilePath="/contents/bootstrap/css/bootstrap.min.css" />
<Nik:ResourceLink runat="server" ResourceLinkType="Style" FilePath="/contents/bootstrap/css/bootstrap-theme.min.css" />
<Nik:ResourceLink runat="server" ResourceLinkType="Style" FilePath="/contents/font-awesome/font-awesome-4.7.0/css/font-awesome.css" />
<Nik:ResourceLink runat="server" ResourceLinkType="Style" FilePath="/contents/select2/css/select2.min.css" />
<Nik:ResourceLink runat="server" ResourceLinkType="Style" FilePath="/contents/css/NikCss.css" />
<Nik:ResourceLink runat="server" ResourceLinkType="Style" FilePath="/contents/bootstrap/css/bootstrap-rtl.css" />
<Nik:ResourceLink runat="server" ResourceLinkType="Style" FilePath="PanelStyle.css" />
<Nik:ResourceLink runat="server" ResourceLinkType="Style" FilePath="Face1.css" />

<Nik:ResourceLink runat="server" ResourceLinkType="Script" FilePath="/contents/js/jquery-1.10.2.min.js" IsTop="true" />
<Nik:ResourceLink runat="server" ResourceLinkType="Script" FilePath="/contents/bootstrap/js/bootstrap.min.js" IsTop="true" />
<Nik:ResourceLink runat="server" ResourceLinkType="Script" FilePath="/contents/select2/js/select2.full.min.js" IsTop="true" />
<Nik:ResourceLink runat="server" ResourceLinkType="Script" FilePath="/contents/js/EngineScript.js" IsTop="true" />

<Nik:ResourceLink runat="server" ResourceLinkType="Script" FilePath="Face1.js" />

	
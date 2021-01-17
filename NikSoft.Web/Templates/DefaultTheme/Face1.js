$(document).ready(function() {	
	/* ------------ menu --------------*/
	$('.toggle-left-nav-btn').click(function(){
		if($('.wrapper').hasClass('open-slide')) {
			$('.wrapper').removeClass('open-slide');
		} else {
			$('.wrapper').addClass('open-slide');
		}
	});
	
	$('.parent-menu').click(function(){
		var ParentLi = $($(this).parents('li')[0]);
		if(ParentLi.find('.sub-menu-hoder').hasClass('close-m')) {
			ParentLi.find('.sub-menu-hoder').removeClass('close-m');
			ParentLi.find('.caret-menu i').removeClass('fa-caret-down');
			ParentLi.find('.caret-menu i').addClass('fa-caret-up');
		} else {
			ParentLi.find('.sub-menu-hoder').addClass('close-m');
			ParentLi.find('.caret-menu i').removeClass('fa-caret-up');
			ParentLi.find('.caret-menu i').addClass('fa-caret-down');
		}
	});
	
	$('.fixed-sidebar-left').hover(function() {
		if($('.wrapper').hasClass('hover-slide')) {
			$('.wrapper').removeClass('hover-slide');
		} else {
			$('.wrapper').addClass('hover-slide');
		}
	});
	
	/* ------------------ Right Menu ------------- */
	$('.mobile-only-view').click(function(){
		if($('.wrapper').hasClass('m-left-open')) {
			$('.wrapper').removeClass('m-left-open');
		} else {
			$('.wrapper').addClass('m-left-open');
		}
	});
	
	
	/* ---------------- page wrapper ---------------- */
	var pageWrap = $(window).height();
	$('.page-wrapper').css('min-height', pageWrap);
	
	$(window).resize(function() {
		$('.page-wrapper').css('min-height', $(window).height());
	});
});
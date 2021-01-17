$(document).ready(function(){
	var slider = $('.slider');
	slider.owlCarousel({
		rtl: true,
		nav: true,
		autoplay: true,
		navText: ["<i class='fa fa-angle-left' aria-hidden='true'></i>", "<i class='fa fa-angle-right' aria-hidden='true'></i>"],
		loop: true,
		animateOut: 'fadeOut',
		animateIn: 'fadeIn',
		items:1
	});
	
	/*******************************/
	
	var winh = $(window).height();
	$('.slide-item').height(winh);
	
	$(window).resize(function(){
		var winh = $(window).height();
		$('.slide-item').height(winh);
	});
	
	/*********************************/
	
	var VidSlider = $('.video-carousel');
	if(VidSlider.length > 0) {
		VidSlider.owlCarousel({
			rtl: false,
			nav: true,
			autoplay: true,
			navText: ["<i class='fa fa-angle-left' aria-hidden='true'></i>", "<i class='fa fa-angle-right' aria-hidden='true'></i>"],
			loop: true,
			animateOut: 'fadeOut',
			animateIn: 'fadeIn',
			items:8
		});
	}
	
	/******************* video slider ******************/
	if($('.k-video-slider').length > 0){
		var player = jwplayer('player');
		$('.fa-window-close').click(function(){
			if($('.video-slider-wrapper').hasClass('in')) {
				$('.video-slider-wrapper').removeClass('in');
			}
			player.stop();
		});
		
		$('.video-link').click(function() {
			if(!$('.video-slider-wrapper').hasClass('in')){
				$('.video-slider-wrapper').addClass('in');
			}
			var parentE = $($(this).parents('.gallery-video-image')[0]);
			var targetSrc = parentE.find('.video-link').data('source');
			player.setup({
				file: targetSrc
			});
			var targetId = parentE.find('.video-link').attr('id');
			$('#video-data').text(targetId.split('-')[1]);
		});
	}
});

// JavaScript Document
(function(){
    var sub$ = jQuery.sub();
	function newPosi(){
		var objW = sub$(window);
		var objC = sub$('.plant_header');
		var broT = objW.scrollTop();
		var top = broT;
		objC.css({'top':top});
		}
    sub$(document).ready(function() {
      	sub$('.plant_hed_nav_hover').click(function(){
		sub$('.next_div').animate({'height':260},0).animate({'width':630},500);
		})
		sub$('.plant_hed_nav_hover').mouseleave(function(){
		sub$('.next_div').animate({'height':0},500).animate({'width':0},0);
		})
		newPosi();
		sub$(window).resize(function(){
			newPosi();
			})
		sub$(window).scroll(function(){
			newPosi();
			})
    });
  })();

function add2favorite(){
	try{
		if(document.all){
			window.external.AddFavorite(location.href,document.title);
		}else if(window.sidebar){
			window.sidebar.addPanel(document.title, location.href,"");
		}else{
			alert('您的浏览器不支持昆仑在线自带收藏功能，需手动加入收藏夹，谢谢您的支持');
		}
	}catch(e){
		alert('您的浏览器不支持昆仑在线自带收藏功能，需手动加入收藏夹，谢谢您的支持');
	}
}

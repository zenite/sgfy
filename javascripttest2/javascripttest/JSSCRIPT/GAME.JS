function changeVillage(villageId, villageName) {
	if(w.villageid != villageId) w.villageid = villageId;
	MM_iframePost('resources.status');
	MM_xmlLoad('village.soldierview&keep=all');
	$('listallcity').style.display = 'none';
}
function foldproduction(obj,close) {
    if ($('production').style.display=='block' || close) {
        obj.className='hide';
        $('production').style.display='none';
        $('taskFloat').className='taskFloat taskFloatStatus1';
        $('taskFloat2').className='taskFloat taskFloatStatus3';
    } else {
        obj.className='show';
        $('production').style.display='block';
        $('taskFloat').className='taskFloat taskFloatStatus2';
        $('taskFloat2').className='taskFloat taskFloatStatus4';
    }
}
function drawingmode(s) {
	if (s) foldproduction($('res_product_button'),1);
	document.cookie="userhabit_drawingpage="+(s?1:0);
	$('drawingmode_on').style.display=s?'none':'block';
	$('drawingmode_off').style.display=s?'block':'none';
	$('drawingpages').style.display=s?'block':'none';
}
function village_rename(p1,p2) {
	MM_showDialog(
		'<p>'+p1+'<input type="text" id="dialog_input" value="" />'+p2+'</p>', '', [
			{title:w.lang.dialog_modify_button,act:function(){if ($('dialog_input').value!='') {MM_closeDialog();MM_iframePost('village.rename&vname='+encodeURIComponent($('dialog_input').value));return false;}}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();return false;}}
		]);
}
function emperor_rename(p1,p2) {
	MM_showDialog(
		'<p>'+p1+'<input type="text" id="dialog_input" value="" />'+p2+'</p>', '', [
			{title:w.lang.dialog_modify_button,act:function(){if ($('dialog_input').value!='') {MM_closeDialog();MM_iframePost('user.rename&uname='+encodeURIComponent($('dialog_input').value));return false;}}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();return false;}}
		]);
}
function alliance_rename(p1,p2) {
	MM_showDialog(
		'<p>'+p1+'<input type="text" id="dialog_input" value="" />'+p2+'</p>', '', [
			{title:w.lang.dialog_modify_button,act:function(){if ($('dialog_input').value!='') {MM_closeDialog();MM_iframePost('alliance.rename&aname='+encodeURIComponent($('dialog_input').value));return false;}}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();return false;}}
		]);
}
function validate_change(){
	setTimeout(function(){$("validate_img").src='index.php?act=validate.image&rand='+Math.random();$("check_code").focus();}, 50);
}
function validate_enter_change(unitx,unity){
	if ($('mv_checkcode').value!='')
	{
	 	MM_closeDialog();
	 	MM_iframePost('map.mountainvillage&uitx='+unitx+'&uity='+unity+'&checkcode='+$('mv_checkcode').value);
	}
	return false;
}
function mountain_village(p1,p2,unitx,unity) {
	var d=new Date();
	MM_showDialog(
	   '<p>'+p1+'<input type="text" id="mv_checkcode"  value="" onkeyup="if (event.keyCode==13) validate_enter_change('+unitx+','+unity+');return false;" maxlength="4" size="10"/>'+'&nbsp;<img id="validate_img" src="index.php?act=validate.image" sgtitle='+SGLang.ValidateCodeDark+' onclick="validate_change();" class="canclick">'+'</p>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){if ($('mv_checkcode').value!='') {MM_closeDialog();MM_iframePost('map.mountainvillage&uitx='+unitx+'&uity='+unity+'&checkcode='+$('mv_checkcode').value);return false;}}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();return false;}}
		]);
	setTimeout(validate_change,50);
}///old:grx:看不清,请点击图片

function mountain_village(p1,p2,unitx,unity) {
	var d = new Date();
	MM_showDialog(

	);
}

function record_coordinates(p1,p2,x,y) {
	MM_showDialog(
		'<p>'+p1+'<input type="text" id="dialoginput" name="dialoginput" value="" />'+p2+'</p>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){ {MM_closeDialog();MM_iframePost('map.addbookmark&x='+x+'&y='+y+'&dialoginput='+encodeURIComponent($('dialoginput').value));return false;}}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();return false;}}
		]);
}
function update_coordinates(p1,p2,id) {
	MM_showDialog(
		'<p>'+p1+'<input type="text" id="dialoginput" name="dialoginput" value="" maxlength="15"/>'+p2+'</p>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){ {MM_closeDialog();MM_iframePost('map.updatebookmark&id='+id+'&dialoginput='+encodeURIComponent($('dialoginput').value));return false;}}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();return false;}}
		]);
}
function logout(p1,p2,p3,p4,p5) {
	var extra_str = (p4 == 1) ? '<br><label><input type="checkbox" id="lock_pwd" value="1" checked />'+p3+'</label></div><div style="text-align:center" class="tips_red">'+p5+'</div>' : '</div>';
	var extar_str
	MM_showDialog(
		'<h4>'+p1+'</h4><div style="width:75%;text-align:left;text-indent:0;margin:5px auto;clear:both"><label><input type="checkbox" id="logout_item" value="1" />'+p2+'</label>'+extra_str, '', [
			{title:w.lang.dialog_confirm_button,act:function(){MM_closeDialog();location.href='index.php?act=login.logout&rand='+(++w.rd)+'&cookie_stat='+(($('logout_item').checked)?'1':'0')+((p4 == 1) ? ('&lock_pwd='+(($('lock_pwd').checked)?'1':'0')) : '');return false;}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();return false;}}
		]);
}
// 金币或者礼金券消费的选择
function consume(p1,p2) {
	//arr = p2.split('|$|');
	MM_showDialog(
		'<h4>'+p2+'</h4><ul class="commodity_buy"><li><label for="consume_type0" class="canclick"><input type="radio" name="consume_type" id="consume_type0" checked="true" value="0">'+SGLang.ConsumeByCoin+'</label></li><li><label for="consume_type1" class="canclick"><input type="radio" name="consume_type" id="consume_type1" value="1" />'+SGLang.ConsumeByTicket+'</li></label></ul>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){if(!$('consume_type0').checked&&!$('consume_type1').checked)return false;MM_closeDialog();MM_xmlLoad(p1+'&consume_type='+(($('consume_type1').checked)?'1':'0'));return false;}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();}}
		]);
}//金币消费 礼金券消费

//by songjing 20110408  活动修改，挑战主将免费5次
function free_consume(p1,p2) {
	//arr = p2.split('|$|');
	MM_showDialog(
		'<h4>'+p2+'</h4>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){MM_closeDialog();MM_xmlLoad(p1);return false;}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();}}
		]);
}

//加召回令的时候修改的
function consume4(p1,p2,p3) {
	//arr = p2.split('|$|');
	MM_showDialog('<h4>'+p2+'</h4><ul class="commodity_buy"><li><label for="consume_type0" class="canclick"><input type="radio" name="consume_type" id="consume_type0" checked="true" value="0">'+SGLang.ConsumeByCoin+'</label></li><li><label for="consume_type1" class="canclick"><input type="radio" name="consume_type" id="consume_type1" value="1" />'+SGLang.ConsumeByTicket+'</label></li><li><label for="consume_type2" class="canclick"><input type="radio" name="consume_type" id="consume_type2" value="2" '+((p3==0)?'disabled="disabled"':'')+' />'+SGLang.ItemGoods285+p3+'</label></li></ul>', '', [{title:w.lang.dialog_confirm_button,act:function(){if(!$('consume_type0').checked&&!$('consume_type1').checked&&!$('consume_type2').checked) {return false;} else if ($('consume_type0').checked) {var ctype = 0;} else if ($('consume_type1').checked) {var ctype = 1;} else {var ctype = 2;}MM_closeDialog();MM_xmlLoad(p1+'&consume_type='+ctype);return false;}},{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();}}]);}


function consume1(p1) {
	//arr = p1.split('|$|');
	MM_showDialog(
		'<h4>'+p1+'</h4><ul class="commodity_buy"><li><label for="consume_type0" class="canclick"><input type="radio" name="consume_type" id="consume_type0" checked="true" value="0" onclick="$(\'consumetype\').value=0;">'+SGLang.ConsumeByCoin+'</label></li><li><label for="consume_type1" class="canclick"><input type="radio" name="consume_type" id="consume_type1" value="1" onclick="$(\'consumetype\').value=1;" />'+SGLang.ConsumeByTicket+'</label></li></ul>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){if(!$('consume_type0').checked&&!$('consume_type1').checked)return false;MM_closeDialog();document.computer_trade_form.action=document.computer_trade_form.action+'&villageid='+w.villageid+'&rand='+(++rd);document.computer_trade_form.submit();return false;}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();}}
		]);
}//金币消费 礼金券消费

function delmsg(msg) {
	MM_showDialog(
		'<h4 class="msgtips"><img src="'+w.img_src+'images/icon/alert.gif" class="attention" />'+msg+'</h4>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){MM_closeDialog();try{document.delform.action+=('&villageid='+w.villageid+'&rand='+(++w.rd));document.delform.submit();}catch(e){};return false;}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();return false;}}
		]);
}

// 确认提示框 参数说明：act,message,submessage,xmlload,title_image,cancel_act
function confirmDialog(a,m,s,x,t,c) {
	var _a=typeof(a)=='function'?a:(a=='keep'?function(){}:function(){x==2?MM_iframePost(a):MM_xmlLoad(a);});
	if (c) {var _c=typeof(c)=='function'?c:(c=='keep'?function(){}:function(){x?MM_xmlLoad(c):MM_iframePost(c);});}
	else var _c=function(){};
	MM_showDialog(
		'<h4 class="msgtips"><img src="'+w.img_src+'images/icon/alert.gif" class="attention" />'+m+'</h4>'+(s?s:''), t?t:'', [
			{title:w.lang.dialog_confirm_button,act:function() {MM_closeDialog();_a();return false;}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();_c();return false;}}
		]);
}
function confirmDialog_noclose(a,m,s,x,t,c,close) {
	var _a=typeof(a)=='function'?a:(a=='keep'?function(){}:function(){x==2?MM_iframePost(a):MM_xmlLoad(a);});
	if (c) {var _c=typeof(c)=='function'?c:(c=='keep'?function(){}:function(){x?MM_xmlLoad(c):MM_iframePost(c);});}
	else var _c=function(){};
	MM_showDialog_noclose(
		'<h4 class="msgtips"><img src="'+w.img_src+'images/icon/alert.gif" class="attention" />'+m+'</h4>'+(s?s:''), t?t:'', [
			{title:w.lang.dialog_confirm_button,act:function() {MM_closeDialog();_a();return false;}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();_c();return false;}}
		],close);
}
// 道具确认提示框 参数说明：act,message,submessage,xmlload,title_image,cancel_act
function useGoodsConfirm(a,m,s,x,t,c) {
	var _a=typeof(a)=='function'?a:(a=='keep'?function(){}:function(){x?MM_xmlLoad(a):MM_iframePost(a);});
	if (c) {var _c=typeof(c)=='function'?c:(c=='keep'?function(){}:function(){x?MM_xmlLoad(c):MM_iframePost(c);});}
	else var _c=function(){};
	MM_showDialog(
		''+m+(s?s:''), t?t:'', [
			{title:w.lang.dialog_confirm_button,act:function() {MM_closeDialog();_a();return false;}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();_c();return false;}}
		]);
}

// 一般提示框 参数说明：act,message,submessage,xmlload,title_image
function alertDialog(a,m,s,x,t) {
	var _a=typeof(a)=='function'?a:(a=='keep'?function(){}:function(){a=='/'?location.href='/':x?MM_xmlLoad(a):MM_iframePost(a);});
	MM_showDialog(
		'<h4 class="msgtips"><img src="'+w.img_src+'images/icon/alert.gif" class="attention" />'+m+'</h4>'+(s?s:''), t?t:'', [
			{title:w.lang.dialog_confirm_button,act:function() {MM_closeDialog();_a();return false;}}
		]);
}
//新手指南
function selectTag(showContent,selfObj,placeId){
	// 操作标签
	var tag = $("tags"+placeId).getElementsByTagName("li");
	var taglength = tag.length;
	for(i=0; i<taglength; i++){
		tag[i].className = "";
	}
	selfObj.parentNode.className = "on";
	// 操作内容
	for(i=0; j=$("tagContent"+placeId+i); i++){
		j.style.display = "none";
	}
	$(showContent).style.display = "block";
}

function showtree(index,nav,sub,pare){
	if(isNaN(index)) return false;
	if($(nav+index).className=='slideup' && $(sub+index).style.display == "block"){
		$(nav+index).className='slidedown';
		$(sub+index).style.display = "none";
	}else{
		var paresubul = $(pare).getElementsByTagName('ul');
		var paresuba = $(pare).getElementsByTagName('a');
		for(m=0;m<paresubul.length;m++){
			if(paresubul[m].style.display == "block"){
				paresubul[m].style.display = "none";
			}
		}
		for(n=0;n<paresuba.length;n++){
			if(paresuba[n].className == "slideup" || paresuba[n].className=='slideupselected'){
				paresuba[n].className = "slidedown";
			}
		}
/*		var navvalue = $(nav).value;
			for(i=0;i<navvalue;i++){
				if($(nav+i).className=='slideup' || $(nav+i).className=='slideupselected'){
					$(nav+i).className='slidedown';
				}
				if($(sub+i).style.display == "block"){
					$(sub+i).style.display = "none";
				}
			}*/

		$(nav+index).className='slideup';
		$(sub+index).style.display = "block";
	}
	guideContent(index);
}

function guideContent(index){
	$('sub'+index).innerHTML = "";
	var gameNews = eval('gameNews_'+index);
	for(var i in gameNews){
		if(!isNaN(parseInt(i))){
			$('sub'+index).innerHTML +="<li><a id='navsub"+index+i+"' target=\"guidecont\" onclick=\"GuideTreeSelected("+index+","+i+");\" class=\"slidedown\" href=\""+gameNews[i]['url']+"\">"+gameNews[i]['title']+"</a></li>";
		}
	}
}
function GuideTreeSelected(index,self){
	if(isNaN(self)) return false;
	var paresubli = $('sub'+index).getElementsByTagName('a');
	var gameNews = eval('gameNews_'+index);
	var newslen = gameNews.length;
	for(i=0;i<paresubli.length;i++){
		if($('navsub'+index+i).className=='slideupselected'){
			$('navsub'+index+i).className='slidedown';
		}
	}
	$('navsub'+index+self).className = 'slideupselected';
	j = parseInt(self)-1;
	if(j>=0 && j<=newslen-1){
		pageup = gameNews[j].url;
	}else{
		pageup = false;
	}
	k = parseInt(self)+1;
	if(k>=0 && k<=newslen-1){
		pagedown = gameNews[k].url;
	}else{
		pagedown = false;
	}
	if(pageup){
		document.getElementById('uppage').innerHTML = "<a href='"+pageup+"' target=\"guidecont\" onclick=\"GuideTreeSelected("+index+","+j+")\">"+SGLang.PagePrevious+"</a>";///old:grx:上一页
	}else{
		document.getElementById('uppage').innerHTML = "";
	}
	if(pagedown){
		document.getElementById('downpage').innerHTML = "<a href='"+pagedown+"' target=\"guidecont\" onclick=\"GuideTreeSelected("+index+","+k+")\">"+SGLang.PageNext+"</a>";///old:grx:下一页
	}else{
		document.getElementById('downpage').innerHTML = "";
	}
}
function GuideTreeSub(showe,showsubs){
	if(isNaN(showe) || isNaN(showsubs)) return false;
	for(i=0;i<showsubs;i++){
		$('guidesub'+i).style.display="none";
		$('guidea'+i).className="";
	}
	$('guidesub'+showe).style.display="block";
	$('guidea'+showe).className="now";
}
// 地图
function hover(id)
{
	var ele = $(id);
	ele.style.display = "block";
	ele.onmousemove = function() {
		$("hover_dong").style.display = "none";
		$("hover_xi").style.display = "none";
		$("hover_nan").style.display = "none";
		$("hover_bei").style.display = "none";
		$("hover_zhong").style.display = "none";
		this.style.display = "block";
	}
	ele.onmouseout = function() {
		this.style.display = "none";
	}
}

function mapDirection(direction)
{
	var centerXY = $("city_div_103").innerHTML;
	var centerArray = centerXY.split(',');
	var centerX = centerArray[0];
	var centerY = centerArray[1];
	var move_tox = parseInt(centerX);
	var move_toy = parseInt(centerY);

	switch(direction)
	{
		case 'east':
			MM_xmlLoad('map.status&uitx='+(move_tox+7)+'&uity='+move_toy+'&focus=1', 'right');
			break;
		case 'west':
			MM_xmlLoad('map.status&uitx='+(move_tox-7)+'&uity='+move_toy+'&focus=1', 'right');
			break;
		case 'south':
			MM_xmlLoad('map.status&uitx='+move_tox+'&uity='+(move_toy-7)+'&focus=1', 'right');
			break;
		case 'north':
			MM_xmlLoad('map.status&uitx='+move_tox+'&uity='+(move_toy+7)+'&focus=1', 'right');
			break;
		case 'center':
			MM_xmlLoad('map.status', 'right');
			break;
		default:
			MM_xmlLoad('map.status', 'right');
			break;
	}
}

function alliance_sendcmd_checkXY()
{
	var target_X = $('Command_Target_X').value.trim();
	var target_Y = $('Command_Target_Y').value.trim();
	return is_select_user();
	if( (target_X=='' && target_Y==''))
	{
		alertDialog('keep',SGlang.VillageFullUnit);///old:grx:请输入城镇完整座标
		return false;
	}
	if(isNaN(target_X) || isNaN(target_Y))
	{
		alertDialog('keep',SGlang.VillageUnitInt);///old:grx:座标必须是数字!
		return false;
	}
	else
	{
		return true;
	}
}

function is_select_user()
{
	var user_no = parseInt($('receive_user_no').innerHTML);
	if(isNaN(user_no)){
		user_no = 0;
	}
	if(user_no == 0){
		alertDialog('keep',SGLang.SelectSinkUser);///old:grx:请选择指令接收人
		return false;
	}else{
		return true;
	}
}

function trade_pc_change(txt_id)
{
	var max_Crop_v = parseInt ($('Crop_curmax').value);		//当前每种资源上限的值
	var max_Lumber_v = parseInt ($('Lumber_curmax').value);
	var max_Clay_v = parseInt ($('Clay_curmax').value);
	var max_Iron_v = parseInt ($('Iron_curmax').value);

	var input_v = parseInt(($('Txt_'+txt_id).value.replace(/^0+/,'')));	//输入框值
	var resource_v = parseInt(Math.floor($(txt_id).value));	//资源值
	var txt_sum_obj = $('txt_sum');		//输入值总数对象
	var excess_sum_obj = $('excess_sum');		//剩余值总数对象

	if(isNaN(input_v)){
		$('Txt_'+txt_id).value = 0;
		input_v = 0;
	}
	$('Txt_'+txt_id).value = input_v;

	if( input_v > parseInt ($(txt_id+'_curmax').value) )
	{
		$('Txt_'+txt_id).value = parseInt ($(txt_id+'_curmax').value);
	}
	var excess_obj = $('excess_'+txt_id);
	excess_obj.innerHTML = parseInt($('Txt_'+txt_id).value) - resource_v;
	txt_sum_obj.innerHTML = (Math.floor($('Txt_Crop').value)+Math.floor($('Txt_Lumber').value)+Math.floor($('Txt_Clay').value)+Math.floor($('Txt_Iron').value));
	excess_sum_obj.innerHTML = Math.floor($('resource_sum').innerHTML) - Math.floor(txt_sum_obj.innerHTML);
	$('trade_title').innerHTML = SGLang.AvgResourceFirst;///old:grx:第一步：分配资源
	$('trade_pre').innerHTML = '';
	$('trade_info').className='tips textcenter';
	var goldcoins = parseInt($('goldcoins').innerHTML);
	if(isNaN(goldcoins)){
		goldcoins = 0;
	}
	$('trade_next').innerHTML =  '<a href="javascript:void(0);" class="redbutton_s" style="position:absolute;left:20px;top:109px;float:none;" onclick="trade_average();">'+SGLang.AvgResourceNext+'</a>';///下一步

}

function trade_average()
{
	var resource_sum = parseInt ($('resource_sum').innerHTML);		//当前资源总数
	var avg_num = parseInt (resource_sum/4);
	var Crop_v = parseInt ($('Txt_Crop').value);		//每个输入框的值
	var Lumber_v = parseInt ($('Txt_Lumber').value);
	var Clay_v = parseInt ($('Txt_Clay').value);
	var Iron_v = parseInt ($('Txt_Iron').value);

	var max_Crop_v = parseInt ($('Crop_curmax').value);		//当前每种资源上限的值
	var max_Lumber_v = parseInt ($('Lumber_curmax').value);
	var max_Clay_v = parseInt ($('Clay_curmax').value);
	var max_Iron_v = parseInt ($('Iron_curmax').value);

	var cur_Crop_v = parseInt ($('Crop').value);		//当前每种资源的值
	var cur_Lumber_v = parseInt ($('Lumber').value);
	var cur_Clay_v = parseInt ($('Clay').value);
	var cur_Iron_v = parseInt ($('Iron').value);

	var new_sum = Crop_v+Lumber_v+Clay_v+Iron_v;
	if (Crop_v>max_Crop_v)			////如果输入的资源大于当前资源上限,则把上限的值给当然资源
		$('Txt_Crop').value = max_Crop_v;
	if (Lumber_v>max_Lumber_v)
		$('Txt_Lumber').value = max_Lumber_v;
	if (Clay_v>max_Clay_v)
		$('Txt_Clay').value = max_Clay_v;
	if (Iron_v>max_Iron_v)
		$('Txt_Iron').value = max_Iron_v;

	/////如果输入的值总和大于当前资源总和,则按比例全部减掉
	var input_sum = parseInt($('Txt_Crop').value) +  parseInt($('Txt_Lumber').value) + parseInt($('Txt_Clay').value) + parseInt($('Txt_Iron').value);
	if( input_sum > resource_sum )
	{
		while(true){////把多出的值从各个资源中平均减掉,如果减到负值,则付为0
			$('Txt_Crop').value = parseInt($('Txt_Crop').value) - Math.floor((input_sum - resource_sum)/4)-1;
			$('Txt_Lumber').value = parseInt($('Txt_Lumber').value) - Math.floor((input_sum - resource_sum)/4)-1;
			$('Txt_Clay').value = parseInt($('Txt_Clay').value) - Math.floor((input_sum - resource_sum)/4)-1;
			$('Txt_Iron').value = parseInt($('Txt_Iron').value) - Math.floor((input_sum - resource_sum)/4)-1;

			$('Txt_Crop').value = parseInt($('Txt_Crop').value) < 0 ? 0 : parseInt($('Txt_Crop').value);
			$('Txt_Lumber').value = parseInt($('Txt_Lumber').value) < 0 ? 0 : parseInt($('Txt_Lumber').value);
			$('Txt_Clay').value = parseInt($('Txt_Clay').value) < 0 ? 0 : parseInt($('Txt_Clay').value);
			$('Txt_Iron').value = parseInt($('Txt_Iron').value) < 0 ? 0 : parseInt($('Txt_Iron').value);

			$('Txt_Crop').value = parseInt($('Txt_Crop').value) > max_Crop_v ? max_Crop_v : parseInt($('Txt_Crop').value);
			$('Txt_Lumber').value = parseInt($('Txt_Lumber').value) > max_Lumber_v ? max_Lumber_v : parseInt($('Txt_Lumber').value);
			$('Txt_Clay').value = parseInt($('Txt_Clay').value) > max_Clay_v ? max_Clay_v : parseInt($('Txt_Clay').value);
			$('Txt_Iron').value = parseInt($('Txt_Iron').value) > max_Iron_v ? max_Iron_v : parseInt($('Txt_Iron').value);
			input_sum = parseInt($('Txt_Crop').value) +  parseInt($('Txt_Lumber').value) + parseInt($('Txt_Clay').value) + parseInt($('Txt_Iron').value);
			if(input_sum > resource_sum)
			{
				continue;
			}else{
				break;
			}
		}
	}

	if( input_sum < resource_sum )
	{
		while(true)
		{
			$('Txt_Crop').value = parseInt($('Txt_Crop').value) + Math.floor((resource_sum - input_sum)/4);
			$('Txt_Lumber').value = parseInt($('Txt_Lumber').value) + Math.floor((resource_sum - input_sum)/4);
			$('Txt_Clay').value = parseInt($('Txt_Clay').value) + Math.floor((resource_sum - input_sum)/4);
			$('Txt_Iron').value = parseInt($('Txt_Iron').value) + Math.floor((resource_sum - input_sum)/4);

			$('Txt_Crop').value = parseInt($('Txt_Crop').value) > max_Crop_v ? max_Crop_v : parseInt($('Txt_Crop').value);
			$('Txt_Lumber').value = parseInt($('Txt_Lumber').value) > max_Lumber_v ? max_Lumber_v : parseInt($('Txt_Lumber').value);
			$('Txt_Clay').value = parseInt($('Txt_Clay').value) > max_Clay_v ? max_Clay_v : parseInt($('Txt_Clay').value);
			$('Txt_Iron').value = parseInt($('Txt_Iron').value) > max_Iron_v ? max_Iron_v : parseInt($('Txt_Iron').value);

			input_sum = parseInt($('Txt_Crop').value) +  parseInt($('Txt_Lumber').value) + parseInt($('Txt_Clay').value) + parseInt($('Txt_Iron').value);
			if((resource_sum-input_sum)>=4)
			{
				continue;
			}else{
				break;
			}
		}
		if((resource_sum-input_sum)==1)
		{
			if(parseInt($('Txt_Lumber').value) < max_Lumber_v)
				$('Txt_Lumber').value = parseInt($('Txt_Lumber').value)+1;
			else if(parseInt($('Txt_Clay').value) < max_Clay_v)
				$('Txt_Clay').value = parseInt($('Txt_Clay').value)+1;
			else if(parseInt($('Txt_Iron').value) < max_Iron_v)
				$('Txt_Iron').value = parseInt($('Txt_Iron').value)+1;
			else if(parseInt($('Txt_Crop').value) < max_Crop_v)
				$('Txt_Crop').value = parseInt($('Txt_Crop').value)+1;

		}else if((resource_sum-input_sum)==2){
			var tv = 2;
			for(var i=0; i<2; i++){
				if(tv>0 && parseInt($('Txt_Lumber').value) < max_Lumber_v){
					$('Txt_Lumber').value = parseInt($('Txt_Lumber').value)+1;
					tv = tv-1;
				}
				if(tv>0 && parseInt($('Txt_Clay').value) < max_Clay_v){
					$('Txt_Clay').value = parseInt($('Txt_Clay').value)+1;
					tv = tv-1;
				}
				if(tv>0 && parseInt($('Txt_Iron').value) < max_Iron_v){
					$('Txt_Iron').value = parseInt($('Txt_Iron').value)+1;
					tv = tv-1;
				}
				if(tv>0 && parseInt($('Txt_Crop').value) < max_Crop_v){
					$('Txt_Crop').value = parseInt($('Txt_Crop').value)+1;
					tv = tv-1;
				}
				if(tv<1)
					break;
			}


		}else if((resource_sum-input_sum)==3){
			var tv = 3;
			for(var i=0; i<3; i++){
				if(tv>0 && parseInt($('Txt_Lumber').value) < max_Lumber_v){
					$('Txt_Lumber').value = parseInt($('Txt_Lumber').value)+1;
					tv = tv-1;
				}
				if(tv>0 && parseInt($('Txt_Clay').value) < max_Clay_v){
					$('Txt_Clay').value = parseInt($('Txt_Clay').value)+1;
					tv = tv-1;
				}
				if(tv>0 && parseInt($('Txt_Iron').value) < max_Iron_v){
					$('Txt_Iron').value = parseInt($('Txt_Iron').value)+1;
					tv = tv-1;
				}
				if(tv>0 && parseInt($('Txt_Crop').value) < max_Crop_v){
					$('Txt_Crop').value = parseInt($('Txt_Crop').value)+1;
					tv = tv-1;
				}
				if(tv<1)
					break;
			}
		}
	}
	input_sum = parseInt($('Txt_Crop').value) +  parseInt($('Txt_Lumber').value) + parseInt($('Txt_Clay').value) + parseInt($('Txt_Iron').value);
	$('txt_sum').innerHTML = input_sum;
	$('excess_sum').innerHTML = resource_sum-input_sum;
	$('excess_Lumber').innerHTML = parseInt($('Txt_Lumber').value)-cur_Lumber_v;
	$('excess_Crop').innerHTML = parseInt($('Txt_Crop').value)-cur_Crop_v;
	$('excess_Clay').innerHTML = parseInt($('Txt_Clay').value)-cur_Clay_v;
	$('excess_Iron').innerHTML = parseInt($('Txt_Iron').value)-cur_Iron_v;

	trade_next();
}



function trade_next()
{
	$('trade_title').innerHTML = SGLang.AvgResourceSecond;///old:grx:第二步：确认分配
	$('trade_info').className='tips textcenter secret';
	$('trade_pre').innerHTML = '<a href="javascript:void(0);" class="redbutton_s" style="top:115px;left:20px;position:absolute;m" onclick="trade_pre();">'+SGLang.AvgResourcePrevious+'</a>';///old:grx:上一步
	var goldcoins = parseInt($('goldcoins').innerHTML);
	var need_goldcoins =  parseInt($('need_goldcoins').innerHTML);
	if(isNaN(need_goldcoins)){
		return false;
	}
	if(isNaN(goldcoins)){
		goldcoins = 0;
	}

		//$('trade_next').innerHTML =  '<a href="#" class="button1 right" onclick="document.computer_trade_form.action=document.computer_trade_form.action+(\'&villageid=\'+w.villageid+\'&rand=\'+(++rd));document.computer_trade_form.submit();"><span> 交 易 </span></a>';
		if ($('by_tickets').value == 1 || $('by_item').value != '')
		{
			//card_num = $('card_num').value;
			$('trade_next').innerHTML =  '<a href="javascript:void(0);" class="redbutton_s" style="position:absolute;top:170px;left:230px;float:left;" onclick="consume3(\''+SGLang.AvgResourceConfirm.replace(/\[XXX\]/g,
		need_goldcoins)+'\','+$('by_tickets').value+",'"+$('by_item').value+"'"+')">'+SGLang.AvgResourceOK +'</a>';
		} else {
			$('trade_next').innerHTML =  '<a href="javascript:void(0);" class="redbutton_s" style="position:absolute;top:170px;left:230px;float:left;" onclick="confirmDialog(function(){document.computer_trade_form.action=document.computer_trade_form.action+(\'&villageid=\'+w.villageid+\'&rand=\'+(++rd));document.computer_trade_form.submit();return false;},\''+SGLang.AvgResourceConfirm.replace(/\[XXX\]/g,
		need_goldcoins)+'\')">'+SGLang.AvgResourceOK +'</a>';
		}///old:grx:确定
		$('title_resource_sum').innerHTML = SGLang.AvgResourceCur;///old:grx:当前资源：
		$('title_txt_sum').innerHTML = SGLang.AvgResourceAfter;///old:grx:交易后资源：
		$('title_excess_sum').innerHTML = SGLang.AvgResourceChange;///old:grx:资源变化：
		$('excess_sum_display').style.display = 'none';
		$('excess_sum_display_2').innerHTML = $('excess_sum').innerHTML;
		$('excess_Crop_display').style.display = '';
		$('excess_Clay_display').style.display = '';
		$('excess_Lumber_display').style.display = '';
		$('excess_Iron_display').style.display = '';



}

function trade_pre()
{
	$('trade_title').innerHTML = SGLang.AvgResourceFirst;///old:grx:第一步：分配资源
	$('trade_info').className='tips textcenter';
	$('trade_pre').innerHTML = '';
	var goldcoins = parseInt($('goldcoins').innerHTML);
	if(isNaN(goldcoins)){
		goldcoins = 0;
	}
	$('trade_next').innerHTML =  '<a href="javascript:void(0);" class="redbutton_s" style="position:absolute;left:20px;top:109px;" onclick="trade_average();">'+SGLang.AvgResourceNext+'</a>';///old:grx:下一步

	$('Txt_Crop').value = 0;
	$('Txt_Lumber').value = 0;
	$('Txt_Clay').value = 0;
	$('Txt_Iron').value = 0;
	var cur_Crop_v = parseInt ($('Crop').value);		//当前每种资源的值
	var cur_Lumber_v = parseInt ($('Lumber').value);
	var cur_Clay_v = parseInt ($('Clay').value);
	var cur_Iron_v = parseInt ($('Iron').value);
	var resource_sum = parseInt ($('resource_sum').innerHTML);		//当前资源总数
	$('excess_Lumber').innerHTML = 0-cur_Lumber_v;
	$('excess_Crop').innerHTML = 0-cur_Crop_v;
	$('excess_Iron').innerHTML = 0-cur_Iron_v;
	$('excess_Clay').innerHTML = 0-cur_Clay_v;
	$('excess_sum').innerHTML = resource_sum;
	$('txt_sum').innerHTML = 0;

	$('title_resource_sum').innerHTML = SGLang.AvgResourceAll;///old:grx:所有资源：
	$('title_txt_sum').innerHTML = SGLang.AvgResourceDistribute;///old:grx:分配资源：
	$('title_excess_sum').innerHTML = SGLang.AvgResourceRemanent;///old:grx:剩余资源：
	$('excess_sum_display').style.display = '';
	$('excess_sum_display_2').innerHTML = '';
	$('excess_Crop_display').style.display = 'none';
	$('excess_Clay_display').style.display = 'none';
	$('excess_Lumber_display').style.display = 'none';
	$('excess_Iron_display').style.display = 'none';

}

var spyId = 9;
var fireboltId = 6;

function tradeSearch(par)
{
	var curp = par.split('=')[0];
	var furl = $('iframedata').src;
	var p = furl.split('?')[1].split('&');
	var newurl='';
	var newarr = new Array();
	re = new RegExp(curp+'=','i');
	for(var i=0; i<p.length; i++)
	{
		if(p[i].match(re) || p[i].match(/villageid=/i) || p[i].match(/rand=/i)|| p[i].match(/btid=18/i) || p[i].match(/act=build.detail/i) || p[i].match(/act=/i) || p[i].match(/nowpage=/i) || p[i].match(/do=/i))
		{
			continue;
		}
		newarr.push(p[i]);
	}
	for(var i=0; i<newarr.length; i++)
	{
		newurl = newurl+newarr[i]+'&';
	}
	MM_iframePost('build.act&btid=18&do=searchTrade&'+newurl+par,'all');
}

function warStart(act, keep, flag) {
	var mye = null;
	if ((mye = document.forms['soldierf']) == null) return;
	var els = mye.elements;
	var type = -1;
	var hasGeneral = false;
	var hasSoldier = false;
	var soldierNum = 0;
	var dest = '';
	var onlySpy = true;
	var self_warvac = 0;
	var target_warvac = 0;
	if (arguments[3])
	{
		self_warvac = arguments[3];
	}
	if (arguments[4])
	{
		target_warvac = arguments[4];
	}

	if (self_warvac!=0){
		alertDialog('keep', SGLang.WarvacUnallowAttact);///old:grx:休假模式不允许攻击别人
		return;
	}


	if (target_warvac == 2){
		alertDialog('keep', SGLang.WarvacUnallowToAttact);///old:grx:适逢盛节，此地止战，该城无法被攻击！
		return;
	}

	for(var ii=0; ii<els.length; ii++)
	{
		if (els[ii].name == 'battlearray')
		{
			parValue = parseInt(els[ii].value);

			if (!isNaN(parValue))
			{
				battlearray = parValue;
				act += '&battlearray=' + els[ii].value;
			}
		}

		if (els[ii].name == 'type' && (flag || els[ii].value > -1))
		{
			parValue = parseInt(els[ii].value);

			if (!isNaN(parValue))
			{
				type = parValue;
				act += '&type=' + els[ii].value;
			}
		}

		if (els[ii].name && els[ii].name.indexOf('soldier') != '-1')
		{
			parName = parseInt(els[ii].name.replace('soldier', ''));
			parValue = parseInt(els[ii].value, 10);

			if (!isNaN(parName) && !isNaN(parValue) && parValue > 0)
			{
				hasSoldier = true;
				act += '&soldier[' + parName + ']=' + parValue;
				if (parName != spyId) onlySpy = false;
				soldierNum += parValue;
			}
		}

		if (els[ii].name && els[ii].name.indexOf('general') != -1)
		{
			parValue = parseInt(els[ii].value);

				if (parValue > 0)
				{
					act += '&'+els[ii].name+'=' + parValue;
					onlySpy = false;
					hasGeneral = true;
				}
		}

		if (els[ii].name && els[ii].name.indexOf('target') != '-1')
		{
			parName = parseInt(els[ii].name.replace('target', ''));
			parValue = parseInt(els[ii].value);

			if (!isNaN(parName) && !isNaN(parValue))
			{
				act += '&target[' + parName + ']=' + parValue;
			}
		}

		if (els[ii].name && els[ii].name == 'setautoatk' && (els[ii].type=="hidden" || els[ii].checked == true))
		{
			var autoatktime = els[ii].type == "hidden" ? $('autoatk_starttime').value : $('autoatk_y').value+"-"+
				$('autoatk_m').value+"-"+
				$('autoatk_d').value+" "+
				$('autoatk_h').value+":"+
				$('autoatk_i').value+":"+
				$('autoatk_s').value;

			var autoatkforce = els[ii].type == "hidden" ? $('autoatk_force').value : ($('autoatk_force').checked ? '1' : '0');

			act += '&autoatk_time=' + autoatktime;
			act += '&autoatk_exception=' + autoatkforce;
		}
	}

	//act += "&start=0";
	//alert(act);
	if ($('battle_type').value != '' && !isNaN($('city_id').value))
	{
		act += '&battle_type=' + $('battle_type').value;
		act += '&city_id=' + $('city_id').value;

		if($('battle_type').value == 'stronghold')
		{
			act += '&stronghold_id=' + $('stronghold_id').value;
			act += '&soldier_class=' + $('soldier_class').value;
		}
		else if($('battle_type').value == 'tripod')
		{
			act += '&tripod_id=' + $('tripod_id').value;
			act += '&camp_id=' + $('camp_id').value;
		}

		if( $('uit_id') && $('uit_id').value != '')
		{
			if(!$('startflag'))
			{
				if($('uit_condition_general').value == '1' && !hasGeneral)
				{
					alertDialog('keep', '请选择一个武将');
					return;
				}
				if(parseInt($('uit_condition_soldier_num').value,10) > soldierNum)
				{
					alertDialog('keep', '派去的兵太少了！不会起到作用的，至少要派'+parseInt($('uit_condition_soldier_num').value,10)+'个兵力');
					return;
				}
			}
			act += '&uit_id=' + $('uit_id').value;
		}
	} else if (!isNaN(parseInt(els['x'].value)) && !isNaN(parseInt(els['y'].value))){
		act += '&x=' + els['x'].value;
		act += '&y=' + els['y'].value;
	} else if (els['name'].value != '' && els['name'].value != '请输入城镇名称') {
		act += '&name=' + encodeURIComponent(els['name'].value);
	} else  {
		alertDialog('keep', SGLang.WarSelectTarget);///old:grx:请选择出兵目标
		return;
	}

	if (type == -1)
	{
		alertDialog('keep', SGLang.WarSelectSoldierClass);///old:grx:请选择出兵类别
		return;
	}

	if (!hasSoldier && type != 3)
	{
		alertDialog('keep', SGLang.WarSelectSoldierType);///old:grx:请选择出兵兵种
		return;
	}

	if (!hasSoldier && !hasGeneral && type == 3)
	{
		alertDialog('keep', SGLang.WarSelectSoldierType);///old:grx:请选择出兵兵种
		return;
	}

	if (onlySpy && type != 2 && type != 3)
	{
		alertDialog('keep', SGLang.WarSelectScoutType);///old:grx:您只选择的侦查兵种，请使用侦查模式！
		els['type'][type].checked = false;
		els['type'][2].checked = true;
		warSpyMode(true);
		return;
	}
	if (target_warvac == 1){
		confirmDialog(act, SGLang.WarvacCannotdestroy);///old:grx:此城止战，无法破坏城镇建设！
		return;
	}

	MM_iframePost(act, keep);
}

//进入(退出)藏兵洞
function getInOutHole(act,keep,str) {
	var mye = null;
	if ((mye = document.forms['soldierf']) == null) return;
	var els = mye.elements;
	var hasSoldier = false;

	for(var ii=0; ii<els.length; ii++)
	{
		if (els[ii].name && els[ii].name.indexOf(str) != '-1')
		{
			parName = parseInt(els[ii].name.replace(str, ''));
			parValue = parseInt(els[ii].value, 10);

			if (!isNaN(parName) && !isNaN(parValue) && parValue > 0)
			{
				hasSoldier = true;
				act += '&soldier[' + parName + ']=' + parValue;
			}
		}
	}

	if (!hasSoldier)
	{
		alertDialog('keep',SGLang.HoleSelectSoldierType);///old:grx:请选择兵种
		return;
	}

	MM_iframePost(act, keep);
}

function warSpyMode(flag) {
	for (var i = 0 ; i <= 33 ; i ++) {
		if (i == 12) i = 30;
		if (flag && i != spyId) {
			$('soldier' + i).value = 0;
			$('soldier' + i).disabled = true;
		} else {
			$('soldier' + i).disabled = false;
		}
		/*
		if (i<4 && (flag || arguments[1])) {
			$('battlearray' + i).disabled = true;
		} else if (i<4) {
			$('battlearray' + i).disabled = false;
		}*/
	}
	if (flag || arguments[1]) {
		$('battlearray').disabled = true;
	} else{
		$('battlearray').disabled = false;
	}
	/*
	if (flag) {
		$('general').selectedIndex = 0;
		$('general').disabled = true;
	} else {
		$('general').disabled = false;
	}*/
	for (i=1; i<6; i++){
		if (flag) {
			$('general' + i).selectedIndex = 0;
			$('general' + i).disabled = true;
			changeGeneralIcon(i,$('general_iconid'+i));
		} else {
			if (arguments[1] && i>2){
				$('general' + i).selectedIndex = 0;
				$('general' + i).disabled = true;
				changeGeneralIcon(i,$('general_iconid'+i));
			}else{
				$('general' + i).disabled = false;
			}
		}
	}
}

function warTypeSelect(obj)
{
	if (obj.value == 0){	//攻击
		warSpyMode(false);
	}else if (obj.value == 1){	//掠夺
		warSpyMode(false);
	}else if (obj.value == 2){	//侦查
		warSpyMode(true);
	}else if (obj.value == 3){	//支援
		warSpyMode(false, true);
	}
}

function warCheckGeneral(selectObj) {
	re = /d_[\d]+/g

	if (re.test(selectObj.options[selectObj.selectedIndex].value))
	{
		alertDialog('keep', SGLang.GeneralLevelLimit);///old:grx:武将达到1级以后才能带兵出征！
		selectObj.selectedIndex = 0;
		return false;
	}

	return true;
}

function warChangNum(select, obj) {
	if ($('soldier' + select).disabled == false) $('soldier' + select).value = obj.innerHTML.match(/[\d]+/);
}

function holeChangNum(select, obj) {
	if ($('hole' + select).disabled == false) $('hole' + select).value = obj.innerHTML.match(/[\d]+/);
}

function warSelectAll() {
	for (i = 0 ; i < 34 ; i++)
	{
		if (i == 12) i = 30;
		if ($('soldier' + i).disabled == false)
		{
			$('soldier' + i).value = $('soldier_link' + i).innerHTML.match(/[\d]+/);
		}
	}
}

function raiseSoldierStart(act, type, name, automove) {
	parType = parseInt(type);

	if ($('num_' + type))
	{
		parNum = parseInt($('num_' + type).value, 10);
	} else {
		parNum = 0;
	}

	if (!isNaN(parType) && !isNaN(parNum) && parNum > 0)
	{
		act += '&type=' + parType + '&num=' + parNum;
	} else {
		alertDialog('keep',SGLang.SoldierRaiseNum);///old:grx:请输入需要招募的士兵数量
		return;
	}

	if (automove) {
		confirmDialog(act, name + SGLang.SoldierRaiseAutomove);///old:grx:开启了<strong class='green'>明修暗渡</strong>，招募后会前往其他城镇。
	} else {
		MM_iframePost(act);
	}
}
function injuredSoldierStart(act, type, name) {
	parType = parseInt(type);

	if ($('num_' + type))
	{
		parNum = parseInt($('num_' + type).value, 10);
	} else {
		parNum = 0;
	}

	if (!isNaN(parType) && !isNaN(parNum) && parNum > 0)
	{
		act += '&type=' + parType + '&num=' + parNum;
	} else {
		alertDialog('keep',SGLang.SoldierInjuredNum);///old:grx:请输入需要招募的士兵数量
		return;
	}
	MM_iframePost(act);
}

function raiseWalldefenceStart(act, type, name) {
	parType = parseInt(type);

	if ($('num_' + type))
	{
		parNum = parseInt($('num_' + type).value, 10);
	} else {
		parNum = 0;
	}

	if (!isNaN(parType) && !isNaN(parNum) && parNum > 0)
	{
		act += '&type=' + parType + '&num=' + parNum;
	} else {
		alertDialog('keep',SGLang.WalldefenceBuildNum.replace(/\[XXX\]/g,
		name));///old:grx:'请输入需要建造的' + name + '数量'
		return;
	}

	MM_iframePost(act);
}

function pc_recruit_soldiers(act, type, num, money, name, max, automove, byitem)
{
	parType = parseInt(type);
	num = parseInt(num);

	lumber=document.getElementById("lumber_now").innerHTML;
	clay=document.getElementById("clay_now").innerHTML;
	iron=document.getElementById("iron_now").innerHTML;
	crop=document.getElementById("crop_now").innerHTML;
	total=parseInt(lumber)+parseInt(clay)+parseInt(iron)+parseInt(crop);
	var max_pc = total / num;
	if (max > -1) max_pc = Math.min(max, max_pc);
	if (!isNaN(parType))
	{
		act += '&type=' + parType;
	} else {
		alertDialog('keep',SGLang.SoldierTypeNoExist);///old:grx:招募兵种不存在
		return;
	}
	if (max_pc<1) {
		alertDialog('keep',SGLang.SoldierShortResource.replace(/\[XXX\]/g,
		name));///old:grx:您的资源不足，无法招募到+name
		return;
	}
	var confirmStr = SGLang.SoldierPCRaise.replace(/\[XXX\]/g,
	Math.floor(max_pc)
	).replace(/\[ZZZ\]/g,
	name
	).replace(/\[YYY\]/g,
	money
	);///old:grx:'招募助手可以招募到 <strong class="red">' + Math.floor(max_pc) + '</strong> 个' + name + '，将花费您 <strong class="red">' + money + '</strong> 个金币。';
	if (automove) confirmStr += '<br /><font class="red smallfont">（'+SGLang.SoldierOpenAutomove.replace(/\[XXX\]/g,
	name)+'）</font>';///old:grx:（' + name + '开启了<strong class="green smallfont">明修暗渡</strong>，招募后会前往其他城镇。）
	if (arguments[7] == 1 || arguments[8] != '')
	{
		consume2(act,confirmStr,arguments[7],arguments[8]);
	} else {
		confirmDialog(act,confirmStr);
	}
}

function towerSetNum(total, toLocal) {
	var mye=null;
	if ((mye=document.forms['soldierf'])==null) return;
	var els = mye.elements;

	for(var ii=0; ii<els.length; ii++)
	{
		if (toLocal)
		{
			if (els[ii].name && els[ii].name.indexOf('trans') != '-1')
			{
				parName = parseInt(els[ii].name.replace('trans', ''));
				parValue = parseInt(els[ii].value, 10);

				if (!isNaN(parName) && !isNaN(parValue) && parValue > 0)
				{
					localTotal = parseInt($('local_total' + parName).innerHTML);
					towerTotal = parseInt($('tower_total' + parName).innerHTML);

					soldierTotal = parseInt($('all_soldier').innerHTML);

					if (parValue > towerTotal)
					{
						alertDialog('keep', SGLang.TowerSoldierNumError);///old:grx:释放的士兵数量超过箭塔内该类士兵的总数！
						return;
					}

					$('local_total' + parName).innerHTML = localTotal + parValue;
					$('tower_total' + parName).innerHTML = towerTotal - parValue;
					$('all_soldier').innerHTML = soldierTotal - parValue;
					els[ii].value = "";
					$('soldier' + parName).value = towerTotal - parValue;
				}
			}
		} else {
			if (els[ii].name && els[ii].name.indexOf('trans') != '-1')
			{
				parName = parseInt(els[ii].name.replace('trans', ''));
				parValue = parseInt(els[ii].value, 10);

				if (!isNaN(parName) && !isNaN(parValue) && parValue > 0)
				{
					localTotal = parseInt($('local_total' + parName).innerHTML);
					towerTotal = parseInt($('tower_total' + parName).innerHTML);
					soldierTotal = parseInt($('all_soldier').innerHTML);

					if (parValue > localTotal)
					{
						alertDialog('keep', SGLang.LocalSoldierNumError);///old:grx:进入的士兵数量超过城内该类士兵的总数！
						return;
					}

					if (soldierTotal + parValue > total)
					{
						alertDialog('keep', SGLang.LocalSoldierUpLimit);///old:grx:进入的士兵数量超过箭塔上限！
						return;
					}

					$('local_total' + parName).innerHTML = localTotal - parValue;
					$('tower_total' + parName).innerHTML = towerTotal + parValue;
					$('all_soldier').innerHTML = soldierTotal + parValue;
                    els[ii].value = "";
					$('soldier' + parName).value = towerTotal + parValue;
				}
			}
		}
	}
}

function towerSet(act) {
	var mye=null;
	if ((mye=document.forms['soldierf'])==null) return;
	var els = mye.elements;
	var hasSoldier = false;

	for(var ii=0; ii<els.length; ii++)
	{
		if (els[ii].name && els[ii].name.indexOf('soldier') != '-1')
		{
			parName = parseInt(els[ii].name.replace('soldier', ''));
			parValue = parseInt(els[ii].value, 10);

			if (!isNaN(parName) && !isNaN(parValue) && parValue >= 0)
			{
				hasSoldier = true;
				act += '&soldier[' + parName + ']=' + parValue;
			}
		}
		if (els[ii].name && els[ii].name.indexOf('auto_defend') != '-1')
		{
			if(els[ii].checked == true) act += '&auto_defend=1';
			else                             act += '&auto_defend=0';
		}
	}

	if (!hasSoldier)
	{
		alertDialog('keep', SGLang.TowerSoldierResetError);///old:grx:调整数量错误
		return;
	}

	MM_iframePost(act);
}

function towerAllTrans( total, toLocal )
{
	var mye=null;
	var localTotal;
	var towerTotal;
	var soldierTotal;
	var parName;
	var transNum;


	if ((mye=document.forms['soldierf'])==null) return;
	var els = mye.elements;
	if (toLocal)
	{
		for(var ii=0; ii<els.length; ii++)
		{
			if (els[ii].name && els[ii].name.indexOf('trans') != '-1')
			{
				parName = parseInt(els[ii].name.replace('trans', ''));

				localTotal = parseInt($('local_total' + parName).innerHTML);
				towerTotal = parseInt($('tower_total' + parName).innerHTML);
				soldierTotal = parseInt($('all_soldier').innerHTML);

				if (!isNaN(parName) && !isNaN(towerTotal) && towerTotal > 0)
				{
					$('local_total' + parName).innerHTML = localTotal + towerTotal;
					$('tower_total' + parName).innerHTML = 0;
					$('all_soldier').innerHTML = soldierTotal - towerTotal;
					$('soldier' + parName).value = 0;
				}
			}
		}
	}
	else
	{
		for(var ii=0; ii<els.length; ii++)
		{
			if (els[ii].name && els[ii].name.indexOf('trans') != '-1')
			{
				parName = parseInt(els[ii].name.replace('trans', ''));

				localTotal = parseInt($('local_total' + parName).innerHTML);
				towerTotal = parseInt($('tower_total' + parName).innerHTML);
				soldierTotal = parseInt($('all_soldier').innerHTML);

				if( total <= soldierTotal )
				{
					alertDialog('keep', SGLang.TowerSoldierUpLimit);///old:grx:箭塔中的士兵数量已达上限！
					return;
				}

				if (!isNaN(parName) && !isNaN(localTotal) && localTotal > 0)
				{
					if( total - soldierTotal - localTotal >= 0 )
					{
						transNum = localTotal;
					}
					else
					{
						transNum = total - soldierTotal;
					}

					$('local_total' + parName).innerHTML = localTotal - transNum;
					$('tower_total' + parName).innerHTML = towerTotal + transNum;
					$('all_soldier').innerHTML = soldierTotal + transNum;
					$('soldier' + parName).value = towerTotal + transNum;

	//				alert($('local_total' + parName).innerHTML+" "+$('tower_total' + parName).innerHTML+" "+$('all_soldier').innerHTML+" "+$('soldier' + parName).value);
				}
			}
		}
	}
}


function startStateMission(act, form) {
	var mye=null;
	if ((mye=document.forms[form])==null) return;
	var els = mye.elements;

	parNum = parseInt(els['general'].value);

	if (!isNaN(parNum) && parNum > 0)
	{
		act += '&gid=' + parNum;
	} else {
		alertDialog('keep', SGLang.GeneralSelectTask);///old:grx:请选择执行任务的武将
		return;
	}

	MM_iframePost(act);
}

function demobGeneral(act, keep)
{
	var gourl = act;
	gourl += (keep == '' ? '' : '&keep=' + keep);
	confirmDialog(gourl, SGLang.GeneralConfirmDemob);///old:grx:您确定要放逐选定的武将么？
}

function rentGeneral(act, name, coin, period, keep)
{
	var gourl = act;
	gourl += (keep == '' ? '' : '&keep=' + keep);
	confirmDialog(gourl, SGLang.GeneralRentDays.replace(/\[AAA\]/g,
	name
	).replace(/\[BBB\]/g,
	coin
	).replace(/\[CCC\]/g,
	period
	));
}
///old:grx:'重金犒赏史实武将 '+name+' '+coin+'金币，<br/>'+name+' 的效命期增加'+period+'天'
function appointGeneral(act, keep)
{
	var gourl = act;
	gourl += (keep == '' ? '' : '&keep=' + keep);
	confirmDialog(gourl, SGLang.GeneralAppointConfirm.replace(/\[XXX\]/g,
	'<strong class="red">1000</strong>'
	));
}
///old:grx:'任命太守需要每种资源 <strong class="red">1000</strong> 单位，您确定要继续么？'
function disappointGeneral(act, keep)
{
	var gourl = act;
	gourl += (keep == '' ? '' : '&keep=' + keep);
	confirmDialog(gourl, SGLang.GeneralDisappointConfirm);///old:grx:您确定要撤销选定的武将的太守职务么？
}

function revivalGeneral(act, lumber, clay, iron, crop, keep)
{
	var gourl = act;
	gourl += (keep == '' ? '' : '&keep=' + keep);
	confirmDialog(gourl, SGLang.GeneralRevivalConfirm.replace(/\[AAA\]/g,
	lumber
	).replace(/\[BBB\]/g,
	clay
	).replace(/\[CCC\]/g,
	iron
	).replace(/\[DDD\]/g,
	crop
	));
}
///old:grx:召回选定的武将需要 <strong class="red">' + lumber + '</strong> 单位木材、<strong class="red">' + clay + '</strong> 单位石料、<strong class="red">' + iron + '</strong> 单位铁砂、<strong class="red">' + crop + '</strong> 单位粮食，您确定要继续么？
function renameGeneral(act, price)
{
	if (act == 0)
	{
		$('generalOptions').style.display = 'none';
		$('notice1').style.display = 'none';
		$('renameGeneral').style.display = '';
	}

	if (act != 0 && $('generalNewName'))
	{
		nameLength = getStrLength($('generalNewName').value);

		if (nameLength > 12 || nameLength < 4)
		{
			alertDialog('keep', SGLang.GeneralNameLenLimit);
			return false;
		}
		///old:grx:武将名字的长度必须在4-12个字符之内！
		act += '&name=' + encodeURIComponent($('generalNewName').value) + '&keep=right';
		//confirmDialog(act, '本操作需要扣除您 <strong class="red">' + price + '</strong> 个金币，您确定要继续么？');
		MM_locateAction(act);
	}
}


function needCoins()
{
	var price = arguments[0];
	var act = arguments[1];

	var msgstr = SGLang.ConsumeCoinConfirm.replace(/\[XXX\]/g,
	price
	);
	///old:grx:本操作需要扣除您 <strong class="red">' + price + '</strong> 个金币，您确定要继续么？
	confirmDialog(act, msgstr,'',1);
}

function checkAppoint(obj)
{
	var mye=null;
	if ((mye=document.forms['appointf'])==null) return;
	var els = mye.elements;
	for(var ii=0; ii<els.length; ii++)
	{
		if (els[ii] != obj && els[ii].value > 0 && els[ii].name && els[ii].name.indexOf('official') != '-1')
		{
			if (els[ii].value == obj.value) els[ii].selectedIndex = 0;
		}
	}
}

function setAppoint(act)
{
	var mye=null;
	var tmpArr=null;
	var confirmStr = '';
	var totalResos = 0;
	if ((mye=document.forms['appointf'])==null)return;
	var els = mye.elements;
	eval('var nowAppointInfo = '+$('json_appoint_info').value);

	for(var ii=0; ii<els.length; ii++)
	{
		if (els[ii].name && els[ii].name.indexOf('official') != '-1')
		{
			tmpArr = els[ii].name.split('|');
			parName = tmpArr[1];
			parValue = parseInt(els[ii].value, 10);

			if (!isNaN(parName) && !isNaN(parValue) && parValue >= 0)
			{
				act += '&official[' + parName + ']=' + parValue;
				/*
				if(parValue > 0 && (!nowAppointInfo[tmpArr[1]] || nowAppointInfo[tmpArr[1]] != parValue))
				{
					confirmStr += tmpArr[3]+'【'+tmpArr[2]+'】: 各项资源 x '+tmpArr[4]+'<br/>';
					totalResos += parseInt(tmpArr[4], 10);
				}
				*/
			}
		}
	}
	//confirmDialog(act,'新任命以下官职：<br/>'+confirmStr+'任命共需花费<font color="blue">各项资源</font> x <font color="red">'+totalResos+'</font>。确认执行任命吗？');
	MM_xmlLoad(act);
}


function showmenu(menuId, actuatorId)
{
	var menu = $(menuId);
 	var actuator = $(actuatorId);

 	if (menu == null || actuator == null) return;

	var display = menu.style.display;
	menu.style.display = (display == "none" ) ? "block" : "none";
	actuator.className = (display == "none" ) ? "slideup" : "slidedown";
	return false;
}

////检查帐号,帐号由大小写字母,数字,下划线组成,且首字符只能为字母
function isAccount(str)
{
	if(/^[a-zA-Z]{1}([a-zA-Z0-9]|[_]){3,15}$/.test(str))
    {
		return true;
    }
	else
	{
		return false;
	}
}

///返回密码强度等级,0为无效,1,2,3代表弱,中,强
function getPasswordLevel(s)
{
	if(s.length < 6 || s.length > 20)
	{
		return 0;
	}
	var ls = 0;
	if (s.match(/[a-z]/ig))
	{
		ls++;
	}
	if (s.match(/[0-9]/ig))
	{
		ls++;
	}
	 if (s.match(/(.[^a-z0-9])/ig))
	{
		ls++;
	}
	if (s.length < 6 && ls > 0)
	{
		ls--;
	}
	return ls
}
////搜索联盟成员,GET提交
function searchUser()
{
	var uname = $('Alliance_UserName').value;	//名字
	var obj_group = $('Alliance_GroupID');		//所属军团
	var obj_postion = $('Alliance_UserPosition');	//类型
	var str_group = '';
	var str_postion = '';
	for(var i=0; i<obj_group.length; i++){
		if(obj_group[i].selected)
			str_group= str_group+obj_group[i].value;
	}
	for(var i=0; i<obj_postion.length; i++){
		if(obj_postion[i].selected)
			str_postion= str_postion+obj_postion[i].value;
	}
	MM_iframePost("alliance.searchusersubmit&uname="+uname+"&gp="+str_group+"&pos="+str_postion);
}

///////如果子多选框有全部未选中,则父多选框也未选中,如果有至少一个选中,则父也选中,用于设定接收人处
function selectP(pid,obj_ck)
{
	var obj_li = $(pid).parentNode;
	var user_no = 0;
	var allcks = $('mainMenu').getElementsByTagName("input");
	var cer = 0;
	if(obj_ck.checked){	//选择一个多选框
		$(pid).checked = true;
		for(var i=0; i<allcks.length; i++)
		{
			if(allcks[i].type.toLowerCase() == "checkbox" && allcks[i].checked){
				if( allcks[i].value == obj_ck.value){
					cer = cer+1;
				}else{
					continue;

				}
			}
		}
		if(cer < 2)
		{
			user_no = user_no +1;
		}
		resetSelectV(obj_ck.value, true);

	}else{		//取消选择
		cer = 0;
		for(var i=0; i<allcks.length; i++)
		{
			if(allcks[i].type.toLowerCase() == "checkbox" && allcks[i].checked){
				if( allcks[i].value == obj_ck.value ){
					cer = cer+1;
				}else{
					continue;

				}
			}
		}
		if(cer < 1)
		{
			user_no = user_no -1;
		}
		var checkboxs = obj_li.getElementsByTagName("input");
		var n=0;
		for(i=0;i<checkboxs.length;i++)
		{
			if(checkboxs[i].type.toLowerCase() == "checkbox"){
				if( checkboxs[i].checked){
					n++;
				}
			}
		}
		if(n==1){
			$(pid).checked = false;
		}else{
			$(pid).checked = true;
		}
		resetSelectV(obj_ck.value, false);
	}
	if(isNaN(user_no) || user_no <0){
		user_no = 0;
	}
}

function selectChild(objP){
	var user_no = 0;
	var s='';
	var obj_li = objP.parentNode;
	var ischeck = true;
	if(objP.checked){
		ischeck = true;
		s='user_no = user_no + 1';
	}else{
		ischeck = false;
		s='user_no = user_no - 1';
	}
	var checkboxs = obj_li.getElementsByTagName("input");
	var allcks = $('mainMenu').getElementsByTagName("input");
	for(i=0;i<checkboxs.length;i++)
	{
		var cer=0;
		if(checkboxs[i].type.toLowerCase() == "checkbox" && checkboxs[i]!=objP ){
			checkboxs[i].checked=ischeck;
			for(var j=0; j<allcks.length; j++){
				if(allcks[j].type.toLowerCase() == "checkbox" && allcks[j].checked && checkboxs[i].value == allcks[j].value)
				{
					cer =cer+1;
				}else{
					continue;
				}

			}
			if(ischeck){
				resetSelectV(checkboxs[i].value, true);
				if(cer<2)
					eval(s);
			}else{
				resetSelectV(checkboxs[i].value, false);
				if(cer<1)
					eval(s);
			}
		}
	}
	if(isNaN(user_no) || user_no <0){
		user_no = 0;
	}

	//$('receive_user_no').innerHTML = user_no;
}

function resetSelectV(v,add)
{
	v = v+',';
	var selected_users = $('selected_users').value+',';
	var vindex = selected_users.indexOf(','+v);
	var slen = selected_users.length;
	if(add){
		if (vindex <0){
			$('selected_users').value = selected_users+v;
		}
	}else{
		if (vindex >=0)
		{
			selected_users = selected_users.substring(0,vindex) + selected_users.substring(vindex+v.length,slen);

		}
		$('selected_users').value = selected_users;
	}

}

////清除选中的多选框
function clearRaidoCheck(obj){
	var checkboxs = $('tongshuai_div').getElementsByTagName('input');
	for(i=0;i<checkboxs.length;i++)
	{
		if(checkboxs[i].type.toLowerCase() == "checkbox"){
			checkboxs[i].checked=false;
		}
	}
	if(obj.value==0){	///联盟指令,全部显示
		for(var i=0; i<6; i++){
			$("div_gp"+i).style.display="block";
		}
	}else{		///军团指令
		var gp = $("user_group_id").value;
		for(var i=0; i<6; i++){
			if(gp==i){
				$("div_gp"+i).style.display="block";
			}
			else{
				$("div_gp"+i).style.display="none";
			}
		}
	}
}

function getRadioValue(radioId){
	var vv=0;
	var form1 = $("sendform");
	for(var i=0; i<form1.Command_Level.length; i++){
		if(form1.Command_Level[i].checked){
			vv=form1.Command_Level[i].value;
			break;
		}
	}
	return vv;
}

function getRadioValue2(){
	var vv=0;
	var form1 = $("sendform");
	for(var i=0; i<form1.action_type.length; i++){
		if(form1.action_type[i].checked){
			vv=form1.action_type[i].value;
			break;
		}
	}
	return vv;
}

/////资源运输,点击要运输的数量,自动加上去
function trade_input_source(input_id, input_value)
{
	/////当前村庄资源值
	var v_lumber = $('village_Trader_Resource_Lumber').value;
	var v_clay = $('village_Trader_Resource_Clay').value;
	var v_iron = $('village_Trader_Resource_Iron').value;
	var v_crop = $('village_Trader_Resource_Crop').value;

	/////已经输入的各资源的值
	var input_lumber = $('Trader_Resource_Lumber').value;
	var input_clay = $('Trader_Resource_Clay').value;
	var input_iron = $('Trader_Resource_Iron').value;
	var input_crop = $('Trader_Resource_Crop').value;
	input_lumber = (isNaN(input_lumber) || input_lumber=='')?0:input_lumber;
	input_clay = (isNaN(input_clay) || input_clay=='')?0:input_clay;
	input_iron = (isNaN(input_iron) || input_iron=='')?0:input_iron;
	input_crop = (isNaN(input_crop) || input_crop=='')?0:input_crop;

	var input_sum = parseInt(input_lumber)+parseInt(input_clay)+parseInt(input_iron)+parseInt(input_crop);		///已输入资源总值

	var v_source = $('village_'+input_id).value;		///此次输入的某类村庄资源

	var iv = $(input_id).value;			///// 已经输入的值
	var source_max = $('load_sum').innerHTML;		////最大运载量
	iv = parseInt(iv);	///输入框值
	iv = isNaN(iv) ? 0 : iv;
	source_max = parseInt(source_max);	///最大运载量
	source_max = isNaN(source_max) ? 0 : source_max;


	var nowv = iv+input_value;

	if(input_sum+input_value > source_max)
	{
		nowv = (source_max - input_sum)+iv;
	}
	nowv = nowv>v_source ? v_source : nowv;		///输入的总值不能大于村庄资源值
	nowv = nowv>source_max ? source_max : nowv;	///输入的总值不能大于最大运载值
	$(input_id).value = nowv;
}

/////检查输入的值,是否为正整数,如果是整数,则返回该值,否则返回0
function check_is_int(v)
{
	v = parseInt(v);
	if(isNaN(v))
	v=0;
	return v;
}

function showFloatInfo(gname, geffect, gtime) {
	var myhtml = '';
	if(gtime == "-1")
	{
		myhtml = gname + '　<br />' + geffect + '　<br />'+SGLang.FloatInfoDisable;///old:grx:span style="color:#993366">尚未激活</span>，点击购买。
	}
	else
	{
		myhtml = gname + '　<br />' + geffect + '　<br />'+SGLang.FloatInfoEndTime+ gtime;///old:grx:结束于
	}
	$('floatinfo').innerHTML = myhtml;
	MM_showHidden('floatinfo','block');
}

function hideFloatInfo() {
	MM_showHidden('floatinfo','none');
}

function showMarketFAQ()
{
	$('floatblockright_content').innerHTML = $('marketFAQ').innerHTML;
	MM_showHidden('floatblockright','block');
}

function tradeSellConfirm()
{
	    var resosSell = document.system_trade_form.elements['Require_Resource_Type'].options[document.system_trade_form.elements['Require_Resource_Type'].selectedIndex].text;
	    var resosSellNum = document.system_trade_form.elements['Require_Resource_Count'].value;

	    var resosBuy = document.system_trade_form.elements['Provide_Resource_Type'].options[document.system_trade_form.elements['Provide_Resource_Type'].selectedIndex].text;
	    var resosBuyNum = document.system_trade_form.elements['Provide_Resource_Count'].value;

	    if( !(resosSellNum>0) || !(resosBuyNum>0) || isNaN(resosSellNum) || isNaN(resosBuyNum) )
	    {
	    		alertDialog('keep', SGLang.TradeResourceLimit);///old:grx:交易的资源值必须大于0
	    		return false;
	    }

		confirmDialog(function(){document.system_trade_form.action=document.system_trade_form.action+('&villageid='+w.villageid+'&rand='+(++w.rd));document.system_trade_form.submit();}, SGLang.TradeResourceConfirm.replace(/\[XXX\]/g,
		resosBuyNum+" "+resosBuy
		).replace(/\[YYY\]/g,
		resosSellNum+" "+resosSell
		));///old:grx:'您确定用'+resosBuyNum+resosBuy+'买入'+resosSellNum+resosSell+'资源吗？'
}

function tradeTransConfirm()
{
    var targetStr = "";
    if(!(document.trade_form.Trader_Resource_Lumber.value > 0
	     || document.trade_form.Trader_Resource_Clay.value > 0
	     || document.trade_form.Trader_Resource_Iron.value > 0
	     || document.trade_form.Trader_Resource_Crop.value > 0))
     {
	     	alertDialog('keep', SGLang.TradeResourceUnallowFull);///old:grx:资源数量不能全部都为空
	    	return false;
     }

    if( document.trade_form.Trader_Target_X.value >0 && document.trade_form.Trader_Target_Y.value >0 )
    {
    	targetStr = "("+document.trade_form.Trader_Target_X.value+","+document.trade_form.Trader_Target_Y.value+")";
    }
    else
    {
    	if( document.trade_form.village_name.value != "")
    	{
    		targetStr =document.trade_form.village_name.value;
    	}
    	else
    	{
		alertDialog('keep', SGLang.TradeTargetVillageUnit);///old:grx:请输入要运输的目标城市名或座标
		return false;
    	}
    }
    confirmDialog(function(){document.trade_form.action=document.trade_form.action+('&villageid='+w.villageid+'&rand='+(++w.rd));document.trade_form.submit();}, SGLang.TradeTargetConfirm.replace(/\[XXX\]/g,
    targetStr
    ));
}
///old:grx:'确定将资源运输到'+targetStr+'吗？'


function autoMove_selectVillage() {
	if (arguments[0] == 1) {
		document.getElementById("autoMove_self").disabled = false;
		document.getElementById("autoMove_vname").disabled = true;
		document.getElementById("autoMove_vx").disabled = true;
		document.getElementById("autoMove_vy").disabled = true;
	} else {
		document.getElementById("autoMove_self").disabled = true;
		document.getElementById("autoMove_vname").disabled = false;
		document.getElementById("autoMove_vx").disabled = false;
		document.getElementById("autoMove_vy").disabled = false;
	}
}

function autoMove_submit(act) {
	var mye=null;
	if ((mye=document.forms['autoMove_form'])==null) return;
	var els = mye.elements;

	for (var i=0; i<els.length; i++) {
		if (els[i].name == 'switch_automove' && els[i].checked == true) {
			act += "&switch_automove="+els[i].value;
		}

		if (els[i].name == 'soldier') {
			act += "&soldier" + els[i].value + "=" + (els[i].checked ? '1' : '0');
		}

		if (els[i].name == 'vtype' && els[i].value == 'self' && els[i].checked == true) {
			if ($('autoMove_self').value == 0) {
				alertDialog('keep', SGLang.TradeSelfVillage);///old:grx:请选择您的城镇
				return false;
			} else {
				act += "&target_vid=" + $('autoMove_self').value;
			}
		} else if (els[i].name == 'vtype' && els[i].value == 'other' && els[i].checked == true) {
			if ($('autoMove_vx').value != '' && $('autoMove_vy').value != '') {
				act += "&target_x=" + $('autoMove_vx').value;
				act += "&target_y=" + $('autoMove_vy').value;
			} else if ($('autoMove_vname').value != '' && $('autoMove_vname').value != SGLang.WarInputVillageName) {///old:grx:请输入城镇名称
				act += "&target_vname=" + $('autoMove_vname').value;
			} else {
				alertDialog('keep', SGLang.TradeSelfVillagename);///old:grx:请填写城镇名称或坐标
				return false;
			}
		}
	}

	MM_iframePost(act, "all");
}

function learnSkillBook(general_id)
{
  if(!general_id)
  {

	  if(!$('general_id') || !$('general_id').value)
	  {
	  	alertDialog('keep',SGLang.GeneralneedInfo);///old:grx:缺少武将信息
			return false;
		}
		var gid = $('general_id').value;
	}
	else
	{
		var gid = general_id;
	}
	var goods_id = $('item_goodsid').value;
	var goods_subid = $('item_goodssubid').value;
	var goods_name = $('item_goodsname').value;
	var goods_isbind = $('item_isbind').value;
	var goods_istrade = $('item_istrade').value;
	var bookid1 = $('bookid1').value;
	var bookid2 = $('bookid2').value;
	var bookid3 = $('bookid3').value;

	if(bookid1 == -1 && bookid2 == -1 && bookid3 == -1)
	{
		alertDialog('keep',SGLang.SkillItemNoOpenOne);///old:grx:当前没有开启的技能栏
		return;
  }
  if(bookid1 > 0 && bookid2 > 0 && bookid3 > 0)
	{
		alertDialog('keep',SGLang.SkillItemNoFreeItem);///old:grx:没有空余的技能栏，请先遗忘一本技能书
		return;
  }

  var msg = '<form name="formLearnSkillBook">';
  msg += '<h4>'+SGLang.SkillItemByBook.replace(/\[XXX\]/g,
  goods_name
  )+'</h4>';///old:grx:将技能书 '+goods_name+' 学习到以下技能栏
  for(var i=1;i<=3;i++)
  {
  	msg += '<input type="radio" name="radioSkillSwitch" value="'+i+'" ';
  	if(eval('bookid'+i) != 0) msg += ' disabled="true" ';
  	msg += ' />'+SGLang.SkillItemNum+i+' ';///old:grx:技能栏
  }
  msg += '</form>';

  MM_showDialog(msg,'',  [
	{
		title:w.lang.dialog_confirm_button,
		act:function(){
			var switchid = getRadioObjValue(document.forms['formLearnSkillBook'].radioSkillSwitch);
			MM_closeDialog();
			MM_xmlLoad('general.learnSkillBook&gid='+gid+'&goodsid='+goods_id+'&bookid='+goods_subid+'&switchid='+getRadioObjValue(document.forms['formLearnSkillBook'].radioSkillSwitch)+'&isbind='+goods_isbind+'&istrade='+goods_istrade+'&keep=all');
		}
	},
	{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();}}
  ]);
}

function learnStuntBook(general_id)
{
	if(!general_id)
  {
		if(!$('general_id') || !$('general_id').value)
	  {
	  	alertDialog('keep',SGLang.GeneralneedInfo);///old:grx:缺少武将信息
			return;
		}
		var gid = $('general_id').value;
	}
	else
	{
		var gid = general_id;
	}
	var goods_id = $('item_goodsid').value;
	var goods_subid = $('item_goodssubid').value;
	var goods_name = $('item_goodsname').value;
	var goods_isbind = $('item_isbind').value;
	var goods_istrade = $('item_istrade').value;
	var stunt_bookid1 = $('stunt_bookid1').value;
	var stunt_bookid2 = $('stunt_bookid2').value;
	var stunt_bookid3 = $('stunt_bookid3').value;

	//alert(goods_id+":"+goods_subid+":"+goods_name+":"+goods_isbind+":"+stunt_bookid1+":"+stunt_bookid2+":"+stunt_bookid3);

	if(stunt_bookid1 == -1 && stunt_bookid2 == -1 && stunt_bookid3 == -1)
	{
		alertDialog('keep',SGLang.GeneralMenumijiNo);///old:grx:当前没有开启的秘籍栏
		return;
  }
  if(stunt_bookid1 > 0 && stunt_bookid2 > 0 && stunt_bookid3 > 0)
	{
		alertDialog('keep',SGLang.GeneralMenuallFull);///old:grx:没有空余的秘籍栏，请先遗忘一本秘籍
		return;
  }

  var msg = '<form name="formLearnStuntBook">';
  msg += '<h4>'+SGLang.GeneralStudyMiji.replace(/\[XXX\]/g,
			goods_name
			)+'</h4>';///old:grx:将秘籍 '+goods_name+' 学习到以下秘籍栏
  for(var i=1;i<=3;i++)
  {
  	msg += '<input type="radio" name="radioSkillSwitch" value="'+i+'" ';
  	if(eval('stunt_bookid'+i) != 0) msg += ' disabled="true" ';
  	msg += ' />'+SGLang.GeneralMenumiji+i+' ';
  }///old:grx:秘籍栏
  msg += '</form>';

  MM_showDialog(msg,'',  [
	{
		title:w.lang.dialog_confirm_button,
		act:function(){
			var switchid = getRadioObjValue(document.forms['formLearnStuntBook'].radioSkillSwitch);
			MM_closeDialog();
			MM_xmlLoad('general.learnStuntBook&gid='+gid+'&goodsid='+goods_id+'&bookid='+goods_subid+'&switchid='+getRadioObjValue(document.forms['formLearnStuntBook'].radioSkillSwitch)+'&isbind='+goods_isbind+'&istrade='+goods_istrade+'&keep=all');
		}
	},
	{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();}}
  ]);
}


// 将formname表单中的多选框转为提交字符串
function checkbox2str(formname)
{
	var tmp_str = '';
	var formelements = document.forms[formname].elements;
	for ( i = 0; i < formelements.length; i++)
	{
		if (formelements[i].type.toLowerCase() == "checkbox" && formelements[i].checked == true)
			tmp_str += '&' + formelements[i].name + '=' + formelements[i].value;
	}
	return tmp_str;
}
function addcontribute()
{
	var goodsid = arguments[0];
	var goodssubid = arguments[1];
	var goodsname = arguments[2];
	var contribute = arguments[3];
	var usenum = $('goods_contributenum').value.trim();
	if(usenum == '' || isNaN(usenum) || parseInt(usenum, 10)<=0 )
	{
		alertDialog('keep',SGLang.GoodsContributeNumError);///old:grx:数量输入错误
		return;
	}
	usenum = parseInt(usenum, 10);
	var add_contribute = contribute * usenum;
	confirmDialog('emperor.addContribute&goodsid='+goodsid+'&goodssubid='+goodssubid+'&usenum='+usenum,SGLang.GoodsContributeConfirm.replace(/\[AAA\]/g,
	usenum).replace(/\[BBB\]/g,
	goodsname).replace(/\[CCC\]/g,
	add_contribute),'',1);///old:grx:确定献纳'+usenum+'个'+goodsname+'（提高'+add_contribute+'点贡献度）吗'
}

function getPra(url,parameter){
	var reg,url2,iLen,iStart,iEnd;
	reg = new RegExp(parameter);
	if(url.search(reg) == -1){
		return "";
	}
	else{
		iLen = parameter.length;
		iStart = url.search(reg) + iLen +1;
		url2 = url.substr(iStart);
		iEnd = iStart + url2.search(/&/i);
		if((iStart - 1) == iEnd){
			return url.substr(iStart);
		}
		else{
			return url.substr(iStart,iEnd - iStart);
		}
	}
}


/*
	1.  onclick
	2.	sum
	3.	num
	4.	div_id
	5.	style
	6.	curPage
*/
function pageCreate()
{
	var url = arguments[0] ? arguments[0] : '';		//url
	var total = parseInt(arguments[1]) ? parseInt(arguments[1]) : 0;	//总记录数
	var num = parseInt(arguments[2]) ? parseInt(arguments[2]) : 10;		//每页多少条
	var div_id = arguments[3] ? arguments[3] : '';		//在插入的DIV的ID
	var style = arguments[4] ? arguments[4] : 'default';	//样式CSS
	var cur_page = parseInt(arguments[5]) ? parseInt(arguments[5]) : 0;		//当前页码
	var act = arguments[6] ? arguments[6] : 'MM_iframePost'; //onclick的事件
	var keep = arguments[7] ? arguments[7] : ''; //keep

	var page_sum = Math.ceil(total/num);			//总页数
	if (page_sum<1)
	{
		page_sum = 1;
	}
	var pagehtml = '';
	if (act == '')
	{
		return;
	}

	if(cur_page == null || isNaN(cur_page) || cur_page == 0) //没有设定显示第几页,
	{
		//cur_page = parseInt(getPra(url,'nowpage'));
		//if ( cur_page == null || cur_page == 'NaN' || cur_page == 0 )
		{
			cur_page = 1;
		}
	}

	if (style == 'sggame')
	{
		pagehtml = page_sggame(url, act, keep, cur_page, page_sum);
	}else if (style == 'sggame2')
	{
		pagehtml = page_sggame2(url, act, keep, cur_page, page_sum);
	}else if (style == 'default')
	{
        pagehtml = getPageHtml(url, act, keep, cur_page, page_sum);
	}else{
		pagehtml = getPageHtml(url, act, keep, cur_page, page_sum);
	}
	//pagehtml = pagehtml.replce("'", "\'");
	//alert(pagehtml);
	document.getElementById(div_id).innerHTML = pagehtml;
}

function getPageHtml(url, act, keep, cur_page, page_sum )
{
	var pagehtml = '';
	//var cur_page;
	//var page_sum;
	//var url;
	cur_page = parseInt(cur_page);
	if (isNaN(cur_page) || cur_page == null) cur_page = 1;
	if(cur_page > page_sum) cur_page = page_sum;
	if(cur_page <1) cur_page = 1;
	//var act ;
	if(cur_page == 1){
		pagehtml += SGLang.PageFirst+'&nbsp;'+SGLang.PagePrevious+'&nbsp;';///old:grx:首页   上一页
	}else{
		act = getPageAct(url, act, keep, 1, cur_page, page_sum);
		pagehtml += '<a href="#" onclick="'+act+'">'+SGLang.PageFirst+'&nbsp;';///old:grx:首页
		act = getPageAct(url, act, keep, 2, cur_page, page_sum);
		pagehtml += '<a href="#" onclick="'+act+'">'+SGLang.PagePrevious+'&nbsp;';///old:grx:上一页
	}
	if (cur_page >= page_sum)
	{
		pagehtml += SGLang.PageNext+'&nbsp;'+SGLang.PageLast+'&nbsp;';///old:grx:下一页  末页
	}else{
		act = getPageAct(url, act, keep, 3, cur_page, page_sum);
		pagehtml += '<a href="#" onclick="'+act+'">'+SGLang.PageNext+'&nbsp;';///old:grx:下一页
		act = getPageAct(url, act, keep, 4, cur_page, page_sum);
		pagehtml += '<a href="#" onclick="'+act+'">'+SGLang.PageLast+'&nbsp;';///old:grx:末页
	}
	return pagehtml;
}

function page_sggame(url, act, keep, cur_page, page_sum )
{
	var pagehtml = '';
	//var cur_page;
	//var page_sum;
	//var url;
	cur_page = parseInt(cur_page);
	if (isNaN(cur_page) || cur_page == null) cur_page = 1;
	if(cur_page > page_sum) cur_page = page_sum;
	if(cur_page <1) cur_page = 1;
	//var act ;
	if(cur_page == 1){
		pagehtml += '<span style="color:#999;">'+SGLang.PageFirst+'</span>&nbsp;<span>|</span>&nbsp;<span style="color:#999;">'+SGLang.PagePrevious+'</span>&nbsp;<span>|</span>&nbsp;';///old:grx:首页  前页
	}else{
		act = getPageAct(url, act, keep, 1, cur_page, page_sum);
		pagehtml += '<span style="color:#0d5d09; cursor:pointer;" onclick="'+act+'">'+SGLang.PageFirst+'</span>&nbsp;<span>|</span>&nbsp;';
		act = getPageAct(url, act, keep, 2, cur_page, page_sum);///old:grx:首页
		pagehtml += '<span style="color:#0d5d09; cursor:pointer;" onclick="'+act+'">'+SGLang.PagePrevious+'</span>&nbsp;<span>|</span>&nbsp;';///old:grx:前页
	}
	pagehtml += '<span>' + cur_page + '/' + page_sum + '</span>&nbsp;<span>|</span>&nbsp';
	if (cur_page >= page_sum)
	{
		pagehtml += '<span style="color:#999;">'+SGLang.PageNext+'</span>&nbsp;<span>|</span>&nbsp;<span style="color:#999;">'+SGLang.PageLast+'</span>&nbsp;';///old:grx:下页  尾页
	}else{
		act = getPageAct(url, act, keep, 3, cur_page, page_sum);
		pagehtml += '<span style="color:#0d5d09; cursor:pointer;" onclick="'+act+'">'+SGLang.PageNext+'</span>&nbsp;<span>|</span>&nbsp;';///old:grx:下页
		act = getPageAct(url, act, keep, 4, cur_page, page_sum);
		pagehtml += '<span style="color:#0d5d09; cursor:pointer;" onclick="'+act+'">'+SGLang.PageLast+'</span>&nbsp;';///old:grx:尾页
	}
	return pagehtml;
}

function page_sggame2(url, act, keep, cur_page, page_sum )
{
	var pagehtml = '';
	//var cur_page;
	//var page_sum;
	//var url;
	cur_page = parseInt(cur_page);
	if (isNaN(cur_page) || cur_page == null) cur_page = 1;
	if(cur_page > page_sum) cur_page = page_sum;
	if(cur_page <1) cur_page = 1;
	//var act ;
	if(cur_page == 1){
		pagehtml += '<span style="color:#999;">'+SGLang.PagePrevious+'</span>&nbsp;|&nbsp;';///old:grx:前页
	}else{
		//act = getPageAct(url, act, keep, 1, cur_page, page_sum);
		//pagehtml += '<span style="color:#0d5d09; cursor:pointer;" onclick="'+act+'">首页</span>&nbsp;|&nbsp;';
		act = getPageAct(url, act, keep, 2, cur_page, page_sum);
		pagehtml += '<span style="color:#0d5d09; cursor:pointer;" onclick="'+act+'">前页</span>&nbsp;|&nbsp;';
	}
	//pagehtml += cur_page + '/' + page_sum + '&nbsp;|&nbsp';
	if (cur_page >= page_sum)
	{
		pagehtml += '<span style="color:#999;">'+SGLang.PageNext+'</span>';///old:grx:下页
	}else{
		act = getPageAct(url, act, keep, 3, cur_page, page_sum);
		pagehtml += '<span style="color:#0d5d09; cursor:pointer;" onclick="'+act+'">'+SGLang.PageNext+'</span>';///old:grx:下页
		//act = getPageAct(url, act, keep, 4, cur_page, page_sum);
		//pagehtml += '<span style="color:#0d5d09; cursor:pointer;" onclick="'+act+'">尾页</span>&nbsp;';
	}
	return pagehtml;
}


function getPageAct(url, act, keep, type, cur_page, page_sum)
{
	var new_page;
	cur_page = parseInt(cur_page);
	if (isNaN(cur_page) || cur_page == null) cur_page = 1;
	page_sum = parseInt(page_sum);
	if(act != 'MM_xmlLoad') act = 'MM_iframePost';
	if (type == "1")	//首页
	{
		act += '(\''+url+'&amp;nowpage=1\'';
	}else if(type == "2"){	//上一页
		new_page = cur_page - 1;
		new_page = new_page < 1 ? 1 :new_page;
		act += '(\''+url+'&amp;nowpage='+new_page+"\'";
	}else if(type == "3"){	//下一页
		new_page = cur_page + 1;
		new_page = new_page > page_sum ? page_sum :new_page;
		act += '(\''+url+'&amp;nowpage='+new_page+"\'";
	}else if(type == "4"){	//末页
		act += '(\''+url+'&amp;nowpage='+page_sum+"\'";
	}else{
		act += '(\''+url+'&amp;nowpage=1\'';
	}

	if (typeof(keep)=="string" && (keep=="right" || keep=="left" || keep=="all"))
	{
		act += ",'"+keep+"')";
	}else{
		act += ")";
	}
	return act;

}

function page_default()
{
	var page_html = '';

}

function getUserInfo()
{
	return w.userid;
}
//聊天
function chatchannel() {
	var ele = $("selectchannel");
	ele.style.display = (ele.style.display == "block") ? "none" : "block";
	var elesub = ele.getElementsByTagName("a");
	for ( var i = 0; i < elesub.length ;  i++) {
		elesub[i].onclick = function() {
			$("channel").innerHTML = this.innerHTML;
			ele.style.display = "none";
		}
	}
}

function general_rename(gid) {
	MM_showDialog(
		'<p>'+SGLang.GeneralRenameInput+'<input type="text" id="dialog_input" value="" />'+'</p>', '', [
			{title:w.lang.dialog_modify_button,act:function(){if ($('dialog_input').value.trim()!='') {MM_closeDialog();MM_iframePost('general.rename&gid='+gid+'&name='+encodeURIComponent($('dialog_input').value));}}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();}}
		]);
}
///old:grx:请输入武将名称：（4-12个字符）

function equipment() {
	var listObj = $('equipmentlist');
	var ele = listObj.getElementsByTagName("li");
	var eleoption = $("equipment_operate");
	for (var i = 0; i < ele.length ; i++ )
	{
		ele[i].onclick = function() {
			eleoption.style.display = "block";
			eleoption.style.top = Math.min(listObj.clientHeight-10,this.parentNode.offsetTop - listObj.scrollTop + ((w.isIE && !w.isIE8)?75:40)) +"px";
			eleoption.style.left = this.offsetLeft + 4 +"px";
		}
		var optionele = eleoption.getElementsByTagName("li");
		for (var j = 0; j < optionele.length ; j++ )
		{
			optionele[j].onclick = function() {this.parentNode.style.display = "none"}
		}
	}
	listObj.onscroll = function(){
		$("equipment_operate").style.display = "none";
	}

	var listObj2 = $('equipmentlist_skillbook');
	var ele2 = listObj2.getElementsByTagName("li");
	var eleoption2 = $("equipment_operate_skillbook");
	for (var i = 0; i < ele2.length ; i++ )
	{
		ele2[i].onclick = function() {
			eleoption2.style.display = "block";
			eleoption2.style.top = Math.min(listObj2.clientHeight-10,this.parentNode.offsetTop - listObj2.scrollTop + ((w.isIE && !w.isIE8)?75:40)) +"px";
			eleoption2.style.left = this.offsetLeft + 4 +"px";
		}
		var optionele = eleoption2.getElementsByTagName("li");
		for (var j = 0; j < optionele.length ; j++ )
		{
			optionele[j].onclick = function() {this.parentNode.style.display = "none"}
		}
	}
	listObj2.onscroll = function(){
		$("equipment_operate_skillbook").style.display = "none";
	}

	var listObj3 = $('equipmentlist_stuntbook');
	var ele3 = listObj3.getElementsByTagName("li");
	var eleoption3 = $("equipment_operate_stuntbook");
	for (var i = 0; i < ele3.length ; i++ )
	{
		ele3[i].onclick = function() {
			eleoption3.style.display = "block";
			eleoption3.style.top = Math.min(listObj3.clientHeight-10,this.parentNode.offsetTop - listObj3.scrollTop + ((w.isIE && !w.isIE8)?75:40)) +"px";
			eleoption3.style.left = this.offsetLeft + 4 +"px";
		}
		var optionele = eleoption3.getElementsByTagName("li");
		for (var j = 0; j < optionele.length ; j++ )
		{
			optionele[j].onclick = function() {this.parentNode.style.display = "none"}
		}
	}
	listObj3.onscroll = function(){
		$("equipment_operate_stuntbook").style.display = "none";
	}
}

function generalEquipItem(gid)
{
   if(!gid)
   {
   	var general_id = parseInt($('general_id').value);
   }
   else
   {
   	var general_id = gid;
   }

   var general_level = parseInt($('general_level').value);
   var item_general_level = parseInt($('item_general_level').value);

   if(!general_id){alertDialog('keep',SGLang.GeneralInfoError);}///old:grx:武将信息错误

   if(general_level < item_general_level)
   {
   	 alertDialog('keep',SGLang.GeneralNeedLevel.replace(/\[XXX\]/g,
		item_general_level));///old:grx:'装备需要武将等级达到'+item_general_level
   }
   else
   {
   	 MM_xmlLoad('general.changeitem&gid='+general_id+'&goodsid='+$('item_goodsid').value+'&goodssubid='+$('item_goodssubid').value+'&isbind='+$('item_isbind').value+'&istrade='+$('item_istrade').value+'&keep=all');
   }
}

// 取flash对象
function getFlashObject(objectId) {
	if (w.isIE) {
		return window[objectId];
	} else {
		return document[objectId];
	}
}

//通用下拉条
function commonlist(id) {
	var ele = document.getElementById(id);
	var elesubtag = ele.getElementsByTagName("ul")[0];
	elesubtag.style.display = "block";
	var option = elesubtag.getElementsByTagName("li");
	for (var i = 0; i < option.length ; i++ )
	{
		option[i].onclick = function() {
			ele.getElementsByTagName("span")[0].innerHTML = this.innerHTML;
			elesubtag.style.display = "none";
		}
		option[i].onmouseover = function() {
			this.style.background = "#444"
		}
		option[i].onmouseout = function() {
			this.style.background = "#2f2f2f"
		}
	}

	var allarea = document.body;
		allarea.onclick = function() {
			elesubtag.style.display = "none";
		}
}

function showInviteCoinType(msg,act,coinNum1,coinNum2)
{
	var price1_by_tickets = arguments[4];
	var price1_card_num = arguments[5];
	var price2_by_tickets = arguments[6];
	var price2_card_num = arguments[7];
	MM_showDialog(
		'<p>'+msg+'</p>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){submitInviteConiType(act,coinNum1,coinNum2,price1_by_tickets,price1_card_num,price2_by_tickets,price2_card_num);}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();}}
		]);
}
function submitInviteConiType()
{
	var act = arguments[0];
	var coinNum1 = arguments[1];
	var coinNum2 = arguments[2];

	var radioObj = document.cointype_form.cointype_choose;
	var type = 0;

	for(var i=0;i<radioObj.length;i++)
	{

		if(radioObj[i].checked == true)
		{
			type = radioObj[i].value;
			break;
		}
	}
	if(type == 1)
	{
		act += '&type=1';
		if (arguments[3] == 1)
		{
			consume(act, SGLang.ConsumeCoinConfirm.replace(/\[XXX\]/g,
	coinNum1
	));
		}
		else
		{
			confirmDialog(act,SGLang.ConsumeCoinConfirm.replace(/\[XXX\]/g,
	coinNum1
	), '', 1);
		}

		return;
	}///old:grx:本操作需要扣除您<strong class=\'red\'>'+ coinNum1 +'</strong>个金币，您确定要继续吗?
	if(type == 2)
	{
		act += '&type=2';
		if (arguments[5] == 1)
		{
			consume(act,SGLang.ConsumeCoinConfirm.replace(/\[XXX\]/g,
	coinNum2
	));
		}
		else
		{
			confirmDialog(act, SGLang.ConsumeCoinConfirm.replace(/\[XXX\]/g,
	coinNum2
	), '',1);
		}
		return;
	}	///old:grx:本操作需要扣除您<strong class=\'red\'>'+ coinNum2 +'</strong>个金币，您确定要继续吗?
	if(type != 0){MM_closeDialog();}
}
function checkGoodsTickets()
{
	if (arguments[0])
	{
		$('bestowuname').disabled=false;
	}
	else
	{
		$('bestowuname').disabled=true;$('check_send').parentNode.style.color='#ddd';
	}
	$('goods_number').value = 0;
	$('totalprice').innerHTML = (arguments[0] == 0) ? SGLang.TicketZero : SGLang.CoinsZero;///old:grx:0礼金券 0金币
}
function MM_submitGoods()
{
	var gid = parseInt(arguments[0]);
	var price = parseInt(arguments[1]);
	var numb = parseInt($('goods_number').value) + 0;
	var goodsname = $('goodsname').innerHTML;
	var coins = parseInt($('totalprice').innerHTML);

	if (typeof(numb)!="number" || isNaN(numb) || numb<1)
	{
		alertDialog('keep',SGLang.GoodsBuyNumError);///old:grx:请输入您想购买的商品数量（大于0）
		return;
	}

	var frd = "";

	if ($('check_send') != null && $('check_send').checked)
	{
		if ($('bestowuname').value == "")
		{
			alertDialog('keep',SGLang.GoodsBestowName);///old:grx:请输入赠送用户的名称
			return;
		}
		frd = $('bestowuname').value;
	}

	if (frd != "") {
		tmpstrforstore = "&friend="+encodeURIComponent(frd);
		confirmAddStr = SGLang.GoodsBestowUserConfirm.replace(/\[XXX\]/g,
		frd
		);///old:grx:'并将其赠与 <font color="green">'+ frd + '</font> '
	} else {
		tmpstrforstore = '';
		confirmAddStr  = '';
	}

	var confirmStr = SGLang.ConsumeCoinConfirm.replace(/\[XXX\]/g,
	coins
	);///old:grx:'此次消费将扣除您<font color="red">'+coins+'金币</font>，是否确定？'
	confirmDialog("store.buygoods&id="+gid+"&num="+numb+tmpstrforstore+"&price="+price+"&keep=all", confirmStr,'','','','',1);
}
function MM_setGoodsNumber()
{
	var price = arguments[0];
	var goldcoin_max = arguments[1];

	var numb = parseInt($('goods_number').value.replace(/^0+/,'')) + 0;
	if (typeof(numb)!="number" || isNaN(numb)) numb = 0;
	var numb_max = Math.floor( goldcoin_max / price );
	if (numb < 0) numb = 0;
	if (numb>numb_max) numb = numb_max;
	$('goods_number').value = (typeof(numb)!="number" || isNaN(numb)) ? 0 : numb;
	$('totalprice').innerHTML = numb * price;
	$('totalprice').innerHTML += SGLang.ConsumeCoin///old:grx:金币
	var coin_left = (typeof(numb)!="number" || isNaN(numb)) ? goldcoin_max : (goldcoin_max - numb * price);
	$('remain').innerText = coin_left .toString();
	$('remain').innerText += SGLang.ConsumeCoin;///old:grx:金币
}

function buyProp()
{
	var prop_num = arguments[0];
	var by_tickets = arguments[1];
	var user_tickets = arguments[2];
	var unit_price = arguments[3];
	var card_num = arguments[4];

	prop_num = parseInt(prop_num.replace(/^0+/,'')) + 0;
	if (typeof(prop_num)!="number" || isNaN(prop_num)) prop_num = 0;
	if (prop_num <= 0)
	{
		alertDialog('keep',SGLang.GoodsContributeNumError);///old:grx:请输入正确的数量
		return false;
	}
	var total_price = prop_num*unit_price;

	confirmDialog('alliance.buyprop&gnum='+prop_num , SGLang.ConsumeCoinConfirm.replace(/\[XXX\]/g,
	total_price
	));
	///old:grx:'本操作需要扣除您<strong class=\'red\'>'+ total_price +'</strong>个金币，您确定要继续吗？'
/*
	if (by_tickets == 0 || total_price > user_tickets)
	{
		confirmDialog('alliance.buyprop&gnum='+prop_num , '本操作需要扣除您<strong class=\'red\'>'+ total_price +'</strong>个金币，您确定要继续吗？');
	}
	else
	{
		consume('alliance.buyprop&amp;gnum='+prop_num , '本操作需要扣除您<strong class=\'red\'>'+ total_price +'</strong>个金币，您确定要继续吗？|$|'+card_num);
	}
*/
}

function remainGoodsNumber()
{
	var goods_max = arguments[0];
	var numb = parseInt($('bestows_goods_number').value.replace(/^0+/,'')) + 0;
	if (typeof(numb)!="number" || isNaN(numb)) numb = 0;
	if (numb < 0) numb = 0;
	if (numb > goods_max) numb = goods_max;
	$('bestows_goods_number').value = Math.min(numb,goods_max);
	$('bestows_goods_remaining').innerHTML = goods_max - numb;
}

function submitGifts()
{
	var id = arguments[0];
	var subid = arguments[1];
	var goodsname = arguments[2];
	var istrade = arguments[3];
	if (typeof(id)!="number" || isNaN(id) || id<1)
	{
		alertDialog('keep',SGLang.GoodsIdError);///old:grx:错误的道具ＩＤ
		return;
	}

	var numb = parseInt($('bestows_goods_number').value) + 0;
	if (typeof(numb)!="number" || isNaN(numb) || numb<1)
	{
		alertDialog('keep',SGLang.GoodsinputBestowNum);///old:grx:请输入您想赠送的道具数量（大于0）
		return;
	}

	var frd = $('bestows_goods_friend').value;
	if (frd == "")
	{
		alertDialog('keep',SGLang.GoodsBestowName);///old:grx:请输入赠送用户的名称
		return;
	}

	var confirmStr = SGLang.GoodsBestowNum.replace(/\[AAA\]/,
	goodsname
	).replace(/\[BBB\]/,
	numb
	).replace(/\[CCC\]/,
	frd
	);
	///old:grx:您要将 <font color="blue">'+goodsname+'</font> <font color="red">× '+numb+'</font> 赠与 <font color="green">'+ frd + '</font> 吗？
	confirmDialog("store.giveitem&id="+id+"&subid="+subid+"&numb="+numb+"&istrade="+istrade+"&friend="+encodeURIComponent(frd)+"&keep=all", confirmStr);
}

function getRadioObjValue(obj)
{
	if(!obj || typeof(obj)!='object') return false;
	var rs = false;

	if(obj.length > 0)
	{
		for(i=0; i<obj.length; i++)
		{
			if(obj[i].checked)
			{
			   rs = obj[i].value;
			}
		}
	}
	else
	{
		if(obj.checked) rs = obj.value;
	}
	return rs;
}

function MM_showBuildHelp(id1,id2,id3,id4,id5,id6)
{
	$(id1).className = "now";
	$(id2).style.display = "block";
	$(id3).className = "normal";
	$(id4).style.display = "none";
	$(id5).className = "normal";
	$(id6).style.display = "none";

}

function alliance_addpro(p1,pos,gid) {
	MM_showDialog(
		'<p>'+p1+'<input type="text" id="dialog_input" value="" /></p>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){if ($('dialog_input').value!='') {MM_closeDialog();MM_iframePost('alliance.addprovince&uname='+encodeURIComponent($('dialog_input').value)+'&pos='+pos+'&gid='+gid);return false;}}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();return false;}}
		]);
}

function alliance_create(p1,p2) {
	MM_showDialog(
		'<p>'+p1+'<input type="text" id="dialog_input" value="" />'+p2+'</p>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){if ($('dialog_input').value!='') {MM_closeDialog();MM_iframePost('alliance.create&aname='+encodeURIComponent($('dialog_input').value));return false;}}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();return false;}}
		]);
}

function alliance_inherit(p1,p2) {
	MM_showDialog(
		'<p>'+p1+'<input type="text" id="dialog_input" value="" />'+p2+'</p>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){if ($('dialog_input').value!='') {MM_closeDialog();MM_iframePost('alliance.inherit&uname='+encodeURIComponent($('dialog_input').value));return false;}}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();return false;}}
		]);
}

function showBuff(o,m) {
	ui.describe(o,m,'indexbuff');
}

function setTaskStatus(ut_id,finish)
{
	if(finish == 1) $('tasklist_'+ut_id).className = 'over';
	else $('tasklist_'+ut_id).className = 'no';
}

function setTaskIconStatus(typeInfo,showTaskInfo)
{
	//typeInfo: id|finish|new|num
	var type1_id = typeInfo[0];
	var type1_finish = typeInfo[1];
	var type1_new = typeInfo[2];
	var type1_num = typeInfo[3];

	if(type1_finish == 1)
	{
		$('taskstatus1').style.display = "block";
		$('taskstatus2').style.display = "none";
	}
	else
	{
		if(type1_new ==1)
		{
			$('taskstatus1').style.display = "none";
		  $('taskstatus2').style.display = "block";
		}
		else
		{
			$('taskstatus1').style.display = "none";
		  $('taskstatus2').style.display = "none";
		}
	}

	if(showTaskInfo != '')
	{
		//showTaskInfo: ut_id|task_id|subtype_id|name|finish|read
		var imgStr = "";
		if(showTaskInfo[5] == 0){
			imgStr = '<img src="'+w.img_src+'images/tasknew.gif"/>';
		}
		else
		{
			if(showTaskInfo[4] == 1){
				imgStr = '<img src="'+w.img_src+'images/taskover.gif"/>';
			}
			else
			{
				imgStr = '<img src="'+w.img_src+'images/taskno.gif"/>';
			}
		}
		$('taskFloat').innerHTML = imgStr+'<a href="javascript:void(0);" onclick="MM_xmlLoad(\'task.main&type_id=10&subtype_id='+showTaskInfo[2]+'&task_id='+showTaskInfo[1]+'&seticon=1\');" ><font>'+SGLang.TaskTitle+showTaskInfo[3]+'('+type1_num+')</font></a>';
		$('taskFloat').style.display = "block";
	}
	else
	{
		$('taskFloat').innerHTML = "";
		$('taskFloat').style.display = "none";
	}

}

var shoptips = {

	shoptips_init : function()
	{
		var dobj = $('shoptis_div');
		if (!dobj) {
			dobj = d.createElement('div');
			dobj.id = 'shoptis_div';
			d.body.appendChild(dobj);
		}
		dobj.style.position = 'absolute';
		dobj.className = "shoptips";
		dobj.style.display = 'block';
		dobj.style.zIndex = '9200';
		return dobj;
	},

	shoptips_over : function(event, msg)
	{
		var dobj = $('shoptis_div');
		if (!dobj){
			try{shoptips.shoptips_init();$('shoptis_div').innerHTML = msg;}catch(e){}
		}else{
			try{dobj.innerHTML = msg;}catch(e){}
		}
	},
	shoptips_move : function(event)
	{
		var dobj = $('shoptis_div');
		var left = 0;
		var top  = 0;
		if (!dobj){
			dobj = shoptips.shoptips_init();
		}
		dobj.style.display = 'block';
		var mouse = ui.getMouseLocation(event);
		left = Math.min($('wrapper').offsetWidth + $('wrapper').offsetLeft - dobj.offsetWidth, mouse.x - 5);
		if ((dobj.offsetHeight + mouse.y + 23) > $('wrapper').offsetHeight) {
			if (dobj.offsetHeight > (mouse.y + 23)) {
				top = Math.round(($('wrapper').offsetHeight - dobj.offsetHeight) / 2);
				left += 20;
				if (dobj.offsetWidth < (mouse.x + 23)) {
					left = mouse.x - dobj.offsetWidth - 20;
				}
			} else {
				top = mouse.y - 5 - dobj.offsetHeight;
			}
		} else {
			top = mouse.y + 23;
		}
		dobj.style['left'] = left + 'px';
		dobj.style['top'] = top + 'px';
		return false;
	},

	shoptips_out : function(event)
	{
		var dobj = $('shoptis_div');
		if (!dobj){
			dobj = shoptips.shoptips_init();
		}
		dobj.style.display = 'none';
	}
};
noticeReadStart = 0;

function noticepage(direction)
{
	// direction:1递增 2递减
	var nlegth = $('noticenum').value;

	if(direction==1)
	{
	//	alert(noticeReadStart);
		noticeReadStart++;
		//alert(noticeReadStart);
		if(noticeReadStart+1 >= nlegth){
			$('downdivno').style.display = 'block';
			$('downdivcan').style.display = 'none';
		}
		if(noticeReadStart > 0){
			$('updivcan').style.display = 'block';
			$('updivno').style.display = 'none';
		}
	}
	else
	{
		//alert(noticeReadStart);
		noticeReadStart--;
		//alert(noticeReadStart);
		if(noticeReadStart-1 < 0){
			$('updivcan').style.display = 'none';
			$('updivno').style.display = 'block';
		}
		if(noticeReadStart < nlegth){
			$('downdivcan').style.display = 'block';
			$('downdivno').style.display = 'none';
		}
	}
	if($('notice_title_'+noticeReadStart)){
		$('notice_title').innerHTML = $('notice_title_'+noticeReadStart).innerHTML;
		$('notice_content').innerHTML = $('notice_content_'+noticeReadStart).innerHTML;
		$('notice_id').value = $('notice_id_'+noticeReadStart).innerHTML;
	}
}

function removeGoods(a,m,s,x,t,c) {
	var _a=typeof(a)=='function'?a:(a=='keep'?function(){}:function(){x?MM_xmlLoad(a):MM_iframePost(a);});
	if (c) {var _c=typeof(c)=='function'?c:(c=='keep'?function(){}:function(){x?MM_xmlLoad(c):MM_iframePost(c);});}
	else var _c=function(){};
	MM_showDialog(
		''+m+(s?s:''), t?t:'', [
			{title:w.lang.dialog_confirm_button,act:function() {MM_closeDialog();_a();return false;}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();_c();return false;}}
		]);
}

function equipChangeTab(tab)
{
  var tabArray = [$("equipmentlist"),$("equipmentlist_skillbook"),$("equipmentlist_stuntbook")];
  var opArray = [$("equipment_operate"),$("equipment_operate_skillbook"),$("equipment_operate_stuntbook")];
  for(var i=0;i<tabArray.length;i++)
  {
  	if(i==tab)
  	{
  		tabArray[i].style.display = "block";
  	}
  	else
  	{
  		tabArray[i].style.display = "none";
  	}
  	opArray[i].style.display = "none";
	}

  var tabList = $('equipagelist_changetab').getElementsByTagName("a");
  for(var i=0;i<tabList.length;i++)
  {
  	if(i == tab)
  		tabList[i].className = "now";
  	else
  		tabList[i].className = "";
	}
}

function showEquipList(cate,pos)
{
	if($('generalequipment') && $('equip_cate'))
	{
		var now_cate = $('equip_cate').value;

		if(cate == now_cate || (cate=="equip" && now_cate==""))
		{
			setEquipDisplay();
			return;
		}
	}
	MM_xmlLoad('general.getEquipList&cate='+cate+'&pos='+pos+'&keep=all');
}

function refreshItemList(cate)
{
	if($('generalequipment') && $('equip_cate'))
	{
		var now_cate = $('equip_cate').value;
		if(cate == now_cate || (cate=="equip" && now_cate==""))
		{
			MM_xmlLoad('general.getEquipList&cate='+cate+'&keep=all');
		}
	}
	else
	{
		MM_xmlLoad('general.getEquipList&cate='+cate+'&keep=all');
	}
}

function setEquipDisplay()
{
	if($('generalequipment'))
	{
		$('generalequipment').style.display = "block";
		MM_showHidden('floatblockright','block');
	}
}


function buffShowTab(tab){
	var tag = $("buff_tab").getElementsByTagName("li");
	var con = $("buff_con").getElementsByTagName("div");
	for(i = 0;i < tag.length;i++){
		tag[i].className = "normal";
	}
	tag[tab].className = "now";
	for(i = 0;i < con.length;i++){
		con[i].style.display='none';
	}
	con[tab].style.display='block';
}


function setEleFocus(parentObj,tag,foucsObj,normalClass,focusClass)
{
    if(!parentObj || tag == '') return false;
    var eleList = parentObj.getElementsByTagName( tag );
    for(var i=0;i<eleList.length;i++) {
        if (eleList[i].id=="focus_exception") continue;
        if(eleList[i] == foucsObj) {
            eleList[i].className = focusClass;
        } else {
            eleList[i].className = normalClass;
        }
    }
}

function readReport()
{
	var gid = $('general_id').value;
	var step = $('battle_step').value;
	if(gid == '' || gid == 0)
	{
		return false;
	}
	MM_iframePost('generalbattle.display_report&general_id='+gid+'&battle_level=0&battle_step='+step+'&keep=all','all');

}

function changeGeneralSteps(gid)
{
	var jsontxt = $('general_list_json').value;
	ojson =  eval('(' + jsontxt + ')');
	$('battle_step').options.length = 0;
	for(i=0;i<ojson[gid]['step_list'].length;i++)
	{
		$('battle_step').options[i] = new Option(parseInt(ojson[gid]['step_list'][i])+1,ojson[gid]['step_list'][i],false,false);
	}
}

function changeValueInt(obj, max)
{
	if(!obj) return false;
	var objValue = obj.value.trim();
	if(objValue == '' || isNaN(parseInt(objValue, 10)) ){
		obj.value='';
	}else{
		objValue = parseInt(objValue, 10);
		if(max && objValue > max){objValue = max; }
		obj.value=objValue;
	}
}
function msg2friend(id,name)
{
    try {
        if ($('msgwrite_touser')) $('msgwrite_touser').value=name;
        else MM_xmlLoad('report.msgwrite&uid='+id);
    } catch(e) {}
    MM_closeDialog();
}
function changeGeneralIcon(objindex,target)
	{
		var obj = $('general'+objindex);
		for(var i in obj.options) {
			try{
				if (obj.options[i].value==obj.value){
				 target.src=img_src+'images/general/1/'+obj.options[i].getAttribute('icon')+'.jpg';
				// alert(img_src+'images/general/1/'+obj.options[i].getAttribute('icon')+'.jpg');
				 }
			}catch(e){}
		}
		for (var i=1; i<6; i++){
			var otherobj = $('general'+i);
			//alert(otherobj.value + ' == ' + obj.value);
			if (otherobj.value == obj.value && i!=objindex){
				$('general_iconid'+i).src=img_src+'images/general/1/M_random.jpg';
				otherobj.options[0].selected = true;
				otherobj.value = 0;
			}
		}
	}

//设置默认出征武将职位
function setDefalutGeneralCoa(str)
{
	if (!str) return;
	var arr = str.split('|');
	var obj;
	var target;
	for(var i=1; i<6; i++){
		obj = $('general'+i);
		$('general_iconid'+i).src=img_src+'images/general/1/M_random.jpg';
		obj.options[0].selected = true;
		obj.value = 0;
	}
	for(var i=0; i<arr.length; i++){
		obj = $('general'+(i+1));
		if (obj.disabled)	continue;
		target = $('general_iconid'+(i+1));
		obj.value = arr[i];
		for(var j in obj.options) {
			try{
				if (obj.options[j].value==obj.value){
				 	target.src=img_src+'images/general/1/'+obj.options[j].getAttribute('icon')+'.jpg';
				 	obj.options[j].selected = true;
				 }
			}catch(e){}
		}
	}
}

//by xubin
function changePage(idscount, cid){
	for( i=1;i<=idscount;i++ ) {
		if ( i == cid ) {
			$("guide-page-"+i).style.display = '';
			$("m"+i).style.backgroundColor = '#b5a78d';
			$("m"+i).style.color = '#2c3523';
		} else {
			$("guide-page-"+i).style.display = 'none';
			$("m"+i).style.backgroundColor = '#ccc';
			$("m"+i).style.color = '#000';
		}
	}
	return false;
}

function buyRes(type){
	var exchange_rate = parseInt($('exchange_rate').value);	//当前兑换比例
	var coin_value = parseInt(($('coin_'+type).value.replace(/^0+/,'')));	//输入金币值
	var max_res = parseInt($('max_'+type).value);  	//单项最大可购买量
	var sameas_res = $('sameas_'+type);	//单项金币兑换量
	var coin_remain;

	if(isNaN(max_res)){max_res = 0;}
	if(isNaN(exchange_rate)){exchange_rate = 1;}
	coin_remain = Math.floor(max_res/exchange_rate);
	if(isNaN(coin_value)) {$('coin_'+type).value = 0;coin_value = 0;}
	$('coin_'+type).value = coin_value;
	if(coin_value >= coin_remain){
		$('coin_'+type).value = coin_remain;
	}
	if(coin_value < 0){
		$('coin_'+type).value = 0;
	}
	sameas_res.innerHTML = $('coin_'+type).value * exchange_rate;
}

function buyReset(){
	var type = new Array('lumber','clay','iron','crop');
	for(i=0; i<type.length; i++){
		$('coin_'+type[i]).value = SGLang.InputCoins; ///old:grx:请输入金币数量
		$('sameas_'+type[i]).innerHTML = 0;
	}
}

function buySubmit(){
	var exchange_rate = parseInt($('exchange_rate').value);	//当前兑换比例
	if(exchange_rate <= 1 || isNaN(exchange_rate)){
		MM_showDialog('<h4 class="msgtips"><img src="'+w.img_src+'images/icon/alert.gif" class="attention" />'+SGLang.Out7canBuy+'</h4>','',[{
			title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();MM_xmlLoad('build.act&do=buy_res&btid=38&keep=all');return false;}}
		]);
		return false;
	}

	var type = new Array('lumber','clay','iron','crop');
	var goldcoins = parseInt($('goldcoins').value);
	var sum_coins = 0;
	var sum_res   = 0;
	var input_flag = true;
	for(i=0; i<type.length; i++){
		coin_value = parseInt($('coin_'+type[i]).value);
		max_value = parseInt($('max_'+type[i]).value);
		res_value  = parseInt($('sameas_'+type[i]).innerHTML);
		if(isNaN(coin_value) || coin_value < 0){coin_value = 0;}
		if(isNaN(res_value)  || coin_value < 0){res_value = 0;}
		if(coin_value*exchange_rate > max_value){
			input_flag = false;
		}
		sum_coins += coin_value;
		sum_res += res_value;
	}
	if(!input_flag || sum_coins*exchange_rate != sum_res){
		MM_showDialog('<h4 class="msgtips"><img src="'+w.img_src+'images/icon/alert.gif" class="attention" />'+SGLang.InputError+'</h4>','',[{
			title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();MM_xmlLoad('build.act&do=buy_res&btid=38&keep=all');return false;}}
		]);
		return false;
	}

	if(sum_coins == 0 || sum_res == 0 || isNaN(sum_coins) || isNaN(sum_res)){
		MM_showDialog('<h4 class="msgtips"><img src="'+w.img_src+'images/icon/alert.gif" class="attention" />'+SGLang.InputCoins+'</h4>','',[{
			title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();MM_xmlLoad('build.act&do=buy_res&btid=38&keep=all');return false;}}
		]);
		return false;
	}
	if(isNaN(goldcoins)){
		goldcoins = 0;
	}
	if(sum_coins > goldcoins){
		MM_showDialog('<h4 class="msgtips"><img src="'+w.img_src+'images/icon/alert.gif" class="attention" />'+SGLang.CoinsNoEnough+'</h4>','',[{
			title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();MM_xmlLoad('build.act&do=buy_res&btid=38&keep=all');return false;}}
		]);///old:grx:您所持的金币不足
		return false;
	}
	if(sum_coins > 0 && sum_res > 0 && (sum_coins*exchange_rate == sum_res)){
		MM_showDialog(
		'<h4 class="msgtips"><img src="'+w.img_src+'images/icon/alert.gif" class="attention" />'+SGLang.UseCoins.replace(/\[AAA\]/g,
	sum_coins).replace(/\[BBB\]/g, sum_res)+'</h4>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){MM_closeDialog();try{document.buy_res_form.action+='&villageid='+w.villageid;document.buy_res_form.submit();}catch(e){};return false;}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();return false;}}
		]);
	}
	///old:grx:花费<strong class="red">'+sum_coins+'</strong>金币购买资源'+sum_res+'
}

// utility function to retrieve an expiration data in proper format;
function getExpDate(days, hours, minutes)
{
	var expDate = new Date();
	if(typeof(days) == "number" && typeof(hours) == "number" && typeof(hours) == "number")
	{
		expDate.setDate(expDate.getDate() + parseInt(days));
		expDate.setHours(expDate.getHours() + parseInt(hours));
		expDate.setMinutes(expDate.getMinutes() + parseInt(minutes));
		return expDate.toGMTString();
	}
}

//utility function called by getCookie()
function getCookieVal(offset)
{
	var endstr = document.cookie.indexOf(";", offset);
	if(endstr == -1)
	{
		endstr = document.cookie.length;
	}
	return unescape(document.cookie.substring(offset, endstr));
}

// primary function to retrieve cookie by name
function getCookie(name)
{
	var arg = name + "=";
	var alen = arg.length;
	var clen = document.cookie.length;
	var i = 0;
	while(i < clen)
	{
		var j = i + alen;
		if (document.cookie.substring(i, j) == arg)
		{
			return getCookieVal(j);
		}
		i = document.cookie.indexOf(" ", i) + 1;
		if(i == 0) break;
	}
	return;
}

// store cookie value with optional details as needed
function setCookie(name, value, expires, path, domain, secure)
{
	document.cookie = name + "=" + escape(value) +
		((expires) ? "; expires=" + expires : "") +
		((path) ? "; path=" + path : "") +
		((domain) ? "; domain=" + domain : "") +
		((secure) ? "; secure" : "");
}

// remove the cookie by setting ancient expiration date
function deleteCookie(name,path,domain)
{
	if(getCookie(name))
	{
		document.cookie = name + "=" +
			((path) ? "; path=" + path : "") +
			((domain) ? "; domain=" + domain : "") +
			"; expires=Thu, 01-Jan-70 00:00:01 GMT";
	}
}

function doOreCompose(id,subid,isbind,istrade,returnURL)
{
	var oreNum = $('composeOreNum').value.trim();
	if(oreNum == '' || isNaN(oreNum) || oreNum <= 0 ){
		MM_showDialog( SGLang.InputCorrectNum );
	}else{
		MM_xmlLoad('smelt.doOreCompose&id='+id+'&subid='+subid+'&isbind='+isbind+'&istrade='+istrade+'&orenum='+oreNum+'&returnURL='+returnURL+'&keep=all');
	}
}

function rebuildOrderlyCnt(freeCnt, totalCnt, notice)
{
	$('orderlyCnt').innerHTML = freeCnt + '/' + totalCnt;
	if (notice != '') {
		$('orderlyNotice').innerHTML = notice;
		$('orderlyNotice').style.color = '#FF0000';
	}
}

function reloadAfterOrderlyUp()
{
	MM_xmlLoad('build.act&do=war_list&btid=9&x='+$('aim_x').value+'&y='+$('aim_y').value+'&vname='+encodeURIComponent($('aim_name').value));
}

function shownpctips(id) {
    if (!w.npcinfo || !w.npcinfo[id]) return false;
    var info=w.npcinfo[id];
    var msgid=Math.floor(Math.random()*info.msg.length);
    if (isNaN(msgid)) return false;
    var html='<h4 class="msgtips"><img src="'+w.img_src+'images/general/2/'+info.icon+'.jpg" class="attention" />'+info.msg[msgid]+'</h4>'
    try {
        MM_showDialog(
            html,
            info.name,
            {title:w.lang.dialog_confirm_button,act:function() {MM_closeDialog();return false;}}
        );
    } catch(e) {}
    return false;
}
function shownpcname(obj,id) {
    if (!w.npcinfo || !w.npcinfo[id]) return false;
    var info=w.npcinfo[id];
    ui.describe(obj,info.name);
}

function check_iteminput(item){
	var item_input = parseInt(($('input_'+item).value.replace(/^0+/,'')));
	var item_had = parseInt($('had_'+item).value);

	if(isNaN(item_had)) return false;
	if(isNaN(item_input) || item_input<0){
		item_input = 0;
		$('input_'+item).value = 0;
	}
	$('input_'+item).value = item_input;
	if(item_input > item_had){
		$('input_'+item).value = 0;
	}
}

function alliance_submit(c){
	var sum = 0;
	var url = '';
	var sumcontri = 0;
	for(i=1;i<=c;i++){
		if(!$('input_'+i)){
			continue;
		}
		item_input = parseInt(($('input_'+i).value.replace(/^0+/,'')));
		amount = parseInt(($('amount_'+i).value.replace(/^0+/,'')));
		item_had = parseInt($('had_'+i).value);
		if(isNaN(item_input)){
			$('input_'+i).value = 0;
			item_input = 0;
		}
		$('input_'+i).value = item_input;
		if(item_input > item_had){
			$('input_'+i).value = 0;
			item_input = 0;
			sum = 0;
			break;
		}
		sum+=item_input;
		sumcontri+=amount*item_input;
		url+= 'in'+i+'='+item_input+'&';
	}
	if(sum <= 0){
		MM_showDialog('<h4 class="msgtips"><img src="'+w.img_src+'images/icon/alert.gif" class="attention" />'+SGLang.CountError+'</h4>','',[{
			title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();return false;}}
		]);
		return false;
	}
	MM_showDialog(
		'<h4 class="msgtips"><img src="'+w.img_src+'images/icon/alert.gif" class="attention" />'+SGLang.AllianceBuy.replace(/\[XXX\]/g,
		sumcontri
		)+'</h4>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){MM_closeDialog();try{MM_xmlLoad('alliance.contributebuy&'+url+'keep=all');}catch(e){};return false;}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();return false;}}
	]);
}

function checkImproveExpGeneral(obj)
{
	var mye=null;
	if ((mye=document.forms['generalImproveExpForm'])==null) return;
	var els = mye.elements;

	for(var ii=0; ii<els.length; ii++)
	{
		if (els[ii] != obj && els[ii].value != ''&& els[ii].name && els[ii].name.indexOf('general_') != '-1')
		{
			if (els[ii].value == obj.value) els[ii].selectedIndex = 0;
		}
	}
}

function setImproveExpGeneralNew(act, price, freeNum)
{
	var mye=null;
	if ((mye=document.forms['generalImproveExpForm'])==null) return;
	var els = mye.elements;
	var param = '';
	var totalPrice = 0;
	for(var ii=0; ii<els.length; ii++)
	{
		if(els[ii].value != '' && els[ii].name && els[ii].name.indexOf('general_') != '-1')
		{
			param += '&generals[]=' + els[ii].value;
			totalPrice += price;
		}
	}
	if(param == '')
	{
		alertDialog('keep', SGLang.ErrorSelectGeneral);
	}
	else
	{
		confirmDialog(function(){if($('useFreeImproveExp') && $('useFreeImproveExp').checked==true){MM_xmlLoad(act+param+'&usefree=1')}else{MM_xmlLoad(act+param);}}, SGLang.ConsumeCoinConfirm.replace(/\[XXX\]/g,totalPrice)+(freeNum>0?SGLang.ImproveExpFreeLeft.replace(/\[AAA\]/g,freeNum).replace(/\[BBB\]/g,freeNum*price):'')+(freeNum>0?'<br/><input type=\'checkbox\' id=\'useFreeImproveExp\' value=\'1\'/>'+SGLang.ImproveExpFreeTips:''));
	}
}

function selectInfluence(str,influence_id)
{
	var arr = str.split(',');
	for (i=1; i<=35; i++)
	{
		$('influence_'+i).className = '';
	}
	for (ii=0; ii<arr.length; ii++)
	{
		$('influence_'+arr[ii]).className = 'span1';
	}
	for (i=1;i<=10;i++)
	{

		$('influence_a_'+i).className = 'purplebutton_b';

	}
	$('influence_a_'+influence_id).className = 'purplebutton_b purplebutton_b_now';
}

function changeInfluence(influence_name,relation_value,relationship)
{
	$('influence_name').innerHTML = influence_name;
	$('relation_value').innerHTML = relation_value;
	$('relationship').innerHTML = relationship;
}

function vmanage(vid, build, url)
{
	w.villageid = vid;
	if (build) {
		MM_xmlLoad('village.soldierview&keep=all');
		if (url == false)
			MM_xmlLoad('build.status');
		else
			MM_xmlLoad('build.status&keep=all');
	}
	if (url != false) MM_xmlLoad(url);
}

function achieveShow(nowid)
{
	var ids = ['achieve_product', 'achieve_build', 'achieve_grow', 'achieve_general', 'achieve_fight', 'achieve_rank'];
	for (var i=0 ; i<ids.length ; i++) {
		if (nowid == ids[i]) {
			$(nowid + '_li').className = 'now';
			$(nowid).style.display = 'block';
		} else {
			$(ids[i] + '_li').className = 'normal';
			$(ids[i]).style.display = 'none';
		}
	}
}

function startITask(uit_id,uit_type,city_id,city_name)
{
	var gid;
	if(uit_type==1 || uit_type==2 || uit_type==3 || uit_type==6)	{
		gid = $('self_gid').value;
		if(gid=='0'){
			alertDialog('keep',SGLang.ErrorSelectGeneral);
			return;
		}
	}else{
		gid = 0;
	}
	switch(uit_type)
	{
		case 1:
			MM_xmlLoad('task.startITask&uit_id='+uit_id+'&gid='+gid+'&city_id='+city_id);
			break;
		case 5:
			MM_xmlLoad('build.act&do=main&btid=18&uit_id='+uit_id+'&vname='+encodeURIComponent(city_name));
			break;
		case 2:
		case 3:
		case 4:
		case 6:
			MM_xmlLoad('build.act&do=war_list&btid=9&x=???&y=???&city_id='+city_id+'&vname='+encodeURIComponent(city_name)+'&battle_type=task&uit_id='+uit_id+'&gid='+gid);
			break;
	}
}

function preLoadImages(imgDir, imgObjs)
{
	var images = [
		'images/bg_dialogshop.gif',
		'images/bg_dialogcontent1.gif'
	];

	for (var i=0; i<images.length; i++) {
		imgObjs[i] = new Image();
		imgObjs[i].src = imgDir + images[i];
	}
}

function lubanRebornCal(inputObj, resosPreCoin, coinUseMax, userCoinNum)
{
	changeValueInt(inputObj);
	var coinNum = inputObj.value.trim();
	coinNum = parseInt(coinNum);
	if(isNaN(coinNum)) coinNum = 0;
	coinNum = Math.abs(coinNum);
	if(coinNum > coinUseMax) coinNum = coinUseMax;
	if(coinNum > userCoinNum) coinNum = userCoinNum;

	if(coinNum == 0) inputObj.value = '';
	else inputObj.value = coinNum;

	$('lubanRebornSaveResos').innerHTML = resosPreCoin * coinNum;
	$('lubanRebornCoin').innerHTML = coinNum;
}

function getrecrul(lable_name){
	valueurl=$('recomment_url').value;
	try{
		MM_showDialog("<div style='margin-left: 7px; width: 90%; height: 70px; line-height: 18px;word-break: break-all;text-align:left;'>"+valueurl+"</div>",lable_name,"");
	    if (window.clipboardData){
		   window.clipboardData.setData('Text',valueurl);
	    }
	}catch(e){

	}
}

function undressScroll(gid, maxCnt, msg)
{
	MM_showDialog(
		'<p>'+msg+SGLang.UndressScrollCnt+'<input type="text" id="dialog_input" style="width:30px;" value="'+maxCnt+'" />'+' <span style="cursor:pointer;color:#009900" onclick="$(\'dialog_input\').value='+maxCnt+'">('+maxCnt+')</span></p>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){if ($('dialog_input').value.trim()!='') {MM_closeDialog();MM_iframePost('general.unladeitem&gid='+gid+'&type=1&keep=all&scrollcnt='+$('dialog_input').value);}}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();}}
		]);
}

function generalEquipScroll()
{
	var general_id = parseInt($('general_id').value);
	var general_level = parseInt($('general_level').value);
	var item_general_level = parseInt($('item_general_level').value);
	var maxCnt = parseInt($('item_cnt').value);

	if(!general_id){alertDialog('keep',SGLang.GeneralInfoError);}///old:grx:武将信息错误

	if(general_level < item_general_level)
	{
		alertDialog('keep',SGLang.GeneralNeedLevel.replace(/\[XXX\]/g,item_general_level));///old:grx:'装备需要武将等级达到'+item_general_level
	}
	else
	{
		MM_showDialog(
		'<p>'+SGLang.dressScrollCnt+'<input type="text" id="dialog_input" style="width:30px;" value="1" />'+' <span style="cursor:pointer;color:#009900" onclick="$(\'dialog_input\').value='+maxCnt+'">('+maxCnt+')</span></p>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){if ($('dialog_input').value.trim()!='') {MM_closeDialog();MM_iframePost('general.changeitem&gid='+general_id+'&goodsid='+$('item_goodsid').value+'&goodssubid='+$('item_goodssubid').value+'&isbind='+$('item_isbind').value+'&istrade='+$('item_istrade').value+'&keep=all&scrollcnt='+$('dialog_input').value);}}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();}}
		]);
	}
}
function skillbookUpgradeSet(jStr)
{
	var goodsInfo = eval(jStr);
	var set = false;
	if(goodsInfo){
		try{
			var nowNum = parseInt($('num_'+goodsInfo[0]+'_'+goodsInfo[1]+'_'+goodsInfo[6]+'_'+goodsInfo[7]).innerHTML);
			if(isNaN(nowNum)) nowNum = 0;
			if(nowNum > 0)
			{
				for(var i=1;i<=2;i++){
					if($('skillbook_use_id_'+i) && $('skillbook_use_id_'+i).value == '0')
					{

						$('skillbook_use_id_'+i).value = goodsInfo[0];
						$('skillbook_use_subid_'+i).value = goodsInfo[1];
						$('skillbook_use_name_'+i).innerHTML = goodsInfo[2];
						$('skillbook_use_icon_'+i).src = goodsInfo[3];
						$('skillbook_use_desc_'+i).value = goodsInfo[4];
						$('skillbook_use_effect_'+i).value = goodsInfo[5];
						$('skillbook_use_isbind_'+i).value = goodsInfo[6];
						$('skillbook_use_istrade_'+i).value = goodsInfo[7];
						set = true;
						$('skillbook_unset_'+i).style.display = 'none';
						$('skillbook_set_'+i).style.display = 'block';

						skillbookSetNum(goodsInfo[0], goodsInfo[1], goodsInfo[6], goodsInfo[7], -1);
						break;
					}
				}
				if(!set) alertDialog('keep',SGLang.ErrorUpgradeItemFull);
			}
			else
			{
				alertDialog('keep',SGLang.ErrorItemNotEnough);
			}
		}catch(e){}
	}
}

function skillbookUpgradeSetNum(upgradeId)
{
	if($('skillbook_upgrade_subid') && $('skillbook_upgrade_subid').value != '0')
	{
		skillbookSetNum(upgradeId, $('skillbook_upgrade_subid').value, $('skillbook_upgrade_isbind').value, $('skillbook_upgrade_istrade').value, -1);
		skillbookSetNum($('skillbook_use_id_1').value, $('skillbook_use_subid_1').value, $('skillbook_use_isbind_1').value, $('skillbook_use_istrade_1').value, -1);
		skillbookSetNum($('skillbook_use_id_2').value, $('skillbook_use_subid_2').value, $('skillbook_use_isbind_2').value, $('skillbook_use_istrade_2').value, -1);
	}
}

function skillbookSetNum(id, subid, isbind, istrade, num)
{
	if(($('skillbook_upgrade_subid') && $('skillbook_upgrade_subid').value != '0') || ($('skillbook_compose_subid_1') && $('skillbook_compose_subid_1').value != '0'))
	{
		if($('num_'+id+'_'+subid+'_'+isbind+'_'+istrade))
		{
			var nowNum = parseInt($('num_'+id+'_'+subid+'_'+isbind+'_'+istrade).innerHTML);
			if(isNaN(nowNum)) nowNum = 0;
			nowNum += num;
			if(nowNum < 0) nowNum = 0;
			$('num_'+id+'_'+subid+'_'+isbind+'_'+istrade).innerHTML = nowNum;
		}
	}
}

function skillbookUpgradeUnsetAll()
{
	try{
		$('skillbook_upgrade_subid').value = '0';
		$('skillbook_upgrade_isbind').value = '0';
		MM_closeRight();
	}catch(e){}
}

function skillbookUpgradeUnset(i)
{
	if($('skillbook_use_id_'+i) && $('skillbook_use_id_'+i).value != '0')
	{
		skillbookSetNum($('skillbook_use_id_'+i).value, $('skillbook_use_subid_'+i).value, $('skillbook_use_isbind_'+i).value, $('skillbook_use_istrade_'+i).value, 1);
		$('skillbook_use_id_'+i).value = 0;
		$('skillbook_use_subid_'+i).value = 0;
		$('skillbook_use_name_'+i).innerHTML = '';
		$('skillbook_use_icon_'+i).src = w.img_src+'images/goods/null.gif';
		$('skillbook_use_desc_'+i).value = '';
		$('skillbook_use_effect_'+i).value = '';
		$('skillbook_use_isbind_'+i).value = '0';
		$('skillbook_use_istrade_'+i).value = '0';
		$('skillbook_unset_'+i).style.display = 'block';
		$('skillbook_set_'+i).style.display = 'none';
	}
}

function skillbookUpradeCalOdds(inputObj)
{
	changeValueInt(inputObj);
	var useNum = inputObj.value.trim();
	useNum = parseInt(useNum);
	if(isNaN(useNum)) useNum = 0;
	useNum = Math.abs(useNum);
	if(useNum > 3) useNum = 3;
	if(useNum == 0) inputObj.value = '';
	else inputObj.value = useNum;
	try{
		var oddsSetting = eval($('skillbook_upgrade_odds_j').value);
		for(var i=0;i<=3;i++)
		{
			if(parseInt(oddsSetting[i]) == 100) break;
		}
		if(i>3) i=3;
		if(useNum > i) useNum = i;
		inputObj.value = useNum;
		$('skillbook_upgrade_odds').innerHTML = oddsSetting[useNum];
	}catch(e){}
}

function skillbookUpgrade()
{
	if($('skillbook_upgrade_subid').value == '0') {alertDialog('keep',SGLang.ErrorSkillbookUpgrade1);return false;}
	if($('skillbook_use_id_1').value == '0' || $('skillbook_use_id_2').value == '0') {alertDialog('keep',SGLang.ErrorSkillbookUpgrade2);return false;}
	MM_xmlLoad('skillbook.upgrade&upgrade_subid='+$('skillbook_upgrade_subid').value+'&upgrade_isbind='+$('skillbook_upgrade_isbind').value+'&upgrade_istrade='+$('skillbook_upgrade_istrade').value+'&use_id_1='+$('skillbook_use_id_1').value+'&use_subid_1='+$('skillbook_use_subid_1').value+'&use_isbind_1='+$('skillbook_use_isbind_1').value+'&use_istrade_1='+$('skillbook_use_istrade_1').value+'&use_id_2='+$('skillbook_use_id_2').value+'&use_subid_2='+$('skillbook_use_subid_2').value+'&use_isbind_2='+$('skillbook_use_isbind_2').value+'&use_istrade_2='+$('skillbook_use_istrade_2').value+'&aid_id='+$('skillbook_aid_id').value+'&aid_subid='+$('skillbook_aid_subid').value+'&aid_num='+$('skillbook_aid_num').value);
}

function skillbookComposeSetNum(composeId)
{
	if($('skillbook_compose_subid_1') && $('skillbook_compose_subid_1').value != '0')
	{
		skillbookSetNum(composeId, $('skillbook_compose_subid_1').value, $('skillbook_compose_isbind_1').value, $('skillbook_compose_istrade_1').value, -1);
		skillbookSetNum($('skillbook_compose_id_2').value, $('skillbook_compose_subid_2').value, $('skillbook_compose_isbind_2').value, $('skillbook_compose_istrade_2').value, -1);
	}
}

function skillbookComposeSet(jStr)
{
	var goodsInfo = eval(jStr);
	var set = false;
	if(goodsInfo){
		try{
			var nowNum = parseInt($('num_'+goodsInfo[0]+'_'+goodsInfo[1]+'_'+goodsInfo[6]+'_'+goodsInfo[7]).innerHTML);
			if(isNaN(nowNum)) nowNum = 0;
			if(nowNum > 0)
			{
				if($('skillbook_compose_id_2') && $('skillbook_compose_id_2').value == '0')
				{
					$('skillbook_compose_id_2').value = goodsInfo[0];
					$('skillbook_compose_subid_2').value = goodsInfo[1];
					$('skillbook_compose_name_2').innerHTML = goodsInfo[2];
					$('skillbook_compose_icon_2').src = goodsInfo[3];
					$('skillbook_compose_desc_2').value = goodsInfo[4];
					$('skillbook_compose_effect_2').value = goodsInfo[5];
					$('skillbook_compose_isbind_2').value = goodsInfo[6];
					$('skillbook_compose_istrade_2').value = goodsInfo[7];
					set = true;
					$('skillbook_compose_unset_2').style.display = 'none';
					$('skillbook_compose_set_2').style.display = 'block';
					skillbookSetNum(goodsInfo[0], goodsInfo[1], goodsInfo[6], goodsInfo[7], -1);
				}
				if(!set) alertDialog('keep',SGLang.ErrorComposeItemFull);
			}
			else
			{
				alertDialog('keep',SGLang.ErrorItemNotEnough);
			}
		}catch(e){}
	}
}

function skillbookComposeUnsetAll()
{
	try{
		$('skillbook_compose_subid_1').value = '0';
		$('skillbook_compose_isbind_1').value = '0';
		MM_closeRight();
	}catch(e){}
}

function skillbookComposeUnset()
{
	if($('skillbook_compose_id_2') && $('skillbook_compose_id_2').value != '0')
	{
		skillbookSetNum($('skillbook_compose_id_2').value, $('skillbook_compose_subid_2').value, $('skillbook_compose_isbind_2').value, $('skillbook_compose_istrade_2').value, 1);
		$('skillbook_compose_id_2').value = '0';
		$('skillbook_compose_subid_2').value = '0';
		$('skillbook_compose_name_2').innerHTML = '';
		$('skillbook_compose_icon_2').src = w.img_src+'images/goods/null.gif';
		$('skillbook_compose_desc_2').value = '';
		$('skillbook_compose_effect_2').value = '';
		$('skillbook_compose_isbind_2').value = '0';
		$('skillbook_compose_istrade_2').value = '0';
		$('skillbook_compose_unset_2').style.display = 'block';
		$('skillbook_compose_set_2').style.display = 'none';
	}
}

function skillbookCompose()
{
	if($('skillbook_compose_subid_1').value == '0' || $('skillbook_compose_id_2').value == '0') {alertDialog('keep',SGLang.ErrorSkillbookCompose1);return false;}
	MM_xmlLoad('skillbook.compose&compose_subid_1='+$('skillbook_compose_subid_1').value+'&compose_isbind_1='+$('skillbook_compose_isbind_1').value+'&compose_istrade_1='+$('skillbook_compose_istrade_1').value+'&compose_id_2='+$('skillbook_compose_id_2').value+'&compose_subid_2='+$('skillbook_compose_subid_2').value+'&compose_isbind_2='+$('skillbook_compose_isbind_2').value+'&compose_istrade_2='+$('skillbook_compose_istrade_2').value);
}


function sellGoods(gid, gsubid, isbind, multi, icon, gname, base_price, cnt_id)
{
	var img_dir = $('sell_img_dir').value;
	$('sell_goodsinfo').style.display = '';

	$('sell_icon').innerHTML = '<img src="'+img_dir+'images/goods/'+icon+'" alt="" width="44" border="0" height="44" class="shoppic">';
	$('sell_name').innerHTML = gname;
	$('sell_total_cnt').innerHTML = $(cnt_id).innerHTML;

	if (multi == 0) {
		$('sell_cnt').disabled = true;
		$('sell_cnt').value = 1;
	} else {
		$('sell_cnt').disabled = false;
		$('sell_cnt').value = $(cnt_id).innerHTML;
	}
	$('sell_cnt_id').value = cnt_id;

	$('sell_pop_li').style.display = 'none';
	$('sell_price').value = base_price * $('sell_cnt').value;

	$('sell_submit').onclick = function(){
		var days = $('sell_days').value;
		var goods_cnt = $('sell_cnt').value;
		var price = $('sell_price').value;

		var getUrl = 'ustore.sell&type=1&goods_id='+gid+'&goods_subid='+gsubid+'&goods_isbind='+isbind+'&goods_cnt='+goods_cnt+'&days='+days+'&price='+price;
		confirmDialog(getUrl, SGLang.sellConfirm);
	};
	if(gid!=249){//熟铜 价格特殊  不用此功能 by wcd
	$('sell_cnt').onkeyup = function(){
		var cnt = $('sell_cnt').value;
		var price = $('sell_price').value;

		if (price < cnt * base_price) {
			$('sell_price').value = cnt * base_price;
		}
	};
	}
}

function sellSoldiers(icon, sname, spop, vid, stype, upkeep, cnt_id)
{
	var img_dir = $('sell_img_dir').value;
	$('sell_goodsinfo').style.display = '';

	$('sell_icon').innerHTML = '<img src="'+img_dir+'images/soldier_m/'+icon+'" alt="" width="44" border="0" height="44" class="shoppic">';
	$('sell_name').innerHTML = sname;
	$('sell_total_cnt').innerHTML = $(cnt_id).innerHTML;

	$('sell_cnt').disabled = false;
	$('sell_cnt').value = $(cnt_id).innerHTML;
	$('sell_cnt_id').value = cnt_id;

	$('sell_pop_li').style.display = '';
	$('sell_pop').value = spop;
	$('sell_price').value = spop;

	$('sell_submit').onclick = function(){
		var days = $('sell_days').value;
		var soldier_cnt = $('sell_cnt').value;
		var price = $('sell_price').value;

		var getUrl = 'ustore.sell&type=2&soldier_type='+stype+'&soldier_vid='+vid+'&soldier_cnt='+soldier_cnt+'&days='+days+'&price='+price;
		confirmDialog(getUrl, SGLang.sellConfirm);
	};

	$('sell_cnt').onkeyup = function(){
		var cnt = $('sell_cnt').value;
		var price = $('sell_price').value;

		if (price < cnt * upkeep) {
			$('sell_price').value = cnt * upkeep;
		}

		$('sell_pop').value = cnt * upkeep;
	};
}

function sellWalldef(icon, wname, vid, wtype, cnt_id)
{
	var img_dir = $('sell_img_dir').value;
	$('sell_goodsinfo').style.display = '';

	$('sell_icon').innerHTML = '<img src="'+img_dir+'images/soldier_m/'+icon+'" alt="" width="44" border="0" height="44" class="shoppic">';
	$('sell_name').innerHTML = wname;
	$('sell_total_cnt').innerHTML = $(cnt_id).innerHTML;

	$('sell_cnt').disabled = false;
	$('sell_cnt').value = $(cnt_id).innerHTML;
	$('sell_cnt_id').value = cnt_id;

	$('sell_pop_li').style.display = 'none';
	$('sell_price').value = 0.5 * $('sell_cnt').value;

	$('sell_submit').onclick = function(){
		var days = $('sell_days').value;
		var walldef_cnt = $('sell_cnt').value;
		var price = $('sell_price').value;

		var getUrl = 'ustore.sell&type=3&soldier_vid='+vid+'&walldef_type='+wtype+'&walldef_cnt='+walldef_cnt+'&days='+days+'&price='+price;
		confirmDialog(getUrl, SGLang.sellConfirm);
	};

	$('sell_cnt').onkeyup = function(){
		var cnt = $('sell_cnt').value;
		var price = $('sell_price').value;

		if (price < cnt * 0.5) {
			$('sell_price').value = Math.ceil(cnt * 0.5);
		}
	};
}

function reduceSellCnt(sellCnt)
{
	var cnt_id = $('sell_cnt_id').value;
	$(cnt_id).innerHTML = $(cnt_id).innerHTML - sellCnt;
	$('sell_total_cnt').innerHTML = $('sell_total_cnt').innerHTML - sellCnt;

	$('sell_goodsinfo').style.display = 'none';
}
function guideMenu(){
	$("all").innerHTML ="<li><a href=\"javascript:void(0);\" onclick=\"showtree("+gameNum[0]+",'navmain','sub','all');\" id=\"navmain"+gameNum[0]+"\" class=\"slidedown\">"+SGLang.Guidenewer+"</a><ul class=\"tree_sub\" id=\"sub"+gameNum[0]+"\"></ul></li>";
	$("all").innerHTML +="<li><a href=\"javascript:void(0);\" onclick=\"showtree("+gameNum[1]+",'navmain','sub','all');\" id=\"navmain"+gameNum[1]+"\" class=\"slidedown\">"+SGLang.Guidepro+"</a><ul class=\"tree_sub\" id=\"sub"+gameNum[1]+"\"></ul></li>";
	$("all").innerHTML +="<li><a href=\"javascript:void(0);\" onclick=\"showtree("+gameNum[2]+",'navmain','sub','all');\" id=\"navmain"+gameNum[2]+"\" class=\"slidedown\">"+SGLang.Guideintro+"</a><ul class=\"tree_sub\" id=\"sub"+gameNum[2]+"\"></ul></li>";
	$("all").innerHTML +="<li><a href=\"javascript:void(0);\" onclick=\"showtree("+gameNum[3]+",'navmain','sub','all');\" id=\"navmain"+gameNum[3]+"\" class=\"slidedown\">"+SGLang.Guiderule+"</a><ul class=\"tree_sub\" id=\"sub"+gameNum[3]+"\"></ul></li>";
}
//金币消费 礼金券消费 功能化道具选择
function consume2(p1,p2,byTicket,byItem) {
	//arr = p2.split('|$|');
	var goldchecked = (byItem == '') ? 'checked="true"' : '';
	var html = '<h4>'+p2+'</h4><ul class="commodity_buy"><li><label for="consume_type0" class="canclick"><input type="radio" name="consume_type" id="consume_type0" '+goldchecked+' value="0" />'+SGLang.ConsumeByCoin+'</label></li>';
	if (byTicket == '1') {
		html = html + '<li><label for="consume_type1" class="canclick"><input type="radio" name="consume_type" id="consume_type1" value="1" />'+SGLang.ConsumeByTicket+'</label></li>';
	}
	if (byItem != '') {
		html = html + '<li><label for="consume_type2" class="canclick"><input type="radio" name="consume_type" id="consume_type2" checked="true" value="2" />'+SGLang.quickUse+byItem+'</label></li>';
	}
	html = html + '</ul>';

	MM_showDialog(
		html, '', [
			{title:w.lang.dialog_confirm_button,act:function(){var ret = false;var consume_type = 0;if ($('consume_type0').checked) {ret = true;}if (byTicket==1 && $('consume_type1').checked) {ret = true; consume_type=1;}if (byItem!='' && $('consume_type2').checked) {ret = true; consume_type=2;}if (ret == false) return false;MM_closeDialog();MM_xmlLoad(p1+'&consume_type='+consume_type);return false;}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();}}
		]);
}

function consume3(p1,byTicket,byItem) {
	//arr = p1.split('|$|');
	var goldchecked = (byItem == '') ? 'checked="true"' : '';
	var html = '<h4>'+p1+'</h4><ul class="commodity_buy"><li><label for="consume_type0" class="canclick"><input type="radio" name="consume_type" id="consume_type0" '+goldchecked+' value="0" />'+SGLang.ConsumeByCoin+'</label></li>';
	if (byTicket == '1') {
		html = html + '<li><label for="consume_type1" class="canclick"><input type="radio" name="consume_type" id="consume_type1" value="1" />'+SGLang.ConsumeByTicket+'</label></li>';
	}
	if (byItem != '') {
		html = html + '<li><label for="consume_type2" class="canclick"><input type="radio" name="consume_type" id="consume_type2" checked="true" value="2" />'+SGLang.quickUse+byItem+'</label></li>';
	}
	html = html + '</ul>';

	MM_showDialog(
		html, '', [
			{title:w.lang.dialog_confirm_button,act:function(){var ret = false;var consume_type = 0;if ($('consume_type0').checked) {ret = true;}if (byTicket==1 && $('consume_type1').checked) {ret = true; consume_type=1;}if (byItem!='' && $('consume_type2').checked) {ret = true; consume_type=2;}if (ret == false) return false;MM_closeDialog();$('consumetype').value=consume_type;document.computer_trade_form.action=document.computer_trade_form.action+'&villageid='+w.villageid+'&rand='+(++rd);document.computer_trade_form.submit();return false;}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();}}
		]);
}//金币消费 礼金券消费

function smeltDecomposeDialog(act,msg,maxNum)
{
	msg = '<p>'+msg+'</p>';
	msg += '<p>'+SGLang.SmeltDecomposeNum+'<input type="text" id="smeltDecomposeNum" value="'+maxNum+'" style="width:40px;text-align:right" />  ('+maxNum+')</p>';
	msg += '<input type="hidden" id="smeltDecomposeAct" value="'+act+'" />';
	msg += '<input type="hidden" id="smeltDecomposeMaxNum" value="'+maxNum+'" />';

	msg = '<div>'+msg+'</div>';
	MM_showDialog(
			msg, '', [
				{title:w.lang.dialog_confirm_button,act:function(){
														var act = '';
														var num = 0;
														var maxNum = 0;
														if($('smeltDecomposeAct')){
															act = $('smeltDecomposeAct').value.trim();
														}
														if($('smeltDecomposeNum')){
															num = parseInt($('smeltDecomposeNum').value.trim());
															if(isNaN(num)) num = 0;
														}
														if($('smeltDecomposeMaxNum')){
															maxNum = parseInt($('smeltDecomposeMaxNum').value.trim());
															if(isNaN(maxNum)) maxNum = 0;
														}
														if(num <= 0){alertDialog('keep', SGLang.ErrorNoInputItemNum);return false;}
														if(num > maxNum){alertDialog('keep', SGLang.ErrorItemNotEnough);return false;}
														if(act==''){return false;}
														act += '&num='+num;
														MM_xmlLoad(act);
													}
				},
				{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();}}
			]);
}

function multiDecompose()
{
	if(!$('jDecomposeListData')) return false;
	var jData = eval($('jDecomposeListData').value.trim());
	var html = '<div id="decomposeListDiv">';
	html += '<p>'+SGLang.ConfireDecompose+'</p>';
	html += '<ul id="decomposeList">';
	for(var i=0;i<jData.length;i++){
		html += '<li><span>'+jData[i].name+'</span>'+'&nbsp;&nbsp;<input name="'+jData[i].goodsid+'|'+jData[i].goodssubid+'|'+jData[i].isbind+'|'+jData[i].istrade+'" value="'+jData[i].num+'" style="width:20px;text-align:right" onclick="javascript:this.select();" onkeyup="javascript:changeValueInt(this, '+jData[i].num+');" /><font color="green">('+jData[i].num+')</font></li>';
	}
	html += '</ul>';
	html += '<br/>';
	html += '</div>';
	MM_showDialog(
			html, SGLang.SmeltDecomposeMulti, [
				{title:w.lang.dialog_confirm_button,act:function(){
														if(!$('decomposeList')) return false;
														var objList = $('decomposeList').getElementsByTagName('input');
														var param = '';
														for(var i=0;i<objList.length;i++){
															if(objList[i].value != '' && !isNaN(objList[i].value) && parseInt(objList[i].value) > 0){
																param += '&list[]='+objList[i].name+'|'+objList[i].value;
															}
														}
														if(param==''){return false;}
														var returnURL='';
														if($('smeltReturnURL')){returnURL = $('smeltReturnURL').value;}
														MM_xmlLoad('smelt.doDecomposeMulti'+param+'&returnURL='+returnURL);
													}
				},
				{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();}}
			]);

}

//据点战出征页面，设置出征武将
function stronghold_general_set(selectobj,attack_corps,defend_corps)
{
	if (selectobj.value==0){
		var arr = attack_corps.split(',');
	}else{
		var arr = defend_corps.split(',');
	}
	var obj;
	for (var i=0; i<arr.length; i++){
		obj = $('general'+(i+1));
		if (arr[i] == 1){
			obj.selectedIndex = 0;
			obj.disabled = true;
		}else{
			obj.selectedIndex = 0;
			obj.disabled = false;
		}
	}
}

//提交有奖问卷
function submitResearchQuestion(count)
{
	var formObj = document.form_research_question;
	var qType;
	var tmpObj;
	var tmpFinish;
	for(var i=0;i<count;i++){
		tmpFinish = false;
		qType = $('questionType'+i).value;
		if(qType== 'radio'){
			tmpObj = formObj.elements['option_'+i];
			if(tmpObj.length == 1){
				if(tmpObj.checked == true) tmpFinish = true;
			}else{
				for(var j=0;j<tmpObj.length; j++){
					if(tmpObj[j].checked == true) tmpFinish = true;
				}
			}
			if(!tmpFinish){
				if(typeof(formObj.elements['input_'+i]) != 'undefined' && formObj.elements['input_'+i].value.trim()!='') tmpFinish = true;
			}
		}
		if(qType== 'checkbox'){
			tmpObj = formObj.elements['option_'+i+'[]'];
			if(tmpObj.length == 1){
				if(tmpObj.checked == true) tmpFinish = true;
			}else{
				for(var j=0;j<tmpObj.length; j++){
					if(tmpObj[j].checked == true) tmpFinish = true;
				}
			}
			if(!tmpFinish){
				if(typeof(formObj.elements['input_'+i]) != 'undefined' && formObj.elements['input_'+i].value.trim()!='') tmpFinish = true;
			}
		}
		if(qType== 'textarea'){
			if(typeof(formObj.elements['input_'+i]) != 'undefined' && formObj.elements['input_'+i].value.trim()!='') tmpFinish = true;
		}
		if(!tmpFinish){alertDialog('keep',SGLang.ResearchQuestionUnfinish); return false;}
	}
	formObj.submit();
}

//自动升级小框
function autograde(cid,pid,txt) {
	MM_showDialog(
		'<p>'+txt+'<input type="text" id="dialoginput" name="dialoginput" value=1 maxlength="2" size="10"/></p>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){ {MM_closeDialog();MM_iframePost('autobuild.created&cid='+cid+'&pid='+pid+'&level='+$('dialoginput').value);return false;}}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();return false;}}
		]);

}

//自动拆除小框
function autodemolish(bid,txt){
	MM_showDialog(
			'<p>'+txt+'<p><input type="radio" id="dialoginput1" name="dialoginput" value=1 checked/>'+SGLang.NormalDemolish+'<input type="radio" id="dialoginput2" name="dialoginput" value=2 />'+SGLang.QuiteDemolish+'</p>', '', [
				{title:w.lang.dialog_confirm_button,act:function(){var d_value=1; if ($('dialoginput1').checked){d_value=1} if($('dialoginput2').checked){d_value=2;} MM_closeDialog();MM_iframePost('build.detailde&bid='+bid+'&type='+d_value);return false;}},
				{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();return false;}}
			]);
}
//资源袋储备
function storeresource(url){
	var storeno = parseInt($('storeno').value,10);
	var url = url+'&storeno='+storeno;
	MM_xmlLoad(url);
}

//资源袋储备判断输入值是否合法
function checkstore(){
	var storeno = parseInt($('storeno').value,10);
	var total = $('total').value;
	if(storeno > total){
		$('storeno').value = total;
	}else{
		$('storeno').value = isNaN(storeno) ? 0 : storeno;
	}
}

//偷天换日功能 by wcd
// 确认提示框 参数说明：act,message,submessage,xmlload,title_image,cancel_act
function confirmDialog2(a,b,what,m,s,x,t,c) {
        var a_len=a.length;
        var a_where=a.indexOf('&f_id=')+6;
        var f_id=a.substring(a_where,a_len);
        var b_len=b.length;
        var b_where=b.indexOf('&id_how=')+8;
        var how_many=b.substring(b_where,b_len);
        //alert(how_many);
    	var _a=typeof(a)=='function'?a:(a=='keep'?function(){}:function(){
	    if($('forget_use').value==0){
	    var re = /YYY/g;
        var as=SGLang.Forget_UseSkill.replace(re,what);
MM_showDialog(
		'<h4 class="msgtips">'+as+'</h4>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){ {MM_closeDialog();
			 MM_xmlLoad(a);return false;}}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){
			    MM_closeDialog();
			$(f_id).onclick();return false;}}
		]);

	    }else{
	        if(how_many>0){
	        MM_xmlLoad(b);//进入使用偷天换日道具
	        }else{

	           // 您当前偷天换日数量不足，请点击这里购买
	            MM_showDialog(
		SGLang.Forget_Use_Buy_TT, '', [
			{title:w.lang.dialog_confirm_button,act:function(){ {MM_closeDialog();
			 return false;}}}
		]);
	        }
	    }
	});
	if (c) {var _c=typeof(c)=='function'?c:(c=='keep'?function(){}:function(){x?MM_xmlLoad(c):MM_iframePost(c);});}
	else var _c=function(){};
	MM_showDialog(
		'<h4 class="msgtips"><input type="hidden" id="forget_use" name="forget_use" value="1"><img src="'+w.img_src+'images/icon/alert.gif" class="attention" />'+m+'</h4>'+(s?s:''), t?t:'', [
			{title:w.lang.dialog_confirm_button,act:function() {MM_closeDialog();_a();return false;}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();_c();return false;}}
		]);
}

//by wcd
function forget_useTT(forget)
{
    if(forget=='use'){
    $('forget_use').value='1';//使用
    }else{
        $('forget_use').value='0';  //丢弃
    }
}
//by wcd  未使用暂时
function textLimit(ld,maxlimit)
{

	if ($(ld).value.length > maxlimit){
		$(ld).value =$(ld).value.substring(0, maxlimit);
	}
}
//装备合成
function composeEquip(act, subid, gname, isbind, istrade, count)
{
	count = count - 0;
	eleid = '249_' + subid + '_' + isbind + '_' + istrade;
	//alert(act);
	if(count<=0 || count == null) return ;
	if ($('stone_id_a') == null) {
		MM_xmlLoad(act);
	} else {

		if ($('stone_id_a').value != '' && $('stone_id_b').value != '' && $('stone_id_c').value != '') return ;
		if ($(eleid).innerHTML <= 0) return ;

		var idstr = '';
		if ($('stone_id_a').value == '') {
			idstr = 'a';
		} else if($('stone_id_b').value == '') {
			idstr = 'b';
		}else{
			idstr = 'c';
		}

		$('stone_img_' + idstr).src = $('stone_img_' + idstr).src.replace('null', '249_' + subid);
		$('stone_txt_' + idstr).innerHTML = gname;
		$('stone_cancel_' + idstr).innerHTML = '数量：<span id="itemcount' + idstr + '">' + count + '</span><a class="redtext" onclick="disComposeEquip(\'' + idstr + '\', ' + subid + ','+isbind+','+istrade+ ')" href="javascript:void(0);">' + '【取消】' + '</a>';
		$('stone_id_' + idstr).value = '249_' + subid + '_' + isbind + '_' + istrade + '_' + count;
	}
	reminvalue = $(eleid).innerHTML - count;
	$(eleid).innerHTML = Math.max(reminvalue,0);
	item_a = ' ';
	item_b = ' ';
	item_c = ' ';
	if ($('stone_id_a') != null && $('stone_id_a').value != '') {
		item_a = $('stone_id_a').value;
	}
	if ($('stone_id_b') != null && $('stone_id_b').value != '') {
		item_b = $('stone_id_b').value;
	}
	if ($('stone_id_c') != null && $('stone_id_c').value != '') {
		item_c = $('stone_id_c').value;
	}
	if ($('stone_id_a') != null && $('stone_id_a').value != '' && $('stone_id_b').value != '' && $('stone_id_c').value != '') {
		MM_xmlLoad('smelt.equipCompose&type=compose&subid_a=' + item_a + '&subid_b=' + item_b + '&subid_c=' + item_c + '&keep=all');
	}
}
//装备合成
function disComposeEquip(idstr, subid, isbind, istrade)
{
	itemcount = $('itemcount' + idstr).innerHTML;
	eleid = '249_' + subid + '_' + isbind + '_' + istrade;
	$('stone_img_' + idstr).src = $('stone_img_' + idstr).src.replace('249_' + subid, 'null');;
	$('stone_txt_' + idstr).innerHTML = '请选择进行合成的道具';
	$('stone_cancel_' + idstr).innerHTML = '';
	$('stone_id_' + idstr).value = '';
	$('equipComposeInfo2').innerHTML = '';
	itemcount = itemcount - 0;
	remin = $(eleid).innerHTML - 0 + itemcount;
	$(eleid).innerHTML = Math.max(remin,0);
	/*item_a = ' ';
	item_b = ' ';
	item_c = ' ';
	if ($('stone_id_a') != null && $('stone_id_a').value != '') {
		item_a = $('stone_id_a').value;
	}
	if ($('stone_id_b') != null && $('stone_id_b').value != '') {
		item_b = $('stone_id_b').value;
	}
	if ($('stone_id_c') != null && $('stone_id_c').value != '') {
		item_c = $('stone_id_c').value;
	}
	if ($('stone_id_a') != null && $('stone_id_a').value != '' || $('stone_id_b').value != '' || $('stone_id_c').value != '') {
		MM_xmlLoad('smelt.equipCompose&type=compose&subid_a=' + item_a + '&subid_b=' + item_b + '&subid_c=' + item_c + '&keep=all');
	}else{
		$('equipComposeInfo2').innerHTML = '';
	}*/

}
function checkequipcompise(gid, subid, isbind, istrade){
	var itemid = gid + '_' + subid + '_' + isbind + '_' + istrade;
	var storeno = parseInt($(itemid).innerHTML,10);
	var total = $('storeno').value;
	if(isNaN(total)){
		$('storeno').value = 0;
	}
	if(total > storeno){
		$('storeno').value = storeno;
	}
}

// 做活动时 金币或者礼金券消费的选择
function act_consume(p1,p2,default1,price1,price2) {
	//arr = p2.split('|$|');
	MM_showDialog(
		'<h4>'+p2+'</h4><ul class="commodity_buy"><li><label for="consume_type0" class="canclick"><input type="radio" name="consume_type" id="consume_type0" checked="true" value="0" onClick="act_persion_war(this)">'+default1+SGLang.ConsumeByCoin+'</label><label for="consume_type2" class="canclick"><input type="radio" name="consume_type" id="consume_type2" value="2" onClick="act_persion_war(this)">'+price1+SGLang.ConsumeByCoin+'</label></li><li><label for="consume_type1" class="canclick"><input type="radio" name="consume_type" id="consume_type1" value="1" onClick="act_persion_war(this)"/>'+default1+SGLang.ConsumeByTicket+'</label><label for="consume_type3" class="canclick"><input type="radio" name="consume_type" id="consume_type3" value="3" onClick="act_persion_war(this)"/>'+price2+SGLang.ConsumeByCoin+'</label></li></ul><input type="hidden" id="type_value" name="type_value" value="0">', '', [
			{title:w.lang.dialog_confirm_button,act:function(){MM_closeDialog();MM_xmlLoad(p1+'&consume_type='+$('type_value').value);return false;}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();}}
		]);
}
function act_persion_war(obj){
	$('type_value').value=obj.value;
}

//by wcd 免战符变时间
function changeNewTime(new_time){
	$('newertips').style.display='none';
	$('newertips2').innerHTML='<div sgtitle="'+new_time+'" style="display:block;">新手保护期</div>';
}
//自动平仓增加次数
function auto_trade_num(p1,p2) {
	MM_showDialog(
		'<p>'+p1+'<input type="text" id="dialog_input" value="" style="width:50px;"/>'+p2+'</p>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){if ($('dialog_input').value!='') {MM_closeDialog();MM_iframePost('superhelper.add_auto&num='+encodeURIComponent($('dialog_input').value));return false;}}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();return false;}}
		]);
}

/**
带输入框的提示框
*/
function showInputDialog(acturl,txt) {
	MM_showDialog(
		'<p>'+txt+'<input type="text" id="dialoginput" name="dialoginput" value=1 maxlength="2" size="10"/></p>', '', [
			{title:w.lang.dialog_confirm_button,act:function(){ {MM_closeDialog();MM_iframePost(acturl+'&inputv='+$('dialoginput').value);return false;}}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();return false;}}
		]);

}

function MM_ucopyMovePos(gcopyid,pos)
{
	if ($('ucopyCanMove').value == '0') {
		alertDialog('keep', SGLang.UcopyCanNotMove);
	} else {
		MM_xmlLoad('ucopy.moveToPos&gcopyid='+gcopyid+'&aimpos='+pos+'&keep=all');
	}
}

function submitSaleGifts()
{
	var id = arguments[0];
	var subid = arguments[1];
	var goodsname = arguments[2];
	var istrade = arguments[3];
	var itemid = arguments[4];
	if (typeof(id)!="number" || isNaN(id) || id<1)
	{
		alertDialog('keep',SGLang.GoodsIdError);///old:grx:错误的道具ＩＤ
		return;
	}

	var numb = parseInt($('bestows_goods_number').value) + 0;
	if (typeof(numb)!="number" || isNaN(numb) || numb<1)
	{
		alertDialog('keep',SGLang.GoodsinputBestowNum);///old:grx:请输入您想赠送的道具数量（大于0）
		return;
	}

	var frd = $('bestows_goods_friend').value;
	if (frd == "")
	{
		alertDialog('keep',SGLang.GoodsBestowName);///old:grx:请输入赠送用户的名称
		return;
	}

	var confirmStr = SGLang.GoodsBestowNum.replace(/\[AAA\]/,
	goodsname
	).replace(/\[BBB\]/,
	numb
	).replace(/\[CCC\]/,
	frd
	);
	///old:grx:您要将 <font color="blue">'+goodsname+'</font> <font color="red">× '+numb+'</font> 赠与 <font color="green">'+ frd + '</font> 吗？
	confirmDialog("saleshop.giveSaleGoods&itemid="+itemid+"&id="   +id+"&subid="+subid+"&numb="+numb+"&istrade="+istrade+"&friend="+encodeURIComponent(frd)+"&keep=all", confirmStr);
}
//关闭战场服tips
function close_campaign_tips(npcids, userids)
{
	var npc_list = npcids.split(',');
	var user_list = userids.split(',');
	try{
		for (var i=0; i<npc_list.length; i++){
			if ($('shownpc'+npc_list[i]))	$('shownpc'+npc_list[i]).style.display='none';
		}
	}catch(e){}
	try{
		for (var i=0; i<user_list.length; i++){
			if ($('showuser'+user_list[i]))	$('showuser'+user_list[i]).style.display='none';
		}
	}catch(e){}
}

// 道具确认提示框 参数说明：goodssubid,isbind,istrade,totalnum,usecount,message,submessage,xmlload,title_image,cancel_act
function useGoodsConfirmResBag(goodssubid,isbind,istrade,totalnum,usecount,m,s,x,t,c) {
	var _a=function(){
		if (usecount > 0) {
			var _num = $('use_resbag_cnt').value;
			var _num = _num - 0;
			if (_num > 0 && _num <= totalnum && _num <= usecount) {
				MM_xmlLoad('store.useitem&id=115&subid='+goodssubid+'&isbind='+isbind+'&istrade='+istrade+'&usecnt='+_num+'&keep=all');
			} else {
				alertDialog('keep', SGLang.ResBagUseCntErr);//请输入正确的资源袋使用数量！
			}
		} else {
			alertDialog('keep', SGLang.ResBagDailyOver);//今天的资源袋可用次数已经用完，不能再使用了！
		}
	};
	if (c) {var _c=typeof(c)=='function'?c:(c=='keep'?function(){}:function(){x?MM_xmlLoad(c):MM_iframePost(c);});}
	else var _c=function(){};
	MM_showDialog(
		''+m+(s?s:''), t?t:'', [
			{title:w.lang.dialog_confirm_button,act:function() {MM_closeDialog();_a();return false;}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();_c();return false;}}
		]);
}

function chooseTripodMyCamp(campid,top,left)
{
	if ($('my_camp_id').value == campid) {
		$('my_camp_id').value = '';
		$('chooseMyCamp').style.display = 'none';
		return ;
	}

	$('my_camp_id').value = campid;
	$('chooseMyCamp').style.display = 'block';
	$('chooseMyCamp').style.top = top + 'px';
	$('chooseMyCamp').style.left = left + 'px';
}

//百废俱兴道具选择源建筑或目标建筑，保证选择的建筑唯一
function changeCopyVillage(selectId, selectType)
{
	var selectValue = $(selectType+''+selectId).value;
	if (selectValue=='-1'){
		return;
	}
	for (var i=1; i<6; i++){
		if (i == selectId)	continue;
		if ($(selectType+''+i).value == selectValue){
			$(selectType+''+i).selectedIndex = 0;
		}

	}
}
//重新选择
function resetCopyVillageSelect(selectType1,selectType2)
{
	for (var i=1; i<6; i++){
		$(selectType1+''+i).selectedIndex = 0;
		$(selectType2+''+i).selectedIndex = 0;
	}
}

//提交复制建筑或资源田的请求操作
function copyVillageSubmit(act,source_vid,target_vid,msg)
{
	var url = act+'&source_vid='+source_vid+'&target_vid='+target_vid;
	for (var i=1; i<6; i++){
		url += '&source_type'+i+'='+$('source_type'+i).value;
		url += '&target_type'+i+'='+$('target_type'+i).value;
	}
	confirmDialog(url, msg);
}

function meltsoul(gid,itemid,nowvalue,maxvalue){
	var boxinfo = $('boxinfo').value;
	if(boxinfo == 0) {alertDialog('keep',"请选择您要使用的魂匣");return false;}
	if(isNaN(nowvalue) || isNaN(maxvalue)) return false;
	var binfo = boxinfo.split("-");
	var box_id = binfo[0];
	var box_in = binfo[1];
	var box_num= binfo[2];
	if(isNaN(box_id) || box_id<=0) return false;
	if(isNaN(box_in) || box_in<=0) return false;
	if(isNaN(box_num) || box_num<=0) return false;
	if(parseInt(box_in)+parseInt(nowvalue) > maxvalue){
		confirmDialog('smelt.useboxsoul&bid='+box_id+'&gid='+gid+'&itemid='+itemid, "您的武器无法存储如此多的魂魄，超过存储上限的魂魄将会消失，请问你要继续这么做吗？");
	}else{
		MM_xmlLoad('smelt.useboxsoul&bid='+box_id+'&gid='+gid+'&itemid='+itemid);
	}
}
//三国战役出兵
function checksoldiers(tabid){
	var total = $(tabid).value;
	if(isNaN(total)){
		$(tabid).value = 0;
		alertDialog('keep', SGLang.IllegalCharacter);
	}
}

//符文兑换JS 验证 wufengdong 2011-07-28
function checkRuneExchange(count){
	var total = $('itemcount').value;
	var reg = /^[0-9]+.?[0-9]*$/;   //判断字符串是否为数字判断正整数 /^[1-9]+[0-9]*]*$/
    if (reg.test(total))
    {
		$('itemcount').value = Math.floor(total);
    }
	if(isNaN(total)){
		$('itemcount').value = 0;
	}
	if((total<20) && (count>=20)){
		$('itemcount').value = 20;
	}
	if(count<20){
		$('itemcount').value = 0;
	}
	if((total>count) && (total>20) && (count>20)){
		$('itemcount').value = count;
	}
}
//符文兑换效果实现
function userRuneCount(){
var count = $('userRuneCount').value;
$('itemcount').value = count;
}

//装备升级使用强化券时的切换标签
function equipQuanTab(tab){
	var flag = $('iscoins').checked;
	if(tab == 1){
		$('equipyuan').style.display='block';
		$('equipquan').style.display='none';
		$('now_li').className='now';
		$('normal_li').className='normal';
		if(flag){
			$('equip_foot_2').style.display='block';
		}else{
			$('equip_foot_1').style.display='block';
		}
	}else {
		$('equipquan').style.display='block';
		$('equipyuan').style.display='none';
		$('normal_li').className='now';
		$('now_li').className='normal';
		$('equip_foot_2').style.display='none';
		$('equip_foot_1').style.display='none';
	}
}

//选择金币不掉级功能后的参数控制
function equipIsCoins(){
	var flag = $('iscoins').checked;
	if(flag){
		$('show_coins').style.display = 'block';
		$('equip_foot_1').style.display = 'none';
		$('equip_foot_2').style.display = 'block';
	}else{
		$('show_coins').style.display = 'none';
		$('equip_foot_1').style.display = 'block';
		$('equip_foot_2').style.display = 'none';
	}
}

//控制神·天子令玩家的验证输入
function checkValue(v){
	var key = $('selectTime'+v).value;
	$('selectTime'+v).value = isNaN(key) ? 0 : key;
}

//神·天子令的输入时间设置 防止超过最大值
function checkVipTime(selectId){
	var maxCount = $('maxCount').value;
	var maxTime = $('maxTime').value;
	var selectTime = parseInt($('selectTime'+selectId).value);
	$('selectTime'+selectId).value = isNaN(selectTime) ? 0 : selectTime;
	if(selectTime == 0) return;
	var sumTime = 0;
	for(var i=1; i <= maxCount; i++){
		if(i == parseInt(selectId)) continue;
		if(isNaN($('selectTime'+i).value)) continue;
		sumTime +=  parseInt($('selectTime'+i).value);
	}
	if((sumTime + selectTime) > maxTime){
		var setTime = maxTime - sumTime;
		$('selectTime'+selectId).value = setTime;
	}
}

//神·天子令建筑选择控制 保证选择的建筑唯一
function changeVipBuildings(selectId){
	var selectValue = $('selectType'+selectId).value;
	if(selectValue == '-1') return;
	var maxCount = $('maxCount').value;
	for (var i=1; i <= maxCount; i++){
		if (i == selectId)	continue;
		if ($('selectType'+i).value == selectValue){
			$('selectType'+i).selectedIndex = 0;
		}
	}
}

//提交神·天子令保护建筑的配置请求
function vipConfigSubmit(act,msg){
	var maxCount = $('maxCount').value;
	var url = act;
	for(var i=1; i <= maxCount; i++ ){
		url += '&bid'+i+'='+$('selectType'+i).value;
		var time = isNaN($('selectTime'+i).value) ? 0 : $('selectTime'+i).value;
		url += '&pt_time'+i+'='+time;
	}
	confirmDialog(url, msg);
}
//判断是否为汉字
//wufengdong 2011-08-02
function isChinese(id)
{
	var code = $(id).value;
	var reg=/[\u4E00-\u9FA5]/g;
	if (reg.test(code)){
		$(id).value = '';
	}
}

// 道具确认提示框 参数说明：goodssubid,isbind,istrade,totalnum,usecount,message,submessage,xmlload,title_image,cancel_act
function useGoodsConfirmRandGift(goodssubid,isbind,istrade,totalnum,usecount,m,s,x,t,c) {
	var _a=function(){
		if (usecount > 0) {
			var _num = $('use_randgift_cnt').value;
			var _num = _num - 0;
			if($('ifbroad').checked){
				var ifbroad = 1;
			}else{
				var ifbroad = 0;
			}
			if (_num > 0 && _num <= totalnum && _num <= usecount) {
				MM_xmlLoad('store.useitem&id=1046&subid='+goodssubid+'&isbind='+isbind+'&istrade='+istrade+'&usecnt='+_num+'&ifbroad='+ifbroad+'&keep=all');
			} else {
				alertDialog('keep', SGLang.RandGiftUseCntErr);
			}
		} else {
			alertDialog('keep', SGLang.RandGiftUseCntErr);
		}
	};
	if (c) {var _c=typeof(c)=='function'?c:(c=='keep'?function(){}:function(){x?MM_xmlLoad(c):MM_iframePost(c);});}
	else var _c=function(){};
	MM_showDialog(
		''+m+(s?s:''), t?t:'', [
			{title:w.lang.dialog_confirm_button,act:function() {MM_closeDialog();_a();return false;}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();_c();return false;}}
		]);
}
//全神贯注道具新加js
function DuEsetValue(id){
	if(id<0 || id>4 || isNaN(id)){
		id=0;
	}
	var id1='item1047_1';
	var id2='item1047_2';
	var id3='item1047_3';
	var id4='item1047_4';
	var id5='item1047_'+id;

	if(id!=0){
		if($(id5).value==0){
			$(id5).checked=true;
			try{
				var var1=$(id1).value;
				if(var1!='undefined' && var1!=''){
					$(id1).value=0;
				}
			}catch(e){}
			try{
				var var2=$(id2).value;
				if(var2!='undefined' && var2!=''){
					$(id2).value=0;
				}
			}catch(e){}
			try{
				var var3=$(id3).value;
				if(var3!='undefined' && var3!=''){
					$(id3).value=0;
				}
			}catch(e){}
			try{
				var var4=$(id4).value;
				if(var4!='undefined' && var4!=''){
					$(id4).value=0;
				}
			}catch(e){}
			$(id5).value=1;//覆盖，重新赋值
			$('item1047').value=id;
		}else{
			$(id5).checked=false;
			try{
				var var1=$(id1).value;
				if(var1!='undefined' && var1!=''){
					$(id1).value=0;
				}
			}catch(e){}
			try{
				var var2=$(id2).value;
				if(var2!='undefined' && var2!=''){
					$(id2).value=0;
				}
			}catch(e){}
			try{
				var var3=$(id3).value;
				if(var3!='undefined' && var3!=''){
					$(id3).value=0;
				}
			}catch(e){}
			try{
				var var4=$(id4).value;
				if(var4!='undefined' && var4!=''){
					$(id4).value=0;
				}
			}catch(e){
			}
			$(id5).value=0;
			$('item1047').value=0;
		}
	}else{
		$(id1).checked=false;
		$(id2).checked=false;
		$(id3).checked=false;
		$(id4).checked=false;
		$(id1).value=0;
		$(id2).value=0;
		$(id3).value=0;
		$(id4).value=0;
		$('item1047').value=0;
	}
}

//npc宝箱
//选择切换
function tabNpcCheck(id){
	var npccheckdid = $('npccheckid').value;
	$('npcgift'+npccheckdid).style.background = '';
	$('npccheckid').value = id;
	$('npcgift'+id).style.background = '#FF9900';
}

//执行问道或者论剑
function runNpcRand(chose, no){
	checkd = $('npccheckid').value;
	if (no == 1 || no == 10 || no == 30){
		$url = 'npcrandgift.runNpcRand&chose='+chose+'&usecnt='+no+'&npcid='+checkd+'&keep=all';
		MM_xmlLoad($url);
	}else{
		alertDialog('keep', SGLang.NpcRandGiftUseCntErr);
	}
}

//购买佩玉或者佩刀的js
// 道具确认提示框 参数说明：goodssubid,isbind,istrade,totalnum,usecount,message,submessage,xmlload,title_image,cancel_act
function buyNpckey(chose,type,m,s,x,t,c) {
	var _a=function(){
		var buycnt = $('buynpc_cnt').value;
		if (chose != 0 && chose != 1) {
			alertDialog('keep', SGLang.NpcRandGiftUseCntErr);
		} else if(type != 1 && type != 2 && type != 3 && type != 4) {
			alertDialog('keep', SGLang.NpcRandGiftUseCntErr);
		} else if(buycnt == 0){
			var buycnt = 1;
		} else {
			//npcrandgift.buyNpcKey&chose=0&type=3
			MM_xmlLoad('npcrandgift.buyNpcKey&chose='+chose+'&type='+type+'&buycnt='+buycnt);
		}
	}
	if (c) {var _c=typeof(c)=='function'?c:(c=='keep'?function(){}:function(){x?MM_xmlLoad(c):MM_iframePost(c);});}
	else var _c=function(){};
	MM_showDialog(
	''+m+(s?s:''), t?t:'', [
	{title:w.lang.dialog_confirm_button,act:function() {MM_closeDialog();_a();return false;}},
	{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();_c();return false;}}
	]);
}

//计算批次的金币数
function MM_setsumcoins(buycnt, type, price){
	if(buycnt == 0 || isNaN(buycnt)){
		$('buynpc_cnt').value = 0;
		var buycnt = 0;
	}
	var buycnt = Math.floor(buycnt);
	$('buynpc_cnt').value = buycnt;
	if(type == 1){
		$('sumkeycnt').innerHTML = buycnt * price;
		$('sumkeycnt').innerHTML += ' '+SGLang.ConsumeCoin;
	}else{
		$('sumkeycnt').innerHTML = buycnt * price;
		$('sumkeycnt').innerHTML += ' '+SGLang.ConsumeZhubi;
	}
}

//计算铸币兑换的数量显示
function MM_setzhubicnt(){
	var changecnt=$('changecnt').value;
	if(changecnt <= 0 || isNaN(changecnt)){
		$('changecnt').value = 0;
		var changecnt = 0;
	}
	var changecnt = Math.floor(changecnt);
	$('changecnt').value = changecnt;
	var changetype = $('npcgifttype').value ? 1 :0;
//	var rateinfo = $('changerate').value ? $('changerate').value : '1:25'; 
//by wcd   解决一个bug
	var rateinfo = '1:25';
	var ratearr =  rateinfo.split(':');
	var rate = ratearr[1];
	if (changecnt * rate > 2000){
		$('sumzhubicnt').innerHTML = 2000;
		$('changecnt').value = 2000/rate;
	}else{
		$('sumzhubicnt').innerHTML = changecnt * rate;
	}
}

//执行佩刀佩玉兑换铸币的操作
function MM_changezhubi(){
	var changecnt = $("changecnt").value;
	var type = $("npcgifttype").value;
	if(changecnt == 0){
		alertDialog('keep', SGLang.ChangeNoErr);
	}else{
		$url = 'npcrandgift.changeZhubi&type='+type+'&changecnt='+changecnt+'&keep=all';
		MM_xmlLoad($url);
	}
}


//验证仓库
function checkstorage(count){
	var total = $('gonum').value;
	var reg = /^[0-9]+.?[0-9]*$/;   //判断字符串是否为数字判断正整数 /^[1-9]+[0-9]*]*$/
    if (reg.test(total))
    {
		$('gonum').value = Math.floor(total);
    }
	if(isNaN(total) || total<1){
		$('gonum').value = 1;
	}
	if((total>count)){
		$('gonum').value = count;
	}
}

//验证仓库
function checklost(count){
	var total = $('rmnum').value;
	var reg = /^[0-9]+.?[0-9]*$/;   //判断字符串是否为数字判断正整数 /^[1-9]+[0-9]*]*$/
    if (reg.test(total))
    {
		$('rmnum').value = Math.floor(total);
    }
	if(isNaN(total) || total<1){
		$('rmnum').value = 1;
	}
	if((total>count)){
		$('rmnum').value = count;
	}
}

//验证 伤兵营
function checkinjured(id,count){
	var total = $('num_'+id).value;
	//alert(total);
	//alert(count);
	var reg = /^[0-9]+.?[0-9]*$/;   //判断字符串是否为数字判断正整数 /^[1-9]+[0-9]*]*$/
    if (reg.test(total))
    {
		$('num_'+id).value = Math.floor(total);
    }
	if(isNaN(total) || total<1){
		$('num_'+id).value = count;
	}
	if((total>count)){
		$('num_'+id).value = count;
	}
}

//金币消费 礼金券消费 功能化道具选择 
function selectconsumertype(p1,p2,bycoin,byTicket,byRune) {
	//arr = p2.split('|$|');
	var goldchecked = (bycoin == '') ? 'checked="true"' : '';
	var html = '<h4>'+p2+'</h4><ul class="commodity_buy">';
	if (bycoin != '') {
		html = html + '<li><label for="consume_type0" class="canclick"><input type="radio" name="consume_type" id="consume_type0" checked="true" value="0" />'+SGLang.ConsumeByCoin+'<strong class=\'red\'>（'+SGLang.NeedCoin+bycoin+'）</strong>'+'</label></li>';
	}
	if (byTicket != '') {
		html = html + '<li><label for="consume_type1" class="canclick"><input type="radio" name="consume_type" id="consume_type1"  value="1" />'+SGLang.ConsumeByTicket+'<strong class=\'red\'>（'+SGLang.NeedTickte+byTicket+'）</strong>'+'</label></li>';
	}
	if (byRune != '') {
		html = html + '<li><label for="consume_type2" class="canclick"><input type="radio" name="consume_type" id="consume_type2"  value="2" />'+SGLang.ConsumeByRune+'<strong class=\'red\'>（'+SGLang.NeedRune+byRune+'）</strong>'+'</label></li>';
	}
	html = html + '</ul>';
	MM_showDialog(
		html, '', [
			{title:w.lang.dialog_confirm_button,act:function(){var ret = false;var consume_type = 0;if (byRune !='' && $('consume_type2').checked) {ret = true; consume_type=2;}if (bycoin !='' && $('consume_type0').checked) {ret = true; consume_type=0;}if (byTicket !='' && $('consume_type1').checked) {ret = true; consume_type=1;}if (ret == false) return false;MM_closeDialog();MM_xmlLoad(p1+'&consume_type='+consume_type);return false;}},
			{title:w.lang.dialog_cancel_button,style:'dialogcancel',act:function(){MM_closeDialog();}}
		]);
}
/**
*赛马页面效果
*/
function startrace(act,aspeed,dspeed){
	var attackhorseId = 1;
	var defendhorseId = 1;
	var attspeed = aspeed - 0;
	var defspeed = dspeed - 0;
	
	//关闭开始按钮
	$('start').style.display = 'none';
	$('maci').style.display = 'none';
	$('jiabian').style.display = 'none';
	$('badou').style.display = 'none';
	
	//被挑战方
	w.defInterval = setInterval(
	function(){
		defendhorseId++;
		predefend = defendhorseId - 1;
		$('defendhorse'+predefend).style.display = 'none';
		$('defendhorse'+defendhorseId).style.display = '';
		if (defendhorseId >= 15) {
			clearInterval(w.defInterval);
			clearInterval(w.attInterval);
			//if(defspeed > attspeed){
				MM_xmlLoad(act);
				//alert('OOOOOOOdefspeed:attspeed='+defspeed+':'+attspeed);
				//return false;
			//}
		}
	},
	defspeed
	);
	//挑战方
	w.attInterval =  setInterval(
	function(){
		attackhorseId++;
		preattack = attackhorseId - 1;
		$('attackhorse'+preattack).style.display = 'none';
		$('attackhorse'+attackhorseId).style.display = '';
		if (attackhorseId >= 15) {
			//clearInterval(attInterval);
			//if(attspeed > defspeed){
			clearInterval(w.defInterval);
			clearInterval(w.attInterval);
				MM_xmlLoad(act);
				//alert('XXXXXXXdefspeed:attspeed='+defspeed+':'+attspeed);
				//return false;
			//}
		}
	},
	attspeed
	);
	/*var execTime = null;
	var execres = null;
	execTime = Math.max(attspeed,defspeed) * 14;
	alert(execTime);
	execres = setTimeout("act",execTime);*/

}
//残片系统切换可合成道具
	function changeListInfo(v){
		var name=v;
		var ww=name.split('_');
		var select1=$('select_one').value;
		var subid=$('goods_subid').value;
		var mainid=ww[1];
		var loopid=ww[2];
		if(select1==mainid)return false;//若没有切换，也就是点击原来的物品，将不会发请求
		var url='canpian.main&main_id='+mainid+'&goods_subid='+subid+'&keep=all';
		MM_xmlLoad(url);
	}
	//清除cd提示 by wcd
	//本来已经用php实现了，但是觉得不太好，就用js重新实现了
	function clearCDTime(bid,type,id){
		var bid=bid;
		var type=type;
		var id='wcd_cd'+id;
		var str=$(id).value;
		var new_str = str.replace(/:/g,'-');
		var new_str = new_str.replace(/ /g,'-');
		var arr = new_str.split('-');
		var timen = new Date(Date.UTC(arr[0],arr[1]-1,arr[2],arr[3]-8,arr[4],arr[5]));
		var value=parseInt((timen.getTime())/1000);//PHP计算时间戳  从页面获取的 
		//var now=new Date().getTime();
		var now=parseInt(new Date().getTime()/1000); //当前PHP显示的时间戳
		//alert('v'+value);alert('now'+now);
		if(type!=1 && type!=2){
			alertDialog('keep', '数据错误');
			return false;
		}
		if(value-now < 5 || value <= now){
			alertDialog('keep', '您的CD已经或者将要结束');
			return false;
		}
		var coin=Math.ceil((value-now)/3600) * 20;
		if(coin==0){
			alertDialog('keep', '您的CD已经或者将要结束');
			return false;
		}
		var confirmStr='将花费您XXX金币，您确认么?';
		var confirmStr=confirmStr.replace(/XXX/g,coin);
		confirmDialog("build.act&btid=42&do=clearCDTime&type="+type+"&id="+bid, confirmStr,'','','','',1);
	}
function changeShow(id){
	var op=$(id);
	var chooseid=op.selectedIndex;
	var gid=op.options[chooseid].value;
	var url='general.generalCreate&id='+gid;
	MM_xmlLoad(url);
}
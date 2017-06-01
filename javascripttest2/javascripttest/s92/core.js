// -*- coding: utf-8; mode: java; -*-
// version: 1.22
var w=window, d=w.document;
var game_version = '1.733';
var can_act = 0;
var IE_all_cache = new Object();
w.rd=Math.ceil(Math.random()*1000000);
w.uh=[];
w.mainAct = "";
w.canvasact = 'resources.status';
w.resos = [];
w.resosnumber = 0;
w.hookedFunctions = [];
w.currentSeconds = 0;
w.maxtips={lumber:"",clay:"",iron:"",crop:""};
w.broadcasts = [];
w.broadcastColor = {1:"#FF0000",2:"#FFFF00",3:"#00FF00",4:"#FF99CC",5:"#00CCFF",6:"#FF0000",7:"#FF00FF",8:"#FF00FF",9:"#00FFFF",10:"#00FFFF"};
w.broadcastDoing = false;
w.timeoutId = '';
w.defInterval = null;
w.attInterval = null;
var mystore_main_usergoldcoins = 0;

var $ = function(){return (typeof(arguments[0])!='string' || !arguments[0] || arguments[0]=="") ? false : d.getElementById(arguments[0]);};
var IE_getElementById = function (id) {if (IE_all_cache[id] == null) IE_all_cache[id] = d.all[id];return IE_all_cache[id];};
if (d.all) {
	if (!d.getElementById) {d.getElementById = IE_getElementById;};
	d.getElementsByName = function(name) {var elem = document.getElementsByTagName("span");var arr = new Array();for(i = 0,iarr = 0; i < elem.length; i++) {	att = elem[i].getAttribute("name");if(att == name) {	arr[iarr] = elem[i];	iarr++;	}}return arr;}
}
var ucalDate = function (mdate) {
	var mt = "";
	var cx = new Date();
	this.cmktime = Math.ceil(cx.getTime()/1000);
	if (!this.cmktime){this.cmktime=0}
	if (!mdate || mdate == "") {
		var cy = cx.getFullYear();
		var cm = cx.getMonth()+1;
		var cd = cx.getDate();
		var ch = cx.getHours();
		var ci = cx.getMinutes();
		var cs = cx.getSeconds();
		if (cm < 10) cm = "0"+cm;
		if (cd < 10) cd = "0"+cd;
		if (ch < 10) ch = "0"+ch;
		if (ci < 10) ci = "0"+ci;
		if (cs < 10) cs = "0"+cs;
		mdate = cy+'-'+cm+'-'+cd+' '+ch+':'+ci+':'+cs;
	}
	this.date = mdate;
	this.year = mdate.substring(0,4);
	this.month = mdate.substring(5,7);
	this.day = mdate.substring(8,10);
	this.hours = mdate.substring(11,13);
	this.minutes = mdate.substring(14,16);
	this.seconds = mdate.substring(17,19);
	cx = new Date(this.year*1, this.month*1-1, this.day*1, this.hours*1, this.minutes*1, this.seconds*1);
	this.smktime = Math.ceil(cx.getTime()/1000);
	if (!this.smktime){this.smktime=0}
	this._mktime = (1*this.smktime)-(1*this.cmktime*1);
}

w.debug = 0;
w.ucal_cdate = new ucalDate();
w.ucal_sdate = w.ucal_cdate;
w.dataUrl = "index.php";
w.isNav = (navigator.appName == "Netscape") ? true : false;
w.isIE  = (navigator.appName.indexOf("Microsoft") != -1) ? true : false;
w.isOp  = (navigator.appName.indexOf("Opera") != -1) ? true : false;
w.isSaf = (navigator.appVersion.indexOf("Safari")!=-1) ? true : false;
w.isMac = (navigator.appVersion.indexOf("Mac") != -1) ? true : false;
w.isIE5 = (navigator.appVersion.indexOf("MSIE 5") != -1) ? true : false;
w.isIE8 = (w.isIE && d.documentMode) ? true : false;
w.isVer = parseInt(navigator.appVersion.charAt(0));
w.ns4 = (document.layers)? true:false;
w.ie4 = (document.all)? true:false;
w.ns6 = (!document.all && document.getElementById) ? true:false;
w.isWin = (navigator.appVersion.toLowerCase().indexOf("win") != -1) ? true : false;

String.prototype.trim=function(){return this.replace(/(^\s*)|(\s*$)/g, "");};
String.prototype.removeEnter=function(){return this.replace(/\n/g, " ").replace(/\r/g, " ")};

Array.prototype.remove=function(n){return (n<0) ? this : this.slice(0,n).concat(this.slice(n+1,this.length));};

if(w.isNav || (typeof HTMLElement!="undefined" && !HTMLElement.prototype.insertAdjacentElement)) {
	HTMLElement.prototype.insertAdjacentElement = function(where,parsedNode) {switch (where) {case 'beforeBegin':this.parentNode.insertBefore(parsedNode,this);break;case 'afterBegin':	this.insertBefore(parsedNode,this.firstChild);break;case 'beforeEnd':this.appendChild(parsedNode);break;case 'afterEnd':if (this.nextSibling) this.parentNode.insertBefore(parsedNode,this.nextSibling);else this.parentNode.appendChild(parsedNode);break;}};
	HTMLElement.prototype.insertAdjacentHTML = function (where,htmlStr) {	var r = this.ownerDocument.createRange();r.setStartBefore(this);var parsedHTML=r.createContextualFragment(htmlStr);this.insertAdjacentElement(where,parsedHTML);};
	HTMLElement.prototype.insertAdjacentText = function (where,txtStr) {var parsedText = document.createTextNode(txtStr);this.insertAdjacentElement(where,parsedText);};
}

/*** Game Main ***/

function MM_xmlAction()
{
	var xmlObj = new Object();
	var xmlUrl = '';
	if (typeof(arguments[0])!="object") {return;}else{xmlObj=arguments[0];}
	if (typeof(arguments[1])=="string") {xmlUrl=arguments[1];}
	try {MM_setClientTime(xmlObj.game[0].time[0].text);MM_showResos(xmlObj.game[0].resos[0].reso);} catch(e) {}
	try {MM_showReports(xmlObj.game[0].reports);} catch(e) {}
	try {MM_showVillage(xmlObj.game[0].village[0]); } catch(e) {}
	try {MM_showVillageList(xmlObj.game[0].villagelist[0].village); } catch(e) {}

	var keep = 'none';
	try {keep = xmlObj.game[0].keep[0].text; } catch(e) {}
	try {MM_pretreatment(xmlObj.game[0].actions[0], xmlObj.game[0].htmls[0].html)} catch(e) {} // 对返回的URL，先预处�?
	try {MM_showHtmls(xmlObj.game[0].htmls[0].html, keep, xmlUrl)} catch(e) {}

	try {MM_cleftPage(xmlObj.game[0].clefts[0].cleft)} catch(e) {}
	try {MM_popBroadcasts(xmlObj.game[0].broadcasts[0].broadcast)} catch(e) {}
	// 执行 xml 里的 JS
	try {eval(xmlObj.game[0].script[0].text);} catch(e) {}

	/*
	if ($('is_build_time_less') && $('is_build_time_less').value == '1' && $('quickbuildbutton')){
		$('quickbuildbutton').style.display='none';
	}
	*/
	try {MM_locateAction(xmlObj.game[0].locat[0]);} catch(e) {}
	MM_xmlLoaded();
}

function insertAdjacentHTML2(element, html)
{
        var oRange = document.createRange() ;
        oRange.setStartBefore( element ) ;
        var oFragment = oRange.createContextualFragment( html );
        element.parentNode.insertBefore( oFragment, element ) ;
}

function MM_popBroadcasts()
{
	if (w.broadcastDoing) return ;
	else w.broadcastDoing = true;

	w.broadcasts = [];
	var BroadcastsObj=new Object();
	if (typeof(arguments[0])!="object"){return;}else{BroadcastsObj=arguments[0];}

	var BroadcastsObjLen = BroadcastsObj.length;
	var ids = [];

	for (var ii=0; ii<BroadcastsObjLen; ii++)
	{
		w.broadcasts[ii] = new Array(BroadcastsObj[ii].attrib['id'], BroadcastsObj[ii].text, BroadcastsObj[ii].attrib['priority']);
		ids[ii] = BroadcastsObj[ii].attrib['id'];
	}

	//set cookie by ids[]
	setCookie('broadcast_ids', ids.join(','));

	MM_hideBroadcastMsg();
}

function MM_hideBroadcastMsg()
{
	var obj = $('broadcastLayer');

	if (w.broadcasts.length > 0) {
		var objMsg = $('broadcastMsg');
		objMsg.innerHTML = '<span style="visibility:hidden">' + objMsg.innerHTML + '</span>';
		w.broadcastTimer = setTimeout('MM_showBroadcastMsg()', 1000);
	} else {
		w.broadcastTimer = setTimeout('MM_showBroadcastMsg()', 0);
	}
}

function MM_showBroadcastMsg()
{
	var objLayer = $('broadcastLayer');
	var objMsg = $('broadcastMsg');
	var msg = w.broadcasts.shift();

	if (msg == undefined) {
		objLayer.style.display = 'none';
		deleteCookie('broadcast_ids');
		w.broadcastDoing = false;
		return ;
	} else {
		MM_delBroadcastCookie();
	}

	objMsg.innerHTML = msg[1];
	objMsg.style.color = w.broadcastColor[msg[2]];
	objLayer.style.display = 'block';

	w.broadcastTimer = setTimeout('MM_hideBroadcastMsg()', 3000);
}

function MM_delBroadcastCookie()
{
	try {
		var ids = getCookie('broadcast_ids').split(',');
	} catch(e) {
		var ids = [];
		for (var i=0; i<w.broadcasts.length; i++)
		{
			ids[i] = w.broadcasts.splice(i,1)[0];
		}
	}
	var cnt = ids.length;

	if (cnt == 1) {
		deleteCookie('broadcast_ids');
	} else {
		ids.shift();
		setCookie('broadcast_ids', ids.join(','));
	}
}

function MM_locateAction(locat,type) {
	var msg=locat.text;
	var _bt1=locat.attrib['act'];
	var _bt2=locat.attrib['cact'];
	var _type=locat.attrib['type'];
	var _js=locat.attrib['js'];

	if(typeof(_js)=='string' && _js != ''){ eval(_js); }

	if (msg=='' && (_bt1==='/' || _bt1==='./')) {location.href="/";return false;}
	if (msg=='') {
		(_bt1!=='' && !isNaN(_bt1)) ? MM_Nav(parseInt(_bt1)) : MM_xmlLoad(_bt1);
		return;
	}

	if(typeof(_bt1)=='string' && _bt1.search(/^function/i)!=-1 ){eval('_bt1='+_bt2);}
	if(typeof(_bt2)=='string' && _bt2.search(/^function/i)!=-1 ){eval('_bt2='+_bt2);}

	if(_type == 'consume') {
		consume(_bt1,_bt2);
	} else if (_type == 'confirm'){
		confirmDialog(_bt1,msg,'',1,'',_bt2);
	} else {
		alertDialog(_bt1,msg);
	}
}

function MM_pretreatment() {
	if (typeof(arguments[0])!="object") return;
	if (typeof(arguments[1])=="object") {
		for (var ii=0; ii<arguments[1].length; ii++) {
			if (arguments[1][ii].attrib['id']=="main") w.mainAct = "" + arguments[0].attrib['act'];
		}
	}
	try{
		var act = arguments[0].attrib['act'];
		if (act == "resources.status" || act == "build.status" || act == "map.status") w.canvasact=act;
		if(w.canvasact == 'map.status') {
			$('vipz').style.display="none";
			$('floatIcon').style.display="none";
			$('floatbutton').style.display="none";
			$('floatvillagename').style.display="none";
			$('div_radar').style.display="block";
			$('divMapBook').style.display="block";
		} else {
			$('vipz').style.display="block";
			$('floatIcon').style.display="block";
			$('floatbutton').style.display="block";
			$('floatvillagename').style.display="block";
			$('div_radar').style.display="none";
			$('divMapBook').style.display="none";
		}
		if(act == "resources.status" || act == "build.status" || act == "map.status" || act == "alliance.info" || act == "report.main" || act == "emperor.main" || act == "store.main" ) {
			$('div_taskdialog').style.display="none";
		}
	} catch(e){}
}

function MM_showHtmls() {
	var HtmlsObj=new Object();
	if (typeof(arguments[0])!="object"){return;}else{HtmlsObj=arguments[0];}

	var HtmlsObjLen = HtmlsObj.length;
	var Htmls = [];
	var nn = 0;
	var id = HtmlsObj[0].attrib['id']
	for(ii=0; ii<HtmlsObjLen; ii++) {
		if (id!=HtmlsObj[ii].attrib['id']) {nn++;id = HtmlsObj[ii].attrib['id'];}
		var html = HtmlsObj[ii].text;
		if (typeof(Htmls[nn])!="object") {Htmls[nn] = [];Htmls[nn]['id'] = id;Htmls[nn]['html'] = html;} else {Htmls[nn]['html'] += html;}
	}

	var Htmls_count = Htmls.length;
	if (arguments[1]!="all" && arguments[1]!="left") try{MM_closeLeft();}catch(e){}
	if (arguments[1]!="all" && arguments[1]!="center") try{MM_closeFloat();}catch(e){}
	if (arguments[1]!="all" && arguments[1]!="right") try{MM_closeRight();}catch(e){}
	if (arguments[1]!="all" && arguments[1]!="dialog") try{MM_closeDialog();}catch(e){}

	for(ii=0; ii<Htmls_count; ii++) {
		var window_id = Htmls[ii]['id'] + '';
		var content = Htmls[ii]['html'];
		if (window_id == 'ucopymovereport') {
			try{
				$(window_id).innerHTML+=content;
				var ucopyLogObj = $('ucopymovereport');
				ucopyLogObj.scrollTop = ucopyLogObj.scrollHeight;
			}catch(e){}
		}else if(window_id == 'npcxiyou') {
			try{
				$(window_id).innerHTML+=content;
				var ucopyLogObj = $('npcxiyou');
				ucopyLogObj.scrollTop = ucopyLogObj.scrollHeight;
			}catch(e){}
		} else {
			try{$(window_id).innerHTML=content;}catch(e){}
		}
		//赛马系统计数器清除句柄
		if (window_id=='floatblockcenter')
		{
			if (w.defInterval!=null) {
				clearInterval(w.defInterval);
				w.defInterval = null;
			}
			if (w.attInterval!=null) {
				clearInterval(w.attInterval);
				w.attInterval = null;
			}
		}
		if (typeof(arguments[2])=="string" && (window_id=='floatblockcenter' || window_id=='taskcontent' || window_id=='floatblockleft' || window_id=='floatblockright'))
		{
			cacheCtrl.setCacheHtml(arguments[2], window_id, content);
		}
		if (window_id=='floatblockleft' || window_id=='floatblockright' || window_id=='floatblockcenter' || window_id=='dialog' || window_id=='vipmap')
		{
			MM_showHidden(window_id, 'block');
		}
		//if (window_id=='floatblockcenter') MM_showHidden('mask_bottom','block');
		if (window_id=='dialog') {
			$('dialog').style['left']='300px';
			$('dialog').style['top']=parseInt(($('wrapper').offsetHeight-$('dialog').offsetHeight)/2)+"px";
			MM_showHidden('mask_top','block');
			MM_showHidden('mask_bottom','block');
		}
		if (window_id=='player_tips') {
		    MM_showHidden('player_tips', 'block');
		    setTimeout(function(){MM_showHidden('player_tips', 'none');}, 8000);
		}
	}
	try{$('describe').style.display = "none";}catch(e){}
	try{$('describe_y').style.display = "none";}catch(e){}
	try{$('indexbuff').style.display = "none";}catch(e){}
	try{$('map_tips').style.display = "none";}catch(e){}
	try{$('shoptis_div').style.display = "none";}catch(e){}
}

function MM_showVillage() {
	if (typeof(arguments[0])!="object") return;
	var class_name = "";
	if (arguments[0].attrib['villagetype'] == 25) village_title = SGLang.ZhouVillage;///old:grx:州城
	else if (arguments[0].attrib['villagetype'] == 26) village_title = SGLang.JunVillage;///old:grx:郡城
	else if (arguments[0].attrib['villagetype'] == 27) {
		village_title = SGLang.DiduVillage;///old:grx:帝都
		class_name = "palacevillage";
	}
	else if (arguments[0].attrib['villagetype'] == 28) village_title = SGLang.OldZhouVillage;///old:grx:州城
	else if (arguments[0].attrib['villagetype'] == 29) village_title = SGLang.OldJunVillage;///old:grx:郡城
	else if (arguments[0].attrib['villagetype'] == 30) village_title = SGLang.OldDiduVillage;///old:grx:帝都
	else if (arguments[0].attrib['assistant'] == 1) village_title = SGLang.Assistant;
	else village_title = SGLang.OtherVillage;///old:grx:分城
	$('currentcity').innerHTML = (arguments[0].attrib['ismain']==1 ? "<em class=\"mainvillage\">["+SGLang.MainVillage+"]</em>" : "<em class=\""+class_name+"\">["+village_title+"]</em>") + arguments[0].attrib['name'];// + '(' + arguments[0].attrib['x'] + ':' + arguments[0].attrib['y'] + ':' + arguments[0].attrib['villagetype'] + ')';
	$('village_name').innerHTML = arguments[0].attrib['name'] + '<span>(' + arguments[0].attrib['statename'] + ':'  + arguments[0].attrib['x'] + ':' + arguments[0].attrib['y'] + ')</span>' //':' + arguments[0].attrib['villagetype'] + ')</span>';
	try{$('newertips').style.display=arguments[0].attrib['newer']==1?"block":"none"}catch(e){}
	try{$('villagename_back').className=arguments[0].attrib['ismain']==1?"main back":"main back1"}catch(e){}
}
///old:grx:主城
function MM_showVillageList() {
	if (typeof(arguments[0]) != 'object') return;
	var listHtml = '';
	for (i = 0 ; i < arguments[0].length ; i++) {
		var class_name = "";
		if (arguments[0][i].attrib['isstatevillage'] == 25) village_title = SGLang.ZhouVillage;///old:grx:州城
		else if (arguments[0][i].attrib['isstatevillage'] == 26) village_title = SGLang.JunVillage;///old:grx:郡城
		else if (arguments[0][i].attrib['isstatevillage'] == 27){
			village_title = SGLang.DiduVillage;///old:grx:帝都
			class_name = "palacevillage";
		}
		else if (arguments[0][i].attrib['isstatevillage'] == 28) village_title = SGLang.OldZhouVillage;///old:grx:州城
		else if (arguments[0][i].attrib['isstatevillage'] == 29) village_title = SGLang.OldJunVillage;///old:grx:郡城
		else if (arguments[0][i].attrib['isstatevillage'] == 30) village_title = SGLang.OldDiduVillage;///old:grx:帝都
		else if (arguments[0][i].attrib['assistant'] == 1) village_title = SGLang.Assistant;
		else village_title = SGLang.OtherVillage;///old:grx:分城
		if (arguments[0][i].attrib['id'] != w.villageid) listHtml += "<li><a href=\"javascript:changeVillage('" + arguments[0][i].attrib['id'] + "', '" + arguments[0][i].attrib['name'] + "');\"> " + (arguments[0][i].attrib['ismain']=="1"?"<em class=\"mainvillage\">["+SGLang.MainVillage:"<em class=\""+class_name+"\">["+village_title)+ "]</em>" + arguments[0][i].attrib['name']+"</a></li>"; // + "("+arguments[0][i].attrib['x']+":"+arguments[0][i].attrib['y']+":"+arguments[0][i].attrib['state']+")"+"</a></li>";
		///old:grx:主城
		else listHtml += "<li><a class=\"nowcity\" href=\"javascript:void(0);\"> " + (arguments[0][i].attrib['ismain']=="1"?"<em class=\"mainvillage\">["+SGLang.MainVillage:"<em class=\""+class_name+"\">["+village_title)+ "]</em>" + arguments[0][i].attrib['name']+"</a></li>";
	}
	var o = $('listallcity');
	o.style.height=(arguments[0].length>25)?'500px':'auto';
	o.innerHTML = listHtml;
}

function MM_showReports() {
	var rnew = 0;
	if (typeof(arguments[0])!="object") {
		return;
	} else {
		rnew += arguments[0][0].attrib['reportnew'];
		rnew += arguments[0][0].attrib['msgnew'];
	}
	$('index_report_button').style.display=(rnew>0?'block':'none');
}

function MM_showClientTimeTask() {
	var clientTime = new Date();
	clientTime.setTime((w.ucal_sdate._mktime + w.ucal_cdate.cmktime)*1000);
	var y = clientTime.getFullYear();
	var m = clientTime.getMonth()+1;
	var d = clientTime.getDate();
	var h = clientTime.getHours();
	var i = clientTime.getMinutes();
	var s = clientTime.getSeconds();
	if (m < 10) m = "0"+m;
	if (d < 10) d = "0"+d;
	if (h < 10) h = "0"+h;
	if (i < 10) i = "0"+i;
	if (s < 10) s = "0"+s;
	$('servTime').innerHTML = h + ':' + i + ':' + s;//y + '-' + m + '-' + d + ' ' + h + ':' + i + ':' + s;
}

function MM_setClientTime() {
	if (typeof(arguments[0])!="string"){return;};
	w.ucal_sdate = new ucalDate(arguments[0]);
}

function MM_showResos() {
	var ResosObj=new Object();
	if (typeof(arguments[0])!="object"){return;}else{ResosObj=arguments[0];}

	w.resos = [];
	w.resosnumber = 0;
	var ResosID = ['now', 'max', 'speed', 'increase'];
	var ResosObjLen = ResosObj.length;
	var showspeedbutton = false;
	for(var ii=0; ii<ResosObjLen; ii++)
	{
		var ResosIDLen = ResosID.length;
		for (nn=0; nn<ResosIDLen; nn++)
		{
			var id = ResosObj[ii].attrib['id'] + '_' + ResosID[nn];
			var val = Math.floor(ResosObj[ii].attrib[ResosID[nn]]);
			mm = w.resos.length;

			if (ResosID[nn]=='now' && id!='population_now'){
				w.resos[mm] = new Object();
				w.resos[mm].id = id;
				w.resos[mm].now = val*1;
				w.resos[mm].max = ResosObj[ii].attrib['max']*1;
				w.resos[mm].increase = ResosObj[ii].attrib['increase']*1;
				if (w.resos[mm].now==w.resos[mm].max) {
				    $(id).className='overflow';
				    w.maxtips[ResosObj[ii].attrib['id']]=', <font class="res_overflow_tips">'+SGLang.ResourceOverflow+'</strong>';
				} else if (w.resos[mm].now>=w.resos[mm].max*0.8) {
				    $(id).className='full';
				    w.maxtips[ResosObj[ii].attrib['id']]=', <font class="res_full_tips">'+SGLang.ResourceFull+'</strong>';
				} else {
				    $(id).className='';
				    w.maxtips[ResosObj[ii].attrib['id']]="";
				}
			}

			if (ResosID[nn]=='increase' && id!='population_increase') {
				if (ResosObj[ii].attrib['buff'] && ResosObj[ii].attrib['buff']==1) {
					$(id).parentNode.className='jbspeed';
				} else {
					$(id).parentNode.className='output';
					showspeedbutton = true;
				}
			}

			if ($(id)!=null && id!='lumber_now' && id!='crop_now' && id!='clay_now' && id!='iron_now') $(id).innerHTML=val;
			if (id == 'crop_increase') $(id).className = val<0?'overflow':'';
		}
	}

	MM_showHidden('increase_speedup', showspeedbutton ? "block" : "none");
	removeFunctionHook('MM_ResosTaskLoop');
	addFunctionHook('MM_ResosTaskLoop()');
}

function MM_ResosTaskLoop() {
	var len = w.resos.length;
	var tye = (w.ucal_sdate._mktime+w.ucal_cdate.cmktime-w.ucal_sdate.smktime)/3600;
	for(var ii=0; ii<len; ii++) {
		var id = w.resos[ii].id, val = Math.floor(w.resos[ii].now + tye * w.resos[ii].increase), mye = $(id);
		if (mye==null || typeof(val)!="number" || isNaN(val)) continue;
		mye.innerHTML = Math.max(0, Math.min(w.resos[ii].max,val));
	}
}

function MM_xmlLoaded() {
	var mye = $('xmlLoading');
	w.can_act = 0;
	if (mye) {MM_showHidden('xmlLoading', 'none');MM_showHidden('mask_alpha', 'none');}
	MM_newer.nextstep();
}

function MM_xmlLoading() {
	var mye = $('xmlLoading');
	if (mye) {MM_showHidden('xmlLoading', 'block');/*MM_showHidden('mask_alpha', 'block');*/}
}

function MM_getDataUrl() {
	var act = "", url = "";
	act = (typeof(arguments[0])=="string" && arguments[0]!="") ? ("?act=" + arguments[0]) : "?act=resources.status";

	++w.rd;
	if (isNaN(w.rd)) w.rd = Math.ceil(Math.random()*1000000);
	var nrd = '' + w.rd;
	if (typeof(w.villageid)!="undefined") {
		act = act.replace(/&villageid=[\d]+/gi, '');
		act = act.replace(/&rand=[\d]+/gi, '');
		url = w.dataUrl + act + "&userid=" + w.userid + "&villageid=" + w.villageid + '&' + w.shamKey + '=' + w.shamValues+ "&rand=" + nrd;
		//url = w.dataUrl + act + "&userid=" + w.userid + "&villageid=" + w.villageid + "&rand=" + nrd;
	} else {
		url = w.dataUrl + act  + w.shamKey + '=' + w.shamValues + "&rand=" + nrd;
		//url = w.dataUrl + act + "&rand=" + nrd;
	}

	if (!(arguments[1]) || (url.indexOf("=")!=-1)) {
		var sta = url.indexOf("=");
		var act = url.substring(sta+1).substring(0, url.substring(sta+1).indexOf("&"));
		var udo = "";
		if (act=="build.act" && url.indexOf("&do=")!=-1) {
			var sta2 = url.indexOf("&do=");
			udo = url.substring(sta2+1).substring(3, url.substring(sta2+1).indexOf("&"));
		}
		// 不被记录历史的act
		if (act=='build.quick' || act=='build.upgrade' || act=='build.detailde' || act=='resources.detailup' || act=='store.buygoods' || act=='store.using'
			|| (act=='build.act' && udo!="" && (udo=='study' || udo=='upgrade' || udo=='raise' || udo=='revival' || udo=='submitTrade' || udo=='systemTrade'))
		) {} else {w.uh[w.uh.length]=url.substring(sta+1);}
	}
	return url;
}

function MM_Nav() {
	if (typeof(arguments[0])!="number" || isNaN(arguments[0])) return;
	var tuhh = w.uh.length + arguments[0]-1;
	if (typeof(tuhh)!="number" || isNaN(tuhh) || tuhh<0 && tuhh>=w.uh.length) return;
	w.uh = w.uh.slice(0,(tuhh+1));
	MM_xmlLoading()
	MM_xmlLoad(w.uh[(w.uh.length-1)]);
}

function MM_iframePost() {
	MM_closeDialog();
	if (arguments[0]!=='' && !isNaN(arguments[0])) {try{MM_Nav(parseInt(arguments[0]));return;}catch(e){}}
	if (arguments[0]==='keep') {MM_xmlLoaded();return;}
	if (arguments[0]==='refresh') {arguments[0]=w.canvasact;}
	if (arguments[0].substr(0,11)==='toploaction') {location.href=arguments[0].substr(12)}
	if (can_act == 1 && $('xmlLoading').style.display != 'none') { return;}

	MM_xmlLoading();
	var act="", mye=null, keep='';
	if (typeof(w.citie)=="string" && w.citie=="") return;
	if ((mye=d.getElementById('iframedata'))==null) return;

	if (typeof(arguments[1])=="string" && (arguments[1]=="right" || arguments[1]=="left" || arguments[1]=="all")) keep='&keep=' + arguments[1];

	mye.src = (typeof(arguments[0])=="string" && arguments[0]!="") ? MM_getDataUrl(arguments[0] + keep) : MM_getDataUrl();
	can_act = 0;
}

function MM_xmlLoad(url,keep,data,action) {//{{{ Create xmlhttp object
	try {if (w.timeoutId) {clearTimeout(w.timeoutId);}}catch(e){}
	try {if (w.debug==1) {MM_iframePost(url,keep,data,action);return;}}catch(e){}
	if (arguments[0]!=='' && !isNaN(arguments[0])) {try{MM_Nav(parseInt(arguments[0]));return;}catch(e){}}
	if (arguments[0]==='keep') {MM_xmlLoaded();return;}
	if (arguments[0]==='refresh') {arguments[0]=w.canvasact;}
	if (arguments[0].substr(0,11)==='toploaction') {location.href=arguments[0].substr(12)}
	if (can_act && $('xmlLoading').style.display != 'none')	return;

	var uri = url;
	var xmlhttp=null;
	try {
		xmlhttp = new XMLHttpRequest();
	} catch (e) {
		var a = ['MSXML2.XMLHTTP.5.0', 'MSXML2.XMLHTTP.4.0', 'MSXML2.XMLHTTP.3.0', 'MSXML2.XMLHTTP', 'MICROSOFT.XMLHTTP.1.0', 'MICROSOFT.XMLHTTP.1', 'MICROSOFT.XMLHTTP', 'MSXML2.DOMDocument', 'Microsoft.XmlDom'];
		for (var ii=0; ii<a.length; ii++) {
			try {
				xmlhttp = new ActiveXObject(a[ii]);
				break;
			} catch (e) {}
		}
	}
	if(xmlhttp) {
		MM_xmlLoading();
		url = (typeof(data)!="string" || !data || data=="") ? ((typeof(url)=="string" && url!="") ? MM_getDataUrl(url) : MM_getDataUrl()) : ((typeof(url)=="string" && url!="") ? MM_getDataUrl(url,1) : MM_getDataUrl("",1));

		xmlhttp.onreadystatechange=function() {
			if (xmlhttp.readyState==4 && xmlhttp.status=="200") {
				try{
					var x = null;
					x = xmlhttp.responseXML;
					x = MM_docToobj(x);
					(typeof(action)=="string" && action!="") ? eval(action+'(x)') : MM_xmlAction(x, uri);
				} catch(e) {}
			}
		}
		if (typeof(keep)=="string" && (keep=="right" || keep=="left" || keep=="all")) url += '&keep=' + keep;
		if (typeof(data)!="string" || !data || data=="") {
			xmlhttp.open("GET", url, true);
			data=null;
		} else {
			xmlhttp.open("POST", url, true);
		}
		xmlhttp.setRequestHeader("Content-Type","application/x-www-form-urlencoded; charset=UTF-8");
		xmlhttp.send(data);
	}
}

function MM_defalutLoad(){return;}

function MM_errorLoad(){return;}

function MM_dataLoad() {
	var xmlObj = new Object();
	var xmlDoc = null;
	try{
		if (w.isIE) {
			xmlDoc = iframedata.document.XMLDocument.documentElement;
			xmlObj.xml = "<"+"?xml version=\"1.0\"?>\n" + xmlDoc.xml;
		} else if (w.isNav || w.isOp){
			xmlDoc = iframedata.document.documentElement;
			var oSerializer = new XMLSerializer();
			xmlObj.xml = "<"+"?xml version=\"1.0\"?>\n" + oSerializer.serializeToString(xmlDoc);
		}
		xmlObj = MM_objectToval(xmlObj, xmlDoc, 'xmlObj');
		MM_xmlAction(xmlObj);
	} catch(e){}
}

function MM_docToobj(xmlDoc) {
	var xmlObj = new Object();
	xmlObj.cmd = "";
	var rootElement = xmlDoc.documentElement;
	if (w.isIE) {
		xmlObj.xml = xmlDoc.xml;
	} else if (w.isNav) {
		var oSerializer = new XMLSerializer();
		xmlObj.xml = "<"+"?xml version=\"1.0\"?>\n" + oSerializer.serializeToString(rootElement);
	}
	if (typeof(xmlDoc)!="object" || !rootElement) return false;
	xmlObj = MM_objectToval(xmlObj, rootElement, 'xmlObj');
	return xmlObj;
}

function MM_objectToval(xmlObj, nElement, parentObjName) {
	var xmlObjNodeName = xmlObjNodeText = "";
	xmlObjNodeName += nElement.nodeName + "";
	try{	xmlObjNodeText = nElement.childNodes[0].nodeValue;} catch(e) {xmlObjNodeText="";}
	if(nElement.nodeName=="script" && nElement.childNodes.length>1) {
		for (ii=0; ii<nElement.childNodes.length; ii++) xmlObjNodeText += nElement.childNodes[ii].data;
	}
	xmlObjNodeText = (!xmlObjNodeText || xmlObjNodeText==null || xmlObjNodeText=="") ? "" : xmlObjNodeText.removeEnter();
	var xmlDocNodeAttributes = nElement.attributes;
	var nowObjName = parentObjName + '.' + xmlObjNodeName;
	var n = 0;
	try{
		eval('nowObj = ' + parentObjName + '.' + xmlObjNodeName);
		if (nowObj && nowObj[0]) n = nowObj.length;	else eval(nowObjName + '=[]');
	} catch(e) {
		eval(nowObjName + '=[]');
	}
	nowObjName += '[' + n + ']';
	eval(nowObjName + ' = new Object()');
	eval(nowObjName + '.text=\'' + xmlObjNodeText.removeEnter().replace(/\\/g, "\\\\").replace(/'/g, "\\'") + '\''); // 结束编辑器错误识别的引号："
	eval(nowObjName + '.attrib=[]');
	for (var ii=0; ii<xmlDocNodeAttributes.length; ii++)
	{
		// alert(nowObjName + '.attrib[\''+xmlDocNodeAttributes[ii].nodeName+'\']=\''+xmlDocNodeAttributes[ii].nodeValue+'\'');
		eval(nowObjName + '.attrib[\''+xmlDocNodeAttributes[ii].nodeName+'\']=\''+xmlDocNodeAttributes[ii].nodeValue+'\'');
	}
	var nChildNodes = nElement.childNodes;
	for (var ii=0; ii<nChildNodes.length; ii++) {
		if (nChildNodes[ii].nodeType==1) xmlObj = MM_objectToval(xmlObj, nChildNodes[ii], nowObjName);
	}
	return xmlObj;
}

function MM_showHidden(id,show) {
	var mye=null;
	if (!id || id == "") return;
	if ((mye=$(id)) == null) return;
	mye.style.display = (typeof(show)=="string" && show!="") ? ((show=="none") ? "none" : "block") : ((mye.style.display=="none") ? "block" : "none");
	return;
}

function MM_cleftPage()
{
	var CleftObj=new Object();
	if (typeof(arguments[0])!="object"){return;}else{CleftObj=arguments[0];}
	var CleftObjLen = CleftObj.length;
	for (var ii=0; ii<CleftObjLen; ii++) {
		var xcpSetupLen = _xcp.setup.length;
		xcpSetupLen = 0;
		_xcp.setup[xcpSetupLen] = [CleftObj[ii].attrib['totalnum']*1, CleftObj[ii].attrib['pagenum']*1, CleftObj[ii].attrib['nodeid'], CleftObj[ii].attrib['style'], CleftObj[ii].attrib['windowid']];
	}
	//alert(_xcp.setup);
	var xcpSetupLen = _xcp.setup.length;
	var scpSetupStr = [];
	var scpSetupTmp = [];
	for (var ii=CleftObjLen-1; ii>=0; ii--)
	{
		if (typeof(scpSetupStr[_xcp.setup[ii][2]])!="undefined")
			continue;

		scpSetupStr[_xcp.setup[ii][2]] = 1;
		scpSetupTmp[scpSetupTmp.length] = _xcp.setup[ii];
	}

	_xcp.setup = scpSetupTmp;
	try{XcpAction();}catch(e){}
}

function MM_refreshTimehook(act)
{
	if (act!='' && act!='null' && act!=null) {
		if (act=='ucopytimer') {
			try {
				$('ucopyCanMove').value='1';
				$('ucopy_can_move_0').style.display='none';
				$('ucopy_can_move_1').style.display='';
			} catch(e) {}
		}else if(act == 'viptime'){
			MM_xmlLoad('build.act&do=strengthen&btid=30');
		}else if(act == 'invalidsoldiers'){
			MM_xmlLoad('build.act&do=main&btid=39');
		}else {
			MM_xmlLoad(act+"&newerstep=1", 'all');
			if (act=="index.queueinfo" && (w.canvasact == "resources.status" || w.canvasact == "build.status")) {
				MM_xmlLoad(w.canvasact+"&newerstep=1", "all");
			}
		}
	} else {
		MM_Nav(0);
	}
}

function MM_timeHook() {
	var myes=null;
	if ((myes=document.getElementsByName("timeHook"))==null) return;
	var myel = myes.length;
	for(var ii=0; ii<myel; ii++) {
		var mye = myes[ii];
		tasktime = new ucalDate(mye.getAttribute('sgtitle'));
		var eTime = tasktime.smktime;
		var iTime = eTime - (w.ucal_sdate._mktime + w.ucal_cdate.cmktime);
		refreshact = mye.getAttribute('act');

		if (iTime>0) {
			hTime = Math.floor(iTime/3600);
			mTime = Math.floor((iTime%3600)/60);
			sTime = iTime%60;
			mye.innerHTML = ((hTime<10) ? ("0" + hTime) : hTime) + ":" + ((mTime<10) ? ("0"+mTime) : mTime) + ":" + ((sTime<10) ? ("0"+sTime) : sTime);
		} else if (iTime<0) {
			if (refreshact == 'ucopytimer') {
				MM_refreshTimehook(refreshact);
			}else if(refreshact == 'viptime'){
				MM_refreshTimehook(refreshact);
			}else if(refreshact == 'invalidsoldiers'){
				MM_refreshTimehook(refreshact);
			}else {
				mye.name = '_timeHook';
				mye.innerHTML='<font class="tips_red" onclick="MM_refreshTimehook(\''+refreshact+'\')">00:00:0?</font>';
			}
		} else {
			mye.innerHTML='<font class="tips_red">00:00:00</font>';
			MM_refreshTimehook(refreshact);
			//MM_Nav(0);
		}
	}
}

function MM_iframeLoad2()
{
	var ifm="";
	ifm+='<iframe width="0" height="0" frameborder="1" scrolling="no" src="" id="iframedata" name="iframedata"';
	ifm+=' onload="MM_dataLoad();" target="iframedata" ';
	ifm+=' style="position:absolute; top:0px; left:0px; width:0px; height:0px; display:none; overflow:auto"></iframe>';
	var ifm2 = '<iframe width="0" height="0" frameborder="0" scrolling="no" src="' + w.ifm2url + '" id="iframeassayer" style="position:absolute; top:0px; left:0px; width:0px; height:0px; display:none; overflow:auto"></iframe>';
	try{
		d.getElementsByTagName('body')[0].insertAdjacentHTML("beforeEnd",ifm2);
	}catch(e){
		insertAdjacentHTML2(d.getElementsByTagName('body')[0], ifm2)
	}
	try{
		d.getElementsByTagName('body')[0].insertAdjacentHTML("beforeEnd",ifm);
	}catch(e){
		insertAdjacentHTML2(d.getElementsByTagName('body')[0], ifm)
	}
	MM_iframeLoad();
}

function MM_iframeLoad(){
	var id="",tp="";
	if (typeof(arguments[0])=="string" && arguments[0]!=""){id=arguments[0];}else{id='iframemain';};
	if (typeof(arguments[1])=="string" && arguments[1]!=""){tp=arguments[1];}else{tp='html';};
	MM_iframeLoadHtml(id,tp);
}

function MM_iframeLoadElement(){
	var f=d.createElement('iframe'),id="",tp="";
	if (typeof(arguments[0])=="string" && arguments[0]!=""){id=arguments[0];}else{id='iframemain';};
	if (typeof(arguments[1])=="string" && arguments[1]!=""){tp=arguments[1];}else{tp='html';};f.className="iframes";
	f.width=1224;f.height=200;f.frameboder=1;f.id=id;f.name=id;f.scrolling="yes";f.src=id+'.'+tp;//f.target=id+'t'
	d.getElementsByTagName('body')[0].appendChild(f);
}

function MM_iframeLoadHtml(){
	var f="",id="",tp="";
	if (typeof(arguments[0])=="string" && arguments[0]!=""){id=arguments[0];}else{id='iframemain';};
	if (typeof(arguments[1])=="string" && arguments[1]!=""){tp=arguments[1];}else{tp='html';};
	document.cookie="__utmaen=1; path=/;";
	f+='<iframe width="0" height="0" frameborder="0" scrolling="no" src="'+id+'.'+tp+'" height="0" id="'+id+'" name="'+id+'"';
	if (tp=='xml'){f+=' onload="MM_dataLoad();"';}; // $('iframemain').src="";}; target="'+id+'t"
	f+=' style="position:absolute; top:0px; left:0px; width:0px; height:0px; display:none; overflow:hidden"></iframe>';f='';
	try{
		d.getElementsByTagName('body')[0].insertAdjacentHTML("beforeEnd",f);
	}catch(e){
		insertAdjacentHTML2(d.getElementsByTagName('body')[0], f);
	}
}

function MM_swapImgRestore(){
	var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
}

function MM_preloadImages(){
	if(!d.images) return;if(!d.MM_p) d.MM_p=new Array();var i,j=d.MM_p.length,a=MM_preloadImages.arguments;for(i=0; i<a.length; i++) {if (a[i].indexOf("#")!=0) {	d.MM_p[j]=new Image;d.MM_p[j++].src=a[i];}}
}

function MM_swapImage() {
  var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
   if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
}

function MM_findObj(n, d) { //v4.01
	var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
	if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
	for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
	if(!x && d.getElementById) x=d.getElementById(n); return x;
}

function MM_changeProp(objName,x,theProp,theValue) { //v6.0
	var obj = MM_findObj(objName);
	if (obj && (theProp.indexOf("style.")==-1 || obj.style)) {
		(theValue == true || theValue == false) ? eval("obj."+theProp+"="+theValue) : eval("obj."+theProp+"='"+theValue+"'");
	}
}

function MM_BarShow(nM,nB,nL) {
	var myes=null;
	if ((myes=d.getElementsByName(nM))==null) return;
	var myel = myes.length;
	for(var ii=0; ii<myel; ii++){
		var mye = myes[ii];
		nL ? ((nB==ii) ? "button1_none" : "button1_block") : ((nB==ii) ? "button2_block" : "button2_none");
		var mye2 = null;
		if ((mye2=d.getElementsByName(nM + '_cont')[ii])==null) continue;
		mye2.style.display = (nB==ii) ? "block" : "none" ;
	}
}

function mainFunction() {
	w.currentSeconds++;
	w.ucal_cdate = new ucalDate();
	if ($('debugid')!=null && $('debugid').checked==true) alert(w.hookedFunctions);
	for (var ii = 0; ii<w.hookedFunctions.length; ii++) eval(w.hookedFunctions[ii]);
}

function addFunctionHook(functionName) {
	w.hookedFunctions[w.hookedFunctions.length]=functionName;
}

function delFunctionHook(ii) {
	w.hookedFunctions[ii-1] = ';';
}

function removeFunctionHook(functionName){
	var newHookedFunctions = [];
	var nn=0;
	for(var ii=0; ii<w.hookedFunctions.length; ii++) {
		if (w.hookedFunctions[ii].indexOf(functionName + '(')==-1) {
			newHookedFunctions[nn] = w.hookedFunctions[ii];
			nn++;
		}
	}
	w.hookedFunctions = newHookedFunctions;
}

function taskTimeHook(ii, eTime) {
	var mye = $('task_' + ii);
	if (mye) {
		var iTime = eTime - (w.ucal_sdate._mktime + w.ucal_cdate.cmktime);
		if (iTime>1) {
			var timeHtml = "";
			hTime=Math.floor(iTime/3600);
			mTime=Math.floor((iTime%3600)/60);
			sTime=iTime%60;
			if (hTime>=1) timeHtml += ((hTime<10) ? ("0" + hTime) : hTime) + ":";
			timeHtml += ((mTime<10) ? ("0"+mTime) : mTime) +":";
			timeHtml += (sTime<10) ? ("0"+sTime) : sTime;
			mye.innerHTML=timeHtml;
		} else {
			w.hookedFunctions[ii] = ';';
			mye.innerHTML='<font color="green">00:00</font>';
		}
	}
}

// 关闭左侧二级框
function MM_closeLeft() {
	try{
		MM_showHidden('floatblockleft','none');
		$('floatblockleft').innerHTML = '&nbsp;';
	} catch(e) {}
}

// 关闭右侧二级框
function MM_closeRight() {
	try{
		MM_showHidden('floatblockright','none');
		$('floatblockright').innerHTML = '&nbsp;';
	} catch(e) {}
}

// 关闭小二级框
function MM_closeFloat() {
	try{
		MM_showHidden('floatblockcenter','none');
		//MM_showHidden('mask_bottom','none');
		$('floatblockcenter').innerHTML = '&nbsp;';
	} catch(e) {}
}

// 关闭对话框
function MM_closeDialog() {
	try{
		MM_showHidden('dialog','none');
		if(Sinan.isShow()==false){
			MM_showHidden('mask_top','none');
			MM_showHidden('mask_bottom','none');
		}
	} catch(e) {}
}

// 显示对话框【标题，信息，按钮】
function MM_showDialog(msg,title,button) {
	var obj=$('dialog');
	obj.style['left']="300px";
	obj.innerHTML='';
	var header=d.createElement("h3");
	var content=d.createElement("div");
	var footer=d.createElement("div");
	header.innerHTML=((title && title!='')?title:SGLang.PublicMsg)+'<span class="dialogcloseico" onclick="MM_closeDialog();" onmouseover="this.className=\'dialogcloseico_over\'" onmouseout="this.className=\'dialogcloseico\'" onmousedown="this.className=\'dialogcloseico_down\'">&nbsp;</span>';
	content.className='dialog_content';
	footer.className='dialog_footer';
	var cflag=false;
	if (button && button.length) for (var i in button) {
		try {
			if (button[i].title!=undefined) {
				var _tmpclass=button[i].style?button[i].style:'dialogconfirm';
				var _tmpclass_over=_tmpclass+'_over';
				var _tmpclass_down=_tmpclass+'_down';
				a=d.createElement('span');
				a.className=_tmpclass;
				eval("a.onmouseover=function(){this.className='"+_tmpclass_over+"';}");
				eval("a.onmouseout=a.onmouseup=function(){this.className='"+_tmpclass+"';}");
				eval("a.onmousedown=function(){this.className='"+_tmpclass_down+"';}");
				a.innerHTML=button[i].title;
				a.onclick=button[i].act;
				footer.appendChild(a);
				cflag=true;
			}
		} catch(e) {}
	}
	if (cflag==false) footer.innerHTML='<span class="dialogconfirm" onclick="MM_closeDialog();" onmouseover="this.className=\'dialogconfirm_over\'" onmouseout="this.className=\'dialogconfirm\'" onmouseup="this.className=\'dialogconfirm\'" onmousedown="this.className=\'dialogconfirm_down\'">'+w.lang.dialog_close_button+'</span>';
	content.innerHTML=msg;
	obj.appendChild(header);
	obj.appendChild(content);
	obj.appendChild(footer);
	MM_showHidden('dialog', 'block');
	obj.style['top']=($('wrapper').offsetHeight-obj.offsetHeight)/2+"px";
	MM_showHidden('mask_top','block');
	MM_showHidden('mask_bottom','block');
	header.onmousedown=function(event){
		return ui.drag.start(obj, event, {region:{xl:0,yl:0,xu:obj.parentNode.offsetWidth-obj.offsetWidth,yu:obj.parentNode.offsetHeight-obj.offsetHeight}});
	}
}
// 显示对话框【标题，信息，按钮】,点击关闭依然可以跳到指定的act去
function MM_showDialog_noclose(msg,title,button,close) {
	var obj=$('dialog');
	obj.style['left']="300px";
	obj.innerHTML='';
	var header=d.createElement("h3");
	var content=d.createElement("div");
	var footer=d.createElement("div");
	header.innerHTML=((title && title!='')?title:SGLang.PublicMsg)+'<span class="dialogcloseico" onclick="MM_closeDialog();MM_xmlLoad('+'\''+close+'\''+');" onmouseover="this.className=\'dialogcloseico_over\'" onmouseout="this.className=\'dialogcloseico\'" onmousedown="this.className=\'dialogcloseico_down\'">&nbsp;</span>';
	content.className='dialog_content';
	footer.className='dialog_footer';
	var cflag=false;
	if (button && button.length) for (var i in button) {
		try {
			if (button[i].title!=undefined) {
				var _tmpclass=button[i].style?button[i].style:'dialogconfirm';
				var _tmpclass_over=_tmpclass+'_over';
				var _tmpclass_down=_tmpclass+'_down';
				a=d.createElement('span');
				a.className=_tmpclass;
				eval("a.onmouseover=function(){this.className='"+_tmpclass_over+"';}");
				eval("a.onmouseout=a.onmouseup=function(){this.className='"+_tmpclass+"';}");
				eval("a.onmousedown=function(){this.className='"+_tmpclass_down+"';}");
				a.innerHTML=button[i].title;
				a.onclick=button[i].act;
				footer.appendChild(a);
				cflag=true;
			}
		} catch(e) {}
	}
	if (cflag==false) footer.innerHTML='<span class="dialogconfirm" onclick="MM_closeDialog();" onmouseover="this.className=\'dialogconfirm_over\'" onmouseout="this.className=\'dialogconfirm\'" onmouseup="this.className=\'dialogconfirm\'" onmousedown="this.className=\'dialogconfirm_down\'">'+w.lang.dialog_close_button+'</span>';
	content.innerHTML=msg;
	obj.appendChild(header);
	obj.appendChild(content);
	obj.appendChild(footer);
	MM_showHidden('dialog', 'block');
	obj.style['top']=($('wrapper').offsetHeight-obj.offsetHeight)/2+"px";
	MM_showHidden('mask_top','block');
	MM_showHidden('mask_bottom','block');
	header.onmousedown=function(event){
		return ui.drag.start(obj, event, {region:{xl:0,yl:0,xu:obj.parentNode.offsetWidth-obj.offsetWidth,yu:obj.parentNode.offsetHeight-obj.offsetHeight}});
	}
}
function MM_SupportReport(id,wardata)
{
	var reportUtil = $(id);
	var sReport = "";
	var _timg = ['shu_','wu_','wei_'];
	var _pre;

	for(var i=0; i < wardata.length; i++) {
		sReport += "<table cellspacing=\"1\" class=\"armynumber\">";
		sReport += "<thead>";
		sReport += "<tr>";
		if (wardata[i][7].length == 12) {
			sReport += "<th colspan=\"14\">";
		} else {
			sReport += "<th colspan=\"18\">";
		}

		sReport += "<a href=\"javascript:MM_xmlLoad('user.main&uid="+wardata[i][0]+"');\"><strong class=\"tips_green\">"+wardata[i][1]+"</strong></a><a href=\"javascript:MM_xmlLoad('report.msgwrite&uid="+wardata[i][0]+"')\"><img alt=\"\" sgtitle=\""+SGLang.ReportSendMsg+"\" src=\""+w.img_src+"images/icon/letter.gif\"></a>";//发信
		sReport += SGLang.ReportFrom;///old:grx:来自
		sReport += "<a href=\"javascript:MM_xmlLoad('village.detail&";
		sReport += (wardata[i][2]=="" ? "uitx="+wardata[i][3]+"&uity="+wardata[i][4] : "vid="+wardata[i][2]);
		sReport += "&keep=all');\"><strong class=\"tips_green\">";
		sReport += (wardata[i][5]=="" ? SGLang.ReportOasis : wardata[i][5]);///old:grx:绿洲
		if (!isNaN(wardata[i][3]) && !isNaN(wardata[i][4])) sReport +=  "（"+wardata[i][3]+","+wardata[i][4]+"）";
		sReport += "</strong></a>";
		sReport += "</th>";
		sReport += "</tr>";
		sReport += "</thead>";
		sReport += "<tbody>";
		sReportImg = "<tr><th>"+SGLang.ReportSoldier+"</th>";///old:grx:兵种
		sReportTotal = "<tr><th class=\"tips_red\">"+SGLang.ReportSoldierNum+"</th>";///old:grx:军队数量
		sReportDead = "<tr class=\"losenumber\"><th>"+SGLang.ReportLoss+"</th>";///old:grx:损失
		_pre = _timg[wardata[i][6]];
		for(var j=0;j < wardata[i][7].length; j++) {
			sReportImg += "<td><img src=\""+w.img_src+"images/soldier/"+(j+1==wardata[i][7].length ? "hero" : _pre+(j<12 ? j : j+18))+".gif\" alt=\"\" sgtitle=\""+wardata[i][7][j][2]+"\" /></th>";
			sReportTotal += "<td>"+(isNaN(parseInt(wardata[i][7][j][0]))?0:parseInt(wardata[i][7][j][0]))+"</td>";
			sReportDead += "<td>"+(isNaN(parseInt(wardata[i][7][j][1]))?0:parseInt(wardata[i][7][j][1]))+"</td>";
		}
		sReport += sReportImg+"</tr>";
		sReport += sReportTotal+"</tr>";
		sReport += sReportDead+"</tr>";
		sReport += "</tbody>";
		sReport += "</table>";
	}

	reportUtil.innerHTML = sReport;
}

// 新手指引
var MM_newer = {
	data : [],
	index : 0,
	readystatus : false,
	init : function(d) {
		this.data=d;
		this.readystatus=true;
	},
	nextstep : function() {
		if (this.readystatus==false) return false;
		if (this.data.length<1 || this.index >= this.data.length) {
			this.destroy();
			return false;
		}
		this.readystatus=false;

		$("mask_alpha").style.display = "block";
		var f=$("newer_wizard");
		var o=$("newer_wizard_frame");
		var c=$("newer_wizard_content");
		var l=$("newer_wizard_layer");
		var act = this.data[this.index][4];
		if (act !== 'keep') act = act+'&newerstep='+(this.index+1);
		f.onclick = function() {MM_newer.readystatus=true;MM_xmlLoad(act);};
		$("newer_wizard_nextstep").onclick = function() {MM_newer.readystatus=true;MM_xmlLoad(act);};

		this.clearflashborder();
		if (this.data[this.index][0]!=null && this.data[this.index][1]!=null && this.data[this.index][2]!=null && this.data[this.index][3]!=null) {
			f.style.left = this.data[this.index][0] + "px";
			f.style.top = this.data[this.index][1] + "px";
			f.style.width = (parseInt(this.data[this.index][2]) + 10) + "px";
			f.style.height = (parseInt(this.data[this.index][3]) + 10) + "px";
			o.style.width = this.data[this.index][2] + "px";
			o.style.height = this.data[this.index][3] + "px";
			f.style.display = "block";
			$('newer_wizard_arrowup').style.display = (this.data[this.index][7] == 1) ? "block" : "none";
			$('newer_wizard_arrowdown').style.display = (this.data[this.index][7] == 2) ? "block" : "none";
			$('newer_wizard_arrowup').style.left = ((parseInt(this.data[this.index][2])-$('newer_wizard_arrowup').offsetWidth)/2+5) + "px";
			$('newer_wizard_arrowdown').style.left = ((parseInt(this.data[this.index][2])-$('newer_wizard_arrowdown').offsetWidth)/2+5) + "px";
			this.flashborder(c,500);
		}

		l.style.left = this.data[this.index][5] + "px";
		l.style.top = this.data[this.index][6] + "px";
		$('newer_wizard_text').innerHTML = "<p>"+this.data[this.index][8]+"</p>";
		l.style.display = "block";

		this.index += 1;
	},
	destroy : function() {
		this.clearflashborder();
		$("newer_wizard").style.display="none";
		$("newer_wizard_layer").style.display="none";
		$("mask_alpha").style.display="none";
		this.data=[];
		this.index=0;
		this.readystatus=false;
	},
	flashborder : function(obj, sec) {
		if (obj.style.display == "none") obj.style.display = "block";
		else obj.style.display = "none";
		MM_newer.fbhandle = setTimeout(function(){MM_newer.flashborder(obj,sec);}, sec);
	},
	clearflashborder : function() {
		if (this.fbhandle) clearTimeout(this.fbhandle);
	}
};

// UI 通用函数
var ui = {
	// 下拉框
	downmenu : function (obj, mid) {
		var menu = $(mid);
		var _out = function() {
			try {ui.dmtimeout = setTimeout(function(){menu.style.display='none';},300);} catch(e) {}
		}
		var _over = function() {
			try {clearTimeout(ui.dmtimeout);} catch(e) {}
		}
		menu.style.display = menu.style.display=='block'?'none':'block';
		obj.onmouseout = _out;
		menu.onmouseout = _out;
		obj.onmouseover = _over;
		menu.onmouseover = _over;
	},

	// 通用滑动门
	commondoor : function(id,ele,tagname,style) {
		try {
			if (!style) style={now:'now',normal:'normal'};
			var slideObj = $(id).getElementsByTagName("li");
			for (var j = 0; j < slideObj.length ; j++)
			{
				var slideObjclass = slideObj[j].className;
				if (slideObjclass == style.now || slideObjclass == style.normal)
				{
					slideObj[j].getElementsByTagName(tagname)[0].style.display = "none"
					slideObj[j].className = style.normal;
				}
			}
			ele.parentNode.getElementsByTagName(tagname)[0].style.display = "block";
			ele.parentNode.className=style.now;
		} catch(e){}
		return false;
	},

	// 取得元素的真实css属性
	getStyle : function (o,a){
		var r=false;
		try {r=(o.currentStyle)?o.currentStyle[a]:d.defaultView.getComputedStyle(o, null)[a];}catch(e){}
		return r;
	},

	// 显示Tips
	describe : function() {
		var obj = arguments[0],msg = arguments[1];
		if (obj == null || msg == null) return false;
		var id = arguments[2] || 'describe';
		var dobj = $(id);
		if (!dobj) {
			dobj = d.createElement('div');
			dobj.id = id;
			d.body.appendChild(dobj);
		}
		dobj.innerHTML = msg;
		dobj.style.position = 'absolute';
		dobj.style.left = '0px';
		dobj.style.top = '-1000px';
		dobj.style.display = 'block';
		dobj.style.zIndex = '9200';
		var _mousemove = function(event) {
			var mouse = ui.getMouseLocation(event);
			dobj.style['left'] = Math.min($('wrapper').offsetWidth+$('wrapper').offsetLeft-dobj.offsetWidth,mouse.x - 5) + 'px';
			dobj.style['top'] = (((dobj.offsetHeight + mouse.y + 20) > $('wrapper').offsetHeight) ? (mouse.y - 5 - dobj.offsetHeight) : (mouse.y + 20)) + 'px';
			return false;
		};

		var _mouseout = function(){
			dobj.style.display = 'none';
			if (window.removeEventListener) {
				obj.removeEventListener('mousemove', _mousemove, false);
				obj.removeEventListener('mouseout', _mouseout, false);
				dobj.removeEventListener('mouseover', _mouseout, false);
			} else if (document.detachEvent) {
				obj.detachEvent('onmousemove', _mousemove);
				obj.detachEvent('onmouseout', _mouseout);
				dobj.detachEvent('onmouseover', _mouseout);
			} else {
				obj.onmousemove = null;
				obj.onmouseout = null;
				dobj.onmouseover = null;
			}
			return false;
		};

		if (window.addEventListener) {
			obj.addEventListener('mousemove', _mousemove, false);
			obj.addEventListener('mouseout', _mouseout, false);
			dobj.addEventListener('mouseover', _mouseout, false);
		} else if (document.attachEvent) {
			obj.attachEvent('onmousemove', _mousemove);
			obj.attachEvent('onmouseout', _mouseout);
			dobj.attachEvent('onmouseover', _mouseout);
		} else {
			obj.onmousemove = _mousemove;
			obj.onmouseout = _mouseout;
			dobj.onmouseover = _mouseout;
		}

		return false;
	},

	// 全部选中[checkbox]
	select_all_checkbox : function() {
		if(arguments.length>0) {
			var checkboxs = document.forms[arguments[0]].elements;
		} else {
			var checkboxs = document.getElementsByTagName("input");
		}
		for(i=0;i<checkboxs.length;i++) {
			if(checkboxs[i].type.toLowerCase() == "checkbox") checkboxs[i].checked=true;
		}
	},

	// 取消全选[checkbox]
	unselect_all_checkbox : function() {
		if(arguments.length>0) {
			var checkboxs = document.forms[arguments[0]].elements;
		} else {
			var checkboxs = document.getElementsByTagName("input");
		}
		for(i=0;i<checkboxs.length;i++) {
			if(checkboxs[i].type.toLowerCase() == "checkbox") checkboxs[i].checked=false;
		}
	},

	// 拖拽
	drag : {
		start : function(obj, e, args) {
			var m=ui.getMouseLocation(e);
			ui.drag.obj = obj;ui.drag.args = args;ui.drag.xm = m.x;ui.drag.ym = m.y;ui.drag.xo = obj.offsetLeft;ui.drag.yo = obj.offsetTop;	ui.drag.yo = obj.offsetTop;
			if (window.addEventListener) {
				window.addEventListener('mousemove', ui.drag.move, false);
				window.addEventListener('mouseup', ui.drag.stop, false);
			} else if (document.attachEvent) {
				document.attachEvent('onmousemove', ui.drag.move);
				document.attachEvent('onmouseup', ui.drag.stop);
			} else {
				window.onmousemove = ui.drag.move;
				window.onmouseup = ui.drag.stop;
			}
			return false;
		},

		stop : function(e) {
			ui.drag.obj = ui.drag.xm = ui.drag.ym = ui.drag.xo = ui.drag.yo = undefined;
			if (window.removeEventListener) {
				window.removeEventListener('mousemove', ui.drag.move, false);
				window.removeEventListener('mouseup', ui.drag.stop, false);
			} else if (document.detachEvent) {
				document.detachEvent('onmousemove', ui.drag.move);
				document.detachEvent('onmouseup', ui.drag.stop);
			} else {
				window.onmousemove = null;
				window.onmouseup = null;
			}
			if (ui.drag.args != undefined && ui.drag.args.stopfunc) ui.drag.args.stopfunc(e);
			return false;
		},

		move : function(e) {
			var m=ui.getMouseLocation(e);
			var l=ui.drag.xo+m.x-ui.drag.xm;
			var t=ui.drag.yo+m.y-ui.drag.ym;
			if (ui.drag.args&&ui.drag.args.region) {
				l=Math.max(ui.drag.args.region.xl, Math.min(ui.drag.args.region.xu, l));
				t=Math.max(ui.drag.args.region.yl, Math.min(ui.drag.args.region.yu, t));
			}
			ui.drag.obj.style['left'] = l + "px";
			ui.drag.obj.style['top'] = t + "px";
			return false;
		}
	},

	// 取得鼠标相对窗口的坐标
	getMouseLocation : function(){
		e = arguments[0];
		if (!d.all) {
			mouseX = e.pageX;
			mouseY = e.pageY;
		} else {
			mouseX = event.clientX + d.documentElement.scrollLeft;
			mouseY = event.clientY + d.documentElement.scrollTop;
		}
		return {x:mouseX,y:mouseY};
	},

	// 添加到收藏夹
	addFavor : function(sURL, sTitle) {
		sURL = sURL || location.href;
		sTitle = sTitle || document.title;
		try {window.external.addFavorite(sURL, sTitle);} catch (e) {
			try {window.sidebar.addPanel(sTitle, sURL, "");} catch (e) {}
		}
	},

	// 跑马灯
	marqueeApp : {
		stopsec : 6,
		init : function(data,id,start) {
			if (data.length<1 || !id) return false;
			start=start||0;
			var oDiv = $(id);
			var i=start;
			var text=data[i];
			var c=document.createElement("div");
			c.style.marginLeft="0";
			c.style.width=oDiv.offsetWidth+"px";
			c.style.overflow="hidden";
			c.id="marquee_content";
			c.innerHTML=text;
			oDiv.appendChild(c);
			setTimeout(function(){ui.marqueeApp.scroll(data, i, oDiv, c, -5)}, ui.marqueeApp.stopsec * 1000);
		},
		scroll : function(data, nowi, oDiv, c, s) {
			// args:数据数组,数据下标,跑马灯容器,跑马灯内容,位移
			var i=nowi,ns=s-5,iv=100;
			if (s>0) ns=Math.max(0,ns);
			else if (s==0) iv=ui.marqueeApp.stopsec*1000;
			else if (s<=-oDiv.offsetWidth) {
				i=(!data[i+1])?0:i+1;
				c.innerHTML=data[i];
				ns=oDiv.offsetWidth;
			}
			c.style.marginLeft=s+"px";
			setTimeout(function(){ui.marqueeApp.scroll(data, i, oDiv, c, ns);}, iv);
		}
	}
};

// 页面载入后需要执行的
var gameload = {
    // IE 取消焦点对象的虚线边框
    nofocus : function() {
        if(document.attachEvent) document.attachEvent("onclick", function(){try{if (window.event.srcElement.tagName=='A'||window.event.srcElement.tagName=='AREA') window.event.srcElement.blur();}catch(e){}});
    },

    // 初始化sgtitle属性效果
    initsgtitle : function() {
        if (window.addEventListener) {
            window.addEventListener('mouseover', function(event) {var t=event.target.getAttribute('sgtitle'), t1=event.target.getAttribute('sgtips'); if(t) ui.describe(event.target,t); else if(t1) ui.describe(event.target,t1,'describe_y');}, false);
        } else if (document.attachEvent) {
            document.attachEvent("onmouseover", function(){try{var t=window.event.srcElement.getAttribute('sgtitle'),t1=window.event.srcElement.getAttribute('sgtips');if(t) ui.describe(window.event.srcElement,t);else if(t1) ui.describe(window.event.srcElement,t1,'describe_y');}catch(e){}});
        }
    },

    // 聊天窗口初始化
    initchat : function() {
        var o = $('nav_sc');
        if (!o) return false;
        if (o.addEventListener) {
            o.addEventListener('mouseover', function(){w.nav_sc_status=1;}, false);
            o.addEventListener('mouseout', function(){w.nav_sc_status=0;}, false);
            o.addEventListener('click', function(){if(!w.nav_sc_status) setTimeout(function(){var vimg=new Image();vimg.src="/index.php?act=validate.image&num=4&rand="+(++w.rd);}, 4000);}, false);
        } else if (o.attachEvent) {
            o.attachEvent('onmouseover', function(){w.nav_sc_status=1;});
            o.attachEvent('onmouseout', function(){w.nav_sc_status=0;});
            o.attachEvent('onclick', function(){if(!w.nav_sc_status) setTimeout(function(){var vimg=new Image();vimg.src="/index.php?act=validate.image&num=4&rand="+(++w.rd);}, 4000);});
        }
    }
};

// 地图
var map = {
	MouseX : 0,
	MouseY : 0,
	OffsetX : 0,
	OffsetY : 0,
	MouseTag : 0,
	Move_Obj : null,
	mouseupact : null,
	moveStopCallBack : {
		stopfunc : function(event){
			var MouseXY = ui.getMouseLocation(event);
			if (Math.abs(parseInt(MouseXY.x) - map.MouseX) < 52 && Math.abs(parseInt(MouseXY.y) - map.MouseY) < 26){
				map.Move_Obj.style.left = map.OffsetX + "px";
				map.Move_Obj.style.top = map.OffsetY + "px";
			}else{
				var centerX = $("map_center_x").value;
				var centerY = $("map_center_y").value;
				var pxOffsetX = parseInt(MouseXY.x) - parseInt(map.MouseX);
				var pxOffsetY = parseInt(MouseXY.y) - parseInt(map.MouseY);
				var mapOffsetXY = map.pxOffsetToMapOffset(pxOffsetX, pxOffsetY);
				centerX = parseInt(centerX) - parseInt(mapOffsetXY.x);
				centerY = parseInt(centerY) - parseInt(mapOffsetXY.y);
				try{clearTimeout(map.mouseupact);}catch(e){};
				MM_xmlLoad('map.status&uitx='+centerX+'&uity='+centerY+'&focus=1', 'right');
				return false;
			}
		}
	},

	frameMousedown : function(event, frame_obj, move_obj){

		var MouseXY = ui.getMouseLocation(event);
		this.MouseX = parseInt(MouseXY.x);
		this.MouseY = parseInt(MouseXY.y);
		this.OffsetX = parseInt(move_obj.offsetLeft);
		this.OffsetY = parseInt(move_obj.offsetTop);
		this.Move_Obj = move_obj;

		ui.drag.start(move_obj, event, map.moveStopCallBack);
		return false;
	},

	frameMouseUp :function(event, frame_obj, move_obj){
		var MouseXY = ui.getMouseLocation(event);
		var MoveOffsetXY = map.cumulativeOffset(move_obj);
		var CX = parseInt(MouseXY.x) - parseInt(MoveOffsetXY.left);
		var CY = parseInt(MouseXY.y) - parseInt(MoveOffsetXY.top);
		var MapXY = map.pxOffsetToMapOffset(CX, CY);
		var DivXY = map.mapOffsetToMapDivID(MapXY.x, MapXY.y);
		var tab_obj = $("map_div_tag");
		tab_obj.style.top = $("city_div_"+DivXY).style.top;
		tab_obj.style.left = $("city_div_"+DivXY).style.left;
		tab_obj.style.display = "block";

		var focusXY = $("city_div_"+DivXY).innerHTML;
		var focusArray = focusXY.split(',');
		var focusX = focusArray[0];
		var focusY = focusArray[1];
		$('map_focus_x').value = focusX;
		$('map_focus_y').value = focusY;
		Radar.setRadarFocus(focusX, focusY, false);

		map.mouseupact=setTimeout(function(){MM_xmlLoad('map.detail&uitx='+focusX+'&uity='+focusY);},1);

		return false;
	},

	frameMouseout : function()
	{
		var map_tips = $('map_tips');
		if (!map_tips)	return false;
		map_tips.style.display = "none";
	},

	frameMouseover : function(event, move_obj, xyindex)
	{

		var MouseXY = ui.getMouseLocation(event);
		var dobj = $('map_tips');
		if (!dobj) {
			dobj = d.createElement('div');
			dobj.id = 'map_tips';
			d.body.appendChild(dobj);
		}

		if (typeof xyindex=='number') {
			var DivXY = xyindex;
		} else {
			var MoveXY = map.cumulativeOffset(move_obj);
			var CX = parseInt(MouseXY.x) - parseInt(MoveXY.left);
			var CY = parseInt(MouseXY.y) - parseInt(MoveXY.top);
			var MapXY = map.pxOffsetToMapOffset(CX, CY);
			var DivXY = map.mapOffsetToMapDivID(MapXY.x, MapXY.y);
		}

		try {
			var focusXYValue = $("city_div_"+DivXY).attributes['mapinfo'].nodeValue;
		} catch(e) {
			return false;
		}
		var focusArray = focusXYValue.split('#');
		var unitx = focusArray[0] || '-';
		var unity = focusArray[1] || '-';
		var state = focusArray[2] || '-';
		var user_name = focusArray[3] || '--';
		var village_name = focusArray[4] || '--';
		var alliance_name = focusArray[5] || '--';
		var village_popu = focusArray[6] || '--';
		var unit_type = focusArray[7] || '0';
		if (unitx == '-' || unity == '-')	return false;
		//dobj.innerHTML = "X:"+unitx+" Y:"+unity + "<br/>州:"+state + "<br/>君主名:" + user_name + "<br/>城镇名:" + village_name + "<br/>联盟名:" + alliance_name + "<br/>人口:" + village_popu;
		var other_str = '';
		if (unit_type == '23'){
			other_str = '<li>'+SGLang.MapType+'：<span class="dark_content">'+SGLang.MapMountainvillage+'</span></li>';
		}
		tmp_html = '<ul>'+ other_str;
		if(unit_type==33)tmp_html += '<li><span class="dark_content">'+village_name+'</span></li>';	//军营
		tmp_html += '<li>'+SGLang.MapUnit+'：<span class="dark_content">'+unitx+':'+unity+'【'+state+'】</span></li>'+
			'<li>'+SGLang.MapUser+'：<span class="dark_escalate">'+user_name+'</span></li>'+
			'<li>'+SGLang.MapVillage+'：<span class="dark_content">'+village_name+'</span></li>'+
			'<li>'+SGLang.MapPopu+'：<span class="dark_content">'+village_popu+'</span></li>'+
			'<li>'+SGLang.MapAlliance+'：<span class="tips_union">'+alliance_name+'</span></li>'+
			'</ul>';///old:grx:坐　标   君　主   城镇名   居　民   联　盟
		dobj.innerHTML = tmp_html;
		dobj.style.display = 'block';
		var mouse = ui.getMouseLocation(event);
		dobj.style['left'] = mouse.x + 'px';
		dobj.style['top'] = (((dobj.offsetHeight + mouse.y + 23) > $('wrapper').offsetHeight) ? (mouse.y - 5 - dobj.offsetHeight) : (mouse.y + 23)) + 'px';
		return false;

	},

	pxOffsetToMapOffset : function(pxOffsetX, pxOffsetY)
	{
		var x = Math.floor((pxOffsetX + 2 * pxOffsetY) / 104 + 0.5);
		var y = Math.floor((pxOffsetX - 2 * pxOffsetY) / 104 + 0.5);
		return {x : x, y: y};
	},

	mapOffsetToMapDivID : function(offsetX, offsetY)
	{
		var divY = offsetX - offsetY;
		var divX = offsetX - Math.floor(divY / 2);
		if (divY%2 == 1){
			return divY*9 + divX;
		}else{
			return divY*9 + divX;
		}
	},

	cumulativeOffset : function(element) {
		var valueT = 0, valueL = 0;
		do {
		  valueT += (element.offsetTop  || 0)-element.scrollTop;
		  valueL += (element.offsetLeft || 0)-element.scrollLeft;
		  element = element.offsetParent;
		} while (element);
		return {left:valueL, top:valueT};
	},

	edgeSelect:function(p) {
		try {
			if (w.map_edge) w.map_edge.style.display='none';
			w.map_edge = $('edge_'+p);
			w.map_edge.style.display='block';
			if (p!=24&&p!=133) {
			    $('edge_24').style.display='none';
			    $('edge_133').style.display='none';
			}
		} catch(e) {}
	},

	openVipmap:function() {
	    $('vipmap').innerHTML ? $('vipmap').style.display = 'block' : MM_xmlLoad('map.vipstatus');
	},

	closeVipmap:function() {
	    $('vipmap').style.display = "none";
	}

};

// 雷达
var Radar = {
	init : function() {
			$('div_radar').style.display = "none";
			$('div_radar').innerHTML = AC_FL_RunContent(
				'codebase', 'http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0',
				'width', '189',
				'height', '189',
				'src',  (w.radarsrc||'radar')+(w.isNav?'':'?r='+w.rd),
				'quality', 'high',
				'pluginspage', 'http://www.macromedia.com/go/getflashplayer',
				'align', 'left',
				'play', 'true',
				'loop', 'false',
				'scale', 'showall',
				'wmode', 'transparent',
				'devicefont', 'false',
				'id', 'flashRadar',
				'bgcolor', '#f1f1ed',
				'name', 'flashRadar',
				'menu', 'false',
				'allowFullScreen', 'false',
				'allowScriptAccess','always',
				'movie', (w.radarsrc||'radar')+(w.isNav?'':'?r='+w.rd),
				'salign', ''
			);
	},
	showMapBook : function() {
		MM_xmlLoad('map.bookmark');
	},
	setRadarFocus : function(x, y, delay) {
		if($('div_radar').innerHTML.trim() == "") {
			this.init();
		} else {
			try {
				var flashObj = getFlashObject('flashRadar');
				if(delay) {
					setTimeout(function(){try{flashObj.setFocusTile(x, y);}catch(e){}},1);
				} else {
					flashObj.setFocusTile(x, y);
				}
			} catch(e) {}
		}
		$('div_radar').style.display = "block";
		//if (w.userhabit.radarshow);this.showRadar();setTimeout(function(){try{var flashObj = getFlashObject('flashRadar');flashObj.unfold(true);flashObj.showMapInfo();}catch(e){alert(e.message)}},1000);
	},
	getMapInfo : function() {
	 	try{
			var centerX = $('map_center_x').value;
			var centerY = $('map_center_y').value;
			var focusX = $('map_focus_x').value;
			var focusY = $('map_focus_y').value;
			//alert('js:'+centerX + "|" + centerY + "|" + focusX + "|" + focusY);
			return centerX + "|" + centerY + "|" + focusX + "|" + focusY;
		}catch(e){};
	},
	getUserInfo : function () {
		return w.userid;
	},
	setMapCenter : function() {
		var x = arguments[0];
		var y = arguments[1];
		MM_xmlLoad('map.status&uitx='+x+'&uity='+y+'&focus=1');
	},
	mapShowDefault : function() {
		MM_xmlLoad('map.status');
	},
	hideRadar: function() {
		var done = arguments[0];
		if(done == 1) $('div_radar').style.height = "28px";
	},
	showRadar : function() {
		var done = arguments[0];
		$('div_radar').style.height = "189px";
		if(done == 1){}
	}
}

Ajax = function()
{
	var xmlhttp;
	var failed;
	var method;
	var responseStatus;
	var url;

	this.xmlhttp = null;
	this.failed = true;
	this.method = 'GET';
	this.createAJAX();
}

Ajax.prototype.createAJAX = function() {
	try {
		this.xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
	} catch (e1) {
		try {
			this.xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
		} catch (e2) {
			this.xmlhttp = null;
		}
	}

	if (! this.xmlhttp) {
		if (typeof XMLHttpRequest != "undefined") {
			this.xmlhttp = new XMLHttpRequest();
		} else {
			this.failed = true;
		}
	}
};

Ajax.prototype.runAJAX = function(urlstring, querystring, method, async) {
    var self = this;
	this.url = urlstring;
	this.query = querystring;

	if(method.toLowerCase() == 'get')
	{
		this.xmlhttp.open("GET", this.url, async);
		this.xmlhttp.send(null);
	}
	else if(method.toLowerCase() == 'post')
	{
	    this.xmlhttp.open("POST", this.query, async);
	    this.xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded;");
	    this.xmlhttp.send(this.query);
	}

	this.xmlhttp.onreadystatechange = function() {
		if(self.xmlhttp.readyState == 4 && self.xmlhttp.status==200) {
			if (self.callback) self.callback(self.xmlhttp.responseText);
		}
	};

	if (!async)
	{
		if(self.xmlhttp.readyState == 4 && self.xmlhttp.status==200) {
			if (self.callback) self.callback(self.xmlhttp.responseText);
		}
	}
};
var TurnBox = {
	k : 1,
	i : 0,
	j : 1,
	l : 0,
	h : 0,
	vUrl:null,
	vAct:0,
	msgShow:null,
	vShow:null,
	vMark:0,
	vIsCon:0,
	init: function(isc,su){
		this.vUrl = su;
		if(this.isBoxSendMsg()){
			this.vUrl+= "&sendmsg=1";
		}
		this.vIsCon = isc;
	},
	play : function()
	{
		this.msgShow = 	$('playbox') || document["playbox"];
		var vAjax = new Ajax();
		var url = './index.php?act=treasurebox.usedbox&'+TurnBox.vUrl;
		vAjax.callback = function(result)
		{
			var json = eval( '('+result+')' );
			var aId = json.id;
			if (aId == '-1')
			{
				TurnBox.msgShow.innerHTML = json.data;
				TurnBox.vMark = 0
				return false;
			} else if (aId == "0") {
				TurnBox.msgShow.innerHTML = SGLang.ItemHaveno;///old:grx:您没有该道具或该道具已使用
				TurnBox.vMark = 0
				return false;
			} else if (aId < -1 ) {
				TurnBox.msgShow.innerHTML = SGLang.PageError;///old:grx:系统错误!请关闭该页面后再次使用
				TurnBox.vMark = 0
				return false;
			} else if (aId > 0 ) {
				if (aId <= 22){
					TurnBox.h = json.id;
				}else{
					TurnBox.h = 11;
				}
				TurnBox.l = json.id;
				TurnBox.msgShow.innerHTML = SGLang.GetPrizeIng;///old:grx:正在抽奖,请稍等...
				TurnBox.vShow = json.data;
				TurnBox.vMark = 1;
			}
		};
		vAjax.runAJAX(url, '', 'get', false);
		if (TurnBox.vMark == 1){
			this.k = 1;
			this.j = 1;
			this.i = 0;
			this.toplay();
		}
	},
	setSendMsg : function()
	{
		if($('boxSendMsgFlg') && $('boxSendMsgFlg').checked == true){
			setCookie('box_showmsg', '1');
		}else{
			setCookie('box_showmsg', '0');
		}
	},
	setSendMsgCheckbox :function()
	{
		if($('boxSendMsgFlg')){
			if(this.isBoxSendMsg()) $('boxSendMsgFlg').checked = true;
			else $('boxSendMsgFlg').checked = false;
		}
	},
	isBoxSendMsg : function()
	{
		if(getCookie('box_showmsg') == '1') return true;
		return false;
	},
	toplay : function()
	{
		this.k = this.k +1;
		for(x=1;x<=22;x++){
			$("f"+x).className="box_goods_"+x;
		}
		//alert("J:"+this.j+"K:"+this.k+"L:"+this.l+"H:"+this.h+"I:"+this.i);
		$("f"+this.j).className="box_goods_"+this.j+" nowgoods";
		this.j = this.j +1;
		if (this.j>22) this.j = 1;

		if (this.k <= (this.l - this.h)){//均速转
			setTimeout(function(){TurnBox.toplay();}, 20);
		}else{//慢速转
			this.i=this.i+1;
			if (this.i>this.h){//中奖
				if (this.j == 1){
					this.vAct = 22;
				}else{
					this.vAct = this.j - 1;
				}
				$("f"+this.vAct).className="box_goods_"+ this.vAct +" nowgoods";
				setTimeout(function(){
					TurnBox.msgShow.innerHTML = TurnBox.vShow;
					if (TurnBox.vIsCon== 1){
						setTimeout(function(){MM_xmlLoad('store.iteminfo&'+TurnBox.vUrl);}, 1000);
					}
				}, 1000);
				return true;
			}
			setTimeout(function(){TurnBox.toplay();}, 20*this.i);
		}
	}
};

var Sinan = {
	maxRound:2,
	slowStepNum:10,
	maxGoodsNum:16,

	isCon:false,
	act:'',
	pos:-1,
	startPos:0,
	nowRound:0,
	isPlaying:false,
	canPlay:true,
	goodsMsg:'',
	stepNumLeft:0,
	init: function(isCon,act){
		this.isCon = isCon;
		this.act = act;
		this.pos = -1;
		this.startPos = 0;
		this.nowRound = 0;
		this.isPlaying = false;
		this.canPlay = true;
		this.goodsMsg = '';
		this.stepNumLeft = 0;
	},
	setShow:function(type)
	{
		if(type=='block'){
			MM_showHidden('sinan','block');
			MM_showHidden('mask_top','block');
			MM_showHidden('mask_bottom','block');
		}else{
			if(!this.isPlaying) this.stop();
		}
	},
	play : function()
	{
		if(this.canPlay == false || this.isPlaying == true) return;
		if(this.isSendMsg()){
			this.act += '&sendmsg=1';
		}
		MM_xmlLoad('sinan.useSinan'+this.act);
	},
	dealResult : function(result)
	{
		var json = eval( '('+result+')' );
		if(json.status == 0){
			alertDialog('keep', json.msg);
			return;
		}else{
			if(this.getSinanNum() > 0){
				this.minusSinanNum();
				this.pos = json.pos;
				this.goodsMsg = json.msg;
				this.isPlaying = true;
				$('sinanMsg').innerHTML = SGLang.SinanPlaying;
				$('sinanCtl').style.display = 'none';
				$('sinanMsg').style.display = 'block';
				this.stepNumLeft = this.maxGoodsNum*this.maxRound + this.pos - this.startPos;
				this.setShow('block');
				this.toplay();
			}
		}
	},
	toplay : function()
	{
		if(!this.canPlay) return;
		this.setShow('block');
		for(var x=0;x<=this.maxGoodsNum-1;x++){
			$("sinanPos"+x).className="pos"+x;
		}
		$("sinanPos"+this.startPos).className="pos"+this.startPos+" bord3";
		if(this.nowRound == this.maxRound && this.startPos == this.pos){
			this.isPlaying = false;
			$('sinanMsg').innerHTML = this.goodsMsg;
			if(this.isCon){
				$('sinanMsg').innerHTML += '<br/>'+SGLang.SinanContinueTips.replace(/\[NUM\]/g,'<span id="sinanSecCount">5</span>');
				$('sinanMsg').innerHTML += '<br/>'+'<a href="javascript:void(0);" onclick="javascript:Sinan.stop();" class="button123_r btn_pos1">'+SGLang.PubStop+'</a>';
				setTimeout(function(){Sinan.secCount();}, 1000);
			}else{
				$('sinanMsg').innerHTML += '<br/><a href="javascript:void(0);" onclick="javascript:MM_xmlLoad(\'sinan.showSinan&iscon=0\');" class="button123_r  btn_pos1">'+SGLang.PubContinue+'</a>';
			}
			$('sinanCtl').style.display = 'none';
			$('sinanMsg').style.display = 'block';
			return;
		}else{
			var slowStepCount = this.slowStepNum-this.stepNumLeft+1;
			if(slowStepCount<0) slowStepCount = 0;
			this.startPos++;
			if(this.stepNumLeft>0) this.stepNumLeft--;
			if(this.startPos > this.maxGoodsNum-1){
				this.nowRound ++;
				this.startPos = 0;
			}
			if (slowStepCount >= 0){//慢速转
				setTimeout(function(){Sinan.toplay();}, 60+slowStepCount*40);
			}else{//匀速转
				setTimeout(function(){Sinan.toplay();}, 60);
			}
		}
	},
	secCount : function()
	{
		if($('sinanSecCount')){
			var sec = $('sinanSecCount').innerHTML.trim();
			sec = parseInt(sec);
			if(isNaN(sec)) sec = 0;
			if(sec >= 1) sec--;
			if(sec > 0){
				$('sinanSecCount').innerHTML = sec;
				setTimeout(function(){Sinan.secCount();}, 1000);
			}else{
				MM_xmlLoad('sinan.showSinan&iscon='+(this.isCon?1:0));
			}
		}
	},
	minusSinanNum : function()
	{
		if($('sinanItemNum')){
			var num = $('sinanItemNum').innerHTML.trim();
			num = parseInt(num);
			if(isNaN(num)) num = 0;
			num--;
			if(num < 0) num = 0;
			$('sinanItemNum').innerHTML = num;
		}
	},
	getSinanNum : function()
	{
		var num = 0;
		if($('sinanItemNum')){
			var num = $('sinanItemNum').innerHTML.trim();
			num = parseInt(num);
			if(isNaN(num)) num = 0;
		}
		return num;
	},
	stop : function()
	{
		this.canPlay = false;
		MM_showHidden('sinan','none');
		MM_showHidden('mask_top','none');
		MM_showHidden('mask_bottom','none');
		$('sinan').innerHTML = '';
		return;
	},
	setSendMsg : function()
	{
		if($('sinanSendMsgFlg') && $('sinanSendMsgFlg').checked == true){
			setCookie('sinan_showmsg', '1');
		}else{
			setCookie('sinan_showmsg', '0');
		}
	},
	setSendMsgCheckbox :function()
	{
		if($('sinanSendMsgFlg')){
			if(this.isSendMsg()) $('sinanSendMsgFlg').checked = true;
			else $('sinanSendMsgFlg').checked = false;
		}
	},
	isSendMsg : function()
	{
		if(getCookie('sinan_showmsg') == '1') return true;
		return false;
	},
	isShow:function()
	{
		if($('sinan') && $('sinan').style.display == 'block') return true;
		return false;
	}
};

// AC_RunActiveContent
function ControlVersion()
{
	var version;
	var axo;
	var e;

	// NOTE : new ActiveXObject(strFoo) throws an exception if strFoo isn't in the registry

	try {
		// version will be set for 7.X or greater players
		axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.7");
		version = axo.GetVariable("$version");
	} catch (e) {}

	if (!version) {
		try {
			// version will be set for 6.X players only
			axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.6");

			// installed player is some revision of 6.0
			// GetVariable("$version") crashes for versions 6.0.22 through 6.0.29,
			// so we have to be careful.

			// default to the first public version
			version = "WIN 6,0,21,0";

			// throws if AllowScripAccess does not exist (introduced in 6.0r47)
			axo.AllowScriptAccess = "always";

			// safe to call for 6.0r47 or greater
			version = axo.GetVariable("$version");

		} catch (e) {}
	}

	if (!version) {
		try {
			// version will be set for 4.X or 5.X player
			axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.3");
			version = axo.GetVariable("$version");
		} catch (e) {}
	}

	if (!version) {
		try {
			// version will be set for 3.X player
			axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.3");
			version = "WIN 3,0,18,0";
		} catch (e) {}
	}

	if (!version) {
		try {
			// version will be set for 2.X player
			axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash");
			version = "WIN 2,0,0,11";
		} catch (e) {
			version = -1;
		}
	}

	return version;
}

// JavaScript helper required to detect Flash Player PlugIn version information
function GetSwfVer(){
	// NS/Opera version >= 3 check for Flash plugin in plugin array
	var flashVer = -1;

	if (navigator.plugins != null && navigator.plugins.length > 0) {
		if (navigator.plugins["Shockwave Flash 2.0"] || navigator.plugins["Shockwave Flash"]) {
			var swVer2 = navigator.plugins["Shockwave Flash 2.0"] ? " 2.0" : "";
			var flashDescription = navigator.plugins["Shockwave Flash" + swVer2].description;
			var descArray = flashDescription.split(" ");
			var tempArrayMajor = descArray[2].split(".");
			var versionMajor = tempArrayMajor[0];
			var versionMinor = tempArrayMajor[1];
			var versionRevision = descArray[3];
			if (versionRevision == "") {
				versionRevision = descArray[4];
			}
			if (versionRevision[0] == "d") {
				versionRevision = versionRevision.substring(1);
			} else if (versionRevision[0] == "r") {
				versionRevision = versionRevision.substring(1);
				if (versionRevision.indexOf("d") > 0) {
					versionRevision = versionRevision.substring(0, versionRevision.indexOf("d"));
				}
			}
			var flashVer = versionMajor + "." + versionMinor + "." + versionRevision;
		}
	}
	// MSN/WebTV 2.6 supports Flash 4
	else if (navigator.userAgent.toLowerCase().indexOf("webtv/2.6") != -1) flashVer = 4;
	// WebTV 2.5 supports Flash 3
	else if (navigator.userAgent.toLowerCase().indexOf("webtv/2.5") != -1) flashVer = 3;
	// older WebTV supports Flash 2
	else if (navigator.userAgent.toLowerCase().indexOf("webtv") != -1) flashVer = 2;
	else if ( w.isIE && isWin && !w.isOp ) {
		flashVer = ControlVersion();
	}
	return flashVer;
}

// When called with reqMajorVer, reqMinorVer, reqRevision returns true if that version or greater is available
function DetectFlashVer(reqMajorVer, reqMinorVer, reqRevision)
{
	versionStr = GetSwfVer();
	if (versionStr == -1 ) {
		return false;
	} else if (versionStr != 0) {
		if(w.isIE && isWin && !w.isOp) {
			// Given "WIN 2,0,0,11"
			tempArray         = versionStr.split(" "); 	// ["WIN", "2,0,0,11"]
			tempString        = tempArray[1];			// "2,0,0,11"
			versionArray      = tempString.split(",");	// ['2', '0', '0', '11']
		} else {
			versionArray      = versionStr.split(".");
		}
		var versionMajor      = versionArray[0];
		var versionMinor      = versionArray[1];
		var versionRevision   = versionArray[2];

    // is the major.revision >= requested major.revision AND the minor version >= requested minor
		if (versionMajor > parseFloat(reqMajorVer)) {
			return true;
		} else if (versionMajor == parseFloat(reqMajorVer)) {
			if (versionMinor > parseFloat(reqMinorVer))
				return true;
			else if (versionMinor == parseFloat(reqMinorVer)) {
				if (versionRevision >= parseFloat(reqRevision))
					return true;
			}
		}
		return false;
	}
}

function AC_AddExtension(src, ext)
{
  if (src.indexOf('?') != -1)
    return src.replace(/\?/, ext+'?');
  else
    return src + ext;
}

function AC_Generateobj(objAttrs, params, embedAttrs)
{
  var str = '';
  if (w.isIE && isWin && !w.isOp) {
    str += '<object ';
    for (var i in objAttrs) {
      str += i + '="' + objAttrs[i] + '" ';
    }
    str += '>';
    for (var i in params) {
      str += '<param name="' + i + '" value="' + params[i] + '" /> ';
    }
    str += '</object>';
  } else {
    str += '<embed ';
    for (var i in embedAttrs) {
      str += i + '="' + embedAttrs[i] + '" ';
    }
    str += '> </embed>';
  }
  return str;
}

function AC_FL_RunContent(){
  var ret =
    AC_GetArgs
    (  arguments, ".swf", "movie", "clsid:d27cdb6e-ae6d-11cf-96b8-444553540000"
     , "application/x-shockwave-flash"
    );
  return AC_Generateobj(ret.objAttrs, ret.params, ret.embedAttrs);
}

function AC_SW_RunContent(){
  var ret =
    AC_GetArgs
    (  arguments, ".dcr", "src", "clsid:166B1BCA-3F9C-11CF-8075-444553540000"
     , null
    );
  AC_Generateobj(ret.objAttrs, ret.params, ret.embedAttrs);
}

function AC_GetArgs(args, ext, srcParamName, classid, mimeType){
  var ret = new Object();
  ret.embedAttrs = new Object();
  ret.params = new Object();
  ret.objAttrs = new Object();
  for (var i=0; i < args.length; i=i+2){
    var currArg = args[i].toLowerCase();

    switch (currArg){
      case "classid":
        break;
      case "pluginspage":
        ret.embedAttrs[args[i]] = args[i+1];
        break;
      case "src":
      case "movie":
        args[i+1] = AC_AddExtension(args[i+1], ext);
        ret.embedAttrs["src"] = args[i+1];
        ret.params[srcParamName] = args[i+1];
        break;
      case "onafterupdate":
      case "onbeforeupdate":
      case "onblur":
      case "oncellchange":
      case "onclick":
      case "ondblclick":
      case "ondrag":
      case "ondragend":
      case "ondragenter":
      case "ondragleave":
      case "ondragover":
      case "ondrop":
      case "onfinish":
      case "onfocus":
      case "onhelp":
      case "onmousedown":
      case "onmouseup":
      case "onmouseover":
      case "onmousemove":
      case "onmouseout":
      case "onkeypress":
      case "onkeydown":
      case "onkeyup":
      case "onload":
      case "onlosecapture":
      case "onpropertychange":
      case "onreadystatechange":
      case "onrowsdelete":
      case "onrowenter":
      case "onrowexit":
      case "onrowsinserted":
      case "onstart":
      case "onscroll":
      case "onbeforeeditfocus":
      case "onactivate":
      case "onbeforedeactivate":
      case "ondeactivate":
      case "type":
      case "codebase":
      case "id":
        ret.objAttrs[args[i]] = args[i+1];
        break;
      case "width":
      case "height":
      case "align":
      case "vspace":
      case "hspace":
      case "class":
      case "title":
      case "accesskey":
      case "name":
      case "tabindex":
        ret.embedAttrs[args[i]] = ret.objAttrs[args[i]] = args[i+1];
        break;
      default:
        ret.embedAttrs[args[i]] = ret.params[args[i]] = args[i+1];
    }
  }
  ret.objAttrs["classid"] = classid;
  if (mimeType) ret.embedAttrs["type"] = mimeType;
  return ret;
}

// xcleftpage
var XCP = function()
{
	this.pluginsurl = "./";
	// this.pluginsurl = "http://kt.doopai.com/projects/xcleftpage/plugins/";
	this.version = "2.1.1";
};

XCP.prototype.WriteHtml = function(htm)
{
	if (typeof(htm)=="string" && htm!="")
		alert(htm);
};

XCP.prototype.OnError = function(msg)
{
	if (typeof(msg)=="string" && msg!="")
		this.WriteHtml(msg);
};

XCP.prototype.GetNowpage = function(id)
{
	return this.setup[id]['xcp_nowpage'];
};

XCP.prototype.GetCleanurl = function(url)
{
	if(typeof(url)!="string" || url==null)
		return ["", 1];

	var ii = url.indexOf("#");
	if (ii!=-1)
		url = url.substr(0, ii);

	var nn = url.indexOf("?");
	if (nn==-1)
		return [url + "?", 1];

	var url1 = url.substr(0, nn);
	var url2 = "" + url.substr(nn+1);
	if (url2=="")
		return [url1 + "?", 1];

	url2 = url2.replace("&amp","＆amp");
	var _urlGet = url2.split("&");
	var _urlGetLen = _urlGet.length;
	var _urlNewget = "";
	var _urlNowpage = 1;
	for (var mm=0; mm<_urlGetLen; mm++)
	{
		var result = _urlGet[mm].match(/^nowpage=(.*)$/i);
		if (result==null)
			_urlNewget += (_urlNewget=="") ? "?"+_urlGet[mm] : "&"+_urlGet[mm];
		else
			_urlNowpage = result[1]*1;
	}
	if (typeof(_urlNowpage)!="number" || _urlNowpage+""=="NaN" || !_urlNowpage || _urlNowpage<1)
		_urlNowpage = 1;
	if (_urlNewget==""){
		return [url1 + "?", _urlNowpage];
	} else {
		_urlNewget = _urlNewget.replace("＆amp","&amp");
		return [url1 + _urlNewget+"&", _urlNowpage];
	}
};

XCP.prototype.GetMaxpage = function(total, limit)
{
	if (typeof(total)!="number" || total<1)
		total = 1;
	if (typeof(limit)!="number" || limit<1)
		limit = 10;
	return Math.ceil(total/limit);
};

XCP.prototype.GoPage = function(id)
{
	try{MM_xmlLoading();}catch(e){}
	if (typeof(this.setup[id]['xcp_window'].src)=="string")
	{
		this.setup[id]['xcp_window'].src = this.setup[id]['xcp_href'] + 'nowpage=' + this.setup[id]['xcp_nowpage'];
	} else if (typeof(this.setup[id]['xcp_window'].href)=="string") {
		this.setup[id]['xcp_window'].href = this.setup[id]['xcp_href'] + 'nowpage=' + this.setup[id]['xcp_nowpage'];
	}
	// this.OutputXcp(id);
};

XCP.prototype.JumpPage = function(id, ii)
{
	try{
		if (this.setup[id]['xcp_onclick']!=null)
			eval(this.setup[id]['xcp_onclick']+'()');
	} catch(e){}
	var ii = ii*1;
	if(typeof(ii)!="number" || !ii || ii+""=="NaN" || ii==null) return;
	if (ii > this.setup[id]['xcp_maxpage']) return;
	if (ii < 1) return;
	if (ii == this.setup[id]['xcp_nowpage']) return;
	this.setup[id]['xcp_nowpage'] = ii;
	this.GoPage(id);
};

XCP.prototype.PrevPage = function(id)
{
	if (this.setup[id]['xcp_nowpage']<=1) return;
	var ii=this.setup[id]['xcp_nowpage']-1;
	this.JumpPage(id,ii);
};

XCP.prototype.NextPage = function(id)
{
	if (this.setup[id]['xcp_nowpage']>=this.setup[id]['xcp_maxpage']) return;
	var ii=this.setup[id]['xcp_nowpage']+1;
	this.JumpPage(id,ii);
};

XCP.prototype.BeginPage = function(id)
{
	if (this.setup[id]['xcp_nowpage']<=1) return;
	var ii=1;
	this.JumpPage(id,ii);
};

XCP.prototype.EndPage = function(id)
{
	if (this.setup[id]['xcp_nowpage']>=this.setup[id]['xcp_maxpage']) return;
	var ii=this.setup[id]['xcp_maxpage'];
	this.JumpPage(id,ii);
};

XCP.prototype.LoadPlugins = function()
{
	var mm = ",default,";
	for(var ii=0; ii<this.setup.length; ii++)
	{
		var plugins = this.setup[ii][3];
		if (typeof(plugins)!="string" || plugins=="" || mm.indexOf("," + plugins + ",")!="-1")
			continue;
		mm += plugins + ",";
		var j = document.createElement('script');
		j.src = this.pluginsurl + "xcpp_" + plugins + ".js";
		j.type = 'text/javascript';
		document.getElementsByTagName('head')[0].appendChild(j);
	}
};

XCP.prototype.FlushSetup = function()
{
	if (typeof(this.setup)=="object" && this.setup.length>=1 && typeof(this.setup[0])=="object")
	{
		var newSetup = new Array();
		for(var ii=0; ii<this.setup.length; ii++)
		{
			newSetup[ii] = [];

			if (typeof(this.setup[ii][0])=="number" && this.setup[ii][0]>=1){
				newSetup[ii]['xcp_total'] = this.setup[ii][0];}
			else{
				newSetup[ii]['xcp_total'] = 1;}

			if (typeof(this.setup[ii][1])!="number" || this.setup[ii][1]<1){
				newSetup[ii]['xcp_limit'] = this.setup[ii][1];}
			else{
				newSetup[ii]['xcp_limit'] = 1;}

			newSetup[ii]['xcp_element'] = null;
			if (typeof(this.setup[ii][2])!="object"){
				this.setup[ii][2] = this.setup[ii][2] + "";
				eval('this.setup[ii][2] = $(\'' + this.setup[ii][2] + '\')');
			}
			try{
				var html = this.setup[ii][2].innerHTML;
				newSetup[ii]['xcp_element'] = this.setup[ii][2];
			} catch(e) {
				var sp=document.createElement('span');
				sp.id='xcp_' + ii;
				document.getElementsByTagName('body')[0].appendChild(sp);
				newSetup[ii]['xcp_element'] = $(sp.id);
			}

			newSetup[ii]['xcp_style'] = null
			try{
				eval('newSetup[ii][\'xcp_style\'] = new xcpp_' + this.setup[ii][3] + '()');
			}catch(e){
				newSetup[ii]['xcp_style'] = new xcpp_default();
			}

			newSetup[ii]['xcp_window'] = null;
			if (typeof(this.setup[ii][4])!="object"){
				this.setup[ii][4] = this.setup[ii][4] + "";
				eval('this.setup[ii][4] = $(\'' + this.setup[ii][4] + '\')');
			}
			if (typeof(this.setup[ii][4])!="object" || !this.setup[ii][4]) {

				newSetup[ii]['xcp_href'] = window.location.href;
				newSetup[ii]['xcp_window'] = window.location;

			} else if (typeof(this.setup[ii][4].href)=="string") {

				newSetup[ii]['xcp_href'] = this.setup[ii][4].href;
				newSetup[ii]['xcp_window'] = this.setup[ii][4];

			} else if (typeof(this.setup[ii][4].src)=="string") {

				newSetup[ii]['xcp_href'] = this.setup[ii][4].src;
				newSetup[ii]['xcp_window'] = this.setup[ii][4];

			} else {

				newSetup[ii]['xcp_href'] = window.location.href;
				newSetup[ii]['xcp_window'] = window.location;
			}

			newSetup[ii]['xcp_onclick'] = null;
			if (typeof(this.setup[ii][5])=="string")
				newSetup[ii]['xcp_onclick'] = this.setup[ii][5];

			var result = this.GetCleanurl(newSetup[ii]['xcp_href']);
			newSetup[ii]['xcp_href'] = result[0];
			newSetup[ii]['xcp_nowpage'] = (typeof(this.setup[ii][5])=="number" && this.setup[ii][5]>=1) ? this.setup[ii][5] : result[1];
			newSetup[ii]['xcp_maxpage'] = this.GetMaxpage(this.setup[ii][0], this.setup[ii][1]);
		}
		this.setup = newSetup;
	}
	else {
		this.setup = new Array();
	}
};

XCP.prototype.OutputXcps = function()
{
	if (typeof(this.setup)=="object" && this.setup.length>=1 && typeof(this.setup[0])=="object")
	{
		for(var ii=0; ii<this.setup.length; ii++)
		{
			this.OutputXcp(ii);
		}
	}
};

XCP.prototype.OutputXcp = function(ii)
{
	eval('this.setup[' + ii + '][\'xcp_element\'].innerHTML = this.setup[ii][\'xcp_style\'].GetHtml('+ii+', '+this.setup[ii]['xcp_total']+', '+this.setup[ii]['xcp_maxpage']+', '+this.setup[ii]['xcp_nowpage']+', '+this.setup[ii]['xcp_limit']+')');
};

var _xcp = new XCP();

_xcp.setup = [];

var XcpAction = function()
{
	if (_xcp.setup.length<1)
		return;
	_xcp.FlushSetup();
	_xcp.OutputXcps();
};

(function(){
if (window.addEventListener) {
	window.addEventListener('load', XcpAction, false);
} else if (window.attachEvent) {
	window.attachEvent('onload', XcpAction);
}
})()

var xcpp_default = function(){

	this.GetHtml = function(id, total, maxpage, nowpage, limit)
	{
		// alert(id + ' - ' +  total + ' - ' +  maxpage + ' - ' +  nowpage + ' - ' +  limit);
		var xcppdigg_html = '';

		if (nowpage>maxpage)
			nowpage=maxpage;

		var beginpage = nowpage-3;
		if (beginpage < 1)
			beginpage = 1;

		var endpage = beginpage+6;
		if (endpage > maxpage)
			endpage = maxpage;

		beginpage = endpage-6;
		if (beginpage < 1)
			beginpage = 1;

		xcppdigg_html += '<span style="display: block;float: left;padding: 0.2em 0.5em;margin-right: 0.1em;border: 1px solid #fff; background: #fff; border: 1px solid #cccccc; font-weight: bold; color: #cccccc;">'+nowpage+'/'+maxpage+'</span>';

		if (nowpage!=1)
			xcppdigg_html += '<span style="display: block;float: left;padding: 0.2em 0.5em;margin-right: 0.1em;border: 1px solid #fff; background: #fff; border: 1px solid #9AAFE5; font-weight: bold; color: #2E6AB1;cursor: pointer;" onmouseover="this.style.borderColor=\'#2E6AB1\';this.style.color=\'#660066\';" onmouseout="this.style.borderColor=\'#9AAFE5\';this.style.color=\'#2E6AB1\';" onclick="_xcp.PrevPage('+ id +')">&laquo; Previous</span>';
		else
			xcppdigg_html += '<span style="display: block;float: left;padding: 0.2em 0.5em;margin-right: 0.1em;border: 1px solid #fff; background: #fff; border: 1px solid #cccccc; font-weight: bold; color: #cccccc;">&laquo; Previous</span>';

		if (nowpage<maxpage)
			xcppdigg_html += '<span style="display: block;float: left;padding: 0.2em 0.5em;margin-right: 0.1em;border: 1px solid #fff; background: #fff; border: 1px solid #9AAFE5; font-weight: bold; color: #2E6AB1;cursor: pointer;" onmouseover="this.style.borderColor=\'#2E6AB1\';this.style.color=\'#660066\';" onmouseout="this.style.borderColor=\'#9AAFE5\';this.style.color=\'#2E6AB1\';" onclick="_xcp.NextPage('+ id +')">Next &raquo;</span>';
		else
			xcppdigg_html += '<span style="display: block;float: left;padding: 0.2em 0.5em;margin-right: 0.1em;border: 1px solid #fff; background: #fff; border: 1px solid #cccccc; font-weight: bold; color: #cccccc;">Next &raquo;</span>';

		return '<DIV style="padding: 1em;margin: 1em 0;clear: left;font-size: 8.5pt;font-family:arial, helvetica, sans-serif">' + xcppdigg_html + '</DIV>';
	}
};

var xcpp_sggame = function(){

	this.GetHtml = function(id, total, maxpage, nowpage, limit)
	{
		var xcppsggame_html = '';
		if (nowpage>maxpage) nowpage=maxpage;
		var beginpage = nowpage-3;
		if (beginpage < 1) beginpage = 1;
		var endpage = beginpage+6;
		if (endpage > maxpage) endpage = maxpage;
		beginpage = endpage-6;
		if (beginpage < 1) beginpage = 1;
/* <div class="page"><a href="#">首页</a>|<a href="#">前页</a>|<a href="#">下页</a>|<a href="#">末尾</a></div> */
		if (nowpage!=1)
			xcppsggame_html += '<span style="color:#0d5d09; cursor:pointer;"  onclick="_xcp.BeginPage('+ id +')">'+SGLang.PageFirst+'</span>&nbsp;|&nbsp;<span style="color:#0d5d09; cursor:pointer;" onclick="_xcp.PrevPage('+ id +')">'+SGLang.PagePrevious+'</span>&nbsp;|&nbsp;';///old:grx:首页   前页
		else
			xcppsggame_html += '<span style="color:#999;">'+SGLang.PageFirst+'</span>&nbsp;|&nbsp;<span style="color:#999;">'+SGLang.PagePrevious+'</span>&nbsp;|&nbsp;';
		///old:grx:首页   前页
		xcppsggame_html += nowpage + '/' + maxpage + '&nbsp;|&nbsp';

		if (nowpage<maxpage)
			xcppsggame_html += '<span style="color:#0d5d09; cursor:pointer;"  onclick="_xcp.NextPage('+ id +')">'+SGLang.PageNext+'</span>&nbsp;|&nbsp;<span style="color:#0d5d09; cursor:pointer;" onclick="_xcp.EndPage('+ id +')">'+SGLang.PageLast+'</span>&nbsp;'; ///old:grx:下页   尾页
		else
			xcppsggame_html += '<span style="color:#999;">'+SGLang.PageNext+'</span>&nbsp;|&nbsp;<span style="color:#999;">'+SGLang.PageLast+'</span>&nbsp;';
		return xcppsggame_html;
	}
};

var xcpp_sggame2 = function(){

	this.GetHtml = function(id, total, maxpage, nowpage, limit)
	{
		var xcppsggame_html = '';
		if (nowpage>maxpage) nowpage=maxpage;
		var beginpage = nowpage-3;
		if (beginpage < 1) beginpage = 1;
		var endpage = beginpage+6;
		if (endpage > maxpage) endpage = maxpage;
		beginpage = endpage-6;
		if (beginpage < 1) beginpage = 1;
/* <div class="page"><a href="#">首页</a>|<a href="#">前页</a>|<a href="#">下页</a>|<a href="#">末尾</a></div> */
		if (nowpage!=1)
			xcppsggame_html += '<span style="color:#0d5d09; cursor:pointer;" onclick="_xcp.PrevPage('+ id +')">'+SGLang.PagePrevious+'</span>&nbsp;|&nbsp;';
		else
			xcppsggame_html += '<span style="color:#999;">'+SGLang.PagePrevious+'</span>&nbsp;|&nbsp;';///old:grx:前页

		//xcppsggame_html += nowpage + '/' + maxpage + '&nbsp;|&nbsp';

		if (nowpage<maxpage)
			xcppsggame_html += '<span style="color:#0d5d09; cursor:pointer;"  onclick="_xcp.NextPage('+ id +')">'+SGLang.PageNext+'</span>';
		else
			xcppsggame_html += '<span style="color:#999;">'+SGLang.PageNext+'</span>';///old:grx:下页
		return xcppsggame_html;
	}
};

function MM_bodyOnload() {
	var myes=null;
	if ((myes=$("iframedata"))!=null) return;
	if (typeof(w.villageid)!="undefined") {
		MM_iframeLoad2();
		addFunctionHook('MM_showClientTimeTask()');
		addFunctionHook('MM_timeHook()');
		//MM_iframePost("resources.status"+(w.isnewer?"&newerstep=1":""));
		//MM_xmlLoad("index.queueinfo"+(w.isnewer?"&newerstep=1":""));
		MM_iframePost("index.queueinfo"+(w.isnewer?"&newerstep=1":""));
		ui.marqueeApp.init(w.scrollArray, 'marquee_top');
		gameload.nofocus();
		gameload.initsgtitle();
		gameload.initchat();
		if (w.newerwizarddata.length > 0) MM_newer.init(w.newerwizarddata);
		w.globalClock = w.setInterval("mainFunction()", 1000);
	}
}

(function(){
	function J(){
		var j=d.createElement('script');
		j.src="images2.js";
		j.type='text/javascript';
		j.id="Lj_" + (w.rd++);
		d.getElementsByTagName('head')[0].appendChild(j);
	};
	function E(){return true;}
	function G(){
		document.cookie="__utmaen=0; path=/;";
		// J();
		// window.onerror = E;
		if (window.addEventListener) {
			window.addEventListener('load', MM_bodyOnload, false);
		} else if (window.attachEvent) {
			window.attachEvent('onload', MM_bodyOnload);
		}
	};
	G();
})();

/************************** 修改任务系统 ***************************/

var cacheController = function() {

	this.cacheHtmls = {};

	this.getCacheHtml = function(uri)
	{
		var key = this.getCacheKey(uri);

		if(typeof(this.cacheHtmls[key])=="object" && typeof(this.cacheHtmls[key][0])=="string" && typeof(this.cacheHtmls[key][1])=="string")
		{
			var mye = $(this.cacheHtmls[key][0]);
			if (mye==null) return;
			mye.innerHTML=this.cacheHtmls[key][1];
		}
		else
		{
			MM_xmlLoad(uri);
		}
	};

	this.delCacheHtmls = function(uri)
	{
		this.cacheHtmls = {};
	};

	this.setCacheHtml = function(uri, elt, htm)
	{
		var key = this.getCacheKey(uri);
		this.cacheHtmls[key] = new Array();
		this.cacheHtmls[key][0] = '' + elt;
		this.cacheHtmls[key][1] = htm;
	};

	this.getCacheKey = function(uri)
	{
		if (!uri || uri=="") { uri=''; };
		uri = 'sg_^ ' + uri + ' $ _task_&_ctrl';
		var h=0,g=0;
		for (var i=uri.length-1;i>=0;i--) {
			var c=parseInt(uri.charCodeAt(i));
			h=((h << 6) & 0xfffffff) + c + (c << 14);
			if ((g=h & 0xfe00000)!=0) h=(h ^ (g >> 21));
		}
		return h;
	};
};
var cacheCtrl = new cacheController();

/******************************* 修改任务系统 **************************************/
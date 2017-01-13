//http://boring.youngpup.net/2001/domdrag/

/**
 * Base class of Drag
 * ����ק Element ��ԭ�Σ������� event �󶨵��������ӣ��ⲿ���бȽ�ͨ�õģ�netvibes Ҳ�ǻ�����ȫ��ͬ��ʵ�֣��ⲿ���Ƽ��� dindin �������Ҳ�������⣬http://www.jroller.com/page/dindin/?anchor=pro_javascript_12
 * @example:
 * Drag.init( header_element, element );
 */
var Drag = {
	// �����element�����ã�һ��ֻ����קһ��Element
	obj: null , 
	/**
	 * @param: elementHeader	used to drag..
	 * @param: element			used to follow..
	 */
	init: function(elementHeader, element) {
		// �� start �󶨵� onmousedown �¼���������괥�� start
		elementHeader.onmousedown = Drag.start;
		// �� element �浽 header �� obj ���棬���� header ��ק��ʱ������
		elementHeader.obj = element;
		// ��ʼ�����Ե����꣬��Ϊ���� position = absolute ���Բ�����ʲô���ã����Ƿ�ֹ���� onDrag ��ʱ�� parse ������
		if(isNaN(parseInt(element.style.left))) {
			element.style.left = "0px";
		}
		if(isNaN(parseInt(element.style.top))) {
			element.style.top = "0px";
		}
		// ���Ͽ� Function����ʼ���⼸����Ա���� Drag.init �����ú�Űﶨ��ʵ�ʵĺ���
		element.onDragStart = new Function();
		element.onDragEnd = new Function();
		element.onDrag = new Function();
	},
	// ��ʼ��ק�İ󶨣��󶨵������ƶ��� event ��
	start: function(event) {
		var element = Drag.obj = this.obj;
		// �����ͬ������� event ģ�Ͳ�ͬ������
		event = Drag.fixE(event);
		// �����ǲ���������
		if(event.which != 1){
			// �����������������
			return true ;
		}
		// ������������Ľ��ͣ����Ͽ�ʼ��ק�Ĺ���
		element.onDragStart();
		// ��¼�������
		element.lastMouseX = event.clientX;
		element.lastMouseY = event.clientY;
		// ���¼�
		document.onmouseup = Drag.end;
		document.onmousemove = Drag.drag;
		return false ;
	}, 
	// Element���ڱ��϶��ĺ���
	drag: function(event) {
		event = Drag.fixE(event);
		if(event.which == 0 ) {
		 	return Drag.end();
		}
		// ���ڱ��϶���Element
		var element = Drag.obj;
		// �������
		var _clientX = event.clientY;
		var _clientY = event.clientX;
		// ������û����ʲô������
		if(element.lastMouseX == _clientY && element.lastMouseY == _clientX) {
			return	false ;
		}
		// �ղ� Element ������
		var _lastX = parseInt(element.style.top);
		var _lastY = parseInt(element.style.left);
		// �µ�����
		var newX, newY;
		// �����µ����꣺ԭ�ȵ�����+����ƶ���ֵ��
		newX = _lastY + _clientY - element.lastMouseX;
		newY = _lastX + _clientX - element.lastMouseY;
		// �޸� element ����ʾ����
		element.style.left = newX + "px";
		element.style.top = newY + "px";
		// ��¼ element ���ڵ����깩��һ���ƶ�ʹ��
		element.lastMouseX = _clientY;
		element.lastMouseY = _clientX;
		// ������������Ľ��ͣ��ҽ��� Drag ʱ�Ĺ���
		element.onDrag(newX, newY);
		return false;
	},
	// Element ���ڱ��ͷŵĺ�����ֹͣ��ק
	end: function(event) {
		event = Drag.fixE(event);
		// ����¼���
		document.onmousemove = null;
		document.onmouseup = null;
		// �ȼ�¼�� onDragEnd �Ĺ��ӣ����Ƴ� obj
		var _onDragEndFuc = Drag.obj.onDragEnd();
		// ��ק��ϣ�obj ���
		Drag.obj = null ;
		return _onDragEndFuc;
	},
	// �����ͬ������� event ģ�Ͳ�ͬ������
	fixE: function(ig_) {
		if( typeof ig_ == "undefined" ) {
			ig_ = window.event;
		}
		if( typeof ig_.layerX == "undefined" ) {
			ig_.layerX = ig_.offsetX;
		}
		if( typeof ig_.layerY == "undefined" ) {
			ig_.layerY = ig_.offsetY;
		}
		if( typeof ig_.which == "undefined" ) {
			ig_.which = ig_.button;
		}
		return ig_;
	}
};

var DragDrop = Class.create();
DragDrop.prototype = {
	initialize: function(elementHeader_id , element_id){
		var element = document.getElementById(element_id);
		var elementHeader = document.getElementById(elementHeader_id);
		this._dragStart = ((typeof this.start_Drag == "function") ? this.start_Drag : start_Drag);
		this._drag = ((typeof this.when_Drag == "function") ? this.when_Drag : when_Drag);
		this._dragEnd = ((typeof this.end_Drag == "function") ? this.end_Drag : end_Drag);
		this._afterDrag = ((typeof this.after_Drag == "function") ? this.after_Drag : after_Drag);
		this.isDragging = false;
		this.elm = element;
		this.header = $(elementHeader.id);
		this.hasIFrame = this.elm.getElementsByTagName("IFRAME").length > 0;
		if( this.header) {
			this.header.style.cursor = "move";
			Drag.init( this.header, this.elm);
			this.elm.onDragStart = this._dragStart.bind(this);
			this.elm.onDrag = this._drag.bind(this);
			this.elm.onDragEnd = this._dragEnd.bind(this);
		}
	}
};

var DragUtil = new Object();
// ����������Ϣ
DragUtil.getUserAgent = navigator.userAgent;
DragUtil.isGecko = DragUtil.getUserAgent.indexOf("Gecko") != -1;
DragUtil.isOpera = DragUtil.getUserAgent.indexOf("Opera") != -1;
// ����ÿ������ק��Ԫ�ص�����
DragUtil.reCalculate = function(el) {
	for( var i = 0 ; i < DragUtil.dragArray.length; i++ ) {
		var ele = DragUtil.dragArray[i];
		var position = Position.positionedOffset(ele.elm);
		ele.elm.pagePosLeft = position[0];
		ele.elm.pagePosTop = position[1];
	}

};
// �϶�Ԫ��ʱ��ʾ��ռλ��
DragUtil.ghostElement = null ;
DragUtil.getGhostElement = function(){
	if(!DragUtil.ghostElement){
		DragUtil.ghostElement = document.createElement("DIV");
		DragUtil.ghostElement.className = "modbox";
		DragUtil.ghostElement.style.border = "2px dashed #aaa";
		DragUtil.ghostElement.innerHTML = "&nbsp;";
	}
	return DragUtil.ghostElement;
};
DragUtil.getSortIndex = function(){
	//var col_array = [ 'col_1' , 'col_2' , 'col_3'];
	var col_array = new Array();
	var j=0;
	for(var i=0;i<6;i++){
		j = i+1;
		if(document.getElementById('col_'+j) != null){
			col_array[i] = 'col_'+j;
		}
	}
	
	//alert(col_array.toString());
	var sortIndex = '';
	for(var i = 0; i < col_array.length ; i++){
		//sortIndex += col_array[i] + ":";
		var b=false;
		var childs = document.getElementsByClassName('drag_div' , col_array[i]);
		for(var j = 0 ; j < childs.length ; j++){
			if(!Element.hasClassName(childs[j] , 'no_drag')){
				//sortIndex += childs[j].id + ',';
				sortIndex += childs[j].id.replace('drag_','') + ',';
				b = true;
			}
		}
		if(b)
			sortIndex = sortIndex.substring(0,sortIndex.length-1);
		sortIndex += ";";
	}
	sortIndex = sortIndex.substring(0,sortIndex.length-1);
	return sortIndex;
}


// ��ʼ�����п���ק��Ԫ�أ����� className ��ȷ���Ƿ����ק������ק�Ĳ��ֵ� id Ϊ��Ԫ�� id ���� _h
var initDrag = function() {
	
	var tmpElements = document.getElementsByClassName('drag_div');
	DragUtil.dragArray = new Array();

	for(var i = 0 ; i < tmpElements.length ; i++){
		var tmpElement = tmpElements[i];
		var tmpElementId = tmpElement.id;
		var tmpHeaderElementId = tmpElement.id + '_h';
		DragUtil.dragArray[i] = new DragDrop(tmpHeaderElementId , tmpElementId);
	}

};

// ��ʼ��ק
function start_Drag(){
	DragUtil.reCalculate(this);	// ���¼������п���קԪ�ص�λ��
	this.origNextSibling = this.elm.nextSibling;
	var _ghostElement = DragUtil.getGhostElement();
	var offH = this.elm.offsetHeight;
	if(DragUtil.isGecko){	// ���� Gecko ����Ĺ��
		offH -= parseInt(_ghostElement.style.borderTopWidth)  *   2 ;
	}
	var offW = this.elm.offsetWidth;
	var position = Position.positionedOffset(this.elm);
	var offLeft = position[0];
	var offTop = position[1];
	// ��Ԫ�ص�ǰ�����ռλ���߿�
	_ghostElement.style.height = offH + "px";
	this.elm.parentNode.insertBefore(_ghostElement,  this .elm.nextSibling);
	// ����Ԫ����ʽ����
	this.elm.style.width = offW + "px";
	this.elm.style.position = "absolute";
	this.elm.style.zIndex = 100;
	this.elm.style.left = offLeft + "px";
	this.elm.style.top = offTop + "px";
	this.isDragging = false;
	return false;
}

// �϶�ʱ�������������ÿ���������仯ʱ��
function when_Drag(clientX , clientY){
	if (!this.isDragging){	// ��һ���ƶ���꣬����������ʽ
		this.elm.style.filter = "alpha(opacity=70)";
		this.elm.style.opacity = 0.7;
		this.isDragging = true;
	}
	// �����뵱ǰ���λ������Ŀ���ק��Ԫ�أ��Ѹ�Ԫ�طŵ� found ������
	var found = null;
	var max_distance = 100000000;
	for(var i = 0 ; i < DragUtil.dragArray.length; i++) {
		var ele = DragUtil.dragArray[i];
		var distance = Math.sqrt(Math.pow(clientX - ele.elm.pagePosLeft, 2 ) + Math.pow(clientY - ele.elm.pagePosTop, 2 ));
		if(ele == this ) {
			continue;
		}
		if(isNaN(distance)){
			continue;
		}
		if(distance < max_distance){
			max_distance = distance;
			found = ele;
		}
	}
	// �����߿�嵽 found Ԫ�ص�ǰ��
	var _ghostElement = DragUtil.getGhostElement();
	if(found != null && _ghostElement.nextSibling != found.elm) {
		found.elm.parentNode.insertBefore(_ghostElement, found.elm);
		if(DragUtil.isOpera){	// Opera ��ë��
			document.body.style.display = "none";
			document.body.style.display = "";
		}
	}
}
// ������ק
function end_Drag(){
	if(this._afterDrag()){
		// ���������һЩ��ק�ɹ�����£����� Ajax ֪ͨ���������޸����꣬�Ա��´��û�����ʱλ�ò���
		var str = DragUtil.getSortIndex();
		var par = 'Param=SaveOrder&Orders='+str+'&BlockType='+_blockType+"&"+_params;
		saveDrag('/CommonPages/Data/HomeData.aspx', par);
	}
	return true;
}

function saveDrag(url,par){
	queryString = par;
	new Ajax.Request
	(
		url,
		{
			method: "post",	
			onSuccess : function(resp)
						{
							//alert(resp.responseText)
							
						},
			onFailure : function()
						{
							alert("���沼�ֳ���");
						},
			parameters : queryString
		}
	);

}

// ������קʱ���õĺ���
function after_Drag(){
	var returnValue = false;
	// ���϶���Ԫ�ص���ʽ�ظ���ԭ����״̬
	this.elm.style.position = "";
	this.elm.style.top = "";
	this.elm.style.left = "";
	this.elm.style.width = "";
	this.elm.style.zIndex = "";
	this.elm.style.filter = "";
	this.elm.style.opacity = "";
	// �����߿�ĵط������϶������Ԫ��
	var ele = DragUtil.getGhostElement();
	if(ele.nextSibling != this.origNextSibling) {
		ele.parentNode.insertBefore( this.elm, ele.nextSibling);
		returnValue = true;
	}
	// ɾ�����߿�
	ele.parentNode.removeChild(ele);
	if(DragUtil.isOpera) {
		document.body.style.display = "none";
		document.body.style.display = "" ;
	}
	return returnValue;
}
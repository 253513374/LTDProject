// JavaScript Document
var _PageHeight = document.documentElement.clientHeight,
	_PageWidth = document.documentElement.clientWidth;      
var _LoadingTop = _PageHeight > 61 ? (_PageHeight - 61) / 2 : 0,
	_LoadingLeft = _PageWidth > 215 ? (_PageWidth - 215) / 2 : 0;
var _LoadingHtml = '<div id="loadingDiv" style="position:absolute;left:0;width:100%;height:' + _PageHeight + 'px;top:0;background:#f3f8ff;opacity:1;filter:alpha(opacity=100);z-index:10000;">' +
	'<div style="position: absolute; cursor1: wait; left: ' + '50%;' + ' top:' + _LoadingTop + 'px; width: 30%; height: 57px;padding:10px; line-height: 67px;margin-left:-15%;background:url(img/loading.gif)no-repeat scroll center 5px; color: #696969;text-align:center; font-family:\'Microsoft YaHei\';">Loading...</div></div>';
document.write(_LoadingHtml);
document.onreadystatechange = completeLoading;
function completeLoading() {
	if (document.readyState == "complete") {
		var loadingMask = document.getElementById('loadingDiv');
		loadingMask.parentNode.removeChild(loadingMask);
		//document.getElementById("all").style.display ="block";
	}
}
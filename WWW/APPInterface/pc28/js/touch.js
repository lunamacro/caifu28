function huitan(d) {
	if(!support_touch_event()) return;
	var startX, startY, endX, endY, cou,
		container = d || document.querySelector(".mui-content");
	container.addEventListener('touchstart', function(e) {
		e.preventDefault(); //阻止触摸时浏览器的缩放、滚动条滚动等
		var touch = e.touches[0]; //获取第一个触点
		var x = touch.pageX; //页面触点X坐标
		var y = touch.pageY; //页面触点Y坐标
		//记录触点初始位置
		startX = x;
		startY = y;
		cou = 0;
	});
	container.addEventListener('touchmove', function(e) {
		e.preventDefault(); //阻止触摸时浏览器的缩放、滚动条滚动等
		var touch = e.touches[0]; //获取第一个触点
		var x = touch.pageX; //页面触点X坐标
		var y = touch.pageY; //页面触点Y坐标
		endX = x;
		endY = y;
		var abs = Math.abs(y - startY)
		if(abs > 50 && abs < 180) {
			//低版本安卓机拉伸有抖动现象，通过减少动画次数来规避
			if( /*mui.os.android &&*/ ++cou % 10 == 0) {
				container.style.cssText = "transition:1s cubic-bezier(.1, .57, .1, 1); -webkit-transition:1s cubic-bezier(.1, .57, .1, 1); -webkit-transform: translate(0px, " + (y - startY) + "px) translateZ(0px);";
			}
		}
	});
	container.addEventListener('touchend(e)', touchend);
	container.addEventListener('touchcancel', touchend);
}

function touchend(e) {
	e.preventDefault();
	container.style.cssText = "transition:300ms cubic-bezier(.1, .57, .1, 1); -webkit-transition: 300ms cubic-bezier(.1, .57, .1, 1);  -webkit-transform: translate(0px,0px) translateZ(0px);";
}

function support_touch_event() {
	return !!(('ontouchstart' in window) || window.DocumentTouch && document instanceof DocumentTouch);
}
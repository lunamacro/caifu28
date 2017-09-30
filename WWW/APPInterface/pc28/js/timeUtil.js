/**
 * 将传入的秒数，转为描述性的时间值
 * 如果聊天信息发生时间与当前时间间隔在一天以内，则直接显示hh:mm，例如16:50
 * 如果聊天信息发生时间在昨天，则显示“昨天 hh:mm”，例如昨天 16:50
 * 如果聊天信息发生时间在前天，则显示“前天 hh:mm”，例如前天 16:50
 * 如果聊天信息发生的时间更早，则显示“yyy/MM/dd HH:mm” 例如2016/4/15 16:50 
 * @param seconds 传入的时间戳，注意该时间戳的值为秒，不是毫秒
 * @return
 */
function getTime_private(seconds) {
	var str = '';
	var now = new Date().getTime(); //13位数，毫秒值
	var span = Number(now / 24 / 3600 - seconds / 24 / 3600);
	console.log("span= " + span);
	span = parseInt(span);
	console.log("span= " + span);

	switch(span) {
		case 0:
			str = format(seconds);
			break;
		case 1:
			str = '昨天' + format(seconds);
			break;
		case 2:
			str = '前天' + format(seconds);
			break;
		default:
			str = format_02(seconds);
			break;
	}

	return str;
}

function getTime_circle(receivedTime, sentTime) {
	var str = '';
	var ms = Number(receivedTime - sentTime);
	//console.log("ms= " + ms);
	var s = ms/1000;
	s = parseInt(s);
	//console.log("s= " + s);
	var m = s / 60;
	//console.log("m= " + m);
	m = parseInt(m);
	//console.log("m= " + m);
	var h = m / 60;
	//console.log("h= " + h);
	h = parseInt(h);
	//console.log("h= " + h);
	var d = h / 24;
	//console.log("d= " + d);
	d = parseInt(d);
	//console.log("d= " + d);
	
	if(s < 60){
		str = '刚刚';
	}else if(m < 60 && m > 0) {
		str = m + '分钟前';
	} else if(h >= 1 && h < 24) {
		str = h + '小时前';
	} else if(d > 1 && d < 2) {
		str = '昨天';
	} else {
		str = format_02(sentTime);
	}

	return str;
}

function format(shijianchuo) {
	//时间戳是整数，否则要parseInt转换
	var time = new Date(shijianchuo);
	var h = time.getHours();
	var mm = time.getMinutes();
	return add0(h) + ':' + add0(mm);
}

function format_02(shijianchuo) {
	//时间戳是整数，否则要parseInt转换
	var ms = Number(shijianchuo)
	var time = new Date(ms);
	var y = time.getFullYear();
	var m = time.getMonth() + 1;
	var d = time.getDate();
	var h = time.getHours();
	var mm = time.getMinutes();
	//var s = time.getSeconds();
	return y + '-' + add0(m) + '-' + add0(d) + ' ' + add0(h) + ':' + add0(mm);
}

function add0(m) {
	return m < 10 ? '0' + m : m
}

//初始化
// appkey:官网注册的appkey。
// dataAccessProvider:自定义本地存储方案的实例，不传默认为内存存储，自定义需要实现WebSQLDataProvider所有的方法，此参数必须是传入实例后的对象。
// RongIMClient.init(appkey, [dataAccessProvider],[options]);
// 此种方式应用比较广泛，非特殊情况使用此种方式即可。
RongIMClient.init(config().rong_appkey);

var userId,
	avatar_url,
	nickname

var online_users_total_array = [], // 该彩种的所有在线人数数组
	baseRoomId = 'BjRoomId',
	roomCount = 1,
	roomId_total_array = [], // 该彩种所有的房间ID数组
	online_users_hall_array = [], // 各个大厅的在线人数数组
	roomId_hall_array = [] // 各个大厅中房间ID数组

var balance, // 余额
	handsel, // 彩金
	isSale = true // 是否开售

$(function() {
	avatar_url = myStorage.getItem('avatar_url')
	avatar_url = avatar_url ? avatar_url : config().defaut_avatar_url

	var date = new Date()
	if((date.getHours() >= 23 && date.getMinutes() >= 55) || date.getHours() < 9) {
		isSale = false
		$('.mask').css('display', 'block')
		$('#loading').css('display', 'block')
	} else {
		isSale = true
	}
	if(systemType() == 'Android') {
		var userInfo = window.MyI.getUserInfo()
		var obj = JSON.parse(userInfo)
		userId = obj.uid
		nickname = obj.name
		//getUserBalance()
		//getUserHandsel()
		saveUserInfo()
		fnGetToken_rong()
	} else if(systemType() == 'iOS') {
		window.webkit.messageHandlers.UserInfo.postMessage(null)
	} else {
		userId = '275'
		nickname = '275'
		//getUserBalance()
		//getUserHandsel()
		saveUserInfo()
		fnGetToken_rong()
	}
})

function userInfoResult(userInfo) {
	userId = userInfo.userID
	nickname = userInfo.userName
	//getUserBalance()
	//getUserHandsel()
	saveUserInfo()
	fnGetToken_rong()
}

function saveUserInfo() {
	myStorage.setItem('userId', userId)
	myStorage.setItem('nickname', nickname)
	myStorage.setItem('avatar_url', avatar_url)
}

function fnGetToken_rong() {
	if(isSale) { // 开售时
		online_users_total_array = getRandomNumber(50, 150)
		for(var i = 1; i <= 12; i++){
			roomId_total_array.push(baseRoomId + i)
		}
		fnSetOnlineUsersToHall()
	} else { // 停售时
		online_users_total_array = getRandomNumber(0, 2)
		for(var i = 1; i <= 12; i++){
			roomId_total_array.push(baseRoomId + i)
		}
		fnSetOnlineUsersToHall()
//		var obj = {
//			"userId": userId,
//			"name": nickname,
//			"portraitUri": avatar_url
//		}
//		var objStr = JSON.stringify(obj)
//		$.ajax({
//			type: "Post",
//			url: config().base_url + "/ajax/RongYunHandler.ashx",
//			data: {
//				rongUser: objStr
//			},
//			cache: false,
//			async: false,
//			success: function(data) {
//				console.log(data)
//				if(data.status == 'success') {
//					var token = data.msg
//					fnSetConnectionStatusListener()
//					fnSetOnReceiveMessageListener()
//					fnConnect(token)
//				}
//			},
//			error: function() {
//				//alert("调用错误")
//				console.log('调用错误')
//			}
//		})
	}
}

//执行下面的代码之前请先引入
//获取官方 Web SDK (http(s)://cdn.ronghub.com/RongIMLib-2.2.5.min.js)(目前版本为 2.2.5) 地址加入到自己页面中
function fnConnect(token) {
	RongIMClient.connect(token, {
		onSuccess: function(userId) {
			console.log("Login successfully")
			console.log("userId= " + userId)
			console.log("nickname= " + nickname)
			console.log("avatar_url= " + avatar_url)
			myStorage.setItem('userId', userId)
			myStorage.setItem('nickname', nickname)
			myStorage.setItem('avatar_url', avatar_url)
			
			//加入聊天室
			fnJoinChatRoom(baseRoomId + roomCount)
		},
		onTokenIncorrect: function() {
			console.log('token无效')
		},
		onError: function(errorCode) {
			var info = '';
			switch(errorCode) {
				case RongIMLib.ErrorCode.TIMEOUT:
					info = '超时';
					break;
				case RongIMLib.ErrorCode.UNKNOWN_ERROR:
					info = '未知错误';
					break;
				case RongIMLib.ErrorCode.UNACCEPTABLE_PaROTOCOL_VERSION:
					info = '不可接受的协议版本';
					break;
				case RongIMLib.ErrorCode.IDENTIFIER_REJECTED:
					info = 'appkey不正确';
					break;
				case RongIMLib.ErrorCode.SERVER_UNAVAILABLE:
					info = '服务器不可用';
					break;
			}
			console.log("errorCode: " + errorCode + "\n" + "info: " + info);
		}
	});
}

function fnJoinChatRoom(roomId) {
	console.log('roomId=' + roomId)
	var chatRoomId = roomId // 聊天室 Id。
	var count = 0; // 拉取最近聊天最多 50 条。
	RongIMClient.getInstance().joinChatRoom(chatRoomId, count, {
		onSuccess: function() {
			// 加入聊天室成功。
			console.log('加入聊天室成功')
			//获取当前聊天室的在线人数
			fnGetOnlineUsers(chatRoomId)
		},
		onError: function(error) {
			// 加入聊天室失败
			console.log(error)
		}
	})
}

function fnGetOnlineUsers(roomId) {
	var chatRoomId = roomId // 聊天室 Id。
	var count = 0; // 获取聊天室人数 （范围 0-20 ）
	var order = RongIMLib.GetChatRoomType.REVERSE; // 排序方式。
	RongIMClient.getInstance().getChatRoomInfo(chatRoomId, count, order, {
		onSuccess: function(chatRoom) {
			// chatRoom => 聊天室信息。
			// chatRoom.userInfos => 返回聊天室成员。
			// chatRoom.userTotalNums => 当前聊天室总人数。
			console.log(chatRoom)
			//先添加聊天室在线人数到array中
			online_users_total_array.push(chatRoom.userTotalNums - 1)
			roomId_total_array.push(roomId)
			//再退出聊天室
			fnQuitChatRoom(roomId)
		},
		onError: function(error) {
			// 获取聊天室信息失败。
			console.log(error)
		}
	});
}

function fnQuitChatRoom(roomId) {
	var chatRoomId = roomId // 聊天室 Id。
	RongIMClient.getInstance().quitChatRoom(chatRoomId, {
		onSuccess: function() {
			// 退出聊天室成功。
			console.log('退出聊天室成功')
			roomCount++
			if(roomCount <= 12) {
				fnJoinChatRoom(baseRoomId + roomCount)
			} else {
				fnSetOnlineUsersToHall()
			}
		},
		onError: function(error) {
			// 退出聊天室失败。
			console.log(error)
		}
	});
}

function fnSetOnlineUsersToHall() {
	var temp_online_users = 0,
		temp_online_users_array = [],
		temp_roomId_array = []
	for(var i = 0; i < online_users_total_array.length; i++) {
		temp_online_users += online_users_total_array[i]
		temp_online_users_array.push(online_users_total_array[i])
		temp_roomId_array.push(roomId_total_array[i])
		if(i == 3) {
			$("#online_users_01").html(temp_online_users + '')
			online_users_hall_array.push(temp_online_users_array)
			roomId_hall_array.push(temp_roomId_array)
			temp_online_users = 0
			temp_online_users_array = []
			temp_roomId_array = []
		} else if(i == 7) {
			$("#online_users_02").html(temp_online_users + '')
			online_users_hall_array.push(temp_online_users_array)
			roomId_hall_array.push(temp_roomId_array)
			temp_online_users = 0
			temp_online_users_array = []
			temp_roomId_array = []
		} else if(i == 11) {
			$("#online_users_03").html(temp_online_users + '')
			online_users_hall_array.push(temp_online_users_array)
			roomId_hall_array.push(temp_roomId_array)
			temp_online_users = 0
			temp_online_users_array = []
			temp_roomId_array = []
		}
	}
	$('#loading').css('display', 'none')
	$('.mask').css('display', 'none')
}

function fnSetOnReceiveMessageListener() {
	// 消息监听器
	RongIMClient.setOnReceiveMessageListener({
		// 接收到的消息
		onReceived: function(message) {
			// 判断会话类型
			if(message.conversationType == RongIMLib.ConversationType.CHATROOM) { //聊天室类型
				console.log("ReceiveMessage= " + JSON.stringify(message));
			}

		}
	});
}

function fnSetConnectionStatusListener() {
	// 设置连接监听状态 （ status 标识当前连接状态 ）
	// 连接状态监听器
	RongIMClient.setConnectionStatusListener({
		onChanged: function(status) {
			switch(status) {
				case RongIMLib.ConnectionStatus.CONNECTED:
					console.log('连接成功');
					break;
				case RongIMLib.ConnectionStatus.CONNECTING:
					console.log('正在连接');
					break;
				case RongIMLib.ConnectionStatus.DISCONNECTED:
					console.log('断开连接');
					break;
				case RongIMLib.ConnectionStatus.KICKED_OFFLINE_BY_OTHER_CLIENT:
					console.log('其他设备登录');
					alert('其他设备登录');
					break;
				case RongIMLib.ConnectionStatus.DOMAIN_INCORRECT:
					console.log('域名不正确');
					break;
				case RongIMLib.ConnectionStatus.NETWORK_UNAVAILABLE:
					console.log('网络不可用');
					
					break;
			}
		}
	});
}

function getUserBalance() {
	var auth = {
		"loginType": "1",
		"app_key": "123456",
		"imei": "444012",
		"os": "Iphone os",
		"os_version": "5.0",
		"app_version": "1.0.0",
		"source_id": "Yek_test",
		"ver": "0.9",
		"uid": userId,
		"crc": "3e64055bf4056d1dc68b85dd4365d649",
		"time_stamp": "20090310113016"
	}
	var info = {
		"uid": userId
	}
	$.ajax({
		type: "Post",
		url: config().base_url + "/ajax/AppGateway.ashx",
		data: {
			"opt": 3,
			"auth": JSON.stringify(auth),
			"info": JSON.stringify(info)
		},
		cache: false,
		async: false,
		success: function(data) {
			console.log(data)
			if(data.error == '0') {
				balance = Number(data.balance)
			} else {
				alert(data.msg)
			}
		},
		error: function() {
			//alert("调用错误")
			console.log('调用错误')
		}
	})
}

function getUserHandsel() {
	var auth = {
		"loginType": "1",
		"app_key": "123456",
		"imei": "444012",
		"os": "Iphone os",
		"os_version": "5.0",
		"app_version": "1.0.0",
		"source_id": "Yek_test",
		"ver": "0.9",
		"uid": userId,
		"crc": "3e64055bf4056d1dc68b85dd4365d649",
		"time_stamp": "20090310113016"
	}
	var info = {
		"uid": userId
	}
	$.ajax({
		type: "Post",
		url: config().base_url + "/ajax/AppGateway.ashx",
		data: {
			"opt": 68,
			"auth": JSON.stringify(auth),
			"info": JSON.stringify(info)
		},
		cache: false,
		async: false,
		success: function(data) {
			console.log(data)
			handsel = Number(data.Handsel)
		},
		error: function() {
			//alert("调用错误")
			console.log('调用错误')
		}
	})
}

function getRandomNumber(minValue, maxValue){
	var numberArray = []
	for(var i=0; i<12; i++){
		var num = Math.floor(Math.random() * minValue + maxValue)
		numberArray.push(num)
	}
	return numberArray
}

function fnRandomStr() {
	var numArrayLength = Math.floor(Math.random() * 10 + 1);
	var letter = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'];
	var str = '';
	for(var i = 0; i < numArrayLength; i++) {
		str += Math.floor(Math.random() * 10);
		var letterRanLength = Math.floor(Math.random() * 2 + 1);
		for(j = 0; j < letterRanLength; j++) {
			str += letter[Math.floor(Math.random() * 26)];
		}
	}
	return str;
}
//初始化
// appkey:官网注册的appkey。
// dataAccessProvider:自定义本地存储方案的实例，不传默认为内存存储，自定义需要实现WebSQLDataProvider所有的方法，此参数必须是传入实例后的对象。
// RongIMClient.init(appkey, [dataAccessProvider],[options]);
// 此种方式应用比较广泛，非特殊情况使用此种方式即可。
RongIMClient.init(config().rong_appkey);

$(function() {

	fnGetToken_rong()
})

function fnGetToken_rong() {
	var obj = {
		"userId": userId,
		"name": nickname,
		"portraitUri": avatar_url
	}
	var objStr = JSON.stringify(obj)
	$.ajax({
		type: "Post",
		url: config().base_url + "/ajax/RongYunHandler.ashx",
		data: {
			rongUser: objStr
		},
		cache: false,
		async: false,
		success: function(data) {
			console.log(data)
			if(data.status == 'success') {
				var token = data.msg
				fnSetConnectionStatusListener()
				fnSetOnReceiveMessageListener()
				fnConnect(token)
			}
		},
		error: function() {
			//showAlertAtOneBtn("调用错误")
			console.log('调用错误')
		}
	})
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
			console.log("roomId= " + roomId)

			fnJoinGroup()
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

function fnSetOnReceiveMessageListener() {
	// 消息监听器
	RongIMClient.setOnReceiveMessageListener({
		// 接收到的消息
		onReceived: function(message) {
			// 判断会话类型
			if(message.conversationType == RongIMLib.ConversationType.GROUP) { //群组类型
				console.log("ReceiveMessage= " + JSON.stringify(message));
				if(message.targetId == roomId) { //本策略下只接收到当前的群组消息
					var extraObj = JSON.parse(message.content.extra)
					var type = extraObj.type
					if(type == 0 && message.senderUserId != userId) {
						type = 1
					}
					var open_num = ''
					if(type == 5) {
						if(extraObj.periods != (periods_current - 1)) {
							return
						}
						var num1 = parseInt(extraObj.open_result.substring(0, 1))
						var num2 = parseInt(extraObj.open_result.substring(1, 2))
						var num3 = parseInt(extraObj.open_result.substring(2, 3))
						var sum = num1 + num2 + num3
						open_num = num1 + '+' + num2 + '+' + num3 + '=' + sum
						var open_type = getOpenType(sum)
						var previous_array = [{
							periods: extraObj.periods,
							open_result: open_num + open_type
						}]
						//设置往期开奖列表
						fnSetPrevious_openResult(previous_array)
						//获取一次用户信息
						getUserInfo(true)
						getUserHandsel()
					}else{
						if(extraObj.periods != periods_current) {
							return
						}
					}

					var msgObj = {
						type: type,
						time: format_02(message.sentTime),
						senderUserId: message.senderUserId,
						nickname: extraObj.nickname,
						avatar: extraObj.avatar,
						periods: extraObj.periods,
						betType: extraObj.betType,
						money: extraObj.money,
						open_result: open_num,
						welcome_user: extraObj.welcome_user
					}
					var msg_array = [msgObj]
					fnSetMsgToItem(msg_array)
				}
			}
		}
	});
}

function getOpenType(sum) {
	var size = ''
	var odd = ''
	if(sum >= 14 && sum <= 21) {
		size = '大'
	} else if(sum <= 13 && sum >= 6) {
		size = '小'
	} else if(sum >= 22) {
		size = '极大'
	} else if(sum <= 5) {
		size = '极小'
	}
	if(sum % 2 != 0) {
		odd = '单'
	} else {
		odd = '双'
	}
	return '(' + size + '、' + odd + ')'
}

function fnSetMsgToItem(msg_array) {
	// 1. 编译模板函数
	var tempFn = doT.template(document.getElementById("msg_temp").innerHTML)
	// 2. 多次使用模板函数
	var resultText = tempFn(msg_array)
	$("#list").before(resultText)

	$('.layout-body').scrollTop($('.layout-body')[0].scrollHeight)
	//$('.layout-body').scrollTop($('.layout-body').height())
}

function fnSetPrevious_openResult(data_array) {
	var new_data_array = []
	if(data_array.length > 1) {
		var count = 0
		for(var i = data_array.length - 1; i >= 0; i--) {
			if(i == data_array.length - 1) {
				$('#periods_previous').html(data_array[i].periods)
				$('#openResult_previous').html(data_array[i].open_result)
			} else {
				count++
				if(count <= 9) {
					var obj = {
						periods: data_array[i].periods,
						open_result: data_array[i].open_result
					}
					new_data_array.push(obj)
				}
			}
		}
	} else if(data_array.length == 1) {
		var obj = {
			periods: $('#periods_previous').html(),
			open_result: $('#openResult_previous').html()
		}
		new_data_array.push(obj)
		$('#periods_previous').html(data_array[0].periods)
		$('#openResult_previous').html(data_array[0].open_result)
	} else {
		return
	}
	// 1. 编译模板函数
	var tempFn = doT.template(document.getElementById("previous_list").innerHTML)
	// 2. 多次使用模板函数
	var resultText = tempFn(new_data_array)
	$("#previous_list_base").after(resultText)
	//移除超过10条的列表
	var li_array = $('#previous_ul').find('li')
	if(li_array.length > 10) {
		for(var i = 0; i < li_array.length; i++) {
			if(i > 9) {
				li_array[i].remove()
			}
		}
	}
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

function fnJoinGroup() {
	var groupId = roomId // 群 Id 。
	var groupName = "VIP房间" + (parseInt(myStorage.getItem('roomIndex')) + 1) // 群名称 。
	RongIMClient.getInstance().joinGroup(groupId, groupName, {
		onSuccess: function() {
			// 加入群成功。
			console.log('加入群成功')

			fnGetHistoryInfo() // 获取云存储的历史消息
		},
		onError: function(error) {
			// error => 加入群失败错误码。
			console.log(error)
		}
	});
}

function fnGetHistoryInfo() {
	var conversationType = RongIMLib.ConversationType.GROUP //群组,其他会话选择相应的消息类型即可。
	var targetId = roomId // 想获取自己和谁的历史消息，targetId 赋值为对方的 Id。
	var timestrap = null; // 默认传 null，若从头开始获取历史消息，请赋值为 0 ,timestrap = 0;
	var count = 5; // 每次获取的历史消息条数，范围 0-20 条，可以多次获取。
	RongIMLib.RongIMClient.getInstance().getHistoryMessages(conversationType, targetId, timestrap, count, {
		onSuccess: function(list, hasMsg) {
			// list => Message 数组。
			// hasMsg => 是否还有历史消息可以获取。
			console.log(list)
			console.log(hasMsg)
			if(list.length > 0) {
				var msg_array = []
				for(var i = 0; i < list.length; i++) {
					var obj = list[i]
					var extraObj = JSON.parse(obj.content.extra)
					var type = extraObj.type
					if(extraObj.periods != periods_current) {
						continue
					}
					if(type == 0 && obj.senderUserId != userId) {
						type = 1
					}
					var open_num = ''
					if(type == 5) {
						var num1 = parseInt(extraObj.open_result.substring(0, 1))
						var num2 = parseInt(extraObj.open_result.substring(1, 2))
						var num3 = parseInt(extraObj.open_result.substring(2, 3))
						var sum = num1 + num2 + num3
						open_num = num1 + '+' + num2 + '+' + num3 + '=' + sum
					} else if(type == 6) {
						continue
					}
					var msgObj = {
						type: type,
						time: format_02(obj.sentTime),
						senderUserId: obj.senderUserId,
						nickname: extraObj.nickname,
						avatar: extraObj.avatar,
						periods: extraObj.periods,
						betType: extraObj.betType,
						money: extraObj.money,
						open_result: open_num,
						welcome_user: extraObj.welcome_user
					}
					msg_array.push(msgObj)
				}
				fnSetMsgToItem(msg_array)
			}
			//发送当前用户进入房间的消息
			var msgObj = {
				type: 6,
				nickname: nickname,
				avatar: avatar_url,
				periods: periods_current,
				betType: '',
				money: 0,
				open_result: '',
				welcome_user: nickname
			}
			sendMessage_room(msgObj)
		},
		onError: function(error) {
			console.log("GetHistoryMessages,errorcode:" + error);
		}
	});
}

function sendMessage_room(obj) {
	var msg = new RongIMLib.TextMessage({
		content: '',
		extra: JSON.stringify(obj)
	});
	var conversationtype = RongIMLib.ConversationType.GROUP // 群组,其他会话选择相应的消息类型即可。
	var targetId = roomId // 目标 Id
	RongIMClient.getInstance().sendMessage(conversationtype, targetId, msg, {
		onSuccess: function(message) {
			//message 为发送的消息对象并且包含服务器返回的消息唯一Id和发送消息时间戳
			console.log("SendText successfully")
			console.log(JSON.stringify(message))
			var extraObj = JSON.parse(message.content.extra)
			var msgObj = {
				type: extraObj.type,
				time: format_02(message.sentTime),
				senderUserId: message.senderUserId,
				nickname: extraObj.nickname,
				avatar: extraObj.avatar,
				periods: extraObj.periods,
				betType: extraObj.betType,
				money: extraObj.money,
				open_result: extraObj.open_result,
				welcome_user: extraObj.welcome_user
			}
			var msg_array = [msgObj]
			fnSetMsgToItem(msg_array)
		},
		onError: function(errorCode, message) {
			var info = '';
			switch(errorCode) {
				case RongIMLib.ErrorCode.TIMEOUT:
					info = '超时'
					break;
				case RongIMLib.ErrorCode.UNKNOWN_ERROR:
					info = '未知错误'
					break;
				case RongIMLib.ErrorCode.REJECTED_BY_BLACKLIST:
					info = '在黑名单中，无法向对方发送消息'
					break;
				case RongIMLib.ErrorCode.NOT_IN_DISCUSSION:
					info = '不在讨论组中'
					break;
				case RongIMLib.ErrorCode.NOT_IN_GROUP:
					info = '不在群组中'
					break;
				case RongIMLib.ErrorCode.NOT_IN_CHATROOM:
					info = '不在聊天室中'
					break;
				default:
					info = message
					break;
			}
			console.log('发送失败:' + info)
		}
	});

}
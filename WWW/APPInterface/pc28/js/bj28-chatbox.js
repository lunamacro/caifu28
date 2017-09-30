var bet_type,
	periods_current = 0, //当前期期号
	periods_next = 0, //下一期期号
	isEnd = false, //当前期是否处于已封盘
	isEnding = false, // 当前期是否处于封盘中
	isSendSaleStopMsg = false, //是否已发送停售消息
	isFirstLoad = true, //是否为第一次加载页面
	weight_scale_json = {} //保存投注选择框各个投注类型的数据

var countdown = 0,
	interval_obj

var last_sumMoney = 0, //上一期投注总金额
	current_sumMoney = 0, //当前期投注总金额
	handsel //彩金

/**********参数列表**********/
var buyParameter = {
	LotteryID: 99, //彩种ID
	IsuseID: "", //期号ID
	IsuseEndTime: "", //结束时间
	IsCurrIsuseEndTime: true, //本期是否已结束
	PlayTypeID: 9901, //玩法ID
	TotalShare: 1, //总份数
	BuyShare: 1, //认购份数
	SchemeContent: 0, //方案内容
	SecrecyLevel: 0, //方案是否保密
	SumMoney: 0, //方案总金额
	SumNum: 1, //注数
	Multiple: 1, //倍数
	SchemeBonusScale: 0, //方案佣金
	IsChase: 0, //是否追号(0-代购1-追号)
	ChaseContent: "", //追号内容格式：期号ID,倍数,金额;
	ChaseSumMoney: 0, //追号任务总金额，包括所有的期数
	Title: "",
	Description: "",
	AutoStopAtWinMoney: 0.00, //中奖后停止
	CoBuy: 0, //发起合买
	Cobuy: 1,
	AssureMoney: 0.00, //保底金额
	IsNullBuyContent: 0,
	IsuseTimeEnd: "", //本期最后时间
	HomeIndex: 1,
	VIPIndex: 5
}

var userId,
	avatar_url,
	nickname,
	roomId,
	hallId
	
var minBetMoney, // 单注最小投注金额
	maxBetMoney, // 单注最大投注金额
	maxBetSumMoney // 当期最大投注金额

$(function() {
	userId = myStorage.getItem('userId')
	nickname = myStorage.getItem('nickname')
	avatar_url = myStorage.getItem('avatar_url')
	roomId = myStorage.getItem('roomId')
	hallId = myStorage.getItem('hallIndex')
	if(hallId == 0) {
		minBetMoney = 10
		maxBetMoney = 20000
		maxBetSumMoney = 80000
	} else if(hallId == 1) {
		minBetMoney = 500
		maxBetMoney = 200000
		maxBetSumMoney = 1000000
	} else if(hallId == 2) {
		minBetMoney = 10
		maxBetMoney = 30000
		maxBetSumMoney = 100000
	}
	
	//切换表情主题  
	$('.exp_hd_item').click(function() {
		var _this = $(this),
			i = _this.data('i')
		$('.exp_hd_item').removeClass('active')
		_this.addClass('active')

		addSelectItem(i)
	})

	$("#btn_send").click(function() {
		if(isEnd) {
			showAlertAtTwoBtn('已封盘，停止下注')
		} else if(LotteryTimer.TitleContent == "离下期投注时间还有：") {
			showAlertAtOneBtn('封盘中，不能投注')
		} else {
			// 显示投注区域
			fnShowBetArea()
			addSelectItem('0')
		}
	})

	$("#btn_bet").click(function() {
		var number = Number($('#editArea_number').val())
		var balance = Number($('#balance').text())
		if((balance + handsel) < number) {
			showAlertAtOneBtn('余额不足，请充值后再投注！')
		} else {
			buyParameter.SumMoney = number
			buyParameter.SchemeContent = bet_type + '|' + buyParameter.PlayTypeID + '|' + buyParameter.SumMoney + '|1'
			buyParameter.HomeIndex = hallId
			console.log(buyParameter)
			//提交投注
			postBet(number)
		}
		$('#editArea_number').val('')
		//隐藏投注区域
		fnHideBetArea()
	})

	$('#btn_min').click(function() {
		$('#editArea_number').val(minBetMoney)
	})

	$('#btn_double').click(function() {
		var number = $('#editArea_number').val()
		if(number == '' || number == 0) {
			showAlertAtOneBtn('请填写投注金额')
		} else {
			var money = number * 2
			if(money >= maxBetMoney) {
				money = maxBetMoney
			}
			$('#editArea_number').val(money)
		}
	})

	//getLotteryInfo()
	LotteryTimer.GetCurrentNo(true);
	LotteryTimer.AutoOpenInfoId = setTimeout("LotteryTimer.GetOpenInfo(true)", 1000); //延时加载开奖信息

	getPreviousOpen()

	getScale()

	getUserInfo(false)
	getUserHandsel()
})

function getUserInfo(isFromReceiveMsg) {
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
				if(isFromReceiveMsg) {
					var open_balance = Number(data.balance)
					var current_balance = Number($('#balance').text())
					console.log("open_balance=" + open_balance)
					console.log("current_balance=" + current_balance)
					console.log("last_sumMoney=" + last_sumMoney)
					var profit_and_loss = open_balance - current_balance
					if(profit_and_loss > 0) {
						showTips('上期赢 +' + profit_and_loss, 'green', 3000)
					} else if(last_sumMoney > 0) {
						showTips('上期亏 ' + last_sumMoney, 'red', 3000)
					}
				}
				$('#balance').text(data.balance)
			} else {
				showAlertAtOneBtn(data.msg)
			}
		},
		error: function() {
			//showAlertAtOneBtn('调用错误')
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

function postBet(money) {
	if(money == '') {
		showAlertAtOneBtn('请填写投注金额')
		return
	}
	if(money < minBetMoney) {
		showAlertAtOneBtn('单笔投注金额不能小于' + minBetMoney + '元宝')
		$('#editArea_number').val('')
		return
	}
	if(money > maxBetMoney) {
		showAlertAtOneBtn('单笔投注金额不能超过' + maxBetMoney + '元宝')
		$('#editArea_number').val('')
		return
	}
	var sumMoney = current_sumMoney + money
	if(sumMoney > maxBetSumMoney) {
		showAlertAtOneBtn('单期投注总金额不能超过' + maxBetSumMoney + '元宝')
		$('#editArea_number').val('')
		return
	}
	
	$('#btn_bet').attr('disabled', true)
	
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
	$.ajax({
		type: "Post",
		url: config().base_url + "/ajax/ApiBuy.ashx",
		data: {
			"opt": 11,
			"auth": JSON.stringify(auth),
			"info": JSON.stringify(buyParameter)
		},
		cache: false,
		async: false,
		success: function(data) {
			console.log(data)
			if(data.status == 'success') {
				current_sumMoney = current_sumMoney + buyParameter.SumMoney
				getUserInfo(false)
				getUserHandsel()
				//发送投注消息
				var msgObj = {
					type: 0,
					nickname: nickname,
					avatar: avatar_url,
					periods: periods_current,
					betType: bet_type,
					money: buyParameter.SumMoney,
					open_result: '',
					welcome_user: ''
				}
				sendMessage_room(msgObj)
				
				$('#btn_bet').attr('disabled', false)
			} else {
				showAlertAtOneBtn(data.msg)
			}
		},
		error: function() {
			//showAlertAtOneBtn("调用错误")
			console.log('调用错误')
		}
	})
}

function getScale() {
	$.ajax({
		type: "Post",
		url: config().base_url + "/ajax/Bjxy28.ashx",
		data: {
			"opt": 2,
			"lotteryId": 99,
			"hall": hallId
		},
		cache: false,
		async: false,
		success: function(data) {
			console.log(data)
			var previous_array = []
			if(data.error == '0') {
				if(data.msg.length > 0) {
					for(var i = 0; i < data.msg.length; i++) {
						var name = data.msg[i].name
						var nameArray = name.split(',')
						if(nameArray.length > 0) {
							for(var j = 0; j < nameArray.length; j++) {
								weight_scale_json[nameArray[j]] = data.msg[i].defaultMoney
							}
						} else {
							weight_scale_json[name] = data.msg[i].defaultMoney
						}
					}
					//console.log(weight_scale_json)
				}
			}
		},
		error: function() {
			//showAlertAtOneBtn("调用错误")
			console.log('调用错误')
		}
	})
}

function getPreviousOpen() {
	$.ajax({
		type: "Post",
		url: config().base_url + "/ajax/Bjxy28.ashx",
		data: {
			"opt": 1,
			"lotteryId": 99
		},
		cache: false,
		async: false,
		success: function(data) {
			console.log(data)
			var previous_array = []
			if(data.error == '0') {
				if(data.msg.length > 0) {
					for(var i = data.msg.length - 1; i >= 0; i--) {
						var open_type = '(' + data.msg[i].type1 + '、' + data.msg[i].type2 + ')'
						var obj = {
							periods: data.msg[i].name,
							open_result: data.msg[i].winLotteryNumber + open_type
						}
						previous_array.push(obj)
					}
				}
				//设置往期开奖列表
				fnSetPrevious_openResult(previous_array)
			}
		},
		error: function() {
			//showAlertAtOneBtn("调用错误")
			console.log('调用错误')
		}
	})
}

function getLotteryInfo() {
	$.ajax({
		type: "post",
		url: config().base_url + "/ajax/DefaultPageIssue_ChaseInfo.ashx",
		data: {
			lotteryId: 99
		},
		cache: false,
		async: true, //设置为异步获取
		dataType: "text",
		success: function(data) {
			console.log(data)
			var nextIssueMsg = data.substring(0, data.indexOf('|'))

			if(nextIssueMsg.length > 0) {
				var msgArray = nextIssueMsg.split(',')
				buyParameter.IsuseID = Number(msgArray[0])
				periods_current = parseInt(msgArray[1])
				periods_next = periods_current + 1
				var nextEndTime = msgArray[4]
				buyParameter.IsuseEndTime = nextEndTime
				if(nextEndTime == '') {
					isEnd = true
					$("#count_down").html('数据出错！')
					$('#periods_count_down').html(periods_current)
					return
				}

				$('#periods_count_down').html(periods_current)

				var nowTime = moment()
				var endBetTime = moment(msgArray[2])
				var endTime = moment(nextEndTime)
				var difference_time = endTime.unix() - nowTime.unix()
				end_time = endTime.unix() - endBetTime.unix()
				console.log('end_time=' + end_time)
				console.log('nowTime.unix()=' + nowTime.unix())
				console.log('endTime.unix()=' + endTime.unix())
				console.log('difference_time=' + difference_time)

				if(difference_time <= end_time && difference_time >= 0) {
					endingTime = difference_time
					countdown = 0
				} else {
					endingTime = end_time
					countdown = difference_time - end_time
				}

				setTimer()
			}
		}
	})
}

function getLotteryInfoForName() {
	$.ajax({
		type: "post",
		url: config().base_url + "/ajax/DefaultPageIssue_ChaseInfo.ashx",
		data: {
			lotteryId: 99,
			name: periods_next
		},
		cache: false,
		async: true, //设置为异步获取
		dataType: "text",
		success: function(data) {
			console.log(data)

			var nextIssueMsg = data.substring(0, data.indexOf('|'))
			if(nextIssueMsg.length > 0) {
				var msgArray = nextIssueMsg.split(',')
				var nextEndTime = msgArray[2]
				if(nextEndTime == '') {
					isEnd = true
					$("#count_down").html('数据出错！')
					$('#periods_count_down').html(periods_current)
					return
				}

				var nowTime = moment()
				var endTime = moment(nextEndTime)
				countdown = endTime.unix() - nowTime.unix()
				console.log('剩余时间=' + countdown)
				if(countdown > 0) {
					buyParameter.IsuseID = msgArray[0]
					periods_current = parseInt(msgArray[1])
					buyParameter.IsuseEndTime = nextEndTime
					$('#periods_count_down').html(periods_current)
					periods_next = periods_current + 1
				}
				if(countdown > 0 && countdown <= (300 - end_time)) {
					//发送开盘系统消息
					fnSetSystemMsg(4, periods_current, '')
					$('#periods_count_down').html(periods_current)
				}
				// 最后设置计时器
				setTimer()
			}
		}
	})
}

var difference_time, // 本地时间与服务器时间的差值
	endingTime = 0,
	end_time, // 封盘时间长度
	intervalTime_end = 0 //已封盘时，每隔10获取一次数据

function setTimer() {
	interval_obj = setInterval(function() {
		if(countdown == 0) {
			if(isEnding) { //封盘中
				endingTime--
				if(endingTime == 0) {
					clearInterval(interval_obj)
					isEnding = false
					endingTime = end_time
					getLotteryInfoForName()
				}
			} else {
				console.log('countdown=' + countdown + ' 封盘中')
				//发送封盘系统消息
				fnSetSystemMsg(3, periods_current, '')
				$("#count_down").html('封盘中')

				last_sumMoney = current_sumMoney
				current_sumMoney = 0
				isEnding = true
			}
		} else {
			if(countdown > 0 && countdown <= (300 - end_time)) {
				countdown--
				isEnd = false
				isEnding = false
				if(countdown <= 60) {
					$("#count_down").html(countdown + '秒')
					if(countdown == 60) {
						//发送离封盘剩余时间的系统消息
						fnSetSystemMsg(2, periods_current, '')
					}
				} else {
					var m = parseInt(countdown / 60)
					var s = parseInt(countdown % 60)
					$("#count_down").html(m + '分' + s + '秒')
				}
			} else {
				isEnd = true
				isEnding = true
				console.log('countdown=' + countdown + '  已封盘')
				$("#count_down").html('已封盘')
				if(!isSendSaleStopMsg) {
					// 发送停售系统信息
					fnSetSystemMsg(7, '', '')
					isSendSaleStopMsg = true
				}
				if(intervalTime_end == 9) {
					intervalTime_end = 0
					clearInterval(interval_obj)
					getLotteryInfo()
				} else {
					intervalTime_end++
				}
			}
		}
	}, 1000)
}

function fnSetSystemMsg(type, periods, open_result) {
	var msgObj = {
		type: type,
		periods: periods,
		open_result: open_result,
		minBetMoney: minBetMoney,
		maxBetMoney: maxBetMoney,
		maxBetSumMoney: maxBetSumMoney
	}
	fnSetMsgToItem([msgObj])
}

function loadDefaultWinSum(index) {
	var winSum
	if(index == '0') {
		winSum = '中奖和值：14 15 16 17 18 19 20 21 22 23 24 25 26 27'
	} else if(index == '1') {
		winSum = '中奖号码：0'
	} else {
		winSum = '中奖和值：3 6 9 12 15 18 21 24'
	}
	$("#win_sum").html(winSum)
}

function loadSelectItemData(index) {
	var data = [],
		weight_array = [],
		scale_array = []
	if(index == '0') {
		weight_array = ['大', '小', '单', '双', '大单', '小单', '大双', '小双', '极大', '极小']
		for(var i = 0; i < weight_array.length; i++) {
			scale_array.push(weight_scale_json[weight_array[i]])
		}
	} else if(index == '1') {
		for(var i = 0; i <= 27; i++) {
			weight_array.push(i + '')
			scale_array.push(weight_scale_json[i])
		}
	} else {
		weight_array = ['红', '绿', '蓝', '豹子']
		for(var i = 0; i < weight_array.length; i++) {
			if(i < weight_array.length - 1) {
				scale_array.push(weight_scale_json['色波'])
			} else {
				scale_array.push(weight_scale_json['豹子'])
			}
		}
	}
	for(var i = 0; i < weight_array.length; i++) {
		var winSum,
			playType
		if(index == '0') {
			winSum = winSum_area0(weight_array[i])
			playType = 9901
		} else if(index == '1') {
			winSum = '中奖号码：' + weight_array[i]
			playType = 9902
		} else {
			winSum = winSum_area2(weight_array[i])
			playType = 9903
		}
		var obj = {
			dataTotal: weight_array.length,
			win_weight: weight_array[i],
			win_scale: scale_array[i],
			winSum: winSum,
			playType: playType
		}
		data.push(obj)
	}

	return data
}

function winSum_area0(win_weight) {
	var win_sum
	if(win_weight == '大') {
		win_sum = '中奖和值：14 15 16 17 18 19 20 21 22 23 24 25 26 27'
	} else if(win_weight == '小') {
		win_sum = '中奖和值：0 1 2 3 4 5 6 7 8 9 10 11 12 13'
	} else if(win_weight == '单') {
		win_sum = '中奖和值：1 3 5 7 9 11 13 15 17 19 21 23 25 27'
	} else if(win_weight == '双') {
		win_sum = '中奖和值：0 2 4 6 8 10 12 14 16 18 20 22 24 26'
	} else if(win_weight == '大单') {
		win_sum = '中奖和值：15 17 19 21 23 25 27'
	} else if(win_weight == '小单') {
		win_sum = '中奖和值：1 3 5 7 9 11 13'
	} else if(win_weight == '大双') {
		win_sum = '中奖和值：14 16 18 20 22 24 26'
	} else if(win_weight == '小双') {
		win_sum = '中奖和值：0 2 4 6 8 10 12'
	} else if(win_weight == '极大') {
		win_sum = '中奖和值：22 23 24 25 26 27'
	} else if(win_weight == '极小') {
		win_sum = '中奖和值：0 1 2 3 4 5'
	}
	return win_sum
}

function winSum_area2(win_weight) {
	var win_sum
	if(win_weight == '红') {
		win_sum = '中奖和值：3 6 9 12 15 18 21 24'
	} else if(win_weight == '绿') {
		win_sum = '中奖和值：1 4 7 10 16 19 22 25'
	} else if(win_weight == '蓝') {
		win_sum = '中奖和值：2 5 8 11 17 20 23 26'
	} else if(win_weight == '豹子') {
		win_sum = '三个数字一致即为中奖'
	}
	return win_sum
}

function addSelectItem(index) {
	var data = loadSelectItemData(index)

	var tempFn = doT.template(document.getElementById("template").innerHTML)
	var resultText = tempFn(data)
	$("#emo_list").html(resultText)

	//默认选中第一个
	$('#weight0').addClass('win_weight')
	$('#item0').addClass('select_highlight')
	loadDefaultWinSum(index)
	bet_type = $('#weight0').html()
	if(index == '0') {
		buyParameter.PlayTypeID = 9901
	} else if(index == '1') {
		buyParameter.PlayTypeID = 9902
	} else {
		buyParameter.PlayTypeID = 9903
	}
}

function fnSelectToggle(obj, dataTotal, winSum, play_type, index) {
	//console.log(obj.id)
	//console.log(dataTotal)
	for(var i = 0; i < dataTotal; i++) {
		$("#item" + i).removeClass('select_highlight')
		$('#weight' + i).removeClass('win_weight')
	}
	$('#weight' + index).addClass('win_weight')
	$("#" + obj.id).addClass('select_highlight')
	if($("#" + obj.id).hasClass('select_highlight')) {
		$("#win_sum").html(winSum)
	}
	bet_type = $('#weight' + index).html()
	buyParameter.PlayTypeID = play_type
}

function fnShowBetArea() {
	$('.bet_area').css("display", "block")
	$('.mask').css('display', 'block');
	$('.mask').click(function() {
		$('#editArea_number').val('')
		$('.bet_area').css("display", "none")
		$('.mask').css('display', 'none');
	})

	//默认加载大小单双区块
	$('.exp_hd_item').removeClass('active')
	$("#area_0").addClass('active')
}

function fnHideBetArea() {
	$('.bet_area').css("display", "none")
	$('.mask').css('display', 'none');
}
/**************彩票投注计时器******************/
var LotteryTimer = {
	MissType: 0, //获取遗漏值参数
	MissR: 0, //获取遗漏值参数

	LocalDate: "", //页面加载时候的本地时间

	ServerDate: "", //页面加载时候的服务器时间

	DefaultSeconds: 0, //初始化间隔秒

	AfterSeconds: 30, //每隔30秒获取服务器时间

	CurrentNo: "", //当前期

	TitleContent: "离下期投注时间还有", //如果有提前截止时间，会切换文本内容（默认为当前值）

	ServerTime: "", //服务器时间(默认为空)

	AdvanceEndTime: "", //提前截止时间

	CurrentEndTime: "", //本期截止时间

	OpenAwardBySeconds: 90, //正常情况下90秒后获取开奖信息

	IsShowTime: false, //是否显示倒计时(默认为False)

	AutoGetIsuseInfoId: null, //自动获取下期的Id值，不为Null时，即正在获取

	AutoOpenInfoId: null, //自动获取开奖公告Id值，不为Null时，即正在获取

	IsEndFlag: true, // 本期是否结束的标志

	Init: function() //初始化数据
	{
		LotteryTimer.LocalDate = new Date(); //初始化本地时间

		//初次加载获取到期号截止时间后,启动计时器
		setInterval(LotteryTimer.StartTimer, 1000);
	},

	StartTimer: function() //启动计时器
	{
		if(LotteryTimer.DefaultSeconds >= LotteryTimer.AfterSeconds) {
			LotteryTimer.ResetLocalDate();
		} else {
			var localNowDate = new Date(); //当前本地时间

			var localT = localNowDate.getTime() - LotteryTimer.LocalDate.getTime(); //本地时间差
			var localS = Math.floor(localT / 1000 % 60); //本地时间差（秒）

			var serverT = LotteryTimer.ServerTime.getTime() - LotteryTimer.ServerDate.getTime(); //服务器时间差

			var serverS = Math.floor(serverT / 1000 % 60); //服务器时间差（秒）

			//校准时间
			if((localS - serverS) > 3) {
				LotteryTimer.ServerTime.setSeconds(LotteryTimer.ServerTime.getSeconds() + (localS - serverS));
			} else {
				LotteryTimer.ServerTime.setSeconds(LotteryTimer.ServerTime.getSeconds() + 1);
			}
		}

		var tempTime = null; //临时时间变量，用来做时间交换处理

		if(LotteryTimer.AdvanceEndTime != "" && LotteryTimer.CurrentEndTime != "" && LotteryTimer.ServerTime != "") {
			//getTime()返回当前时间的毫秒数
			var t = LotteryTimer.CurrentEndTime.getTime() - LotteryTimer.ServerTime.getTime(); //时间差
			var m = Math.floor(t / 1000 / 60 % 60); //时间差（分）
			var s = Math.floor(t / 1000 % 60); //时间差（秒）

			//倒计时小于2秒时开始获取期号信息
			if(LotteryTimer.AutoGetIsuseInfoId == null) {
				if((m * 60 + s) <= 1) //小于2秒时自动获取下期投注信息
				{
					LotteryTimer.ResetLocalDate();

					LotteryTimer.AutoGetIsuseInfoId = setInterval("LotteryTimer.GetCurrentNo(false)", 1000);
				}
			}

			var tt = Math.floor(LotteryTimer.AdvanceEndTime.getTime() - LotteryTimer.ServerTime.getTime()); //当前时间与服务器时间的差值（毫秒）

			if(tt > 0) //还未截止
			{
				buyParameter.IsCurrIsuseEndTime = true; //允许投注

				//允许投注,显示倒计时
				LotteryTimer.TitleContent = "离截止时间还有：";
				LotteryTimer.IsShowTime = true;

				LotteryTimer.IsEndFlag = true
				isEnd = false

				tempTime = new Date(LotteryTimer.AdvanceEndTime.toString());
			} else {
				buyParameter.IsCurrIsuseEndTime = false; //不允许投注

				tt = Math.floor(LotteryTimer.CurrentEndTime.getTime() - LotteryTimer.ServerTime.getTime());

				if(tt > 0) {
					//不允许投注,显示倒计时
					LotteryTimer.TitleContent = "离下期投注时间还有：";
					LotteryTimer.IsShowTime = true;

					if(LotteryTimer.IsEndFlag) {
						//发送封盘系统消息
						fnSetSystemMsg(3, periods_current, '')
						$("#count_down").html('封盘中')
						last_sumMoney = current_sumMoney
						current_sumMoney = 0
						LotteryTimer.IsEndFlag = false
					}

					tempTime = new Date(LotteryTimer.CurrentEndTime.toString());
				} else {
					LotteryTimer.IsShowTime = false;
				}
			}
		} else {
			LotteryTimer.IsShowTime = false;
		}

		if(LotteryTimer.IsShowTime == true) {

			var t = tempTime.getTime() - LotteryTimer.ServerTime.getTime(); //时间差

			//var d = Math.floor(t / 1000 / 60 / 60 / 24); //时间差（天）
			//var h = Math.floor(t / 1000 / 60 / 60 % 24); //时间差（小时）
			var m = Math.floor(t / 1000 / 60 % 60); //时间差（分）
			var s = Math.floor(t / 1000 % 60); //时间差（秒）

			if(LotteryTimer.TitleContent == "离下期投注时间还有：") {
				$("#count_down").html("封盘中")
			} else {
				if(s <= 59 && m <= 0) {
					$("#count_down").html(s + '秒')
					if(s == 59) {
						//发送离封盘剩余时间的系统消息
						fnSetSystemMsg(2, periods_current, '')
					}
				} else {
					$("#count_down").html(m + '分' + s + '秒')
				}
			}
		} else {
			//$("#is-count-down b:eq(2)").html("00");
			//$("#is-count-down b:eq(3)").html("00");
			isEnd = true
		}

		LotteryTimer.DefaultSeconds++;
	},

	GetCurrentNo: function(isLoad) //获取当前期号
	{
		$.ajax({
			type: "post",
			url: config().base_url + "/ajax/DefaultPageIssue_ChaseInfo.ashx",
			data: {
				lotteryId: buyParameter.LotteryID
			},
			cache: false,
			async: true, //设置为异步获取
			dataType: "text",
			success: function(response) {
				console.log(response)
				if(response != null) {
					window.clearInterval(LotteryTimer.AutoGetIsuseInfoId);
					LotteryTimer.AutoGetIsuseInfoId = null;

					if(response.split('|').length > 0 && response.split('|')[0].split(',').length > 4) {
						var arrInfo = response.split('|');
						var currIsuse = arrInfo[0]; //当前期信息
						//var chaseIsuse = arrInfo[1]; //追号信息

						var arrcurrIsuse = currIsuse.split(','); //解析当前期信息

						var currIsuse = Number(arrcurrIsuse[1]); // 当前期期号
						buyParameter.IsuseEndTime = arrcurrIsuse[2]; //当前期截止时间

						$('#periods_count_down').html(currIsuse)
						if(isFirstLoad) {
							isFirstLoad = false
						} else {
							if(currIsuse > periods_current) {
								//发送开盘系统消息
								fnSetSystemMsg(4, currIsuse, '')
								$('#periods_count_down').html(currIsuse)

								$('#div_stopTime').css('display', 'none')
								$('#div_startTime').css('display', 'block')
								$('#div_money').css('display', 'block')
							} else {
								if(buyParameter.LotteryID == 99) {
									if((LotteryTimer.ServerTime.getHours() == 23 && LotteryTimer.ServerTime.getMinutes() >= 55) ||
										LotteryTimer.ServerTime.getHours() < 9) {
										if(!isSendSaleStopMsg) {
											// 发送停售系统信息
											fnSetSystemMsg(7, '', '')
											isSendSaleStopMsg = true
											$('#div_startTime').css('display', 'none')
											$('#div_money').css('display', 'none')
											$('#div_stopTime').css('display', 'block')
										}
									}
								} else if(buyParameter.LotteryID == 98) {
									if(LotteryTimer.ServerTime.getHours() == 19) {
										if(!isSendSaleStopMsg) {
											// 发送停售系统信息
											fnSetSystemMsg(7, '', '')
											isSendSaleStopMsg = true
											$('#div_startTime').css('display', 'none')
											$('#div_money').css('display', 'none')
											$('#div_stopTime').css('display', 'block')
										}
									}
								}
							}
						}
						periods_current = currIsuse

						var tempTime = new Date(buyParameter.IsuseEndTime.replace(new RegExp("-", "g"), "/")); //期截时间

						LotteryTimer.ResetLocalDate();

						try {
							var t = Math.floor(tempTime.getTime() - LotteryTimer.ServerTime.getTime()); //时间差

							if(t < 0 && buyParameter.IsCurrIsuseEndTime == false && buyParameter.IsuseID != "") {
								//启动定时器，每1秒请求一次
								LotteryTimer.AutoGetIsuseInfoId = setInterval("LotteryTimer.GetCurrentNo(false)", 1000);
								return;
							}
						} catch(e) {
							//启动定时器，每1秒请求一次
							LotteryTimer.AutoGetIsuseInfoId = setInterval("LotteryTimer.GetCurrentNo(false)", 1000);
							return;
						}

						//$(".zhuihaoBox").html(chaseIsuse); //显示追号期号信息
						buyParameter.IsuseID = arrcurrIsuse[0]; //期号
						LotteryTimer.AdvanceEndTime = new Date(buyParameter.IsuseEndTime.replace(new RegExp("-", "g"), "/")); //提前截止时间    
						LotteryTimer.CurrentEndTime = new Date(arrcurrIsuse[4].replace(new RegExp("-", "g"), "/")); //本期最后截止时间

						if(isLoad == true) {
							LotteryTimer.Init();
						}

						$(".count-center strong:eq(0)").html(arrcurrIsuse[1]);
						LotteryTimer.CurrentNo = arrcurrIsuse[1].substring(9);
						//chaseIsuseInit(); //添加追号信息

						//如果不是初次加载，90秒后开始获取开奖信息
//						if(isLoad == false && LotteryTimer.AutoOpenInfoId == null) {
//							LotteryTimer.AutoOpenInfoId = setTimeout("LotteryTimer.GetOpenInfo(true)", 1000 * LotteryTimer.OpenAwardBySeconds);
//						}
					}
				} else {
					//启动定时器，每1秒请求一次
					LotteryTimer.AutoGetIsuseInfoId = setInterval("LotteryTimer.GetCurrentNo(false)", 1000);
				}
			},
			error: function(status) {}
		});
	},

	GetAlreadyOpenNo: function() //已开多少期，还剩余多少期
	{
		//      $.ajax({
		//          type: "post",
		//          url: "../ajax/GetTodayIsusesCount.ashx",
		//          data: { lotteryId: Lottery.LotID },
		//          cache: false,
		//          async: true,
		//          dataType: "json",
		//          success: function (result) {
		//              if (result.errCode == 0) {
		//                  $(".count-center p strong:eq(1)").text(result.OpenCount);
		//                  $(".count-center p strong:eq(2)").text(result.Total - result.OpenCount);
		//                  $("#sell-status i").text(result.OpenCount);
		//                  $("#sell-status b").text(result.Total - result.OpenCount);
		//              }
		//          },
		//          error: function (status) {
		//          }
		//      });
	},

	GetOpenInfo: function(isLoad) //获取开奖信息
	{
		//      $.ajax({
		//          type: "post",
		//          url: "../ajax/GetHighFrequencyColor.ashx",
		//          data: { lotteryId: Lottery.LotID },
		//          cache: false,
		//          async: true,
		//          dataType: "json",
		//          success: function (result) {
		//              window.clearInterval(LotteryTimer.AutoOpenInfoId);
		//              LotteryTimer.AutoOpenInfoId = null;
		//              /*
		//              2015-07-31增加，主要是提高用户体验（5.3.4用户体验报告）
		//              */
		//              $("#loadOpenInfoData").remove();
		//              $("#open-list-news tr").show();
		//              if (-103 == result.errCode) {
		//                  $("#open-list-news tr:first").nextAll().hide();
		//                  $("#open-list-news tr:first").nextAll().eq(0).html("<td colspan='2'>暂无开奖记录</td>").show();
		//              }
		//              if (result.errCode == 0) {
		//                  var NewCurrentNo = parseInt(result.NewName.substring(9)); //最新开奖的期号
		//
		//                  if (LotteryTimer.CurrentNo - NewCurrentNo == 1 || isLoad == true) {
		//                      LotteryTimer.GetAlreadyOpenNo();
		//
		//                      if (result.NewNumber != "") {
		//                          $("#news-open-info i:eq(0)").text(result.NewName);
		//                          $("#news-open-info i:eq(1)").text(result.NewDate);
		//                          $("#news-open-number b:eq(0)").text(result.NewNumber.substring(0, 1));
		//                          $("#news-open-number b:eq(1)").text(result.NewNumber.substring(1, 2));
		//                          $("#news-open-number b:eq(2)").text(result.NewNumber.substring(2, 3));
		//                          $("#news-open-number b:eq(3)").text(result.SumNumber);
		//                      }
		//
		//                      var count = 0;
		//
		//                      result.OpenInfo.sort(function (a, b) { return (a.ID < b.ID) ? 1 : -1 });
		//                      $.each(result.OpenInfo, function (i, d) {
		//                          if (d.WinLotteryNumber != "" && count < 10) {
		//                              if (d.WinLotteryNumber.length > 10) {
		//                                  $("#open-list-news tr").eq(count + 1).find("td:eq(1)").text("- -");
		//                              }
		//                              else {
		//                                  var winNum = d.WinLotteryNumber;
		//
		//                                  var num1 = Number(winNum.substring(0, 1));
		//                                  var num2 = Number(winNum.substring(1, 2));
		//                                  var num3 = Number(winNum.substring(2));
		//
		//                                  var sumNum = num1 + num2 + num3;
		//                                  var showWinNumber = '' + num1 + '+' + num2 + '+' + num3 + '=' + sumNum;
		//                                  $("#open-list-news tr").eq(count + 1).find("td:eq(0)").text(d.Name);
		//                                  $("#open-list-news tr").eq(count + 1).find("td:eq(1)").html(showWinNumber);
		//                                  $("#open-list-news tr").eq(count + 1).find("td:eq(2)").text(d.AfterThree);
		//                              }
		//                              count++;
		//                          }
		//                      });
		//
		//                      
		//                  }
		//                  if (LotteryTimer.CurrentNo - NewCurrentNo != 1) {
		//
		//                      //继续获取开奖信息
		//                      LotteryTimer.AutoOpenInfoId = setTimeout("LotteryTimer.GetOpenInfo(true)", 10000);
		//                  }
		//              }
		//              else if (result.errCode == -102)//数据获取异常，重新获取
		//              {
		//                  //继续获取开奖信息
		//                  LotteryTimer.AutoOpenInfoId = setTimeout("LotteryTimer.GetOpenInfo(true)", 10000);
		//              }
		//          },
		//          error: function (state) {
		//          }
		//      });
	},

	GetServerTime: function() //获取服务器时间
	{
		$.ajax({
			type: "post",
			url: config().base_url + "/ajax/getServerTime.ashx",
			cache: false,
			async: false, //使用同步去获取时间，True为异步
			dataType: "text",
			success: function(result) {
				LotteryTimer.ServerDate = new Date(result.replace(new RegExp("-", "g"), "/"));
				LotteryTimer.ServerTime = new Date(result.replace(new RegExp("-", "g"), "/")); //服务器时间
			},
			error: function(status) {
				LotteryTimer.GetServerTime();
			}
		});
	},

	ResetLocalDate: function() {
		LotteryTimer.LocalDate = new Date(); //重置当前存储的本地时间

		//超过指定时间后，获取一次服务器时间
		LotteryTimer.DefaultSeconds = 0;
		LotteryTimer.GetServerTime();
	}

};
/**********扩展方法**********/
Array.prototype.contains = function (obj) {
    var i = this.length;
    while (i--) {
        if (this[i] == obj) {
            return true;
        }
    }
    return false;
}

/**********参数列表**********/
var buyParameter = {
    LotteryID: 99, //彩种ID
    IsuseID: "", //期号ID
    IsuseEndTime: "", //结束时间
    IsCurrIsuseEndTime: false, //本期是否已结束
    PlayTypeID: 9901, //玩法ID
    TotalShare: 0, //总份数
    BuyShare: 0, //认购份数
    SchemeContent: 0, //方案内容
    SecrecyLevel: 0, //方案是否保密
    SumMoney: 0, //方案总金额
    SumNum: 0, //注数
    Multiple: 0, //倍数
    SchemeBonusScale: 0, //方案佣金
    IsChase: 0, //是否追号(0-代购1-追号)
    ChaseContent: "", //追号内容格式：期号ID,倍数,金额;
    ChaseSumMoney: 0.00, //追号任务总金额，包括所有的期数
    Title: "",
    Description: "",
    AutoStopAtWinMoney: 0.00, //中奖后停止
    CoBuy: 0, //发起合买
    AssureMoney: 0, //保底金额
    IsNullBuyContent: 0,
    IsuseTimeEnd: "",//本期最后时间
    HomeIndex: "",
    VIPIndex: ""
};

/**********投注方法**********/
var Lottery =
{
    LotID: 0,  //采种编号
    LotName: "",  //彩中名称
    LotTypeID: 9901,  //彩种分类，1 乐透型（分前后区，如双色球、大乐透） 2 乐透型（单区，如22选5 36选7） 3 排列型（如：3D、排3/5、七星彩、时时彩等）
    PlayTypeID: 0,  //玩法编号
    PlayTypeName: "",  //玩法名称
    PlayTypeForRegex: "", //玩法正则表达式
    RedBallCount: 0,    //红球总个数
    MaxDBallCount: 0,    //胆码最大个数
    BlueBallCount: 0,   //篮球个总数
    TotalMoney: 0,  //总金额
    TotalMultiple: 1, //总倍数
    MaxAllowInvestMultiple: 999999, //允许投注的最大倍数,默认999倍
    TotalInvestNum: 0, //总注数
    Price: 2,
    LotteryNumber: "", //投注号码
    BetContentList: [], //投注内容集合
    DaXiaoContentList: ["大", "小", "单", "双", "大单", "大双", "小单", "小双", "极大", "极小"],
    TeShuContentList: ["豹子", "红", "绿", "蓝"],
    MissingValues: "", //遗漏值，Json字符串
    PlayTab: 0,
    SelectButton: function () { //选号按钮 
        //选球按钮
        $(".balls li b,.balls li a,.balls li em b").click(function () {

            Lottery.ClearSelectAll();

            if (Lottery.BallStatus($(this))) {

                $(this).removeClass("active");

            } else {
                $(this).addClass("active");

                if ($(this).prop("tagName") == "A") {
                    Lottery.LotteryNumber = $.trim($(this).find("span").text());
                } else {
                    Lottery.LotteryNumber = $.trim($(this).text());
                }

                if ($.inArray(Lottery.LotteryNumber, Lottery.DaXiaoContentList) >= 0) {
                    Lottery.PlayTypeName = "大小单双";
                    Lottery.PlayTypeID = "9901";
                    buyParameter.PlayTypeID = "9901";
                } else if ($.inArray(Lottery.LotteryNumber, Lottery.TeShuContentList) >= 0) {
                    Lottery.PlayTypeName = "特殊玩法";
                    Lottery.PlayTypeID = "9903";
                    buyParameter.PlayTypeID = "9903";

                } else {
                    Lottery.PlayTypeName = "猜数字";
                    Lottery.PlayTypeID = "9902";
                    buyParameter.PlayTypeID = "9902";
                }
            }

            Lottery.SetSelectInfo();
        });
    },
    ClearSelectAll: function () {
        $(".balls li").find("b").removeClass("active");
        $(".balls li").find("a").removeClass("active");
    },
    ClearInvestList: function () {//清空投注列表
        CodeArea.CodeList = [];
        CodeArea.ShowCodeArea();
    },
    BallStatus: function (obj) {//是否为选中状态
        return obj.hasClass("active");
    },
    MustBeDigitKey: function () { //文本框输入只能是数字按键
        if (window.event.keyCode < 48 || window.event.keyCode > 57)
            return false;
        return true;
    },
    ValidateInputMultiple: function (obj) { //检查倍数的输入是否正确
        if (parseInt($(obj).val(), 10) < 1 || parseInt($(obj).val(), 10) > this.MaxAllowInvestMultiple) {
            alert("必须为小于或者等于" + this.MaxAllowInvestMultiple + "的正整数");
            $(obj).val(5);
        }
    },
    SetSelectInfo: function () { //设置选号信息
        var investNum = this.GetLotteryInvestNum();
        var betMonery = Number($(".betmoney-ipt").val()) | 0;
        if (investNum == 0) betMonery = 0;
        $(".cp-affirm p b:eq(0)").text(investNum);
        $(".cp-affirm p b:eq(1)").text(betMonery);
        this.Price = betMonery;
    },
    ShowBetresultInfo: function () { //显示投注信息
        $(".buy-number-1 b:eq(0)").text(this.TotalInvestNum);
        $(".buy-number-1 b:eq(1)").text(this.TotalMoney);
    },
    MustBeLegalKey: function () { //单式上传，按键控制，必须是数字键及合法的符号键
        if (window.event.keyCode == 43 || window.event.keyCode == 44 || window.event.keyCode == 124 || window.event.keyCode == 61 || window.event.keyCode == 32 || window.event.keyCode == 40 || window.event.keyCode == 41 || window.event.keyCode == 13 || window.event.keyCode == 45)
            return true;
        if (window.event.keyCode < 48 || window.event.keyCode > 57)
            return false;
        return true;
    },
    Combination: function (m, n) { //算组合数，m中取n
        if (m == 0 || m < n) return 0;
        var investNum = 1;
        for (var i = m; i > n; i--) {
            investNum *= i;
        }
        var f = 1;
        for (var i = m - n; i > 0; i--) {
            f *= i;
        }

        return investNum /= f;
    },
    //计算总金额、总
    CalcSumMoney: function () {
        Lottery.TotalMoney = 0;
        Lottery.TotalInvestNum = 0;
        $.each(CodeArea.CodeList, function (i, d) {
            Lottery.TotalMoney += d.Money;
            Lottery.TotalInvestNum += d.InvestNum;
        });
        Lottery.UpdateShare(); //更新合买信息
    },
    //计算注数
    GetLotteryInvestNum: function () {
        var investNum = Lottery.GetSelectedBallCount($(".balls li"));
        return investNum;
    },
    GetSelectedBallCount: function (obj) { //取选中球的个数
        return obj.find("b.active").length | obj.find("a.active").length;
    },
    GetSelectedBallText: function (obj, splitChar) { //取选中球的号码
        if (splitChar == undefined)
            return "Please enter the char for split the number";

        var temp = "";
        $(obj).find("b.active").each(function () {
            temp += $.trim($(this).text()) + splitChar; //按分割符合拼接号码
        });
        if (temp.substring(temp.length - 1, temp.length) == splitChar && splitChar != "")
            temp = temp.substring(0, temp.length - 1);

        return $.trim(temp);
    },
    GetLotteryNumber: function () { //取投注内容
        var tmpLotteryNumber = Lottery.LotteryNumber;
        this.LotteryNumber = $.trim(tmpLotteryNumber);
    },
    CastBasket: function () { //将号码投到篮子中
        var investNum = this.GetLotteryInvestNum(); //注数
        this.GetLotteryNumber();

        CodeArea.PlayName = this.PlayTypeName;
        CodeArea.InvestNum = investNum;
        CodeArea.Code = this.LotteryNumber + "|" + this.PlayTypeID;
        CodeArea.Money = this.Price;
        CodeArea.ShowContent = this.LotteryNumber.replace(/\)\(/g, " ").replace(/\(*\)*/g, "").replace(/\-/g, "_ ");
        CodeArea.AddTextToCodeArea();

        this.ClearSelectAll();
        this.SetSelectInfo();
        CodeArea.ShowCodeArea();

        this.LotteryNumber = "";
        this.PlayTypeName = "";
        this.PlayTypeID = "";
        this.Price = 0;
    },
    GetRandomNumber: function (startNum, endNum, count, splitStr, isSort, isRepeat) { //取随机数并按随机数的数量组合起来
        //startNum随机数开始
        //endNum随机数结束
        //count随机的数量
        //splitStr随机数之间分隔符
        //isSort是否需要排序s
        //isRepeat是否可以有重复号码
        var arrs = new Array(); //随机数集合
        var nums = "";

        //取随机不重复的号码
        while (arrs.length < count) {
            var num = parseInt(Math.floor(Math.random() * endNum + startNum));
            if (isRepeat == true) {
                arrs.push(num);
            } else {
                if (arrs.contains(num) == false) arrs.push(num);
            }
        }

        //排序数字从小到大
        if (isSort == true) arrs.sort(function (a, b) { return a > b ? 1 : -1 });

        //把随机数按组合起来分隔符为splitStr
        for (var i = 0; i < arrs.length; i++) {
            nums += arrs[i] + splitStr;
        }

        nums = nums.substring(0, nums.lastIndexOf(splitStr));
        return nums;
    },
    GetRandomDXDS: function (count) { //大小单双随机数
        var dxds = ["大", "小", "单", "双"];
        var nums1 = "";
        var nums2 = "";
        for (var i = 0; i < count; i++) {
            nums1 = Math.floor(Math.random() * dxds.length + 1) - 1;
            nums2 = Math.floor(Math.random() * dxds.length + 1) - 1;
        }
        return dxds[nums1] + dxds[nums2];
    },
    Submit: function () { //提交投注

        if (!CheckIsLogin()) return;

        //buyParameter.PlayTypeID = Lottery.PlayTypeID;
        buyParameter.SumNum = Lottery.TotalInvestNum; //总注数
        buyParameter.SumMoney = Lottery.TotalMoney;
        buyParameter.SumNum = Lottery.TotalInvestNum;
        buyParameter.Multiple = Lottery.TotalMultiple;
        buyParameter.SchemeBonusScale = 0;
        buyParameter.IsChase = $("#chase").is(":checked") ? 1 : 0;
        buyParameter.TotalShare = $("#totalshare").val();
        buyParameter.BuyShare = $("#buyshare").val();
        buyParameter.SecrecyLevel = $(".secrecy_level input:checked").attr("value");
        buyParameter.HomeIndex = 1;
        buyParameter.VIPIndex = 5;

        var assureShare = $("#assureshare").val();

        var SuccessType = 1; //投注方式：1、代购 2、追号 3、合买（入伙）

        if (buyParameter.IsNullBuyContent == 1) {
            buyParameter.SumMoney = sponsorMoney;
            buyParameter.SumNum = sumNum;
            buyParameter.Multiple = $(".faqi_shangchuan input:eq(1)").val();
            buyParameter.TotalShare = $(".faqi_shangchuan input:eq(2)").val();
            buyParameter.BuyShare = $(".faqi_shangchuan input:eq(3)").val();
            buyParameter.AssureMoney = assureMoney;
            buyParameter.SecrecyLevel = $(".fengcheng-list_baomi li[checked=checked]").attr("value");
            assureShare = $("#fqassure").val();

            //方案佣金
            buyParameter.SchemeBonusScale = $(".fengcheng-list li[checked=checked]").attr("value");
            if ($.trim($("#fqtitle ").val()) != "") {
                buyParameter.Title = $("#fqtitle").val();
            }
            if ($.trim($("#fqdescription").val()) != "") {
                buyParameter.Description = $("#fqdescription").val();
            }
            buyParameter.SchemeContent = "";
            // alert(buyParameter.Title + "\n" + buyParameter.Description + "\n" + buyParameter.SecrecyLevel + "\n" + buyParameter.SchemeBonusScale);
        }

        //投注的限制条件判断
        if (!$("#tongyi").is(":checked")) {
            alert("请先阅读用户投注协议，谢谢！");
            return;
        }
        if (buyParameter.IsuseID == "") {
            alert("没有期号，无法投注。");
            return;
        }
        if (!buyParameter.IsCurrIsuseEndTime) {
            alert("本期投注已截止，谢谢。");
            return;
        }
        if (CodeArea.CodeList.length == 0 && buyParameter.IsNullBuyContent == 0) {
            alert("请先将号码添加到号码篮。");
            return;
        }
        if (buyParameter.SumMoney < 2 || buyParameter.Multiple < 1) {
            alert("请选择正确的投注号码或者倍数。");
            return;
        }
        if (parseFloat(buyParameter.IsuseID) < 1) {
            alert("暂未销售，请稍后再进行购买。");
            return;
        }
        if (buyParameter.SumNum < 1) {
            alert("请输入投注内容。");
            return;
        }
        if (buyParameter.Multiple < 1) {
            alert("请输入正确的倍数。");
            return;
        }
        if (buyParameter.TotalShare < 1) {
            alert("请输入正确的总份数。");
            return;
        }
        if (buyParameter.BuyShare < 1) {
            alert("请输入正确的认购份数。");
            return;
        }
        if (buyParameter.IsNullBuyContent == 0) {
            buyParameter.SchemeContent = "";
            for (var i = 0; i < CodeArea.CodeList.length; i++) {
                buyParameter.SchemeContent += "\n" + CodeArea.CodeList[i].Code + "|" + CodeArea.CodeList[i].Money + "|" + CodeArea.CodeList[i].InvestNum;  //
            }
            buyParameter.SchemeContent = buyParameter.SchemeContent.substring(1);

        }

        if (buyParameter.IsChase == 1) { //追号

            if (Lottery.ChaseSumMoney <= 0) {
                alert("追号金额不能为空。");
                return;
            }
            if (Lottery.ChaseContent.length == 0) {
                alert("追号内容错误。");
                return;
            }
            if ($("#cKaSaw").is(":checked")) {
                buyParameter.AutoStopAtWinMoney = $("#autoStopAtWinMoney").val();
            }

            buyParameter.SumMoney = buyParameter.SumMoney / buyParameter.Multiple; //追号则表示单期单倍的金额
            buyParameter.ChaseContent = Lottery.ChaseContent.substring(0, Lottery.ChaseContent.length - 1);
            buyParameter.ChaseSumMoney = Lottery.ChaseSumMoney;
            //彩金计算
            var handAmount = parseFloat(BJXY28_Default.GetHandselAmount().value);
            var handBalance = parseFloat(0.00);
            var consumptionMoney = Lottery.ChaseSumMoney;
            if (consumptionMoney <= handAmount) {
                handAmount = consumptionMoney;
                handBalance = parseFloat(0.00);
            }
            else {
                handBalance = consumptionMoney - handAmount;
            }
            var tipStr = '';
            tipStr += "<div>&nbsp;&nbsp;&nbsp;&nbsp;总金额：　" + Lottery.ChaseSumMoney + " 元</div>";
            tipStr += "<div>彩金消费：　" + handAmount.toFixed(2) + "元</div>";
            tipStr += "<div>余额消费：　" + handBalance.toFixed(2) + "元</div>";
            tipStr += "<div>按“确定”即表示您已阅读《用户投注协议》并立即提交代购方案，确定要提交投注方案吗？</div>";
            var art = dialog({
                id: 'buydiv',
                title: '您要申请' + Lottery.LotName + '投注，详细内容：',
                width: 500,
                content: tipStr,
                fixed: true,
                ok: function () {
                    if ($("#cKaSaw").is(":checked")) {
                        buyParameter.AutoStopAtWinMoney = $("#autoStopAtWinMoney").val();
                    }
                    SuccessType = 2;
                    Lottery.AjaxPost(SuccessType);
                    //订单防重复处理
                    $("#divSubmit").hide();
                    if ($("#dgBtnDoing").length <= 0) {
                        $("#divSubmit").after('<div id="dgBtnDoing" style="background:#666; width:148px;height:31px; margin:0 auto 0 auto; font-size:14px; padding-top:5px; color:#fff; font-weight:bold; cursor:pointer;">正在购买中</div>');
                    }
                    else {
                        $("#dgBtnDoing").show();
                    }
                },
                okValue: '确定',
                cancel: function () { },
                cancelValue: '取消'
            });
            art.showModal();
        } else { //代购，合买

            buyParameter.Cobuy = 1;
            if ((buyParameter.SumMoney < Lottery.Price) || (buyParameter.SumMoney > 1000000)) {

                alert("单个方案的总金额必须在" + Lottery.Price + "元至 1000000 元之间。");
                return;
            }
            buyParameter.AssureMoney = (assureShare * (buyParameter.SumMoney / buyParameter.TotalShare)).toFixed(2);
            var TipStr = '';
            TipStr += "<div>注　数：　" + buyParameter.SumNum + "</div>";
            TipStr += "<div>倍　数：　" + buyParameter.Multiple + "</div>";
            TipStr += "<div>总金额：　" + buyParameter.SumMoney.toFixed(2) + " 元</div>";
            TipStr += "<div>总份数：　" + buyParameter.TotalShare + " 份</div>";
            TipStr += "<div>每　份：　" + (buyParameter.SumMoney / buyParameter.TotalShare).toFixed(2) + " 元</div>";
            TipStr += "<div>保　底：　" + assureShare + " 份，" + buyParameter.AssureMoney + "元</div>";
            TipStr += "<div>购　买：　" + buyParameter.BuyShare + " 份，" + ((buyParameter.SumMoney / buyParameter.TotalShare) * buyParameter.BuyShare).toFixed(2) + " 元</div>";
            var handAmount = parseFloat(BJXY28_Default.GetHandselAmount().value);
            var handBalance = parseFloat(0.00);
            var consumptionMoney = (buyParameter.SumMoney / buyParameter.TotalShare) * buyParameter.BuyShare;
            if (consumptionMoney <= handAmount) {
                handAmount = consumptionMoney;
                handBalance = parseFloat(0.00);
            }
            else {
                handBalance = consumptionMoney - handAmount;
            }
            TipStr += "<div>彩金消费：" + handAmount.toFixed(2) + "元</div>";
            TipStr += "<div>余额消费：" + handBalance.toFixed(2) + "元</div>";
            TipStr += "<div>按“确定”即表示您已阅读《用户投注协议》并立即提交代购方案，确定要提交投注方案吗？</div>";
            var art = dialog({
                id: 'buydiv',
                title: '您要发起' + Lottery.LotName + '方案，详细内容：',
                width: 500,
                content: TipStr,
                fixed: true,
                ok: function () {
                    if ($("#cKaSaw").is(":checked")) {
                        buyParameter.AutoStopAtWinMoney = $("#autoStopAtWinMoney").val();
                    }
                    SuccessType = 1;
                    Lottery.AjaxPost(SuccessType);
                    //订单防重复处理
                    $("#divSubmit").hide();
                    if ($("#dgBtnDoing").length <= 0) {
                        $("#divSubmit").after('<div id="dgBtnDoing" style="background:#666; width:148px;height:31px; margin:0 auto 0 auto; font-size:14px; padding-top:5px; color:#fff; font-weight:bold; cursor:pointer;">正在购买中</div>');
                    }
                    else {
                        $("#dgBtnDoing").show();
                    }
                },
                okValue: '确定',
                cancel: function () { },
                cancelValue: '取消'
            });

            art.showModal();
        }
    },
    AjaxPost: function (SuccessType) {
        //ajax提交投注内容
        $.ajax({
            type: "post",
            url: "/Ajax/Buy.ashx?tt=" + Math.random(),
            data: buyParameter,
            cache: false,
            async: true,
            dataType: "json",
            success: function (result) {
                if (parseInt(result.error, 10) > 0) {

                    var money = buyParameter.SumMoney;
                    if (buyParameter.IsChase == 1) {
                        money = buyParameter.ChaseSumMoney;
                    } else if (buyParameter.Cobuy == 2 || buyParameter.IsNullBuyContent == 1) {
                        money = ((buyParameter.SumMoney / buyParameter.TotalShare) * buyParameter.BuyShare);
                    }
                    window.location.href = "../Home/Room/UserBuySuccess.aspx?lotteryid=" + buyParameter.LotteryID + "&type=" + SuccessType + "&money=" + money + "&schemeid=" + result.error;
                    CodeArea.ClearAll();
                    return;
                } else if (parseInt(result.error, 10) == -808) {
                    location.href = "/UserLogin.aspx?RequestLoginPage=CQSSC/Default.aspx";
                    return;
                } else if (parseInt(result.error, 10) == -107) {
                    var okfunc = function () {
                        window.location.href = "/Home/Room/OnlinePay/Alipay02/Send_Default.aspx";
                    };
                    var cancelfunc = function () {
                        //订单防重复处理
                        $("#divSubmit").show();
                        if ($("#dgBtnDoing").length > 0) {
                            $("#dgBtnDoing").hide();
                        }
                    };
                    confirm("您的余额不足，是否立即充值？", okfunc, cancelfunc);
                    return;
                } else {
                    alert(result.msg);
                    //订单防重复处理
                    $("#divSubmit").show();
                    if ($("#dgBtnDoing").length > 0) {
                        $("#dgBtnDoing").hide();
                    }
                }
            }, error: function (xmlHttpRequest, textStatus, errorThrown) {
                alert("error");
                //订单防重复处理
                $("#divSubmit").show();
                if ($("#dgBtnDoing").length > 0) {
                    $("#dgBtnDoing").hide();
                }
            }
        });
    },
    ValidateShare: function () { //校验合买份数
        var totalshare = parseInt($("#totalshare").val());
        var buyshare = parseInt($("#buyshare").val());
        var assureshare = parseInt($("#assureshare").val());
        this.ShareInfo();
        if (isNaN(totalshare)) {
            //$("#totalshare").val(Lottery.TotalMoney);
            this.ShareInfo();
            return;
        }
        if (isNaN(buyshare)) {
            //$("#totalshare").val(totalshare);
            this.ShareInfo();
            return;
        }
        if (isNaN(assureshare)) {
            //$("#assureshare").val(0);
            this.ShareInfo();
            return;
        }
        var shareMoney = Lottery.TotalMoney / totalshare;

        if (Math.round(shareMoney * 100) / 100 != shareMoney) {
            $("#totalshare").val(Lottery.TotalMoney);
            this.ShareInfo();
            //显示最少认购份数
            $("#buyshare").nextAll("strong").find("em").text(0);
            return;
        }
        if (shareMoney < 1) {
            $("#totalshare").val(Lottery.TotalMoney);
            this.ShareInfo();
            return;
        }
        if (buyshare > totalshare) {
            $("#buyshare").val(totalshare);
            this.ShareInfo();
            return;
        }
        if ((buyshare + assureshare) > totalshare) {
            $("#assureshare").val(totalshare - buyshare);
            this.ShareInfo();
            return;
        }

        //计算最少认购份数
        var minBuyShare = 0;
        if (totalshare != 0) {
            var buyAndAssureScale = $("#labInitiateSchemeMinBuyAndAssureScale").val();
            minBuyShare = totalshare * buyAndAssureScale;
        }

        var minV = (minBuyShare.toString().indexOf(".") > -1 ? parseInt(minBuyShare) + 1 : minBuyShare);
        //显示最少认购份数
        $("#buyshare").nextAll("strong").find("em").text(minV);
        if (buyshare < 1) {
            $("#buyshare").val(1);
        }
        this.ShareInfo();
    },
    ShareInfo: function () {
        //总分数
        var totalshare = parseInt($("#totalshare").val());
        //认购份数
        var buyshare = parseInt($("#buyshare").val());
        //保底
        var assureshare = parseInt($("#assureshare").val());
        //每份的金额  
        var shareMoney = Lottery.TotalMoney / totalshare;

        if (shareMoney == Infinity || isNaN(shareMoney))
            shareMoney = 0;

        if (totalshare > 0 && buyshare == 0)
            $("#buyshare").val(1);

        buyParameter.AssureMoney = isNaN(shareMoney * assureshare) ? 0 : (shareMoney * assureshare).toFixed(2);
        //显示总金额
        $("#totalshare").next("span").find("em").text(shareMoney.toFixed(2));
        //显示认购金额
        $("#buyshare").next("span").find("em").text(isNaN(shareMoney * buyshare) ? 0 : (shareMoney * buyshare).toFixed(2));
        //显示保底金额
        $("#assureshare").next("span").find("em").text(buyParameter.AssureMoney);
        //显示最多保底份数
        $("#assureshare").nextAll("strong").text("最多可保底" + ((totalshare - buyshare) * shareMoney).toFixed(2) + "元");
    },
    UpdateShare: function () {
        if ($("#cobuy").is(":checked")) {
            $("#totalshare").val(Lottery.TotalMoney);
            var HiOpt_InitiateSchemeMinBuyAndAssureScale = $("#labInitiateSchemeMinBuyAndAssureScale").val();
            var buyShare = Math.round(Lottery.TotalMoney * HiOpt_InitiateSchemeMinBuyAndAssureScale);
            buyShare = buyShare > 1 ? buyShare : "1";
            if (Lottery.TotalMoney == 0) {
                buyShare = 0;
            }
            $("#buyshare").val(buyShare);
            // $("#buyshare").val(Lottery.TotalMoney > 0 ? 1 : 0);
            Lottery.ValidateShare();
        } else {
            $("#totalshare").val(1);
            $("#buyshare").val(1);
        }
    },
    GetBallState: function (obj) { //获得号码状态
        return obj.hasClass("active");
    }
}

/********显示选号信息********/
var CodeArea = {
    init: function () {
        this.CodeList = [];
        this.Code = "";
        this.Money = 0;
        this.InvestNum = 0;
        this.ShowContent = "";
        this.PlayName = "五星直选";
    },
    ShowCodeArea: function () { //显示选号内容
        $("#number-list li").remove(); //移除号码框内容<span class=\"xiugai\">修改</span>
        var lis = "";
        for (var i = this.CodeList.length - 1; i >= 0; i--) {
            var tile = this.CodeList[i].Code.split("|")[0] + " [" + this.CodeList[i].InvestNum + "注，" + this.CodeList[i].Money + "元]";
            var li = "<li title=\"" + tile + "\">";
            li += "        <p><b>" + this.CodeList[i].PlayName + "&nbsp;</b><b class=\"col-red\"style=\"letter-spacing:1px;\">" + CodeArea.ShowCodeFormat(this.CodeList[i].ShowContent) + "</b>";
            li += "           [" + this.CodeList[i].InvestNum + "注，" + this.CodeList[i].Money + "元]</p>";
            li += "        <a href=\"javascript:;\" onclick=\"CodeArea.DelCodeArea(" + i + ")\" >删除</a>";
            li += "   </li>";
            lis += li;
        }
        $("#number-list").html(lis + $("#number-list").html());
        $(".betmoney-ipt").val("");
        Lottery.CalcSumMoney(); //计算总注数、总金额
        Lottery.ShowBetresultInfo(); //显示投注信息
        chaseCalculateTotalMoney();
    },
    AddTextToCodeArea: function () {
        this.CodeList.push({ Code: this.Code, InvestNum: this.InvestNum, ShowContent: this.ShowContent, PlayName: this.PlayName, Money: this.Money });
        this.Code = "";
        this.Money = 0;
        this.InvestNum = 0;
        this.ShowContent = "";
        this.PlayName = "五星直选";

    },
    DelCodeArea: function (i) {
        this.CodeList.splice(i, 1);
        this.ShowCodeArea();

    },
    ClearAll: function () { //清空所有
        this.CodeList = [];
        this.ShowCodeArea();
    },
    ShowCodeFormat: function (val) {
        if (val.length >= 22) {
            return val.substr(0, 22) + ("<font style=\"color:#C4C4CF;font-weight:0xp;\"> ...</font>");
        }
        return val;
    }
}

/************自定义***************/
var sponsorMoney = 0;
var buyshareMoeny = 0;
var assureMoney = 0;
var sumNum = 0;

var isTwoDan = false; //是否为包二胆
var thisAreaId = null;
var isFunSimplex = radioGroup;
var starlevel = 5;
var tip = "";

//禁止随机按钮
function isDisabled(is) {
    var style = { "color": "#e36128", "background": "url(../Images/selected_btn.png) no-repeat", "cursor": "pointer" };
    if (is) {
        style = { "color": "gray", "background": "url(../Images/p3_suiji_but_grey.png) no-repeat", "cursor": "default" };
    }
    $(".selected_btnbox a:eq(0)").css(style).attr("disabled", is);
    $(".selected_btnbox a:eq(1)").css(style).attr("disabled", is);
    $(".selected_btnbox a:eq(2)").css(style).attr("disabled", is);
}

function chaseIsuseInit() {
    $("input[name='number']").keypress(function () { //控制文本框只能输入数字
        return Lottery.MustBeDigitKey();
    }).keyup(function () {
        var text = $(this).val();
        var patrn = /^[0-9]*[0-9][0-9]*$/; ///^\d+$/g
        if (!patrn.test(text)) {
            $(this).val(1);
        }
    });
    $(".zhuihaoBox_title input:eq(0)").change(function () {
        if ($(".zhuihaoBox_title input:eq(2)").val() == "")
            $(".zhuihaoBox_title input:eq(2)").val("1");
        changeChaseInfo();
    });
    $(".zhuihaoBox_title input:eq(1)").keyup(function () {
        var text = $(this).val();
        var patrn = /^[0-9]*[1-9][0-9]*$/; ///^\d+$/g
        if (!patrn.test(text)) {
            $(this).val(1);
        }
        if (parseInt(text) > $(".zhuihaoBox_con dd").length) {
            $(this).val($(".zhuihaoBox_con dd").length);
        }
        $(".zhuihaoBox_con dd input[type='text']").attr("disabled", false);
        $(".zhuihaoBox_con dd").hide();
        $(".zhuihaoBox_title input:eq(0)").attr("checked", true);
        $(".zhuihaoBox_title input:eq(2)").val("1");
        changeChaseInfo();
    });

    $(".zhuihaoBox_title input:eq(2)").keyup(function () {
        if ($(this).val() > 99) {
            $(this).val(99);
        }
        $(".zhuihaoBox_title input:eq(0)").attr("checked", true);
        $(".zhuihaoBox_con dd input:eq(1)").attr("disabled", false);
        changeChaseInfo();
    });
    $(".zhuihaoBox_con dd").find("input:eq(0)").change(function () {
        var ischecked = $(this).attr("checked");
        if (!ischecked) {
            $(this).parents("dd").find("input:eq(1)").val(0);
            $(this).parents("dd").find(".amount em").text(0);
        } else {

            $(this).parents("dd").find("input:eq(1)").val(1);
            $(this).parents("dd").find(".amount em").text(Lottery.TotalMoney * 1);
        }
        var checkedCount = $(".zhuihaoBox_con dd").find("input:eq(0):checked").length;
        if (checkedCount == 0) {
            $(".zhuihaoBox_title input:eq(0)").attr("checked", false);
            checkedCount = 1;
        } else {
            $(".zhuihaoBox_title input:eq(0)").attr("checked", true);
        }
        $(".zhuihaoBox_title input[name='number']").eq(0).val(checkedCount);
        chaseCalculateTotalMoney();
    });
    $(".zhuihaoBox_con dd").find("input:eq(1)").keyup(function () {
        if ($(this).val() == "0") {
            $(this).val(1);
        }
        if ($(this).val() > 99) {
            $(this).val(99);
        }
        $(".zhuihaoBox_title input[name='number']").eq(1).val("1");
        $(this).parents("dd").find(".amount em").text(Lottery.TotalMoney * parseInt($(this).val()));
        chaseCalculateTotalMoney();
    });
    $(".zhuihaoBox_con dd").hide();
    changeChaseInfo();
}

function changeChaseInfo() {
    var chasehowmany = parseInt($(".zhuihaoBox_title input[name='number']").eq(0).val()); //多少期
    var multiple = parseInt($(".zhuihaoBox_title input[name='number']").eq(1).val()); //倍数
    var ischase = $(".zhuihaoBox_title input:eq(0)").is(":checked");
    if (chasehowmany > $(".zhuihaoBox_con dd").length) {
        chasehowmany = $(".zhuihaoBox_con dd").length;
    }
    if (ischase) {
        $(".zhuihaoBox_con dd").find("input:eq(0)").attr("checked", false);
        $(".zhuihaoBox_con dd").find("input:eq(1)").val("0");
        $(".zhuihaoBox_con dd").find(".amount em").text("0");
        for (var i = 0; i < chasehowmany; i++) {

            $(".zhuihaoBox_con dd").eq(i).show();
            $(".zhuihaoBox_con dd").eq(i).find("input:eq(0)").attr("checked", true);
            $(".zhuihaoBox_con dd").eq(i).find("input:eq(1)").val(multiple);
            $(".zhuihaoBox_con dd").eq(i).find(".amount em").text(Lottery.TotalMoney);
        }
    } else {

        $(".zhuihaoBox_con dd").find("input:eq(0)").attr("checked", false);
        $(".zhuihaoBox_con dd").find("input:eq(1)").val("0");
        $(".zhuihaoBox_con dd").find(".amount em").text("0");
    }
    chaseCalculateTotalMoney();
    //is(":visible")
    var height = $(".zhuihaoBox_con dd:visible").length * 33;
    if (height != 0) {
        $(".zhuihaoBox_con").height(height > 300 ? 300 : height);
        $(".zhuihaoBox").height(height + 99 > 400 ? 400 : height + 99);

        return;
    }
    $(".zhuihaoBox_con").height((33 * chasehowmany) > 300 ? 300 : (33 * chasehowmany));
    $(".zhuihaoBox").height((33 * chasehowmany + 99) > 400 ? 400 : (33 * chasehowmany + 99));
}

function chaseCalculateTotalMoney() {
    Lottery.ChaseSumMoney = 0;
    Lottery.ChaseContent = "";  //追号内容格式：期号ID,倍数,金额;
    var count = 0;
    var totalMultiple = 0;
    $(".zhuihaoBox_con dd").each(function () {
        if ($(this).find("input:eq(0)").is(":checked")) {
            var multiple = $(this).find("input:eq(1)").val();
            var isusesID = $(this).find("input:eq(0)").val();
            count++;
            totalMultiple += parseInt(multiple);
            Lottery.ChaseSumMoney += parseInt(multiple) * (Lottery.TotalMoney / Lottery.TotalMultiple);
            Lottery.ChaseContent += isusesID + "," + multiple + "," + parseInt(multiple) * (Lottery.TotalMoney / Lottery.TotalMultiple) + ";";
            $(this).find(".amount em").text(parseInt(multiple) * (Lottery.TotalMoney / Lottery.TotalMultiple));
        }
    });
    $(".zhuihaoBox .result").text("追号期数：" + count + "期  累计倍数：" + totalMultiple + "倍  金额：" + Lottery.ChaseSumMoney);
}


function padLeft(v, n) {
    var k = "";
    if (v == null || v == null) return;
    for (var i = v.length; i <= n; i++) {
        k += "0";
    }
    return k + v;
}

function funSimplex() { //单式函数
    if (eval($("#upload_context").attr("por"))) {
        alert("请输入号码。");
        $("#upload_context").val("");
        $("#upload_context").focus();
        return;
    }
    if ($("#upload_context").val().replace(/\n{2,}/g, '\n').split('\n').length > 1000) {
        alert("您的录入已经超过1000注，请删除超出部分或者使用.txt文件上传。");
        return;
    }
    // $.jBox.tip("过滤掉格式不正确的投注内容，请核对。");
    ValidateInvestContent($("#upload_context").val().replace(/\n{2,}/g, '\n').replace(/\+/g, "#"));
    $("#upload_context").focus();
}

//确认选号单选函数
function radioGroup() {
    var investNum = Lottery.GetLotteryInvestNum();
    if (investNum == 0) {
        alert("至少选择1注号码才能投注。");
        return;
    }

    $betMoney = $(".betmoney-ipt");
    var money = Number($betMoney.val());
    if (money <= 0) {
        alert("请填写投注金额。");
        $betMoney.focus();
        return;
    }
    if (money > Lottery.MaxAllowInvestMultiple) {
        alert("每注不能大于" + Lottery.MaxAllowInvestMultiple + "元。");
        return;
    }
    Lottery.CastBasket();
}


/********初始化信息********/
function intit() {
    Lottery.LotID = 99;
    Lottery.LotName = "北京幸运28";
    Lottery.PlayTypeID = 9902;
    Lottery.PlayTypeName = "";
    Lottery.SelectButton();
    CodeArea.init();

    $("#totalshare").val(1);
    $("#buyshare").val(1);

    //控制输入投注金额
    $(".betmoney-ipt").change(function () {
        var text = $(this).val();
        var patrn = /^[0-9]*[1-9][0-9]*$/; ///^\d+$/g
        if (!patrn.test(text)) {
            $(this).val(5);
        }
        if ($(this).val() >= Lottery.MaxAllowInvestMultiple) {
            $(this).val(5);
        }
        Lottery.SetSelectInfo();
        //Lottery.TotalMultiple = $(this).val();总倍数
        Lottery.CalcSumMoney();
        Lottery.ShowBetresultInfo();

    }).keypress(function () { //控制文本框只能输入数字
        return Lottery.MustBeDigitKey();
    }).keyup(function () {
        if ($(this).val() >= Lottery.MaxAllowInvestMultiple)
            $(this).val(5);
    });
    //号码牌选择金额
    $("#paiMoney li").click(function () {
        var mupicer = Number($(".betmoney-ipt").val());
        var tupicer = Number($(this).text());
        $(".betmoney-ipt").val(tupicer + mupicer);
        $(".betmoney-ipt").change();
    });


    $("#random-btn a:eq(3)").click(function () { //清空
        CodeArea.ClearAll();
    });

    //立即投注
    $("#confirm-ball").click(function () {
        isFunSimplex();
    });

    $(".cway-list li").click(function () {
        $(".cway-list li").attr("checked", false).removeClass("on");
        $(this).attr("checked", true).addClass("on");
    });
    $(".item_hemai_cnt input:gt(1)")
        .keypress(function () { //控制文本框只能输入数字
            return Lottery.MustBeDigitKey();
        })
        .change(function () {
            if (Lottery.TotalMoney == 0) {
                $(this).val(0);
                return
            };
            var text = $(this).val();
            var patrn = /^[0-9]*[1-9][0-9]*$/; ///^\d+$/g
            if ($(this).attr("id") == "assureshare")
                patrn = /^[0-9]*[0-9][0-9]*$/; ///^\d+$/g
            if (!patrn.test(text)) {
                $(this).val(1);
            }
            Lottery.ValidateShare();
        })
        .keyup(function () {
            Lottery.ValidateShare();
        });
    /*代购*/
    $("#genbuy").change(function () {
        $("#totalshare").val(1);
        $("#buyshare").val(1);
        $("#assureshare").val(0);
        $("#schemeTile").val("");
        $("#schemedescription").val("");
        $(".item_hemai").hide();
        $(".moreOperate_con").hide();
        $(".buy-number-1 input").attr("disabled", false);
        $(".buy-number-1 span").attr("forbidden", "");
        $("#div_gmfstip").text("购买人自行全额购");
    });

    //方案提成、保密设置等点击事件
    $('ul.fengcheng-list > li,ul.fengcheng-list_baomi >li,ul.cway-list>li,ul.cbuy-list_baomi>li').click(function () {
        $(this).css({ 'background': '#ff7b23', 'border': '1px solid #dd590a', 'color': '#fff' })
        $(this).siblings().css({ 'background': '#fff', 'border': '1px solid #b5b5b5', 'color': '#333' })
    });
    /*方案提成*/
    $(".fengcheng-list li").click(function () {
        $(".fengcheng-list li").attr("checked", false);
        $(this).attr("checked", true);
    });
    /*保密方式*/
    $(".fengcheng-list_baomi li").click(function () {
        $(".fengcheng-list_baomi li").attr("checked", false);
        $(this).attr("checked", true);
    });
    /*可选切换*/
    $("#cstitle").hide();
    $("#csdes").hide();
    $("#choosableinfo").click(function () {
        if ($(this).is(":checked")) {
            $("#cstitle").show();
            $("#csdes").show();
        } else {
            $("#cstitle").hide();
            $("#csdes").hide();
        }
    });

}

/**************彩票投注计时器******************/
var LotteryTimer =
{
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

    Init: function ()//初始化数据
    {
        LotteryTimer.LocalDate = new Date(); //初始化本地时间

        //初次加载获取到期号截止时间后,启动计时器
        setInterval(LotteryTimer.StartTimer, 1000);
    },

    StartTimer: function ()//启动计时器
    {
        if (LotteryTimer.DefaultSeconds >= LotteryTimer.AfterSeconds) {
            LotteryTimer.ResetLocalDate();
        }
        else {
            var localNowDate = new Date(); //当前本地时间

            var localT = localNowDate.getTime() - LotteryTimer.LocalDate.getTime(); //本地时间差
            var localS = Math.floor(localT / 1000 % 60); //本地时间差（秒）

            var serverT = LotteryTimer.ServerTime.getTime() - LotteryTimer.ServerDate.getTime(); //服务器时间差

            var serverS = Math.floor(serverT / 1000 % 60); //服务器时间差（秒）

            //校准时间
            if ((localS - serverS) > 3) {
                LotteryTimer.ServerTime.setSeconds(LotteryTimer.ServerTime.getSeconds() + (localS - serverS));
            } else {
                LotteryTimer.ServerTime.setSeconds(LotteryTimer.ServerTime.getSeconds() + 1);
            }
        }

        var tempTime = null; //临时时间变量，用来做时间交换处理

        if (LotteryTimer.AdvanceEndTime != "" && LotteryTimer.CurrentEndTime != "" && LotteryTimer.ServerTime != "") {
            //getTime()返回当前时间的毫秒数
            var t = LotteryTimer.CurrentEndTime.getTime() - LotteryTimer.ServerTime.getTime(); //时间差
            var m = Math.floor(t / 1000 / 60 % 60); //时间差（分）
            var s = Math.floor(t / 1000 % 60); //时间差（秒）

            //倒计时小于2秒时开始获取期号信息
            if (LotteryTimer.AutoGetIsuseInfoId == null) {
                if ((m * 60 + s) <= 1)//小于2秒时自动获取下期投注信息
                {
                    LotteryTimer.ResetLocalDate();

                    LotteryTimer.AutoGetIsuseInfoId = setInterval("LotteryTimer.GetCurrentNo(false)", 1000);
                }
            }

            var tt = Math.floor(LotteryTimer.AdvanceEndTime.getTime() - LotteryTimer.ServerTime.getTime()); //当前时间与服务器时间的差值（毫秒）

            if (tt > 0)//还未截止
            {
                buyParameter.IsCurrIsuseEndTime = true; //允许投注

                //允许投注,显示倒计时
                LotteryTimer.TitleContent = "离截止时间还有：";
                LotteryTimer.IsShowTime = true;
                tempTime = new Date(LotteryTimer.AdvanceEndTime.toString());
            } else {
                buyParameter.IsCurrIsuseEndTime = false; //不允许投注

                tt = Math.floor(LotteryTimer.CurrentEndTime.getTime() - LotteryTimer.ServerTime.getTime());

                if (tt > 0) {
                    //不允许投注,显示倒计时
                    LotteryTimer.TitleContent = "离下期投注时间还有：";
                    LotteryTimer.IsShowTime = true;
                    tempTime = new Date(LotteryTimer.CurrentEndTime.toString());
                } else {
                    LotteryTimer.IsShowTime = false;
                }
            }
        } else {
            LotteryTimer.IsShowTime = false;
        }

        if (LotteryTimer.IsShowTime == true) {
            $("#is-text-show").html(LotteryTimer.TitleContent);

            var t = tempTime.getTime() - LotteryTimer.ServerTime.getTime(); //时间差

            var d = Math.floor(t / 1000 / 60 / 60 / 24); //时间差（天）
            var h = Math.floor(t / 1000 / 60 / 60 % 24); //时间差（小时）
            var m = Math.floor(t / 1000 / 60 % 60); //时间差（分）
            var s = Math.floor(t / 1000 % 60); //时间差（秒）

            if (d > 0) {
                $("#is-count-down span:eq(0)").show();
                $("#is-count-down span:eq(1)").show();
                $("#is-count-down span:eq(2)").hide();
                $("#is-count-down span:eq(3)").hide();
            }
            else if (h > 0) {
                $("#is-count-down span:eq(1)").show();
                $("#is-count-down span:eq(2)").show();
                $("#is-count-down span:eq(3)").hide();
            }
            $("#is-count-down b:eq(0)").html(d);
            $("#is-count-down b:eq(1)").html(h < 10 ? "0" + h : h);
            $("#is-count-down b:eq(2)").html(m < 10 ? "0" + m : m);
            $("#is-count-down b:eq(3)").html(s < 10 ? "0" + s : s);
        }
        else {
            $("#is-count-down b:eq(2)").html("00");
            $("#is-count-down b:eq(3)").html("00");
        }

        LotteryTimer.DefaultSeconds++;
    },

    GetCurrentNo: function (isLoad)//获取当前期号
    {
        $.ajax({
            type: "post",
            url: "../ajax/DefaultPageIssue_ChaseInfo.ashx",
            data: { lotteryId: Lottery.LotID },
            cache: false,
            async: true, //设置为异步获取
            dataType: "text",
            success: function (response) {
                if (response != null) {
                    window.clearInterval(LotteryTimer.AutoGetIsuseInfoId);
                    LotteryTimer.AutoGetIsuseInfoId = null;

                    if (response.split('|').length > 0 && response.split('|')[0].split(',').length > 4) {
                        var arrInfo = response.split('|');
                        var currIsuse = arrInfo[0]; //当前期信息
                        var chaseIsuse = arrInfo[1]; //追号信息

                        var arrcurrIsuse = currIsuse.split(','); //解析当前期信息

                        buyParameter.IsuseEndTime = arrcurrIsuse[2]; //当前期截止时间

                        var tempTime = new Date(buyParameter.IsuseEndTime.replace(new RegExp("-", "g"), "/"));  //期截时间

                        LotteryTimer.ResetLocalDate();

                        try {
                            var t = Math.floor(tempTime.getTime() - LotteryTimer.ServerTime.getTime()); //时间差

                            if (t < 0 && buyParameter.IsCurrIsuseEndTime == false && buyParameter.IsuseID != "") {
                                //启动定时器，每1秒请求一次
                                LotteryTimer.AutoGetIsuseInfoId = setInterval("LotteryTimer.GetCurrentNo(false)", 1000);
                                return;
                            }
                        } catch (e) {
                            //启动定时器，每1秒请求一次
                            LotteryTimer.AutoGetIsuseInfoId = setInterval("LotteryTimer.GetCurrentNo(false)", 1000);
                            return;
                        }

                        $(".zhuihaoBox").html(chaseIsuse); //显示追号期号信息
                        buyParameter.IsuseID = arrcurrIsuse[0]; //期号
                        LotteryTimer.AdvanceEndTime = new Date(buyParameter.IsuseEndTime.replace(new RegExp("-", "g"), "/"));  //提前截止时间    
                        LotteryTimer.CurrentEndTime = new Date(arrcurrIsuse[4].replace(new RegExp("-", "g"), "/")); //本期最后截止时间

                        if (isLoad == true) {
                            LotteryTimer.Init();
                        }

                        $(".count-center strong:eq(0)").html(arrcurrIsuse[1]);
                        LotteryTimer.CurrentNo = arrcurrIsuse[1].substring(9);
                        chaseIsuseInit(); //添加追号信息

                        //如果不是初次加载，90秒后开始获取开奖信息
                        if (isLoad == false && LotteryTimer.AutoOpenInfoId == null) {
                            LotteryTimer.AutoOpenInfoId = setTimeout("LotteryTimer.GetOpenInfo(true)", 1000 * LotteryTimer.OpenAwardBySeconds);
                        }
                    }
                }
                else {
                    //启动定时器，每1秒请求一次
                    LotteryTimer.AutoGetIsuseInfoId = setInterval("LotteryTimer.GetCurrentNo(false)", 1000);
                }
            },
            error: function (status) {
            }
        });
    },

    GetAlreadyOpenNo: function ()//已开多少期，还剩余多少期
    {
        $.ajax({
            type: "post",
            url: "../ajax/GetTodayIsusesCount.ashx",
            data: { lotteryId: Lottery.LotID },
            cache: false,
            async: true,
            dataType: "json",
            success: function (result) {
                if (result.errCode == 0) {
                    $(".count-center p strong:eq(1)").text(result.OpenCount);
                    $(".count-center p strong:eq(2)").text(result.Total - result.OpenCount);
                    $("#sell-status i").text(result.OpenCount);
                    $("#sell-status b").text(result.Total - result.OpenCount);
                }
            },
            error: function (status) {
            }
        });
    },

    GetOpenInfo: function (isLoad)//获取开奖信息
    {
        $.ajax({
            type: "post",
            url: "../ajax/GetHighFrequencyColor.ashx",
            data: { lotteryId: Lottery.LotID },
            cache: false,
            async: true,
            dataType: "json",
            success: function (result) {
                window.clearInterval(LotteryTimer.AutoOpenInfoId);
                LotteryTimer.AutoOpenInfoId = null;
                /*
                2015-07-31增加，主要是提高用户体验（5.3.4用户体验报告）
                */
                $("#loadOpenInfoData").remove();
                $("#open-list-news tr").show();
                if (-103 == result.errCode) {
                    $("#open-list-news tr:first").nextAll().hide();
                    $("#open-list-news tr:first").nextAll().eq(0).html("<td colspan='2'>暂无开奖记录</td>").show();
                }
                if (result.errCode == 0) {
                    var NewCurrentNo = parseInt(result.NewName.substring(9)); //最新开奖的期号

                    if (LotteryTimer.CurrentNo - NewCurrentNo == 1 || isLoad == true) {
                        LotteryTimer.GetAlreadyOpenNo();

                        if (result.NewNumber != "") {
                            $("#news-open-info i:eq(0)").text(result.NewName);
                            $("#news-open-info i:eq(1)").text(result.NewDate);
                            $("#news-open-number b:eq(0)").text(result.NewNumber.substring(0, 1));
                            $("#news-open-number b:eq(1)").text(result.NewNumber.substring(1, 2));
                            $("#news-open-number b:eq(2)").text(result.NewNumber.substring(2, 3));
                            $("#news-open-number b:eq(3)").text(result.SumNumber);
                        }

                        var count = 0;

                        result.OpenInfo.sort(function (a, b) { return (a.ID < b.ID) ? 1 : -1 });
                        $.each(result.OpenInfo, function (i, d) {
                            if (d.WinLotteryNumber != "" && count < 10) {
                                if (d.WinLotteryNumber.length > 10) {
                                    $("#open-list-news tr").eq(count + 1).find("td:eq(1)").text("- -");
                                }
                                else {
                                    var winNum = d.WinLotteryNumber;

                                    var num1 = Number(winNum.substring(0, 1));
                                    var num2 = Number(winNum.substring(1, 2));
                                    var num3 = Number(winNum.substring(2));

                                    var sumNum = num1 + num2 + num3;
                                    var showWinNumber = '' + num1 + '+' + num2 + '+' + num3 + '=' + sumNum;
                                    $("#open-list-news tr").eq(count + 1).find("td:eq(0)").text(d.Name);
                                    $("#open-list-news tr").eq(count + 1).find("td:eq(1)").html(showWinNumber);
                                    $("#open-list-news tr").eq(count + 1).find("td:eq(2)").text(d.AfterThree);
                                }
                                count++;
                            }
                        });

                        
                    }
                    if (LotteryTimer.CurrentNo - NewCurrentNo != 1) {

                        //继续获取开奖信息
                        LotteryTimer.AutoOpenInfoId = setTimeout("LotteryTimer.GetOpenInfo(true)", 10000);
                    }
                }
                else if (result.errCode == -102)//数据获取异常，重新获取
                {
                    //继续获取开奖信息
                    LotteryTimer.AutoOpenInfoId = setTimeout("LotteryTimer.GetOpenInfo(true)", 10000);
                }
            },
            error: function (state) {
            }
        });
    },

    GetServerTime: function ()//获取服务器时间
    {
        $.ajax({
            type: "post",
            url: "../../ajax/getServerTime.ashx",
            cache: false,
            async: false, //使用同步去获取时间，True为异步
            dataType: "text",
            success: function (result) {
                LotteryTimer.ServerDate = new Date(result.replace(new RegExp("-", "g"), "/"));
                LotteryTimer.ServerTime = new Date(result.replace(new RegExp("-", "g"), "/")); //服务器时间
            },
            error: function (status) {
                LotteryTimer.GetServerTime();
            }
        });
    },

    ResetLocalDate: function () {
        LotteryTimer.LocalDate = new Date(); //重置当前存储的本地时间

        //超过指定时间后，获取一次服务器时间
        LotteryTimer.DefaultSeconds = 0;
        LotteryTimer.GetServerTime();
    }

};

/********页面加载时所执行的脚本********/
$(function () {
    intit();
    isDisabled(true);
    LotteryTimer.GetCurrentNo(true);
    LotteryTimer.AutoOpenInfoId = setTimeout("LotteryTimer.GetOpenInfo(true)", 1000); //延时加载开奖信息

    function tab(tabli, tabcon, tabev) {
        $("#" + tabli).children("li").bind(tabev, function () {
            $("#" + tabli).children("li").removeClass("curr");
            $(this).addClass("curr");
            $("#" + tabli).nextAll("." + tabcon).css('display', 'none');
            $("#" + tabli).nextAll("." + tabcon).eq($(this).index()).css('display', 'block');
        })
    }
    tab("ord_chotabli", "paly_choice_con", "click") //排列3选项卡调用方法
    tab("pl3_danshi_tabli3", "pl3_danshi_con", "click") //幸运号码调用方法

    /*复制/粘贴选项内容切换*/
    $('#ssc .lect > label').each(function (e) {
        $(this).click(function () {
            $('.lect-box > div').eq(e).removeClass('hide');
            $('.lect-box > div').eq(e).siblings().addClass('hide');
        });
    });

    /*今日开奖号码收起*/
    $('.today-lottery > h4 > span').toggle(
			 function () {
			     $('.today-lottery .list').hide(1000);
			     $(this).removeClass('active').text("展开");

			 },
			 function () {
			     $('.today-lottery .list').show(1000);
			     $(this).addClass('active').text("收起");
			 }
		);
    /*遗漏选择*/
    $('.select').mouseover(function () {
        $(this).find('dl').removeClass('hide');
    });
    $('.select').mouseout(function () {
        $(this).find('dl').addClass('hide');
    });
    $('.select > dl > dd').click(function () {
        var con = $(this).text();
        $(this).parent().addClass('hide');
        $(this).parent().prev().text(con);
    });
});

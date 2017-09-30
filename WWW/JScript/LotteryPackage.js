var Lottery = {
    LotID: 0,
    LotName: "",
    LotTypeID: 0,   //彩种分类，1 乐透型（分前后区，如双色球、大乐透） 2 乐透型（单区，如22选5 36选7） 3 排列型（如：3D、排3/5、七星彩、时时彩等）
    PlayTypeID: 0,
    PlayTypeName: "",
    SelectAllAreaID: "",  //选号区块ID
    SelectRAreaID: "",  //选号区块ID（前区/胆码区）
    SelectTAreaID: "",  //选号区块ID（拖码区）
    SelectBAreaID: "",  //选号区块ID（后区）
    playTypeForRegex: "", //玩法正则表达式
    RedBallCount: 0,    //红球总个数
    MaxDBallCount: 0,    //胆码最大个数
    BlueBallCount: 0,   //篮球个总数
    TotalMoney: 0,  //总金额
    TotalMultiple: 1, //总倍数
    MaxAllowInvestMultiple: 999, //允许投注的最大倍数,默认999倍
    TotalInvestNum: 0, //总注数
    Price: 2,
    LotteryNumber: "", //投注内容
    MissingValues: "", //遗漏值，Json字符串
    //选号，Type 表示红、篮球类型，值为1 = r（红）、2 = r（胆）、3 = r（拖）、4 = b（蓝球） 
    SelectBall: function (obj, Type) {
        var RorB = (Type == 4 ? "b" : "r");

        var Selected = this.GetBallState(obj, Type);
        if (Selected) {
            obj.removeClass(RorB + "ball");

            this.CheckFull();
            return;
        }
        var DRedCount = 0;
        var TRedCount = 0; //拖码
        var RedCount = 0;
        var BlueCount = 0;

        switch (this.LotTypeID) {
            case 1:
            case 2:
                DRedCount = this.getSelectedRedBallCount();
                RedCount = DRedCount;
                BlueCount = this.getSelectedBlueBallCount();

                //红球个数判断
                if (Type != 4) {
                    if (Type == 1 || Type == 2 || Type == 3) {
                        if (this.RedBallCount > 0) {
                            if (RedCount >= this.RedBallCount) {
                                parent.msg("红球最多允许选" + this.RedBallCount + "个");
                                return;
                            }
                        }
                    }
                }

                //蓝球个数判断
                if (this.BlueBallCount > 0) {
                    if (BlueCount >= this.BlueBallCount) {
                        parent.msg("蓝球最多允许选" + this.BlueBallCount + "个");
                        return;
                    }
                }

                break;
        }
        obj.addClass(RorB + "ball");
        this.CheckFull();
    },
    //设置号码的状态
    SetBallState: function (sender, Type, State) {
        var RorB = (Type == 4 ? "b" : "r");
        if (!State) {
            $(sender).removeClass(RorB + "ball");
        }
        else {
            $(sender).addClass(RorB + "ball");
        }
    },
    //快速选号
    QuickSelect: function (obj) {
        var Type = obj.attr("rel");

        obj.parent().parent().find("b").removeClass("rball");
        switch (Type) {
            case "Q":
                obj.parent().parent().find("b").addClass("rball");
                break;
            case "D":
                obj.parent().parent().find("b:gt(4)").addClass("rball");
                break;
            case "X":
                obj.parent().parent().find("b:lt(5)").addClass("rball");
                break;
            case "J":
                obj.parent().parent().find("b:odd").addClass("rball");
                break;
            case "O":
                obj.parent().parent().find("b:even").addClass("rball");
                break;
            case "Z":
                var zs = new Array("0", "1", "2", "4", "6", "10");
                for (var i = 0; i < zs.length; i++) {
                    obj.parent().parent().find("b:eq(" + zs[i] + ")").addClass("rball");
                }
                break;
            case "H":
                var hs = new Array("3", "5", "7", "8", "9");
                for (var i = 0; i < hs.length; i++) {
                    obj.parent().parent().find("b:eq(" + hs[i] + ")").addClass("rball");
                }
                break;
        }

        this.CheckFull();
    },
    //号码状态
    GetBallState: function (obj, Type) {
        if (Type == 4) {
            return obj.hasClass("bball");
        }

        return obj.hasClass("rball");
    },

    //清除选号
    ClearSelect: function (Type) {
        switch (Type) {
            case 0:

                if (this.SelectAllAreaID != "" && ($("#" + this.SelectAllAreaID + " b.rball").length > 0 || $("#" + this.SelectAllAreaID + " b.bball").length > 0)) {
                    $("#" + this.SelectAllAreaID + " b").removeClass("rball").removeClass("bball");
                }

                $("#" + this.SelectRAreaID + " b").removeClass("rball");

                if (this.SelectBAreaID != "" && this.getSelectedBlueBallCount() > 0) {
                    $("#" + this.SelectBAreaID + " b").removeClass("bball");
                }

                break;
            case 1:
            case 2:
                $("#" + this.SelectRAreaID + " b").removeClass("rball");

                break;
            case 3:
                $("#" + this.SelectTAreaID + " b").removeClass("rball");

                break;
            case 4:
                $("#" + this.SelectBAreaID + " b").removeClass("bball");

                break;
        }
        this.CheckFull();
        if (this.LotID == 5 || this.LotID == 39) {
            $(".l_bnzh_p strong").eq(0).html(0);
            $(".l_bnzh_p strong").eq(1).html(0);
            $(".l_bnzh_p strong").eq(2).html(0);
            $(".l_bnzh_p strong").eq(3).html(0);
        }
        else {
            $(".l_bnzh_p strong").eq(0).html(0);
            $(".l_bnzh_p strong").eq(1).html(0);
        }
    },
    //取投注内容   按照彩种Id升序排序
    GetLotteryNumber: function () {
        this.LotteryNumber = "";
        var tmpLotteryNumber = "";
        this.GetLotteryInvestNum();
        if (this.TotalInvestNum < 1) {
            return "";
        }
        switch (this.PlayTypeID) {
            case 501:   //双色球单复式-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += " # ";
                tmpLotteryNumber += this.getSelectedBlueBallText(" ");
                break;
            case 601:   //3D直选单/复式-取投注内容
                $("#tz_3d ul").each(function (i, n) {
                    var temp = "";
                    $(n).find("b.rball").each(function (i, b) {
                        temp += $.trim($(b).text());
                    });

                    if (temp.length > 1) {
                        temp = "(" + temp + ")";
                    }

                    tmpLotteryNumber += $.trim(temp);
                });
                break;
            case 3901:  //大乐透普通投注-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += " # ";
                tmpLotteryNumber += this.getSelectedBlueBallText(" ");
                break;
            case 6301: //排列三-普通投注-取投注内容
                $("#tz_pl3 ul").each(function (i, n) {
                    var temp = "";
                    $(n).find("b.rball").each(function (i, b) {
                        temp += $.trim($(b).text());
                    });

                    if (temp.length > 1) {
                        temp = "(" + temp + ")";
                    }

                    tmpLotteryNumber += $.trim(temp);
                });
                break;
            case 6401: //排列五-普通投注-取投注内容
                $("#" + this.SelectRAreaID + " ul").each(function (i, n) {
                    var temp = "";
                    $(n).find("b.rball").each(function (i, b) {
                        temp += $.trim($(b).text());
                    });

                    if (temp.length > 1) {
                        temp = "(" + temp + ")";
                    }

                    tmpLotteryNumber += $.trim(temp);
                });
                break;
        }
        this.LotteryNumber = $.trim(tmpLotteryNumber);
    },
    //取选中的红球号码，如果是胆拖玩法则表示胆码
    getSelectedRedBallText: function (splitChar) {
        if (splitChar == undefined)
            return "Please enter the char for split the number";
        var temp = "";
        $("#" + this.SelectRAreaID + " b.rball").each(function (i, n) {
            temp += ($.trim($(n).text()) + splitChar);
        });
        if (temp.substring(temp.length - 1, temp.length) == splitChar && splitChar != "")
            temp = temp.substring(0, temp.length - 1);
        return $.trim(temp);
    },

    //取选中的红球拖码
    getSelectedTuoBallText: function (splitChar) {
        if (splitChar == undefined)
            return "Please enter the char for split the number";
        var temp = "";
        $("#" + this.SelectTAreaID + " b.rball").each(function (i, n) {
            temp += ($.trim($(n).text()) + splitChar);
        });
        if (temp.substring(temp.length - 1, temp.length) == splitChar && splitChar != "")
            temp = temp.substring(0, temp.length - 1);
        return $.trim(temp);
    },

    //取选中的蓝球号码
    getSelectedBlueBallText: function (splitChar) {
        if (splitChar == undefined)
            return "Please enter the char for split the number";
        var temp = "";
        $("#" + this.SelectBAreaID + " b.bball").each(function (i, n) {
            temp += ($.trim($(n).text()) + splitChar);
        });
        if (temp.substring(temp.length - 1, temp.length) == splitChar && splitChar != "")
            temp = temp.substring(0, temp.length - 1);
        return $.trim(temp);
    },
    //算注数  按照彩种Id升序排序
    GetLotteryInvestNum: function () {
        var InvestNum = 0;
        switch (this.PlayTypeID) {
            case 501: //双色球-普通投注-取注数
                if ((this.getSelectedRedBallCount() < 6) || (this.getSelectedBlueBallCount() < 1)) {
                    InvestNum = 0;
                }
                else {
                    InvestNum = this.Combination(this.getSelectedRedBallCount(), 6) * this.getSelectedBlueBallCount();
                }

                break;
            case 601: //3D-普通投注-取注数

                InvestNum = 1;
                $("#tz_3d ul").each(function (i, n) {
                    InvestNum *= $(n).find("b.rball").length;
                });

                break;
            case 3901: //大乐透-普通投注-取注数
                //追加投注每注3元
                this.PlayTypeID == 3902 ? this.Price = 3 : this.Price = 2;

                if ((this.getSelectedRedBallCount() < 5) || (this.getSelectedBlueBallCount() < 2)) {
                    InvestNum = 0;
                }
                else {
                    InvestNum = this.Combination(this.getSelectedRedBallCount(), 5) * this.Combination(this.getSelectedBlueBallCount(), 2);
                }

                break;
            case 6301: //排列三-普通投注-取注数

                InvestNum = 1;
                $("#" + this.SelectRAreaID + " ul").each(function (i, n) {
                    InvestNum *= $(n).find("b.rball").length;
                });

                break;
            case 6401: //排列五-普通投注-取注数
                InvestNum = 1;
                $("#" + this.SelectRAreaID + " ul").each(function (i, n) {
                    InvestNum *= $(n).find("b.rball").length;
                });

                break;
        }
        this.TotalInvestNum = InvestNum;
    },
    //取选中红球个数,在胆拖玩法中表示胆码的个数
    getSelectedRedBallCount: function () {
        return $("#" + this.SelectRAreaID + " b.rball").length;
    },

    //取选中蓝球个数
    getSelectedBlueBallCount: function () {
        return $("#" + this.SelectBAreaID + " b.bball").length;
    },

    //检测是否完成选号
    CheckFull: function () {

        this.GetLotteryInvestNum();

        if (this.TotalInvestNum < 1) {
            $("#btnAddPick").show();
            $("#div_addToSelector").hide();
        }
        else {
            $("#btnAddPick").hide();
            $("#div_addToSelector").show();
        }

        if (this.LotID == 5 || this.LotID == 39) {
            $(".l_bnzh_p strong").eq(0).html(this.getSelectedRedBallCount());
            $(".l_bnzh_p strong").eq(1).html(this.getSelectedBlueBallCount());
            $(".l_bnzh_p strong").eq(2).html(this.TotalInvestNum);
            $(".l_bnzh_p strong").eq(3).html(this.TotalInvestNum * this.Price);
        }
        else {
            $(".l_bnzh_p strong").eq(0).html(this.TotalInvestNum);
            $(".l_bnzh_p strong").eq(1).html(this.TotalInvestNum * this.Price);
        }
    },

    //机选，Type 表示红、篮球类型，值为1 = r（红）、2 = r（胆）、3 = r（拖）、4 = b（蓝球）
    RandBall: function (Type, MaxNumber, Count) {

        this.ClearSelect(Type);
        MaxNumber = MaxNumber - 1;
        var RorB = (Type == 4 ? "b" : "r");
        var tmpCount = Count;
        var i = 0;
        var BallNum = 0;

        switch (this.LotTypeID) {
            case 1:
            case 2:
                if (Count == 0) {
                    tmpCount = $("#selectRBall").val();
                }

                if (Type == 1) {
                    if (Count == 0) {
                        tmpCount = $("#selectBBall").val();
                    }
                }

                while (i < tmpCount) {
                    BallNum = Lottery.GetRandomNumber(MaxNumber, 1);
                    if ($("#" + (Type == 4 ? this.SelectBAreaID : this.SelectRAreaID) + " b").eq(BallNum).hasClass(RorB + "ball")) {

                        continue;
                    }

                    this.SetBallState($("#" + (Type == 4 ? this.SelectBAreaID : this.SelectRAreaID) + " b").eq(BallNum), Type, true);

                    i++;
                }

                this.CheckFull();
                break;

            case 3: //排列型
                $("#" + this.SelectRAreaID + " ul").each(function (i, n) {
                    BallNum = Lottery.GetRandomNumber(MaxNumber, 2);

                    $(this).find("b").eq(BallNum).addClass("rball");
                });

                this.CheckFull();
                break;
            default:
                break;
        }
    },

    //取随机数
    GetRandomNumber: function (MaxNumber, Type) {
        switch (Type) {
            case 1:
                return parseInt(Math.floor((MaxNumber + 1) * Math.random()));
                break;
            case 2:
                return Math.ceil(Math.random() * MaxNumber) - 1;
                break;
        }
    },

    //计算总注数、总金额
    CalcSumMoney: function () {
        this.TotalMultiple = parseInt($("#tb_Multiple").val(), 10);
        this.TotalMoney = CodeArea.GetLotteryInvestNum() * this.Price * this.TotalMultiple;

        $("#lab_Num").text(CodeArea.GetLotteryInvestNum().toString());
        $("#lab_SumMoney").text(this.TotalMoney.toString());

        $("#lab_ShareMoney").text((this.TotalMoney / StrToInt($("#tb_Share").val())).toFixed(2));
        $("#lab_BuyMoney").text((StrToInt($("#tb_BuyShare").val()) * StrToFloat($("#lab_ShareMoney").text())).toFixed(2));
        $("#lab_AssureMoney").text((StrToInt($("#tb_AssureShare").val()) * StrToFloat($("#lab_ShareMoney").text())).toFixed(2));
    },

    //允许修改 总注数 总金额 以供特殊条件下显示使用，如单式上传时
    CalcInvestAndSumMoneySpecialCase: function (investNum, sumMoney) {
        $("#sumAmount").html("购买注数：<font color='red'>" + investNum + "</font> 注");
        $("#sumMoney").html("购买金额：<font color='red'>" + sumMoney + "</font> 元");
    },

    //投注倍数增加
    MultipleAdd: function () {
        if ($("#Chase").is(":checked")) {
            return;
        }
        parseInt($("#tb_Multiple").val(), 10) >= this.MaxAllowInvestMultiple ? $("#tb_Multiple").val(this.MaxAllowInvestMultiple) : $("#tb_Multiple").val(parseInt($("#tb_Multiple").val(), 10) + 1);
        this.CalcSumMoney();
    },

    //投注倍数减少
    MultipleSub: function () {
        if ($("#Chase").is(":checked")) {
            return;
        }
        parseInt($("#tb_Multiple").val(), 10) <= 1 ? $("#tb_Multiple").val(1) : $("#tb_Multiple").val(parseInt($("#tb_Multiple").val(), 10) - 1);
        this.CalcSumMoney();
    },

    //必须是数字按键
    MustBeDigitKey: function () {
        if (window.event.keyCode < 48 || window.event.keyCode > 57)
            return false;
        return true;
    },
    //检查倍数的输入是否正确
    ValidateInputMultiple: function () {
        if (parseInt($("#tb_Multiple").val(), 10) < 1 || parseInt($("#tb_Multiple").val(), 10) > this.MaxAllowInvestMultiple) {
            alert("倍数必须为小于或者等于" + this.MaxAllowInvestMultiple + "的正整数");
            $("#tb_Multiple").val(1);
        }
    },
    //算组合数，m中取n
    Combination: function (m, n) {
        if (m == 0 || m < n) return 0;
        var InvestNum = 1;
        for (var i = m; i > n; i--) {
            InvestNum *= i;
        }
        var f = 1;
        for (var i = m - n; i > 0; i--) {
            f *= i;
        }

        return InvestNum /= f;
    },

    //算排列数，m中排n
    Permutation: function (m, n) {
        if (m == 0 || m < n) return 0;
        var InvestNum = 1;
        for (var i = m; i > m - n; i--) {
            InvestNum *= i;
        }

        return InvestNum;
    },
    //幸运选号 提交投注
    LuckNumberSubmit: function () {
        this.ClearInvestList();
        ValidateInvestContent($("#HidLuckNumber").val().replace(/\+/g, "#"));
        Lottery.Submit();
    },
    //清空投注列表
    ClearInvestList: function () {
        CodeArea.CodeList = [];
        CodeArea.ShowCodeArea();
    }
};

//投注内容显示框
var CodeArea = {
    init: function () {
        this.CodeList = [];
        this.Code = "";
        this.InvestNum = 0;
        this.ShowContent = "";
        this.PlayName = "复式";

        $("#list_LotteryNumber").html("");
    },

    AddTextToCodeArea: function () {

        if (Lottery.LotID == 82) {
            ShowNumberFormat(this);
        } else {
            var Num = this.Code.split(":").length
            if (Num > 1) {
                var Codes = this.Code.split(":");
                var InvestNums = this.InvestNum.split(":");
                var ShowContents = this.ShowContent.split(":");
                var PlayNames = "单式";

                for (var i = 0; i < Num; i++) {
                    this.CodeList.push({ Code: Codes[i], InvestNum: InvestNums[i], ShowContent: ShowContents[i], PlayName: PlayNames });
                }
            }
            else {
                this.CodeList.push({ Code: this.Code, InvestNum: this.InvestNum, ShowContent: this.ShowContent, PlayName: this.PlayName });
            }

        }
        this.Code = "";
        this.InvestNum = 0;
        this.ShowCodeArea();
        //计算追号任务总额
        Lottery.ChaseCalculateTotalMoney();
    },
    ShowCodeArea: function () {
        var L = Lottery;
        L.TotalInvestNum = 0;
        L.TotalMoney = 0;

        var frag = document.createDocumentFragment();
        $(this.CodeList).each(function (i, n) {
            var li = document.createElement("li");
            li.style.cursor = "pointer";
            li.innerHTML = "<em>" + n.InvestNum + " 注</em>"
                            + "<span betway=\"Single\">" + n.PlayName + " | " + n.ShowContent + "</span>"
                            + "<button type=\"button\" onclick=\"CodeArea.DelCodeArea(" + i + ")\" class=\"delBtn\"></button>";

            frag.appendChild(li);

            L.TotalInvestNum += n.InvestNum;
            L.TotalMoney += n.InvestNum * L.Price * L.TotalMultiple;
        });

        $("#list_LotteryNumber").html("");
        $("#list_LotteryNumber").append(frag);
        L.CalcSumMoney();
    },
    DelCodeArea: function (i) {
        this.CodeList.splice(i, 1);
        this.ShowCodeArea();
        //计算追号任务总额
        Lottery.ChaseCalculateTotalMoney();
    },

    ClearAll: function () {
        this.CodeList = [];
        this.ShowCodeArea();
    },

    GetLotteryInvestNum: function () {

        var LotteryInvestNum = 0;

        $(this.CodeList).each(function (i, n) {
            LotteryInvestNum += parseInt(n.InvestNum);
        });

        return LotteryInvestNum;
    }
};



var lottery = [[5, "SSQ", "双色球", 14], [6, "FC3D", "福彩3D", 30], [39, "DLT", "大乐透", 14], [63, "SZPL3", "排列3", 30], [64, "SZPL5", "排列5", 30], [59, "AHFC15X5", "15选5", 30], [58, "DF6J1", "东方6+1", 14], [13, "QLC", "七乐彩", 14], [9, "TC22X5", "22选5", 30]];

var isuseCount = 14;

function splitScheme(lotteryNumber, LotID) {
    return parent.LotteryPackage.SplitScheme(lotteryNumber, LotID).value;
}

function btn_ClearClick() {

    try {
        $("#list_LotteryNumber").empty()

        $("#list_LotteryNumber").val("");
        $("#HidLotteryNumber").val("");
        //  o_lab_Num1.text("0");
        //  calculateMoney();

        for (var i = 1; i < 5; i++) {
            $("#tdIssue" + i).html(0);
            $("#tdZS" + i).html(0);
            $("#tdTC" + i).val(1);
            $("#tdMoneTC" + i).html(0);
        }
        $("#Period").val("");
        $("#tdZS5").html(0);
        $("#tdTC5").val(1);
        $("#tdMoneTC5").html(0);
        $("#SumMonty").html(0);
        return true;
    }
    catch (e) {
        return false;
    }
}

function MReduction(val) {
    var Share = parseInt($("#tbShare").val());
    if (val == 0) {
        if (Share == 1) {

            return;
        }
        $("#tbShare").val(Share - 1);
        if ($("#tbShare").val() <= 1) {
            $(".l_bnzh_mqjx_a1").removeClass("active");
        }
    }
    else {
        $("#tbShare").val(Share + 1);
        if ($("#tbShare").val() > 1) {
            $(".l_bnzh_mqjx_a1").addClass("active");
        }
    }
    btn_ClearClick();
    iframe_playtypes.btn_2_RandManyClick($("#tbShare").val());
}

function onblurShare(val) {
    if (/\D/.test($(val).val())) {
        $(val).val(1);
    }
    if (parseInt($(val).val()) <= 0 || $(val).val() == "") {
        $(val).val(1);
    }
    if ($(val).val() > 1) {
        $(".l_bnzh_mqjx_a1").addClass("active");
    }
    if ($(val).val() <= 1) {
        $(".l_bnzh_mqjx_a1").removeClass("active");
    }
    btn_ClearClick();
    iframe_playtypes.btn_2_RandManyClick($("#tbShare").val());
}

//倍数

function onblurMultiPle2(Mult, Type) {

    var Mults = $(Mult).val();
    if (Mult.value <= 0) {
        $(Mult).val(1);
    }
    if (Mult.value > 999) {
        $(Mult).val(999);
        Mults = 999;
    }
    if (isNaN(parseInt(Mults))) {
        $(Mult).val(1);
        Mults = 1
    }
    var IsuseCount = parseInt($("#tdIssue" + Type).html());
    if (Type == 5) {
        IsuseCount = parseInt($("#Period").val());
    }
    var Num = parseInt($("#tdZS" + Type).html());
    $("#tdMoneTC" + Type).html(IsuseCount * parseInt(Mults) * 2 * Num);

    if ($("#HidType").val() == Type) {
        $("#HidIsuseCount").val(IsuseCount);
        $("#HidMultiple").val($("#tdTC" + Type).val());
        $("#HidNums").val($("#tdZS" + Type).html());

        $("#HidMoney").val($("#tdMoneTC" + Type).html());

        Result(Type);
    }

}



//倍数
function onblurMultiPle(Mult, Type) {
    var Mults = $(Mult).val();
    if (Mult.value <= 0) {
        $(Mult).val(1);
    }
    if (Mult.value > 999) {
        $(Mult).val(999);

    }
    if (isNaN(parseInt(Mults))) {
        $(Mult).val(1);
    }
}

function onblurPeriod(val) {
    if ($(val).val() <= 0) {
        $(val).val("1");
    }
    // $(val).val($(val).val().replace(/[^\d]/g, '1'));
    if (isNaN(parseInt($(val).val()))) {
        $(val).val("");
        $("#tdMoneTC5").html(0);
        return
    }
    if (parseInt($(val).val()) <= 0) {
        $(val).val("");
        $("#tdMoneTC5").html(0);
        return
    }

    var Period = parseInt($(val).val());
    var Note = parseInt($("#tdZS5").html());

    $("#tdMoneTC5").html(Note * Period * 2);
    if ($("#HidType").val() == '5') {
        Result(5);
    }

    if ($("#oneMonth").attr("checked")) {
        Period = $("#tdIssue1").text();
        $("#HidMultiple").val($("#tdTC1").val());
        $("#HidNums").val($("#tdZS1").html());
        $("#HidMoney").val($("#tdMoneTC1").html());
    }
    else if ($("#oneQuarterly").attr("checked")) {
        Period = $("#tdIssue2").text();
        $("#HidMultiple").val($("#tdTC2").val());
        $("#HidNums").val($("#tdZS2").html());
        $("#HidMoney").val($("#tdMoneTC2").html());
    }
    else if ($("#halfYear").attr("checked")) {
        Period = $("#tdIssue3").text();
        $("#HidMultiple").val($("#tdTC3").val());
        $("#HidNums").val($("#tdZS3").html());
        $("#HidMoney").val($("#tdMoneTC3").html());
    }
    else if ($("#oneYear").attr("checked")) {
        Period = $("#tdIssue4").text();
        $("#HidMultiple").val($("#tdTC4").val());
        $("#HidNums").val($("#tdZS4").html());
        $("#HidMoney").val($("#tdMoneTC4").html());
    }
    else if ($("#custom").attr("checked")) {
        $("#HidMultiple").val($("#tdTC5").val());
        $("#HidNums").val($("#tdZS5").html());
        $("#HidMoney").val($("#tdMoneTC5").html());
    }
    $("#HidIsuseCount").val(Period);


}

function changeBetType(obj) {

    var IsuseCount = $("#tdIssue" + obj).html();
    switch (obj) {
        case 1:
            $("#Packages").html("包月套餐");

            break;
        case 2:
            $("#Packages").html("包季套餐");
            break;
        case 3:
            $("#Packages").html("半年套餐");
            break;
        case 4:
            $("#Packages").html("整年套餐");
            break;
        case 5:
            $("#Packages").html("自定义套餐");
            IsuseCount = $("#Period").val();
            break;
    }

    $("#hdPackages").val(obj);
    $("#HidIsuseCount").val(IsuseCount);
    $("#HidMultiple").val($("#tdTC" + obj).val());
    $("#HidNums").val($("#tdZS" + obj).html());
    $("#HidMoney").val($("#tdMoneTC" + obj).html());
    $("#HidType").val(obj);
    Result(obj);
}

function btnOkchend() {

    if (!$("#chkAgrrement").is(":checked")) {
        alert("请先阅读用户投注协议，谢谢！");
        return false;
    }

    var Period = $("#Period").val();
    if ($("#hdPackages").val() == 5) {
        if (Period == "") {
            alert("自定义注数不未空！")
            return false;
        }

    }

    //订单防重复处理
    $("#dgBtn").hide();
    if ($("#dgBtnDoing").length <= 0) {
        $("#dgBtn").after('<input type="button" value="正在购买中" id="dgBtnDoing" style="background: #666;color: #fff;font-size: 14px;font-weight: bold;text-indent: 0;">');
    }
    else {
        $("#dgBtnDoing").show();
    }

    var lotteryId = $("#HidLotteryID").val();
    var playTypeId = $("#HidPlayTypeID").val();
    var hidType = $("#HidType").val();
    var isuseCount = $("#HidIsuseCount").val();
    var multiple = $("#HidMultiple").val();
    var nums = $("#HidNums").val();
    var betType = $("#HidBetType").val();
    var money = $("#HidMoney").val();
    var tbAuto = $("#tbAutoStopAtWinMoney").is(':checked');
    var lotteryNumber = $("#HidLotteryNumber").val();
    $.ajax({
        type: "POST",
        url: "../ajax/LotteryPackage.ashx",
        dataType: "json",
        cache: false,
        async: true,
        data: "lotteryId=" + lotteryId + "&playTypeId=" + playTypeId + "&hidType=" + hidType + "&isuseCount=" + isuseCount + "&multiple="
         + multiple + "&nums=" + nums + "&betType=" + betType + "&money=" + money + "&tbAuto=" + tbAuto + "&lotteryNumber=" + lotteryNumber,
        success: function (data) {
            //            window.location.href = "LotteryPackage.aspx";
            if (data.error > 0) {
                var typeid = 2;
                window.location.href = "/Home/Room/UserBuySuccess.aspx?lotteryid=" + lotteryId + "&type=" + typeid + "&money=" + money + "&schemeid=" + data.error + "&hid=1";
                return;
            } else if (data.error == -808) {
                document.getElementById("lotterySubmit").click();
                //订单防重复处理
                $("#dgBtn").show();
                if ($("#dgBtnDoing").length > 0) {
                    $("#dgBtnDoing").hide();
                }
                return;
            } else if (data.error == -5) {
                var okfunc = function () {
                    window.location.href = "/Home/Room/OnlinePay/Alipay02/Send_Default.aspx"
                    //订单防重复处理
                    $("#dgBtn").show();
                    if ($("#dgBtnDoing").length > 0) {
                        $("#dgBtnDoing").hide();
                    }
                };
                var cancelfunc = function () {
                    //订单防重复处理
                    $("#dgBtn").show();
                    if ($("#dgBtnDoing").length > 0) {
                        $("#dgBtnDoing").hide();
                    }
                };
                confirm("用户余额不足,是否跳转到充值页面?", okfunc, cancelfunc);
                return;
            }
            else {
                alert(data.msg);
                //订单防重复处理
                $("#dgBtn").show();
                if ($("#dgBtnDoing").length > 0) {
                    $("#dgBtnDoing").hide();
                }
                return;
            }
        }
    });
}

function GetLuckNumber() {
    var BetType = $("#hdLuckyType").val();
    var name = "";
    switch (BetType) {
        case "0":
            var XiZuo = $("#ddlSX").val();
            if (XiZuo == "") {
                alert("请选择生肖！");
                return false;
            }
            name = XiZuo;
            break;
        case "1":
            var ddlSX = $("#ddlXiZuo").val();
            if (ddlSX == "") {
                alert("请选择星座！");
                return false;
            }
            name = ddlSX;
            break;
        case "2":
            // var date = $("#tbDate").val();

            var date = $("#ddlYear").val() + "-" + $("#ddlMonth").val() + "-" + $("#ddlDay").val();

            if (date == "") {
                alert("请输入出生日期！");
                $("#tbDate").focus();
                return false;
            }
            name = date;
            break;
        case "3":
            name = $("#tbName").val();

            if (name == "" || name == "支持中英文") {
                alert("请输入姓名！");
                $("#tbName").focus();
                return false;
            } break;
    }

    var v = LotteryPackage.GenerateLuckLotteryNumber(parseInt($("#HidLotteryID").val()), BetType, name, parseInt($("#hdLuckyNum").val())).value;

    iframe_playtypes.btn_2_AddClick(v);
    //    debugger
    //      if (v.split("|").length < 2) {
    //       
    //        $("#tbDate").val("");
    //    } else {

    //  //  alert(v);
    //        iframe_playtypes.btn_2_AddClick(v);
    //    }

}

function Result(obj) {
    $("#SumMonty").html($("#tdMoneTC" + obj).html());
}

function DelRows(obj) {
    var index = parseInt($(obj).parent().parent().parent().find("li").length) - parseInt($(obj).parent().parent().prevAll("li").length) - 1;
    var Numlist = $("#HidLotteryNumber").val().split("\n");
    var Numstr = "";
    for (var i = 0; i < Numlist.length; i++) {
        if (i != index) {
            Numstr += Numlist[i] + "\n";
        }
    }
    Numstr = Numstr.substring(0, Numstr.length - 1);
    $(obj).parent().parent().remove();
    $("#HidLotteryNumber").val(Numstr);

    var Mult1 = parseInt($(window.parent.document).find("#tdTC1").val());
    var Mult2 = parseInt($(window.parent.document).find("#tdTC2").val());
    var Mult3 = parseInt($(window.parent.document).find("#tdTC3").val());
    var Mult4 = parseInt($(window.parent.document).find("#tdTC4").val());

    var IsuseCount = parseInt($(window.parent.document).find("#HidCount").val());
    var TC1 = $(window.parent.document).find("#tdMoneTC1");
    var TC2 = $(window.parent.document).find("#tdMoneTC2");
    var TC3 = $(window.parent.document).find("#tdMoneTC3");
    var TC4 = $(window.parent.document).find("#tdMoneTC4");

    TC1.html(parseInt(TC1.html()) - (IsuseCount * 2) * Mult1);
    TC2.html(parseInt(TC2.html()) - (IsuseCount * 3 * 2) * Mult2);
    TC3.html(parseInt(TC3.html()) - (IsuseCount * 6 * 2) * Mult3);
    TC4.html(parseInt(TC4.html()) - (IsuseCount * 12 * 2) * Mult4);

    var ZS1 = $(window.parent.document).find("#tdZS1");
    var ZS2 = $(window.parent.document).find("#tdZS2");
    var ZS3 = $(window.parent.document).find("#tdZS3");
    var ZS4 = $(window.parent.document).find("#tdZS4");
    var ZS5 = $(window.parent.document).find("#tdZS5");

    ZS1.html(parseInt(ZS1.html()) - 1);
    ZS2.html(parseInt(ZS2.html()) - 1);
    ZS3.html(parseInt(ZS3.html()) - 1);
    ZS4.html(parseInt(ZS4.html()) - 1);
    ZS5.html(parseInt(ZS5.html()) - 1);

    $(window.parent.document).find("#HidNums").val(ZS1.html());
    $(window.parent.document).find("#HidMoney").val(TC1.html());

    var Type = $(window.parent.document).find("#HidType").val();

    $(window.parent.document).find("#SumMonty").html($(window.parent.document).find("#tdMoneTC" + Type).html());
}
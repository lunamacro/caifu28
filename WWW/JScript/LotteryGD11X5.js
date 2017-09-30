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
    SelectBTAreaID: "",  //选好区快ID（后区拖码）
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
        //-----处理3d、排列三的组选单式选球，双击变蓝色--------begin
        if (Lottery.PlayTypeID == 6302 || Lottery.PlayTypeID == 602) {
            //选球个数的判断
            if (this.getSelectedRedBallCount() == 3 || (this.getSelectedRedBallCount() == 1 && this.getSelectedBlueBallCount() == 1)) {
                //msg("只能是单式投注");
                if (this.GetBallState(obj, 1)) {
                    obj.removeClass("rball");
                } else if (this.GetBallState(obj, 4)) {
                    obj.removeClass("bball");
                }
                this.CheckFull();
                return;
            }
            if (this.GetBallState(obj, 1)) {
                obj.removeClass("rball");
                obj.addClass("bball");
            } else if (this.GetBallState(obj, 4)) {
                obj.removeClass("bball");
            } else {
                obj.addClass("rball");
            }

            this.CheckFull();
            return;
        }
        //-----处理3d、排列三的组选单式选球，双击变蓝色--------end

        //-----任九场 场数限制------------------start
        var team_count = 0;
        $("#tz_rjc ul").each(function (i, n) {
            if ($(n).find("b.rball").length > 0) {
                team_count++;
            }
        });
        if (team_count >= 9 && obj.parent().find("b.rball").length < 1) {
            msg("最多只能选择9场比赛进行投注");
            return;
        }
        //-----任九场 场数限制------------------end

        //-----时时乐、重庆时时彩 组三------------------start
        if (this.PlayTypeID == 2905 || this.PlayTypeID == 2811) {
            if (Lottery.GetBallState(obj, 4)) {
                obj.removeClass("bball");
                obj.removeClass("rball");
            } else if (obj.parent().find("b.rball").length >= 1 && obj.parent().find("b.bball").length == 1) {
                if (this.GetBallState(obj, 1)) {
                    obj.removeClass("rball");
                } else if (this.GetBallState(obj, 4)) {
                    obj.removeClass("bball");
                    obj.addClass("rball");
                } else {
                    obj.addClass("rball");
                    $("#divBall b.bball").removeClass("bball").addClass("rball");
                }
            } else if (obj.parent().find("b.rball").length <= 2 && obj.parent().find("b.bball").length == 0) {
                if (this.GetBallState(obj, 1)) {
                    obj.removeClass("rball");
                    obj.addClass("bball");
                } else {
                    obj.addClass("rball");
                }
            } else {
                if (Lottery.GetBallState(obj, 1)) {
                    obj.removeClass("rball");
                } else {
                    obj.addClass("rball");
                }
            }

            this.CheckFull();
            return;
        }

        //组选12普通投注和组选4普通投注双击球变蓝色
        if (Lottery.PlayTypeID == 6809 || Lottery.PlayTypeID == 6814) {
            if (this.GetBallState(obj, 4)) {
                obj.removeClass("bball");
                obj.removeClass("rball");
            }
            else if (obj.parent().find("b.bball").length < 1) {
                if (($("#div_zuxuanputongtouzhu").find("b.rball").length <= 3 && Lottery.PlayTypeID == 6809) || ($("#div_zuxuanputongtouzhu").find("b.rball").length <= 2 && Lottery.PlayTypeID == 6814)) {
                    if (this.GetBallState(obj, 1)) {
                        obj.removeClass("rball");
                        obj.addClass("bball");

                    } else if (this.GetBallState(obj, 4)) {
                        obj.removeClass("bball");
                    } else {
                        obj.addClass("rball");
                    }
                }
                else {
                    if (this.GetBallState(obj, 1)) {
                        obj.removeClass("rball");
                    }
                    else {
                        obj.addClass("rball");
                    }
                }
            }
            else {
                if ($("#div_zuxuanputongtouzhu").find("b.rball").length <= 2 || $("#div_zuxuanputongtouzhu").find("b.rball").length <= 1) {
                    if (this.GetBallState(obj, 1)) {
                        obj.parent().find("b.bball").addClass("rball");
                        obj.parent().find("b.bball").removeClass("bball");
                        obj.removeClass("rball");
                        obj.addClass("bball");

                    } else if (this.GetBallState(obj, 4)) {
                        obj.removeClass("bball");
                    } else {
                        obj.addClass("rball");
                    }
                }
                else {
                    if (this.GetBallState(obj, 1)) {
                        obj.removeClass("rball");
                    }
                    else {
                        obj.addClass("rball");
                        if (obj.parent().find("b.bball").length == 1) {
                            if ((Lottery.PlayTypeID == 6809 && $("#div_zuxuanputongtouzhu").find("b.rball").length > 2) || (Lottery.PlayTypeID == 6814 && $("#div_zuxuanputongtouzhu").find("b.rball").length > 1)) {
                                obj.parent().find("b.bball").addClass("rball");
                                obj.parent().find("b.bball").removeClass("bball");
                            }
                        }

                    }
                }
                if (obj.parent().find("b.bball").length == 1) {
                    if ((Lottery.PlayTypeID == 6809 && $("#div_zuxuanputongtouzhu").find("b.rball").length > 2) || (Lottery.PlayTypeID == 6814 && $("#div_zuxuanputongtouzhu").find("b.rball").length > 1)) {
                        obj.parent().find("b.bball").addClass("rball");
                        obj.parent().find("b.bball").removeClass("bball");
                    }
                }
            }

            this.CheckFull();
            return;
        }
        //-----时时乐 组三------------------end
        //-----重庆时时彩 五星复选----------start
        if (Lottery.PlayTypeID == 2802) {
            if (this.GetBallState(obj, 1)) {
                obj.removeClass("rball");
            } else {
                obj.parent().children("b.rball").removeClass("rball");
                obj.addClass("rball");
            }

            this.CheckFull();
            return;
        }

        //----大乐透--------------------start
        if (Lottery.PlayTypeID == 3903 || Lottery.PlayTypeID == 3904) {
            if ($("#tz_bball .bball").size() > 1 && $(obj).parent().parent().attr("id") == "tz_bball") {
                alert("后区胆码只能选2个");
                return;
            }
        }

        //----大乐透--------------------end

        //-----重庆时时彩 五星复选----------end
        var RorB = (Type == 4 ? "b" : "r");

        var Selected = this.GetBallState(obj, Type);
        if (Selected) {
            obj.removeClass(RorB + "ball");

            this.CheckFull();
            return;
        }

        //----大乐透--------------------start
        if (Lottery.PlayTypeID == 3903 || Lottery.PlayTypeID == 3904) {
            if ($("#tz_bball .bball").size() > 1 && $(obj).parent().parent().attr("id") == "tz_bball") {
                alert("后区胆码只能选2个");
                return;
            }
        }

        if (Lottery.PlayTypeID == 3906) {
            if (this.GetBallState(obj, Type)) {
                obj.removeClass(RorB + "ball");

                this.CheckFull();
                return;
            }
            else {
                var qqdm = $("#tz_rball b[class='rball']"); //前区胆码
                var hqdm = $("#tz_rtball b[class='bball']"); //后区胆码
                var hqtm = $("#tz_bball b[class='bball']"); //后区拖码
                var objId = obj.parent().parent().attr("id");

                if (objId == "tz_rball") {
                    if (qqdm.length >= 5) {
                        msg("最多只允许选5个前区号码");
                        return;
                    }
                    else {
                        obj.addClass("rball");
                        this.CheckFull();
                        return;
                    }
                }
                if (objId == "tz_rtball") {
                    if (hqdm.length >= 1) {
                        msg("最多只允许选1个后区胆码");
                        return;
                    }
                    else {
                        obj.addClass("bball");
                        $("#tz_bball b").eq(parseInt(obj.text() - 1)).removeClass("bball");
                        this.CheckFull();
                        return;
                    }
                }
                if (objId == "tz_bball") {
                    if (hqtm.length >= 11) {
                        msg("最多只允许选11个后区拖码");
                        return;
                    }
                    else {
                        obj.addClass("bball");
                        $("#tz_rtball b").eq(parseInt(obj.text() - 1)).removeClass("bball");
                        this.CheckFull();
                        return;
                    }
                }
            }
        }

        if (Lottery.PlayTypeID == 3907) {
            if ($("#tz_bball .bball").size() >= 1 && $(obj).parent().parent().attr("id") == "tz_bball") {
                alert("后区胆码只能选1个");
                return;
            }
        }
        //----大乐透--------------------end 

        var DRedCount = 0;
        var TRedCount = 0; //拖码
        var RedCount = 0;
        var BlueCount = 0;

        switch (this.LotTypeID) {
            case 1:
            case 2:
                DRedCount = this.getSelectedRedBallCount();
                TRedCount = this.getSelectedTuoBallCount(); //拖码
                RedCount = DRedCount + TRedCount;
                BlueCount = this.getSelectedBlueBallCount();

                //红球个数判断
                if (Type != 4) {
                    if (Type == 2) {
                        if (this.MaxDBallCount > 0 && DRedCount >= this.MaxDBallCount) {
                            msg("最多允许选" + this.MaxDBallCount + "个胆码");
                            return;
                        }
                    }

                    if (Type == 1 || Type == 2 || Type == 3) {
                        if (this.RedBallCount > 0) {
                            if (RedCount >= this.RedBallCount) {
                                msg("红球最多允许选" + this.RedBallCount + "个");
                                return;
                            }
                        }
                    }
                }

                //胆拖选号互斥
                if (this.MaxDBallCount > 0) {
                    if (obj.parent().parent()[0].id == "" + this.SelectRAreaID + "") {
                        $("#" + this.SelectTAreaID + " b").eq(parseInt(obj.text(), 10) - 1).removeClass("rball");
                    }
                    else if (obj.parent().parent()[0].id == "" + this.SelectTAreaID + "") {
                        $("#" + this.SelectRAreaID + " b").eq(parseInt(obj.text(), 10) - 1).removeClass("rball");
                    }
                }
                //后区胆拖选号互斥
                if (Lottery.PlayTypeID == 3906 || Lottery.PlayTypeID == 3907 || Lottery.PlayTypeID == 3908 || Lottery.PlayTypeID == 3909) {
                    if (obj.parent().parent()[0].id == "" + this.SelectBAreaID + "") {
                        //后区胆码
                        $("#" + this.SelectBTAreaID + " b").eq(parseInt(obj.text(), 10) - 1).removeClass("bball");
                    }
                    else if (obj.parent().parent()[0].id == "" + this.SelectBTAreaID + "") {
                        //后区拖码
                        $("#" + this.SelectBAreaID + " b").eq(parseInt(obj.text(), 10) - 1).removeClass("bball");
                    }
                }


                //蓝球个数判断
                if (this.BlueBallCount > 0) {
                    if (BlueCount >= this.BlueBallCount) {
                        msg("蓝球最多允许选" + this.BlueBallCount + "个");
                        return;
                    }
                }

                break;
        }

        //-------------------幸运武林场选-----------------------------//
        if (Lottery.PlayTypeID == 8201) {
            this.SetBallState(obj, 3, true);
            var count = GetBallSelectedNum(false);
            if (count > 1) {
                this.SetBallState(obj, 3, false);
                alert("只能选择一场比赛投注");
                this.CheckFull();
                return;
            }
        }

        if (Lottery.PlayTypeID == 8202) {
            this.SetBallState(obj, 3, true);
            var count = GetBallSelectedNum(false);
            if (count > 2) {
                this.SetBallState(obj, 3, false);
                alert("只能选择两场比赛投注");
                this.CheckFull();
                return;
            }

        }
        //-----------------------武林英雄场选
        if (Lottery.PlayTypeID == 8214) {
            var wlyx_ltfy = "c1";  // 擂台风云 投注方式
            var wlyx_ltfy_arr = $("#wlyx_cx_play input");
            for (var i = 0; i < wlyx_ltfy_arr.length; i++) {
                if ($(wlyx_ltfy_arr[i]).is(":checked")) {
                    wlyx_ltfy = $(wlyx_ltfy_arr[i]).val();
                    break;
                }
            }
            if (wlyx_ltfy == "c1") {
                this.SetBallState(obj, 3, true);
                var count = GetBallSelectedNum(true);
                if (count > 1) {
                    this.SetBallState(obj, 3, false);
                    alert("只能选择一场比赛投注");
                    this.CheckFull();
                    return;
                }
            } else if (wlyx_ltfy == "c2") {
                this.SetBallState(obj, 3, true);
                var count = GetBallSelectedNum(true);
                if (count > 2) {
                    this.SetBallState(obj, 3, false);
                    alert("只能选择二场比赛投注");
                    this.CheckFull();
                    return;
                }
            }



        }
        //---------------------幸运武林结束--------------------------------------//

        //---------------------江苏快3 start  ----------------------------------//
        if (Lottery.PlayTypeID == 8302 || Lottery.PlayTypeID == 8308) {
            if (obj.hasClass("btn_selected")) {
                obj.removeClass("rball btn_selected");
                obj.removeClass("rball");
                obj.addClass("btn_no_selected");
            } else {
                obj.removeClass("btn_no_selected");
                obj.removeClass("rball");
                obj.addClass("rball btn_selected");
            }
            this.CheckFull();
            return;
        }
        //---------------------江苏快3 End  ----------------------------------//


        //---------------------广西快3 start  ----------------------------------//
        if (Lottery.PlayTypeID == 8902 || Lottery.PlayTypeID == 8908) {
            if (obj.hasClass("btn_selected")) {
                obj.removeClass("rball btn_selected");
                obj.removeClass("rball");
                obj.addClass("btn_no_selected");
            } else {
                obj.removeClass("btn_no_selected");
                obj.removeClass("rball");
                obj.addClass("rball btn_selected");
            }
            this.CheckFull();
            return;
        }
        //---------------------广西快3 End  ----------------------------------//

        //---------------------快赢481 Start-------------------------------------//
        if (Lottery.PlayTypeID == 6808) {
            //组选24胆拖
            this.SetBallState(obj, 3, true);
            var danNum = $("#div_dantuo_chongdantuo .tz_ball").eq(0).find("b.rball").text();
            if (danNum.length > 3) {
                this.SetBallState(obj, 3, false);
                alert('最多只能选择3个胆码');
                this.CheckFull();
                return;
            }
        }
        if (Lottery.PlayTypeID == 6810) {
            //组选12胆拖
            this.SetBallState(obj, 3, true);
            var danNum = $("#div_dantuo_chongdantuo .tz_ball").eq(0).find("b.rball").text();
            if (danNum.length > 2) {
                this.SetBallState(obj, 3, false);
                alert('最多只能选择2个胆码');
                this.CheckFull();
                return;
            }
        }




        if (Lottery.PlayTypeID == 6811 || Lottery.PlayTypeID == 6813 || Lottery.PlayTypeID == 6815 || Lottery.PlayTypeID == 6816) {
            //组选12重胆拖、组6胆拖、组4胆拖、组4重胆拖
            this.SetBallState(obj, 3, true);
            var danNum = $("#div_dantuo_chongdantuo .tz_ball").eq(0).find("b.rball").text();
            if (danNum.length > 1) {
                this.SetBallState(obj, 3, false);
                alert('最多只能选择1个胆码');
                this.CheckFull();
                return;
            }
        }

        //胆拖码互斥
        if (Lottery.LotID == 68) {
            this.SetBallState(obj, 3, true);
            if (obj.parent().parent().parent()[0].id == "tz_danma") {
                $("#tz_tuoma b").eq(parseInt(obj.text(), 10) - 1).removeClass("rball");
            }
            if (obj.parent().parent().parent()[0].id == "tz_tuoma") {
                $("#tz_danma b").eq(parseInt(obj.text(), 10) - 1).removeClass("rball");
            }
        }
        if (this.PlayTypeID == 8305) {//江苏肃快3二同号单选胆拖互斥 
            if (obj.parent().parent()[0].id == "tz_ertonghaodanxuan") {
                $("#tz_erbutonghao b").eq((parseInt(obj.text(), 10) > 6 ? parseInt(obj.text(), 10) % 10 : parseInt(obj.text(), 10)) - 1).removeClass("rball");
            }
            else if (obj.parent().parent()[0].id == "tz_erbutonghao") {
                $("#tz_ertonghaodanxuan b").eq((parseInt(obj.text(), 10) > 6 ? parseInt(obj.text(), 10) % 10 : parseInt(obj.text(), 10)) - 1).removeClass("rball");
            }
        }
        //---------------------快赢481 End---------------------------------------//

        //---------------------河南22选5 Start ---------------------------------//
        if (Lottery.PlayTypeID == 6901) {
            this.SetBallState(obj, 3, true);
            var selectedCount = this.getSelectedRedBallCount();
            if (selectedCount >= 15) {
                this.SetBallState(obj, 3, false);
                alert("最多只能选择14个号码");
                this.CheckFull();
                return;
            }
        }
        //---------------------河南22选5 End  ----------------------------------//
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
                obj.parent().parent().parent().find("b").addClass("rball");
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

    //快速选号 江西11选5
    QuickSelect_jx11x5: function (obj) {
        var Type = obj.attr("rel");
        obj.parent().parent().parent().find("b").removeClass("rball");
        switch (Type) {
            case "Q":
                if ((this.LotID == 70 || this.LotID == 62 || this.LotID == 78 || this.LotID == 88) && investType == 2) {
                    //十一运夺金、江西11选5、广东11选5
                    $(".tz_hmbox b").each(function (i, n) {
                        if (!$(n).hasClass("rball")) {
                            $($(".tzrgbox b")[i]).addClass("rball");
                        }
                    });
                }
                else {
                    obj.parent().parent().parent().find("b").addClass("rball");
                }

                break;
            case "D":
                if ((this.LotID == 70 || this.LotID == 62) && investType == 2) {
                    $(".tz_hmbox").parent().parent().parent().find("b:gt(4)").removeClass("rball");
                }
                obj.parent().parent().parent().find("b:gt(4)").addClass("rball");
                break;
            case "X":
                if ((this.LotID == 70 || this.LotID == 62) && investType == 2) {
                    $(".tz_hmbox").parent().parent().parent().find("b:lt(5)").removeClass("rball");
                }
                obj.parent().parent().parent().find("b:lt(5)").addClass("rball");
                break;
            case "J":
                if ((this.LotID == 70 || this.LotID == 62) && investType == 2) {
                    $(".tz_hmbox").parent().parent().parent().find("b:odd").removeClass("rball");
                }
                obj.parent().parent().parent().find("b:odd").addClass("rball");
                break;
            case "O":
                if ((this.LotID == 70 || this.LotID == 62) && investType == 2) {
                    $(".tz_hmbox").parent().parent().parent().find("b:even").removeClass("rball");
                }
                obj.parent().parent().parent().find("b:even").addClass("rball");
                break;
            case "Z":
                var zs = new Array("0", "1", "2", "4", "6", "10");
                for (var i = 0; i < zs.length; i++) {
                    obj.parent().parent().parent().find("b:eq(" + zs[i] + ")").addClass("rball");
                }
                break;
            case "H":
                var hs = new Array("3", "5", "7", "8", "9");
                for (var i = 0; i < hs.length; i++) {
                    obj.parent().parent().parent().find("b:eq(" + hs[i] + ")").addClass("rball");
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

                if (this.SelectTAreaID != "" && this.getSelectedTuoBallCount() > 0) {
                    $("#" + this.SelectTAreaID + " b").removeClass("rball");
                }
                if (this.SelectTAreaID != "" && this.getSelectedBlueBallCount() > 0) {
                    $("#" + this.SelectBAreaID + " b").removeClass("bball");
                }
                if (this.SelectTAreaID != "" && this.getSelectedBlueBTAreaBallCount() > 0) {
                    $("#" + this.SelectBTAreaID + " b").removeClass("bball");
                }
                if (Lottery.PlayTypeID == "3906") {
                    if (this.SelectTAreaID != "" && $("#tz_rtball .bball").length > 0) {
                        $("#" + this.SelectTAreaID + " b").removeClass("bball");
                    }
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
        $("#sumAmount").html("购买注数：<font color='red'>0</font> 注");
        $("#sumMoney").html("购买金额：<font color='red'>0</font> 元");
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


            case 201: //四场进球彩-普通投注-取投注内容
                $("#tz_jqc ul").each(function (i, n_ul) {
                    var temp = "";
                    $(n_ul).find("b.rball").each(function (i, n_b) {
                        temp += $.trim($(n_b).text()) == "3+" ? "3" : $.trim($(n_b).text());
                    });
                    if (temp.length > 1) {
                        temp = "(" + temp + ")";
                    }
                    tmpLotteryNumber += temp;
                });
                break;
            case 301:
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
            case 501:   //双色球单复式-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += " # ";
                tmpLotteryNumber += this.getSelectedBlueBallText(" ");
                break;
            case 502:   //双色球胆拖-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += " , ";
                tmpLotteryNumber += this.getSelectedTuoBallText(" ");
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

            case 602: //3D-组选单式-取投注内容
                if (this.getSelectedRedBallCount() == 3) {
                    tmpLotteryNumber = this.getSelectedRedBallText("");
                } else if (this.getSelectedRedBallCount() == 1 && this.getSelectedBlueBallCount() == 1) {
                    if (parseInt(this.getSelectedRedBallText(""), 10) < parseInt(this.getSelectedBlueBallText(""), 10)) {
                        tmpLotteryNumber = this.getSelectedRedBallText("") + this.getSelectedBlueBallText("") + this.getSelectedBlueBallText("");
                    } else {
                        tmpLotteryNumber = this.getSelectedBlueBallText("") + this.getSelectedBlueBallText("") + this.getSelectedRedBallText("");
                    }
                }
                break;
            case 603: //3D-组6复式-取投注内容
            case 604: //3D-组3复式-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText("");
                break;
            case 605: //3D-直选和值-取投注内容
            case 606: //3D-组选和值-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(",");
                break;
            case 901:  //22选5普通投注-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                break;

            case 1301: //七乐彩普通投注-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                break;
            case 1501: //六场半全场-普通投注-取投注内容
                $("#tz_lcbqc ul").each(function (i, n_ul) {
                    var temp = "";
                    $(n_ul).find("b.rball").each(function (i, n_b) {
                        temp += $.trim($(n_b).text());
                    });
                    if (temp.length > 1) {
                        temp = "(" + temp + ")";
                    }
                    tmpLotteryNumber += temp;
                });
                break;
            case 2802: //重庆时时彩-五星复选-取投注内容
            case 2803: //重庆时时彩-五、三、二、一星直选-取投注内容
            case 2805: //重庆时时彩-五星通选-取投注内容
                $("#tz_cqssc ul:visible").each(function (i, n_ul) {
                    var temp = "";
                    $(n_ul).find("b.rball").each(function (i, n_b) {
                        temp += $.trim($(n_b).text());
                    });
                    if (temp.length > 1) {
                        temp = "(" + temp + ")";
                    }
                    tmpLotteryNumber += temp;
                });
                for (var i = 0; i < 5 - $("#tz_cqssc ul:visible").length; i++) {
                    tmpLotteryNumber = "-" + tmpLotteryNumber;
                }
                break;
            case 2804: //重庆时时彩-大小单双-取投注内容
                $("#div_daxiao_danshuang ul").each(function (i, n_ul) {
                    var temp = "";
                    $(n_ul).find("b.rball").each(function (i, n_b) {
                        temp += $.trim($(n_b).text()) + ",";
                    });
                    temp = temp.substr(0, temp.length - 1);
                    temp = temp + "@";
                    tmpLotteryNumber += temp;
                });
                tmpLotteryNumber = tmpLotteryNumber.substr(0, tmpLotteryNumber.length - 1);
                break;
            case 2806: //重庆时时彩-二星组合-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText("");
                break;
            case 2808: //重庆时时彩-二星包点-取投注内容
            case 2810: //重庆时时彩-三星直选包点-取投注内容
            case 2815: //重庆时时彩-三星组选包点-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(",");
                break;
            case 2811: //重庆时时彩-三星组三-取投注内容
                if (this.getSelectedRedBallCount() >= 3) {
                    tmpLotteryNumber = this.getSelectedRedBallText("");
                } else if (this.getSelectedRedBallCount() == 1 && this.getSelectedBlueBallCount() == 1) {
                    if (parseInt(this.getSelectedRedBallText(""), 10) < parseInt(this.getSelectedBlueBallText(""), 10)) {
                        tmpLotteryNumber = this.getSelectedRedBallText("") + this.getSelectedBlueBallText("") + this.getSelectedBlueBallText("");
                    } else {
                        tmpLotteryNumber = this.getSelectedBlueBallText("") + this.getSelectedBlueBallText("") + this.getSelectedRedBallText("");
                    }
                }
                break;
            case 2812: //重庆时时彩-三星组六-取投注内容 
            case 2813: //重庆时时彩-三星直选组合-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText("");
                break;
            case 2814: //重庆时时彩-三星包胆-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(",");
                break;
            case 2902: //
                $("#tz_ssl ul").each(function (i, n_ul) {
                    var temp = "";
                    $(n_ul).find("b.rball").each(function (i, n_b) {
                        temp += $.trim($(n_b).text());
                    });
                    if (temp.length > 1) {
                        temp = "(" + temp + ")";
                    }
                    tmpLotteryNumber += temp;
                });
                break;
            case 2904: //时时乐-组六-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText("");
                break;
            case 2905: //时时乐-组三-取投注内容
                if (this.getSelectedRedBallCount() >= 3) {
                    tmpLotteryNumber = this.getSelectedRedBallText("");
                } else if (this.getSelectedRedBallCount() == 1 && this.getSelectedBlueBallCount() == 1) {
                    if (parseInt(this.getSelectedRedBallText(""), 10) < parseInt(this.getSelectedBlueBallText(""), 10)) {
                        tmpLotteryNumber = this.getSelectedRedBallText("") + this.getSelectedBlueBallText("") + this.getSelectedBlueBallText("");
                    } else {
                        tmpLotteryNumber = this.getSelectedBlueBallText("") + this.getSelectedBlueBallText("") + this.getSelectedRedBallText("");
                    }
                }
                break;
            case 2906: //时时乐-直选和值-取投注内容
            case 2907: //时时乐-组选和值-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(",");
                break;

            case 2908: //时时乐-前2-取投注内容
            case 2909: //时时乐-后2-取投注内容
                $("#div_qianhou2 ul").each(function (i, n_ul) {
                    var temp = "";
                    $(n_ul).find("b.rball").each(function (i, n_b) {
                        temp += $.trim($(n_b).text());
                    });
                    if (temp.length > 1) {
                        temp = "(" + temp + ")";
                    }
                    tmpLotteryNumber += temp;
                });
                break;
            case 2910: //时时乐-前1-取投注内容
            case 2911: //时时乐-后1-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText("");
                break;

            case 3901:  //大乐透普通投注-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += " # ";
                tmpLotteryNumber += this.getSelectedBlueBallText(" ");
                break;
            case 3902:  //大乐透追加普通-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += " # ";
                tmpLotteryNumber += this.getSelectedBlueBallText(" ");
                break;
            case 3903:  //大乐透胆拖-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += " , ";
                tmpLotteryNumber += this.getSelectedTuoBallText(" ");
                tmpLotteryNumber += " # ";
                tmpLotteryNumber += this.getSelectedBlueBallText(" ");
                break;
            case 3904:  //大乐透追加胆拖-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += " , ";
                tmpLotteryNumber += this.getSelectedTuoBallText(" ");
                tmpLotteryNumber += " # ";
                tmpLotteryNumber += this.getSelectedBlueBallText(" ");
                break;
            case 3906:
            case 3908:
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += " # ";
                tmpLotteryNumber += this.getSelectedTuoBlueDMBallText(" ");
                tmpLotteryNumber += " , ";
                tmpLotteryNumber += this.getSelectedBlueBallText(" ");
                break;
            case 3907:
            case 3909:
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += " , ";
                tmpLotteryNumber += this.getSelectedTuoBallText(" ");
                tmpLotteryNumber += " # ";
                if ($("#tz_bball .bball").length > 0) {
                    tmpLotteryNumber += this.getSelectedBlueBallText(" ")
                    tmpLotteryNumber += " , ";
                }
                tmpLotteryNumber += this.getSelectedBlueBTAreaBallText(" ")
                break;
            case 6201: //十一运夺金-任选一普通投注-取投注内容
            case 6202: //十一运夺金-任选二普通投注-取投注内容
            case 6203: //十一运夺金-任选三普通投注-取投注内容
            case 6204: //十一运夺金-任选四普通投注-取投注内容
            case 6205: //十一运夺金-任选五普通投注-取投注内容
            case 6206: //十一运夺金-任选六普通投注-取投注内容
            case 6207: //十一运夺金-任选七普通投注-取投注内容
            case 6208: //十一运夺金-任选八普通投注-取投注内容
            case 6211: //十一运夺金-前二组选-取投注内容
            case 6212: //十一运夺金-前三组选-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                break;

            case 6213: //十一运夺金-前二胆拖投注-取投注内容
            case 6214: //十一运夺金-前三胆拖投注-取投注内容
            case 6215: //十一运夺金-任选二胆拖投注-取投注内容
            case 6216: //十一运夺金-任选三胆拖投注-取投注内容
            case 6217: //十一运夺金-任选四胆拖投注-取投注内容
            case 6218: //十一运夺金-任选五胆拖投注-取投注内容
            case 6219: //十一运夺金-任选六胆拖投注-取投注内容
            case 6220: //十一运夺金-任选七胆拖投注-取投注内容
            case 6221: //十一运夺金-任选八胆拖投注-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += " , ";
                tmpLotteryNumber += this.getSelectedTuoBallText(" ");
                break;
            case 6209: //十一运夺金-前二直选-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += ",";
                tmpLotteryNumber += this.getSelectedTuoBallText(" ");
                break;
            case 6210: //十一运夺金-前三直选-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += ",";
                tmpLotteryNumber += this.getSelectedTuoBallText(" ");
                tmpLotteryNumber += ",";
                $("#tz_qian3zhi").find("b.rball").each(function (i, n) {
                    tmpLotteryNumber += $.trim($(n).text()) + " ";
                });
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
            case 6302: //排列三-组选单式-取投注内容
                if (this.getSelectedRedBallCount() == 3) {
                    tmpLotteryNumber = this.getSelectedRedBallText("");
                } else if (this.getSelectedRedBallCount() == 1 && this.getSelectedBlueBallCount() == 1) {
                    if (parseInt(this.getSelectedRedBallText(""), 10) < parseInt(this.getSelectedBlueBallText(""), 10)) {
                        tmpLotteryNumber = this.getSelectedRedBallText("") + this.getSelectedBlueBallText("") + this.getSelectedBlueBallText("");
                    } else {
                        tmpLotteryNumber = this.getSelectedBlueBallText("") + this.getSelectedBlueBallText("") + this.getSelectedRedBallText("");
                    }
                }
                break;
            case 6303: //排列三-组6复式-取投注内容
            case 6304: //排列三-组3复式-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText("");
                break;
            case 6305: //排列三-直选和值-取投注内容
            case 6306: //排列三-组选和值-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(",");
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
                //--------------快赢481 Start --------------------------//                                                                                                                                                                                                                                                                                            
            case 6801: //任选一                  
            case 6802: //任选二
            case 6806: //直选普通投注
            case 6804: //任选三
                $("#tz_ky481 ul").each(function (i, n_ul) {
                    var temp = "";
                    if ($(n_ul).find("b.rball").length < 1) {
                        temp += "-";
                    } else {
                        $(n_ul).find("b.rball").each(function (i, n_b) {
                            temp += $.trim($(n_b).text());
                        });
                        if (temp.length > 1) {
                            temp = "(" + temp + ")";
                        }
                    }
                    tmpLotteryNumber += temp;
                })
                break;
            case 6803: //任选二全包
                $("#div_renxuanerquanbao ul").each(function (i, n_ul) {
                    var temp = "";
                    if ($(n_ul).find("b.rball").length < 1) {
                        temp += "-";
                    } else {
                        $(n_ul).find("b.rball").each(function (i, n_b) {
                            temp += $.trim($(n_b).text());
                        });
                        if (temp.length > 1) {
                            temp = "(" + temp + ")";
                        }
                    }
                    tmpLotteryNumber += temp;
                })
                break;
            case 6805: //任选三全包
                $("#div_renxuansanquanbao ul").each(function (i, n_ul) {
                    var temp = "";
                    if ($(n_ul).find("b.rball").length < 1) {
                        temp += "-";
                    } else {
                        $(n_ul).find("b.rball").each(function (i, n_b) {
                            temp += $.trim($(n_b).text());
                        });
                        if (temp.length > 1) {
                            temp = "(" + temp + ")";
                        }
                    }
                    tmpLotteryNumber += temp;
                })
                break;
            case 6807: //组选24普通投注
            case 6812: //组6普通投注
                tmpLotteryNumber = $("#div_zuxuanputongtouzhu").find("b.rball").text();
                break;
            case 6809: //组12普通投注
                if ($("#div_zuxuanputongtouzhu").find("b.bball").length == 1) {
                    var vBlue = $("#div_zuxuanputongtouzhu").find("b.bball").text();
                    tmpLotteryNumber = vBlue + vBlue + $("#div_zuxuanputongtouzhu").find("b.rball").text();
                }
                else {
                    tmpLotteryNumber = $("#div_zuxuanputongtouzhu").find("b.rball").text();
                }
                break;
            case 6814: //组4普通投注
                if ($("#div_zuxuanputongtouzhu").find("b.bball").length == 1) {
                    var vBlue = $("#div_zuxuanputongtouzhu").find("b.bball").text();
                    tmpLotteryNumber = vBlue + vBlue + vBlue + $("#div_zuxuanputongtouzhu").find("b.rball").text();
                }
                else {
                    tmpLotteryNumber = $("#div_zuxuanputongtouzhu").find("b.rball").text();
                }
                break;
            case 6808: //组24胆拖投注
            case 6810: //组12胆拖投注
            case 6811: //组12重胆拖投注
            case 6813: //组6胆拖投注
            case 6815: //组4胆拖投注
            case 6816: //组4重胆拖投注
                var danNum = $("#div_dantuo_chongdantuo .tz_ball").eq(0).find("b.rball").text();
                var tuoNum = $("#div_dantuo_chongdantuo .tz_ball").eq(1).find("b.rball").text();
                tmpLotteryNumber += danNum + "," + tuoNum;
                break;
                //---------------快赢481 End-----------------------------//                                                                                                                                                                                                                                                                                    
            case 6901: //直选普通投注
            case 6902: //好运2
            case 6903: //好运3
            case 6904: //好运4
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                break;
                //---------------河南22选5 Start------------------------//                                                                                                                                                                                                                                                                                
            case 7001: //江西11选5-任选一普通投注-取投注内容
            case 7002: //江西11选5-任选二普通投注-取投注内容
            case 7003: //江西11选5-任选三普通投注-取投注内容
            case 7004: //江西11选5-任选四普通投注-取投注内容
            case 7005: //江西11选5-任选五普通投注-取投注内容
            case 7006: //江西11选5-任选六普通投注-取投注内容
            case 7007: //江西11选5-任选七普通投注-取投注内容
            case 7008: //江西11选5-任选八普通投注-取投注内容
            case 7011: //江西11选5-前二组选-取投注内容
            case 7012: //江西11选5-前三组选-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                break;

            case 7013: //江西11选5-前二胆拖投注-取投注内容
            case 7014: //江西11选5-前三胆拖投注-取投注内容
            case 7015: //江西11选5-任选二胆拖投注-取投注内容
            case 7016: //江西11选5-任选三胆拖投注-取投注内容
            case 7017: //江西11选5-任选四胆拖投注-取投注内容
            case 7018: //江西11选5-任选五胆拖投注-取投注内容
            case 7019: //江西11选5-任选六胆拖投注-取投注内容
            case 7020: //江西11选5-任选七胆拖投注-取投注内容
            case 7021: //江西11选5-任选八胆拖投注-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += " , ";
                tmpLotteryNumber += this.getSelectedTuoBallText(" ");
                break;
            case 7009: //江西11选5-前二直选-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += ",";
                tmpLotteryNumber += this.getSelectedTuoBallText(" ");
                break;
            case 7010: //江西11选5-前三直选-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += ",";
                tmpLotteryNumber += this.getSelectedTuoBallText(" ");
                tmpLotteryNumber += ",";
                $("#tz_qian3zhi").find("b.rball").each(function (i, n) {
                    tmpLotteryNumber += $.trim($(n).text()) + " ";
                });
                break;
            case 7401: //足球胜负-普通投注-取投注内容
                $("#tz_sfc ul").each(function (i, n_ul) {
                    var temp = "";
                    $(n_ul).find("b.rball").each(function (i, n_b) {
                        temp += $.trim($(n_b).text());
                    });
                    if (temp.length > 1) {
                        temp = "(" + temp + ")";
                    }
                    tmpLotteryNumber += temp;
                });
                break;
            case 7501: //任九场-普通投注-取投注内容
                $("#tz_rjc ul").each(function (i, n_ul) {
                    var temp = "";
                    if ($(n_ul).find("b.rball").length < 1) {
                        temp += "-";
                    } else {
                        $(n_ul).find("b.rball").each(function (i, n_b) {
                            temp += $.trim($(n_b).text());
                        });
                        if (temp.length > 1) {
                            temp = "(" + temp + ")";
                        }
                    }
                    tmpLotteryNumber += temp;
                });
                break;

            case 7801: //广东11选5-任选一普通投注-取投注内容
            case 7802: //广东11选5-任选二普通投注-取投注内容
            case 7803: //广东11选5-任选三普通投注-取投注内容
            case 7804: //广东11选5-任选四普通投注-取投注内容
            case 7805: //广东11选5-任选五普通投注-取投注内容
            case 7806: //广东11选5-任选六普通投注-取投注内容
            case 7807: //广东11选5-任选七普通投注-取投注内容
            case 7808: //广东11选5-任选八普通投注-取投注内容
            case 7811: //广东11选5-前二组选-取投注内容
            case 7812: //广东11选5-前三组选-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                break;

            case 7813: //广东11选5-前二胆拖投注-取投注内容
            case 7814: //广东11选5-前三胆拖投注-取投注内容
            case 7815: //广东11选5-任选二胆拖投注-取投注内容
            case 7816: //广东11选5-任选三胆拖投注-取投注内容
            case 7817: //广东11选5-任选四胆拖投注-取投注内容
            case 7818: //广东11选5-任选五胆拖投注-取投注内容
            case 7819: //广东11选5-任选六胆拖投注-取投注内容
            case 7820: //广东11选5-任选七胆拖投注-取投注内容
            case 7821: //广东11选5-任选八胆拖投注-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += " , ";
                tmpLotteryNumber += this.getSelectedTuoBallText(" ");
                break;
            case 7809: //广东11选5-前二直选-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += ",";
                tmpLotteryNumber += this.getSelectedTuoBallText(" ");
                break;
            case 7810: //广东11选5-前三直选-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += ",";
                tmpLotteryNumber += this.getSelectedTuoBallText(" ");
                tmpLotteryNumber += ",";
                $("#tz_qian3zhi").find("b.rball").each(function (i, n) {
                    tmpLotteryNumber += $.trim($(n).text()) + " ";
                });

                break;


            case 8201:
                tmpLotteryNumber = this.getSelectedRedBallText("");

                if (this.getSelectedRedBallCount() >= 2) {
                    tmpLotteryNumber = "(" + tmpLotteryNumber + ")";
                } else { tmpLotteryNumber = tmpLotteryNumber; }

                var signStr = "--";
                var SelectenNumbers = $("#div_XYWL_XC b.rball");
                if (SelectenNumbers.length > 0) {
                    var markAttr = $(SelectenNumbers[0]).parent().attr("mark") + "";
                    if (markAttr == "1")
                        tmpLotteryNumber = tmpLotteryNumber + "--"; //sfdsatr
                    else if (markAttr == "2")
                        tmpLotteryNumber = "-" + tmpLotteryNumber + "-";
                    else if (markAttr == "3")
                        tmpLotteryNumber = "--" + tmpLotteryNumber;
                }


                break;
            case 8202:
                tmpLotteryNumber = "";
                var tmpLotNum = "";
                var LotNumStr = "";
                var UrNodes = $("#div_XYWL_XC ul");
                var signStr = ""; //记号 用于选择的是哪两场

                for (var d = 0; d < UrNodes.length; d++) {
                    if ($(UrNodes[d]).find("b.rball").length > 0) {
                        signStr += $($(UrNodes[d]).find("b.rball")[0]).parent().attr("mark") + "";
                    }
                }


                for (var a = 0; a < UrNodes.length; a++) {
                    //单式
                    if ($(UrNodes[a]).find("b.rball").length <= 0)
                        continue
                    //复式
                    if ($(UrNodes[a]).find("b.rball").length >= 2) {
                        //获取投注号码 复式
                        tmpLotNum = "("
                        var lotNums = $(UrNodes[a]).find("b.rball");
                        for (var i = 0; i < lotNums.length; i++) {
                            tmpLotNum += $(lotNums[i]).text();
                        }
                        tmpLotNum = tmpLotNum + ")"
                    } else {
                        //获取投注号码 单式
                        tmpLotNum = $($(UrNodes[a]).find("b.rball")[0]).text();
                    }
                    if (signStr == "13") {
                        tmpLotNum += "-";
                        signStr = "一三";
                    }
                    if (tmpLotNum != "") {
                        LotNumStr += tmpLotNum;
                    }
                }


                if (signStr == "12")
                    tmpLotteryNumber = LotNumStr + "-";
                else if (signStr == "23")
                    tmpLotteryNumber = "-" + LotNumStr;
                else if (signStr == "一三")
                    tmpLotteryNumber = LotNumStr;
                break;

            case 8203:
                tmpLotteryNumber = "";
                var tmpLotNum = "";
                var LotNumStr = "";
                var UrNodes = $("#div_XYWL_XC ul");
                for (var a = 0; a < UrNodes.length; a++) {
                    if ($(UrNodes[a]).find("b.rball").length <= 0)
                        continue

                    if ($(UrNodes[a]).find("b.rball").length >= 2) {
                        tmpLotNum = "("
                        var lotNums = $(UrNodes[a]).find("b.rball");
                        for (var i = 0; i < lotNums.length; i++) {
                            tmpLotNum += $(lotNums[i]).text();
                        }
                        tmpLotNum = tmpLotNum + ")"
                    } else {
                        tmpLotNum = $($(UrNodes[a]).find("b.rball")[0]).text();
                    }
                    if (tmpLotNum != "") {
                        LotNumStr += tmpLotNum;
                    }
                }

                tmpLotteryNumber = LotNumStr;
                break;
            case 8204: tmpLotteryNumber = "R1," + this.getSelectedRedBallText(" ");
                break;
            case 8205: tmpLotteryNumber = "R2," + this.getSelectedRedBallText(" ");
                break;
            case 8206: tmpLotteryNumber = "R3," + this.getSelectedRedBallText(" ");
                break;
            case 8207: tmpLotteryNumber = "R4," + this.getSelectedRedBallText(" ");
                break;
            case 8208: tmpLotteryNumber = "R5," + this.getSelectedRedBallText(" ");
                break;
            case 8209: tmpLotteryNumber = "R6," + this.getSelectedRedBallText(" ");
                break;
            case 8210: tmpLotteryNumber = "R7," + this.getSelectedRedBallText(" ");
                break;
            case 8211:
                tmpLotteryNumber = "S1," + this.getSelectedRedBallText(" ");
                break;
            case 8212:

                // 取号码
                var s1 = "", s2 = "";
                //第一镖 选中的号码
                var obj = $($("#div_XYWL_SX li[mark='1']")[0]).find("b.rball");
                //第二镖 选中的号码
                var obj2 = $($("#div_XYWL_SX li[mark='2']")[0]).find("b.rball");

                for (var i = 0; i < obj.length; i++) {
                    s1 += $(obj[i]).text() + " ";
                }
                for (var j = 0; j < obj2.length; j++) {
                    s2 += $(obj2[j]).text() + " ";
                }
                if (s1 == "" || s2 == "") { return; }
                tmpLotteryNumber = "S2," + trim(s1) + " , " + trim(s2);
                //                if (s1 == "" || s2 == "") {
                //                    InvestNum = 0;
                //                } else {
                //                    var ary_s1 = trim(s1).split(' ');
                //                    var ary_s2 = trim(s2).split(' ');
                //                    var repeat = "";
                //                    var number = "";
                //                    // 取注数
                //                    for (var x = 0; x < ary_s1.length; x++) {
                //                        for (var j = 0; j < ary_s2.length; j++) {
                //                            if (trim(ary_s1[x]) == trim(ary_s2[j])) {
                //                                continue;
                //                            }
                //                            number = "S2," + (ary_s1[x] + " " + ary_s2[j] + "$");
                //                            if (repeat.indexOf(number) == -1) {
                //                                repeat += number;
                //                            }
                //                        }
                //                    }
                //                    if (repeat == "")
                //                        tmpLotteryNumber = "";
                //                    else
                //                        tmpLotteryNumber = trim(repeat);
                //                }


                break;

            case 8213:
                // 取号码
                var s1 = "", s2 = "", s3 = "";
                //第一镖 选中的号码
                var obj = $($("#div_XYWL_SX li[mark='1']")[0]).find("b.rball");
                //第二镖 选中的号码
                var obj2 = $($("#div_XYWL_SX li[mark='2']")[0]).find("b.rball");
                //第三镖 选中的号码
                var obj3 = $($("#div_XYWL_SX li[mark='3']")[0]).find("b.rball");

                for (var i = 0; i < obj.length; i++) {
                    s1 += $(obj[i]).text() + " ";
                }
                for (var j = 0; j < obj2.length; j++) {
                    s2 += $(obj2[j]).text() + " ";
                }
                for (var c = 0; c < obj3.length; c++) {
                    s3 += $(obj3[c]).text() + " ";
                }
                if (s1 == "" || s2 == "" || s3 == "") { return; }
                tmpLotteryNumber = "S3," + trim(s1) + " , " + trim(s2) + " , " + trim(s3);
                //                if (s1 == "" || s2 == "" || s3 == "") {
                //                    InvestNum = 0;

                //                } else {
                //                    var ary_s1 = trim(s1).split(' ');
                //                    var ary_s2 = trim(s2).split(' ');
                //                    var ary_s3 = trim(s3).split(' ');
                //                    var repeat = "", number = "";
                //                    // 取注数
                //                    for (var x = 0; x < ary_s1.length; x++) {
                //                        for (var j = 0; j < ary_s2.length; j++) {
                //                            for (var k = 0; k < ary_s3.length; k++) {
                //                                if ((trim(ary_s1[x]) == trim(ary_s2[j])) || (trim(ary_s1[x]) == trim(ary_s3[k])) || (trim(ary_s2[j]) == trim(ary_s3[k]))) {
                //                                    continue;
                //                                }
                //                                number = "S3," + (ary_s1[x] + " " + ary_s2[j] + " " + ary_s3[k] + "$"); ;
                //                                if (repeat.indexOf(number) == -1) {
                //                                    repeat += number;
                //                                }
                //                            }
                //                        }
                //                    }
                //                    if (repeat == "")
                //                        tmpLotteryNumber = "";
                //                    else
                //                        tmpLotteryNumber = trim(repeat);
                //                }


                break;

            case 8214: tmpLotteryNumber = getWLYXlotteryNumber();
                break;

            case 8301: //和值-普通投注-取投注内容
            case 8303: //和值-普通投注-取投注内容
            case 8304: //和值-普通投注-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(",");
                break;
            case 8302: //和值-普通投注-取投注内容
            case 8306: //和值-普通投注-取投注内容
            case 8308: //和值-普通投注-取投注内容
            case 8307: //和值-普通投注-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText("");
                break;
            case 8305: //和值-普通投注-取投注内容
                var t = this.getSelectedTuoBallText(",").split(',');
                var s = "(" + this.getSelectedRedBallText("") + ")";
                for (var i = 0; i < t.length; i++) {
                    tmpLotteryNumber += "(" + t[i] + ")" + s + (i < t.length - 1 ? "," : "");
                }

                break;

            case 8701: //湖南幸运赛车-前一普通投注-取投注内容
            case 8708: //湖南幸运赛车-位置投注-取投注内容
            case 8703: //湖南幸运赛车-前二复式投注-取投注内容
            case 8706: //湖南幸运赛车-前三复式投注-取投注内容
            case 8714: //湖南幸运赛车-组二复式投注-取投注内容
            case 8716: //湖南幸运赛车-组三复式投注-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                break;
            case 8702: //湖南幸运赛车-前二普通投注-取投注内容
            case 8704: //湖南幸运赛车-前二胆拖投注-取投注内容
            case 8707: //湖南幸运赛车-前三胆拖投注-取投注内容
            case 8709: //湖南幸运赛车-过两关普通投注-取投注内容
            case 8710: //湖南幸运赛车-过两关胆拖投注-取投注内容
            case 8712: //湖南幸运赛车-过三关胆拖投注-取投注内容
            case 8715: //湖南幸运赛车-组二胆拖投注-取投注内容
            case 8717: //湖南幸运赛车-组三胆拖投注-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += " , ";
                tmpLotteryNumber += this.getSelectedTuoBallText(" ");
                break;
            case 8705: //湖南幸运赛车-前三普通投注-取投注内容
            case 8711: //湖南幸运赛车-过三关普通投注-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += " , ";
                $("#tz_qian3zhi").find("b.rball").each(function (i, n) {
                    tmpLotteryNumber += $(n).text() + " ";
                });
                tmpLotteryNumber += ", ";
                tmpLotteryNumber += this.getSelectedTuoBallText(" ");

                tmpLotteryNumber = $.trim(tmpLotteryNumber);
                break;
            case 8713: //湖南幸运赛车-大小奇偶投注-取投注内容
                $("#ul_dxjo").find("b.rball").each(function (i, n) {
                    tmpLotteryNumber += $(n).text() + " ";
                });
                tmpLotteryNumber = $.trim(tmpLotteryNumber);
                break;

            case 8801: //上海11选5-任选一普通投注-取投注内容
            case 8802: //上海11选5-任选二普通投注-取投注内容
            case 8803: //上海11选5-任选三普通投注-取投注内容
            case 8804: //上海11选5-任选四普通投注-取投注内容
            case 8805: //上海11选5-任选五普通投注-取投注内容
            case 8806: //上海11选5-任选六普通投注-取投注内容
            case 8807: //上海11选5-任选七普通投注-取投注内容
            case 8808: //上海11选5-任选八普通投注-取投注内容
            case 8811: //上海11选5-前二组选-取投注内容
            case 8812: //上海11选5-前三组选-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                break;

            case 8813: //上海11选5-前二胆拖投注-取投注内容
            case 8814: //上海11选5-前三胆拖投注-取投注内容
            case 8815: //上海11选5-任选二胆拖投注-取投注内容
            case 8816: //上海11选5-任选三胆拖投注-取投注内容
            case 8817: //上海11选5-任选四胆拖投注-取投注内容
            case 8818: //上海11选5-任选五胆拖投注-取投注内容
            case 8819: //上海11选5-任选六胆拖投注-取投注内容
            case 8820: //上海11选5-任选七胆拖投注-取投注内容
            case 8821: //上海11选5-任选八胆拖投注-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += " , ";
                tmpLotteryNumber += this.getSelectedTuoBallText(" ");
                break;
            case 8809: //上海11选5-前二直选-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += ",";
                tmpLotteryNumber += this.getSelectedTuoBallText(" ");
                break;
            case 8810: //上海11选5-前三直选-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(" ");
                tmpLotteryNumber += ",";
                tmpLotteryNumber += this.getSelectedTuoBallText(" ");
                tmpLotteryNumber += ",";
                $("#tz_qian3zhi").find("b.rball").each(function (i, n) {
                    tmpLotteryNumber += $.trim($(n).text()) + " ";
                });

                break;

            case 8901: //广西快3和值-普通投注-取投注内容
            case 8903: //广西快3和值-普通投注-取投注内容
            case 8904: //广西快3和值-普通投注-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText(",");
                break;
            case 8902: //广西快3和值-普通投注-取投注内容
            case 8906: //广西快3和值-普通投注-取投注内容
            case 8908: //广西快3和值-普通投注-取投注内容
            case 8907: //广西快3和值-普通投注-取投注内容
                tmpLotteryNumber = this.getSelectedRedBallText("");
                break;
            case 8905: //和值-普通投注-取投注内容
                var t = this.getSelectedTuoBallText(",").split(',');
                var s = "(" + this.getSelectedRedBallText("") + ")";
                for (var i = 0; i < t.length; i++) {
                    tmpLotteryNumber += "(" + t[i] + ")" + s + (i < t.length - 1 ? "," : "");
                }

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

    //取选中的篮球球胆码
    getSelectedTuoBlueDMBallText: function (splitChar) {
        if (splitChar == undefined)
            return "Please enter the char for split the number";
        var temp = "";
        $("#" + this.SelectTAreaID + " b.bball").each(function (i, n) {
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

    //取选中的后区拖码蓝球号码
    getSelectedBlueBTAreaBallText: function (splitChar) {
        if (splitChar == undefined)
            return "Please enter the char for split the number";
        var temp = "";
        $("#" + this.SelectBTAreaID + " b.bball").each(function (i, n) {
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



            case 201: //四场进球-普通投注-取注数
                InvestNum = 1;
                $("#tz_jqc ul").each(function (i, n_ul) {
                    InvestNum *= $(n_ul).find("b.rball").length;
                });
                break;
            case 301:
                InvestNum = 1;
                $("#" + this.SelectRAreaID + " ul").each(function (i, n) {
                    InvestNum *= $(n).find("b.rball").length;
                });
                break;
            case 501: //双色球-普通投注-取注数
                if ((this.getSelectedRedBallCount() < 6) || (this.getSelectedBlueBallCount() < 1)) {
                    InvestNum = 0;
                }
                else {
                    InvestNum = this.Combination(this.getSelectedRedBallCount(), 6) * this.getSelectedBlueBallCount();
                }

                break;
            case 502: //双色球-胆拖-取注数

                if ((this.getSelectedRedBallCount() + this.getSelectedTuoBallCount()) < 7 || this.getSelectedBlueBallCount() < 1 || this.getSelectedRedBallCount() < 1) {
                    InvestNum = 0;
                } else {
                    InvestNum = this.Combination(this.getSelectedTuoBallCount(), 6 - this.getSelectedRedBallCount()) * this.getSelectedBlueBallCount();
                }

                break;

            case 601: //3D-普通投注-取注数
                InvestNum = 1;
                $("#tz_3d ul").each(function (i, n) {
                    InvestNum *= $(n).find("b.rball").length;
                });

                break;
            case 602: //3D-组选单式-取注数
                if (this.getSelectedRedBallCount() == 3 || (this.getSelectedRedBallCount() == 1 && this.getSelectedBlueBallCount() == 1)) {
                    InvestNum = 1;
                } else {
                    InvestNum = 0;
                }
                break;
            case 603: //3D-组6复式-取注数
                InvestNum = this.getSelectedRedBallCount() < 4 ? 0 : this.Combination(this.getSelectedRedBallCount(), 3);
                break;
            case 604: //3D-组3复式-取注数
                InvestNum = 1;
                if (this.getSelectedRedBallCount() < 2) {
                    InvestNum = 0;
                } else if (this.getSelectedRedBallCount() == 2) {
                    InvestNum = 2;
                } else {
                    InvestNum = this.Combination(this.getSelectedRedBallCount(), 2) * 2;
                }
                break;
            case 605: //3D-直选和值-取注数
                InvestNum = 0;
                var invest_context = this.getSelectedRedBallText(",");
                var investStrs = invest_context.split(",");
                for (var i = 0; i < investStrs.length; i++) {
                    InvestNum += GetInvestmentCount.getInvestment(605, parseInt(investStrs[i], 10));
                }
                break;
            case 606: //3D-组选和值-取注数
                InvestNum = 0;
                var invest_context = this.getSelectedRedBallText(",");
                var investStrs = invest_context.split(",");
                for (var i = 0; i < investStrs.length; i++) {
                    InvestNum += GetInvestmentCount.getInvestment(606, parseInt(investStrs[i], 10));
                }

                break;
            case 901: //22选5-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 5 ? 0 : this.Combination(this.getSelectedRedBallCount(), 5);
                break;
            case 1301: //七乐彩-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 7 ? 0 : this.Combination(this.getSelectedRedBallCount(), 7);
                break;
            case 1501: //六场半全场-普通投注-取注数
                InvestNum = 1;
                $("#tz_lcbqc ul").each(function (i, n_ul) {
                    InvestNum *= $(n_ul).find("b.rball").length;
                });
                break;
            case 2802: //重庆时时彩-五星复选-取注数
                InvestNum = 4;
                $("#tz_cqssc ul").each(function (i, n_ul) {
                    InvestNum *= $(n_ul).find("b.rball").length;
                });
                break;
            case 2803: //重庆时时彩-五、三、二、一星直选-取注数              
            case 2805: //重庆时时彩-五星通选-取注数
                InvestNum = $("#tz_cqssc ul:visible").length > 0 ? 1 : 0;
                $("#tz_cqssc ul:visible").each(function (i, n_ul) {
                    InvestNum *= $(n_ul).find("b.rball").length;
                });
                break;
            case 2804: //重庆时时彩-大小单双-取注数
                InvestNum = 1;
                $("#div_daxiao_danshuang ul").each(function (i, n_ul) {
                    InvestNum *= $(n_ul).find("b.rball").length;
                });
                break;
            case 2806: //重庆时时彩-二星组选-取注数
                InvestNum = this.getSelectedRedBallCount() < 2 ? 0 : this.Combination(this.getSelectedRedBallCount(), 2);
                break;
            case 2808: //重庆时时彩-二星包点-取注数
            case 2810: //重庆时时彩-三星直选包点-取注数
            case 2815: //重庆时时彩-三星组选包点-取注数
                InvestNum = 0;
                var invest_context = this.getSelectedRedBallText(",");
                var investStrs = invest_context.split(",");
                for (var i = 0; i < investStrs.length; i++) {
                    InvestNum += GetInvestmentCount.getInvestment(Lottery.PlayTypeID, parseInt(investStrs[i], 10));
                }
                break;
            case 2811: //重庆时时彩-三星组三-取注数
                if (this.getSelectedRedBallCount() == 1 && $("#div_3xing_zu3_zu6_zuhe").find("b.bball").length == 1) {
                    InvestNum = 1;
                } else {
                    InvestNum = this.getSelectedRedBallCount() < 3 ? 0 : this.getSelectedRedBallCount() * (this.getSelectedRedBallCount() - 1);
                }
                break;
            case 2812: //重庆时时彩-三星组六-取注数
                InvestNum = this.getSelectedRedBallCount() < 3 ? 0 : this.Combination(this.getSelectedRedBallCount(), 3);
                break;
            case 2813: //重庆时时彩-三星直选组合-取注数
                InvestNum = this.getSelectedRedBallCount() < 3 ? 0 : this.Permutation(this.getSelectedRedBallCount(), 3);
                break;
            case 2814: //重庆时时彩-三星组选包胆-取注数
                InvestNum = 55 * this.getSelectedRedBallCount();
                break;

            case 2902: //时时乐-普通投注-取注数
                InvestNum = 1;
                $("#tz_ssl ul").each(function (i, n) {
                    InvestNum *= $(n).find("b.rball").length;
                });
                break;
            case 2904: //时时乐-组六-取注数
                InvestNum = this.getSelectedRedBallCount() < 3 ? 0 : this.Combination(this.getSelectedRedBallCount(), 3);
                break;
            case 2905: //时时乐-组三-取注数
                if (this.getSelectedRedBallCount() == 1 && $("#div_zu3").find("b.bball").length == 1) {
                    InvestNum = 1;
                } else {
                    InvestNum = this.getSelectedRedBallCount() < 3 ? 0 : this.getSelectedRedBallCount() * (this.getSelectedRedBallCount() - 1);
                }
                break;
            case 2906: //时时乐-直选和值-取注数
            case 2907: //时时乐-组选和值-取注数
                InvestNum = 0;
                var invest_context = this.getSelectedRedBallText(",");
                var investStrs = invest_context.split(",");
                for (var i = 0; i < investStrs.length; i++) {
                    InvestNum += GetInvestmentCount.getInvestment(Lottery.PlayTypeID, parseInt(investStrs[i], 10));
                }
                break;
            case 2908: //时时乐-前2-取注数
            case 2909: //时时乐-后2-取注数
                InvestNum = 1;
                $("#div_qianhou2 ul").each(function (i, n) {
                    InvestNum *= $(n).find("b.rball").length;
                });
                break;
            case 2910: //时时乐-前1-取注数
            case 2911: //时时乐-后1-取注数
                InvestNum = this.getSelectedRedBallCount();
                break;
            case 3901: //大乐透-普通投注-取注数
            case 3902: //大乐透-追加普通-取注数
                //追加投注每注3元
                this.PlayTypeID == 3902 ? this.Price = 3 : this.Price = 2;

                if ((this.getSelectedRedBallCount() < 5) || (this.getSelectedBlueBallCount() < 2)) {
                    InvestNum = 0;
                }
                else {
                    InvestNum = this.Combination(this.getSelectedRedBallCount(), 5) * this.Combination(this.getSelectedBlueBallCount(), 2);
                }

                break;

            case 3903: //大乐透-胆拖投注-取注数
            case 3904: //大乐透-追加胆拖-取注数
                //追加投注每注3元
                this.PlayTypeID == 3904 ? this.Price = 3 : this.Price = 2;

                if (this.getSelectedRedBallCount() < 1 || this.getSelectedTuoBallCount() < 2 || this.getSelectedRedBallCount() + this.getSelectedTuoBallCount() < 6 || this.getSelectedBlueBallCount() < 2) {
                    InvestNum = 0;
                }
                else {
                    InvestNum = this.Combination(this.getSelectedTuoBallCount(), 5 - this.getSelectedRedBallCount()) * this.Combination(this.getSelectedBlueBallCount(), 2);
                }

                break;
            case 3906: //大乐透-后去胆拖
            case 3908:
                this.PlayTypeID == 3906 ? this.Price = 2 : this.Price = 3;
                var redQDCount = this.getSelectedRedBallCount();
                var redHDCount = $("#tz_rtball .bball").length;
                var blurHTCount = this.getSelectedBlueBallCount();
                if (redQDCount < 5 || blurHTCount < 2 || redHDCount < 1) {
                    InvestNum = 0;
                }
                else {
                    InvestNum = Math.ceil(this.Combination(blurHTCount, 1));
                }
                break;
            case 3907: //大乐透-双区胆拖投注-取注数
            case 3909:
                this.PlayTypeID == 3907 ? this.Price = 2 : this.Price = 3;
                var redCount = $(".tz_ball B[class='rball']").length;
                var redQDCount = $("#tz_rball B[class='rball']").length;
                var redTMCount = $("#tz_rtball B[class='rball']").length;
                var blueCount = $(".tz_ball B[class='bball']").length;
                var blueQDCount = $("#tz_bball B[class='bball']").length;
                var blueTMCount = $("#tz_qball B[class='bball']").length;
                if (redCount < 6 || redQDCount < 1 || blueCount < 2 || blueTMCount < 2) {
                    InvestNum = 0;
                }
                else {
                    InvestNum = Math.ceil(this.Combination(redTMCount, 5 - redQDCount) * this.Combination(blueTMCount, 2 - blueQDCount));
                }

                break;

            case 6201: //十一运夺金任选一-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount();
                break;
            case 6202: //十一运夺金任选二-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 2 ? 0 : this.Combination(this.getSelectedRedBallCount(), 2);
                break;
            case 6215: //十一运夺金任选二-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() == 1 ? this.Combination(this.getSelectedTuoBallCount(), 1) : 0;
                break;
            case 6203: //十一运夺金任选三-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 3 ? 0 : this.Combination(this.getSelectedRedBallCount(), 3);
                break;
            case 6216: //十一运夺金任选三-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 3 ? this.Combination(this.getSelectedTuoBallCount(), 3 - this.getSelectedRedBallCount()) : 0;
                break;
            case 6204: //十一运夺金任选四-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 4 ? 0 : this.Combination(this.getSelectedRedBallCount(), 4);
                break;
            case 6217: //十一运夺金任选四-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 4 ? this.Combination(this.getSelectedTuoBallCount(), 4 - this.getSelectedRedBallCount()) : 0;
                break;
            case 6205: //十一运夺金任选五-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 5 ? 0 : this.Combination(this.getSelectedRedBallCount(), 5);
                break;
            case 6218: //十一运夺金任选五-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 5 ? this.Combination(this.getSelectedTuoBallCount(), 5 - this.getSelectedRedBallCount()) : 0;
                break;
            case 6206: //十一运夺金任选六-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 6 ? 0 : this.Combination(this.getSelectedRedBallCount(), 6);
                break;
            case 6219: //十一运夺金任选六-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 6 ? this.Combination(this.getSelectedTuoBallCount(), 6 - this.getSelectedRedBallCount()) : 0;
                break;
            case 6207: //十一运夺金任选七-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 7 ? 0 : this.Combination(this.getSelectedRedBallCount(), 7);
                break;
            case 6220: //十一运夺金任选七-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 7 ? this.Combination(this.getSelectedTuoBallCount(), 7 - this.getSelectedRedBallCount()) : 0;
                break;
            case 6208: //十一运夺金任选八-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 8 ? 0 : this.Combination(this.getSelectedRedBallCount(), 8);
                break;
            case 6221: //十一运夺金任选八-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 8 ? this.Combination(this.getSelectedTuoBallCount(), 8 - this.getSelectedRedBallCount()) : 0;
                break;
            case 6209: //十一运夺金前二-普通投注-取注数
                var f_number = this.getSelectedRedBallText(",");
                if (f_number == "" || f_number == null) {
                    InvestNum = 0;
                    break;
                }
                var s_number = this.getSelectedTuoBallText(",");
                if (s_number == "" || s_number == null) {
                    InvestNum = 0;
                    break;
                }
                var first_numbers = f_number.split(",");
                var second_numbers = s_number.split(",");
                var count = 0;
                for (var i = 0; i < first_numbers.length; i++) {
                    for (var j = 0; j < second_numbers.length; j++) {
                        if (first_numbers[i] != second_numbers[j]) {
                            count++;
                        }
                    }
                }
                InvestNum = count;
                break;
            case 6211: //十一运夺金前二-组选投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 2 ? 0 : this.Combination(this.getSelectedRedBallCount(), 2);
                break;
            case 6213: //十一运夺金前二-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() == 1 ? this.Combination(this.getSelectedTuoBallCount(), 1) : 0;
                break;
            case 6210: //十一运夺金前三-普通投注-取注数
                var num1 = this.getSelectedRedBallText(",").split(",");
                var num2 = this.getSelectedTuoBallText(",").split(",");
                var temp = "";
                $("#tz_qian3zhi b.rball").each(function (i, n) {
                    temp += ($.trim($(n).text()) + ",");
                });
                if (temp.substring(temp.length - 1, temp.length) == ",")
                    temp = temp.substring(0, temp.length - 1);
                var num3 = temp.split(",");

                if (num1[0] == "" || num2[0] == "" || num3[0] == "") {
                    InvestNum = 0;
                    break;
                }
                var count = 0;
                for (var i = 0; i < num1.length; i++) {
                    for (var x = 0; x < num2.length; x++) {
                        for (var j = 0; j < num3.length; j++) {
                            if (num1[i] != num2[x] && num1[i] != num3[j] && num2[x] != num3[j]) {
                                count++;
                            }
                        }
                    }
                }
                InvestNum = count;
                break;
            case 6212: //十一运夺金前三-组选投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 3 ? 0 : this.Combination(this.getSelectedRedBallCount(), 3);
                break;
            case 6214: //十一运夺金前三-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 3 ? this.Combination(this.getSelectedTuoBallCount(), 3 - this.getSelectedRedBallCount()) : 0;
                break;
            case 6301: //排列三-普通投注-取注数
                InvestNum = 1;
                $("#tz_pl3 ul").each(function (i, n) {
                    InvestNum *= $(n).find("b.rball").length;
                });

                break;
            case 6302: //排列三-组选单式-取注数
                if (this.getSelectedRedBallCount() == 3 || (this.getSelectedRedBallCount() == 1 && this.getSelectedBlueBallCount() == 1)) {
                    InvestNum = 1;
                } else {
                    InvestNum = 0;
                }
                break;
            case 6303: //排列三-组6复式-取注数
                InvestNum = this.getSelectedRedBallCount() < 4 ? 0 : this.Combination(this.getSelectedRedBallCount(), 3);
                break;
            case 6304: //排列三-组3复式-取注数
                InvestNum = 1;
                if (this.getSelectedRedBallCount() < 2) {
                    InvestNum = 0;
                } else if (this.getSelectedRedBallCount() == 2) {
                    InvestNum = 2;
                } else {
                    InvestNum = this.Combination(this.getSelectedRedBallCount(), 2) * 2;
                }
                break;
            case 6305: //排列三-直选和值-取注数
                InvestNum = 0;
                var invest_context = this.getSelectedRedBallText(",");
                var investStrs = invest_context.split(",");
                for (var i = 0; i < investStrs.length; i++) {
                    InvestNum += GetInvestmentCount.getInvestment(6305, parseInt(investStrs[i], 10));
                }
                break;
            case 6306: //排列三-组选和值-取注数
                InvestNum = 0;
                var invest_context = this.getSelectedRedBallText(",");
                var investStrs = invest_context.split(",");
                for (var i = 0; i < investStrs.length; i++) {
                    InvestNum += GetInvestmentCount.getInvestment(6306, parseInt(investStrs[i], 10));
                }

                break;
            case 6401: //排列五-普通投注-取注数
                InvestNum = 1;
                $("#" + this.SelectRAreaID + " ul").each(function (i, n) {
                    InvestNum *= $(n).find("b.rball").length;
                });

                break;

                //--------------快赢481 Start-----------------//                                                                                                                                                                                                                                                                                                                                                
            case 6801: //快赢481任选一
                InvestNum = $("#tz_ky481 b.rball").length;
                break;
            case 6802: //任选二
                InvestNum = 0;
                var rowNum1 = $("#tz_ky481 .tz_ball").eq(0).find("b.rball").text(); //自由泳选中号码个数
                var rowNum2 = $("#tz_ky481 .tz_ball").eq(1).find("b.rball").text(); //仰泳选中号码个数
                var rowNum3 = $("#tz_ky481 .tz_ball").eq(2).find("b.rball").text(); //蛙泳选中号码个数
                var rowNum4 = $("#tz_ky481 .tz_ball").eq(3).find("b.rball").text(); //蝶泳选中号码个数
                if (rowNum1.length >= 1 && rowNum2 >= 1) {
                    InvestNum += this.Combination(rowNum1.length, 1) * this.Combination(rowNum2.length, 1);
                }
                if (rowNum1.length >= 1 && rowNum3.length >= 1) {
                    InvestNum += this.Combination(rowNum1.length, 1) * this.Combination(rowNum3.length, 1);
                }
                if (rowNum1.length >= 1 && rowNum4.length >= 1) {
                    InvestNum += this.Combination(rowNum1.length, 1) * this.Combination(rowNum4.length, 1);
                }
                if (rowNum2.length >= 1 && rowNum3.length >= 1) {
                    InvestNum += this.Combination(rowNum2.length, 1) * this.Combination(rowNum3.length, 1);
                }
                if (rowNum2.length >= 1 && rowNum4.length >= 1) {
                    InvestNum += this.Combination(rowNum2.length, 1) * this.Combination(rowNum4.length, 1);
                }
                if (rowNum3.length >= 1 && rowNum4.length >= 1) {
                    InvestNum += this.Combination(rowNum3.length, 1) * this.Combination(rowNum4.length, 1);
                }
                break;
            case 6803: //任选二全包
                InvestNum = 0;
                var rowNum1 = $("#div_renxuanerquanbao .tz_ball").eq(0).find("b.rball").text(); //任选二全包第一位
                var rowNum2 = $("#div_renxuanerquanbao .tz_ball").eq(1).find("b.rball").text(); //任选二全包第二位
                if ((rowNum2.length + rowNum1.length >= 2) && rowNum1.length >= 1 && rowNum2.length >= 1) {
                    for (var j = 0; j < rowNum1.length; j++) {
                        for (var i = 0; i < rowNum2.length; i++) {
                            if (rowNum1[j] == rowNum2[i]) {
                                InvestNum += 6;
                            }
                            else {
                                InvestNum += 12;
                            }
                        }
                    }
                }
                break;
            case 6804: //任选三
                InvestNum = 0;
                var rowNum1 = $("#tz_ky481 .tz_ball").eq(0).find("b.rball").text(); //自由泳选中号码个数
                var rowNum2 = $("#tz_ky481 .tz_ball").eq(1).find("b.rball").text(); //仰泳选中号码个数
                var rowNum3 = $("#tz_ky481 .tz_ball").eq(2).find("b.rball").text(); //蛙泳选中号码个数
                var rowNum4 = $("#tz_ky481 .tz_ball").eq(3).find("b.rball").text(); //蝶泳选中号码个数
                if (rowNum1.length >= 1 && rowNum2.length >= 1 && rowNum3.length >= 1) {
                    InvestNum += this.Combination(rowNum1.length, 1) * this.Combination(rowNum2.length, 1) * this.Combination(rowNum3.length, 1);
                }
                if (rowNum1.length >= 1 && rowNum2.length >= 1 && rowNum4.length >= 1) {
                    InvestNum += this.Combination(rowNum1.length, 1) * this.Combination(rowNum2.length, 1) * this.Combination(rowNum4.length, 1);
                }
                if (rowNum1.length >= 1 && rowNum3.length >= 1 && rowNum4.length >= 1) {
                    InvestNum += this.Combination(rowNum1.length, 1) * this.Combination(rowNum3.length, 1) * this.Combination(rowNum4.length, 1);
                }
                if (rowNum2.length >= 1 && rowNum3.length >= 1 && rowNum4.length >= 1) {
                    InvestNum += this.Combination(rowNum2.length, 1) * this.Combination(rowNum3.length, 1) * this.Combination(rowNum4.length, 1);
                }
                break;
            case 6805: //任选三全包
                InvestNum = 0;
                var rowNum1 = $("#div_renxuansanquanbao .tz_ball").eq(0).find("b.rball").text(); //任选三全包第一位
                var rowNum2 = $("#div_renxuansanquanbao .tz_ball").eq(1).find("b.rball").text(); //任选三全包第二位
                var rowNum3 = $("#div_renxuansanquanbao .tz_ball").eq(2).find("b.rball").text(); //任选三全包第三位
                if ((rowNum3.length + rowNum2.length + rowNum1.length >= 3) && rowNum1.length >= 1 && rowNum2.length >= 1 && rowNum3.length >= 1) {
                    for (var j = 0; j < rowNum1.length; j++) {
                        for (var i = 0; i < rowNum2.length; i++) {
                            for (var k = 0; k < rowNum3.length; k++) {
                                if (rowNum1[j] == rowNum2[i] && rowNum1[j] == rowNum3[k]) {
                                    InvestNum += 4;
                                }
                                if ((rowNum1[j] == rowNum2[i] && rowNum1[j] != rowNum3[k]) || (rowNum1[j] == rowNum3[k] && rowNum1[j] != rowNum2[i]) || (rowNum3[k] == rowNum2[i] && rowNum1[j] != rowNum3[k])) {
                                    InvestNum += 12;
                                }
                                if (rowNum1[j] != rowNum2[i] && rowNum1[j] != rowNum3[k] && rowNum2[i] != rowNum3[k]) {
                                    InvestNum += 24;
                                }
                            }
                        }
                    }
                }
                break;
            case 6806: //直选普通投注
                InvestNum = 0;
                var rowNum1 = $("#tz_ky481 .tz_ball").eq(0).find("b.rball").text(); //自由泳选中号码个数
                var rowNum2 = $("#tz_ky481 .tz_ball").eq(1).find("b.rball").text(); //仰泳选中号码个数
                var rowNum3 = $("#tz_ky481 .tz_ball").eq(2).find("b.rball").text(); //蛙泳选中号码个数
                var rowNum4 = $("#tz_ky481 .tz_ball").eq(3).find("b.rball").text(); //蝶泳选中号码个数
                if (rowNum1.length >= 1 && rowNum2.length >= 1 && rowNum3.length >= 1 && rowNum4.length >= 1) {
                    InvestNum = this.Combination(rowNum1.length, 1) * this.Combination(rowNum2.length, 1) * this.Combination(rowNum3.length, 1) * this.Combination(rowNum4.length, 1);
                }
                break;
            case 6807: //组选24普通投注
                InvestNum = 0;
                var rowNum = $("#div_zuxuanputongtouzhu b.rball").text();
                if (rowNum.length >= 4) {
                    InvestNum = this.Combination(rowNum.length, 4);
                }
                break;
            case 6808: //组选24胆拖投注
                InvestNum = 1;
                var danNum = $("#div_dantuo_chongdantuo .tz_ball").eq(0).find("b.rball").text();
                var tuoNum = $("#div_dantuo_chongdantuo .tz_ball").eq(1).find("b.rball").text();
                var tempNum = 1;
                if (((danNum.length + tuoNum.length) < 5) || danNum.length < 1) {
                    InvestNum = 0;
                }


                for (var i = 0; i < (4 - danNum.length) ; i++) {
                    InvestNum *= (tuoNum.length - i);
                }

                for (var i = 1; i < (5 - danNum.length) ; i++) {
                    tempNum *= i;
                }


                InvestNum = (InvestNum / tempNum);
                break;
            case 6809: //组12普通投注
                InvestNum = 0;
                if ($("#div_zuxuanputongtouzhu").find("b.bball").length == 1 && $("#div_zuxuanputongtouzhu").find("b.rball").length == 2) {
                    InvestNum = 1;
                }
                else {
                    var rowNum = $("#div_zuxuanputongtouzhu b.rball").text();
                    InvestNum = this.Combination(rowNum.length, 3) * 3;
                }
                break;
            case 6810: //组12胆拖投注
                InvestNum = 1;
                var danNum = $("#div_dantuo_chongdantuo .tz_ball").eq(0).find("b.rball").text();
                var tuoNum = $("#div_dantuo_chongdantuo .tz_ball").eq(1).find("b.rball").text();
                var tempNum = 1;
                if (((danNum.length + tuoNum.length) < 3) || danNum.length < 1) {
                    InvestNum = 0;
                }

                for (var i = 0; i < (3 - danNum.length) ; i++) {
                    InvestNum *= (tuoNum.length - i);
                }

                for (var i = 1; i < (4 - danNum.length) ; i++) {
                    tempNum *= i;
                }

                InvestNum = 3 * (InvestNum / tempNum);
                break;
            case 6811: //组12重胆拖投注
                InvestNum = 1;
                var danNum = $("#div_dantuo_chongdantuo .tz_ball").eq(0).find("b.rball").text();
                var tuoNum = $("#div_dantuo_chongdantuo .tz_ball").eq(1).find("b.rball").text();
                var tempNum = 1;
                if (((danNum.length + tuoNum.length) < 4) || danNum.length < 1) {
                    InvestNum = 0;
                }

                if (danNum.length == 1) {
                    for (var i = 0; i < (3 - danNum.length) ; i++) {
                        InvestNum *= (tuoNum.length - i);
                    }

                    for (var i = 1; i < (4 - danNum.length) ; i++) {
                        tempNum *= i;
                    }

                    InvestNum = InvestNum / tempNum;
                } else if (danNum.length == 2) {
                    for (var i = 0; i < tuoNum.length; i++)
                        InvestNum = tuoNum.length * 2;
                }
                break;
            case 6812: //组选6普通投注
                InvestNum = 0;
                var rowNum = $("#div_zuxuanputongtouzhu b.rball").text();
                if (rowNum.length >= 2) {
                    InvestNum = this.Combination(rowNum.length, 2);
                }
                break;
            case 6813: //组选6胆拖投注
                InvestNum = 1;
                var danNum = $("#div_dantuo_chongdantuo .tz_ball").eq(0).find("b.rball").text();
                var tuoNum = $("#div_dantuo_chongdantuo .tz_ball").eq(1).find("b.rball").text();
                var tempNum = 1;
                if (((danNum.length + tuoNum.length) < 2) || danNum.length < 1) {
                    InvestNum = 0;
                }


                for (var i = 0; i < (2 - danNum.length) ; i++) {
                    InvestNum *= (tuoNum.length - i);
                }

                for (var i = 1; i < (3 - danNum.length) ; i++) {
                    tempNum *= i;
                }


                InvestNum = (InvestNum / tempNum);
                break;
            case 6814: //组4普通投注
                InvestNum = 0;
                if ($("#div_zuxuanputongtouzhu").find("b.bball").length == 1 && $("#div_zuxuanputongtouzhu").find("b.rball").length == 1) {
                    InvestNum = 1;
                }
                else {
                    var rowNum = $("#div_zuxuanputongtouzhu b.rball").text();
                    InvestNum = this.Combination(rowNum.length, 2) * 2;
                }
                break;
            case 6815: //组4胆拖投注
                InvestNum = 1;
                var danNum = $("#div_dantuo_chongdantuo .tz_ball").eq(0).find("b.rball").text();
                var tuoNum = $("#div_dantuo_chongdantuo .tz_ball").eq(1).find("b.rball").text();
                var tempNum = 1;
                if (((danNum.length + tuoNum.length) < 2) || danNum.length < 1) {
                    InvestNum = 0;
                }

                for (var i = 0; i < (2 - danNum.length) ; i++) {
                    InvestNum *= (tuoNum.length - i);
                }

                for (var i = 1; i < (3 - danNum.length) ; i++) {
                    tempNum *= i;
                }

                InvestNum = 2 * (InvestNum / tempNum);
                break;
            case 6816: //组4重胆拖投注
                InvestNum = 1;
                var danNum = $("#div_dantuo_chongdantuo .tz_ball").eq(0).find("b.rball").text();
                var tuoNum = $("#div_dantuo_chongdantuo .tz_ball").eq(1).find("b.rball").text();
                var tempNum = 1;
                if (((danNum.length + tuoNum.length) < 2) || danNum.length < 1) {
                    InvestNum = 0;
                }

                for (var i = 0; i < (2 - danNum.length) ; i++) {
                    InvestNum *= (tuoNum.length - i);
                }

                for (var i = 1; i < (3 - danNum.length) ; i++) {
                    tempNum *= i;
                }

                InvestNum = InvestNum / tempNum;

                break;
                //--------------快赢481 End------------------//                                                                                                                                                                                                                                                                                  
                //--------------河南22选5 Start--------------//                                                                                                                                                                                                                                                                                 
            case 6901: //直选普通投注
                InvestNum = this.getSelectedRedBallCount() < 2 ? 0 : this.Combination(this.getSelectedRedBallCount(), 5);
                break;
            case 6902: //好运2
                InvestNum = this.getSelectedRedBallCount() < 2 ? 0 : this.Combination(this.getSelectedRedBallCount(), 2);
                break;
            case 6903: //好运3
                InvestNum = this.getSelectedRedBallCount() < 3 ? 0 : this.Combination(this.getSelectedRedBallCount(), 3);
                break;
            case 6904: //好运4
                InvestNum = this.getSelectedRedBallCount() < 4 ? 0 : this.Combination(this.getSelectedRedBallCount(), 4);
                break;
                //--------------河南22选5 End  --------------//                                                                                                                                                                                                                                                                                                                                          

            case 7001: //江西11选5任选一-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount();
                break;
            case 7002: //江西11选5任选二-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 2 ? 0 : this.Combination(this.getSelectedRedBallCount(), 2);
                break;
            case 7015: //江西11选5任选二-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() == 1 ? this.Combination(this.getSelectedTuoBallCount(), 1) : 0;
                break;
            case 7003: //江西11选5任选三-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 3 ? 0 : this.Combination(this.getSelectedRedBallCount(), 3);
                break;
            case 7016: //江西11选5任选三-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 3 ? this.Combination(this.getSelectedTuoBallCount(), 3 - this.getSelectedRedBallCount()) : 0;
                break;
            case 7004: //江西11选5任选四-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 4 ? 0 : this.Combination(this.getSelectedRedBallCount(), 4);
                break;
            case 7017: //江西11选5任选四-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 4 ? this.Combination(this.getSelectedTuoBallCount(), 4 - this.getSelectedRedBallCount()) : 0;
                break;
            case 7005: //江西11选5任选五-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 5 ? 0 : this.Combination(this.getSelectedRedBallCount(), 5);
                break;
            case 7018: //江西11选5任选五-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 5 ? this.Combination(this.getSelectedTuoBallCount(), 5 - this.getSelectedRedBallCount()) : 0;
                break;
            case 7006: //江西11选5任选六-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 6 ? 0 : this.Combination(this.getSelectedRedBallCount(), 6);
                break;
            case 7019: //江西11选5任选六-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 6 ? this.Combination(this.getSelectedTuoBallCount(), 6 - this.getSelectedRedBallCount()) : 0;
                break;
            case 7007: //江西11选5任选七-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 7 ? 0 : this.Combination(this.getSelectedRedBallCount(), 7);
                break;
            case 7020: //江西11选5任选七-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 7 ? this.Combination(this.getSelectedTuoBallCount(), 7 - this.getSelectedRedBallCount()) : 0;
                break;
            case 7008: //江西11选5任选八-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 8 ? 0 : this.Combination(this.getSelectedRedBallCount(), 8);
                break;
            case 7021: //江西11选5任选八-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 8 ? this.Combination(this.getSelectedTuoBallCount(), 8 - this.getSelectedRedBallCount()) : 0;
                break;
            case 7009: //江西11选5前二-普通投注-取注数
                var f_number = this.getSelectedRedBallText(",");
                if (f_number == "" || f_number == null) {
                    InvestNum = 0;
                    break;
                }
                var s_number = this.getSelectedTuoBallText(",");
                if (s_number == "" || s_number == null) {
                    InvestNum = 0;
                    break;
                }
                var first_numbers = f_number.split(",");
                var second_numbers = s_number.split(",");
                var count = 0;
                for (var i = 0; i < first_numbers.length; i++) {
                    for (var j = 0; j < second_numbers.length; j++) {
                        if (first_numbers[i] != second_numbers[j]) {
                            count++;
                        }
                    }
                }
                InvestNum = count;
                break;
            case 7011: //江西11选5前二-组选投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 2 ? 0 : this.Combination(this.getSelectedRedBallCount(), 2);
                break;
            case 7013: //江西11选5前二-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() == 1 ? this.Combination(this.getSelectedTuoBallCount(), 1) : 0;
                break;
            case 7010: //江西11选5前三-普通投注-取注数
                var num1 = this.getSelectedRedBallText(",").split(",");
                var num2 = this.getSelectedTuoBallText(",").split(",");
                var temp = "";
                $("#tz_qian3zhi b.rball").each(function (i, n) {
                    temp += ($.trim($(n).text()) + ",");
                });
                if (temp.substring(temp.length - 1, temp.length) == ",")
                    temp = temp.substring(0, temp.length - 1);
                var num3 = temp.split(",");

                if (num1[0] == "" || num2[0] == "" || num3[0] == "") {
                    InvestNum = 0;
                    break;
                }
                var count = 0;
                for (var i = 0; i < num1.length; i++) {
                    for (var x = 0; x < num2.length; x++) {
                        for (var j = 0; j < num3.length; j++) {
                            if (num1[i] != num2[x] && num1[i] != num3[j] && num2[x] != num3[j]) {
                                count++;
                            }
                        }
                    }
                }
                InvestNum = count;
                break;
            case 7012: //江西11选5前三-组选投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 3 ? 0 : this.Combination(this.getSelectedRedBallCount(), 3);
                break;
            case 7014: //江西11选5前三-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 3 ? this.Combination(this.getSelectedTuoBallCount(), 3 - this.getSelectedRedBallCount()) : 0;
                break;
            case 7401: //足球胜负-普通投注-取注数
                InvestNum = 1;
                $("#tz_sfc ul").each(function (i, n_ul) {
                    InvestNum *= $(n_ul).find("b.rball").length;
                });
                break;
            case 7501: //任九场-普通投注-取注数
                InvestNum = 1;
                var count = 0;
                $("#tz_rjc ul").each(function (i, n_ul) {
                    if ($(n_ul).find("b.rball").length > 0) {
                        InvestNum *= $(n_ul).find("b.rball").length;
                        count++;
                    }
                });
                if (count < 9) {
                    InvestNum = 0;
                }
                break;


            case 7801: //广东11选5任选一-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount();
                break;
            case 7802: //广东11选5任选二-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 2 ? 0 : this.Combination(this.getSelectedRedBallCount(), 2);
                break;
            case 7815: //广东11选5任选二-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() == 1 ? this.Combination(this.getSelectedTuoBallCount(), 1) : 0;
                break;
            case 7803: //广东11选5任选三-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 3 ? 0 : this.Combination(this.getSelectedRedBallCount(), 3);
                break;
            case 7816: //广东11选5任选三-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 3 ? this.Combination(this.getSelectedTuoBallCount(), 3 - this.getSelectedRedBallCount()) : 0;
                break;
            case 7804: //广东11选5任选四-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 4 ? 0 : this.Combination(this.getSelectedRedBallCount(), 4);
                break;
            case 7817: //广东11选5任选四-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 4 ? this.Combination(this.getSelectedTuoBallCount(), 4 - this.getSelectedRedBallCount()) : 0;
                break;
            case 7805: //广东11选5任选五-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 5 ? 0 : this.Combination(this.getSelectedRedBallCount(), 5);
                break;
            case 7818: //广东11选5任选五-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 5 ? this.Combination(this.getSelectedTuoBallCount(), 5 - this.getSelectedRedBallCount()) : 0;
                break;
            case 7806: //广东11选5任选六-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 6 ? 0 : this.Combination(this.getSelectedRedBallCount(), 6);
                break;
            case 7819: //广东11选5任选六-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 6 ? this.Combination(this.getSelectedTuoBallCount(), 6 - this.getSelectedRedBallCount()) : 0;
                break;
            case 7807: //广东11选5任选七-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 7 ? 0 : this.Combination(this.getSelectedRedBallCount(), 7);
                break;
            case 7820: //广东11选5任选七-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 7 ? this.Combination(this.getSelectedTuoBallCount(), 7 - this.getSelectedRedBallCount()) : 0;
                break;
            case 7808: //广东11选5任选八-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 8 ? 0 : this.Combination(this.getSelectedRedBallCount(), 8);
                break;
            case 7821: //广东11选5任选八-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 8 ? this.Combination(this.getSelectedTuoBallCount(), 8 - this.getSelectedRedBallCount()) : 0;
                break;
            case 7809: //广东11选5前二-普通投注-取注数
                var f_number = this.getSelectedRedBallText(",");
                if (f_number == "" || f_number == null) {
                    InvestNum = 0;
                    break;
                }
                var s_number = this.getSelectedTuoBallText(",");
                if (s_number == "" || s_number == null) {
                    InvestNum = 0;
                    break;
                }
                var first_numbers = f_number.split(",");
                var second_numbers = s_number.split(",");
                var count = 0;
                for (var i = 0; i < first_numbers.length; i++) {
                    for (var j = 0; j < second_numbers.length; j++) {
                        if (first_numbers[i] != second_numbers[j]) {
                            count++;
                        }
                    }
                }
                InvestNum = count;
                break;
            case 7811: //广东11选5前二-组选投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 2 ? 0 : this.Combination(this.getSelectedRedBallCount(), 2);
                break;
            case 7813: //广东11选5前二-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() == 1 ? this.Combination(this.getSelectedTuoBallCount(), 1) : 0;
                break;
            case 7810: //广东11选5前三-普通投注-取注数
                var num1 = this.getSelectedRedBallText(",").split(",");
                var num2 = this.getSelectedTuoBallText(",").split(",");
                var temp = "";
                $("#tz_qian3zhi b.rball").each(function (i, n) {
                    temp += ($.trim($(n).text()) + ",");
                });
                if (temp.substring(temp.length - 1, temp.length) == ",")
                    temp = temp.substring(0, temp.length - 1);
                var num3 = temp.split(",");
                if (num1[0] == "" || num2[0] == "" || num3[0] == "") {
                    InvestNum = 0;
                    break;
                }
                var count = 0;
                for (var i = 0; i < num1.length; i++) {
                    for (var x = 0; x < num2.length; x++) {
                        for (var j = 0; j < num3.length; j++) {
                            if (num1[i] != num2[x] && num1[i] != num3[j] && num2[x] != num3[j]) {
                                count++;
                            }
                        }
                    }
                }
                InvestNum = count;
                break;
            case 7812: //广东11选5前三-组选投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 3 ? 0 : this.Combination(this.getSelectedRedBallCount(), 3);
                break;
            case 7814: //广东11选5前三-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 3 ? this.Combination(this.getSelectedTuoBallCount(), 3 - this.getSelectedRedBallCount()) : 0;
                break;


                //---------------幸运武林---------------//                                                                                                                                                                                                                                                                                                                                                                        
            case 8201:
                InvestNum = 1;
                InvestNum = $("#div_XYWL_XC b.rball").length;
                break;
            case 8202:
                InvestNum = 1;
                var countNum = 0;
                $("#div_XYWL_XC ul").each(function (i, n_ul) {

                    if ($(n_ul).find("b.rball").length >= 1) {
                        countNum++;
                        InvestNum *= $(n_ul).find("b.rball").length;
                    }
                });
                if (countNum >= 2) {
                } else { InvestNum = 0; }
                break;
            case 8203:
                InvestNum = 1;
                var countNum = 0;
                $("#div_XYWL_XC ul").each(function (i, n_ul) {

                    if ($(n_ul).find("b.rball").length >= 1) {
                        countNum++;
                        InvestNum *= $(n_ul).find("b.rball").length;
                    }
                });

                if (countNum >= 3) {
                } else { InvestNum = 0; }

                break;

            case 8204:
                InvestNum = $("#div_XYWL_RX b.rball").length;
                break;
            case 8205:
                InvestNum = this.getSelectedRedBallCount() < 2 ? 0 : this.Combination(this.getSelectedRedBallCount(), 2);
                //InvestNum = $("#div_XYWL_XC b.rball").length;
                break;
            case 8206:
                InvestNum = this.getSelectedRedBallCount() < 3 ? 0 : this.Combination(this.getSelectedRedBallCount(), 3);
                //InvestNum = $("#div_XYWL_XC b.rball").length;
                break;
            case 8207:
                InvestNum = this.getSelectedRedBallCount() < 4 ? 0 : this.Combination(this.getSelectedRedBallCount(), 4);
                //InvestNum = $("#div_XYWL_XC b.rball").length;
                break;
            case 8208:
                InvestNum = this.getSelectedRedBallCount() < 5 ? 0 : this.Combination(this.getSelectedRedBallCount(), 5);
                //InvestNum = $("#div_XYWL_XC b.rball").length;
                break;
            case 8209:
                InvestNum = this.getSelectedRedBallCount() < 6 ? 0 : this.Combination(this.getSelectedRedBallCount(), 6);
                //InvestNum = $("#div_XYWL_XC b.rball").length;
                break;
            case 8210:
                InvestNum = this.getSelectedRedBallCount() < 7 ? 0 : this.Combination(this.getSelectedRedBallCount(), 7);
                //InvestNum = $("#div_XYWL_XC b.rball").length;
                break;
            case 8211:
                InvestNum = this.getSelectedRedBallCount();
                //InvestNum = $("#div_XYWL_XC b.rball").length;
                break;
            case 8212:
                // 取号码
                var s1 = "", s2 = "";
                //第一镖 选中的号码
                var obj = $($("#div_XYWL_SX li[mark='1']")[0]).find("b.rball");
                //第二镖 选中的号码
                var obj2 = $($("#div_XYWL_SX li[mark='2']")[0]).find("b.rball");

                for (var i = 0; i < obj.length; i++) {
                    s1 += $(obj[i]).text() + " ";
                }
                for (var j = 0; j < obj2.length; j++) {
                    s2 += $(obj2[j]).text() + " ";
                }
                if (s1 == "" || s2 == "") {
                    InvestNum = 0;
                } else {
                    var ary_s1 = trim(s1).split(' ');
                    var ary_s2 = trim(s2).split(' ');
                    var repeat = "", number = "";
                    // 取注数
                    for (var x = 0; x < ary_s1.length; x++) {
                        for (var j = 0; j < ary_s2.length; j++) {
                            if (trim(ary_s1[x]) == trim(ary_s2[j])) {
                                continue;
                            }
                            number = (ary_s1[x] + " " + ary_s2[j] + "\n");
                            if (repeat.indexOf(number) == -1) {
                                repeat += number;
                            }
                        }
                    }
                    if (repeat == "")
                        InvestNum = 0;
                    else
                        InvestNum = trim(repeat).split('\n').length;

                }

                break;
            case 8213:
                // 取号码
                var s1 = "", s2 = "", s3 = "";
                //第一镖 选中的号码
                var obj = $($("#div_XYWL_SX li[mark='1']")[0]).find("b.rball");
                //第二镖 选中的号码
                var obj2 = $($("#div_XYWL_SX li[mark='2']")[0]).find("b.rball");
                //第三镖 选中的号码
                var obj3 = $($("#div_XYWL_SX li[mark='3']")[0]).find("b.rball");

                for (var i = 0; i < obj.length; i++) {
                    s1 += $(obj[i]).text() + " ";
                }
                for (var j = 0; j < obj2.length; j++) {
                    s2 += $(obj2[j]).text() + " ";
                }
                for (var c = 0; c < obj3.length; c++) {
                    s3 += $(obj3[c]).text() + " ";
                }

                if (s1 == "" || s2 == "" || s3 == "") {
                    InvestNum = 0;

                } else {
                    var ary_s1 = trim(s1).split(' ');
                    var ary_s2 = trim(s2).split(' ');
                    var ary_s3 = trim(s3).split(' ');
                    var repeat = "", number = "";
                    // 取注数
                    for (var x = 0; x < ary_s1.length; x++) {
                        for (var j = 0; j < ary_s2.length; j++) {
                            for (var k = 0; k < ary_s3.length; k++) {
                                if ((trim(ary_s1[x]) == trim(ary_s2[j])) || (trim(ary_s1[x]) == trim(ary_s3[k])) || (trim(ary_s2[j]) == trim(ary_s3[k]))) {
                                    continue;
                                }
                                number = (ary_s1[x] + " " + ary_s2[j] + " " + ary_s3[k] + "\n");;
                                if (repeat.indexOf(number) == -1) {
                                    repeat += number;
                                }
                            }
                        }
                    }
                    if (repeat == "")
                        InvestNum = 0;
                    else
                        InvestNum = trim(repeat).split('\n').length;
                }

                break;

            case 8214:
                InvestNum = GetLotteryInvestNum();
                break;


            case 8301: //和值-普通投注-取注数
            case 8302: //三同号通选-普通投注-取注数
            case 8303: //三同号单选-普通投注-取注数
            case 8308: //三连号通选-普通投注-取注数
            case 8304: //二同号复选-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount();
                break;
            case 8306: //三不同号-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 2 ? this.Combination(this.getSelectedRedBallCount(), 3) : 0;
                break;
            case 8305: //二同号单选-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() * this.getSelectedTuoBallCount();
                break;
            case 8307: //二不同号-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 1 ? this.Combination(this.getSelectedRedBallCount(), 2) : 0;
                break;


            case 8701: //湖南幸运赛车-普通投注-取注数
            case 8708: //湖南幸运赛车-位置投注-取注数
                InvestNum = this.getSelectedRedBallCount();
                break;
            case 8702: //湖南幸运赛车-前二普通定位投注-取注数
                var f_number = this.getSelectedRedBallText(",");
                if (f_number == "" || f_number == null) {
                    InvestNum = 0;
                    break;
                }
                var s_number = this.getSelectedTuoBallText(",");
                if (s_number == "" || s_number == null) {
                    InvestNum = 0;
                    break;
                }
                var first_numbers = f_number.split(",");
                var second_numbers = s_number.split(",");
                var count = 0;
                for (var i = 0; i < first_numbers.length; i++) {
                    for (var j = 0; j < second_numbers.length; j++) {
                        if (first_numbers[i] != second_numbers[j]) {
                            count++;
                        }
                    }
                }
                InvestNum = count;
                break;
            case 8703: //湖南幸运赛车-前二复式投注-取注数
                InvestNum = this.getSelectedRedBallCount() * (this.getSelectedRedBallCount() - 1);
                break;
            case 8704: //湖南幸运赛车-前二胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() == 1 ? this.Combination(this.getSelectedTuoBallCount() > 1 ? this.getSelectedTuoBallCount() : 0, 1) * 2 : 0;
                break;
            case 8705: //湖南幸运赛车-前三普通定位投注-取注数
                var num1 = this.getSelectedRedBallText(",").split(",");
                var num2 = this.getSelectedTuoBallText(",").split(",");
                var temp = "";
                $("#tz_qian3zhi b.rball").each(function (i, n) {
                    temp += ($.trim($(n).text()) + ",");
                });
                if (temp.substring(temp.length - 1, temp.length) == ",")
                    temp = temp.substring(0, temp.length - 1);
                var num3 = temp.split(",");

                if (num1[0] == "" || num2[0] == "" || num3[0] == "") {
                    InvestNum = 0;
                    break;
                }
                var count = 0;
                for (var i = 0; i < num1.length; i++) {
                    for (var x = 0; x < num2.length; x++) {
                        for (var j = 0; j < num3.length; j++) {
                            if (num1[i] != num2[x] && num1[i] != num3[j] && num2[x] != num3[j]) {
                                count++;
                            }
                        }
                    }
                }
                InvestNum = count;
                break;
            case 8706: //湖南幸运赛车-前三复式投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 2 ? this.Combination(this.getSelectedRedBallCount(), 3) * 6 : 0;
                break;
            case 8707: //湖南幸运赛车-前三胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() == 1 ? this.Combination(this.getSelectedTuoBallCount() > 2 ? this.getSelectedTuoBallCount() : 0, 2) * 6 : this.getSelectedRedBallCount() == 2 ? this.Combination(this.getSelectedTuoBallCount() > 1 ? this.getSelectedTuoBallCount() : 0, 1) * 6 : 0;
                break;
            case 8709: //湖南幸运赛车-过两关普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() * this.getSelectedTuoBallCount();
                break;
            case 8710: //湖南幸运赛车-过两关胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() == 1 ? this.Combination(this.getSelectedTuoBallCount() > 1 ? this.getSelectedTuoBallCount() : 0, 1) * 2 : 0;
                break;
            case 8711: //湖南幸运赛车-过三关普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() * this.getSelectedTuoBallCount() * $("#tz_qian3zhi").find("b.rball").length;
                break;
            case 8712: //湖南幸运赛车-过三关胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() == 1 ? this.Combination(this.getSelectedTuoBallCount() > 2 ? this.getSelectedTuoBallCount() : 0, 2) * 6 : this.getSelectedRedBallCount() == 2 ? this.Combination(this.getSelectedTuoBallCount() > 1 ? this.getSelectedTuoBallCount() : 0, 1) * 6 : 0;
                break;
            case 8713: //湖南幸运赛车-大小奇偶投注-取注数
                InvestNum = $("#ul_dxjo").find("b.rball").length;
                break;
            case 8714: //湖南幸运赛车-组二普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 1 ? this.Combination(this.getSelectedRedBallCount(), 2) : 0;
                break;
            case 8715: //湖南幸运赛车-组二胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() == 1 ? this.getSelectedTuoBallCount() > 1 ? this.getSelectedTuoBallCount() : 0 : 0;
                break;
            case 8716: //湖南幸运赛车-组三普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 2 ? this.Combination(this.getSelectedRedBallCount(), 3) : 0;
                break;
            case 8717: //湖南幸运赛车-组三胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() == 1 ? this.Combination(this.getSelectedTuoBallCount() > 2 ? this.getSelectedTuoBallCount() : 1, 2) : this.getSelectedRedBallCount() == 2 ? this.Combination(this.getSelectedTuoBallCount() > 1 ? this.getSelectedTuoBallCount() : 0, 1) : 0;
                break;

                ////////////////上海11选5//////////////////
            case 8801: //上海11选5任选一-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount();
                break;
            case 8802: //上海11选5任选二-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 2 ? 0 : this.Combination(this.getSelectedRedBallCount(), 2);
                break;
            case 8815: //上海11选5任选二-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() == 1 ? this.Combination(this.getSelectedTuoBallCount(), 1) : 0;
                break;
            case 8803: //上海11选5任选三-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 3 ? 0 : this.Combination(this.getSelectedRedBallCount(), 3);
                break;
            case 8816: //上海11选5任选三-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 3 ? this.Combination(this.getSelectedTuoBallCount(), 3 - this.getSelectedRedBallCount()) : 0;
                break;
            case 8804: //上海11选5任选四-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 4 ? 0 : this.Combination(this.getSelectedRedBallCount(), 4);
                break;
            case 8817: //上海11选5任选四-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 4 ? this.Combination(this.getSelectedTuoBallCount(), 4 - this.getSelectedRedBallCount()) : 0;
                break;
            case 8805: //上海11选5任选五-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 5 ? 0 : this.Combination(this.getSelectedRedBallCount(), 5);
                break;
            case 8818: //上海11选5任选五-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 5 ? this.Combination(this.getSelectedTuoBallCount(), 5 - this.getSelectedRedBallCount()) : 0;
                break;
            case 8806: //上海11选5任选六-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 6 ? 0 : this.Combination(this.getSelectedRedBallCount(), 6);
                break;
            case 8819: //上海11选5任选六-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 6 ? this.Combination(this.getSelectedTuoBallCount(), 6 - this.getSelectedRedBallCount()) : 0;
                break;
            case 8807: //上海11选5任选七-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 7 ? 0 : this.Combination(this.getSelectedRedBallCount(), 7);
                break;
            case 8820: //上海11选5任选七-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 7 ? this.Combination(this.getSelectedTuoBallCount(), 7 - this.getSelectedRedBallCount()) : 0;
                break;
            case 8808: //上海11选5任选八-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 8 ? 0 : this.Combination(this.getSelectedRedBallCount(), 8);
                break;
            case 8821: //上海11选5任选八-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 8 ? this.Combination(this.getSelectedTuoBallCount(), 8 - this.getSelectedRedBallCount()) : 0;
                break;
            case 8809: //上海11选5前二-普通投注-取注数
                var f_number = this.getSelectedRedBallText(",");
                if (f_number == "" || f_number == null) {
                    InvestNum = 0;
                    break;
                }
                var s_number = this.getSelectedTuoBallText(",");
                if (s_number == "" || s_number == null) {
                    InvestNum = 0;
                    break;
                }
                var first_numbers = f_number.split(",");
                var second_numbers = s_number.split(",");
                var count = 0;
                for (var i = 0; i < first_numbers.length; i++) {
                    for (var j = 0; j < second_numbers.length; j++) {
                        if (first_numbers[i] != second_numbers[j]) {
                            count++;
                        }
                    }
                }
                InvestNum = count;
                break;
            case 8811: //上海11选5前二-组选投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 2 ? 0 : this.Combination(this.getSelectedRedBallCount(), 2);
                break;
            case 8813: //上海11选5前二-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() == 1 ? this.Combination(this.getSelectedTuoBallCount(), 1) : 0;
                break;
            case 8810: //上海11选5前三-普通投注-取注数
                var num1 = this.getSelectedRedBallText(",").split(",");
                var num2 = this.getSelectedTuoBallText(",").split(",");
                var temp = "";
                $("#tz_qian3zhi b.rball").each(function (i, n) {
                    temp += ($.trim($(n).text()) + ",");
                });
                if (temp.substring(temp.length - 1, temp.length) == ",")
                    temp = temp.substring(0, temp.length - 1);
                var num3 = temp.split(",");
                if (num1[0] == "" || num2[0] == "" || num3[0] == "") {
                    InvestNum = 0;
                    break;
                }
                var count = 0;
                for (var i = 0; i < num1.length; i++) {
                    for (var x = 0; x < num2.length; x++) {
                        for (var j = 0; j < num3.length; j++) {
                            if (num1[i] != num2[x] && num1[i] != num3[j] && num2[x] != num3[j]) {
                                count++;
                            }
                        }
                    }
                }
                InvestNum = count;
                break;
            case 8812: //上海11选5前三-组选投注-取注数
                InvestNum = this.getSelectedRedBallCount() < 3 ? 0 : this.Combination(this.getSelectedRedBallCount(), 3);
                break;
            case 8814: //上海11选5前三-胆拖投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 0 && this.getSelectedRedBallCount() < 3 ? this.Combination(this.getSelectedTuoBallCount(), 3 - this.getSelectedRedBallCount()) : 0;
                break;


            case 8901: //广西快3和值-普通投注-取注数
            case 8902: //广西快3三同号通选-普通投注-取注数
            case 8903: //广西快3三同号单选-普通投注-取注数
            case 8908: //广西快3三连号通选-普通投注-取注数
            case 8904: //广西快3二同号复选-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount();
                break;
            case 8906: //广西快3三不同号-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 2 ? this.Combination(this.getSelectedRedBallCount(), 3) : 0;
                break;
            case 8905: //广西快3二同号单选-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() * this.getSelectedTuoBallCount();
                break;
            case 8907: //广西快3二不同号-普通投注-取注数
                InvestNum = this.getSelectedRedBallCount() > 1 ? this.Combination(this.getSelectedRedBallCount(), 2) : 0;
                break;

            default:
                break;
        }

        this.TotalInvestNum = InvestNum;
    },

    //取选中红球个数,在胆拖玩法中表示胆码的个数
    getSelectedRedBallCount: function () {
        return $("#" + this.SelectRAreaID + " b.rball").length;
    },

    //取选中拖球个数
    getSelectedTuoBallCount: function () {
        return $("#" + this.SelectTAreaID + " b.rball").length;
    },

    //取选中蓝球个数
    getSelectedBlueBallCount: function () {
        return $("#" + this.SelectBAreaID + " b.bball").length;
    },

    //取选中蓝球个数
    getSelectedBlueBTAreaBallCount: function () {
        return $("#" + this.SelectBTAreaID + " b.bball").length;
    },

    //机选 1 注
    RandClick: function () {
        this.ClearSelect(0);

        this.RandBall(1, 33, 6);
        this.RandBall(2, 16, 1);
        this.btnAddPickClick();

        $("#btnAddPick").unbind('click');
    },

    //机选 n 注 
    RandManyClick: function (n) {
        for (var i = 1; i <= n; i++) {
            this.ClearSelect(0);

            this.RandBall(1, 33, 6);
            this.RandBall(2, 16, 1);
            this.btnAddPickClick();
        }

        $("#btnAddPick").unbind('click');
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

        $("#sumAmount").html("购买注数：<font color='red'>" + this.TotalInvestNum + "</font> 注");
        if (this.PlayTypeID == 602 || this.PlayTypeID == 6302) {
            $("#sumMoney").html("购买金额：<font color='red'>" + this.TotalInvestNum * this.Price + "</font> 元<font color='red'>&nbsp;&nbsp;&nbsp;组三号码请将重复号多点击一次 </font>");
        }
        else {
            $("#sumMoney").html("购买金额：<font color='red'>" + this.TotalInvestNum * this.Price + "</font> 元");
        }
        if (this.PlayTypeID == 2811) {
            if (this.TotalInvestNum == 1) {
                $("#sumMoney").html("购买金额：<font color='red'>" + this.TotalInvestNum * this.Price + "</font> 元&nbsp;&nbsp;&nbsp;<font color='red'>组三号码请将重复号多点击一次</font>");
            }
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

    //单式上传，按键控制，必须是数字键及合法的符号键
    MustBeLegalKey: function () {
        if (window.event.keyCode == 43 || window.event.keyCode == 44 || window.event.keyCode == 124 || window.event.keyCode == 61 || window.event.keyCode == 32 || window.event.keyCode == 40 || window.event.keyCode == 41 || window.event.keyCode == 13)
            return true;
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

    //购买方式改变，代购、追号、合买
    ChangeBuyWay: function () {

        if ($("#CoBuy").is(":checked")) {
            //合买
            //  var Opt_InitiateSchemeLimitUpperScale = StrToFloat($("#HidOpt_InitiateSchemeLimitUpperScale").val());
            var HiOpt_InitiateSchemeMinBuyAndAssureScale = StrToFloat($("#HiOpt_InitiateSchemeMinBuyAndAssureScale").val());

            var Money = StrToFloat($("#lab_SumMoney").html(), 1);
            //  var BuyShare = Math.round(Money * Opt_InitiateSchemeLimitUpperScale);
            var BuyShare = Math.round(Money * HiOpt_InitiateSchemeMinBuyAndAssureScale);
            $("#tb_Share").val(Money < 1 ? "1" : Money);
            $("#tb_BuyShare").val(BuyShare < 1 ? "1" : BuyShare);
            $("#trGoon").hide();
            $("#trShowJion").show();
            $("#chase_selected").hide();
            $("#tbAutoStopAtWinMoney").attr("checked", false);

            $("#tb_Multiple").attr("disabled", false).css("height", $("#tb_Multiple").height() == "16" ? "17" : "16");
            $("#CoBuy").css({ "height": "20px" });
            $("#div_gmfstip").text("由多人共同出资购买彩票");
        }
        if ($("#Chase").is(":checked")) {

            //追号，将合买初始化
            $("#tb_Share").val("1");
            $("#tb_BuyShare").val("1");
            $("#tb_AssureShare").val("0");
            $("#tb_Multiple").val("1");
            $("#trGoon").show();
            $("#trShowJion").hide();
            $("#chase_selected").show();
            $("#tbAutoStopAtWinMoney").attr("checked", true);

            $("#tb_Multiple").attr("disabled", true).css("height", $("#tb_Multiple").height() == "16" ? "17" : "16");
            $("#Chase").css({ "height": "20px" });

            $("#div_QH_Today").height($("#RpToday").height() > 200 ? 200 : $("#RpToday").height());
            $("#div_gmfstip").text("连续多期购买同样的号码");


            //幸运武林
            if (this.LotID == 82) {
                $("#spanChaseIsuseCount").text(1);
                $("#LbChaseMoney").text(2);
                $("#spanWinMoney").text($("#spanPlayTypeMoney").text());
                $("#spanSchemeProfit").text(parseFloat($("#spanPlayTypeMoney").text()) - parseFloat($("#LbChaseMoney").text()));
                //
            }
        }
        if ($("#GenBuy").is(":checked")) {
            //代购，将合买初始化
            $("#tb_Share").val("1");
            $("#tb_BuyShare").val("1");
            $("#tb_AssureShare").val("0");
            $("#chase_selected").hide();
            $("#tbAutoStopAtWinMoney").attr("checked", false);

            $("#tb_Multiple").attr("disabled", false).css("height", $("#tb_Multiple").height() == "16" ? "17" : "16");
            $("#trGoon").hide();
            $("#trShowJion").hide();
            $("#div_gmfstip").text("购买人自行全额购");
        }

        this.CalcSumMoney();
    },

    //改变购买方式的可见性genBuy,coBuy,chase 大于0表示可见
    ChangeVisibleOfBuyWay: function (genBuy, coBuy, chase) {
        if (genBuy > 0) {
            $("#GenBuy").show();
            $("#lab_GenBuy").show();
        } else {
            $("#GenBuy").hide();
            $("#lab_GenBuy").hide();
        }
        if (coBuy > 0) {
            $("#CoBuy").show();
            $("#lab_CoBuy").show();
        } else {
            $("#CoBuy").hide();
            $("#lab_CoBuy").hide();
        }
        if (chase > 0) {
            $("#Chase").show();
            $("#lab_Chase").show();
        } else {
            $("#Chase").hide();
            $("#lab_Chase").hide();
        }
    },

    //玩法切换，清空列表，回到代购方式
    ChangePlayTypeInit: function () {
        $("#tr_invest_list").show();
        $("#div_btn_queren").show();
        $("#GenBuy").click();
        Lottery.ChangeBuyWay();
        Lottery.ClearInvestList();
    },

    //上传方式切换
    ChangeUploadWays: function () {
        $("#txtSelectType").val(1);
        if ($("#upload_now").is(":checked")) {
            $("#div_upload_now").show();
            $("#div_upload_after").hide();
            $("#tr_invest_list").show();
            $("#div_btn_queren").show();
            this.ChangeVisibleOfBuyWay(1, 1, 1);
            this.ChangePlayTypeInit();
        }
        else if ($("#upload_after").is(":checked")) {//先投注后上传
            $("#txtSelectType").val(3);
            $("#invest_total_count").val("");
            $("#invest_total_multiple").val(1);
            $("#div_upload_now").hide();
            $("#div_upload_after").show();
            $("#tr_invest_list").hide();
            $("#div_btn_queren").hide();
            this.ChangeVisibleOfBuyWay(0, 1, 0);
            $("#CoBuy").click();
            Lottery.ChangeBuyWay();
        }
    },

    //先发起后上传功能，计算金额
    CalculateMoneyUploadAfter: function () {
        var totalMoney = (parseInt($("#invest_total_count").val(), 10) < 1 ? 0 : parseInt($("#invest_total_count").val(), 10)) * (parseInt($("#invest_total_multiple").val(), 10) < 0 ? 0 : parseInt($("#invest_total_multiple").val(), 10)) * this.Price;
        $("#invest_total_money").text("￥" + totalMoney.toFixed(2));
        if (isNaN(totalMoney)) {
            $("#invest_total_money").text("￥0.00");
            return;
        }
        CodeArea.CodeList = [];
        $("#tb_Multiple").val(parseInt($("#invest_total_multiple").val(), 10) < 0 ? 0 : parseInt($("#invest_total_multiple").val(), 10));
        CodeArea.PlayName = "";
        CodeArea.Code = "";
        CodeArea.InvestNum = parseInt($("#invest_total_count").val(), 10) < 1 ? 0 : parseInt($("#invest_total_count").val(), 10);
        CodeArea.ShowContent = "";
        CodeArea.AddTextToCodeArea();
        this.CalcSumMoney();
    },

    //足彩预投
    PreviouslyInvest: function () {
        CodeArea.CodeList = [];
        $("#tb_Multiple").val(1);
        CodeArea.PlayName = "";
        CodeArea.Code = "";
        CodeArea.InvestNum = 5;
        CodeArea.ShowContent = "";
        CodeArea.AddTextToCodeArea();
        this.CalcSumMoney();
    },

    //增加合买描述
    Add_scheme_desc: function () {
        if ($("#Add_scheme_title").is(":checked")) {
            $("#scheme_title").show();
            $("#scheme_desc").show();
        }
        else {
            $("#scheme_title").hide();
            $("#scheme_desc").hide();
        }
    },

    //校验合买份数是否符合分割条件
    ValidateShareSpilt: function () {
        var HiOpt_InitiateSchemeMinBuyAndAssureScale = StrToFloat($("#HiOpt_InitiateSchemeMinBuyAndAssureScale").val());
        var SumShare = StrToInt($("#tb_Share").val());
        var Share = Math.round(SumShare * HiOpt_InitiateSchemeMinBuyAndAssureScale);
        Share = Share == 0 ? 0 : Share;
        var OK = false;

        $("#tb_Share").val(SumShare);

        if (SumShare < 0) {
            msg("输入的份数非法。");

            OK = false;
        }
        else if (SumShare == 1) {
            OK = true;
        }
        else {
            if (SumShare > 1) {
                var multiple = StrToInt($("#tb_Multiple").val());
                var SumNum = StrToInt($("#lab_Num").text());
                var SumMoney = multiple * this.Price * SumNum;
                var ShareMoney = SumMoney / SumShare;
                var ShareMoney2 = Math.round(ShareMoney * 100) / 100;

                if (ShareMoney == ShareMoney2)
                    OK = true;

                if (ShareMoney < 1) {
                    OK = false;
                }
            }
        }

        if (!OK) {
            if ($("#lab_SumMoney").html() == "0") {
                Share = 1;
                $("#tb_Share").val(1);
                return;
            }
            var okfunc = function () { $("#tb_Share").focus(); };
            var cancelfunc = function () {
                Share = 1;
                $("#tb_Share").val(1);
            };
            confirm("份数为 0 或者不能除尽，将产生误差，并且金额不能小于 1 元。<br>按“确定”重新输入，按“取消”自动更正为 1 份，请选择。", okfunc, cancelfunc);
            return;
        }
        if (parseInt($("#tb_AssureShare").val()) <= 0) {
            $("#tb_AssureShare").val("0");
        }

        if (parseInt($("#tb_BuyShare").val()) <= parseInt(Share)) {
            $("#tb_BuyShare").val(Share);
        }
        this.CalcSumMoney();
    },

    //校验合买中发起人的认购份数是否达到最低标准
    ValidateInitIDBuyShare: function () {

        var BuyShare = StrToInt($("#tb_BuyShare").val());
        var Share = StrToInt($("#tb_Share").val());
        var AssureShare = StrToInt($("#tb_AssureShare").val());

        $("#tb_BuyShare").val(BuyShare);
        $("#tb_Share").val(Share);
        $("#tb_AssureShare").val(AssureShare);

        if ((BuyShare < 1) || (BuyShare > Share)) {
            var okfunc = function () { $("#tb_BuyShare").focus(); };
            var cancelfunc = function () {
                $("#tb_BuyShare").val(Share);
                BuyShare = Share;
            };
            confirm("购买份数不能为 0 以及大于总份数同时份数必须为整数。<br>按“确定”重新输入，按“取消”自动更正为 " + Share + " 份，请选择。", okfunc, cancelfunc);
            return;
        }

        if ((BuyShare + AssureShare) > Share) {
            AssureShare = Share - BuyShare;
            msg("购买和保底份数大于总份数，保底份数自动调整为 " + AssureShare + "。");
            $("#tb_AssureShare").val(AssureShare);
        }
        var SumShare = StrToInt($("#lab_SumMoney").html());
        var ShareNumber = StrToInt($("#tb_Share").val());
        //        $("#tb_Share").val(SumShare == 0 ? 1 : SumShare);
        var HiOpt_InitiateSchemeMinBuyAndAssureScale = StrToFloat($("#HiOpt_InitiateSchemeMinBuyAndAssureScale").val());
        var LeastShare = Math.round(SumShare * HiOpt_InitiateSchemeMinBuyAndAssureScale);
        if ((BuyShare + AssureShare) < LeastShare) {
            LeastShare = LeastShare - AssureShare;
            if (LeastShare > ShareNumber) {
                LeastShare = StrToInt($("#tb_Share").val());
            }
            msg("购买和保底份数小于系统设置最低购买分数，购买份数自动调整为 " + LeastShare + "。");
            $("#tb_BuyShare").val(LeastShare);
        }

        this.CalcSumMoney();
        return;
    },

    //校验保底设置是否正确
    ValidateAssureShare: function () {
        var Share = StrToInt($("#tb_Share").val());
        var AssureShare = StrToInt($("#tb_AssureShare").val());
        var BuyShare = StrToInt($("#tb_BuyShare").val());

        $("#tb_Share").val(Share);
        $("#tb_AssureShare").val(AssureShare);
        $("#tb_BuyShare").val(BuyShare);

        if (AssureShare < 0) {
            msg("输入的保底份数非法。");
            $("#tb_AssureShare").val("0");
            this.CalcSumMoney();
            return;
        }

        if ((Share == 1) && (AssureShare > 0)) {
            msg("此方案只分为 1 份，不能保底。");
            $("#tb_AssureShare").val("0");
            this.CalcSumMoney();
            return;
        }
        if (AssureShare > (Share - 1)) {
            var AutoAssureShare = Math.round(Share / 2);
            if ((AutoAssureShare + BuyShare) > Share)
                AutoAssureShare = Share - BuyShare;
            var okfunc = function () {
                $("#tb_AssureShare").focus();
            };
            var cancelfunc = function () {
                $("#tb_AssureShare").val(AutoAssureShare);
                AssureShare = AutoAssureShare;
            };
            confirm("保底份数不能大于和等于总份数。按“确定”重新输入，按“取消”自动更正为 " + AutoAssureShare + " 份，请选择。", okfunc, cancelfunc);
            return;
        }
        if ((BuyShare + AssureShare) > Share) {
            BuyShare = Share - AssureShare;
            msg("购买份数与保底份数和大于总份数，购买份数自动调整为 " + BuyShare + " 份。");
            $("#tb_BuyShare").val(BuyShare);
        }

        this.CalcSumMoney();
        return;
    },

    //追号的全选按钮状态改变事件
    ChaseCheckedBoxSelectedChange: function () {
        $("#div_QH_Today input:checkbox").each(function (i, n) {
            if (!($("#cb_All").is(":checked") && $(this).is(":checked"))) {
                $(this).attr("checked", $("#cb_All").is(":checked"));
                Lottery.ChaseChildCheckedBoxSelectedChange($(this));
            }
        });
    },

    //追号，某期的复选框状态改变事件
    ChaseChildCheckedBoxSelectedChange: function (obj) {
        if (obj.length <= 0) {
            return;
        }

        var obj_TxtBNum = $("#" + obj[0].id.replace("check", "times"));
        var obj_TxtMoney = $("#" + obj[0].id.replace("check", "money"));

        if (obj.is(":checked")) {
            obj_TxtBNum.val("1").attr("disabled", false).css("width", obj_TxtBNum.width() == "44" ? "45" : "44");
        }
        else {
            obj_TxtBNum.val("1").attr("disabled", true).css("width", obj_TxtBNum.width() == "44" ? "45" : "44");
            obj_TxtMoney.val("0");
        }

        this.ChaseCalculateTotalMoney();
    },

    //追号,计算追号任务的总金额
    ChaseCalculateTotalMoney: function () {
        var needMoney = 0;

        $("#div_QH_Today input:checkbox").each(function (i, n) {
            if ($(this).is(":checked")) {
                $("#" + $(this)[0].id.replace("check", "money")).val(parseInt($("#" + $(this)[0].id.replace("check", "times")).val()) * parseInt($("#lab_SumMoney").html()));
                needMoney += parseInt($("#" + $(this)[0].id.replace("check", "money")).val());
            }
        });

        $("#LbSumMoney").html(needMoney);
    },

    //追号列表中投注倍数改变
    ChaseListMultipleChange: function (obj) {
        //判断输入必须为数字
        if ((isNaN(obj.val()) == true) || (obj.val() <= 0)) {
            msg("倍数格式有误，已自动重置为 1");
            obj.val(1);
        }

        //判断范围
        if (Number(obj.val()) > Number($("#HidMaxTimes").val()) - 1) {
            msg("倍数超出范围，最大倍数为 " + String(Number($("#HidMaxTimes").val()) - 1) + "，已自动重置为 1");
            obj.val(1);
        }

        this.ChaseCalculateTotalMoney();
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

    //提交投注
    Submit: function () {
        //提交按钮可用状态
        this.ChangeSubmitButtonState(false);
        var IsuseID = $("#HidIsuseID").val();
        var IsuseEndTime = $("#HidIsuseEndTime").val();
        var PlayTypeID = this.PlayTypeID;
        var TotalShare = $("#tb_Share").val();
        var BuyShare = $("#tb_BuyShare").val();
        var SecrecyLevel = $("input[name='SecrecyLevel']:checked").val();
        var SchemeContent = "";
        var SumMoney = StrToFloat($("#lab_SumMoney").text());
        var SumNum = $("#lab_Num").text();

        var AssureMoney = $("#lab_AssureMoney").text();
        var LotteryID = this.LotID;
        var Multiple = $("#tb_Multiple").val();
        var SchemeBonusScale = $("#ddl_bonus_precent").val();
        var isChase = 0;
        var ChaseContent = "";
        var ChaseSumMoney = 0.00;

        var Cobuy = 0
        var OpenUserList = ""; //招募对象
        var Title = $("#tb_Title").val();
        var Description = $("#tb_Description").val();
        var AutoStopAtWinMoney = ($("#tbAutoStopAtWinMoney").is(":checked") ? 1 : 0);

        var isNullBuyContent = 0;

        var BuyMoney = 0;

        var SelectType = $("#txtSelectType").val(); //6636投注方式(1文件上传,2自选投注)
        var AddAttribute = 1;
        if ($("#cbAdditional") && $("#cbAdditional").is(":checked"))
            AddAttribute = 0;
        if ($("#add_price") && $("#add_price").is(":checked"))
            AddAttribute = 0;

        //投注的限制条件判断

        if (!$("#chkAgrrement").is(":checked")) {
            this.ChangeSubmitButtonState(true);
            msg("请先阅读用户投注协议，谢谢！");
            return false;
        }
        if ($("#toCurrIsuseEndTime").html() == "") {
            this.ChangeSubmitButtonState(true);
            msg("没有期号，无法投注。");
            return false;
        }

        if ($("#toCurrIsuseEndTime").html() == "本期已截止投注") {
            this.ChangeSubmitButtonState(true);
            msg("本期投注已截止，谢谢。");
            return false;
        }

        if (CodeArea.CodeList.length < 1) {
            this.ChangeSubmitButtonState(true);
            msg("请先将号码添加到号码篮");
            return;
        }
        if (this.PlayType < 1 || this.LotType < 1) {
            this.ChangeSubmitButtonState(true);
            msg("您的选择条件当前不足以发起追号,请重试");
            return;
        }
        if (this.TotalMoney < 2 || this.TotalMultiple < 1) {
            this.ChangeSubmitButtonState(true);
            msg("请选择正确的投注号码或者倍数");
            return;
        }
        if (parseFloat(IsuseID) < 1) {
            this.ChangeSubmitButtonState(true);
            msg("暂未销售，请稍后再进行购买");
            return;
        }

        for (var i = 0; i < CodeArea.CodeList.length; i++) {
            SchemeContent += "\n" + CodeArea.CodeList[i].Code;
        }
        SchemeContent = SchemeContent.substring(1);

        if (SumNum < 1) {
            this.ChangeSubmitButtonState(true);
            msg("请输入投注内容。");
            return false;
        }
        if (Multiple < 1) {
            $("#tb_Multiple").focus();
            this.ChangeSubmitButtonState(true);
            msg("请输入正确的倍数。");
            return false;
        }
        if (TotalShare < 1) {
            $("#tb_Share").focus();
            this.ChangeSubmitButtonState(true);
            msg("请输入正确的份数。");
            return false;
        }
        if (StrToFloat($("#lab_ShareMoney").text()) < 1) {
            $("#tb_Share").focus();
            this.ChangeSubmitButtonState(true);
            msg("每份金额不能小于 1 元。");
            return false;
        }
        var investType = 1; //投注方式 1代购 2 追号 3合买（入伙）
        //根据购买方式拼凑数据
        if ($("#Chase").is(":checked")) {//追号
            if (StrToInt($("#LbSumMoney").text()) > 0) {

                if (StrToInt($("#tbAutoStopAtWinMoney").val()) < 0) {
                    this.ChangeSubmitButtonState(true);
                    msg("追号截止金额错误!");
                    return;
                }

                var TipStr = '您要申请' + this.LotName + '投注，详细内容：\n\n';

                TipStr += "　　总金额：　" + $("#LbSumMoney").text() + " 元\n\n";
                var okfunc = function () { };
                var cancelfunc = function () { this.ChangeSubmitButtonState(true); };
                confirm(TipStr + "按“确定”即表示您已阅读《用户投注协议》并立即提交代购方案，确定要提交投注方案吗？");
                return;
            }
            else {
                this.ChangeSubmitButtonState(true);
                msg("请输入投注内容或勾选所要追的期号！");
                return false;
            }
            isChase = 1;
            SumMoney = parseInt(SumMoney, 10) / parseInt(Multiple, 10);
            ChaseSumMoney = parseInt($("#LbSumMoney").text(), 10)
            ChaseContent = "";
            BuyMoney = ChaseSumMoney;
            $("#div_QH_Today input:checkbox").each(function (i, n) {
                if ($(this).is(":checked")) {
                    ChaseContent += $(this).val() + "," + $("#times" + $(this).val()).val() + "," + $("#money" + $(this).val()).val() + ";";
                }
            });
            ChaseContent = ChaseContent.substring(0, ChaseContent.length - 1);
            investType = 2;
        } else {//代购、合买

            if ($("#upload_after").is(":checked") || this.PlayTypeID == 201 || this.PlayTypeID == 1501 || this.PlayTypeID == 7401 || this.PlayTypeID == 7501) {
                isNullBuyContent = 1;
            }
            //初始化四个值

            var HiOpt_InitiateSchemeMinBuyAndAssureScale = StrToFloat($("#HiOpt_InitiateSchemeMinBuyAndAssureScale").val());

            var Opt_InitiateSchemeLimitScale = 0;
            if (HiOpt_InitiateSchemeMinBuyAndAssureScale != "") {
                Opt_InitiateSchemeLimitScale = HiOpt_InitiateSchemeMinBuyAndAssureScale;
            }

            if (Opt_InitiateSchemeLimitScale <= 0) {
                Opt_InitiateSchemeLimitScale = 0.1;
            }

            if ((BuyShare) < Math.round(TotalShare * Opt_InitiateSchemeLimitScale)) {

                msg("发起人最少认购 " + (Opt_InitiateSchemeLimitScale * 100) + "%。(" + Math.round(TotalShare * Opt_InitiateSchemeLimitScale) + ' 份， ' + (Math.round(TotalShare * Opt_InitiateSchemeLimitScale) * StrToFloat($("#lab_ShareMoney").text())) + ' 元)');

                $("#tb_BuyShare").focus();
                this.ChangeSubmitButtonState(true);
                return false;
            }

            if ((parseInt(BuyShare, 10) + parseInt($("#tb_AssureShare").val(), 10)) > TotalShare) {
                $("#tb_AssureShare").focus();
                this.ChangeSubmitButtonState(true);
                msg("保底和购买的份数大于总份数。");
                return false;
            }

            if ((SumMoney < this.Price) || (SumMoney > 1000000)) {
                this.ChangeSubmitButtonState(true);
                msg("单个方案的总金额必须在" + this.Price + "元至 1000000 元之间。");
                return false;
            }

            var TipStr = '您要发起' + this.LotName + '方案，详细内容：\n\n';
            TipStr += "　　注　数：　" + SumNum + "\n";
            TipStr += "　　倍　数：　" + Multiple + "\n";
            TipStr += "　　总金额：　" + SumMoney.toFixed(2) + " 元\n\n";
            TipStr += "　　总份数：　" + TotalShare + " 份\n";
            TipStr += "　　每　份：　" + $("#lab_ShareMoney").text() + " 元\n\n";
            TipStr += "　　保　底：　" + $("#tb_AssureShare").val() + " 份，" + $("#lab_AssureMoney").text() + " 元\n";
            TipStr += "　　购　买：　" + BuyShare + " 份，" + $("#lab_BuyMoney").text() + " 元\n\n";

            var okfunc = function () {
                Cobuy = $("#CoBuy").is(":checked") ? 2 : 1;
                AutoStopAtWinMoney = $("#tbAutoStopAtWinMoney").val();
                BuyMoney = parseInt(BuyShare, 10) * parseFloat($("#lab_ShareMoney").text(), 10) + parseFloat($("#lab_AssureMoney").text(), 10);
                investType = 1;
            };
            var cancelfunc = function () { this.ChangeSubmitButtonState(true); };
            confirm(TipStr + "按“确定”即表示您已阅读《用户投注协议》并立即提交方案，确定要提交方案吗？", okfunc, cancelfunc);
            return;
        }

        var post_data = {
            IsuseID: IsuseID,
            IsuseEndTime: IsuseEndTime,
            PlayTypeID: PlayTypeID,
            TotalShare: TotalShare,
            BuyShare: BuyShare,
            SecrecyLevel: SecrecyLevel,
            SchemeContent: SchemeContent,
            SumMoney: SumMoney,
            SumNum: SumNum,
            AssureMoney: AssureMoney,
            LotteryID: LotteryID,
            Multiple: Multiple,
            SchemeBonusScale: SchemeBonusScale,
            isChase: isChase,
            ChaseContent: ChaseContent,
            ChaseSumMoney: ChaseSumMoney,
            Cobuy: Cobuy,
            OpenUserList: OpenUserList,
            Title: Title,
            Description: Description,
            AutoStopAtWinMoney: AutoStopAtWinMoney,
            isNullBuyContent: isNullBuyContent,
            SelectType: SelectType,
            AddAttribute: AddAttribute
        };

        //ajax提交投注内容
        $.ajax({
            type: "post",
            url: "/Ajax/Buy.ashx",
            data: post_data,
            cache: false,
            async: false,
            dataType: "json",
            success: function (result) {

                if (parseInt(result.error, 10) > 0) {
                    location.href = "/Home/Room/UserBuySuccess.aspx?LotteryID=" + Lottery.LotID + "&Type=" + investType + "&Money=" + BuyMoney + "&SchemeID=" + result.error;

                    return;
                } if (result.error == "-100") {
                    alert(result.msg)
                    location.href = "/Default.aspx";

                    return;
                }
                else {
                    if (result.error == "-107") {//余额不足
                        alert(result.msg)
                        location.href = "/Home/Room/OnlinePay/Default.aspx?BuyID=" + result.buyID;

                        return;
                    }
                    else if (result.msg == "请重新登录！") {
                        alert(result.msg);

                        $.ajax({
                            type: "POST",
                            url: "/ajax/UserLogin.ashx",
                            data: "action=loginout",
                            timeout: 20000,
                            cache: false,
                            async: false,
                            dataType: "json",
                            complete: function () { location.href = "../UserLogin.aspx"; }
                        });

                        return;
                    }
                    msg(result.msg);
                }
            },

            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("error");
            },

            complete: function (XMLHttpRequest, SuccessOrErrorthrown) {
                Lottery.ChangeSubmitButtonState(true);
            }
        });
    },


    //幸运选号 提交投注
    LuckNumberSubmit: function () {
        this.ClearInvestList();
        ValidateInvestContent($("#HidLuckNumber").val().replace(/\+/g, "#"));
        Lottery.Submit();
    },

    //投注按钮的状态设置
    ChangeSubmitButtonState: function (isCanUsed) {
        if (isCanUsed) {
            $("#submitting").hide();
            $("#btn_OK").show();
        } else {
            $("#submitting").show();
            $("#btn_OK").hide();
        }
    },

    //清空投注列表
    ClearInvestList: function () {
        CodeArea.CodeList = [];
        CodeArea.ShowCodeArea();
        $("#tb_Share").val("1");
        $("#lab_BuyMoney").val("0.00");
        $("#tb_BuyShare").val("1");
        $("#lab_ShareMoney").val("0.00");
    },

    //智能机选（定胆杀号） n注
    IntelligentRandSelected: function (n) {

        var Red = Lottery.getSelectedRedBallText(" ");
        var Blue = Lottery.getSelectedBlueBallText(" ");
        var Reds = Red.split(" ");
        var Blues = Blue.split(" ");
        var RedLength = Reds.length;
        var BlueLength = Blues.length;
        if (Red == "") {
            RedLength = 0;
        }
        if (Blue == "") {
            BlueLength = 0;
        }
        for (var i = 1; i <= n; i++) {
            var Q = new Array();
            var Q1 = new Array();
            var str = "";

            if ($("#expected_number").is(":checked")) {
                if (RedLength <= (this.LotID == 5 ? 6 : 5)) {
                    for (var r = 0; r < RedLength; r++) {
                        Q.push(Reds[r]);
                        str = Red;
                    }

                    for (var j = 0; j < (this.LotID == 5 ? 6 : 5) - RedLength; j++) {
                        var BallNum = this.GetRandomNumber((this.LotID == 5 ? 33 : 35), 1);

                        while (str.indexOf(BallNum) > -1 || BallNum < 1 || BallNum == undefined) {
                            BallNum = this.GetRandomNumber((this.LotID == 5 ? 33 : 35), 1);
                        }

                        str += " " + BallNum;
                        Q.push(BallNum);
                    }
                }
                else {
                    for (var a = 0; a < (this.LotID == 5 ? 6 : 5) ; a++) {
                        var BallNum = Reds[this.GetRandomNumber(RedLength, 1) - 1];

                        while (str.indexOf(BallNum) > -1 || BallNum < 1 || BallNum == undefined) {
                            BallNum = Reds[this.GetRandomNumber(RedLength, 1) - 1];
                        }

                        str += " " + BallNum;
                        Q.push(BallNum);
                    }
                }

                str = "";
                if (BlueLength <= (this.LotID == 5 ? 1 : 2)) {
                    for (var r = 0; r < BlueLength; r++) {
                        Q1.push(Blues[r]);
                        str = Blue;
                    }

                    for (var j = 0; j < (this.LotID == 5 ? 1 : 2) - BlueLength; j++) {
                        var BallNum = this.GetRandomNumber((this.LotID == 5 ? 16 : 12), 1);

                        while (str.indexOf(BallNum) > -1 || BallNum < 1 || BallNum == undefined) {
                            BallNum = this.GetRandomNumber((this.LotID == 5 ? 16 : 12), 1);
                        }

                        str += " " + BallNum;
                        Q1.push(BallNum);
                    }
                }
                else {
                    for (var a = 0; a < (this.LotID == 5 ? 1 : 2) ; a++) {
                        var BallNum = Blues[this.GetRandomNumber(RedLength, 1) - 1];

                        while (str.indexOf(BallNum) > -1 || BallNum < 1 || BallNum == undefined) {
                            BallNum = Blues[this.GetRandomNumber(RedLength, 1) - 1];
                        }

                        str += " " + BallNum;
                        Q1.push(BallNum);
                    }
                }
            } else if ($("#killed_number").is(":checked")) {

                for (var j = 0; j < (this.LotID == 5 ? 6 : 5) ; j++) {
                    var BallNum = this.GetRandomNumber((this.LotID == 5 ? 33 : 35), 1);

                    while (str.indexOf(BallNum) > -1 || Red.indexOf(this.PadLeft(BallNum.toString(), "0", 2)) > -1 || BallNum < 1 || BallNum == undefined) {
                        BallNum = this.GetRandomNumber((this.LotID == 5 ? 33 : 35), 1);
                    }

                    str += " " + BallNum;
                    Q.push(BallNum);
                }

                str = "";
                for (var j = 0; j < (this.LotID == 5 ? 1 : 2) ; j++) {
                    var BallNum = this.GetRandomNumber((this.LotID == 5 ? 16 : 12), 1);
                    while (str.indexOf(BallNum) > -1 || Blue.indexOf(this.PadLeft(BallNum.toString(), "0", 2)) > -1 || BallNum < 1 || BallNum == undefined) {
                        BallNum = this.GetRandomNumber((this.LotID == 5 ? 16 : 12), 1);
                    }

                    str += " " + BallNum;
                    Q1.push(BallNum);
                }
            }
            else {
                if (RedLength > 30 || BlueLength > 10) {
                    msg("没有满足的条件，请重新选择！");
                    return;
                }
            }

            //}
            Q.sort(function f(a, b) { return Number(a) - Number(b) });
            Q1.sort(function f(a, b) { return Number(a) - Number(b) })
            str = "";

            for (var k = 0; k < Q.length; k++) {
                str += " " + String(this.PadLeft(Q[k].toString(), "0", 2));
            }

            str += " +";
            for (var a = 0; a < Q1.length; a++) {
                str += " " + String(this.PadLeft(Q1[a].toString(), "0", 2));
            }

            str = str.trim();

            //加到投注列表
            this.IntelligentAddToInvestList(str);
        }
    },

    //字符，左填充 str表示原字符串，fillstr表示填充字符，n 表示目标长度
    PadLeft: function (str, fillstr, n) {
        if (n < 1)
            n = 1;
        str = str.toString();
        if (str.length < n) {
            for (var i = 0; i < n - str.length; i++) {
                str = fillstr + str;
            }
        }
        return str;
    },

    //跟新总份数计算最少购买份数
    CalculateCopies: function () {

        var SumShare = StrToInt($("#lab_SumMoney").html());
        var investType = $("input:radio[name='buy']:checked").val();
        if (investType == "1" || investType == "3") {
            //代购、追号
            SumShare = 1;
        }
        $("#tb_Share").val(SumShare == 0 ? 1 : SumShare);
        var HiOpt_InitiateSchemeMinBuyAndAssureScale = StrToFloat($("#HiOpt_InitiateSchemeMinBuyAndAssureScale").val());
        var Share = Math.round(SumShare * HiOpt_InitiateSchemeMinBuyAndAssureScale);
        Share = Share == 0 ? 1 : Share;
        $("#tb_BuyShare").val(Share);

        Lottery.CalcSumMoney();
    },
    //智能机选 添加到投注列表
    IntelligentAddToInvestList: function (investNumber) {

        CodeArea.PlayName = "单式";
        CodeArea.Code = investNumber.replace("+", "#");
        CodeArea.InvestNum = 1;
        CodeArea.ShowContent = "<font color=\"red\">" + investNumber.split("+")[0] + "</font> \+ <font color=\"blue\">" + investNumber.split("+")[1] + "</font>";
        CodeArea.AddTextToCodeArea();
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

    //方案上传
    AddTextToCodeAreaOfUpload: function () {
        var play_names = this.PlayName.toString().split(';');
        var codes = this.Code.toString().split(';');
        var invest_nums = this.InvestNum.toString().split(';');
        var show_contents = this.ShowContent.toString().split(';');
        if (play_names.length < 1) {
            return;
        }
        for (var i = 0; i < play_names.length; i++) {
            this.CodeList.push({ Code: codes[i], InvestNum: parseInt(invest_nums[i], 10), ShowContent: show_contents[i], PlayName: play_names[i] });
        }
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


    //快乐十分选三前直、选二连直调用
    AddTextToCodeArea1: function () {
        this.ShowCodeArea1();
    },

    ShowCodeArea1: function () {
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
        //var SumShare = StrToInt($("#lab_SumMoney").html());
        Lottery.CalculateCopies();
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

//号码-注数 键值对
var GetInvestmentCount = {
    getInvestment: function (playType_id, number) { //playType_id 玩法id、number 所选的号码
        if (isNaN(number))
            number = parseInt(number, 10);
        switch (playType_id) {
            case 6305:
                switch (number) {
                    case 0: return 1;
                    case 1: return 3;
                    case 2: return 6;
                    case 3: return 10;
                    case 4: return 15;
                    case 5: return 21;
                    case 6: return 28;
                    case 7: return 36;
                    case 8: return 45;
                    case 9: return 55;
                    case 10: return 63;
                    case 11: return 69;
                    case 12: return 73;
                    case 13: return 75;
                    case 14: return 75;
                    case 15: return 73;
                    case 16: return 69;
                    case 17: return 63;
                    case 18: return 55;
                    case 19: return 45;
                    case 20: return 36;
                    case 21: return 28;
                    case 22: return 21;
                    case 23: return 15;
                    case 24: return 10;
                    case 25: return 6;
                    case 26: return 3;
                    case 27: return 1;
                    default: return 0;
                }
                break;
            case 6306:
                switch (number) {
                    case 1: return 1;
                    case 2: return 2;
                    case 3: return 2;
                    case 4: return 4;
                    case 5: return 5;
                    case 6: return 6;
                    case 7: return 8;
                    case 8: return 10;
                    case 9: return 11;
                    case 10: return 13;
                    case 11: return 14;
                    case 12: return 14;
                    case 13: return 15;
                    case 14: return 15;
                    case 15: return 14;
                    case 16: return 14;
                    case 17: return 13;
                    case 18: return 11;
                    case 19: return 10;
                    case 20: return 8;
                    case 21: return 6;
                    case 22: return 5;
                    case 23: return 4;
                    case 24: return 2;
                    case 25: return 2;
                    case 26: return 1;
                    default: return 0;
                }
                break;
            case 605:
                switch (number) {
                    case 0: return 1;
                    case 1: return 3;
                    case 2: return 6;
                    case 3: return 10;
                    case 4: return 15;
                    case 5: return 21;
                    case 6: return 28;
                    case 7: return 36;
                    case 8: return 45;
                    case 9: return 55;
                    case 10: return 63;
                    case 11: return 69;
                    case 12: return 73;
                    case 13: return 75;
                    case 14: return 75;
                    case 15: return 73;
                    case 16: return 69;
                    case 17: return 63;
                    case 18: return 55;
                    case 19: return 45;
                    case 20: return 36;
                    case 21: return 28;
                    case 22: return 21;
                    case 23: return 15;
                    case 24: return 10;
                    case 25: return 6;
                    case 26: return 3;
                    case 27: return 1;
                    default: return 0;
                }
                break;
            case 606:
                switch (number) {
                    case 1: return 1;
                    case 2: return 2;
                    case 3: return 2;
                    case 4: return 4;
                    case 5: return 5;
                    case 6: return 6;
                    case 7: return 8;
                    case 8: return 10;
                    case 9: return 11;
                    case 10: return 13;
                    case 11: return 14;
                    case 12: return 14;
                    case 13: return 15;
                    case 14: return 15;
                    case 15: return 14;
                    case 16: return 14;
                    case 17: return 13;
                    case 18: return 11;
                    case 19: return 10;
                    case 20: return 8;
                    case 21: return 6;
                    case 22: return 5;
                    case 23: return 4;
                    case 24: return 2;
                    case 25: return 2;
                    case 26: return 1;
                    default: return 0;
                }
                break;
            case 2906:
                switch (number) {
                    case 0: return 1;
                    case 1: return 3;
                    case 2: return 6;
                    case 3: return 10;
                    case 4: return 15;
                    case 5: return 21;
                    case 6: return 28;
                    case 7: return 36;
                    case 8: return 45;
                    case 9: return 55;
                    case 10: return 63;
                    case 11: return 69;
                    case 12: return 73;
                    case 13: return 75;
                    case 14: return 75;
                    case 15: return 73;
                    case 16: return 69;
                    case 17: return 63;
                    case 18: return 55;
                    case 19: return 45;
                    case 20: return 36;
                    case 21: return 28;
                    case 22: return 21;
                    case 23: return 15;
                    case 24: return 10;
                    case 25: return 6;
                    case 26: return 3;
                    case 27: return 1;
                    default: return 0;
                }
                break;
            case 2907:
                switch (number) {
                    case 1: return 1;
                    case 2: return 2;
                    case 3: return 2;
                    case 4: return 4;
                    case 5: return 5;
                    case 6: return 6;
                    case 7: return 8;
                    case 8: return 10;
                    case 9: return 11;
                    case 10: return 13;
                    case 11: return 14;
                    case 12: return 14;
                    case 13: return 15;
                    case 14: return 15;
                    case 15: return 14;
                    case 16: return 14;
                    case 17: return 13;
                    case 18: return 11;
                    case 19: return 10;
                    case 20: return 8;
                    case 21: return 6;
                    case 22: return 5;
                    case 23: return 4;
                    case 24: return 2;
                    case 25: return 2;
                    case 26: return 1;
                    default: return 0;
                }
                break;
            case 2808:
                switch (number) {
                    case 0: return 1;
                    case 1: return 1;
                    case 2: return 2;
                    case 3: return 2;
                    case 4: return 3;
                    case 5: return 3;
                    case 6: return 4;
                    case 7: return 4;
                    case 8: return 5;
                    case 9: return 5;
                    case 10: return 5;
                    case 11: return 4;
                    case 12: return 4;
                    case 13: return 3;
                    case 14: return 3;
                    case 15: return 2;
                    case 16: return 2;
                    case 17: return 1;
                    case 18: return 1;
                    default: return 0;
                }
                break;
            case 2810:
                switch (number) {
                    case 0: return 1;
                    case 1: return 3;
                    case 2: return 6;
                    case 3: return 10;
                    case 4: return 15;
                    case 5: return 21;
                    case 6: return 28;
                    case 7: return 36;
                    case 8: return 45;
                    case 9: return 55;
                    case 10: return 63;
                    case 11: return 69;
                    case 12: return 73;
                    case 13: return 75;
                    case 14: return 75;
                    case 15: return 73;
                    case 16: return 69;
                    case 17: return 63;
                    case 18: return 55;
                    case 19: return 45;
                    case 20: return 36;
                    case 21: return 28;
                    case 22: return 21;
                    case 23: return 15;
                    case 24: return 10;
                    case 25: return 6;
                    case 26: return 3;
                    case 27: return 1;
                    default: return 0;
                }
                break;
            case 2815:
                switch (number) {
                    case 0: return 1;
                    case 1: return 1;
                    case 2: return 2;
                    case 3: return 3;
                    case 4: return 4;
                    case 5: return 5;
                    case 6: return 7;
                    case 7: return 8;
                    case 8: return 10;
                    case 9: return 12;
                    case 10: return 13;
                    case 11: return 14;
                    case 12: return 15;
                    case 13: return 15;
                    case 14: return 15;
                    case 15: return 15;
                    case 16: return 14;
                    case 17: return 13;
                    case 18: return 12;
                    case 19: return 10;
                    case 20: return 8;
                    case 21: return 7;
                    case 22: return 5;
                    case 23: return 4;
                    case 24: return 3;
                    case 25: return 2;
                    case 26: return 1;
                    case 27: return 1;
                    default: return 0;
                }
                break;
        }
    }
};


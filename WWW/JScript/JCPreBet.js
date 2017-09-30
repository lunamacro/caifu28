
/// <reference path="jquery-1.8.3.min.js" />


var PreBet = {
    isCheck: true, //是否选中过关方式
    isDan: false, //是否设胆（奖金优化不支持胆码设置）
    PreBetType: 1, //优化方式（默认平均优化）

    /*--------判断是否选中过关方式 Start--------*/
    IsChecked: function () {
        PreBet.isCheck = false;
        if ($("#gTab_Content0 label").length < 1) {
            PreBet.isCheck = true;
        }
        else {
            $("#gTab_Content0 label").each(function () {
                if (!$(this).attr("style")) {
                    if ($(this).find("input").attr("checked") == "checked") {
                        PreBet.isCheck = true;
                        return;
                    }
                }
            });
        }
        if (parseInt($("#showCount").html()) > 1000) {
            alert("请选择少于1000注");
            return;
        }
        PreBet.IsSetDan();
        PreBet.OpenPreBet();
    },
    /*--------判断是否选中过关方式 End--------*/

    /*--------判断是否设胆 Start--------*/
    IsSetDan: function () {
        PreBet.isDan = false;
        $("#selectTeams tr").each(function () {
            if (!$(this).attr("style")) {
                if ($(this).children().eq(3).find("input").attr("checked") == "checked") {
                    PreBet.isDan = true;
                    return;
                }
            }
        });
    },
    /*--------判断是否设胆 End--------*/

    /*--------奖金优化页面跳转 Start--------*/
    OpenPreBet: function () {
        var MatchId = 0; //获取对阵ID
        var Spf = ""; //选择的胜平负
        var StrHtml = ""; //需要传的对阵参数
        var StrOn = ""; //过关方式
        var Selectzd = "";
        /*
        2015-07-15修改
        */
        if ("active" == $("#gTab li:last").attr("class")) {
            alert("暂不支持多串过关优化");
            return;
        }
        if (PreBet.isCheck == false || parseInt($("#showMoney").text().replace("￥", "")) == 0) {
            alert("请选择过关方式");
            return;
        }
        if (PreBet.isDan == true) {
            alert("奖金优化暂不支持胆码设置");
            return;
        }
        var playid = $("#playid").val();
        switch (parseInt(playid)) {
            case 7201: //让球胜平负 
                $("#selectTeams tr").each(function () {
                    if (!$(this).attr("style") && !$(this).children().attr("style")) {
                        MatchId = $(this).attr("id");
                        MatchId = MatchId.replace('sel', '');
                        $(this).find("label").each(function () {
                            if ($(this).is(":hidden") == false) {
                                var val = $(this).find("input").val();
                                var select = "";
                                if (val == "3") {
                                    val = 1;
                                    select = "Win";
                                }
                                else if (val == "1") {
                                    val = 2;
                                    select = "Flat";
                                }
                                else if (val == "0") {
                                    val = 3;
                                    select = "Lose";
                                }
                                Spf += val + ",";
                                Selectzd += select + ",";
                            }
                        });
                        Spf = Spf.substr(Spf, Spf.length - 1);
                        Selectzd = Selectzd.substr(Selectzd, Selectzd.length - 1);
                        StrHtml += MatchId + ":" + Spf + ":" + Selectzd + "|";
                        Spf = "";
                        Selectzd = "";
                    }
                });
                StrOn = PreBet.GetPassWay();
                break;
            case 7207: //胜平负
                $("#selectTeams tr").each(function () {
                    if (!$(this).attr("style") && !$(this).children().attr("style")) {
                        MatchId = $(this).attr("id");
                        MatchId = MatchId.replace('sel', '');
                        $(this).find("label").each(function () {
                            if ($(this).is(":hidden") == false) {
                                var val = $(this).find("input").val();
                                var select = "";
                                if (val == "3") {
                                    val = 1;
                                    select = "SPFWin";
                                }
                                else if (val == "1") {
                                    val = 2;
                                    select = "SPFFlat";
                                }
                                else if (val == "0") {
                                    val = 3;
                                    select = "SPFLose";
                                }
                                Spf += val + ",";
                                Selectzd += select + ",";
                            }
                        });
                        Spf = Spf.substr(Spf, Spf.length - 1);
                        Selectzd = Selectzd.substr(Selectzd, Selectzd.length - 1);
                        StrHtml += MatchId + ":" + Spf + ":" + Selectzd + "|";
                        Spf = "";
                        Selectzd = "";
                    }
                });
                StrOn = PreBet.GetPassWay();
                break;
            case 7202: //比分
                $("#selectTeams tr").each(function () {
                    if (!$(this).attr("style")) {
                        MatchId = $(this).attr("id");
                        MatchId = MatchId.replace('sel', '');
                        $(this).find("td:eq(2)").find("label").each(function () {
                            if ($(this).is(":hidden") == false) {
                                var val = $.trim($(this).find("div").text());
                                val = PreBet.StrReplace(val);
                                var select = PreBet.Strzd(val);
                                Spf += val + ",";
                                Selectzd += select + ",";
                            }
                        });
                        Spf = Spf.substr(Spf, Spf.length - 1);
                        Selectzd = Selectzd.substr(Selectzd, Selectzd.length - 1);
                        StrHtml += MatchId + ":" + Spf + ":" + Selectzd + "|";
                        Spf = "";
                        Selectzd = "";
                    }
                });
                StrOn = PreBet.GetPassWay();
                break;
            case 7203: //总进球
                $("#selectTeams tr").each(function () {
                    if (!$(this).attr("style")) {
                        MatchId = $(this).attr("id");
                        MatchId = MatchId.replace('sel', '');
                        $(this).find("td:eq(2)").find("label").each(function () {
                            if ($(this).is(":hidden") == false) {
                                var val = $.trim($(this).find("div").text());
                                val = parseInt(val) + 1;
                                var select = PreBet.Strzd(val);
                                Spf += val + ",";
                                Selectzd += select + ",";
                            }
                        });
                        Spf = Spf.substr(Spf, Spf.length - 1);
                        Selectzd = Selectzd.substr(Selectzd, Selectzd.length - 1);
                        StrHtml += MatchId + ":" + Spf + ":" + Selectzd + "|";
                        Spf = "";
                        Selectzd = "";
                    }
                });
                StrOn = PreBet.GetPassWay();
                break;
            case 7204: //半全场
                $("#selectTeams tr").each(function () {
                    if (!$(this).attr("style")) {
                        MatchId = $(this).attr("id");
                        MatchId = MatchId.replace('sel', '');
                        $(this).find("td:eq(2)").find("label").each(function () {
                            if ($(this).is(":hidden") == false) {
                                var val = $.trim($(this).find("div").text());
                                val = val.replace("-", "");
                                val = PreBet.StrReplace(val);
                                var select = PreBet.Strzd(val);
                                Spf += val + ",";
                                Selectzd += select + ",";
                            }
                        });
                        Spf = Spf.substr(Spf, Spf.length - 1);
                        Selectzd = Selectzd.substr(Selectzd, Selectzd.length - 1);
                        StrHtml += MatchId + ":" + Spf + ":" + Selectzd + "|";
                        Spf = "";
                        Selectzd = "";
                    }
                });
                StrOn = PreBet.GetPassWay();
                break;
            case 7301: //胜负
                $("#selectTeams tr").each(function () {
                    if (!$(this).attr("style") && !$(this).children().attr("style")) {
                        MatchId = $(this).attr("id");
                        MatchId = MatchId.replace('sel', '');
                        $(this).find("label").each(function () {
                            if ($(this).is(":hidden") == false) {
                                var val = $(this).find("input").val();
                                var select = "";
                                if (val == "1") {
                                    val = 1;
                                    select = "MainLose";
                                }
                                else if (val == "2") {
                                    val = 2;
                                    select = "MainWin";
                                }
                                Spf += val + ",";
                                Selectzd += select + ",";
                            }
                        });
                        Spf = Spf.substr(Spf, Spf.length - 1);
                        Selectzd = Selectzd.substr(Selectzd, Selectzd.length - 1);
                        StrHtml += MatchId + ":" + Spf + ":" + Selectzd + "|";
                        Spf = "";
                        Selectzd = "";
                    }
                });
                StrOn = PreBet.GetPassWay();
                break;
            case 7302: //让分胜负
                $("#selectTeams tr").each(function () {
                    if (!$(this).attr("style") && !$(this).children().attr("style")) {
                        MatchId = $(this).attr("id");
                        MatchId = MatchId.replace('sel', '');
                        $(this).find("label").each(function () {
                            if ($(this).is(":hidden") == false) {
                                var val = $(this).find("input").val();
                                var select = "";
                                if (val == "1") {
                                    val = 1;
                                    select = "Letmainlose";
                                }
                                else if (val == "2") {
                                    val = 2;
                                    select = "Letmainwin";
                                }
                                Spf += val + ",";
                                Selectzd += select + ",";
                            }
                        });
                        Spf = Spf.substr(Spf, Spf.length - 1);
                        Selectzd = Selectzd.substr(Selectzd, Selectzd.length - 1);
                        StrHtml += MatchId + ":" + Spf + ":" + Selectzd + "|";
                        Spf = "";
                        Selectzd = "";
                    }
                });
                StrOn = PreBet.GetPassWay(); /*过关方式*/
                break;
            case 7303:
                StrOn = PreBet.GetPassWay(); /*过关方式*/
                $("#selectTeams tr").each(function () {
                    if (!$(this).attr("style") && !$(this).children().attr("style")) {
                        MatchId = $(this).attr("id");
                        MatchId = MatchId.replace('sel', '');
                        $(this).find("label").each(function (i, d) {
                            if ($(this).is(":hidden") == false) {
                                var val = $(this).find("input").val();
                                var select = PreBet.Strzd(val);
                                Spf += val + ",";
                                Selectzd += select + ",";
                            }
                        });
                        Spf = Spf.substr(Spf, Spf.length - 1);
                        Selectzd = Selectzd.substr(Selectzd, Selectzd.length - 1);
                        StrHtml += MatchId + ":" + Spf + ":" + Selectzd + "|";
                        Spf = "";
                        Selectzd = "";
                    }
                });
                break;
            case 7304:
                $("#selectTeams tr").each(function () {
                    if (!$(this).attr("style") && !$(this).children().attr("style")) {
                        MatchId = $(this).attr("id");
                        MatchId = MatchId.replace('sel', '');
                        $(this).find("label").each(function () {
                            if ($(this).is(":hidden") == false) {
                                var val = $(this).find("input").val();
                                var select = "";
                                if (val == "1") {
                                    select = "small";
                                } else if (val == "2") {
                                    select = "big";
                                }
                                Spf += val + ",";
                                Selectzd += select + ",";
                            }
                        });
                        Spf = Spf.substr(Spf, Spf.length - 1);
                        Selectzd = Selectzd.substr(Selectzd, Selectzd.length - 1);
                        StrHtml += MatchId + ":" + Spf + ":" + Selectzd + "|";
                        Spf = "";
                        Selectzd = "";
                    }
                });
                StrOn = PreBet.GetPassWay(); /*过关方式*/
                break;
        }
        var money = $("#showMoney").text().replace("￥", "");
        //alert(StrHtml + " " + StrOn); return;
        window.open("/Home/Room/JCPreBet.aspx?MatchId=" + StrHtml + "&StrOn=" + StrOn + "&InvestNum=" + $("#showCount").text() + "&Multiple=" + $("#buybs").val() + "&PlayTypeID=" + $("#playid").val() + "&Money=" + money);
    },
    /*--------奖金优化页面跳转 End--------*/

    /*--------过关方式 Start--------*/
    GetPassWay: function () {
        var StrOn = ""; //过关方式
        if ($("#gTab_Content0 label").length < 1) {
            StrOn = "单关";
        }
        else {
            $("#gTab_Content0 label").each(function () {
                if (!$(this).attr("style")) {
                    if ($(this).find("input").attr("checked") == "checked") {
                        StrOn += $(this).find("input").val() + ",";
                    }
                }
            });
            StrOn = StrOn.substr(StrOn, StrOn.length - 1);
        }
        return StrOn;
    },
    /*--------过关方式 End--------*/

    /*--------页面加载显示金额 Start--------*/
    CalculationBody: function () {
        $("#yhTab tr").each(function () {
            var castMoney = 1;
            var investNum = $(this).find("td:eq(3)").text(); //注数

            $(this).find("em").each(function () {
                castMoney = parseFloat($(this).text()) * castMoney;
            });

            var str = castMoney * 2 * parseFloat(investNum);
            var sumMoney = str.toString();
            if (sumMoney.indexOf(".") > 0) {
                sumMoney = sumMoney.substring(0, sumMoney.indexOf(".") + 3);
            }
            $(this).find("input").val(sumMoney);
        });

        PreBet.InvestAdd();
    },
    /*--------页面加载显示金额 End--------*/

    /*--------初始化 Start--------*/
    PlayInit: function () {
        $("#yhTab tr").each(function () {
            $(this).find("td:eq(3)").text(1);
            PreBet.PlayMoney(this);
        });
    },
    /*--------初始化 Start--------*/

    /*--------计算预测金额 Start--------*/
    PlayMoney: function (obj) {
        var castMoney = 1;
        var PlanMoney = $("#tbMoney").val(); //计划购买金额
        var investNum = $(obj).find("td:eq(3)").text(); //注数

        $(obj).find("em").each(function () {
            castMoney = parseFloat($(this).text()) * castMoney;
        });

        var str = castMoney * 2 * parseFloat(investNum);
        var sumMoney = str.toString();
        if (sumMoney.indexOf(".") > 0) {
            sumMoney = sumMoney.substring(0, sumMoney.indexOf(".") + 3);
        }
        $(obj).find("input").val(sumMoney);
    },
    /*--------计算预测金额 End--------*/

    /*--------获取数值索引 Start--------*/
    GetIndex: function (arr, value) {
        var str = arr.toString();
        var index = str.indexOf(value);
        if (index >= 0) {
            //存在返回索引
            var reg1 = new RegExp("((^|,)" + value + "(,|$))", "gi");
            return str.replace(reg1, "$2@$3").replace(/[^,@]/g, "").indexOf("@");
        } else {
            return -1;
        }
    },
    /*--------获取数值索引 End--------*/

    /*--------平均优化 Start--------*/
    InvestAdd: function () {
        if (parseInt($("#tbMoney").val()) % 2 != 0) {
            alert("请输入偶数金额再进行优化");
            return;
        }
        if (parseInt($("#tbMoney").val()) < (parseInt($("#InvestNum").text()) * 2)) {
            alert("计划购买金额不能小于投注金额");
            return;
        }
        $(".hedbtn input").removeClass("curr");
        $(".hedbtn input:eq(0)").addClass("curr");

        PreBet.PlayInit();

        var InvestCount = 0;
        var InvestSumCount = 0; //已分配的总注数
        var sumTr = $("#yhTab tr").length; //注数行
        var sumMoney = parseInt($("#tbMoney").val()); //计划购买
        var sumCount = sumMoney / 2; //计划购买注数
        var castMoney = 0; //注数行每注奖金
        var castSumMoney = 0; //注数行每注累计奖金
        var castSumDown = 0; //注数行每注倒数奖金
        var castAvgCount = 0; //平均注数

        var forCount = 0; //循环次数
        $("#yhTab tr").each(function () { //计算每注总奖金
            castMoney = parseFloat($(this).find("td").last().text());
            castSumMoney += castMoney;
            forCount++;
        });
        $("#yhTab tr").each(function () { //计算每注倒数总奖金
            castMoney = parseFloat($(this).find("td").last().text());
            castSumDown += castSumMoney / castMoney;
            forCount++;
        });
        castAvgCount = (sumCount - sumTr) / castSumDown; //先给每行一注，减掉行数

        var indexArray = new Array();
        var valueArray = new Array();

        $("#yhTab tr").each(function () { //计算每注倒数总奖金
            castMoney = parseFloat($(this).find("td").last().text());
            InvestCount = castAvgCount * (castSumMoney / castMoney) + 1;
            InvestSumCount += Math.floor(InvestCount);
            $(this).find("td").eq(3).text(Math.floor(InvestCount));
            PreBet.PlayMoney(this);

            indexArray.push($(this).find("td").eq(3).text());
            valueArray.push(parseFloat($(this).find("td").eq(4).find("input").val()).toFixed(2));
            forCount++;
        });
        console.info("已分配注数：" + InvestSumCount + "，总注数：" + sumCount);

        if (InvestSumCount > sumCount) {
            var forLen = InvestSumCount - sumCount;
            for (var i = 0; i < forLen; i++) {
                var max = Math.max.apply(Math, valueArray).toFixed(2);
                var index = PreBet.GetIndex(valueArray, max);
                InvestCount = parseInt(indexArray[index]) - 1;

                if (InvestSumCount > sumCount) {
                    $("#yhTab tr").eq(index).find("td").eq(3).text(InvestCount);
                    PreBet.PlayMoney($("#yhTab tr").eq(index));
                    InvestSumCount--;

                    indexArray[index] = InvestCount;
                    valueArray[index] = parseFloat($("#yhTab tr").eq(index).find("td").eq(4).find("input").val()).toFixed(2);
                    forCount++;
                }
            }
            console.info("4:" + forCount);
        }

        if (InvestSumCount < sumCount) {
            var forLen = sumCount - InvestSumCount;
            for (var i = 0; i < forLen; i++) {
                var min = Math.min.apply(Math, valueArray).toFixed(2);
                var index = PreBet.GetIndex(valueArray, min);
                InvestCount = parseInt(indexArray[index]) + 1;

                if (index == -1) {
                    console.info("valueArray:" + valueArray.toString());
                    console.info("min:" + min);
                    console.info("index:" + index);
                    return;
                }

                if (InvestSumCount < sumCount) {
                    $("#yhTab tr").eq(index).find("td").eq(3).text(InvestCount);
                    PreBet.PlayMoney($("#yhTab tr").eq(index));
                    InvestSumCount++;

                    indexArray[index] = InvestCount;
                    valueArray[index] = parseFloat($("#yhTab tr").eq(index).find("td").eq(4).find("input").val()).toFixed(2);
                    forCount++;
                }
            }
            console.info("5:" + forCount);
        }

        PreBetType = 1; //平均优化

        $("#tbMoney").val(parseInt($("#tbMoney").val()));
        $("#SumMoney").text($("#tbMoney").val());
        PreBet.SumMultiple(); /*倍数计算*/
    },
    /*--------平均优化 End--------*/

    /*--------博热优化 Start--------*/
    InvestHot: function () {
        if ($(".hedbtn input:eq(1)").hasClass("wuxiao")) return;

        if (parseInt($("#tbMoney").val()) % 2 != 0) {
            alert("请输入偶数金额再进行优化");
            return;
        }
        if (parseInt($("#tbMoney").val()) < (parseInt($("#InvestNum").text()) * 2)) {
            alert("计划购买金额不能小于投注金额");
            return;
        }
        $(".hedbtn input").removeClass("curr");
        $(".hedbtn input:eq(1)").addClass("curr");

        PreBet.PlayInit();

        var InvestCount = 0;
        var InvestSumCount = 0; //已分配的总注数
        var sumTr = $("#yhTab tr").length; //注数行
        var sumMoney = parseInt($("#tbMoney").val()); //计划购买
        var sumCount = sumMoney / 2; //计划购买注数
        var castMoney = 0; //注数行每注奖金
        var castSumMoney = 0; //注数行每注累计奖金
        var castSumDown = 0; //注数行每注倒数奖金
        var castAvgCount = 0; //平均注数

        var forCount = 0; //循环次数
        $("#yhTab tr").each(function () { //计算每注总奖金
            castMoney = parseFloat($(this).find("td").last().text());
            castSumMoney += castMoney;
            forCount++;
        });
        $("#yhTab tr").each(function () { //计算每注倒数总奖金
            castMoney = parseFloat($(this).find("td").last().text());
            castSumDown += castSumMoney / castMoney;
            forCount++;
        });
        castAvgCount = (sumCount - sumTr) / castSumDown; //先给每行一注，减掉行数

        var indexArray = new Array();
        var valueArray = new Array();
        var valueArrayHot = new Array();

        $("#yhTab tr").each(function () { //计算每注倒数总奖金
            castMoney = parseFloat($(this).find("td").last().text());
            InvestCount = castAvgCount * (castSumMoney / castMoney) + 1;

            if (parseInt(sumMoney / castMoney / 2) < InvestCount && InvestCount > (parseInt(sumMoney / castMoney / 2) + 1)) {
                InvestCount = parseInt(sumMoney / castMoney / 2) + 1;
            }

            InvestSumCount += Math.floor(InvestCount);
            $(this).find("td").eq(3).text(Math.floor(InvestCount));
            PreBet.PlayMoney(this);

            indexArray.push($(this).find("td").eq(3).text());
            valueArray.push(parseFloat($(this).find("td").eq(4).find("input").val()).toFixed(2));
            valueArrayHot.push(castMoney.toFixed(2));
            forCount++;
        });
        console.info("已分配注数：" + InvestSumCount + "，总注数：" + sumCount);

        if (InvestSumCount > sumCount) {
            var forLen = InvestSumCount - sumCount;
            for (var i = 0; i < forLen; i++) {
                var max = Math.max.apply(Math, valueArray).toFixed(2);
                var index = PreBet.GetIndex(valueArray, max);
                InvestCount = parseInt(indexArray[index]) - 1;

                if (InvestSumCount > sumCount) {
                    $("#yhTab tr").eq(index).find("td").eq(3).text(InvestCount);
                    PreBet.PlayMoney($("#yhTab tr").eq(index));
                    InvestSumCount--;

                    indexArray[index] = InvestCount;
                    valueArray[index] = parseFloat($("#yhTab tr").eq(index).find("td").eq(4).find("input").val()).toFixed(2);
                    forCount++;
                }
            }
            console.info("4:" + forCount);
        }

        if (InvestSumCount < sumCount) {
            var forLen = sumCount - InvestSumCount;
            for (var i = 0; i < forLen; i++) {
                var min = Math.min.apply(Math, valueArray).toFixed(2);
                var index = PreBet.GetIndex(valueArray, min);
                InvestCount = parseInt(indexArray[index]) + 1;

                if (index == -1) {
                    console.info("valueArray:" + valueArray.toString());
                    console.info("min:" + min);
                    console.info("index:" + index);
                    return;
                }

                if (InvestSumCount < sumCount) {
                    if (parseFloat($("#yhTab tr").eq(index).find("td").eq(4).find("input").val()).toFixed(2) > sumMoney) {
                        break;
                    } else {
                        $("#yhTab tr").eq(index).find("td").eq(3).text(InvestCount);
                        PreBet.PlayMoney($("#yhTab tr").eq(index));
                        InvestSumCount++;

                        indexArray[index] = InvestCount;
                        valueArray[index] = parseFloat($("#yhTab tr").eq(index).find("td").eq(4).find("input").val()).toFixed(2);
                    }
                    forCount++;
                }
            }
            if (InvestSumCount < sumCount) {
                var min = Math.min.apply(Math, valueArrayHot).toFixed(2);
                var index = PreBet.GetIndex(valueArrayHot, min);

                if (index == -1) {
                    console.info("valueArray:" + valueArrayHot.toString());
                    console.info("min:" + min);
                    console.info("index:" + index);
                    return;
                }

                InvestCount = parseInt(indexArray[index]) + (sumCount - InvestSumCount);
                $("#yhTab tr").eq(index).find("td").eq(3).text(InvestCount);
                PreBet.PlayMoney($("#yhTab tr").eq(index));
            }
            console.info("5:" + forCount);
        }

        PreBetType = 2; //博热优化

        $("#tbMoney").val(parseInt($("#tbMoney").val()));
        $("#SumMoney").text($("#tbMoney").val());
        PreBet.SumMultiple(); /*倍数计算*/
    },
    /*--------博热优化 End--------*/

    /*--------博冷优化 Start--------*/
    InvestCold: function () {
        if ($(".hedbtn input:eq(2)").hasClass("wuxiao")) return;
        if (parseInt($("#tbMoney").val()) % 2 != 0) {
            alert("请输入偶数金额再进行优化");
            return;
        }
        if (parseInt($("#tbMoney").val()) < (parseInt($("#InvestNum").text()) * 2)) {
            alert("计划购买金额不能小于投注金额");
            return;
        }
        $(".hedbtn input").removeClass("curr");
        $(".hedbtn input:eq(2)").addClass("curr");

        PreBet.PlayInit();

        var InvestCount = 0;
        var InvestSumCount = 0; //已分配的总注数
        var sumTr = $("#yhTab tr").length; //注数行
        var sumMoney = parseInt($("#tbMoney").val()); //计划购买
        var sumCount = sumMoney / 2; //计划购买注数
        var castMoney = 0; //注数行每注奖金
        var castSumMoney = 0; //注数行每注累计奖金
        var castSumDown = 0; //注数行每注倒数奖金
        var castAvgCount = 0; //平均注数

        var forCount = 0; //循环次数
        $("#yhTab tr").each(function () { //计算每注总奖金
            castMoney = parseFloat($(this).find("td").last().text());
            castSumMoney += castMoney;
            forCount++;
        });
        $("#yhTab tr").each(function () { //计算每注倒数总奖金
            castMoney = parseFloat($(this).find("td").last().text());
            castSumDown += castSumMoney / castMoney;
            forCount++;
        });
        castAvgCount = (sumCount - sumTr) / castSumDown; //先给每行一注，减掉行数

        var indexArray = new Array();
        var valueArray = new Array();
        var valueArrayHot = new Array();

        $("#yhTab tr").each(function () { //计算每注倒数总奖金
            castMoney = parseFloat($(this).find("td").last().text());
            InvestCount = castAvgCount * (castSumMoney / castMoney) + 1;

            if (parseInt(sumMoney / castMoney / 2) < InvestCount && InvestCount > (parseInt(sumMoney / castMoney / 2) + 1)) {
                InvestCount = parseInt(sumMoney / castMoney / 2) + 1;
            }

            InvestSumCount += Math.floor(InvestCount);
            $(this).find("td").eq(3).text(Math.floor(InvestCount));
            PreBet.PlayMoney(this);

            indexArray.push($(this).find("td").eq(3).text());
            valueArray.push(parseFloat($(this).find("td").eq(4).find("input").val()).toFixed(2));
            valueArrayHot.push(castMoney.toFixed(2));
            forCount++;
        });
        console.info("已分配注数：" + InvestSumCount + "，总注数：" + sumCount);

        if (InvestSumCount > sumCount) {
            var forLen = InvestSumCount - sumCount;
            for (var i = 0; i < forLen; i++) {
                var max = Math.max.apply(Math, valueArray).toFixed(2);
                var index = PreBet.GetIndex(valueArray, max);
                InvestCount = parseInt(indexArray[index]) - 1;

                if (InvestSumCount > sumCount) {
                    $("#yhTab tr").eq(index).find("td").eq(3).text(InvestCount);
                    PreBet.PlayMoney($("#yhTab tr").eq(index));
                    InvestSumCount--;

                    indexArray[index] = InvestCount;
                    valueArray[index] = parseFloat($("#yhTab tr").eq(index).find("td").eq(4).find("input").val()).toFixed(2);
                    forCount++;
                }
            }
            console.info("4:" + forCount);
        }

        if (InvestSumCount < sumCount) {
            var forLen = sumCount - InvestSumCount;
            for (var i = 0; i < forLen; i++) {
                var min = Math.min.apply(Math, valueArray).toFixed(2);
                var index = PreBet.GetIndex(valueArray, min);
                InvestCount = parseInt(indexArray[index]) + 1;

                if (index == -1) {
                    console.info("valueArray:" + valueArray.toString());
                    console.info("min:" + min);
                    console.info("index:" + index);
                    return;
                }

                if (InvestSumCount < sumCount) {
                    if (parseFloat($("#yhTab tr").eq(index).find("td").eq(4).find("input").val()).toFixed(2) > sumMoney) {
                        break;
                    } else {
                        $("#yhTab tr").eq(index).find("td").eq(3).text(InvestCount);
                        PreBet.PlayMoney($("#yhTab tr").eq(index));
                        InvestSumCount++;

                        indexArray[index] = InvestCount;
                        valueArray[index] = parseFloat($("#yhTab tr").eq(index).find("td").eq(4).find("input").val()).toFixed(2);
                    }
                    forCount++;
                }
            }
            if (InvestSumCount < sumCount) {
                var max = Math.max.apply(Math, valueArrayHot).toFixed(2);
                var index = PreBet.GetIndex(valueArrayHot, max);

                if (index == -1) {
                    console.info("valueArray:" + valueArrayHot.toString());
                    console.info("min:" + max);
                    console.info("index:" + index);
                    return;
                }

                InvestCount = parseInt(indexArray[index]) + (sumCount - InvestSumCount);
                $("#yhTab tr").eq(index).find("td").eq(3).text(InvestCount);
                PreBet.PlayMoney($("#yhTab tr").eq(index));
            }
            console.info("5:" + forCount);
        }

        PreBetType = 3; //博冷优化

        $("#tbMoney").val(parseInt($("#tbMoney").val()));
        $("#SumMoney").text($("#tbMoney").val());
        PreBet.SumMultiple(); /*倍数计算*/
    },
    /*--------博冷优化 End--------*/

    /*--------手动输入预测奖金 Start--------*/
    MoneyInputBlur: function (obj) {
        var castMoney = $("#" + obj + " td").find("input:text").val();
        var investNum = $("#" + obj + " td:eq(3)").text();
        var sumMoney = $("#SumMoney").text();
        if (parseFloat(castMoney) <= 0) {
            $("#" + obj + " td").find("input:text").val(0);
            $("#" + obj + " td:eq(3)").text(0);
            $("#SumMoney").text(parseInt(sumMoney) - (parseInt(investNum) * 2));
            PreBet.SumMultiple(); /*倍数计算*/
        }
    },
    /*--------手动输入预测奖金 End--------*/

    /*--------计划购买金额递增 Start--------*/
    PlanMoneyAdd: function () {
        var money = $("#tbMoney").val();
        if (parseInt(money) % 2 != 0) {
            money = parseInt(money) + 1;
        }
        else {
            money = parseInt(money) + 2;
        }
        if (money > 0) {
            $("#tbMoney").val(money);
        }
        var obj = $(".hedbtn").find(".curr").attr("id");
        if (obj == "btnAvg") {
            PreBet.InvestAdd();
        }
        if (obj == "btnHot") {
            PreBet.InvestHot();
        }
        if (obj == "btnCold") {
            PreBet.InvestCold();
        }
    },
    /*--------计划购买金额递增 End--------*/

    /*--------计划购买金额递减 Start--------*/
    PlayMoneyMinus: function () {
        var money = $("#tbMoney").val();
        if (parseInt(money) % 2 != 0) {
            money = parseInt(money) - 1;
        }
        else {
            money = parseInt(money) - 2;
        }
        if (money > 0) {
            $("#tbMoney").val(money);
        }
        var obj = $(".hedbtn").find(".curr").attr("id");
        if (obj == "btnAvg") {
            PreBet.InvestAdd();
        }
        if (obj == "btnHot") {
            PreBet.InvestHot();
        }
        if (obj == "btnCold") {
            PreBet.InvestCold();
        }
    },
    /*--------计划购买金额递减 End--------*/

    /*--------预测金额增加 Start--------*/
    CastMoneyAdd: function (obj) {
        var InvestNum = $("#" + obj + " td:eq(3)").text();
        var SumMoney = $("#SumMoney").text();
        var sum = 1;
        $("#" + obj + " td:eq(3)").text(parseInt(InvestNum) + 1);
        $("#SumMoney").text(parseInt(SumMoney) + 2);

        PreBet.SumMultiple(); /*倍数计算*/

        $("#" + obj + " td:eq(5) em").each(function () {
            sum = parseFloat($(this).text()) * sum;
        });
        sum = sum * 2;

        var inputnum = parseFloat(sum) * parseInt($("#" + obj + " td:eq(3)").text());
        inputnum = inputnum.toFixed(2);
        $("#" + obj + " input:text").val(inputnum);
    },
    /*--------预测金额增加 End--------*/

    /*--------预测金额递减 Start--------*/
    CastMoneyMinus: function (obj) {
        var InvestNum = $("#" + obj + " td:eq(3)").text();
        var SumMoney = $("#SumMoney").text();
        var sum = 1;
        var str_sum = "";
        $("#" + obj + " td:eq(3)").text(parseInt(InvestNum) - 1);
        if (InvestNum <= 1) {
            $("#" + obj + " td:eq(3)").text(1);
            return;
        }
        $("#SumMoney").text(parseInt(SumMoney) - 2);

        PreBet.SumMultiple(); /*倍数计算*/

        $("#" + obj + " td:eq(5) em").each(function () {
            sum = parseFloat($(this).text()) * sum;
        });
        sum = sum * 2;
        var str = sum.toString();
        if (str.indexOf(".") > 0) {
            str_sum = str.substring(0, str.indexOf(".") + 3);
        }
        else {
            str_sum = str;
        }

        var inputnum = parseFloat(str_sum) * parseInt($("#" + obj + " td:eq(3)").text());
        inputnum = inputnum.toString();
        if (inputnum.indexOf(".") > 0) {
            inputnum = inputnum.substring(0, inputnum.indexOf(".") + 3);
        }

        $("#" + obj + " input:text").val(inputnum);

    },
    /*--------预测金额递减 End--------*/

    /*--------预测金额手动 Start--------*/
    CastMoneyRevise: function () {
        var Revisemoney = parseFloat($("#txt_pl").val());
        $("#yhTab").find("tr").each(function (i, j) {
            var InvestNum = $(j).find("td:eq(3)").text();
            var sum = 1;
            var str_sum = "";

            $(j).find("td:eq(5) em").each(function () {
                sum = parseFloat($(this).text()) * sum;
            });

            sum = sum * 2;
            var num = parseInt(Revisemoney / sum);
            var small = num * sum;
            var big = (num + 1) * sum;
            if (num != 0) {
                if (Math.abs(small - Revisemoney) >= Math.abs(big - Revisemoney)) {
                    str_sum = big;
                    $(j).find("td:eq(3)").text(num + 1);
                }
                else {
                    str_sum = small;
                    $(j).find("td:eq(3)").text(num);
                }

                $(j).find("input:text").val(str_sum.toFixed(2));
            }
        });
        PreBet.SumMultiple(); /*倍数计算*/
        $("#pop-edit").hide();
    },
    /*--------预测金额手动 End--------*/

    InputBlur: function (obj) {
        var val = obj.value;
        if (val % 2 != 0) {
            alert("请输入偶数金额再进行优化");
            return;
        }
        var obj = $(".hedbtn").find(".curr").attr("id");
        if (obj == "btnAvg") {
            PreBet.InvestAdd();
        }
        if (obj == "btnHot") {
            PreBet.InvestHot();
        }
        if (obj == "btnCold") {
            PreBet.InvestCold();
        }
    },
    /*--------增加倍数 Start--------*/
    AddMultiple: function (m) {
        if (m == 0) {
            var multiple = parseInt($("#txtMultiple").val()) - 1;
            if (multiple < 1) {
                alert("倍数不能少于1");
                return;
            }
            $("#txtMultiple").val(multiple);
        } else if (m == 1) {
            var multiple = parseInt($("#txtMultiple").val()) + 1;
            $("#txtMultiple").val(multiple);
        }
        PreBet.SumMultiple();
    },
    /*--------增加倍数 End--------*/

    /*--------倍数计算 Start--------*/
    SumMultiple: function () {
        //计算购买金额
        var multiple = parseInt($("#txtMultiple").val());
        if (multiple > 99999) {
            multiple = 99999;
            $("#txtMultiple").val("99999");
        }
        var InvestCount = 0;
        $("#yhTab tr").each(function () {
            InvestCount += parseInt($(this).find("td").eq(3).text());
        });
        $("#SumMoney").text(InvestCount * 2 * multiple);
    },
    /*--------倍数计算 End--------*/

    /*--------立即投注 Start--------*/
    Submit: function () {

        if (!CheckIsLogin()) return;

        if ($("#agreeBuy").attr("checked") != "checked") {
            alert("请阅读《用户投注协议》并同意");
            return;
        }

        var SchemeCodes = $("#hidCode").val(); //投注号码
        var LotteryID = $("#hidLotteryID").val(); //彩种ID
        var PlayTypeID = $("#hidPlayTypeID").val(); //玩法ID
        var LotteryName = LotteryID == 72 ? "竞彩足球" : "竞彩篮球";
        var MatchID = $("#hidMatchID").val(); //对阵ID
        var CodeFormat = $("#hidFormat").val(); //号码格式
        var Multiple = $("#txtMultiple").val();

        var SchemeTitle = $("#schemeTitle").val();
        var SchemeContent = $("#schemedescription").val();
        var Share = $("#totalShare").val();
        var BuyShare = $("#buyshare").val();
        var HiOpt_InitiateSchemeMinBuyAndAssureScale = $("#HiOpt_InitiateSchemeMinBuyAndAssureScale").val();
        var minBuyShare = Math.ceil(HiOpt_InitiateSchemeMinBuyAndAssureScale * Share);
        if (minBuyShare > BuyShare) {
            alert("至少购买" + minBuyShare + "份。");
            return;
        }
        var AssureMoney = (parseInt($("#SumMoney").text()) / parseInt($("#totalShare").val()) * parseInt($("#assureshare").val())).toFixed(2);
        var SchemeBonusScale = parseInt($("#ul_cway_list li[checked='checked']").val()) / 100;
        var SecrecyLevel = $("#cbuy-list input:checked").val();
        var isHe = $("#isHe").val();

        SchemeCodes = SchemeCodes.substr(0, SchemeCodes.length - 1);
        var GGWay = "";
        var InvestNum = "";
        var PlayTeam = "";
        var CastMoney = "";
        $("#yhTab tr").each(function () {
            GGWay += $(this).find("td:eq(1)").text() + "-";
            InvestNum += $(this).find("td:eq(3)").text() + "-";
            $(this).find("td:eq(5) p").each(function () {
                PlayTeam += $(this).text() + "|";
            });
            PlayTeam = PlayTeam + "-";
            CastMoney += $(this).find("input:text").val() + "-";
        });
        PlayTeam = PlayTeam.substr(0, PlayTeam.length - 1);
        GGWay = GGWay.substr(0, GGWay.length - 1);
        InvestNum = InvestNum.substr(0, InvestNum.length - 1);
        CastMoney = CastMoney.substr(0, CastMoney.length - 1);

        var TipStr = "您要发起" + LotteryName + "方案，详细内容：\n\n";
        TipStr += "　　注　数：　" + parseInt($("#SumMoney").text()) / 2 / parseInt($("#txtMultiple").val()) + "\n";
        TipStr += "　　倍　数：　" + $("#txtMultiple").val() + "\n";
        TipStr += "　　总金额：　" + parseInt($("#SumMoney").text()).toFixed(2) + " 元\n\n";
        TipStr += "　　总份数：　" + parseInt($("#totalShare").val()) + "份\n";
        TipStr += "　　每　份：　" + (parseInt($("#SumMoney").text()) / parseInt($("#totalShare").val())).toFixed(2) + " 元\n\n";
        TipStr += "　　保　底：　" + $("#assureshare").val() + " 份，" + (parseInt($("#SumMoney").text()) / parseInt($("#totalShare").val()) * parseInt($("#assureshare").val())).toFixed(2) + " 元\n";
        TipStr += "　　购　买：　" + (parseInt($("#SumMoney").text()) / parseInt($("#totalShare").val()) * parseInt($("#buyshare").val())).toFixed(2) + " 元\n\n";
        var okfunc = function () {
            //订单防重复处理
            $("#divSubmit,#HeSubmit").hide();
            if ($("#dgBtnDoing").length <= 0) {
                if (parseInt(isHe) == 0) {
                    $("#divSubmit").after('<div id="dgBtnDoing" style="background:#666;float:left; width:100px;height:30px; line-height:30px; margin:0 auto 0 auto; font-size:14px; padding-top:5px; text-align:center;color:#fff; font-weight:bold; cursor:pointer;">正在购买中</div>');
                } else {
                    $("#HeSubmit").after('<div id="heBtnDoing" style="background:#666;float:left; width:100px;height:30px; line-height:30px; margin:0 auto 0 auto; font-size:14px; padding-top:5px; text-align:center;color:#fff; font-weight:bold; cursor:pointer;">正在购买中</div>');
                }
            }
            else {
                if (parseInt(isHe) == 0) {
                    $("#dgBtnDoing").show();
                } else {
                    $("#heBtnDoing").show();
                }
            }

            $.ajax({
                type: "POST",
                url: "/ajax/JCPreBet.ashx",
                data: "LotteryID=" + LotteryID + "&SchemeCodes=" + SchemeCodes + "&GGWay=" + GGWay + "&InvestNum=" + InvestNum + "&PlayTeam=" + PlayTeam + "&Multiple=" + Multiple + "&CastMoney=" + CastMoney + "&PlayTypeID=" + PlayTypeID + "&MatchID=" + MatchID + "&CodeFormat=" + CodeFormat + "&PreBetType=" + PreBetType + "&SchemeTitle=" + SchemeTitle + "&SchemeContent=" + SchemeContent + "&AssureMoney=" + AssureMoney + "&Share=" + Share + "&BuyShare=" + BuyShare + "&SecrecyLevel=" + SecrecyLevel + "&SchemeBonusScale=" + SchemeBonusScale,
                cache: false,
                async: false,
                dataType: "json",
                success: function (result) {
                    if (parseInt(result.error, 10) > 0) {
                        window.location.href = "/Home/Room/UserBuySuccess.aspx?LotteryID=" + LotteryID + "&Type=1&Money=" + $("#SumMoney").text() + "&SchemeID=" + result.error;
                        return;
                    }
                    if (result.error == "-111") {
                        location.href = "/UserLogin.aspx?RequestLoginPage=JCZC/Buy_SPF.aspx";
                        return;
                    }
                    if (result.error == "-107") {//余额不足
                        var okfunc_error = function () {
                            window.location.href = "/Home/Room/OnlinePay/Alipay02/Send_Default.aspx";
                        };
                        var cancelfunc_error = function () {
                            //订单防重复处理
                            $("#divSubmit,#HeSubmit").show();
                            $("#dgBtnDoing,#heBtnDoing").hide();
                        };
                        confirm("您的余额不足，是否立即充值？", okfunc_error, cancelfunc_error);
                        return;
                    }
                    //订单防重复处理
                    $("#divSubmit,#HeSubmit").show();
                    $("#dgBtnDoing,#heBtnDoing").hide();
                    alert(result.msg);
                },
                error: function (x, s, t) {
                    //订单防重复处理
                    $("#divSubmit,#HeSubmit").show();
                    $("#dgBtnDoing,#heBtnDoing").hide();
                    alert(t);
                }
            });
        };
        var cancelfunc = function () { return; };
        confirm(TipStr + "按“确定”即表示您已阅读《用户投注协议》并立即提交方案，确定要提交方案吗？", okfunc, cancelfunc);
    },
    /*--------立即投注 End--------*/

    StrReplace: function (obj) {
        var result = "1";
        switch (obj) {
            case "胜胜":
            case "1:0":
                result = "1";
                break;
            case "胜平":
            case "2:0":
                result = "2";
                break;
            case "胜负":
            case "2:1":
                result = "3";
                break;
            case "平胜":
            case "3:0":
                result = "4";
                break;
            case "平平":
            case "3:1":
                result = "5";
                break;
            case "平负":
            case "3:2":
                result = "6";
                break;
            case "负胜":
            case "4:0":
                result = "7";
                break;
            case "负平":
            case "4:1":
                result = "8";
                break;
            case "负负":
            case "4:2":
                result = "9";
                break;
            case "5:0": result = "10"; break;
            case "5:1": result = "11"; break;
            case "5:2": result = "12"; break;
            case "胜其他": result = "13"; break;
            case "0:0": result = "14"; break;
            case "1:1": result = "15"; break;
            case "2:2": result = "16"; break;
            case "3:3": result = "17"; break;
            case "平其他": result = "18"; break;
            case "0:1": result = "19"; break;
            case "0:2": result = "20"; break;
            case "1:2": result = "21"; break;
            case "0:3": result = "22"; break;
            case "1:3": result = "23"; break;
            case "2:3": result = "24"; break;
            case "0:4": result = "25"; break;
            case "1:4": result = "26"; break;
            case "2:4": result = "27"; break;
            case "0:5": result = "28"; break;
            case "1:5": result = "29"; break;
            case "2:5": result = "30"; break;
            case "负其他": result = "31"; break;
        }
        return result;
    },

    Strzd: function (obj) {
        var PlayTypeID = $("#playid").val();
        var result = "";
        if (PlayTypeID == "7203") {
            if (obj == "1")
                result = "In0";
            else if (obj == "2")
                result = "In1";
            else if (obj == "3")
                result = "In2";
            else if (obj == "4")
                result = "In3";
            else if (obj == "5")
                result = "In4";
            else if (obj == "6")
                result = "In5";
            else if (obj == "7")
                result = "In6";
            else if (obj == "8")
                result = "In7";
        }
        else if (PlayTypeID == "7204") {
            if (obj == "1")
                result = "SS";
            else if (obj == "2")
                result = "SP";
            else if (obj == "3")
                result = "SF";
            else if (obj == "4")
                result = "PS";
            else if (obj == "5")
                result = "PP";
            else if (obj == "6")
                result = "PF";
            else if (obj == "7")
                result = "FS";
            else if (obj == "8")
                result = "FP";
            else if (obj == "9")
                result = "FF";
        }
        else if (PlayTypeID == "7202") {
            if (obj == "1")
                result = "S10";
            else if (obj == "2")
                result = "S20";
            else if (obj == "3")
                result = "S21";
            else if (obj == "4")
                result = "S30";
            else if (obj == "5")
                result = "S31";
            else if (obj == "6")
                result = "S32";
            else if (obj == "7")
                result = "S40";
            else if (obj == "8")
                result = "S41";
            else if (obj == "9")
                result = "S42";
            else if (obj == "10")
                result = "S50";
            else if (obj == "11")
                result = "S51";
            else if (obj == "12")
                result = "S52";
            if (obj == "13")
                result = "Sother";
            else if (obj == "14")
                result = "P00";
            else if (obj == "15")
                result = "P11";
            else if (obj == "16")
                result = "P22";
            else if (obj == "17")
                result = "P33";
            else if (obj == "18")
                result = "Pother";
            else if (obj == "19")
                result = "F01";
            else if (obj == "20")
                result = "F02";
            else if (obj == "21")
                result = "F12";
            else if (obj == "22")
                result = "F03";
            else if (obj == "23")
                result = "F13";
            else if (obj == "24")
                result = "F23";
            else if (obj == "25")
                result = "F04";
            else if (obj == "26")
                result = "F14";
            else if (obj == "27")
                result = "F24";
            else if (obj == "28")
                result = "F05";
            else if (obj == "29")
                result = "F15";
            else if (obj == "30")
                result = "F25";
            else if (obj == "31")
                result = "Fother";
        } else if (PlayTypeID == "7303") {
            if (obj == "1" || obj == "2")
                result = "DifferGuest1_5";
            else if (obj == "3" || obj == "4")
                result = "DifferGuest6_10";
            else if (obj == "5" || obj == "6")
                result = "DifferGuest11_15";
            else if (obj == "7" || obj == "8")
                result = "DifferGuest16_20";
            else if (obj == "9" || obj == "10")
                result = "DifferGuest21_25";
            else if (obj == "11" || obj == "12")
                result = "DifferGuest26";
        }
        return result;
    },

    BetTeamOver: function (obj) {
        $(obj).attr("trhover");
    },

    BetTeamOut: function (obj) {
        $(obj).removeClass("trhover");
    },

    /*--------鼠标移入 Start--------*/
    TeamOver: function (obj) {
        var team = "";
        var sum = 1;
        var str_sum = "";
        $(obj).find("td:eq(2)").find("p").each(function () {
            team += $(this).text() + " * ";
        });
        $(obj).find("td:eq(5)").find("em").each(function () {
            sum = parseFloat($(this).text()) * sum;
        });
        sum = sum * 2;
        var str = sum.toString();
        if (str.indexOf(".") > 0) {
            str_sum = str.substring(0, str.indexOf(".") + 3);
        }
        else {
            str_sum = str;
        }
        $("#Group").text(team + " 2 = " + str_sum);
        $(obj).addClass("trhover");
    },
    /*--------鼠标移入 End--------*/

    /*--------鼠标移出 Start--------*/
    TeamOut: function (obj) {
        $("#Group").text("");
        $(obj).removeClass("trhover");
    }
    /*--------鼠标移出 End--------*/
};

$(function () {
    $("#gTab li:eq(0)").click(function () {
        $("#Scheme_join,#lookDetails").nextAll().show();
    });
    $("#gTab li:eq(1)").click(function () {
        $("#Scheme_join,#lookDetails").nextAll().hide();
    });

});


var tip = "";                                           //方案上传文本框提示内容
var dzgdURL = "../Home/Room/DingZhiGenDan.aspx";        //定制跟单URL
var notLogin = "-808";                                  //没有登录
var moneyNotEnough = "-107";                            //金额不足
var lotID = "74";
var playID = "7401";
var isuse = "";


$(function () {
    var isuselist = $("#hide_isuses").val().split("|");
    $(".isuse").each(function (i, j) {
        $(j).click(function () {
            $(".isuse").removeClass("namered");
            $(this).addClass("namered");
            isuse = isuselist[i].split(",")[1];
            $("#currentIssueId").val(isuselist[i].split(",")[0]);
            IssueInfo.getIsuseInfo();
        });
    });

    $(".isuse").slice(0, 1).click();

    tip = "请输入号码，最多录入1000注，若超过1000注请以.txt文档上传\n请参照标准格式:\n\n11113333000033\n00001111333300";

    /*
    *   头部大玩法切换，选号-合买-定制跟单
    */
    $(".zc_top_lieb01 li").each(function () {
        $(this).click(function () {
            $(this).siblings().removeClass("redMenu");
            $(this).addClass("redMenu");
            changeBuyWay($(this).attr("tag"));
        })
    });

    $(".questionMark").mouseenter(function () {
        $(".pop_prompt").removeClass("hide").addClass("show");
    });
    $(".questionMark").mouseleave(function () {
        $(".pop_prompt").removeClass("show").addClass("hide");
    });

    /**
    *   选号-投注方式切换
    */
    $("#tzWay li").click(function () {
        $(this).siblings().removeClass("zc_li_select");
        $(this).addClass("zc_li_select");
        changeTZWay($(this).attr("tag"));
    });

    /*
    *   参与合买-合买发起人名称搜索按钮点击事件
    */
    $("#btn_nameSearch").click(function () {
        if (checkCurrentIssueIsEnd()) {
            return;
        }
        var name = $("#txt_schemeUserName").val().replace(/[\s]/g, "");
        if ("" == name) {
            alert("请输入方案发起人名称。");
            $("#txt_schemeUserName").val("");
            $("#txt_schemeUserName").focus();
            return;
        }
        var tag = $("#fqhm_whereList a[class='active']").attr("tag");
        $("#hmFrame").attr("src", "../Home/Room/SchemeAll.aspx?radom=" + Math.random() + "&lotteryid=" + lotID + "&isuseid=" + $("#currentIssueId").val() + "&username=" + name + "&filter=" + tag);
    });

    /*
    *   发起合买查询条件列表 
    */
    $("#fqhm_whereList a").click(function () {
        if (checkCurrentIssueIsEnd()) {
            return;
        }
        $(this).siblings().removeClass("active");
        $(this).addClass("active");
        $("#hmFrame").attr("src", "../Home/Room/SchemeAll.aspx?radom=" + Math.random() + "&lotteryid=" + lotID + "&isuseid=" + $("#currentIssueId").val() + "&filter=" + $(this).attr("tag"));
        $("#txt_schemeUserName").val("");
    });

    //    IssueInfo.Init(); //初始化期号信息
    PTTZ.InitEvent(); //普通投注对象初始化
    YX5Z.InitEvent(); //预选5注对象初始化
    numberChange();
});

//发起后上传的金额验证
function numberChange() {
    $(".faqi_fangan").find(".subtract").click(function () {
        if (parseInt($(this).parent().find("input").val()) <= 2) {
            $(this).parent().find("input").val("1");
            $(this).css("background", "url(../Images/subtract.png) no-repeat");
        }
        else {
            $(this).parent().find("input").val(parseInt($(this).parent().find("input").val()) - 1);
            $(this).css("background", "url(../Images/subtract1.png) no-repeat");
            $(this).parent().find(".add").css("background", "url(../Images/add.png) no-repeat");
        }
        FQHSC.Multiple = parseInt($(this).parent().find("input").val());
        regulateNum();
    });
    $(".faqi_fangan").find(".add").click(function () {
        if (parseInt($(this).parent().find("input").val()) >= 998) {
            $(this).parent().find("input").val("999");
            $(this).css("background", "url(../Images/disabled_add.png) no-repeat");
        }
        else {
            $(this).parent().find("input").val(parseInt($(this).parent().find("input").val()) + 1);
            $(this).css("background", "url(../Images/add.png) no-repeat");
            $(this).parent().find(".subtract").css("background", "url(../Images/subtract1.png) no-repeat");
        }
        FQHSC.Multiple = parseInt($(this).parent().find("input").val());
        regulateNum()
    });
    $(".faqi_fangan").find("input").keyup(function () {
        if ($(this).val() <= 1) {
            $(this).val(1);
            $(this).parent().find(".subtract").css("background", "url(../Images/subtract.png) no-repeat");
        }
        if ($(this).val() >= 999) {
            $(this).val(999);
            $(this).parent().find(".add").css("background", "url(../Images/disabled_add.png) no-repeat");
        }
        FQHSC.Num = parseInt($(this).parent().find("input").val());
        regulateNum()
    }).keypress(function () {
        if (parseInt($(this).val()) >= 999) return false;
        return Lottery.MustBeDigitKey();
    });
    $(".upload").find(".fc-input").keypress(function () {
        return Lottery.MustBeDigitKey();
    });
    $(".upload").find(".fc-input").each(function (i, j) {
        $(j).keyup(function () {
            switch (i) {
                case 0:
                    var nums = (FQHSC.Multiple * 2 * FQHSC.Num / parseInt($(this).val())).toString();
                    if (nums.indexOf(".") >= 0 && nums.split(".")[1].length > 3) {
                        FQHSC.FSNum = FQHSC.Multiple * 2 * FQHSC.Num;
                        $(this).val(FQHSC.FSNum);
                    }
                    else if (parseInt($(this).val()) >= FQHSC.Multiple * 2 * FQHSC.Num) {
                        FQHSC.FSNum = FQHSC.Multiple * 2 * FQHSC.Num;
                        $(this).val(FQHSC.FSNum);
                    }
                    else if ($(this).val() == "" || $(this).val() == "0" || parseInt($(this).val()).toString().length != $(this).val().length) {
                        FQHSC.FSNum = 1;
                        $(this).val(1);
                    }
                    else {
                        FQHSC.FSNum = parseInt($(this).val());
                    }
                    if (FQHSC.MyRG < Math.round(FQHSC.FSNum * parseFloat($("#labInitiateSchemeMinBuyAndAssureScale").val()))) {
                        FQHSC.MyRG = Math.round(FQHSC.FSNum * parseFloat($("#labInitiateSchemeMinBuyAndAssureScale").val()));
                        $(".upload").find(".fc-input").slice(1, 2).val(FQHSC.MyRG);
                    }
                    if (FQHSC.MyRG > FQHSC.FSNum) {
                        FQHSC.MyRG = FQHSC.FSNum;
                        $(".upload").find(".fc-input").slice(1, 2).val(FQHSC.MyRG);
                    }
                    var n = Math.round(FQHSC.FSNum * parseFloat($("#labInitiateSchemeMinBuyAndAssureScale").val()));
                    if (n <= 0) {
                        $(".upload").find(".fengcheng_left").slice(1, 2).find("strong em").html("1");
                    }
                    else {
                        $(".upload").find(".fengcheng_left").slice(1, 2).find("strong em").html(n);
                    }
                    if (FQHSC.MyBD > FQHSC.FSNum - FQHSC.MyRG) {
                        FQHSC.MyBD = FQHSC.FSNum - FQHSC.MyRG;
                        $(".upload").find(".fc-input").slice(2, 3).val(FQHSC.MyBD);
                    }
                    break;
                case 1:
                    if ($(this).val() == "" || $(this).val() == "0" || parseInt($(this).val()).toString().length != $(this).val().length) {
                        FQHSC.MyRG = 1;
                        $(this).val(1);
                    }
                    else if (parseInt($(this).val()) < Math.round(FQHSC.FSNum * parseFloat($("#labInitiateSchemeMinBuyAndAssureScale").val()))) {
                        $(this).val(Math.round(FQHSC.FSNum * parseFloat($("#labInitiateSchemeMinBuyAndAssureScale").val())));
                        FQHSC.MyRG = parseInt($(this).val());
                    }
                    else if (parseInt($(this).val()) > FQHSC.FSNum) {
                        $(this).val(FQHSC.FSNum);
                        FQHSC.MyRG = FQHSC.FSNum;
                        FQHSC.MyBD = 0;
                        $(".upload").find(".fc-input").slice(2, 3).val(0);
                    }
                    else {
                        FQHSC.MyRG = $(this).val();
                    }
                    if (FQHSC.MyBD > FQHSC.FSNum - FQHSC.MyRG) {
                        FQHSC.MyBD = FQHSC.FSNum - FQHSC.MyRG;
                        $(".upload").find(".fc-input").slice(2, 3).val(FQHSC.MyBD);
                    }
                    break;
                case 2:
                    if ($(this).val() == "" || parseInt($(this).val()).toString().length != $(this).val().length) {
                        FQHSC.MyBD = 0;
                        $(this).val(0);
                    }
                    else if (FQHSC.FSNum - FQHSC.MyRG <= 0) {
                        FQHSC.MyBD = 0;
                        $(this).val(0);
                    }
                    else if (FQHSC.FSNum - FQHSC.MyRG < parseInt($(this).val())) {
                        FQHSC.MyBD = FQHSC.FSNum - FQHSC.MyRG;
                        $(this).val(FQHSC.FSNum - FQHSC.MyRG);
                    }
                    else {
                        FQHSC.MyBD = $(this).val();
                    }
                    break;
                default:
                    break;
            }
            regulateNum();
        });
    });
}

function regulateNum() {
    FQHSC.MyBD = parseFloat($("#fqassure").val());
    $(".betMoneyCount").html((FQHSC.Multiple * 2 * FQHSC.Num).toFixed(2));
    $(".upload").find(".fengcheng_left").slice(0, 1).find("em").html((FQHSC.Multiple * 2 * FQHSC.Num / FQHSC.FSNum).toFixed(2));
    $(".upload").find(".fengcheng_left").slice(1, 2).find("span em").html((FQHSC.Multiple * 2 * FQHSC.Num / FQHSC.FSNum * FQHSC.MyRG).toFixed(2));
    $(".upload").find(".fengcheng_left").slice(2, 3).find("span em").html((FQHSC.Multiple * 2 * FQHSC.Num / FQHSC.FSNum * FQHSC.MyBD).toFixed(2));
    $(".upload").find(".fengcheng_left").slice(2, 3).find("strong em").html(FQHSC.FSNum - FQHSC.MyRG);
    $("#lab_ShareMoney").html((FQHSC.Multiple * 2 * FQHSC.Num / parseFloat($("#tb_Share").val())).toFixed(2));
}

function checkCurrentIssueIsEnd() {
    //截止
    if ($("#CurrentIssueIsEnd").val() == "true") {
        alert("当前期已经截至。");
        return true;
    } else if ($("#matchList tr").length == 0) {
        alert("没有对阵信息。");
        return true;
    }
    return false;
}

/**
*   改变购买方式
*/
function changeBuyWay(tag) {
    $(".zc_qiuleibiao div").hide();
    $("#hideDiv").children().hide();
    switch (tag) {
        case "tz":          //投注
            $(".zc_tz").show();
            $(".zc_xk").show();
            if (!$("#yx5z").is(":visible")) {
                $("#selectNumberDiv").show();
            }
            break;
        case "hm":          //合买
            $("#selectNumberDiv").hide();
            $(".zc_fqhm_top").show();
            $(".zc_hmList").show();
            if (checkCurrentIssueIsEnd()) {
                return;
            }
            $("#fqhm_whereList a").siblings().removeClass("active");
            $("#fqhm_whereList a[tag='4']").addClass("active");
            var url = "../Home/Room/SchemeAll.aspx?radom=" + Math.random() + "&lotteryid=" + lotID + "&isuseid=" + $("#currentIssueId").val();
            $("#hmFrame").attr("src", url);
            break;
        case "dzgd":        //定制跟单
            $("#selectNumberDiv").hide();
            $(".zc_dingzhigd_top").show();
            $(".zc_dingzhigengdan").show();
            $("#span_name").text("用户名：");
            var url = dzgdURL + "?tag=phb&lotteryID=" + lotID + "&from=page";
            $("#dzgdFrame").attr("src", url);
            break;
    }
}

function DZGDLoginOver() {//定制跟单登录完成
    if (CheckIsLogin()) {
        var url = dzgdURL + "?tag=list&lotteryID=" + lotID;
        $("#dzgdFrame").attr("src", url);
    }
}

/**
*   改变投注方式
*/
function changeTZWay(tag) {
    $(".zc_tz").children().hide();
    $("#selectNumberDiv").show();
    $("#selectNumberDiv").css("margin-top", "0px");
    $("#hide_tzWay").val(tag);
    switch (tag) {
        case "pttz":        //普通投注
            $("#pttz").show();
            //选号信息显示
            $("#selectNumberInfo p").show();
            $("#selectNumberInfo div").show();
            break;
        case "dssc":        //单式上传
            $("#updateFrame").attr("src", "/Home/Room/SchemeUpload.aspx?id=74&PlayType=7401&Isuse=" + $("#currentIssueId").val());
            $("#selectNumberDiv").css("margin-top", "-18px");
            //由于方案上传不需要显示选号信息，所以隐藏
            $("#selectNumberInfo p").hide();
            $("#selectNumberInfo div").hide();
            $("#dssc").show();
            $("#txt_schemeList").val(tip);
            break;
        case "yx5z":        //预投5注
            $("#selectNumberDiv").hide();
            $("#yx5z").show();
            break;
    }
}

function PTTZLoginOver() {
    if (CheckIsLogin()) {
        if (Lottery.Num < 1) {
            var okfunc = function () { PTTZ.JX1Z(); };
            var cancelfunc = function () { };
            confirm("至少选择1注号码才能投注，是否机选1注碰碰运气?", okfunc, cancelfunc);
        }
    }
}


var PTTZ = {
    InitEvent: function () {
        $("#userName").click(function () {
            if (!CheckIsLogin()) {
                CreateLogin("DZGDLoginOver()");
                return;
            }
            var url = dzgdURL + "?tag=searchUser&lotteryID=" + lotID + "&userName=" + $("#txtName").val() + "&from=page";
            $("#dzgdFrame").attr("src", url);
        });
        /**
        *   普通投注-购买方式切换
        */
        $("#pttzBuyWay input").click(function () {
            var tag = $(this).attr("tag");
            if ("dg" == tag) {          //如果是代购点击就隐藏合买
                $("#pttz_hm").hide();
                $("#buyWayTipPTTZ").css("width", "125px");
                $("#tipTextForPTTZ").text("购买人自行全额购");
            } else {
                $("#pttz_hm").show();
                $("#buyWayTipPTTZ").css("width", "162px");
                $("#tipTextForPTTZ").text("由多人共同出资购买彩票");
            }
            $("#pttz_buyWay").val(tag);
        });
        /**
        *   普通投注-[全选]按钮点击事件
        */
        $("#pttz table tbody tr td strong").live("click", function () {
            //判断这个快捷键实现什么功能
            if ($(this).html() == "全") {
                $(this).parent().parent().find("td[class='mark'] b").removeClass("normal").addClass("active");
                $(this).html("清").addClass("active");
                $(this).parent().parent().attr("isSelected", "true"); //标识这行已经选了
            } else {
                $(this).parent().parent().find("td[class='mark'] b").removeClass("active").addClass("normal");
                $(this).html("全").removeClass("active");
                $(this).parent().parent().attr("isSelected", ""); //标识这行没有选了
            }
            //计算选号注数
            PTTZ.CheckSelectNumber();
        });
        /**
        *   普通投注-选号点击事件
        */
        $("#pttz table tbody tr td[class='mark']").live("click", (function () {
            var b = $(this).find("b");  //赛事号码
            var c = b.attr("class");
            if (c == "normal" || typeof (c) == "undefined") {
                b.removeClass("normal").addClass("active");                   //选中赛事后的样式
            } else {
                b.removeClass("active").addClass("normal");                //取消选中赛事后的样式
            }
            //得到这行选了多少个赛事了
            var len = $(this).parent().find("td[class='mark'] b[class='active']").length;
            //如果没有选赛事
            if (len == 0) {
                //标识这行没有选了
                $(this).parent().attr("isSelected", "");
            } else {
                if (len > 2) {
                    //这行已经选了3场赛事，快捷键就是【清】删除全部选中
                    $(this).parent().find("td > strong").html("清").addClass("active");
                } else {
                    $(this).parent().find("td > strong").html("全").removeClass("active");
                }
                $(this).parent().attr("isSelected", "true"); //标识这行已经选了
            }
            //计算选号注数
            PTTZ.CheckSelectNumber();
        }));

        /**
        *   清除全部选号
        */
        $("#clearAllSelect").click(function () {
            var tzWay = $("#hide_tzWay").val(); //得到投注方式，取值-普通投注、方案上传、预选5注
            if ("pttz" == tzWay) {
                PTTZ.ClearAllSelect();
            } else if ("dssc" == tzWay) {
                var okfunc = function () {
                    $("#txt_schemeList").val("");
                    $("#txt_schemeList").focus();
                };
                var cancelfunc = function () { };
                confirm("确认要清除上传方案内容吗?清除后需要重新填写才能进行投注。", okfunc, cancelfunc);
            }
        });
        /**
        *   选号操作项点击事件-机选1注、机选5注、机选10注、清空列表
        */
        $("#selectNumberOperate a").click(function () {
            var tag = $(this).attr("tag");
            switch (tag) {
                case "jx1z":        //机选1注
                    PTTZ.JX1Z();
                    break;
                case "jx5z":        //机选5注
                    PTTZ.JX5Z();
                    break;
                case "jx10z":       //机选10注
                    PTTZ.JX10Z();
                    break;
                case "qklb":        //清空列表
                    PTTZ.QKLB();
                    break;
            }
        });

        /**
        *   确认选号
        */
        $("#btnConfirmSelectNumber").click(function () {
            if (parseInt($("#selectZSMoney").html()) > 20000) {
                alert("每注不能大于20000元。");
                return;
            }
            if (checkCurrentIssueIsEnd()) {
                return;
            }
            var tzWay = $("#hide_tzWay").val();
            if ("pttz" == tzWay) {
                PTTZ.PTTZFunction();
            } else if ("dssc" == tzWay) {
                PTTZ.DSSC();
            }
            //调用计算函数
            PTTZ.Calculate();
        });

        /**
        *   单式上传-方案内容获得焦点事件
        */
        $("#txt_schemeList").focus(function () {
            var schemeContent = $("#txt_schemeList").val();
            if ("" == schemeContent || "undefined" == typeof (schemeContent) || schemeContent == tip) {
                $("#txt_schemeList").val("");
                return;
            }

        });
        /**
        *   单式上传-方案内容获得焦点事件
        */
        $("#txt_schemeList").blur(function () {
            var schemeContent = $("#txt_schemeList").val();
            if ("" == schemeContent || "undefined" == typeof (schemeContent) || schemeContent == tip) {
                $("#txt_schemeList").val(tip);
                return;
            }
        });
        /**
        *   普通投注-方案提成
        */
        $("#pttz_fatc b").click(function () {
            $(this).siblings().removeClass("active");
            $(this).addClass("active");
        });

        /**
        *   倍数减
        */
        $("#multipleMinus").click(function () {
            var sumMultiple = parseFloat($("#sumMultiple").val());
            if (sumMultiple > 1) {
                sumMultiple--;
                Lottery.Multiple--;
                $("#sumMultiple").val(sumMultiple);
            }
            PTTZ.Calculate();
        });

        /**
        *   倍数加
        */
        $("#multipleAdd").click(function () {
            var sumMultiple = parseFloat($("#sumMultiple").val());
            if (sumMultiple < 99) {
                sumMultiple++;
                Lottery.Multiple++;
                $("#sumMultiple").val(sumMultiple);
            }
            PTTZ.Calculate();
        });

        /**
        *   倍数加
        */
        $("#sumMultiple").blur(function () {
            var sumMultiple = $("#sumMultiple").val().trim();
            var reg = /^\d+$/;
            if (reg.test(sumMultiple)) {
                $("#sumMultiple").val(sumMultiple);
                Lottery.Multiple = sumMultiple;
            } else {
                $("#sumMultiple").val(1);
            }
            PTTZ.Calculate();
        });

        /**
        *   普通投注-总份数文本框失去焦点事件
        */
        $("#pttz_hm_totalShare").blur(function () {
            if (Lottery.Num < 1) {
                return;
            }
            //总金额
            var totalMoney = Lottery.GetSumMoney();
            //得到总份数
            var totalShare = parseFloat($("#pttz_hm_totalShare").val());
            if (!(/^[0-9]+$/.test($("#pttz_hm_totalShare").val())) || totalShare < 1) {
                $("#pttz_hm_totalShare").val(totalMoney);
            } else {
                //每份的金额
                var eachShareMoney = totalMoney / totalShare;
                //是否可以除尽
                if (Math.round(eachShareMoney * 100) / 100 != eachShareMoney) {
                    //如果除不尽那么就取总金额的值
                    $("#pttz_hm_totalShare").val(totalMoney);
                } else {
                    //除尽后的钱必须大于1块钱
                    //每份的钱必须大于0 
                    if (eachShareMoney < 1) {
                        $("#pttz_hm_totalShare").val(totalMoney);
                    }
                    //购买份数
                    var buyShare = parseFloat($("#pttz_hm_buyShare").val());
                    //保底份数
                    var assureShare = parseFloat($("#pttz_hm_assureShare").val());
                    //最低认购多少(百分比)
                    var assureScale = parseFloat($("#hide_pttz_minbuyShare").val());
                    //最低认购多少份
                    var minBuyShare = totalShare * (assureScale / 100);
                    //如果购买的份数大于总份数，那么认购最低认购份数
                    if (buyShare > totalShare) {
                        $("#pttz_hm_buyShare").val(minBuyShare < 1 ? (totalShare == 0 ? "0" : "1") : (minBuyShare.toString().indexOf(".") > -1 ? parseInt(minBuyShare) + 1 : minBuyShare));
                    }
                    //购买份数
                    buyShare = parseFloat($("#pttz_hm_buyShare").val());
                    //计算保底份数
                    if ((assureShare + buyShare) >= totalShare) {
                        $("#pttz_hm_assureShare").val(totalShare - buyShare);
                    }
                }
            }
            PTTZ.CalculateHM();
        });
        /**
        *   普通投注-认购份数文本框失去焦点事件
        */
        $("#pttz_hm_buyShare").blur(function () {
            if (Lottery.Num < 1) {
                return;
            }
            //得到总份数
            var totalShare = parseFloat($("#pttz_hm_totalShare").val());
            //购买份数
            var buyShare = parseFloat($("#pttz_hm_buyShare").val());
            //保底份数
            var assureShare = parseFloat($("#pttz_hm_assureShare").val());
            //最低认购多少(百分比)
            var assureScale = parseFloat($("#hide_pttz_minbuyShare").val());
            //最低认购多少份
            var minBuyShare = totalShare * (assureScale / 100);
            //1.如果认购的份数大于总分数，那么就认购最低认购份数
            //2.如果客户录入的数据是-1或1.1这样的，那么也是认购最低认购份数
            if (buyShare > totalShare || buyShare < minBuyShare || buyShare < 1 || !(/^[0-9]+$/.test($("#pttz_hm_buyShare").val()))) {
                $("#pttz_hm_buyShare").val(minBuyShare < 1 ? (totalShare == 0 ? "0" : "1") : (minBuyShare.toString().indexOf(".") > -1 ? parseInt(minBuyShare) + 1 : minBuyShare));
            }
            buyShare = parseFloat($("#pttz_hm_buyShare").val());
            //计算保底份数
            if (assureShare + buyShare >= totalShare) {
                $("#pttz_hm_assureShare").val(totalShare - buyShare);
            }
            PTTZ.CalculateHM();
        });
        /**
        *   普通投注-保底份数文本框失去焦点事件
        */
        $("#pttz_hm_assureShare").blur(function () {
            if (Lottery.Num < 1) {
                return;
            }
            if (!(/^[0-9]+$/.test($("#pttz_hm_assureShare").val()))) {
                $("#pttz_hm_assureShare").val(0);
            } else {
                //得到总份数
                var totalShare = parseFloat($("#pttz_hm_totalShare").val());
                //购买份数
                var buyShare = parseFloat($("#pttz_hm_buyShare").val());
                //保底份数
                var assureShare = parseFloat($("#pttz_hm_assureShare").val());
                //计算保底份数
                if (assureShare + buyShare >= totalShare) {
                    $("#pttz_hm_assureShare").val(totalShare - buyShare);
                }
            }

            PTTZ.CalculateHM();
        });

        /**
        *   普通投注-全额保底按钮点击事件
        */
        //        $("#pttz_hm_allBuyShare").click(function () {
        //            if (Lottery.Num < 1) {
        //                return;
        //            }
        //            //（总份数-认购份数）* 每份的金额
        //            $("#pttz_hm_assureShare").val(parseFloat($("#pttz_hm_totalShare").val()) - parseFloat($("#pttz_hm_buyShare").val()));
        //            PTTZ.CalculateHM();
        //        });

        /**
        *   投注
        */
        $("#btnTZ").click(function () {
            if (checkCurrentIssueIsEnd()) {
                return;
            }
            if (!$("#agreement").is(":checked")) {
                alert("请先阅读并同意《委托投注规则》后才能继续");
                return;
            }
            if (!CheckIsLogin()) {
                CreateLogin("PTTZLoginOver()");
                return;
            }
            if (Lottery.GetSumMoney() > 20000) {
                alert("投注金额不能大于2万。");
                return;
            }
            var tzWay = $("#hide_tzWay").val();
            //获得彩种投注号码
            var lotteryNumber = Lottery.GetLotteryNumber();
            if ("pttz" == tzWay) {
                if (Lottery.Num < 1) {
                    var okfunc = function () { PTTZ.JX1Z(); };
                    var cancelfunc = function () { };
                    confirm("至少选择1注号码才能投注，是否机选1注碰碰运气?", okfunc, cancelfunc);
                    return;
                }
            } else if ("dssc" == tzWay) {
                if (Lottery.Num < 1) {
                    var okfunc = function () {
                        PTTZ.JX1Z();
                        $("#txt_schemeList").focus();
                    };
                    var cancelfunc = function () { };
                    confirm("你还没有上传方案内容，是否机选1注碰碰运气?", okfunc, cancelfunc);
                    return;
                }
            }
            var title = "";
            var Description = "";
            var TotalShare = 1;
            var EachShareMoney = Lottery.GetSumMoney();
            var BuyShare = 1;
            var AssureShare = 0;
            var AssureMoney = 0;
            var SchemeBonusScale = 0;
            var buyWay = $("#pttz_buyWay").val();

            if ("dg" == buyWay) {

            } else if ("hm" == buyWay) {
                title = $("#pttz_hm_schemeTitile").val().replace(/[\s]/g, "");
                Description = $("#pttz_hm_schemeDesc").val().replace(/[\s]/g, "");
                TotalShare = parseFloat($("#pttz_hm_totalShare").val());
                EachShareMoney = parseFloat($("#pttz_eachShareMoney").val());
                BuyShare = parseFloat($("#pttz_hm_buyShare").val());
                AssureShare = parseFloat($("#pttz_hm_assureShare").val());
                AssureMoney = AssureShare * EachShareMoney; //保底金额
                SchemeBonusScale = parseFloat($("#pttz_fatc b[class='active']").attr("tag"));
            }
            PTTZBuy.LotName = "胜负彩";                                //彩种名称
            PTTZBuy.LotteryID = lotID;                                  //彩种ID
            PTTZBuy.IsuseID = $("#currentIssueId").val();              //期号ID						
            PTTZBuy.PlayTypeID = playID;                               //玩法ID
            PTTZBuy.Title = title;                                     //方案标题  				
            PTTZBuy.Description = Description;                         //方案描述  				
            PTTZBuy.SchemeContent = Lottery.GetLotteryNumber();        //方案内容  				
            PTTZBuy.SumNum = Lottery.Num;                              //注数    				
            PTTZBuy.Multiple = Lottery.Multiple;                       //倍数    				
            PTTZBuy.SumMoney = Lottery.GetSumMoney();                  //方案总金额   		 	
            PTTZBuy.TotalShare = TotalShare;                           //总份数  				
            PTTZBuy.EachShareMoney = EachShareMoney;                   //每份多少钱  			
            PTTZBuy.BuyShare = BuyShare;                               //认购份数  				
            PTTZBuy.AssureShare = AssureShare;                         //保底份数  				
            PTTZBuy.AssureMoney = AssureMoney;                         //保底金额
            PTTZBuy.SecrecyLevel = $("#pttz_bmWay input[name='pttz_bmWay']:checked").attr("tag");; //方案是否保密 0 不保密 1 到截止 2 到开奖 3 永远  
            PTTZBuy.SchemeBonusScale = SchemeBonusScale;               //方案佣金  				
            PTTZBuy.IsuseEndTime = $("#currentIssueEndTime").val();    //结束时间      			
            PTTZBuy.CurrentIssueIsEnd = $("#CurrentIssueIsEnd").val(); //本期是否已结束   		
            PTTZBuy.isNullBuyContent = 0;                              //预投5					注：这个参数值必须是"1"
            PTTZBuy.ChaseSumMoney = 0.00;                              //追号任务总金额，包括所有的期数
            if ("fqhsc" == buyWay) {
                PTTZBuy.Title = $(".upload").find(".cbuy-inputxx").slice(0, 1).val().replace(/[\s]/g, "");
                PTTZBuy.Description = $(".upload").find(".cbuy-inputxx").slice(1, 2).val().replace(/[\s]/g, "");
                PTTZBuy.SchemeContent = "";
                PTTZBuy.SumNum = FQHSC.Num;
                PTTZBuy.Multiple = FQHSC.Multiple;
                PTTZBuy.SumMoney = FQHSC.Multiple * FQHSC.Num * 2;
                PTTZBuy.TotalShare = FQHSC.FSNum;
                PTTZBuy.EachShareMoney = FQHSC.Num * 2 * FQHSC.Multiple / FQHSC.FSNum;
                PTTZBuy.BuyShare = FQHSC.MyRG;
                PTTZBuy.AssureShare = FQHSC.MyBD;
                PTTZBuy.AssureMoney = FQHSC.MyBD * EachShareMoney; //保底金额
                PTTZBuy.SecrecyLevel = $(".upload").find(".fengcheng-list_baomi").find("li[checked$='checked']").attr("value");
                PTTZBuy.SchemeBonusScale = $("#fengcheng_cway_list").find(".on").attr("value");
                PTTZBuy.isNullBuyContent = 1;
            }
            PTTZBuy.Buy(buyWay); //购买
        });
    },

    /**
    *   清除全部选中
    */
    ClearAllSelect: function () {
        $("#pttz table tbody tr").attr("isSelected", "");
        $("#pttz table tbody tr td[class='mark'] b").removeClass("active").addClass("normal");
        $("#pttz table tbody tr td strong").html("全").removeClass("active");
        //计算选号注数
        PTTZ.CheckSelectNumber();
    },

    /**
    *   检查选号
    */
    CheckSelectNumber: function () {
        //得到选了多少场赛事了，胜负彩必须选满14场赛事才能构成一注
        var len = $("#pttz table tbody tr[isSelected='true']").length;
        var zs = 1; //注数
        if (len > 13) {
            $("#pttz table tbody tr[isSelected='true']").each(function (i, tr) {
                zs *= ($(this).find("td[class='mark'] b[class='active']").length);
            });
        } else {
            zs = 0;
        }
        $("#selectZS").text(zs); //选了多少注
        $("#selectZSMoney").text(parseFloat(zs) * 2); //选了多少注 * 2 得到金额
        $("#hide_selectNumberNum").val(zs);
    },
    /*
    *   计算注数、倍数、金额、合买信息
    */
    Calculate: function () {
        //总注数 注：彩票类中的注数值会在每次点击确认选号后累加
        $("#sumZS").text(Lottery.Num);
        //计算
        Lottery.Multiple = parseFloat($("#sumMultiple").val());
        //总金额= 注数*倍数*2
        $("#sumMoney").text(Lottery.GetSumMoney());
        $("#pttz_hm_totalShare").val(Lottery.GetSumMoney());
        PTTZ.CalculateHM();
    },
    /**
    *   确认选号普通投注
    */
    PTTZFunction: function () {
        var zs = parseFloat($("#hide_selectNumberNum").val());
        if (zs < 1) {
            alert("至少选择14场赛事才能投注!");
            return;
        }
        Lottery.NumAdd(zs);                 //累积注数
        PTTZ.AddLotteryNumberToNumberList();     //添加投注号码到号码列表
    },

    /*
    *   添加选号到号码列表中
    */
    AddLotteryNumberToNumberList: function () {
        var lotteryNumber = "";
        var temp = "";
        var trList = $("#pttz table tbody tr");
        var trLen = trList.length;
        var tip = "";
        for (var i = 0; i < trLen; i++) {
            //每一行的选号结果
            var numberList = $(trList[i]).find("td[class='mark'] b[class='active']");
            //判断每一行选了多少个结果，如果有2个或者3个就需要用()括起来
            var len = numberList.length;
            if (len > 1) {
                temp = "(";
                $(numberList).each(function () {
                    temp += $(this).text();
                });
                temp += ")";
            } else {
                temp = $(numberList[0]).text(); //只有一个结果
            }
            //如果一注投注号码长度太长了，在号码列表中放不下，会还行，所以超出的长度使用...来代替
            if ((lotteryNumber + temp).length > 20) {
                lotteryNumber += lotteryNumber.indexOf("...") > -1 ? "" : "...";
            } else {
                lotteryNumber += temp;
            }
            //保存完整的投注号码。 鼠标悬停上面的时候显示完整的信息
            tip += temp;
        }
        PTTZ.AddSelectNumber($("#hide_selectNumberNum").val(), tip, lotteryNumber);
    }
    ,
    AddSelectNumber: function (zs, fullLotteryNumber, lotteryNumber) {
        var dfs = zs > 1 ? "复式" : "单式";
        var temp = fullLotteryNumber + "|" + PTTZBuy.PlayTypeID + "|" + zs * 2 + "|" + zs;
        $("#selectNumberList").prepend("<li title=" + fullLotteryNumber + "[" + zs + "注，" + zs * 2 + "元]" + " lotteryNumber = '" + temp + "'><p><b>" + dfs + "</b><b class=\"col-red\"> " + lotteryNumber + " </b>[<span tag='zs'>" + zs + "注</span>，" + zs * 2 + "元]</p><a href='javascript:;' onclick='deleteSelectNumber(this)'>删除</a></li>");
        PTTZ.ClearAllSelect();
    }
    ,
    /**
    *   确认选号单式上传
    */
    DSSC: function () {
        //        var schemeContent = $("#txt_schemeList").val();
        schemeContent = $("#txt_schemeList").val();
        if ("" == schemeContent || schemeContent == tip) {
            alert("请录入方案内容或上传方案。");
            $("#txt_schemeList").focus();
            return;
        }
        var reg = /[310]{14}/; //正则表达式 号码是格式只能是3、1、0  而且必须是14个
        var numberList = schemeContent.split("\n");
        var len = numberList.length;
        var array = new Array();
        var errorArray = new Array();
        //开始循环验证方案上传文本框里面的文本，如何条件后的数据存放在【array】数组中
        for (var i = 0; i < len; i++) {

            if ("" == numberList[i]) continue;
            if (reg.test(numberList[i]) && numberList[i].length == 14) {
                array.push(numberList[i]);
            } else {
                errorArray.push(numberList[i]);
            }
        }

        var temp = "";
        len = array.length;
        if (len > 0) {
            var li = "";
            for (var i = 0; i < len; i++) {
                temp = array[i];
                Lottery.NumAdd(1); //注数累计
                //                PTTZ.AddSelectNumber(1, temp, temp);
                var dfs = "单式";
                var _temp = temp + "|" + PTTZBuy.PlayTypeID + "|" + 1 * 2 + "|" + 1;
                li += "<li title=" + temp + "[" + 1 + "注，" + 1 * 2 + "元]" + " lotteryNumber = '" + _temp + "'><p><b>" + dfs + "</b><b class=\"col-red\"> " + temp + " </b>[<span tag='zs'>" + 1 + "注</span>，" + 1 * 2 + "元]</p><a href='javascript:;' onclick='deleteSelectNumber(this)'>删除</a></li>";
            }
            $("#selectNumberList").prepend(li);
            PTTZ.ClearAllSelect();
        }
        //        var tip = "过滤掉格式不正确的投注内容，请核对。";
        //        //        $.jBox.tip("过滤掉格式不正确的投注内容，请核对。");
        len = errorArray.length;
        temp = "";
        if (len > 0) {
            for (var i = 0; i < len; i++) {
                temp += errorArray[i] + "\n";
            }
            $("#txt_schemeList").val(temp);
            $.jBox.tip("过滤掉格式不正确的投注内容，请核对。");
            return;
        }
        $("#txt_schemeList").val(tip);
    },
    /**
    *   机选1注
    */
    JX1Z: function () {
        if (checkCurrentIssueIsEnd()) {
            return;
        }
        var number = ["3", "1", "0"];
        var len = 14;
        var lotteryNumber = "";
        var zs = 1;
        var dfs = zs > 1 ? "复式" : "单式";
        for (var i = 0; i < len; i++) {
            lotteryNumber += (number[Math.round(Math.random() * 2)]);
        }
        Lottery.NumAdd(1); //注数累计
        PTTZ.AddSelectNumber(1, lotteryNumber, lotteryNumber);
        PTTZ.Calculate();
    },

    /**
    *   机选5注
    */
    JX5Z: function () {
        if (checkCurrentIssueIsEnd()) {
            return;
        }
        for (var i = 0; i < 5; i++) {
            PTTZ.JX1Z();
        }
    },

    /**
    *   机选10注
    */
    JX10Z: function () {
        if (checkCurrentIssueIsEnd()) {
            return;
        }
        for (var i = 0; i < 2; i++) {
            PTTZ.JX5Z();
        }
    },

    /**
    *   清空列表
    */
    QKLB: function () {
        if (checkCurrentIssueIsEnd()) {
            return;
        }
        var okfunc = function () {
            $("#selectNumberList").html("");
            Lottery.Num = 0;
            PTTZ.Calculate();
            $("#pttz_hm_buyShare").val("0");
            $("#pttz_hm_assureShare").val("0");
            $("#shareMoney").text("0");
            $("#buyShareMoney").text("0");
            $("#assureMoney").text("0");
            $("#pttz_hm_maxAssureMoney").text("0");
        };
        var cancelfunc = function () { };
        confirm("确认要清除所有选号吗?", okfunc, cancelfunc);
    },
    /*
    *   计算合买
    */
    CalculateHM: function () {
        //总金额
        var sumMoney = Lottery.GetSumMoney();
        //总份数
        var totalShare = parseFloat($("#pttz_hm_totalShare").val());
        //认购份数
        var buyShare = parseFloat($("#pttz_hm_buyShare").val());
        //保底份数        
        var assureShare = parseFloat($("#pttz_hm_assureShare").val());
        //每份的金额        
        var eachShareMoney = sumMoney / totalShare;
        //购买份数
        var buyShare = parseFloat($("#pttz_hm_buyShare").val());
        //最低认购多少(百分比)
        var assureScale = parseFloat($("#hide_pttz_minbuyShare").val());
        //最少认购多少份
        var minBuyShare = totalShare * (assureScale / 100);
        //如果最少认购份数小于1，那么就认购1份，否则就根据具体的份数来认购。
        $("#pttz_hm_minbuyShare").text(minBuyShare < 1 ? (totalShare == 0 ? "0" : "1") : (minBuyShare.toString().indexOf(".") > -1 ? parseInt(minBuyShare) + 1 : minBuyShare));
        //如果认购的份数小于最低认购数，那么就认购最低认购数。
        if (buyShare < minBuyShare) {
            $("#pttz_hm_buyShare").val(minBuyShare < 1 ? (totalShare == 0 ? "0" : "1") : (minBuyShare.toString().indexOf(".") > -1 ? parseInt(minBuyShare) + 1 : minBuyShare));
        }
        //总份数多少钱
        $("#shareMoney").text(eachShareMoney * totalShare);
        //认购份数多少钱
        $("#buyShareMoney").text(eachShareMoney * buyShare);
        //最多可以保底多少钱
        $("#pttz_hm_maxAssureMoney").text(eachShareMoney * (totalShare - buyShare));
        //保底多少钱
        $("#assureMoney").text(eachShareMoney * assureShare);
    }
}


/**
*   删除某一注
*/
function deleteSelectNumber(obj) {
    var zs = parseFloat($(obj).parent().find("span[tag='zs']").text());
    Lottery.NumMinus(zs);
    PTTZ.Calculate();
    $(obj).parent().remove(); //删除这一注
    if ($("#selectNumberList li").length == 0) {
        $("#pttz_hm_buyShare").val("0");
        $("#shareMoney").text("0");
        $("#buyShareMoney").text("0");
        $("#assureMoney").text("0");
        $("#pttz_hm_maxAssureMoney").text("0");
    }
}

function YX5ZLoginOver() {
    if (CheckIsLogin()) {
        $("#yt5zTZ").click();
    }
}

/**
*	预选5注对象
*/
var YX5Z = {
    InitEvent: function () {
        /**
        *   预选5注-购买方式切换
        */
        //        $("#yx5zBuyWay input").click(function () {
        //            var tag = $(this).attr("tag");
        //            if ("dg" == tag) {          //如果是代购点击就隐藏合买
        //                $("#yx5z_hm").hide();
        //                $("#buyWayTipYX5Z").css("width", "125px");
        //                $("#tipTextForYX5Z").text("购买人自行全额购");
        //            } else {
        //                $("#yx5z_hm").show();
        //                $("#buyWayTipYX5Z").css("width", "162px");
        //                $("#tipTextForYX5Z").text("由多人共同出资购买彩票");
        //            }
        //            $("#yt5zBuyWay").val(tag);
        //        });

        /**
        *   预选5注-方案上限值点击事件
        **/
        $("#Limit").keyup(function () {
            if (parseInt($(this).val()) >= 20000) {
                $(this).val(20000);
            }
            else if (parseInt($(this).val()) < 2 || $(this).val() == "") {
                $(this).val(2);
            }
            //            else if (parseInt($(this).val()) / 2 != parseInt((parseInt($(this).val()) / 2))) {
            //                $(this).val($(this).val().substring(0, $(this).val().length - 1));
            //            }
            YTInfo.Limit = parseInt($(this).val());
            YTInfo.MyFC = parseInt($(this).val());
            YX5Z.YTChange();
        }).keypress(function () {
            return Lottery.MustBeDigitKey();
            //            if(Lottery.MustBeDigitKey())
            //            {
            //                if("48,50,52,54,56".indexOf(window.event.keyCode)>=0)
            //                {
            //                    return true;
            //                }
            //                return false;
            //            }
            //            return false;
        });

        /**
        *   预选5注-方案提成点击事件
        */
        $("#yx5z_fatc b").click(function () {
            $(this).siblings().removeClass("active");
            $(this).addClass("active");
        });

        /**
        *   预选5注-认购份数文本框失去焦点事件
        */
        $("#yx5z_hm_buyShare").blur(function () {
            if ($(this).val() == "") {
                $(this).val(1);
                YTInfo.MyRG = 1;
            }
            else {
                YTInfo.MyRG = parseInt($(this).val());
            }
            YX5Z.YTChange();
        }).keypress(function () {
            return Lottery.MustBeDigitKey();
        });
        /**
        *   预选5注-保底份数文本框失去焦点事件
        */
        $("#yx5z_hm_assureShare").keyup(function () {
            if ($(this).val() == "") {
                $(this).val(0);
                YTInfo.MyBD = 0;
            }
            else {
                YTInfo.MyBD = parseInt($(this).val());
            }
            YX5Z.YTChange();
        }).keypress(function () {
            return Lottery.MustBeDigitKey();
        });
        /**
        *   预选5注-全额保底按钮点击事件
        */
        //        $("#yx5z_hm_allBuyShare").click(function () {
        //            //总份数-认购份数
        //            $("#yx5z_hm_assureShare").val(YX5ZBuy.TotalShare - YX5ZBuy.BuyShare);
        //            YX5Z.CalculateYX5ZHMInfo();//调用预选5注计算合买信息方法计算合买。
        //        });
        /**
        *   预投5注-立即投注按钮点击事件
        */
        $("#yt5zTZ").click(function () {
            if (checkCurrentIssueIsEnd()) {
                return;
            }
            if (!$("#yx5z_agreement").is(":checked")) {
                alert("请先阅读并同意《委托投注规则》后才能继续");
                return;
            }
            if (!CheckIsLogin()) {
                CreateLogin("YX5ZLoginOver()");
                return;
            }
            if (YTInfo.Limit <= 0) {
                alert("预投金额不能小于2");
                return;
            }
            YX5ZBuy.LotName = "胜负彩";                                 //彩种名称
            YX5ZBuy.LotteryID = lotID;                                  //彩种id
            YX5ZBuy.IsuseID = $("#currentIssueId").val();               //期号id
            YX5ZBuy.PlayTypeID = playID;                                //玩法id
            YX5ZBuy.Title = $("#yx5z_hm_schemeTitle").val();            //方案标题
            YX5ZBuy.Description = $("#yx5z_hm_schemeDesc").val();       //方案描述
            YX5ZBuy.SchemeContent = "";                                 //方案内容，预选5注不需要上传方案内容，但是isNullBuyContent这个参数值必须传递“1”
            YX5ZBuy.SumNum = 0;                                         //预选5注
            YX5ZBuy.Multiple = 1;                                       //倍数
            YX5ZBuy.SumMoney = YTInfo.Limit;                            //总金额
            YX5ZBuy.TotalShare = YTInfo.MyFC;                           //总份数
            YX5ZBuy.EachShareMoney = 1;                                 //每份多少钱
            YX5ZBuy.BuyShare = YTInfo.MyRG;                             //购买多少份
            YX5ZBuy.AssureShare = YTInfo.MyBD;                          //保底多少份
            YX5ZBuy.AssureMoney = YTInfo.MyBD;         //保底金额
            YX5ZBuy.SecrecyLevel = $("#yx5z_bmWay input[name='yx5z_bmWay']:checked").attr("tag"); //保密程度 0 不保密 1 到截止 2 到开奖 3 永远
            YX5ZBuy.SchemeBonusScale = $("#yx5z_fatc b[class='active']").attr("tag");            //方案佣金
            YX5ZBuy.IsuseEndTime = $("#currentIssueEndTime").val();     //这期的截止时间
            YX5ZBuy.CurrentIssueIsEnd = $("#CurrentIssueIsEnd").val();  //这期是否已经截止
            YX5ZBuy.isNullBuyContent = 1;                               //预选5注 这个值传递1
            YX5ZBuy.ChaseSumMoney = 0.0;                                //追号任务总金额，包括所有的期数 没有追号就传0
            YX5ZBuy.LimitMoney = YTInfo.Limit;                          //上限值
            YX5ZBuy.Buy("hm");
        });
    },

    /**
    *   预投信息改变
    */
    YTChange: function () {
        $("#yx5z_hm_totalShare").val(YTInfo.Limit);
        var ZSRG = parseInt($("#labInitiateSchemeMinBuyAndAssureScale").val() * YTInfo.MyFC + 0.9);
        if (ZSRG < 1) {
            ZSRG = 1;
        }
        $("#yx5z_hm_minbuyShare").html(ZSRG);

        if (YTInfo.MyRG < ZSRG) {
            YTInfo.MyRG = ZSRG;
            $("#yx5z_hm_buyShare").val(ZSRG);
        }
        else if (YTInfo.MyRG > YTInfo.Limit) {
            YTInfo.MyRG = YTInfo.Limit;
            $("#yx5z_hm_buyShare").val(YTInfo.Limit);
        }
        $("#yx5z_hm_buyShareMoney").html(YTInfo.MyRG);

        var ZDBD = YTInfo.Limit - YTInfo.MyRG;
        $("#yx5z_hm_maxAssureMoney").html(ZDBD);
        if (YTInfo.MyBD > ZDBD) {
            YTInfo.MyBD = ZDBD;
            $("#yx5z_hm_assureShare").val(ZDBD);
        }
    },


    /**
    *   计算预选5注合买信息
    */
    CalculateYX5ZHMInfo: function () {
        var buyShare = parseFloat($("#yx5z_hm_buyShare").val());            //购买份数
        var totalShare = parseFloat($("#yx5z_hm_totalShare").val());        //总份数
        var assureShare = parseFloat($("#yx5z_hm_assureShare").val());      //保底份数

        var eachShareMoney = (10 / totalShare);                                                                   //每份的金额
        $("#yx5z_hm_totalShareMoney").text((eachShareMoney * totalShare).toFixed(1));                             //分成多少份金额
        $("#yx5z_hm_buyShareMoney").text((buyShare * eachShareMoney).toFixed(1));                                 //购买份数金额
        $("#yx5z_hm_assureShareMoney").text((assureShare * eachShareMoney).toFixed(1));                           //保底金额
        $("#yx5z_hm_maxAssureMoney").text(((totalShare - buyShare - assureShare) * eachShareMoney).toFixed(1));   //最大保底金额

        //-------------------------赋值合买购买参数-------------------------\\
        YX5ZBuy.LotName = "胜负彩";                                 //彩种名称
        YX5ZBuy.LotteryID = lotID;                                   //彩种id
        YX5ZBuy.IsuseID = $("#currentIssueId").val();               //期号id
        YX5ZBuy.PlayTypeID = playID;                                //玩法id
        YX5ZBuy.Title = $("#yx5z_hm_schemeTitle").val();            //方案标题
        YX5ZBuy.Description = $("#yx5z_hm_schemeDesc").val();       //方案描述
        YX5ZBuy.SchemeContent = "";                                 //方案内容，预选5注不需要上传方案内容，但是isNullBuyContent这个参数值必须传递“1”
        YX5ZBuy.SumNum = 5;                                         //预选5注
        YX5ZBuy.Multiple = 1;                                       //倍数
        YX5ZBuy.SumMoney = YX5ZBuy.SumNum * YX5ZBuy.Multiple * 2;           //总金额
        YX5ZBuy.TotalShare = totalShare;                            //总份数
        YX5ZBuy.EachShareMoney = eachShareMoney;                    //每份多少钱
        YX5ZBuy.BuyShare = buyShare;                                //购买多少份
        YX5ZBuy.AssureShare = assureShare;                          //保底多少份
        YX5ZBuy.AssureMoney = assureShare * eachShareMoney;         //保底金额
        YX5ZBuy.SecrecyLevel = $("#yx5z_bmWay input[name='yx5z_bmWay']:checked").attr("tag"); //保密程度 0 不保密 1 到截止 2 到开奖 3 永远
        YX5ZBuy.SchemeBonusScale = $("#yx5z_fatc b[class='active']").attr("tag");            //方案佣金
        YX5ZBuy.IsuseEndTime = $("#currentIssueEndTime").val();     //这期的截止时间
        YX5ZBuy.CurrentIssueIsEnd = $("#CurrentIssueIsEnd").val();  //这期是否已经截止
        YX5ZBuy.isNullBuyContent = 1;                               //预选5注 这个值传递1
        YX5ZBuy.ChaseSumMoney = 0.0;                                //追号任务总金额，包括所有的期数 没有追号就传0

    }
}


/**
*   预选5注投注购买对象
*/
var PTTZBuy = {
    LotName: "胜负彩",                            //彩种名称
    LotteryID: lotID,                          //彩种ID
    IsuseID: "",                            //期号ID						
    PlayTypeID: playID,                         //玩法ID
    Title: "",                              //方案标题  				
    Description: "",                        //方案描述  				
    SchemeContent: "",                      //方案内容  				
    SumNum: 0,                              //注数    				
    Multiple: 0,                            //倍数    				
    SumMoney: 0,                            //方案总金额   		 	
    TotalShare: 0,                          //总份数  				
    EachShareMoney: 0,                      //每份多少钱  			
    BuyShare: 0,                            //认购份数  				
    AssureShare: 0,                         //保底份数  				
    AssureMoney: 0,                         //保底金额
    SecrecyLevel: 0,                        //方案是否保密 0 不保密 1 到截止 2 到开奖 3 永远  
    SchemeBonusScale: 0,                    //方案佣金  				
    IsuseEndTime: "",                       //结束时间      			
    CurrentIssueIsEnd: true,                //本期是否已结束   		
    isNullBuyContent: 0,                    //预投5					注：这个参数值必须是"1"
    ChaseSumMoney: 0.00,                    //追号任务总金额，包括所有的期数
    /**
    *   显示购买信息
    */
    ShowBuyInfo: function (buyWay) {
        var handlerUrl = "/ajax/GetUserHandsel.ashx";
        var tag = true;
        var successFunc = function (handselMoney) {
            if ("hm" == buyWay) {
                var TipStr = "";
                TipStr += "<div>注　数：　" + PTTZBuy.SumNum + "</div>";
                TipStr += "<div>倍　数：　" + PTTZBuy.Multiple + "</div>";
                TipStr += "<div>总金额：　" + PTTZBuy.SumMoney.toFixed(2) + " 元</div>";
                var SchemeBonusScale = PTTZBuy.SchemeBonusScale;
                TipStr += "<div>方案提成：　" + ((SchemeBonusScale == 0) ? "无" : SchemeBonusScale + "%") + "</div>";
                TipStr += "<div>总份数：　" + PTTZBuy.TotalShare + " 份</div>";
                TipStr += "<div>每　份：　" + PTTZBuy.EachShareMoney.toFixed(2) + " 元</div>";
                TipStr += "<div>保　底：　" + PTTZBuy.AssureShare + " 份，" + (PTTZBuy.AssureShare * PTTZBuy.EachShareMoney).toFixed(2) + "元</div>";
                TipStr += "<div>购　买：　" + PTTZBuy.BuyShare + " 份，" + (PTTZBuy.EachShareMoney * PTTZBuy.BuyShare).toFixed(2) + " 元</div>";
                var handAmount = parseFloat(handselMoney);
                var handBalance = parseFloat(0.00);
                var consumptionMoney = parseFloat(PTTZBuy.EachShareMoney * PTTZBuy.BuyShare);
                if (consumptionMoney <= handAmount) {
                    handAmount = consumptionMoney;
                    handBalance = parseFloat(0.00);
                }
                else {
                    handBalance = consumptionMoney - handAmount;
                }
                TipStr += "<div>彩金消费：　" + handAmount.toFixed(2) + "元</div>";
                TipStr += "<div>余额消费：　" + handBalance.toFixed(2) + "元</div>";
                TipStr += "<div>按“确定”即表示您已阅读《用户投注协议》并立即提交方案，确定要提交方案吗？</div>";
                var art = dialog({
                    id: 'buydiv',
                    title: '您要发起[ ' + PTTZBuy.LotName + ' ]方案，详细内容：\n\n',
                    width: 500,
                    content: TipStr,
                    fixed: true,
                    ok: function () {
                        tag = true;
                        //订单防重复处理
                        $("#btnTZ").hide();
                        if ($("#dgBtnDoing").length <= 0) {
                            $("#btnTZ").after('<div id="dgBtnDoing" style="background:#666; width:148px;height:26px; margin:20px auto 0 auto; font-size:14px; padding-top:10px; color:#fff; font-weight:bold; cursor:pointer;">正在购买中</div>');
                        }
                        else {
                            $("#dgBtnDoing").show();
                        }
                        PTTZBuy.SendBuyRequest();
                    },
                    okValue: '确定',
                    cancel: function () {
                        tag = false;
                        //订单防重复处理
                        $("#btnTZ").show();
                        if ($("#dgBtnDoing").length > 0) {
                            $("#dgBtnDoing").hide();
                        }
                    },
                    cancelValue: '取消'
                });
                art.showModal();
            } else {
                var TipStr = "";
                TipStr += "<div>注　数：　" + PTTZBuy.SumNum + "</div>";
                TipStr += "<div>倍　数：　" + PTTZBuy.Multiple + "</div>";
                TipStr += "<div>总金额：　" + PTTZBuy.SumMoney.toFixed(2) + " 元</div>";
                TipStr += "<div>总份数：　" + PTTZBuy.TotalShare + " 份</div>";
                TipStr += "<div>每　份：　" + PTTZBuy.EachShareMoney.toFixed(2) + " 元</div>";
                TipStr += "<div>购　买：　" + PTTZBuy.BuyShare + " 份，" + (PTTZBuy.EachShareMoney * PTTZBuy.BuyShare).toFixed(2) + " 元</div>";
                var handAmount = parseFloat(handselMoney);
                var handBalance = parseFloat(0.00);
                var consumptionMoney = parseFloat(PTTZBuy.EachShareMoney * PTTZBuy.BuyShare);
                if (consumptionMoney <= handAmount) {
                    handAmount = consumptionMoney;
                    handBalance = parseFloat(0.00);
                }
                else {
                    handBalance = consumptionMoney - handAmount;
                }
                TipStr += "<div>彩金消费：" + handAmount.toFixed(2) + "元</div>";
                TipStr += "<div>余额消费：" + handBalance.toFixed(2) + "元</div>";

                TipStr += "<div>按“确定”即表示您已阅读《用户投注协议》并立即提交方案，确定要提交方案吗？</div>";
                var art = dialog({
                    id: 'buydiv',
                    title: '您要发起[ ' + PTTZBuy.LotName + ' ]方案，详细内容：',
                    width: 500,
                    content: TipStr,
                    fixed: true,
                    ok: function () {
                        tag = true;
                        //订单防重复处理
                        $("#btnTZ").hide();
                        if ($("#dgBtnDoing").length <= 0) {
                            $("#btnTZ").after('<div id="dgBtnDoing" style="background:#666; width:148px;height:26px; margin:20px auto 0 auto; font-size:14px; padding-top:10px; color:#fff; font-weight:bold; cursor:pointer;">正在购买中</div>');
                        }
                        else {
                            $("#dgBtnDoing").show();
                        }
                        PTTZBuy.SendBuyRequest();
                    },
                    okValue: '确定',
                    cancel: function () {
                        tag = false;
                        //订单防重复处理
                        $("#btnTZ").show();
                        if ($("#dgBtnDoing").length > 0) {
                            $("#dgBtnDoing").hide();
                        }
                    },
                    cancelValue: '取消'
                });
                art.showModal();
            }
        };

        f_ajaxPost(handlerUrl, { "act": "GetUserHandselMoney" }, successFunc, null, null);
        return tag;
    },
    GetBuyParameter: function () {
        var BuyParameter = {
            LotName: PTTZBuy.LotName,                               //彩种名称
            LotteryID: PTTZBuy.LotteryID,                           //彩种ID
            IsuseID: PTTZBuy.IsuseID,                               //期号ID						
            PlayTypeID: PTTZBuy.PlayTypeID,                         //玩法ID
            Title: PTTZBuy.Title,                                   //方案标题  				
            Description: PTTZBuy.Description,                       //方案描述  				
            SchemeContent: PTTZBuy.SchemeContent,                   //方案内容  				
            SumNum: PTTZBuy.SumNum,                                 //注数    				
            Multiple: PTTZBuy.Multiple,                             //倍数    				
            SumMoney: PTTZBuy.SumMoney,                             //方案总金额   		 	
            TotalShare: PTTZBuy.TotalShare,                         //总份数  				
            EachShareMoney: PTTZBuy.EachShareMoney,                 //每份多少钱  			
            BuyShare: PTTZBuy.BuyShare,                             //认购份数  				
            AssureShare: PTTZBuy.AssureShare,                       //保底份数  				
            AssureMoney: PTTZBuy.AssureMoney,                       //保底金额
            SecrecyLevel: PTTZBuy.SecrecyLevel,                     //方案是否保密 0 不保密 1 到截止 2 到开奖 3 永远  
            SchemeBonusScale: PTTZBuy.SchemeBonusScale,             //方案佣金  				
            IsuseEndTime: PTTZBuy.IsuseEndTime,                     //结束时间      			
            CurrentIssueIsEnd: PTTZBuy.CurrentIssueIsEnd,           //本期是否已结束   		
            isNullBuyContent: PTTZBuy.isNullBuyContent,             //预投5					注：这个参数值必须是"1"
            ChaseSumMoney: PTTZBuy.ChaseSumMoney                   //追号任务总金额，包括所有的期数
        };
        return BuyParameter;
    },
    Buy: function (buyWay) {
        if (PTTZBuy.ShowBuyInfo(buyWay)) {

        }
    },
    /**
    *   发送购买请求
    */
    SendBuyRequest: function () {

        //ajax提交投注内容
        $.ajax({
            type: "post",
            url: "../ajax/Buy.ashx",
            cache: false,
            async: true,
            data: PTTZBuy.GetBuyParameter(),
            dataType: "json",
            success: function (result) {
                if (parseInt(result.error, 10) > 0) {//购买成功
                    location.href = "/Home/Room/UserBuySuccess.aspx?LotteryID=" + lotID + "&Money=" + PTTZBuy.SumMoney + "&SchemeID=" + result.error;
                    return;
                } else if (notLogin == result.error) {
                    alert(result.msg);
                    //订单防重复处理
                    $("#btnTZ").show();
                    if ($("#dgBtnDoing").length > 0) {
                        $("#dgBtnDoing").hide();
                    }
                    return;
                } else if (moneyNotEnough == result.error) {//金额不足
                    var okfunc = function () {
                        window.location.href = "/Home/Room/OnlinePay/Alipay02/Send_Default.aspx";
                    };
                    var cancelfunc = function () {
                        //订单防重复处理
                        $("#btnTZ").show();
                        if ($("#dgBtnDoing").length > 0) {
                            $("#dgBtnDoing").hide();
                        }
                    };
                    confirm("您的余额不足，是否立即充值？", okfunc, cancelfunc);
                    return;
                } else if (result.error == "-808") {
                    location.href = "/UserLogin.aspx?RequestLoginPage=SFC/Default.aspx";
                    return;
                } else {
                    alert(result.msg);
                    //订单防重复处理
                    $("#btnTZ").show();
                    if ($("#dgBtnDoing").length > 0) {
                        $("#dgBtnDoing").hide();
                    }
                }
            }
        });
    }
}


/**
*   预选5注投注购买对象
*/
var YX5ZBuy = {
    LotName: "",                            //彩种名称
    LotteryID: "",                          //彩种ID
    IsuseID: "",                            //期号ID						
    PlayTypeID: "",                         //玩法ID
    Title: "",                              //方案标题  				
    Description: "",                        //方案描述  				
    SchemeContent: "",                      //方案内容  				
    SumNum: 0,                              //注数    				
    Multiple: 0,                            //倍数    				
    SumMoney: 0,                            //方案总金额   		 	
    TotalShare: 0,                          //总份数  				
    EachShareMoney: 0,                      //每份多少钱  			
    BuyShare: 0,                            //认购份数  				
    AssureShare: 0,                         //保底份数  				
    AssureMoney: 0,                         //保底金额
    SecrecyLevel: 0,                        //方案是否保密 0 不保密 1 到截止 2 到开奖 3 永远  
    SchemeBonusScale: 0,                    //方案佣金  				
    IsuseEndTime: "",                       //结束时间      			
    CurrentIssueIsEnd: true,                //本期是否已结束   		
    isNullBuyContent: 0,                    //预投5					注：这个参数值必须是"1"
    ChaseSumMoney: 0.00,                    //追号任务总金额，包括所有的期数
    LimitMoney: 0,                          //上限值
    /**
    *   显示购买信息
    */
    ShowBuyInfo: function (buyWay) {
        var handlerUrl = "/ajax/GetUserHandsel.ashx";
        var tag = false;
        var successFunc = function (handselMoney) {
            if ("hm" == buyWay) {
                var TipStr = "";
                TipStr += "<div>注　数：　" + YX5ZBuy.SumNum + "</div>";
                TipStr += "<div>倍　数：　" + YX5ZBuy.Multiple + "</div>";
                TipStr += "<div>总金额：　" + YX5ZBuy.SumMoney.toFixed(2) + " 元</div>";
                var SchemeBonusScale = YX5ZBuy.SchemeBonusScale;
                TipStr += "<div>方案提成：　" + ((SchemeBonusScale == 0) ? "无" : SchemeBonusScale + "%") + "</div>";
                TipStr += "<div>总份数：　" + YX5ZBuy.TotalShare + " 份</div>";
                TipStr += "<div>每　份：　" + YX5ZBuy.EachShareMoney.toFixed(2) + " 元</div>";
                TipStr += "<div>保　底：　" + YX5ZBuy.AssureShare + " 份，" + (YX5ZBuy.AssureShare * YX5ZBuy.EachShareMoney).toFixed(2) + "元</div>";
                TipStr += "<div>购　买：　" + YX5ZBuy.BuyShare + " 份，" + (YX5ZBuy.EachShareMoney * YX5ZBuy.BuyShare).toFixed(2) + " 元</div>";
                var handAmount = parseFloat(handselMoney);
                var handBalance = parseFloat(0.00);
                var consumptionMoney = parseFloat(YX5ZBuy.EachShareMoney * YX5ZBuy.BuyShare);
                if (consumptionMoney <= handAmount) {
                    handAmount = consumptionMoney;
                    handBalance = parseFloat(0.00);
                }
                else {
                    handBalance = consumptionMoney - handAmount;
                }
                TipStr += "<div>彩金消费：　" + handAmount.toFixed(2) + "元</div>";
                TipStr += "<div>余额消费：　" + handBalance.toFixed(2) + "元</div>";

                TipStr += "<div>按“确定”即表示您已阅读《用户投注协议》并立即提交方案，确定要提交方案吗？</div>";
                var art = dialog({
                    id: 'buydiv',
                    title: '您通过[ 预选5注 ]发起[ ' + YX5ZBuy.LotName + ' ]方案，详细内容：',
                    width: 500,
                    content: TipStr,
                    fixed: true,
                    ok: function () {
                        tag = true;
                        //订单防重复处理
                        $("#yt5zTZ").hide();
                        if ($("#dgBtnDoing_y").length <= 0) {
                            $("#yt5zTZ").after('<div id="dgBtnDoing_y" style="background:#666; width:148px;height:26px; margin:20px auto 0 auto; font-size:14px; padding-top:10px; color:#fff; font-weight:bold; cursor:pointer;">正在购买中</div>');
                        }
                        else {
                            $("#dgBtnDoing_y").show();
                        }
                        YX5ZBuy.SendBuyRequest();
                    },
                    okValue: '确定',
                    cancel: function () {
                        tag = false;
                    },
                    cancelValue: '取消'
                });
                art.showModal();
            } else {
                var TipStr = "您通过[ 预选5注 ]发起[ " + YX5ZBuy.LotName + " ]方案，详细内容：\n\n";
                TipStr += "　　注　数：　" + YX5ZBuy.SumNum + "\n";
                TipStr += "　　倍　数：　" + YX5ZBuy.Multiple + "\n";
                TipStr += "　　总金额：　" + YX5ZBuy.SumMoney.toFixed(2) + " 元\n\n";
                TipStr += "　　总份数：　" + YX5ZBuy.TotalShare + " 份\n";
                TipStr += "　　每　份：　" + YX5ZBuy.EachShareMoney.toFixed(2) + " 元\n\n";
                TipStr += "　　购　买：　" + YX5ZBuy.BuyShare + " 份，" + (YX5ZBuy.EachShareMoney * YX5ZBuy.BuyShare).toFixed(2) + " 元\n\n";
                var handAmount = parseFloat(handselMoney);
                var handBalance = parseFloat(0.00);
                var consumptionMoney = parseFloat(YX5ZBuy.EachShareMoney * YX5ZBuy.BuyShare);
                if (consumptionMoney <= handAmount) {
                    handAmount = consumptionMoney;
                    handBalance = parseFloat(0.00);
                }
                else {
                    handBalance = consumptionMoney - handAmount;
                }
                TipStr += "　　彩金消费：　" + handAmount.toFixed(2) + "元\n";
                TipStr += "　　余额消费：　" + handBalance.toFixed(2) + "元\n\n";
                tag = confirm(TipStr + "按“确定”即表示您已阅读《用户投注协议》并立即提交方案，确定要提交方案吗？");
            }
        };
        f_ajaxPost(handlerUrl, { "act": "GetUserHandselMoney" }, successFunc, null, null);
        return tag;
    },
    GetBuyParameter: function () {
        var BuyParameter = {
            LotName: YX5ZBuy.LotName,                           //彩种名称
            LotteryID: YX5ZBuy.LotteryID,                       //彩种ID
            IsuseID: YX5ZBuy.IsuseID,                           //期号ID						
            PlayTypeID: YX5ZBuy.PlayTypeID,                     //玩法ID
            Title: YX5ZBuy.Title,                               //方案标题  				
            Description: YX5ZBuy.Description,                   //方案描述  				
            SchemeContent: YX5ZBuy.SchemeContent,               //方案内容  				
            SumNum: YX5ZBuy.SumNum,                             //注数    				
            Multiple: YX5ZBuy.Multiple,                         //倍数    				
            SumMoney: YX5ZBuy.SumMoney,                         //方案总金额   		 	
            TotalShare: YX5ZBuy.TotalShare,                     //总份数  				
            EachShareMoney: YX5ZBuy.EachShareMoney,             //每份多少钱  			
            BuyShare: YX5ZBuy.BuyShare,                         //认购份数  				
            AssureShare: YX5ZBuy.AssureShare,                   //保底份数  				
            AssureMoney: YX5ZBuy.AssureMoney,                   //保底金额
            SecrecyLevel: YX5ZBuy.SecrecyLevel,                 //方案是否保密 0 不保密 1 到截止 2 到开奖 3 永远  
            SchemeBonusScale: YX5ZBuy.SchemeBonusScale,         //方案佣金  				
            IsuseEndTime: YX5ZBuy.IsuseEndTime,                 //结束时间      			
            CurrentIssueIsEnd: YX5ZBuy.CurrentIssueIsEnd,       //本期是否已结束   		
            isNullBuyContent: YX5ZBuy.isNullBuyContent,         //预投5					注：这个参数值必须是"1"
            ChaseSumMoney: YX5ZBuy.ChaseSumMoney,               //追号任务总金额，包括所有的期数
            LimitMoney: YX5ZBuy.LimitMoney                      //上限值
        };
        return BuyParameter;
    },
    Buy: function (buyWay) {


        if (!CheckIsLogin()) {
            CreateLogin("PTTZLoginOver()");
            return;
        }

        if (YX5ZBuy.ShowBuyInfo(buyWay)) {

        }
    },
    /**
    *   发送购买请求
    */
    SendBuyRequest: function () {
        //ajax提交投注内容
        $.ajax({
            type: "post",
            url: "../ajax/Buy.ashx",
            cache: false,
            async: true,
            data: YX5ZBuy.GetBuyParameter(),
            dataType: "json",
            success: function (result) {
                if (parseInt(result.error, 10) > 0) {
                    location.href = "/Home/Room/UserBuySuccess.aspx?LotteryID=" + lotID + "&Money=" + YX5ZBuy.SumMoney + "&SchemeID=" + result.error;
                    return;
                } else if (notLogin == result.error) {
                    alert(result.msg);
                    //订单防重复处理
                    $("#yt5zTZ").show();
                    if ($("#dgBtnDoing_y").length > 0) {
                        $("#dgBtnDoing_y").hide();
                    }
                    return;
                } else if (moneyNotEnough == result.error) {//金额不足
                    alert(result.msg);
                    //订单防重复处理
                    $("#yt5zTZ").show();
                    if ($("#dgBtnDoing_y").length > 0) {
                        $("#dgBtnDoing_y").hide();
                    }
                    location.href = "/Home/Room/OnlinePay/Default.aspx";
                } else if (result.error == "-808") {
                    location.href = "/UserLogin.aspx?RequestLoginPage=SFC/Default.aspx";
                    return;
                } else {
                    alert(result.msg);
                    //订单防重复处理
                    $("#yt5zTZ").show();
                    if ($("#dgBtnDoing_y").length > 0) {
                        $("#dgBtnDoing_y").hide();
                    }
                }
            }
        });
    }
}

/**
*   彩票类
*/
var Lottery = {
    ValidateInputMultiple: function (obj) { //检查倍数的输入是否正确
        if (parseInt($(obj).val(), 10) < 1 || parseInt($(obj).val(), 10) > this.MaxAllowInvestMultiple) {
            alert("倍数必须为小于或者等于" + this.MaxAllowInvestMultiple + "的正整数");
            $(obj).val(1);
        }
    },
    MustBeDigitKey: function () { //文本框输入只能是数字按键
        if (window.event.keyCode < 48 || window.event.keyCode > 57)
            return false;
        return true;
    },
    /**
    *   投注号码
    */
    LotteryNumber: "",
    /*
    *   注数
    */
    Num: 0,
    /*
    *   倍数
    */
    Multiple: 1,
    /*
    *   追加投注号码
    */
    LotteryNumberPush: function (number) {
        Lottery.LotteryNumber += number + "\n";
    },
    /*
    *   获得用户选择的投注号码
    */
    GetLotteryNumber: function () {
        var temp = "";
        var j = 0;
        var liList = $("#selectNumberList li");
        var len = liList.length;
        for (var i = 0; i < len; i++, j++) {
            temp += $(liList[i]).attr("lotteryNumber") + (j == len - 1 ? "" : "\n");
        }
        Lottery.LotteryNumber = temp;
        return Lottery.LotteryNumber;
    },
    /*
    *   获得总金额
    */
    GetSumMoney: function () {
        return Lottery.Num * Lottery.Multiple * 2;  //总金额= 注数*倍数*2
    },
    /*
    *   添加注数
    */
    NumAdd: function (num) {
        Lottery.Num += num;
    },
    /*
    * 减去注数
    */
    NumMinus: function (num) {
        Lottery.Num -= num;
    }
}

/**
*   期号信息
*/
var IssueInfo = {
    lotteryId: lotID,
    timeGetIsuseInfo: null, //获取期号定时器
    timeGetServerTime: null, //定时获得服务器定时器
    loadLotteryTime: null,  //加载彩种时间定时器
    /*
    *   初始化
    */
    Init: function () {
        //        IssueInfo.getIsuseInfo(); //获得期号信息
    },
    /**
    *   获取期号信息的Ajax请求
    */
    getIsuseInfo: function () {
        try {
            $.ajax({
                type: "post",
                url: "../ajax/DefaultPageIssue_ChaseInfo.ashx",
                data: { "lotteryId": IssueInfo.lotteryId, "name": isuse },
                cache: false,
                async: true,
                dataType: "text",
                success: function (result) {
                    IssueInfo.getIsuseInfoCallback(result); //获取期号回调函数
                },
                error: function (status) {
                    alert(status);
                }
            });
        } catch (e) {
            alert("获取期号信息失败,请刷新页面。");
        }
    },
    /**
    *   获取期号信息回调函数
    */
    getIsuseInfoCallback: function (response) {
        if (response == null) {
            IssueInfo.timeGetIsuseInfo = setTimeout("getIsuseInfo()", 10000);
            return;
        }
        $("#CurrentIssueIsEnd").val(false); //未截止

        //将timeGetIsuseInfo移除
        if (IssueInfo.timeGetIsuseInfo != null) {
            clearTimeout(IssueInfo.timeGetIsuseInfo);
        }
        var issueInfoArray = response.split("|");
        //显示当前期信息
        IssueInfo.showCurrentInfo(issueInfoArray[0]);

        //获得对阵信息
        IssueInfo.getMatchInfo();

        //获得服务器时间
        IssueInfo.getServerTime();
    },
    /*
    *   显示当前期信息
    */
    showCurrentInfo: function (currentIssueInfo) {
        var currentInfoArray = currentIssueInfo.split(",");
        //        $("#currentIssueId").val(currentInfoArray[0]);               //期号id
        //        if (typeof (currentInfoArray[1]) == "undefined")
        //        {
        //            $("#currentIssue").text("0");   //期号名称
        //        } else
        //        {
        //            $("#currentIssue").text(" " + currentInfoArray[1] + " ");   //期号名称
        //        }
        $("#currentIssueEndTime").val(currentInfoArray[2]);         //期号截止时间
        $("#updateFrame").attr("src", "/Home/Room/SchemeUpload.aspx?id=74&PlayType=4501&Isuse=" + $("#currentIssueId").val());
    },
    /*
    *   获得服务器时间ajax请求
    */
    getServerTime: function () {
        try {
            $.ajax({
                type: "post",
                url: "../ajax/getServerTime.ashx",
                cache: false,
                async: true,
                dataType: "text",
                success: function (result) {
                    IssueInfo.getServerTimeCallback(result);
                },
                error: function (status) {
                    //alert("获取服务器时间失败,请刷新页面");
                }
            });

        } catch (e) {
            // 如果失败了，就继续读取
            IssueInfo.timeGetServerTime = setTimeout("IssueInfo.getServerTime()", 1000);
        }
    },
    /*
    *   获得服务器时间回调函数
    */
    getServerTimeCallback: function (response) {
        if (response == null) {
            IssueInfo.timeGetServerTime = setTimeout("IssueInfo.getServerTime()", 1000);
            return;
        }

        //将timeGetServerTime移除
        if (IssueInfo.timeGetServerTime != null) {
            clearTimeout(IssueInfo.timeGetServerTime);
        }

        var serverTime = new Date(response.replace(new RegExp("-", "g"), "/")); //服务器时间
        var timePoor = serverTime.getTime() - new Date().getTime(); //计算服务时间与本地时间的差
        var isuseEndTime = new Date($("#currentIssueEndTime").val().replace(new RegExp("-", "g"), "/"));  //期截时间
        var to = isuseEndTime.getTime() - serverTime.getTime();

        var d = Math.floor(to / (1000 * 60 * 60 * 24));   //天
        var h = Math.floor(to / (1000 * 60 * 60)) % 24;   //小时
        var m = Math.floor(to / (1000 * 60)) % 60;        //分钟
        var s = Math.floor(to / 1000) % 60;               //秒



        var resultHtml = "<span style=\"color:#DF1515;\"><b>本期已截止投注</b></span>";
        if (!isNaN(d)) {

            if (d < 0) {
                $("#CurrentIssueIsEnd").val(true); //截止
                IssueInfo.loadLotteryTime = setTimeout("IssueInfo.Init()", 20000); //重新读取
            } else {
                clearTimeout(IssueInfo.loadLotteryTime);
                $("#CurrentIssueIsEnd").val(false); //未截止
                resultHtml = '<span class="zc_sj">' + (d > 9 ? String(d) : "0" + String(d)) + '</span>\
                              <span>天</span>\
                              <span class="zc_sj">' + (h > 9 ? String(h) : "0" + String(h)) + '</span>\
                              <span>小时</span>\
                              <span class="zc_sj">' + (m > 9 ? String(m) : "0" + String(m)) + '</span>\
                              <span>分</span>\
                              <span class="zc_sj">' + (s > 9 ? String(s) : "0" + String(s)) + '</span>\
                              <span>秒</span>';
            }
        }
        $(".zc_jiezhi_shijian").html(resultHtml);
        IssueInfo.timeGetServerTime = setTimeout("IssueInfo.getServerTime()", 1000);
    },
    /*
    *   获得对阵信息
    */
    getMatchInfo: function () {

        //截止
        if ($("#CurrentIssueIsEnd").val() == "true") {
            return;
        }
        try {
            $.ajax({
                type: "post",
                url: "../ajax/ZCGetMatchInfo.ashx",
                cache: false,
                async: true,
                data: { "lotteryId": IssueInfo.lotteryId, "issueId": $("#currentIssueId").val() }, //期号id会在getIsuseInfo函数中赋值
                dataType: "text",
                success: function (result) {
                    var json = eval("(" + result + ")");
                    if (json.errCode == "0") {
                        var matchList = json.data;
                        var tbody = "";
                        for (var i = 0; i < matchList.length; i++) {
                            var tr = "<tr>";
                            tr += "<td>" + matchList[i].MatchNumber + "</td>";
                            tr += "<td>" + matchList[i].Game + "</td>";
                            tr += "<td>" + matchList[i].DateTime + "</td>";
                            tr += "<td><i class=\"col-blue\">" + matchList[i].HostTeam + "</i> VS <i class=\"col-blue\">" + matchList[i].QuestTeam + "</i></td>";
                            tr += "<td>" + (matchList[i].winScale == "0" ? "-" : parseFloat(matchList[i].winScale).toFixed(2)) + "</td>";
                            tr += "<td>" + (matchList[i].drawScale == "0" ? "-" : parseFloat(matchList[i].drawScale).toFixed(2)) + "</td>";
                            tr += "<td>" + (matchList[i].lostScale == "0" ? "-" : parseFloat(matchList[i].lostScale).toFixed(2)) + "</td>";
                            tr += "<td class=\"mark\"><b class='normal'>3</b></td>";
                            tr += "<td class=\"mark\"><b class='normal'>1</b></td>";
                            tr += "<td class=\"mark\"><b class='normal'>0</b></td>";
                            tr += "<td><strong>全</strong></td>";
                            tr += "</tr>";
                            tbody += tr;
                        }
                        $("#matchList").html(tbody);
                    }
                },
                error: function (status) {
                    //alert("获取胜负彩对阵信息失败,请刷新页面后重试。");
                }
            });

        } catch (e) {
            //alert("获取胜负彩对阵信息失败,请刷新页面后重试。");
        }
    }
}

var FQHSC = {

    Num: 1,
    Multiple: 1,
    Summoney: 2,
    FSNum: 1,
    MyRG: 1,
    MyBD: 0

}

/**
*   预投信息
*/

var YTInfo = {
    Limit: 2,
    MyFC: 2,
    MyRG: 1,
    MyBD: 0
}
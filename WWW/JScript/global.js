$(function () {

    //选项卡切换效果（兄弟节点）
    $('.tabs > a').each(function (e) {
        $(this).click(function () {
            $(this).addClass('active');
            $(this).siblings().removeClass('active');
        })
    })

    $(".tabs > li").each(function (e) {
        $(this).mouseover(function () {
            var i = $(this).index();
            var mid = $(this).attr("mid");
            var dit = $(this).attr("tid");

            $('#' + dit).siblings("div").hide();
            $('#'+dit).show();
            $("#HidOpenWin").val(dit);
            GetLuckNumberse(mid, dit);

            $(this).addClass('active').siblings().removeClass('active');
        })
    });

    //菜单切换效果
    $('.menu li, .menu a').click(function () {
        $('.menu li, .menu a').removeClass('active');
        $(this).addClass('active');
    })

    //分页
    $('.page .num').click(function () {
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
    })


    //排序点击效果
    $('.order-by').toggle(
        function () {
            $(this).find('span').addClass('active');
        },
        function () {
            $(this).find('span').removeClass('active');
        }
    )

    //双色球背景切换
    $('.ball li > b').toggle(
        function () {
            $(this).addClass('active');
        },
        function () {
            $(this).removeClass('active');
        }
    )

    $("#openwin li").mouseover(function () {
        $(this).addClass("active").siblings().removeClass("active");
        if (!isNaN($(this).attr("mid"))) {
            if ($(this).attr("mid") == "0") {
                $("#szWinlottery").show();
                $("#jcWinlottery").hide();
                $("#gpWinlottery").hide();
            }
            else if ($(this).attr("mid") == "1") {
                $("#jcWinlottery").show();
                $("#szWinlottery").hide();
                $("#gpWinlottery").hide();
            }
            else if ($(this).attr("mid") == "2") {
                $("#gpWinlottery").show();
                $("#jcWinlottery").hide();
                $("#szWinlottery").hide();
            }
        }
    });
})

var currentLotteryID = null;

function Page_load(lotteryId) {
    currentLotteryID = lotteryId;

    loadLottery(currentLotteryID);
}

function loadLottery(lotteryID) {
    //获取当前投注奖期信息
    GetIsuseInfo(lotteryID);

    //获取投注时间信息
    GetServerTime(lotteryID);
}

//获取当前投注奖期信息
var time_GetIsuseInfo = null;
function GetIsuseInfo(lotteryID) {
    currentLotteryID = lotteryID;

    $.ajax({
        type: "post",
        url: "../ajax/DefaultPageIssue_ChaseInfo.ashx",
        data: { lotteryId: currentLotteryID },
        cache: false,
        async: false,
        dataType: "text",
        success: function (result) {
            if (result.split('|').length > 0 && result.split('|')[0].split(',').length > 1 && result.split('|')[0].split(',')[1] != $("#currIsuseName").text()) {
                GetIsuseInfo_callback(result);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert("call error");
        }
    });
}

function GetIsuseInfo_callback(response) {
    if (response == null) {
        time_GetIsuseInfo = setTimeout("GetIsuseInfo(" + currentLotteryID + ");", 10000);

        return;
    }

    //将time_GetIsuseInfo移除
    if (time_GetIsuseInfo != null) {
        clearTimeout(time_GetIsuseInfo);
    }

    var v = response;

    if (v.indexOf("|") == -1) {
        return;
    }

    var arrInfo = v.split('|');

    if (arrInfo.length != 3) {
        return;
    }

    var currIsuse = arrInfo[0];
    var arrcurrIsuse = currIsuse.split(',');
    $("#HidssqIsusesid").val(arrcurrIsuse[0]);
    $("#currIsuseName").html(arrcurrIsuse[1]);
    $("#HidIsuseEndTime").val(arrcurrIsuse[2]);
    if (arrcurrIsuse[3] > 0) {
        $("#ssqTotalMoney").html(arrcurrIsuse[3]);
    }
    else {
        $("#ssqTotalMoney").css("color", "gray");
        $("#ssqTotalMoney").html("0");
    }
}

//定时读取最近的开奖信息的定时器
var time_GetServerTime = null;

//获取服务器时间
function GetServerTime(lotteryID) {

    currentLotteryID = lotteryID;

    try {
        $.ajax({
            type: "post",
            url: "../ajax/getServerTime.ashx",
            cache: false,
            async: false,
            dataType: "text",
            success: function (result) {
                GetServerTime_callback(result);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert("call error");
            }
        });
    }
    catch (e) {
        //如果失败了，就继续读取
        time_GetServerTime = setTimeout("GetServerTime(" + lotteryID + ");", 10000);
    }
}

function GetServerTime_callback(response) {
    if (response == null) {

        time_GetServerTime = setTimeout("GetServerTime(" + currentLotteryID + ");", 10000);

        return;
    }

    //将time_GetServerTime移除
    if (time_GetServerTime != null) {
        clearTimeout(time_GetServerTime);
    }

    var serverTime = response;

    var IsuseEndTime = new Date($("#HidIsuseEndTime").val().replace(new RegExp("-", "g"), "/"));
    var TimePoor = new Date(serverTime.replace(new RegExp("-", "g"), "/")).getTime() - new Date().getTime();
    var to = IsuseEndTime.getTime() - new Date(serverTime.replace(new RegExp("-", "g"), "/")).getTime();

    var d = Math.floor(to / (1000 * 60 * 60 * 24));
    var h = Math.floor(to / (1000 * 60 * 60)) % 24;
    var m = Math.floor(to / (1000 * 60)) % 60;
    var s = Math.floor(to / 1000) % 60;

    if (!isNaN(d)) {
        if (d < 0) {
            $("#toCurrIsuseEndTime").html("本期已截止投注");

            var lottery = setTimeout("loadLottery(" + currentLotteryID + ");", 20000);

            return;
        }
        else {
            clearTimeout(lottery);
            $("#toCurrIsuseEndTime").html((d > 0 ? ((d > 9 ? String(d) : "0" + String(d)) + "天") : "") + ((h > 0 || d > 0) ? ((h > 9 ? String(h) : "0" + String(h)) + "时") : "") + ((m > 9 ? String(m) : "0" + String(m)) + "分") + ((s > 9 ? String(s) : "0" + String(s)) + "秒后截止"));
        }
    }

    setTimeout("showIsuseTime(" + IsuseEndTime.getTime() + ", " + TimePoor + ", " + 1000 + "," + currentLotteryID + ")", 1000);
}

//显示当前期的投注时间
var lockIsuseTime = null;
function showIsuseTime(eTime, tPoor, goTime, lotteryID) {

    if (goTime >= 600000)//10分钟
    {
        GetServerTime(lotteryID);

        return;
    }

    var serverTime = new Date().getTime() + tPoor;
    var IsuseEndTime = new Date($("#HidIsuseEndTime").val().replace(new RegExp("-", "g"), "/"));
    var to = IsuseEndTime.getTime() - serverTime;

    var d = Math.floor(to / (1000 * 60 * 60 * 24));
    var h = Math.floor(to / (1000 * 60 * 60)) % 24;
    var m = Math.floor(to / (1000 * 60)) % 60;
    var s = Math.floor(to / 1000) % 60;

    if (!isNaN(d)) {
        if (d < 0) {
            $("#toCurrIsuseEndTime").html("本期已截止投注");
            var lottery = setTimeout("loadLottery(" + lotteryID + ");", 20000);

            return;
        }
        else {
            clearTimeout(lottery);
            $("#toCurrIsuseEndTime").html((d > 0 ? ((d > 9 ? String(d) : "0" + String(d)) + "天") : "") + ((h > 0 || d > 0) ? ((h > 9 ? String(h) : "0" + String(h)) + "时") : "") + ((m > 9 ? String(m) : "0" + String(m)) + "分") + ((s > 9 ? String(s) : "0" + String(s)) + "秒后截止"));
        }
    }

    if (lockIsuseTime != null) {
        clearTimeout(lockIsuseTime);
    }

    lockIsuseTime = setTimeout("showIsuseTime(" + eTime + "," + tPoor + "," + (goTime + 1000) + "," + lotteryID + ")", 1000);
}

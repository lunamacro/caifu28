var currentLotteryIDTwo = null;

function Page_loadTwo(lotteryId) {
    currentLotteryIDTwo = lotteryId;

    loadLotteryTwo(currentLotteryIDTwo);
}

function loadLotteryTwo(lotteryID) {
    //获取当前投注奖期信息
    GetIsuseInfoTwo(lotteryID);

    //获取投注时间信息
    GetServerTimeTwo(lotteryID);
}

//获取当前投注奖期信息
var time_GetIsuseInfoTwo = null;
function GetIsuseInfoTwo(lotteryID) {
    currentLotteryIDTwo = lotteryID;

    $.ajax({
        type: "post",
        url: "../ajax/DefaultPageIssue_ChaseInfo.ashx",
        data: { lotteryId: currentLotteryIDTwo },
        cache: false,
        async: false,
        dataType: "text",
        success: function (result) {
            if (result.split('|').length > 0 && result.split('|')[0].split(',').length > 1 && result.split('|')[0].split(',')[1] != $("#currIsuseNamedlt").text()) {
                GetIsuseInfo_callbackTwo(result);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert("call error");
        }
    });
}

function GetIsuseInfo_callbackTwo(response) {
    if (response == null) {
        time_GetIsuseInfoTwo = setTimeout("GetIsuseInfoTwo(" + currentLotteryIDTwo + ");", 10000);

        return;
    }

    //将time_GetIsuseInfoTwo移除
    if (time_GetIsuseInfoTwo != null) {
        clearTimeout(time_GetIsuseInfoTwo);
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
    $("#HiddltIsusesid").val(arrcurrIsuse[0]);
    $("#currIsuseNamedlt").html(arrcurrIsuse[1]);
    $("#HidIsuseEndTimedlt").val(arrcurrIsuse[2]);
    if (arrcurrIsuse[3] > 0) {
        $("#dltTotalMoney").html(arrcurrIsuse[3]);
    }
    else {
        $("#dltTotalMoney").css("color", "gray");
        $("#dltTotalMoney").html("0");
    }
}

//定时读取最近的开奖信息的定时器
var time_GetServerTimeTwo = null;

//获取服务器时间
function GetServerTimeTwo(lotteryID) {

    currentLotteryIDTwo = lotteryID;

    try {
        $.ajax({
            type: "post",
            url: "../ajax/getServerTime.ashx",
            cache: false,
            async: false,
            dataType: "text",
            success: function (result) {
                GetServerTime_callbackTwo(result);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert("call error");
            }
        });
    }
    catch (e) {
        //如果失败了，就继续读取
        time_GetServerTimeTwo = setTimeout("GetServerTimeTwo(" + lotteryID + ");", 10000);
    }
}

function GetServerTime_callbackTwo(response) {
    if (response == null) {

        time_GetServerTimeTwo = setTimeout("GetServerTimeTwo(" + currentLotteryIDTwo + ");", 10000);

        return;
    }

    //将time_GetServerTimeTwo移除
    if (time_GetServerTimeTwo != null) {
        clearTimeout(time_GetServerTimeTwo);
    }

    var serverTime = response;

    var IsuseEndTime = new Date($("#HidIsuseEndTimedlt").val().replace(new RegExp("-", "g"), "/"));
    var TimePoor = new Date(serverTime.replace(new RegExp("-", "g"), "/")).getTime() - new Date().getTime();
    var to = IsuseEndTime.getTime() - new Date(serverTime.replace(new RegExp("-", "g"), "/")).getTime();

    var d = Math.floor(to / (1000 * 60 * 60 * 24));
    var h = Math.floor(to / (1000 * 60 * 60)) % 24;
    var m = Math.floor(to / (1000 * 60)) % 60;
    var s = Math.floor(to / 1000) % 60;

    if (!isNaN(d)) {
        if (d < 0) {
            $("#toCurrIsuseEndTimedlt").html("本期已截止投注");

            var lottery = setTimeout("loadLotteryTwo(" + currentLotteryIDTwo + ");", 20000);

            return;
        }
        else {
            clearTimeout(lottery);
            $("#toCurrIsuseEndTimedlt").html((d > 0 ? ((d > 9 ? String(d) : "0" + String(d)) + "天") : "") + ((h > 0 || d > 0) ? ((h > 9 ? String(h) : "0" + String(h)) + "时") : "") + ((m > 9 ? String(m) : "0" + String(m)) + "分") + ((s > 9 ? String(s) : "0" + String(s)) + "秒后截止"));
        }
    }

    setTimeout("showIsuseTimeTwo(" + IsuseEndTime.getTime() + ", " + TimePoor + ", " + 1000 + "," + currentLotteryIDTwo + ")", 1000);
}

//显示当前期的投注时间
var lockIsuseTimeTwo = null;
function showIsuseTimeTwo(eTime, tPoor, goTime, lotteryID) {

    if (goTime >= 600000)//10分钟
    {
        GetServerTimeTwo(lotteryID);

        return;
    }

    var serverTime = new Date().getTime() + tPoor;
    var IsuseEndTime = new Date($("#HidIsuseEndTimedlt").val().replace(new RegExp("-", "g"), "/"));
    var to = IsuseEndTime.getTime() - serverTime;

    var d = Math.floor(to / (1000 * 60 * 60 * 24));
    var h = Math.floor(to / (1000 * 60 * 60)) % 24;
    var m = Math.floor(to / (1000 * 60)) % 60;
    var s = Math.floor(to / 1000) % 60;

    if (!isNaN(d)) {
        if (d < 0) {
            $("#toCurrIsuseEndTimedlt").html("本期已截止投注");
            var lottery = setTimeout("loadLotteryTwo(" + lotteryID + ");", 20000);

            return;
        }
        else {
            clearTimeout(lottery);
            $("#toCurrIsuseEndTimedlt").html((d > 0 ? ((d > 9 ? String(d) : "0" + String(d)) + "天") : "") + ((h > 0 || d > 0) ? ((h > 9 ? String(h) : "0" + String(h)) + "时") : "") + ((m > 9 ? String(m) : "0" + String(m)) + "分") + ((s > 9 ? String(s) : "0" + String(s)) + "秒后截止"));
        }
    }

    if (lockIsuseTimeTwo != null) {
        clearTimeout(lockIsuseTimeTwo);
    }

    lockIsuseTimeTwo = setTimeout("showIsuseTimeTwo(" + eTime + "," + tPoor + "," + (goTime + 1000) + "," + lotteryID + ")", 1000);
}
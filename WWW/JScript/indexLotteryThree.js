var currentLotteryIDThree = null;

function Page_loadThree(lotteryId) {
    currentLotteryIDThree = lotteryId;

    loadLotteryThree(currentLotteryIDThree);
}

function loadLotteryThree(lotteryID) {
    //获取当前投注奖期信息
    GetIsuseInfoThree(lotteryID);

    //获取投注时间信息
    GetServerTimeThree(lotteryID);
}

//获取当前投注奖期信息
var time_GetIsuseInfoThree = null;
function GetIsuseInfoThree(lotteryID) {
    currentLotteryIDThree = lotteryID;

    $.ajax({
        type: "post",
        url: "../ajax/DefaultPageIssue_ChaseInfo.ashx",
        data: { lotteryId: currentLotteryIDThree },
        cache: false,
        async: false,
        dataType: "text",
        success: function (result) {
            if (result.split('|').length > 0 && result.split('|')[0].split(',').length > 1 && result.split('|')[0].split(',')[1] != $("#currIsuseName3d").text()) {
                GetIsuseInfo_callbackThree(result);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert("call error");
        }
    });
}

function GetIsuseInfo_callbackThree(response) {
    if (response == null) {
        time_GetIsuseInfoThree = setTimeout("GetIsuseInfoThree(" + currentLotteryIDThree + ");", 10000);

        return;
    }

    //将time_GetIsuseInfoThree移除
    if (time_GetIsuseInfoThree != null) {
        clearTimeout(time_GetIsuseInfoThree);
    }

    var v = response;

    if (v.indexOf("|") == -1) {
        return;
    }

    var arrInfo = v.split('|');

    //if (arrInfo.length != 3) {
    //    return;
    //}

    var currIsuse = arrInfo[0];
    var arrcurrIsuse = currIsuse.split(',');
    var loName=$("#HidOpenWin").val();
    $("#currIsuseNameCQSSC").html(arrcurrIsuse[1]);

    var isuseName = arrcurrIsuse[1];
    console.log(isuseName);
    $("#currIsuseName" + loName).text(isuseName);
    $("#HidIsuseEndTime3d").val(arrcurrIsuse[2]);
    
}

//定时读取最近的开奖信息的定时器
var time_GetServerTimeThree = null;

//获取服务器时间
function GetServerTimeThree(lotteryID) {

    currentLotteryIDThree = lotteryID;

    try {
        $.ajax({
            type: "post",
            url: "../ajax/getServerTime.ashx",
            cache: false,
            async: false,
            dataType: "text",
            success: function (result) {
                GetServerTime_callbackThree(result);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert("call error");
            }
        });
    }
    catch (e) {
        //如果失败了，就继续读取
        time_GetServerTimeThree = setTimeout("GetServerTimeThree(" + lotteryID + ");", 10000);
    }
}

function GetServerTime_callbackThree(response) {
    if (response == null) {

        time_GetServerTimeThree = setTimeout("GetServerTimeThree(" + currentLotteryIDThree + ");", 10000);

        return;
    }

    //将time_GetServerTimeThree移除
    if (time_GetServerTimeThree != null) {
        clearTimeout(time_GetServerTimeThree);
    }

    var serverTime = response;

    var IsuseEndTime = new Date($("#HidIsuseEndTime3d").val().replace(new RegExp("-", "g"), "/"));
    var TimePoor = new Date(serverTime.replace(new RegExp("-", "g"), "/")).getTime() - new Date().getTime();
    var to = IsuseEndTime.getTime() - new Date(serverTime.replace(new RegExp("-", "g"), "/")).getTime();

    var d = Math.floor(to / (1000 * 60 * 60 * 24));
    var h = Math.floor(to / (1000 * 60 * 60)) % 24;
    var m = Math.floor(to / (1000 * 60)) % 60;
    var s = Math.floor(to / 1000) % 60;

    if (!isNaN(d)) {
        if (d < 0) {
            var loName = $("#HidOpenWin").val();
            $("#toCurrIsuseEndTime" + loName).html("本期已截止投注");

            var lottery = setTimeout("loadLotteryThree(" + currentLotteryIDThree + ");", 20000);

            return;
        }
        else {
            clearTimeout(lottery);
            $("#toCurrIsuseEndTime" + loName).html((d > 0 ? ((d > 9 ? String(d) : "0" + String(d)) + "天") : "") + ((h > 0 || d > 0) ? ((h > 9 ? String(h) : "0" + String(h)) + "时") : "") + ((m > 9 ? String(m) : "0" + String(m)) + "分") + ((s > 9 ? String(s) : "0" + String(s)) + "秒后截止"));
        }
    }

    setTimeout("showIsuseTimeThree(" + IsuseEndTime.getTime() + ", " + TimePoor + ", " + 1000 + "," + currentLotteryIDThree + ")", 1000);
}

//显示当前期的投注时间
var lockIsuseTimeThree = null;
function showIsuseTimeThree(eTime, tPoor, goTime, lotteryID) {

    if (goTime >= 600000)//10分钟
    {
        GetServerTimeThree(lotteryID);

        return;
    }

    var serverTime = new Date().getTime() + tPoor;
    var IsuseEndTime = new Date($("#HidIsuseEndTime3d").val().replace(new RegExp("-", "g"), "/"));
    var to = IsuseEndTime.getTime() - serverTime;

    var d = Math.floor(to / (1000 * 60 * 60 * 24));
    var h = Math.floor(to / (1000 * 60 * 60)) % 24;
    var m = Math.floor(to / (1000 * 60)) % 60;
    var s = Math.floor(to / 1000) % 60;

    if (!isNaN(d)) {
        if (d < 0) {
            var loName = $("#HidOpenWin").val();
            $("#toCurrIsuseEndTime" + loName).html("本期已截止投注");
            var lottery = setTimeout("loadLotteryThree(" + lotteryID + ");", 20000);

            return;
        }
        else {
            clearTimeout(lottery);
            var loName = $("#HidOpenWin").val();
            $("#toCurrIsuseEndTime" + loName).html((d > 0 ? ((d > 9 ? String(d) : "0" + String(d)) + "天") : "") + ((h > 0 || d > 0) ? ((h > 9 ? String(h) : "0" + String(h)) + "时") : "") + ((m > 9 ? String(m) : "0" + String(m)) + "分") + ((s > 9 ? String(s) : "0" + String(s)) + "秒后截止"));
        }
    }

    if (lockIsuseTimeThree != null) {
        clearTimeout(lockIsuseTimeThree);
    }

    lockIsuseTimeThree = setTimeout("showIsuseTimeThree(" + eTime + "," + tPoor + "," + (goTime + 1000) + "," + lotteryID + ")", 1000);
}
// Public.js 文件
function Request(strName) {
    var url = location.search;
    var reg = new RegExp("(^|&)" + strName + "=([^&]*)(&|$)");
    var r = url.substr(url.indexOf("?") + 1).match(reg);
    if (r != null)
        return unescape(r[2]);
    return null;
}

function GetRandNumber(Number) {
    return Math.ceil(Math.random() * Number);
}

//增加 trim(),ltrim(),rtrim() 四个函数
String.prototype.ltrim = function () { return ltrim(this); }
String.prototype.rtrim = function () { return rtrim(this); }
String.prototype.rtrimWithReturn = function () { return rtrimWithReturn(this); }
String.prototype.trim = function () { return trim(this); }

//此处为独立函数 
function ltrim(str) {
    var i;
    for (i = 0; i < str.length; i++) {
        if ((str.charAt(i) != " ") && (str.charAt(i) != " ") && (str.charAt(i).charCodeAt() != 13) && (str.charAt(i).charCodeAt() != 10) && (str.charAt(i).charCodeAt() != 32))
            break;
    }
    str = str.substring(i, str.length);
    return str;
}

function rtrim(str) {
    var i;
    for (i = str.length - 1; i >= 0; i--) {
        if ((str.charAt(i) != " ") && (str.charAt(i) != " ") && (str.charAt(i).charCodeAt() != 13) && (str.charAt(i).charCodeAt() != 10) && (str.charAt(i).charCodeAt() != 32))
            break;
    }
    str = str.substring(0, i + 1);
    return str;
}

function rtrimWithReturn(str) {
    var i;
    for (i = str.length - 1; i >= 0; i--) {
        if (str.charAt(i) != " " && str.charAt(i) != " " && str.charAt(i) != "\n")
            break;
    }
    str = str.substring(0, i + 1);
    return str;
}

function trim(str) {
    return ltrim(rtrim(str));
}

function DateToString(datetime) {
    return datetime.getYear() + "-" + (datetime.getMonth() + 1) + "-" + datetime.getDate() + " " + datetime.getHours() + ":" + datetime.getMinutes() + ":" + datetime.getSeconds();
}

function StrToInt(str) {
    str = $.trim(str);
    if (str == "")
        return 0;

    var i = parseInt(str, 10);
    if (isNaN(i))
        return 0;

    return i;
}

function StrToFloat(str) {
    var NewStr = "";
    if (!isNaN(str)) {

        for (var i = 0; i < str.length; i++) {
            if (str.charAt(i) != "," && str.charAt(i) != " ")
                NewStr += str.charAt(i);
        }

        if (NewStr == "")
            return 0;

        var f = parseFloat(NewStr);
        if (isNaN(f))
            return 0;

        return f;

    }
}

function Round(Num, Len) {
    var temp = 1;
    for (var i = 0; i < Len; i++)
        temp *= 10;

    return Math.round(Num * temp) / 100;
}

document.onkeydown = function (event)//关F5
{
    event = event || window.event;
    if (event.keyCode == 116) {
        event.keyCode = 0;
        event.cancelBubble = true;
        return false;
    }
}

//function document.oncontextmenu()//关右键菜单
//{
//	return false;
//}

function CheckMoneyOnPress() {
    if (window.event.keyCode < 48 || window.event.keyCode > 57) {
        return false;
    }

    return true;
}

function CheckMoneyOnPressDecimal(sender) {
    //if(sender.value.search(/[0-9]{1,}.[0-9]{1,}/) != 0) 
    if (sender.value.search(/[0|1].[0-9]{1,}/) != 0) {
        sender.value = "";
        sender.focus();
    }
}


function $Id(id) {
    return document.getElementById(id);
}

var JsLoader = new Object();
JsLoader.Load = function (js, callback) {
    js = allcaijsfilepath + js + "?r=" + Math.random();
    var script = document.createElement("script");
    script.type = "text/javascript";

    script.onreadystatechange = function () {
        if (script.readyState && script.readyState != 'loaded' && script.readyState != 'complete') {
            return;
        }
        if (callback) {
            callback();
        }
    };
    script.src = js;
    var head = document.getElementsByTagName('head').item(0);
    head.appendChild(script);
}

function gethosturl() {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var pos = strFullPath.indexOf(strPath);
    var prePath = strFullPath.substring(0, pos);
    return (prePath);
}

var FrameObj6636 = null;
function InitFrame6636() {
    if (FrameObj6636 == null) {
        var _iframeTmp = document.createElement("iframe");
        _iframeTmp.id = "Page_LoadingFrame";
        _iframeTmp.style.position = "absolute";
        _iframeTmp.style.filter = "alpha(opacity=0)";
        _iframeTmp.style.border = "0px";
        _iframeTmp.style.zIndex = 999;
        try {
            _iframeTmp.style.width = document.documentElement.scrollWidth - 100 + "px";
            _iframeTmp.style.height = document.documentElement.scrollHeight - 350 + document.documentElement.scrollTop + "px";
        } catch (e) {
            _iframeTmp.style.width = "1000px";
            _iframeTmp.style.height = "1000px";
        }
        if (document.documentElement.scrollHeight + document.documentElement.scrollTop < window.screen.availHeight)
            _iframeTmp.style.height = window.screen.availHeight - 350 + "px";
        _iframeTmp.style.top = "0px";
        _iframeTmp.style.left = "0px";
        FrameObj6636 = _iframeTmp;
    }
}

function showLoadingTip6636() {
    var tip = document.createElement("div");
    tip.id = "loading_tip";
    tip.style.position = "absolute";
    tip.style.backgroundColor = "#fff";
    tip.style.border = "0px";
    tip.style.borderStyle = "ridge";
    tip.style.fontSize = "12px";
    tip.style.zIndex = 1000;
    tip.style.left = document.documentElement.offsetWidth - 190 + "px";
    tip.style.top = "0px";
    tip.style.width = "190px";
    tip.style.height = "14px";
    tip.style.color = "#666";
    tip.innerHTML = "<img alt='' src='../html/images/loading.gif'>"
    if (document.getElementById("Page_LoadingFrame"))
        document.body.appendChild(tip);
}

function clearLoadingTip6636() {
    if (document.getElementById("loading_tip"))
        document.body.removeChild(document.getElementById("loading_tip"));
}

function SetLoading6636() {
    InitFrame6636();
    if (!document.getElementById("Page_LoadingFrame")) {
        document.body.appendChild(FrameObj6636);
        showLoadingTip6636();
    }
}
function ClearLoading6636() {
    if (document.getElementById("Page_LoadingFrame")) {
        document.body.removeChild(document.getElementById("Page_LoadingFrame"));
        clearLoadingTip6636();
    }
}

function Ajax6636(handler, action, data, isObj, successFun, loading) {

    if (typeof (loading) == "undefined")
        loading = true;

    if (loading)
        SetLoading6636();

    var objStr;
    if (isObj != "false") {
        objStr = fw.json.tostring(data);
    }
    else {
        objStr = data;
    }
    var dataStr = "action=" + action + "&data=" + escape(objStr);

    dataStr += "&r=" + Math.random();
    $.ajax({
        url: handler, data: dataStr, async: true, type: "post", global: loading,
        success: successFun,
        error: function () { ClearLoading6636(); alert("登录异常，请重试一次，谢谢。可能是网络延时原因。"); return; },
        complete: function (XMLHttpRequest, textStatus) {
            ClearLoading6636();
        }
    });

}

function f_ajaxPost(handlerUrl, post_data, successCallback, errCallback, completeCallback) {
    $.ajax({
        type: "post",
        url: handlerUrl,
        data: post_data,
        cache: false,
        async: false,
        dataType: "json",
        success: function (result) {
            if (successCallback) {
                successCallback(result);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (errCallback) {
                errCallback(XMLHttpRequest, textStatus, errorThrown);
            }
        },
        complete: function (XMLHttpRequest, SuccessOrErrorthrown) {
            if (completeCallback) {
                completeCallback(XMLHttpRequest, SuccessOrErrorthrown);
            }
        }
    });
}
window.alert = function (str) {
    var d = dialog({
        content: str,
        fixed: true
    });
    d.showModal();
    setTimeout(function () { d.close().remove(); }, 2000);
};
window.confirm = function (str, okfunc, cancelfunc) {
    str = str.replace(/\n/g, "<br>");
    var d = dialog({
        title: "提示",
        content: str,
        fixed: true,
        ok: function () {
            if (okfunc) {
                okfunc();
            }
        },
        okValue: "确定",
        cancel: function () {
            if (cancelfunc) {
                cancelfunc();
            }
        },
        cancelValue: "取消"
    });
    d.showModal();
};
/*
 * 全额保底
 * totalShareControl : 总份数
 * shareControl : 每份金额
 * myShareControl : 我认购的份数
 * objControl : 保底输入框
 * assureMoney : 保底金额
 * obj : this
*/
function fullGuarantee(totalShareControl, shareControl, myShareControl, objControl, assureMoney, obj) {
    if (totalShareControl && myShareControl && objControl && obj.checked) {
        var left = parseFloat($("#" + totalShareControl).val()) - parseFloat($("#" + myShareControl).val());
        $("#" + objControl).val(left);
        $("#" + objControl).attr("disabled", true);
    }
    else {
        //$("#" + objControl).val('0');
        $("#" + objControl).attr("disabled", false);
    }
    $("#" + assureMoney).html(parseFloat(parseFloat($("#" + shareControl).html()) * parseFloat($("#" + objControl).val())).toFixed(2));
    //解绑事件
    $('#' + totalShareControl).unbind("change");
    $('#' + myShareControl).unbind("change");
    $('#' + objControl).unbind("change");
    $('#' + shareControl).unbind();
    //绑定事件
    $("#" + totalShareControl).bind('change', function () {
        fullGuarantee(totalShareControl, shareControl, myShareControl, objControl, assureMoney, obj);
    });
    $("#" + myShareControl).bind('change', function () {
        fullGuarantee(totalShareControl, shareControl, myShareControl, objControl, assureMoney, obj);
    });
    $("#" + objControl).bind('change', function () {
        fullGuarantee(totalShareControl, shareControl, myShareControl, objControl, assureMoney, obj);
    });
    $("#" + shareControl).bind('DOMNodeInserted', function (e) {
        fullGuarantee(totalShareControl, shareControl, myShareControl, objControl, assureMoney, obj);
    });
}
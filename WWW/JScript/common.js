// JavaScript Document
$(function () {

    //sidebar javascript
    var iWinH = $(window).outerHeight();
    var iHeadH = $(".header").outerHeight();
    $("#sidebar_insi").css({
        height: iWinH - iHeadH + 'px'
    });
    $(".sidebar_con").css({
        'min-height': iWinH - iHeadH + 'px'
    });
    $(".sidebar_con2").css({
        'min-height': iWinH - iHeadH + 'px'
    });

    $(window).resize(function (event) {
        iWinH = $(window).outerHeight();
        iHeadH = $(".header").outerHeight();
        $("#sidebar_insi").css({
            height: iWinH - iHeadH + 'px'
        });
        $(".sidebar_con").css({
            'min-height': iWinH - iHeadH + 'px'
        });
        $(".sidebar_con2").css({
            'min-height': iWinH - iHeadH + 'px'
        });
    });

    $(window).load(function () {
        //$.mCustomScrollbar.defaults.scrollButtons.enable = true; //enable scrolling buttons by default
        //$.mCustomScrollbar.defaults.axis = "yx"; //enable 2 axis scrollbars by default
        //$("#sidebar_insi").mCustomScrollbar({ theme: "minimal-dark" });
    });

    $("#sidebar_con dl").children("dt").bind('click', function (event) {
        if ($(this).nextAll("dd").css('display') == 'none') {
            $("#sidebar_con dl").children("dd").slideUp(500);
            $(this).nextAll("dd").slideDown(500);
        } else {
            $(this).nextAll("dd").slideUp(500);
        }
    });
    $("#sidebar_con2 dl").children("dt").bind('click', function (event) {
        if ($(this).nextAll("dd").css('display') == 'none') {
            $("#sidebar_con2 dl").children("dd").slideUp(500);
            $(this).nextAll("dd").slideDown(500);
        } else {
            $(this).nextAll("dd").slideUp(500);
        }
    });

    $('#sidebar_but').bind('click', function () {
        if ($("#sidebar").css('left') == '0px') {
            $("#sidebar").animate({ left: -$("#sidebar_con").outerWidth(true) + 'px' }, 500);
            $("#sidebar").animate({ left: -$("#sidebar_con2").outerWidth(true) + 'px' }, 500);
            $(this).addClass('open');
            $("#main").animate({ 'padding-left': '14px' }, 500);
        } else {
            $("#sidebar").animate({ left: 0 + 'px' }, 500);
            $(this).removeClass('open');
            $("#main").animate({ 'padding-left': $("#sidebar").outerWidth(true) + 'px' }, 500);
        }
    });

    $(".newspic_addbut").bind('click', function () {

        if ($(".newspic_addwrap").css('display') == 'none') {
            $(".newspic_addwrap").slideDown(200);
        }
    })

})
/**
*  @des ajax异步请求方法
*  @todo 许振兴 2015-06-15
*  @handlerUrl:一般处理程序地址
*  @post_data:需要提交的数据 {"act":"open","id":"1"}
*  @loadingDiv:需要遮罩层的控件ID
*  @successCallback:成功时的回调方法
*  @errCallback:错误时的回调方法
*  @completeCallback:完成时的回调方法
*  @return 返回http请求对象
*/
function f_ajaxPost(handlerUrl, post_data, loadingDiv, successCallback, errCallback, completeCallback) {
    if (loadingDiv) {
        $("#" + loadingDiv).showLoading();
    }
    $.ajax({
        type: "post",
        url: handlerUrl,
        data: post_data,
        cache: false,
        async: true,
        dataType: "json",
        success: function (result) {
            if (successCallback) {
                if (loadingDiv) {
                    $("#" + loadingDiv).hideLoading();
                }
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
/**
*  @des ajax同步请求方法
*  @todo 许振兴 2016-01-08
*  @handlerUrl:一般处理程序地址
*  @post_data:需要提交的数据 {"act":"open","id":"1"}
*  @loadingDiv:需要遮罩层的控件ID
*  @successCallback:成功时的回调方法
*  @errCallback:错误时的回调方法
*  @completeCallback:完成时的回调方法
*  @return 返回http请求对象
*/
function f_ajaxPost_sync(handlerUrl, post_data, loadingDiv, successCallback, errCallback, completeCallback) {
    if (loadingDiv) {
        $("#" + loadingDiv).showLoading();
    }
    $.ajax({
        type: "post",
        url: handlerUrl,
        data: post_data,
        cache: false,
        async: false,
        dataType: "json",
        success: function (result) {
            if (successCallback) {
                if (loadingDiv) {
                    $("#" + loadingDiv).hideLoading();
                }
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
function trim(str) { //删除左右两端的空格
    return str.replace(/(^\s*)|(\s*$)/g, "");
}
/**
*  @des 获取页面请求对象，url中"?"符后的字串
*  读取值先应 创建对象 var obj =GetRequest（）
*  页面读取参数值为 obj["参数名称"]
*  @todo 许振兴 2015-06-15
*  @return 返回http请求对象
*/
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = (strs[i].split("=")[1]);
        }
    }
    return theRequest;
}
/**
*  @des  格式化JSON时间
*  @jsonDate JSON时间
*  @type 类型；0：长时间；1：短时间
*  @todo 许振兴 2015-06-15
*  @return 返回时间
*/
function jsonDateFormat(jsonDate, type) {
    try {
        var date = new Date(parseInt(jsonDate.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var seconds = date.getSeconds();
        var milliseconds = date.getMilliseconds();
        if (type == 0) {
            return date.getFullYear() + "-" + addZero(month) + "-" + addZero(day) + " " + addZero(hours) + ":" + addZero(minutes) + ":" + addZero(seconds);
        }
        else {
            return date.getFullYear() + "-" + addZero(month) + "-" + addZero(day);
        }
    } catch (ex) {
        return "";
    }
}
function addZero(timeCol) {
    if (timeCol != undefined) {
        if (timeCol.toString().length == 1) {
            return "0" + timeCol;
        }
        return timeCol;
    }
}
// JavaScript Document
var isShow = true;
$(function () {

    var _iWinW = $(window).outerWidth();
    var timer = null;
    var speed = 500;

    //----------------------------------------index banner sta ----------------------------------
    //少于1920情况下banner居中
    function resleft() {
        _iWinW = $(window).outerWidth();
        if (_iWinW < 1920) {
            var _iLeft = (1920 - _iWinW) / 2;
            $("#bancen").css('margin-left', -_iLeft + 'px');
            $(".inside_banner img").css('margin-left', -_iLeft + 'px');
        }
    }
    resleft();
    //可视宽度变化时候，banner居中
    $(window).resize(function () {
        resleft();
    });

    var _oBan = $("#selidbox");
    var _iLiW = _oBan.children("li").outerWidth(true);
    var _iLiNum = _oBan.children("li").length;

    _oBan.css("width", _iLiW * _iLiNum + "px");

    //向左动画
    function SelidLeft() {
        _oBan.stop().animate({ left: -_iLiW + "px" }, speed, function () {
            _oBan.append(_oBan.children("li:eq(0)"));
            _oBan.css("left", 0);
        });
    }
    timer = setInterval(SelidLeft, 3000);
    //鼠标移动到banner图上动画停止，移开再重新开始滑动动画
    _oBan.hover(function () {
        clearInterval(timer);
    }, function () {
        timer = setInterval(SelidLeft, 3000);
    });
    //----------------------------------------index banner end ----------------------------------

    $("#user_list dl").children('dd:odd').addClass('bgcolor');
    $("#user_list2 dl").children('dd:odd').addClass('bgcolor');

    $("#openoffbut").click(function () {
        if ($("#openIs").length <= 0) {
            $("#openoffbut").after("<input id='openIs' type='hidden' value='1' />");
        }
        if ($("#openIs").val() == 2)
        {
            $("#openIs").val("1");
            $("#openoffbut").addClass("open");
            $(".num_search").slideDown();
        }
        else {
            $("#openIs").val("2");
            $("#openoffbut").removeClass("open");
            $(".num_search").slideUp();
        }
    });
    //----------------------------------------Search Block & None end ----------------------------------
    var oRoll = $("#rollbox")
    var iLilen = $("#rollbox").children('li').length;
    var iLiw = $("#rollbox").children('li').outerWidth(true);
    var iAlloyw = parseInt(iLilen / 12);
    var iFocus = $("#focusbox");

    //alert(iAlloyw);
    oRoll.css({
        width: iLiw * 3 * iAlloyw + 'px',
        position: 'relative'
    });
    //增加焦点
    for (var i = 0; i < iAlloyw; i++) {
        iFocus.append('<span></span>')
    }
    iFocus.children('span:eq(0)').addClass('curr');

    iFocus.children('span').bind('click', function () {
        oRoll.animate({ 'left': -$(this).index() * iLiw * 3 + 'px' }, 500);
        iFocus.children('span').removeClass('curr');
        $(this).addClass('curr');
    });
    //----------------------------------------代理商 javascript end ----------------------------------

    $(".nav ul").children('li').hover(function () {
        $(this).children('ul').stop().slideDown('400');
    }, function () {
        $(this).children('ul').stop().slideUp('400');
    });


})
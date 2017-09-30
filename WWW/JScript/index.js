window.$ = jQuery;
var boxCount = 0;
$.ajax({
    type: "POST",
    url: "/ajax/UserLogin.ashx",
    timeout: 20000,
    cache: false,
    async: false,
    dataType: "json",
    error: function () {
        alert("登录异常，请重试一次，谢谢。可能是网络延时原因。");
        return;
    },
    success: function (json) {
        if (isNaN(json.message)) {
            return;
        }

        if (parseInt(json.message) < 1) {
            return;
        }

        $("#DivUserinfo input").val(json.message);
        $("#topUserName").text(json.name);
        $("#hidMoney").text(json.Balance);

        if (json.ismanager == "True") {
            $("#r_login_manger").show();
        }
        else {
            $("#r_login_manger").hide();
        }
    }
});

if (parseInt($("#DivUserinfo input").val()) > 0) {
    $("#userInfo").show();
    $("#userName").text("欢迎您，" + $("#topUserName").text());
    $("#userMoney").html("账户余额：<span id='sp_Money'>******</span> <img src='/images/eye_close.png' style='margin-top:-4px;cursor:pointer;' title='显示余额' onclick='showOrHideMoney(1)' id='imgShow'><img src='/images/eye_open.png' style='margin-top:-4px;cursor:pointer;display:none;' title='隐藏余额' onclick='showOrHideMoney(0)' id='imgHide'>");
	$("#topUserMoney").html($("#userMoney").html());
    $("#lbefore").hide();
    $("#user").hide();
    $("#lafter").show();
    $("#viewAccount").show();
}
else {
    $("#userInfo").hide();
    $("#lbefore").show();
    $("#user").show();
    $("#lafter").hide();
    $("#viewAccount").hide();
}

$().ready(function () {
    $("#r_login_crefresh").click(function () {
        $("#r_login_cimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
    });

    $("#r_login_cimg").click(function () {
        $("#r_login_cimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
    });

    $("#kaijiang li").mouseover(function () {
        $(this).addClass("cur").siblings().removeClass("cur");

        if (!isNaN($(this).attr("mid"))) {
            $("#scroll_con").children("div").hide().eq(parseInt($(this).attr("mid"))).show();
            boxCount = parseInt($(this).attr("mid"));
        }
    });

    $("#gonggao li").mouseover(function () {
        $(this).addClass("cur").siblings().removeClass("cur");

        if (!isNaN($(this).attr("mid"))) {
            $("#wzgg_con").children("div").hide().eq(parseInt($(this).attr("mid"))).show();
        }
    });

    $("#cpTab0 li").each(function (i) {//check事件
        this.onclick = function () {
            $(this).addClass("active").parent().end().siblings().removeClass("active");
            InitData($(this).attr("mid"));
        }
    });

    $(".anyClass").jCarouselLite({
        btnNext: ".next",
        btnPrev: ".prev",
        visible: 2
    });

    $("img", "#slide_div").each(function (index, domEle) {
        $(domEle).attr("src", $(domEle).attr("url"));
    });

    $("#Image93").unbind("mouseover");
    $("#Image93").unbind("mouseout");
    $("#top_Gmcpcon").unbind("mouseover");
    $("#top_Gmcpcon").unbind("mouseout");
})

//登陆输入提示的处理
$("#r_login_u").click(function () {
    $("#r_login_tip").hide();
});
$("#r_login_p").click(function () {
    $("#r_login_tip").hide();
});
$("#r_login_c").click(function () {
    $("#r_login_tip").hide();
});
//login click event
$("#r_login_btn").click(function () {
    $("#r_login_tip").show();
    if ($("#r_login_u").val() == "") {
        $("#r_login_tip").html("请输入合法的用户名!");
        //$("#r_login_u").focus();
        $("#r_login_tip").css({ "top": "-30px" });
    } else if ($("#r_login_p").val() == "") {
        $("#r_login_tip").html("请输入合法的密码!");
        //$("#r_login_p").focus();
        $("#r_login_tip").css({ "top": "0px" });
    } else if ($("#r_login_c").val() == "") {
        $("#r_login_tip").html("请输入正确的验证码!!");
        //$("#r_login_c").focus();
        $("#r_login_tip").css({ "top": "25px" });
    } else {
        $("#r_login_tip").hide();

        $.ajax({
            type: "POST",
            url: "/ajax/UserLogin.ashx",
            data: "UserName=" + $("#r_login_u").val() + "&PassWord=" + $("#r_login_p").val() + "&RegCode=" + $("#r_login_c").val(),
            timeout: 20000,
            cache: false,
            async: false,
            dataType: "json",
            success: callSuccesslogin_btn,
            error: callErrorlogin_btn
        });
    }
});

function callSuccesslogin_btn(json) {
    $("#r_login_cimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
    $("#r_login_p").val('');
    if (isNaN(json.message)) {
        alert(json.message);
        return;
    }

    if (parseInt(json.message) < 1) {
        return;
    }

    if (json.ismanager == "True") {
        $("#r_login_manger").show();
        $("#user_login_manger").show();
    }
    else {
        $("#r_login_manger").hide();
        $("#user_login_manger").hide();
    }

    $("#DivUserinfo input").val(json.message);
    $("#hidMoney").text(json.Balance);
    $("#topUserName").text(json.name);
    $("#user_login_manger").hide();
    $("#userInfo").hide();
    $("#lbefore").show();
    $("#user").show();
    $("#lafter").hide();
    $("#viewAccount").hide();
    if ($("#HandselAmount").length <= 0) {
        $("body").append("<input type='hidden' id='HandselAmount'>");
    }
    alert(json.HandselAmount);
    $("#HandselAmount").val(json.HandselAmount);
}

function callErrorlogin_btn(a, b, c) {
    alert("登录异常，请重试一次，谢谢。可能是网络延时原因。");
}

$("#r_login_close").click(function () {
    $.ajax({
        type: "POST",
        url: "/ajax/UserLogin.ashx",
        data: "action=loginout",
        timeout: 20000,
        cache: false,
        async: false,
        dataType: "json",
        error: callErrorlogin,
        success: callSuccesslogin
    });
});

function callSuccesslogin(json, textStatus) {
    $("#r_login_cimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
    if (isNaN(json.message)) {
        alert(json.message);

        return;
    }
    // 清空
    $("#r_login_u").val("");
    $("#r_login_p").val("");
    $("#r_login_c").val("");

    $("#DivUserinfo input").val(json.message);
    $("#hidMoney").text(json.Balance);
    $("#lbefore").hide();
    $("#user").hide();
    $("#lafter").show();
    $("#viewAccount").show();
    $("#loginLay").hide();
    $("#userInfo").show();
    $("#userName").text("欢迎您，" + json.name);
    $("#userMoney").text("账户余额：" + json.Balance + "元");
}

function callErrorlogin() {
    alert("登录异常，请重试一次，谢谢。可能是网络延时原因。");
}

function InitData(LotteryID) {
    var tbody = "";

    $.ajax({
        type: "POST", //用POST方式传输
        dataType: "json", //数据格式:JSON
        url: 'Join/SchemeList.ashx', //目标地址
        data: "LotteryID=" + LotteryID + "&TopNum=11",
        beforeSend: function () { $("#divload").show(); $("#SchemeList").hide(); }, //发送数据之前
        complete: function () { $("#divload").hide(); $("#SchemeList").show(); }, //接收数据完毕
        success: function (json) {
            $("#SchemeList tr:gt(0)").remove();

            try {
                $("#SchemeList").append(json[0][0]["Content"]);
            } catch (e) { };

            $("#SchemeList tr:gt(0):odd").attr("class", "");
            $("#SchemeList tr:gt(0):even").attr("class", "th_even");

            $("#SchemeList tr:gt(0)").hover(function () {
                $(this).addClass('th_on');
            }, function () {
                $(this).removeClass('th_on');
            });

            $(".Share").keyup(function () {
                if (/\D/.test(this.value))
                    this.value = parseInt(this.value) || 1;
            });

            $(".join img").click(function () {
                if (!CreateLogin(this)) {
                    return;
                }
                join($(this).attr("mid"), $("#tbShare_" + $(this).attr("mid")).val(), $("#spanShareMoney_" + $(this).attr("mid")).text().replace(/[^\d]/g, ''), LotteryID);
            });

            $(".Share").blur(function () {
                $(this).val($(this).val().replace(/[^\d]/g, ''));

                if (parseInt($(this).val()) > parseInt($(this).parent().prev().children().html())) {
                    $(this).val($(this).parent().prev().children().html());
                }

                if (parseInt($(this).val()) <= 0) {
                    $(this).val(1);
                }
            });
        }
    });
}

function join(SchemeID, Share, ShareMoney, LotteryID) {
    $.ajax({
        type: "POST", //用POST方式传输
        dataType: "json", //数据格式:JSON
        url: 'Join/Join.ashx', //目标地址
        data: "SchemeID=" + SchemeID + "&BuyShare=" + Share + "&ShareMoney=" + ShareMoney + "&LotteryID=" + LotteryID,
        beforeSend: function () { $("#divload").show(); $("#Pagination").hide(); }, //发送数据之前
        complete: function () { $("#divload").hide(); $("#Pagination").show() }, //接收数据完毕
        success: function (json) {
            alert(json.message);
            if ($("#hidLotteryID").val()) {
                InitData(0);
            }
            else {
                InitData(LotteryID);
            }
        }
    });
}

var imgcounts = $("#imgCount").val();

$.fn.hideFocus = function () {
    $(this).focus(function () { this.blur() })
};

(function (m, M, w, o, T, h, ul, bt, a, t, d) {
    $(function (p, n) {
        ul = $('#slide_div ul');
        bt = $('#slide_btn .btn02');
        li = $('li', ul);
        a = $('a', ul);
        t = $('#slide_a');
        var g = $("img", a);
        g.attr('src', g.attr('url'));
        p = bt.last().next();
        n = p.next();
        for (i = 1; i < M; i++) {
            li.eq(i).remove()
        }
        d = auto();
        p.click(prev);
        n.click(next);
        $('a', p).hideFocus();
        $('a', n).hideFocus();
        bt.click(pos)
    });
    function auto() {
        return setTimeout(next, o)
    }
    function pos(n, t) {
        if (h) {
            n = m; m = $(this).attr('target') - 0;
            if (m != n) {
                set(m - n)
            }
        }
    }
    function prev() {
        if (h) {
            m--;
            m = m < 0 ? M - 1 : m;
            set(-1)
        }
    }
    function next() {
        if (h) {
            m++;
            m = m >= M ? 0 : m;
            set(1)
        }
    }
    function set(b, e) {
        h = 0; clearTimeout(d);
        ul.prepend(li.eq(m));
        b = ul.find('li');
        b.eq(1).fadeOut(T);
        b.eq(0).fadeIn(T, function () { b.eq(1).remove(); h = 1; d = auto() });
        bt.removeClass('btn01');
        bt.eq(m).addClass('btn01');
        e = a.eq(m);
        t.attr('href', e.attr('href'));
        t.html(e.attr('txt'));
        var g = $("img", e);
        g.attr('src', g.attr('url'));
    }
})(0, imgcounts, 450, 1000 * imgcounts, '', 1);

var myTimer;
var speed = 100; //速度毫秒 值越小速度越快
var stepSpeed = 4; //值越大越快
$(function () {
    var mybox = $(".scroll_box");
    //向上
    $(".scroll_up").bind("mouseover", function () {
        var nowPos = mybox[boxCount].scrollTop; //当前值
        changePos(mybox, nowPos, 0);
    }).bind("mouseout", function () {
        if (myTimer) { window.clearInterval(myTimer); }
    });
    //向下
    $(".scroll_down").bind("mouseover", function () {
        var nowPos = mybox[boxCount].scrollTop; //当前值
        var maxPos = mybox[boxCount].scrollHeight - mybox.outerHeight(); //最大值
        changePos(mybox, nowPos, maxPos);
    }).bind("mouseout", function () {
        if (myTimer) { window.clearInterval(myTimer); }
    });
});

function changePos(box, from, to) {
    if (myTimer) { window.clearInterval(myTimer); }
    var temStepSpeed = stepSpeed;
    if (from > to) {
        myTimer = window.setInterval(function () {
            if (box[boxCount].scrollTop > to) { box[boxCount].scrollTop -= (5 + temStepSpeed); temStepSpeed += temStepSpeed; }
            else { window.clearInterval(myTimer); }
        }, speed);
    } else if (from < to) {
        myTimer = window.setInterval(function () {
            if (box[boxCount].scrollTop < to) { box[boxCount].scrollTop += (5 + temStepSpeed); temStepSpeed += temStepSpeed; }
            else { window.clearInterval(myTimer); }
        }, speed);
    }
}

$(".butterL").click(function () {
    $(".JScon").scrollLeft(100);
});

$(".butterR").click(function () {
    $(".JScon").scrollLeft(-100);
});

//鼠标经过变色
$(function () {
    $("tr").hover(function () {
        $(this).addClass("th_on");
    }, function () {
        $(this).removeClass("th_on");
    });
});

var loadState = true;
$(window).bind("scroll", function (event) {
    var top = $(document).scrollTop();
    if (loadState) {
        if (parseInt(top) > 201) {
            // 加载合买方案
            InitData(72);
            $("#frmmain").attr("src", "/Lottery/Quick_SFC.aspx");
            loadState = false;
        }
    }
});

//合买份数递减
function MReduction(id) {
    var Share = parseInt($("#tbShare_" + id).val());
    if (Share - 1 <= 0) {
        $("#tbShare_" + id).val(1);

        return;
    }

    $("#tbShare_" + id).val((Share - 1).toString());
}

//合买份数递增
function MAddition(id) {
    var Share = parseInt($("#tbShare_" + id).val());
    var Surplus = parseInt($("#spanSurplus_" + id).text());

    if (Share + 1 > Surplus) {
        $("#tbShare_" + id).val(Surplus);

        return;
    }

    $("#tbShare_" + id).val(parseInt(Share + 1).toString());
}
//显示余额
function showOrHideMoney(state) {
    if (state == 0) {
        $("#sp_Money").html("******");
        $("#imgShow").show();
        $("#imgHide").hide();
    }
    else {
        $("#sp_Money").html(parseFloat($("#hidMoney").text()).toFixed(2));
        $("#imgHide").show();
        $("#imgShow").hide();
    }
}

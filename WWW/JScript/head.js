$.ajax({
    type: "POST",
    url: "/ajax/UserLogin.aspx",
    timeout: 20000,
    cache: false,
    async: false,
    dataType: "json",
    success: function (json) {
        if (isNaN(json.message) || (parseInt(json.message) < 1)) { return; }
        if (json.ismanager == "True") {
            $("#user_login_manger").show();
        }
        /*
        2015-07-15修改
        */
        if (json.isCpsManager == "True") {
            $("#user_login_manger").attr("href", "/Cps/admin/AgentAuditing.aspx");
        }
        $("#DivUserinfo input").val(json.message);
        $("#topUserName").text(json.name);
        $("#hidMoney").text(json.Balance);
    },
    error: function () { return; }
});

if (parseInt($("#DivUserinfo input").val()) > 0) {
    $("#lbefore").hide();
    $("#user").hide();
    $("#lafter").show();
    $("#userInfo").show();
    $("#viewAccount").show();
    $("#userName").text("欢迎您，" + $("#topUserName").text());
    $("#userMoney").text("账户余额：" + $("#hidMoney").text());
}
else {
    $("#userInfo").hide();
    $("#lbefore").show();
    $("#user").show();
    $("#lafter").hide();
    $("#viewAccount").hide();
}

$(function () {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "/ajax/Head.ashx",
        data: "",
        success: function (data) {
            $("#cai_1").append(data.Emphasis);
            $("#cai_2").append(data.unEmphasis);
        },
        error: function (e) { }
    });

    $('#spanLotteries,#divLotteries').bind({
        mouseover: function () {
            $("#divLotteries").show();
            //$("#spanLotteries").attr("style", "background:url(../Images/xuanze01.png) no-repeat");
        },
        mouseout: function () {
            $("#divLotteries").hide();
            //$("#spanLotteries").attr("style", "background:url(../Images/xuanze02.png) no-repeat");
        }
    });

    $("#DialogClose").click(function () {
        if ($(".dia_div").length > 0)
            $(".dia_div").remove();
        $("#loginLay").slideUp(500);
        $("body").css("overflow", "visible");
        $("#error_tips").hide();
        $("#lu,#lp").val("");
        if (window.evalscript != "") { eval(window.evalscript); }
    });

    $("#zhuceClose").click(function () {
        if ($(".dia_div").length > 0)
            $(".dia_div").remove();
        $("#registerLay").slideUp(500);
        $("body").css("overflow", "visible");
        $("#error_reg,#error_mreg").hide();
        $("#regName,#regPwd1,#regPwd2,#regMpwd1,#regMpwd2,#regCode,#regMobile").val("");
        $(".zhucheBody span img").hide();
    });

    //注册选项卡
    $("#regtype").children("li").bind('click', function () {

        $("#regtype").children("li").removeClass('curr');
        $(this).addClass('curr');
        $(".zhuche_dlkuang").find(".zhucheBody").css("display", "none");
        $(".zhuche_dlkuang").find(".zhucheBody").eq($(this).index()).css('display', 'block');
        $("#error_reg,#error_mreg").hide();
        if ($("#regtype li:eq(0)").hasClass("curr")) {
            $("#registerLay").css("height", "490");
        }
        else {
            $("#registerLay").css("height", "470");
        }

    });

    $("#login").click(function () {
        CreateLogin(this);
    });

    $("#regster").click(function () {
        $("#r_login_cimg2").click();
        var vDiv = $("<div class='dia_div'></div>");
        $("body").append(vDiv);
        $("#registerLay").slideDown(500);
        $("body").css("overflow", "hidden");
        vDiv.css({
            width: $(window).width(),
            height: $(window).height(),
            left: 0,
            top: 0
        });
        $("#error_reg").hide();
        $("#getCode").show();
        $("#sendOK").hide();
        //$("#regName").focus();

        document.onkeydown = function (e) {
            if (e == null) { //ie
                keycode = event.keyCode;
            } else { // mozilla
                keycode = e.which;
            }
            if (keycode == 13) {
                $("#regsterbtn").click();
            }
            if (keycode == 27) {
                if ($(".dia_div").length > 0)
                    $(".dia_div").remove();
                $("#registerLay").slideUp(500);
                $("body").css("overflow", "visible");
                $("#error_reg,#error_mreg").hide();
                $(".zhucheBody span img").hide();
                $("#regName,#regPwd1,#regPwd2,#regMpwd1,#regMpwd2,#regCode,#regMobile").val("");
            }
        }
    });

    $("#toRegster").click(function () {
        $("#registerLay").slideDown(500);
        $("#loginLay").slideUp(500);
        $("#getCode").show();
        $("#sendOK").hide();
        document.onkeydown = function (e) {
            if (e == null) { //ie
                keycode = event.keyCode;
            } else { // mozilla
                keycode = e.which;
            }
            if (keycode == 13) {
                $("#regsterbtn").click();
            }
            if (keycode == 27) {
                if ($(".dia_div").length > 0)
                    $(".dia_div").remove();
                $("#registerLay").slideUp(500);
                $("body").css("overflow", "visible");
                $("#regName,#regPwd1,#regPwd2,#regMpwd1,#regMpwd2,#regCode,#regMobile").val("");
            }
        }
    });

    $("#lu").blur(function () {
        $.ajax({
            type: "POST",
            url: "/ajax/CheckUserName.aspx",
            data: "UserName=" + $("#lu").val() + "&PassWord=" + $("#lp").val() + "&RegCode=" + $("#yzmtext").val(),
            timeout: 20000,
            cache: false,
            async: true,
            dataType: "json",
            error: callErrorfloginbtn,
            success: callSuccessfloginbtn
        });
    });

    $("#toLogin,#toLogin2").click(function () {
        $("#loginLay").slideDown(500);
        $("#registerLay").slideUp(500);
        document.onkeydown = function (e) {
            if (e == null) { //ie
                keycode = event.keyCode;
            } else { // mozilla
                keycode = e.which;
            }
            if (keycode == 13) {
                $("#floginbtn").click();
            }
            if (keycode == 27) {
                if ($(".dia_div").length > 0)
                    $(".dia_div").remove();
                $("#loginLay").slideUp(500);
                $("body").css("overflow", "visible");
                $("#error_tips").hide();
                $("#lu,#lp").val("");
            }
        }
    });

    $("#regName").bind({
        focus: function () {

        },
        blur: function () {
            RegsterUserName();
        }
    });

    $("#regPwd1").bind({
        focus: function () {

        },
        blur: function () {
            isRegisterPwd1();
        }
    });

    $("#regPwd2").bind({
        focus: function () {

        },
        blur: function () {
            isRegisterPwd2();
        }
    });

    $("#regMobile").bind({
        focus: function () {

        },
        blur: function () {
            if ($("#regMobile").val().length != 11) {
                $("#error_mreg").show();
                $("#error_mreg").html("手机号码输入错误，应为11位数");
                $("#showMobile img").css("display", "block");
                $("#showMobile img").attr("src", "/Images/error.png");
                addTrueOrFalse($("#regMobile"), false);
                return;
            }
            checkMobileName();
        }
    });

    $("#regMpwd1").bind({
        focus: function () {

        },
        blur: function () {
            isRegisterPwd1();
        }
    });

    $("#regMpwd2").bind({
        focus: function () {

        },
        blur: function () {
            isRegisterPwd2();
        }
    });

    $("#regCode").bind({
        focus: function () {

        },
        blur: function () {
            if ($("#regCode").val().length != 6) {
                $("#error_mreg").show();
                $("#error_mreg").html("验证码输入错误，应为6位数");
                $("#showCode img").css("display", "block");
                $("#showCode img").attr("src", "/Images/error.png");
                addTrueOrFalse($("#regCode"), false);
            }
            else {
                $("#error_mreg").hide();
                $("#error_mreg").html("");
                $("#showCode img").css("display", "block");
                $("#showCode img").attr("src", "/Images/right.png");
                addTrueOrFalse($("#regCode"), true);
            }
        }
    });

    //    $("#yzmup").click(function () {
    //        $("#yzmimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
    //    });

    //    $("#yzmimg").click(function () {
    //        $("#yzmimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
    //    });

    $("#topLoginOut").click(function () {
        $.ajax({
            type: "POST",
            url: "/ajax/UserLogin.aspx",
            data: "action=loginout",
            timeout: 20000,
            cache: false,
            async: false,
            dataType: "json",
            error: callErrortopLoginOut,
            success: callSuccesstopLoginOut
        });
    });

    function callSuccesstopLoginOut(json, textStatus) {
        if (isNaN(json.message)) {
            alert(json.message);

            return;
        }

        window.location.href = "/";

        $("#DivUserinfo input").val(json.message);
        $("#hidMoney").text(json.Balance);
        $("#user_login_manger").hide();
        $("#userInfo").hide();
        $("#lbefore").show();
        $("#user").show();
        $("#lafter").hide();
        $("#viewAccount").hide();
    }

    function callErrortopLoginOut() {
        alert("登录异常，请重试一次，谢谢。可能是网络延时原因!");
    }
})

function CheckIsLogin() {
    if (parseInt($("#DivUserinfo input").val()) > 0) {
        return true;
    } else {
        return false;
    }
}

function CreateLogin(script) {
    $("#r_login_cimg").click();
    if (parseInt($("#DivUserinfo input").val()) > 0) {
        eval(script);
        return true;
    }

    var vDiv = $("<div class='dia_div'></div>");
    $("body").append(vDiv);
    $("#loginLay").slideDown(500);
    $("body").css("overflow", "hidden");
    vDiv.css({
        width: $(window).width(),
        height: $(window).height(),
        left: 0,
        top: 0
    });
    $("#error_tips").hide();
    $("#lu").focus();

    document.onkeydown = function (e) {
        if (e == null) { //ie
            keycode = event.keyCode;
        } else { // mozilla
            keycode = e.which;
        }
        if (keycode == 13) {
            $("#floginbtn").click();
        }
        if (keycode == 27) {
            if ($(".dia_div").length > 0)
                $(".dia_div").remove();
            $("#loginLay").slideUp(500);
            $("body").css("overflow", "visible");
            $("#error_tips").hide();
            $("#lu,#lp").val("");
        }
    }

    window.evalscript = script;

    return false;
}
function czdingzhi() {
    window.location.href = window.location.href + "?type=1";
    //window.location.href = window.location.href;
}
function RegsterUserName() {
    var nameLen = StringLength($("#regName").val());
    if (nameLen > 16 || nameLen < 5) {
        $("#error_reg").show();
        $("#error_reg").html("用户名长度为5-16个字符，可使用数字、英文、中文");
        $("#showUserName img").css("display", "block");
        $("#showUserName img").attr("src", "/Images/error.png");
        addTrueOrFalse($("#regName"), false);
        return false;
    }
    var patrn = /[^0-9A-Za-z\u4e00-\u9fa5]/;
    if (patrn.test($("#regName").val())) {
        $("#error_reg").show();
        $("#error_reg").html("请使用汉字，数字或字母");
        $("#showUserName img").css("display", "block");
        $("#showUserName img").attr("src", "/Images/error.png");
        addTrueOrFalse($("#regName"), false);
        return false;
    }
    else {
        return checkUserName();
    }
}

function checkUserName() {
    if ($("#regName").val() == "") {
        $("#error_reg").show();
        $("#error_reg").html("用户名不能为空");
        $("#showUserName img").css("display", "block");
        $("#showUserName img").attr("src", "/Images/error.png");
        addTrueOrFalse($("#regName"), false);
    }

    var result = 0;
    $.ajax({
        type: "POST",
        url: "/ajax/CheckName.ashx", //发送请求的地址
        timeout: 20000,
        cache: false,
        async: false,
        data: "UserName=" + $("#regName").val(),
        dataType: "json",
        error: callError,
        success: callSuccess
    });

    function callSuccess(json, textStatus) {
        result = json.message;
    }

    function callError() {
        alert("检验失败，请重试一次，谢谢。可能是网络延时原因。");
    }

    if (Number(result) < 0) {
        if (Number(result) == -2) {
            $("#error_reg").show();
            $("#error_reg").html("用户名：" + $("#regName").val() + "已被占用，请重新输入");
            $("#hidReg").val(-2);
            $("#showUserName img").css("display", "block");
            $("#showUserName img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regName"), false);
            return false;
        }

        if (Number(result) == -3) {
            $("#error_reg").show();
            $("#error_reg").html("用户名长度为5-16个字符，可使用数字、英文、中文");
            $("#hidReg").val(-3);
            $("#showUserName img").css("display", "block");
            $("#showUserName img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regName"), false);
            return false;
        }

        if (Number(result) == -4) {
            $("#error_reg").show();
            $("#error_reg").html("用户名中包含敏感字符，请换一个用户名");
            $("#hidReg").val(-4);
            $("#showUserName img").css("display", "block");
            $("#showUserName img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regName"), false);
            return false;
        }
    }
    else {
        $("#hidReg").val(0);
        $("#error_reg").hide();
        $("#showUserName img").css("display", "block");
        $("#showUserName img").attr("src", "/Images/right.png");
        addTrueOrFalse($("#regName"), true);
    }
    return true;
}
//文本控件添加正确或失败样式
//obj：控件的对象；boolStr: true || false
function addTrueOrFalse(obj, boolStr) {
    if (boolStr) {
        $(obj).removeClass("validError");
        $(obj).addClass("validRight");
        return;
    }
    else {
        $(obj).removeClass("validRight");
        $(obj).addClass("validError");
        return;
    }
}

function checkMobileName() {
    if ($("#regMobile").val() == "") {
        $("#error_mreg").show();
        $("#error_mreg").html("用户名不能为空");
        $("#showMobile img").css("display", "block");
        $("#showMobile img").attr("src", "/Images/error.png");
        addTrueOrFalse($("#regMobile"), false);
    }
    var result = 0;
    $.ajax({
        type: "POST",
        url: "/ajax/CheckName.ashx", //发送请求的地址
        timeout: 20000,
        cache: false,
        async: true,
        data: "UserName=" + $("#regMobile").val() + "&Mobile=" + $("#regMobile").val(),
        dataType: "json",
        error: callError,
        success: callSuccess
    });

    function callSuccess(json, textStatus) {
        result = json.message
        if (Number(result) < 0) {
            if (Number(result) == -2) {
                $("#error_mreg").show();
                $("#error_mreg").html("手机号：" + $("#regMobile").val() + "已被占用，请重新输入");
                $("#showMobile img").css("display", "block");
                $("#showMobile img").attr("src", "/Images/error.png");
                addTrueOrFalse($("#regMobile"), false);
                return false;
            }

            if (Number(result) == -4) {
                $("#error_mreg").show();
                $("#error_mreg").html("手机号中包含敏感字符，请换一个手机号");
                $("#showMobile img").css("display", "block");
                $("#showMobile img").attr("src", "/Images/error.png");
                addTrueOrFalse($("#regMobile"), false);
                return false;
            }

            if (Number(result) == -5) {
                $("#error_mreg").show();
                $("#error_mreg").html("手机号：" + $("#regMobile").val() + "已绑定，请重新输入");
                $("#showMobile img").css("display", "block");
                $("#showMobile img").attr("src", "/Images/error.png");
                addTrueOrFalse($("#regMobile"), false);
                return false;
            }
        }
        else {
            $("#error_mreg").hide();
            $("#showMobile img").css("display", "block");
            $("#showMobile img").attr("src", "/Images/right.png");
            addTrueOrFalse($("#regMobile"), true);
        }
    }

    function callError() {
        alert("检验失败，请重试一次，谢谢。可能是网络延时原因。");
    }

    if (Number(result) < 0) {
        if (Number(result) == -2) {
            $("#error_mreg").show();
            $("#error_mreg").html("手机号：" + $("#regMobile").val() + "已被占用，请重新输入");
            $("#showMobile img").css("display", "block");
            $("#showMobile img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regMobile"), false);
            return false;
        }

        if (Number(result) == -4) {
            $("#error_mreg").show();
            $("#error_mreg").html("手机号中包含敏感字符，请换一个手机号");
            $("#showMobile img").css("display", "block");
            $("#showMobile img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regMobile"), false);
            return false;
        }

        if (Number(result) == -5) {
            $("#error_mreg").show();
            $("#error_mreg").html("手机号：" + $("#regMobile").val() + "已绑定，请重新输入");
            $("#showMobile img").css("display", "block");
            $("#showMobile img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regMobile"), false);
            return false;
        }
    }
    else {
        $("#error_mreg").hide();
        $("#showMobile img").css("display", "block");
        $("#showMobile img").attr("src", "/Images/right.png");
        addTrueOrFalse($("#regMobile"), true);
    }
    return true;
}

function isRegisterPwd1() {
    if ($("#regtype li:eq(0)").hasClass("curr")) {
        var PwdLen = $("#regPwd1").val().length;
        var PwdTwoLen = $("#regPwd2").val().length;
        if (PwdLen > 16) {
            $("#error_reg").show();
            $("#error_reg").html("密码长度请不要超过16个字符");
            $("#showPwd1 img").css("display", "block");
            $("#showPwd1 img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regPwd1"), false);
            return false;
        }
        if (PwdLen < 6) {
            $("#error_reg").show();
            $("#error_reg").html("密码长度在6-16个字符，区分大小写");
            $("#showPwd1 img").css("display", "block");
            $("#showPwd1 img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regPwd1"), false);
            return false;
        }

        var patrn = /\s/;
        if (patrn.test($("#regPwd1").val())) {
            $("#error_reg").show();
            $("#error_reg").html("密码请不要使用空格");
            $("#showPwd1 img").css("display", "block");
            $("#showPwd1 img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regPwd1"), false);
            return false;
        }
        if (PwdTwoLen != 0) {
            if (!isPwdSame()) {
                $("#error_reg").show();
                $("#error_reg").html("您两次输入的密码不一致");
                $("#showPwd2 img").css("display", "block");
                $("#showPwd2 img").attr("src", "/Images/error.png");
                addTrueOrFalse($("#regPwd2"), false);
                return false;
            }

        }

        if ($("#regPwd2").val().length != 0) {
            if (isPwdSame()) {
                $("#error_reg").hide();
                $("#error_reg").html("");
                $("#showPwd2 img").css("display", "block");
                $("#showPwd2 img").attr("src", "/Images/right.png");
                if ($("#regPwd1").hasClass("validRight")) {
                    addTrueOrFalse($("#regPwd2"), true);
                }
                else {
                    addTrueOrFalse($("#regPwd2"), false);
                }
            }
            else {
                $("#showPwd2 img").css("display", "block");
                $("#showPwd2 img").attr("src", "/Images/error.png");
                addTrueOrFalse($("#regPwd2"), false);
                return false;
            }
        }
        // 初始化
        var service = new PasswrodValid();
        // 访问公有成员变量
        service.control = "regPwd1";
        service.controlMarginLeft = "29%";
        service.msgMarginLeft = "28%";
        // 调用公有方法
        var result = service.retrieve();
        if (!result) {
            return false;
        }
        $("#error_reg").hide();
        $("#error_reg").html("");
        $("#showPwd1 img").css("display", "block");
        $("#showPwd1 img").attr("src", "/Images/right.png");
        return true;
    }
    else {
        var PwdLen = $("#regMpwd1").val().length;
        var PwdTwoLen = $("#regMpwd2").val().length;
        if ($("#regMobile").val().length <= 0) {
            addTrueOrFalse($("#regMobile"), false);
        }
        if ($("#regCode").val().length <= 0) {
            addTrueOrFalse($("#regCode"), false);
        }
        if (PwdLen > 16) {
            $("#error_mreg").show();
            $("#error_mreg").html("密码长度请不要超过16个字符");
            $("#showMpwd1 img").css("display", "block");
            $("#showMpwd1 img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regMpwd1"), false);
            addTrueOrFalse($("#regMpwd2"), false);
            return false;
        }
        if (PwdLen < 6) {
            $("#error_mreg").show();
            $("#error_mreg").html("密码长度在6-16个字符，区分大小写");
            $("#showMpwd1 img").css("display", "block");
            $("#showMpwd1 img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regMpwd1"), false);
            addTrueOrFalse($("#regMpwd2"), false);
            return false;
        }

        var patrn = /\s/;
        if (patrn.test($("#regMpwd1").val())) {
            $("#error_mreg").show();
            $("#error_mreg").html("密码请不要使用空格");
            $("#showMpwd1 img").css("display", "block");
            $("#showMpwd1 img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regMpwd1"), false);
            addTrueOrFalse($("#regMpwd2"), false);
            return false;
        }
        if (PwdTwoLen != 0) {
            if (!isPwdSame()) {
                $("#error_mreg").show();
                $("#error_mreg").html("您两次输入的密码不一致");
                $("#showMpwd2 img").css("display", "block");
                $("#showMpwd2 img").attr("src", "/Images/error.png");
                addTrueOrFalse($("#regMpwd2"), false);
                return false;
            }

        }

        if ($("#regMpwd2").val().length != 0) {
            if (isPwdSame()) {
                $("#error_reg").hide();
                $("#error_reg").html("");
                $("#showMpwd2 img").css("display", "block");
                $("#showMpwd2 img").attr("src", "/Images/right.png");
                if (!$("#regMpwd1").hasClass("validError")) {
                    addTrueOrFalse($("#regMpwd2"), true);
                }
            }
            else {
                $("#showMpwd2 img").css("display", "block");
                $("#showMpwd2 img").attr("src", "/Images/error.png");
                addTrueOrFalse($("#regMpwd2"), false);
                return false;
            }
        }


        // 初始化
        var service = new PasswrodValid();
        // 访问公有成员变量
        service.control = "regMpwd1";
        service.controlMarginLeft = "29%";
        service.msgMarginLeft = "28%";
        // 调用公有方法
        var result = service.retrieve();
        if (!result) {
            addTrueOrFalse($("#regMpwd1"), false);
            return false;
        }

        $("#error_mreg").hide();
        $("#error_mreg").html("");
        $("#showMpwd1 img").css("display", "block");
        $("#showMpwd1 img").attr("src", "/Images/right.png");
        addTrueOrFalse($("#regMpwd1"), true);
        addTrueOrFalse($("#regMpwd2"), true);
        return true;
    }
}

function isRegisterPwd2() {
    if ($("#regtype li:eq(0)").hasClass("curr")) {
        var PwdLen = $("#regPwd2").val().length;
        if (PwdLen > 16) {
            $("#error_reg").show();
            $("#error_reg").html("密码长度请不要超过16个字符");
            $("#showPwd2 img").css("display", "block");
            $("#showPwd2 img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regPwd2"), false);
            return false;
        }
        if (PwdLen < 6) {
            $("#error_reg").show();
            $("#error_reg").html("密码长度在6-16个字符，区分大小写");
            $("#showPwd2 img").css("display", "block");
            $("#showPwd2 img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regPwd2"), false);
            return false;
        }
        var patrn = /\s/;
        if (patrn.test($("#regPwd2").val())) {
            $("#error_reg").show();
            $("#error_reg").html("密码请不要使用空格");
            $("#showPwd2 img").css("display", "block");
            $("#showPwd2 img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regPwd2"), false);
            return false;
        }

        if (isPwdSame()) {
            $("#error_reg").hide();
            $("#error_reg").html("");
            $("#showPwd2 img").css("display", "block");
            $("#showPwd2 img").attr("src", "/Images/right.png");
            if ($("#regPwd1").hasClass("validRight")) {
                addTrueOrFalse($("#regPwd2"), true);
            }
            else {
                addTrueOrFalse($("#regPwd2"), false);
            }
        }
        else {
            $("#error_reg").show();
            $("#error_reg").html("您两次输入的密码不一致");
            $("#showPwd2 img").css("display", "block");
            $("#showPwd2 img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regPwd2"), false);
            return false;
        }

        $("#error_reg").hide();
        $("#error_reg").html("");
        $("#showPwd2 img").css("display", "block");
        $("#showPwd2 img").attr("src", "/Images/right.png");
        if ($("#regPwd1").hasClass("validRight")) {
            addTrueOrFalse($("#regPwd2"), true);
        }
        else {
            addTrueOrFalse($("#regPwd2"), false);
        }
        return true;
    }
    else {
        var PwdLen = $("#regMpwd2").val().length;
        if (PwdLen > 16) {
            $("#error_mreg").show();
            $("#error_mreg").html("密码长度请不要超过16个字符");
            $("#showMpwd2 img").css("display", "block");
            $("#showMpwd2 img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regMpwd2"), false);
            return false;
        }
        if (PwdLen < 6) {
            $("#error_mreg").show();
            $("#error_mreg").html("密码长度在6-16个字符，区分大小写");
            $("#showMpwd2 img").css("display", "block");
            $("#showMpwd2 img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regMpwd2"), false);
            return false;
        }
        var patrn = /\s/;
        if (patrn.test($("#regMpwd2").val())) {
            $("#error_mreg").show();
            $("#error_mreg").html("密码请不要使用空格");
            $("#showMpwd2 img").css("display", "block");
            $("#showMpwd2 img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regMpwd2"), false);
            return false;
        }

        if (isPwdSame()) {
            $("#error_mreg").hide();
            $("#error_mreg").html("");
            $("#showMpwd2 img").css("display", "block");
            $("#showMpwd2 img").attr("src", "/Images/right.png");
            if (!$("#regMpwd1").hasClass("validError")) {
                addTrueOrFalse($("#regMpwd2"), true);
            }
            else {
                addTrueOrFalse($("#regMpwd2"), false);
            }
        }
        else {
            $("#error_mreg").show();
            $("#error_mreg").html("您两次输入的密码不一致");
            $("#showMpwd2 img").css("display", "block");
            $("#showMpwd2 img").attr("src", "/Images/error.png");
            addTrueOrFalse($("#regMpwd2"), false);
            return false;
        }

        $("#error_mreg").hide();
        $("#error_mreg").html("");
        $("#showMpwd2 img").css("display", "block");
        $("#showMpwd2 img").attr("src", "/Images/right.png");
        return true;
    }
}

function isPwdSame() {
    if ($("#regtype li:eq(0)").hasClass("curr")) {
        var passwordOne = $("#regPwd1").val();
        var passwordTwo = $("#regPwd2").val();
        if (passwordOne == passwordTwo) {
            return true;
        } else {
            return false;
        }
    }
    else {
        var passwordOne = $("#regMpwd1").val();
        var passwordTwo = $("#regMpwd2").val();
        if (passwordOne == passwordTwo) {
            return true;
        } else {
            return false;
        }
    }
}

function checkReg() {
    var IsAgree = $("#ckbAgree").is(":checked");
    var reg = $("#hidReg").val();
    if (!RegsterUserName() | !isRegisterPwd1() | !isRegisterPwd2()) {
        $("#error_reg").show();
        if (parseInt(reg) == -2) $("#error_reg").html("用户名：" + $("#regName").val() + "已被占用，请重新输入");
        else if (parseInt(reg) == -3) $("#error_reg").html("用户名长度为5-16个字符，可使用数字、英文、中文");
        else if (parseInt(reg) == -4) $("#error_reg").html("用户名中包含敏感字符，请换一个用户名");
        else $("#error_reg").html("请检查用户名或密码输入是否正确");
        return false;
    }

    if (!RegsterQQ()) {
        $("#error_reg").show();
        $("#error_reg").html("请输入正确的QQ号码");
        return;
    }

    if ($("#tbCode2").val() == "") {
        $("#tipserr").text("请输入验证码！");
        return false;
    }


    var strBool = false;
    $.ajax({
        type: "POST",
        url: "/ajax/PassWordCode.ashx",
        data: "RegCode=" + $("#tbCode2").val(),
        timeout: 20000,
        cache: false,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data.message == "-1") {
                $("#error_tips").html("验证码已过期，请重新输入");
                $("#error_tips").show();
                $("#r_login_cimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
                $("#tbCode").val("");
                $("#tbCode").focus();
                return;
            }
            if (data.message == "-2") {
                $("#error_tips").html("验证码输入错误，请重新输入");
                $("#error_tips").show();
                $("#r_login_cimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
                $("#tbCode").val("");
                $("#tbCode").focus();
                return;
            }
            $("#error_tips").hide();
            strBool = true;
        }
    });


    if (!strBool) {
        $("#tipserr").text("验证码输入有误，请重新输入！");
        $("#r_login_cimg2").click();
        return false;
    }

    if (!IsAgree) {
        alert("必须同意注册协议才能注册");
        return false;
    }

    var message = Home_Room_UserControls_WebHead.Register($("#regName").val(), $("#regPwd1").val(), $("#regPwd2").val(), $("#QQ").val()).value;
    if (message == "-1") {
        alert("会员注册不成功");
    }
    else if (message == "-2") {
        alert("注册成功后登录失败");
    }
    else if (message == "-3") {
        alert("用户名中包含敏感字符，请换一个用户名");
    }
    else if (message == "-4") {
        alert("密码强度过弱");
    }
    else if (message == "-5") {
        alert("QQ号码不正确");
    }
    else {
        window.location.href = message;
    }
}
function RegsterQQ() {
    var k = $("#QQ").val();
    var reg = /^\d{4,12}$/;
    if (!reg.test(k)) {
        $("#error_reg").show();
        $("#error_reg").html("请输入正确的QQ号码");
        $("#showMobile img").css("display", "block");
        $("#showMobile img").attr("src", "/images/login/icon_error.png");
        addTrueOrFalse($("#QQ"), false);
        return false;
    }
    addTrueOrFalse($("#QQ"), true);
    $("#error_reg").html("");
    return true;
}
function RegsterQQMp() {
    var k = $("#MpQQ").val();
    var reg = /^\d{4,12}$/;
    if (!reg.test(k)) {
        $("#error_mreg").show();
        $("#error_mreg").html("请输入正确的QQ号码");
        $("#showMobile img").css("display", "block");
        $("#showMobile img").attr("src", "/images/login/icon_error.png");
        addTrueOrFalse($("#MpQQ"), false);
        return false;
    }
    addTrueOrFalse($("#MpQQ"), true);
    $("#error_mreg").html("");
    return true;
}
function checkMobileReg() {
    var IsAgree = $("#ckbMagree").is(":checked");
    if (!isRegisterPwd1() | !isRegisterPwd2()) {
        return false;
    }

    if (!RegsterQQMp()) {
        $("#error_mreg").show();
        $("#error_mreg").html("请输入正确的QQ号码");
        return;
    }

    if (!IsAgree) {
        alert("必须同意注册协议才能注册");
        return false;
    }

    var message = Home_Room_UserControls_WebHead.MobileRegister($("#regMobile").val(), $("#regMpwd1").val(), $("#regMpwd2").val(), $("#regCode").val(), $("#MpQQ").val()).value;
    if (message == "-1") {
        alert("会员注册不成功");
    }
    else if (message == "-2") {
        alert("注册成功后登录失败");
    }
    else if (message == "-3") {
        alert("验证码输入错误");
    }
    else if (message == "-4") {
        alert("密码强度过弱");
    }
    else if (message == "-5") {
        alert("QQ号码不正确");
    }
    else {
        window.location.href = message;
    }
}

function StringLength(str) {
    return str.replace(/[^\x00-\xff]/g, "**").length
}

function forget() {
    location.href = "/Home/Room/FoundPwd/Retrieve_password.aspx?name=" + $("#lu").val();
}

$.fn.numeral = function () {
    $(this).css("ime-mode", "disabled");
    this.bind("keypress", function (event) {
        var keyCode = 0;
        if (event.charCode != undefined) {
            keyCode = event.charCode;
        } else {
            keyCode = event.keyCode;
        }

        if (keyCode == 46) {
            if (this.value.indexOf(".") != -1) {
                return false;
            }
        } else {
            return (keyCode >= 46 && keyCode <= 57) || keyCode == 0;
        }
    });
    this.bind("blur", function () {
        if (this.value.lastIndexOf(".") == (this.value.length - 1)) {
            this.value = this.value.substr(0, this.value.length - 1);
        } else if (isNaN(this.value)) {
            this.value = "";
        }
    });
    this.bind("paste", function () {
        var s = clipboardData.getData('text');
        if (!/\D/.test(s));
        value = s.replace(/^0*/, '');
        return false;
    });
    this.bind("dragenter", function () {
        return false;
    });
    this.bind("keyup", function () {
        if (/(^0+)/.test(this.value)) this.value = this.value.replace(/^0*/, '');
    });
};

msg = function (message) {
    $("#info_dlg_content").html(message);
    tb_show("温馨提示", "#TB_inline?width=470&amp;inlineId=info_dlg", "");
    $("#info_dlg_ok").focus();
}

msg = function (message, isreload) {
    $("#info_dlg_content").html(message);
    tb_show("温馨提示", "#TB_inline?width=470&amp;inlineId=info_dlg", "", isreload);
    $("#info_dlg_ok").focus();
}

confirmTip = function (message, fn) {
    $("#confirm_dlg_content").html(message);
    tb_show("温馨提示", "#TB_inline?width=470&amp;inlineId=confirm_dlg", "");
    $("#confirm_dlg_yes").focus();
}

$("#info_dlg_ok").click(function () {
    tb_remove();
});



$(function () {

    /*悬浮快捷框*/
    $(".suspenbox").hover(function () {
        $(this).children("span").css('display', 'block');
    }, function () {
        $(this).children("span").css('display', 'none');
    });

    $("#online_advice").hover(function () {
        $(this).children('.onlineQQ').stop(true, false).slideDown(200);
    }, function () {
        $(this).children('.onlineQQ').stop(true, false).slideUp(200);
    });

    $("#QRcode").hover(function () {
        $(this).children('.QRcodebig').stop(true, false).show(200);
    }, function () {
        $(this).children('.QRcodebig').stop(true, false).hide(200);
    });

    $(window).on('scroll', function () {
        var st = $(document).scrollTop();
        if (st > 200) {
            $('#gototop').fadeIn(200);
        } else {
            $('#gototop').fadeOut(200);
        }
    })

    $('#gototop').bind('click', function () {
        $('html,body').animate({ 'scrollTop': 0 }, 500);
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
function f_ajaxPost_HeadUse(handlerUrl, post_data, loadingDiv, successCallback, errCallback, completeCallback) {
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

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserReg.aspx.cs" Inherits="UserReg" %>

<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>会员注册 - <%=_Site.Name %></title>
    <meta name="description" content="会员注册，<%=_Site.Name %>彩票网是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站，包含双色球、时时乐、时时彩、足彩等众多彩种的实时开奖信息、图表走势、分析预测等。" />
    <meta name="keywords" content="会员注册，双色球开奖，双色球走势图，3d走势图，福彩3d，时时彩" />
    <link rel="stylesheet" type="text/css" href="Style/global.css" />
    <link href="Style/login.css" rel="stylesheet" />
    <link href="JScript/PaswordValid/css/style.css" rel="stylesheet" />
    <style type="text/css">
        #btnLogin {
            display: none;
        }

        #showUserName img, #showPassWord img, #showPassWord2 img, #showMobile img, #showMpwd1 img, #showMpwd2 img {
            display: none;
        }

        .loginwrapboxInput {
            border: 1px #ccc solid;
            height: 21px;
            padding: 2px;
            width: 200px;
        }

        .spanCss {
            color: #666;
            width: 100px;
            height: 49px;
            line-height: 49px;
            font-size: 14px;
            text-align: right;
            border: 0px solid #ccc;
            display: inline-block;
        }

        .loginwrapbox li {
        }

        #refr {
            position: relative;
            top: -37px;
            top: -33px\9;
            top: -36px\9 \0;
            *top: -42px;
            left: 242px;
        }
    </style>
    <link rel="shortcut icon" href="favicon.ico" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <div id="LogoImage" runat="server">
            </div>
            <div class="regbut">
                <a href="UserLogin.aspx">已拥有帐号，现在登录！</a>
            </div>
        </div>
        <div class="content">
            <div class="loginhead">
            </div>
            <div class="logincon">
                <ul id="tabli">
                    <li class="curr"><a href="javascript:void(0)">用户名注册</a></li>
                    <li><a href="javascript:void(0)">手机号码注册</a></li>
                </ul>
                <div class="loginwrapbox hide" style="display: block">
                    <div class="promptinfo" id="error_reg">
                    </div>
                    <div style="border-radius: 3px; border: 1px solid #ced1d0; background: #fff; width: 363px; margin-left: auto; margin-right: auto; padding: 20px 0 20px 0;">
                        <ul>
                            <li><span class="spanCss">用户名：</span><span><input type="text" id="regName" class="loginwrapboxInput" /></span></li>
                            <li><span class="spanCss">设置密码：</span><span><input type="password" id="regPwd1" class="loginwrapboxInput" /></span></li>
                            <li><span class="spanCss">确认密码：</span><span><input type="password" id="regPwd2" class="loginwrapboxInput" /></span></li>
                            <li runat="server" id="liCpsCode"><span class="spanCss">推广编码：</span><span><input type="text" id="regCpsCode" class="loginwrapboxInput" runat="server" /></span></li>
                            <li><span class="spanCss">QQ：</span><span><input type="text" id="QQ" class="loginwrapboxInput" onblur="RegsterQQ()" /></span></li>
                            <li><span class="spanCss">验证码：</span><span><input type="text" id="tbCode" runat="server" class="loginwrapboxInput" /><a href="javascript:void(0)" id="refr"><img src="regcode.aspx?rnd=" id="r_login_cimg" alt="" style="height: 25px;" /></a></span></li>
                        </ul>
                    </div>
                    <div class="terms">
                        <label>
                            <input type="checkbox" checked="checked" id="ckbAgree" />我已满18周岁，阅读并同意<a href="/Home/Web/UserRegAgree.aspx" target="_blank">《用户注册协议》</a></label>
                    </div>
                    <div class="loginbtn">
                        <input type="button" value="立即注册" onclick="checkReg()" />
                    </div>
                </div>
                <div class="loginwrapbox hide">
                    <div class="promptinfo" id="error_mreg">
                    </div>
                    <div style="border-radius: 3px; border: 1px solid #ced1d0; background: #fff; width: 363px; margin-left: auto; margin-right: auto; padding: 20px 0 20px 0;">
                        <ul>
                            <li><span class="spanCss">手机号码：</span><span><input type="text" id="regMobile" class="loginwrapboxInput" /></span></li>
                            <li><span class="spanCss">设置密码：</span><span><input type="password" id="regMpwd1" class="loginwrapboxInput" /></span></li>
                            <li><span class="spanCss">确认密码：</span><span><input type="password" id="regMpwd2" class="loginwrapboxInput" /></span></li>
                            <li  runat="server" id="limCpsCode"><span class="spanCss">推广编码：</span><span><input type="text" id="regMCpsCode" class="loginwrapboxInput" runat="server" /></span></li>
                            <li><span class="spanCss">QQ：</span><span><input type="text" id="MpQQ" class="loginwrapboxInput" /></span></li>
                            <li><span class="spanCss">验证码：</span><span><input type="text" id="regMCode" class="loginwrapboxInput" /></span>
                                <div class="captchamob">
                                    <input type="button" id="getCode" onclick="GetMobileCode()" value="获取验证码" />
                                    <span id="sendOK" style="display: none;"><em id="time_tgy">60</em> 秒后重发</span>
                                </div>
                            </li>
                        </ul>
                    </div>
                    <div class="terms">
                        <label>
                            <input type="checkbox" checked="checked" id="ckbMagree" />我已满18周岁，阅读并同意<a href="/Home/Web/UserRegAgree.aspx" target="_blank">《用户注册协议》</a></label>
                    </div>
                    <div class="loginbtn">
                        <input type="button" value="立即注册" onclick="checkMobileReg()" />
                    </div>
                </div>
            </div>
            <div class="loginfoot">
            </div>
        </div>
        <div class="footer">
            <p>
                版权所有：<%= _Site.Company %> ICP备案：<a href="http://www.miibeian.gov.cn/"><%= _Site.ICPCert %></a>
            </p>
            <p>
                公司地址：<%= _Site.Address %> 客服热线：<%= _Site.ServiceTelephone %> 邮政编码：<%= _Site.PostCode %>
            </p>
            <p>
                本站提示：彩票有风险，投注需谨慎 不向未满18周岁的青少年出售彩票！
            </p>
        </div>
        <input type="hidden" value="" id="hidReg" />
    </form>
</body>
</html>
<script src="/JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
<script src="JScript/PaswordValid/PasswordValid.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">

    $(function () {
        $("#tabli").children('li').bind("click", function () {
            $("#tabli").children('li').removeClass('curr');
            $(this).addClass('curr');
            $(".logincon").find(".loginwrapbox").css('display', 'none');
            $(".logincon").find(".loginwrapbox").eq($(this).index()).css('display', 'block');
        })

        $("#regName").blur(function () {
            RegsterUserName();
        });
        $("#regPwd1").keyup(function () {
            // 初始化
            var service = new PasswrodValid();
            // 访问公有成员变量
            service.control = "regPwd1";
            service.controlMarginLeft = "28%";
            service.msgMarginLeft = "28%";
            // 调用公有方法
            service.retrieve();
        });
        $("#regMpwd1").keyup(function () {
            // 初始化
            var service = new PasswrodValid();
            // 访问公有成员变量
            service.control = "regMpwd1";
            service.controlMarginLeft = "28%";
            service.msgMarginLeft = "28%";
            // 调用公有方法
            service.retrieve();
        });
    });

    function RegsterUserName() {
        var nameLen = StringLength($("#regName").val());
        if (nameLen > 16 || nameLen < 5) {
            $("#error_reg").html("用户名长度为5-16个字符，可使用数字、英文、中文");
            $("#showUserName img").css("display", "block");
            $("#showUserName img").attr("src", "/images/login/icon_error.png");
            addTrueOrFalse($("#regName"), false);
            return false;
        }
        var patrn = /[^0-9A-Za-z\u4e00-\u9fa5]/;
        if (patrn.test($("#regName").val())) {
            $("#error_reg").html("请使用汉字，数字或字母");
            $("#showUserName img").css("display", "block");
            $("#showUserName img").attr("src", "/images/login/icon_error.png");
            addTrueOrFalse($("#regName"), false);
            return false;
        }
        else {
            return checkUserName();
        }
    }
    function RegsterQQ() {
        var k = $("#QQ").val();
        var reg = /^\d{4,12}$/;
        if (!reg.test(k)) {
            $("#error_reg").html("请输入你正确的QQ号");
            $("#showUserName img").css("display", "block");
            $("#showUserName img").attr("src", "/images/login/icon_error.png");
            addTrueOrFalse($("#QQ"), false);
            return false;
        }
        addTrueOrFalse($("#QQ"), true);
        $("#error_reg").html("");
        return true;
    }
    function checkUserName() {
        if ($("#regName").val() == "") {
            $("#error_reg").html("用户名不能为空");
            $("#showUserName img").css("display", "block");
            $("#showUserName img").attr("src", "/images/login/icon_error.png");
            addTrueOrFalse($("#regName"), false);
        }

        var result = 0;
        $.ajax({
            type: "POST",
            url: "../ajax/CheckName.ashx", //发送请求的地址
            timeout: 20000,
            cache: false,
            async: false,
            data: "UserName=" + $("#regName").val(),
            dataType: "json",
            error: callError,
            success: callSuccess
        });

        function callSuccess(json, textStatus) {
            result = json.message
        }

        function callError() {
            alert("检验失败，请重试一次，谢谢。可能是网络延时原因。");
        }

        if (Number(result) < 0) {
            if (Number(result) == -2) {
                $("#error_reg").html("用户名：" + $("#regName").val() + "已被占用，请重新输入");
                $("#hidReg").val(-2);
                $("#showUserName img").css("display", "block");
                $("#showUserName img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regName"), false);
                return false;
            }

            if (Number(result) == -3) {
                $("#error_reg").html("用户名长度为6-16个字符，可使用数字、英文、中文");
                $("#hidReg").val(-3);
                $("#showUserName img").css("display", "block");
                $("#showUserName img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regName"), false);
                return false;
            }

            if (Number(result) == -4) {
                $("#error_reg").html("用户名中包含敏感字符，请换一个用户名");
                $("#hidReg").val(-4);
                $("#showUserName img").css("display", "block");
                $("#showUserName img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regName"), false);
                return false;
            }
        }
        else {
            $("#hidReg").val(0);
            $("#error_reg").html("");
            $("#showUserName img").css("display", "block");
            $("#showUserName img").attr("src", "/images/login/icon_right.png");
            addTrueOrFalse($("#regName"), true);
        }
        return true;
    }

    function StringLength(str) {
        return str.replace(/[^\x00-\xff]/g, "**").length
    }

    $("#regPwd1,#regMpwd1").blur(function () {
        isRegisterPwd1();
    });

    $("#regPwd2,#regMpwd2").blur(function () {
        isRegisterPwd2();
    });

    function isRegisterPwd1() {
        if ($("#tabli li:eq(0)").hasClass("curr")) {
            // 初始化
            var service = new PasswrodValid();
            // 访问公有成员变量
            service.control = "regPwd1";
            service.controlMarginLeft = "28%";
            service.msgMarginLeft = "28%";
            // 调用公有方法
            service.retrieve();
            var PwdLen = $("#regPwd1").val().length;
            var PwdTwoLen = $("#regPwd2").val().length;
            if (PwdLen > 16) {
                $("#error_reg").html("密码长度请不要超过16个字符");
                $("#showPassWord img").css("display", "block");
                $("#showPassWord img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regPwd1"), false);
                return false;
            }
            if (PwdLen < 6) {
                $("#error_reg").html("密码长度在6-16个字符，区分大小写");
                $("#showPassWord img").css("display", "block");
                $("#showPassWord img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regPwd1"), false);
                return false;
            }

            var patrn = /\s/;
            if (patrn.test($("#regPwd1").val())) {
                $("#error_reg").html("密码请不要使用空格");
                $("#showPassWord img").css("display", "block");
                $("#showPassWord img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regPwd1"), false);
                return false;
            }
            if (PwdTwoLen != 0) {
                if (!isPwdSame()) {
                    $("#error_reg").html("您两次输入的密码不一致");
                    $("#showPassWord2 img").css("display", "block");
                    $("#showPassWord2 img").attr("src", "/images/login/icon_error.png");
                    addTrueOrFalse($("#regPwd2"), false);
                    return false;
                }

            }

            if ($("#regPwd2").val().length != 0) {
                if (isPwdSame()) {
                    $("#error_reg").html("");
                    $("#showPassWord2 img").css("display", "block");
                    $("#showPassWord2 img").attr("src", "/images/login/icon_right.png");
                    if (!$("#regPwd1").hasClass("validError")) {
                        addTrueOrFalse($("#regPwd2"), true);
                    } else {
                        addTrueOrFalse($("#regPwd2"), false);
                    }
                }
                else {
                    $("#showPassWord2 img").css("display", "block");
                    $("#showPassWord2 img").attr("src", "/images/login/icon_error.png");
                    addTrueOrFalse($("#regPwd2"), false);
                    return false;
                }
            }

            $("#error_reg").html("");
            $("#showPassWord img").css("display", "block");
            $("#showPassWord img").attr("src", "/images/login/icon_right.png");
            return true;
        }
        else {
            // 初始化
            var service = new PasswrodValid();
            // 访问公有成员变量
            service.control = "regMpwd1";
            service.controlMarginLeft = "28%";
            service.msgMarginLeft = "28%";
            // 调用公有方法
            service.retrieve();
            var PwdLen = $("#regMpwd1").val().length;
            var PwdTwoLen = $("#regMpwd2").val().length;
            if (PwdLen > 16) {
                $("#error_mreg").html("密码长度请不要超过16个字符");
                $("#showMpwd1 img").css("display", "block");
                $("#showMpwd1 img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regMpwd1"), false);
                return false;
            }
            if (PwdLen < 6) {
                $("#error_mreg").html("密码长度在6-16个字符，区分大小写");
                $("#showMpwd1 img").css("display", "block");
                $("#showMpwd1 img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regMpwd1"), false);
                return false;
            }

            var patrn = /\s/;
            if (patrn.test($("#regMpwd1").val())) {
                $("#error_mreg").html("密码请不要使用空格");
                $("#showMpwd1 img").css("display", "block");
                $("#showMpwd1 img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regMpwd1"), false);
                return false;
            }
            //if (PwdTwoLen != 0) {
            //    if (!isPwdSame()) {
            //        $("#error_mreg").html("您两次输入的密码不一致");
            //        $("#showMpwd2 img").css("display", "block");
            //        $("#showMpwd2 img").attr("src", "/images/login/icon_error.png");
            //        addTrueOrFalse($("#regMpwd1"), false);
            //        return false;
            //    }

            //}

            //if ($("#regMpwd2").val().length != 0) {
            //    if (isPwdSame()) {
            //        $("#error_reg").html("");
            //        $("#showMpwd2 img").css("display", "block");
            //        $("#showMpwd2 img").attr("src", "/images/login/icon_right.png");
            //        addTrueOrFalse($("#regMpwd1"), true);
            //    }
            //    else {
            //        $("#showMpwd2 img").css("display", "block");
            //        $("#showMpwd2 img").attr("src", "/images/login/icon_error.png");
            //        addTrueOrFalse($("#regMpwd1"), false);
            //        return false;
            //    }
            //}

            //if ($("#regMpwd2").val().length != 0) {
            //    if (isPwdSame()) {
            //        $("#error_mreg").html("");
            //        $("#showMpwd2 img").css("display", "block");
            //        $("#showMpwd2 img").attr("src", "/images/login/icon_right.png");
            //        if (!$("#regMpwd1").hasClass("validError")) {
            //            addTrueOrFalse($("#regMpwd2"), true);
            //        }
            //        else {
            //            addTrueOrFalse($("#regMpwd2"), false);
            //        }
            //    }
            //    else {
            //        $("#showMPwd2 img").css("display", "block");
            //        $("#showMPwd2 img").attr("src", "/images/login/icon_error.png");
            //        addTrueOrFalse($("#regMpwd2"), false);
            //        return false;
            //    }
            //}

            $("#error_mreg").html("");
            $("#showMpwd1 img").css("display", "block");
            $("#showMpwd1 img").attr("src", "/images/login/icon_right.png");
            return true;
        }
    }

    function isRegisterPwd2() {
        if ($("#tabli li:eq(0)").hasClass("curr")) {
            var PwdLen = $("#regPwd2").val().length;
            if (PwdLen > 16) {
                $("#error_reg").html("密码长度请不要超过16个字符");
                $("#showPassWord2 img").css("display", "block");
                $("#showPassWord2 img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regPwd2"), false);
                return false;
            }
            if (PwdLen < 6) {
                $("#error_reg").html("密码长度在6-16个字符，区分大小写");
                $("#showPassWord2 img").css("display", "block");
                $("#showPassWord2 img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regPwd2"), false);
                return false;
            }
            var patrn = /\s/;
            if (patrn.test($("#regPwd2").val())) {
                $("#error_reg").html("密码请不要使用空格");
                $("#showPassWord2 img").css("display", "block");
                $("#showPassWord2 img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regPwd2"), false);
                return false;
            }

            if (isPwdSame()) {
                $("#error_reg").html("");
                $("#showPassWord2 img").css("display", "block");
                $("#showPassWord2 img").attr("src", "/images/login/icon_right.png");
                if (!$("#regPwd1").hasClass("validError")) {
                    addTrueOrFalse($("#regPwd2"), true);
                } else {
                    addTrueOrFalse($("#regPwd2"), false);
                }
            }
            else {
                $("#error_reg").html("您两次输入的密码不一致");
                $("#showPassWord2 img").css("display", "block");
                $("#showPassWord2 img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regPwd2"), false);
                return false;
            }

            $("#error_reg").html("");
            $("#showPassWord2 img").css("display", "block");
            $("#showPassWord2 img").attr("src", "/images/login/icon_right.png");
            $("#showPassWord img").attr("src", "/images/login/icon_right.png");
            return true;
        }
        else {
            var PwdLen = $("#regMpwd2").val().length;
            if (PwdLen > 16) {
                $("#error_mreg").html("密码长度请不要超过16个字符");
                $("#showMpwd2 img").css("display", "block");
                $("#showMpwd2 img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regMpwd2"), false);
                return false;
            }
            if (PwdLen < 6) {
                $("#error_mreg").html("密码长度在6-16个字符，区分大小写");
                $("#showMpwd2 img").css("display", "block");
                $("#showMpwd2 img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regMpwd2"), false);
                return false;
            }
            var patrn = /\s/;
            if (patrn.test($("#regMpwd2").val())) {
                $("#error_mreg").html("密码请不要使用空格");
                $("#showMpwd2 img").css("display", "block");
                $("#showMpwd2 img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regMpwd2"), false);
                return false;
            }

            if (isPwdSame()) {
                $("#error_mreg").html("");
                $("#showMpwd2 img").css("display", "block");
                $("#showMpwd2 img").attr("src", "/images/login/icon_right.png");
                if (!$("#regMpwd1").hasClass("validError")) {
                    addTrueOrFalse($("#regMpwd2"), true);
                }
                else {
                    addTrueOrFalse($("#regMpwd2"), false);
                }
            }
            else {
                $("#error_mreg").html("您两次输入的密码不一致");
                $("#showMpwd2 img").css("display", "block");
                $("#showMpwd2 img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regMpwd2"), false);
                return false;
            }

            $("#error_mreg").html("");
            $("#showMpwd2 img").css("display", "block");
            $("#showMpwd2 img").attr("src", "/images/login/icon_right.png");
            $("#showMpwd img").attr("src", "/images/login/icon_right.png");
            return true;
        }
    }

    function isPwdSame() {
        if ($("#tabli li:eq(0)").hasClass("curr")) {
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

    $("#regMobile").blur(function () {
        if ($("#regMobile").val().length != 11) {
            $("#error_mreg").html("手机号码输入错误，应为11位数");
            $("#showMobile img").css("display", "block");
            $("#showMobile img").attr("src", "/images/login/icon_error.png");
            addTrueOrFalse($("#regMobile"), false);
            return;
        }
        checkMobileName();
    });
    $("#MpQQ").blur(function () {
        return RegsterQQMp();
    });
    function RegsterQQMp() {
        var k = $("#MpQQ").val();
        var reg = /^\d{4,12}$/;
        if (!reg.test(k)) {
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
    function checkMobileName() {
        if ($("#regMobile").val() == "") {
            $("#error_mreg").html("手机号不能为空");
            $("#showMobile img").css("display", "block");
            $("#showMobile img").attr("src", "/images/login/icon_error.png");
            addTrueOrFalse($("#regMobile"), false);
            return false;
        }

        var result = 0;
        $.ajax({
            type: "POST",
            url: "../ajax/CheckName.ashx", //发送请求的地址
            timeout: 20000,
            cache: false,
            async: false,
            data: "UserName=" + $("#regMobile").val() + "&Mobile=" + $("#regMobile").val(),
            dataType: "json",
            error: callError,
            success: callSuccess
        });

        function callSuccess(json, textStatus) {
            result = json.message
        }

        function callError() {
            alert("检验失败，请重试一次，谢谢。可能是网络延时原因。");
        }

        if (Number(result) < 0) {
            if (Number(result) == -2) {
                $("#error_mreg").html("手机号：" + $("#regMobile").val() + "已被占用，请重新输入");
                $("#showMobile img").css("display", "block");
                $("#showMobile img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regMobile"), false);
                return false;
            }

            if (Number(result) == -4) {
                $("#error_mreg").html("手机号中包含敏感字符，请换一个手机号");
                $("#showMobile img").css("display", "block");
                $("#showMobile img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regMobile"), false);
                return false;
            }

            if (Number(result) == -5) {
                $("#error_mreg").html("手机号：" + $("#regMobile").val() + "已绑定，请重新输入");
                $("#showMobile img").css("display", "block");
                $("#showMobile img").attr("src", "/images/login/icon_error.png");
                addTrueOrFalse($("#regMobile"), false);
                return false;
            }
        }
        else {
            $("#error_mreg").html("");
            $("#showMobile img").css("display", "block");
            $("#showMobile img").attr("src", "/images/login/icon_right.png");
            addTrueOrFalse($("#regMobile"), true);
        }
        return true;
    }

    $("#refr").click(function () {
        $("#r_login_cimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
    });

    function checkReg() {
        var IsAgree = $("#ckbAgree").is(":checked");
        var reg = $("#hidReg").val();
        if (!isRegisterPwd2() | !isRegisterPwd1() | !RegsterUserName()) {
            if (parseInt(reg) == -2) $("#error_reg").html("用户名：" + $("#regName").val() + "已被占用，请重新输入");
            else if (parseInt(reg) == -3) $("#error_reg").html("用户名长度为6-16个字符，可使用数字、英文、中文");
            else if (parseInt(reg) == -4) $("#error_reg").html("用户名中包含敏感字符，请换一个用户名");
            else $("#error_reg").html("请检查用户名或密码输入是否正确");
            return false;
        }

        if (!IsAgree) {
            alert("必须同意注册协议才能注册");
            return false;
        }
        if (!RegsterQQ()) {
            $("#error_reg").html("请输入正确的QQ号码");
            return;
        }
        if ($("#tbCode").val() == "") {
            $("#error_reg").html("请输入验证码");
            return;
        }
        // 初始化
        var service = new PasswrodValid();
        // 访问公有成员变量
        service.control = "regPwd1";
        service.controlMarginLeft = "28%";
        service.msgMarginLeft = "28%";
        // 调用公有方法
        var result = service.retrieve();
        if (!result) {
            $("#regPwd1").focus();
            return false;
        }
        $.ajax({
            type: "POST",
            url: "/ajax/PassWordCode.ashx",
            data: "RegCode=" + $("#tbCode").val(),
            timeout: 20000,
            cache: false,
            async: false,
            dataType: "json",
            success: function (data) {
                if (data.message == "-1") {
                    $("#error_reg").html("验证码已过期，请重新输入");
                    $("#r_login_cimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
                    $("#tbCode").val("");
                    $("#tbCode").focus();
                }
                if (data.message == "-2") {
                    $("#error_reg").html("验证码输入错误，请重新输入");
                    $("#r_login_cimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
                    $("#tbCode").val("");
                    $("#tbCode").focus();
                }
                if (data.message == "0") {
                    var message = UserReg.Register($("#regName").val(), $("#regPwd1").val(), $("#regPwd2").val(), $("#QQ").val()).value;
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
                        message = "/Home/Room/UserRegSuccess.aspx";
                        window.location.href = message;
                    }
                }
            }
        });
    }

    function checkMobileReg() {
        var IsAgree = $("#ckbMagree").is(":checked");
        if (!isRegisterPwd2() | !isRegisterPwd1() | !checkMobileName()) {
            return false;
        }

        if (!IsAgree) {
            alert("必须同意注册协议才能注册");
            return false;
        }
        if (!RegsterQQMp()) {
            alert("请输入正确的QQ号码");
            return false;
        }
        if ($("#regMCode").val() == "") {
            $("#error_mreg").html("请输入验证码");
            return;
        }
        // 初始化
        var service = new PasswrodValid();
        // 访问公有成员变量
        service.control = "regMpwd1";
        service.controlMarginLeft = "28%";
        service.msgMarginLeft = "28%";
        // 调用公有方法
        var result = service.retrieve();
        if (!result) {
            $("#regMpwd1").focus();
            return false;
        }
        var message = UserReg.MobileRegister($("#regMobile").val(), $("#regMpwd1").val(), $("#regMpwd2").val(), $("#regMCode").val(), $("#MpQQ").val()).value;
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

    function GetMobileCode() {
        var code = UserReg.CheckMobile($("#regMobile").val()).value;
        if (code == -1) {
            alert("您输入的手机号码有误，请重新输入");
        }
        else if (code == -2) {
            alert("请稍后再重新发送！");
        }
        else {
            $("#getCode").hide();
            $("#sendOK").show();

            timeInterval = setInterval("time()", 1000);
        }
    }

    var index = 59;
    var timeInterval = null;
    function time() {
        $("#time_tgy").text(index);
        if (index == 0) {
            clearInterval(timeInterval);
            timeInterval = null;
            index = 59;
            $("#getCode").show();
            $("#sendOK").hide();
            $("#time_tgy").text(index);
        }
        index--;
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
</script>

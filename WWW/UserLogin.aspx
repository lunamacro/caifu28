<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserLogin.aspx.cs" Inherits="UserLogin" %>

<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    
    <title>会员登录 </title>
    
    <link href="Style/login.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #btnLogin{ display:none; }
        #showUserName img,#showPassWord img{ display:none; }
    </style>
    <link rel="shortcut icon" href="favicon.ico" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="header">
        <div class="logo" id="LogoImage" runat="server">
            </div>
    </div>
    <div class="content">
        <div class="loginhead">
        </div>
        <div class="logincon">
            <div class="loginwrapbox">
                <div class="promptinfo" id="error_tips" runat="server">
                    </div>
                <div class="logininpitbox">
                    <div class="loginname">
                        <label>
                            <span>用户名：</span><input type="text" id="tbUserName" autocomplete="off" placeholder="用户名/手机号" runat="server" /></label><i id="showUserName"><img src="../images/login/icon_error.png"
                                alt="" /></i></div>
                    <div class="loginpassword">
                        <label>
                            <span>密&nbsp;码：</span><input type="password" id="tbPassWord" autocomplete="off" placeholder="密码" runat="server" /></label><i id="showPassWord"><img src="../images/login/icon_right.png"
                                alt="" /></i></div>
                    <div class="captcha">
                        <label>
                            <span>验证码：</span><input type="text" id="tbCode" /></label>
                        <div class="captchaimg">
                            <img src="regcode.aspx?rnd=" id="r_login_cimg" alt="" /><a href="javascript:void(0)" id="refr"></a></div>
                    </div>
                </div>
                <div class="forgetword">
                   </div>
                <div class="loginbtn">
                    <input type="button" id="btnOK" value="立即登录" onclick="Checkcode()" />
                    <asp:Button ID="btnLogin" runat="server" Text="立即登录" onclick="btnLogin_Click" 
                        UseSubmitBehavior="False" /></div>
            </div>
        </div>
        <div class="loginfoot">
        </div>
    </div>
    <div class="footer">
       
    </div>
    <input type="hidden" id="hLogin" value="0" runat="server" />
    <script src="JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
    </form>
</body>
</html>
<script type="text/javascript" language="javascript">

    if ($("#tbUserName").val() != "") {
        $("#showUserName img").css("display", "block");
        $("#showUserName img").attr("src", "/Images/login/icon_right.png");
    }
    if ($("#tbPassWord").val() != "") {
        $("#showPassWord img").css("display", "block");
        $("#showPassWord img").attr("src", "/Images/login/icon_right.png");
    }
    $("#tbUserName").focus();

    document.onkeydown = function (e) {
        if (e == null) { //ie
            keycode = event.keyCode;
        } else { // mozilla
            keycode = e.which;
        }
        if (keycode == 13) {
            $("#btnOK").click();
        }
    }

    $("#tbUserName").blur(function () {
        if ($(this).val() == "") {
            $("#error_tips").html("用户名不能为空");
            $("#showUserName img").css("display", "block");
            $("#showUserName img").attr("src", "/Images/login/icon_error.png");
            return;
        }
        $.ajax({
            type: "POST",
            url: "/ajax/CheckUserName.aspx",
            data: "UserName=" + $("#tbUserName").val() + "&PassWord=" + $("#tbPassWord").val(),
            timeout: 20000,
            cache: false,
            async: false,
            dataType: "json",
            error: callErrorfloginbtn,
            success: callSuccessfloginbtn
        });
    });

    function callSuccessfloginbtn(json, textStatus) {
        $("#error_tips").html("");
        $("#showUserName img").css("display", "block");
        $("#showUserName img").attr("src", "/Images/login/icon_right.png");
        if (isNaN(json.message)) {
            $("#error_tips").html(json.message);
            $("#showUserName img").css("display", "block");
            $("#showUserName img").attr("src", "/Images/login/icon_error.png");
            return;
        }
    }

    function callErrorfloginbtn() {
        msg("登录异常，请重试一次，谢谢。可能是网络延时原因!");
    }

    $("#refr").click(function () {
        $("#r_login_cimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
    });

    $("#tbPassWord").blur(function () {
        if ($(this).val() == "") {
            $("#error_tips").html("密码不能为空");
            $("#showPassWord img").css("display", "block");
            $("#showPassWord img").attr("src", "/Images/login/icon_error.png");
        }
        else if ($(this).val().length < 6) {
            $("#error_tips").html("密码不能小于6位数");
            $("#showPassWord img").css("display", "block");
            $("#showPassWord img").attr("src", "/Images/login/icon_error.png");
        }
        else {
            $("#error_tips").html("");
            $("#showPassWord img").css("display", "block");
            $("#showPassWord img").attr("src", "/Images/login/icon_right.png");
        }
    });

    function Checkcode() {
        if ($("#tbUserName").val() != "") {
            $("#showUserName img").css("display", "block");
            $("#showUserName img").attr("src", "/Images/login/icon_right.png");
        }
        if ($("#tbPassWord").val() != "") {
            $("#showPassWord img").css("display", "block");
            $("#showPassWord img").attr("src", "/Images/login/icon_right.png");
        }
        if ($("#tbCode").val() == "") {
            $("#error_tips").html("请输入验证码");
            return;
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
                    $("#error_tips").html("验证码已过期，请重新输入");
                    $("#r_login_cimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
                    $("#tbCode").val("");
                    $("#tbCode").focus();
                }
                if (data.message == "-2") {
                    $("#error_tips").html("验证码输入错误，请重新输入");
                    $("#r_login_cimg").attr('src', "/regcode.aspx?rnd=" + Math.random());
                    $("#tbCode").val("");
                    $("#tbCode").focus();
                }
                if (data.message == "0") {
                    $("#btnLogin").click();
                }
            }
        });
    }

    function forget() {
        
    }

</script>

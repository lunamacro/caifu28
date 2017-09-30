<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MobileValided.aspx.cs" Inherits="CPS_MobileValided" %>

<%@ Register Src="userControls/IndexHeader.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="userControls/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-推广联盟-验证手机号码</title>
    <meta name="description" content="<%=_Site.Name %>-推广联盟-验证手机号码" />
    <meta name="keywords" content="<%=_Site.Name %>-推广联盟-验证手机号码" />
    <link type="text/css" href="css/common.css" rel="stylesheet" />
    <link type="text/css" href="../Style/user.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Header ID="Header1" runat="server" />
    <div class="user_banner">
        <h3>
            手机验证<span>Mobile Bind</span></h3>
    </div>
    <div class="content">
        <div class="inside_bg">
            <div class="insicon">
                <div class="commain">
                    <div class="commain" style="border-top: 1px solid #E1E2E2;">
                        <div class="mainbor">
                            <div class="yzwrap" style="padding-left: 0px">
                                <div class="yzinpu">
                                    <label>
                                        <span>手机号码:</span><asp:TextBox runat="server" ID="txtMobile" MaxLength="18" Style='padding-left: 10px;
                                            font-size: 13px;' placeholder="请输入手机号码" /></label><div class="hqyzm">
                                                <a href="javascript:;" id="btnSendVerifyCode">获取验证码</a><em id="time"></em></div>
                                </div>
                                <div class="promptxx">
                                    请输入您使用的手机号码</div>
                                <div class="yzinpu">
                                    <label>
                                        <span>验证码:</span>
                                        <asp:TextBox runat="server" ID="txtCode" MaxLength="6" Style='padding-left: 10px;
                                            font-size: 13px;' placeholder="请输入验证码" /></label>
                                </div>
                                <div class="promptxx">
                                    请输入手机收到的短信验证码，验证码有效期为10分钟。</div>
                                <div class="yzbtn">
                                    <div class="qrbtn">
                                        <asp:Button runat="server" ID="btnMobileBind" Text="立即绑定" Style="margin-left: 40px;"
                                            CssClass="reg" OnClick="btnMobileBind_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <uc2:Footer ID="Footer2" runat="server" />
    </form>
</body>


<script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>
<script type="text/javascript">
    var index = 59;
    var TimeInterval = null;
    window.onload = function () {
        $("#btnSendVerifyCode").bind("click", function () {

            var mobile = $("#txtMobile").val();
            if ("" == mobile) {
                alert("请输入手机号码");
                return;
            }
            $.ajax({
                type: "post",
                url: "/ajax/SendVerifyCode.ashx",
                data: { "operate": "CheckMobileIsExist", "Mobile": mobile },
                cache: false,
                async: false,
                timeout: 30 * 1000,
                dataType: "json",
                success: function (result) {
                    if ("0" == result.error) {
                        $.ajax({
                            type: "post",
                            url: "/ajax/SendVerifyCode.ashx",
                            data: { "operate": "send", "To": mobile },
                            cache: false,
                            async: true,
                            timeout: 30 * 1000,
                            dataType: "json",
                            success: function (result) {
                                if ("0" == result.error) {
                                    $("#btnSendVerifyCode").hide();
                                    TimeInterval = setInterval("SendVerifyCodeSuccess()", 1000);
                                    $("#time").html(index + "秒后重发");
                                } else {
                                    alert("获取验证码失败");
                                }

                            }, error: function () {
                                alert("获取验证码异常");
                            }
                        });
                    } else {
                        alert("手机号码已经被使用");
                    }
                }, error: function () {
                    alert("验证手机号码是否使用异常");
                }
            });
        });
    }
    /*
    *   验证码发送成功
    */
    function SendVerifyCodeSuccess() {
        if (index - 1 < 0) {
            clearInterval(TimeInterval);
            TimeInterval = null;
            $("#btnSendVerifyCode").show();
            index = 59;
            $("#time").html("");
            return;
        }
        index--;
        $("#time").html(index + "秒后重发");
    }
</script>
</html>

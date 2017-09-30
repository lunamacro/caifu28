<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Apply_Promote.aspx.cs" Inherits="CPS_Apply_Promote" %>

<%@ Register Src="userControls/IndexHeader.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="userControls/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-申请推广员/代理商</title>
    <meta name="description" content="<%=_Site.Name %>-申请推广员/代理商" />
    <meta name="keywords" content="<%=_Site.Name %>-申请推广员/代理商" />
    <link type="text/css" href="css/common.css" rel="stylesheet" />
    <style type="text/css">
        .tip {
            height: 32px;
            color: Red;
        }

            .tip img {
                border: 0px;
                max-height: 16px;
                max-width: 16px;
                padding-top: 5px;
                margin-right: 5px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:Header ID="Header1" runat="server" />
        <div class="inside_banner apply_banner">
            <h3>申请推广<span>Login For Extension</span></h3>
        </div>
        <div class="content">
            <div class="inside_bg">
                <div class="insicon">
                    <div class="sidebar">
                        <ul>
                            <li class="curr" id="tgy" runat="server"><a href="Apply_Promote.aspx?type=tgy">推广员注册</a></li>
                            <li id="dls" runat="server"><a href="Apply_Promote.aspx?type=dls">代理商注册</a></li>
                        </ul>
                    </div>
                    <div class="commain">
                        <div class="reg_wrap" id="div_tgy" runat="server">
                            <h3>
                                <span>我已有账户，直接 <a href="Default.aspx">登录</a></span>注册基本信息（必填）
                            </h3>
                            <div class="inpubox">
                                <div class="reginpu">
                                    <label>
                                        <span>用户名：</span>
                                        <asp:TextBox runat="server" ID="txtName_tgy" MaxLength="18" Style='padding-left: 10px; font-size: 13px; width: 280px;'
                                            placeholder="请输入用户名" />
                                    </label>
                                    <div id="nameTip_tgy" class="tip">
                                        <img src="" alt="" />
                                        <span></span>
                                    </div>
                                </div>
                                <div class="reginpu">
                                    <label>
                                        <span>密 码：</span>
                                        <asp:TextBox runat="server" ID="txtPwd_tgy" MaxLength="18" TextMode="Password" Style='padding-left: 10px; font-size: 13px; width: 280px;'
                                            placeholder="请输入密码" />
                                    </label>
                                    <div id="pwdTip_tgy" class="tip">
                                        <img src="" alt="" />
                                        <span></span>
                                    </div>
                                </div>
                                <div class="reginpu">
                                    <label style="width: 263px;">
                                        <span>手机号码：</span>
                                        <asp:TextBox runat="server" ID="txtPhone_tgy" MaxLength="11" placeholder="请输入手机号码"
                                            Style='padding-left: 10px; font-size: 13px; width: 120px;' />
                                    </label>
                                    <div id="mobileTip_tgy" class="tip">
                                        <input class="obtaincode" type="button" id="btnSendVerifyCode_tgy" value="获取验证码" />
                                        <img src="" alt="" /><span></span>
                                    </div>
                                </div>
                                <div class="reginpu">
                                    <label style="width: 278px;">
                                        <span>验证码：</span>
                                        <asp:TextBox runat="server" ID="txtCode_tgy" MaxLength="6" placeholder="请输入验证码" Style='padding-left: 10px; font-size: 13px; width: 120px;' />
                                    </label>
                                    <div id="codeTip_tgy" class="tip">
                                        <img src="" alt="" /><span></span>
                                    </div>
                                </div>
                                <div class="regcheck">
                                    <label>
                                        <input type="checkbox" id="agreement_tgy" checked="checked">我已阅读并同意<a href="PromotionAgeement.aspx"
                                            target="_blank">《<%=_Site.Name %>推广联盟协议》</a>
                                    </label>
                                </div>
                                <div class="regbut">
                                    <asp:Button runat="server" ID="btnRegister_tgy" Style="cursor: pointer;" Text="注 册" CssClass="reg" OnClick="btnRegister_tgy_Click" />
                                    <input class="reset" type="button" id="clearInput_tgy" value="重置" />
                                </div>
                            </div>
                        </div>
                        <div class="reg_wrap" id="div_dls" runat="server" style="display: none;">
                            <h3>
                                <span>我已有账户，直接 <a href="Default.aspx">登录</a></span>注册基本信息（必填）</h3>
                            <div class="inpubox">
                                <div class="reginpu">
                                    <label>
                                        <span>用户名：</span>
                                        <asp:TextBox runat="server" ID="txtName_dls" MaxLength="18" Style='padding-left: 10px; font-size: 13px; width: 280px;'
                                            placeholder="请输入用户名" />
                                    </label>
                                    <div id="nameTip_dls" class="tip">
                                        <img src="" alt="" />
                                        <span></span>
                                    </div>
                                </div>
                                <div class="reginpu">
                                    <label>
                                        <span>密 码：</span>
                                        <asp:TextBox runat="server" ID="txtPwd_dls" MaxLength="18" TextMode="Password" Style='padding-left: 10px; font-size: 13px; width: 280px;'
                                            placeholder="请输入密码" />
                                    </label>
                                    <div id="pwdTip_dls" class="tip">
                                        <img src="" alt="" />
                                        <span></span>
                                    </div>
                                </div>
                                <div class="reginpu">
                                    <label style="width: 263px;">
                                        <span>手机号码：</span>
                                        <asp:TextBox runat="server" ID="txtPhone_dls" MaxLength="11" placeholder="请输入手机号码"
                                            Style='padding-left: 10px; font-size: 13px; width: 120px;' />
                                    </label>
                                    <div id="mobileTip_dls" class="tip">
                                        <input class="obtaincode" type="button" id="btnSendVerifyCode_dls" value="获取验证码" />
                                        <img src="" alt="" /><span></span>
                                    </div>
                                </div>
                                <div class="reginpu">
                                    <label style="width: 278px;">
                                        <span>验证码：</span>
                                        <asp:TextBox runat="server" ID="txtCode_dls" MaxLength="6" placeholder="请输入验证码" Style='padding-left: 10px; font-size: 13px; width: 120px;' />
                                    </label>
                                    <div id="codeTip_dls" class="tip">
                                        <img src="" alt="" /><span></span>
                                    </div>
                                </div>
                                <div class="regcheck">
                                    <label>
                                        <input type="checkbox" id="agreement_dls" checked="checked" />我已阅读并同意<a href="PromotionAgeement.aspx"
                                            target="_blank">《<%=_Site.Name %>推广联盟协议》</a>
                                    </label>
                                </div>
                                <div class="regbut">
                                    <asp:Button runat="server" ID="btnRegister_dls" Text="注 册" CssClass="reg" OnClick="btnRegister_dls_Click" />
                                    <input class="reset" type="button" id="clearInput_dls" value="重置" />
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
<script type="text/javascript" src="js/CPS.js"></script>
<script type="text/javascript">
    window.onload = function () {
        Register.Type = "<%=type %>";
        Register.Init();

        $("#txtPhone_dls").bind("keydown", inputNumber);
        $("#txtPhone_tgy").bind("keydown", inputNumber);
    }
    function inputNumber(e) {
        var keyCode = e.keyCode;
        // 数字
        if (keyCode >= 48 && keyCode <= 57) return true
        // 小数字键盘
        if (keyCode >= 96 && keyCode <= 105) return true
        // Backspace键
        if (keyCode == 8 || keyCode == 37 || keyCode == 38 || keyCode == 39 || keyCode == 40 || keyCode == 190) return true
        return false;
    }
</script>
</html>

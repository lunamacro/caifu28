<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgentAddPromote.aspx.cs"
    Inherits="CPS_Agent_AgentAddPromote" %>

<%@ Register Src="../userControls/AgentHeader.ascx" TagName="AgentHeader" TagPrefix="uc1" %>
<%@ Register Src="../userControls/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-推广联盟-增加推广员</title>
    <meta name="description" content="<%=_Site.Name %>-推广联盟-增加推广员" />
    <meta name="keywords" content="<%=_Site.Name %>-推广联盟-增加推广员" />
    <link type="text/css" href="../css/common.css" rel="stylesheet" />
    <style type="text/css">
        .tip
        {
            height: 32px;
            color: Red;
        }
        .tip img
        {
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
    <uc1:AgentHeader ID="AgentHeader1" runat="server" />
    <div class="user_banner">
        <h3>
            增加推广员<span>Add Promote</span></h3>
    </div>
    <div class="content">
        <div class="inside_bg">
            <div class="insicon">
                <div class="sidebar">
                    <ul class="fourli">
                        <li><a href="AgentNumber.aspx">会员列表</a></li>
                        <li><a href="AgentPromoteList.aspx">推广员列表</a></li>
                        <li><a href="AgentPromoteNumber.aspx">推广员发展的会员</a></li>
                        <li class="curr"><a href="AgentAddPromote.aspx">增加推广员</a></li>
                    </ul>
                </div>
                <div class="commain">
                    <div class="mainbor">
                        <div class="commain" style="border-top: 1px solid #E1E2E2;">
                            <div class="mainbor">
                                <div class="reg_wrap" style="border: 0px;">
                                    <h3>
                                        基本信息（必填）</h3>
                                    <div class="inpubox">
                                        <div class="reginpu">
                                            <label>
                                                <span>用户名：</span>
                                                <asp:TextBox runat="server" ID="txtName_tgy" MaxLength="18" CssClass="border_shadow" Style='padding-left: 10px;
                                                    font-size: 13px; width: 280px;' placeholder="请输入用户名" />
                                            </label>
                                            <div id="nameTip_tgy" class="tip">
                                                <img src="" alt="" />
                                                <span></span>
                                            </div>
                                        </div>
                                        <div class="reginpu">
                                            <label>
                                                <span>密 码：</span>
                                                <asp:TextBox runat="server" ID="txtPwd_tgy" MaxLength="18" CssClass="border_shadow"  TextMode="Password" Style='padding-left: 10px;
                                                    font-size: 13px; width: 280px;' placeholder="请输入密码" />
                                            </label>
                                            <div id="pwdTip_tgy" class="tip">
                                                <img src="" alt="" />
                                                <span></span>
                                            </div>
                                        </div>
                                        <%--<div class="reginpu">
                                            <label style="width: 263px;">
                                                <span>标识码：</span>
                                                <asp:TextBox runat="server" ID="txtSerialNumber" MaxLength="11" placeholder="请输入标识码"
                                                    Style='padding-left: 10px; font-size: 13px; width: 120px;' />
                                            </label>
                                            <div id="mobileTip_tgy" class="tip">
                                                <img src="" alt="" /><span></span>
                                            </div>
                                        </div>--%>
                                        <div class="regcheck">
                                            <label>
                                                <input type="checkbox" id="agreement_tgy" checked="checked">我已阅读并同意<a href="../PromotionAgeement.aspx"
                                                    target="_blank">《<%=_Site.Name %>推广联盟协议》</a>
                                            </label>
                                        </div>
                                        <div class="regbut">
                                            <asp:Button runat="server" ID="btnRegister_tgy" Text="增 加" CssClass="reg" OnClick="btnRegister_tgy_Click" />
                                            <input class="reset" type="button" id="clearInput_tgy" value="重置" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <uc2:Footer ID="Footer1" runat="server" />
    <input type="hidden" id="hide_IsCheckMobileOK" value="false" />
    </form>
</body>
<script type="text/javascript">
    window.onload = function () {
        $("#txtName_tgy").blur(function () {
            var result = FlyFish.CheckIsUserName("txtName_tgy");
            switch (parseFloat(result)) {
                case 0:     //正确
                    Register.ShowTipImg("nameTip_tgy", "loading");
                    Register.WriteTip("nameTip_tgy", "");
                    var b = false;
                    $.ajax({
                        type: "post",
                        url: "/ajax/CheckUserName.aspx",
                        data: { "UserName": FlyFish.Trim($("#txtName_tgy").val()), "operate": "ValidateUserName" },
                        cache: false,
                        async: false,
                        timeout: 30 * 1000,
                        dataType: "json",
                        success: function (result) {
                            if ("0" == result.error) {
                                Register.ShowTipImg("nameTip_tgy", "error");
                                Register.WriteTip("nameTip_tgy", "用户名已存在");
                                b = false;
                            } else {
                                Register.ShowTipImg("nameTip_tgy", "ok");
                                Register.WriteTip("nameTip_tgy", "");
                                b = true;
                            }
                        }, error: function () {
                            Register.ShowTipImg("nameTip_tgy", "error");
                            Register.WriteTip("nameTip_tgy", "检查用户名是否存在失败，请重试");
                            b = false;
                        }
                    });
                    return b;
                case -1:    //控件不存在
                    Register.ShowTipImg("nameTip_tgy", "error");
                    Register.WriteTip("nameTip_tgy", "请输入用户名");
                    return false;
                case 1:     //没有输入文本
                    Register.ShowTipImg("nameTip_tgy", "error");
                    Register.WriteTip("nameTip_tgy", "请输入用户名");
                    return false;
                case 2:     //用户名不符合规则
                    Register.ShowTipImg("nameTip_tgy", "error");
                    Register.WriteTip("nameTip_tgy", "用户名只能是 中文、_ 、数字、大小写字母组成");
                    return false;
            }
            return true;
        });
        $("#txtPwd_tgy").blur(function () {
            var result = FlyFish.CheckIsUserName("txtPwd_tgy");
            switch (parseFloat(result)) {
                case 0:     //正确
                    Register.ShowTipImg("pwdTip_tgy", "ok");
                    Register.WriteTip("pwdTip_tgy", "");
                    return true;
                case -1:    //控件不存在
                    Register.ShowTipImg("pwdTip_tgy", "error");
                    Register.WriteTip("pwdTip_tgy", "请输入用户名");
                    return false;
                case 1:     //没有输入文本
                    Register.ShowTipImg("pwdTip_tgy", "error");
                    Register.WriteTip("pwdTip_tgy", "请输入用户名");
                    return false;
                case 2:     //用户名不符合规则
                    Register.ShowTipImg("pwdTip_tgy", "error");
                    Register.WriteTip("pwdTip_tgy", "用户名只能是 中文、_ 、数字、大小写字母组成");
                    return false;
            }
            return true;
        });
    }
</script>
</html>

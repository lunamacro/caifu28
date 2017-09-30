<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="CPS_Default" %>

<%@ Register Src="userControls/IndexHeader.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="userControls/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-推广联盟</title>
    <meta name="description" content="<%=_Site.Name %>-推广联盟" />
    <meta name="keywords" content="<%=_Site.Name %>-推广联盟" />
    <link type="text/css" href="css/common.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <uc1:Header ID="Header1" runat="server" />
        <div class="banner">
            <div class="logowrap">
                <div class="logobg">
                </div>
                <div class="logobox">
                    <h2>登录推广联盟</h2>
                    <div class="logoinput">
                        <div class="loginTip" id="loginTip" runat="server" style="display: none">
                        </div>
                        <div class="loginuser">
                            <input type="text" id="txtName" runat="server" maxlength="18" placeholder="用户名" />
                        </div>
                        <div class="loginpassword">
                            <input type="password" id="txtPwd" maxlength="18" runat="server" placeholder="密码" />
                        </div>
                        <div class="">
                            <asp:Button ID="btnLogin" runat="server" Text="登 录" CssClass="loginbut"
                                OnClick="btnLogin_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="bancen" id="bancen">
                <ul id="selidbox">
                    <%=GetPics() %>
                </ul>
            </div>
        </div>
        <div class="content">
            <div class="aboutcommwrap">
                <div class="about">
                    <h3>推广联盟简介</h3>
                    <div class="aboutinfo">
                        <%=promotionAlliance %>
                    </div>
                   
                </div>
            </div>
            <!--Commission HTML end-->
            <div class="whywrap">
                <div class="why_earn_commission">
                    <h3>如何赚取佣金</h3>
                    <ul>
                        <li class="earnstep1"><a href="Apply_Promote.aspx">
                            <div class="earnimg">
                            </div>
                            <div class="earnname">
                                申请推广
                            </div>
                            <div class="earninfo">
                                点击“申请推广”，选择成为代理商或者推广员，获取专属的推广链接地址。
                            </div>
                        </a></li>
                        <li class="earnstep2">
                            <div class="earnimg">
                            </div>
                            <div class="earnname">
                                发展会员
                            </div>
                            <div class="earninfo">
                                根据推广中心的推广链接，推广员可以通过二维码或者广告样式发展会员。
                            </div>
                        </li>
                        <li class="earnstep3">
                            <div class="earnimg">
                            </div>
                            <div class="earnname">
                                会员购彩
                            </div>
                            <div class="earninfo">
                                发展的会员在成功购彩之后，将根据购彩金额以及佣金比例计算您获得的佣金。
                            </div>
                        </li>
                        <li class="earnstep4">
                            <div class="earnimg">
                            </div>
                            <div class="earnname">
                                佣金发放
                            </div>
                            <div class="earninfo">
                                管理员在每月月初，会将上一个月的佣金一次性发放到您的账户。
                            </div>
                        </li>
                        <li class="earnstep5">
                            <div class="earnimg">
                            </div>
                            <div class="earnname">
                                提款
                            </div>
                            <div class="earninfo">
                                推广员在我的账户可以查看佣金明细，并随时可以申请提款。
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <!--Why earn commission HTML end-->
        </div>
        <uc2:Footer ID="Footer2" runat="server" />
    </form>
</body>
<script type="text/javascript" src="js/CPS.js"></script>
<script type="text/javascript">
    window.onload = function () {
        Login.Init();
    }
</script>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PromoteAddUser.aspx.cs" Inherits="CPS_Promote_PromoteAddUser" %>
<%@ Register Src="../userControls/IndexHeader.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="../userControls/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%= _Site.Name%>-推广联盟-好友邀请</title>
    <meta name="description" content="<%= _Site.Name%>-推广联盟-好友邀请" />
    <meta name="keywords" content="<%= _Site.Name%>-推广联盟-好友邀请" />
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta name="viewport" content="width=device-width; initial-scale=1.0;  minimum-scale=1.0; maximum-scale=1.0" />
    <link type="text/css" href="../css/common.css" rel="stylesheet" />
    <style type="text/css">
        .btn{ border:0px; height:44px; width:176px; text-align:center; color:White; background-color:transparent; cursor:pointer;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Header ID="Header1" runat="server" />
    <!--Header HTML end-->
    <div class="user_banner">
        <h3>
            好友邀请<span>Friend Request</span></h3>
    </div>
    <div class="content">
        <div class="inside_bg">
            <div class="insicon">
                <div class="sidebar">
                    <ul>
                        <li class="curr"><a href="javascript:;">好友邀请</a></li>
                    </ul>
                </div>
                <div class="commain">
                    <div class="mainbor">
                        <div class="userinfo_tourist">
                            <h4>
                                尊敬的用户，您好！<span> 【<asp:Literal runat="server" ID="l_userName"></asp:Literal>】</span>邀请您加入<i>【<asp:Literal runat="server" ID="l_siteName"></asp:Literal>】</i>，共同赢大奖。</h4>
                            <p>【<asp:Literal runat="server" ID="l_siteName2"></asp:Literal>】郑重承诺：账户安全、交易便捷、提款方便。</p>
                            <p>【<asp:Literal runat="server" ID="l_siteName3"></asp:Literal>】网站中奖金额最大可达500万，不要犹豫，下一个大奖得主就是您！</p>
                            
                            <div class="applybut apply_promotion">
                                <asp:Button ID="btnOk" runat="server" Text="我也要中奖" onclick="btnOk_Click" CssClass="btn" /></div>
                            <div class="applybut apply_agent">
                                <asp:Button ID="btnCancel" runat="server" Text="无情拒绝" onclick="btnCancel_Click"  CssClass="btn" style=" background:gray"/></div>
                        </div>
                        <div class="trading_tourist">
                            <h4>
                                <span>新闻公告</span></h4>
                            <ul>
                                <asp:Repeater runat="server" ID="rpt_newsList">
                                    <ItemTemplate>
                                        <li><div class="newstime"><%#Eval("DateTime") %></div><a href='New_View.aspx?ID=<%#Eval("ID") %>&NewType=xwgg' target="_blank"><%#Eval("Title") %></a></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <uc2:Footer ID="Footer2" runat="server" />
    </form>
</body>
</html>

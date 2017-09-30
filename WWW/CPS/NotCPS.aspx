<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NotCPS.aspx.cs" Inherits="CPS_NotCPS" %>

<%@ Register Src="userControls/IndexHeader.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="userControls/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-我要申请推广员/代理商</title>
    <meta name="description" content="<%=_Site.Name %>-我要申请推广员/代理商" />
    <meta name="keywords" content="<%=_Site.Name %>-我要申请推广员/代理商" />
    <link type="text/css" href="css/common.css" rel="stylesheet" />
    <style type="text/css">
        .btn {
            border: 0px;
            width: 176px;
            height: 44px;
            background-color: transparent;
            cursor: pointer;
            color: White;
        }

        .userinfo_tourist p {
            line-height: 27px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:Header ID="Header1" runat="server" />
        <!--Header HTML end-->
        <div class="user_banner">
            <h3>我要申请<span>I want to apply for the</span></h3>
        </div>
        <div class="content">
            <div class="inside_bg">
                <div class="insicon">
                    <div class="sidebar">
                        <ul>
                            <li class="curr"><a href="javascript:void(0);">我要申请</a></li>
                        </ul>
                    </div>
                    <div class="commain">
                        <div class="mainbor">
                            <div class="userinfo_tourist">
                                <h4>尊敬的用户，欢迎来到推广联盟。</h4>
                                <p>
                                    满足以下条件，您将可以轻松加入推广联盟，一次邀请好友，永久获取佣金。
                                </p>
                                <p>
                                    1、同意<a href="PromotionAgeement.aspx" target="_blank">《<%=_Site.Name %>推广联盟协议》</a>。
                                </p>
                                <p>
                                    2、拥有一定数量的彩民用户。
                                </p>
                                <p>
                                    3、联系网站管理员，加盟成为我们的合作代理商。
                                </p>
                            </div>
                            <div class="trading_tourist">
                                <h4>
                                    <span>新闻公告</span></h4>
                                <ul>
                                    <asp:Repeater runat="server" ID="rpt_newsList">
                                        <ItemTemplate>
                                            <li>
                                                <div class="newstime">
                                                    <%#Eval("DateTime") %>
                                                </div>
                                                <a href='New_View.aspx?ID=<%#Eval("ID") %>&NewType=xwgg' target="_blank">
                                                    <%#Eval("Title") %></a></li>
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
<script type="text/javascript">
    var handUrl = "/CPS/ashx/Normal.ashx";
    $("#btn_tgy").click(function () {
        var okfunc = function () {
            var successFunc = function (json) {
                if (json) {
                    var art = dialog({
                        title: '提示',
                        content: json.result,
                        ok: function () {
                            if (json.url) {
                                window.location.href = json.url;
                            }
                        },
                        okValue: '确定'
                    });
                    art.showModal();
                }
            };
            f_ajaxPost(handUrl, { "act": "ApplyPromoter", "ID": "<%=_User.ID%>", "url": "<%=Request.Url.ToString()%>" }, successFunc, null, null);
        };
        var cancelfunc = function () { };
        confirm("确认要申请成为推广员吗?", okfunc, cancelfunc);
    });
    $("#btn_dls").click(function () {
        var okfunc = function () {
            var successFunc = function (json) {
                if (json) {
                    var art = dialog({
                        title: '提示',
                        content: json.result,
                        ok: function () {
                            if (json.url) {
                                window.location.href = json.url;
                            }
                        },
                        okValue: '确定'
                    });
                    art.showModal();
                }
            };
            f_ajaxPost(handUrl, { "act": "ApplyAgent", "ID": "<%=_User.ID%>", "url": "<%=Request.Url.ToString()%>" }, successFunc, null, null);
        };
        var cancelfunc = function () { };
        confirm("确认要申请成为代理商吗?", okfunc, cancelfunc);
    });
</script>

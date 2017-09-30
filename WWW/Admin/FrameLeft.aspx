<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrameLeft.aspx.cs" Inherits="Admin_FrameLeft" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script src="../JScript/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
    <link href="../Style/common.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/common.js" type="text/javascript"></script>
    <link href="../Style/jquery.mCustomScrollbar.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .selecteda {
            background: url(../images/Sprite.png) no-repeat -280px -268px;
            background-color: #6aa1e3;
            color: White;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            if ($("#hidComeptenceID").val() == "1") {
                $("#sidebar_con").hide();
                $("#sidebar_con2").show();
            }

            $("#sidebar_insi a").each(function () {
                $(this).on("click", function () {
                    $("a").removeClass("selecteda");
                    $(this).addClass("selecteda");
                });

            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="sidebar" id="sidebar" style="height: auto; overflow: auto;">
            <div class="sidebar_insi" id="sidebar_insi">
				
                <div class="sidebar_con" id="sidebar_con2">
                    
                    <dl class="user_manage">
                        <dt><h2>用户管理</h2></dt>
                        <dd><h3><a href="Users.aspx" target="mainFrame">用户一览表</a></h3></dd>
                        <dd><h3><a href="UserAccountDetail.aspx" target="mainFrame">用户账户明细</a></h3></dd>
                        <dd><h3><a href="UserAddMoney.aspx" target="mainFrame">用户账户充值</a></h3></dd>
						<dd><h3><a href="UserAddHandsel.aspx" target="mainFrame">用户彩金充值</a></h3></dd>
                    </dl>
                    <dl class="lottery_manage">
                        <dt><h2>彩票业务中心</h2></dt>
                        <dd><h3><a href="Isuse.aspx" target="mainFrame">期号管理</a></h3></dd>
                        <dd><h3><a href="BetUpdate.aspx" target="mainFrame">倍率与回水设置</a></h3></dd>
                        <dd><h3><a href="SchemeListPc.aspx" target="mainFrame">PC28回水操作</a></h3></dd>
                        <!--dd><h3><a href="SchemeQuash2.aspx" target="mainFrame">方案撤单</a></h3></dd-->
                        <dd><h3><a href="Open.aspx" target="mainFrame">开奖&amp;派奖</a></h3></dd>
                        <dd><h3><a href="SchemeList.aspx" target="mainFrame">方案查询</a></h3></dd>
                        <dd><h3><a href="WinList.aspx" target="mainFrame">中奖查询</a></h3></dd>
                    </dl>
					
                    <dl class="financial_manage">
                        <dt><h2>财务中心</h2></dt>
                        <dd><h3><a href="FinanceAddMoney.aspx" target="mainFrame">用户充值明细表</a></h3></dd>
                        <dd><h3><a href="AccountDetails.aspx" target="mainFrame">用户交易明细表</a></h3></dd>
                        <dd><h3><a href="FinanceWin.aspx" target="mainFrame">用户中奖明细表</a></h3></dd>
                        <dd><h3><a href="UserDistill.aspx" target="mainFrame">处理用户提款申请</a></h3></dd>
                        <dd><h3><a href="UserDistillWaitPay.aspx" target="mainFrame">待付款用户一览表</a></h3></dd>
                        <dd><h3><a href="FinanceDistill.aspx" target="mainFrame">用户提款明细表</a></h3></dd>
                        <dd><h3><a href="FinanceBalanceAgent.aspx" target="mainFrame">公司收支汇总表</a></h3></dd>
                        <dd><h3><a href="Handsel.aspx" target="mainFrame">充值送彩金设置</a></h3></dd>
                    </dl>

                    <dl class="system_settings">
                        <dt><h2>系统设置</h2></dt>
                        <dd><h3><a href="Site2.aspx" target="mainFrame">站点资料</a></h3></dd>
                        <dd><h3><a href="AppPicManage.aspx" target="mainFrame">APP轮播图片</a></h3></dd>
                        <dd><h3><a href="AppSetting.aspx" target="mainFrame">APP版本设置</a></h3></dd>
                        <dd><h3><a href="SiteAffiches.aspx" target="mainFrame">通知公告</a></h3></dd>
                        <dd><h3><a href="PaymentSetting.aspx" target="mainFrame">支付方式设置</a></h3></dd>
                    </dl>
                </div>
            </div>
            <div class="sidebar_but" id="sidebar_but" style="display: none">
            </div>
        </div>
        <asp:HiddenField ID="hidComeptenceID" runat="server" />
    </form>
</body>
</html>
<script src="../JScript/jquery.mCustomScrollbar.concat.min.js" type="text/javascript"></script>

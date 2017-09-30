<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AgentAccount.ascx.cs"
    Inherits="CPS_userControls_AgentAccount" %>
<div class="Agent_info">
    <h4>
        <asp:Literal runat="server" ID="l_sayHello"/> 代理商 <b><i>[ <asp:Literal runat="server" ID="l_name"/> ]</i> </b>，以下是您的账户信息：</h4></h4>
    <div class="agentbox">
        <ul id="rollbox">
           

            <li><span>今日新增会员：</span><i><asp:Literal runat="server" ID="l_AddUserCountForDay" Text="0" /></i></li>
            <li><span>今日会员购彩金额：</span><i><asp:Literal runat="server" ID="l_UserBuyLotteryMoneyForDay" Text="0.00" /></i></li>
            <li><span>今日会员提现金额：</span><i><asp:Literal runat="server" ID="l_UserBuyLotteryCommissionForDay" Text="0.00" /></i></li>

            <li><span>本月新增会员：</span><i><asp:Literal runat="server" ID="l_AddUserCountForMonth" Text="0" /></i></li>
            <li><span>本月会员购彩金额：</span><i><asp:Literal runat="server" ID="l_UserBuyLotteryMoneyForMonth" Text="0.00" /></i></li>
            <li><span>本月会员提现金额：</span><i><asp:Literal runat="server" ID="l_UserBuyLotteryCommissionForMonth" Text="0.00" /></i></li>

            <li><span>会员总计：</span><i><asp:Literal runat="server" ID="l_AddUserCountForTotal" Text="0" /></i></li>
            <li><span>会员购彩金额总计：</span><i><asp:Literal runat="server" ID="l_UserBuyLotteryMoneyForTotal" Text="0.00" /></i></li>
            <li><span>会员提现金额总计：</span><i><asp:Literal runat="server" ID="l_UserBuyLotteryCommissionForTotal" Text="0.00" /></i></li>

             <li><span>今日添加推广员：</span><i><asp:Literal runat="server" ID="l_AddPromoteCountForDay" Text="0" /></i></li>
            <li><span>今日推广员购彩金额：</span><i><asp:Literal runat="server" ID="l_PromoteBuyLotteryMoneyForDay" Text="0.00" /></i></li>
            <li><span>今日推广员提现金额：</span><i><asp:Literal runat="server" ID="l_PromoteBuyLotteryCommissionForDay" Text="0.00" /></i></li>

            <li><span>本月添加推广员：</span><i><asp:Literal runat="server" ID="l_AddPromoteCountForMonth" Text="0" /></i></li>
            <li><span>本月推广员购彩金额：</span><i><asp:Literal runat="server" ID="l_PromoteBuyLotteryMoneyForMonth" Text="0.00" /></i></li>
            <li><span>本月推广员提现金额：</span><i><asp:Literal runat="server" ID="l_PromoteBuyLotteryCommissionForMonth" Text="0.00" /></i></li>

            <li><span>推广员总计：</span><i><asp:Literal runat="server" ID="l_AddPromoteCountForTotal" Text="0" /></i></li>
            <li><span>推广员购彩金额总计：</span><i><asp:Literal runat="server" ID="l_PromoteBuyLotteryMoneyForTotal" Text="0.00" /></i></li>
            <li><span>推广员提现金额总计：</span><i><asp:Literal runat="server" ID="l_PromoteBuyLotteryCommissionForTotal" Text="0.00" /></i></li>

            <li><span>今日推广员总会员数：</span><i><asp:Literal runat="server" ID="l_PromoteAddUserCountForDay" Text="0" /></i></li>
            <li><span>今日推广员会员总购彩金额：</span><i><asp:Literal runat="server" ID="l_PromoteAddUserBuyLotteryMoneyForDay" Text="0.00" /></i></li>
            <li><span>今日推广员会员总提现金额：</span><i><asp:Literal runat="server" ID="l_PromoteAddUserBuyLotteryCommissionForDay" Text="0.00" /></i></li>

            <li><span>本月推广员总会员数：</span><i><asp:Literal runat="server" ID="l_PromoteAddUserCountForMonth" Text="0.00" /></i></li>
            <li><span>本月推广员会员总购彩金额：</span><i><asp:Literal runat="server" ID="l_PromoteAddUserBuyLotteryMoneyForMonth" Text="0.00" /></i></li>
            <li><span>本月推广员会员总提现金额：</span><i><asp:Literal runat="server" ID="l_PromoteAddUserBuyLotteryCommissionForMonth" Text="0.00" /></i></li>

            <li><span>推广员会员数总计：</span><i><asp:Literal runat="server" ID="l_PromoteAddUserCountForTotal" Text="0" /></i></li>
            <li><span>推广员会员购彩金额总计：</span><i><asp:Literal runat="server" ID="l_PromoteAddUserBuyLotteryMoneyForTotal" Text="0.00" /></i></li>
            <li><span>推广员会员提现金额总计：</span><i><asp:Literal runat="server" ID="l_PromoteAddUserBuyLotteryCommissionForTotal" Text="0.00" /></i></li>
             <!-- 第一页 第一行
            <li><span></span><i></i></li>
            <li><span></span><i></i></li>
            <li><span>今日获得佣金总计：</span><i><asp:Literal runat="server" ID="l_SumCommissionForDay" Text="0" /></i></li>

            <li><span></span><i></i></li>
            <li><span></span><i></i></li>
            <li><span>本月获得佣金总计：</span><i><asp:Literal runat="server" ID="l_SumCommissionForMonth" Text="0" /></i></li>

            <li><span></span><i></i></li>
            <li><span></span><i></i></li>
            <li><span>佣金总计：</span><i><asp:Literal runat="server" ID="l_SumCommissionTotal" Text="0" /></i></li>
                 -->

            <li><span></span><i></i></li>
            <li><span></span><i></i></li>
            <li><span></span><i></i></li>

            <li><span></span><i></i></li>
            <li><span></span><i></i></li>
            <li><span></span><i></i></li>

            <li><span></span><i></i></li>
            <li><span></span><i></i></li>
            <li><span></span><i></i></li>
        </ul>
        <div class="focusbox" id="focusbox">
        </div>
    </div>
</div>

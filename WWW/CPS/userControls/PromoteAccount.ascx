<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PromoteAccount.ascx.cs" Inherits="CPS_userControls_PromoteAccount" %>
<div class="userinfo">
                            <h4>
                                <asp:Literal runat="server" ID="l_sayHello"/> 推广员 <b><i>[ <asp:Literal runat="server" ID="l_name"/> ]</i> </b>，以下是您的账户信息：</h4>
                            <ul>
                               
                                <li><span>今日新增会员：</span><i><asp:Literal runat="server" ID="l_userCountForDay" Text="0"/></i>人</li>
                                <li><span>本月新增会员：</span><i><asp:Literal runat="server" ID="l_userCountForMonth" Text="0"/></i>人</li>
                                <li><span>会员总计：</span><i><asp:Literal runat="server" ID="l_sumUserCount" Text="0"/></i>人</li>

                                <li><span>今日购彩会员数：</span><i><asp:Literal runat="server" ID="l_userBuyCountForDay" Text="0"/></i>人</li>
                                <li><span>本月购彩会员数：</span><i><asp:Literal runat="server" ID="l_userBuyCountForMonth" Text="0"/></i>人</li>
                                <li><span>会员购彩总数：</span><i><asp:Literal runat="server" ID="l_sumUserBuyCount" Text="0"/></i>人</li>

                                <li><span>今日会员购彩金额：</span><i><asp:Literal runat="server" ID="l_userBuyMoneyForDay" Text="0.00"/></i>元</li>
                                <li><span>本月会员购彩总额：</span><i><asp:Literal runat="server" ID="l_userBuyMoneyForMonth" Text="0.00"/></i>元</li>
                                <li><span>会员购彩总额：</span><i><asp:Literal runat="server" ID="l_sumUserBuyMoney" Text="0.00"/></i>元</li>

                                <li><span>今日获得佣金：</span><i><asp:Literal runat="server" ID="l_commissionForDay" Text="0.00"/></i>元</li>
                                <li><span>本月应收佣金：</span><i><asp:Literal runat="server" ID="l_commissionForMonth" Text="0.00"/></i>元</li>
                                <li><span>获得佣金总计：</span><i><asp:Literal runat="server" ID="l_sumCommission" Text="0.00"/></i>元</li>
                            </ul>
                        </div>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Scheme.aspx.cs" Inherits="Admin_Scheme" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>查看方案详情-<%=_Site.Name %></title>
    <meta name="description" content="彩票网是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站。" />
    <meta name="keywords" content="竞彩开奖，彩票走势图，超级大乐透，排列3/5" />
    
    <link rel="shortcut icon" href="../../favicon.ico" />
    <link href="/Style/global.css" rel="stylesheet" />
    <link href="/Style/scheme-detail.css" rel="stylesheet" />
    <style>
        #btnOK {
            width: 80px;
            height: 30px;
            border: 0px;
            margin: 0px auto;
            cursor: pointer;
            background-color: #4d96e3;
            color: #ffffff;
            border-radius: 6px;
            font-size: 14px;
        }

        .red {
            color: Red;
        }

        #prebet-q {
            margin: auto;
            text-align: center;
            cursor: pointer;
            line-height: 30px;
            width: 100px;
            font-weight: bold;
            font-family: "Microsoft YaHei UI";
        }

            #prebet-q:hover {
                color: #fa8446;
            }

        #prebet-h {
            margin-top: 15px;
        }

        #btn_Single {
            padding: 5px 10px;
            line-height: 1;
            border: 1px solid #EEC35D;
            color: #E36128;
            background-color: #FBF5E2;
            border-radius: 3px;
            cursor: pointer;
        }

        .scheme > table table th, .scheme > table table td {
            padding: 3px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main scheme">
            <h2 id="HTScheme" runat="server"></h2>
            <h4>方案基本信息</h4>
            <table cellspacing="0">
                <tr>
                    <th>方案发起人：
                    </th>
                    <td>
                        <div class="left sponsor-results" id="HDUserName" runat="server">
                            【发起人历史战绩】
                        </div>
                        <span>
                            <asp:Label ID="labAtTop" runat="server" Visible="True"></asp:Label></span>
                        <div class="right customize">
                            <%-- <button class="only" type="button">定制此彩种</button>--%>
                            <%--<asp:Button ID="btn_Single" runat="server" Text="定制此彩种"
                              OnClientClick="return CreateLogin(this);" />--%>
                            <%--<input id="btn_Single" style="cursor:pointer;" type="button" runat="server" value="定制此彩种" onclick="CreateLogin('loginOver()');" />--%>
                            <%--<button type="button">定制所有彩种</button>--%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>方案信息：
                    </th>
                    <td>
                        <table>
                            <tr>
                                <th align="center">方案编号
                                </th>
                                <th align="center">总金额
                                </th>
                                <th align="center">倍数
                                </th>
                                <th align="center">份数
                                </th>
                                <th align="center">每份
                                </th>
                                <th align="center">佣金比例
                                </th>
                                <th align="center">保底金额
                                </th>
                                <th align="center">购买进度
                                </th>
                                <th align="center">状态
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="labSchemeNumber" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <i class="col-red">￥<asp:Label ID="labSchemeMoney" runat="server"></asp:Label></i>元
                                </td>
                                <td>
                                    <asp:Label ID="labMultiple" runat="server"></asp:Label>倍
                                </td>
                                <td>
                                    <asp:Label ID="labShare" runat="server"></asp:Label>份
                                </td>
                                <td>
                                    <i class="col-red">￥<asp:Label ID="labShareMoney" runat="server">0.00</asp:Label></i>元
                                </td>
                                <td class="col-red">
                                    <asp:Label ID="lbSchemeBonus" runat="server"></asp:Label>%
                                </td>
                                <td>
                                    <asp:Label ID="labAssureMoney" runat="server"></asp:Label>
                                </td>
                                <td class="col-red">
                                    <asp:Label ID="labSchedule" runat="server"></asp:Label>%
                                </td>
                                <td class="col-red">
                                    <asp:Label ID="lbState" runat="server"></asp:Label>
                                </td>
                                <asp:TextBox ID="tbSchemeID" runat="server" Width="30px" Visible="False"></asp:TextBox>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th>投注内容：
                    </th>
                    <td>
                        <div id="StrNumber">
                            <asp:Label ID="labLotteryNumber" runat="server"></asp:Label>
                            <%= StrNumber %>
                            <asp:HyperLink ID="linkDownloadScheme" runat="server" Visible="False" Target="_blank">下载方案</asp:HyperLink>
                            <asp:LinkButton ID="lbUploadScheme" runat="server" Visible="False" OnClientClick="return CreateUplaodDialog()">方案上传</asp:LinkButton>
                            <!--点击展开查看方案begin-->
                            <%-- <asp:Label ID="labLotteryNumber" runat="server"></asp:Label>--%>
                            <!--点击展开查看方案end-->
                        </div>
                    </td>
                </tr>
                <tr id="trWinNumber">
                    <th>开奖号码：
                    </th>
                    <td>
                        <asp:Label ID="lbWinNumber" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <%--<h4>方案投注信息</h4>--%>
            <%--<table cellspacing="0">
                <tr>
                    <th>方案标题：</th>
                    <td><asp:Label ID="labSchemeTitle" runat="server" Style="word-break: break-all; word-wrap: break-word"></asp:Label></td>
                </tr>
                <tr>
                    <th>方案描述：</th>
                    <td><asp:Label ID="labSchemeDescription" runat="server" Style="word-break: break-all;
                                        word-wrap: break-word"></asp:Label></td>
                </tr>
                <tr>
                    <th>分享给好友：</th>
                    <td><asp:Label ID="labSchemeADUrl" runat="server"></asp:Label></td>
                </tr>
                <tr class="my-buy" id="trBuy" runat="server" style=" display:none">
                    <th>我要认购：</th>
                    <td class="buy-number-1">
                    <span ></span>
                         <%= StrHMuser %>
                         我想认购 <span class="minus"></span><input type="text" onkeyup="value=value.replace(/[^\d]/g,'')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g,''))"  id="tbShare" value="1"  runat="server"/><span class="add"></span>
                        份,总金额 <i class="col-red" id="labSumMoney" runat="server">0.00</i> 元 
                        <em>【<a href="OnlinePay/Default.aspx" target="_blank">用户充值</a>】【<a href="AccountDetail.aspx" target="_blank">账户明细</a>】</em>
                    </td>
                </tr>
            </table>--%>
            <h4>方案认购信息</h4>
            <table cellspacing="0">
                <tr>
                    <th>参与用户列表：
                    </th>
                    <td>总共有
                    <asp:Label ID="lbUser" runat="server"></asp:Label>
                        个用户参与。【<a href="javascript:void(0)" onclick="onUserListClick()">打开/隐藏明细</a>】
                    </td>
                </tr>
                <tr id="trUserListDetail">
                    <th>&nbsp;
                    </th>
                    <td>
                        <div id="UserList" runat="server">
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>我的认购记录：
                    </th>
                    <td>
                        <asp:Label ID="labMyBuy" runat="server"></asp:Label>
                        <asp:DataGrid ID="g" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None"
                            ShowHeader="False" OnItemCommand="g_ItemCommand" OnItemDataBound="g_ItemDataBound">
                            <Columns>
                                <asp:BoundColumn DataField="Share">
                                    <HeaderStyle Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="DetailMoney">
                                    <HeaderStyle Width="10%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn>
                                    <HeaderStyle Width="35%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="DateTime">
                                    <HeaderStyle Width="20%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn>
                                    <HeaderStyle Width="10%"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:TemplateColumn>
                                    <HeaderStyle Width="20%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <ShoveWebUI:ShoveConfirmButton ID="btnQuashBuy" BackgroupImage="images/btnBack02.gif"
                                            Style="font-size: 9pt; cursor: pointer; border-top-style: none; font-family: Tahoma; border-right-style: none; border-left-style: none; border-bottom-style: none"
                                            runat="server" Height="20px" Width="84px" Text="我要撤消" CommandName="QuashBuy"
                                            AlertText="确信要撤消此认购记录吗？" onblur="return SetbtnOKFocus();" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn Visible="False" DataField="QuashStatus">
                                    <HeaderStyle Width="0px"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="Buyed">
                                    <HeaderStyle Width="0px"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="IsuseID"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="Code"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="BuyedShare"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="Schedule"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="id"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="isWhenInitiate"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="SchemeShare"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
            </table>
            <h4>方案中奖信息</h4>
            <table class="win-info" cellspacing="0">
                <tr>
                    <th>中奖情况：
                    </th>
                    <td>
                        <asp:Label ID="labWin" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>我的奖金：
                    </th>
                    <td>
                        <asp:Label ID="lbReward" runat="server" ForeColor="red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>本次赠送积分：
                    </th>
                    <td>
                        <i class="col-red" id="lbScoring" runat="server">128</i> 积分
                    </td>
                </tr>
            </table>
        </div>
        <div style="text-align: center; margin-bottom: 30px;">
            <%--<a onclick="history.go(-1)" class="btn_operate">返回</a>--%>
            <input type="button" id="btnOK" value="返回" onclick="history.go(-1)" />
        </div>
        <asp:HiddenField ID="hfID" runat="server" />
        <asp:HiddenField ID="hdSchemeID" runat="server" />
        <asp:HiddenField ID="hdEachMoney" runat="server" />
        <asp:HiddenField ID="hdBalance" runat="server" />
        <asp:HiddenField ID="hidLotteryID" runat="server" />
        <asp:HiddenField ID="hidSchemeState" runat="server" />
        <input type="hidden" id="hf_hmpe" runat="server" value="0" />
        <input type="hidden" id="hidMaxMoney" runat="server" />
    </form>
    <script src="../../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var o_labWinNumber = $("#<%=lbWinNumber.ClientID%>");
            var o_trWinNumber = $("#trWinNumber");

            if (o_labWinNumber.html() != "") {
                o_trWinNumber.show();
            }
            else {
                o_trWinNumber.hide();
            }
        });
        function onUserListClick() {
            if ($("#trUserListDetail").is(":visible")) {
                $("#trUserListDetail").hide();
            }
            else {
                $("#trUserListDetail").show();
            }
        }

        $("#prebet-q").toggle(function () {
            $(this).next().fadeIn("slow");
            $(this).find("i").text("关闭");
        }, function () {
            $(this).next().fadeOut("slow");
            $(this).find("i").text("查看");
        });
    </script>
</body>
</html>

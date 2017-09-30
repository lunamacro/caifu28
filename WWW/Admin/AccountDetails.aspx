<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountDetails.aspx.cs" Inherits="Admin_AccountDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>彩票业务中心-用户交易明细</title>
    
    <link href="../../Style/sandPage.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
    <link href="../Style/common.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/common.js" type="text/javascript"></script>
    <link href="../Style/jquery.mCustomScrollbar.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/jquery.mCustomScrollbar.concat.min.js" type="text/javascript"></script>
    <script src="../JScript/sandPage.js" type="text/javascript"></script>
    <style type="text/css">
        tr
        {
            height: 30px;
        }
        
        .newsTable tbody td
        {
            text-align: center;
        }
        
        .newsTable .time
        {
            width: 5%;
        }
        
        .newsTable .title
        {
            width: 15%;
        }
        
        .newsTable .isShow
        {
            width: 30%;
        }
        
        .newsTable .isRead
        {
            width: 8%;
        }
        
        .newsTable .edit
        {
            width: 8%;
        }
        
        .newsTable .btnEdit
        {
            display: block;
            float: left;
            color: #3977C3;
            font-family: "微软雅黑";
            width: 70px;
            margin: 10px;
            height: 22px;
            line-height: 22px;
            text-align: center;
            border: 0px;
            text-decoration: underline;
            cursor: pointer;
            text-decoration: none;
            background: #C7E8FE;
        }
        
        .btnEdit:hover
        {
            background: #D6F7FE;
        }
        
        #btn
        {
            display: block;
            height: 32px;
            width: 82px;
            background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#C2E6FE), to(#C2E6FE));
            border: 0px;
            cursor: pointer;
        }
        
        #btn:hover
        {
            border: 1px solid #3977C3;
        }
        
        
        
        #sand Button
        {
            padding: 5px 12px;
            margin: 10px 3px;
            font-size: 13px;
            border: 1px solid #DFDFDF;
            background-color: #FFF;
            color: #9a9a9a;
            text-decoration: none; /*-moz-border-radius: 4px;     -webkit-border-radius: 4px; border-radius: 4px;*/
        }
        
        .topcoun
        {
            padding: 5px 10px 5px 2px;
            height: 20px;
            font-size: 14px;
        }
        
        .topbuttom
        {
            text-align: right;
            float: right;
            padding-right: 135px;
            margin-top: 40px;
        }
        
        .newsTable
        {
            font-size: 12px;
        }
        
        #tab
        {
            border: 1px solid #dfdfdf;
        }
        
        #tab th
        {
            border: 1px solid #dfdfdf;
            background: #f7f7f7;
        }
        
        #tab td
        {
            border: 1px solid #dfdfdf;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="hdCurDiv" runat="server" value="divBuy" />
    <div class="main">
        <div class="maincon">
            <h2>
                用户交易明细</h2>
            <div class="accountd_wrap">
                <div id="divAccountDetail">
                    <table border="0" cellpadding="0" cellspacing="0" class="wraptable">
                        <tr>
                            <td>
                                用户名：<input type="text" runat="server" onblur="oblur(1)" onclick="oclick(1)" id="keyword1"
                                    size="22" value="输入用户名" width="120px" style="color: rgb(153, 153, 153); height: 20px;
                                    font-size: 14px;" />
                                &nbsp;&nbsp; &nbsp;&nbsp; 交易类型:<asp:DropDownList ID="ddlTradeType" runat="server"
                                    AutoPostBack="false" Height="20px" Font-Size="14px">
                                    <asp:ListItem Value="1,2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 61,62,71,72,91,101,102,103,104,105,106,107,108,109, 301,302,401,402"
                                        Text="全部"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="网上充值"></asp:ListItem>
                                    <asp:ListItem Value="91" Text="手工充值"></asp:ListItem>
                                    <asp:ListItem Value="107" Text="提款"></asp:ListItem>
                                    <asp:ListItem Value="101,105,106,108,109,201,202,301,402" Text="消费"></asp:ListItem>
                                    <asp:ListItem Value="102,103,104,401" Text="冻结"></asp:ListItem>
                                    <asp:ListItem Value="4,5,6,71" Text="解除冻结"></asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;&nbsp; &nbsp; 流水号：<input type="text" runat="server" onblur="oblur(2)" onclick="oclick(2)"
                                    id="keyword2" size="22" value="输入流水号" width="130px" style="color: rgb(153, 153, 153);
                                    height: 20px; font-size: 14px;" />
                                &nbsp;&nbsp; 方案编号：<input type="text" runat="server" onblur="oblur(3)" width="120px"
                                    onclick="oclick(3)" id="keyword3" size="22" value="输入方案编号" style="color: rgb(153, 153, 153);
                                    height: 20px; font-size: 14px;" />
                            </td>
                        </tr>
                        <tr>
                            <td height="30" colspan="8" class="tobcoun">
                                <table border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            开始时间：
                                            <asp:DropDownList ID="ddlYear" Height="20px" runat="server">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="True" Height="20px"
                                                OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                                                <asp:ListItem Value="1">1 月</asp:ListItem>
                                                <asp:ListItem Value="2">2 月</asp:ListItem>
                                                <asp:ListItem Value="3">3 月</asp:ListItem>
                                                <asp:ListItem Value="4">4 月</asp:ListItem>
                                                <asp:ListItem Value="5">5 月</asp:ListItem>
                                                <asp:ListItem Value="6">6 月</asp:ListItem>
                                                <asp:ListItem Value="7">7 月</asp:ListItem>
                                                <asp:ListItem Value="8">8 月</asp:ListItem>
                                                <asp:ListItem Value="9">9 月</asp:ListItem>
                                                <asp:ListItem Value="10">10月</asp:ListItem>
                                                <asp:ListItem Value="11">11月</asp:ListItem>
                                                <asp:ListItem Value="12">12月</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlDay" Height="20px" runat="server">
                                            </asp:DropDownList>
                                            结束时间：
                                            <asp:DropDownList ID="ddlYear1" Height="20px" runat="server">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlMonth1" runat="server" Height="20px" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                                                <asp:ListItem Value="1">1 月</asp:ListItem>
                                                <asp:ListItem Value="2">2 月</asp:ListItem>
                                                <asp:ListItem Value="3">3 月</asp:ListItem>
                                                <asp:ListItem Value="4">4 月</asp:ListItem>
                                                <asp:ListItem Value="5">5 月</asp:ListItem>
                                                <asp:ListItem Value="6">6 月</asp:ListItem>
                                                <asp:ListItem Value="7">7 月</asp:ListItem>
                                                <asp:ListItem Value="8">8 月</asp:ListItem>
                                                <asp:ListItem Value="9">9 月</asp:ListItem>
                                                <asp:ListItem Value="10">10月</asp:ListItem>
                                                <asp:ListItem Value="11">11月</asp:ListItem>
                                                <asp:ListItem Value="12">12月</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlDay1" Height="20px" runat="server">
                                            </asp:DropDownList>
                                            &nbsp;&nbsp&nbsp
                                            <asp:Button runat="server" ID="btnGO" OnClick="btnGO_Click" Text="查询" Style="cursor: pointer;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table id="tab" class="newsTable" cellspacing="0" rules="all" style="border-collapse: collapse;">
                        <thead>
                            <tr>
                                <th>
                                    流水号
                                </th>
                                <th>
                                    用户名
                                </th>
                                <th>
                                    交易时间
                                </th>
                                <th>
                                    摘要
                                </th>
                                <th>
                                    收入(元)
                                </th>
                                <th>
                                    支出(元)
                                </th>
                                <th>
                                    彩金
                                </th>
                                <th>
                                    彩金余额
                                </th>
                                <%--     <th class="isRead">
                               (手续费)
                            </th>--%>
                                <th>
                                    中奖金额
                                </th>
                                <th>
                                    中奖总金额
                                </th>
                                <th>
                                    来源
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <%--onitemcommand="rptSchemes_ItemCommand" OnItemDataBound="rptSchemes_ItemDataBound"--%>
                            <asp:Repeater ID="rptSchemes" runat="server" EnableViewState="false">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%#Eval("ID")%>
                                        </td>
                                        <td>
                                            <%#Eval("UserName")%>
                                        </td>
                                        <td>
                                            <%#Eval("DateTime")%>
                                        </td>
                                        <td>
                                            <%#Eval("Memo")%>
                                        </td>
                                        <td class="MoneyAdd">
                                            <%#  Shove._Convert.StrToDouble(Eval("MoneyAdd").ToString(),0).ToString("0.00") %>
                                        </td>
                                        <td class="MoneySub">
                                            <%#  Shove._Convert.StrToDouble(Eval("MoneySub").ToString(), 0).ToString("0.00")%>
                                        </td>
                                        <td class="HandselAmount">
                                            <%#  Shove._Convert.StrToDouble(Eval("HandselAmount").ToString(), 0).ToString("0.00")%>
                                        </td>
                                        <td class="HandselTotal">
                                            <%#  Shove._Convert.StrToDouble(Eval("HandselTotal").ToString(), 0).ToString("0.00")%>
                                        </td>
                                        <%-- <td class="isRead">

                                    <%#  Shove._Convert.StrToDouble(Eval("FormalitiesFees").ToString(), 0).ToString("0.00")%>

                                  
                                    </td>--%>
                                        <td class="Reward">
                                            <%#  Shove._Convert.StrToDouble(Eval("Reward").ToString(), 0).ToString("0.00")%>
                                        </td>
                                        <td class="SumReward">
                                            <%#  Shove._Convert.StrToDouble(Eval("SumReward").ToString(), 0).ToString("0.00")%>
                                        </td>
                                        <td>
                                            <%#  Eval("Comefrom")!=""?Eval("Comefrom"):"系统"%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    <tfoot>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td id="tdMoneyAddTotal" style="text-align:center"></td>
                            <td id="tdMoneySubTotal" style="text-align:center"></td>
                            <td id="tdHandselAmountTotal" style="text-align:center"></td>
                            <td id="tdHandselTotalTotal" style="text-align:center"></td>
                            <td id="tdRewardTotal" style="text-align:center"></td>
                            <td id="tdSumRewardTotal" style="text-align:center"></td>
                            <td></td>
                        </tr>
                    </tfoot>
                    </table>
                    <div id="sand" style="text-align: right; padding-right: 30px;">
                    </div>
                    <input name="index" value="1" id="index" hidden="hidden" type="hidden" />
                    <table class="wraptable" cellspacing="0" rules="all" style="border-collapse: collapse;">
                        <tr style="display: none;">
                            <td width="385" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                支出交易笔数： <span class="red12">
                                    <asp:Label ID="lblOutCount" runat="server" Text="0"></asp:Label>
                                </span>
                            </td>
                            <td width="390" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                收入交易笔数： <span class="red12">
                                    <asp:Label ID="lblInCount" runat="server" Text="0"></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                本页支出金额合计： <span class="red12">
                                    <asp:Label ID="lblOutMoney" runat="server" Text="0.00"></asp:Label>
                                </span>
                            </td>
                            <td bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                本页收入金额合计： <span class="red12">
                                    <asp:Label ID="lblInMoney" runat="server" Text="0.00"></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 300px;">
                                <div style="margin-top: -35px;">
                                    支出金额总计： <span style="color: Red;">
                                        <asp:Label ID="lblOutMoneySUM" runat="server" Text="0.00"></asp:Label>
                                    </span>&nbsp;&nbsp;&nbsp; 收入金额总计： <span style="color: Red;">
                                        <asp:Label ID="lblInMoneySUM" runat="server" Text="0.00"></asp:Label>
                                    </span>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    function oclick(obj)
    {
        var test;
        var keyword;
        if (parseInt(obj) == 1)
        {
            keyword = $('#' + '<%=keyword1.ClientID %>');
            test = keyword.val();
            if (test == "输入用户名")
            {
                keyword.val("");
            }
        }
        else if (parseInt(obj) == 2)
        {
            keyword = $('#' + '<%=keyword2.ClientID %>');
            test = keyword.val();
            if (test == "输入流水号")
            {
                keyword.val("");
            }
        }
        else if (parseInt(obj) == 3)
        {
            keyword = $('#' + '<%=keyword3.ClientID %>');
            test = keyword.val();
            if (test == "输入方案编号")
            {
                keyword.val("");
            }
        }
    }
    function oblur(obj)
    {
        var test;
        var keyword;
        if (parseInt(obj) == 1)
        {
            keyword = $('#' + '<%=keyword1.ClientID %>');
            test = keyword.val();
            if (test == "")
            {
                keyword.val("输入用户名");
            }
        }
        else if (parseInt(obj) == 2)
        {
            keyword = $('#' + '<%=keyword2.ClientID %>');
            test = keyword.val();
            if (test == "")
            {
                keyword.val("输入流水号");
            }
        }
        else if (parseInt(obj) == 3)
        {
            keyword = $('#' + '<%=keyword3.ClientID %>');
            test = keyword.val();
            if (test == "")
            {
                keyword.val("输入方案编号");
            }
        }
    }
</script>
<script type="text/javascript">

    var pageIndex = parseInt("<%=PageIndex%>");
    var pageCount = parseInt("<%=PageCount%>");
    var dataCount = parseInt("<%=DataCount%>");

    $(function ()
    {

        var totalPage = pageCount;
        var totalRecords = dataCount;
        var pageNo = pageIndex;

        //初始化分页控件
        //有些参数是可选的，比如lang，若不传有默认值
        sand.init({
            pno: pageNo,
            //总页码
            total: totalPage,
            //总数据条数
            totalRecords: totalRecords,
            //链接前部
            hrefFormer: 'MonitoringLog',
            //链接尾部
            hrefLatter: '.aspx',
            getLink: function (n)
            {
                return "javascript:submit(" + n + ")";
            },
            lang: {
                prePageText: '上一页',
                nextPageText: '下一页',
                totalPageBeforeText: '共',
                totalPageAfterText: '页',
                totalRecordsAfterText: '条数据',
                gopageBeforeText: '转到',
                gopageButtonOkText: '确定',
                gopageAfterText: '页',
                buttonTipBeforeText: '第',
                buttonTipAfterText: '页'
            }
        });
        //生成
        sand.generPageHtml();
    });

    function submit(index)
    {
        $("#index").val(index);
        $("#form1").submit();
    }
</script>
<script type="text/javascript">
    window.onload = function ()
    {
        SetTableRowColor();
    }

    function showtable()
    {
        var mainTable = document.getElementById("tab");
        var li = mainTable.getElementsByTagName("tr");
        for (var i = 1; i <= li.length - 1; i++)
        {
            li[i].style.backgroundColor = "transparent";
            li[i].onmouseover = function ()
            {

                this.style.backgroundColor = "#fefdde";
            }
            li[i].onmouseout = function ()
            {

                this.style.backgroundColor = "transparent";
                SetTableRowColor();
            }
        }
    }

    showtable();

    function SetTableRowColor()
    {
        $("#tab tr:odd").css("background-color", "#F3F8FE");
        $("#tab tr:even").css("background-color", "#F7F7F7");
    }
</script>
<script type="text/javascript">
    var moneyAddTotal = 0.00;
    var moneySubTotal = 0.00;
    var handselAmountTotal = 0.00;
    var handselTotalTotal = 0.00;
    var rewardTotal = 0.00;
    var sumRewardTotal = 0.00;
    $("#tab tbody tr").each(function () {
        moneyAddTotal += parseFloat($(this).find(".MoneyAdd").text(), 10);
        moneySubTotal += parseFloat($(this).find(".MoneySub").text(), 10);
        handselAmountTotal += parseFloat($(this).find(".HandselAmount").text(), 10);
        handselTotalTotal += parseFloat($(this).find(".HandselTotal").text(), 10);
        rewardTotal += parseFloat($(this).find(".Reward").text(), 10);
        sumRewardTotal += parseFloat($(this).find(".SumReward").text(), 10);
    });
    $("#tdMoneyAddTotal").text(moneyAddTotal.toFixed(2));
    $("#tdMoneySubTotal").text(moneySubTotal.toFixed(2));
    $("#tdHandselAmountTotal").text(handselAmountTotal.toFixed(2));
    $("#tdHandselTotalTotal").text(handselTotalTotal.toFixed(2));
    $("#tdRewardTotal").text(rewardTotal.toFixed(2));
    $("#tdSumRewardTotal").text(sumRewardTotal.toFixed(2));
</script>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FinanceDistill.aspx.cs" Inherits="Admin_FinanceDistill" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户提款明细表
</title>
    
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
            height: 40px;
        }
        .newsTable tbody td
        {
            text-align: center;
        }
        .newsTable .time
        {
            width: 13%;
        }
        .newsTable .title
        {
            width: 8%;
        }
        .newsTable .isShow
        {
            width: 8%;
        }
        .newsTable .isRead
        {
            width: 8%;
        }
        .newsTable .edit
        {
            width: 8%;
            border-right: 1px solid #dfdfdf;
        }
        
        .btnEdit:hover
        {
            background: #D6F7FE;
        }
        
        .topbuttom
        {
            text-align: right;
            padding-right: 120px;
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
        .btnblue
        {
            width: 80px;
            height: 30px;
            background: url(../images/Sprite.png) no-repeat -405px -320px;
            border: 0;
            color: #fff;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" action="FinanceDistill.aspx" method="post">
    <div class="main">
        <div class="maincon">
            <h2>
                用户提款明细表</h2>
            <div class="financeDis_wrap">
                <table cellspacing="0" cellpadding="0" border="0" class="wraptable">
                    <tr>
                        <td>
                            <font face="微软雅黑">&nbsp;用户名：
                                <asp:TextBox ID="tbUserName" Font-Size="14px" runat="server"></asp:TextBox>&nbsp;
                                <asp:DropDownList ID="ddlYear" runat="server" Width="88px">
                                </asp:DropDownList>
                                &nbsp;
                                <asp:DropDownList ID="ddlMonth" runat="server" Height="19px" Font-Size="14px" Width="80px">
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
                                &nbsp; 状态：
                                <asp:DropDownList ID="ddlStatus" runat="server" Height="19px" Font-Size="14px" Width="80px">
                                    <asp:ListItem Value="100">全部</asp:ListItem>
                                    <asp:ListItem Value="0">申请中</asp:ListItem>
                                    <asp:ListItem Value="1">付款成功</asp:ListItem>
                                    <asp:ListItem Value="-1">已拒绝</asp:ListItem>
                                    <asp:ListItem Value="-2">用户撤销提款</asp:ListItem>
                                    <asp:ListItem Value="10">已接受提款</asp:ListItem>
                                    <asp:ListItem Value="11">支付宝处理中</asp:ListItem>
                                    <asp:ListItem Value="12">支付宝付款失败</asp:ListItem>
                                </asp:DropDownList>
                            </font>
                            <asp:Button ID="btnRead" runat="server" Text="读取数据" OnClick="btnRead_Click" CssClass="btnblue" />
                            <span style="color: #ff0000">(不输入用户名表示全部用户)</span><asp:TextBox ID="tbID" runat="server"
                                Width="100px" Visible="False"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table id="tab" class="newsTable" cellspacing="0" rules="all" style="border-collapse: collapse;">
                    <thead>
                        <tr>
                            <th class="isShow">
                                提款流水号
                            </th>
                            <th class="title">
                                用户名称
                            </th>
                            <th class="isShow">
                                 昵称
                            </th>
                            <th class="time">
                                申请时间
                            </th>
                            <th class="isRead">
                                提取金额
                            </th>
                            <%-- <th class="isRead">
                            手续费
                        </th>--%>
                            <th class="isRead">
                                状态
                            </th>
                            <th class="isRead">
                                银行提款
                            </th>
                            <th class="title">
                                银行卡号
                            </th>
                            <th class="edit">
                                备注
                            </th>
                            <th class="edit">
                                受理时间
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptSchemes" runat="server" EnableViewState="false">
                            <ItemTemplate>
                                <tr>
                                    <td class="time">
                                        <%#Eval("ID")%>
                                    </td>
                                    <td class="title">
                                        <a style="color: Blue; font-size: 14px;" href="UserDetail.aspx?SiteID=1&ID=<%# Eval("UserID")%>">
                                            <%#Eval("Name")%></a>
                                    </td>
                                    <td class="isShow">
                                        <%#Eval("NickName")%>
                                    </td>
                                    <td class="isRead">
                                        <%#Eval("DateTime")%>
                                    </td>
                                    <td class="isRead HandselMoney">
                                        <%#(Convert.ToDouble(Eval("Money"))+Convert.ToDouble(Eval("HandselMoney"))).ToString("0.00")%>
                                    </td>
                                    <%-- <td class="isRead">

                                       <%#Eval("FormalitiesFees")%>
                                </td>--%>
                                    <td class="isRead">
                                        <%# Apply((Eval("Result").ToString())) %>
                                    </td>
                                    <td class="isRead">
                                        <%# Eval("BankCardNumber").ToString() == "" ? "支付宝提款" : Eval("BankName")%>
                                    </td>
                                    <td class="title">
                                        <%# Eval("BankCardNumber").ToString() == "" ? "支付宝账号:" + Eval("AlipayName") : Eval("BankCardNumber")%>
                                    </td>
                                    <td class="isRead">
                                        <%#Eval("Memo")%>
                                    </td>
                                    <td class="isRead">
                                        <%#Eval("HandleDateTime")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>

                </table>
                <div id="sand" class="topbutton">
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" value="<%=PageIndex%>" id="PageIndex" name="PageIndex" />
    <input type="hidden" value="<%=PageCount%>" id="PageCount" name="PageCount" />
    <input type="hidden" value="<%=DataCount%>" id="DataCount" name="DataCount" />
    </form>
</body>
</html>
<script type="text/javascript">
    function thisSubmit(n) {

        $("#PageIndex").val(n);
        $("#form1").submit();
    }

    $(function () {


        var totalPage = $("#PageCount").val();
        var totalRecords = $("#DataCount").val();
        var pageNo = $("#PageIndex").val();

        //            var parameter = "?radom=" + Math.random() + "&lotteryid=" + lotteryID + "&filter=" + filter + "&isuseid=" + isuseID + "&search=" + search + "&sort=" + sort + "&order=" + order;
        //            //初始化分页控件
        //有些参数是可选的，比如lang，若不传有默认值

        sand.init({
            pno: pageNo,
            //总页码
            total: totalPage,
            //总数据条数
            totalRecords: totalRecords,
            //链接前部
            //                hrefFormer: 'schemeall',
            //                //链接尾部
            //                hrefLatter: '.aspx',

            getLink: function (n) {

                return 'javascript:thisSubmit(' + n + ')';
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
</script>
<script type="text/javascript">
    window.onload = function () {
        SetTableRowColor();
    }

    function showtable() {
        var mainTable = document.getElementById("tab");
        var li = mainTable.getElementsByTagName("tr");
        for (var i = 1; i <= li.length - 1; i++) {
            li[i].style.backgroundColor = "transparent";
            li[i].onmouseover = function () {

                this.style.backgroundColor = "#fefdde";
            }
            li[i].onmouseout = function () {

                this.style.backgroundColor = "transparent";
                SetTableRowColor();
            }
        }
    }

    showtable();

    function SetTableRowColor() {
        $("#tab tr:odd").css("background-color", "#F3F8FE");
        $("#tab tr:even").css("background-color", "#F7F7F7");
    } 
</script>
<script type="text/javascript">
    var handselMoneyTotal = 0.00;
    $("#tab tbody tr").each(function () {
        handselMoneyTotal += parseFloat($(this).find(".HandselMoney").text(), 10);
    });
    $("#tdHandselMoneyTotal").text(handselMoneyTotal.toFixed(2));
</script>

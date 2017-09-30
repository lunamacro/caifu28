<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FinanceWin.aspx.cs" Inherits="Admin_FinanceWin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户中奖明细表</title>
    
    <link href="../../Style/sandPage.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
    <link href="../Style/common.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/sandPage.js" type="text/javascript"></script>
    <script src="../Components/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="../Components/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript">
        function oncheck() {
            $(this).val("asda");
        }

        function oncheckStart() {
            var MoneyStart = $("#tbMoneyStart").val();
            if (MoneyStart != "") {
                if (isNaN(MoneyStart)) {
                    $("#tbMoneyStart").val("");
                    $("#tbMoneyStart").focus();
                    alert("请填写正确金额！");
                }
            }

        }
        function oncheckEnd() {
            var MoneyEnd = $("#tbMoneyEnd").val();
            if (MoneyEnd != "") {
                if (isNaN(MoneyEnd)) {
                    $("#tbMoneyEnd").val("");
                    $("#tbMoneyEnd").focus();
                    alert("请填写正确金额！");
                }
            }
        }
    </script>
    <style type="text/css">
        
        tr
        {
            height: 30px;
        }
        .wraptable tbody td
        {
            text-align: center;
        }
        .wraptable .time
        {
            width: 15%;
        }
        .wraptable .title
        {
            width: 20%;
        }
        .wraptable .isShow
        {
            width: 10%;
        }
        .wraptable .isRead
        {
            width: 10%;
        }
        .wraptable .edit
        {
            width: 15%;
            border-right: 1px solid #dfdfdf;
        }
        .wraptable .btnEdit
        {
            display: block;
            float: left;
            color: #3977C3;
            font-family: "微软雅黑";
            width: 50px;
            margin: 10px;
            height: 22px;
            line-height: 22px;
            text-align: center;
            border: 0px;
            text-decoration: underline;
            cursor: pointer;
            background: url('../images/Sprite.png') no-repeat scroll -205px -320px transparent;
        }
        .btnEdit:hover
        {
            background: url("../images/Sprite.png") no-repeat scroll -274px -320px rgba(0, 0, 0, 0);
        }
         #tab{ border:1px solid #dfdfdf;}
         #tab th{  border:1px solid #dfdfdf; background:#f7f7f7;}
         #tab td{  border:1px solid #dfdfdf; }
         .btnblue
        {
            width: 80px;
            height: 30px;
            background: url(../images/Sprite.png) no-repeat -405px -320px;
            border: 0;
            color: #fff;}
    </style>
</head>
<body>
    <form id="form1" runat="server" action="FinanceWin.aspx" method="post">
    <div class="main">
        <div class="maincon">
            <h2>
                用户中奖明细表</h2>
            <div class="financewin_wrap">
                <table cellspacing="0" cellpadding="0" border="0" class="topp">
                    <tr>
                        <td>
                            用户名：
                            <asp:TextBox ID="tbName" runat="server" Width="134px"></asp:TextBox>
                            彩种：<asp:DropDownList ID="ddlLottery" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLottery_SelectedIndexChanged"
                                Width="140px">
                            </asp:DropDownList>
                            &nbsp;&nbsp;&nbsp; 期号：<asp:DropDownList ID="ddlIsuse" runat="server" Width="140px">
                            </asp:DropDownList>
                            购买时间：<asp:TextBox ID="tbTimeStart" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"
                                Width="140px"></asp:TextBox>
                            至
                            <asp:TextBox ID="tbTimeEnd" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"
                                Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            购买方式：<asp:DropDownList ID="ddlFromClient" runat="server" Width="135px">
                                <asp:ListItem Value="0">全部</asp:ListItem>
                                <asp:ListItem Value="1">网站</asp:ListItem>
                                <asp:ListItem Value="2">手机</asp:ListItem>
                            </asp:DropDownList>
                            中奖金额：<asp:TextBox ID="tbMoneyStart" onblur="oncheck(this);" runat="server" Width="125px"></asp:TextBox>
                            至
                            <asp:TextBox ID="tbMoneyEnd" runat="server" onblur="oncheckEnd();" Width="125px"></asp:TextBox>
                            &nbsp; 真实购买：<asp:DropDownList ID="ddlIsBuy" runat="server" Height="25px" Width="50px">
                                <asp:ListItem Value="0">是</asp:ListItem>
                                <asp:ListItem Value="1">否</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;<asp:Button ID="btnRead" runat="server" Text="读取数据" OnClick="btnRead_Click" CssClass="btnblue" />&nbsp;&nbsp
                            <span style="color: Red; font-size: 13px;">(不输入用户名表示全部用户)</span>
                            <asp:TextBox ID="tbID" runat="server" Visible="False"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table id="tab" class="wraptable" cellspacing="0" rules="all" style="border-collapse:collapse;">
                    <thead>
                        <tr>
                            <th class="time">
                                用户名称
                            </th>
                            <th class="title">
                                时间
                            </th>
                            <th class="isShow">
                                中奖金额
                            </th>
                            <th class="isRead">
                                方案
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptSchemes" runat="server" EnableViewState="false">
                            <ItemTemplate>
                                <tr>
                                    <td class="time">
                                        <a style="color: Blue; font-size: 13px;" href="../Admin/UserDetail.aspx?SiteID=<%# Eval("SiteID") %>&ID=<%# Eval("UserID") %>">
                                            <%#Eval("Name")%></a>
                                    </td>
                                    <td class="title">
                                        <%#Eval("DateTime")%>
                                    </td>
                                    <td class="isShow Money">
                                        <%#  Shove._Convert.StrToDouble(Eval("Money").ToString(), 0).ToString("0.00")%>
                                    </td>
                                    <td class="edit">
                                        <a style="color: Blue;" href="Scheme.aspx?id=<%# Eval("SchemeId") %>" >查看方案</a>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td></td>
                            <td></td>
                            <td id="tdMoneyTotal" style="text-align:center"></td>
                            <td></td>
                        </tr>
                    </tfoot>
                </table>
                <div id="sand" class="topbutton">
                </div>
                <div class="winmoney_num">
                    中奖总金额：<span style="color: Red"><%= WinMoney %></span>
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
    var moneyTotal = 0.00;
    $("#tab tbody tr").each(function () {
        moneyTotal += parseFloat($(this).find(".Money").text(), 10);
    });
    $("#tdMoneyTotal").text(moneyTotal.toFixed(2));
</script>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SchemeList.aspx.cs" Inherits="Admin_SchemeList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    
    <title>彩票业务中心-方案查询</title>
    <link type="text/css" href="../Style/Site.css" rel="stylesheet" />
    <script src="../Components/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <style type="text/css">
        .numer {
            border: 1px solid #a3ccf8;
            padding: 0px 5px;
            height: 25px;
            width: 72%;
        }

        #tab {
            border-bottom: 1px solid #dfdfdf;
        }

            #tab th {
                border: 1px solid #dfdfdf;
            }

            #tab td {
                border: 1px solid #dfdfdf;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="title">
                方案查询
            </div>
            <div class="schemeQuery">
                <div class="searchList">
                    <table cellspacing="0" cellpading="0">
                        <tr>
                            <td class="td1">彩种:
                            </td>
                            <td class="td2">
                                <asp:DropDownList runat="server" ID="ddl_lotteryList" OnSelectedIndexChanged="ddl_lotteryList_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td class="td1">
                                <asp:CheckBox runat="server" ID="zuhe" Text="只看组合投注" />
                            </td>
                            <td class="td2">
                            </td>
                            <td class="td1">方案状态:
                            </td>
                            <td class="td2">
                                <asp:DropDownList runat="server" ID="ddl_schemeState">
                                    <asp:ListItem Value="1" Text="全部" Selected="True" />
                                    <asp:ListItem Value="2" Text="招募中" />
                                    <asp:ListItem Value="3" Text="未出票" />
                                    <asp:ListItem Value="4" Text="已出票" />
                                    <asp:ListItem Value="5" Text="已撤单" />
                                    <asp:ListItem Value="6" Text="已流单" />
                                    <asp:ListItem Value="7" Text="未中奖" />
                                    <asp:ListItem Value="8" Text="已中奖" />
                                </asp:DropDownList>
                            </td>
                            <td class="td1">方案号:
                            </td>
                            <td class="td2">
                                <%--        <asp:DropDownList runat="server" ID="ddl_winState">
                                    <asp:ListItem Value="1" Text="全部" Selected="True" />
                                    <asp:ListItem Value="2" Text="未开奖" />
                                    <asp:ListItem Value="3" Text="未中奖" />
                                    <asp:ListItem Value="4" Text="已中奖" />
                                    <%--<asp:ListItem Value="5" Text="已退款" />
                                </asp:DropDownList>--%>
                                <asp:TextBox CssClass="numer" runat="server" ID="txt_SchemeNumber" />
                            </td>
                            <td class="td4" rowspan="2">
                                <asp:Button runat="server" ID="btn_search" Text="搜索" CssClass="btn_operate" OnClick="btn_search_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td1">开始时间:
                            </td>
                            <td class="td3">
                                <asp:TextBox runat="server" ID="txt_startDate" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                            </td>
                            <td class="td1">截止时间:
                            </td>
                            <td class="td3">
                                <asp:TextBox runat="server" ID="txt_endDate" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                            </td>
                            <td class="td1">购彩方式:
                            </td>
                            <td class="td2">
                                <asp:DropDownList runat="server" ID="ddl_buyWay">
                                    <asp:ListItem Value="-1" Text="全部" Selected="True" />
                                    <asp:ListItem Value="1" Text="网站端" />
                                    <asp:ListItem Value="2" Text="安卓端" />
                                    <asp:ListItem Value="3" Text="苹果端" />
                                    <asp:ListItem Value="4" Text="手机站点" />
                                    <asp:ListItem Value="5" Text="其它" />
                                </asp:DropDownList>
                            </td>
                            <td class="td1">发起人:
                            </td>
                            <td class="td3">
                                <asp:TextBox runat="server" ID="txt_userName" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="dataList">
                    <table id="tab" cellspacing="0" rules="all" style="border-collapse: collapse;">
                        <thead>
                            <tr>
                                <th>彩种
                                </th>
                                <th>方案号
                                </th>
                                <th>发起人
                                </th>
                                <th>发起时间
                                </th>
                                <th>方案总金额
                                </th>
                                <th>税后奖金
                                </th>
                                <th style="display: none;">投注内容
                                </th>
                                <th>份数
                                </th>
                                <th>每份
                                </th>
                                <th>进度
                                </th>
                                <th>状态
                                </th>
                                <th>来源
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rpt_list">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%#Eval("LotteryName")%>
                                        </td>
                                        <td>
                                            <a href='Scheme.aspx?id=<%#Eval("ID")%>'>
                                                <%#Eval("SchemeNumber")%>
                                                &nbsp;&nbsp;
                                                (投注：<%#Eval("BetNum")%>)
                                            </a>
                                        </td>
                                        <td>
                                            <%#Eval("InitiateName")%>
                                        </td>
                                        <td>
                                            <%#Eval("DateTime")%>
                                        </td>
                                        <td>
                                            <%#Shove._Convert.StrToDouble(Eval("Money").ToString(), 0).ToString("0.00")%>
                                        </td>
                                        <td>
                                            <%# Shove._Convert.StrToDouble(Eval("WinMoney").ToString(), 0) == 0 ? "--" : Eval("WinMoney")%>
                                        </td>
                                        <td style="display: none;">
                                            <a href='../Home/Web/DownloadSchemeFile.aspx?id=<%#Eval("ID")%>' target="_blank">查看详细信息</a>
                                        </td>
                                        <td>
                                            <%#Eval("Share")%>
                                        </td>
                                        <td>
                                            <%#Shove._Convert.StrToDouble(Eval("EachShareMoney").ToString(),0).ToString("0.00")%></a>
                                        </td>
                                        <td style="color: Red;">
                                            <%#Eval("Schedule")%>%
                                        </td>
                                        <td style="color: Red;">
                                            <%# GetSchemeState(Shove._Convert.StrToInt(Eval("Share").ToString(), 0), Shove._Convert.StrToInt(Eval("BuyedShare").ToString(), 0), Shove._Convert.StrToBool(Eval("Buyed").ToString(), false), Shove._Convert.StrToShort(Eval("QuashStatus").ToString(), 0), Shove._Convert.StrToBool(Eval("IsOpened").ToString(), false), Shove._Convert.StrToDouble(Eval("WinMoney").ToString(), 0))%>
                                        </td>
                                        <td>
                                            <%#TranComeFrom(Eval("FromClient").ToString())%><%#TranHomeFrom(Eval("HomeIndex").ToString())%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <div id="sand" style="text-align: right; padding-right: 30px;" runat="server">
                </div>
            </div>
        </div>
    </form>
</body>
<link href="../../Style/sandPage.css" rel="stylesheet" type="text/css" />
<script src="../../JScript/sandPage.js" type="text/javascript"></script>
<script src="../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $("#ddl_schemeState").change(function () {
        $("#ddl_winState option").eq(0).attr("selected", "selected");
    });
    $("#ddl_winState").change(function () {
        $("#ddl_schemeState option").eq(0).attr("selected", "selected");
    });
</script>
<script type="text/javascript">

    var pageIndex = parseInt("<%=PageIndex%>");
    var pageCount = parseInt("<%=PageCount%>");
    var dataCount = parseInt("<%=DataCount%>");

    $(function () {
        var totalPage = pageCount;
        var totalRecords = dataCount;
        var pageNo = pageIndex;
        var zuheCheck = $("#zuhe").is(':checked') ? "yes" : "no";
        var parameter = "?lotteryID=" + $("#ddl_lotteryList option:selected").val() + "&issueID=" + $("#ddl_issueList option:selected").val() + "&schemeState=" + $("#ddl_schemeState option:selected").val() + "&winState=" + $("#ddl_winState option:selected").val();
        parameter += "&startTime=" + $("#txt_startDate").val() + "&endTime=" + $("#txt_endDate").val() + "&buyWay=" + $("#ddl_buyWay option:selected").val() + "&userName=" + $("#txt_userName").val() + "&zuheValue=" + zuheCheck;
        //初始化分页控件
        //有些参数是可选的，比如lang，若不传有默认值
        sand.init({
            pno: pageNo,
            //总页码
            total: totalPage,
            //总数据条数
            totalRecords: totalRecords,
            //链接前部
            hrefFormer: 'SchemeList',
            //链接尾部
            hrefLatter: '.aspx',
            getLink: function (n) {
                return this.hrefFormer + this.hrefLatter + parameter + "&PageIndex=" + n;
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
        $("#sand_btn_go_input").attr("onkeypress", "");
        $("#sand_btn_go_input").keyup(function () {
            var value = parseFloat($(this).val());
            if (isNaN(value)) {
                value = 1;
            }
            $(this).val(value);
        });
    });
</script>
</html>
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

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Isuse.aspx.cs" Inherits="Admin_Isuse" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    
    <title>彩票业务中心-期号列表</title>
    <link type="text/css" href="../Style/Site.css" rel="stylesheet" />
    <style>
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
                期号列表
            </div>
            <div class="Isuse">
                <div class="searchList">
                    <table cellspacing="0" cellpading="0">
                        <tr>
                            <td class="td1">彩种
                            </td>
                            <td class="td2">
                                <asp:DropDownList ID="ddlLottery" runat="server" Width="144px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlLottery_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td align="center" width="120px">
                                <asp:Button runat="server" ID="btn_add" CssClass="btn_operate" Text="增加" OnClick="btn_add_Click" />
                            </td>
                            <td>已添加的最后期号：<asp:Literal ID="lt_lastIssue" runat="server" Text="-" />， 开始时间：<asp:Literal
                                ID="lt_lastIssueStartTime" runat="server" Text="-" />， 截止时间：<asp:Literal ID="lt_lastIssueEndTime"
                                    runat="server" Text="-" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="dataList">
                    <table cellspacing="0" rules="all" style="border-collapse: collapse;" id="tab">
                        <thead>
                            <tr>
                                <th>期号名称
                                </th>
                                <th>开始时间
                                </th>
                                <th>截止时间
                                </th>
                                <th>修改
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rpt_issueList">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%#Eval("Name") %>
                                        </td>
                                        <td>
                                            <%#Eval("StartTime", "{0:yyyy-MM-dd HH:mm:ss}")%>
                                        </td>
                                        <td>
                                            <%#Eval("EndTime","{0:yyyy-MM-dd HH:mm:ss}") %>
                                        </td>
                                        <td>
                                            <%#GetEditLinkByLotteryID(Eval("LotteryID").ToString(), Eval("ID").ToString())%>
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
        <asp:HiddenField runat="server" ID="hide_lotteryID" />
    </form>
</body>
<link href="../../Style/sandPage.css" rel="stylesheet" type="text/css" />
<script src="../../JScript/sandPage.js" type="text/javascript"></script>
<script src="../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $("#btn_add").click(function () {
        if ("-1" == $("#ddlLottery option:selected").val()) {
            alert("请选择彩种。");
            return false;
        }
        return true;
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
        var parameter = "?LotteryID=" + $("#hide_lotteryID").val();
        //初始化分页控件
        //有些参数是可选的，比如lang，若不传有默认值
        sand.init({
            pno: pageNo,
            //总页码
            total: totalPage,
            //总数据条数
            totalRecords: totalRecords,
            //链接前部
            hrefFormer: 'Isuse',
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

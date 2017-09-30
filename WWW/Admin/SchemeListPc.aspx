<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SchemeListPc.aspx.cs" Inherits="Admin_SchemeListPc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title>彩票业务中心-PC28方案查询</title>
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
                PC28方案查询
            </div>
            <div class="schemeQuery">
                <div class="searchList" style="height: 130px;">
                    <table cellspacing="0" cellpading="0">
                       
                        <tr>
                            <td class="td1">开始时间:
                            </td>
                            <td class="td3">
                                <asp:TextBox runat="server" ID="txt_startDate" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
                            </td>
                            <td class="td1">截止时间:
                            </td>
                            <td class="td3">
                                <asp:TextBox runat="server" ID="txt_endDate" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
                            </td>
                             
                            <td class="td4" rowspan="2">
                                <asp:Button runat="server" ID="btn_rebate" Text="回水" CssClass="btn_operate" OnClick="btn_Rebate_Click" />
                                <asp:Label runat="server" ID="lab_rebate" Text="今天已经点过回水了" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td1"> 发起人:
                            </td>
                            <td class="td2">
                               <asp:TextBox runat="server" ID="txt_userName" />
                            </td>

                           <td class="td1">搜索：
                            </td>
                            <td class="td2">
                                 <asp:Button runat="server" ID="btn_search" Text="搜索" CssClass="btn_operate" OnClick="btn_search_Click" />
                            </td>
                            
                           
                        </tr>
                    </table>
                </div>
                <div style="clear: both;"></div>
                <div class="dataList" style="display: block; margin-top: 10px;">
                    <table id="tab" cellspacing="0" rules="all" style="border-collapse: collapse;">
                        <thead>
                            <tr>
                               
                                <th rowspan="2">会员
                                </th>
                                <th colspan="3">玩法一
                                </th>
                                <th colspan="3">玩法二
                                </th>
                                <th colspan="3">玩法三
                                </th>
                            </tr>
                            <tr>
                                <th>投注
                                </th>
                                <th>奖金
                                </th>
                                <th style="color:red">盈亏
                                </th>
                                <th>投注
                                </th>
                                <th>奖金
                                </th>
                                <th style="color:red">盈亏
                                </th>
                                <th>投注
                                </th>
                                <th>奖金
                                </th>
                                <th style="color:red">盈亏
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rpt_list">
                                <ItemTemplate>
                                    <tr>
                                        
                                        <td>
                                            <a href='UserDetail.aspx?SiteID=1&id=<%#Eval("uid")%>'><%#Eval("uname")%></a>
                                        </td>                                       
                                        <td>
                                            <%#Shove._Convert.StrToDouble(Eval("pay0").ToString(), 0).ToString("0.00")%>
                                        </td>
                                        <td>
                                            <%#Shove._Convert.StrToDouble(Eval("win0").ToString(), 0).ToString("0.00")%>
                                        </td>
                                        <td style="color:red">
                                            <%#Shove._Convert.StrToDouble(Eval("lose0").ToString(),0).ToString("0.00")%>
                                        </td>
                                         <td>
                                            <%#Shove._Convert.StrToDouble(Eval("pay1").ToString(), 0).ToString("0.00")%>
                                        </td>
                                        <td>
                                            <%#Shove._Convert.StrToDouble(Eval("win1").ToString(), 0).ToString("0.00")%>
                                        </td>
                                        <td style="color:red">
                                            <%#Shove._Convert.StrToDouble(Eval("lose1").ToString(),0).ToString("0.00")%>
                                        </td>
                                         <td>
                                            <%#Shove._Convert.StrToDouble(Eval("pay2").ToString(), 0).ToString("0.00")%>
                                        </td>
                                        <td>
                                            <%#Shove._Convert.StrToDouble(Eval("win2").ToString(), 0).ToString("0.00")%>
                                        </td>
                                        <td style="color:red">
                                            <%#Shove._Convert.StrToDouble(Eval("lose2").ToString(),0).ToString("0.00")%>
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
        var parameter = "?lotteryID=" + $("#ddl_lotteryList option:selected").val() + "&issueID=" + $("#ddl_issueList option:selected").val() + "&schemeState=" + $("#ddl_schemeState option:selected").val() + "&winState=" + $("#ddl_winState option:selected").val();
        parameter += "&startTime=" + $("#txt_startDate").val() + "&endTime=" + $("#txt_endDate").val() + "&buyWay=" + $("#ddl_buyWay option:selected").val() + "&userName=" + $("#txt_userName").val();
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

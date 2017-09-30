<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SchemeQuash2.aspx.cs" Inherits="Admin_SchemeQuash2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    
    <title>彩票业务中心-方案撤单</title>
    <link type="text/css" href="../Style/Site.css" rel="stylesheet" />
    <style>
        #tab{ border-bottom:1px solid #dfdfdf;}
        #tab th{  border:1px solid #dfdfdf;}
        #tab td{  border:1px solid #dfdfdf; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="title">
                方案撤单
            </div>
            <div class="schemeQuash">
                <div class="searchList">
                    <table cellspacing="0" cellpading="0">
                        <tr>
                            <td class="td1">彩种:</td>
                            <td class="td2">
                                <asp:DropDownList ID="ddlLottery" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLottery_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td class="td3">
                                <asp:RadioButton runat="server" ID="chk_schemeNumber" GroupName="type" Text="方案编号" Checked="true"/>
                                <asp:RadioButton runat="server" ID="chk_userName" GroupName="type"  Text=" 用户名"/>
                            </td>
                            <td class="td4">
                                <asp:TextBox runat="server" ID="txt_schemeNumberAndUserName" Text="" placeholder="方案号/用户名" />
                            </td>
                            <td class="td5">
                                <asp:Button runat="server" ID="btn_search" Text="搜索" CssClass="btn_operate" 
                                    onclick="btn_search_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="dataList">
                    <table id="tab" cellspacing="0" rules="all" style="border-collapse:collapse;">
                        <thead>
                            <tr>
                                <th>发起人</th>
                                <th>方案编号</th>
                                <th>彩种</th>
                                <th>倍数</th>
                                <th>金额</th>
                                <th>进度</th>
                                <th>撤单</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rpt_list">
                                <ItemTemplate>
                                    <tr>
                                        <td><asp:HiddenField runat="server" ID="hide_id" Value='<%#Eval("ID")%>' /> <%#Eval("InitiateName")%></td>
                                        <td><a href='Scheme.aspx?id=<%#Eval("ID")%>'> <%#Eval("SchemeNumber")%></a></td>
                                        <%--<td><a href='../Home/Web/DownloadSchemeFile.aspx?id=<%#Eval("ID")%>' target="_blank">查看详细信息</a></td>--%>
                                        <td><%#Eval("LotteryName")%></td>
                                        <td><%#Eval("Multiple")%></td>
                                        <td><%#Convert.ToDouble(Eval("Money")).ToString("0.00")%></td>
                                        <td><asp:HiddenField runat="server" ID="hide_Schedule" Value='<%#Eval("Schedule")%>' /><%#Eval("Schedule")%>%</td>
                                        <td><asp:Button runat="server" ID="btnQuash" Text="撤单" OnClick="btnQuash_click" OnClientClick="return showConfirmAndDisabledButton('确认要撤销这个方案吗?',this);" /></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <div id="sand" style=" text-align:right; padding-right:30px;" runat="server"></div>
            </div>
        </div>
    </form>
</body>
    <link href="../../Style/sandPage.css" rel="stylesheet" type="text/css" />
    <script src="../../JScript/sandPage.js" type="text/javascript"></script>
    <script src="../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">
//        $("#chk_schemeNumber").click(function () {
//            $("#txt_schemeNumberAndUserName").val("");
//        });
//        $("#chk_userName").click(function () {
//            $("#txt_schemeNumberAndUserName").val("");
//        });

        //去除空格
        function myTirm(val) {
            return val.replace(/[\s]/g, "");
        }

        /*
        *   显示Confirm对话框并且禁用按钮
        */
        function showConfirmAndDisabledButton(message,btn) {
//            $(btn).attr("disabled", "disabled")
//            if (confirm(message)) {
//                $(btn).removeAttr("disabled");
//                return true;
//            }
//            $(btn).removeAttr("disabled");
            //            return false;
            return confirm(message);
        }
    </script>
    <script type="text/javascript">

        var pageIndex = parseInt("<%=PageIndex%>");
        var pageCount = parseInt("<%=PageCount%>");
        var dataCount = parseInt("<%=DataCount%>");

        $(function () {
            var totalPage = pageCount;
            var totalRecords = dataCount;
            var pageNo = pageIndex;
            var parameter = "";
            //初始化分页控件
            //有些参数是可选的，比如lang，若不传有默认值
            sand.init({
                pno: pageNo,
                //总页码
                total: totalPage,
                //总数据条数
                totalRecords: totalRecords,
                //链接前部
                hrefFormer: 'SchemeQuash2',
                //链接尾部
                hrefLatter: '.aspx',
                getLink: function (n) {
                    return this.hrefFormer + this.hrefLatter + parameter + "?LotteryID=" + $("#ddlLottery option:selected").val() + "&PageIndex=" + n + "&searchType=" + ($("#chk_schemeNumber").is(":checked") ? "schemeNumber" : "userName");
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
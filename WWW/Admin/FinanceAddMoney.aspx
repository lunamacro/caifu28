<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FinanceAddMoney.aspx.cs"
    Inherits="Admin_FinanceAddMoney" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户充值明细表</title>

    <script src="../Components/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../../Style/sandPage.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
    <link href="../Style/common.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/common.js" type="text/javascript"></script>
    <link href="../Style/jquery.mCustomScrollbar.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/jquery.mCustomScrollbar.concat.min.js" type="text/javascript"></script>
    <script src="../JScript/sandPage.js" type="text/javascript"></script>
    <style type="text/css">
        tr {
            height: 30px;
        }

        .newsTable tbody td {
            text-align: center;
        }

        .newsTable .time {
            width: 10%;
        }

        .newsTable .title {
            width: 10%;
        }

        .newsTable .isShow {
            width: 10%;
        }

        .newsTable .isRead {
            width: 10%;
        }

        .newsTable .edit {
            width: 10%;
        }

        .newsTable .btnEdit {
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

        .btnEdit:hover {
            background: #D6F7FE;
        }

        #btn {
            display: block;
            height: 32px;
            width: 82px;
            background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#C2E6FE), to(#C2E6FE));
            border: 0px;
            cursor: pointer;
        }

            #btn:hover {
                border: 1px solid #3977C3;
            }


        #sand Button {
            padding: 5px 12px;
            margin: 10px 3px;
            font-size: 13px;
            border: 1px solid #DFDFDF;
            background-color: #FFF;
            color: #9a9a9a;
            text-decoration: none; /*-moz-border-radius: 4px;     -webkit-border-radius: 4px; border-radius: 4px;*/
        }

        .tobbutton {
            font-size: 12px;
            text-align: right;
            padding-right: 135px;
            margin-top: 30px;
        }

        .tabttom {
            margin-left: 50px;
        }

        #tab {
            border: 1px solid #dfdfdf;
        }

            #tab th {
                border: 1px solid #dfdfdf;
                background: #f7f7f7;
            }

            #tab td {
                border: 1px solid #dfdfdf;
            }

        .td1 {
            width: 110px;
            text-align: right;
        }

        .toptable tr {
            line-height: 40px;
        }

        .td2 {
            width: 500px;
        }

        .td3 {
            width: 100px;
        }

            .td2 input, .td3 input {
                padding: 5px 10px;
                font-size: 13px;
                border: 1px solid #A3CCF8;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="maincon">
                <h2>用户充值明细表</h2>
                <div class="financedd_wrap">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0" class="toptable">
                        <tr>
                            <td class="td1">用户名：</td>
                            <td class="td2">
                                <input type="text" runat="server" onblur="oblur(1)" onclick="oclick(1)" id="keyword1" value="" placeholder="输入用户名" style="color: rgb(153, 153, 153); font-size: 14px; width: 115px;" />
                                <span style="color: #ff0000">(不输入用户名表示全部用户)</span>
                            </td>
                            <td class="td1" style="display:none">流水号：</td>
                            <td class="td3" style="display:none">
                                <input type="text" onkeyup="value=value.replace(/[^\d]/g,'')"
                                    onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g,''))"
                                    runat="server" onclick="oclick(2)" id="keyword2" size="22" value="" placeholder="输入输入流水号" style="color: rgb(153, 153, 153); font-size: 15px; width: 115px;" />
                            </td>
                            <td class="td1" style="display:none">支付商流水号：
                            </td>
                            <td class="td3" style="display:none">
                                <input type="text" runat="server" onblur="oblur(3)"
                                    onclick="oclick(3)" id="keyword3" size="22" value="" placeholder="输入输入支付商流水号" style="color: rgb(153, 153, 153); font-size: 15px; width: 125px;" />
                            </td>
                            <td style="width: 50px;">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="td1">开始时间：
                            </td>
                            <td class="td3">
                                <input type="text" id="tbStartTime" readonly="readonly"
                                    onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd' })" value="" style="font-size: 15px;"
                                    runat="server" />
                                至：
                                <input type="text" id="tbEndTime" readonly="readonly" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd' })"
                                    value="" style="font-size: 15px;" runat="server" />
                            </td>
                            <td class="td1">是否成功：
                            </td>
                            <td>
                                <asp:DropDownList Width="120" ID="ddlResult" Height="20px"
                                    Font-Size="14px" runat="server">
                                    <asp:ListItem Value="-1" Text="全部"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="是"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="否"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: left; padding-left: 55px;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button runat="server" Text="查询" ID="btnSearch" OnClick="btnSearch_Click" />
                                <asp:Button runat="server" Text="导出" ID="btnOut" OnClick="btnOut_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">支付金额总计： <span style="color: Red;">
                                <asp:Label ID="lblSumPayMoney" runat="server" Text="0.00"></asp:Label></span>元
                            &nbsp; &nbsp; &nbsp;
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td>
                                <asp:CheckBox ID="cbIsPayTime" runat="server"
                                    Height="22px" Width="50px" />&nbsp;
                            <asp:Label ID="Label1" runat="server"> 充值发起的时间：</asp:Label>
                            </td>
                            <td>银行：<asp:DropDownList Width="120" ID="ddlBankName" Height="20px"
                                Font-Size="14px" runat="server">
                            </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table id="tab" class="newsTable" cellspacing="0" rules="all" style="border-collapse: collapse;">
                        <thead>
                            <tr>
                                <th class="time">用户名
                                </th>
                                <th class="time">微信昵称
                                </th>
                                <th class="title">流水号
                                </th>
                                <th class="isShow">付款账号信息
                                </th>
                                <th class="isRead">时间
                                </th>
                                <th class="isRead">支付方式
                                </th>
                                <th class="isRead">支付金额
                                </th>
                                <th class="isRead">彩金(赠送)
                                </th>
                                <th class="edit">状态
                                </th>
                                <th class="edit">操作
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptSchemes" runat="server" EnableViewState="false">
                                <ItemTemplate>
                                    <tr>
                                        <td class="time">
                                            <a style="color: Blue;" href="UserDetail.aspx?SiteID=1&ID=<%#Eval("UserID")%>">
                                                <%#Eval("Name")%></a>
                                        </td>
                                        <td class="title">
                                            <%#Eval("NickName")%>
                                        </td>
                                        <td class="title">
                                            <%#Eval("PayNumber")%>
                                        </td>
                                        <td class="isShow">
                                            <%#Eval("AlipayNo")%>
                                        </td>
                                        <td class="isRead">
                                            <%#Eval("DateTime")%>
                                        </td>
                                        <td class="isRead">
                                            <%# getPayType( Eval("PayType").ToString())%>
                                        </td>
                                        <td style="display: none;">
                                            <asp:Label ID="Label1" runat="server"><%#Eval("UserID")%></asp:Label>
                                        </td>
                                        <td class="edit">
                                            <%#  (Convert.ToDouble(Eval("Money"))).ToString("0.00") %>
                                        </td>
                                        <td class="edit">
                                            <%#  (Convert.ToDouble(Eval("HandselMoney"))).ToString("0.00") %>
                                        </td>
                                        <td class="edit">
                                            <div id="result_changeStatus_<%#Eval("PayNumber") %>"><%# getChargeStat(Eval("Result").ToString()) %></div>
                                        </td>
                                        <td class="edit">
                                            <a id="changeStatus_<%#Eval("PayNumber") %>" onclick="changeStatus('changeStatus_<%#Eval("PayNumber") %>')" style="cursor: pointer;"><u>修改状态</u></a>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                    <div id="sand" style="text-align: right; padding-right: 30px;"></div>
                    <input name="index" value="1" id="index" type="hidden" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
<script type="text/javascript">

    var pageIndex = parseInt("<%=PageIndex%>");
    var pageCount = parseInt("<%=PageCount%>");
    var dataCount = parseInt("<%=DataCount%>");

    $(function () {

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
            hrefFormer: 'FinanceAddMoney',
            //链接尾部
            hrefLatter: '.aspx',
            getLink: function (n) {
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

    function submit(index) {
        $("#index").val(index);
        $("#form1").submit();
    }
</script>
<script type="text/javascript">
    $(function () {
        intil();
    });
    function intil() {
        var cb = $("#" + "<%=cbIsPayTime.ClientID %>");

        cb.click(function () {
            if ($(this).prop("checked")) {

                $("#" + "<%=tbStartTime.ClientID %>").prop("disabled", false);
                $("#" + "<%=tbEndTime.ClientID %>").prop("disabled", false);
            }
            else {
                $("#" + "<%=tbStartTime.ClientID %>").prop("disabled", true);
                $("#" + "<%=tbEndTime.ClientID %>").prop("disabled", true);
            }
        });
    }
    function oclick(obj) {
        var test;
        var keyword;
        if (parseInt(obj) == 1) {
            keyword = $('#' + '<%=keyword1.ClientID %>');
            test = keyword.val();
            if (test == "输入用户名") {
                keyword.val("");
            }
        }
        else if (parseInt(obj) == 2) {
            keyword = $('#' + '<%=keyword2.ClientID %>');
            test = keyword.val();
            if (test == "输入流水号") {
                keyword.val("");
            }
        }
        else if (parseInt(obj) == 3) {
            keyword = $('#' + '<%=keyword3.ClientID %>');
                test = keyword.val();
                if (test == "输入支付商流水号") {
                    keyword.val("");
                }
            }

}
function oblur(obj) {
    var test;
    var keyword;
    if (parseInt(obj) == 1) {
        keyword = $('#' + '<%=keyword1.ClientID %>');
        test = keyword.val();
        if (test == "") {
            keyword.val("输入用户名");
        }
    }
    else if (parseInt(obj) == 2) {
        keyword = $('#' + '<%=keyword2.ClientID %>');
        test = keyword.val();
        if (test == "") {
            keyword.val("输入流水号");
        }
    }
    else if (parseInt(obj) == 3) {
        keyword = $('#' + '<%=keyword3.ClientID %>');
            test = keyword.val();
            if (test == "") {
                keyword.val("输入支付商流水号");
            }
        }
}
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
    function changeStatus(obj) {
        var control = $("#" + obj);
        $(control).hide();
        var html = "<div id='div_" + obj + "'><select id='select_" + obj + "'><option value='1'>完成</option><option value='0'>未完成</option><option value='-1'>拒绝</option></select> <input type=button value=保存 onclick='saveActionbutton(\"" + obj + "\")'> <input type=button value=取消 onclick='removeActionButton(\"" + obj + "\")'></div>";
        $(control).after(html);
    }
    function removeActionButton(obj) {
        $("#div_" + obj).remove();
        $("#" + obj).show();
    }
    function saveActionbutton(obj) {
        var seleVal = $("#select_" + obj).val();
        if (confirm("确认修改此条记录的状态吗?")) {
            switch (seleVal) {
                case "0":
                    $("#result_" + obj).html("<span style=\"color:Red;\">未成功</span>");
                    break;
                case "1":
                    $("#result_" + obj).html("<font color='#11cc22'>成功</font>");
                    break;
                case "-1":
                    $("#result_" + obj).html("<font color='#1122ff'>拒绝</font>");
                    break;
            }
            //数据后台操作
            var handlerUrl = "/Admin/Handler/FinanceAddMoney.ashx";
            var successFunc = function (json) {

            };
            f_ajaxPost(handlerUrl, { "act": "ResetStatus", "ID": obj.substring(13, obj.length), "Result": seleVal })
            removeActionButton(obj);
        }
    }
</script>

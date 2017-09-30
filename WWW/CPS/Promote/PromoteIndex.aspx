<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PromoteIndex.aspx.cs" Inherits="CPS_Promote_PromoteIndex" %>

<%@ Register Src="../userControls/Footer.ascx" TagName="Footer" TagPrefix="uc1" %>
<%@ Register Src="../userControls/PromoteHeader.ascx" TagName="PromoteHeader" TagPrefix="uc2" %>
<%@ Register Src="../userControls/PromoteAccount.ascx" TagName="PromoteAccount" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-推广联盟</title>
    <meta name="description" content="<%=_Site.Name %>-推广联盟" />
    <meta name="keywords" content="<%=_Site.Name %>-推广联盟" />
    <link type="text/css" href="../css/common.css" rel="stylesheet" />
    <style type="text/css">
        .number_record h4 span {
            padding: 0px 10px;
            cursor: pointer;
        }

            .number_record h4 span.curr {
                color: orange;
            }

        .sidebar li {
            width: 248px;
        }
         .insicon {width:100%}

        .main {margin:10px}
        .newsTable {width:100%}
        .newsTable tr {
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
            border-right: 1px solid #dfdfdf;
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

        #btnSearch {
            border-radius: 5px;
            width: 70px;
            height: 30px;
            background-color: #529DE6;
            color: White;
        }

        #btnOut {
            border-radius: 5px;
            width: 70px;
            height: 30px;
            background-color: #529DE6;
            color: White;
        }

        #btnSearch:hover {
            border: 1px solid #3977C3;
        }

        #btnOut:hover {
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

        .toptable {
            margin-left: 50px;
            width: 85%;
        }

        .top {
            margin-left: 50px;
        }

        .topbuttom {
            text-align: right;
            padding-right: 30px;
            margin: 30px 0;
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

        .tb_money {
            width: 96%;
            text-align: left;
        }

            .tb_money span {
                margin-right: 15px;
            }

        .demo-chat {
            width: 50%;
            margin: 0 auto;
        }

        .table_chart {
            width: 800px;
            height: 350px;
        }

            .table_chart th {
                text-align: center;
                border-left: solid 1px #DFDFDF;
                height: 40px;
            }

            .table_chart td {
                text-align: center;
                border-bottom: solid 1px #DFDFDF;
                width: 40px;
            }

                .table_chart td dl {
                    margin: 0 auto;
                    width: 40px;
                }

                .table_chart td dt {
                    border-top: solid 1px #DFDFDF;
                    border-right: solid 1px #DFDFDF;
                    border-left: solid 1px #DFDFDF;
                    width: 40px;
                }

                    .table_chart td dt.red {
                        background: #D60004;
                        background: -moz-linear-gradient(top, #FF0509 0%, #D60004 100%);
                        background: -webkit-gradient(linear, left top, left bottom, color-stop(0%, #FF0509), color-stop(100%, #D60004));
                        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#FF0509', endColorstr='#D60004', GradientType=0);
                        border-color: #C70003;
                        box-shadow: 0px 0px 1px #FFF inset;
                        border-radius: 3px 3px 0px 0px;
                    }

                    .table_chart td dt.green {
                        background: #278F24;
                        background: -moz-linear-gradient(top, #2DA329 0%, #278F24 100%);
                        background: -webkit-gradient(linear, left top, left bottom, color-stop(0%, #2DA329), color-stop(100%, #278F24));
                        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#2DA329', endColorstr='#278F24', GradientType=0);
                        border-color: #227A1F;
                        box-shadow: 0px 0px 1px #FFF inset;
                        border-radius: 3px 3px 0px 0px;
                    }

                    .table_chart td dt.none {
                        border: none;
                    }

                    .table_chart td dt:hover {
                        opacity: 0.65;
                    }

                .table_chart td dd {
                    position: absolute;
                    width: 40px;
                }

        .div_month {
            width: 100%;
            height: 20px;
            margin-bottom: 5px;
        }

            .div_month .mname {
                float: left;
                width: 25%;
                text-align: right;
            }

            .div_month .mmoney {
                float: left;
                width: 75%;
                text-align: left;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc2:PromoteHeader ID="PromoteHeader1" runat="server" />
    <!--Header HTML end-->
    <div class="user_banner">
        <h3><%=subTitle %><span>My Team</span></h3>
    </div>
    <!--Banner HTML end-->
    <!--Banner HTML end-->
    <div class="content">
        <div class="inside_bg">
            <div class="insicon">
                <div class="sidebar">
                    <ul>
                        <li class="curr"><a href="javascript:void(0);" onclick="location.reload()"><%=subTitle %></a></li>
                    </ul>
                </div>
                 <div class="main">
            <div class="maincon">
                <div class="finanbalan_wrap">
                    <table cellspacing="0" cellpadding="0" border="0" class="wraptable" style="margin-bottom: 10px">
                        <tr>
                            <td>开始时间：
                                            <asp:DropDownList ID="ddlYear0" Height="20px" runat="server">
                                            </asp:DropDownList>
                                <asp:DropDownList ID="ddlMonth0" runat="server" AutoPostBack="True" Height="20px"
                                    OnSelectedIndexChanged="ddlMonth0_SelectedIndexChanged">
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
                                <asp:DropDownList ID="ddlDay0" Height="20px" runat="server">
                                </asp:DropDownList>
                                结束时间：
                                            <asp:DropDownList ID="ddlYear1" Height="20px" runat="server">
                                            </asp:DropDownList>
                                <asp:DropDownList ID="ddlMonth1" runat="server" Height="20px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlMonth1_SelectedIndexChanged">
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
                                用户关键字：
                            <asp:TextBox ID="txtUserKeyword" runat="server"></asp:TextBox>
                                &nbsp;&nbsp&nbsp
                                            <asp:Button runat="server" ID="btnGO" Text="查询" Style="cursor: pointer;" OnClick="btnGO_Click" />
                            </td>
                            <td style="display: none">时间: <font face="微软雅黑">&nbsp;<asp:DropDownList ID="ddlYear" Height="25px" runat="server"
                                Width="88px" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                            </asp:DropDownList>
                                &nbsp;
                                <asp:DropDownList ID="ddlMonth" runat="server" Width="80px" Height="25px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
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
                                &nbsp; </font>
                            </td>
                        </tr>
                    </table>
                    <table class="tb_money" cellspacing="0" cellpadding="0" border="0" style="width: 96%; margin-left: 30px; display: none;">
                        <tr>
                            <td>合计购彩交易额：<asp:Label runat="server" ID="lb_Buy" ForeColor="Red" />
                                合计充值金额：<asp:Label runat="server" ID="lb_SurrogateIn" ForeColor="Red" />
                                合计中奖金额：<asp:Label runat="server" ID="lb_WinMoney" ForeColor="Green" />
                                合计佣金：<asp:Label runat="server" ID="lb_CPS" ForeColor="Green" />
                                合计兑换金额：<asp:Label runat="server" ID="lb_ExpertsIn" ForeColor="Green" />
                                合计盈利：<asp:Label runat="server" ID="lb_Earning" />
                            </td>
                        </tr>
                    </table>
                    <table id="tab" class="newsTable" cellspacing="0" rules="all" style="border-collapse: collapse; margin-top: 10px">
                        <thead>
                            <tr>
                                <%--
                            <th style="width: 5%">
                                日
                            </th>
                            <th style="width: 10%">
                                购彩交易额
                            </th>
                            <th style="width: 9%">
                                充值
                            </th>
                            <th style="width: 9%">
                                彩金
                            </th>
                            <th style="width: 9%">
                                中奖金额
                            </th>
                            <th style="width: 9%">
                                积分兑换金额
                            </th>
                            <th style="width: 9%">
                                保底冻结金额
                            </th>
                            <th style="width: 9%">
                                追号冻结金额
                            </th>
                            <th style="width: 9%">
                                提款冻结金额
                            </th>
                            <th style="width: 9%">
                                可提款金额
                            </th>
                            <th style="width: 10%">
                                盈利(不含佣金)
                            </th>
                                --%>
                                <th>用户名
                                </th>
                                <th>微信昵称
                                </th>
                                <%--<th>上级代理
                                </th>--%>
                                <th>充值
                                </th>
                                <th>余额</th>
                                <th>彩金</th>
                                <th>提现金额
                                </th>
                                <th>消费金额
                                </th>
                                <th>盈利
                                </th>
                                <th>是否纳入统计
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptSchemes" runat="server" EnableViewState="false">
                                <ItemTemplate>
                                    <tr>
                                        <%--
                                    <td style="border-width: 0px 0px 1px 1px; border-left: medium">
                                        <%#Eval("Day")%>
                                    </td>
                                    <td>
                                        <%#  (Convert.ToDouble(Eval("Buy"))).ToString("0.00")%>
                                    </td>
                                    <td>
                                        <%#  (Convert.ToDouble(Eval("SurrogateIn"))).ToString("0.00")%>
                                    </td>
                                    <td>
                                        <%#  (Convert.ToDouble(Eval("BuyHandsel"))).ToString("0.00")%>
                                    </td>
                                    <td>
                                        <%#  (Convert.ToDouble(Eval("WinMoney"))).ToString("0.00")%>
                                    </td>
                                    <td>
                                        <%#  (Convert.ToDouble(Eval("ExpertsIn"))).ToString("0.00")%>
                                    </td>
                                    <td>
                                        <%#  (Convert.ToDouble(Eval("BDFrozen"))).ToString("0.00")%>
                                    </td>
                                    <td>
                                        <%#  (Convert.ToDouble(Eval("ChaseFrozen"))).ToString("0.00")%>
                                    </td>
                                    <td>
                                        <%#  (Convert.ToDouble(Eval("DrawingFrozen"))).ToString("0.00")%>
                                    </td>
                                    <td>
                                        <%#  Convert.ToDouble(Eval("OkDrawing")).ToString("0.00")%>
                                    </td>
                                    <td class="edit" style="border-right: 1px solid #dfdfdf;">
                                        <%#  (Convert.ToDouble(Eval("Earning"))).ToString("0.00")%>
                                    </td>
                                        --%>
                                        <td>
                                            <% if (SourceFrom==null)
                                               { %>
                                            <a href="PromoteIndex.aspx?SourceFrom=Agent&ParentName=<%#Eval("Name")%>&groupid=<%#Eval("GroupUserId")%>"><%#Eval("Name")%>（<%#Eval("cnt")%>）</a>
                                            <% }else{ %>
                                            <a href="UserAccountDetail.aspx?SiteID=1&ID=<%#Eval("GroupUserId")%>&UserName=<%#Eval("Name")%>" target="_blank"><%#Eval("Name")%></a>
                                            <% } %>
                                        </td>
                                        <td>
                                            <%#Eval("NickName") %>
                                            &nbsp;&nbsp;<a href='AgentUserBuyLotteryDetails.aspx?UserID=<%#Eval("GroupUserId") %>&UserName=<%#HttpUtility.UrlEncode(Eval("Name") + "") %>&OperatorType=1'
                                                        target="_blank"><font color="#ff0000">(购彩记录)</font></a>
                                        </td>
                                        <%--<td>
                                            #Eval("ParentName")
                                        </td>--%>
                                        <td class="pay">
                                            <%#  (Convert.ToDouble(Eval("pay"))).ToString("0.00")%>
                                        </td>
                                        <td class="Balance">
                                            <%#  (Convert.ToDouble(Eval("Balance"))).ToString("0.00")%>
                                        </td>
                                        <td class="Handse">
                                            <%#  (Convert.ToDouble(Eval("Handse"))).ToString("0.00")%>
                                        </td>
                                        <td class="dis">
                                            <%#  (Convert.ToDouble(Eval("dis"))).ToString("0.00")%>
                                        </td>
                                        <td class="win">
                                            <%#  (Convert.ToDouble(Eval("win"))).ToString("0.00")%>
                                        </td>
                                        <td class="profit">
                                            <%#  (Convert.ToDouble(Eval("pay")) - Convert.ToDouble(Eval("dis")) ).ToString("0.00")%>
                                        </td>
                                        <td>
                                            <input class="chkbox" type="checkbox" checked="checked" onchange="javascript:calTotalByPage();" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td style="text-align:center">统计</td>
                                <td></td>
                                <td id="tdPay" style="text-align: center"></td>
                                <td id="tdBan" style="text-align: center"></td>
                                <td id="tdHan" style="text-align: center"></td>
                                <td id="tdDis" style="text-align: center"></td>
                                <td id="tdWin" style="text-align: center"></td>
                                <td id="tdProfit" style="text-align: center"></td>
                                <td></td>
                            </tr>
                        </tfoot>
                    </table>
                    <div id="sand" style="text-align: right; padding-right: 30px;">
                    </div>
                    <input name="index" value="1" id="index" hidden="hidden" type="hidden" />
                </div>
            </div>
            <%--        <div class="demo-chat" style="display: none">
            <canvas id="canvas" height="500" width="800">
            </canvas>
        </div>--%>
        </div>
        <asp:HiddenField runat="server" ID="hfToosTips" />
        <asp:HiddenField runat="server" ID="hfMoneys" />
            </div>
        </div>
    </div>
    <!--Content HTML end-->
    <uc1:Footer ID="Footer1" runat="server" />
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
            hrefFormer: 'MonitoringLog',
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
    function calTotalByPage() {
        var pay = 0.00;
        var dis = 0.00;
        var win = 0.00;
        var profit = 0.00;
        var ban = 0.00;
        var han = 0.00;
        $("#tab tbody tr").each(function () {
            if ($(this).find(".chkbox").attr("checked") == "checked") {
                pay += parseFloat($(this).find(".pay").text(), 10);
                ban += parseFloat($(this).find(".Balance").text(), 10);
                han += parseFloat($(this).find(".Handse").text(), 10);
                dis += parseFloat($(this).find(".dis").text(), 10);
                win += parseFloat($(this).find(".win").text(), 10);
                profit += parseFloat($(this).find(".profit").text(), 10);
            }
        });
        $("#tdPay").text(pay.toFixed(2));
        $("#tdBan").text(ban.toFixed(2));
        $("#tdHan").text(han.toFixed(2));
        $("#tdDis").text(dis.toFixed(2));
        $("#tdWin").text(win.toFixed(2));
        $("#tdProfit").text(profit.toFixed(2));
    }
    calTotalByPage();
</script>


<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgentCommission.aspx.cs"
    Inherits="CPS_Agent_AgentCommission" %>

<%@ Register Src="../userControls/AgentHeader.ascx" TagName="AgentHeader" TagPrefix="uc1" %>
<%@ Register Src="../userControls/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-佣金明细</title>
    <meta name="description" content="<%=_Site.Name %>-佣金明细" />
    <meta name="keywords" content="<%=_Site.Name %>-佣金明细" />
    <link type="text/css" href="../css/common.css" rel="stylesheet" />
    <link type="text/css" href="../../Style/sandPage.css" rel="stylesheet" />
    <style type="text/css">
        .schemeNumber a
        {
            color: Blue;
        }
        .schemeNumber a:hover
        {
            text-decoration: underline;
        }
        .column1
        {
            width: 5%;
        }
        .column2
        {
            width: 15%;
        }
        .column3
        {
            width: 15%;
        }
        .column4
        {
            width: 10%;
        }
        .column5
        {
            width: 20%;
        }
        .column5 a
        {
            color: Blue;
        }
        .column5 a:hover
        {
            text-decoration: underline;
        }
        .column6
        {
            width: 15%;
        }
        .column7
        {
            width: 10%;
        }
        .column8
        {
            width: 10%;
        }
        .red
        {
            color: Red;
        }
        .num_search ul li{ float:left; margin-right:15px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:AgentHeader ID="AgentHeader1" runat="server" />
    <div class="user_banner">
        <h3>
            我的佣金<span>My Commission</span></h3>
    </div>
    <div class="content">
        <div class="inside_bg">
            <div class="insicon">
                <div class="sidebar">
                    <ul>
                        <li class="curr"><a href="AgentCommission.aspx">佣金明细</a></li>
                        <li><a href="AgentCommissionSet.aspx">佣金设置</a></li>
                    </ul>
                </div>
                <div class="commain">
                    <div class="mainbor">
                        <div id="user_list" class="commission_record">
                            <h4>
                                <span>佣金明细</span>
                                <div class="choice">
                                    <a id="openoffbut" class="open" href="javascript:;">展开更多筛选</a>
                                </div>
                            </h4>
                            <div class="num_search" runat="server" id="divSearch">
                                <ul>
                                    <li>
                                        用户名：
                                        <asp:TextBox runat="server" ID="txtName" MaxLength="18" Width="130" CssClass="cpsInput border_shadow" placeholder="用户名"/>
                                    </li>
                                    <li>
                                        彩种：
                                        <asp:DropDownList runat="server" ID="ddlLottery" CssClass="cpsSelect" Width="100">
                                        </asp:DropDownList>
                                    </li>
                                    <li>
                                        开奖时间：
                                        <asp:TextBox runat="server" ID="txtStartDate" CssClass="cpsInput cpsTimeInput border_shadow" placeholder="开始时间" onFocus="WdatePicker({el:'txtStartDate',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                        —
                                        <asp:TextBox runat="server" ID="txtEndDate" CssClass="cpsInput cpsTimeInput border_shadow" placeholder="截止时间" onFocus="WdatePicker({el:'txtEndDate',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                    </li>
                                    <li>
                                        身份：
                                        <asp:DropDownList runat="server" ID="ddlTypeList" CssClass="cpsSelect">
                                            <asp:ListItem Text="全部" Value="-1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="我的会员" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="我的推广员" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="我的推广员发展的会员" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </li>
                                    <li>
                                        <input type="button" id="btnSearch" value="筛选" class="btnAction" />
                                    </li>
                                </ul>
                            </div>
                            <dl>
                                <dt>
                                    <div class="column1">
                                        序号</div>
                                    <div class="column2">
                                        会员用户名</div>
                                    <div class="column3">
                                        开奖时间</div>
                                    <div class="column4">
                                        彩种</div>
                                    <div class="column5">
                                        方案编号</div>
                                    <div class="column6">
                                        购买金额</div>
                                    <div class="column7">
                                        佣金比例</div>
                                    <div class="column8">
                                        我的佣金</div>
                                </dt>
                                <asp:Repeater runat="server" ID="rpt_list" OnItemDataBound="rpt_list_ItemDataBound">
                                    <ItemTemplate>
                                        <dd>
                                            <div class="column1">
                                                <%# Container.ItemIndex + 1%>
                                            </div>
                                            <div class="column2">
                                                <%#Eval("UserName")%></div>
                                            <div class="column3">
                                                <%#Shove._Convert.StrToDateTime(Eval("PrintOutDateTime").ToString(), DateTime.Now.ToString()).ToString("yyyy-MM-dd HH:mm:ss")%></div>
                                            <div class="column4">
                                                <%#Eval("LotteryName")%></div>
                                            <div class="column5">
                                                <a href='/Home/Room/Scheme.aspx?ID=<%#Eval("SchemeID") %>' target="_blank">
                                                    <%#Eval("SchemeNumber")%></a></div>
                                            <div class="column6">
                                                <%#Shove._Convert.StrToDouble(Eval("DetailMoney").ToString(), 0).ToString("0.00")%></div>
                                            <div class="column7">
                                                <%#Eval("BonusScale").ToString()%></div>
                                            <div class="column8">
                                                <%#Shove._Convert.StrToDouble(Eval("Money").ToString(), 0).ToString("0.00")%>
                                            </div>
                                        </dd>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <dd>
                                    <div class="column1">
                                        总 计
                                    </div>
                                    <div class="column2">
                                        -</div>
                                    <div class="column3">
                                        -</div>
                                    <div class="column4">
                                        -</div>
                                    <div class="column5">
                                        -</div>
                                    <div class="column6 red">
                                        <%=buyLotteryMoney.ToString("0.00")%></div>
                                    <div class="column7">
                                        -</div>
                                    <div class="column8 red">
                                        <%=buyLotteryBonus.ToString("0.00")%>
                                    </div>
                                </dd>
                            </dl>
                            <div id="sand" style="text-align: right; padding-right: 30px;" runat="server">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <uc2:Footer ID="Footer1" runat="server" />
    </form>
    <input type="hidden" id="hide_date" value="day" runat="server" />
</body>
<script type="text/javascript">
    //过滤内容点击事件
    $("#btnSearch").click(function () {
        var parameter = "Name=" + FlyFish.Trim($("#txtName").val());
        parameter += "&startDate=" + FlyFish.Trim($("#txtStartDate").val());
        parameter += "&endDate=" + FlyFish.Trim($("#txtEndDate").val());
        parameter += "&lottery=" + FlyFish.Trim($("#ddlLottery option:selected").val());
        parameter += "&userType=" + FlyFish.Trim($("#ddlTypeList option:selected").val());
        window.location.href = "?" + parameter;
    });

    var pageIndex = parseInt("<%=pageIndex%>");
    var pageCount = parseInt("<%=pageCount%>");
    var dataCount = parseInt("<%=dataCount%>");

    $(function () {
        var totalPage = pageCount;
        var totalRecords = dataCount;
        var pageNo = pageIndex;
        var parameter = "Name=" + FlyFish.Trim($("#txtName").val());
        parameter += "&startDate=" + FlyFish.Trim($("#txtStartDate").val());
        parameter += "&endDate=" + FlyFish.Trim($("#txtEndDate").val());
        parameter += "&lottery=" + FlyFish.Trim($("#ddlLottery option:selected").val());
        parameter += "&userType=" + FlyFish.Trim($("#ddlTypeList option:selected").val());
        //初始化分页控件
        //有些参数是可选的，比如lang，若不传有默认值
        sand.init({
            pno: pageNo,
            //总页码
            total: totalPage,
            //总数据条数
            totalRecords: totalRecords,
            //链接前部
            hrefFormer: 'AgentCommission',
            //链接尾部
            hrefLatter: '.aspx',
            getLink: function (n) {
                return this.hrefFormer + this.hrefLatter + "?" + parameter + "&PageIndex=" + n;
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

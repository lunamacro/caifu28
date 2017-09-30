<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PromoteCommission.aspx.cs"
    Inherits="CPS_Promote_PromoteCommission" %>

<%@ Register Src="../userControls/Footer.ascx" TagName="Footer" TagPrefix="uc1" %>
<%@ Register Src="../userControls/PromoteHeader.ascx" TagName="PromoteHeader" TagPrefix="uc2" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-推广联盟-我的佣金</title>
    <meta name="description" content="<%=_Site.Name %>-推广联盟-我的佣金" />
    <meta name="keywords" content="<%=_Site.Name %>-推广联盟-我的佣金" />
    <link type="text/css" href="../css/common.css" rel="stylesheet" />
    <link type="text/css" rel="stylesheet" href="../../Style/sandPage.css" />
    <style type="text/css">
        .schemeNumber a {
            color: Blue;
        }

            .schemeNumber a:hover {
                text-decoration: underline;
            }

        .num_search ul li {
            float: left;
            margin-right: 15px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc2:PromoteHeader ID="PromoteHeader1" runat="server" />
        <!--Header HTML end-->
        <div class="commission_banner">
            <h3>我的佣金<span>My Commission</span></h3>
        </div>
        <!--Banner HTML end-->
        <div class="content">
            <div class="inside_bg">
                <div class="insicon">
                    <div class="sidebar">
                        <ul>
                            <li class="curr"><a href="javascript:void(0);" onclick="location.reload()">我的佣金</a></li>
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
                                        <li>用户名：
                                        <asp:TextBox runat="server" ID="txtName" CssClass="cpsInput border_shadow" MaxLength="18" placeholder="用户名" />
                                        </li>
                                        <li>彩种：
                                        <asp:DropDownList runat="server" ID="ddlLottery" CssClass="cpsSelect" Width="100">
                                        </asp:DropDownList>
                                        </li>
                                        <li>开奖时间：
                                        <asp:TextBox runat="server" ID="txtStartDate" placeholder="开始时间" CssClass="cpsInput cpsTimeInput border_shadow" onFocus="WdatePicker({el:'txtStartDate',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                            —
                                    <asp:TextBox runat="server" ID="txtEndDate" placeholder="截止时间" CssClass="cpsInput cpsTimeInput border_shadow" onFocus="WdatePicker({el:'txtEndDate',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                        </li>
                                        <li>
                                            <input type="button" id="btnSearch" class="btnAction" value="筛选" runat="server" />
                                        </li>
                                    </ul>
                                </div>
                                <dl>
                                    <dt>
                                        <div style="width: 5%">
                                            序号
                                        </div>
                                        <div style="width: 15%">
                                            会员用户名
                                        </div>
                                        <div style="width: 15%">
                                            开奖时间
                                        </div>
                                        <div style="width: 10%">
                                            彩种
                                        </div>
                                        <div style="width: 20%">
                                            方案编号
                                        </div>
                                        <div style="width: 15%">
                                            购买金额
                                        </div>
                                        <div style="width: 10%">
                                            佣金比例
                                        </div>
                                        <div style="width: 10%">
                                            我的佣金
                                        </div>
                                    </dt>
                                    <asp:Repeater runat="server" ID="rpt_list" OnItemDataBound="rpt_list_ItemDataBound">
                                        <ItemTemplate>
                                            <dd>
                                                <div style="width: 5%">
                                                    <%# Container.ItemIndex + 1%>
                                                </div>
                                                <div style="width: 15%">
                                                    <%#Eval("BuyUserName")%>
                                                </div>
                                                <div style="width: 15%">
                                                    <%#Shove._Convert.StrToDateTime(Eval("DateTime").ToString(), DateTime.Now.ToString()).ToString("yyyy-MM-dd HH:mm:ss")%>
                                                </div>
                                                <div style="width: 10%">
                                                    <%#Eval("LotteryName")%>
                                                </div>
                                                <div style="width: 20%;" class="schemeNumber">
                                                    <a href='/Home/Room/Scheme.aspx?ID=<%#Eval("SchemeID") %>' target="_blank">
                                                        <%#Eval("SchemeNumber")%></a>
                                                </div>
                                                <div style="width: 15%">
                                                    <%#Shove._Convert.StrToDouble(Eval("BuyMoney").ToString(), 0).ToString("0.00")%>
                                                </div>
                                                <div style="width: 10%">
                                                    <%#Eval("BonusScale").ToString()%>
                                                </div>
                                                <div style="width: 10%">
                                                    <%#Shove._Convert.StrToDouble(Eval("Bonus").ToString(), 0).ToString("0.00")%>
                                                </div>
                                            </dd>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <dd>
                                        <div style="width: 5%">
                                            总 计
                                        </div>
                                        <div style="width: 15%">
                                            -
                                        </div>
                                        <div style="width: 15%">
                                            -
                                        </div>
                                        <div style="width: 10%">
                                            -
                                        </div>
                                        <div style="width: 20%">
                                            -
                                        </div>
                                        <div style="width: 15%; color: Red;">
                                            <%=buyLotteryMoney.ToString("0.00")%>
                                        </div>
                                        <div style="width: 10%">
                                            -
                                        </div>
                                        <div style="width: 10%; color: Red;">
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
        <!--Content HTML end-->
        <input type="hidden" id="hide_date" value="day" runat="server" />
        <uc1:Footer ID="Footer1" runat="server" />
    </form>
</body>

<script type="text/javascript">
    //过滤内容点击事件
    $("#btnSearch").click(function () {
        var param = "Name=" + FlyFish.Trim($("#txtName").val());
        param += "&startDate=" + FlyFish.Trim($("#txtStartDate").val());
        param += "&endDate=" + FlyFish.Trim($("#txtEndDate").val());
        param += "&lottery=" + FlyFish.Trim($("#ddlLottery option:selected").val());
        window.location.href = "?" + param;
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
        //初始化分页控件
        //有些参数是可选的，比如lang，若不传有默认值
        sand.init({
            pno: pageNo,
            //总页码
            total: totalPage,
            //总数据条数
            totalRecords: totalRecords,
            //链接前部
            hrefFormer: 'PromoteCommission',
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

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PromoteNumber.aspx.cs" Inherits="CPS_Promote_PromoteNumber" %>

<%@ Register Src="../userControls/Footer.ascx" TagName="Footer" TagPrefix="uc1" %>
<%@ Register Src="../userControls/PromoteHeader.ascx" TagName="PromoteHeader" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-推广联盟-我的会员</title>
    <meta name="description" content="<%=_Site.Name %>-推广联盟-我的会员" />
    <meta name="keywords" content="<%=_Site.Name %>-推广联盟-我的会员" />
    <link href="../css/common.css" rel="stylesheet" />
    <link type="text/css" href="../../Style/sandPage.css" rel="stylesheet" />
    <style type="text/css">
        .number_record h4 span
        {
            padding: 0px 10px;
            cursor: pointer;
        }
        .number_record h4 span.curr
        {
            color: orange;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc2:PromoteHeader ID="PromoteHeader1" runat="server" />
    <!--Header HTML end-->
    <div class="member_banner">
        <h3>
            我的会员<span>My Members</span></h3>
    </div>
    <!--Banner HTML end-->
    <div class="content">
        <div class="inside_bg">
            <div class="insicon">
                <div class="sidebar">
                    <ul>
                        <li class="curr"><a href="javascript:void(0);" onclick="location.reload()">我的会员</a></li>
                    </ul>
                </div>
                <div class="commain">
                    <div class="mainbor">
                        <div id="user_list" class="number_record">
                            <h4>
                                <span usertype="1" runat="server" id="span1">会员列表</span> <span usertype="2" runat="server"
                                    id="span2">转入会员列表</span> <span usertype="3" runat="server" id="span3">转出会员列表</span>
                                <div class="choice">
                                    <a href="javascript:;" tag="all" runat="server" id="dateForAll">全部</a> <a href="javascript:;"
                                        tag="day" runat="server" id="dateForDay">今日</a> <a href="javascript:;" tag="month"
                                            runat="server" id="dateForMonth">最近一月</a> <a href="javascript:;" tag="year" runat="server"
                                                id="dateForYear">最近一年</a> <a id="openoffbut" class="open"  href="javascript:;">展开更多筛选</a>
                                </div>
                            </h4>
                            <div class="num_search" runat="server" id="divSearch">
                            <ul>
                                        <li>用户名：
                                        <asp:TextBox runat="server" ID="txtName" CssClass="cpsInput border_shadow" MaxLength="18" placeholder="用户名" />
                                        </li>
                                        <li><span style="width: 150px;" id="timeType" runat="server">时间：</span>
                                    <asp:TextBox runat="server" ID="txtStartDate" CssClass="cpsInput cpsTimeInput border_shadow" placeholder="开始时间" onFocus="WdatePicker({el:'txtStartDate',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                    <i>—</i>
                                    <asp:TextBox runat="server" ID="txtEndDate" CssClass="cpsInput cpsTimeInput border_shadow"  placeholder="截止时间" onFocus="WdatePicker({el:'txtEndDate',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                        </li>
                                        <li>
                                            <input type="button" id="btnSearch" class="btnAction" value="筛选" runat="server" />
                                        </li>
                                    </ul>
                                <%--<div class="sea_user">
                                    <label>
                                        <span>用户名：</span>
                                        <asp:TextBox runat="server" ID="txtName" MaxLength="18" CssClass="cpsInput border_shadow" placeholder="用户名"></asp:TextBox>
                                    </label>
                                </div>
                                <div class="sea_buytime">
                                    
                                </div>
                                <div class="sea_but" style="margin: 0px; padding-left: 30px;">
                                    <span></span>
                                    <input type="button" id="btnSearch" value="筛选" /></div>--%>
                            </div>
                            <dl>
                                <dt>
                                    <div style="width: 10%">
                                        序号</div>
                                    <div style="width: 15%">
                                        会员用户名</div>
                                    <div style="width: 15%">
                                        时间</div>
                                    <div style="width: 20%">
                                        消费总额</div>
                                    <div style="width: 20%">
                                        我的佣金</div>
                                    <div style="width: 20%">
                                        购彩记录</div>
                                </dt>
                                <asp:Repeater runat="server" ID="rpt_list" OnItemDataBound="rpt_list_ItemDataBound">
                                    <ItemTemplate>
                                        <dd>
                                            <div style="width: 10%">
                                                <%#Container.ItemIndex+1 %></div>
                                            <div style="width: 15%">
                                                <%#Eval("UserName")%></div>
                                            <div style="width: 15%">
                                                <%#Eval("DateTime")%></div>
                                            <div style="width: 20%">
                                                <%#Shove._Convert.StrToDouble(Eval("SumMoney").ToString(), 0).ToString("0.00")%></div>
                                            <div style="width: 20%">
                                                <%#Shove._Convert.StrToDouble(Eval("SumBonus").ToString(), 0).ToString("0.00")%></div>
                                            <div style="width: 20%">
                                                <a href='UserBuyLotteryDetails.aspx?UserID=<%#Eval("UserID") %>&UserName=<%#HttpUtility.UrlEncode(Eval("UserName") + "") %>'
                                                    target="_blank">购彩记录</a></div>
                                        </dd>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <dd>
                                    <div style="width: 10%">
                                        总 计
                                    </div>
                                    <div style="width: 15%">
                                        -</div>
                                    <div style="width: 15%">
                                        -</div>
                                    <div style="width: 20%; color: Red;">
                                        <%=sumMoney.ToString("0.00")%></div>
                                    <div style="width: 20%; color: Red;">
                                        <%=sumBonus.ToString("0.00")%></div>
                                    <div style="width: 20%">
                                        -</div>
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
    <input type="hidden" id="hideDate" value="day" runat="server" />
    <input type="hidden" id="hideUserType" value="1" runat="server" />
    <uc1:Footer ID="Footer1" runat="server" />
    </form>
</body>
<script type="text/javascript">
    window.onload = function () {
        //会员类型点击事件
        $("#user_list h4 span").bind("click", function () {
            $("#hideUserType").val($(this).attr("userType"));
            var param = "date=" + $("#hideDate").val();
            param += "&searchIsShow=" + $("#divSearch").is(":visible");
            param += "&userType=" + $("#hideUserType").val();
            window.location.href = "?" + param;
        });

        //时间类型点击事件
        $(".choice a[id != 'openoffbut']").click(function () {
            $("#hideDate").val($(this).attr("tag"));
            var param = "date=" + $("#hideDate").val();
            param += "&searchIsShow=" + $("#divSearch").is(":visible");
            param += "&userType=" + $("#hideUserType").val();
            window.location.href = "?" + param;
        });

        //过滤内容点击事件
        $("#btnSearch").click(function () {
            var param = "date=" + $("#hideDate").val();
            param += "&searchIsShow=" + $("#divSearch").is(":visible");
            param += "&userType=" + $("#hideUserType").val();
            param += "&userName=" + escape(FlyFish.Trim($("#txtName").val()));
            param += "&startDate=" + FlyFish.Trim($("#txtStartDate").val());
            param += "&endDate=" + FlyFish.Trim($("#txtEndDate").val());
            window.location.href = "?" + param;
        });
    }
</script>
<script type="text/javascript">

    var pageIndex = parseInt("<%=pageIndex%>");
    var pageCount = parseInt("<%=pageCount%>");
    var dataCount = parseInt("<%=dataCount%>");

    $(function () {
        var totalPage = pageCount;
        var totalRecords = dataCount;
        var pageNo = pageIndex;
        var parameter = "date=" + $("#hideDate").val();
        parameter += "&searchIsShow=" + $("#divSearch").is(":visible");
        parameter += "&userType=" + $("#hideUserType").val();
        parameter += "&userName=" + FlyFish.Trim($("#txtName").val());
        parameter += "&startDate=" + FlyFish.Trim($("#txtStartDate").val());
        parameter += "&endDate=" + FlyFish.Trim($("#txtEndDate").val());
        //初始化分页控件
        //有些参数是可选的，比如lang，若不传有默认值
        sand.init({
            pno: pageNo,
            //总页码
            total: totalPage,
            //总数据条数
            totalRecords: totalRecords,
            //链接前部
            hrefFormer: 'PromoteNumber',
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

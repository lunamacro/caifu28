<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommissionScaleRecord.aspx.cs"
    Inherits="CPS_Agent_CommissionScaleRecord" %>

<%@ Register Src="../userControls/AgentHeader.ascx" TagName="AgentHeader" TagPrefix="uc1" %>
<%@ Register Src="../userControls/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-推广联盟-佣金设置记录</title>
    <meta name="description" content="<%=_Site.Name %>-推广联盟-佣金设置记录" />
    <meta name="keywords" content="<%=_Site.Name %>-推广联盟-佣金设置记录" />
    <link type="text/css" href="../css/common.css" rel="stylesheet" />
    <link type="text/css" href="../../Style/sandPage.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <uc1:AgentHeader ID="AgentHeader1" runat="server" />
        <div class="user_banner">
            <h3>我的佣金<span>My Commission</span></h3>
        </div>
        <div class="content">
            <div class="inside_bg">
                <div class="insicon">
                    <div class="sidebar">
                        <ul class="fourli">
                            <li><a href="AgentCommission.aspx">佣金明细</a></li>
                            <li class="curr"><a href="AgentCommissionSet.aspx">佣金设置</a></li>
                        </ul>
                    </div>
                    <div class="commain">
                        <div class="mainbor">
                            <div class="admininfo">
                                <div class="admininfo">
                                    <div id="user_list" class="number_record">
                                        <h4>
                                            <span style="width: 240px;">
                                                <%=Shove._Web.Utility.GetRequest("UserName") %>-佣金设置记录</span>
                                            <div class="choice">
                                                <a id="openoffbut" class="open" href="javascript:;">展开更多筛选</a>
                                            </div>
                                        </h4>
                                        <div class="num_search">
                                            <ul>
                                                <li>彩种：
                                                <asp:DropDownList runat="server" ID="ddlLottery" CssClass="cpsSelect" Width="100">
                                                </asp:DropDownList>
                                                </li>
                                                <li>生效时间：
                                                <asp:TextBox runat="server" ID="txtStartDate" CssClass="cpsInput cpsTimeInput border_shadow"  placeholder="开始时间" onFocus="WdatePicker({el:'txtStartDate',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                                    —
                                                <asp:TextBox runat="server" ID="txtEndDate" CssClass="cpsInput cpsTimeInput border_shadow" placeholder="截止时间" onFocus="WdatePicker({el:'txtEndDate',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                                </li>
                                                <li>
                                                    <input type="button" id="btnSearch" value="筛选" class="btnAction" />
                                                </li>
                                            </ul>
                                        </div>
                                        <dl>
                                            <dt>
                                                <div class="system_numtime">
                                                    序号
                                                </div>
                                                <div class="system_numname">
                                                    彩种名称
                                                </div>
                                                <div class="system_numlottoy">
                                                    原始佣金比例
                                                </div>
                                                <div class="system_numcode">
                                                    修改后的佣金比例
                                                </div>
                                                <div class="system_numcommratio">
                                                    生效时间
                                                </div>
                                            </dt>
                                            <asp:Repeater ID="rpt_dataList" runat="server">
                                                <ItemTemplate>
                                                    <dd>
                                                        <div class="system_numtime">
                                                            <%#Container.ItemIndex+1 %>
                                                        </div>
                                                        <div class="system_numname">
                                                            <%#Eval("LotteryName")%>
                                                        </div>
                                                        <div class="system_numlottoy">
                                                            <%#Eval("PrimaryBonusScale")%>
                                                        </div>
                                                        <div class="system_numcode">
                                                            <%#Eval("BonusScale")%>
                                                        </div>
                                                        <div class="system_numcommratio">
                                                            <%#Eval("InureTime")%>
                                                        </div>
                                                    </dd>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </dl>
                                        <div id="sand" style="text-align: right; padding-right: 30px;" runat="server">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <uc2:Footer ID="Footer1" runat="server" />
        <input type="hidden" runat="server" id="hideCpsID" value="" />
        <input type="hidden" runat="server" id="hideUserName" value="" />
    </form>
</body>
<script type="text/javascript">
    window.onload = function () {
        $("#btnSearch").click(function () {
            var param = "CpsID=" + FlyFish.Trim($("#hideCpsID").val());
            param += "&UserName=" + FlyFish.Trim($("#hideUserName").val());
            param += "&lotteryID=" + $("#ddlLottery option:selected").val();
            param += "&startTime=" + FlyFish.Trim($("#txtStartDate").val());
            param += "&endTime=" + FlyFish.Trim($("#txtEndDate").val());
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
        var parameter = "?CpsID=" + FlyFish.Trim($("#hideCpsID").val());
        parameter += "&UserName=" + FlyFish.Trim($("#hideUserName").val());
        parameter += "&lotteryID=" + $("#ddlLottery option:selected").val();
        parameter += "&startTime=" + FlyFish.Trim($("#txtStartDate").val());
        parameter += "&endTime=" + FlyFish.Trim($("#txtEndDate").val());
        //初始化分页控件
        //有些参数是可选的，比如lang，若不传有默认值
        sand.init({
            pno: pageNo,
            //总页码
            total: totalPage,
            //总数据条数
            totalRecords: totalRecords,
            //链接前部
            hrefFormer: 'CommissionScaleRecord',
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

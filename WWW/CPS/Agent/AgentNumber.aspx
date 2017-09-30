<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgentNumber.aspx.cs" Inherits="CPS_Agent_AgentNumber" %>

<%@ Register Src="../userControls/AgentHeader.ascx" TagName="AgentHeader" TagPrefix="uc1" %>
<%@ Register Src="../userControls/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-

transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-会员列表</title>
    <meta name="description" content="<%=_Site.Name %> 推广联盟 会员列表" />
    <meta name="keywords" content="<%=_Site.Name %> 推广联盟 会员列表" />
    <link href="../css/common.css" rel="stylesheet" />
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
    </style>
    <link type="text/css" href="../../Style/sandPage.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <uc1:AgentHeader ID="AgentHeader1" runat="server" />
        <div class="user_banner">
            <h3>我的会员<span>My Members</span></h3>
        </div>
        <div class="content">
            <div class="inside_bg">
                <div class="insicon">
                    <div class="sidebar">
                        <ul>
                            <li class="curr"><a href="#">会员列表</a></li>
                            <li  style="display:none"><a href="AgentPromoteList.aspx">推广员列表</a></li>
                            
                            <li style="display:none"><a href="AgentPromoteNumber.aspx">推广员发展的会员</a></li>
                            <li style="display:none"><a href="AgentAddPromote.aspx">增加推广员</a></li>
                        </ul>
                    </div>
                    <div class="commain">
                        <div class="mainbor">
                            <div id="user_list" class="number_record">
                                <h4>
                                    <span usertype="1" runat="server" id="span1">会员列表</span> 
                                    <span usertype="2" runat="server" id="span2" style="display:none">转入会员列表</span> 
                                    <span usertype="3" runat="server" id="span3" style="display:none">转出会员列表</span>
                                    <div class="choice">
                                        <a href="javascript:;" tag="all" runat="server" id="dateForAll">全部</a> <a href="javascript:;"
                                            tag="day" runat="server" id="dateForDay">今日</a> <a href="javascript:;" tag="month"
                                                runat="server" id="dateForMonth">最近一月</a> <a href="javascript:;" tag="year" runat="server"
                                                    id="dateForYear">最近一年</a> <a id="openoffbut" class="open" href="javascript:;">展开更多筛选</a>
                                    </div>
                                </h4>
                                <div class="num_search" runat="server" id="divSearch">
                                    <ul>
                                        <li>用户名：<asp:TextBox runat="server" ID="txtName" CssClass="cpsInput border_shadow" placeholder="用户名" /></li>
                                        <li>
                                            <span style="width: 150px;" id="timeType" runat="server">时间：</span>
                                            <asp:TextBox runat="server" ID="txtStartDate" placeholder="开始时间" CssClass="cpsInput cpsTimeInput border_shadow" onFocus="WdatePicker({el:'txtStartDate',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                            —
                                        <asp:TextBox runat="server" ID="txtEndDate" placeholder="截止时间" CssClass="cpsInput cpsTimeInput border_shadow" onFocus="WdatePicker({el:'txtEndDate',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                        </li>
                                        <li>
                                            <input type="button" id="btnSearch" value="筛选" class="btnAction" />
                                        </li>
                                    </ul>
                                    <%--<div class="sea_user">
                                    <label>
                                        <span>用户名：</span>
                                        <asp:TextBox runat="server" ID="txtName"></asp:TextBox>
                                    </label>
                                </div>
                                <div class="sea_buytime">
                                    <span style="width: 150px;" id="timeType" runat="server">注册时间：</span>
                                    <asp:TextBox runat="server" ID="txtStartDate" onFocus="WdatePicker({el:'txtStartDate',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                    <i>—</i>
                                    <asp:TextBox runat="server" ID="txtEndDate" onFocus="WdatePicker({el:'txtEndDate',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                </div>
                                <div class="sea_but" style="margin: 0px; padding-left: 30px;">
                                    <span></span>
                                    <input type="button" id="btnSearch" value="筛选"></div>--%>
                                </div>
                                <dl>
                                    <dt>
                                        <div style="width: 5%">
                                            序号
                                        </div>
                                        <div style="width: 20%">
                                            会员用户名
                                        </div>
                                        <div style="width: 15%">
                                            微信名称
                                        </div>

                                        <div style="width: 20%">
                                            时间
                                        </div>
                                        <div style="width: 8%">
                                            购彩记录
                                        </div>
                                        <div style="width: 8%">充值明细</div>
                                        <div style="width: 8%">取款明细</div>
                                        <div style="width: 8%">彩金</div>
                                        <div style="width: 8%">盈亏</div>
                                    </dt>
                                    <asp:Repeater runat="server" ID="rpt_list" OnItemDataBound="rpt_list_ItemDataBound">
                                        <ItemTemplate>
                                            <dd>
                                                <div style="width: 5%">
                                                    <%#Container.ItemIndex+1 %>
                                                </div>
                                                <div style="width: 20%">
                                                    <%#Eval("UserName")%>
                                                </div>
                                                <div style="width: 15%">
                                                    <%#Eval("RealityName")%>
                                                </div>
                                                <div style="width: 20%">
                                                    <%#Eval("DateTime")%>
                                                </div>
                                                
                                                <div style="width: 8%">
                                                    <a href='AgentUserBuyLotteryDetails.aspx?UserID=<%#Eval("UserID") %>&UserName=<%#HttpUtility.UrlEncode(Eval("UserName") + "") %>&OperatorType=1'
                                                        target="_blank">购彩记录</a>
                                                </div>
                                                <div class="pay" style="width: 8%"> <a href='AgentUserPayDetails.aspx?UserID=<%#Eval("UserID") %>&UserName=<%#HttpUtility.UrlEncode(Eval("UserName") + "") %>'
                                                        target="_blank">
                                                    <%#  (Convert.ToDouble(Eval("pay"))).ToString("0.00")%></a></div>
                                                <div class="dis" style="width: 8%">
                                                    <a href='AgentUserWithdrawDetails.aspx?UserID=<%#Eval("UserID") %>&UserName=<%#HttpUtility.UrlEncode(Eval("UserName") + "") %>'
                                                        target="_blank">
                                                    <%#  (Convert.ToDouble(Eval("dis"))).ToString("0.00")%></a>
                                                   </div>
                                                <div style="width: 8%">
                                                    <%#  (Convert.ToDouble(Eval("Handse"))).ToString("0.00")%>
                                                </div>
                                                <div class="profit" style="width: 8%"><%#  (Convert.ToDouble(Eval("pay")) - Convert.ToDouble(Eval("dis"))).ToString("0.00")%></div>
                                            </dd>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <dd>
                                        <div style="width: 5%">
                                            总 计
                                        </div>
                                        <div style="width: 20%">
                                            -
                                        </div>
                                        <div style="width: 15%">
                                            -
                                        </div>
                                        <div style="width: 20%">
                                            -
                                        </div>
                                        <div style="width: 8%">
                                            -
                                        </div>
                                        <div id="payPageTotal" style="width: 8%; color: Red;">0.00</div>
                                        <div id="disPageTotal" style="width: 8%; color: Red;">0.00</div>
                                        <div style="width: 8%">
                                            -
                                        </div>
                                        <div id="profitPageTotal" style="width: 8%; color: Red;">0.00</div>
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
        <input type="hidden" id="hideDate" value="day" runat="server" />
        <input type="hidden" id="hideUserType" value="1" runat="server" />
        <input type="hidden" id="reUserID" value="1" runat="server" />
        <uc2:Footer ID="Footer1" runat="server" />
    </form>
</body>
</html>
<script type="text/javascript">
    window.onload = function () {
        //会员类型点击事件
        $("#user_list h4 span").bind("click", function () {
            $("#hideUserType").val($(this).attr("userType"));
            var param = "date=" + $("#hideDate").val();
            param += "&searchIsShow=" + $("#divSearch").is(":visible");
            param += "&userType=" + $("#hideUserType").val();
            param += "&UserID=" + FlyFish.Trim($("#reUserID").val());
            window.location.href = "?" + param;
        });

        //时间类型点击事件
        $(".choice a[id != 'openoffbut']").click(function () {
            $("#hideDate").val($(this).attr("tag"));
            var param = "date=" + $("#hideDate").val();
            param += "&searchIsShow=" + $("#divSearch").is(":visible");
            param += "&userType=" + $("#hideUserType").val();
            param += "&UserID=" + FlyFish.Trim($("#reUserID").val());
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
            param += "&UserID=" + FlyFish.Trim($("#reUserID").val());
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
        parameter += "&UserID=" + FlyFish.Trim($("#reUserID").val());
        //初始化分页控件
        //有些参数是可选的，比如lang，若不传有默认值
        sand.init({
            pno: pageNo,
            //总页码
            total: totalPage,
            //总数据条数
            totalRecords: totalRecords,
            //链接前部
            hrefFormer: 'AgentNumber',
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
<script type="text/javascript">
    var payPageTotal = 0.00;
    $(".pay").each(function () {
        payPageTotal += parseFloat($(this).text(), 10);
    });
    $("#payPageTotal").text(payPageTotal.toFixed(2));

    var disPageTotal = 0.00;
    $(".dis").each(function () {
        disPageTotal += parseFloat($(this).text(), 10);
    });
    $("#disPageTotal").text(disPageTotal.toFixed(2));

    var profitPageTotal = 0.00;
    $(".profit").each(function () {
        profitPageTotal += parseFloat($(this).text(), 10);
    });
    $("#profitPageTotal").text(profitPageTotal.toFixed(2));
</script>
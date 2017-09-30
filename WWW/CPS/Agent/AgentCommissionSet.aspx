<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgentCommissionSet.aspx.cs"
    Inherits="CPS_Agent_AgentCommissionSet" %>

<%@ Register Src="../userControls/AgentHeader.ascx" TagName="AgentHeader" TagPrefix="uc1" %>
<%@ Register Src="../userControls/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-推广联盟-佣金设置</title>
    <meta name="description" content="<%=_Site.Name %>-推广联盟-佣金设置" />
    <meta name="keywords" content="<%=_Site.Name %>-推广联盟-佣金设置" />
    <link type="text/css" href="../css/common.css" rel="stylesheet" />
    <link type="text/css" href="../../Style/sandPage.css" rel="stylesheet" />
    <style type="text/css">
        .tobbutton {
            padding-left: 540px;
        }
        .num_search ul li{ float:left; margin-right:15px;}
    </style>
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
                        <ul>
                            <li><a href="AgentCommission.aspx">佣金明细</a></li>
                            <li class="curr"><a href="AgentCommissionSet.aspx">佣金设置</a></li>
                        </ul>
                    </div>
                    <div class="commain">
                        <div class="mainbor">
                            <div id="user_list" class="commission_record">
                                <h4>
                                    <span>推广员佣金比例</span>
                                    <div class="choice">
                                        <a id="openoffbut" class="open" href="javascript:;">展开更多筛选</a>
                                    </div>
                                </h4>
                                <div class="num_search" runat="server" id="divSearch">
                                    <ul>
                                        <li>用户名：<input runat="server" id="txtName" placeholder="用户名" type="text" class="cpsInput border_shadow" /></li>
                                        <li>
                                            <input type="button" id="btnSearch" value="筛选" class="btnAction" />
                                        </li>
                                    </ul>
                                </div>
                                <dl>
                                    <dt>
                                        <div class="ag_numtime">
                                            序号
                                        </div>
                                        <div class="ag_numname">
                                            会员用户名
                                        </div>
                                        <div class="ag_numlottoy">
                                            注册时间
                                        </div>
                                        <div class="ag_numcode">
                                            双色球
                                        </div>
                                        <div class="ag_numamount">
                                            福彩3D
                                        </div>
                                        <div class="ag_numcommratio">
                                            七乐彩
                                        </div>
                                        <div class="ag_numdevelopment">
                                            更多彩种设置
                                        </div>
                                        <div class="ag_numview">
                                            历史记录
                                        </div>
                                    </dt>
                                    <asp:Repeater ID="rpt_dataList" runat="server">
                                        <ItemTemplate>
                                            <dd>
                                                <div class="ag_numtime">
                                                    <%#Container.ItemIndex+1%>
                                                </div>
                                                <div class="ag_numname">
                                                    <%#Eval("UserName")%>
                                                </div>
                                                <div class="ag_numlottoy">
                                                    <%# Convert.ToDateTime(Eval("RegisterTime").ToString()).ToString("yyyy-MM-dd HH:mm:ss")%>
                                                </div>
                                                <div class="ag_numcode">
                                                    <%# DAL.Functions.F_CpsGetBonusScale(Convert.ToInt64(Eval("CpsID")), 5)%>
                                                </div>
                                                <div class="ag_numamount">
                                                    <%# DAL.Functions.F_CpsGetBonusScale(Convert.ToInt64(Eval("CpsID")), 6)%>
                                                </div>
                                                <div class="ag_numcommratio">
                                                    <%# DAL.Functions.F_CpsGetBonusScale(Convert.ToInt64(Eval("CpsID")), 13)%>
                                                </div>
                                                <div class="ag_numdevelopment">
                                                    <a href='CommissionEdit.aspx?CpsID=<%#Eval("CpsID") %>&UserName=<%#Eval("UserName") %>&UserType=<%#Eval("UserType")%>'
                                                        target="_blank">更多设置</a>
                                                </div>
                                                <div class="ag_numview">
                                                    <a href='CommissionScale2.aspx?CpsID=<%#Eval("CpsID") %>&UserName=<%# HttpUtility.UrlEncode(Eval("UserName") + "") %>&UserType=<%#Eval("UserType")%>'
                                                        target="_blank">历史记录</a>
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
        <uc2:Footer ID="Footer1" runat="server" />
    </form>
</body>
<script type="text/javascript">
    $(function () {
        $("#btnSearch").click(function () {
            var param = "userName=" + FlyFish.Trim($("#txtName").val());
            window.location.href = "?" + param;
        });
    });
</script>
<script type="text/javascript">

    var pageIndex = parseInt("<%=pageIndex%>");
    var pageCount = parseInt("<%=pageCount%>");
    var dataCount = parseInt("<%=dataCount%>");

    $(function () {
        var totalPage = pageCount;
        var totalRecords = dataCount;
        var pageNo = pageIndex;
        var parameter = "";
        parameter += "userName=" + FlyFish.Trim($("#txtName").val());
        //初始化分页控件
        //有些参数是可选的，比如lang，若不传有默认值
        sand.init({
            pno: pageNo,
            //总页码
            total: totalPage,
            //总数据条数
            totalRecords: totalRecords,
            //链接前部
            hrefFormer: 'AgentCommissionSet',
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

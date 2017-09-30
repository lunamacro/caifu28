<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommissionScale2.aspx.cs" Inherits="CPS_Agent_CommissionScale2" %>

<%@ Register Src="../userControls/AdministrationTop.ascx" TagName="AdministrationTop"
    TagPrefix="uc1" %>
<%@ Register Src="../userControls/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-推广联盟-佣金比例</title>
    <meta name="description" content="<%=_Site.Name %>-推广联盟-佣金比例" />
    <meta name="keywords" content="<%=_Site.Name %>-推广联盟-佣金比例" />
    <link type="text/css" href="../css/common.css" rel="stylesheet" />
    <link type="text/css" href="../../Style/sandPage.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <uc1:AdministrationTop ID="AdministrationTop1" runat="server" />
    <div class="systemsettings_banner">
        <h3>
            系统设置<span>System Settings</span></h3>
    </div>
    <div class="content">
        <div class="inside_bg">
            <div class="insicon">
                <div class="sidebar">
                    <ul class="fourli">
                        <li class="curr"><a href="CommissionScale.aspx">佣金比例</a></li>
                        <li><a href="UserTransfer.aspx">会员转移</a></li>
                        <li><a href="UserTransferRecord.aspx">会员转移记录</a></li>
                        <li><a href="SystemParamSet.aspx">参数设置</a></li>
                    </ul>
                </div>
                <div class="commain">
                    <div class="mainbor">
                        <div class="admininfo">
                            <div class="admininfo">
                                <div id="user_list" class="number_record">
                                    <h4>
                                        <span>佣金比例</span>
                                        <div class="choice">
                                            <a id="openoffbut" class="open" href="javascript:;">展开更多筛选</a>
                                        </div>
                                    </h4>
                                    <div class="num_search" runat="server" id="divSearch">
                                        <ul>
                                            <li>
                                                修改时间：
                                                <asp:TextBox runat="server" ID="txtStartDate" placeholder="开始时间" CssClass="cpsInput border_shadow cpsTimeInput" onFocus="WdatePicker({el:'txtStartDate',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                                <i>—</i>
                                                <asp:TextBox runat="server" ID="txtEndDate" placeholder="截止时间" CssClass="cpsInput border_shadow cpsTimeInput" onFocus="WdatePicker({el:'txtEndDate',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                            </li>
                                            <li>
                                                <input type="button" id="btnSearch" value="筛选"  class="btnAction" />
                                            </li>
                                        </ul>
                                    </div>
                                    <dl>
                                        <dt>
                                            <div style="width: 4%;">
                                                序号</div>
                                            <div style="width: 20%;">
                                                修改时间</div>
                                            <div style="width: 20%;">
                                                双色球</div>
                                            <div style="width: 20%;">
                                                福彩3D</div>
                                            <div style="width: 20%;">
                                                七乐彩</div>
                                            <div style="width: 10%;">
                                                查看更多</div>
                                        </dt>
                                        <asp:Repeater ID="rpt_dataList" runat="server">
                                            <ItemTemplate>
                                                <dd>
                                                    <div style="width: 4%;">
                                                        <%#Container.ItemIndex+1%></div>
                                                    
                                                    <div style="width: 20%;">
                                                        <%#Shove._Convert.StrToDateTime(Eval("HandlelDateTime").ToString(), DateTime.Now.ToString()).ToString("yyyy-MM-dd HH:mm:ss")%></div>
                                                    <div style="width: 20%;">
                                                        <%# DAL.Functions.F_CpsGetBonusScale2(Convert.ToInt64(Eval("Type")), 5)%></div>
                                                    <div style="width: 20%;">
                                                        <%# DAL.Functions.F_CpsGetBonusScale2(Convert.ToInt64(Eval("Type")), 6)%></div>
                                                    <div style="width: 20%;">
                                                        <%# DAL.Functions.F_CpsGetBonusScale2(Convert.ToInt64(Eval("Type")), 13)%></div>
                                                    
                                                    <div style="width: 10%;">
                                                        <a href='CommissionScaleRecord.aspx?CpsID=<%#Eval("CpsID") %>&UserName=<%# HttpUtility.UrlEncode(UserName + "") %>&UserType=<%#Eval("Type")%>'
                                                            target="_blank">查看更多</a>
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
    <input type="hidden" runat="server" id="hideCpsID" value="" />
    <input type="hidden" runat="server" id="hideUserName" value="" />
    <input type="hidden" runat="server" id="hideUserType" value="" />
    <uc2:Footer ID="Footer1" runat="server" />
    </form>
</body>
<script type="text/javascript">
    $(function () {
        $("#btnSearch").click(function () {
            var parameter = "?";
            parameter += "CpsID=" + FlyFish.Trim($("#hideCpsID").val());
            parameter += "&UserName=" + FlyFish.Trim($("#hideUserName").val());
            parameter += "&UserType=" + FlyFish.Trim($("#hideUserType").val());
            parameter += "&StartTime=" + FlyFish.Trim($("#txtStartDate").val());
            parameter += "&EndTime=" + FlyFish.Trim($("#txtEndDate").val());
            window.location.href = parameter;
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
        var parameter = "?";
        parameter += "CpsID=" + FlyFish.Trim($("#hideCpsID").val());
        parameter += "&UserName=" + FlyFish.Trim($("#hideUserName").val());
        parameter += "&UserType=" + FlyFish.Trim($("#hideUserType").val());
        parameter += "&StartTime=" + FlyFish.Trim($("#txtStartDate").val());
        parameter += "&EndTime=" + FlyFish.Trim($("#txtEndDate").val());

        //初始化分页控件
        //有些参数是可选的，比如lang，若不传有默认值
        sand.init({
            pno: pageNo,
            //总页码
            total: totalPage,
            //总数据条数
            totalRecords: totalRecords,
            //链接前部
            hrefFormer: 'CommissionScale2',
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
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserBuyLotteryDetails.aspx.cs"
    Inherits="CPS_Promote_UserBuyLotteryDetails" %>

<%@ Register Src="../userControls/Footer.ascx" TagName="Footer" TagPrefix="uc1" %>
<%@ Register Src="../userControls/PromoteHeader.ascx" TagName="PromoteHeader" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-推广联盟-购彩记录</title>
    <meta name="description" content="<%=_Site.Name %>-推广联盟-购彩记录" />
    <meta name="keywords" content="<%=_Site.Name %>-推广联盟-购彩记录" />
    <link href="../css/common.css" rel="stylesheet" />
    <link type="text/css" href="../../Style/sandPage.css" rel="stylesheet" />
    <style type="text/css">
        .numcode a
        {
            color: Blue;
        }
        .numcode a:hover
        {
            text-decoration: underline;
        }
        .schemeNumber a
        {
            color: Blue;
        }
        .schemeNumber a:hover
        {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc2:PromoteHeader ID="PromoteHeader1" runat="server" />
    <div class="user_banner">
        <h3>
           购彩记录<span>BuyLottery Details</span></h3>
    </div>
    <div class="content">
        <div class="inside_bg">
            <div class="insicon">
                <div class="sidebar">
                    <ul>
                        <li class="curr"><a href="javascript:void(0);">购彩记录</a></li>
                    </ul>
                </div>
                <div class="commain">
                    <div class="mainbor">
                        <div id="user_list" class="number_record">
                            <h4>
                                <span style="padding: 0px 10px; width: 200px;"><%=userName  %>-购彩记录</span>
                                <div class="choice">
                                    <a id="openoffbut" class="open" href="javascript:;">展开更多筛选</a>
                                </div>
                            </h4>
                            <div class="num_search" runat="server" id="divSearch">
                                <div class="sea_lottey" style="padding-left: 0px;">
                                    <label>
                                        <span>彩种：</span>
                                        <asp:DropDownList runat="server" ID="ddlLottery">
                                        </asp:DropDownList>
                                    </label>
                                </div>
                                <div class="sea_buytime">
                                    <span>时间：</span>
                                    <asp:TextBox runat="server" ID="txtStartTime" onFocus="WdatePicker({el:'txtStartTime',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                    <i>—</i>
                                    <asp:TextBox runat="server" ID="txtEndTime" onFocus="WdatePicker({el:'txtEndTime',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})" />
                                </div>
                                <div class="sea_but" style="margin: 0px; padding-left: 20px;">
                                    <span></span>
                                    <input type="button" id="btnSearch" value="筛选" /></div>
                            </div>
                            <dl>
                                <dt>
                                    <div style="width: 10%; text-align: center;">
                                        序号</div>
                                    <div style="width: 15%; text-align: center;">
                                        <a href="javascript:;" style="color: blue; text-decoration: underline" onclick="order('PrintOutDateTime',this)"
                                            orderway="desc" runat="server" id="orderForPrintOutDateTime">出票时间</a></div>
                                    <div style="width: 10%; text-align: center;">
                                        彩种</div>
                                    <div style="width: 35%; text-align: center;">
                                        方案编号</div>
                                    <div style="width: 10%; text-align: center;">
                                        <a href="javascript:;" style="color: blue; text-decoration: underline" onclick="order('BuyMoney',this)"
                                            orderway="desc" runat="server" id="orderForBuyMoney">购买金额</a></div>
                                    <div style="width: 10%; text-align: center;">
                                        佣金比例</div>
                                    <div style="width: 10%; text-align: center;">
                                        <a href="javascript:;" style="color: blue; text-decoration: underline" onclick="order('Money',this)"
                                            orderway="desc" runat="server" id="orderForMyCommission">我的佣金</a></div>
                                </dt>
                                <asp:Repeater runat="server" ID="rpt_list" OnItemDataBound="rpt_list_ItemDataBound">
                                    <ItemTemplate>
                                        <dd>
                                            <div style="width: 10%; text-align: center;">
                                                <%#Container.ItemIndex + 1 %></div>
                                            <div style="width: 15%; text-align: center;">
                                                <%#Eval("DateTime")%></div>
                                            <div style="width: 10%; text-align: center;">
                                                <%#Eval("LotteryName")%></div>
                                            <div style="width: 35%; text-align: center;" class='schemeNumber'>
                                                <a href='/Home/Room/Scheme.aspx?ID=<%#Eval("SchemeID") %>' target="_blank">
                                                    <%#Eval("SchemeNumber")%></a></div>
                                            <div style="width: 10%; text-align: center;">
                                                <%#Shove._Convert.StrToDouble(Eval("BuyMoney").ToString(), 0).ToString("0.00")%></div>
                                            <div style="width: 10%; text-align: center;">
                                                <%#Eval("BonusScale")%></div>
                                            <div style="width: 10%; text-align: center;">
                                                <%#Shove._Convert.StrToDouble(Eval("Bonus").ToString(), 0).ToString("0.00")%></div>
                                        </dd>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <dd>
                                    <div style="width: 10%; text-align: center;">
                                        总 计</div>
                                    <div style="width: 15%; text-align: center;">
                                        -</div>
                                    <div style="width: 10%; text-align: center;">
                                        -</div>
                                    <div style="width: 35%; text-align: center;">
                                        -</div>
                                    <div style="width: 10%; text-align: center; color: Red;">
                                        <%=sumBuyMoney.ToString("0.00") %></div>
                                    <div style="width: 10%; text-align: center;">
                                        -</div>
                                    <div style="width: 10%; text-align: center; color: Red;">
                                        <%=sumMoney.ToString("0.00") %></div>
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
    <input type="hidden" id="hideUserID" value="" runat="server" />
    <input type="hidden" id="hideUserName" value="" runat="server" />
    <input type="hidden" id="hideOrder" value="PrintOutDateTime" runat="server" />
    <input type="hidden" id="hideOrderWay" value="desc" runat="server" />
    <uc1:Footer ID="Footer1" runat="server" />
    </form>
</body>
<script type="text/javascript">
    function order(tag, a) {
        $("#hideOrder").val(tag);
        $("#hideOrderWay").val($(a).attr("orderWay"));
        var param = "UserID=" + FlyFish.Trim($("#hideUserID").val());
        param += "&UserName=" + FlyFish.Trim($("#hideUserName").val());
        param += "&Lottery=" + FlyFish.Trim($("#ddlLottery option:selected").val());
        param += "&StartTime=" + FlyFish.Trim($("#txtStartTime").val());
        param += "&EndTime=" + FlyFish.Trim($("#txtEndTime").val());
        param += "&order=" + $("#hideOrder").val(); ;
        param += "&orderWay=" + $("#hideOrderWay").val(); ;

        window.location.href = "?" + param;
    }
    $(function () {
        $("#btnSearch").click(function () {
            var param = "UserID=" + FlyFish.Trim($("#hideUserID").val());
            param += "&UserName=" + FlyFish.Trim($("#hideUserName").val());
            param += "&Lottery=" + FlyFish.Trim($("#ddlLottery option:selected").val());
            param += "&StartTime=" + FlyFish.Trim($("#txtStartTime").val());
            param += "&EndTime=" + FlyFish.Trim($("#txtEndTime").val());
            param += "&order=" + $("#hideOrder").val(); ;
            param += "&orderWay=" + $("#hideOrderWay").val(); ;

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

        var parameter = "UserID=" + FlyFish.Trim($("#hideUserID").val());
        parameter += "&UserName=" + FlyFish.Trim($("#hideUserName").val());
        parameter += "&Lottery=" + FlyFish.Trim($("#ddlLottery option:selected").val());
        parameter += "&StartTime=" + FlyFish.Trim($("#txtStartTime").val());
        parameter += "&EndTime=" + FlyFish.Trim($("#txtEndTime").val());
        parameter += "&order=" + $("#hideOrder").val(); ;
        parameter += "&orderWay=" + $("#hideOrderWay").val(); ;
        //初始化分页控件
        //有些参数是可选的，比如lang，若不传有默认值
        sand.init({
            pno: pageNo,
            //总页码
            total: totalPage,
            //总数据条数
            totalRecords: totalRecords,
            //链接前部
            hrefFormer: 'UserBuyLotteryDetails',
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

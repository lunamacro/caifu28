<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgentDate.aspx.cs" Inherits="CPS_Agent_AgentDate" %>

<%@ Register Src="../userControls/AgentHeader.ascx" TagName="AgentHeader" TagPrefix="uc1" %>
<%@ Register Src="../userControls/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-推广联盟-我的推广</title>
    <meta name="description" content="<%=_Site.Name %>-推广联盟-我的推广" />
    <meta name="keywords" content="<%=_Site.Name %>-推广联盟-我的推广" />
    <link type="text/css" rel="stylesheet" href="../css/common.css" />
    <style type="text/css">
        .column1
        {
            width: 10%;
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
            width: 15%;
        }
        .column5
        {
            width: 15%;
        }
        .column6
        {
            width: 10%;
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
        /*.aff_linkbox a
        {
            display: inline-block;
            width: 80px;
            height: 25px;
            line-height: 25px;
            text-align: center;
            border: 1px solid #dfdfdf;
        }
        .aff_linkbox a:active
        {
            border-color: Red;
        }*/
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:AgentHeader ID="AgentHeader1" runat="server" />
    <div class="user_banner">
        <h3>
            我的推广<span>My Information</span></h3>
    </div>
    <div class="content">
        <div class="inside_bg">
            <div class="insicon">
                <div class="sidebar">
                    <ul>
                        <li class="curr"><a href="javascript:void(0);" onclick="location.reload()">我的推广</a></li>
                    </ul>
                </div>
                <div class="commain">
                    <div class="mainbor">
                        <div class="affiliate_links">
                            <h4>
                                <span>推广链接</span></h4>
                            <div class="aff_linkbox">
                                <ul>
                                    <li>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;PC端推广地址：<i id="address"><%= this.GetLinkByType("pc") %></i>&nbsp;&nbsp;&nbsp;&nbsp;<span
                                        class="linka"><a href="javascript:void(0);" id="btnCopyPC" class="btnAction" style="display:inline-block;">复 制</a>&nbsp;&nbsp;<a
                                            href="javascript:void(0);" onclick="showTwoDimensionCode('pc')" class="btnAction" style="display:inline-block;">查看二维码</a></span>
                                    </li>
                                </ul>
                                <div class="QR_code" id="twoDimensionCodeContainer" style="display: none;">
                                    <img src='http://qr.liantu.com/api.php?text=<%=this.GetLinkByType("pc") %>&w=240'
                                        id="pc_twoDimensionCode" style="display: none;" alt="">
                                    <p style="text-align: center; width: 100%;" id="type">
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="comm_ratio">
                            <h4>
                                <span>我的佣金比例</span></h4>
                            <div class="ration_wrap">
                                <ul>
                                    <li runat="server" id="li_ssq" visible="false"><i>双色球：</i><span><asp:Literal ID="ltSSQ"
                                        runat="server" Text="0.00" /></span></li>
                                    <li runat="server" id="li_jxssc" visible="false"><i>江西时时彩：</i><span><asp:Literal
                                        ID="ltJXSSC" runat="server" Text="0.00" /></span></li>
                                    <li runat="server" id="li_jczq" visible="false"><i>竞彩足球：</i><span><asp:Literal ID="ltJCZQ"
                                        runat="server" Text="0.00" /></span></li>
                                    <li runat="server" id="li_qlc" visible="false"><i>七乐彩：</i><span><asp:Literal ID="ltQLC"
                                        runat="server" Text="0.00" /></span></li>
                                    <li runat="server" id="li_syydj" visible="false"><i>十一运夺金：</i><span><asp:Literal
                                        ID="ltSYYDJ" runat="server" Text="0.00" /></span></li>
                                    <li runat="server" id="li_jclq" visible="false"><i>竞彩蓝球：</i><span><asp:Literal ID="ltJCLQ"
                                        runat="server" Text="0.00" /></span></li>
                                    <li runat="server" id="li_fc3d" visible="false"><i>福彩3D：</i><span><asp:Literal ID="ltFC3D"
                                        runat="server" Text="0.00" /></span></li>
                                    <li runat="server" id="li_jsk3" visible="false"><i>江苏快3：</i><span><asp:Literal ID="ltJSK3"
                                        runat="server" Text="0.00" /></span></li>
                                    <li runat="server" id="li_sfc" visible="false"><i>胜负彩：</i><span><asp:Literal ID="ltSFC"
                                        runat="server" Text="0.00" /></span></li>
                                    <li runat="server" id="li_qxc" visible="false"><i>七星彩：</i><span><asp:Literal ID="ltQXC"
                                        runat="server" Text="0.00" /></span></li>
                                    <li runat="server" id="li_cqssc" visible="false"><i>重庆时时彩：</i><span><asp:Literal
                                        ID="ltCQSSC" runat="server" Text="0.00" /></span></li>
                                    <li runat="server" id="li_rx9" visible="false"><i>任选九：</i><span><asp:Literal ID="ltRXJ"
                                        runat="server" Text="0.00" /></span></li>
                                    <li runat="server" id="li_pl3" visible="false"><i>排列三：</i><span><asp:Literal ID="ltPL3"
                                        runat="server" Text="0.00" /></span></li>
                                    <li runat="server" id="li_jx11x5" visible="false"><i>江西11选5：</i><span><asp:Literal
                                        ID="ltJX11X5" runat="server" Text="0.00" /></span></li>
                                    <li runat="server" id="li_scjqc" visible="false"><i>四场进球彩：</i><span><asp:Literal
                                        ID="ltSCJQC" runat="server" Text="0.00" /></span></li>
                                    <li runat="server" id="li_pl5" visible="false"><i>排列五：</i><span><asp:Literal ID="ltPL5"
                                        runat="server" Text="0.00" /></span></li>
                                    <li runat="server" id="li_gd11x5" visible="false"><i>广东11选5：</i><span><asp:Literal
                                        ID="ltGD11X5" runat="server" Text="0.00" /></span></li>
                                    <li runat="server" id="li_lcbqc" visible="false"><i>六场半全场：</i><span><asp:Literal
                                        ID="ltLCBQC" runat="server" Text="0.00" /></span></li>
                                    <li runat="server" id="li_dlt" visible="false"><i>大乐透：</i><span><asp:Literal ID="ltDLT"
                                        runat="server" Text="0.00" /></span></li>
                                </ul>
                            </div>
                        </div>
                        <div id="user_list" class="commiss_statement">
                            <h4>
                                <span>佣金结算表</span> &nbsp;&nbsp;&nbsp;&nbsp;年份：<asp:DropDownList runat="server" ID="ddlYear"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                </asp:DropDownList>
                            </h4>
                            <dl>
                                <dt>
                                    <div class="column1">
                                        时间</div>
                                    <div class="column2">
                                        我的会员交易量</div>
                                    <div class="column3">
                                        我的推广员交易量</div>
                                    <div class="column4">
                                        推广员会员交易量</div>
                                    <div class="column5">
                                        交易量</div>
                                    <div class="column6">
                                        佣金收入</div>
                                    <div class="column7">
                                        查看明细</div>
                                    <div class="column8">
                                        状态</div>
                                </dt>
                                <asp:Repeater runat="server" ID="rpt_list" OnItemDataBound="rpt_list_ItemDataBound">
                                    <ItemTemplate>
                                        <dd>
                                            <div class="column1">
                                                <%#Eval("Year")%>-<%#Eval("Month")%></div>
                                            <div class="column2">
                                                <%#Shove._Convert.StrToDouble(Eval("AgentMemberBuyLottery").ToString(), 0).ToString("0.00")%></div>
                                            <div class="column3">
                                                <%#Shove._Convert.StrToDouble(Eval("AgentPromoteBuyLottery").ToString(), 0).ToString("0.00")%></div>
                                            <div class="column4">
                                                <%#Shove._Convert.StrToDouble(Eval("AgentPromoteMemberBuyLottery").ToString(), 0).ToString("0.00")%></div>
                                            <div class="column5">
                                                <%#Shove._Convert.StrToDouble(Eval("BuyMoney").ToString(), 0).ToString("0.00")%></div>
                                            <div class="column6">
                                                <%#Shove._Convert.StrToDouble(Eval("PayBonus").ToString(), 0).ToString("0.00")%></div>
                                            <div class="column7">
                                                <%#this.GetDetailsLink(Eval("Year"), Eval("Month"))%></div>
                                            <div class="column8">
                                                <%# Convert.ToBoolean(Eval("IsPayOff")) ? "已发放" : "未发放"%></div>
                                        </dd>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <dd>
                                    <div class="column1">
                                        总 计</div>
                                    <div class="column2 red">
                                        <%=sumAgentMemberBuyLottery.ToString("0.00")%></div>
                                    <div class="column3 red">
                                        <%=sumAgentPromoteBuyLottery.ToString("0.00")%></div>
                                    <div class="column4 red">
                                        <%=sumAgentPromoteMemberBuyLottery.ToString("0.00")%></div>
                                    <div class="column5 red">
                                        <%=sumBuyMoney.ToString("0.00")%></div>
                                    <div class="column6 red">
                                        <%=sumPayBonus.ToString("0.00")%></div>
                                    <div class="column7 red">
                                        -</div>
                                    <div class="column8">
                                        -</div>
                                </dd>
                            </dl>
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
    function showTwoDimensionCode(tag) {
        switch (tag) {
            case "pc":
                $("#twoDimensionCodeContainer").show();
                $("#pc_twoDimensionCode").show();
                $("#type").html("亲,快扫我吧!");
                break;
        }
    }
    $(function () {
        init();
    });
    function init() {
        var clip = new ZeroClipboard.Client(); // 新建一个对象
        clip.setHandCursor(true);
        clip.setText($("#address").html()); // 设置要复制的文本。
        clip.addEventListener("mouseUp", function (client) {
            alert("复制成功！");
        });
        // 注册一个 button，参数为 id。点击这个 button 就会复制。
        clip.glue("btnCopyPC"); // 和上一句位置不可调换
    }
    
</script>
</html>

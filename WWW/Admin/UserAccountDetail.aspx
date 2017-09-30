<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserAccountDetail.aspx.cs"
    Inherits="Admin_UserAccountDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户账户明细</title>
    
    <link href="../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../Style/main.css" type="text/css" rel="stylesheet" />
    <link href="../../Style/sandPage.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
    <link href="../Style/common.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/common.js" type="text/javascript"></script>
    <link href="../Style/jquery.mCustomScrollbar.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/jquery.mCustomScrollbar.concat.min.js" type="text/javascript"></script>
    <script src="../JScript/sandPage.js" type="text/javascript"></script>
    <script src="../Components/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <style type="text/css">
        #btn {
            display: block;
            height: 32px;
            width: 82px;
            background: url(../images/Sprite.png) no-repeat -105px -320px;
            text-indent: -9999px;
        }

        #del {
            top: 5px;
            display: block;
            float: left;
            color: #3977c3;
            font-family: "微软雅黑";
            width: 54px;
            margin: 10px;
            height: 25px;
            line-height: 22px;
            text-align: center;
            background-image: url(../images/Sprite.png) no-repeat -208px -318px;
        }

            #del:hove {
                background: #D6F7FE;
            }


        .send {
            text-align: right;
            padding-right: 30px;
        }

        .toptable {
        }

        .topbuttom {
            text-align: right;
            padding-right: 30px;
            margin: 30px 0;
        }

        #tab {
            border: 1px solid #dfdfdf;
            width: 100%;
        }

            #tab th {
                border: 1px solid #dfdfdf;
                background: #f7f7f7;
            }

            #tab td {
                border: 1px solid #dfdfdf;
            }

        .btnblue {
            width: 80px;
            height: 30px;
            background: url(../images/Sprite.png) no-repeat -405px -320px;
            border: 0;
            color: #fff;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="maincon">
                <h2>用户账户明细</h2>
                <div class="main_list">
                    <%--<br />
        <table width="96%" height="34" border="0" align="center" cellpadding="0" cellspacing="1"
            bgcolor="White">
            <tr>
                <td height="32" bgcolor="White" class="STYLE14" style="border-color:White;">
                    <div class="STYLE4" style="width:1200px;">
                        <div align="center" style="height: 14px; width: 1200px; color:Black"">
                            用户账户明细</div>
                    </div>
                </td>
            </tr>
        </table>--%>
                    <table cellspacing="0" cellpadding="0" width="90%" border="0" align="center">
                        <tr>
                            <td style="height: 30px">
                                <font face="微软雅黑">&nbsp;用户名：
                                <asp:TextBox ID="tbUserName" runat="server"></asp:TextBox>&nbsp; 摘要：
                                <select id="selMemo" runat="server">
                                    <option value="0">全部摘要</option>
                                    <option value=",1,">充值</option>
                                    <option value=",2,">中奖</option>
                                    <option value=",3,">兑换</option>
                                    <option value=",4,">保底冻结解除</option>
                                    <option value=",5,">追号冻结解除</option>
                                    <option value=",6,">提款冻结解除</option>
                                    <option value=",7,8,9,">撤单</option>
                                    <option value=",51,52,">佣金</option>
                                    <option value=",101,">购买</option>
                                    <option value=",102,">保底冻结</option>
                                    <option value=",103,">追号冻结</option>
                                    <option value=",104,">提款冻结</option>
                                    <%--<option value="短信费">短信费</option>--%>
                                    <%--<option value="手续费">手续费</option>--%>
                                    <option value=",107,">提款</option>
                                </select>
                                    <asp:DropDownList Style="display: none;" ID="ddlYear" runat="server" Width="88px">
                                    </asp:DropDownList>
                                    &nbsp;
                                <asp:DropDownList Style="display: none;" ID="ddlMonth" runat="server" Width="80px"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
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
                                    &nbsp;
                                <asp:DropDownList ID="ddlDay" Style="display: none;" runat="server" Width="88px">
                                </asp:DropDownList>
                                    <asp:TextBox ID="tbStartTime" runat="server" placeholder="开始时间" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>
                                    <asp:TextBox ID="tbEndTime" runat="server" placeholder="结束时间" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>
                                    &nbsp;</font>
                                <ShoveWebUI:ShoveConfirmButton ID="btnRead" runat="server" Text="读取数据" OnClick="btnRead_Click"
                                    CssClass="btnblue" />
                                <asp:TextBox ID="tbID" runat="server" Width="100px" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td align="center">
                                <%-- 删除的部分--%>
                                <%-- <div id="Div1" class="tobbutton">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 第<asp:Label ID="Label8"
                                    runat="server" Text="1"></asp:Label>
                                页&nbsp; 共<asp:Label ID="Label9" runat="server"></asp:Label>
                                页
                                <asp:LinkButton ID="LinkButton5" runat="server" OnClick="LinkButton1_Click">首页</asp:LinkButton>
                                <asp:LinkButton ID="LinkButton6" runat="server" OnClick="LinkButton2_Click">上一页</asp:LinkButton>
                                <asp:LinkButton ID="LinkButton7" runat="server" OnClick="LinkButton3_Click">下一页</asp:LinkButton>
                                <asp:LinkButton ID="LinkButton8" runat="server" OnClick="LinkButton4_Click">尾页</asp:LinkButton>
                            </div>--%>
                            </td>
                        </tr>
                    </table>
                    <table id="tab" class="newsTable" cellspacing="0" rules="all" style="border-collapse: collapse;">
                        <thead>
                            <tr>
                                <th class="isRead" style="width: 5%">时间
                                </th>
                                <th class="isRead">摘要
                                </th>
                                <th class="isRead" style="width: 5%">收入(元)
                                </th>
                                <th class="isRead" style="width: 5%">支出(元)
                                </th>
                                <th class="isRead" style="width: 5%">彩金
                                </th>
                                <th class="isRead" style="width: 5%">彩金余额
                                </th>
                                <%--    <th class="isRead" style="width:5%">
                                (手续费)
                            </th>--%>
                                <th class="isRead" style="width: 5%">余额
                                </th>
                                <%--<th class="isRead" style="width: 5%">
                                冻结金额
                            </th>--%>
                                <th class="isRead" style="width: 5%">中奖金额
                                </th>
                                <th class="edit" style="width: 5%">中奖总额
                                </th>
                                <th class="edit" style="width: 5%">当次不可提款金额
                                </th>
                                <th class="edit" style="width: 5%">限额比例
                                </th>
                                <th class="edit" style="width: 5%">来源
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptSchemes" runat="server" EnableViewState="false" OnItemDataBound="rptSchemes_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td class="isRead" style="width: 5%">
                                            <%#Eval("DateTime")%>
                                        </td>
                                        <td class="isRead" style="text-align: left;">
                                            <asp:Label runat="server" ID="id" Text='<%#Eval("Memo")%>'></asp:Label>
                                            <asp:Label runat="server" ID="ids" Visible="false"></asp:Label>
                                            <asp:Label runat="server" ID="schid" Text='<%#Eval("SchemeID")%>' Visible="false"></asp:Label>
                                        </td>
                                        <td class="isRead" style="width: 5%">
                                            <%#Eval("MoneyAdd").ToString()==""?"0.00":Convert.ToDecimal(Eval("MoneyAdd")).ToString("0.00")%>
                                        </td>
                                        <td class="isRead" style="width: 5%">
                                            <%#Eval("MoneySub").ToString() == "" ? "0.00" : Convert.ToDecimal(Eval("MoneySub")).ToString("0.00")%>
                                        </td>
                                        <td class="isRead" style="width: 5%">
                                            <%#Eval("HandselAmount").ToString() == "" ? "0.00" : Convert.ToDecimal(Eval("HandselAmount")).ToString("0.00")%>
                                        </td>
                                        <td class="isRead" style="width: 5%">
                                            <%#Eval("HandselTotal").ToString() == "" ? "0.00" : Convert.ToDecimal(Eval("HandselTotal")).ToString("0.00")%>
                                        </td>
                                        <%--<td class="isRead" style="width:5%">
                                        <%#  Eval("FormalitiesFees")%>
                                    </td>--%>
                                        <td class="isRead" style="width: 5%">
                                            <%#Eval("Balance").ToString() == "" ? "0.00" : Convert.ToDecimal(Eval("Balance")).ToString("0.00")%>
                                        </td>
                                        <%--                                    <td class="isRead" style="width: 5%">
                                        <%#Eval("Balance").ToString() == "" ? "0.00" : Convert.ToDecimal(Eval("Balance")).ToString("0.00")%>
                                    </td>--%>
                                        <td class="isRead" style="width: 5%">
                                            <%#Eval("Reward").ToString() == "" ? "0.00" : Convert.ToDecimal(Eval("Reward")).ToString("0.00")%>
                                        </td>
                                        <td class="edit" style="width: 5%">
                                            <%#Eval("SumReward").ToString() == "" ? "0.00" : Convert.ToDecimal(Eval("SumReward")).ToString("0.00")%>
                                        </td>
                                        <td class="edit" style="width: 5%">
                                            <%#ReWriteStr(Eval("NoCash").ToString() == "" ? "0.00" : Convert.ToDecimal(Eval("NoCash")).ToString("0.00"))%>
                                        </td>
                                        <td class="edit" style="width: 5%">
                                            <%#Calculation(Eval("NoCash") ,Eval("MoneyAdd"))%>
                                        </td>
                                        <td class="edit" style="width: 5%">
                                            <%#  Eval("Comefrom")!=""?Eval("Comefrom"):"系统"%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                    <div id="sand" style="text-align: right; padding-right: 30px;">
                    </div>
                    <input name="index" value="1" id="index" hidden="hidden" type="hidden" />
                    <br />
                </div>
            </div>
        </div>
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

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserDistillWaitPay.aspx.cs"
    Inherits="Admin_UserDistillWaitPay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>待付款用户一览表</title>
    
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
        tr {
            height: 35px;
        }

        .newsTable tbody td {
            text-align: center;
        }

        .newsTable .time {
            width: 3%;
        }

        .newsTable .title {
            width: 5%;
        }

        .newsTable .ti {
            width: 6%;
        }

        .newsTable .isShow {
            width: 4.5%;
        }

        .newsTable .isRead {
            width: 7%;
        }

        .newsTable .edit {
            width: 5%;
            border-right: 1px solid #dfdfdf;
        }

        .newsTable .btnEdit {
            display: block;
            float: left;
            color: #3977C3;
            font-family: "微软雅黑";
            width: 70px;
            margin: 10px;
            height: 22px;
            line-height: 22px;
            text-align: center;
            border: 0px;
            text-decoration: underline;
            cursor: pointer;
            text-decoration: none;
            background: #C7E8FE;
        }

        .btnEdit:hover {
            background: #D6F7FE;
        }

        #lbtnPayByAlipay {
            border-radius: 5px;
            height: 32px;
            width: 102px;
            background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#C2E6FE), to(#C2E6FE));
            border: 0px;
            cursor: pointer;
        }

            #lbtnPayByAlipay:hover {
                border: 1px solid #3977C3;
            }

        #lbtnPayByBank {
            height: 32px;
            width: 102px;
            background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#C2E6FE), to(#C2E6FE));
            border: 0px;
            cursor: pointer;
        }

            #lbtnPayByBank:hover {
                border: 1px solid #3977C3;
            }

        Button:hove {
            border: 1px solid #3977C3;
        }

        .text {
            text-align: center;
            font-size: 18px;
        }

        .topbuttom {
            text-align: right;
            padding-right: 120px;
        }

        .toder {
            margin-left: 50px;
            margin-top: 5px;
        }

        #tab {
            border: 1px solid #dfdfdf;
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
                <h2>待付款用户一览表</h2>
                <div class="waitpay_wrap">
                    <asp:HiddenField ID="hfCurPayType" runat="server" Value="银行" />
                    <table cellspacing="0" cellpadding="0" class="toder" border="0">
                        <tr>
                            <td>&nbsp;开始日期
                            <asp:TextBox Height="19px" runat="server" ID="tbBeginTime" Width="100px" onblur="if(this.value=='') this.value=document.getElementById('hBeginTime').value"
                                name="tbBeginTime" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                                &nbsp;结束日期
                            <asp:TextBox runat="server" Height="19px" ID="tbEndTime" Width="100px" name="tbEndTime"
                                onblur="if(this.value=='') this.value=document.getElementById('hEndTime').value"
                                onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                                &nbsp;&nbsp;&nbsp;用户名称
                            <asp:TextBox runat="server" ID="tbUserName" Width="100px" Height="19px" />
                                &nbsp;<span style="color: #ff0000"><asp:Button ID="btnSearch" runat="server" Text="搜索"
                                    OnClick="btnSearch_Click" CssClass="btnblue" />
                                </span>&nbsp;&nbsp;&nbsp; 账户 <span style="color: #ff0000">
                                    <asp:DropDownList ID="ddlAccountType" Height="22px" runat="server" Width="109px"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlAccountType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;<%--<asp:Button  runat="server"  ID="lbtnPayByAlipay"  Text="查看所有代付用户" OnClientClick="return true;" onclick="lbtnPayByAlipay_Click" />
                            &nbsp;</span>--%>
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellspacing="0" cellpadding="0" class="wraptable" style="display: none;">
                        <tr>
                            <td>
                                <table border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td id="PayByBank" runat="server" class="NotSelectedTab" width="100px" align="center">
                                            <asp:Button ID="lbtnPayByBank" OnClientClick="return true;" runat="server" Text="提款到银行卡"
                                                OnClick="lbtnPayByBank_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table id="myIcaileTab" runat="server" cellspacing="0" rules="all" style="border-collapse: collapse;" class="wraptable">
                        <tr>
                            <td>
                                <table width="100%" border="0" cellpadding="0" cellspacing="1">
                                    <tr>
                                        <td height="30" align="center">
                                            <table width="100%" id="tab" class="newsTable" cellspacing="0" rules="all" style="border-collapse: collapse;">
                                                <thead>
                                                    <tr>
                                                        <th class="time">序号
                                                        </th>
                                                        <th class="time">流水号
                                                        </th>
                                                        <th class="isShow">用户名
                                                        </th>
                                                        <th class="ti">昵称
                                                        </th>
                                                        <th class="title">提取金额
                                                        </th>
                                                        <%--<th class="isShow">
                                                    手续费
                                                </th>--%>
                                                        <th class="title">应付金额
                                                        </th>
                                                        <th class="title">申请时间
                                                        </th>
                                                        <th class="isRead">状态
                                                        </th>
                                                        <th class="edit">提款银行卡帐号
                                                        </th>
                                                        <th class="isRead">开户银行地址
                                                        </th>
                                                        <th class="isRead">开户银行
                                                        </th>
                                                        <th class="isShow" style="width: 6%">持卡人姓名
                                                        </th>
                                                        <th class="edit">操作
                                                        </th>
                                                        <th class="edit">备注
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptSchemes" runat="server" EnableViewState="false" OnItemCommand="rptSchemes_ItemCommand">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="isShow">
                                                                    <%# Container.ItemIndex + 1%>
                                                                </td>
                                                                <td class="isShow">
                                                                    <%#Eval("ID")%>
                                                                </td>
                                                                <td class="isShow">
                                                                    <%#Eval("Name")%>
                                                                </td>
                                                                <td class="isShow">
                                                                    <%#Eval("NickName")%>
                                                                </td>
                                                                <td class="isShow">
                                                                    <%#  (Shove._Convert.StrToDouble(Eval("Money").ToString(), 0) + Shove._Convert.StrToDouble(Eval("HandselMoney").ToString(), 0)).ToString("0.00")%>
                                                                </td>
                                                                <%-- <td class="isShow">

                                                               <%#  Shove._Convert.StrToDouble(Eval("FormalitiesFees").ToString(),0).ToString("0.00")%>
                                                        </td>--%>
                                                                <td class="isShow">
                                                                    <%# (Convert.ToDouble(DataBinder.Eval(Container.DataItem, "Money")) + Shove._Convert.StrToDouble(Eval("HandselMoney").ToString(), 0) - Convert.ToDouble(DataBinder.Eval(Container.DataItem, "FormalitiesFees"))).ToString("0.00")%>
                                                                </td>
                                                                <td class="isShow">
                                                                    <%# Eval("DateTime")%>
                                                                </td>
                                                                <td class="isRead">
                                                                    <%# DataBinder.Eval(Container.DataItem, "Result").ToString()=="10"?"接受提款(待付款)":"出错"%>
                                                                </td>
                                                                <td class="isRead">
                                                                    <%#Eval("BankCardNumber")%>
                                                                </td>
                                                                <td class="isShow">
                                                                    <%#Eval("BankAddress")%>
                                                                </td>
                                                                <td class="isRead">
                                                                    <%#Eval("BankName")%>
                                                                </td>
                                                                <td class="isRead">
                                                                    <%#Eval("BankUserName")%>
                                                                </td>
                                                                <td class="edit">
                                                                    <asp:Button ID="btnPay" runat="server" CommandArgument='<%#Eval("UserID")%>' CommandName="Pay"
                                                                        Height="22px" Width="75" Text="已线下付款" OnClientClick="return confirm('您确认已线下付款了吗?');" />
                                                                    <asp:Button ID="btnNoAccept" runat="server" CommandArgument='<%#Eval("UserID")%>'
                                                                        CommandName="DistillNoAccept" Height="22px" Width="75" Text="拒绝提款" OnClientClick="return confirm('您确认拒绝提款吗? 此操作将会返还会员提款的金额!');" />
                                                                </td>
                                                                <td class="edit">
                                                                    <asp:TextBox ID="idd" runat="server" Visible="false" Text='<%#Eval("ID") %>'></asp:TextBox>
                                                                    <asp:TextBox ID="tbMemo" runat="server" MaxLength="50" Height="16px" Width="80" />
                                                                    <asp:Button ID="btnMemo" runat="server" CommandArgument='<%# Eval("Memo") %>' CommandName="EditMemo"
                                                                        Height="22px" Width="40" Text="修改" OnClientClick="return confirm('您确认要修改吗?');" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                            <div id="sand" class="topbutton" style="text-align: right; padding-right: 30px;">
                                            </div>
                                        </td>
                                    </tr>
                            </td>
                        </tr>
                    </table>
                    <ShoveWebUI:ShoveConfirmButton ID="btnOKAll" Visible="false" runat="server" Width="129px"
                        Height="28px" Text="已全部转账" AlertText="确信已全部转账吗？" OnClick="btnOKAll_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <ShoveWebUI:ShoveConfirmButton ID="btnAlipayToBank" Style="display: none" runat="server"
                    Width="129px" Height="28px" Text="批量派发至银行卡" AlertText="确信输入无误，并立即派发用户的银行卡吗？"
                    OnClick="btnAlipayToBank_Click" />
                </div>
            </div>
        </div>
        <input type="hidden" value="<%=PageIndex%>" id="PageIndex" name="PageIndex" />
        <input type="hidden" value="<%=PageCount%>" id="PageCount" name="PageCount" />
        <input type="hidden" value="<%=DataCount%>" id="DataCount" name="DataCount" />
    </form>
</body>
</html>
<script type="text/javascript">
    function thisSubmit(n) {

        $("#PageIndex").val(n);
        $("#form1").submit();
    }


    $(function () {


        var totalPage = $("#PageCount").val();
        var totalRecords = $("#DataCount").val();
        var pageNo = $("#PageIndex").val();

        //            var parameter = "?radom=" + Math.random() + "&lotteryid=" + lotteryID + "&filter=" + filter + "&isuseid=" + isuseID + "&search=" + search + "&sort=" + sort + "&order=" + order;
        //            //初始化分页控件
        //有些参数是可选的，比如lang，若不传有默认值
        sand.init({
            pno: pageNo,
            //总页码
            total: totalPage,
            //总数据条数
            totalRecords: totalRecords,
            //链接前部
            //                hrefFormer: 'schemeall',
            //                //链接尾部
            //                hrefLatter: '.aspx',

            getLink: function (n) {

                return 'javascript:thisSubmit(' + n + ')';
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

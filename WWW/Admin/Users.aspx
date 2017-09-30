<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="Admin_Users" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户一览表</title>

    <link href="../Style/Admin.css" type="text/css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src="../Components/My97DatePicker/WdatePicker.js"></script>
    <link href="../../Style/sandPage.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
    <link href="../Style/common.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/common.js" type="text/javascript"></script>
    <link href="../Style/jquery.mCustomScrollbar.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/jquery.mCustomScrollbar.concat.min.js" type="text/javascript"></script>
    <script src="../JScript/sandPage.js" type="text/javascript"></script>
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

        .newsTable .isRead {
            width: 5%;
        }

        .newTable a:hover {
            color: #888;
        }

        .newTable a:hover {
            color: #000;
            text-decoration: none;
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

        .table1 tr {
            line-height: 50px;
        }

        .td1 {
            width: 110px;
            text-align: right;
        }

        .td2 {
            width: 500px;
        }

        .td3 {
            width: 200px;
        }

            .td2 input, .td3 input {
                padding: 5px 10px;
                font-size: 13px;
                border: 1px solid #A3CCF8;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="maincon">
                <h2>用户一览表</h2>
                <div class="main_list">
                    <br />
                    <%--<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="table1">
                        <tr>
                            <td class="td1">用户名：</td>
                            <td class="td3">--%>
                    <asp:TextBox ID="tbUserName" runat="server" CssClass="" placeholder="请输入用户名"></asp:TextBox>&nbsp;
                                <ShoveWebUI:ShoveConfirmButton
                                    ID="ShoveConfirmButton2" runat="server"
                                    BorderWidth="0px" Text="搜索用户" BorderStyle="None" OnClick="btnSearch_Click" CssClass="btnblue" />&nbsp;&nbsp;&nbsp;
                            <%--</td>
                            <td class="td1">手机号码：
                            </td>
                            <td class="td3">--%>
                    <asp:TextBox ID="tbMobile" runat="server" placeholder="请输入微信名称"></asp:TextBox>&nbsp;
                                <ShoveWebUI:ShoveConfirmButton
                                    ID="ShoveConfirmButton3" runat="server"
                                    BorderWidth="0px" Text="搜索昵称" BorderStyle="None" OnClick="ShoveConfirmButton3_Click" CssClass="btnblue" />&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="tbDaili" runat="server" placeholder="请输入上级代理账号"></asp:TextBox>&nbsp;
                                <ShoveWebUI:ShoveConfirmButton
                                    ID="ShoveConfirmButton4" runat="server"
                                    BorderWidth="0px" Text="搜索上级代理" BorderStyle="None" CssClass="btnblue" OnClick="ShoveConfirmButton4_Click" />&nbsp;&nbsp;&nbsp;
                            <%--</td>
                            <td class="td3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="td1">开始日期：
                            </td>
                            <td class="td3" colspan="3">--%><br /><br />
                    开始日期：<asp:TextBox runat="server" ID="tbBeginTime" name="tbBeginTime" onFocus="WdatePicker({el:'tbBeginTime',dateFmt:'yyyy-MM-dd HH:mm:ss', maxDate:'%y-%M-%d'})"
                        Height="15px" />&nbsp;
                                结束日期：
                                <asp:TextBox runat="server" ID="tbEndTime" name="tbEndTime" onFocus="WdatePicker({el:'tbEndTime',dateFmt:'yyyy-MM-dd HH:mm:ss', maxDate:'%y-%M-%d'})"
                                    Height="15px" />&nbsp;
                                <ShoveWebUI:ShoveConfirmButton ID="ShoveConfirmButton1"
                                    runat="server" BorderWidth="0px" Text="搜索" BorderStyle="None" CssClass="btnblue"
                                    OnClick="btnSearchByRegDate_Click" />&nbsp;
                                <ShoveWebUI:ShoveConfirmButton ID="btnSearchNoPay"
                                    runat="server" BorderWidth="0px" Text="未充值用户" BorderStyle="None" CssClass="btnblue"
                                    OnClick="btnSearchNoPay_Click" />
                    <asp:Button ID="btnDownload" Style="background-image: url(../Images/Admin/buttbg.gif);"
                        runat="server" BorderWidth="0px" Width="60px" Text="导出下载" BorderStyle="None"
                        Height="20px" OnClick="btnDownload_Click" Visible="false" />
                    &nbsp;
                            <ShoveWebUI:ShoveConfirmButton ID="btnSelect" BackgroupImage="../Images/Admin/buttbg.gif"
                                runat="server" BorderWidth="0px" Width="60px" Text="全部会员" BorderStyle="None"
                                Height="20px" OnClick="btnSelect_Click" Visible="false" />
                    <%--</td>
                        </tr>
                    </table>--%>
                    <br />
                    <br />
                    <table id="tab" class="newsTable" cellspacing="0" rules="all" style="border-collapse: collapse;">
                        <thead>
                            <tr>
                                <th class="isRead" style="width: 15%">用户名
                                </th>
                                <th class="isRead" style="width: 10%">昵称
                                </th>
                                <th style="width: 15%;">上级代理</th>
                                <th class="isRead" style="width: 10%">代理分组</th>
                                <th class="isRead" style="width: 10%">代理级别</th>
                                <th class="isRead" style="width: 10%">注册时间
                                </th>
                                <th class="isRead" style="width: 5%;">余额
                                </th>
                                <th class="isRead" style="width: 5%;">冻结
                                </th>
                                <th class="isRead" style="width: 5%;">彩金
                                </th>
                                <th class="isRead" style="width: 5%;">冻结彩金
                                </th>
                                <th class="isRead" style="width: 5%;">不可提款
                                </th>
                                <th class="isRead" style="width: 5%;">操作
                                </th>
                                <!--th class="edit">开户银行</!--th>
                                <th class="edit">银行卡号</th-->
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptSchemes" runat="server" EnableViewState="false">
                                <ItemTemplate>
                                    <tr id="useritem_<%#Eval("ID")%>">
                                        <td class="time">
                                            <a href="UserDetail.aspx?SiteID=1&ID=<%#Eval("ID")%>"
                                                title='<%# Eval("Name").ToString() %>'>
                                                <%# Eval("Name").ToString().Length ==0 ? Eval("NickName").ToString() : (Eval("Name").ToString())%>
                                            </a>
                                        </td>
                                        <td class="time" style="text-align: center; width: 12px;" title='<%# Eval("NickName").ToString() %>'>
                                           <%# Eval("NickName").ToString() %>
                                        </td>
                                        <td><%#Eval("ParentName") %></td>
                                        <td><%#Eval("GroupName")%></td>
                                        <td class="isRead">
                                            <%#  getAgentRank(Eval("isAgent").ToString()) %>
                                        </td>
                                        
                                        <td class="isRead">
                                            <%#  DateTime.Parse(Eval("RegisterTime").ToString()).ToString("yyyy-MM-dd")%>
                                        </td>
                                        <td class="isRead">
                                            <%#  Convert.ToDouble(Eval("Balance")).ToString("0.00")%>
                                        </td>
                                        <td class="isRead">
                                            <%#  Convert.ToDouble(Eval("Freeze")).ToString("0.00")%>
                                        </td>
                                        <td class="isRead">
                                            <%#  Convert.ToDouble(Eval("HandselAmount")).ToString("0.00")%>
                                        </td>
                                        <td class="isRead">
                                            <%#  Convert.ToDouble(Eval("HandselForzen")).ToString("0.00")%>
                                        </td>
                                        <td class="isRead">
                                            <%#  Convert.ToDouble(Eval("NoCash")).ToString("0.00")%>
                                        </td>
                                        <td class="isRead">
                                            <a href="javascript:;" onclick="deleteUser('<%#Eval("ID")%>');">删除用户</a>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>

                    <div id="sand" style="text-align: right; padding-right: 30px;"></div>
                    <input name="index" value="1" id="index" type="hidden" />
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

    function deleteUser(uid) {
        var seleVal = $("#useritem_" + uid);
        if (confirm("确认删除此会员吗?")) {
            //数据后台操作
            var handlerUrl = "/Admin/Handler/Andy.ashx";
            $.ajax({
                type: "Post",
                url: handlerUrl,
                data: {
                    "action": "deleteUser",
                    "userId": uid
                },
                cache: false,
                async: false,
                success: function (data) {
                    if (data.error == '0') {
                        alert("删除成功")
                        seleVal.remove();
                    }
                    else {
                        alert("删除失败")
                    }

                },
                error: function () {
                    //showAlertAtOneBtn('调用错误')
                    console.log('调用错误')
                }
            })
        }
    }


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

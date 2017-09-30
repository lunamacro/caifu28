<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserAddHandsel.aspx.cs" Inherits="Admin_UserAddHandsel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户彩金充值</title>
    
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
    </style>
    <style type="text/css">
        body {
            font-family: 微软雅黑;
        }

        input {
            border: 1px #ccc solid;
        }

        tr {
            height: 35px;
            margin-left: 30px;
        }

        td {
            border-bottom: 0px White;
        }

        #btnUpdate {
            border-radius: 5px;
            width: 100px;
            height: 30px;
            background-color: #529DE6;
            color: White;
        }

            #btnUpdate:hover {
                border: 1px solid #3977C3;
            }
    </style>
    <script language="javascript" type="text/javascript">
        var handlerUrl = "Handler/Handsel.ashx";

        window.onload = function () {
            //if ($("#IsHandN")[0].checked) {
            //    $("#trHandsel").hide();
            //}
            //else {
            //    $("#trHandsel").show();
            //}
            $("#tbUserName").focus(function () {

                $("#Name").css("color", "#ffff");
                $("#Name").text("请输入用户名");

            }).blur(function () {
                var tex = $("#tbUserName").val();

                if (tex == "") {

                    $("#Name").css("color", "red");
                    $("#Name").text("用户名不能为空");
                } else {
                    $("#Name").text("");

                }
                getHandselMoneyFunc();
            })


            $("#tbMoney").focus(function () {

                $("#Money").css("color", "#ffff");
                $("#Money").text("请输入充值金额");

            }).blur(function () {
                var tex = $("#tbMoney").val();

                if (tex == "") {

                    $("#Money").css("color", "red");
                    $("#Money").text("金额不能为空");
                } else {
                    $("#Money").text("");

                }
                getHandselMoneyFunc();

            });

            //            $("#IsHandY").click(function () {
            //                $("#trHandsel").show();
            //            });
            //            $("#IsHandN").click(function () {
            //                $("#trHandsel").hide();
            //            });
        }
        function getHandselMoneyFunc() {
            var successFunc = function (json) {
                if (json) {
                    $("#Handsel").html("￥" + parseFloat(json).toFixed(2));
                }
                else {
                    $("#Handsel").html("￥0.00");
                }
            };
            f_ajaxPost(handlerUrl, { "act": "GetHandselMoney", "handselMoney": $("#tbMoney").val(), "userName": $("#tbUserName").val() }, "", successFunc, null, null);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="maincon">
                <h2>用户彩金充值</h2>
                <div class="main_list">
                    <br />
                    <table cellspacing="0" cellpadding="0" width="90%" align="left" border="0" style="line-height: 2; width: 550px; margin-left: 20px;">
                        <!--2015-07-17修改-->
                        <tr style="display: none;">
                            <td align="right">选择站点：
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSites" runat="server" Width="172px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">用户名称：
                            </td>
                            <td>
                                <asp:TextBox ID="tbUserName" runat="server" BorderStyle="Solid" MaxLength="50" Width="168px"
                                    BorderWidth="1px"></asp:TextBox>
                                <span id="Name" style=""></span>
                            </td>
                        </tr>
                        <%--<tr>
                        <td align="right">
                            是否参与赠送彩金活动：
                        </td>
                        <td>
                            <label>
                                <input type="radio" name="IsHand" id="IsHandY" runat="server" />是</label>
                            <label style="margin-left: 10px;">
                                <input type="radio" name="IsHand" id="IsHandN" runat="server" checked />否</label>
                        </td>
                    </tr>--%>
                        <tr>
                            <td align="right">充值彩金金额：
                            </td>
                            <td>
                                <asp:TextBox ID="tbMoney" runat="server" BorderStyle="Solid" Width="168px" BorderWidth="1px"></asp:TextBox><span
                                    id="Money" style=""></span>
                            </td>
                        </tr>
                        <%--<tr id="trHandsel">
                        <td align="right">
                            赠送彩金：
                        </td>
                        <td>
                            <span id="Handsel" style="color: Red;" runat="server">￥0.00</span>
                        </td>
                    </tr>--%>
                        <%--<tr>
                        <td align="right">
                            充值方式：
                        </td>
                        <td>
                            <asp:RadioButton ID="rb1" runat="server" Checked="True" GroupName="rb" Text="正常手工充值" />
                            <asp:RadioButton ID="rb2" runat="server" Text="奖励" GroupName="rb" Style="margin-left: 5px;" />
                            <asp:RadioButton ID="rb3" runat="server" Text="购彩" GroupName="rb" Style="margin-left: 5px;" />
                            <asp:RadioButton ID="rb4" runat="server" Text="预付款" GroupName="rb" Style="margin-left: 5px;" />
                            <asp:RadioButton ID="rb5" runat="server" Text="转帐户" GroupName="rb" Style="margin-left: 5px;" />
                            <asp:RadioButton ID="rb6" runat="server" Text="其它" GroupName="rb" Style="margin-left: 5px;" />
                        </td>
                    </tr>--%>
                        <tr>
                            <td align="right">摘&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;要：
                            </td>
                            <td>
                                <asp:TextBox ID="tbMessage" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Label ID="Label3" runat="server" ForeColor="Red">&nbsp;提示：如果发生充值错误，可以再次用负数进行充减。</asp:Label>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btnGO0" runat="server" Text="立即充值" alerttext="确定输入正确并立即充值吗？" OnClick="btnGO_Click"
                                    CssClass="blueButtonClass" />
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

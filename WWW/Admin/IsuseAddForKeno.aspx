<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IsuseAddForKeno.aspx.cs"
    Inherits="Admin_IsuseAddForKeno" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    
    <title>彩票业务中心-期号添加</title>
    <link type="text/css" href="../Style/Site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="title">
                <a href="Isuse.aspx" target="mainFrame">期号管理</a> >> 期号添加
            </div>
            <div class="IsuseAddContent">
                <table id="Table1" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="td1">开始日期:</td>
                        <td class="td2">
                            <asp:TextBox ID="tbDate" runat="server" MaxLength="10"></asp:TextBox></td>
                        <td class="td3">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="td1">增加天数:</td>
                        <td class="td2">
                            <asp:TextBox ID="tbDays" runat="server" MaxLength="10" placeholder="增加天数"></asp:TextBox></td>
                        <td class="td3" width="400">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="td1">&nbsp;</td>
                        <td class="td2" colspan="2">高频性彩票，您只需要选择增加的天数，系统会自动在指定的天数内，增加每天的所有期号</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td colspan="2" align="left">
                            <asp:Button runat="server" ID="btnAdd" Text="增加" OnClick="btnAdd_Click" CssClass="btn_operate" />
                            <asp:Button runat="server" ID="btnBack" Text="取消" OnClick="btnBack_Click" CssClass="btn_operate" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <asp:HiddenField runat="server" ID="tbLotteryID" />
    </form>
</body>
</html>

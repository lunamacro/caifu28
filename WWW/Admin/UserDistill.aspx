<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserDistill.aspx.cs" Inherits="Admin_UserDistill" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>处理用户提款申请</title>
    
    <script src="../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
    <link href="../Style/common.css" rel="stylesheet" type="text/css" />
    <link href="../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../Style/main.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        .visibility {
            margin: 0px;
        }

        .hidden {
            display: none;
            margin: 0px;
        }

        .btnblue {
            width: 80px;
            height: 30px;
            background: url(../images/Sprite.png) no-repeat -405px -320px;
            border: 0;
            color: #fff;
        }
    </style>
    <script language="javascript" type="text/jscript">
        function CheckAll(form) {
            for (var i = 0; i < form.elements.length; i++) {
                var e = form.elements[i];
                if (e.type == "checkbox")
                    e.checked = form.chkAll.checked;
            }
        }
        function checkAddSMS(form) {
            var bln = false;
            for (i = 0; i < form.elements.length; i++) {
                var e = form.elements[i];
                if (e.type == "checkbox" && e.checked && e.name != "chkAll") {
                    bln = true;
                    break;
                }
            }

            if (bln) {
                if (confirm('确定要将选定的客户加入到短信营销吗?')) {
                    return true;
                }
                else {
                    for (var i = 0; i < form.elements.length; i++) {
                        var e = form.elements[i];
                        if (e.type == "checkbox")
                            e.checked = false;
                    }

                    return false;
                }
            }
            else {
                alert('请选择您要操作的客户');
                for (var i = 0; i < form.elements.length; i++) {
                    var e = form.elements[i];
                    if (e.type == "checkbox")
                        e.checked = false;
                }

                return;
            }
        }

        function checkAddEMail(form) {
            var bln = false;
            for (i = 0; i < form.elements.length; i++) {
                var e = form.elements[i];
                if (e.type == "checkbox" && e.checked && e.name != "chkAll") {
                    bln = true;
                    break;
                }
            }

            if (bln) {
                if (confirm('确定要选定的客户加入到邮件群发吗?')) {
                    return true;
                }
                else {
                    for (var i = 0; i < form.elements.length; i++) {
                        var e = form.elements[i];
                        if (e.type == "checkbox")
                            e.checked = false;
                    }

                    return false;
                }
            }
            else {
                alert('请选择您要操作的客户');
                for (var i = 0; i < form.elements.length; i++) {
                    var e = form.elements[i];
                    if (e.type == "checkbox")
                        e.checked = false;
                }

                return;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="maincon">
                <h2>处理用户提款申请</h2>
                <div class="newspic_addwrap">
                    <table id="Table1" cellspacing="0" cellpadding="0" width="96%" border="0" align="center">
                        <tr>
                            <td align="center">
                                <asp:DataList ID="g" runat="server" Width="100%" OnItemCommand="g_ItemCommand">
                                    <ItemTemplate>
                                        <font face="微软雅黑">
                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td style="height: 19px" align="left">
                                                        <input id="tbSiteID" type="hidden" size="1" value='<%# DataBinder.Eval(Container.DataItem,"SiteID") %>'
                                                            runat="server" />
                                                        <input id="tbID" type="hidden" size="1" value='<%# DataBinder.Eval(Container.DataItem,"ID") %>'
                                                            runat="server" />
                                                        <input id="tbUserID" type="hidden" size="1" value='<%# DataBinder.Eval(Container.DataItem,"UserID") %>'
                                                            runat="server" />
                                                        <input id="tbMoney" type="hidden" size="1" value='<%# DataBinder.Eval(Container.DataItem,"Money") %>'
                                                            runat="server" />
                                                        <input id="tbRealityName" type="hidden" size="1" value='<%# DataBinder.Eval(Container.DataItem,"NickName") %>'
                                                            runat="server" />
                                                        <input id="tbBankUserName" type="hidden" size="1" value='<%# DataBinder.Eval(Container.DataItem,"BankUserName") %>'
                                                            runat="server" />
                                                        <input id="tbAlipayID" type="hidden" size="1" value=''
                                                            runat="server" />
                                                        <input id="tbAlipayName" type="hidden" size="1" value=''
                                                            runat="server" />
                                                        <input id="tbMemo" type="hidden" size="1" value='<%# DataBinder.Eval(Container.DataItem,"Memo") %>'
                                                            runat="server" />
                                                        <input id="tbPersonal" type="hidden" size="1" value='-1' runat="server" />


                                                        用户姓名&nbsp;<font color="#ff0000"><%# DataBinder.Eval(Container.DataItem,"Name")%></font>&nbsp;
                                                    昵称&nbsp;<font color="#ff0000"><%# DataBinder.Eval(Container.DataItem,"NickName")%></font>&nbsp;
                                                        支行名称&nbsp;<font color="#ff0000"><%# DataBinder.Eval(Container.DataItem,"BankName")%></font>&nbsp;
                                                        银行地址&nbsp;<font color="#ff0000"><%# DataBinder.Eval(Container.DataItem,"BankAddress")%></font>&nbsp;
                                                        卡号&nbsp;<font color="#ff0000"><%# DataBinder.Eval(Container.DataItem,"BankCardNumber")%></font>
                                                            帐户名&nbsp;<font color="#ff0000"><%# DataBinder.Eval(Container.DataItem,"UserCradName") %></font>&nbsp;
                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 20px" align="left">申请时间&nbsp;<font color="#ff0000"><%# DataBinder.Eval(Container.DataItem,"DateTime")%></font>
                                                        &nbsp;提取金额<font color="#ff0000">
                                                            <%# Convert.ToDouble(DataBinder.Eval(Container.DataItem,"Money")).ToString("0.00")%></font>&nbsp;账户余额 <font color="#ff0000">
                                                                <%# double.Parse(Eval("Balance").ToString()).ToString("0.00")%></font>&nbsp;冻结金额
                                                    <font color="#ff0000">
                                                        <%# double.Parse(Eval("Freeze").ToString()).ToString("0.00")%></font>&nbsp;彩金 <font
                                                            color="#ff0000">
                                                            <%# double.Parse(Eval("HandselAmount").ToString()).ToString("0.00")%></font>&nbsp;冻结彩金
                                                    <font color="#ff0000">
                                                        <%# double.Parse(Eval("HandselForzen").ToString()).ToString("0.00")%></font>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 20px" align="left">提款类型&nbsp;<font color="#ff0000"><%# DataBinder.Eval(Container.DataItem,"MoneyType").ToString()=="1"?"彩金提款":"余额提款"%></font>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">理由
                                                    <asp:TextBox ID="tbMemo1" runat="server" Width="442px" MaxLength="50"></asp:TextBox>&nbsp;
                                                    <ShoveWebUI:ShoveConfirmButton ID="btnNoAccept" runat="server" CommandName="btnNoAccept"
                                                        Text="拒绝提款" CssClass="btnblue" AlertText="确定要拒绝比条用户提款申请吗？" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="5px"></td>
                                                </tr>
                                                <tr>
                                                    <td class="td2" height="30" align="left">摘要
                                                    <asp:TextBox ID="tbMemo2" runat="server" Width="442px" MaxLength="50"></asp:TextBox>&nbsp;
                                                    <ShoveWebUI:ShoveConfirmButton ID="btnAccept" runat="server" CommandName="btnAccept"
                                                        Text="接受提款" CssClass="btnblue" AlertText="确定接受提款吗？" />
                                                        <input id="tbBankName" type="hidden" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"BankName")%>' />
                                                        <input id="tbBankCardNumber" type="hidden" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"BankCardNumber")%>' />
                                                        <input id="tbIsCps" type="hidden" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"IsCps")%>' />
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                        </font>
                                    </ItemTemplate>
                                </asp:DataList>
                                <asp:Label ID="labTip" runat="server" ForeColor="Red" Text="暂无提款申请。"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BetUpdate.aspx.cs" Inherits="Admin_BetUpdate" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" href="../Style/Site.css" rel="stylesheet" />
    <style type="text/css">
        th {
            vertical-align: middle;
        }

        .timeSetContent {
            padding: 0 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="title">
                赔率与回水规则设置 
            </div>
            
            <div class="timeSetContent">
                <div style="float:left;width:47%">
            
                    <div style="float:left;margin-left:10%;">
                        选择大厅：
                        <asp:DropDownList ID="ddl_lottery" runat="server" Width="100" Height="26" OnSelectedIndexChanged="ddl_homelist_SelectedLotteryChange" AutoPostBack="true">
                            <asp:ListItem Text="加拿大28" Value="98"></asp:ListItem>
                            <asp:ListItem Text="北京28" Value="99"></asp:ListItem>
                        </asp:DropDownList>

                        <asp:DropDownList ID="ddl_homelist" runat="server" Width="140" Height="26" OnSelectedIndexChanged="ddl_homelist_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="普通会员房" Value="0"></asp:ListItem>
                            <asp:ListItem Text="贵宾会员房" Value="1"></asp:ListItem>
                            <asp:ListItem Text="VIP会员房" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>


                <table cellspacing="0" cellpading="0" style="float:left; width:100%;margin-top:10px">
                    <thead>
                        <tr>
                            <th>玩法名称</th>
                            <th>赔率</th>
                            <th>保存</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rpt_BetList">
                            <ItemTemplate>
                                <tr>
                                    <td class="td1">
                                        <asp:HiddenField runat="server" ID="WinTypeID" Value='<%#Eval("id") %>' />
                                        <%# Eval("Name") %>
                                    </td>
                                    <td class="td2">
                                        <asp:TextBox ID="DefaultMoney" runat="server"
                                            Text='<%# Shove._Convert.StrToDouble(Eval("DefaultMoney").ToString(),0.00).ToString("#0.00") %>' MaxLength="6">
                                        </asp:TextBox>
                                    </td>
                                    <td class="td4">
                                        <asp:Button ID="btn_save" runat="server" Text="保存" CssClass="btn_operate" OnClick="btn_save_Click" OnClientClick="return confirm('确认无误提交吗?')" /></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
                    </div>
                <div style="float:left;width:47%;margin-left:5%">
                    <div style="margin-left:45%;">
                        <asp:DropDownList ID="ddl_rueltList" runat="server" Width="140" Height="26" OnSelectedIndexChanged="ddl_rueltList_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="普通会员房" Value="0"></asp:ListItem>
                            <asp:ListItem Text="贵宾会员房" Value="1"></asp:ListItem>
                            <asp:ListItem Text="VIP会员房" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>


                <table cellspacing="0" cellpading="0" style="float:left;width:100%;margin-top:10px">
                    <thead>
                        <tr><th colspan="4" style="border-bottom:1px solid #ffffff;">回水设置</th></tr>
                        <tr>
                             
                            <th class="td1">最小值</th>
                            <th class="td2">最大值</th>
                            <th class="td2">比例</th>
                            <th class="td4">保存</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rpt_backteul">
                            <ItemTemplate>
                                <tr>
                                    <td class="td2">
                                        <asp:HiddenField runat="server" ID="teulid" Value='<%#Eval("ID") %>' />
                                        <asp:TextBox ID="MinMoney" runat="server" Text='<%# Shove._Convert.StrToDouble(Eval("MinMoney").ToString(),0.00).ToString("#0.00") %>'></asp:TextBox>
                                    </td>
                                    <td class="td2">
                                        <asp:TextBox ID="MaxMoney" runat="server" Text='<%# Shove._Convert.StrToDouble(Eval("MaxMoney").ToString(),0.00).ToString("#0.00") %>'></asp:TextBox>
                                    </td>
                                    <td class="td2">
                                        <asp:TextBox ID="Proportion" runat="server" Text='<%# Eval("Proportion") %>'></asp:TextBox>
                                    </td>
                                    <td class="td4">
                                         <asp:Button ID="btn_saveteul" runat="server" Text="保存" CssClass="btn_operate"  OnClientClick="return confirm('确认无误提交吗?')" OnClick="btn_saveteul_Click"/>
                                     </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>

                    <table cellspacing="0" cellpading="0" style="float:left;width:100%;margin-top:40px">
                    <thead>
                        <tr><th colspan="5" style="border-bottom:1px solid #ffffff;">投注限额</th></tr>
                        <tr>
                            <th class="td2">大厅</th>
                            <th class="td2">单注最低</th>
                            <th class="td2">单注最高</th>
                            <th class="td2">总额最高</th>
                            <th class="td4">保存</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rpt_limit">
                            <ItemTemplate>
                                <tr>
                                    <td class="td2">
                                        <%#Eval("hall_name") %>
                                     </td>
                                    <td class="td2">
                                        <asp:HiddenField runat="server" ID="limitid" Value='<%#Eval("ID") %>' />
                                        <asp:TextBox ID="MinLimit" runat="server" Text='<%#Eval("limit_min") %>'></asp:TextBox>
                                    </td>
                                    <td class="td2">
                                        <asp:TextBox ID="MaxLimit" runat="server" Text='<%#Eval("limit_max") %>'></asp:TextBox>
                                    </td>
                                    <td class="td2">
                                        <asp:TextBox ID="MaxLimitAll" runat="server" Text='<%# Eval("limit_max_all") %>'></asp:TextBox>
                                    </td>
                                    <td class="td4">
                                         <asp:Button ID="btn_savelimit" runat="server" Text="保存" CssClass="btn_operate"  OnClientClick="return confirm('确认无误提交吗?')" OnClick="btn_savelimit_Click"/>
                                     </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>


                    </div>
            </div>
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Open.aspx.cs" Inherits="Admin_Open"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title>彩票业务中心-开奖&派奖</title>
    <link type="text/css" href="../Style/Site.css" rel="stylesheet" />
    <script src="/kindeditor/kindeditor-min.js" type="text/javascript"></script>
    <script src="/kindeditor/lang/zh_CN.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Admin/JavaScript/Public.js"></script>
    <script type="text/javascript">
        var taxSwitch =<%=taxSwitch%>;
        function CalcMoneyNoWithTax(sender) {
            var WinMoney = StrToFloat(sender.value);

            var tbMoneyNoWithTax = document.getElementById(sender.id.replace("tbMoney", "tbMoneyNoWithTax"));

            if (!tbMoneyNoWithTax) {
                return;
            }
            if(taxSwitch==1)
            {
                if (WinMoney < 10000) {
                    tbMoneyNoWithTax.value = WinMoney;

                    return;
                }
                tbMoneyNoWithTax.value = Round(WinMoney * 0.8, 2);
            }
            else
            {
                tbMoneyNoWithTax.value = WinMoney;
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="title">
                开奖&派奖
            </div>
            <div class="OpenContent">
                <table cellpadding="0" cellspacing="0" width="35%" style="display: block; float: left;">
                    <tr>
                        <td class="td1">彩种:</td>
                        <td width="400">
                            <asp:DropDownList ID="ddlLottery" runat="server" Width="100px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlLottery_SelectedIndexChanged" Style="display: block; float: left;">
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                        <asp:DropDownList ID="ddlIsuse" runat="server" Width="110px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlIsuse_SelectedIndexChanged" Style="display: block; float: left; margin-left: 10px;">
                        </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="td1">开奖号码:</td>
                        <td width="400">
                            <asp:TextBox ID="tbWinNumber" runat="server" MaxLength="50" Style="ime-mode: disabled"></asp:TextBox>
                            <asp:Label ID="labTip" runat="server" ForeColor="Red">格式：31031100111110</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="td1">开奖操作:</td>
                        <td width="400">
                              <ShoveWebUI:ShoveConfirmButton ID="btnGO_Step1" runat="server" BackgroupImage="../Images/Admin/buttbg.gif"
                                Width="60px" Height="20px" Text="开奖1" OnClick="btnGO_Step1_Click" />
                                &nbsp;
                        <ShoveWebUI:ShoveConfirmButton ID="btnGO_Step2" Enabled="false" runat="server" BackgroupImage="../Images/Admin/buttbg.gif"
                            Width="60px" Height="20px" Text="开奖2" OnClick="btnGO_Step2_Click" />
                                &nbsp;
                        <ShoveWebUI:ShoveConfirmButton ID="btnGO_Step3" Enabled="false" runat="server" BackgroupImage="../Images/Admin/buttbg.gif"
                            Width="60px" Height="20px" Text="开奖3" OnClick="btnGO_Step3_Click" /><%--OnClientClick="editor.sync();"--%>
                        </td>
                    </tr>



                    <tr>
                        <td class="td1">&nbsp;</td>
                        <td width="400">
                            <ShoveWebUI:ShoveConfirmButton ID="btnGO" runat="server" BackgroupImage="../Images/Admin/buttbg.gif"
                                Width="60px" Height="20px" Text="开奖" AlertText="确信输入无误，并立即开奖吗？"
                                OnClick="btnGO_Click" Visible="false" />
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" width="40%" id="WinNumberOther" runat="server" style="display: block; float: left; margin-left: 0px;">
                    <tr>
                        <td style="height: 30px;">请输入本期奖金</td>
                    </tr>
                    <tr>
                        <td>
                            <span runat="server" id="sfcname"><b>胜负彩</b><br />
                            </span>
                            <asp:GridView ID="g" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333"
                                GridLines="None" OnRowDataBound="g_RowDataBound" Width="331px" BorderStyle="Solid"
                                BorderWidth="1px" DataKeyNames="DefaultMoney,DefaultMoneyNoWithTax">
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#EFF3FB" />
                                <EditRowStyle BackColor="#2461BF" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="奖级" />
                                    <asp:TemplateField HeaderText="奖金">
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbMoney" runat="server" MaxLength="10" Style="text-align: center; ime-mode: disabled" Width="100"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="税后奖金">
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbMoneyNoWithTax" MaxLength="10" Style="text-align: center; ime-mode: disabled" runat="server" Width="100"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <span runat="server" id="rjcname">
                                <br />
                                <b>任九场</b></span>
                            <asp:GridView ID="g1" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333"
                                GridLines="None" OnRowDataBound="g1_RowDataBound" Width="331px" BorderStyle="Solid"
                                BorderWidth="1px" DataKeyNames="DefaultMoney,DefaultMoneyNoWithTax">
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#EFF3FB" />
                                <EditRowStyle BackColor="#2461BF" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="奖级" />
                                    <asp:TemplateField HeaderText="奖金">
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbMoney" runat="server" MaxLength="10" Style="text-align: center; ime-mode: disabled" Width="100"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="税后奖金">
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbMoneyNoWithTax" MaxLength="10" Style="text-align: center; ime-mode: disabled" runat="server" Width="100"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">

                          
                        </td>
                    </tr>
                </table>
            </div>

        </div>
        <div>


            <input id="h_SchemeID" type="hidden" runat="server" />
            <br />
        </div>
    </form>
</body>
<script type="text/javascript" src="../JScript/jquery-1.8.3.min.js"></script>
<script type="text/javascript">
    window.onload = function () {
        $("#g tr td input:text").each(function (i, o) {
            //alert(i + "-" + $(o).val());
            //i % 2 == 0 是"最大奖金额度"文本框，不然就是"积分比例"文本框


            if (i % 2 == 0) {
                $(o).bind("keydown", function (e) {
                    var keyCode = e.keyCode;
                    // 数字
                    if (keyCode >= 48 && keyCode <= 57) return true
                    // 小数字键盘
                    if (keyCode >= 96 && keyCode <= 105) return true
                    // Backspace键
                    if (keyCode == 8 || keyCode == 37 || keyCode == 38 || keyCode == 39 || keyCode == 40 || keyCode == 190 || keyCode == 110) return true
                    return false;
                }).bind("blur", function () {
                    var val = $(o).val().replace(new RegExp(",", "g"), "");
                    if ("" == val) {
                        $(o).val("");
                        // CalcMoneyNoWithTax(o);
                        return;
                    }
                    if (!/^\d+((\.)?\d+)?$/.test(val)) {
                        $(o).val("");
                        // CalcMoneyNoWithTax(o);
                        return;
                    }
                    CalcMoneyNoWithTax(o);
                });
            } else {
                //注数文本框
                $(o).bind("keydown", function (e) {
                    var keyCode = e.keyCode;
                    // 数字
                    if (keyCode >= 48 && keyCode <= 57) return true
                    // 小数字键盘
                    if (keyCode >= 96 && keyCode <= 105) return true
                    //不能出现小数点
                    //if (keyCode == 190) return false;
                    // Backspace键
                    if (keyCode == 8 || keyCode == 37 || keyCode == 38 || keyCode == 39 || keyCode == 40) return true
                    return false;
                }).bind("blur", function () {
                    var val = $(o).val();
                    if ("" == val) {
                        $(o).val("");
                        return;
                    }
                    //                    if (!/^\d+$/.test(val)) {
                    //                        $(o).val("");
                    //                        return;
                    //                    }
                });
            }
        });
        $("#g1 input").keydown(function (e) {
            var keyCode = e.keyCode;
            // 数字
            if (keyCode >= 48 && keyCode <= 57) return true
            // 小数字键盘
            if (keyCode >= 96 && keyCode <= 105) return true
            //不能出现小数点
            //if (keyCode == 190) return false;
            // Backspace键
            if (keyCode == 8 || keyCode == 37 || keyCode == 38 || keyCode == 39 || keyCode == 40) return true
            return false;
        });
        $("#g1 input").change(function (e) {
            if ($("#g1 input").length > 0) {
                if(taxSwitch==1)
                {
                    if ($("#g1 input")[0].name.indexOf("tbMoney") > 0) {
                        var mon = e.currentTarget.value;
                        if (mon < 10000) {
                            $("#g1 input")[1].value = mon;
                            return;
                        }
                        $("#g1 input")[1].value = Round(mon * 0.8, 2);
                    }
                }
                else
                {
                    if ($("#g1 input")[0].name.indexOf("tbMoney") > 0) {
                        var mon = e.currentTarget.value;
                        $("#g1 input")[1].value = mon;
                        return;
                    }
                }
            }
        });
        lockTxt();
    }
    function lockTxt()
    {
        var inpus_sfc= $("#g input[type=text]");
        var inpus_rjc= $("#g1 input[type=text]");
        for (var i = 0; i < inpus_sfc.length; i++) {
            if(inpus_sfc[i].name.indexOf('tbMoneyNoWithTax')>0)
            {
                inpus_sfc[i].readOnly=true;
            }
        }
        for (var i = 0; i < inpus_rjc.length; i++) {
            if(inpus_rjc[i].name.indexOf('tbMoneyNoWithTax')>0)
            {
                inpus_rjc[i].readOnly=true;
            }
        }
    }
</script>
</html>

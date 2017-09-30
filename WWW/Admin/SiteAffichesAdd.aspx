<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SiteAffichesAdd.aspx.cs"
    Inherits="Admin_SiteAffichesAdd" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>站点公告</title>
    
    <link href="../Style/common.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/common.js" type="text/javascript"></script>
    <script src="/Components/kindeditor/kindeditor-min.js" type="text/javascript"></script>
    <script src="/Components/kindeditor/lang/zh_CN.js" type="text/javascript"></script>
    <script src="../Components/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="maincon">
                <h2>站点公告 &gt;&gt; 公告添加</h2>
                <div class="siteaffadd_wrap">
                    <table class="siteafftable" cellspacing="0" cellpadding="0" align="center" border="0">
                        <tr class="siteafftime">
                            <td>时间：<asp:TextBox ID="tbDateTime" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>&nbsp;
                        <asp:CheckBox ID="cbisShow" runat="server" Text="是否显示" Checked="True"></asp:CheckBox>&nbsp;
                        <asp:CheckBox ID="cbisCommend" runat="server" Text="是否推荐" Checked="False" Visible="true"></asp:CheckBox>&nbsp;
                        <asp:CheckBox ID="cbTitleRed" runat="server" Text="标题加红" Checked="False"></asp:CheckBox>
                            </td>
                        </tr>
                        <tr class="siteafftitl">
                            <td>标题：<asp:TextBox ID="tbTitle" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="siteafftabli">
                            <td>
                                <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                    AutoPostBack="True" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                                    <asp:ListItem Value="1" Selected="True">地址型</asp:ListItem>
                                    <asp:ListItem Value="2">内容型</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="trUrl" runat="server">
                            <td>地址：<asp:TextBox ID="tbUrl" runat="server" Height="20px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trUrlOK" runat="server">
                            <td>
                                <%--    <ShoveWebUI:ShoveConfirmButton ID="ShoveConfirmButton1" runat="server" 
                                 Text="增加" AlertText="确信输入无误，并立即增加此公告吗？"
                                OnClick="btnAdd_Click" />--%>


                                <asp:Button ID="ShoveConfirmButton1" runat="server"
                                    Text="保存" AlertText="确信输入无误，并立即增加此公告吗？"
                                    OnClick="btnAdd_Click" />
                            </td>
                        </tr>
                        <tbody id="trContent" runat="server" visible="false">
                            <tr>
                                <td></td>
                            </tr>
                            <script type="text/javascript">
                                var editor;
                                KindEditor.ready(function (K) {
                                    editor = K.create('#tbContent', {
                                        cssPath: '/Components/kindeditor/plugins/code/prettify.css',
                                        uploadJson: '/Components/kindeditor/asp.net/upload_json.ashx',
                                        fileManagerJson: '/Components/kindeditor/asp.net/file_manager_json.ashx',
                                        resizeType: 0,
                                        allowFileManager: true
                                    });
                                });
                            </script>
                            <tr>
                                <td class="inputextbox">
                                    <textarea rows="1" cols="1" runat="server" id="tbContent" name="tbContent"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnAdd" runat="server" Text="保存" AlertText="确信输入无误，并立即增加此公告吗？" OnClientClick="editor.sync();" OnClick="btnAdd_Click" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

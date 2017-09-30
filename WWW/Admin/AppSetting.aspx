<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppSetting.aspx.cs" Inherits="Admin_AppSetting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link type="text/css" href="../Style/Site.css" rel="stylesheet" />
    <style type="text/css">
        th {
            vertical-align: middle;
        }

        .timeSetContent {
            padding: 0 10px;
        }

        .w500 {width:490px}
        .ta {width:490px;height:100px}
    </style>
</head>
<body>
     <form id="form1" runat="server">
        <div style="width:100%;clear:both">
            <div class="title">
                安卓端APP版本设置 
            </div>
            
            <div class="timeSetContent">
                <table cellspacing="0" cellpading="0" style="float:left;">
                    <tbody>
                        <tr>
                            <td width="100px" style="text-align:right;">版本号：</td>
                            <td width="500px" style="text-align:left;"><asp:TextBox ID="androidVersion" runat="server" Text=""  CssClass="input" /></td>
                        </tr>
                        <tr>
                            <td  style="text-align:right;">下载地址：</td>
                            <td  style="text-align:left;"><asp:TextBox ID="androidUrl" runat="server" Text=""  CssClass="input w500" /></td>
                        </tr>
                        <tr>
                            <td style="text-align:right;">是否强制更新：</td>
                            <td style="text-align:left;"><asp:CheckBox ID="androidForce" runat="server" Text="强制更新"/></td>
                        </tr>
                        <tr>
                            <td style="text-align:right;">新版本描述：</td>
                            <td style="text-align:left;"><asp:TextBox ID="androidDes" TextMode="MultiLine" runat="server" Text=""  CssClass="input ta" /></td>
                        </tr>
                        <tr>
                            <td style="text-align:right;">更新样式：</td>
                            <td style="text-align:left;"><asp:RadioButton ID="androidType0" GroupName="androidType" runat="server" Text="应用内更新"  CssClass="input" /> 
                                <asp:RadioButton ID="androidType1" GroupName="androidType" runat="server" Text="打开网页更新"  CssClass="input" /> 
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>

        <div style="width:100%;margin-top:20px;clear:both">
            <div class="title">
                IOS端APP版本设置 
            </div>
            
            <div class="timeSetContent">
                <table cellspacing="0" cellpading="0" style="float:left;">
                    <tbody>
                        <tr>
                            <td width="100px" style="text-align:right;">版本号：</td>
                            <td width="500px" style="text-align:left;"><asp:TextBox ID="IOSVersion" runat="server" Text=""  CssClass="input" /></td>
                        </tr>
                        <tr>
                            <td  style="text-align:right;">下载地址：</td>
                            <td  style="text-align:left;"><asp:TextBox ID="IOSUrl" runat="server" Text=""  CssClass="input w500" /></td>
                        </tr>
                        <tr>
                            <td style="text-align:right;">是否强制更新：</td>
                            <td style="text-align:left;"><asp:CheckBox ID="IOSForce" lotID="78" runat="server" Text="强制更新"/></td>
                        </tr>
                        <tr>
                            <td style="text-align:right;">新版本描述：</td>
                            <td style="text-align:left;"><asp:TextBox ID="IOSDes" TextMode="MultiLine" runat="server" Text=""  CssClass="input ta" /></td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div style="width:100%;margin-top:20px;clear:both">
                <div class="title">
                    其他设置 
                </div>
                <div class="timeSetContent">
                    <table cellspacing="0" cellpading="0" style="float:left;">
                        <tbody>
                            <tr>
                                <td width="100px" style="text-align:right;">APP下载二维码：</td>
                                <td width="500px" style="text-align:left;"><asp:TextBox ID="qrCodeUrl" runat="server" Text=""  CssClass="input w500"  /></td>
                            </tr>
                            <tr>
                                <td width="100px" style="text-align:right;">APP欢迎公告：</td>
                                <td width="500px" style="text-align:left;"><asp:TextBox ID="welcomeText" runat="server" TextMode="MultiLine" Text="" placeholder="打开APP后弹出的欢迎提示框" CssClass="input  w500" /><br />
                            <asp:CheckBox ID="welcomeSwitch" runat="server" Text="开启"  /></td>
                            </tr>
                            <tr>
                            <td class="td4" colspan="2">
                                <asp:Button ID="btn_saveteul" runat="server" Text="保存" CssClass="btn_operate"  OnClientClick="return confirm('确认无误提交吗?')" OnClick="btn_saveteul_Click"/>
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

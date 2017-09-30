<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentSetting.aspx.cs" Inherits="Admin_PaymentSetting" %>

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
                微信支付设置 
            </div>
            
            <div class="timeSetContent">
                <table cellspacing="0" cellpading="0" style="float:left;">
                    <tbody>
                        <tr>
                            <td width="100px" style="text-align:right;">支付名称：</td>
                            <td width="500px" style="text-align:left;"><asp:TextBox ID="wxPayName" runat="server" Text=""  CssClass="input" /></td>
                        </tr>
                        <tr>
                            <td  style="text-align:right;">二维码图片：</td>
                            <td  style="text-align:left;">
                                <asp:Image ID="wxQrcodeImage" runat="server" Width="100" />
                                <asp:FileUpload  name="wxQrcode" id="wxQrcode" runat="server" accept=".png,.jpg,.jpeg,.gif,.bmp" />
                                
                            </td>
                        </tr>
                        <tr>
                            <td  style="text-align:right;">最小限额：</td>
                            <td  style="text-align:left;"><asp:TextBox ID="wxPayMin" runat="server" Text=""  CssClass="input" /> 元</td>
                        </tr>
                        <tr>
                            <td style="text-align:right;">是否启用：</td>
                            <td style="text-align:left;"><asp:CheckBox ID="wxPaySwitch" runat="server" Text="开启"/></td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>

        <div style="width:100%;margin-top:20px;clear:both">
            <div class="title">
               支付宝支付设置 
            </div>
            
            <div class="timeSetContent">
                <table cellspacing="0" cellpading="0" style="float:left;">
                    <tbody>
                        <tr>
                            <td width="100px" style="text-align:right;">支付名称：</td>
                            <td width="500px" style="text-align:left;"><asp:TextBox ID="aliPayName" runat="server" Text=""  CssClass="input" /></td>
                        </tr>
                        <tr>
                            <td  style="text-align:right;">二维码图片：</td>
                            <td  style="text-align:left;">
                                <asp:Image ID="aliQrcodeImage" runat="server" Width="100" />
                                <asp:FileUpload  name="aliQrcode" id="aliQrcode" runat="server" accept=".png,.jpg,.jpeg,.gif,.bmp" />
                            </td>
                        </tr>
                        <tr>
                            <td  style="text-align:right;">最小限额：</td>
                            <td  style="text-align:left;"><asp:TextBox ID="aliPayMin" runat="server" Text=""  CssClass="input" /> 元</td>
                        </tr>
                        <tr>
                            <td style="text-align:right;">是否启用：</td>
                            <td style="text-align:left;"><asp:CheckBox ID="aliPaySwitch" runat="server" Text="开启"/></td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div style="width:100%;margin-top:20px;clear:both">
                <div class="title">
                   网络银行支付设置 
                </div>
                <div class="timeSetContent">
                    <table cellspacing="0" cellpading="0" style="float:left;">
                        <tbody>
                            <tr>
                                <td width="100px" style="text-align:right;">支付名称：</td>
                                <td width="500px" style="text-align:left;"><asp:TextBox ID="bankPayName" runat="server" Text=""  CssClass="input w500"  /></td>
                            </tr>
                            <tr>
                                <td width="100px" style="text-align:right;">开户银行：</td>
                                <td width="500px" style="text-align:left;"><asp:TextBox ID="bankPayBankName" runat="server" Text=""  CssClass="input w500"  /></td>
                            </tr>
                            <tr>
                                <td width="100px" style="text-align:right;">银行卡号：</td>
                                <td width="500px" style="text-align:left;"><asp:TextBox ID="bankPayCard" runat="server" Text=""  CssClass="input w500"  /></td>
                            </tr>
                            <tr>
                                <td width="100px" style="text-align:right;">开户人姓名：</td>
                                <td width="500px" style="text-align:left;"><asp:TextBox ID="bankPayShoukuanren" runat="server" Text=""  CssClass="input w500"  /></td>
                            </tr>
                            <tr>
                                <td  style="text-align:right;">最小限额：</td>
                                <td  style="text-align:left;"><asp:TextBox ID="bankPayMin" runat="server" Text=""  CssClass="input" /> 元</td>
                            </tr>
                            <tr>
                                <td style="text-align:right;">是否启用：</td>
                                <td style="text-align:left;"><asp:CheckBox ID="bankPaySwitch" runat="server" Text="开启"/></td>
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

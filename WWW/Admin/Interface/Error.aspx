﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Admin_Interface_Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>出错啦！</title>
    <link href="../../Style/Surrogate.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        .STYLE1
        {
            color: Red;
            font-size: 15px;
            font-weight: bold;
            padding-left: 20px;
        }
        .STYLE2
        {
            color: #6B6B6B;
            font-size: 13px;
            font-weight: bold;
            padding-left: 20px;
        }
    </style>
    <link rel="shortcut icon" href="../../favicon.ico" />
</head>
<body>
    <form id="form1" runat="server">
    <br />
    <br />
    <table width="508" border="0" cellpadding="0" cellspacing="0" align="center">
        <tr>
            <td style="background-image: url(../../Images/NotExists/Arrow.gif); height: 39px">
                &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td style="background-image: url(../../Images/NotExists/News_success_bg3.gif)">
                <table id="tabError" runat="server" width="508" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="120" align="center" valign="middle">
                            <table border="0" cellspacing="0" cellpadding="0" style="width: 60%">
                                <tr>
                                    <td width="20%">
                                        &nbsp;<img src="../../Images/Cry_036.gif" alt="足球胜负、大乐透" />
                                    </td>
                                    <td width="80%" valign="middle">
                                        <font color="#ff6600"><strong>对不起，访问出错！请重新打开网站！ </strong></font>
                                        <br />
                                        <br />
                                        请参考以下原因后再做此操作：
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" height="34px" style="padding-left: 20px;" colspan="2">
                                        <asp:Label ID="labTip" runat="server" ForeColor="Red"> </asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="30" valign="top" align="center" style="background-image: url(../../Images/NotExists/News_success_bg3.gif)">
                <a href="javascript:history.go(-1);" style="background-color: #FD9A00; cursor: pointer;
                    color: White; width: 120px; border: solid #CCCCCC 1px;">返回上一页</a>
            </td>
        </tr>
        <tr>
            <td style="background-image: url(../../Images/NotExists/news_success_bg2.gif); background-repeat: no-repeat;
                height: 9px">
                &nbsp;&nbsp;
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

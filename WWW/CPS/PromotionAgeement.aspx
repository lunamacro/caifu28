<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PromotionAgeement.aspx.cs"
    Inherits="CPS_PromotionAgeement" %>

<%@ Register Src="userControls/IndexHeader.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="userControls/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-推广联盟协议</title>
    <meta name="description" content="<%=_Site.Name %>-推广联盟协议" />
    <meta name="keywords" content="<%=_Site.Name %>-推广联盟协议" />
    <link type="text/css" href="css/common.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Header ID="Header1" runat="server" />
    <div class="user_banner">
        <h3>
            推广协议<span>Promotion Ageement</span></h3>
    </div>
    <div class="content">
        <div class="inside_bg">
            <div class="insicon">
                <div class="sidebar">
                    <ul>
                        <li class="curr"><a href="javascript:;"><%=_Site.Name %>推广联盟协议</a></li>
                    </ul>
                </div>
                <div class="commain">
                    <div class="contact">
                        <label id="labAgreement" runat="server" />
                        <%--<a onclick="window.opener=null" href="javascript:window.close()" class="aboutbtn" style=" border:0px; background-color:red; color:White; margin:0px 400px;">已阅读并同意以上协议</a>
                       <input type="button" onclick="javascript:window.close()" value="已阅读并同意以上协议" class="aboutbtn" style=" border:0px; background-color:red; color:White; margin:0px 400px;"/>--%>
                    </div>
                </div>
                <div style="text-align: center; padding: 0px 0px 20px 0px;">
                    <a onclick="window.opener=null" href="javascript:window.close()" style="color: Red;">
                        我已了解，关闭此页</a>
                </div>
            </div>
        </div>
    </div>
    <uc2:Footer ID="Footer2" runat="server" />
    </form>
</body>

</html>

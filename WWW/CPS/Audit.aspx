<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Audit.aspx.cs" Inherits="CPS_Audit" %>

<%@ Register Src="userControls/IndexHeader.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="userControls/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-审核信息</title>
    <meta name="description" content="<%=_Site.Name %>-审核信息" />
    <meta name="keywords" content="<%=_Site.Name %>-审核信息" />
    <link type="text/css" href="css/common.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <uc1:Header ID="Header1" runat="server" />
        <!--Header HTML end-->
        <div class="user_banner">
            <h3>审核信息<span>Auditing Information</span></h3>
        </div>
        <div class="content">
            <div class="inside_bg">
                <div class="insicon">
                    <div class="sidebar">
                        <ul>
                            <li class="curr"><a href="###">审核信息</a></li>
                        </ul>
                    </div>
                    <div class="commain">
                        <div class="mainbor">
                            <div class="userinfo_auditinfo">
                                <p>
                                    <asp:Label runat="server" ID="lbl_state"></asp:Label>
                                </p>
                                <p>
                                    如有需要请拨打服务热线：<%=_Site.ServiceTelephone %>
                                </p>
                            </div>
                            <div class="trading_tourist">
                                <h4>
                                    <span>推广指南</span></h4>
                                <ul>
                                    <asp:Repeater runat="server" ID="rpt_newsList">
                                        <ItemTemplate>
                                            <li>
                                                <div class="newstime">
                                                    <%#Eval("DateTime") %>
                                                </div>
                                                <a href='New_View.aspx?ID=<%#Eval("ID") %>&NewType=tgzn' target="_blank">
                                                    <%#Eval("Title") %></a></li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!--Banner HTML end-->
        <uc2:Footer ID="Footer2" runat="server" />
    </form>
</body>

</html>

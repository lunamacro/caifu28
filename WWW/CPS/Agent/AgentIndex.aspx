<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgentIndex.aspx.cs" Inherits="CPS_Agent_AgentIndex" %>

<%@ Register Src="../userControls/AgentHeader.ascx" TagName="AgentHeader" TagPrefix="uc1" %>
<%@ Register Src="../userControls/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<%@ Register Src="../userControls/AgentAccount.ascx" TagName="AgentAccount" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-推广联盟</title>
    <meta name="description" content="<%=_Site.Name %>-推广联盟" />
    <meta name="keywords" content="<%=_Site.Name %>-推广联盟" />
    <link type="text/css" href="../css/common.css" rel="stylesheet" />
    <style type="text/css">
        .column1
        {
            width: 5%;
        }
        .column2
        {
            width: 15%;
        }
        .column3
        {
            width: 15%;
        }
        .column4
        {
            width: 15%;
        }
        .column5
        {
            width: 20%;
             overflow:hidden;text-overflow: ellipsis;white-space: nowrap; 
        }
        .column5 a
        {
            color: Blue;
        }
        .column5 a:hover
        {
            text-decoration: underline;
        }
        .column6
        {
            width: 10%;
        }
        .column7
        {
            width: 10%;
        }
        .column8
        {
            width: 10%;
        }
        .red
        {
            color: Red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:AgentHeader ID="AgentHeader1" runat="server" />
    <div class="user_banner">
        <h3>
            首页<span>Home Page</span></h3>
    </div>
    <div class="content">
        <div class="inside_bg">
            <div class="insicon">
                <div class="sidebar">
                    <ul>
                        <li class="curr"><a href="javascript:;" onclick="location.reload()">首页</a></li>
                    </ul>
                </div>


                <div class="commain">
                    <div class="mainbor">
                        <uc3:AgentAccount ID="AgentAccount1" runat="server" />

                    </div>
                </div>
            </div>
        </div>
    </div>
    <uc2:Footer ID="Footer1" runat="server" />
    </form>
</body>
</html>

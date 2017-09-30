<%@ Page Language="C#" AutoEventWireup="true" CodeFile="New_View.aspx.cs" Inherits="CPS_New_View" %>
<%@ Register src="userControls/IndexHeader.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="userControls/Footer.ascx" tagname="Footer" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title><%=_Site.Name %>-新闻详情</title>
    <meta name="description" content="<%=_Site.Name %>-新闻详情" />
    <meta name="keywords" content="<%=_Site.Name %>-新闻详情" />
    <link type="text/css" href="css/common.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Header ID="Header1" runat="server" />
    <div class="inside_banner">
	    <img src="images/news_banner.jpg" alt="" />
    </div>
    <div class="content">
	    <div class="inside_bg">
		    <div class="insicon">
			    <div class="news_detiled">
				    <h4><asp:Literal runat="server" ID="Title"></asp:Literal></h4>
				    <div class="news_vtime">来源：<span><%=_Site.Name %></span>&nbsp;&nbsp;发布时间：<i><asp:Literal runat="server" ID="DateTime"></asp:Literal></i></div>
				    <div class="condetiled"><asp:Literal runat="server" ID="Content"></asp:Literal></div>
			    </div>
		    </div>
	    </div>
    </div>
    <uc2:Footer ID="Footer2" runat="server" />
    </form>
</body>

</html>

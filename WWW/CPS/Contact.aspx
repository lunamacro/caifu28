<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Contact.aspx.cs" Inherits="CPS_Contact" %>
<%@ Register src="userControls/IndexHeader.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="userControls/Footer.ascx" tagname="Footer" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title><%=_Site.Name %>-联系我们</title>
    <meta name="description" content="<%=_Site.Name %>-联系我们" />
    <meta name="keywords" content="<%=_Site.Name %>-联系我们" />
    <link type="text/css" href="css/common.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Header ID="Header1" runat="server" />
    <div class="inside_banner contact_banner">
	    <h3>联系我们<span>Contact US</span></h3>
    </div>
    <div class="content">
	    <div class="inside_bg">
		    <div class="insicon">
			<div class="sidebar">
				<ul>
					<li class="curr"><a href="javascript:;">联系我们</a></li>
				</ul>
			</div>
			<div class="commain">
				<div class="contact">
                    <label id="lbContent" runat="server" />
				</div>
			</div>
		    </div>
	    </div>
    </div>
    <uc2:Footer ID="Footer2" runat="server" />
    </form>
</body>

</html>

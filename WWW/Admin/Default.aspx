<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>后台管理系统</title>
    
    <script src="../JScript/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="../JScript/common.js" type="text/javascript"></script>
    <script src="../JScript/jquery.mCustomScrollbar.concat.min.js" type="text/javascript"></script>
    <link href="../Style/common.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/jquery.mCustomScrollbar.concat.min.js" type="text/javascript"></script>
</head>
<frameset framespacing="0" rows="58,*,40" frameborder="NO" cols="*">
        <FRAME  name="topFrame" src="FrameTop.aspx" noResize scrolling="no" ></FRAME>
        <FRAMESET border="2" frameSpacing="0" rows="*" frameBorder="NO" cols="175,*">
            <FRAME   name="leftFrame" src="FrameLeft.aspx" noResize scrolling="yes"></FRAME>
            
            <FRAME   name="mainFrame" src="<%=SubPage %>"></FRAME>
            <div style=" position:fixed; top:0px; z-index:999; bottom:0px; width:300px;height:300px; border:1px solid #dfdfdf;"></div>
        </FRAMESET>
        <FRAME id="bottomFrame" name="bottomFrame" src="FrameBottom.aspx" noResize scrolling="no"></FRAME>
    </frameset>
</html>

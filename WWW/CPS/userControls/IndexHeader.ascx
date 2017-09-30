<%@ Control Language="C#" AutoEventWireup="true" CodeFile="IndexHeader.ascx.cs" Inherits="CPS_userControls_IndexHeader" %>
<div class="header">
    <div class="head">
        <h1 class="logo"><a href="Default.aspx">
            <asp:Image ID="img_logo" runat="server" Style="width: 218px; height: 66px;" /></a></h1>
        <div class="slogan"><%--欢迎加入推广联盟--%></div>
        <div class="nav">
            <ul>
                <li><a href="../Default.aspx" runat="server" id="Default">首页</a></li>
                <li><a href="../News.aspx" target="_blank" runat="server" id="News">新闻公告</a></li>
                <li><a href="../Contact.aspx" target="_blank" runat="server" id="Contact">联系我们</a></li>
            </ul>
        </div>
    </div>
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AgentHeader.ascx.cs" Inherits="CPS_userControls_AgentHeader" %>
<div class="header">
    <div class="head">
        <h1 class="logo"><a href="../Default.aspx">
            <asp:Image ID="img_logo" runat="server" Style="width: 218px; height: 66px;" /></a></h1>
        <div class="nav">
            <ul>
                <li><a href="../Agent/AgentIndex.aspx" runat="server" id="AgentIndex">首页</a></li>
                <li><a href="../Agent/AgentPromoteList.aspx" target="_blank" runat="server" id="AgentNumber">我的会员</a></li>
                <li>
                    <input type="button" class="LoginOut" onclick="isLoginout()" value="退出" />
                    <style type="text/css">
                        .LoginOut{ color:#666; border:0px; height:30px; background-color:transparent;font-weight: bold;font-family: Microsoft YaHei;border-style: solid;border-color: #FFF;line-height: 32px;font-size: 15px; cursor:pointer; margin:0px 20px;}
                        .LoginOut:hover{color: #DF1515;}
                    </style>
                </li>
            </ul>
        </div>
    </div>
</div>
<script type="text/javascript">
    var handUrl = "/CPS/ashx/Normal.ashx";
    function isLoginout() {
        var okfunc = function () {
            var successFunc = function (json) {
                if (json && json.url) {
                    window.location.href = json.url;
                }
            };
            f_ajaxPost(handUrl, { "act": "isLoginOut" }, successFunc, null, null);
        };
        var cancelfunc = function () { };
        confirm("确认要退出吗?", okfunc, cancelfunc);
    }
</script>
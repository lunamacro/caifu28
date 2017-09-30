<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdministrationTop.ascx.cs" Inherits="CPS_userControls_AdministrationTop" %>
<link href="../css/common.css" rel="stylesheet" type="text/css" />

<div class="header">
    <div class="head">
        <h1 class="logo"><a href="../Default.aspx">
            <asp:Image ID="img_logo" runat="server" Style="width: 218px; height: 66px;" /></a></h1>
        <div class="nav">
            <ul>
                <li><a href="../admin/AgentAuditing.aspx" target="_blank" runat="server" id="sjAuditing">商家审核</a></li>
                <li><a href="../admin/AgentManagement.aspx" target="_blank" runat="server" id="sjManagement">商家管理</a></li>
                <li><a href="../admin/CommissionManagement.aspx" target="_blank" runat="server" id="yjManagement">佣金管理</a></li>
                <li><a href="../admin/SystemParamSet.aspx" target="_blank" runat="server" id="xtSet">系统设置</a></li>
                <li>
                    <input type="button" class="LoginOut" onclick="isLoginout()" value="退出" />
                    <style type="text/css">
                        .LoginOut {
                            color: #666;
                            border: 0px;
                            height: 30px;
                            background-color: transparent;
                            font-weight: bold;
                            font-family: Microsoft YaHei;
                            border-style: solid;
                            border-color: #FFF;
                            line-height: 32px;
                            font-size: 15px;
                            cursor: pointer;
                            margin: 0px 20px;
                        }

                            .LoginOut:hover {
                                color: #DF1515;
                            }
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

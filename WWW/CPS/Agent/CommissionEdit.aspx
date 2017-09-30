<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommissionEdit.aspx.cs" Inherits="CPS_Agent_CommissionEdit" %>

<%@ Register Src="../userControls/AgentHeader.ascx" TagName="AgentHeader" TagPrefix="uc1" %>
<%@ Register Src="../userControls/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-推广联盟-佣金设置</title>
    <meta name="description" content="<%=_Site.Name %>-推广联盟-佣金设置" />
    <meta name="keywords" content="<%=_Site.Name %>-推广联盟-佣金设置" />
    <link type="text/css" rel="stylesheet" href="../css/common.css" />
    <style type="text/css">
        .column1 {
            width: 10%;
        }

        .column2 {
            width: 15%;
        }

        .column3 {
            width: 15%;
        }

        .column4 {
            width: 25%;
        }

            .column4 input, .column5 input {
                text-align: center;
                height: 28px;
                border: 1px solid #80B3E7;
            }

        .column5 {
            width: 15%;
        }

        .column6 {
            width: 20%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:AgentHeader ID="AgentHeader1" runat="server" />
        <div class="user_banner">
            <h3>我的佣金<span>My Commission</span></h3>
        </div>
        <div class="content">
            <div class="inside_bg">
                <div class="insicon">
                    <div class="sidebar">
                        <ul class="fourli">
                            <li><a href="AgentCommission.aspx">佣金明细</a></li>
                            <li class="curr"><a href="AgentCommissionSet.aspx">佣金设置</a></li>
                        </ul>
                    </div>
                    <div class="commain">
                        <div class="mainbor">
                            <div class="admininfo">
                                <div class="admininfo">
                                    <div id="user_list" class="number_record">
                                        <h4>
                                            <span style="width: 250px;">
                                                <%=Shove._Web.Utility.GetRequest("UserName") %>-佣金设置</span></h4>

                                        <dl>
                                            <dt>
                                                <div class=" column1">
                                                    序号
                                                </div>
                                                <div class="column2">
                                                    彩种名称
                                                </div>
                                                <div class="column3">
                                                    我的佣金比例
                                                </div>
                                                <div class="column4">
                                                    推广员佣金比例
                                                </div>
                                                <div class="column5">
                                                    修改为
                                                </div>
                                                <div class="column6">
                                                    保存
                                                </div>
                                            </dt>
                                            <asp:Repeater ID="rpt_dataList" runat="server">
                                                <ItemTemplate>
                                                    <dd>
                                                        <div class="column1">
                                                            <asp:HiddenField runat="server" ID="hide_LotteryID" Value='<%#Eval("LotteryID")%>' />
                                                            <%#Container.ItemIndex + 1 %>
                                                        </div>
                                                        <div class="column2">
                                                            <asp:HiddenField runat="server" ID="hide_LotteryName" Value='<%#Eval("LotteryName")%>' />
                                                            <%#Eval("LotteryName")%>
                                                        </div>
                                                        <div class="column3">
                                                            <%#Eval("ParentBonusScale")%>
                                                        </div>
                                                        <div class="column4">
                                                            <%#Eval("BonusScale") %>
                                                        </div>
                                                        <div class="column5">
                                                            <asp:TextBox runat="server" ID="txtAlter" Text='0.02' MaxLength="5" class="bonusScale border_shadow" Style="margin: 5px;"></asp:TextBox>
                                                        </div>
                                                        <div class="column6">
                                                            <input type="button" id="btnSave" class="btnOperate" value="保存" onclick="checkInput(this,<%#Eval("LotteryID")%>,'<%#Eval("LotteryName")%>    ',<%#Container.ItemIndex %>)" />
                                                        </div>
                                                    </dd>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </dl>
                                        <div class="unified_set">
                                            所有彩种统一修改为：
                                            <input type="text" value="0.02" id="txtBonus" maxlength="5" class="bonusScale" />
                                            <input type="button" value="确定" id="btnAlter" class="btnAction" style="display: inline-block;" />
                                            <span>建议输入0.01-0.1之间的数值。</span>
                                        </div>
                                        <div class="unified_set" style="text-align: center;">
                                            <%--<asp:Button runat="server" ID="btnEditOver" Text="修改完成" CssClass="btnAction" OnClick="btnEditOver_Click"
                                                OnClientClick="return confirm('确认无误提交修改吗?')" />--%>
                                            <input type="button" class="btnAction" id="btnEditOver" value="修改完成" />
                                            <%--<asp:Button runat="server" ID="btnReturnDefault" Text="恢复默认值" CssClass="btnAction"
                                                OnClick="btnReturnDefault_Click" OnClientClick="return confirm('确认要恢复系统默认吗?')" />--%>
                                            <input type="button" class="btnAction" id="btnReturnDefault" value="恢复默认值" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <uc2:Footer ID="Footer1" runat="server" />
        <input type="hidden" runat="server" id="hide_userType" value="" />
    </form>
</body>
<script type="text/javascript">
    $(function () {
        $("#btnAlter").bind("click", function () {
            var bonus = parseFloat($("#txtBonus").val());
            if (bonus >= 1) {
                alert("佣金比例不能大于等于1");
                bonus = 0.02;
                $("#user_list2 input:text").val(bonus);
                return;
            }
            $("#user_list input:text").val(bonus);
            alert("修改完成,请点击修改完成来完成操作");
        });
        $("#btnEditOver").click(function(){
            var okfunc=function(){
                var handUrl="/CPS/ashx/Normal.ashx";
                var successfunc=function(json){
                    if(json)
                    {
                        alert(json.result);
                        if(json.url)
                        {
                            setTimeout(function(){window.location.href=json.url;},2000);
                        }
                    }
                };
                var jsonStr="";
                for (var i = 0; i < $("#user_list input[type=hidden]").length/2; i++) {
                    jsonStr+= "{\"LotteryID\":"+$("#user_list input[type=hidden]").eq(2*i).val()+",";
                    jsonStr+= "\"LotteryName\":\""+$("#user_list input[type=hidden]").eq(2*i+1).val()+"\",";
                    jsonStr+= "\"CommissionRate\":"+ $("#user_list input[type=text]").eq(i).val()+"},";
                }
                jsonStr="["+jsonStr.substr(0,jsonStr.length-1)+"]";
                f_ajaxPost(handUrl,{"act":"SaveCommissionRateAllUser","CpsID":"<%=Request.QueryString["CpsID"]%>","url":"<%=Request.Url%>","jsonStr":jsonStr,"isReset":"0","userType":"<%=Request.QueryString["UserType"]%>"},successfunc,null,null);
            };
            var cancelfunc=function(){};
            confirm("确认无误，提交修改吗？",okfunc,cancelfunc);
        });
        $("#btnReturnDefault").click(function(){
            var okfunc=function(){
                var handUrl="/CPS/ashx/Normal.ashx";
                var successfunc=function(json){
                    if(json)
                    {
                        alert(json.result);
                        if(json.url)
                        {
                            setTimeout(function(){window.location.href=json.url;},2000);
                        }
                    }
                };
                var jsonStr="";
                for (var i = 0; i < $("#user_list input[type=hidden]").length/2; i++) {
                    jsonStr+= "{\"LotteryID\":"+$("#user_list input[type=hidden]").eq(2*i).val()+",";
                    jsonStr+= "\"LotteryName\":\""+$("#user_list input[type=hidden]").eq(2*i+1).val()+"\",";
                    jsonStr+= "\"CommissionRate\":"+ $("#user_list input[type=text]").eq(i).val()+"},";
                }
                jsonStr="["+jsonStr.substr(0,jsonStr.length-1)+"]";
                f_ajaxPost(handUrl,{"act":"SaveCommissionRateAllUser","CpsID":"<%=Request.QueryString["CpsID"]%>","url":"<%=Request.Url%>","jsonStr":jsonStr,"isReset":"1","userType":"<%=Request.QueryString["UserType"]%>"},successfunc,null,null);
            };
            var cancelfunc=function(){};
            confirm("确认要恢复系统默认吗？",okfunc,cancelfunc);
        });
    });
    function checkInput(obj, lotteryId, lotteryName,index) {
        var prevList = $(obj).parent().prevAll();
        var bonusScale = $(prevList[1]).find("input").val();
        var userType = $("#hide_userType").val();
        switch (userType) {
            case "2":
                if ("" == FlyFish.Trim(bonusScale) || parseFloat(bonusScale) <= 0) {
                    alert("请输入正确的推广员佣金比例");
                    return false;
                }
                break;
            case "1":
                if ("" == FlyFish.Trim(bonusScale) || parseFloat(bonusScale) <= 0) {
                    alert("请输入正确的代理商佣金比例");
                    return false;
                }
                break;
        }
        var okfunc = function () {
            var handUrl = "/CPS/ashx/Normal.ashx";
            var successfunc = function (json) {
                if (json) {
                    alert(json.result);
                    if (json.url) {
                        setTimeout(function () { window.location.href = json.url; }, 2000);
                    }
                }
            };
            f_ajaxPost(handUrl, { "act": "SaveCommissionRateSingleUser", "lotteryID": lotteryId, "LotteryName": lotteryName, "commission": $(":text").eq(index).val(), "CpsID": "<%=Request.QueryString["CpsID"]%>", "url": "<%=Request.Url%>" }, successfunc, null, null);
        };
        var cancelfunc = function () { };
        confirm("确认无误提交吗?", okfunc, cancelfunc);
    }
    window.onload = function () {
        var agentDefault = 0.05;
        var promoteDefault = 0.02;
        var tag = "promote";
        var list = $("input:text");
        list.bind("focus", function () {
            $(this).select();
        });
        list.bind("blur", function () {
            var val = $(this).val();

            var temp = agentDefault;

            switch (tag) {
                case "agent":
                    temp = agentDefault;
                    break;
                case "promote":
                    temp = promoteDefault;
                    break;
            }

            if (!FlyFish.IsFloat(val)) {
                $(this).val(temp);
            }
            if (parseFloat(val) >= 1) {
                $(this).val(temp);
            }
        });
        list.bind("keydown", function (e) {
            var keyCode = e.keyCode;
            // 数字
            if (keyCode >= 48 && keyCode <= 57) return true
            // 小数字键盘
            if (keyCode >= 96 && keyCode <= 105) return true
            // Backspace键
            if (keyCode == 8 || keyCode == 37 || keyCode == 38 || keyCode == 39 || keyCode == 40 || keyCode == 190) return true
            return false;
        });
    }
</script>
</html>

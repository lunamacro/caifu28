<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Handsel.aspx.cs" Inherits="Admin_Handsel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>彩票业务中心-充值送彩金设置</title>
    <link href="../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../Style/main.css" type="text/css" rel="stylesheet" />
    <link href="../Components/My97DatePicker/skin/WdatePicker.css" rel="stylesheet" type="text/css" />
    <link href="../../Style/sandPage.css" rel="stylesheet" type="text/css" />
    <link href="../JScript/artDialog/css/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../JScript/showloading/showLoading.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        #btn {
            display: block;
            height: 32px;
            width: 82px;
            background: url(../images/Sprite.png) no-repeat -105px -320px;
            text-indent: -9999px;
        }

        #del {
            top: 5px;
            display: block;
            float: left;
            color: #3977c3;
            font-family: "微软雅黑";
            width: 54px;
            margin: 10px;
            height: 25px;
            line-height: 22px;
            text-align: center;
            background-image: url(../images/Sprite.png) no-repeat -208px -318px;
        }

            #del:hove {
                background: #D6F7FE;
            }


        .send {
            text-align: right;
            padding-right: 30px;
        }

        .toptable {
        }

        #tab th {
            width: 150px;
        }

        #showMsg table {
            width: 100%;
            margin: 0px auto;
            color: #000;
            text-align: center;
            border-collapse: collapse;
        }

            #showMsg table th {
                height: 30px;
            }

            #showMsg table td {
                border: 1px solid #ccc;
                width: 100px;
                height: 30px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="maincon">
                <h2>充值送彩金设置</h2>
                <div class="main_list">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                        <tr>
                            <td>
                                <table id="Table1" cellspacing="0" cellpadding="0" width="90%" border="0">
                                    <tr>
                                        <td style="height: 40px; margin-left: 10px;">活动时间：<input type="text" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd' })" class="Wdate"
                                            id="txtStartTime" runat="server" />
                                            -
                                        <input type="text" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd' })" class="Wdate" id="txtEndTime"
                                            runat="server" />&nbsp; 赠送对象：<select id="selGiveObj" runat="server">
                                                <option value="-1">所有</option>
                                                <option value="0">新用户</option>
                                                <option value="1">老用户</option>
                                            </select>
                                            &nbsp;赠送方式
                                        <select id="selGiveType" runat="server">
                                            <option value="-1">所有</option>
                                            <option value="0">定额</option>
                                            <option value="1">比例</option>
                                        </select>
                                            <ShoveWebUI:ShoveConfirmButton ID="ShoveConfirmButton" runat="server" Text="搜索" OnClick="ShoveConfirmButton_Click"
                                                CssClass="blueButtonClass" />
                                            <input type="button" id="buttonAdd" value="增加活动" class="blueButtonClass" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <table id="tab" class="newsTable" cellpadding="0" cellspacing="0" width="100%">
                                    <thead>
                                        <tr>
                                            <th>开始时间
                                            </th>
                                            <th>结束时间
                                            </th>
                                            <th>赠送对象
                                            </th>
                                            <th>赠送方式
                                            </th>
                                            <th>操作
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptSchemes" runat="server" EnableViewState="false" OnItemCommand="rptSchemes_ItemCommand">
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <%#Convert.ToDateTime(Eval("StartTime")).ToString("yyyy-MM-dd") %>
                                                    </td>
                                                    <td>
                                                        <%#Convert.ToDateTime(Eval("EndTime")).ToString("yyyy-MM-dd") %>
                                                    </td>
                                                    <td>
                                                        <%#JudgeObjectType(Eval("GiveObject").ToString())%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("GiveType").ToString()=="0"?"定额":"比例" %>
                                                    </td>
                                                    <td>
                                                        <a href="#" id="show_<%#Eval("ID") %>" onclick="getQuota(<%#Eval("ID") %>,<%#Eval("GiveType")%>,'<%# Convert.ToDateTime(Eval("StartTime")).ToString("yyyy-MM-dd") %>','<%#Convert.ToDateTime(Eval("EndTime")).ToString("yyyy-MM-dd") %>','<%#Eval("GiveObject") %>')">查看</a> <a href="#" onclick="updateRules(<%#Eval("ID") %>)">修改</a> <a href="#" onclick="deleteRules(<%#Eval("ID") %>)">删除</a>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                                <div id="sand" class="tobbutton" style="text-align: right">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#D8D8D8"
                                    style="margin-top: 10px;">
                                    <tr>
                                        <td colspan="2" bgcolor="#F8F8F8" class="black12">&nbsp;注：若充值金额大于最高额度，按照设定的最高金额赠送彩金！
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <input type="hidden" value="<%=PageIndex%>" id="PageIndex" name="PageIndex" />
        <input type="hidden" value="<%=PageCount%>" id="PageCount" name="PageCount" />
        <input type="hidden" value="<%=DataCount%>" id="DataCount" name="DataCount" />
        <!--dialog对话框-->
        <div id="divFudu" style="display: none;">
            <div id="showMsg">
            </div>
        </div>
        <!--dialog对话框-->
    </form>
</body>
</html>
<script src="../JScript/artDialog/jquery-1.10.2.js" type="text/javascript"></script>
<link href="../Style/common.css" rel="stylesheet" type="text/css" />
<script src="../JScript/common.js" type="text/javascript"></script>
<link href="../Style/jquery.mCustomScrollbar.css" rel="stylesheet" type="text/css" />
<script src="../JScript/jquery.mCustomScrollbar.concat.min.js" type="text/javascript"></script>
<script src="../JScript/sandPage.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript" src="../Components/My97DatePicker/WdatePicker.js"></script>
<script src="../JScript/artDialog/dialog-min.js" type="text/javascript"></script>
<script src="../JScript/artDialog/dialog-plus-min.js" type="text/javascript"></script>
<script src="../JScript/showloading/jquery.showLoading.js" type="text/javascript"></script>
<script type="text/javascript">
    var handlerUrl = "Handler/Handsel.ashx";
    function thisSubmit(n) {

        $("#PageIndex").val(n);
        $("#form1").submit();


    }

    $(function () {
        var totalPage = $("#PageCount").val();
        var totalRecords = $("#DataCount").val();
        var pageNo = $("#PageIndex").val();
        //初始化分页控件
        //有些参数是可选的，比如lang，若不传有默认值
        sand.init({
            pno: pageNo,
            //总页码
            total: totalPage,
            //总数据条数
            totalRecords: totalRecords,
            //链接前部
            //                hrefFormer: 'schemeall',
            //                //链接尾部
            //                hrefLatter: '.aspx',
            getLink: function (n) {

                return 'javascript:thisSubmit(' + n + ')';
            },
            lang: {
                prePageText: '上一页',
                nextPageText: '下一页',
                totalPageBeforeText: '共',
                totalPageAfterText: '页',
                totalRecordsAfterText: '条数据',
                gopageBeforeText: '转到',
                gopageButtonOkText: '确定',
                gopageAfterText: '页',
                buttonTipBeforeText: '第',
                buttonTipAfterText: '页'
            }
        });
        //生成
        sand.generPageHtml();
        $("#buttonAdd").click(function () {
            window.location.href = "HandselAddOrEdit.aspx";
        });
    });

    window.onload = function () {
        SetTableRowColor();
    }

    function showSection(id) {
        var d = dialog({
            title: '区间范围',
            content: $("#divFudu").html(),
            width: 500
        });
        d.showModal();
    }

    function showtable() {
        var mainTable = document.getElementById("tab");
        var li = mainTable.getElementsByTagName("tr");
        for (var i = 1; i <= li.length - 1; i++) {
            li[i].style.backgroundColor = "transparent";
            li[i].onmouseover = function () {
                this.style.backgroundColor = "#fefdde";
            }
            li[i].onmouseout = function () {

                this.style.backgroundColor = "transparent";
                SetTableRowColor();
            }
        }
    }


    showtable();

    function SetTableRowColor() {

        $("#tab tr:odd").css("background-color", "#F3F8FE");

        $("#tab tr:even").css("background-color", "#F7F7F7");

    }
    //查看配置信息
    function getQuota(id, giveType, startTime, endTime, giveObj) {
        var giveName = "";
        if (giveType == 0) {
            giveName = "定额";
        }
        else {
            giveName = "比例";
        }
        var successFunc = function (json) {
            json = json.handselsection;
            if (giveObj == 0) {
                giveObj = "新用户";
            }
            else if (giveObj == 1) {
                giveObj = "老用户";
            }
            else {
                giveObj = "所有用户";
            }
            var headHtml = "<div style='margin-bottom:10px;'>开始时间:<b>" + startTime + "</b> 结束时间:<b>" + endTime + "</b> 赠送对象:<b>" + giveObj + "</b></div>";
            if (json.length > 0) {
                var tempStr = "<table style='border:1px #ccc solid; width:100%;'><tr><th>最低限额</th><th>最高限额</th><th>" + giveName + "</th></tr>";
                for (var i = 0; i < json.length; i++) {
                    tempStr += "<tr><td>" + parseFloat(json[i].ConditionLowest).toFixed(2) + "</td><td>" + parseFloat(json[i].ConditionHighest).toFixed(2) + "</td><td>" + parseFloat(json[i].Numerical).toFixed(2) + "</td></tr>";
                }
                tempStr += "</table>";
                $("#showMsg").html(headHtml + tempStr);
            }
            else {
                $("#showMsg").html("<div style='text-align:center;font-weight:bold;'>暂无配置信息</div>");
            }
            showSection(id);
        };
        f_ajaxPost(handlerUrl, { "act": "GetQuota", "handselRuleID": id }, "showMsg", successFunc, null, null);
    }
    //删除活动规则
    function deleteRules(id) {
        var d = dialog({
            title: '提示',
            width: 300,
            content: '确认要删除此活动吗？',
            ok: function () {
                var successFunc = function (json) {
                    if (json && json.IsOk == true) {
                        alert("删除成功");
                        $("#ShoveConfirmButton").click();
                    }
                };
                f_ajaxPost(handlerUrl, { "act": "DeleteRules", "HandselRuleID": id }, "form1", successFunc, null, null);
            },
            okValue: '确定',
            cancel: function () { },
            cancelValue: '取消'
        });
        d.showModal();
    }
    function updateRules(id) {
        window.location.href = "HandselAddOrEdit.aspx?type=update&id=" + id;
    }
</script>

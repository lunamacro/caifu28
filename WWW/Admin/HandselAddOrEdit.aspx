<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HandselAddOrEdit.aspx.cs"
    Inherits="Admin_HandselAddOrEdit" %>

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
                                        <td style="height: 40px; margin-left: 10px;">&nbsp;活动开始时间：<input type="text" onfocus="var txtEndTime=$dp.$('txtEndTime');WdatePicker({onpicked:function(){txtEndTime.focus();},maxDate:'#F{$dp.$D(\'txtEndTime\')}'})"
                                            class="Wdate" id="txtStartTime" />
                                            &nbsp;活动结束时间：<input type="text" onfocus="WdatePicker({minDate:'#F{$dp.$D(\'txtStartTime\')}'})"
                                                class="Wdate" id="txtEndTime" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 40px; margin-left: 10px;">请选择赠送对象：<label><input type="radio" name="robject" checked value="0" />新用户</label>
                                            <label>
                                                <input type="radio" name="robject" value="1" />老用户</label>
                                            <label>
                                                <input type="radio" name="robject" value="2" />所有用户</label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 40px; margin-left: 10px;">请选择赠送方式：<label><input type="radio" name="rtypes" checked value="0" id="lblDingE" />定额</label>
                                            <label>
                                                <input type="radio" name="rtypes" value="1" id="lblDingB" />比例</label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 40px; margin-left: 10px;" id="addTd">
                                            <div class="HandselSection">
                                                请输入赠送<span class="spAmount">金额</span>：<input type="text" class="txtAmount" onkeyup="if(isNaN(value))execCommand('undo')"
                                                    onafterpaste="if(isNaN(value))execCommand('undo')" />&nbsp;请输入充值最低限额：<input type="text"
                                                        class="txtLowest" onkeyup="if(isNaN(value))execCommand('undo')" onafterpaste="if(isNaN(value))execCommand('undo')" />&nbsp;请输入充值最高限额：<input
                                                            type="text" class="txtHighest" onkeyup="if(isNaN(value))execCommand('undo')"
                                                            onafterpaste="if(isNaN(value))execCommand('undo')" />&nbsp;<input type="button" value="+"
                                                                style="width: 20px;" title="添加" class="btnAddHtml" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="width: 50%; text-align: center; margin-top: 10px;">
                                    <input type="button" name="btnSearch" value="增加" id="btnAdd" class="blueButtonClass">
                                    &nbsp;&nbsp;
                                <input type="button" name="btnSearch" value="返回" id="btnBack" class="blueButtonClass">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#D8D8D8"
                                    style="margin-top: 10px;">
                                    <tr>
                                        <td colspan="2" bgcolor="#F8F8F8" class="black12">&nbsp;赠送方式定额：比如设置为5元，表示无论充值多少钱，都赠送5元。<br />
                                            &nbsp;赠送方式比例：0.1-1 如：比如设置为0.01， 表示充值100元赠送彩金1元<br />
                                            &nbsp;最底限额：比如设置为50元，表示最少充值50元才能参与该活动。<br />
                                            &nbsp;最高限额：比如最低限额设置为50元，最高限额设置设置为100元，表示充值50至100元才能参与该活动。 
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
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
    var request = GetRequest();
    var num = 1;
    $(function () {
        FillRules();
        $(".btnAddHtml").click(function () {
            addControl();
        });
        function addControl() {
            $(".HandselSection").eq(0).after("<div class='HandselSection' id=\"HandselSection_" + num + "\" style='margin-top:5px;'>请输入赠送<span class='spAmount'>金额</span>：<input type=\"text\" class=\"txtAmount\" onkeyup=\"if(isNaN(value))execCommand('undo')\" onafterpaste=\"if(isNaN(value))execCommand('undo')\"/>&nbsp;请输入充值最低限额：<input type=\"text\" class=\"txtLowest\"  onkeyup=\"if(isNaN(value))execCommand('undo')\" onafterpaste=\"if(isNaN(value))execCommand('undo')\"/>&nbsp;请输入充值最高限额：<input type=\"text\" class=\"txtHighest\"  onkeyup=\"if(isNaN(value))execCommand('undo')\" onafterpaste=\"if(isNaN(value))execCommand('undo')\"/>&nbsp;<input type=\"button\" value=\"-\" style=\"width: 20px;\" title=\"删除\" class=\"btnRemoveHtml\" onclick='$(\"#HandselSection_" + num + "\").remove()'/></div>");
            num++;
            $(".txtAmount").blur(function () {
                if ($("#lblDingB")[0].checked) {
                    if (this.value > 1 || this.value < 0) {
                        this.value = "0.1";
                        alert("当赠送方式为比例时，赠送比例的数值应在0至1之间！");
                    }
                }
            });
            if ($("#lblDingB")[0].checked) {
                $(".spAmount").html("比例");
            }
            else {
                $(".spAmount").html("金额");
            }
        }
        $("#btnAdd").click(function () {
            //验证
            //验证是否为空
            var tempControls = $("input[type=text]");
            for (var i = 0; i < tempControls.length; i++) {
                if (tempControls[i].value == "") {
                    alert("请填写完整的信息再提交");
                    return false;
                }
            }
            var sectionList = "";
            for (var i = 0; i < $(".HandselSection input[type=text]").length; i++) {
                if (i % 3 == 0) {
                    sectionList += "{\"Numerical\":" + $(".HandselSection input[type=text]")[i].value + ",";
                }
                else if (i % 3 == 1) {
                    sectionList += "\"ConditionLowest\":" + $(".HandselSection input[type=text]")[i].value + ",";
                }
                else if (i % 3 == 2) {
                    sectionList += "\"ConditionHighest\":" + $(".HandselSection input[type=text]")[i].value + "},";
                }
            }
            sectionList = sectionList.substring(0, sectionList.length - 1);
            sectionList = "[" + sectionList + "]";
            var postData = [];
            postData.push({ name: "act", value: "AddActivity" });
            postData.push({ name: "startTime", value: $("#txtStartTime").val() });
            postData.push({ name: "endTime", value: $("#txtEndTime").val() });
            postData.push({ name: "objType", value: $("input[name='robject']:checked").val() });
            postData.push({ name: "giveType", value: $("input[name='rtypes']:checked").val() });
            postData.push({ name: "sectionList", value: sectionList });
            if (request["type"] == "update") {
                postData.push({ name: "changeType", value: "update" });
                postData.push({ name: "handselRuleID", value: request["id"] });
            }

            var successFunc = function (json) {
                if (json && json.IsOk) {
                    alert(json.Msg);
                    window.location.href = "Handsel.aspx";
                }
                else {
                    alert(json.Msg);
                }
            };
            f_ajaxPost(handlerUrl, postData, "form1", successFunc, null, null);
        });

        $("#btnBack").click(function () {
            window.location.href = "Handsel.aspx";
        });
        $("#lblDingB").click(function () {
            $(".spAmount").html("比例");
            for (var i = 0; i < $(".txtAmount").length; i++) {
                if (parseFloat($(".txtAmount")[i].value) > 1 || parseFloat($(".txtAmount")[i].value) < 0) {
                    $(".txtAmount")[i].value = "0.1";
                }
            }
        });
        $("#lblDingE").click(function () {
            $(".spAmount").html("金额");
        });
        $(".txtAmount").blur(function () {
            if ($("#lblDingB")[0].checked) {
                if (this.value > 1 || this.value < 0) {
                    this.value = "0.1";
                    alert("当赠送方式为比例时，赠送比例的数值应在0至1之间！");
                }
            }
        });
    });
    function FillRules() {
        var id = request["id"];
        var type = request["type"];
        if (type == "update") {
            $("#btnAdd").val("保存");
            var successFunc = function (json) {
                if (json) {
                    json = json[0];
                    $("#txtStartTime").val(jsonDateFormat(json.StartTime, 1));
                    $("#txtEndTime").val(jsonDateFormat(json.EndTime, 1));
                    $("input[name='robject'][value=" + json.GiveObject + "]").attr("checked", true);
                    $("input[name='rtypes'][value=" + json.GiveType + "]").attr("checked", true);
                    FillSection();
                }
            };
            f_ajaxPost(handlerUrl, { "act": "GetHandselRule", "handselRuleId": id }, "form1", successFunc, null, null);
        }
    }
    function FillSection() {
        var id = request["id"];
        var type = request["type"];
        if (type == "update") {
            var successFunc = function (json) {
                if (json) {
                    for (var i = 0; i < json.length - 1; i++) {
                        $(".btnAddHtml").click();
                    }
                    var num = 0;
                    for (var i = 0; i < json.length; i++) {
                        $(".HandselSection input[type=text]")[num].value = json[i].Numerical.toFixed(2);
                        $(".HandselSection input[type=text]")[num + 1].value = json[i].ConditionLowest;
                        $(".HandselSection input[type=text]")[num + 2].value = json[i].ConditionHighest;
                        num = num + 3;
                    }
                    if ($("#lblDingB")[0].checked) {
                        $(".spAmount").html("比例");
                    }
                    else {
                        $(".spAmount").html("定额");
                    }
                }
            };
            f_ajaxPost(handlerUrl, { "act": "GetHandselSection", "handselRuleId": id }, "form1", successFunc, null, null);
        }
    }
</script>

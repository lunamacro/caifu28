<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserDetail.aspx.cs" Inherits="Admin_UserDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户管理-用户基本资料</title>
    
    <link type="text/css" href="../Style/Site.css" rel="stylesheet" />
    <link href="../Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hfBankInProvince" runat="server" Value="true" />
        <asp:HiddenField ID="hfBankInCity" runat="server" Value="true" />
        <asp:HiddenField ID="hfBankTypeName" runat="server" Value="true" />
        <asp:HiddenField ID="hfBankName" runat="server" Value="true" />
        <div class="container">
            <div class="title">
                用户基本资料
            </div>
            <div class="optionsContent">
                <table>
                    <tr>
                        <td class="td1">超级权限设置：
                        </td>
                        <td class="td2">
                            <asp:RadioButton ID="adminsetting1" runat="server" Text="普通会员" GroupName="admin"></asp:RadioButton>
                            <asp:RadioButton ID="adminsetting2" runat="server" Text="网站管理员" GroupName="admin"></asp:RadioButton>
                        </td>
                        <td>
                            <font color="#ff0000">(请谨慎选择)</font>
                        </td>
                    </tr>

                    <tr>
                        <td class="td1">用户名：
                        </td>
                        <td class="td2">
                            <asp:TextBox ID="tbUserName" Enabled="true" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;&nbsp;
                        <asp:LinkButton ID="btnUserAccount" runat="server" CssClass="li3" OnClick="btnUserAccount_Click">查看用户账户明细</asp:LinkButton>
                            <asp:TextBox ID="tbUserID" Style="z-index: 107; left: 128px; position: absolute; top: 13px"
                                runat="server" Visible="False"></asp:TextBox>
                            <asp:TextBox ID="tbSiteID" runat="server" Style="z-index: 107; left: 266px; position: absolute; top: 14px"
                                Visible="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="td1">昵称：
                        </td>
                        <td class="td2">
                            <asp:TextBox ID="tbUserRealityName" CssClass="input" runat="server" MaxLength="25"></asp:TextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="td1">登录密码：
                        </td>
                        <td class="td2">
                            <asp:TextBox ID="tbUserPassword" CssClass="input" runat="server" MaxLength="25"></asp:TextBox>
                        </td>
                        <td>&nbsp;&nbsp;不更改密码请留空</td>
                    </tr>
                    <tr>
                        <td class="td1">支付密码：
                        </td>
                        <td class="td2">
                            <asp:TextBox ID="tbUserPasswordAdv" CssClass="input" runat="server" MaxLength="25"></asp:TextBox>
                        </td>
                        <td>&nbsp;&nbsp;不更改支付密码请留空</td>
                    </tr>
                    <tr class="line">
                        <td colspan="3">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">1、代理权限设置：
                        </td>
                    </tr>
                   
                    <tr>
                        <td class="td1">代理身份：
                        </td>
                        <td class="td2">
                            <asp:RadioButton ID="dlrb1" runat="server" Text="普通会员" GroupName="agent"></asp:RadioButton>
                            <asp:RadioButton ID="dlrb2" runat="server" Text="代理会员" GroupName="agent"></asp:RadioButton>
                            <asp:RadioButton ID="dlrb3" runat="server" Text="代理组管理" GroupName="agent"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="td1">代理组：
                        </td>
                        <td class="td2">
                            <select id="selectGroup" class="input" name="selectGroup" runat="server"></select>
                            <select id="selectRefer" class="input" name="selectRefer" runat="server"></select><br />(仅在设置为普通会员时有效)

                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="td1">推广链接：
                        </td>
                        <td class="td2" colspan="2">
                             <% =siteUrl %>AppGateWay/?pid=<%=uid%>

                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="td1">二维码：
                        </td>
                        <td class="td2">
                            <div id="qrcode"></div>
                            <img id="img-buffer" src="/Images/logo_s.png" style="display:none">
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td colspan="4" align="center" bgcolor="#ececec">二维码编辑</td>
                                </tr>
                                <tr>
                                    <td width="20%" align="right">链接地址：</td>
                                    <td width="30%" align="left"><textarea id="text" class="input" style="width:100%"><% =siteUrl %>AppGateWay/?pid=<%=uid%></textarea></td>
                                    <td width="20%" align="right">二维码样式：</td>
                                    <td width="30%" align="left"><select id="mode" class="input" style="width: 120px;">
                                            <option value="0">默认</option>
                                            <option value="2">文本-Box</option>
                                            <option value="4" selected="selected">图片-Box</option>
                                        </select></td>
                                </tr>
                                <tr>
                                    <td width="20%" align="right">LOGO地址：</td>
                                    <td width="30%" align="left"><input id="imgurl" type="text" value="/Images/logo_s.png" class="input" /></td>
                                    <td width="20%" align="right">文字内容：</td>
                                    <td width="30%" align="left"> <input id="label" type="text" value="财富28" class="input" /></td>
                                </tr>
                                <tr>
                                    <td width="20%" align="right">文本大小：</td>
                                    <td width="30%" align="left"><input id="fontsize" type="range" value="11" min="1" max="15" step="1" /></td>
                                    <td width="20%" align="right">文本颜色：</td>
                                    <td width="30%" align="left"><input id="fontcolor" type="color" value="#ff9818" /></td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="center" bgcolor="#ececec">
                                        <input style="background:#0094ff;color:#fff;padding:5px 10px 5px 10px;" type="button" onclick="refreshQR();" value="生成二维码" />
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>
                    <tr class="line">
                        <td colspan="3">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">2、联系方式和个人信息：
                        </td>
                    </tr>
                    <tr>
                        <td class="td1">手机号码：
                        </td>
                        <td class="td2">
                            <asp:TextBox ID="tbUserMobile" CssClass="input" runat="server" MaxLength="25"></asp:TextBox>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbUserMobileValid" runat="server" Text="已通过验证" />
                        </td>
                    </tr>
                   
                    <tr class="line">
                        <td colspan="3">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">3、银行卡信息：
                        </td>
                    </tr>
                    <tr>
                        <td class="td1">银行卡号：
                        </td>
                        <td class="td2">
                            <asp:TextBox ID="tbUserBankCardNumber" CssClass="input" runat="server" MaxLength="25"></asp:TextBox>
                        </td>
                        <td class="bank-error"></td>
                    </tr>
                    <tr>
                        <td class="td1">开户银行：
                        </td>
                        <td class="td2">
                            <asp:TextBox ID="tbBankName" CssClass="input" runat="server" MaxLength="25"></asp:TextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="td1">开户银行地址：
                        </td>
                        <td class="td2">
                            <asp:TextBox ID="tbBankAddress" CssClass="input" runat="server" MaxLength="25"></asp:TextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="td1">持卡人姓名：
                        </td>
                        <td class="td2">
                            <asp:TextBox ID="tbUserCradName" CssClass="input" runat="server" MaxLength="25"></asp:TextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr class="line">
                        <td colspan="3">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="td1"></td>
                        <td class="td2">
                            <%--<asp:CheckBox ID="cbPrivacy" runat="server" Text="用户资料保密" />--%>&nbsp;&nbsp;
                        <asp:CheckBox ID="cbisCanLogin" runat="server" Text="允许登录" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="td1"></td>
                        <td class="td2">
                            <asp:Button ID="btnSave" runat="server" CssClass="btn_operate" Text="修改" OnClick="btnSave_Click" />
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
<link href="../Components/My97DatePicker/skin/WdatePicker.css" rel="stylesheet" />
<script src="../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
<script src="../JScript/jquery-qrcode-0.14.0.min.js" type="text/javascript"></script>
<script src="../Components/My97DatePicker/WdatePicker.js"></script>
<script type="text/javascript" language="javascript">
    $(function () {
        getAgent($("#selectGroup").val());

        //生成二维码
        //$("#qrcode").qrcode({ width: 300, height: 300, render: !!document.createElement('canvas').getContext ? 'canvas' : 'table', text: "http://sygj.br66.cn/wxapi/wx_login.aspx?refferid=<%=uid%>"   });
        var options = {
            render: "image",
            ecLevel: 'H',//识别度
            fill: '#000',//二维码颜色
            background: '#ffffff',//背景颜色
            quiet: 2,//边距
            width: 300,//宽度
            height: 300,
            text: "<% =siteUrl %>AppGateWay/?pid=<%=uid%>",//二维码内容
            //中间logo start
            mode: 4,
            mSize: 0.15,
            mPosX: 0.5,
            mPosY: 0.5,
            image: $('#img-buffer')[0]//logo图片
        };
        $('#qrcode').empty().qrcode(options);

        



        $('#selectGroup').change(function(){ 
            getAgent($('#selectGroup').val())
        })  


        function getAgent(groupid) {
            var data = {
                action: "getAgent",
                groupid:groupid
            };

            $.post("Handler/Andy.ashx", data, function (d) {
                $("#selectRefer").empty();
                if(d.code=="200"){
                    var data = d.data;
                    for (var i = 0; i < data.length; i++) {
                        $("#selectRefer").append("<option value='" + data[i].value + "'>" + data[i].text + "</option>");
                    };
                }
                else{
                    $("#selectRefer").append("<option value='0'>无代理会员</option>");
                }

            },"json")
        }
    })



    function refreshQR() {

        $('#img-buffer').attr("src", $("#imgurl").val());
        var msize = 0.15;
        if (parseInt($("#mode").val())==2) {
            msize = parseFloat($("#fontsize").val())/100
        }


        var options = {
            render: "image",
            ecLevel: 'H',//识别度
            fill: '#000',//二维码颜色
            background: '#ffffff',//背景颜色
            quiet: 2,//边距
            width: 300,//宽度
            height: 300,
            text: $("#text").val(),
            //中间logo start
            mode: parseInt($("#mode").val()),
            mSize: msize,
            mPosX: 0.5,
            mPosY: 0.5,
            image: $('#img-buffer')[0],//logo图片

            label: $("#label").val(),
            fontname: '微软雅黑',
            fontsize: $("#fontsize").val(),
            fontcolor: $("#fontcolor").val()
        };
        $('#qrcode').empty().qrcode(options);


    }
</script>

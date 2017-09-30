<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Site2.aspx.cs" Inherits="Admin_Site2" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>系统设置-站点资料</title>

    <link type="text/css" href="../Style/Site.css" rel="stylesheet" />
    <style>
        .lotteryList > div {
            height: 30px;
            line-height: 30px;
            float: left;
            width: 100%;
            padding: 5px;
        }

            .lotteryList > div input {
                margin: 10px;
            }

        .dl_weixin {
            width: 100%;
            height: 220px;
        }

            .dl_weixin dl {
                float: left;
                margin-right: 10px;
            }

            .dl_weixin dt.top {
                width: 100%;
                line-height: 30px;
                font-weight: bold;
                text-align: center;
            }

            .dl_weixin dt.bottom {
                width: 100%;
                line-height: 24px;
                text-align: center;
            }

            .dl_weixin dd {
                width: 100%;
                text-align: center;
            }

        .siteContent {
            min-height: 240px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="title">
                网站二维码
            </div>
            <div class="siteContent">
                <div class="dl_weixin">
                    <asp:HiddenField runat="server" ID="hf_weixin" />
                    <asp:HiddenField runat="server" ID="hf_h5" />
                    <asp:HiddenField runat="server" ID="hf_app" />
                    <dl id="weixin" qrvalue="">
                        <dt class="top">微信客服</dt>
                        <dd>
							<img src="/Images/qrcode.png" width=200>
                            <asp:FileUpload  name="wxQrcode" id="wxQrcode" runat="server" accept=".png,.jpg,.jpeg,.gif,.bmp" />
                            <asp:Button ID="upButton" runat="server" CssClass="btn_operate" Text="上传图片" OnClick="btn_upload_Click" />
                            <asp:Image runat="server" ID="img_weixin" style="display: none" />
                        </dd>
                        <dt class="bottom"  style="display: none"><a href="javascript:void(0)" class="weixin_edit">编辑二维码</a></dt>
                    </dl>
                    <dl id="h5" qrvalue=""  style="display:none">
                        <dt class="top">手机网站</dt>
                        <dd>
                            <asp:Image runat="server" ID="h5_img" />
                        </dd>
                        <dt class="bottom"><a href="javascript:void(0)" class="h5_edit">编辑二维码</a> </dt>
                    </dl>
                    <dl id="app" qrvalue=""  style="display:none">
                        <dt class="top">APP下载</dt>
                        <dd>
                            <asp:Image runat="server" ID="app_img" />
                        </dd>
                        <dt class="bottom"><a href="javascript:void(0)" class="app_edit">编辑二维码</a></dt>
                    </dl>
                </div>
                <table style="width: 100%; display: none" class="QRCode">
                    <tr class="default">
                        <td class="td1">名称:
                        </td>
                        <td class="edit_title"></td>
                        <td style="width: 55%" rowspan="12" valign="top">
                            <div id="container">
                            </div>
                            <%--                        <a id="download" download="qrcode.png" href="javascript:void(0)">保存二维码</a> <a class="CannelEdit"
                            href="javascript:void(0)">取消</a>--%>
                        </td>
                    </tr>
                    <tr class="default">
                        <td style="width: 10%" class="td1">二维码尺寸:
                        </td>
                        <td style="width: 35%">
                            <input id="size" type="range" value="124" min="100" max="200" step="1" /><label class="size_value"></label>
                        </td>
                        <td></td>
                    </tr>
                    <tr class="default">
                        <td class="td1">背景颜色:
                        </td>
                        <td>
                            <input id="bg-color" type="color" value="#ffffff" />
                        </td>
                        <td></td>
                    </tr>
                    <tr class="default">
                        <td class="td1">前景颜色:
                        </td>
                        <td>
                            <input id="color" type="color" value="#000000" />
                        </td>
                        <td></td>
                    </tr>
                    <tr class="default">
                        <td class="td1">地址:
                        </td>
                        <td>
                            <textarea id="text" class="input">http://sls53.demo.shovesoft.net/</textarea>
                        </td>
                        <td></td>
                    </tr>
                    <tr class="img default font">
                        <td class="td1">类型:
                        </td>
                        <td>
                            <select id="mode" class="input" style="width: 120px;">
                                <option value="0">默认</option>
                                <%--                            <option value="1">文本-Strip</option>--%>
                                <option value="2">文本-Box</option>
                                <%--                            <option value="3">图片-Strip</option>--%>
                                <option value="4" selected="selected">图片-Box</option>
                            </select>
                        </td>
                        <td></td>
                    </tr>
                    <tr class="font">
                        <td class="td1">文本内容:
                        </td>
                        <td>
                            <input id="label" type="text" value="晓风彩票" class="input" />
                        </td>
                        <td></td>
                    </tr>
                    <tr class="font">
                        <td class="td1">文本大小:
                        </td>
                        <td>
                            <input id="fontsize" type="range" value="11" min="1" max="15" step="1" />
                        </td>
                        <td></td>
                    </tr>
                    <tr class="font">
                        <td class="td1">字体名称:
                        </td>
                        <td>
                            <input id="font" type="text" value="微软雅黑" class="input" />
                        </td>
                        <td></td>
                    </tr>
                    <tr class="font">
                        <td class="td1">文本颜色:
                        </td>
                        <td>
                            <input id="fontcolor" type="color" value="#ff9818" />
                        </td>
                        <td></td>
                    </tr>
                    <tr class="img">
                        <td class="td1">嵌入Logo:
                        </td>
                        <td>
                            <span id='uploadSpan'>
                                <input id="image" type="file" accept="image/png,image/gif" />
                            </span>
                            <img id="img-buffer" src="../Images/logo.png" border="1" style="display: none" />
                        </td>
                        <td></td>
                    </tr>
                    <tr class="default">
                        <td class="td1"></td>
                        <td>
                            <input type="button" value="保存" id="btnSave" class="btn_operate" />
                            <input type="button" value="取消" class="btn_operate CannelEdit" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <select id="render" style="display: none">
                                <option value="canvas" selected="selected">Canvas</option>
                                <option value="image">Image</option>
                                <option value="div">DIV</option>
                            </select>
                            <input id="minversion" type="range" value="6" min="1" max="10" step="1" style="display: none" />
                            <select id="eclevel" style="display: none">
                                <option value="L">L - Low (7%)</option>
                                <option value="M">M - Medium (15%)</option>
                                <option value="Q">Q - Quartile (25%)</option>
                                <option value="H" selected="selected">H - High (30%)</option>
                            </select>
                            <input id="imagesize" type="range" value="20" min="1" max="25" step="1" style="display: none" />
                            <input id="quiet" type="range" value="3" min="0" max="4" step="1" style="display: none" />
                            <input id="radius" type="range" value="50" min="0" max="50" step="10" style="display: none" />
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
            <div class="title">
                修改站点资料<span style="font-size: 12px; color: red;">(带红色*为必填)</span>
            </div>
            <div class="siteContent">
                <table>
                    <tr>
                        <td class="td1">网站名称:
                        </td>
                        <td class="td2">
                            <asp:TextBox ID="txt_siteName" runat="server" Text="" placeholder="网站名称" CssClass="input" /><span
                                style="color: red;"> *</span>
                        </td>
                        <td class="td3">公司名称:
                        </td>
                        <td class="td4">
                            <asp:TextBox ID="txt_company" runat="server" Text="" placeholder="公司名称" CssClass="input" /><span
                                style="color: red;"> *</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="td1">地址:
                        </td>
                        <td class="td2">
                            <asp:TextBox ID="txt_address" runat="server" Text="" placeholder="公司地址" CssClass="input" /><span
                                style="color: red;"> *</span>
                        </td>
                        <td class="td3">邮编:
                        </td>
                        <td class="td4">
                            <asp:TextBox ID="txt_post" runat="server" Text="" placeholder="邮编" CssClass="input" /><span
                                style="color: red;"> *</span>
                        </td>
                    </tr>
                   
                    <tr>
                        <td class="td1">QQ号码:
                        </td>
                        <td class="td2">
                            <asp:TextBox ID="txt_QQ" runat="server" Text="" placeholder="QQ号码" CssClass="input" /><span
                                style="color: red;"> *</span><br />
                            <span style="font-size: 12px;">用","分隔多个号码</span>
                        </td>
                        <td class="td3">客服微信:
                        </td>
                        <td class="td4">
                            <asp:TextBox ID="txt_email" runat="server" Text="" placeholder="Email" CssClass="input" /><span
                                style="color: red;"> *</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="td1">ICP证书号:
                        </td>
                        <td class="td2">
                            <asp:TextBox ID="txt_ICP" runat="server" Text="" placeholder="ICP证书号" CssClass="input" /><span
                                style="color: red;"> *</span>
                        </td>
                        <td class="td3">站点域名:
                        </td>
                        <td class="td4">
                            <asp:TextBox ID="txt_siteURL" runat="server" Text="" placeholder="站点域名" CssClass="input" /><span
                                style="color: red;"> *</span>
                        </td>
                    </tr>
                     <tr>
                        <td class="td1">客服热线:
                        </td>
                        <td class="td2">
                            <asp:TextBox ID="txt_serverMobile" runat="server" Text="" placeholder="服务电话" CssClass="input" /><span
                                style="color: red;"> *</span>
                        </td>
                        <td class="td3">&nbsp;</td>
                        <td class="td4">&nbsp;</td>
                    </tr>
                     <tr>
                        <td class="td1">首页公告栏:
                        </td>
                        <td class="td2" colspan="3">
                            <asp:TextBox ID="txt_mobile" runat="server" TextMode="MultiLine" Text="" placeholder="首页滚动通知" CssClass="input" />
                        </td>
                    </tr>



                    <tr>
                        <td class="td1">
                            <span style="color: red;">*</span> 使用彩种:
                        </td>
                        <td colspan="3" class="td2">
                            <div class="lotteryList" runat="server" id="useLotteryList">
                                <div style="background-color:#f0f8ff;display:none">
                                    <strong>足彩：</strong>
                                    <asp:CheckBox ID="chk_sfc" lotID="74" runat="server" Text="胜负彩" />
                                    <asp:CheckBox ID="chk_rjc" lotID="75" runat="server" Text="任九场" />
>
                                    <asp:CheckBox ID="chk_zqdc" lotID="45" runat="server" Text="北京单场" />
                                </div>
                                <div style="display:none">
                                    <strong>高频彩：</strong>
                                    <asp:CheckBox ID="chk_ssc" lotID="28" runat="server" Text="重庆时时彩" />
                                    <asp:CheckBox ID="chk_xjssc" lotID="66" runat="server" Text="新疆时时彩" />
                                    <asp:CheckBox ID="chk_syydj" lotID="62" runat="server" Text="十一运夺金" />
                                    <asp:CheckBox ID="chk_jsk3" lotID="83" runat="server" Text="江苏快3" />
                                </div>
                                <div style="background-color:#f0f8ff;">
                                    <strong> </strong>
                                    <asp:CheckBox ID="chk_gd11x5" lotID="78" runat="server" Text="广东11选5"  style="display:none"/>
                                    <asp:CheckBox ID="chk_bjpk10" lotID="94" runat="server" Text="北京PK10"  style="display:none"/>
                                    <asp:CheckBox ID="chk_jndxy28" lotID="98" runat="server" Text="加拿大28" />
                                    <asp:CheckBox ID="chk_bjxy28" lotID="99" runat="server" Text="北京28" />
                                    <asp:CheckBox ID="chk_txffc" lotID="100" runat="server" Text="腾讯分分彩"  style="display:none"/>
                                </div>
                                 <div style="background-color:#f0f8ff;display:none">
                                    <strong>竞彩：</strong>
                                    <asp:CheckBox ID="chk_jclq" lotID="73" runat="server" Text="竞彩篮球" />
                                    <asp:CheckBox ID="chk_jczq" lotID="72" runat="server" Text="竞彩足球" />
                                </div>
                                <div style="display:none">
                                    <strong>福彩：</strong>
                                    <asp:CheckBox ID="chk_ssq" lotID="5" runat="server" Text="双色球" />
                                    <asp:CheckBox ID="chk_cjdlt" lotID="39" runat="server" Text="超级大乐透" />
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr style="height: 5px;">
                        <td colspan="4" style="border-top: solid 1px #eee;"></td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btn_update" runat="server" CssClass="btn_operate" Text="修改" OnClick="btn_update_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
<script src="../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
<script src="../JScript/jquery.qrcode.js" type="text/javascript"></script>
<script src="../JScript/scripts.js" type="text/javascript"></script>
<script src="../JScript/ff-range.js" type="text/javascript"></script>
<script type="text/ecmascript">
    var editType = "";
    var html = $("#uploadSpan").html();
    $(function () {
        $("#btn_update").click(function () {
            var siteName = myTirm($("#txt_siteName").val());
            var company = myTirm($("#txt_company").val());
            var address = myTirm($("#txt_address").val());
            var post = myTirm($("#txt_post").val());
            var responsiblePerson = myTirm($("#txt_responsiblePerson").val());
            var contactPerson = myTirm($("#txt_contactPerson").val());
            var mobile = myTirm($("#txt_mobile").val());
            var fax = myTirm($("#txt_fax").val());
            var phone = myTirm($("#txt_phone").val());
            var email = myTirm($("#txt_email").val());
            var QQ = myTirm($("#txt_QQ").val());
            var serverMobile = myTirm($("#txt_serverMobile").val());
            var ICP = myTirm($("#txt_ICP").val());
            var siteURL = myTirm($("#txt_siteURL").val());
            if ("" == siteName) {
                alert("站点名称不能为空。");
                $("#txt_siteName").val("").focus();
                return false;
            }
            if ("" == company) {
                alert("公司名称不能为空。");
                $("#txt_company").val("").focus();
                return false;
            }
            if ("" == address) {
                alert("公司地址不能为空。");
                $("#txt_address").val("").focus();
                return false;
            }
            if ("" == post) {
                alert("邮编不能为空。");
                $("#txt_post").val("").focus();
                return false;
            }
            if (!(/\d{5}/.test(post))) {
                alert("请输入正确的邮编,邮编格式为：5位数字。");
                $("#txt_post").val("").focus();
                return false;
            }
            if ("" == responsiblePerson) {
                alert("负责人不能为空。");
                $("#txt_responsiblePerson").val("").focus();
                return false;
            }
            if ("" == contactPerson) {
                alert("联系人不能为空。");
                $("#txt_contactPerson").val("").focus();
                return false;
            }
            if ("" == mobile) {
                alert("电话号码不能为空。");
                $("#txt_mobile").val("").focus();
                return false;
            }
            if (!(/^\d+$|^[0-9-]+$/.test(mobile))) {
                alert("电话号码格式不正确。");
                $("#txt_mobile").val("").focus();
                return false;
            }
            if ("" == fax) {
                alert("传真不能为空。");
                $("#txt_fax").val("").focus();
                return false;
            }
            if (!((/^\d+$|^[0-9-*]+$/).test(fax))) {
                alert("传真格式不正确。");
                $("#txt_fax").val("").focus();
                return false;
            }
            if ("" == phone) {
                alert("手机号码不能为空。");
                $("#txt_phone").val("").focus();
                return false;
            }
            if (!((/^\d{11}$/).test(phone))) {
                alert("手机号码不正确，手机号码格式为11为数字。");
                $("#txt_phone").val("").focus();
                return false;
            }
            if ("" == email) {
                alert("邮箱不能为空。");
                $("#txt_email").val("").focus();
                return false;
            }
            //[\d\w]+\@[\d\w]+(\.((com)|(cn)))*
            //var match = email.match("[\d\w]+\@[\d\w]+(\.((com)|(cn)))*");
            //alert(email);
            if (!(/[\d\w]+\@[\d\w]+(\.((com)|(cn)))*/.test(email))) {
                alert("邮箱格式不正确，正确的邮箱格式如：cpkf@eims.com.cn。");
                $("#txt_email").val("").focus();
                return false;
            }
            if ("" == QQ) {
                alert("QQ号码不能为空。");
                $("#txt_QQ").val("").focus();
                return false;
            }
            if (!(/^([0-9]+(,)?)+$/.test(QQ))) {
                alert("QQ号码格不正确，只能是数字 长度最长15 最低为5位数，多个号码使用\",\"隔开。");
                $("#txt_QQ").val("").focus();
                return false;
            }
            if ("" == serverMobile) {
                alert("服务电话不能为空。");
                $("#txt_serverMobile").val("").focus();
                return false;
            }
            if (!((/^\d+$|^[0-9-]+$/).test(serverMobile))) {
                alert("服务电话格式不正确。");
                $("#txt_serverMobile").val("").focus();
                return false;
            }
            if ("" == ICP) {
                alert("ICP证书号不能为空。");
                $("#txt_ICP").val("").focus();
                return false;
            }
            if ("" == siteURL) {
                alert("站点域名不能为空。");
                $("#txt_siteURL").val("").focus();
                return false;
            }
            if (!(/(http(s)?:\/\/)?(([\d\w\W]+)+(\.)*([\/.?=&])?)+/.test(siteURL)) || siteURL.split(".").length < 2) {
                alert("站点域名格式不正确，请输入正确的域名格式。");
                $("#txt_siteURL").val("").focus();
                return false;
            }
            return confirm("请确认无误后修改站点资料。");
        });

        $(".CannelEdit").click(function () {
            $(".QRCode").hide();
            $(".dl_weixin").show();
        });

        $(".weixin_edit").click(function () {
            editType = "1";
            $(".QRCode").show();
            $(".dl_weixin").hide();
            $(".edit_title").text("微信");
            SetValue("weixin");

            InitQRCodeType($("#mode").val());
        });
        $(".h5_edit").click(function () {
            editType = "2";
            $(".QRCode").show();
            $(".dl_weixin").hide();
            $(".edit_title").text("手机网站");
            SetValue("h5");

            InitQRCodeType($("#mode").val());
        });
        $(".app_edit").click(function () {
            editType = "3";
            $(".QRCode").show();
            $(".dl_weixin").hide();
            $(".edit_title").text("APP下载");
            SetValue("app");

            InitQRCodeType($("#mode").val());
        });

        $("#btnSave").click(function () {
            var Size = $("#size").val();
            var BGColor = $("#bg-color").val();
            var QRCodeColor = $("#color").val();
            var Url = $("#text").val();
            var QRCodeType = $("#mode").val();
            var ImageSrc = $("#img-buffer").attr("src");
            var Option = "";
            var FontContent = $("#label").val();
            var FontSize = $("#fontsize").val();
            var FontFamily = $("#font").val();
            var FontColor = $("#fontcolor").val();
			

            var qrValue = "{'Size':'" + Size + "','BGColor':'" + BGColor + "','QRCodeColor':'" + QRCodeColor + "','Url':'" + Url
             + "','QRCodeType':'" + QRCodeType + "','ImageSrc':'" + ImageSrc + "','FontContent':'" + FontContent + "','FontSize':'" + FontSize + "','FontFamily':'" + FontFamily + "','FontColor':'" + FontColor + "'}";

            var data = $("#container canvas")[0].toDataURL('image/png');
            if (editType == "1") {
                Option = "WeiXin";
                $("#weixin").attr("qrvalue", qrValue);
                $("#img_weixin").attr("src", data);
            }
            if (editType == "2") {
                Option = "H5";
                $("#h5").attr("qrvalue", qrValue);
                $("#h5_img").attr("src", data);
            }
            if (editType == "3") {
                Option = "APP";
                $("#app").attr("qrvalue", qrValue);
                $("#app_img").attr("src", data);
            }
            $(".QRCode").hide();
            $(".dl_weixin").show();

            $("#image").after($("#image").clone().val(''));
            $("#image").remove();
            $("#image").on('change', onImageInput);

            Admin_Site2.SaveQRCodeImage("{\"Option\":\"" + Option + "\",\"Size\":\"" + Size + "\",\"BGColor\":\"" + BGColor
             + "\",\"QRCodeColor\":\"" + QRCodeColor + "\",\"Url\":\"" + Url + "\",\"QRCodeType\":\"" + QRCodeType + "\",\"ImageSrc\":\"" +
             ImageSrc + "\",\"QRCodeImg\":\"" + data + "\",\"FontContent\":\"" + FontContent + "\",\"FontSize\":\"" +
             FontSize + "\",\"FontFamily\":\"" + FontFamily + "\",\"FontColor\":\"" + FontColor + "\"}");
        });

        Init("weixin");
        Init("h5");
        Init("app");
    });

    function SetValue(id) {
        var jsonValue = eval('(' + $("#" + id).attr("qrvalue") + ')');

        $("#size").val(jsonValue.Size);
        $("#bg-color").val(jsonValue.BGColor);
        $("#color").val(jsonValue.QRCodeColor);
        $("#text").val(jsonValue.Url);
        $("#mode").val(jsonValue.QRCodeType);
        $("#img-buffer").attr("src", jsonValue.ImageSrc);
        $("#label").val(jsonValue.FontContent);
        $("#fontsize").val(jsonValue.FontSize);
        $("#font").val(jsonValue.FontFamily);
        $("#fontcolor").val(jsonValue.FontColor);
        update();
    }

    function Init(id) {
        $("#weixin").attr("qrvalue", $("#hf_weixin").val());
        $("#h5").attr("qrvalue", $("#hf_h5").val());
        $("#app").attr("qrvalue", $("#hf_app").val());
		
        //        var jsonValue = eval('(' + $("#" + id).attr("qrvalue") + ')');
        //        if (id == "weixin")
        //        {
        //            $("#img_weixin").attr("src", jsonValue.QRCodeImg);
        //        }
        //        if (id == "h5")
        //        {
        //            $("#h5_img").attr("src", jsonValue.QRCodeImg);
        //        }

        //        if (id == "app")
        //        {
        //            $("#app_img").attr("src", jsonValue.QRCodeImg);
        //        }
    }

    //去除空格
    function myTirm(val) {
        return val.replace(/[\s]/g, "");
    }

    $("#mode").change(function () {
        InitQRCodeType($(this).val());
    });

    function InitQRCodeType(optionValue) {
        $(".QRCode tbody>tr").hide();
        if (optionValue == "0") {
            $(".QRCode .default").show();
        }
        if (optionValue == "1") {
            $(".QRCode .default").show();
            $(".QRCode .font").show();
        }
        if (optionValue == "2") {
            $(".QRCode .default").show();
            $(".QRCode .font").show();
        }
        if (optionValue == "3") {
            $(".QRCode .default").show();
            $(".QRCode .img").show();
        }
        if (optionValue == "4") {
            $(".QRCode .default").show();
            $(".QRCode .img").show();
        }
    }

</script>
</html>

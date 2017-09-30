<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Join_Default" %>
<%@ Register Src="~/Home/Room/UserControls/WebHead.ascx" TagName="WebHead" TagPrefix="uc2" %>
<%@ Register Src="~/Home/Room/UserControls/WebFoot.ascx" TagName="WebFoot" TagPrefix="uc1" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>合买大厅-北京幸运28-广东11选5-财富28娱乐信息科技有限公司</title>
    <meta name="description" content="北京单场官方网站,加拿大幸运28" />
    <meta name="keywords" content="平台提供最热门彩种重庆时时彩，天津时时彩，五分彩，北京幸运28，加拿大幸运28、北京赛车，北京单场，大乐透，十一运夺金，胜负彩，新疆时时彩，任九彩，
广东11选5，山东11选5，上海时时乐，江苏快三，福彩3D，双色球等，后续将陆续推出更多游戏项目。" />
    <link href="../Style/partnering.css" rel="stylesheet" type="text/css" />
    <link href="../Style/global.css" rel="stylesheet" type="text/css" />
    <link href="../Style/index.css" rel="stylesheet" type="text/css" />
    <link href="../Style/style.css" rel="stylesheet" type="text/css" />
    <link href="../../Style/sandPage.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .group_buy a {
            cursor: pointer;
        }

        .group_buy strong {
            margin-left: 4px;
            padding: 2px;
            background-color: #EA6C6F;
            color: #fff;
            border-radius: 2px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc2:WebHead ID="WebHead1" runat="server" />
        <div class="main">
            <div class="fire-scheme border-gray">
                <h3 class="segoe-ui col-red text-center border-gray-bottom">红人方案</h3>
                <div id="hongren">
                </div>
            </div>
            <div class="chart border-gray">
                <ul class="tabs clear">
                    <li class="active" onclick="uid=0;pags=1; clik(''); "><a>全部彩种</a>|<span></span><input
                        type="text" id="tp" value="1" style="display: none;" /></li>
                    <%=lottidt %>
                </ul>
                <div id="name" style="height: 100px;">
                </div>
            </div>
            <table cellspacing="0" class="group_buy" id="tabload">
                <tbody>
                    <tr class="caib_top">
                        <th class="t_c">序号
                        </th>
                        <th style="text-align: center;">发起人
                        </th>
                        <th class="t_z">战绩<span></span>
                        </th>
                        <th class="t_caizhong">彩种
                        </th>
                        <th style="text-align: center;">方案进度 <span></span>
                        </th>
                        <th class="t_r" align="right">方案总金额/<span></span>
                        </th>
                        <th class="totalmoney01">每份
                        </th>
                        <th style="text-align: center">认购份数
                        </th>
                        <th class="t_c">操作
                        </th>
                    </tr>
                </tbody>
            </table>
            <div id="sand" style="text-align: right; padding-right: 30px;">
            </div>
            <div class="space-30">
            </div>
        </div>
        <!-- 页脚
		================================================== -->
        <input type="hidden" id="lid" value="" runat="server" />
        <input type="hidden" id="hong" value="-1" />
        <input type="hidden" id="pat" value="1" />
        <input type="hidden" id="ye" value="1" />
        <uc1:WebFoot ID="WebFoot1" runat="server" />
        <script src="../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
        <script src="../../JScript/sandPage.js" type="text/javascript"></script>
        <script type="text/javascript">
            $(function () {

                //彩种选项卡
                $('.chart .tabs > li').click(function () {
                    $(this).addClass('active');
                    $(this).siblings().removeClass('active');
                });

                //分页样式
                $('.page .num').click(function () {
                    $(this).addClass('active');
                    $(this).siblings().removeClass('active');
                });

                var li = $("#lid").val();

                if (li == "") {
                    InitData(pag);

                } else {
                    $(".tabs li").removeClass("active");
                    $("#li" + li).addClass("active");
                    clik(li);
                }
            });

            function MReduction1(id) {   //判断认购份数
                var Share = parseInt($("#txtShare_" + id).val());

                if (isNaN(Share)) {
                    $("#txtShare_" + id).val(1);
                    return;
                }
                if (Share == 1) {
                    $("#txtShare_" + id).val(1);
                    return;

                } else {
                    Share--;
                    $("#txtShare_" + id).val(Share);
                }
            }

            //合买份数递增
            function MAddition1(id) {
                var Share = parseInt($("#txtShare_" + id).val());
                if (isNaN(Share))
                    Share = 0;

                Share++;
                $("#txtShare_" + id).val(Share);
                var Surplus = parseInt($("#spSurplus_" + id).text());
                if (Surplus <= Share) {
                    $("#txtShare_" + id).val(Surplus);
                    return;
                }
            }

            function blue(id) {
                var Surplus = parseInt($("#spSurplus_" + id).text());

                //判断份数是否正确
                var Share = parseInt($("#txtShare_" + id).val());
                if (isNaN(Share)) {
                    Share = 1;

                    $("#txtShare_" + id).val("剩" + Surplus + "份");
                    return;
                }

                var Surplus = parseInt($("#spSurplus_" + id).text());
                $("#txtShare_" + id).val(Share);
                if (Surplus < Share) {
                    $("#txtShare_" + id).val(Surplus);
                    return;
                }
                if (Share <= 0) {
                    $("#txtShare_" + id).val(1);
                }
            }

            //点击名人显示彩种
            function hongclik(obj) {
                pag = 1;
                uid = obj;
                var naid = $("#tp").val();
                clik(naid);
            }

            //全部信息点击名人查看彩种
            function hongcliks(obj) {
                pag = 1;
                pag = 1;
                uid = obj;
                InitData(pag);
            }

            //购买份数判断
            function setFocus(obj) {
                var inputVal = $(obj).val();

                if (inputVal.indexOf("份") >= 0) {
                    $(obj).val("");
                }
            }

            //购买
            function setBlur(obj) {

                var inputId = $(obj).attr("id");
                var inputVal = $(obj).val();
                var regStr = /^[1-9][0-9]*$/;
                var totalSurplus = $("#" + inputId.replace("txtShare_", "spSurplus_")).text();

                if (inputVal == ' ' || inputVal == null || !inputVal) {
                    $(obj).val(inputVal);
                    return;
                }
                if (!regStr.test(inputVal)) {
                    alert("请输入正确的购买份数");
                    $("#" + inputId).val("1");
                    return;
                }
                if (parseInt(totalSurplus) < parseInt(inputVal)) {
                    alert("剩余购买份数不足！");
                    $(obj).val(surplus);
                    return;
                }
            }

            //全部信息购买调用
            function join(SchemeID, Share, ShareMoney, LotteryID) {
                $.ajax({
                    type: "POST", //用POST方式传输
                    dataType: "json", //数据格式:JSON
                    url: 'Join.ashx', //目标地址
                    data: "SchemeID=" + SchemeID + "&BuyShare=" + Share + "&ShareMoney=" + ShareMoney + "&LotteryID=" + LotteryID,
                    success: function (json) {
                        if (json.message == "您的账户余额不足，请先充值，谢谢。") {
                            var okfunc = function () { location.href = "/Home/Room/OnlinePay/Alipay02/Send_Default.aspx"; };
                            var cancelfunc = function () { };
                            confirm("您的账户余额不足，请先充值！\n\n按“确定”立即充值", okfunc, cancelfunc);
                            return;
                        }
                        else {
                            alert(json.message);
                            $("#name p").remove();
                            $("#name img").remove();
                            $("#name a").remove();
                            $("#name input").remove();
                            InitData(pag);
                        }
                    }
                });
            }

            //点击彩种购买调用
            function joint(SchemeID, Share, ShareMoney, LotteryID, obj) {
                $(obj).unbind("click");
                var TipStr = "您要入伙此合买方案，详细内容：\n\n";
                TipStr += "　　份　数：　" + Share + " 份\n";
                TipStr += "　　每　份：　" + ShareMoney + " 元\n";
                TipStr += "　　总金额：　" + parseFloat(ShareMoney * Share).toFixed(2) + " 元\n\n";
                var okfunc = function () {
                    $.ajax({
                        type: "POST", //用POST方式传输
                        dataType: "json", //数据格式:JSON
                        url: 'Join.ashx', //目标地址
                        data: "SchemeID=" + SchemeID + "&BuyShare=" + Share + "&ShareMoney=" + ShareMoney + "&LotteryID=" + LotteryID,

                        success: function (json) {
                            if (json.message == "您的账户余额不足，请先充值，谢谢。") {
                                var okfunc_success = function () { location.href = "/Home/Room/OnlinePay/Alipay02/Send_Default.aspx"; };
                                var cancelfunc_success = function () { $(obj).bind("click", { "obj": obj }, GM_Clicks); };
                                confirm("您的账户余额不足，请先充值！\n\n按“确定”立即充值", okfunc_success, cancelfunc_success);
                            }
                            else {
                                alert(json.message);
                                $("#name p").remove();
                                $("#name img").remove();
                                $("#name a").remove();
                                $("#name input").remove();
                                caiye($("#lid").val());
                            }
                        }
                    });
                };
                var cancelfunc = function () { $(obj).bind("click", { "obj": obj }, GM_Clicks); };
                confirm(TipStr + "按“确定”即表示您已阅读《用户代购合买协议》并立即参与合买方案，确定要入伙吗？", okfunc, cancelfunc);
                return;
            }

            //全部页面
            function quye(id) {
                $("#tabload td").remove();
                $("#name p").remove();
                $("#name img").remove();
                InitData(id);
            }

            //彩种页面
            function caiye(id) {
                $("#tabload td").remove();
                $("#name p").remove();
                $("#name img").remove();
                clik(id);
            }

            var pageinfo = {
                index: 0
            }
            var pag = 1; //全部信息当前页数
            var uid = 0; //默认用户ID                                     

            var totalPage = 0;
            var totalRecords = 0;

            function pageLoad() {
                sand.init({
                    //当前页
                    pno: pag,
                    //总页码
                    total: totalPage,
                    //总数据条数
                    totalRecords: totalRecords,
                    //链接前部
                    //hrefFormer: 'MonitoringLog',
                    //链接尾部
                    //hrefLatter: '.aspx',
                    getLink: function (n) {
                        return "javascript:InitData(" + n + ")";
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
            }

            function InitData(page) {

                //首次加载查看合买信息  
                var na = "quan";
                var lid = $("#lid").val();

                $.ajax({
                    type: "POST", //用POST方式传输
                    dataType: "json", //数据格式:JSON
                    url: "HeMai.ashx", //目标地址
                    async: false,
                    data: "lottid=" + lid + "&name=" + na + "&pag=" + page + "&userid=" + uid,
                    success: function (data) {
                        $("#tabload tr:gt(0)").remove();
                        $("#hongren a").remove();
                        $("#name p").remove();
                        $("#name a").remove();
                        $("#name img").remove();
                        $("#name input").remove();
                        pag = data.page;
                        totalPage = data.pagsize;
                        totalRecords = data.pagecount;
                        //生成
                        pageLoad();
                        try {

                            if (data.top == "暂无名人") {

                                $("#hongren").append('<a>暂无红人</a>');

                            } else {

                                $("#hongren").append(data.top);

                            }
                            if (data.masf == "72") {

                                $("#name").append('<p class=\"border-gray-top\" style="  color:red; " >合买截止：赛前或官方截止前 10分钟 </p>');
                                $("#name").append('<p class=\"border-gray-top\" style="  color:red; "> </p><a id=\"buy-button\" href=\"../JCZC/Buy_SPF.aspx" target=\"_blank\" ></a>');
                                $("#name").append('<img src=\"../Images/PassTatistics/72.png\"> ');
                            }
                            else if (data.masf == "45") {

                                $("#name").append('<p class=\"border-gray-top\" style="  color:red; " >合买截止：赛前或官方截止前 10分钟 </p>');
                                $("#name").append('<p class=\"border-gray-top\" style="  color:red; "> </p><a id=\"buy-button\" href=\"../BJDC/Buy_RQSPF.aspx" target=\"_blank\" ></a>');
                                $("#name").append('<img src=\"../Images/PassTatistics/45.png\"> ');
                            }
                            else if (data.masf == "73") {

                                $("#name").append('<p class=\"border-gray-top\" style="  color:red; " >合买截止：赛前或官方截止前 10分钟 </p>');
                                $("#name").append('<p class=\"border-gray-top\" style="  color:red; "> </p><a id=\"buy-button\" href=\"../JCLC/Buy_SF.aspx" target=\"_blank\" ></a>');
                                $("#name").append('<img src=\"../Images/PassTatistics/73.png\"> ');
                            }
                            else {
                                $("#name img").remove();
                                $("#name").append(data.masf);
                                $("#pageseq").change(function () {
                                    var natt = $("#pageseq").val();

                                    Wei(natt);

                                });

                                LotteryTimer.Init();

                            }
                            if (data.message == "暂无合买信息") {

                            }
                            else {
                                $("#tabload").append(data.message);
                                $(".t_r span a").click(function () {
                                    GM_Click(this);
                                });
                                $(".group_input").keyup(function () {
                                    if (/\D/.test(this.value)) {
                                        this.value = parseInt(this.value) || 1;
                                    }
                                });
                            }
                        } catch (e) {

                        };
                    }
                });
            }

            //点击彩种显示合买信息
            function clik(lid) {
                var na = "quan";
                $("#tp").val(lid);
                $("#lid").val(lid);

                $.ajax({
                    type: "POST", //用POST方式传输
                    dataType: "json", //数据格式:JSON
                    url: "HeMai.ashx", //目标地址
                    async: false,
                    data: "lottid=" + lid + "&name=" + na + "&pag=1&userid=" + uid,
                    success: function (data) {
                        $("#tabload tr:gt(0)").remove();
                        $("#hongren a").remove();
                        $("#name p").remove();
                        $("#name a").remove();
                        $("#name img").remove();
                        $("#name input").remove();
                        pag = data.page;
                        totalPage = data.pagsize;
                        totalRecords = data.pagecount;
                        //生成
                        pageLoad();
                        try {

                            if (data.top == "暂无名人") {

                                $("#hongren").append('<a>暂无红人</a>');

                            } else {

                                $("#hongren").append(data.top);

                            }
                            if (data.masf == "72") {

                                $("#name").append('<p class=\"border-gray-top\" style="  color:red; " >合买截止：赛前或官方截止前 10分钟 </p>');
                                $("#name").append('<p class=\"border-gray-top\" style="  color:red; "> </p><a id=\"buy-button\" href=\"../JCZC/Buy_SPF.aspx" target=\"_blank\" ></a>');
                                $("#name").append('<img src=\"../Images/PassTatistics/72.png\"> ');
                            }
                            else if (data.masf == "45") {

                                $("#name").append('<p class=\"border-gray-top\" style="  color:red; " >合买截止：赛前或官方截止前 10分钟 </p>');
                                $("#name").append('<p class=\"border-gray-top\" style="  color:red; "> </p><a id=\"buy-button\" href=\"../BJDC/Buy_RQSPF.aspx" target=\"_blank\" ></a>');
                                $("#name").append('<img src=\"../Images/PassTatistics/45.png\"> ');
                            }
                            else if (data.masf == "73") {

                                $("#name").append('<p class=\"border-gray-top\" style="  color:red; " >合买截止：赛前或官方截止前 10分钟 </p>');
                                $("#name").append('<p class=\"border-gray-top\" style="  color:red; "> </p><a id=\"buy-button\" href=\"../JCLC/Buy_SF.aspx" target=\"_blank\" ></a>');
                                $("#name").append('<img src=\"../Images/PassTatistics/73.png\"> ');
                            }
                            else {
                                $("#name img").remove();
                                $("#name").append(data.masf);
                                $("#pageseq").change(function () {
                                    var natt = $("#pageseq").val();

                                    Wei(natt);

                                });

                                LotteryTimer.Init();

                            }
                            if (data.message == "暂无合买信息") {

                            }
                            else {
                                $("#tabload").append(data.message);
                                $(".t_r span a").click(function () {
                                    GM_Click(this);

                                });
                                $(".group_input").keyup(function () {
                                    if (/\D/.test(this.value)) {
                                        this.value = parseInt(this.value) || 1;
                                    }
                                });
                            }
                        } catch (e) {

                        };
                    }
                });
            }

            function GM_Click(obj) {
                if (!CreateLogin(obj)) {
                    return;
                }
                var islottery = 1;
                $.ajax({
                    type: "POST", //用POST方式传输
                    url: "../ajax/DropLottery.ashx", //目标地址
                    async: false,
                    data: "lottery=" + $(obj).attr("rel"),
                    success: function (data) {
                        islottery = data;
                    }
                });
                if (islottery < 1) {
                    alert("该彩种已经禁止销售");
                    return false;
                }

                var buyShare = $("#txtShare_" + $(obj).attr("mid")).val();
                if (buyShare.indexOf("剩") >= 0) {
                    alert("请输入购买份数");
                    return;
                }
                joint($(obj).attr("mid"), $("#txtShare_" + $(obj).attr("mid")).val(), $("#spanShareMoney_" + $(obj).attr("mid") + " span").html(), $(obj).attr("rel"), obj);
            }

            function GM_Clicks(josns) {
                var obj = josns.data.obj;
                if (!CreateLogin(obj)) {
                    return;
                }
                var islottery = 1;
                $.ajax({
                    type: "POST", //用POST方式传输
                    url: "../ajax/DropLottery.ashx", //目标地址
                    async: false,
                    data: "lottery=" + $(obj).attr("rel"),
                    success: function (data) {
                        islottery = data;
                    }
                });
                if (islottery < 1) {
                    alert("该彩种已经禁止销售");
                    return false;
                }

                var buyShare = $("#txtShare_" + $(obj).attr("mid")).val();
                if (buyShare.indexOf("剩") >= 0) {
                    alert("请输入购买份数");
                    return;
                }
                joint($(obj).attr("mid"), $("#txtShare_" + $(obj).attr("mid")).val(), $("#spanShareMoney_" + $(obj).attr("mid") + " span").html(), $(obj).attr("rel"), obj);
            }

        </script>
        <script type="text/javascript">
            /**************彩票投注计时器******************/
            var LotteryTimer =
    {
        IsCurrIsuseEndTime: false, //是否允许投注

        CurrentNo: "", //当前期

        ShowHtml: "销售截止时间：本期已截止销售", //显示倒计时HTML

        ServerTime: "", //服务器时间(默认为空)

        AdvanceEndTime: "", //提前截止时间

        IsShowTime: false, //是否显示倒计时(默认为False)

        StartTimerId: "",

        Init: function ()//初始化数据
        {
            if (LotteryTimer.StartTimerId != "") clearInterval(LotteryTimer.StartTimerId);

            if ($("#tee").val() != undefined) {
                LotteryTimer.AdvanceEndTime = new Date($("#tee").val().replace(new RegExp("-", "g"), "/")); //获取终止时间
                LotteryTimer.StartTimerId = setInterval(LotteryTimer.StartTimer, 1000);
            }
        },

        StartTimer: function ()//启动计时器
        {
            LotteryTimer.GetServerTime();

            if (LotteryTimer.AdvanceEndTime != "") {
                //getTime()返回当前时间的毫秒数
                var tt = LotteryTimer.AdvanceEndTime.getTime() - LotteryTimer.ServerTime.getTime(); //当前时间与服务器时间的差值（毫秒）
                var d = Math.floor(tt / 1000 / 60 / 60 / 24); //时间差（天）
                var h = Math.floor(tt / 1000 / 60 / 60 % 24); //时间差（小时）
                var m = Math.floor(tt / 1000 / 60 % 60); //时间差（分）
                var s = Math.floor(tt / 1000 % 60); //时间差（秒）

                if (tt > 0)//还未截止
                {
                    LotteryTimer.ShowHtml = "销售截止时间：" + d + "天" + h + "小时" + m + "分" + s + "秒";
                    LotteryTimer.IsCurrIsuseEndTime = true; //允许投注
                    LotteryTimer.IsShowTime = true;

                } else {
                    LotteryTimer.ShowHtml = "销售截止时间：本期已截止销售";
                    LotteryTimer.IsCurrIsuseEndTime = false; //不允许投注
                    LotteryTimer.IsShowTime = false;
                }

                $("#timer").text(LotteryTimer.ShowHtml);
            }
        },

        GetServerTime: function ()//获取服务器时间
        {
            $.ajax({
                type: "post",
                url: "../../ajax/getServerTime.ashx",
                cache: false,
                async: false, //使用同步去获取时间，True为异步
                dataType: "text",
                success: function (result) {
                    LotteryTimer.ServerTime = new Date(result.replace(new RegExp("-", "g"), "/")); //服务器时间
                },
                error: function (status) {
                    LotteryTimer.ServerTime = "";
                }
            });
        }
    };
        </script>
    </form>
</body>
</html>

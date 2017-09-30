var orderby = "";
var num = "";
var Condition = "";
var State = "";
var PlayTypeID = "";
var Name = "";
var type = "";

$().ready(function () {
    InitData(0);

    /*$("#EachPageNum a").click(function() {
    $("#EachPageNum").children("a").removeClass("current");
    $(this).addClass("current");
    num = $(this).html();
    InitData(0);
    });*/

    $("#govalue").keyup(function () {
        $(this).val($(this).val().replace(/[^\d]/g, ''));

        if (parseInt($(this).val()) > parseInt($("#last").val())) {
            $(this).val($("#last").val());
        }

        if (parseInt($(this).val()) <= 0) {
            $(this).val(1);
        }
    });

    $("#govalue").keydown(function () {
        if (event.keyCode == 13) { InitData(parseInt($("#govalue").val()) - 1); return false; }
    });

    $("#Btn_Go").click(function () {
        if (isNaN($("#govalue").val())) {
            return false;
        }
        InitData(parseInt($("#govalue").val()) - 1);
    });
});

function pageselectCallback(page_id, jq) {
    InitData(page_id);
}

function InitData(pageindx) {
    var tbody = "";
    var LotteryID = $("#hidLotteryID").val();
    num = $("#hidEachPageNum").val(); //每页数量
    type = $("#hidPageType").val(); //0、合买大厅首页 1、合买大厅详情页
    var initiateName = $("#hidInitiateUserName").val();
    if (initiateName) {
        Condition = "Name=" + initiateName;
    }
    if (type == "1") {
        var arrayPerson = Join_Project_List.BindPersonAges(LotteryID).value;
        if (arrayPerson.length == 2) {
            $("#hidPersonAgesList").val(arrayPerson[0]);
            $("#hidPersonAgesNameList").val(arrayPerson[1]);

            var hmmr = Join_Project_List.BindPersonAgeInfo(LotteryID, arrayPerson[0], arrayPerson[1]).value;
            $("#dv_hmmr ul").html(hmmr);
        }
    }

    $.ajax({
        type: "POST", //用POST方式传输
        dataType: "json", //数据格式:JSON
        url: 'ProjectList.ashx', //目标地址
        data: "p=" + (pageindx + 1) + "&LotteryID=" + LotteryID + "&orderby=" + orderby + "&EachPageNum=" + num + "&type=" + type + Condition,
        beforeSend: function () { $("#divload").show(); $("#SchemeList").hide(); }, //发送数据之前
        complete: function () { $("#divload").hide(); $("#SchemeList").show() }, //接收数据完毕
        success: function (json) {
            $("#SchemeList tr:gt(0)").remove();
            try {
                $("#SchemeList").append(json[0][0]["Content"]);
                $("#Pagination").html(json[1][0]["page"]);
            } catch (e) { };

            $("#SchemeList tr:gt(0):odd").attr("class", "");
            $("#SchemeList tr:gt(0):even").attr("class", "th_even");

            $("#SchemeList tr:gt(0)").hover(function () {
                $(this).addClass('th_on');
            }, function () {
                $(this).removeClass('th_on');
            });


            $(".Share").keyup(function () {
                if (/\D/.test(this.value))
                    this.value = parseInt(this.value) || 1;
            });

            $(".join img").click(function () {
                var buyShare = $("#txtSurplus_" + $(this).attr("mid")).val();
                if (buyShare.indexOf("剩余") >= 0) {
                    parent.msg("请输入购买份数");
                    return;
                    //buyShare = $("#tbShare_" + $(this).attr("mid")).val(); //购买份数
                }
                var shareMoney = $("#spanShareMoney_" + $(this).attr("mid")).text().split('元')[0]; //每份金额
                var joinMoney = parseInt(buyShare) * parseInt(shareMoney); //购买金额
                var Balance = $("#hidBalance").val();
                var Users = $("#hidUsers").val();
                if (Users == null || Users == "") {
                    alert("请先登录！");
                    location.href = "../../UserLogin.aspx";
                }
                else {
                    if (Balance < joinMoney) {
                        alert("余额不足，请去充值");
                        location.href = "../../Home/Room/OnlinePay/Alipay02/Send_Default.aspx";
                    }
                    else {
                        join($(this).attr("mid"), buyShare, joinMoney, LotteryID);
                    }
                }
            });

            $(".Share").blur(function () {
                $(this).val($(this).val().replace(/[^\d]/g, ''));

                if (parseInt($(this).val()) > parseInt($(this).parent().prev().children().html())) {
                    $(this).val($(this).parent().prev().children().html());
                }

                if (parseInt($(this).val()) <= 0) {
                    $(this).val(1);
                }
            });
        }
    });
}

function HmmrSchemes(initiateName) {
    $("#SerachCondition").val(initiateName);
    $("#srearch").click();
}

function setFocus(obj) {
    var inputVal = $(obj).val();
    if (inputVal.indexOf("份") >= 0) {
        $(obj).val("");
    }
}

function setBlur(obj) {
    var inputId = $(obj).attr("id");
    var inputVal = $(obj).val();
    var regStr = /^[1-9][0-9]*$/;   
    var totalSurplus = $("#" + inputId.replace("txtSurplus_", "spanSurplus_")).text();

    if (inputVal == ' ' || inputVal == null || !inputVal) {
        $(obj).val(surplus);
        return;
    }
    if (!regStr.test(inputVal)) {
        parent.msg("请输入正确的购买份数");
        $(obj).val(surplus);
        return;
    }
    if (parseInt(totalSurplus) < parseInt(inputVal)) {
        parent.msg("剩余购买份数不足！");
        $(obj).val(surplus);
        return;
    }
}

function Sort(ordercolumn, ordertipid) {
    var ordertype = ""; //1:desc,0:asc
    var $orderimg = $("#" + ordertipid);
    if ($orderimg.html() != "") {
        var imgsrc = $("img", $orderimg).attr("src");

        if (imgsrc.indexOf("asc") > -1) {
            $(".ordertip").empty();
            $orderimg.html("&nbsp;<img src=\"Images/sort_desc.gif\" align=\"absmiddle\">");

            ordertype = 1;
        }
        else {
            $(".ordertip").empty();
            $orderimg.html("&nbsp;<img src=\"Images/sort_asc.gif\" align=\"absmiddle\">");

            ordertype = 0;
        }
    }
    else {
        $(".ordertip").empty();
        $orderimg.html("&nbsp;<img src=\"Images/sort_desc.gif\" align=\"absmiddle\">");
        ordertype = 1;
    }

    orderby = ordercolumn + "_" + ordertype;

    InitData(0);
}
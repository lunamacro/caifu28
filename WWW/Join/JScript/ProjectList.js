var Condition = "";
var State = "";
var PlayTypeID = "";
var Name = "";
var SchemeBonusScale = "";

$().ready(function () {
    $("#myTab li").click(function () {
        $("#myTab").children("li").removeClass("active");
        $(this).addClass("active");
        PlayTypeID = "";
        if (!isNaN($(this).attr("mid"))) {
            PlayTypeID = "&PlayTypeID=" + $(this).attr("mid");
        }
        Condition = State + PlayTypeID + Name;

        InitData(0);
    });

    $("#srearch").click(function () {
        Name = "";
        if (($("#SerachCondition").val() != "" && $("#SerachCondition").val() != "发起人")) {
            Name = "&Name=" + $("#SerachCondition").val();
        }
        Condition = State + PlayTypeID + Name;

        InitData(0);
    });

    $(".f_hot a").click(function () {
        Name = "";
        Name = "&Name=" + $(this).html();
        Condition = State + PlayTypeID + Name;
        InitData(0);
    });

    //方案状态（1、未满员 2、已撤单 100、满员）
    $("#state_term").change(function () {
        State = "";

        if ($(this).val() != "") {
            State = "&State=" + $(this).val();
        }

        Condition = State + PlayTypeID + Name;
        InitData(0);
    });

    //佣金比例
    $("#schemeBonus_term").change(function () {
        SchemeBonusScale = ""

        SchemeBonusScale = "&SchemeBonusScale=" + $(this).val();
        Condition = State + PlayTypeID + Name + SchemeBonusScale;

        InitData(0);
    });

    $(".f_class span").each(function () {
        var lotId = parseInt($(this).attr("mid"));
        if (lotId == parseInt($("#hidLotteryID").val())) {
            $(this).addClass("curr");
        }
        else {
            $(this).removeClass("curr");
        }
    });

    $("#cpTab0 li").each(function (i) {//check事件
        this.onclick = function () {
            $(this).addClass("active").parent().end().siblings().removeClass("active");
            InitData($(this).attr("mid"));
        }
    });
});

function join(SchemeID, Share, ShareMoney, LotteryID) {
    $.ajax({
        type: "POST", //用POST方式传输
        dataType: "json", //数据格式:JSON
        url: 'Join.ashx', //目标地址
        data: "SchemeID=" + SchemeID + "&BuyShare=" + Share + "&ShareMoney=" + ShareMoney + "&LotteryID=" + LotteryID,
        beforeSend: function () { $("#divload").show(); $("#SchemeList").hide(); }, //发送数据之前
        complete: function () { $("#divload").hide(); $("#SchemeList").show() }, //接收数据完毕
        success: function (json) {
            if (!$("#hidLotteryID").val()) {
                InitData(0);
            }
            else {
                InitData(LotteryID);
            }           
            msg(json.message);
        }
    });
}

//合买份数递减
function MReduction(id) {
    var Share = parseInt($("#tbShare_" + id).val());
    if (Share - 1 <= 0) {
        $("#tbShare_" + id).val(1);

        return;
    }

    $("#tbShare_" + id).val((Share - 1).toString());
}

//合买份数递增
function MAddition(id) {
    var Share = parseInt($("#tbShare_" + id).val());
    var Surplus = parseInt($("#spanSurplus_" + id).text());

    if (Share + 1 > Surplus) {
        $("#tbShare_" + id).val(Surplus);

        return;
    }

    $("#tbShare_" + id).val(parseInt(Share + 1).toString());
}
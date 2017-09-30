
$(function ()
{

    var oBtn = document.getElementById('btn');
    var oMask = document.getElementById('mask');
    var oBox = document.getElementById('box');
    var oClose = document.getElementById('close');
    var oReSubmit = document.getElementById('ReSubmit');

    var Hidcommission = $("#Hidcommission").val();
    var HiOpt_InitiateSchemeMinBuyAndAssureScale = $("#HiOpt_InitiateSchemeMinBuyAndAssureScale").val();

    $("#ul_cway_list li").each(function ()
    {
        if ($(this).val() == Hidcommission)
        {
            $("#ul_cway_list li").removeClass("on").removeAttr("checked");
            $(this).addClass("on").attr("checked", "checked");
        }
    });

    $("#ul_cway_list li").each(function (i)
    {
        $(this).on("click", function ()
        {
            $("#ul_cway_list li").removeClass("on").removeAttr("checked");
            $(this).addClass("on").attr("checked", "checked");
        });
    });

    $("#btn").click(function ()
    {
        var summoney = $("#SumMoney").text();
        var totalShare = summoney;
        var buyshare = Math.ceil(HiOpt_InitiateSchemeMinBuyAndAssureScale * totalShare);
        var onemoney = summoney / totalShare;
        var assureshare = totalShare - buyshare;

        $("#isHe").val(1);
        $("#totalShare").val(totalShare);
        $("#totalShare + span").children("em").text(onemoney);
        $("#buyshare").val(buyshare);
        $("#buyshare + span").children("em").text(buyshare * onemoney);
        $("#minbuy").text(buyshare * onemoney);
        $("#assureshare").val(0);
        $("#assureshare + span").children("em").text(0);
        $("#maxassure").text(assureshare * onemoney);

        var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
        var scrollLeft = document.documentElement.scrollLeft || document.body.scrollLeft;

        //遮罩层
        oMask.style.display = 'block';

        oMask.style.width = Math.max(document.body.offsetWidth, document.documentElement.clientWidth) + 'px';
        oMask.style.height = Math.max(document.body.offsetHeight, document.documentElement.clientHeight) + 'px';

        //弹出层
        oBox.style.display = 'block';
        oBox.style.left = (document.documentElement.clientWidth - oBox.offsetWidth) / 2 + scrollLeft + 'px';
        oBox.style.top = (document.documentElement.clientHeight - oBox.offsetHeight) / 2 + scrollTop + 'px';
    })

    $("#close").click(function ()
    {
        oMask.style.display = 'none';
        oBox.style.display = 'none';
        $("#isHe").val(0);
        $("#totalShare").val(1);
        $("#buyshare").val(1);
        $("#assureshare").val(0);
        $("#ul_cway_list:checked").val(0);
        $("#cbuy-list:checked").val(0);
    })

    $("#ReSubmit").click(function ()
    {
        oMask.style.display = 'none';
        oBox.style.display = 'none';
        $("#isHe").val(0);
        $("#totalShare").val(1);
        $("#buyshare").val(1);
        $("#assureshare").val(0);
        $("#ul_cway_list:checked").val(0);
        $("#cbuy-list:checked").val(0);
    })

    $(".item_hemai_cnt input:gt(1)").keypress(function ()
    {
        //控制文本框只能输入数字
        if (window.event.keyCode < 48 || window.event.keyCode > 57)
            return false;
        return true;
    }).change(function ()
    {
        var text = $(this).val();
        var patrn = /^[0-9]*[1-9][0-9]*$/; ///^\d+$/g
        if ($(this).attr("id") == "assureshare")
            patrn = /^[0-9]*[0-9][0-9]*$/; ///^\d+$/g
        if (!patrn.test(text))
        {
//            $(this).val(1);
        }

    }).keyup(function ()
    {
        BuyCalculation();
    });

    window.onscroll = function ()
    {

        if (oBox.style.display == 'none') return;

        var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
        var scrollLeft = document.documentElement.scrollLeft || document.body.scrollLeft;

        oBox.style.left = (document.documentElement.clientWidth - oBox.offsetWidth) / 2 + scrollLeft + 'px';
        oBox.style.top = (document.documentElement.clientHeight - oBox.offsetHeight) / 2 + scrollTop + 'px';

    }

    window.onresize = function ()
    {
        var oMask = document.getElementById('mask');
        if (oMask.style.display == 'none') return;

        oMask.style.width = Math.max(document.body.offsetWidth, document.documentElement.clientWidth) + 'px';
        oMask.style.height = Math.max(document.body.offsetHeight, document.documentElement.clientHeight) + 'px';

        if (oBox.style.display == 'none') return;

        var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
        var scrollLeft = document.documentElement.scrollLeft || document.body.scrollLeft;

        oBox.style.left = (document.documentElement.clientWidth - oBox.offsetWidth) / 2 + scrollLeft + 'px';
        oBox.style.top = (document.documentElement.clientHeight - oBox.offsetHeight) / 2 + scrollTop + 'px';

    }

});

function BuyCalculation()
{
    var HiOpt_InitiateSchemeMinBuyAndAssureScale = $("#HiOpt_InitiateSchemeMinBuyAndAssureScale").val();

    var summoney = parseInt($("#SumMoney").text()); //总金额

    var totalShare = parseInt($("#totalShare").val()); //分成多少份

    var buyshare = parseInt($("#buyshare").val()); //我要认购多少份

    var assureshare = parseInt($("#assureshare").val()); //我要保底多少份

    if (isNaN(assureshare)) assureshare = 0;

    $("#assureshare").val(assureshare);

    assureshare = parseInt($("#assureshare").val());

    totalShare = summoney % totalShare != 0 ? summoney : totalShare;

    var minBuyshare = Math.ceil(HiOpt_InitiateSchemeMinBuyAndAssureScale * totalShare); //至少购买多少份

    if (isNaN(buyshare))
    {
        buyshare = minBuyshare;
        $("#buyshare").val(buyshare);
    }

    if (buyshare > totalShare)
    {
        $("#buyshare").val(totalShare);
        buyshare = parseInt($("#buyshare").val());  //我要认购多少份
    }

    var maxAssureshare = totalShare - buyshare; //最多保底多少份

    var onemoney = summoney / totalShare; //每份多少钱

    if (assureshare > maxAssureshare)
    {
        $("#assureshare").val(maxAssureshare);
        assureshare = parseInt($("#assureshare").val());
    }

    $("#totalShare").val(totalShare);
    $("#totalShare + span").children("em").text(onemoney);
    $("#buyshare + span").children("em").text(buyshare * onemoney);
    $("#minbuy").text(minBuyshare);
    $("#assureshare + span").children("em").text(assureshare * onemoney);
    $("#maxassure").text(maxAssureshare * onemoney);
}
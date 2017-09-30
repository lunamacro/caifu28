function openDingZhiGenDan(url) {

    var html = '<div id="pop" style="display:none;z-index:99998;background: rgba(150, 150, 150, 0.3); position:absolute;top:0px;"></div>\
                    <div id="qihoo-pop" style="display:none;overflow:hidden;z-index:999999;width:450px;height:260px;position:fixed;top:200px;left:460px;border:2px solid #BB4150;border-radius: 5px;background:white; -webkit-box-shadow: 0 0 10px 0 #A1424F;box-shadow: 0 0 10px 0 #A1424F;">\
                      <iframe  src="' + url + '" id="dzgdFrame" name="dzgdFrame" scrolling="no" frameborder="0" width="1010px" height="100%" ></iframe>\
                      <div style="z-index:9999999;color:#DA3828;position:relative; top:-258px; left:430px;font-weight:bold;cursor:pointer; font-size:20px;" title="关闭" onclick="closeDZGDPopupLayer()">X</div>\
                    </div>';
    $("#pop").remove();
    $("#qihoo-pop").remove();
    $(document.body).append(html);
    $("#pop").fadeIn(200);
    $("#qihoo-pop").fadeIn(500);
    $("#pop").width("100%");
    $("#pop").height($(document).height());
    $("#qihoo-pop").css("left", ($(document).width() - $("#qihoo-pop").width()) / 2);
    $(window).resize(function () {
        $("#pop").height($(document).height());
        $("#qihoo-pop").css("left", ($(document).width() - $("#qihoo-pop").width()) / 2);
    });
}

/*
*   关闭定制跟单弹出层
*/
function closeDZGDPopupLayer() {
    $("#qihoo-pop").fadeOut(200);
    $("#pop").fadeOut(500, function () {
        location.reload();
    });
}
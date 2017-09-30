var AjaxUrl = "../../ajax/Admin_Handler.ashx";

var GetSfcHTML = function () {
    var _obj = {};
    _obj.LotteryID = $("#tbLotteryID").val();
    _obj.IsuseName = $("#tbIsuseName").find("option:selected").text();
    Ajax6636(AjaxUrl, "GetSfcHTML", _obj, "true",
        function (data, textStatus) {
            if (data == "false") {
                alert("抓取数据出现异常！");
                return;
            }
            var list = fw.json.parse(data); 
            BindText(list, $("#tbLotteryID").val(), false);
        });
}

var GetSfcData = function () {
    var _obj = {};
    _obj.LotteryID = $("#tbLotteryID").val();
    _obj.IsuseName = $("#tbIsuseName").val().split(":")[0];
    
    Ajax6636(AjaxUrl, "GetSfcData", _obj, "true",
        function (data, textStatus) {
            if (data == "false") {
                BindText(null, $("#tbLotteryID").val(), true);
                return;
            }
            var model = fw.json.parse(data); 
            $("#txtStartTime").val(model.Starttime);
            $("#txtEndTime").val(model.Endtime);
            if ($("#tbLotteryID").val() == "2")
                BindTextJQC(model.List);
            else
                BindText(model.List, $("#tbLotteryID").val(), false);
        });
    }

var BindTextJQC = function (list) {
    for (var i = 0; i < list.length; i++) {
        if ($("#txtHostTeam" + (i / 2 + 1)))
            $("#txtHostTeam" + (i / 2 + 1)).val(list[i].Hometeam);
        if ($("#txtDateTime" + (i / 2 + 1)))
            $("#txtDateTime" + (i / 2 + 1)).val(list[i].Matchtime);
        if ($("#txtQuestTeam" + (i / 2 + 1)))
            $("#txtQuestTeam" + (i / 2 + 1)).val(list[i + 1].Hometeam);
        i++;
    }
}

var BindText = function (list, lotteryID, isClear) {
    if (lotteryID == "74") {
        if (isClear) {
            for (var i = 0; i < 14; i++) {
                if ($("#txtLeagueName" + (i + 1)))
                    $("#txtLeagueName" + (i + 1)).val("");
                if ($("#txtHostTeam" + (i + 1)))
                    $("#txtHostTeam" + (i + 1)).val("");
                if ($("#txtQuestTeam" + (i + 1)))
                    $("#txtQuestTeam" + (i + 1)).val("");
                if ($("#txtDateTime" + (i + 1)))
                    $("#txtDateTime" + (i + 1)).val("");
            }
        }
        else
            for (var i = 0; i < list.length; i++) {
                if ($("#txtLeagueName" + (i + 1)))
                    $("#txtLeagueName" + (i + 1)).val(list[i].Leaguename);
                if ($("#txtHostTeam" + (i + 1)))
                    $("#txtHostTeam" + (i + 1)).val(list[i].Hometeam);
                if ($("#txtQuestTeam" + (i + 1)))
                    $("#txtQuestTeam" + (i + 1)).val(list[i].Awayteam);
                if ($("#txtDateTime" + (i + 1)))
                    $("#txtDateTime" + (i + 1)).val(list[i].Matchtime);
            }
        }
        else if (lotteryID == "15") {
            if (isClear) {
                for (var i = 0; i < 7; i++) {
                    if ($("#txtHostTeam" + (i + 1)))
                        $("#txtHostTeam" + (i + 1)).val("");
                    if ($("#txtQuestTeam" + (i + 1)))
                        $("#txtQuestTeam" + (i + 1)).val("");
                    if ($("#txtDateTime" + (i + 1)))
                        $("#txtDateTime" + (i + 1)).val("");
                }
            }
            else
                for (var i = 0; i < list.length; i++) {
                    if ($("#txtHostTeam" + (i + 1)))
                        $("#txtHostTeam" + (i + 1)).val(list[i].Hometeam);
                    if ($("#txtQuestTeam" + (i + 1)))
                        $("#txtQuestTeam" + (i + 1)).val(list[i].Awayteam);
                    if ($("#txtDateTime" + (i + 1)))
                        $("#txtDateTime" + (i + 1)).val(list[i].Matchtime);
                }
            }
            else if (lotteryID == "2") {
                if (isClear) {
                    for (var i = 0; i < 5; i++) {
                        if ($("#txtHostTeam" + (i + 1)))
                            $("#txtHostTeam" + (i + 1)).val("");
                        if ($("#txtQuestTeam" + (i + 1)))
                            $("#txtQuestTeam" + (i + 1)).val("");
                        if ($("#txtDateTime" + (i + 1)))
                            $("#txtDateTime" + (i + 1)).val("");
                    }
                }
                else
                    for (var i = 0; i < list.length; i++) {
                        if ($("#txtHostTeam" + (i + 1)))
                            $("#txtHostTeam" + (i + 1)).val(list[i].Hometeam);
                        if ($("#txtQuestTeam" + (i + 1)))
                            $("#txtQuestTeam" + (i + 1)).val(list[i].Awayteam);
                        if ($("#txtDateTime" + (i + 1)))
                            $("#txtDateTime" + (i + 1)).val(list[i].Matchtime);
                    }
            }
            }

            var formSubmit = function () {
                //setTimeout(function () { document.getElementById('form1').submit(); }, 0);
                $('#form1').submit();
                return false;
            }

$(window).ready(function () {
    SchemeMonitor.FirstLoad();
});
/*
*   方案监控JS
*/
var SchemeMonitor = {
    ObjID: "#SchemeMonitor",
    IsFirstLoad: true,
    IsShow: true,
    SetCookeName: "SchemeMonitorIsShow",
    OldHeight: 0,
    NewHeight: 72,
    ShowAnimateTime: 500,
    GetDataTimeout: 30 * 1000,
    IntervalTime: 30 * 1000

    /*
    *   生成HTML代码
    */
    , GenerateHtml: function () {
        var html = "";
        html += "<div class=\"SchemeMonitorContainer\" id=\"SchemeMonitor\">";
        html += "<div class=\"topbar\">";
        html += "    <i class=\"close\" style=\" display:none;\">关闭</i><i class=\"show\" style=\" display:none;\">显示</i> &nbsp;方案监控";
        html += "</div>";
        html += "<div class=\"data\">";
        html += "    <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">";
        html += "        <thead>";
        html += "            <tr>";
        html += "                <th class=\"lottery\">彩种</th>";
        html += "                <th class=\"currIssue\">当前期</th>";
        html += "                <th class=\"noPrintOutSchemeCount\">未出票数</th>";
        html += "                <th class=\"fullNoPrintOutSchemeCount\">已满员未出票数</th>";
        html += "                <th class=\"noOpenIssueCount\">本期之前未开奖期数</th>";
        html += "           </tr>";
        html += "        </thead>";
        html += "        <tbody>";
        html += "            <tr class=\"firstLoad\">";
        html += "                <td style=\"border-right: 0px;\" colspan=\"5\">";
        html += "                    <span></span>拼命获取数据中......<img src=\"/Images/Admin/loading3.gif\" />";
        html += "                </td>";
        html += "            </tr>";
        html += "        </tbody>";
        html += "    </table>";
        html += "    <div class=\"masking\" id=\"masking\"></div>";
        html += "    <div class=\"loading\" id=\"loading\"><img src=\"/Images/loading.gif\" /></div>";
        html += "</div>";
        html += "</div>";
        $(window.document.body).append(html);
    }

    /*
    *   第一次加载
    */
    , FirstLoad: function () {
        SchemeMonitor.GenerateHtml();
        SchemeMonitor.RegisterEvent();
        SchemeMonitor.IsShow = SchemeMonitor.GetSet(SchemeMonitor.SetCookeName);
        if (null == SchemeMonitor.IsShow || "" == SchemeMonitor.IsShow || true == SchemeMonitor.IsShow || "true" == SchemeMonitor.IsShow) {
            $(SchemeMonitor.ObjID).find(".topbar .close").show();
            SchemeMonitor.Show();
        } else {
            $(SchemeMonitor.ObjID).find(".topbar .show").show();
            SchemeMonitor.Hide();
        }
        SchemeMonitor.GetData();
    }

    /*
    *   注册事件
    */
    , RegisterEvent: function () {
        $(SchemeMonitor.ObjID).find(".topbar .close").bind("click", function () {
            $(this).hide();
            $(this).siblings().eq(0).show();
            SchemeMonitor.IsShow = false;
            SchemeMonitor.SaveSet();
            SchemeMonitor.Hide();

        });
        $(SchemeMonitor.ObjID).find(".topbar .show").bind("click", function () {
            $(this).hide();
            $(this).siblings().eq(0).show();
            SchemeMonitor.IsShow = true;
            SchemeMonitor.SaveSet();
            SchemeMonitor.Show();

        });
    }

    /*
    *   定时获取
    */
    , SchemeMonitorSetInterval: function () {
        setInterval(function () {
            SchemeMonitor.GetData();
        }, SchemeMonitor.IntervalTime);
    }

    /*
    *   从服务器获取数据
    */
    , GetData: function () {
        if (!SchemeMonitor.IsFirstLoad) {
            SchemeMonitor.ShowLoading();
        }
        $.ajax({
            type: "get",
            url: "/ajax/SchemeMonitor.ashx",
            cache: false,
            async: true,
            timeout: SchemeMonitor.GetDataTimeout,
            dataType: "json",
            success: function (result) {
                SchemeMonitor.RequestSuccessCallback(result);
                SchemeMonitor.HideLoading();
            },
            error: function (x, s, t) {
                SchemeMonitor.RequestFailCallback();
            }
        });
    }

    /*
    *   刷新,重新获取数据
    */
    , Refresh: function () {
        $(SchemeMonitor.ObjID).find(".data table tbody").html("<tr class=\"firstLoad\"><td style=\"border-right: 0px;\" colspan=\"5\"><span></span>拼命获取数据中......<img src=\"/Images/Admin/loading3.gif\" /></td></tr>");
        SchemeMonitor.GetData();
    }

    /*
    *   显示方案监控
    */
    , Show: function () {
        $(SchemeMonitor.ObjID).find(".data").show().css("height", SchemeMonitor.OldHeight).stop().animate({ "height": SchemeMonitor.NewHeight }, SchemeMonitor.ShowAnimateTime);
    }

    /*
    *   隐藏方案监控
    */
    , Hide: function () {
        SchemeMonitor.OldHeight = 0;
        $(SchemeMonitor.ObjID).find(".data").animate({ "height": SchemeMonitor.OldHeight }, SchemeMonitor.ShowAnimateTime);
    }

    /*
    *   从服务器获取到数据之后的回调
    */
    , RequestSuccessCallback: function (result) {
        if (SchemeMonitor.IsShow == false) {
            SchemeMonitor.OldHeight = 0;
        } else {
            SchemeMonitor.OldHeight = $(SchemeMonitor.ObjID).find(".data table tbody").innerHeight() + $(SchemeMonitor.ObjID).find(".topbar").height() - 4;
        }

        /*-----------------------------------------*/
        /* 拼接生成Table
        /*-----------------------------------------*/
        var html = "";
        var tr = "<tr><td class=\"lottery\">[lottery]</td><td class=\"currIssue\">[currIssue]</td><td class=\"noPrintOutSchemeCount\">[noPrintOutSchemeCount]</td><td class='fullNoPrintOutSchemeCount'>[FullNoPrintOutSchemeCount]</td><td class=\"noOpenIssueCount\">[noOpenIssueCount]</td></tr>";
        if (parseFloat(result.error) == 0) {
            var len = result.Data.length;
            for (var i = 0; i < len; i++) {
                var issue = result.Data[i];
                html += (tr.replace("[lottery]", issue.LotteryName).replace("[currIssue]", issue.IssueName).replace("[noPrintOutSchemeCount]", issue.NoPrintOutSchemeCount)
                        .replace("[FullNoPrintOutSchemeCount]", issue.FullNoPrintOutSchemeCount).replace("[noOpenIssueCount]", issue.NoOpenIssueCount));
            }
            $(SchemeMonitor.ObjID).find(".data table tbody").html(html);
        } else {
            SchemeMonitor.RequestFailCallback();
        }

        SchemeMonitor.NewHeight = $(SchemeMonitor.ObjID).find(".data table tbody").innerHeight() + $(SchemeMonitor.ObjID).find(".topbar").height() - 4;

        if ("true" == SchemeMonitor.IsShow || true == SchemeMonitor.IsShow) {
            SchemeMonitor.Show();
        }
        //第一次加载完成后设置定时器
        if (SchemeMonitor.IsFirstLoad) {
            SchemeMonitor.IsFirstLoad = false;
            SchemeMonitor.SchemeMonitorSetInterval();
        }
    }

    /*
    *    从服务器获取数据失败回调
    */
    , RequestFailCallback: function () {
        $(SchemeMonitor.ObjID).find(".data table tbody").html("<tr style='height:45px;'><td colspan=\"5\">服务器繁忙&nbsp;&nbsp;<a href='javascript:;' onclick='SchemeMonitor.Refresh()'>刷新</a></td></tr>");
    }

    /*
    *   显示Loading动画
    */
    , ShowLoading: function () {
        var masking = $(SchemeMonitor.ObjID).find("#masking");
        var loading = $(SchemeMonitor.ObjID).find("#loading");
        var width = $(SchemeMonitor.ObjID).width();
        var height = $(SchemeMonitor.ObjID).find(".data table tbody").innerHeight() + $(SchemeMonitor.ObjID).find(".topbar").height();
        masking.css({ "width": width, "height": height, "display": "block" });
        loading.css({ "left": (width - loading.width()) / 2, "top": (height - loading.height()) / 2, "display": "block" });
    }

    /*
    *   隐藏loading动画
    */
    , HideLoading: function () {
        var masking = $(SchemeMonitor.ObjID).find("#masking");
        var loading = $(SchemeMonitor.ObjID).find("#loading");
        masking.hide();
        loading.hide();
    }

    /*
    从cookie中获取设置
    */
    , GetSet: function (name) {
        var cookie = document.cookie;
        if ("" == cookie) {
            return "";
        }
        //获得所有的cookies
        var cookieArray = cookie.split(";");
        if (cookieArray.length < 1) {
            return "";
        }
        //循环所有的cookies
        for (var i = 0; i < cookieArray.length; i++) {
            //name=value
            var tempCookie = cookieArray[i];
            //得到键和值
            var tempArray = tempCookie.split("=");
            //cookie名称
            var tempName = tempArray[0];
            if (name == tempName) {
                i = cookieArray.length;
                return tempArray[1];
            }
        }
        return "false";
    }

    /**
    保存设置(是否显示)
    */
    , SaveSet: function () {
        document.cookie = (SchemeMonitor.SetCookeName + "=" + SchemeMonitor.IsShow);
    }
};
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="News.aspx.cs" Inherits="CPS_News" %>

<%@ Register Src="userControls/IndexHeader.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="userControls/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%=_Site.Name %>-新闻公告/推广指南</title>
    <meta name="description" content="<%=_Site.Name %>-新闻公告/推广指南" />
    <meta name="keywords" content="<%=_Site.Name %>-新闻公告/推广指南" />
    <link type="text/css" href="css/common.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Header ID="Header1" runat="server" />
    <div class="inside_banner news_banner">
        <h3>新闻公告<span>News</span></h3>
    </div>
    <div class="content">
        <div class="inside_bg" style=" height:500px;">
            <div class="insicon">
                <div class="sidebar">
                    <ul id="typeList">
                        <li runat="server" id="li_xwgg"><a href="News.aspx?NewType=xwgg">新闻公告</a></li>
                        <li runat="server" id="li_tgzn"><a href="News.aspx?NewType=tgzn">推广指南</a></li>
                    </ul>
                </div>
                <div class="commain">
                    <ul class="news">
                        <asp:Repeater runat="server" ID="rpt_list">
                            <ItemTemplate>
                                <li><span class="newstime">
                                    <%#FormatDateTime(Eval("DateTime").ToString())%></span><a href='New_View.aspx?ID=<%#Eval("ID") %>&NewType=<%=newType %>'
                                        target="_blank"><%#Eval("Title") %></a></li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    <div id="sand" style="text-align: right; padding-right: 30px;" runat="server">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <uc2:Footer ID="Footer2" runat="server" />
    </form>
</body>

<link href="../../Style/sandPage.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>
<script type="text/javascript" src="../../JScript/sandPage.js"></script>
<script type="text/javascript">
    $("#typeList li").click(function () {
        $("#typeList li").removeClass("curr");
        $(this).addClass("curr");
    });
</script>
<script type="text/javascript">

    var pageIndex = "<%=PageIndex %>";
    var pageCount = "<%=PageCount %>";
    var dataCount = "<%=DataCount %>";
    var newType = "<%=newType %>";

    $(function () {
        var totalPage = pageCount;
        var totalRecords = dataCount;
        var pageNo = pageIndex;
        var parameter = "?NewType=" + newType;
        //初始化分页控件
        //有些参数是可选的，比如lang，若不传有默认值
        sand.init({
            pno: pageNo,
            //总页码
            total: totalPage,
            //总数据条数
            totalRecords: totalRecords,
            //链接前部
            hrefFormer: 'News',
            //链接尾部
            hrefLatter: '.aspx',
            getLink: function (n) {
                return this.hrefFormer + this.hrefLatter + parameter + "&PageIndex=" + n;
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
        $("#sand_btn_go_input").attr("onkeypress", "");
        $("#sand_btn_go_input").keyup(function () {
            var value = parseFloat($(this).val());
            if (isNaN(value)) {
                value = 1;
            }
            $(this).val(value);
        });
    });
</script>
</html>

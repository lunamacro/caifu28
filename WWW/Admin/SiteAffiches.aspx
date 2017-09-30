<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SiteAffiches.aspx.cs" Inherits="Admin_SiteAffiches" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>站点公告</title>
    
    <link href="../../Style/sandPage.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
    <link href="../Style/common.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/common.js" type="text/javascript"></script>
    <link href="../Style/jquery.mCustomScrollbar.css" rel="stylesheet" type="text/css" />
    <script src="../JScript/jquery.mCustomScrollbar.concat.min.js" type="text/javascript"></script>
    <script src="../JScript/sandPage.js" type="text/javascript"></script>
</head>
<body>
    <form id="form2" runat="server" action="SiteAffiches.aspx" method="post">
    <div class="content">
        <div class="main" id="main">
            <div class="maincon">
                <h2>
                    站点公告</h2>
                <div class="main_list">
                    <table width="100%" id="tab" class="newsTable" cellpadding="0" cellspacing="0">
                        <thead>
                            <tr>
                                <th class="time">
                                    时间
                                </th>
                                <th class="title">
                                    标题
                                </th>
                                <th class="isShow">
                                    显示
                                </th>
                                <th class="isRead">
                                    推荐
                                </th>
                                <th class="edit">
                                    编辑
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptSchemes" runat="server" EnableViewState="false" OnItemCommand="rptSchemes_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td class="time">
                                            <%#Eval("DateTime")%>
                                        </td>
                                        <td class="title">
                                            <%#Eval("Title")%>
                                        </td>
                                        <td class="isShow">
                                            <asp:CheckBox runat="server" ID="chk_isShow" Enabled="false" Checked='<%#Eval("isShow")%>' />
                                        </td>
                                        <td class="isRead">
                                            <asp:CheckBox runat="server" ID="isRead" Enabled="false" Checked='<%#Eval("isCommend")%>' />
                                        </td>
                                        <td class="edit">
                                            <asp:Button runat="server" CssClass="btnEdit" ID="Editt" CommandArgument='<%#Eval("ID")%>'
                                                CommandName="edit" Text="修改" />
                                            <asp:Button runat="server" ID="delt" CommandArgument='<%#Eval("ID")%>' CommandName="Del"
                                                OnClientClick="return confirm('确认删除该条公告吗？');" Text="删除" CssClass="btnEdit" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                    <div id="sand" class="send" style="text-align: right; padding-right: 30px;">
                    </div>
                    <div class="newspic_addbut">
                        <asp:Button runat="server" ID="btn" Text="" OnClick="btn_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" value="<%=PageIndex%>" id="PageIndex" name="PageIndex" />
    <input type="hidden" value="<%=PageCount%>" id="PageCount" name="PageCount" />
    <input type="hidden" value="<%=DataCount%>" id="DataCount" name="DataCount" />
    </form>
</body>
</html>
<script type="text/javascript">
    $(function ()
    {

        $("#tab tbody tr").find("td:gt(0)").css("border-left", "none");

    });
</script>
<script type="text/javascript">
    function thisSubmit(n)
    {
        $("#PageIndex").val(n);
        $("#form2").submit();
    }

    $(function ()
    {
        var totalPage = $("#PageCount").val();
        var totalRecords = $("#DataCount").val();
        var pageNo = $("#PageIndex").val();

        //            var parameter = "?radom=" + Math.random() + "&lotteryid=" + lotteryID + "&filter=" + filter + "&isuseid=" + isuseID + "&search=" + search + "&sort=" + sort + "&order=" + order;
        //            //初始化分页控件
        //有些参数是可选的，比如lang，若不传有默认值
        sand.init({
            pno: pageNo,
            //总页码
            total: totalPage,
            //总数据条数
            totalRecords: totalRecords,
            //链接前部
            //                hrefFormer: 'schemeall',
            //                //链接尾部
            //                hrefLatter: '.aspx',
            getLink: function (n)
            {

                return 'javascript:thisSubmit(' + n + ')';
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
    });
</script>
<script type="text/javascript">
    window.onload = function ()
    {
        SetTableRowColor();
    }
    function showtable()
    {
        var mainTable = document.getElementById("tab");
        var li = mainTable.getElementsByTagName("tr");
        for (var i = 1; i <= li.length - 1; i++)
        {
            li[i].style.backgroundColor = "transparent";
            li[i].onmouseover = function ()
            {

                this.style.backgroundColor = "#fefdde";
            }
            li[i].onmouseout = function ()
            {

                this.style.backgroundColor = "transparent";
                SetTableRowColor();
            }
        }
    }

    showtable();

    function SetTableRowColor()
    {
        $("#tab tr:odd").css("background-color", "#F3F8FE");
        $("#tab tr:even").css("background-color", "#F7F7F7");
    }


</script>

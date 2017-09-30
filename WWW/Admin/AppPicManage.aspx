<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppPicManage.aspx.cs" Inherits="Admin_AppPicManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>APP图片轮播管理</title>
    <link href="../../Style/sandPage.css" rel="stylesheet" type="text/css" />
    <link href="../Style/common.css" rel="stylesheet" type="text/css" />
    <link href="../Style/jquery.mCustomScrollbar.css" rel="stylesheet" type="text/css" />
    <link href="../JScript/showloading/showLoading.css" rel="stylesheet" type="text/css" />
    <link href="/JScript/artDialog/css/ui-dialog.css" rel="stylesheet" />
    <style type="text/css">
        .newsTable thead th {
            border: 1px solid #dfdfdf;
        }

        tr {
            height: 30px;
        }

        .newsTable tbody td {
            font-size: 12px;
            border: 1px solid #dfdfdf;
            border-top: 0px;
        }

        .newsTable tbody td {
            text-align: center;
        }

        .newsTable .time {
            width: 15%;
        }

        .newsTable .title {
            width: 10%;
        }

        .newsTable .isShow {
            width: 30%;
        }

        .newsTable .isRead {
            width: 10%;
        }

        .newsTable .edit {
            width: 20%;
            border-right: 1px solid #dfdfdf;
        }

        .newsTable .btnEdit {
            display: block;
            float: left;
            color: #3977C3;
            font-family: "微软雅黑";
            width: 70px;
            margin: 10px;
            height: 22px;
            line-height: 22px;
            text-align: center;
            border: 0px;
            text-decoration: underline;
            cursor: pointer;
            text-decoration: none;
            background: #C7E8FE;
        }

        .btnEdit:hover {
            background: #D6F7FE;
        }

        #btn {
            display: block;
            height: 32px;
            width: 82px;
            background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#C2E6FE), to(#C2E6FE));
            border: 0px;
            cursor: pointer;
        }

            #btn:hover {
                border: 1px solid #3977C3;
            }

        .taptable {
            padding-left: 55px;
        }

        .topbutton {
            text-align: right;
            padding-right: 250px;
        }

        .newsTable {
            margin-left: 50px;
            font-size: 12px;
        }
    </style>
</head>
<body>
    <div class="main">
        <div class="maincon">
            <h2>APP图片轮播管理</h2>
            <div class="userfeed_wrap">
                <form runat="server" id="form1" action="Handler/UploadFile.ashx" method="post">
                    <div style="width: 90%; margin: 5px auto 5px auto;">
                        上传新的图片：<input type="file" name="newFile" id="newFile" runat="server" accept=".png,.jpg,.jpeg,.gif,.bmp" />
                        <input type="button" value="上传" class="btnAdd" id="btnUpload" />
                        (图片的大小：宽：350px 高：150px)
                    </div>
                    <input type="hidden" value="App" id="uploadType" name="uploadType" />
                </form>
                <table cellspacing="0" cellpadding="0" border="0" class="taptable">
                    <tr>
                        <table width="90%" id="tab" class="newsTable" border="0" cellpadding="0" cellspacing="0">
                            <thead>
                                <tr>
                                    <th>序号
                                    </th>
                                    <th>图片
                                    </th>
                                    <th>图片名称
                                    </th>
                                    <th>图片大小(KB)
                                    </th>
                                    <th>操作
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptSchemes" runat="server" EnableViewState="false">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%#Convert.ToInt32(Eval("ID"))+1 %>
                                            </td>
                                            <td style="width: 300px;">
                                                <img src="<%#Eval("CurrentUrl")%>" title="<%#Eval("FileName")%>" style="max-width: 200px; max-height: 100px; margin: 5px; border: 1px #ccc solid;" />
                                            </td>
                                            <td>
                                                <%#Eval("FileName")%>
                                            </td>
                                            <td>
                                                <%#Eval("FileSize")%>
                                            </td>
                                            <td>
                                                <a href="#" onclick='delfile("<%#Eval("FileName")%>")'>删除</a>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <script src="../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="../JScript/public.js" type="text/javascript"></script>
    <script src="../JScript/jquery.mCustomScrollbar.concat.min.js" type="text/javascript"></script>
    <script src="../JScript/sandPage.js" type="text/javascript"></script>
    <script src="../JScript/showloading/jquery.showLoading.js" type="text/javascript"></script>
    <script src="/JScript/artDialog/dialog-min.js" type="text/jscript"></script>
    <script src="/JScript/artDialog/dialog-plus-min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#btnUpload").click(function () {
                if ($("#newFile").val() != "") {
                    $(".main").showLoading();
                    form1.submit();
                }
                else {
                    alert("请选择需要上传的图片");
                }
            });
        });
        function delfile(filename) {
            var okfunc = function () {
                $(".main").showLoading();
                window.location.href = "?delfile=" + filename;
            };
            var cancelfunc = function () { };
            confirm("确认删除此图片？", okfunc, cancelfunc);
        }

        window.onload = function () {

            SetTableRowColor();

        }


        function showtable() {
            var mainTable = document.getElementById("tab");
            var li = mainTable.getElementsByTagName("tr");
            for (var i = 1; i <= li.length - 1; i++) {
                li[i].style.backgroundColor = "transparent";
                li[i].onmouseover = function () {

                    this.style.backgroundColor = "#fefdde";
                }
                li[i].onmouseout = function () {

                    this.style.backgroundColor = "transparent";
                    SetTableRowColor();
                }
            }
        }


        showtable();

        function SetTableRowColor() {

            $("#tab tr:odd").css("background-color", "#F3F8FE");

            $("#tab tr:even").css("background-color", "#F7F7F7");

        }
    </script>
</body>
</html>

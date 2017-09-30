<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PasswordValid.aspx.cs" Inherits="PasswordValid" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="JScript/PaswordValid/css/style.css" rel="stylesheet" />
    <title></title>
    <style>
        body {
            font-size: 12px;
        }

        li {
            list-style: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ul>
            <li>
                <span><input type="password" id="Password_1" style="width: 250px;" /></span>
            </li>
        </ul>
    </form>
</body>
</html>
<script src="JScript/jquery-1.8.3.min.js"></script>
<script src="JScript/PaswordValid/PasswordValid.js"></script>
<script type="text/javascript">
    $("#Password_1").keyup(function () {
        // 初始化
        var service = new PasswrodValid();
        // 访问公有成员变量
        service.control = "Password_1";
        // 调用公有方法
        service.retrieve();
    });
</script>

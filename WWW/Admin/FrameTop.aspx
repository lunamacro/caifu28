<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrameTop.aspx.cs" Inherits="Admin_FrameTop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="../JScript/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="../JScript/common.js" type="text/javascript"></script>
    <script src="../JScript/jquery.mCustomScrollbar.concat.min.js" type="text/javascript"></script>
    <link href="../Style/common.css" rel="stylesheet" type="text/css" />
    <link href="../Style/jquery.mCustomScrollbar.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form2" runat="server">
        <div class="header">
            <div class="heade">
                <img src="/Images/logo.png" alt="" style="width: 193px; height: 59px;" />
                <div class="shortcut_menu">
                    欢迎你！<asp:Label runat="server" ID="name"></asp:Label><asp:LinkButton runat="server" OnClientClick="return confirm('确认要退出吗?')" OnClick="btnLoginOut_Click" ID="btnLoginOut" Text="【退出】"></asp:LinkButton>
                    &nbsp;<a href="../wx_gateway/html/" target="_top">进入网站首页 </a>
                </div>
            </div>
        </div>
    </form>

</body>
</html>
 <script src="../Admin\JavaScript/soundmanager2-jsmin.js" type="text/javascript"></script>
<script type="text/javascript">
    var url = "";
    $(function () {
        url = this.URL
        setInterval(promptmusic, 6200);
    })

    function promptmusic() {

        $.ajax({
            url: 'Handler/PromptMusic.ashx',
            type: 'POST',
            data: { 'urls': url },
            dataType: 'text',
            async: false,
            error: function () {

            },
            success: function (data) {
                var result = data.split(',');
                if (result[0] >0) {
                    soundManager.onready(function (status) {
                        soundManager.play('sound_id', 'JavaScript/tsyy.wav');
                    });
                }
                if (result[1] > 0) {
                    soundManager.onready(function (status) {
                        soundManager.play('sound_id', 'JavaScript/yiyo.wav');
                    });
                }
            }
        });
    }
</script>


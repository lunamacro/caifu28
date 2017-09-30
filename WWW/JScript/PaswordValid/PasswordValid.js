PasswrodValid = function () {
    //自身的指针
    var me = this;
    //公有变量
    me.control = "";
    me.controlMarginLeft = "";
    me.msgMarginLeft = "";
    // 公有方法
    me.retrieve = function () {
        return processDisplayData();
    };
    //私有变量
    var ajaxId = "introducerService";
    //私有方法
    function processDisplayData() {
        var obj = $("#" + me.control);
        var password = obj.val();
        var html = "<span id='passwordIsOk_" + me.control + "' style='display:block;text-algin:center;height:30px;width: 186px;margin-left: " + me.controlMarginLeft + ";margin-right: auto;'><span id='valid_r_" + me.control + "' class='validStr'>弱</span><span id='valid_z_" + me.control + "' class='validStr'>中</span><span id='valid_q_" + me.control + "' class='validStr'>强</span></span>";
        //检验密码强度
        if ($("#passwordIsOk_" + me.control).length <= 0) {
            $(obj).parent().after(html);
        }
        //弱密码
        if (password.length < 6 || password.noRepeatStr().length < 3 || !ruoCheck(password)) {
            ruoMima();
        }
            //强密码
        else if (password.length >= 6 && password.noRepeatStr().length >= 3 && isContainSpecial(password) && checkStr(password) && checkNum(password)) {
            qiangMima();
        }
            //中密码
        else {
            zhongMima();
        }
        //提示内容
        if (password.length < 6) {
            showMsg(obj, "请输入6-16位数字、字母或常用符号，字母区分大小写");
            return false;
        }
        if (password.length > 16) {
            showMsg(obj, "请输入6-16位数字、字母或常用符号，字母区分大小写");
            return false;
        }
        if (password.noRepeatStr().length < 3 || !ruoCheck(password)) {
            showMsg(obj, "您输入的密码强度过弱，请重新输入，试试字母、数字、常用符号的组合");
            return false;
        }
        else {
            successFunc(obj);
            return true;
        }
    }
    //枚举弱密码检测函数
    function ruoCheck(pwd) {
        if (!isContainSpecial(pwd) && !checkStr(pwd)) {
            for (var i = 0; i < pwd.length - 1; i++) {
                var result = parseInt(pwd[i]) - parseInt(pwd[i + 1]);
                if (result == -1 || result == 1) {
                    continue;
                }
                else {
                    return true;
                }
            }
            return false;
        }
        return true;
    }
    //弱密码
    function ruoMima() {
        $("#valid_r_" + me.control).addClass("validRed");
        $("#valid_z_" + me.control).removeClass("validRed");
        $("#valid_q_" + me.control).removeClass("validRed");
    }
    //中等密码
    function zhongMima() {
        $("#valid_z_" + me.control).addClass("validRed");
        $("#valid_r_" + me.control).removeClass("validRed");
        $("#valid_q_" + me.control).removeClass("validRed");
    }
    //强密码
    function qiangMima() {
        $("#valid_q_" + me.control).addClass("validRed");
        $("#valid_r_" + me.control).removeClass("validRed");
        $("#valid_z_" + me.control).removeClass("validRed");
    }
    //显示错误信息
    function showMsg(obj, msg) {
        if ($("#validMsg_" + me.control).length <= 0) {
            $(obj).parent().after("<span id='validMsg_" + me.control + "' style='display:block;text-align:left;margin-left: " + me.msgMarginLeft + ";margin-right: auto;color:red;'></span>");
        }
        $("#validMsg_" + me.control).css("width", $(obj).width() + "px");
        $("#validMsg_" + me.control).html(msg);
        $(obj).addClass("validError");
        $(obj).removeClass("validRight");
    }
    //显示正确信息
    function successFunc(obj) {
        $("#validMsg_" + me.control).html("");
        $(obj).addClass("validRight");
        $(obj).removeClass("validError");
    }
    //检验字符串中是否包含特殊字符
    function isContainSpecial(str) {
        var specialStr = /\~|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\_|\-|\=|\+|\\|\||\[|\]|\{|\}|\;|\'|\:|\"|\,|\.|\/|\<|\>|\?/g;
        var arr = str.match(specialStr);
        if (arr == null) {
            return false;
        }
        if (arr.length > 0) {
            return true;
        }
        return false;
    }
    //检验是否包含英文字母
    function checkStr(value) {
        var re = new RegExp("[a-zA-Z]");
        var len = re.test(value);
        return len;
    }
    //检验是否包含数字
    function checkNum(value) {
        var re = new RegExp("[0-9]");
        var len = re.test(value);
        return len;
    }
    //过滤重复字符
    String.prototype.noRepeatStr = function () {
        var tempArr = new Array();
        for (var i = 0; i < this.length; i++) {
            if (tempArr.join('').indexOf(this.charAt(i)) == -1)
                tempArr[tempArr.length] = this.charAt(i);
        }
        return tempArr.join('');
    }
};
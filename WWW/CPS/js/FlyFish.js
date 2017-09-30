
/*
*	常用js操作
*/
var FlyFish = {
    UserNameReg: /^[_0-9a-zA-Z\u4e00-\u9fa5]{1,18}$/, //用户名正则表达式    中文、_ 、数字、大小写字母组成
    UserPwdReg: /^[_0-9a-zA-Z]{1,18}$/,               //密码正则表达式      _ 、数字、大小写字母组成
    MobileReg: /1[0-9]{10}/,                          //手机号码正则表达式  1开头后面接10位数字
    /*
    *	获得URL参数
    *	返回参数 如果URL后面不带参数就返回NULL
    */
    GetUrlParams: function () {
        var url = window.location.href;
        var index = url.indexOf("?");
        if (index == -1) {
            return null;
        }
        var paramStr = url.substr(index + 1, url.length - index);
        var paramArray = paramStr.split("&");
        var array = new Array();
        for (var i = 0; i < paramArray.length; i++) {
            var temp = paramArray[i].split("=");
            array[temp[0]] = temp[1];
        }
        return array;
    },

    /*
    *   oldString       原来的字符串
    *	replaceString	需要替换的字符串
    *	newString		将需要替换的字符串替换成新的字符串
    *	返回替换后的新字符串
    */
    StringReplace: function (oldString, replaceString, newString) {
        return oldString.replace(new RegExp(replaceString, "g"), newString);
    },

    /*
    *   去除空格
    */
    Trim: function (oldString) {
        return (oldString + "").replace(/[\s]/g, "");
    },

    /*
    *   检查是否是用户名
    *   controlID   控件ID
    *   返回值：    0 正确，-1 ID控件不存在, 1 没有输入值，2表示用户名不符合规则
    */
    CheckIsUserName: function (controlID) {
        var userName = $("#" + controlID).val();
        if ("undefined" == typeof (userName)) {
            return -1;
        }
        userName = FlyFish.Trim(userName);
        if ("" == userName || 0 == userName) {
            return 1;
        }
        if (!FlyFish.UserNameReg.test(userName)) {
            return 2;
        }
        return 0;
    }

    /*
    *   检查是否是用户密码
    *   controlID   控件ID
    *   返回值：    0 正确，-1 ID控件不存在, 1 没有输入值，2表示用户名不符合规则
    */
    , CheckIsUserPwd: function (controlID) {
        var userPwd = $("#" + controlID).val();
        if ("undefined" == typeof (userPwd)) {
            return -1;
        }
        userPwd = FlyFish.Trim(userPwd);
        if ("" == userPwd || 0 == userPwd) {
            return 1;
        }
        if (!FlyFish.UserPwdReg.test(userPwd)) {
            return 2;
        }
        return 0;
    }

    /*
    *   检查是否是手机号码
    */
    , CheckIsMobile: function (mobile) {
        if ("undefined" == mobile) {
            return -1;
        }
        mobile = FlyFish.Trim(mobile);
        if ("" == mobile || 0 == mobile) {
            return 1;
        }
        if (!FlyFish.MobileReg.test(mobile)) {
            return 2;
        }
        return 0;
    }

    /*
    *   控制不能输入空格
    */
    , NotInputSpace: function () {
        var val = $(this).val();
        $(this).val(FlyFish.Trim(val));
    }

    /*
    *   输入int
    */
    , InputInt: function (e) {
        var val = $(this).val();
        $(this).val(val.replace(/\D+/, ""));
    }

    /*
    *   是否是float
    */
    , IsFloat: function (val) {
        var reg = /^\d+((\.)?\d+)?$/;
        return reg.test(val);
    }
};

/*
*   验证类
*/
var Validate = {
    IntReg: "^(-)?\d+$",

    /*
    *   是否是int数值，可以是正整数和负数（如：1、-1不能出现小数点）
    */
    IsInt: function (val) {
        var temp = parseFloat(val);
        if (isNaN(temp)) return false;
        return new RegExp(Validate.IntReg).test(temp);
    }
};
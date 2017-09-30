/*
*   登陆
*/
var Login = {
    LoginNameID: "txtName",
    LoginPwdID: "txtPwd",
    /*
    *   初始化
    */
    Init: function () {
//        $("#" + Login.LoginNameID).val("");
//        $("#" + Login.LoginPwdID).val("");
        /*
        *   登陆按钮
        */
        $("#btnLogin").bind("click", function () {
            return Login.LoginFunction();
        });

        $("#" + Login.LoginNameID).bind("blur", function () {
            Login.CheckLoginName();
        });

//        $("#" + Login.LoginNameID).bind("keyup", FlyFish.NotInputSpace);
//        $("#" + Login.LoginPwdID).bind("keyup", FlyFish.NotInputSpace);

        $("#" + Login.LoginPwdID).bind("blur", function () {
            Login.CheckLoginPwd();
        });
    }
    /*
    *   登陆函数 FlyFish.Tirm函数在FlyFish.js文件中定义
    */
    , LoginFunction: function () {
        if (Login.CheckLoginName() && Login.CheckLoginPwd()) {
            return true;
        }
        return false;
    }
    /*
    *   检查登陆名
    *   loginName   用户名
    */
    , CheckLoginName: function () {
        var result = FlyFish.CheckIsUserName(Login.LoginNameID);
        switch (parseFloat(result)) {
            case 0:     //正确
                Login.HideLoginTip();
                return true;
            case -1:    //控件不存在
                Login.WriteLoginTip("请输入用户名");
                return false;
            case 1:     //没有输入文本
                Login.WriteLoginTip("请输入用户名");
                return false;
            case 2:     //用户名不符合规则
                Login.WriteLoginTip("用户名只能是 中文、_ 、数字、大小写字母组成");
                return false;
        }
        return true;
    }
    /*
    *   检查登陆密码
    */
    , CheckLoginPwd: function () {
        var result = FlyFish.CheckIsUserPwd(Login.LoginPwdID);
        switch (parseFloat(result)) {
            case 0:     //正确
                Login.HideLoginTip();
                return true;
            case -1:    //控件不存在
                Login.WriteLoginTip("请输入密码");
                return false;
            case 1:     //没有输入文本
                Login.WriteLoginTip("请输入密码");
                return false;
            //case 2:     //用户名不符合规则
            //    Login.WriteLoginTip("密码只能是 _ 、数字、大小写字母组成");
            //    return false;
        }
        return true;
    }
    /*
    *   写入登陆提示
    */
    , WriteLoginTip: function (tip) {
        var loginTip = $("#loginTip");
        loginTip.html(tip);
        loginTip.show();
    }
    , HideLoginTip: function () {
        var loginTip = $("#loginTip");
        loginTip.html("");
        //loginTip.hide();
    }
}

/*
*   注册
*/
var Register = {
    Type: "",
    LoginNameID: "txtName_",
    LoginPwdID: "txtPwd_",
    MobileID: "txtPhone_",
    CodeID: "txtCode_",
    ClearInputID: "clearInput_",
    SendVerifyCodeID: "btnSendVerifyCode_",
    RegisterID: "btnRegister_",
    TimeInterval: null,     //倒数计时器对象
    TimeIndex: 59,          //倒数计时器数字
    Init: function () {
//        $("#" + (Register.LoginNameID + Register.Type)).val("");
//        $("#" + (Register.LoginPwdID + Register.Type)).val("");
        //$("#" + (Register.LoginNameID + Register.Type)).focus();
        $("#" + (Register.LoginNameID + Register.Type)).bind("blur", function () {
            Register.CheckLoginName();
        })

        $("#" + (Register.LoginPwdID + Register.Type)).bind("blur", function () {
            Register.CheckLoginPwd();
        });


        $("#" + (Register.CodeID + Register.Type)).bind("blur", function () {
            Register.CheckCode();
        });

//        $("#" + (Register.LoginNameID + Register.Type)).bind("keyup", FlyFish.NotInputSpace);
//        $("#" + (Register.LoginPwdID + Register.Type)).bind("keyup", FlyFish.NotInputSpace);
//        $("#" + (Register.MobileID + Register.Type)).bind("keyup", FlyFish.InputInt);
//        $("#" + (Register.CodeID + Register.Type)).bind("keyup", FlyFish.InputInt);

        $("#" + (Register.ClearInputID + Register.Type)).bind("click", function () {
            Register.ClearInput();
        });

        $("#" + (Register.SendVerifyCodeID + Register.Type)).bind("click", function () {
            if (Register.CheckMobile()) {
                $(this).hide();
                Register.ShowTipImg("mobileTip_", "loading");
                Register.SendVerifyCode();
            }
        });
        $("#" + (Register.RegisterID + Register.Type)).bind("click", function () {
            if (!($("#agreement_" + Register.Type).is(":checked"))) {
                alert("请先同意推广联盟协议。");
                return false;
            }
            if (Register.CheckLoginName() && Register.CheckLoginPwd() && Register.CheckMobile() && Register.CheckCode() && Register.CheckCodeIsOK()) {
                clearInterval(Register.TimeInterval);
                Register.TimeInterval = null;
                Register.ShowTipImg("mobileTip_", "");
                Register.WriteTip("mobileTip_", "");
                $("#" + (Register.SendVerifyCodeID + Register.Type)).show();
                return true;
            }
            return false;
        });
    }

    /*
    *   检查登陆名
    */
    , CheckLoginName: function () {
        var result = FlyFish.CheckIsUserName((Register.LoginNameID + Register.Type));
        switch (parseFloat(result)) {
            case 0:     //正确
                Register.ShowTipImg("nameTip_", "loading");
                Register.WriteTip("nameTip_", "");
                return Register.CheckLoginNameIsExist();
            case -1:    //控件不存在
                Register.ShowTipImg("nameTip_", "error");
                Register.WriteTip("nameTip_", "请输入用户名");
                return false;
            case 1:     //没有输入文本
                Register.ShowTipImg("nameTip_", "error");
                Register.WriteTip("nameTip_", "请输入用户名");
                return false;
            case 2:     //用户名不符合规则
                Register.ShowTipImg("nameTip_", "error");
                Register.WriteTip("nameTip_", "用户名只能是 中文、_ 、数字、大小写字母组成");
                return false;
        }
        return true;
    }

    , /*
    *   检查登陆密码
    */
    CheckLoginPwd: function () {
        var result = FlyFish.CheckIsUserPwd((Register.LoginPwdID + Register.Type));
        switch (parseFloat(result)) {
            case 0:     //正确
                Register.ShowTipImg("pwdTip_", "ok");
                Register.WriteTip("pwdTip_", "");
                return true;
            case -1:    //控件不存在
                Register.ShowTipImg("pwdTip_", "error");
                Register.WriteTip("pwdTip_", "请输入密码");
                return false;
            case 1:     //没有输入文本
                Register.ShowTipImg("pwdTip_", "error");
                Register.WriteTip("pwdTip_", "请输入密码");
                return false;
            //case 2:     //用户密码不符合规则
            //    Register.ShowTipImg("pwdTip_", "error");
            //    Register.WriteTip("pwdTip_", "密码只能是 _ 、数字、大小写字母组成");
            //    return false;
        }
        return true;
    }
    /*
    *   检查验证码
    */
    , CheckCode: function () {
        var code = $("#" + (Register.CodeID + Register.Type)).val();
        if ("undefined" == code) {
            Register.ShowTipImg("codeTip_", "error");
            Register.WriteTip("codeTip_", "请输入验证码");
            return false;
        } else if ("" == code || 0 == code.length) {
            Register.ShowTipImg("codeTip_", "error");
            Register.WriteTip("codeTip_", "请输入验证码");
            return false;
        } else {
            var reg = /^\d+$/;
            if (!reg.test(code)) {
                Register.ShowTipImg("codeTip_", "error");
                Register.WriteTip("codeTip_", "验证码格式错误");
                return false;
            } else {
                Register.ShowTipImg("codeTip_", "ok");
                Register.WriteTip("codeTip_", "");
                return true;
            }
        }
    }
    /*
    *   检查验证码是否正确
    */
    , CheckCodeIsOK: function () {
        var b = true;
        $.ajax({
            type: "post",
            url: "/ajax/SendVerifyCode.ashx",
            data: { "operate": "Validate", "To": $("#" + (Register.MobileID + Register.Type)).val(), "verifyCode": $("#" + (Register.CodeID + Register.Type)).val() },
            cache: false,
            async: false,
            timeout: 30 * 1000,
            dataType: "json",
            success: function (result) {
                if ("0" == result.error) {
                    b = true;
                } else {
                    alert("验证码错误，请重新输入");
                    b = false;
                }
            }, error: function () {
                alert("检查验证码失败，请重试");
                b = false;
            }
        });
        return b;
    }
    , CheckMobile: function () {
        var result = FlyFish.CheckIsMobile($("#" + (Register.MobileID + Register.Type)).val());
        switch (parseFloat(result)) {
            case 0:     //正确
                Register.ShowTipImg("mobileTip_", "loading");
                Register.WriteTip("mobileTip_", "");
                return Register.CheckMobileIsExist();
            case -1:    //控件不存在
                Register.ShowTipImg("mobileTip_", "error");
                Register.WriteTip("mobileTip_", "请输入手机号码");
                return false;
            case 1:     //没有输入文本
                Register.ShowTipImg("mobileTip_", "error");
                Register.WriteTip("mobileTip_", "请输入手机号码");
                return false;
            case 2:     //用户密码不符合规则
                Register.ShowTipImg("mobileTip_", "error");
                Register.WriteTip("mobileTip_", "手机号码只能11位数字组成");
                return false;
        }
        return true;
    }
    , CheckMobileIsExist: function () {
        var b = false;
        try {
            $.ajax({
                type: "post",
                url: "/ajax/SendVerifyCode.ashx",
                data: { "operate": "CheckMobileIsExist", "Mobile": $("#" + (Register.MobileID + Register.Type)).val() },
                cache: false,
                async: false,
                timeout: 30 * 1000,
                dataType: "json",
                success: function (result) {
                    if ("0" == result.error) {
                        Register.ShowTipImg("mobileTip_", "ok");
                        Register.WriteTip("mobileTip_", "");
                        if (null == Register.TimeInterval) {
                            $("#" + (Register.SendVerifyCodeID + Register.Type)).show();
                        }
                        b = true;
                    } else {
                        Register.ShowTipImg("mobileTip_", "error");
                        Register.WriteTip("mobileTip_", "手机号码已经被使用");
                        b = false;
                    }
                }, error: function () {
                    Register.ShowTipImg("mobileTip_", "");
                    $("#" + (Register.SendVerifyCodeID + Register.Type)).show();
                    alert("验证手机号码失败");
                    b = false;
                }
            });
        } catch (e) {
            Register.ShowTipImg("mobileTip_", "");
            $("#" + (Register.SendVerifyCodeID + Register.Type)).show();
            alert("验证手机号码失败");
            b = false;
        }
        return b;
    }
    /*
    *   根据类型显示对应的提示图片
    */
    , ShowTipImg: function (divID, type) {
        var src = "";
        switch (type) {
            case "error":
                src = "error.png";
                break;
            case "loading":
                src = "loading.gif";
                break;
            case "ok":
                src = "ok.png";
                break;
        }
        if ("" == src) {
            $("#" + (divID + Register.Type)).find("img").hide();
        } else {
            $("#" + (divID + Register.Type)).find("img").attr("src", "/CPS/images/" + src).show();
        }
    }
    /*
    *   写入提示文字
    */
    , WriteTip: function (divID, tip) {
        $("#" + (divID + Register.Type)).find("span").html(tip);
    }
    , ClearInput: function () {
        $("#" + Register.LoginNameID + Register.Type).val("");
        $("#" + Register.LoginPwdID + Register.Type).val("");
        $("#" + Register.MobileID + Register.Type).val("");
        $("#" + Register.CodeID + Register.Type).val("");

        Register.ShowTipImg("nameTip_", "");
        Register.WriteTip("nameTip_", "");

        Register.ShowTipImg("pwdTip_", "");
        Register.WriteTip("pwdTip_", "");

        Register.ShowTipImg("mobileTip_", "");
        Register.WriteTip("mobileTip_", "");

        Register.ShowTipImg("codeTip_", "");
        Register.WriteTip("codeTip_", "");

        $("#" + (Register.LoginNameID + Register.Type)).focus();
    }

    /*
    *   检查用户名是否存在
    */
    , CheckLoginNameIsExist: function () {
        var b = false;
        $.ajax({
            type: "post",
            url: "/ajax/CheckUserName.aspx",
            data: { "UserName": FlyFish.Trim($("#" + (Register.LoginNameID + Register.Type)).val()), "operate": "ValidateUserName" },
            cache: false,
            async: false,
            timeout: 30 * 1000,
            dataType: "json",
            success: function (result) {
                if ("0" == result.error) {
                    Register.ShowTipImg("nameTip_", "error");
                    Register.WriteTip("nameTip_", "用户名已存在");
                    b = false;
                } else {
                    Register.ShowTipImg("nameTip_", "ok");
                    Register.WriteTip("nameTip_", "");
                    b = true;
                }
            }, error: function () {
                Register.ShowTipImg("nameTip_", "error");
                Register.WriteTip("nameTip_", "检查用户名是否存在失败，请重试");
                b = false;
            }
        });
        return b;
    }
    /*
    *   发送验证码
    */
    , SendVerifyCode: function () {
        $.ajax({
            type: "post",
            url: "/ajax/SendVerifyCode.ashx",
            data: { "operate": "send", "To": $("#" + (Register.MobileID + Register.Type)).val() },
            cache: false,
            async: true,
            timeout: 30 * 1000,
            dataType: "json",
            success: function (result) {
                Register.ShowTipImg("mobileTip_", "");
                if ("0" == result.error) {
                    Register.ShowTipImg("mobileTip_", "ok");
                    Register.SendVerifyCodeSuccess();
                    Register.TimeInterval = setInterval("Register.SendVerifyCodeSuccess()", 1000);
                } else {
                    $("#" + (Register.SendVerifyCodeID + Register.Type)).show();
                    alert("获取验证码失败");
                }

            }, error: function () {
                Register.ShowTipImg("mobileTip_", "");
                $("#" + (Register.SendVerifyCodeID + Register.Type)).show();
                alert("获取验证码异常");
            }
        });
    }

    /*
    *   获取验证码成功
    */
    , SendVerifyCodeSuccess: function () {
        if (Register.TimeIndex <= 0) {
            clearInterval(Register.TimeInterval);
            Register.TimeIndex = 59;
            Register.TimeInterval = null;
            Register.ShowTipImg("mobileTip_", "");
            Register.WriteTip("mobileTip_", "");
            $("#" + (Register.SendVerifyCodeID + Register.Type)).show();
            return;
        }
        Register.WriteTip("mobileTip_", "<i style='color:blue'>" + Register.TimeIndex + "秒后可重新获取</i>");
        Register.TimeIndex = Register.TimeIndex - 1;
    }
}
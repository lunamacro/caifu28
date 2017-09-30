var hotlist = {
    function InitData() {
        $.ajax({
            type: "POST", //用POST方式传输
            dataType: "json", //数据格式:JSON
            url: 'Join/HotList.ashx', //目标地址
            data: "",
            success: function (data) {
                alert(data.message);
            },
            error: function () {
                alert("异常");
            }
        });
    }
}

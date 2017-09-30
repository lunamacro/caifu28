using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

using Shove.Database;
using SLS.AutomaticOpenLottery.Task.App_Code;
using donet.io.rong.messages;
using donet.io.rong.models;
using donet.io.rong;
using Newtonsoft.Json;

namespace SLS.Score.Task
{
    /// <summary>
    /// Task 的摘要说明
    /// </summary>
    public class Task
    {
        private long _counter = 0;
        private long _delCounter = 0;

        private System.Threading.Thread _thread;

        private Message _msg = new Message("Task");
        private Log _log = new Log("Task");

        public int _state = 0;   // 0 停止 1 运行中 2 置为停止


        private  Shove._IO.IniFile ini = new Shove._IO.IniFile(System.AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
        int _timeGap = 5;


       

        //房间数组
        String[] bjRooms = {
                "9901", "9905",  "9909"};
        String[] jndRooms = {
                "9801", "9805","9809" };

        //假人数据  130个
        String[] fakeArr = {"花骨朵er","为 迩 封 鈊","蓝色灬飞扬","屎性ハ改°","情 比 纸 薄",
"「似水流年」°","浅陌凉汐","思念成瘾","じ十指相扣☆","╰素颜小姐",
"甯缺勿滥丶","____落尽残殇","心如死灰゛","後知後觉","七星瓢虫⊕",
"︶ε╰叽叽歪歪","後会无期","空城旧梦","夏殇¤落樱","没 心 没 肺 °",
"ぃ绣滊泡泡℃","嘟嘟⊕糖果","晴天。小曦","孤独患者","暇装bu爱    迩媞坏银",
"嘘！安静点","不痛不痒≈","夏末°微伤","洞房不败~","梦醒时分°",
"旧人成梦","顺萁咱然丶","紫蝶之纞","安然失笑ゝ ","ヾ︷浅色年华",
"_____流氓先森〃","屁颠屁颠-->","∝逢床作戏","じ★怃鈊嗳你","浮浮沉沉﹋",
"㊣儿⑧经","夏至ゝ未至","心力憔悴〤","缌念λ蓇",".·☆蝶舞飞扬☆·. ",
"故作坚强","本人已屎","Summer·不离不弃","So丶各自安好","行尸走肉",
"此号已封","麦芽糖糖ぴ","空城旧颜°","゛浮殇若梦╮ ","三好先森",
"正二8紧","浅夏﹌初凉","一纸荒年","╬茡潞扣","私定终生ら",
"尐猫咪咪","じ浮浮沉沉☆","樱婲の涙","麽心麽肺","旧 情 未 了",
"半醉〞巴黎づ","果冻布丁℃","薄暮凉年∞ ","毁了 悔了","浅心蓝染△",
"黯繎落泪〃","神经兮兮°","﹏诉丶那锻綪","坟场做戏﹏","ˊ命锺鉒顁。",
"______花季末了","卑微旳嗳","陌路离殇℡","安颜如夏    ︶浅Se年华﹌","ヾ乱世浮华つ",
"安之若素","罂粟Ω妖娆","κiζs呆呆尐糖","# 空城旧梦","二无止境",
"间间单单ヾ ","自欺欺人","陌落ミ繁华﹏","花落な莫离い","Mé、尐捣蛋",
"あ为谁痴狂ゼ","魔鬼先森","天意弄人","覆水难收╰","童心未泯",
"誮舞⊕霓裳","不爱我？滚！","墨羽尘曦","独守空城","# 念念不忘丶",
"╰流年已逝╮","尐嘴゛亲亲","流年、独殇","紫陌≈红尘","神经兮兮°",
"浮生若梦ァ","一 念 执 著","查无此人゛","妖言惑众 ","莫泆莫忘",
"谨色安年 *","三寸日光¤","红尘殇雪","烟花易凉。","AAAAA斯文败类",
"一曲离殇","笑靥っ如誮","吧唧吧唧","雨dē印记","半糖主义",
"旧日巴黎﹏","冷暖自知ら","蛋疼先森","冷月葬魂","那时°年少",
"一世妖娆","绝蝂de爱♂","勿念心安。","誮开一夏","我要疯的更高"};
        String[] fakeIconArr = { "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503217574802&di=8f7ca0f3eeb7226be302f8bcb8a83270&imgtype=0&src=http%3A%2F%2Fimg.bitscn.com%2Fupimg%2Fallimg%2Fc160120%2F1453262X42A60-4493D.jpg",
                                   "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503217737893&di=e5832a8b340c00ec47ecc80fcc670d46&imgtype=0&src=http%3A%2F%2Fimg.woyaogexing.com%2F2014%2F08%2F12%2F94b4a0cae4f1417f%2521200x200.png",
                                   "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503217766050&di=a7646320f3cbe3790d95383bd0d20f03&imgtype=0&src=http%3A%2F%2Fwww.qqwangming.org%2Fuploads%2Fallimg%2F140415%2F0034211344-15.png", 
                                   "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503217781160&di=0d97b00b545088c155558170a078fefd&imgtype=0&src=http%3A%2F%2Fwww.qqzhi.com%2Fuploadpic%2F2014-10-13%2F072443741.jpg", 
                                   "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503217802083&di=dcd8a34c9c1b27973fb5f6dd407889b1&imgtype=0&src=http%3A%2F%2Fimg.qqzhi.com%2Fupload%2Fimg_1_3226345356D1511750267_23.jpg",
                                
                                   "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503217833924&di=cedc0617879514eb86ad8baf8065c221&imgtype=0&src=http%3A%2F%2Fimg16.3lian.com%2Fgif2016%2Fgif16%2Fq1%2F58%2F61.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503217851330&di=180b12ed2707de9edef3ffb7dd42bd5c&imgtype=0&src=http%3A%2F%2Fimg.bitscn.com%2Fupimg%2Fallimg%2Fc160120%2F1453262QE21Z-24364.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503217870312&di=2fe2f5ddd2ab8234b71f7bb84e01b713&imgtype=0&src=http%3A%2F%2Fimg.name2012.com%2Fuploads%2Fallimg%2F2014-10%2F17-025750_760.jpg", 
                                "https://ss3.bdstatic.com/70cFv8Sh_Q1YnxGkpoWK1HF6hhy/it/u=1668291609,3800041450&fm=26&gp=0.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503217892790&di=1a1a99a9536f0ad3aa9ebb23bc665441&imgtype=0&src=http%3A%2F%2Fwww.qqleju.com%2Fuploads%2Fallimg%2F160928%2F28-084123_661.jpg",
                                
                                "https://ss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=3404690727,2803808396&fm=26&gp=0.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218013424&di=063fe76ab3a060ce490571aec89bace4&imgtype=0&src=http%3A%2F%2Fimg.bitscn.com%2Fupimg%2Fallimg%2Fc160120%2F1453262RQ5620-3V007.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218033683&di=e861615dedfadf294a4bba1b3a154eae&imgtype=0&src=http%3A%2F%2Fwww.itouxiang.net%2Fuploads%2Fallimg%2F20151218%2F070311294349393.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218040692&di=2eda000b31f9f0f5657a32c7c5ac0874&imgtype=0&src=http%3A%2F%2Fimg.qqzhi.com%2Fupload%2Fimg_3_3518905412D2724661559_23.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218077984&di=de9264ab260a00ad7f4647912bcd9fc3&imgtype=0&src=http%3A%2F%2Fpic2.ooopic.com%2F13%2F22%2F98%2F22b1OOOPICe6.jpg",
                               
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218122694&di=c147e76c2bdfdaa0b3148b970c0af41d&imgtype=0&src=http%3A%2F%2Fwmimg.sc115.com%2Ftx%2Fnew%2Fpic%2F0729%2F16075m4he4c4qd5.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218175461&di=b8a23a84e3e428a9d814d3bc8924bc22&imgtype=0&src=http%3A%2F%2Fimg.qqzhi.com%2Fupload%2Fimg_0_2997621697D3166368033_23.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218165334&di=b3ab229ba31c9dbf76befab83ff9399b&imgtype=0&src=http%3A%2F%2Fwww.jf258.com%2Fuploads%2F2014-08-14%2F112607926.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218209147&di=539104becd24b161694a8460b52a9009&imgtype=0&src=http%3A%2F%2Fimg.duoziwang.com%2F2017%2F05%2F10%2FB3723.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218188832&di=2e9d5c29bc3ad2385284ae7ff8a0655d&imgtype=0&src=http%3A%2F%2Fwww.qqwangming.org%2Fuploads%2Fallimg%2F140415%2F0034211344-15.png",
                               
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218262065&di=ff01baca22eef9a6816a5766e8f99205&imgtype=0&src=http%3A%2F%2Fwww.qqzhi.com%2Fuploadpic%2F2014-12-17%2F144926794.jpg",
                                "https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=3621202919,1699974293&fm=26&gp=0.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218317627&di=2bed6b2a31b34f39f4209e1cd79d1826&imgtype=0&src=http%3A%2F%2Fimg4.duitang.com%2Fuploads%2Fitem%2F201409%2F09%2F20140909214740_CZn3t.thumb.224_0.jpeg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218272773&di=7d5bc88e13d9724839ea4d45f8637cad&imgtype=0&src=http%3A%2F%2Fimg.woyaogexing.com%2F2014%2F08%2F12%2F94b4a0cae4f1417f%2521200x200.png", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218332653&di=58101c3183be41765592d8f54a2c75d0&imgtype=0&src=http%3A%2F%2Fattach.bbs.miui.com%2Fforum%2F201701%2F02%2F124948tsb8ey8oe3rqeqd0.jpg",
                               
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218427822&di=87e948d4d346e7f7e3bf531afd0f05ab&imgtype=0&src=http%3A%2F%2Fimg.duoziwang.com%2F2017%2F04%2F13%2FB3483.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218456056&di=a9926be92a05d31eaca80d38facd6bc3&imgtype=0&src=http%3A%2F%2Fup.qqjia.com%2Fz%2F25%2Ftu32700_24.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218439100&di=e1d146a06b3f5b868bbcfbef5d0fe11d&imgtype=0&src=http%3A%2F%2Fwww.sychinacnr.com%2Fpics%2Fbd9967040.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218476650&di=b979a2e93a96679d6f0db41e670700c5&imgtype=0&src=http%3A%2F%2Fimg.qqzhi.com%2Fupload%2Fimg_1_355768792D350331087_23.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218490020&di=7e9c26e5cdb21a96d3ef42c82ab23a3d&imgtype=0&src=http%3A%2F%2Fimg.qqzhi.com%2Fupload%2Fimg_3_194391915D2490455772_11.jpg",

                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218518482&di=634afc65f4a821dd2c006038016db314&imgtype=0&src=http%3A%2F%2Fimg1.3lian.com%2F2015%2Fgif%2Fw3%2F59%2F81.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218528164&di=0908d85489a14f2c712a50f78d8c6b11&imgtype=0&src=http%3A%2F%2Fimg16.3lian.com%2Fgif2016%2Fgif16%2Fq1%2F68%2F101.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218545075&di=8aee9e7cb8500b045b2c01ab43cfc527&imgtype=0&src=http%3A%2F%2Fimg.bitscn.com%2Fupimg%2Fallimg%2Fc160120%2F1453263039343P-1XT0.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218555885&di=431285a2e883671f6ea94e7cfbe9ad85&imgtype=0&src=http%3A%2F%2Fimg.bitscn.com%2Fupimg%2Fallimg%2Fc160120%2F1453262SH1X0-105941.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218569752&di=7f956709e1efdb62e6eeabdc75df26c8&imgtype=0&src=http%3A%2F%2Fwww.duoziwang.com%2Fuploads%2F1512%2F1-15122R331580-L.jpg",

                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218630243&di=74dae9147f8211661707cd345a2b5947&imgtype=0&src=http%3A%2F%2Fimg.qqzhi.com%2Fupload%2Fimg_0_121714187D2059687777_21.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218644573&di=c84b92339d570137aed678a9e21d09f5&imgtype=0&src=http%3A%2F%2Ftx.haiqq.com%2Fuploads%2Fallimg%2F170507%2F02315210L-3.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218669663&di=25e1b02e4859f8d3a7ceb0b979678ca5&imgtype=0&src=http%3A%2F%2Ff.hiphotos.baidu.com%2Fzhidao%2Fwh%253D600%252C800%2Fsign%3D64a499f8962bd4074292dbfb4bb9b269%2F5fdf8db1cb13495464a3efb4574e9258d0094aa4.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218689898&di=e04d50a042587bec1eabb46fb7bf938d&imgtype=0&src=http%3A%2F%2Fwww.iwx.me%2Fuploads%2Fallimg%2Fc140508%2F13c50O14HE0-1C40_lit.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218701945&di=a400c25f7ee520b57817b36e6cc0bb8b&imgtype=0&src=http%3A%2F%2Fwww.qq1234.org%2Fuploads%2Fallimg%2F140801%2F1K4512G8-34.jpg",

                                "https://ss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=469737983,2008903988&fm=26&gp=0.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218747390&di=d11564a9811b70a4ba71a92a5954fb31&imgtype=0&src=http%3A%2F%2Fimg.duoziwang.com%2F2017%2F04%2F15%2FB0905.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218755973&di=a979e01049902f55534a45a831916614&imgtype=0&src=http%3A%2F%2Fimg.cxdq.com%2Fd%2Ffile%2F2016%2F07-29%2Fe21014ceece5c57d95797101767a99cb.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218768880&di=8fbe3fa27d81a7e86141a1b0b9cf1922&imgtype=0&src=http%3A%2F%2Fimg.bitscn.com%2Fupimg%2Fallimg%2Fc160120%2F145326304T21Z-19D26.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218776816&di=e69b35aab8f5ddaadc08fb59b2d3b380&imgtype=0&src=http%3A%2F%2Fwww.hczhanjia.cn%2Fimg%2Fbd14350267.jpg",

                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218823294&di=efceb538841196b98ee6e6967d955ae0&imgtype=0&src=http%3A%2F%2Fwww.qxjlm.com%2Ftupians%2Fbd15337122.jpg", 
                                "https://ss2.bdstatic.com/70cFvnSh_Q1YnxGkpoWK1HF6hhy/it/u=1672885911,2912097993&fm=26&gp=0.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218870491&di=7b63109b69102fe7e8b1edbbe34810ba&imgtype=0&src=http%3A%2F%2Fimg.bitscn.com%2Fupimg%2Fallimg%2Fc160120%2F1453262YI5940-319203.jpg", 
                                "https://ss0.bdstatic.com/70cFuHSh_Q1YnxGkpoWK1HF6hhy/it/u=3986029145,3965206119&fm=26&gp=0.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218897085&di=f1675bb6c54ccdf8c16b7ab839f96f7a&imgtype=0&src=http%3A%2F%2Fwww.91danji.com%2Fattachments%2F201410%2F05%2F11%2F5t7r6bfkg.jpg",

                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218937305&di=db81c38b4986ae43a51d9e055777f3d6&imgtype=0&src=http%3A%2F%2Fwww.qqzhi.com%2Fuploadpic%2F2014-08-03%2F040714240.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218953873&di=adcd6551ce6ad055cf17d880db9d1910&imgtype=0&src=http%3A%2F%2Fimg17.3lian.com%2Fd%2Ffile%2F201701%2F04%2F269045dfb526cd7038a66b27885e1d7a.jpg", 
                                "https://ss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=158496765,117053386&fm=26&gp=0.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218977303&di=f091f96af85be3d3854fd3e0c40550ec&imgtype=0&src=http%3A%2F%2Fp1.qqyou.com%2Ftouxiang%2FUploadPic%2F2015-5%2F23%2F2015052315532983138.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503218960550&di=4987df4aeab585ca1b9ffa5f60a60966&imgtype=0&src=http%3A%2F%2Fwww.duoziwang.com%2Fuploads%2F1512%2F1-1512262243590-L.png",

                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219038943&di=665b5646c5d3072b6ccc6c54d01d09f8&imgtype=0&src=http%3A%2F%2Fwww.qqzhi.com%2Fuploadpic%2F2014-09-20%2F181722419.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219050095&di=9a8e752a3751d64b5c43c5fcfae61531&imgtype=0&src=http%3A%2F%2Fwww.qqleju.com%2Fuploads%2Fallimg%2F160213%2F13-053516_87.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219059196&di=d4b425e063d1fc5dabf88525d4a331df&imgtype=0&src=http%3A%2F%2Fimg.duoziwang.com%2F2017%2F04%2F15%2FB0428.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219112090&di=579a28745c0f442feaf96447bb25510a&imgtype=0&src=http%3A%2F%2Fmvimg2.meitudata.com%2F53a52b9eac7727494.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219133662&di=842bcce0e33d8cfcf8af834bed65fd0f&imgtype=0&src=http%3A%2F%2Fimg.qqzhi.com%2Fupload%2Fimg_3_2706096394D2567195847_23.jpg",

                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219164966&di=2f1b1d7fb8ec6426dc954caf2b47be03&imgtype=0&src=http%3A%2F%2Fimg.zcool.cn%2Fcommunity%2F01c4ad5542c0da0000019ae95a5802.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219179703&di=e2aa65783794eb3ba9a7b0aa34154afb&imgtype=0&src=http%3A%2F%2Fimg.qqzhi.com%2Fupload%2Fimg_5_1640939724D4263648597_11.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219187083&di=04214686f6c2e6a1a84acb69e49b43f9&imgtype=0&src=http%3A%2F%2Fwww.enviroinvest.com.cn%2Fupload%2Fimages%2FTB1AGHAKFXXXXblXVXXXXXXXXXX_%2521%25210-item_pic.jpg_210x210.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219203992&di=d31b9975f235f07bc01c34d42f6f8284&imgtype=0&src=http%3A%2F%2Fimg.duoziwang.com%2F2017%2F03%2F13%2FB0767.jpg", 
                                "https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=3111409029,1966624399&fm=26&gp=0.jpg",

                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219267285&di=c80aa2f94b282ab16e99247a74fed747&imgtype=0&src=http%3A%2F%2Fwww.qqzhi.com%2Fuploadpic%2F2015-01-10%2F003627830.jpg",
                                "https://ss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=3146281992,2389949933&fm=26&gp=0.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219319691&di=8a91afedbf7f9e90d1f99bd28615a3b4&imgtype=0&src=http%3A%2F%2Fimg16.3lian.com%2Fgif2016%2Fgif16%2Fq2%2F22%2F82.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219334452&di=a143c100d8b94c483957c2cbf8502ceb&imgtype=0&src=http%3A%2F%2Fwww.qxjlm.com%2Ftupians%2Fbd15684661.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219304830&di=06aab7718b73f68119266fd6c212ca42&imgtype=0&src=http%3A%2F%2Fd.hiphotos.baidu.com%2Fzhidao%2Fpic%2Fitem%2F64380cd7912397dd12791cf45e82b2b7d0a2873a.jpg",

                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219382566&di=e6f2bfa573978d342fc43461570d3dbd&imgtype=0&src=http%3A%2F%2Fimg.qqzhi.com%2Fupload%2Fimg_0_2603378776D2044597854_23.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219395965&di=151a0f2911123ab57c21c922919c564c&imgtype=0&src=http%3A%2F%2Fpic.qqtn.com%2Fup%2F2016-4%2F2016042709464041022.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503814134&di=4c899243df9268d0adfc8f1579ff15ab&imgtype=jpg&er=1&src=http%3A%2F%2Fimg.bitscn.com%2Fupimg%2Fallimg%2Fc160120%2F1453262Y16250-14UE.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219367316&di=955698dcbb399f1dd0d05b2b101867e2&imgtype=0&src=http%3A%2F%2Fwww.qqjay.com%2Fuploads%2Fallimg%2F160207%2F1_0QR09156.png", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219431296&di=a87d264fbb4a6671168417eea76dae29&imgtype=0&src=http%3A%2F%2Fwww.th7.cn%2Fd%2Ffile%2Fp%2F2016%2F05%2F20%2F2407e37ea13ee44e2cc76b9367e0e41e.jpg",

                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219457820&di=1cb4f1c1fe9f5461d9d014375b598f86&imgtype=0&src=http%3A%2F%2Fwww.itouxiang.net%2Fuploads%2Fallimg%2F20151218%2F182330433420347.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219470028&di=4bb6162ad97dc9f711e19fb8f7db383a&imgtype=0&src=http%3A%2F%2Fwww.jf258.com%2Fuploads%2F2014-09-07%2F140802855.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219478564&di=ccbc595cbd759279133db34c3de520e7&imgtype=0&src=http%3A%2F%2Fwww.qqzhi.com%2Fuploadpic%2F2014-10-30%2F071453573.jpg", 
                                "https://ss2.bdstatic.com/70cFvnSh_Q1YnxGkpoWK1HF6hhy/it/u=3650522251,396278551&fm=26&gp=0.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219515048&di=285cbe875f199a3e7b98a89e6834d5cb&imgtype=0&src=http%3A%2F%2Fwww.qqzhi.com%2Fuploadpic%2F2014-09-25%2F211949594.jpg",

                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219547049&di=9fc04de1ccab1509c33659d37da27c8a&imgtype=0&src=http%3A%2F%2Fwww.qqzhi.com%2Fuploadpic%2F2014-09-25%2F211949594.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219565667&di=ed8efca8f8775d42f562e315e6f6b027&imgtype=0&src=http%3A%2F%2Fimg2.itiexue.net%2F1627%2F16275122.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219587078&di=3d80973bc0d85477a106f86a08eb27dc&imgtype=0&src=http%3A%2F%2Fwww.jf258.com%2Fuploads%2F2014-08-29%2F193313751.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219605559&di=a20499f8f906c1df945b5f0cb2744112&imgtype=0&src=http%3A%2F%2Fp1.qqyou.com%2Ftouxiang%2FUploadPic%2F2015-6%2F29%2F2015062917182188981.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219531969&di=bce39352ce2b7fa781316ffaeb6d81f2&imgtype=0&src=http%3A%2F%2Fwww.18weixin.com%2Fupload%2Fday_130831%2F201308311203054895.jpg",

                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219650301&di=ac0fa4dd626ea2c0f76479ffe1b6016a&imgtype=0&src=http%3A%2F%2Fwww.qqwangming.org%2Fuploads%2Fallimg%2Fc150831%2F1440bAOC0-5K25.png", 
                                "https://ss2.bdstatic.com/70cFvnSh_Q1YnxGkpoWK1HF6hhy/it/u=263514756,2179360472&fm=26&gp=0.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219692419&di=19cdebb3ccdb566efb9c3091203ce159&imgtype=0&src=http%3A%2F%2Fp1.qqyou.com%2Ftouxiang%2FUploadPic%2F2015-4%2F28%2F2015042814452215149.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219679610&di=2877495e262bfd5c41ef967e057129fa&imgtype=0&src=http%3A%2F%2Fimg.duoziwang.com%2Fuploads%2F1512%2F1-1512191036450-L.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219702050&di=6cd03157f41ee5b31c7ba3db1de8df3b&imgtype=0&src=http%3A%2F%2Fimg.qqzhi.com%2Fupload%2Fimg_4_594697882D3068528316_21.jpg",

                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219744148&di=aeb169caaaca2048430a0baa89c378ab&imgtype=0&src=http%3A%2F%2Fqq1234.org%2Fuploads%2Fallimg%2F140805%2F3_140805130801_6.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219725299&di=04313eed2fb634fa2514fed4b61e61da&imgtype=0&src=http%3A%2F%2Fwww.qqzhi.com%2Fuploadpic%2F2015-01-17%2F174617392.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219774563&di=dde78a73211aebc970e7b9c1efcbb651&imgtype=0&src=http%3A%2F%2Ffile.popoho.com%2F2016-08-19%2F3cc7b273470d75326f4cf532feb6e4c8.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219761975&di=791f8aa76938ca0d117735a0b87dc5bf&imgtype=0&src=http%3A%2F%2Fimg17.3lian.com%2Fd%2Ffile%2F201701%2F17%2F78ce93d5825eb7e26f6b99a31ccd684a.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219790418&di=d62ecf0d5abeefdbbfc4e41942b070a6&imgtype=0&src=http%3A%2F%2Ffiles.jb51.net%2Ffile_images%2Farticle%2F201408%2F20140827092123149.jpg",

                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219822836&di=4466603448ac58ba95268889384c9ec9&imgtype=0&src=http%3A%2F%2Fwww.qqzhi.com%2Fuploadpic%2F2015-01-14%2F072958461.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219839949&di=2f37754d8caf7c248db7254f17d77984&imgtype=0&src=http%3A%2F%2Fwenwen.soso.com%2Fp%2F20100914%2F20100914224016-1234271274.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219854540&di=f9bcc282c9866d09d7df8792d0213c1b&imgtype=0&src=http%3A%2F%2Fwww.jf258.com%2Fuploads%2F2015-05-14%2F235018808.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219867415&di=553f86cc4b17658b8c720b592cbb5264&imgtype=0&src=http%3A%2F%2Fwww.duoziwang.com%2Fuploads%2F1506%2F1-15061H205240-L.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219882076&di=2146be113b2edced3639352c5ad4f321&imgtype=0&src=http%3A%2F%2Fwww.th7.cn%2Fd%2Ffile%2Fp%2F2016%2F05%2F20%2F14c359dea38bb3d329491fdbcfa00307.jpg",

                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219924990&di=51534c655eda55a0985bf7f9166fc8d5&imgtype=0&src=http%3A%2F%2Fwww.qqzhi.com%2Fuploadpic%2F2015-01-09%2F183132502.jpg",
                                "https://ss2.bdstatic.com/70cFvnSh_Q1YnxGkpoWK1HF6hhy/it/u=2199439716,1154666630&fm=26&gp=0.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219949851&di=c974207f6d6237a7f62cd3604a54a651&imgtype=0&src=http%3A%2F%2Fimg.duoziwang.com%2F2017%2F03%2F04%2FB1992.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219962889&di=ad1e89b987804153eec69069555a6545&imgtype=0&src=http%3A%2F%2Fp.3761.com%2Fpic%2F76771419817980.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503219940205&di=65f27909cfe47dfdc50434fc0e83b8b0&imgtype=0&src=http%3A%2F%2Fimg.qqzhi.com%2Fupload%2Fimg_5_3109134768D1901131503_21.jpg",

                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220004879&di=e1a59a43cbb4b005bf516f960b3d0fd3&imgtype=0&src=http%3A%2F%2Fimg.duoziwang.com%2F2017%2F04%2F15%2FB0562.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220012324&di=bf7d9ae968eb705e495579caf6a143a0&imgtype=0&src=http%3A%2F%2Fimg.duoziwang.com%2F2017%2F05%2F10%2FB2846.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220024473&di=dd19fc4be301bb03f2fd85a68690b13c&imgtype=0&src=http%3A%2F%2Fimg.duoziwang.com%2F2017%2F04%2F13%2FB1855.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220054797&di=bd66cba9adf4249c6e11cc47082a447d&imgtype=0&src=http%3A%2F%2Fimg.name2012.com%2Fuploads%2Fallimg%2F2015-01%2F12-043913_353.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220041050&di=f3fce4c19f4e30f0d29c361c3c15b77a&imgtype=0&src=http%3A%2F%2Fimg.duoziwang.com%2F2017%2F04%2F13%2FB3468.jpg",

                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503814816&di=f7fce04865ab41ab22faeebbac6a057b&imgtype=jpg&er=1&src=http%3A%2F%2Fwww.qqzhi.com%2Fuploadpic%2F2014-05-08%2F121006617.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220081132&di=d658fbba8c4c0dd869ecadaf32611170&imgtype=0&src=http%3A%2F%2Fwww.jf258.com%2Fuploads%2F2015-05-15%2F032721480.jpg", 
                                "https://ss3.bdstatic.com/70cFv8Sh_Q1YnxGkpoWK1HF6hhy/it/u=3998854953,230253840&fm=26&gp=0.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220102341&di=3142d298965d45daa2a9a886d55a37e4&imgtype=0&src=http%3A%2F%2Ft.meipaituw.com%2Fmeipaitu%2F0152699e18e1455f8c.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220132856&di=b040a5a97ff8d8538b85277c18f469ce&imgtype=0&src=http%3A%2F%2Fimg.duoziwang.com%2F2017%2F04%2F13%2FB3456.jpg",

                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220176964&di=09d040805beee1797a9a18c30baac585&imgtype=0&src=http%3A%2F%2Fwww.qqzhi.com%2Fuploadpic%2F2014-05-04%2F144604680.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220191012&di=1d293fc3e0cae80a2ddedf6ccf5006b1&imgtype=0&src=http%3A%2F%2Fimg.bitscn.com%2Fupimg%2Fallimg%2Fc160120%2F145326303150-2Ja0.jpg", 
                                "https://ss3.bdstatic.com/70cFv8Sh_Q1YnxGkpoWK1HF6hhy/it/u=2103385685,288062578&fm=26&gp=0.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220225293&di=7cc9355a8dc45a9769f12fa25f562bbd&imgtype=0&src=http%3A%2F%2Fimg.qqzhi.com%2Fupload%2Fimg_2_1169174918D3960424720_11.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220215495&di=12115fad3e8e1f25b5e64bf7b2fbd2af&imgtype=0&src=http%3A%2F%2Ffile.popoho.com%2Fjsqq%2F20160704%2F5_160704184303_11.jpg",

                                "https://ss1.bdstatic.com/70cFuXSh_Q1YnxGkpoWK1HF6hhy/it/u=1564031279,429276498&fm=26&gp=0.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220245616&di=8158df48fecfcf6348bf856dcd8c7d5c&imgtype=0&src=http%3A%2F%2Fwww.caslon.cn%2Fpics%2Fallimg%2Fbd20226620.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220265242&di=4b1b35694d984890cf55b8c6a1bbfa4c&imgtype=0&src=http%3A%2F%2Fi.gtimg.cn%2Fopen%2Fapp_icon%2F01%2F07%2F52%2F33%2F1101075233_p2.png",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220294496&di=d48af60a60f3854e5c91bdbe86e29e35&imgtype=0&src=http%3A%2F%2Fqq.lameng.net%2Fuploads%2Fallimg%2F141007%2F1121446028-0.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220282838&di=90aa94032f64606015e5dc95f523f11a&imgtype=0&src=http%3A%2F%2Fwww.duoziwang.com%2Fuploads%2F1512%2F1-15121R13A70-L.jpg",

                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220314791&di=bd1dc19833d5f9804b385e874b9179db&imgtype=0&src=http%3A%2F%2Fimg.duoziwang.com%2F2017%2F04%2F06%2FB0157.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220306233&di=a753d11c9b8c9871460768146361b361&imgtype=0&src=http%3A%2F%2Fwww.qqzhi.com%2Fuploadpic%2F2015-01-14%2F073000933.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220329154&di=0c80ac3b1037e1196fb279f5b5a8e2b3&imgtype=0&src=http%3A%2F%2Fimg.qqzhi.com%2Fupload%2Fimg_1_990373348D936585651_21.jpg",
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220347767&di=00776cab66021cbd55cbdc98519153a9&imgtype=0&src=http%3A%2F%2Fimg.duoziwang.com%2F2017%2F04%2F23%2FB6144.jpg", 
                                "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1503220370800&di=113ef8ad204367ca4e24086cbf372372&imgtype=0&src=http%3A%2F%2Fimg.duoziwang.com%2F2017%2F04%2F13%2FB2377.jpg"};

        String[] fakeBetArr = { "05", "06", "07", "08","大", "小", "单", "双", "大单", "大双", "小单", "小双", "极大", "极小", 
                                "红","绿","蓝", "09","19","20","21","豹子"};
        String[][] fakeMoneyArr = new string[3][];



        private SqlConnection conn;
        RongCloud rongcloud;
        public Task()
        {
            fakeMoneyArr[0] = new String[] { "10", "20", "50", "100", "200", "500", "150", "300", "1000", "15", "25", "55", "260", "550", "355", "75", "225", "900", "750", "275" };
            fakeMoneyArr[1] = new String[] { "50", "100", "200", "500", "800", "1200", "1000", "1500", "2000", "75", "125", "175", "225", "450", "550", "1250", "750", "555", "255", "999" };
            fakeMoneyArr[2] = new String[] { "500", "800", "1000", "2000", "1500", "5000", "8000", "2500", "10000", "750", "555", "2255", "999", "7550", "1225", "5555", "2550", "6580", "4500", "1200" };

            string _connectionString = ini.Read("Options", "ConnectionString");
            conn = Shove.Database.MSSQL.CreateDataConnection<System.Data.SqlClient.SqlConnection>(_connectionString);

             //融云IM的两个值
            string appKey = ini.Read("Options", "AppKey");
            string appSecret = ini.Read("Options", "AppSecret");
            rongcloud = RongCloud.getInstance(appKey, appSecret);
        }

        public void Run()
        {
            // 已经启动
            if (_state == 1)
            {
                return;
            }

            lock (this) // 确保临界区被一个 Thread 所占用
            {
                _state = 1;

                _counter = 0;
                _thread = new System.Threading.Thread(new System.Threading.ThreadStart(Do));
                _thread.IsBackground = true;

                _thread.Start();

                _msg.Send("Task Start.");
                _log.Write("Task Start.");
            }
        }

        public void Exit()
        {
            _state = 2;
        }

        public void Do()
        {
            while (true)
            {
                if (_state == 2)
                {
                    _msg.Send("Task Stop.");
                    _log.Write("Task Stop.");

                    _state = 0;

                    Stop();

                    return;
                }

                System.Threading.Thread.Sleep(1000);   // 1秒为单位

                _counter++;
                _delCounter++;

                if (_delCounter>=3600*3)
                {
                     _delCounter = 0;
                    try
                    {
                        string nowDate = DateTime.Now.AddHours(-3).ToString("yyyy-MM-dd HH:mm:ss");
                        string dsql = "delete from T_FakeBet where [DateTime]<'"+nowDate+"'";
                        MSSQL.ExecuteNonQuery(conn, dsql);
                    }
                    catch (Exception ex)
                    {
                        _log.Write("Exception：" + ex.Message);
                    }
                }

               

                //默认30秒执行一次
                if (_counter >= _timeGap)
                {
                    _counter = 0;

                    try
                    {

                        int h = DateTime.Now.Hour;
                        if (h !=19)
                        {
                            //加拿大
                            pushOpen(98, jndRooms, 81, "", "");
                        }
                        if (h>=9)
                        {
                            //北京
                            pushOpen(99, bjRooms, 81, "", "");
                        }

                       
                       

                        Random ran = new Random();
                        _timeGap = ran.Next(2, 15);

                    }
                    catch (Exception ex)
                    {
                        _log.Write("Exception：" + ex.Message);
                    }
                }
            }
        }

        private void Stop()
        {
            if (_thread != null)
            {
                _thread.Abort();
                _thread = null;
            }
        }

        public void pushOpen(int lotteryID, string[] rooms, int type, string qihao, string lucknumber)
        {
            

            //融云发送服务，新
            PushMsg pushMsg = new PushMsg();
            pushMsg.type = type;
            pushMsg.time = DateTime.Now;
            pushMsg.senderUserId = 66;
            
            pushMsg.periods = qihao;
            pushMsg.open_result = lucknumber;
            pushMsg.welcome_user = 0;
            //新增的
            pushMsg.lotteryID = lotteryID;
            pushMsg.homeIndex = 1;

            // CodeSuccessReslut messagepublishChatroomResult = rongcloud.message.publishChatroom("66", rooms, messagepublishChatroomTxtMessage);
            //  log.Write("publishChatroom:[code][" + messagepublishChatroomResult.getCode() + "][" + messagepublishChatroomResult.getErrorMessage() + "]");

            for (int i = 0; i < 3; i++)
            {
                Random ran = new Random();
                int RandUser = ran.Next(0, 129);
                int RandBet = ran.Next(0, 17);
                int RandMoney = ran.Next(0, 19);

                pushMsg.nickname = fakeArr[RandUser];
                pushMsg.avatar = fakeIconArr[RandUser];
                pushMsg.betType = fakeBetArr[RandBet];
                pushMsg.money = int.Parse(fakeMoneyArr[i][RandMoney]);
                pushMsg.homeIndex = i;

                
                string json = JsonConvert.SerializeObject(pushMsg);
                TxtMessage messagepublishChatroomTxtMessage = new TxtMessage("投注推送", json);
                //string groupId = (lotteryID * 100 + (i + 1)).ToString();
                string[] roomArray = { rooms[i] };

                CodeSuccessReslut messagepublishChatroomResult = rongcloud.message.publishGroup("66", roomArray, messagepublishChatroomTxtMessage, null, null, 0, 0);
                //_log.Write("publishGroup:[code][" + messagepublishChatroomResult.getCode() + "][" + messagepublishChatroomResult.getErrorMessage() + "]");

                string isql="insert into T_FakeBet ([NickName],[Money],[LotteryNumber],[lotteryID],[homeIndex],[Avatar]) values ('" + pushMsg.nickname + "'," + pushMsg.money + ",'" + pushMsg.betType + "'   ," + lotteryID + "," + pushMsg.homeIndex + ",'" + pushMsg.avatar + "')";
                
                int addResult=MSSQL.ExecuteNonQuery(conn, isql);
                //_log.Write("SQL=" + isql + "   result=" + addResult);

            }

        }
    }
}
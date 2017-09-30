using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class scheme
{
    int Multiple = 0;//投注倍数
    int PlanNo = 0;//方案号
    int PlayType = 0;//子玩法(比如1单式2复式3胆拖)
    int LotteryType = 0;//彩种【附录A】北单胜平欠20、竟彩34
    int PlayTypeDs = 0;//过关方式(默认为0)默认就行
    string UserName = "admin";//用户名
    string UserPwd = "";//密码(md5编码Gbk)
    int Term = 0;//彩期
    int Amount = 1;//注数*2* multiple（倍数）投注金额 (则代购就是购买金额，则是合买为发起方案总金额)
    string Content = "";//投注内容
    int PlanType = 1;//购买方式 如 1代购、2合买
    int AddAttribute = 1;//追加购买
    int PublicStatus = 0;//方案是否公开 如 1不公开、2公开、3开奖后公开
    int SelectType = 2;//投注方式(1文件上传,2自选投注)
    string UploadFilePath = "";//如果单式上传就有上传路径

    string PassMode = "";//竟彩投注的是单关、过关
    string PassType = "";//玩法串(2代表2串1)

    int FounderAmount = 1;//合买认购金额 默认1(发起者购买份数)
    int FounderBdAmount = 0;//是否保底 默认0
    int Commision = 0;//是否提成 默认0
    string Other = "团结就是力量";//合买宣言 默认“团结就是力量”
    int PerAmount = 0;//每份金额
    int Part = 0;//认购份数

    int ChangeType = 0;//Pc=2、客户端=6

    public int multiple { get { return Multiple; } set { Multiple = value; } }
    public int planNo { get { return PlanNo; } set { PlanNo = value; } }
    public int playType { get { return PlayType; } set { PlayType = value; } }
    public int lotteryType { get { return LotteryType; } set { LotteryType = value; } }
    public int playTypeDs { get { return PlayTypeDs; } set { PlayTypeDs = value; } }
    public string userName { get { return UserName; } set { UserName = value; } }
    public string userPwd { get { return UserPwd; } set { UserPwd = value; } }
    public int term { get { return Term; } set { Term = value; } }
    public int amount { get { return Amount; } set { Amount = value; } }
    public string content { get { return Content; } set { Content = value; } }
    public int planType { get { return PlanType; } set { PlanType = value; } }
    public int addAttribute { get { return AddAttribute; } set { AddAttribute = value; } }
    public int publicStatus { get { return PublicStatus; } set { PublicStatus = value; } }
    public int selectType { get { return SelectType; } set { SelectType = value; } }
    public string uploadFilePath { get { return UploadFilePath; } set { UploadFilePath = value; } }
    public string passMode { get { return PassMode; } set { PassMode = value; } }
    public string passType { get { return PassType; } set { PassType = value; } }
    public int founderAmount { get { return FounderAmount; } set { FounderAmount = value; } }
    public int founderBdAmount { get { return FounderBdAmount; } set { FounderBdAmount = value; } }
    public int commision { get { return Commision; } set { Commision = value; } }
    public string other { get { return Other; } set { Other = value; } }
    public int perAmount { get { return PerAmount; } set { PerAmount = value; } }
    public int part { get { return Part; } set { Part = value; } }
    public int changeType { get { return ChangeType; } set { ChangeType = value; } }
}
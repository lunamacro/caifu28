<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="BJXY28_Default" %>

<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<%@ Register Src="~/Home/Room/UserControls/WebFoot.ascx" TagName="WebFoot" TagPrefix="uc1" %>
<%@ Register Src="~/Home/Room/UserControls/WebHead.ascx" TagName="WebHead" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>幸运28 - <%=_Site.Name %></title>
    <link href="../Style/global.css" rel="stylesheet" type="text/css" />
    <link href="../Style/lottery.css?v=1.2.0" rel="stylesheet" type="text/css" />
    <link href="../Style/style.css" rel="stylesheet" type="text/css" />
    <link href="cqssc.css" rel="stylesheet" type="text/css" />
    <link href="../JScript/artDialog/css/ui-dialog.css" rel="stylesheet" />
    <link href="../JScript/jBox/Skins/Blue/jbox.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #ssc .new-win > p {
            overflow: hidden;
            width: 93%;
        }

        #prompt {
            width: 98%;
        }

        #confirm-ball {
            height: 31px;
        }

        #random-btn a {
            line-height: 15px;
        }

        #txtMultiple {
            height: 20px;
        }

        #lotterySubmit {
            color: White;
            text-decoration: none;
            padding: 10px 32px;
        }

        #number-list li {
            cursor: default;
        }

        .omit span {
            margin: 0 3px;
        }

        /*#ssc .lect-box .paste {
            padding: 0px;
        }

        #iframe_scheme_upload_page {
            margin-left: 45px;
        }

        .paste label {
            vertical-align: top;
            font-size: 13px;
            margin-top: 110px;
            margin-right: 20px;
        }

        .paste div {
            margin-left: 45px;
            height: 300px;
        }

        .paste textarea {
            width: 560px;
            height: 200px;
            resize: none;
        }*/

        #ssc .notice > p.num > b {
            display: inline-block;
            margin-right: 2px;
            height: 50px;
            line-height: 50px;
            width: 42px;
            color: #333;
            font-size: 22px;
            text-align: center;
            background: url(../images/lucky28.png) no-repeat -206px -130px;
        }

        #wxzx {
            background-image: url(/BJXY28/lucky28_game_bg_meitu_1.jpg);
        }

        #ssc .lect-num .balls {
            position: relative;
            padding-left: 70px;
            width: 380px;
        }

        #ssc .lect-box .balls li > b, #wxzx .balls li a > b, #wxzx .balls li em > b,
        #ssc .lect-box .balls .omit > span {
            display: inline-block;
            margin-right: 3px;
            width: 32px;
            text-align: center;
        }

        #ssc .lect-box .balls .omit {
            position: absolute;
            left: 0;
            bottom: 5px;
            color: #8a8a8a;
        }

        #wxzx .balls li > b, #wxzx .balls li a > b, #wxzx .balls li em > b {
            height: 40px !important;
            width: 40px !important;
            line-height: 40px !important;
            color: #555;
            margin-right: 6px !important;
            font-size: 18px;
            background: url(../Images/lucky28.png) no-repeat -2px -133px;
            cursor: pointer;
        }

        #wxzx .balls li b.active,
        #wxzx .balls li a b.active,
        #wxzx .balls li em b.active {
            background-position: -84px -133px !important;
            color: #f9f9f9;
        }



        #wxzx .balls li > b.green,
        #wxzx .balls li em > b.green {
            background-position: -125px -133px;
        }

        #wxzx .balls li > b.blue,
        #wxzx .balls li em > b.blue {
            background-position: -166px -133px;
        }

        #wxzx .balls li > b.red,
        #wxzx .balls li em > b.red {
            background-position: -207px -133px;
        }

        #wxzx .balls li > a, #wxzx .balls li > em {
            height: 46px;
            width: 167px;
            line-height: 46px;
            margin: 0px;
            font-size: 14px;
            border: 1px solid #dedede;
            border-right: 0;
            /*background-color: #f1f1f1;*/
            color: #333;
            text-align: center;
            cursor: pointer;
            text-decoration: none;
        }

        #wxzx .balls li > a, #wxzx .balls li > em {
            display: table-cell;
            vertical-align: top;
        }

            #wxzx .balls li > a.active {
                background: #DFDFDF;
            }

        #wxzx .balls li a .bold {
            font-size: 20px;
            font-weight: bold;
        }

        #wxzx .balls li a .rate {
            font-size: 13px;
        }

        #wxzx .balls li .small {
            width: 83px;
            border-right: 0;
        }

        #wxzx .balls li .right-border {
            border: 1px solid #dedede;
        }

        #wxzx .balls .betmoney {
            height: 60px;
            border: 1px solid #dedede;
            width: 669px;
            margin-bottom: 64px;
        }

            #wxzx .balls .betmoney ul > li {
                width: 60px;
                height: 60px;
                text-align: center;
                line-height: 60px;
                display: inline-block;
                cursor: pointer;
                margin-right: 5px;
                background: url('../Images/lucky28.png') no-repeat 0% 0% scroll transparent;
            }

            #wxzx .balls .betmoney ul li > span {
                width: 100%;
                height: 60px;
                line-height: 60px;
                font-size: 12px;
                color: #f9f9f9;
                text-align: center;
                display: inline-block;
            }

            #wxzx .balls .betmoney ul > li:nth-child(1) {
                background-position: 0px -581px;
            }

            #wxzx .balls .betmoney ul > li:nth-child(2) {
                background-position: -78px -581px;
            }

            #wxzx .balls .betmoney ul > li:nth-child(3) {
                background-position: -159px -581px;
            }

            #wxzx .balls .betmoney ul > li:nth-child(4) {
                background-position: -240px -581px;
            }

            #wxzx .balls .betmoney ul > li:nth-child(5) {
                background-position: -320px -581px;
            }

        .betmoney-ipt {
            outline: none;
            width: 100px;
            border: 1px solid #B7B7B7;
            margin: 0 8px;
            padding-left: 3px;
            line-height: 2;
        }

        #ssc .lect-num .lect-box {
            margin-top: 0px;
        }

        #ssc .lect-num .balls {
            padding-left: 56px;
        }

        #ssc .lect-box .balls .omit > span {
            margin-right: 11px;
            width: 35px;
        }

        #ssc .lect-box .balls .omit {
            bottom: 0 !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" onsubmit="return false">
        <uc2:WebHead ID="WebHead1" runat="server" />
        <div class="main lottery" id="ssc">
            <!-- 招牌 -->
            <div class="signage">
                <img class="logo" width="65" src="../Images/pc28.png">
                <div class="left">
                    <h1>幸运28</h1>
                    <p>
                        9:05～23:55；每5分钟一期
                    </p>
                </div>
                <div class="right">
                    <a href="###" target="view_window">参与合买</a>| <a href="###"
                        target="_self">定制跟单</a>| <a href="###" target="view_window">开奖</a>| <a href="###" target="view_window">走势图</a>|
                <a href="###" target="view_window">玩法介绍</a>
                </div>
                <div class="count-center">
                    <p>
                        第<strong>0</strong>期，今天已开<strong class="col-red">0</strong>期，还剩<strong class="col-red">0</strong>期
                    </p>
                    <div>
                        <em id="is-text-show">离截止时间还有：</em><span id="is-count-down"><span style="display: none;"><b>00</b> 天 </span><span style="display: none;"><b>00</b> 时 </span><span><b>00</b> 分 </span><span><b>00</b> 秒</span></span>
                    </div>
                </div>
            </div>
            <div class="content clear">
                <div class="left">
                    <!-- 选号区 -->
                    <div class="lect-num border-gray">
                        <!-- 普通/粘贴选项内容切换 -->
                        <div class="lect-box">
                            <!-- 普通 -->
                            <div class="common">
                                <!-- 按号码选 -->
                                <div class="number clear" id="wxzx" prompt="从个、十、百、千、万位各选1个或多个号码，所选号码与开奖号码一一对应，即中奖<span class='col-red'>100000</span>元。">
                                    <div class="balls left" style="width: 697px; margin-top: 60px;">
                                        <ul class="clear">
                                            <li>
                                                <b>00</b>
                                                <b class="green">01</b> <b class="blue">02</b> <b class="red">03</b>
                                                <b class="green">04</b> <b class="blue">05</b> <b class="red">06</b>
                                                <b class="green">07</b> <b class="blue">08</b> <b class="red">09</b>
                                                <b class="green">10</b> <b class="blue">11</b> <b class="red">12</b>
                                                <b>13</b>
                                                <div class="omit miss0">
                                                    <span>288</span><span>188</span><span>88</span><span>58</span><span>38</span><span>28</span><span>13</span>
                                                    <span>13</span><span>13</span><span>13</span><span>13</span><span>13</span><span>12</span><span>12</span>
                                                </div>
                                            </li>
                                            <li><b>14</b>
                                                <b class="red">15</b> <b class="green">16</b> <b class="blue">17</b>
                                                <b class="red">18</b> <b class="green">19</b> <b class="blue">20</b>
                                                <b class="red">21</b> <b class="green">22</b> <b class="blue">23</b>
                                                <b class="red">24</b> <b class="green">25</b> <b class="blue">26</b>
                                                <b>27</b>
                                                <div class="omit miss1">
                                                    <span>12</span><span>12</span><span>13</span><span>13</span><span>13</span><span>13</span><span>13</span>
                                                    <span>13</span><span>28</span><span>38</span><span>58</span><span>88</span><span>188</span><span>288</span>
                                                </div>
                                            </li>
                                            <li style="margin-bottom: 0; height: 50px;">
                                                <a>BIG <span class="bold">大</span>x<i class="rate">2</i></a>
                                                <a>SMALL  <span class="bold">小</span>x<i class="rate">2</i></a>
                                                <a>ODD  <span class="bold">单</span>x<i class="rate">2</i></a>
                                                <a class="right-border">EVEN  <span class="bold">双</span>x<i class="rate">2</i></a>
                                                <div class="omit miss2">
                                                </div>
                                            </li>
                                            <li style="margin-bottom: 0; height: 50px;">
                                                <a class="small"><span>大单</span>x<i class="rate">4.2</i></a>
                                                <a class="small"><span>大双</span>x<i class="rate">4.6</i></a>
                                                <a class="small"><span>小单</span>x<i class="rate">4.6</i></a>
                                                <a class="small"><span>小双</span>x<i class="rate">4.2</i></a>
                                                <a><span class="bold">极大</span>x<i class="rate">15</i></a>
                                                <a class="right-border"><span class="bold">极小 </span>x<i class="rate">15</i></a>
                                                <div class="omit miss2">
                                                </div>
                                            </li>
                                            <li style="margin-bottom: 20px; height: 50px;">
                                                <a><span>豹子</span>x<i class="rate">50</i></a>
                                                <em style="width: 335px;">色波 x <i class="rate">3</i>&nbsp;
                                                    <b class="red">红</b><b class="green">绿</b><b class="blue">蓝</b></em>
                                                <a class="right-border"></a>
                                                <div class="omit miss2">
                                                </div>
                                            </li>
                                        </ul>
                                        <div class="betmoney">
                                            <div style="float: left; width: 250px;">
                                                <p style="height: 60px; line-height: 60px; text-align: center; font-size: 16px;">
                                                    投注金额<input type="text" class="betmoney-ipt" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" maxlength = 6/>
                                                </p>
                                            </div>
                                            <div style="float: left;">
                                                <ul id="paiMoney">
                                                    <li><span>5</span></li>
                                                    <li><span>100</span></li>
                                                    <li><span>500</span></li>
                                                    <li><span>1000</span></li>
                                                    <li><span>5000</span></li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <!-- 确认选号 -->
                        <div class="cp-affirm">
                            <div class="line">
                            </div>
                            <p style="width: 240px;">
                                您当前选择了 <b class="col-red">0</b> 注，共 <b class="col-red">0</b> 元
                            </p>
                            <a class="btn" id="confirm-ball" href="javascript:;"></a>
                        </div>
                        <!-- 选号列表 -->
                        <div class="cp-num-list">
                            <div class="list border-gray" id="touzhu">
                                <ul class="clear" id="number-list">
                                </ul>
                                <div id="random-btn" class="selected_btnbox">
                                    <a class="btn" href="javascript:void(0);" style="margin-top: 10px; margin-left: 12px;">机选1注</a>
                                    <a class="btn" href="javascript:void(0);" style="margin-left: 12px;">机选5注</a>
                                    <a class="btn" href="javascript:void(0);" style="margin-left: 12px;">机选10注</a>
                                    <a class="btn" href="javascript:void(0);" style="margin-left: 12px;">清空列表</a>
                                </div>
                            </div>
                            <div class="number buy-number-1" onselectstart="return false;" id="touzhumoney">
                                共 <b class="col-red">0 </b>注 &nbsp;,&nbsp; 总金额 <b class="col-red">0 </b>元 
                            </div>
                            <div class="moreOperate">
                                <dl class="more_select">
                                    <dt><strong>购买方式：</strong></dt>
                                    <dd>
                                        <label>
                                            <input class="ssq_kk" type="radio" name="operate" id="genbuy" checked="checked">代购</label>
                                    </dd>

                                    <dd class="tips" id="div_gmfstip">购买人自行全额购</dd>
                                    <div class="questionMark">
                                        <div class="pop_prompt">
                                            <div class="pop_con">
                                                <h2>代购：</h2>
                                                <p>
                                                    是指方案发起人自己一人全额认购方案的购彩形式。若中奖，奖金也由发起人一人独享。
                                                </p>
                                            </div>
                                            <div class="arrow">
                                                <s></s><em></em>
                                            </div>
                                        </div>
                                    </div>
                                </dl>
                            </div>
                            <div class="item_hemai hide22" style="display: none;">
                                <div class="item_hemai_cnt">
                                    <div class="cbuy-list">
                                        <b>方案标题：</b>
                                        <input type="text" class="cbuy-inputxx" name="founderAmountInput" value="" maxlength="20"
                                            placeholder="请输入方案标题" id="schemeTile">
                                        <strong>可以填写 <em>20</em> 字</strong>
                                    </div>
                                    <div class="cbuy-list">
                                        <b>方案描述：</b>
                                        <input type="text" class="cbuy-inputxx" name="founderAmountInput" placeholder="请输入方案描述"
                                            value="" maxlength="50" id="schemedescription">
                                        <strong>可以填写 <em>50</em> 字</strong>
                                    </div>
                                    <div class="cbuy-list">
                                        <i class="red">*</i> <b>我要分为：</b>
                                        <input type="text" class="cbuy-input" name="founderAmountInput" value="0" id="totalshare">
                                        份 <span>每份：<em id="em_ShareMoney">0.00</em>元</span> <strong>每份至少1元，且必须能整除到分</strong>
                                    </div>
                                    <div class="cbuy-list">
                                        <i class="red">*</i> <b>我要认购：</b>
                                        <input type="text" class="cbuy-input" name="founderAmountInput" value="0" id="buyshare">
                                        份 <span>共计：<em>0.00</em>元</span> <strong>至少购买<em>0</em>份</strong>
                                    </div>
                                    <div class="cbuy-list">
                                        <b>我要保底：</b>
                                        <input type="text" class="cbuy-input" name="founderAmountInput" value="0" id="assureshare">
                                        份
                                        <label>
                                            <input type="checkbox" onclick="fullGuarantee('totalshare', 'em_ShareMoney', 'buyshare', 'assureshare', 'em_AssureMoney', this)" />全额保底</label>
                                        <span>共计：<em id="em_AssureMoney">0.00</em>元</span> <strong>最多可保底0.00元</strong> <a title="“保底”原意是合买截止时，方案仍未被认购完（进度100％），“保底”承诺者（一般指发起人）以他的帐户来认购完该方案">什么是保底？</a>
                                    </div>
                                    <div class="cbuy-list">
                                        <b>方案提成：</b>
                                        <ul class="cway-list" id="ul_cway_list" runat="server">
                                            <li class="on" value="0" checked="checked">无</li>
                                            <li value="1">1%</li>
                                            <li value="2">2%</li>
                                            <li value="3">3%</li>
                                            <li value="4">4%</li>
                                            <li value="5">5%</li>
                                            <li value="6">6%</li>
                                            <li value="7">7%</li>
                                            <li value="8">8%</li>
                                            <li value="9">9%</li>
                                            <li value="10">10%</li>
                                        </ul>
                                    </div>
                                    <div class="cbuy-list">
                                        <b>保密设置：</b>
                                        <ul class="cbuy-list_baomi">
                                            <li class="cbuy-list_no">不公开</li>
                                            <li class="cbuy-list_gongkai">公开</li>
                                            <li class="cbuy-list_kaihougk">开奖后公开</li>
                                        </ul>
                                    </div>
                                </div>
                                <span class="baomi_zhushi">注：方案进度+保底》+90%，即可出票</span>
                            </div>
                            <div class="moreOperate_con hide22" style="display: none;">
                                <div class="zhuihaoBox">
                                </div>
                            </div>
                            <p class="secrecy_level" style="background-color: #F7F7F7; border: 1px solid #D5D5D5; width: 584px;">
                                <span>方案保密：</span><label><input type="radio" name="way" checked="checked" value="0">不保密</label>
                                <label>
                                    <input type="radio" name="way" value="1">跟单可见</label>
                                <label>
                                    <input type="radio" name="way" value="2">保密到开奖</label>
                                <label>
                                    <input type="radio" name="way" value="3">永久保密</label>
                            </p>
                            <div class="pailie3_submit_but">
                                <div class="submit_buybut" id="divSubmit" runat="server">
                                    <a href="javascript:void(0)" id="lotterySubmit" style="cursor: pointer;" onclick="return CreateLogin('Lottery.Submit()');">立即投注</a>
                                </div>
                                <div class="submit_buybut_u" id="divunSubmit" runat="server">
                                    <a href="javascript:void(0)">暂停销售</a>
                                </div>
                                <p>
                                    <label>
                                        <input type="checkbox" checked="checked" id="tongyi">
                                        我已阅读并同意</label><a href="/Home/Room/BuyProtocol.aspx?LotteryID=28" target="_blank">《用户投注协议》</a>
                                </p>
                            </div>
                        </div>
                    </div>
                    <!-- 今日开奖 -->
                    <div class="today-lottery border-gray hide">
                        <h4>今日开奖号码<span class="col-blue active">收起</span></h4>
                        <div class="list clear">
                            <table cellspacing="0" id="open-list-one">
                                <tr>
                                    <th>期次
                                    </th>
                                    <th>开奖号
                                    </th>
                                    <th>十位
                                    </th>
                                    <th>个位
                                    </th>
                                    <th>后三
                                    </th>
                                </tr>
                            </table>
                            <table cellspacing="0" id="open-list-two">
                                <tr>
                                    <th>期次
                                    </th>
                                    <th>开奖号
                                    </th>
                                    <th>十位
                                    </th>
                                    <th>个位
                                    </th>
                                    <th>后三
                                    </th>
                                </tr>
                            </table>
                            <table cellspacing="0" id="open-list-three">
                                <tr>
                                    <th>期次
                                    </th>
                                    <th>开奖号
                                    </th>
                                    <th>十位
                                    </th>
                                    <th>个位
                                    </th>
                                    <th>后三
                                    </th>
                                </tr>
                            </table>
                            <table cellspacing="0" id="open-list-four">
                                <tr>
                                    <th>期次
                                    </th>
                                    <th>开奖号
                                    </th>
                                    <th>十位
                                    </th>
                                    <th>个位
                                    </th>
                                    <th>后三
                                    </th>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="help right">
                    <div class="new-win border-gray" id="new-zx">
                        <h3>最新资讯</h3>
                        <%=NewsInfo%>
                    </div>
                    <div class="notice border-gray" style="margin-bottom: 10px;">
                        <div class="kjgg_top">
                            <span class="kjgg_top_gongg">开奖公告</span> <a href="../WinQuery/DetailsGaoPin.aspx?ID=28" target="_blank">更多</a>
                        </div>
                        <p id="news-open-info">
                            第<i>00000000-000</i>期 <i>01-01(周日)</i>
                        </p>
                        <p class="num" id="news-open-number">
                            <b>0</b>+<b>0</b>+<b>0</b>=<b>0</b>
                        </p>
                        <p id="sell-status">
                            今日已开 <i>0</i> 期，还剩 <b class="col-red">0</b> 期
                        </p>
                        <table cellspacing="0" id="open-list-news">
                            <tr>
                                <th>期次
                                </th>
                                <th>开奖号码
                                </th>
                                <th>形态
                                </th>
                            </tr>
                            <tr id="loadOpenInfoData">
                                <td rowspan="4" colspan="3">正在获取开奖信息......</td>
                            </tr>
                            <tr class=" hide">
                                <td></td>
                                <td class="num"></td>
                                <td></td>

                                <td></td>
                            </tr>
                            <tr class=" hide">
                                <td></td>
                                <td class="num"></td>
                                <td></td>
                            </tr>
                            <tr class=" hide">
                                <td></td>
                                <td class="num"></td>
                                <td></td>
                            </tr>
                            <tr class=" hide">
                                <td></td>
                                <td class="num"></td>
                                <td></td>
                            </tr>
                            <tr class=" hide">
                                <td></td>
                                <td class="num"></td>
                                <td></td>
                            </tr>
                            <tr class=" hide">
                                <td></td>
                                <td class="num"></td>
                                <td></td>
                            </tr>
                            <tr class=" hide">
                                <td></td>
                                <td class="num"></td>
                                <td></td>
                            </tr>
                            <tr class=" hide">
                                <td></td>
                                <td class="num"></td>
                                <td></td>
                            </tr>
                            <tr class=" hide">
                                <td></td>
                                <td class="num"></td>
                                <td></td>
                            </tr>
                            <tr class=" hide">
                                <td></td>
                                <td class="num"></td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                    <div class="new-win border-gray">
                        <h3>最新中奖</h3>
                        <%=WinRanking%>
                    </div>

                </div>
            </div>
        </div>
        <uc1:WebFoot ID="WebFoot1" runat="server" />
        <input id="hidMultiple" type="hidden" runat="server" />
        <asp:HiddenField runat="server" ID="labInitiateSchemeMinBuyAndAssureScale" />
    </form>
</body>
</html>
<script src="../JScript/jquery-1.8.3.min.js" type="text/javascript"></script>
<script src="bjxy28.js?v=1.1.1" type="text/javascript"></script>
<script src="../JScript/jBox/jquery.jBox-2.3.min.js" type="text/javascript"></script>
<script src="../JScript/artDialog/dialog-min.js" type="text/jscript"></script>
<script src="../JScript/artDialog/dialog-plus-min.js" type="text/javascript"></script>

<%@ Application Language="C#" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        //在应用程序启动时运行的代码
        //CPS推广注册码 44a1s.html
        //string myurl = HttpContext.Current.Request.Url.ToString();
        //string[] result = myurl.Split('/');
        //if (result[result.Length - 1].Length == 10 && result[result.Length - 1].Contains(".html"))
        //{
        //    DAL.Tables.T_Cps tcps = new DAL.Tables.T_Cps();
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    dt = tcps.Open("id", "SerialNumber='" + result[result.Length - 1].Split('.')[0].ToString() + "'", "");
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        string strurl = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/UserReg.aspx?cpsId=" + dt.Rows[0][0].ToString() + "";
        //        HttpContext.Current.Response.Redirect(strurl, true);
        //    }
        //}
    }

    void Application_BeginRequest(object sender, EventArgs e)
    {
        try
        {
            //过滤请求的服务器的文件（暂时不使用，原因：影响到了CPS推广用户的注册功能）
            //string tempFile = allowFiles();
            //string fileName = Request.Url.AbsoluteUri.Split('/')[Request.Url.AbsoluteUri.Split('/').Length - 1];
            //if (fileName.Contains("?"))
            //{
            //    fileName = fileName.Substring(0, fileName.IndexOf('?'));
            //}
            //if (!tempFile.ToLower().Contains("," + fileName.ToLower() + ",") && !fileName.ToLower().Contains(".ashx"))
            //{
            //    Response.End();
            //    Response.Clear();
            //}
            Shove._Web.Security.InjectionInterceptor.Run();
        }
        catch
        {

        }
    }

    void Application_End(object sender, EventArgs e)
    {
        //在应用程序关闭时运行的代码

    }

    void Application_Error(object sender, EventArgs e)
    {
        //CPS推广注册码 44a1s.aspx
        string myurl = HttpContext.Current.Request.Url.ToString();
        string[] result = myurl.Split('/');
        if (result[result.Length - 1].Length == 10 && result[result.Length - 1].Contains(".aspx"))
        {
            //DAL.Tables.T_Cps tcps = new DAL.Tables.T_Cps();
            //System.Data.DataTable dt = new System.Data.DataTable();
            //dt = tcps.Open("id", "SerialNumber='" + result[result.Length - 1].Split('.')[0].ToString() + "'", "");
            //if (dt != null && dt.Rows.Count > 0)
            //{
            string url = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;
            url = url + "/UserReg.aspx?flag=" + Shove._Security.Encrypt.Encrypt3DES(PF.GetCallCert(), result[result.Length - 1].Split('.')[0].ToString(), PF.DesKey); ;
          
            HttpContext.Current.Response.Redirect(url, true);
            //}
        }

        //在出现未处理的错误时运行的代码
        Exception objErr = Server.GetLastError().GetBaseException();

        if (objErr == null)
        {
            Server.ClearError();

            return;
        }

        string Url = "空";
        try
        {
            Url = HttpContext.Current.Request.Url.ToString();
        }
        catch { }

        string ErrorMsg = "Error, PageName: " + Url + "。 ErrorMsg: " + objErr.Message + " Source:" + objErr.Source + " StackTrace:" + objErr.StackTrace + "。";

        new Log("System").Write(ErrorMsg);

    }

    void Session_Start(object sender, EventArgs e)
    {
        //在新会话启动时运行的代码


    }

    void Session_End(object sender, EventArgs e)
    {
        //在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式 
        //设置为 StateServer 或 SQLServer，则不会引发该事件。

    }
    /// <summary>
    /// 允许通过的文件
    /// </summary>
    /// <returns></returns>
    string allowFiles()
    {
        return ",Default.aspx,AccountDetails.aspx,Addaward.aspx,AddawardEdit.aspx,AddawardList.aspx,AppAutoUpdate.aspx,AppPicManage.aspx,AppUpdateLogDetails.aspx,BettingAgreement.aspx,Bjdcgg.aspx,BjdcggEdit.aspx,ChaseDetail.aspx,ChaseDetails.aspx,ChaseList.aspx,Competence.aspx,CompetenceAdd.aspx,CompetenceEdit.aspx,ContactUs.aspx,CPSPicManage.aspx,Default.aspx,Extension.aspx,FinanceAddMoney.aspx,FinanceBalance.aspx,FinanceDistill.aspx,FinanceWin.aspx,FrameBottom.aspx,FrameLeft.aspx,FrameTop.aspx,Handsel.aspx,HandselAddOrEdit.aspx,ImageNews.aspx,ImageNewsAdd.aspx,ImageNewsEdit.aspx,InputWinNumber.aspx,Error.aspx,Success.aspx,IssueForSFC.aspx,Isuse.aspx,IsuseAdd.aspx,IsuseAdd3.aspx,IsuseAddForBJDC.aspx,IsuseAddForKeno.aspx,IsuseEdit.aspx,Jclcdg.aspx,JclcdgEdit.aspx,Jclcgg.aspx,JclcggEdit.aspx,Jczcdg.aspx,JczcdgEdit.aspx,Jczcgg.aspx,JczcggEdit.aspx,JczcsgEdit.aspx,Jczcsglist.aspx,Jczcsgsave.aspx,LoginCount.aspx,LoginLog.aspx,LotteryInformation.aspx,LotteryRecommend.aspx,LotteryTimeSet.aspx,MonitoringLog.aspx,News.aspx,NewsAdd.aspx,NewsEdit.aspx,NotificationOptions.aspx,NotificationTemplates.aspx,OnlinePayGateway2.aspx,AlipayNotify.aspx,Open.aspx,OpenManualByJC.aspx,OptionEmail.aspx,Options2.aspx,PackageList.aspx,Personages.aspx,PersonagesAdd.aspx,PersonagesEdit.aspx,PrintOutput.aspx,ProcessingMoney.aspx,PromotionAlliance.aspx,PushOption.aspx,PushSendMessage.aspx,PushTemplate.aspx,RegisterAgreement.aspx,Scheme.aspx,SchemeAtTop2.aspx,SchemeList.aspx,SchemeQuash2.aspx,SendEmail.aspx,SendEmailList.aspx,SendSMS.aspx,SendSMSList.aspx,SendStationSMS.aspx,Sensitivekeywords.aspx,SetPoints.aspx,SetSMSOptions.aspx,SetWinDetailsForIssue.aspx,Site2.aspx,SiteAffiches.aspx,SiteAffichesAdd.aspx,SiteAffichesEdit.aspx,SiteImageManage.aspx,StationSMSList.aspx,UserAccountDetail.aspx,UserAddHandsel.aspx,UserAddMoney.aspx,UserDetail.aspx,UserDistill.aspx,UserDistillGeneralLedger.aspx,UserDistillSuccess.aspx,UserDistillUnsuccess.aspx,UserDistillWaitPay.aspx,UserFeedbacks.aspx,UserIntegralDetail.aspx,Users.aspx,Welcome.aspx,WinList.aspx,YeJiao.aspx,YejiaoAdd.aspx,YeJiaoEdit.aspx,YSS_Condition.aspx,CheckUserName.aspx,UserLogin.aspx,Buy_BQC.aspx,Buy_DS.aspx,Buy_RQSPF.aspx,Buy_ZJQS.aspx,Buy_ZQBF.aspx,about.aspx,contact.aspx,FriendLink.aspx,job.aspx,SiteMap.aspx,zlhz.aspx,download.aspx,My97DatePicker.htm,Welcome.htm,Confirm.aspx,AgentAuditing.aspx,AgentAuditingNotOK.aspx,AgentManagement.aspx,AgentThePromoteManagement.aspx,Auditing.aspx,AuditingRecord.aspx,CommissionDetail.aspx,CommissionDetailTwo.aspx,CommissionEdit.aspx,CommissionGiveOut.aspx,CommissionGiveOutTwo.aspx,CommissionManagement.aspx,CommissionScale.aspx,CommissionScale2.aspx,CommissionScaleRecord.aspx,CPSAccountBook.aspx,MemberList.aspx,PromoteAuditing.aspx,PromoteAuditingNotOK.aspx,PromoteList.aspx,PromoteManagement.aspx,SystemParamSet.aspx,UserBuyLotteryDetails.aspx,UserTransfer.aspx,UserTransferRecord.aspx,UserTransferTwo.aspx,AgentAddPromote.aspx,AgentCommission.aspx,AgentCommissionSet.aspx,AgentDate.aspx,AgentIndex.aspx,AgentNumber.aspx,AgentPromoteList.aspx,AgentPromoteNumber.aspx,AgentUserBuyLotteryDetails.aspx,CommissionEdit.aspx,CommissionScale2.aspx,CommissionScaleRecord.aspx,Apply_Promote.aspx,Audit.aspx,Contact.aspx,Default.aspx,MobileValided.aspx,News.aspx,New_View.aspx,NotCPS.aspx,PromoteAddUser.aspx,PromoteCommission.aspx,PromoteDate.aspx,PromoteIndex.aspx,PromoteNumber.aspx,UserBuyLotteryDetails.aspx,PromotionAgeement.aspx,AdministrationTop.ascx,AgentAccount.ascx,AgentHeader.ascx,Footer.ascx,IndexHeader.ascx,PromoteAccount.ascx,PromoteHeader.ascx,CqsscOrder.aspx,Default.aspx,Default.aspx,Default.aspx,Error.aspx,ForgetPassword.aspx,Default.aspx,GD11X5Order.aspx,Default.aspx,Default.aspx,DefaultJCLQ.aspx,DLT.aspx,DLTWinDetail.aspx,SFC.aspx,SFCBetDistribution.aspx,SSQ.aspx,SSQWinDetail.aspx,help_account_safe.aspx,help_Account_Security.aspx,help_Analyse_Miss.aspx,help_BankAndAlipay.aspx,help_Buy.aspx,help_buy_distill.aspx,help_card_pwd.aspx,help_Chase.aspx,help_ChaseAndBuy.aspx,help_cobuy.aspx,help_Cobuy1.aspx,help_Commission.aspx,help_Community.aspx,Help_Default.aspx,help_Double.aspx,help_DoubleAndChase.aspx,help_Draw_Money.aspx,help_Email.aspx,help_FollowFriendScheme.aspx,help_followscheme.aspx,help_HotAndCool.aspx,help_Invest.aspx,help_Login.aspx,help_LogOut.aspx,help_Miss.aspx,help_NewsPaper.aspx,help_Prize.aspx,help_Scheme.aspx,help_scheme_safe.aspx,help_Send.aspx,help_TrendChart.aspx,help_UserReg.aspx,help_viewbuy.aspx,play_1.aspx,play_13.aspx,play_15.aspx,play_2.aspx,Play_28.aspx,play_3.aspx,play_39.aspx,play_45.aspx,play_5.aspx,play_6.aspx,play_61.aspx,play_62.aspx,play_63.aspx,play_64.aspx,play_70.aspx,play_72.aspx,play_73.aspx,play_74.aspx,play_75.aspx,play_78.aspx,Play_83.aspx,AccountAddMoney.aspx,AccountDetail.aspx,AccountDetails.aspx,AccountDrawMoney.aspx,AccountFreezeDetail.aspx,AccountHandselMoney.aspx,AccountWinMoney.aspx,BindBankCard.aspx,BuyConfirm.aspx,BuyProtocol.aspx,AHFC15X5.htm,DF6J1.htm,DLT.htm,FC3D.htm,QLC.htm,SSQ.htm,SZPL3.htm,SZPL5.htm,TC22X5.htm,Chase.aspx,ChaseDetail.aspx,ChaseExecutedSchemes.aspx,ChaseLogin.aspx,DelMessage.aspx,DingZhiGenDan.aspx,Distill.aspx,DistillByAlipay.aspx,DistillBybank.aspx,DistillDetail.aspx,DistillDetailHandsel.aspx,DistillFee.aspx,EditPassWord.aspx,EmailReg.aspx,FollowScheme.aspx,FollowSchemeHistory.aspx,Retrieve_password.aspx,Retrieve_password_thress.aspx,Retrieve_password_two.aspx,Retrieve_password_two_2.aspx,help.htm,help_1.htm,help_13.htm,help_15.htm,help_1_9.htm,help_2.htm,help_29.htm,help_3.htm,help_39.htm,help_41.htm,help_5.htm,help_58.htm,help_59.htm,help_6.htm,help_61.htm,help_62.htm,help_63.htm,help_64.htm,help_65.htm,help_68.htm,help_69.htm,help_70.htm,help_77.htm,help_78.htm,help_9.htm,Invest.aspx,JCPreBet.aspx,Login.aspx,Message.aspx,MyAttention.aspx,MyFollowScheme.aspx,Default.aspx,PayQuery.aspx,QueryResult.aspx,Receive.aspx,Receive1.aspx,Send.aspx,Receive.aspx,Send.aspx,AlipayNotify.aspx,Default.aspx,Receive.aspx,Send.aspx,Send2.aspx,AlipayNotify.aspx,Default.aspx,Receive.aspx,Send.aspx,Send0.aspx,Send1.aspx,Send2.aspx,Send_99Bill.aspx,Send_Alipay.aspx,Send_CellPay.aspx,Send_CFT.aspx,Send_Default.aspx,Send_GFB.aspx,Send_SFT.aspx,Send_SZX.aspx,Send_YeePay.aspx,Send_YL.aspx,CallBack.aspx,Notify.aspx,Trade.aspx,CardPasswordValid.aspx,Default.aspx,AutoReceive.aspx,Receive.aspx,Send.aspx,ReceiveClient.aspx,SendClient.aspx,BodyCharge.aspx,Receive.aspx,Send.aspx,Default.aspx,DefaultClinet.aspx,Fail.aspx,FailClient.aspx,Receive.aspx,send.aspx,Notify.aspx,Receive.aspx,Send.aspx,Receive.aspx,Send.aspx,OK.aspx,OKClient.aspx,Default.aspx,notifySFT.aspx,pageUrlSFT.aspx,SendOrderSFT.html,SendOrderSFT2.aspx,PayQuery.aspx,QueryResult.aspx,Receive.aspx,Send.aspx,purchase.aspx,query.aspx,Receive.aspx,refund.aspx,void.aspx,Receive.aspx,Send.aspx,purchase.aspx,query.aspx,Receive.aspx,BackReceive.aspx,FrontReceive.aspx,Send.aspx,Default.aspx,PayQuery.aspx,Receive.aspx,Send.aspx,HeadGlide.aspx,SafeSet.aspx,Scheme.aspx,SchemeAll.aspx,SchemeUpload.aspx,SchemeUploadAfter.aspx,ScoringChange.aspx,ScoringDetail.aspx,FloatNotify.html,UserBuySuccess.aspx,Index_banner.ascx,ShowAccountBalance.ascx,TrendChartHead.ascx,UserMyIcaile.ascx,WebFoot.ascx,WebHead.ascx,WebTheusercenter.ascx,UserEdit.aspx,UserEmailBind.aspx,UserMobileBind.aspx,UserRegSuccess.aspx,ViewAccount.aspx,ViewChase.aspx,ViewMessage.aspx,DownloadSchemeFile.aspx,Score.aspx,ShowAffiches.aspx,ShowAffichesList.aspx,ShowNews.aspx,ShowNewsPage.aspx,ShowNewSpecial.aspx,FloatNotify.html,UserRegAgree.aspx,Buy_DX.aspx,Buy_DX_DG.aspx,Buy_Mixed_Pass.aspx,Buy_RFSF.aspx,Buy_RFSF_DG.aspx,Buy_SF.aspx,Buy_SFC.aspx,Buy_SFC_DG.aspx,Buy_SF_DG.aspx,BuyConfirm.aspx,Buy_BQC.aspx,Buy_BQC_DG.aspx,Buy_DGGD.aspx,Buy_Mixed_Pass.aspx,Buy_RQSPF.aspx,Buy_RQSPF_DG.aspx,Buy_SPF.aspx,Buy_SPF_DG.aspx,Buy_ZJQS.aspx,Buy_ZJQS_DG.aspx,Buy_ZQBF.aspx,Buy_ZQBF_DG.aspx,dt.aspx,FilterShrink.aspx,Scheme.aspx,Default.aspx,Default.aspx,JSK3Order.aspx,JSK3_EBTH.htm,JSK3_ETHDX.htm,JSK3_ETHFX.htm,JSK3_HZ.htm,JSK3_SBTH.htm,JSK3_SLHTX.htm,JSK3_STHDX.htm,JSK3_STHTX.htm,Default.aspx,JX11X5Order.aspx,Default.aspx,JxsscOrder.aspx,index.html,map.html,map.html,1.html,2.html,3.html,LotteryPackage.aspx,Mobile.aspx,Default.aspx,List.aspx,View.aspx,NotFound.aspx,PasswordValid.aspx,Default.aspx,Default.aspx,Default.aspx,Default.aspx,regcode.aspx,Default.aspx,RJC_HMSchemes.aspx,Default.aspx,SFC_HMSchemes.aspx,Default.aspx,BuildDataImage.aspx,BuildImage.aspx,Default.aspx,stoneTest.htm,Default.aspx,SYYDJOrder.aspx,Test.aspx,TestOrder.aspx,Default.aspx,HelpCenter.ascx,UserLogin.aspx,UserReg.aspx,DetailsGaoPin.aspx,DetailsLzq.aspx,OpenInfoList.aspx,3D.aspx,CQSSC.aspx,DLT.aspx,GD11X5.aspx,JSK3.aspx,JX11X5.aspx,JXSSC.aspx,SSQ.aspx,SYYDJ.aspx,LotterySwitch.ascx,,";
    } 
</script>

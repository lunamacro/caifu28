<%@ WebHandler Language="C#" Class="Common" %>

using System;
using System.Web;

public class Common : IHttpHandler {

    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    String appKey = "pgyu6atqp98xu";
    String appSecret = "9ziqHNjO6ZY";
    
    public void ProcessRequest (HttpContext context) {
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");
        context.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST");
        context.Response.Clear();
        context.Response.ContentType = "application/json";


        string opt = Shove._Web.Utility.GetRequest("opt");

        if (string.IsNullOrEmpty(opt))
        {
            context.Response.Clear();

            sb.Append("{\"error\":\"-101\",");
            sb.Append("\"msg\":\"缺少opt\"}");
            context.Response.Write(sb);
            return;
        }
        

        //获取服务器时间
        if (opt.Equals("1"))
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(DateTime.Now - startTime).TotalSeconds; // 相差秒数
            
            sb.Append("{\"error\":\"0\",");
            sb.Append("\"msg\":" + timeStamp + "}");
            context.Response.Write(sb);
            return;
        }

        //获取APP轮播图片
        #region 获取APP轮播图片
        if (opt.Equals("2"))
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "/Uploadfile/AppPic";
            string[] fileArray = System.IO.Directory.GetFiles(path);
            System.Text.StringBuilder picSb = new System.Text.StringBuilder();
            for (int i = 0; i < fileArray.Length; i++)
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileArray[i]);
                
                string title = "";
                string method = "";
                string content = "";
                
                if (fileInfo.Name.IndexOf("beij28_wanfa")>=0)
                {
                    title = "北京28玩法说明";
                    method = "WebView";
                    content = "http://www.300h.cn/BJXY28/html/gameshows.html";
                }
                if (fileInfo.Name.IndexOf("jnd28_wanfa") >= 0)
                {
                    title = "加拿大28玩法说明";
                    method = "WebView";
                    content = "http://www.300h.cn/JND28/html/gameshows.html";
                }
                if (fileInfo.Name.IndexOf("share") >= 0)
                {
                    title = "VIP分享";
                    method = "App";
                    content = "share";
                }

                picSb.Append("{\"picUrl\":\"" + "http://" + context.Request.Url.Authority + "/Uploadfile/AppPic/" + fileInfo.Name + "\",\"title\":\"" + title + "\",\"method\":\"" + method + "\",\"content\":\"" + content + "\"},");
                
                
            }
            if (picSb.Length > 0)
            {
                picSb = picSb.Remove(picSb.Length - 1, 1);
            }
           
            sb.Append("{");
            sb.Append("\"error\":\"0\",");
            sb.Append("\"msg\":\"数据获取成功\",");
            sb.Append("\"picList\":[" + picSb.ToString() + "]");
            sb.Append("}");
            context.Response.Write(sb);
            return;
        }
        #endregion

        //获取大厅和房间人数
        #region 获取大厅和房间人数
        if (opt.Equals("3"))
        {
            string LotteryId = context.Request["LotteryId"];

            int ranNumber = 0; //在真实人数基础上增加指定人数
            int onlineNumber = 300;   //基础人数
            Random rnd = new Random();

            sb.Append("[");
            for (int i = 0; i < 3; i++ )
            {
                sb.Append("[");
                int sum = 0;
                for (int j = 0; j < 4; j++)
                {
                    ranNumber = rnd.Next(1, 200);
                    int pNum = onlineNumber + ranNumber;
                    sum += pNum;
                    sb.Append("\""+pNum+"\",");
                }
                sb.Append("\""+sum+"\"]");
                if(i<2)
                {
                    sb.Append(",");
                }
                onlineNumber -= 100;
                
            }
            sb.Append("]");

            System.Text.StringBuilder sblimit = new System.Text.StringBuilder();
            string sql = @"SELECT * From T_BetLimit WHERE lotteryId=" + LotteryId + " order by paixu";
            System.Data.DataTable dt = Shove.Database.MSSQL.Select(sql);
            sblimit.Append("[");
            int ri = 0;
            foreach (System.Data.DataRow row in dt.Rows)
            {
                sblimit.Append("[\"" + row["limit_min"] + "\",\"" + row["limit_max"] + "\",\"" + row["limit_max_all"] + "\"]");
                ri++;
                if (ri < dt.Rows.Count)
                {
                    sblimit.Append(",");
                }
               
            }
            sblimit.Append("]");


            context.Response.Write(buildCallBackObj(true, sb.ToString(), sblimit.ToString()));
            context.ApplicationInstance.CompleteRequest();
            context.Response.End();


        }
        #endregion



        //获取融云Token
        #region 获取融云Token
        if (opt.Equals("4"))
        {
            string strRongUser = context.Request.Form["info"];

            donet.io.rong.models.RongUser rongUser;
            try
            {
                rongUser = (donet.io.rong.models.RongUser)Newtonsoft.Json.JsonConvert.DeserializeObject(strRongUser, typeof(donet.io.rong.models.RongUser));
            }
            catch
            {
                context.Response.Clear();
                context.Response.Write(buildCallBack(false, "异常请求"));
                context.ApplicationInstance.CompleteRequest();
                return;
            }
            if (rongUser == null)
            {
                context.Response.Write(buildCallBack(false, "The rongUser is null"));
                context.ApplicationInstance.CompleteRequest();
                return;
            }

            
            donet.io.rong.models.TokenReslut tokenReslut = null;
            donet.io.rong.methods.User user = new donet.io.rong.methods.User(appKey, appSecret);

            System.Data.DataTable dt = Shove.Database.MSSQL.Select("select * from T_UserToken where UserID=" + rongUser.UserId);
            if (null == dt || dt.Rows.Count == 0)
            {
                tokenReslut = user.getToken(rongUser.UserId, rongUser.Name, rongUser.PortraitUri);
                try
                {
                    string addSql = "insert into T_UserToken([userId],[token]) values(" + rongUser.UserId + ",'" + tokenReslut.getToken() + "')";
                    int result = Shove.Database.MSSQL.ExecuteNonQuery(addSql);
                    if (result < 0)
                    {
                        context.Response.Write(buildCallBack(false, "the userId is not exist"));
                        context.ApplicationInstance.CompleteRequest();
                        context.Response.End();
                        return;
                    }
                }
                catch (Exception e)
                {
                    string errorMsg = e.Message;
                }
            }
            else
            {
                tokenReslut = new donet.io.rong.models.TokenReslut(200, dt.Rows[0]["Token"].ToString(), rongUser.UserId, "");
            }
            if (null != tokenReslut)
            {
                context.Response.Write(buildCallBack(true, tokenReslut.getToken()));
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();
                return;

            }

            context.Response.Write(buildCallBack(false, "The tokenReslut is null"));
            context.ApplicationInstance.CompleteRequest();
            context.Response.End();

        }
        #endregion


        //加入融云群组
        #region 加入融云群组
        if (opt.Equals("5"))
        {

            string userId = context.Request.Form["userId"];
            string groupId = context.Request.Form["groupId"];
            string groupName = context.Request.Form["groupName"];
            
            string sql = "";
            if (userId == null || userId.Equals(""))
            {
                context.Response.Write(buildCallBack(false, "userId is null"));
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();
                return;
            }

            if (groupId == null || groupId.Equals(""))
            {
                context.Response.Write(buildCallBack(false, "groupId is null"));
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();
                return;
            }
            if (groupName == null || groupName.Equals(""))
            {
                context.Response.Write(buildCallBack(false, "groupName is null"));
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();
                return;
            }


            long queryUserResult = new DAL.Tables.T_Users().GetCount("ID=" + userId);

            if (queryUserResult == 0)
            {
                context.Response.Write(buildCallBack(false, "the user is not exist"));
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();
                return;
            }
            else
            {
                donet.io.rong.methods.Group group = new donet.io.rong.methods.Group(appKey, appSecret);
                string[] userIdArray = { userId };
                donet.io.rong.models.CodeSuccessReslut codeSuccessResult = group.join(userIdArray, groupId, groupName);
                if (codeSuccessResult.getCode() == 200)
                {
                    context.Response.Write(buildCallBack(true, "200"));
                }
                else
                {
                    context.Response.Write(buildCallBack(false, codeSuccessResult.getErrorMessage()));
                }

                context.ApplicationInstance.CompleteRequest();
                context.Response.End();
            }


            context.Response.Write(buildCallBack(false, "error"));
            context.ApplicationInstance.CompleteRequest();
            context.Response.End();
        }
        #endregion


        //退出融云群组
        #region 退出融云群组
        if (opt.Equals("6"))
        {

            string userId = context.Request.Form["userId"];
            string groupId = context.Request.Form["groupId"];

            string sql = "";
            if (userId == null || userId.Equals(""))
            {
                context.Response.Write(buildCallBack(false, "userId is null"));
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();
                return;
            }

            if (groupId == null || groupId.Equals(""))
            {
                context.Response.Write(buildCallBack(false, "groupId is null"));
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();
                return;
            }

            long queryUserResult = new DAL.Tables.T_Users().GetCount("ID=" + userId);

            if (queryUserResult == 0)
            {
                context.Response.Write(buildCallBack(false, "the user is not exist"));
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();
                return;
            }
            else
            {
                donet.io.rong.methods.Group group = new donet.io.rong.methods.Group(appKey, appSecret);
                string[] userIdArray = { userId };
                donet.io.rong.models.CodeSuccessReslut codeSuccessResult = group.quit(userIdArray, groupId);
                if (codeSuccessResult.getCode() == 200)
                {
                    context.Response.Write(buildCallBack(true, "200"));
                }
                else
                {
                    context.Response.Write(buildCallBack(false, codeSuccessResult.getErrorMessage()));
                }

                context.ApplicationInstance.CompleteRequest();
                context.Response.End();
            }


            context.Response.Write(buildCallBack(false, "error"));
            context.ApplicationInstance.CompleteRequest();
            context.Response.End();
        }
        #endregion

        //获取我的购买列表
        #region 获取我的购买列表
        if (opt.Equals("7"))
        {
            string userId = context.Request.Form["uid"];
            string lotteryId = context.Request.Form["lotteryId"];
            string pageIndex = context.Request.Form["pageIndex"];
            
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            long UserID = Shove._Convert.StrToLong(userId, -1);

            Users _User = new Users(1)[1, UserID];
            if (_User == null)
            {
                sb.Append("{\"error\":\"-109\",");
                sb.Append("\"msg\":\"异常用户信息\"}");
                new Log("AppGateway_Exception").Write("异常用户信息_4502");
                context.Response.Write(sb.ToString());
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();
            }

            int PageIndex = Shove._Convert.StrToInt(pageIndex, -1);
            int PageSize = 10;
            int Sort = 5;
            int SortType = 0;
            int Status = 0;
            int IsPurchasing = 1;

            if (string.IsNullOrEmpty(lotteryId))
            {
                sb.Append("{\"error\":\"-109\",");
                sb.Append("\"msg\":\"彩种ID不能为空\"}");

                new Log("AppGateway_Exception").Write("彩种ID不能为空_4554");
                context.Response.Write(sb.ToString());
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();

            }

            if (PageIndex < 0)
            {
                sb.Append("{\"error\":\"-109\",");
                sb.Append("\"msg\":\"页码参数错误\"}");

                context.Response.Write(sb.ToString());
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();

            }

            if (PageSize <= 0)
            {
                sb.Append("{\"error\":\"-109\",");
                sb.Append("\"msg\":\"每页数据条数参数错误\"}");

                context.Response.Write(sb.ToString());
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();

            }

            if (Sort < 0)
            {
                sb.Append("{\"error\":\"-109\",");
                sb.Append("\"msg\":\"排序参数错误\"}");

                context.Response.Write(sb.ToString());
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();

            }

            string strSort = "";
            switch (Sort)
            {
                case 0:
                    strSort = "Schedule " + (SortType == 0 ? "desc" : "asc");
                    break;
                case 1:
                    strSort = "[Money] " + (SortType == 0 ? "desc" : "asc");
                    break;
                case 2: //每份金额
                    strSort = "Round((Money / Share), 2) " + (SortType == 0 ? "desc" : "asc");
                    break;
                case 3:
                    strSort = "AtTopStatus " + (SortType == 0 ? "desc" : "asc");
                    break;
                case 4:
                    strSort = "[Level] " + (SortType == 0 ? "desc" : "asc");
                    break;
                case 5:
                    strSort = "[Datetime] " + (SortType == 0 ? "desc" : "asc");
                    break;
                default:
                    break;
            }

            string strCondition = "";
            strCondition = "UserID=" + UserID.ToString();
            if (!lotteryId.Equals("-1"))
            {
                strCondition += " and LotteryID in (" + Shove._Web.Utility.FilteSqlInfusion(lotteryId) + ")";
            }

            if (Status == 1)
            {
                //已中奖
                strCondition += " and SchemeIsOpened = 1 and WinMoney > 0";
            }
            else if (Status == 2)
            {
                //未开奖
                strCondition += " and SchemeIsOpened = 0 and quashStatus=0";
            }
            else if (Status == 3)
            {
                //追号
                strCondition += " and isChase = 1";
            }
            else if (Status == 4)
            {
                //合买
                strCondition += " and isPurChasing = 0";//1表示代购 0 表示合买
            }

            int TotalRowCount = 0;
            int PageCount = 0;

            //获取方案
            System.Data.DataSet ds = null;
            int intResult = DAL.Procedures.P_Pager(ref ds, PageIndex, PageSize, 0,
                "UserID,id, initiateUserID,0 as joinUserID,myBuyMoney,myBuyShare, schemeNumber, initiateName,[datetime], money," +
                "Round((Money / Share), 2) as shareMoney, share, (Share - BuyedShare) as surplusShare, Cast((AssureMoney / (Money / Share)) as int) as assureShare," +
                "assureMoney, multiple, schemeBonusScale, secrecyLevel, quashStatus, level, isnull(winMoneyNoWithTax, 0) as winMoneyNoWithTax, schedule, schemeIsOpened," +
                "title,description, replace(lotteryNumber, ' + ', '-') as lotteryNumber, replace(WinLotteryNumber, ' + ', '-') as winNumber, buyed, isPurchasing, lotteryID," +
                "lotteryName,isuseID, isuseName,isChase,chaseTaskID,fromClient,isOpened,winMoney,buyedShare,detailMoney,handselMoney,ISNULL(IsPreBet,0) AS isPreBet",
                "V_Schemes_UserDetail", strSort, strCondition, "ID", ref TotalRowCount, ref PageCount);

            if (intResult < 0)
            {

                sb.Append("{\"error\":\"-110\",");
                sb.Append("\"msg\":\"无符合条件的信息\"}");

                context.Response.Write(sb.ToString());
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();

            }

            if (ds == null)
            {

                sb.Append("{\"error\":\"-111\",");
                sb.Append("\"msg\":\"未知错误\"}");
                context.Response.Write(sb.ToString());
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();

            }

            
            

            if (ds.Tables.Count <= 0)
            {

                sb.Append("{\"error\":\"-111\",");
                sb.Append("\"msg\":\"未知错误\"}");
                context.Response.Write(sb.ToString());
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();

            }

            System.Data.DataTable dtDetail = ds.Tables[0].Clone();
            dtDetail.Columns["DateTime"].DataType = typeof(String);
            Sites _sites = new Sites()[1];
            int zifuNum = Convert.ToInt32(_sites.SiteOptions["Opt_UserControlHiedAndShow"].ToString(""));
            foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
            {
                System.Data.DataRow drNews = dtDetail.NewRow();
                drNews.ItemArray = dr.ItemArray;
                string strTemps = string.Format("{0:yyyy-MM-dd}", Shove._Convert.StrToDateTime(drNews["DateTime"].ToString(), "1999-01-01"));
                drNews["DateTime"] = strTemps;
                if (zifuNum > 0 && dr["InitiateUserID"].ToString() != userId)
                {
                    drNews["InitiateName"] = dr["InitiateName"].ToString().Substring(0, zifuNum) + "***";
                }
                drNews["myBuyMoney"] = drNews["detailMoney"];
                drNews["myBuyShare"] = Convert.ToInt32(drNews["detailMoney"]) / Convert.ToInt32(drNews["shareMoney"]);
                dtDetail.Rows.Add(drNews);
            }

            //取购买明细表
            string strSchemeIDs = "";
            foreach (System.Data.DataRow dr in dtDetail.Rows)
            {
                strSchemeIDs += "," + dr["Id"].ToString();
            }

            if (!strSchemeIDs.Equals(""))
            {
                strSchemeIDs = strSchemeIDs.Remove(0, 1);
            }

            System.Data.DataTable dtBuyDetails = GetBuyDetails(strSchemeIDs);

            //根据下注日期分组
            System.Data.DataView dvSchemList = dtDetail.DefaultView;
            string[] strColumn = new string[] { "datetime" };
            System.Data.DataTable dtNewOne = dvSchemList.ToTable(true, strColumn);
            System.Data.DataSet dsNew = new System.Data.DataSet();
            if (dtDetail.Rows.Count <= 0)
            {
                dsNew.Tables.Add(dtDetail);
            }
            int count = 0;
            foreach (System.Data.DataRow drOne in dtNewOne.Rows)
            {
                count++;
                System.Data.DataRow[] drSelect = dtDetail.Select("datetime='" + drOne["datetime"] + "'");//获取重复的日期
                System.Data.DataTable dtNewTwo = dtDetail.Clone();//复制表结构
                foreach (System.Data.DataRow drTwo in drSelect)
                {
                    dtNewTwo.Rows.Add(drTwo.ItemArray);//添加数据到新建表中
                }
                dtNewTwo.TableName = "dt" + count;
                dsNew.Tables.Add(dtNewTwo);
            }

            dsNew.Tables[0].Columns["SerialNumber"].ColumnName = "serialNumber";
            dsNew.Tables[0].Columns["RecordCount"].ColumnName = "recordCount";

            //添加方案状态
            for (int j = 0; j < dsNew.Tables.Count; j++)
            {
                dsNew.Tables[j].Columns.Add("schemeStatus", typeof(string));
                for (int i = 0; i < dsNew.Tables[j].Rows.Count; i++)
                {
                    string schemeStatus = PF.SchemeState(Convert.ToInt32(dsNew.Tables[j].Rows[i]["Share"]), Convert.ToInt32(dsNew.Tables[j].Rows[i]["BuyedShare"]), Convert.ToBoolean(dsNew.Tables[j].Rows[i]["Buyed"]), Convert.ToInt32(dsNew.Tables[j].Rows[i]["QuashStatus"]), Convert.ToBoolean(dsNew.Tables[j].Rows[i]["schemeIsOpened"]), Convert.ToDouble(dsNew.Tables[j].Rows[i]["winMoneyNoWithTax"]));
                    dsNew.Tables[j].Rows[i]["SchemeStatus"] = schemeStatus;
                }
            }
            for (int j = 0; j < dsNew.Tables.Count; j++)
            {
                dsNew.Tables[j].Columns.Add("isHide", typeof(int));
                dsNew.Tables[j].Columns.Add("secretMsg", typeof(string));
                for (int i = 0; i < dsNew.Tables[j].Rows.Count; i++)
                {
                    System.Data.DataTable competencesDt = new DAL.Tables.T_CompetencesOfUsers().Open("COUNT(UserID)", "UserID=" + userId, "");
                    string isHide = "0";//可以查看购买内容
                    string secretMsg = "公开";
                    if (dsNew.Tables[j].Rows[i]["initiateUserID"].ToString() != userId && Convert.ToInt32(competencesDt.Rows[0][0]) <= 0)
                    {
                        switch (dsNew.Tables[j].Rows[i]["secrecyLevel"].ToString())
                        {
                            //跟单可见
                            //case "1":
                            //    isHide = "1";
                            //    secretMsg = "跟单可查看投注详情";
                            //    break;
                            //到开奖
                            case "2":
                                if (dsNew.Tables[j].Rows[i]["isOpened"].ToString() != "1")
                                {
                                    isHide = "1";
                                    secretMsg = "开奖后可查看投注详情";
                                }
                                break;
                            //永远
                            case "3":
                                isHide = "1";//不可以查看购买内容
                                secretMsg = "该方案已被永久保密";
                                break;
                        }
                    }
                    dsNew.Tables[j].Rows[i]["isHide"] = isHide;
                    dsNew.Tables[j].Rows[i]["secretMsg"] = secretMsg;
                }
            }


            sb.Append("{");
            sb.Append("\"error\":\"0\",");
            sb.Append("\"msg\":\"\",");
            sb.Append("\"serverTime\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\",");
            sb.Append("\"schemeList\":" + OverRideGetJsonByDataset(dsNew) + ",");
            sb.Append("\"schemeBuyDetails\":" + GetJsonByDataTable(dtBuyDetails, 0) + "");
            sb.Append("}");

            string strRegex = @"([\d]{1,}[.][\d]{2})[\d][\d]";
            string strResult = RegexReplace(sb.ToString(), strRegex, "$1");
            context.Response.Write(strResult);
            context.ApplicationInstance.CompleteRequest();
            context.Response.End();


        }
        #endregion


        
        //简易获取用户余额
        #region 简易获取用户余额
        if (opt.Equals("8"))
        {
            string userId = context.Request.Form["uid"];
            Users _User = new Users(1)[1, int.Parse(userId)];
            sb.Append("{\"error\":\"0\",");
            sb.Append("\"balance\":\"" + (_User.Balance + _User.HandselAmount).ToString("0.00") + "\"");
            sb.Append("}");
            context.Response.Write(sb.ToString());
            context.ApplicationInstance.CompleteRequest();
            context.Response.End();
        }
        #endregion


        //获取APP版本
        #region 获取APP版本
        if (opt.Equals("9"))
        {

            Sites _Site = new Sites()[1];
            if (_Site == null)
            {
                sb.Append("{\"error\":\"-99\",");
                sb.Append("\"msg\":\"数据获取失败\"}");
                context.Response.Write(sb);
                return;
            }
            
            
            System.Data.DataTable dt = Shove.Database.MSSQL.Select("Select top 1 * from T_AppSetting");
            
            sb.Append("{\"error\":\"0\",");
            sb.Append("\"androidVersion\":\"" + dt.Rows[0]["androidVersion"] + "\",");
            sb.Append("\"androidUrl\":\"" + dt.Rows[0]["androidUrl"] + "\",");
            sb.Append("\"androidForce\":\"" + dt.Rows[0]["androidForce"] + "\",");
            sb.Append("\"androidDes\":\"" + dt.Rows[0]["androidDes"] + "\",");
            sb.Append("\"androidType\":\"" + dt.Rows[0]["androidType"] + "\",");
            sb.Append("\"IOSVersion\":\"" + dt.Rows[0]["IOSVersion"] + "\",");
            sb.Append("\"IOSUrl\":\"" + dt.Rows[0]["IOSUrl"] + "\",");
            sb.Append("\"IOSForce\":\"" + dt.Rows[0]["IOSForce"] + "\",");
            sb.Append("\"IOSDes\":\"" + dt.Rows[0]["IOSDes"] + "\",");
            sb.Append("\"qrCodeUrl\":\"" + dt.Rows[0]["qrCodeUrl"] + "\",");
            sb.Append("\"welcomeText\":\"" + dt.Rows[0]["welcomeText"] + "\",");
            sb.Append("\"welcomeSwitch\":\"" + dt.Rows[0]["welcomeSwitch"] + "\",");
            sb.Append("\"notice\":\"" + _Site.Mobile + "\"");
            sb.Append("}");
            context.Response.Write(sb.ToString());
            context.ApplicationInstance.CompleteRequest();
            context.Response.End();
        }
        #endregion

        //获取关于我们页面信息
        #region 获取关于我们页面信息
        if (opt.Equals("10"))
        {
            System.Data.DataTable dt = Shove.Database.MSSQL.Select("Select top 1 a.*,b.Url from T_Sites a left join T_SiteUrls b on a.ID=b.SiteID where a.ID=1");

            string qrcode = "http://" + HttpContext.Current.Request.Url.Host.ToString() + "/Images/qrcode.png";
            sb.Append("{\"error\":\"0\",");
            sb.Append("\"siteurl\":\"" + dt.Rows[0]["Url"] + "\",");
            sb.Append("\"qq\":\"" + dt.Rows[0]["QQ"] + "\",");
            sb.Append("\"wx\":\"" + dt.Rows[0]["Email"] + "\",");
            sb.Append("\"wxqrcode\":\"" + qrcode + "\"");
            sb.Append("}");
            context.Response.Write(sb.ToString());
            context.ApplicationInstance.CompleteRequest();
            context.Response.End();
        }
        #endregion

        //通过IP获取推荐人ID
        #region 通过IP获取推荐人ID
        if (opt.Equals("11"))
        {
            string refid = "0";
            
            string ipAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            
            if (ipAddress != null)
            {
                System.Data.DataTable ipRecord = Shove.Database.MSSQL.Select("Select top 1 * from T_RefferIP where IPAddress='" + ipAddress + "' and [DateTime]> DATEADD(d,-1, GETDATE()) order by ID desc");
                if (ipRecord == null || ipRecord.Rows.Count == 0)
                {
                    refid = "-1";
                }
				else{
					refid = ipRecord.Rows[0]["refid"].ToString();
				}
            }

            if (int.Parse(refid) > 0)
            {
                sb.Append("{\"error\":\"0\",");
				sb.Append("\"msg\":\"" + refid + "\"");
            }
			else{
				sb.Append("{\"error\":\"-120\",");
				sb.Append("\"msg\":\"" + ipAddress + "\"");
			}
            
            
            sb.Append("}");
            context.Response.Write(sb.ToString());
            context.ApplicationInstance.CompleteRequest();
            context.Response.End();
        }
        #endregion
        
        
        
        
        
        
        

    }







    /// <summary>
    /// 获取竞彩优化数据返回JSON数据到前台
    /// </summary>
    /// <param name="dataTable">数据表</param>
    /// <returns>JSON字符串</returns>   
    /// <remarks></remarks>
    public static string GetJsonByDataTable(System.Data.DataTable dataTable, int isHideBuyContent)
    {
        if (dataTable == null || dataTable.Rows.Count == 0)
        {
            return "[]";
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.Append("[ ");
        for (int i = 0; i < dataTable.Rows.Count; i++)
        {
            sb.Append("{ ");
            for (int j = 0; j < dataTable.Columns.Count; j++)
            {
                if (dataTable.Columns[j].ColumnName.ToString() != "lotteryNumber")
                {
                    sb.Append("\"" + dataTable.Columns[j].ColumnName.ToString() + "\":" + "\"" + dataTable.Rows[i][j].ToString().Replace("\r", "\\r").Replace("\n", "\\n").Replace("//", "/").Replace("\"", "'") + "\",");
                }
                else
                {
                    System.Data.DataTable DTLotteryNumber = new DAL.Tables.T_SchemesMixcast().Open("*", "SchemeId=" + dataTable.Rows[i]["ID"].ToString() + "", "ID");
                    if (isHideBuyContent == 0)
                    {
                        if (DTLotteryNumber == null || DTLotteryNumber.Rows.Count <= 0)
                        {
                            sb.Append("\"buyContent\": [ {\"playType\":\"\",\"sumMoney\":\"\",\"sumNum\":\"\",\"lotteryNumber\":\"\"}],");
                        }
                        else
                        {
                            sb.Append("\"buyContent\":[");
                            foreach (System.Data.DataRow DR in DTLotteryNumber.Rows)
                            {
                                sb.Append("[{\"playType\":\"" + DR["PlayTypeID"].ToString() + "\",\"sumMoney\":\"" + DR["Money"].ToString() + "\",\"sumNum\":\"" + DR["InvestNum"].ToString() + "\",\"lotteryNumber\":\"" + DR["lotteryNumber"].ToString() + "\"}],");
                            }
                            sb = sb.Remove(sb.Length - 1, 1);
                            sb.Append("],");

                        }
                    }
                    else
                    {
                        sb.Append("\"buyContent\": [],");
                    }
                }
            }

            sb = sb.Remove(sb.Length - 1, 1);
            // sb.set(sb.length() - 1);

            if (i == dataTable.Rows.Count - 1)
            {
                sb.Append("} ");
            }
            else
            {
                sb.Append("}, ");
            }
        }
        sb.Append("]");

        return sb.ToString();
    }


    public static string OverRideGetJsonByDataset(System.Data.DataSet ds)
    {
        if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
        {
            //如果查询到的数据为空
            //return "{\"ok\":false}";
            return "[]";
        }
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //sb.Append("{\"ok\":true,");
        //sb.Append("\"rCount\":"+ds.Tables[0].Rows.Count+",");

        int j = 1;
        sb.Append("[");
        foreach (System.Data.DataTable dt in ds.Tables)
        {
            sb.Append("{");
            sb.Append(string.Format("\"{0}\":\"{1}\"", "date", dt.Rows[0]["datetime"]));

            sb.Append(",\"dateDetail\":[");
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                sb.Append("{");
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    if (dr.Table.Columns[i].ColumnName != "lotteryNumber")
                    {
                        sb.AppendFormat("\"{0}\":\"{1}\",", dr.Table.Columns[i].ColumnName.Replace("\"", "\\\"").Replace("\'", "\\\'"), ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), " \\r\\n").Replace(Convert.ToString((char)10), " \\r\\n");
                    }
                    else
                    {
                        System.Data.DataTable DTLotteryNumber = new DAL.Tables.T_SchemesMixcast().Open("*", "SchemeId=" + dr["ID"].ToString() + "", "ID");
                        if (DTLotteryNumber == null || DTLotteryNumber.Rows.Count <= 0)
                        {
                            sb.Append("\"buyContent\": [ {\"playType\":\"\",\"sumMoney\":\"\",\"sumNum\":\"\",\"lotteryNumber\":\"\"}],");
                        }
                        else if (dr["isHide"].ToString() == "1")
                        {
                            sb.Append("\"buyContent\": [],");
                        }
                        else
                        {
                            sb.Append("\"buyContent\":[");
                            foreach (System.Data.DataRow DR in DTLotteryNumber.Rows)
                            {
                                sb.Append("[{\"playType\":\"" + DR["PlayTypeID"].ToString() + "\",\"sumMoney\":\"" + DR["Money"].ToString() + "\",\"sumNum\":\"" + DR["InvestNum"].ToString() + "\",\"lotteryNumber\":\"" + DR["lotteryNumber"].ToString() + "\"}],");
                            }
                            sb = sb.Remove(sb.Length - 1, 1);
                            sb.Append("],");

                        }
                    }

                }
                if (sb.ToString().Substring(sb.Length - 1) == ",")
                {
                    sb.Remove(sb.Length - 1, 1);
                }

                sb.Append("},");
            }

            if (sb.ToString().Substring(sb.Length - 1) == ",")
            {
                sb.Remove(sb.Length - 1, 1);
            }

            sb.Append("]},");

            j++;
        }
        if (sb.ToString().Substring(sb.Length - 1) == ",")
        {
            sb.Remove(sb.Length - 1, 1);
        }
        sb.Append("]");
        return sb.ToString();
    }

    public static string ObjToStr(object ob)
    {
        if (ob == null)
        {
            return string.Empty;
        }
        else
            return ob.ToString();
    }
    
    
    /// <summary>
    /// 返回方案的购买明细
    /// </summary>
    /// <param name="SchemeIDs"></param>
    /// <returns></returns>
    private System.Data.DataTable GetBuyDetails(string SchemeIDs)
    {
        if (SchemeIDs.Equals(""))
        {
            return null;
        }

        //购买明细ID，用户ID，购买时间，方案ID，购买份数，撤销状态，购买金额
        string strSQL = "select Id, userId, datetime, schemeId, share, quashStatus, detailMoney";
        strSQL += " from V_BuyDetailsWithQuashedAll with (nolock) where SiteID = 1 and SchemeID in (" + SchemeIDs + ") and QuashStatus = 0 order by [id]";
        System.Data.DataTable dtBuyDetails = Shove.Database.MSSQL.Select(strSQL);
        //DataTable dtBuyDetails = Shove.Database.MSSQL.Select(strSQL,
        //                    new Shove.Database.MSSQL.Parameter("@SchemeIDs", SqlDbType.VarChar, 0, ParameterDirection.Input, Shove._Web.Utility.FilteSqlInfusion(SchemeIDs)));

        return dtBuyDetails;
    }


    //正则表达式替换通用方法
    public string RegexReplace(string StrReplace, string strRegex, string NewStr)
    {
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(strRegex, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);

        return regex.Replace(StrReplace, NewStr);
    }

    private string buildCallBack(bool isSuccess, string text)
    {
        System.Text.StringBuilder sbcb = new System.Text.StringBuilder();
        sbcb.Append("{\"error\":\"" + (isSuccess ? "0" : "-211") + "\",");
        sbcb.Append("\"msg\":\"" + text + "\"}");
        return sbcb.ToString();
    }

    private string buildCallBackObj(bool isSuccess, string text, string text2)
    {
        System.Text.StringBuilder sbcb = new System.Text.StringBuilder();
        sbcb.Append("{\"error\":\"" + (isSuccess ? "0" : "-211") + "\",");
        sbcb.Append("\"msg\":" + text + ",");
        sbcb.Append("\"msg2\":" + text2 + "}");
        return sbcb.ToString();
    }
    
    
    
 
    public bool IsReusable {
        get {
            return false;
        }
    }



    
}
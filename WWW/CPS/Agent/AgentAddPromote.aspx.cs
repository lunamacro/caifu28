using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

public partial class CPS_Agent_AgentAddPromote : CPSAgentBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnRegister_tgy_Click(object sender, EventArgs e)
    {
        try
        {
            string userName = Shove._Web.Utility.FilteSqlInfusion(this.txtName_tgy.Text.Trim());
            string userPwd = Shove._Web.Utility.FilteSqlInfusion(this.txtPwd_tgy.Text.Trim());
            if (string.IsNullOrEmpty(userName))
            {
                Shove._Web.JavaScript.Alert(this, "请输入用户名");
                return;
            }
            if (string.IsNullOrEmpty(userPwd))
            {
                Shove._Web.JavaScript.Alert(this, "请输入密码");
                return;
            }
            long result3 = new DAL.Tables.T_Users().GetCount("Name='" + userName + "' or or Mobile = '" + userName + "'");
            if (result3 > 0)
            {
                Shove._Web.JavaScript.Alert(this, "用户名已经被使用");
                return;
            }
            string returnDesc = "";
            Users user = new Users(1);
            user.Name = userName;
            user.Password = userPwd;
            user.PasswordAdv = userPwd;
            user.CpsID = CPSBLL.getCpsIDByUserID(_User.ID);
            user.UserType = 2;
            int result = user.Add(ref returnDesc);
            if (result < 0)
            {
                Shove._Web.JavaScript.Alert(this, "添加CPS推广员失败");
                return;
            }

            DAL.Tables.T_Cps cps = new DAL.Tables.T_Cps();
            cps.SiteID.Value = 1;
            cps.OwnerUserID.Value = result;
            cps.DateTime.Value = DateTime.Now.ToString();
            cps.ON.Value = true;
            cps.Type.Value = 2;
            cps.ParentID.Value = CPSBLL.getCpsIDByUserID(_User.ID);
            cps.OperatorID.Value = 1;   //操作员
            cps.HandlelDateTime.Value = DateTime.Now;

            //检查成为推广员是否需要审核
            string sql = "select PromoterAuditings from T_Sites";
            DataTable dt = Shove.Database.MSSQL.Select(sql);
            if (null != dt && dt.Rows.Count > 0 && !(Shove._Convert.StrToBool(dt.Rows[0]["PromoterAuditings"].ToString(), true)))//不需要审核
            {
                cps.HandleResult.Value = 1;
                cps.SerialNumber.Value = CPSBLL.CreateSerialNumber();
            }
            else//需要审核
            {
                cps.HandleResult.Value = 0;
            }
            long cpsID = cps.Insert();
            if (cpsID < 0)
            {
                Shove._Web.JavaScript.Alert(this, "添加CPS推广员失败");
                return;
            }
            //不需要审核
            if (!Shove._Convert.StrToBool(dt.Rows[0]["PromoterAuditings"].ToString(), true))
            {
                //写入会员转移表
                DAL.Tables.T_CpsUserChange tcpsUserChange = new DAL.Tables.T_CpsUserChange();
                if (tcpsUserChange.GetCount("ChangeType < 1 and Type = 2 and UserID=" + result + " ") < 1)//推广员
                {
                    tcpsUserChange.UserID.Value = result;
                    tcpsUserChange.DateTime.Value = DateTime.Now;
                    tcpsUserChange.OperatorID.Value = 1;
                    tcpsUserChange.NowUserID.Value = _User.ID;
                    tcpsUserChange.Type.Value = 2;
                    tcpsUserChange.ChangeType.Value = -1;

                    tcpsUserChange.Insert();
                }

                Sites _site = new Sites()[1];
                string UseLotteryListQuickBuy = _site.UseLotteryListQuickBuy;
                DAL.Tables.T_CpsSiteBonusScale tCpsSiteBonusScale = new DAL.Tables.T_CpsSiteBonusScale();
                dt = tCpsSiteBonusScale.Open("LotteryID,PromoterBonusScale", "LotteryID in (" + UseLotteryListQuickBuy + ")", "id asc");
                if (null != dt && dt.Rows.Count > 0)
                {
                    DataTable dt2 = Shove.Database.MSSQL.Select("select top 1 memo from T_CpsUsersBonusScaleLog order by id desc");

                    long Scaleid = 1;
                    if (dt2 != null && dt2.Rows.Count > 0)
                    {
                        Scaleid = Shove._Convert.StrToLong(dt2.Rows[0][0].ToString(), 1) + 1;
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        int v = CPSBLL.SetCommission(cpsID, Convert.ToInt64(dr["LotteryID"]), Convert.ToDouble(dr["PromoterBonusScale"]), Scaleid, ref returnDesc);
                        if (v < 0)
                        {
                            Shove._Web.JavaScript.Alert(this, "添加CPS推广员失败");
                            return;
                        }
                    }
                }
            }
            this.txtName_tgy.Text = "";
            this.txtPwd_tgy.Text = "";
            Shove._Web.JavaScript.Alert(this, "添加成功,信息审核中");
        }
        catch (Exception ex)
        {
            Shove._Web.JavaScript.Alert(this, "添加失败，服务器繁忙");
        }
    }
}
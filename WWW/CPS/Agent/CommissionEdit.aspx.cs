using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_Agent_CommissionEdit : CPSAgentBase
{
    public string UserType = "";
    public long CpsID = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        UserType = Shove._Web.Utility.GetRequest("UserType");
        CpsID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("CpsID"), -1);
        this.hide_userType.Value = UserType;
        if (UserType != "1" && UserType != "2" || -1 == CpsID)
        {
            Shove._Web.JavaScript.Alert(this, "参数错误");
            return;
        }
        if (!IsPostBack)
        {
            this.BindCommissionScale();
        }
    }


    /// <summary>
    /// 绑定佣金比例
    /// </summary>
    private void BindCommissionScale()
    {
        if (null != _User)
        {
            string sql = "select ID,OwnerUserID,LotteryID,BonusScale,(select BonusScale from T_CpsUsersBonusScale where OwnerUserID = " + CPSBLL.getCpsIDByUserID(_User.ID) + " and LotteryID = c.LotteryID)as ParentBonusScale,(select Name from T_Lotteries where ID = c.LotteryID)as LotteryName from T_CpsUsersBonusScale as c where c.OwnerUserID = " + CpsID;
            this.rpt_dataList.DataSource = Shove.Database.MSSQL.Select(sql);
            this.rpt_dataList.DataBind();
        }
    }

    /// <summary>
    /// 全部修改
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAllSave_Click(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// 保存佣金比例
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataTable dt = new DAL.Tables.T_Cps().Open("ParentID", "ID = " + CpsID + " and ID <> -1", "");
        if (null == dt && dt.Rows.Count == 0)
        {
            Shove._Web.JavaScript.Alert(this, "修改失败,数据库繁忙");
            return;
        }
        RepeaterItemCollection items = rpt_dataList.Items;
        foreach (RepeaterItem item in items)
        {
            string lotteryID = (item.FindControl("hide_LotteryID") as HiddenField).Value;
            string LotteryName = (item.FindControl("hide_LotteryName") as HiddenField).Value;
            double commission = Shove._Convert.StrToDouble((item.FindControl("txtAlter") as TextBox).Text, -1);
            if (commission <= 0)
            {
                Shove._Web.JavaScript.Alert(this, "请输入正确的[" + LotteryName + "]佣金比例");
                return;
            }
            //上级ID，如果上级ID 不等于-1表示有上级
            long ParentID = Shove._Convert.StrToLong(dt.Rows[0]["ParentID"] + "", -1);
            if (-1 != ParentID)
            {
                double parentBonusScale = DAL.Functions.F_CpsGetBonusScale(ParentID, Convert.ToInt64(lotteryID));
                if (commission > parentBonusScale && parentBonusScale != 0)
                {
                    Shove._Web.JavaScript.Alert(this, "修改失败,当前用户的佣金比例不能大于上级代理商的佣金比例");
                    return;
                }
            }
        }
        DataTable dt2 = Shove.Database.MSSQL.Select("select top 1 memo from T_CpsUsersBonusScaleLog order by id desc");

        long Scaleid = 1;
        if (dt2 != null && dt2.Rows.Count > 0)
        {
            Scaleid = Shove._Convert.StrToLong(dt2.Rows[0][0].ToString(), 1) + 1;
        }
        foreach (RepeaterItem item in items)
        {
            string lotteryID = (item.FindControl("hide_LotteryID") as HiddenField).Value;
            string LotteryName = (item.FindControl("hide_LotteryName") as HiddenField).Value;
            double commission = Shove._Convert.StrToDouble((item.FindControl("txtAlter") as TextBox).Text, -1);

            string returnDesc = "";
            int v = CPSBLL.SetCommission(CpsID, Convert.ToInt64(lotteryID), commission, Scaleid, ref returnDesc);
            if (v < 0)
            {
                Shove._Web.JavaScript.Alert(this, "修改失败");
                return;
            }
        }
        Shove._Web.JavaScript.Alert(this, "修改成功!", this.Request.Url.ToString());
    }


    /// <summary>
    /// 恢复默认按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReturnDefault_Click(object sender, EventArgs e)
    {
        DataTable dt = new DAL.Tables.T_Cps().Open("ParentID", "ID = " + CpsID + " and ID <> -1", "");
        if (null == dt && dt.Rows.Count == 0)
        {
            Shove._Web.JavaScript.Alert(this, "修改失败,数据库繁忙");
            return;
        }
        long ParentID = Shove._Convert.StrToLong(dt.Rows[0]["ParentID"] + "", -1);
        double Commission = 0;
        string[] UseLotteryArray = _Site.UseLotteryListQuickBuy.Split(',');
        switch (UserType)
        {
            case "1":
                Commission = 0.05;//代理商
                break;
            case "2":
                Commission = 0.02;//推广员
                break;
        }
        //父级id不等于-1 表示有上级代理商，需要验证默认佣金是否大于上级代理商的佣金了
        if (-1 != ParentID)
        {
            foreach (string lotteryID in UseLotteryArray)
            {
                double parentBonusScale = DAL.Functions.F_CpsGetBonusScale(ParentID, Convert.ToInt64(lotteryID));
                if (Commission > parentBonusScale && parentBonusScale != 0)
                {
                    Shove._Web.JavaScript.Alert(this, "修改失败,当前用户的佣金比例不能大于上级代理商的佣金比例");
                    return;
                }
            }
        }
        DataTable dt2 = Shove.Database.MSSQL.Select("select top 1 memo from T_CpsUsersBonusScaleLog order by id desc");

        long Scaleid = 1;
        if (dt2 != null && dt2.Rows.Count > 0)
        {
            Scaleid = Shove._Convert.StrToLong(dt2.Rows[0][0].ToString(), 1) + 1;
        }
        foreach (string lotteryID in UseLotteryArray)
        {
            string returnDesc = "";
            int v = CPSBLL.SetCommission(CpsID, Convert.ToInt64(lotteryID), Commission,Scaleid, ref returnDesc);
            if (v < 0)
            {
                Shove._Web.JavaScript.Alert(this, "修改失败");
                return;
            }
        }
        Shove._Web.JavaScript.Alert(this, "修改成功", this.Request.Url.ToString());
    }

    /// <summary>
    /// 修改完成按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEditOver_Click(object sender, EventArgs e)
    {
        DataTable dt = new DAL.Tables.T_Cps().Open("ParentID", "ID = " + CpsID + " and ID <> -1", "");
        if (null == dt && dt.Rows.Count == 0)
        {
            Shove._Web.JavaScript.Alert(this, "修改失败,数据库繁忙");
            return;
        }
        RepeaterItemCollection items = rpt_dataList.Items;
        foreach (RepeaterItem item in items)
        {
            string lotteryID = (item.FindControl("hide_LotteryID") as HiddenField).Value;
            string LotteryName = (item.FindControl("hide_LotteryName") as HiddenField).Value;
            double commission = Shove._Convert.StrToDouble((item.FindControl("txtAlter") as TextBox).Text, -1);
            if (commission <= 0)
            {
                Shove._Web.JavaScript.Alert(this, "请输入正确的[" + LotteryName + "]佣金比例");
                return;
            }
            //上级ID，如果上级ID 不等于-1表示有上级
            long ParentID = Shove._Convert.StrToLong(dt.Rows[0]["ParentID"] + "", -1);
            if (-1 != ParentID)
            {
                double parentBonusScale = DAL.Functions.F_CpsGetBonusScale(ParentID, Convert.ToInt64(lotteryID));
                if (commission > parentBonusScale && parentBonusScale != 0)
                {
                    Shove._Web.JavaScript.Alert(this, "修改失败,当前用户的佣金比例不能大于上级代理商的佣金比例");
                    return;
                }
            }
        }
        DataTable dt2 = Shove.Database.MSSQL.Select("select top 1 memo from T_CpsUsersBonusScaleLog order by id desc");

        long Scaleid = 1;
        if (dt2 != null && dt2.Rows.Count > 0)
        {
            Scaleid = Shove._Convert.StrToLong(dt2.Rows[0][0].ToString(), 1) + 1;
        }
        foreach (RepeaterItem item in items)
        {
            string lotteryID = (item.FindControl("hide_LotteryID") as HiddenField).Value;
            string LotteryName = (item.FindControl("hide_LotteryName") as HiddenField).Value;
            double commission = Shove._Convert.StrToDouble((item.FindControl("txtAlter") as TextBox).Text, -1);

            string returnDesc = "";
            int v = CPSBLL.SetCommission(CpsID, Convert.ToInt64(lotteryID), commission,Scaleid, ref returnDesc);
            if (v < 0)
            {
                Shove._Web.JavaScript.Alert(this, "修改失败");
                return;
            }
        }
        Shove._Web.JavaScript.Alert(this, "修改成功!", this.Request.Url.ToString());
    }
}
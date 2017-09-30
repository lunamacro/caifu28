using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Shove.Database;

public partial class CPS_Agent_AgentIndex : CPSAgentBase
{
    public double sumMoney = 0.00;
    public double sumBonus = 0.00;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
       
    }
    protected void rpt_list_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            sumMoney += Shove._Convert.StrToDouble(drv["DetailMoney"].ToString(), 0.00);
            sumBonus += Shove._Convert.StrToDouble(drv["Money"].ToString(), 0.00);
        }
    }
}
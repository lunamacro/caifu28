using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_News : CPSPage
{

    public int PageIndex = 1;
    public int PageSize = 15;
    public long PageCount = 1;
    public long DataCount = 1;
    public string newType = "xwgg";

    protected void Page_Load(object sender, EventArgs e)
    {
        PageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("PageIndex"), 1);
        newType = Shove._Web.Utility.GetRequest("NewType");
        if ("" == newType) newType = "xwgg";
        if ("xwgg" == newType)
        {
            li_xwgg.Attributes.Add("class","curr");
        }
        else 
        {
            li_tgzn.Attributes.Add("class", "curr");
        }
        if (!IsPostBack)
        {
            this.BindData();
        }
    }

    private void BindData()
    {

        CPSBLL.CPSNews cps = new CPSBLL.CPSNews();
        this.DataCount = cps.GetSumNewsCount(newType);
        this.PageCount = ((this.DataCount - 1) / this.PageSize) + 1;
        DataTable dt = cps.GetCPSNews(newType, PageIndex, PageSize);
        if (null != dt && dt.Rows.Count > 0)
        {
            this.rpt_list.DataSource = dt;
            this.rpt_list.DataBind();
        }
        else 
        {
            this.sand.Style.Add("display", "none");
        }
    }

    public string FormatDateTime(string dateTime)
    {
        DateTime temp = Shove._Convert.StrToDateTime(dateTime, DateTime.Now.ToString());
        string str = "<i>" + temp.Month + "-" + temp.Day + "</i>";
        str += "<b>" + temp.Year + "</b>";
        return str;
    }
}
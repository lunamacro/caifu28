using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// UserModel 的摘要说明
/// </summary>
public class UserModel
{
	public UserModel()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
}

public class reg 
{
    public string userName { set; get; }
    public string password { set; get; }
    public string realName { set; get; }
    public string mobile { set; get; }
    public string certNo { set; get; }
    public string email { set; get; }
    public string provider { set; get; }
    public string outUid { set; get; }
}

public class update {

    public string userName { set; get; }
    public string status { set; get; }
    public string realName { set; get; }
    public string mobile { set; get; }
    public string certNo { set; get; }
    public string email { set; get; }

}

public class updatePwd {
    public string userName { set; get; }
    public string oldPwd { set; get; }
    public string newPwd { set; get; }
}

public class bindCard {
    public string userName { set; get; }
    public string certNo { set; get; }
    public string bank { set; get; }
    public string realName { set; get; }
    public string bankCard { set; get; }
    public string openCardAddress { set; get; }
    public string zfbCard { set; get; }

}

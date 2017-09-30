using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UlinePayUtil
{
    /// <summary>
    /// UlinePayNotifyEntity 的摘要说明
    /// </summary>
    public class UlinePayNotifyEntity
    {
        public UlinePayNotifyEntity()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public string result_code { get; set; } //SUCCESS
        public string total_fee { get; set; } //1
        public string mch_id { get; set; }  //100002153066
        public string return_code { get; set; } //SUCCESS
        public string trade_type { get; set; }  //NATIVE
        public string is_subscribe { get; set; }    //N
        public string nonce_str { get; set; }   //37b87d5c0e415824cf74b9f3d5512b86
        public string time_end { get; set; }    //20170805173618
        public string transaction_id { get; set; }  //4100002153066150192576500000351
        public string fee_type { get; set; }    //CNY
        public string trade_state { get; set; } //SUCCESS
        public string sign { get; set; }    //5B13082CF3ED1BFCBFEFB23F1F8A8095
        public string openid { get; set; }  //odbfdwQTosEwfFbu0hnB-Rv27Fh8
        public string cash_fee { get; set; }    //1
        public string out_trade_no { get; set; }    //U1708051736069428
        public string out_transaction_id { get; set; }  //4006762001201708054638173088
        public string bank_type { get; set; }   //CFT



    }
}
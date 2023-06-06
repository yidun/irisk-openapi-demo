using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
// 本接口用于上报应用的举报信息。
namespace Com.Netease.Is.Irisk.Demo
{
    class RiskReportDataDemo
    {
        public static void riskCheck()
        {
            // 产品id，每个应用接入时，会分配secretId和私钥secretKey。
            String secretId = "your_secret_id";
            // 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
            String secretKey = "your_secret_key";
            // 每个业务接入时，均会分配业务 ID，有对应的密钥 secretId。
            String businessId = "your_business_id";
            // 版本号，如500
            String version = "500";
            // 接口URL
            String apiUrl = "http://ir-open.dun.163.com/v5/risk/reportData";
            // 随机码，32位
            String nonce = "BWJOGAEbplxiaFxSsSV4nzdeznJJWfk7";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            long curr = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            String timestamp = curr.ToString();
            // 设置公共参数
            parameters.Add("secretId", secretId);
            parameters.Add("businessId", businessId);
            parameters.Add("version", version);
            parameters.Add("timestamp", timestamp);
            parameters.Add("nonce", nonce);
            // 举报渠道
            parameters.Add("reportChannel", "reportChannel");
            // 举报时间
            parameters.Add("reportTime", DateTimeOffset.Now.ToUnixTimeMilliseconds());
            // 举报人信息
            Dictionary<string, object> whistleblowerMap = new Dictionary<string, object>();
            whistleblowerMap.Add("account", "account01");
            whistleblowerMap.Add("roleId", "roleId01");
            whistleblowerMap.Add("roleName", "roleName01");
            whistleblowerMap.Add("serverId", "serverId01");
            whistleblowerMap.Add("level", "10");
            whistleblowerMap.Add("recharge", "9999");
            string whistleblowerJson = JsonConvert.SerializeObject(whistleblowerMap);
            parameters.Add("whistleblower", whistleblowerJson);
            // 被举报人信息
            Dictionary<string, object> reportedPerson = new Dictionary<string, object>();
            reportedPerson.Add("account", "account02");
            reportedPerson.Add("roleId", "roleId02");
            reportedPerson.Add("roleName", "roleName02");
            reportedPerson.Add("serverId", "serverId02");
            reportedPerson.Add("level", "10");
            reportedPerson.Add("recharge", "9999");
            string reportedPersonJson = JsonConvert.SerializeObject(reportedPerson);
            parameters.Add("reportedPerson", reportedPersonJson);
            // 举报类型
            parameters.Add("reportType", "reportType01");
            // 举报场景
            parameters.Add("reportScene", "reportScene01");
            // 举报内容详情
            parameters.Add("reportData", "data");
            String signature = Utils.genSignature(secretKey, parameters);
            parameters.Add("signature", signature);
            // 发送HTTP请求
            HttpClient client = Utils.makeHttpClient();
            String result = Utils.doPost(client, apiUrl, parameters, 10000);
            Console.WriteLine(result);
            if (result != null) 
            {
                JObject ret = JObject.Parse(result);
                int code = ret.GetValue("code").ToObject<Int32>();
                String msg = ret.GetValue("msg").ToObject<String>();
                if (code == 200) 
                {
                    Console.WriteLine(String.Format("SUCCESS: code={0}, msg={1}", code, msg));
                } else 
                {
                    Console.WriteLine(String.Format("ERROR: code={0}, msg={1}", code, msg));
                }
            }
        }
    }
}
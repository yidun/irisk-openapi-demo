using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
// See https://aka.ms/new-console-template for more information
namespace Com.Netease.Is.Irisk.Demo
{
    class RiskConfigDemo
    {
        public static void riskConfig()
        {
            // 产品id，每个应用接入时，会分配secretId和私钥secretKey。
            String secretId = "your_secret_id";
            // 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
            String secretKey = "your_secret_key";
            // 每个业务接入时，均会分配业务 ID，有对应的密钥 secretId。
            String businessId = "your_business_id";
            // 版本号，如400
            String version = "400";
            // 接口URL
            String apiUrl = "https://ir-open.dun.163.com/v4/risk/getConfig";
            // 随机码，32位
            String nonce = "BWJOGAEbplxiaFxSsSV4nzdeznJJWfk7";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            long curr = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            String timestamp = curr.ToString();
            // 1.设置公共参数
            parameters.Add("secretId", secretId);
            parameters.Add("businessId", businessId);
            parameters.Add("version", version);
            parameters.Add("timestamp", timestamp);
            parameters.Add("nonce", nonce);
            // 用户/ 玩家的IP，或当前客户端业务事件发生时的公网IP地址（ipv4）
            parameters.Add("ip", "1.1.1.1");
            // 从易盾SDK获取的拉取配置的参数，参考反外挂通用查询接口。数据格式见base64(Request)，Request为SDK直接调用服务端统一配置下发接口(/v4/c)请求参数。
            parameters.Add("sdkData", "your_sdk_data");
            // 2.生成签名信息，使用secretKey签名的数据，校验权限
            String signature = Utils.genSignature(secretKey, parameters);
            parameters.Add("signature", signature);

            // 3.发送HTTP请求
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
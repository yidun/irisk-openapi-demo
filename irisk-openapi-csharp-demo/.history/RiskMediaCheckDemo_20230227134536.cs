using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
// See https://aka.ms/new-console-template for more information
namespace Com.Netease.Is.Irisk.Demo
{
    class RiskMediaCheckDemo
    {
        public static void riskCheck()
        {
            // 产品id，每个应用接入时，会分配secretId和私钥secretKey。
            String secretId = "1ded450ce116bcffd62e603c99ee7834";
            // 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
            String secretKey = "accd3781b86e95d8a84c81b2c62e9e71";
            // 每个业务接入时，均会分配业务 ID，有对应的密钥 secretId。
            String businessId = "ff1c6edcdec98a00d3eddc763c52d1e2";
            // 版本号，如400
            String version = "400";
            // 接口URL
            String apiUrl = "http://ir-open.dun.163.com/v5/risk/mediaCheck";
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
            // 文件数据
            parameters.Add("mediaData", "your_token");
            // 文件名称
            parameters.Add("mediaName", "your_token");
            // 用户/ 玩家的IP，或当前客户端业务事件发生时的公网IP地址（ipv4）
            parameters.Add("ip", "183.136.182.141");
            // 生成签名信息，使用secretKey签名的数据，校验权限
            // 设置私有参数
            // 用户/玩家的角色 ID，非游戏类型应用，roleId 可以与 roleAccount 相同
            parameters.Add("roleId", "yyyyyyy");
            // 用户/玩家的角色名称，非游戏类型应用，roleName 可以是当前用户昵称相同
            parameters.Add("nickname", "yyyyyyy");
            // 用户/玩家的角色的服务器名称
            parameters.Add("server", "yyyyyyy");
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
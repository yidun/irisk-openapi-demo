using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
// See https://aka.ms/new-console-template for more information
namespace Com.Netease.Is.Irisk.Demo
{
    class MobileRiskCheckDemo
    {
        public static void riskCheck()
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
            String apiUrl = "https://ir-open.dun.163.com/v4/risk/check";
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
            // 风控SDK上报的数据后回参获取到的 token，应用方需要从智能风控客户端SDK获取该数据。详情请查看客户端接入文档。
            parameters.Add("token", "your_token");
            // 用户/ 玩家的IP，或当前客户端业务事件发生时的公网IP地址（ipv4）
            parameters.Add("ip", "183.136.182.141");
            // 2.生成签名信息，使用secretKey签名的数据，校验权限
            String signature = Utils.genSignature(secretKey, parameters);
            parameters.Add("signature", signature);
            // 3. 设置私有参数
            // 用户/玩家的角色 ID，非游戏类型应用，roleId 可以与 roleAccount 相同
            parameters.Add("roleId", "yyyyyyy");
            // 用户/玩家的角色名称，非游戏类型应用，roleName 可以是当前用户昵称相同
            parameters.Add("roleName", "yyyyyyy");
            // 用户/玩家的角色的服务器名称
            parameters.Add("roleServer", "yyyyyyy");
            // 用户/玩家的账号
            parameters.Add("account", "zzzzzzz");
            // 用户/玩家的等级
            parameters.Add("roleLevel", "150");
            // 游戏类型应用的版本号
            parameters.Add("gameVersion", "1.0.2");
            // 游戏类型应用的资源版本号
            parameters.Add("assetVersion", "assetVersion");
            // 额外/拓展的信息，应用 / 游戏方可以自己构建json结构，最大长度：2048。不同场景构建信息见分场景接入说明
            parameters.Add("extData", "");
            // 4.发送HTTP请求
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
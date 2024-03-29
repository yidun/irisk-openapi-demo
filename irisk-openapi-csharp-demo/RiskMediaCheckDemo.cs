﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
// 本接口通过AI算法对上报的图片进行分析，识别是否存在外挂行为。
namespace Com.Netease.Is.Irisk.Demo
{
    class RiskMediaCheckDemo
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
            // 图片数据，图片支持编码为BASE64的数据，无需包含base64编码请求头部分
            parameters.Add("mediaData", "auMW9NLW5rNaa6vXVpq2jTfy1Kemr2UuWyvu9L7662dvL7Oik3cp5J5PJ/dr35/56UrrvP5ML+X/pJ//9k=");
            // 图片文件名，格式如xxx.jpg，需要包含.格式的文件后缀名
            parameters.Add("mediaName", "xxx.jpg");
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
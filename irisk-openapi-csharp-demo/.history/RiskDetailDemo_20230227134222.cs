﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
// See https://aka.ms/new-console-template for more information
namespace Com.Netease.Is.Irisk.Demo
{
    class RiskDetailDemo
    {
        public static void riskDetail()
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
            String apiUrl = "https://ir-open.dun.163.com/v5/risk/detail";
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
            // 2.生成签名信息，使用secretKey签名的数据，校验权限
            String signature = Utils.genSignature(secretKey, parameters);
            parameters.Add("signature", signature);
            // 3. 设置私有参数
            parameters.Add("beginTimestamp", "1667959831798");
            parameters.Add("endTimestamp", "1667959915103");
            // 用于分页查询的关联标记
            parameters.Add("startFlag", "");
            // 用户/玩家的账号
            parameters.Add("account", "zzzzzzz");
            // 用户/玩家的角色ID
            parameters.Add("roleId", "yyyyyyy");
            // 风险等级, 1-低风险, 2-中风险, 3-高风险
            parameters.Add("riskLevel", "10");
            // 包名
            parameters.Add("packageName", "com.aaa.bbb");
            // app版本
            parameters.Add("appVersion", "1.0.2");
            parameters.Add("ip", "192.168.1.1");
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
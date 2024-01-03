using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
// 本接口用于智能风控嫌疑数据在线检测，并且接口会同步返回检测结果。
namespace Com.Netease.Is.Irisk.Demo
{
    class RiskSceneCheckDemo
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
            String version = "603";
            // 接口URL
            String apiUrl = "http://ir-open.dun.163.com/v6/risk/check";
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
            // 风控SDK上报的数据后回参获取到的 token，应用方需要从智能风控客户端SDK获取该数据。详情请查看客户端接入文档。
            parameters.Add("token", "your_token");
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
            // 用户/玩家的账号
            parameters.Add("account", "zzzzzzz");
            // 用户/玩家的等级
            parameters.Add("level", "150");
            // 游戏类型应用的版本号
            parameters.Add("gameVersion", "1.0.2");
            // 游戏类型应用的资源版本号
            parameters.Add("assetVersion", "assetVersion");
            // 额外/拓展的信息，应用 / 游戏方可以自己构建json结构，最大长度：2048。不同场景构建信息见分场景接入说明
            parameters.Add("extData", "");
            // 活动操作的目标，比如：A给B点赞，则target为B。如果target是手机号或邮箱，请提供hash值，hash算法：md5(target)。如没有，可传空
            parameters.Add("target", "");
            // 活动的唯一标识，用于标记场景下的细分类别，如：注册-自主注册、注册-受邀请注册；再如：登录- app登录、登录-web登录等
            parameters.Add("activityId", "");
            // 用户用于登录的手机号码或者关联的手机号码，默认国内手机号。如有海外手机，需包含国家地区代码，
            // 格式为“+447410xxx186（+44即为国家码）”。如果需要加密，支持传入hash值，hash算法：md5(phone)
            parameters.Add("phone", "");
            // 用户的邮箱，如果需要加密，支持传入hash值，hash算法：md5(email)
            parameters.Add("email", "");
            // 用户的注册IP
            parameters.Add("registerIp", "");
            // 用户的注册时间，单位：毫秒
            parameters.Add("registerTime", timestamp);
            // 场景字段
            parameters.Add("sceneData", "{\"appChannel\":\"app_store\",\"callbackUrl\":\"your_call_back_url\",\"createTime\":1688106777462,\"currency\":\"USD\",\"money\":1.22,\"orderCreateTime\":1688106777462,\"orderFinishTime\":1688106777462,\"orderId\":\"GPA.3329-6166-4658-28227-01\",\"orderPayTime\":1688106777462,\"payChannel\":\"如app_store\",\"payCurrency\":\"USD\",\"payMoney\":1.11,\"payTime\":1688106777462,\"sceneType\":\"APP_OR_GAME_RECHARGE\"}");
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
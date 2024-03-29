package com.netease.irisk.openapi.demo.v5;

import com.alibaba.fastjson.JSONObject;
import com.netease.irisk.openapi.demo.util.HttpUtil;
import com.netease.irisk.openapi.demo.util.SignatureUtils;

import java.io.UnsupportedEncodingException;
import java.util.HashMap;
import java.util.Map;

/**
 * 本接口用于智能风控嫌疑数据在线检测，并且接口会同步返回检测结果。
 */
public class RiskCheckDemo {

    // 产品id，每个应用接入时，会分配secretId和私钥secretKey。
    private static final String SECRET_ID = "your_secret_id";

    // 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
    private static final String SECRET_KEY = "your_secret_key";

    // 每个业务接入时，均会分配业务 ID
    private static final String BUSINESS_ID = "your_business_id";

    // 版本号，如500
    private static final String VERSION = "500";

    // 接口URL
    private static final String API_URL = "http://ir-open.dun.163.com/v5/risk/check";

    // 随机码，32位
    private static final String NONCE = "BWJOGAEbplxiaFxSsSV4nzdeznJJWfk7";

    public static void main(String[] args) throws Exception {
        // 获取参数，所有参数都要加入签名计算
        Map<String, Object> params = getParams();
        String response = HttpUtil.sendHttpPost(API_URL, JSONObject.toJSONString(params));
        JSONObject jsonObject = JSONObject.parseObject(response, JSONObject.class);
        Integer code = jsonObject.getInteger("code");
        String msg = jsonObject.getString("msg");
        String data = jsonObject.getString("data");
        if (code == 200) {
            System.out.printf("请求成功, data=%s%n", data);
        } else {
            System.out.printf("请求失败, msg=%s%n", msg);
        }
    }

    private static Map<String, Object> getParams() throws UnsupportedEncodingException {
        Map<String, Object> params = new HashMap<>();
        // 调用接口当前时间，单位毫秒
        Long timeStamp = System.currentTimeMillis();
        params.put("secretId", SECRET_ID);
        params.put("businessId", BUSINESS_ID);
        params.put("nonce", NONCE);
        params.put("timestamp", timeStamp);
        params.put("version", VERSION);
        // 风控SDK上报的数据后回参获取到的 token，应用方需要从智能风控客户端SDK获取该数据。详情请查看客户端接入文档。
        params.put("token", "your_token");
        // 用户/ 玩家的IP，或当前客户端业务事件发生时的公网IP地址（ipv4）
        params.put("ip", "183.136.182.141");
        // 用户/玩家的角色 ID，非游戏类型应用，roleId 可以与 roleAccount 相同
        params.put("roleId", "yyyyyyy");
        // 用户/玩家的角色名称，非游戏类型应用，nickname 可以是当前用户昵称相同
        params.put("nickname", "yyyyyyy");
        // 用户/玩家的角色的服务器名称
        params.put("server", "yyyyyyy");
        // 用户/玩家的账号
        params.put("account", "zzzzzzz");
        // 用户/玩家的等级
        params.put("level", 150);
        // 游戏类型应用的版本号
        params.put("gameVersion", "1.0.2");
        // 游戏类型应用的资源版本号
        params.put("assetVersion", "assetVersion");
        // 额外/拓展的信息，应用 / 游戏方可以自己构建json结构，最大长度：2048。不同场景构建信息见分场景接入说明
        params.put("extData", "");
        // 活动操作的目标，比如：A给B点赞，则target为B。如果target是手机号或邮箱，请提供hash值，hash算法：md5(target)。如没有，可传空
        params.put("target", "");
        // 活动的唯一标识，用于标记场景下的细分类别，如：注册-自主注册、注册-受邀请注册；再如：登录- app登录、登录-web登录等
        params.put("activityId", "");
        // 用户用于登录的手机号码或者关联的手机号码，默认国内手机号。如有海外手机，需包含国家地区代码，
        // 格式为“+447410xxx186（+44即为国家码）”。如果需要加密，支持传入hash值，hash算法：md5(phone)
        params.put("phone", "");
        // 用户的邮箱，如果需要加密，支持传入hash值，hash算法：md5(email)
        params.put("email", "");
        // 用户的注册IP
        params.put("registerIp", "");
        // 用户的注册时间，单位：毫秒
        params.put("registerTime", timeStamp);

        // 使用secretKey签名的数据，校验权限
        String signature = SignatureUtils.genSignature(SECRET_KEY, params);
        params.put("signature", signature);
        return params;
    }
}

package com.netease.irisk.openapi.demo.mobile;

import com.alibaba.fastjson.JSONObject;
import com.netease.irisk.openapi.demo.util.HttpUtil;
import com.netease.irisk.openapi.demo.util.SignatureUtils;

import java.util.HashMap;
import java.util.Map;

/**
 * 本接口用于透传使用，获取sdk所需的配置参数以及特征配置等信息。
 * 游戏反外挂场景下，getConfig 调用由 SDK 自动完成。
 */
public class RiskConfigDemo {

    // 产品id，每个应用接入时，会分配secretId和私钥secretKey。
    private static final String SECRET_ID = "your_secret_id";

    // 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
    private static final String SECRET_KEY = "your_secret_key";

    // 每个业务接入时，均会分配业务 ID
    private static final String BUSINESS_ID = "your_business_id";

    // 版本号，如400
    private static final String VERSION = "400";

    // 接口URL
    private static final String API_URL = "http://ir-open.dun.163.com/v4/risk/getConfig";

    // 随机码，32位
    private static final String NONCE = "mmm888f73yyy59440583zzz9bfcc79de";

    public static void main(String[] args) throws Exception {
        Map<String, Object> params = new HashMap<>();
        // 调用接口当前时间，单位毫秒
        Long timeStamp = System.currentTimeMillis();
        params.put("secretId", SECRET_ID);
        params.put("businessId", BUSINESS_ID);
        params.put("nonce", NONCE);
        params.put("timestamp", timeStamp);
        params.put("version", VERSION);
        // 用户/ 玩家的IP，或当前客户端业务事件发生时的公网IP地址（ipv4）
        params.put("ip", "1.1.1.1");
        // 从易盾SDK获取的拉取配置的参数，参考反外挂通用查询接口。数据格式见base64(Request)，Request为SDK直接调用服务端统一配置下发接口(/v4/c)请求参数。
        params.put("sdkData", "your_sdk_data");
        // 使用secretKey签名的数据，校验权限
        String signature = SignatureUtils.genSignature(SECRET_KEY, params);
        params.put("signature", signature);


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
}

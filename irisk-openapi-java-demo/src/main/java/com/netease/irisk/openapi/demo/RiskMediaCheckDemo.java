package com.netease.irisk.openapi.demo;

import com.alibaba.fastjson.JSONObject;
import com.netease.irisk.openapi.demo.util.HttpUtil;
import com.netease.irisk.openapi.demo.util.SignatureUtils;

import java.io.UnsupportedEncodingException;
import java.util.HashMap;
import java.util.Map;

/**
 * @author dongyisheng@corp.netease.com
 * @date 2023/2/16 15:34
 */
public class RiskMediaCheckDemo {

    // 产品id，每个应用接入时，会分配secretId和私钥secretKey。
    private static final String SECRET_ID = "your_secret_id";

    // 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
    private static final String SECRET_KEY = "your_secret_key";

    // 每个业务接入时，均会分配业务 ID
    private static final String BUSINESS_ID = "your_business_id";

    // 版本号，如400
    private static final String VERSION = "400";

    // 接口URL
    private static final String API_URL = "http://ir-open.dun.163.com/v5/risk/mediaCheck";

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
        // 文件数据
        params.put("mediaData", "test");
        // 文件名称
        params.put("mediaName", "test");
        // 用户/ 玩家的IP，或当前客户端业务事件发生时的公网IP地址（ipv4）
        params.put("ip", "183.136.182.141");
        // 用户/玩家的角色 ID，非游戏类型应用，roleId 可以与 roleAccount 相同
        params.put("roleId", "yyyyyyy");
        // 用户/玩家的角色名称，非游戏类型应用，nickname 可以是当前用户昵称相同
        params.put("nickname", "yyyyyyy");
        // 用户/玩家的角色的服务器名称
        params.put("server", "yyyyyyy");

        // 使用secretKey签名的数据，校验权限
        String signature = SignatureUtils.genSignature(SECRET_KEY, params);
        params.put("signature", signature);
        return params;
    }

}

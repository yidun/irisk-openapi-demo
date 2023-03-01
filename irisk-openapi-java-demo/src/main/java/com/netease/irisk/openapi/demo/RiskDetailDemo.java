package com.netease.irisk.openapi.demo.mobile;

import com.alibaba.fastjson.JSONObject;
import com.netease.irisk.openapi.demo.util.HttpUtil;
import com.netease.irisk.openapi.demo.util.SignatureUtils;

import java.util.HashMap;
import java.util.Map;

/**
 * 本接口的功能是智能风控明细数据查询，主要用于数据同步：从易盾拉取数据场景，以固定时间窗口拉取数据。
 * 例如，每次查询1分钟前的数据，时间跨度也是1分钟，则可以按1分钟时间窗口，周期性滑动拉取数据。
 */
public class RiskDetailDemo {

    // 产品id，每个应用接入时，会分配secretId和私钥secretKey。
    private static final String SECRET_ID = "your_secret_id";

    // 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
    private static final String SECRET_KEY = "your_secret_key";

    // 每个业务接入时，均会分配业务 ID
    private static final String BUSINESS_ID = "your_business_id";

    // 版本号，如400
    private static final String VERSION = "400";

    // 接口URL
    private static final String API_URL = "http://ir-open.dun.163.com/v4/risk/detail";

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

        // 使用secretKey签名的数据，校验权限
        String signature = SignatureUtils.genSignature(SECRET_KEY, params);
        params.put("signature", signature);
        params.put("beginTimestamp", 1667959831798L);
        params.put("endTimestamp", 1667959915103L);
        // 用于分页查询的关联标记
        params.put("startFlag", "");
        // 用户/玩家的账号
        params.put("account", "zzzzzzz");
        // 用户/玩家的角色ID
        params.put("roleId", "yyyyyyy");
        // 风险等级, 10-高风险, 20-中风险, 30-低风险
        params.put("riskLevel", 10);
        // 包名
        params.put("packageName", "com.aaa.bbb");
        // app版本
        params.put("appVersion", "1.0.2");
        params.put("ip", "192.168.1.1");


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

package com.netease.irisk.openapi.demo.v5;

import com.alibaba.fastjson.JSONObject;
import com.netease.irisk.openapi.demo.util.HttpUtil;
import com.netease.irisk.openapi.demo.util.SignatureUtils;

import java.io.UnsupportedEncodingException;
import java.util.HashMap;
import java.util.Map;

/**
 * @author dongyisheng@corp.netease.com
 * @date 2023/6/6 17:31
 */
public class RiskReportDataDemo {

    // 产品id，每个应用接入时，会分配secretId和私钥secretKey。
    private static final String SECRET_ID = "your_secret_id";

    // 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
    private static final String SECRET_KEY = "your_secret_key";

    // 每个业务接入时，均会分配业务 ID
    private static final String BUSINESS_ID = "your_business_id";

    // 版本号，如500
    private static final String VERSION = "500";

    // 接口URL
    private static final String API_URL = "http://ir-open.dun.163.com/v5/risk/reportData";

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
            System.out.printf("请求失败, msg=%s, desc=%s%n", msg, jsonObject.getString("desc"));
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

        // 举报渠道
        params.put("reportChannel", "reportChannel");
        // 举报时间
        params.put("reportTime", System.currentTimeMillis());
        // 举报人信息
        Map<String, Object> whistleblowerMap = new HashMap<>();
        whistleblowerMap.put("account", "account01");
        whistleblowerMap.put("roleId", "roleId01");
        whistleblowerMap.put("roleName", "roleName01");
        whistleblowerMap.put("serverId", "serverId01");
        whistleblowerMap.put("level", "10");
        whistleblowerMap.put("recharge", "9999");
        params.put("whistleblower", JSONObject.toJSONString(whistleblowerMap));
        // 被举报人信息
        Map<String, Object> reportedPerson = new HashMap<>();
        reportedPerson.put("account", "account02");
        reportedPerson.put("roleId", "roleId02");
        reportedPerson.put("roleName", "roleName02");
        reportedPerson.put("serverId", "serverId02");
        reportedPerson.put("level", "10");
        reportedPerson.put("recharge", "9999");
        params.put("reportedPerson", JSONObject.toJSONString(reportedPerson));
        // 举报类型
        params.put("reportType", "reportType01");
        // 举报场景
        params.put("reportScene", "reportScene01");
        // 举报内容详情
        params.put("reportData", "data");

        // 使用secretKey签名的数据，校验权限
        String signature = SignatureUtils.genSignature(SECRET_KEY, params);
        params.put("signature", signature);
        return params;
    }
}

package com.netease.irisk.openapi.demo;

import com.alibaba.fastjson.JSONObject;
import com.netease.irisk.openapi.demo.util.HttpUtil;
import com.netease.irisk.openapi.demo.util.SignatureUtils;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.Base64;
import java.util.HashMap;
import java.util.Map;
import java.util.Scanner;

/**
 * 本接口通过AI算法对上报的图片进行分析，识别是否存在外挂行为。
 */
public class RiskMediaCheckDemo {

    // 产品id，每个应用接入时，会分配secretId和私钥secretKey。
    private static final String SECRET_ID = "your_secret_id";

    // 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
    private static final String SECRET_KEY = "your_secret_key";

    // 每个业务接入时，均会分配业务 ID
    private static final String BUSINESS_ID = "your_business_id";

    // 版本号，如500
    private static final String VERSION = "500";

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

    private static Map<String, Object> getParams() throws IOException {
        Map<String, Object> params = new HashMap<>();
        // 调用接口当前时间，单位毫秒
        Long timeStamp = System.currentTimeMillis();
        params.put("secretId", SECRET_ID);
        params.put("businessId", BUSINESS_ID);
        params.put("nonce", NONCE);
        params.put("timestamp", timeStamp);
        params.put("version", VERSION);
        // 图片数据，图片支持编码为BASE64的数据，无需包含base64编码请求头部分
        String filePath = "your_img_path";
        String mediaData = imgEncode(filePath);
        params.put("mediaData", mediaData);
        // 图片文件名，格式如xxx.jpg，需要包含.格式的文件后缀名
        params.put("mediaName", "xxx.jpg");
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

    /**
     * 根据图片路径进行base64编码
     * @param filePath
     * @return
     * @throws IOException
     */
    private static String imgEncode(String filePath) throws IOException {
        byte[] fileBytes = Files.readAllBytes(Paths.get(filePath));
        return Base64.getEncoder().encodeToString(fileBytes);
    }

    /**
     * 从文件中读取字符串
     * @param filePath
     * @return
     * @throws FileNotFoundException
     */
    private static String getStringFromFile(String filePath) throws FileNotFoundException {
        // 创建一个File对象，表示要读取的文件
        File file = new File(filePath);

        // 创建一个Scanner对象，关联到文件
        Scanner scanner = new Scanner(file);
        StringBuilder sb = new StringBuilder();

        // 逐行读取文件中的字符串数据，直到文件结束
        while (scanner.hasNextLine()) {
            String line = scanner.nextLine();
            sb.append(line);
        }
        // 关闭Scanner对象
        scanner.close();
        return sb.toString();
    }

}

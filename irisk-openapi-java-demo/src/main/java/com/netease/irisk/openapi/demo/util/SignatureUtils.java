package com.netease.irisk.openapi.demo.util;

import org.apache.commons.codec.digest.DigestUtils;
import org.apache.commons.lang3.ObjectUtils;
import org.apache.commons.lang3.StringUtils;

import java.io.UnsupportedEncodingException;
import java.util.Arrays;
import java.util.Map;

public class SignatureUtils {

    /**
     * 签名方法，注意params的value不要为null(空字符串和null的区别)，否则可能造成前后端签名不一致
     *
     * @param secretKey
     * @param params
     * @return
     */
    public static String genSignature(String secretKey, Map<String, Object> params) throws UnsupportedEncodingException {
        String signature = "";
        if (StringUtils.isEmpty(secretKey) || params.isEmpty()) {
            return signature;
        }
        // 1. 参数名按照ASCII码表升序排序
        String[] keys = params.keySet().toArray(new String[0]);
        Arrays.sort(keys);

        // 2. 按照排序拼接参数名与参数值
        StringBuilder paramBuffer = new StringBuilder();
        for (String key : keys) {
            paramBuffer.append(key).append(params.get(key) == null ? "" : params.get(key));
        }
        // 3. 将secretKey拼接到最后
        paramBuffer.append(secretKey);

        // 4. MD5是128位长度的摘要算法，用16进制表示，一个十六进制的字符能表示4个位，所以签名后的字符串长度固定为32个十六进制字符。
        signature = DigestUtils.md5Hex(paramBuffer.toString().getBytes("UTF-8"));
        return signature;
    }
}

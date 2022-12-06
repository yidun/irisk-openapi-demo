<?php
/** 反外挂嫌疑在线检测接口API示例 */
/** 产品ID，从【易盾官网-服务管理-已开通业务】页面获取 */
const SECRET_ID = "your_secretk_id";
/** 密钥，从【易盾官网-服务管理-已开通业务】页面获取 */
const SECRET_KEY = "your_secretk_key";
const BUSINESS_ID = "your_business_id";
const VERSION = "400";
/** 接口URL */
const API_URL = "http://ir-open.dun.163.com/v4/risk/getConfig";
/** API timeout*/
const API_TIMEOUT = 2;
require("../../util.php");

/**
 * 反外挂请求接口简单封装
 * $params 请求参数
 */
function check($params)
{
    $params["secretId"] = SECRET_ID;
    $params["businessId"] = BUSINESS_ID;
    $params["version"] = VERSION;
    $params["timestamp"] = time() * 1000;
    // 随机码 32位
    $params["nonce"] = "mmm888f73yyy59440583zzz9bfcc79de";
    $params["ip"] = "1.1.1.1";
    $params["sdkData"] = "your_sdk_data";

    $params["signature"] = gen_signature(SECRET_KEY, $params);
    $params = toUtf8($params);


    $result = curl_post($params, API_URL, API_TIMEOUT);
    if ($result === FALSE) {
        return array("code" => 500, "msg" => "file_get_contents failed.");
    } else {
        return json_decode($result, true);
    }
}

/**
 * 计算参数签名
 * $params 请求参数
 * $secretKey secretKey
 */
function gen_signature($secretKey, $params){
    $params = array(
        "secretId" => $params["secretId"],
        "businessId" => $params["businessId"],
        "version" => $params["version"],
        "timestamp" => $params["timestamp"],
        "nonce" => $params["nonce"],
        "ip" => $params["ip"],
        "sdkData" => $params["sdkData"],
    );
    ksort($params);
    $buff="";
    foreach($params as $key=>$value){
        if($value !== null) {
            $buff .=$key;
            $buff .=$value;
        }
    }
    $buff .= $secretKey;
    return md5($buff);
}

// 简单测试
function main()
{
    echo "mb_internal_encoding=" . mb_internal_encoding() . "\n";

    $params = array(

    );
    var_dump($params);

    $ret = check($params);
    var_dump($ret);
    if ($ret["code"] == 200) {
        $data = $ret["data"];
        echo "请求成功，data: {$data}";
        var_dump($ret);
    } else {
        $msg = $ret["msg"];
        echo "请求失败，msg: {$msg}";
        var_dump($ret);
    }
}

main();
?>
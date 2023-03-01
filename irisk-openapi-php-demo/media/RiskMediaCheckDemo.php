<?php
/** 本接口通过AI算法对上报的图片进行分析，识别是否存在外挂行为。 */
/** 产品ID，从【易盾官网-服务管理-已开通业务】页面获取 */
const SECRET_ID = "your_secret_id";
/** 密钥，从【易盾官网-服务管理-已开通业务】页面获取 */
const SECRET_KEY = "your_secret_key";
const BUSINESS_ID = "your_business_id";
const VERSION = "400";
/** 接口URL */
const API_URL = "http://ir-open.dun.163.com/v5/risk/mediaCheck";
/** API timeout*/
const API_TIMEOUT = 5;
require("../util.php");

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
    // 图片数据，图片支持编码为BASE64的数据，无需包含base64编码请求头部分
    $params["mediaData"] = "auMW9NLW5rNaa6vXVpq2jTfy1Kemr2UuWyvu9L7662dvL7Oik3cp5J5PJ/dr35/56UrrvP5ML+X/pJ//9k=";
    // 图片文件名，格式如xxx.jpg，需要包含.格式的文件后缀名
    $params["mediaName"] = "xxx.jpg";
    // 用户/ 玩家的IP，或当前客户端业务事件发生时的公网IP地址（ipv4）
    $params["ip"] = "183.136.182.141";
    // 用户/玩家的角色 ID，非游戏类型应用，roleId 可以与 roleAccount 相同
    $params["roleId"] = "yyyyyyy";
    // 用户/玩家的角色名称，非游戏类型应用，roleName 可以是当前用户昵称相同
    $params["nickname"] = "yyyyyyy";
    // 用户/玩家的角色的服务器名称
    $params["server"] = "yyyyyyy";
    $params["signature"] = gen_signature(SECRET_KEY, $params);
    $params = toUtf8($params);


    $result = curl_post($params, API_URL, API_TIMEOUT);
    if ($result === FALSE) {
        return array("code" => 500, "msg" => "file_get_contents failed.");
    } else {
        return json_decode($result, true);
    }
}

// 简单测试
function main()
{
    echo "mb_internal_encoding=" . mb_internal_encoding() . "\n";

    $params = array(
    );

    $ret = check($params);
    var_dump($params);
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
<?php
/** 反外挂嫌疑在线检测接口API示例 */
/** 产品ID，从【易盾官网-服务管理-已开通业务】页面获取 */
const SECRET_ID = "your_secretk_id";
/** 密钥，从【易盾官网-服务管理-已开通业务】页面获取 */
const SECRET_KEY = "your_secretk_key";
const BUSINESS_ID = "your_business_id";
const VERSION = "400";
/** 接口URL */
const API_URL = "http://ir-open.dun.163.com/v4/risk/check";
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
    // 风控SDK上报的数据后回参获取到的 token，应用方需要从智能风控客户端SDK获取该数据。详情请查看客户端接入文档。
    $params["token"] = "your_token";
    // 用户/ 玩家的IP，或当前客户端业务事件发生时的公网IP地址（ipv4）
    $params["ip"] = "1.1.1.1";
    $params["signature"] = gen_signature(SECRET_KEY, $params);
    // 用户/玩家的角色 ID，非游戏类型应用，roleId 可以与 roleAccount 相同
    $params["roleId"] = "yyyyyyy";
    // 用户/玩家的角色名称，非游戏类型应用，roleName 可以是当前用户昵称相同
    $params["roleName"] = "yyyyyyy";
    // 用户/玩家的角色的服务器名称
    $params["roleServer"] = "yyyyyyy";
    // 用户/玩家的账号
    $params["account"] = "zzzzzzz";
    // 用户/玩家的等级
    $params["riskLevel"] = 150;
    // 游戏类型应用的版本号
    $params["gameVersion"] = "1.0.2";
    // 游戏类型应用的资源版本号
    $params["assetVersion"] = "assetVersion";
    // 额外/拓展的信息，应用 / 游戏方可以自己构建json结构，最大长度：2048。不同场景构建信息见分场景接入说明
    $params["extData"] = "";
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
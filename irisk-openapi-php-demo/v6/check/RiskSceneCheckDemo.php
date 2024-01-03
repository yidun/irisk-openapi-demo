<?php
/** 本接口用于智能风控嫌疑数据在线检测，并且接口会同步返回检测结果。 */
/** 产品ID，从【易盾官网-服务管理-已开通业务】页面获取 */
const SECRET_ID = "your_secret_id";
/** 密钥，从【易盾官网-服务管理-已开通业务】页面获取 */
const SECRET_KEY = "your_secret_key";
const BUSINESS_ID = "you_business_id";
const VERSION = "603";
/** 接口URL */
const API_URL = "http://ir-open.dun.163.com/v6/risk/check";
/** API timeout*/
const API_TIMEOUT = 5;
require("../../util.php");

/**
 * 智能风控请求接口简单封装
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
    $params["ip"] = "183.136.182.141";
    // 用户/玩家的角色 ID，非游戏类型应用，roleId 可以与 roleAccount 相同
    $params["roleId"] = "yyyyyyy";
    // 用户/玩家的角色名称，非游戏类型应用，roleName 可以是当前用户昵称相同
    $params["nickname"] = "yyyyyyy";
    // 用户/玩家的角色的服务器名称
    $params["server"] = "yyyyyyy";
    // 用户/玩家的账号
    $params["account"] = "zzzzzzz";
    // 用户/玩家的等级
    $params["level"] = 150;
    // 当前用户身份，有助于对不同身份用户设置分层策略，方便业务进行精细化安全运营。
    $params["identity"] = "visitor";
    // 当前用户是否付费用户，有助于对用户设置分层策略。
    $params["payUser"] = "true";
    // 当前用户是否认证用户（实名/真人认证），有助于对不同安全等级用户设置分层策略。
    $params["verified"] = "true";
    // 额外/拓展的信息，应用 / 游戏方可以自己构建json结构，最大长度：2048。不同场景构建信息见分场景接入说明
    $params["extData"] = "";
    // 活动操作的目标，比如：A给B点赞，则target为B。如果target是手机号或邮箱，请提供hash值，hash算法：md5(target)。如没有，可传空
    $params["target"] = "";
    // 活动的唯一标识，用于标记场景下的细分类别，如：注册-自主注册、注册-受邀请注册；再如：登录- app登录、登录-web登录等
    $params["activityId"] = "";
    // 用户用于登录的手机号码或者关联的手机号码，默认国内手机号。如有海外手机，需包含国家地区代码，
    // 格式为“+447410xxx186（+44即为国家码）”。如果需要加密，支持传入hash值，hash算法：md5(phone)
    $params["phone"] = "";
    // 用户的邮箱，如果需要加密，支持传入hash值，hash算法：md5(email)
    $params["email"] = "";
    // 用户的注册IP
    $params["registerIp"] = "";
    // 用户的注册时间，单位：毫秒
    $params["registerTime"] = time() * 1000;
    // 场景信息，json格式，不同场景构建信息见分场景接入说明
    $params["sceneData"] = "{\"appChannel\":\"app_store\",\"callbackUrl\":\"your_call_back_url\",\"createTime\":1688106777462,\"currency\":\"USD\",\"money\":1.22,\"orderCreateTime\":1688106777462,\"orderFinishTime\":1688106777462,\"orderId\":\"GPA.3329-6166-4658-28227-01\",\"orderPayTime\":1688106777462,\"payChannel\":\"如app_store\",\"payCurrency\":\"USD\",\"payMoney\":1.11,\"payTime\":1688106777462,\"sceneType\":\"APP_OR_GAME_RECHARGE\"}";
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
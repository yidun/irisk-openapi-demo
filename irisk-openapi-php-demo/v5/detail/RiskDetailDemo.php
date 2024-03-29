<?php
/**
 * 本接口的功能是智能风控明细数据查询，主要用于数据同步：从易盾拉取数据场景，以固定时间窗口拉取数据。
 * 例如，每次查询1分钟前的数据，时间跨度也是1分钟，则可以按1分钟时间窗口，周期性滑动拉取数据。
 */
/** 产品ID，从【易盾官网-服务管理-已开通业务】页面获取 */
const SECRET_ID = "your_secret_id";
/** 密钥，从【易盾官网-服务管理-已开通业务】页面获取 */
const SECRET_KEY = "your_secret_key";
const BUSINESS_ID = "your_business_id";
const VERSION = "500";
/** 接口URL */
const API_URL = "http://ir-open.dun.163.com/v5/risk/detail";
/** API timeout*/
const API_TIMEOUT = 2;
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
    $params["beginTimestamp"] = 1667959831798;
    $params["endTimestamp"] = 1667959915103;
    // 用于分页查询的关联标记
    $params["startFlag"] = "1.1.1.1";
    // 用户/玩家的账号
    $params["account"] = "zzzzzzz";
    // 用户/玩家的角色ID
    $params["roleId"] = "yyyyyyy";
    // 风险等级, 10-高风险, 20-中风险, 30-低风险
    $params["riskLevel"] = "10";
    // 包名(仅限Android/iOS平台)
    $params["packageName"] = "com.aaa.bbb";
    // app版本(仅限Android/iOS平台)
    $params["appVersion"] = "1.0.2";
    $params["ip"] = "192.168.1.1";
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
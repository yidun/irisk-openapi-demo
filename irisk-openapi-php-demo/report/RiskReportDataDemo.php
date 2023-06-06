<?php
/** 本接口用于上报应用的举报信息。 */
/** 产品ID，从【易盾官网-服务管理-已开通业务】页面获取 */
const SECRET_ID = "your_secret_id";
/** 密钥，从【易盾官网-服务管理-已开通业务】页面获取 */
const SECRET_KEY = "your_secret_key";
const BUSINESS_ID = "your_business_id";
const VERSION = "500";
/** 接口URL */
const API_URL = "http://ir-open.dun.163.com/v5/risk/reportData";
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
    // 举报渠道
    $params['reportChannel'] = 'reportChannel';
    // 举报时间
    $params['reportTime'] = round(microtime(true) * 1000);
    // 举报人信息
    $whistleblowerMap = array(
        'account' => 'account01',
        'roleId' => 'roleId01',
        'roleName' => 'roleName01',
        'serverId' => 'serverId01',
        'level' => '10',
        'recharge' => '9999'
    );
    $params['whistleblower'] = json_encode($whistleblowerMap);
    // 被举报人信息
    $reportedPerson = array(
        'account' => 'account02',
        'roleId' => 'roleId02',
        'roleName' => 'roleName02',
        'serverId' => 'serverId02',
        'level' => '10',
        'recharge' => '9999'
    );
    $params['reportedPerson'] = json_encode($reportedPerson);
    // 举报类型
    $params['reportType'] = 'reportType01';
    // 举报场景
    $params['reportScene'] = 'reportScene01';
    // 举报内容详情
    $params['reportData'] = 'data';
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
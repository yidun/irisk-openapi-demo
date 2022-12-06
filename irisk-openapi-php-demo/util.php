<?php
/** php内部使用的字符串编码 */
define("INTERNAL_STRING_CHARSET", "auto");
/** api signatureMethod,默认MD5 */
define("SIGNATURE_METHOD", "MD5");

/**
 * curl post请求
 * @params 输入的参数
 */
function curl_post($params, $url, $timout){
    $ch = curl_init();
    $json = json_encode($params);
    curl_setopt($ch, CURLOPT_URL, $url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
    // 设置超时时间
    curl_setopt($ch, CURLOPT_TIMEOUT, $timout);
    // POST数据
    curl_setopt($ch, CURLOPT_POST, 1);
    // 把post的变量加上
    curl_setopt($ch, CURLOPT_POSTFIELDS, $json);
    curl_setopt($ch, CURLOPT_HTTPHEADER, array('Content-Type:'.'application/json; charset=UTF-8'));
    $output = curl_exec($ch);
    curl_close($ch);
    return $output;
}

/**
 * 将输入数据的编码统一转换成utf8
 * @params 输入的参数
 */
function toUtf8($params){
    $utf8s = array();
    foreach ($params as $key => $value) {
        $utf8s[$key] = is_string($value) ? mb_convert_encoding($value, "utf8", INTERNAL_STRING_CHARSET) : $value;
    }
    return $utf8s;
}


?>
var utils = require("./utils");
// 产品id，每个应用接入时，会分配secretId和私钥secretKey。
var secretId = "your_secretk_id";
// 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
var secretKey = "your_secretk_key";
// 每个业务接入时，均会分配业务 ID
var businessId = "your_business_id";
// 版本号，如400
var version = "400";
// 本接口用于透传使用，获取sdk所需的配置参数以及特征配置等信息。
var apiurl = "http://ir-open.dun.163.com/v4/risk/getConfig";
//请求参数
var post_data = {
    // 设置用于计算签名的参数
    secretId: secretId,
    businessId: businessId,
    version: version,
    timestamp: new Date().getTime(),
    nonce: "mmm888f73yyy59440583zzz9bfcc79de",
    ip: "1.1.1.1",
    sdkData: "your_sdk_data"
}
var signature = utils.genSignature(secretKey, post_data);
post_data.signature = signature;
//http请求结果
var responseCallback = function (responseData) {
    var data = JSON.parse(responseData);
    var code = data.code;
    var msg = data.msg;
    if (code == 200) {
        var result = JSON.stringify(data.data);
        console.log('请求成功:code=' + code + ',msg=' + msg);
        console.log('data=' + result);
    } else {
        console.log('请求失败:code=' + code + ',msg=' + msg);
    }
}
utils.sendHttpRequest(apiurl, "POST", post_data, responseCallback);
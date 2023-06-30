var utils = require("../utils");
// 产品id，每个应用接入时，会分配secretId和私钥secretKey。
var secretId = "your_secret_id";
// 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
var secretKey = "your_secret_key";
// 每个业务接入时，均会分配业务 ID
var businessId = "your_business_id";
// 版本号，如500
var version = "500";
// 当客户端出现风控SDK接口被屏蔽，获取不到业务配置信息时（如初始化功能配置、特征配置等），可通过此接口获取业务配置信息，并下发到客户端完成配置。
var apiurl = "http://ir-open.dun.163.com/v5/risk/getConfig";
//请求参数
var post_data = {
    // 设置用于计算签名的参数
    secretId: secretId,
    businessId: businessId,
    version: version,
    timestamp: new Date().getTime(),
    nonce: "mmm888f73yyy59440583zzz9bfcc79de",
    ip: "1.1.1.1",
    sdkData: "your_sdk_data",
    extData: "your_sdk_data",
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
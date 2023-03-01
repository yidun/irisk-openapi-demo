var utils = require("./utils");
// 产品id，每个应用接入时，会分配secretId和私钥secretKey。
var secretId = "1ded450ce116bcffd62e603c99ee7834";
// 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
var secretKey = "accd3781b86e95d8a84c81b2c62e9e71";
// 每个业务接入时，均会分配业务 ID
var businessId = "ff1c6edcdec98a00d3eddc763c52d1e2";
// 版本号，如400
var version = "400";
// 本接口通过AI算法对上报的图片进行分析，识别是否存在外挂行为。
var apiurl = "http://ir-open.dun.163.com/v5/risk/mediaCheck";
//请求参数
var post_data = {
    // 设置用于计算签名的参数
    secretId: secretId,
    businessId: businessId,
    version: version,
    timestamp: new Date().getTime(),
    nonce: "mmm888f73yyy59440583zzz9bfcc79de",
    // 图片数据，图片支持编码为BASE64的数据，无需包含base64编码请求头部分
    mediaData: "auMW9NLW5rNaa6vXVpq2jTfy1Kemr2UuWyvu9L7662dvL7Oik3cp5J5PJ/dr35/56UrrvP5ML+X/pJ//9k=",
    // 图片文件名，格式如xxx.jpg，需要包含.格式的文件后缀名
    mediaName: "xxx.jpg",
    // 用户/ 玩家的IP，或当前客户端业务事件发生时的公网IP地址（ipv4）
    ip: "1.1.1.1",
    roleId: "yyyyyyy",
    nickname: "yyyyyyy",
    server: "",
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
var utils = require("./utils");
// 产品id，每个应用接入时，会分配secretId和私钥secretKey。
var secretId = "1ded450ce116bcffd62e603c99ee7834";
// 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
var secretKey = "accd3781b86e95d8a84c81b2c62e9e71";
// 每个业务接入时，均会分配业务 ID
var businessId = "ff1c6edcdec98a00d3eddc763c52d1e2";
// 版本号，如400
var version = "400";
// 本接口的功能是智能风控明细数据查询，主要用于数据同步：从易盾拉取数据场景，以固定时间窗口拉取数据。
var apiurl = "http://localhost:28082/v5/risk/detail";
//请求参数
var post_data = {
    // 设置用于计算签名的参数
    secretId: secretId,
    businessId: businessId,
    version: version,
    timestamp: new Date().getTime(),
    nonce: "mmm888f73yyy59440583zzz9bfcc79de",
    beginTimestamp: new Date().getTime(),
    endTimestamp: new Date().getTime(),
    // 用于分页查询的关联标记
    startFlag: "",
    // 用户/玩家的账号
    account: "",
    // 用户/玩家的角色ID
    roleId: "",
    // 风险等级, 1-低风险, 2-中风险, 3-高风险
    riskLevel: 1,
    // 包名
    packageName: "com.aaa.bbb",
    // app版本号
    appVersion: "",
    ip: "192.168.1.1",
    appVersion: "1.0.2",
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
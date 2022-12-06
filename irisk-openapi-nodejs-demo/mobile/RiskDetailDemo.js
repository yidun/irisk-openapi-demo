var utils = require("./utils");
// 产品id，每个应用接入时，会分配secretId和私钥secretKey。
var secretId = "your_secretk_id";
// 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
var secretKey = "your_secretk_key";
// 每个业务接入时，均会分配业务 ID
var businessId = "your_business_id";
// 版本号，如400
var version = "400";
// 本接口的功能是智能风控明细数据查询，主要用于数据同步：从易盾拉取数据场景，以固定时间窗口拉取数据。
var apiurl = "http://ir-open.dun.163.com/v4/risk/detail";
//请求参数
var post_data = {
    // 1.设置用于计算签名的参数
    secretId: secretId,
    businessId: businessId,
    version: version,
    timestamp: new Date().getTime(),
    nonce: "mmm888f73yyy59440583zzz9bfcc79de"
}
var signature = utils.genSignature(secretKey, post_data);
post_data.signature = signature;
// 2.设置私有参数
post_data.beginTimestamp = new Date().getTime();
post_data.endTimestamp = new Date().getTime();
// 用于分页查询的关联标记
post_data.startFlag = "";
// 用户/玩家的账号
post_data.account = "zzzzzzz";
// 用户/玩家的角色ID
post_data.roleId = "yyyyyyy";
// 风险等级, 10-高风险, 20-中风险, 30-低风险
post_data.riskLevel = 10;
// 包名
post_data.packageName = "com.aaa.bbb";
// app版本号
post_data.appVersion = "1.0.2";
post_data.ip = "192.168.1.1";
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
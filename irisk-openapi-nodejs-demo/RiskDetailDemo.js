var utils = require("./utils");
// 产品id，每个应用接入时，会分配secretId和私钥secretKey。
var secretId = "your_secret_id";
// 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
var secretKey = "your_secret_key";
// 每个业务接入时，均会分配业务 ID
var businessId = "your_business_id";
// 版本号，如500
var version = "500";
// 本接口的功能是智能风控明细数据查询，主要用于数据同步：从易盾拉取数据场景，以固定时间窗口拉取数据。
// 例如，每次查询1分钟前的数据，时间跨度也是1分钟，则可以按1分钟时间窗口，周期性滑动拉取数据。
var apiurl = "http://ir-open.dun.163.com/v5/risk/detail";
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
    // 包名(仅限Android/iOS平台)
    packageName: "com.aaa.bbb",
    // app版本(仅限Android/iOS平台)
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
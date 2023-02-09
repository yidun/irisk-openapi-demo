var utils = require("../utils");
// 产品id，每个应用接入时，会分配secretId和私钥secretKey。
var secretId = "your_secretk_id";
// 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
var secretKey = "your_secretk_key";
// 每个业务接入时，均会分配业务 ID
var businessId = "your_business_id";
// 版本号，如400
var version = "400";
// 本接口用于智能风控检测结果数据在线查询（唯有调用此接口后，才会对上报数据进行检测并获取命中结果。)
var apiurl = "http://ir-open.dun.163.com/v4/web/check";
//请求参数
var post_data = {
    // 1.设置用于计算签名的参数
    secretId: secretId,
    businessId: businessId,
    version: version,
    timestamp: new Date().getTime(),
    nonce: "mmm888f73yyy59440583zzz9bfcc79de",
    // 风控SDK上报的数据后回参获取到的 token，应用方需要从智能风控客户端SDK获取该数据。详情请查看客户端接入文档。
    token: "your_token",
    // 用户/ 玩家的IP，或当前客户端业务事件发生时的公网IP地址（ipv4）
    ip: "1.1.1.1",
}
var signature = utils.genSignature(secretKey, post_data);
post_data.signature = signature;
// 2.设置私有参数
// 用户/玩家的角色 ID，非游戏类型应用，roleId 可以与 roleAccount 相同
post_data.roleId = "yyyyyyy";
// 用户/玩家的角色名称，非游戏类型应用，roleName 可以是当前用户昵称相同
post_data.roleName = "yyyyyyy";
// 用户/玩家的角色的服务器名称
post_data.roleServer = "yyyyyyy";
// 用户/玩家的账号
post_data.account = "zzzzzzz";
// 用户/玩家的等级
post_data.roleLevel = 150;
// 游戏类型应用的版本号
post_data.gameVersion = "1.0.2";
// 游戏类型应用的资源版本号
post_data.assetVersion = "assetVersion";
// 额外/拓展的信息，应用 / 游戏方可以自己构建json结构，最大长度：2048。不同场景构建信息见分场景接入说明
post_data.extData = "";
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
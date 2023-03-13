var utils = require("./utils");
// 产品id，每个应用接入时，会分配secretId和私钥secretKey。
var secretId = "1ded450ce116bcffd62e603c99ee7834";
// 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
var secretKey = "accd3781b86e95d8a84c81b2c62e9e71";
// 每个业务接入时，均会分配业务 ID
var businessId = "ff1c6edcdec98a00d3eddc763c52d1e2";
// 版本号，如500
var version = "500";
// 本接口用于智能风控嫌疑数据在线检测，并且接口会同步返回检测结果。
var apiurl = "http://ir-open.dun.163.com/v5/risk/check";
//请求参数
var post_data = {
    // 设置用于计算签名的参数
    secretId: secretId,
    businessId: businessId,
    version: version,
    timestamp: new Date().getTime(),
    nonce: "mmm888f73yyy59440583zzz9bfcc79de",
    // 风控SDK上报的数据后回参获取到的 token，应用方需要从智能风控客户端SDK获取该数据。详情请查看客户端接入文档。
    token: "your_token",
    // 用户/ 玩家的IP，或当前客户端业务事件发生时的公网IP地址（ipv4）
    ip: "1.1.1.1",
    // 用户/玩家的角色 ID，非游戏类型应用，roleId 可以与 account 相同
    roleId: "yyyyyyy",
    // 用户/玩家的昵称
    nickname: "yyyyyyy",
    // 用户/玩家的角色服务器名称
    server: "",
    // 用户/玩家的账号
    account: "yyyyyyy",
    // 用户/玩家的等级
    level: 150,
    // 游戏类型应用的版本号
    gameVersion: "1.0.2",
    // 游戏类型应用的资源版本号
    assetVersion: "assetVersion",
    // 额外/拓展的信息，应用 / 游戏方可以自己构建json结构，最大长度：2048。不同场景构建信息见分场景extData接入说明
    extData: "",
    // 活动操作的目标，比如：A给B点赞，则target为B。如果target是手机号或邮箱，请提供hash值，hash算法：md5(target)。如没有，可传空
    target: "",
    // 活动的唯一标识，用于标记场景下的细分类别，如：注册-自主注册、注册-受邀请注册；再如：登录- app登录、登录-web登录等
    activityId: "",
    // 用户用于登录的手机号码或者关联的手机号码，默认国内手机号。如有海外手机，需包含国家地区代码，
    // 格式为“+447410xxx186（+44即为国家码）”。如果需要加密，支持传入hash值，hash算法：md5(phone)
    phone: "",
    // 用户的邮箱，如果需要加密，支持传入hash值，hash算法：md5(email)
    email: "",
    // 用户的注册IP
    registerIp: "",
    // 用户的注册时间，单位：毫秒
    registerTime: new Date().getTime(),
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
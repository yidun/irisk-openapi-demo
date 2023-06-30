import hashlib
import time
import urllib.request as urlrequest
import json


class RiskCheckDemo(object):
    """本接口用于智能风控嫌疑数据在线检测，并且接口会同步返回检测结果。"""

    API_URL = "http://ir-open.dun.163.com/v6/risk/check"

    def __init__(self, secret_id, secret_key, business_id, version):
        """
        Args:
            secret_id (str) 产品id，每个应用接入时，会分配secretId和私钥secretKey。
            secret_key (str) 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
            business_id (str) 每个业务接入时，均会分配业务 ID
            version (str) 版本号，如600
        """
        self.secret_id = secret_id
        self.secret_key = secret_key
        self.business_id = business_id
        self.version = version

    def gen_signature(self, params):
        buff = ""
        for k in sorted(params.keys()):
            buff += str(k) + str(params[k])
        buff += self.secret_key
        return hashlib.md5(buff.encode("utf8")).hexdigest()

    def check(self, params):
        """请求易盾接口
        Args:
            params (object) 请求参数
        Returns:
            请求结果，json格式
        """
        params["secretId"] = self.secret_id
        params["businessId"] = self.business_id
        params["version"] = self.version
        params["timestamp"] = int(time.time() * 1000)
        # 随机码，32位
        params["nonce"] = "mmm888f73yyy59440583zzz9bfcc79de"
        params["signature"] = self.gen_signature(params)

        try:
            headers = {"Content-Type": 'application/json'}
            params = json.dumps(params)
            params = bytes(params, 'utf8')
            request = urlrequest.Request(url=self.API_URL, data=params, headers=headers)
            content = urlrequest.urlopen(request, timeout=1).read()
            return json.loads(content)
        except Exception as ex:
            print("调用API接口失败:", str(ex))


if __name__ == "__main__":
    """示例代码入口"""
    SECRET_ID = "your_secret_id"
    SECRET_KEY = "your_secret_key"
    BUSINESS_ID = "your_business_id"
    VERSION = "600"
    api = RiskCheckDemo(SECRET_ID, SECRET_KEY, BUSINESS_ID, VERSION)

    params = {
        # 更多参数见官方文档
        # 风控SDK上报的数据后回参获取到的 token，应用方需要从智能风控客户端SDK获取该数据。详情请查看客户端接入文档。
        "token": "your_token",
        # 用户/ 玩家的IP，或当前客户端业务事件发生时的公网IP地址（ipv4）
        "ip": "1.1.1.1",
        # 用户/玩家的角色 ID，非游戏类型应用，roleId 可以与 roleAccount 相同
        "roleId": "yyyyyyy",
        # 用户/玩家的角色名称，非游戏类型应用，roleName 可以是当前用户昵称相同
        "nickname": "yyyyyyy",
        # 用户/玩家的角色的服务器名称
        "server": "yyyyyyy",
        # 用户/玩家的账号
        "account": "zzzzzzz",
        # 用户/玩家的等级
        "level": "150",
        # 游戏类型应用的版本号
        "gameVersion": "1.0.2",
        # 游戏类型应用的资源版本号
        "assetVersion": "0.2.1",
        # 额外/拓展的信息，应用 / 游戏方可以自己构建json结构，最大长度：2048。不同场景构建信息见分场景接入说明
        "extData": "",
        # 活动操作的目标，比如：A给B点赞，则target为B。如果target是手机号或邮箱，请提供hash值，hash算法：md5(target)。如没有，可传空
        "target": "",
        # 活动的唯一标识，用于标记场景下的细分类别，如：注册-自主注册、注册-受邀请注册；再如：登录- app登录、登录-web登录等
        "activityId": "",
        # 用户用于登录的手机号码或者关联的手机号码，默认国内手机号。如有海外手机，需包含国家地区代码，
        # 格式为“+447410xxx186（+44即为国家码）”。如果需要加密，支持传入hash值，hash算法：md5(phone)
        "phone": "",
        # 用户的邮箱，如果需要加密，支持传入hash值，hash算法：md5(email)
        "email": "",
        # 用户的注册IP
        "registerIp": "",
        # 用户的注册时间，单位：毫秒
        "registerTime": int(time.time() * 1000),
        "sceneData": "{\"appChannel\":\"app_store\",\"callbackUrl\":\"your_call_back_url\",\"createTime\":1688106777462,\"currency\":\"USD\",\"money\":1.22,\"orderCreateTime\":1688106777462,\"orderFinishTime\":1688106777462,\"orderId\":\"GPA.3329-6166-4658-28227-01\",\"orderPayTime\":1688106777462,\"payChannel\":\"如app_store\",\"payCurrency\":\"USD\",\"payMoney\":1.11,\"payTime\":1688106777462,\"sceneType\":\"APP_OR_GAME_RECHARGE\"}"
    }

    ret = api.check(params)

    code: int = ret["code"]
    msg: str = ret["msg"]
    if code == 200:
        print("msg=%s, data=%s" % (msg, ret["data"]))
    else:
        print("ERROR: code=%s, msg=%s, desc=%s" % (ret["code"], ret["msg"], ret["desc"]))

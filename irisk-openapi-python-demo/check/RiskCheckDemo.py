import hashlib
import time
import urllib.request as urlrequest
import json


class RiskCheckDemo(object):
    """本接口用于智能风控检测结果数据在线查询（唯有调用此接口后，才会对上报数据进行检测并获取命中结果。)"""

    API_URL = "http://ir-open.dun.163.com/v5/risk/check"

    def __init__(self, secret_id, secret_key, business_id, version):
        """
        Args:
            secret_id (str) 产品id，每个应用接入时，会分配secretId和私钥secretKey。
            secret_key (str) 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
            business_id (str) 每个业务接入时，均会分配业务 ID
            version (str) 版本号，如400
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
    VERSION = "400"
    api = RiskCheckDemo(SECRET_ID, SECRET_KEY, BUSINESS_ID, VERSION)

    params = {
        # 更多参数见官方文档
        # 风控SDK上报的数据后回参获取到的 token，应用方需要从智能风控客户端SDK获取该数据。详情请查看客户端接入文档。
        "token": "phyZwZcJ/jlBXEBQVUPEaCbAE65qqlfh",
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
        "target": "",
        "activityId": "",
        "phone": "",
        "email": "",
        "registerIp": "",
        "registerTime": ""
    }

    ret = api.check(params)

    code: int = ret["code"]
    msg: str = ret["msg"]
    if code == 200:
        print("msg=%s, data=%s" % (msg, ret["data"]))
    else:
        print("ERROR: code=%s, msg=%s" % (ret["code"], ret["msg"]))

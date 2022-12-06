import hashlib
import time
import urllib.request as urlrequest
import json


class RiskCheckDemo(object):
    """本接口的功能是智能风控明细数据查询，主要用于数据同步：从易盾拉取数据场景，以固定时间窗口拉取数据。"""

    API_URL = "http://ir-open.dun.163.com/v4/risk/detail"

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

    def get_signature_params(self, params):
        signature_params = {
            "secretId": params["secretId"],
            "businessId": params["businessId"],
            "nonce": params["nonce"],
            "timestamp": params["timestamp"],
            "version": params["version"]
        }
        return signature_params

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
        params["signature"] = self.gen_signature(self.get_signature_params(params))

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
    SECRET_ID = "your_secretk_id"
    SECRET_KEY = "your_secretk_key"
    BUSINESS_ID = "your_business_id"
    VERSION = "400"
    api = RiskCheckDemo(SECRET_ID, SECRET_KEY, BUSINESS_ID, VERSION)

    params = {
        "beginTimestamp": 1667959831798,
        "endTimestamp": 1667959915103,
        # 用于分页查询的关联标记。
        "startFlag": "",
        # 用户/玩家的账号
        "account": "zzzzzzz",
        # 用户/玩家的角色ID
        "roleId": "yyyyyyy",
        # 风险等级, 10-高风险, 20-中风险, 30-低风险
        "riskLevel": 10,
        # 包名
        "packageName": "com.aaa.bbb",
        # app版本
        "appVersion": "1.0.2",
        # ip地址
        "ip": "192.168.1.1"

    }

    ret = api.check(params)

    code: int = ret["code"]
    msg: str = ret["msg"]
    if code == 200:
        print("msg=%s, data=%s" % (msg, ret["data"]))
    else:
        print("ERROR: code=%s, msg=%s" % (ret["code"], ret["msg"]))

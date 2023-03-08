import hashlib
import time
import urllib.request as urlrequest
import json


class RiskMediaCheckDemo(object):
    """本接口通过AI算法对上报的图片进行分析，识别是否存在外挂行为。"""

    API_URL = "http://ir-open.dun.163.com/v5/risk/mediaCheck"

    def __init__(self, secret_id, secret_key, business_id, version):
        """
        Args:
            secret_id (str) 产品id，每个应用接入时，会分配secretId和私钥secretKey。
            secret_key (str) 产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
            business_id (str) 每个业务接入时，均会分配业务 ID
            version (str) 版本号，如500
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
    VERSION = "500"
    api = RiskMediaCheckDemo(SECRET_ID, SECRET_KEY, BUSINESS_ID, VERSION)

    params = {
        # 图片数据，图片支持编码为BASE64的数据，无需包含base64编码请求头部分
        "mediaData": "auMW9NLW5rNaa6vXVpq2jTfy1Kemr2UuWyvu9L7662dvL7Oik3cp5J5PJ/dr35/56UrrvP5ML+X/pJ//9k=",
        # 图片文件名，格式如xxx.jpg，需要包含.格式的文件后缀名
        "mediaName": "xxx.jpg",
        # 用户/玩家的角色ID
        "roleId": "yyyyyyy",
        # 用户/玩家的角色名称，非游戏类型应用，nickname 可以是当前用户昵称相同
        "nickname": "yyyyyyy",
        # 用户/玩家的角色的服务器名称
        "server": "com.aaa.bbb",
        # 用户/ 玩家的IP，或当前客户端业务事件发生时的公网IP地址（ipv4）
        "ip": "192.168.1.1"
    }

    ret = api.check(params)

    code: int = ret["code"]
    msg: str = ret["msg"]
    if code == 200:
        print("msg=%s, data=%s" % (msg, ret["data"]))
    else:
        print("ERROR: code=%s, msg=%s" % (ret["code"], ret["msg"]))

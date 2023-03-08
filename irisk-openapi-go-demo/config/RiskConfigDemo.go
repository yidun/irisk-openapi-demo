package main

import (
	"crypto/md5"
	"encoding/hex"
	"encoding/json"
	"fmt"
	simplejson "github.com/bitly/go-simplejson"
	"io/ioutil"
	"net/http"
	"sort"
	"strconv"
	"strings"
	"time"
)

/**
 * 当客户端出现风控SDK接口被屏蔽，获取不到业务配置信息时（如初始化功能配置、特征配置等），可通过此接口获取业务配置信息，并下发到客户端完成配置。
 */

const (
	apiUrl     = "http://ir-open.dun.163.com/v5/risk/getConfig" //接口url
	secretId   = "your_secret_id"                               //产品id，每个应用接入时，会分配secretId和私钥secretKey。
	secretKey  = "your_secret_key"                              //产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
	businessId = "your_business_id"                             //每个业务接入时，均会分配业务 ID，有对应的密钥 secretId。
	version    = "500"                                          //版本号，如500
)

// 请求易盾接口
func check(params map[string]interface{}) *simplejson.Json {
	marshal, _ := json.Marshal(params)
	jsonStr := string(marshal)
	println(jsonStr)
	resp, err := http.Post(apiUrl, "application/json", strings.NewReader(jsonStr))

	if err != nil {
		fmt.Println("调用API接口失败:", err)
		return nil
	}

	defer resp.Body.Close()

	contents, _ := ioutil.ReadAll(resp.Body)
	result, _ := simplejson.NewJson(contents)
	return result
}

// 生成签名信息
func genSignature(params map[string]interface{}) string {
	var paramStr string
	keys := make([]string, 0, len(params))
	for k := range params {
		keys = append(keys, k)
	}
	sort.Strings(keys)
	for _, key := range keys {
		paramStr += key + params[key].(string)
	}
	paramStr += secretKey
	md5Reader := md5.New()
	md5Reader.Write([]byte(paramStr))
	return hex.EncodeToString(md5Reader.Sum(nil))
}

func main() {
	params := map[string]interface{}{
		"secretId":   secretId,
		"businessId": businessId,
		"version":    version,
		"timestamp":  strconv.FormatInt(time.Now().UnixNano()/1000000, 10),
		"nonce":      "mmm888f73yyy59440583zzz9bfcc79de",
		// 从易盾SDK获取的拉取配置的参数，参考反外挂通用查询接口。数据格式见base64(Request)，Request为SDK直接调用服务端统一配置下发接口(/v4/c)请求参数。
		"sdkData": "your_sdk_data",
		// 用户/ 玩家的IP，或当前客户端业务事件发生时的公网IP地址（ipv4）
		"ip":      "1.1.1.1",
		"extData": "",
	}
	params["signature"] = genSignature(params)

	ret := check(params)

	code, _ := ret.Get("code").Int()
	message, _ := ret.Get("msg").String()
	data, _ := ret.Get("data").MarshalJSON()
	if code == 200 {
		fmt.Printf("请求成功：msg=%s, data=%s", message, data)
	} else {
		fmt.Printf("请求失败: code=%d, msg=%s", code, message)
	}
}

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
 * 本接口通过AI算法对上报的图片进行分析，识别是否存在外挂行为。
 */

const (
	apiUrl     = "http://ir-open.dun.163.com/v5/risk/mediaCheck" //接口url
	secretId   = "your_secret_id"                                //产品id，每个应用接入时，会分配secretId和私钥secretKey。
	secretKey  = "your_secret_key"                               //产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
	businessId = "your_business_id"              //每个业务接入时，均会分配业务 ID，有对应的密钥 secretId。
	version    = "500"                                           //版本号，如500
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
		// 图片数据，图片支持编码为BASE64的数据，无需包含base64编码请求头部分
		"mediaData": "auMW9NLW5rNaa6vXVpq2jTfy1Kemr2UuWyvu9L7662dvL7Oik3cp5J5PJ/dr35/56UrrvP5ML+X/pJ//9k=",
		// 图片文件名，格式如xxx.jpg，需要包含.格式的文件后缀名
		"mediaName": "xxx.jpg",
		// 用户/玩家的角色ID
		"roleId": "yyyyyyy",
		// 用户/玩家的角色名称，非游戏类型应用，nickname 可以是当前用户昵称相同
		"nickname": "yyyyy",
		// 用户/玩家的角色的服务器名称
		"server": "yyyyy",
		// ip地址
		"ip": "192.168.1.1",
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

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
 * 本接口的功能是智能风控明细数据查询，主要用于数据同步：从易盾拉取数据场景，以固定时间窗口拉取数据。
 * 例如，每次查询1分钟前的数据，时间跨度也是1分钟，则可以按1分钟时间窗口，周期性滑动拉取数据。
 */

const (
	apiUrl     = "http://ir-open.dun.163.com/v5/risk/detail" //接口url
	secretId   = "your_secret_id"                            //产品id，每个应用接入时，会分配secretId和私钥secretKey。
	secretKey  = "your_secret_key"                           //产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
	businessId = "your_business_id"                          //每个业务接入时，均会分配业务 ID，有对应的密钥 secretId。
	version    = "400"                                       //版本号，如400
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
		// 风控SDK上报的数据后回参获取到的 token，应用方需要从智能风控客户端SDK获取该数据。详情请查看客户端接入文档。
		"beginTimestamp": "1667959831798",
		"endTimestamp":   "1667959915103",
		// 用于分页查询的关联标记。
		"startFlag": "",
		// 用户/玩家的账号
		"account": "zzzzzzz",
		// 用户/玩家的角色ID
		"roleId": "yyyyyyy",
		// 风险等级, 1-低风险, 2-中风险, 3-高风险
		"riskLevel": "1",
		// 包名(仅限Android/iOS平台)
		"packageName": "com.aaa.bbb",
		// app版本(仅限Android/iOS平台)
		"appVersion": "1.0.2",
		"ip":         "192.168.1.1",
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

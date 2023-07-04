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
 * 本接口用于上报应用的举报信息。
 */

const (
	apiUrl     = "http://ir-open.dun.163.com/v5/risk/reportData" //接口url
	secretId   = "your_secret_id"                                //产品id，每个应用接入时，会分配secretId和私钥secretKey。
	secretKey  = "your_secret_key"                               //产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
	businessId = "your_business_id"                              //每个业务接入时，均会分配业务 ID，有对应的密钥 secretId。
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
	params := make(map[string]interface{})
	// 调用接口当前时间，单位毫秒
	params["secretId"] = secretId
	params["businessId"] = businessId
	params["nonce"] = "mmm888f73yyy59440583zzz9bfcc79de"
	params["timestamp"] = strconv.FormatInt(time.Now().UnixNano()/1000000, 10)
	params["version"] = version

	// 举报渠道
	params["reportChannel"] = "reportChannel"
	// 举报时间
	params["reportTime"] = strconv.FormatInt(time.Now().UnixNano()/1000000, 10)
	// 举报人信息
	whistleblowerMap := make(map[string]interface{})
	whistleblowerMap["account"] = "account01"
	whistleblowerMap["roleId"] = "roleId01"
	whistleblowerMap["roleName"] = "roleName01"
	whistleblowerMap["serverId"] = "serverId01"
	whistleblowerMap["level"] = "10"
	whistleblowerMap["recharge"] = "9999"
	whistleblower, _ := json.Marshal(whistleblowerMap)
	params["whistleblower"] = string(whistleblower)
	// 被举报人信息
	reportedPerson := make(map[string]interface{})
	reportedPerson["account"] = "account02"
	reportedPerson["roleId"] = "roleId02"
	reportedPerson["roleName"] = "roleName02"
	reportedPerson["serverId"] = "serverId02"
	reportedPerson["level"] = "10"
	reportedPerson["recharge"] = "9999"
	reportedPersonJSON, _ := json.Marshal(reportedPerson)
	params["reportedPerson"] = string(reportedPersonJSON)
	// 举报类型
	params["reportType"] = "reportType01"
	// 举报场景
	params["reportScene"] = "reportScene01"
	// 举报内容详情
	params["reportData"] = "data"
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

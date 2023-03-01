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
 * 本接口用于智能风控检测结果数据在线查询（唯有调用此接口后，才会对上报数据进行检测并获取命中结果。)
 * 游戏反外挂场景下，check 调用由 SDK 自动完成。
 */

const (
	apiUrl     = "http://ir-open.dun.163.com/v4/risk/check" //接口url
	secretId   = "your_secretk_id"         //产品id，每个应用接入时，会分配secretId和私钥secretKey。
	secretKey  = "your_secretk_key"         //产品密钥，每个应用接入时，会分配secretId和私钥secretKey。
	businessId = "your_business_id"         //每个业务接入时，均会分配业务 ID，有对应的密钥 secretId。
	version    = "400"                                      //版本号，如400
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
	var signatureParams map[string]string
	signatureParams = make(map[string]string)
	signatureParams["secretId"] = params["secretId"].(string)
	signatureParams["businessId"] = params["businessId"].(string)
	signatureParams["nonce"] = params["nonce"].(string)
	signatureParams["version"] = params["version"].(string)
	signatureParams["timestamp"] = params["timestamp"].(string)
	signatureParams["ip"] = params["ip"].(string)
	signatureParams["token"] = params["token"].(string)
	var paramStr string
	keys := make([]string, 0, len(signatureParams))
	for k := range signatureParams {
		keys = append(keys, k)
	}
	sort.Strings(keys)
	for _, key := range keys {
		paramStr += key + signatureParams[key]
	}
	paramStr += secretKey
	md5Reader := md5.New()
	md5Reader.Write([]byte(paramStr))
	return hex.EncodeToString(md5Reader.Sum(nil))
}

func main() {
	params := map[string]interface{}{
		"secretId":   secretId,
		"secretKey":  secretKey,
		"businessId": businessId,
		"version":    version,
		"timestamp":  strconv.FormatInt(time.Now().UnixNano()/1000000, 10),
		"nonce":      "mmm888f73yyy59440583zzz9bfcc79de",
		// 风控SDK上报的数据后回参获取到的 token，应用方需要从智能风控客户端SDK获取该数据。详情请查看客户端接入文档。
		"token": "your_token",
		// 用户/ 玩家的IP，或当前客户端业务事件发生时的公网IP地址（ipv4）
		"ip": "1.1.1.1",
		// 用户/玩家的角色 ID，非游戏类型应用，roleId 可以与 roleAccount 相同
		"roleId": "yyyyyyy",
		// 用户/玩家的角色名称，非游戏类型应用，roleName 可以是当前用户昵称相同
		"roleName": "yyyyyyy",
		// 用户/玩家的角色的服务器名称
		"roleServer": "yyyyyyy",
		// 用户/玩家的账号
		"account": "zzzzzzz",
		// 用户/玩家的等级
		"roleLevel": 150,
		// 游戏类型应用的版本号
		"gameVersion": "1.0.2",
		// 游戏类型应用的资源版本号
		"assetVersion": "assetVersion",
		// 额外/拓展的信息，应用 / 游戏方可以自己构建json结构，最大长度：2048。不同场景构建信息见分场景接入说明
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

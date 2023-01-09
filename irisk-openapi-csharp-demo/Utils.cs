using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Utilities.Encoders;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Com.Netease.Is.Irisk.Demo
{
    class Utils
    {
        // 根据secretKey和parameters生成签名
        public static String genSignature(String secretKey, Dictionary<String, String> parameters)
        {
            parameters = parameters.OrderBy(o => o.Key, StringComparer.Ordinal).ToDictionary(o => o.Key, p => p.Value);
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<String, String> kv in parameters)
            {
                builder.Append(kv.Key).Append(kv.Value);
            }
            builder.Append(secretKey);
            String tmp = builder.ToString();
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(tmp));
            builder.Clear();
            foreach (byte b in result)
            {
                builder.Append(b.ToString("x2").ToLower());
            }
            return builder.ToString();
        }

        public static HttpClient makeHttpClient()
        {
            HttpClient client = new HttpClient() {};
            client.DefaultRequestHeaders.Connection.Add("keep-alive");
            client.SendAsync(new HttpRequestMessage
            {
                Method = new HttpMethod("POST"),
                RequestUri = new Uri("https://ir-open.dun.163.com")
            }).Wait();
            return client;
        }

        // 执行post操作
        public static String doPost(HttpClient client, String url, Dictionary<String, String> parameters, int timeOutInMillisecond)
        {
            StringContent jsonContent = new(
                JsonSerializer.Serialize(parameters),
                Encoding.UTF8,
                "application/json");
            Task<HttpResponseMessage> task = client.PostAsync(url, jsonContent);
            if (task.Wait(timeOutInMillisecond))
            {
                HttpResponseMessage response = task.Result;
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    Task<string> result = response.Content.ReadAsStringAsync();
                    result.Wait();
                    return result.Result;
                }
            }
            return null;
        }


    }

}
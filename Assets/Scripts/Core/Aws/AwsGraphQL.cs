using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Aws
{
    // 與 Aws 進行 GraphQL 的呼叫功能
    public class AwsGraphQL
    {
        // 東京
        private const string API_URL = "https://fi5q2ovrefdalajuopti54tesq.appsync-api.ap-northeast-1.amazonaws.com/graphql";
        private const string API_KEY = "da2-qbu74o3vyjguxlqkgnuid7q4aq";

        public async UniTask<string> Post(string query, string token = "")
        {
            using var client = new HttpClient();

            HttpResponseMessage response;

            if (token != null)
            {
                client.DefaultRequestHeaders.Add("authorization", token);
                client.DefaultRequestHeaders.Add("x-api-key", "");
            }
            else
            {
                client.DefaultRequestHeaders.Add("x-api-key", API_KEY);
            }

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Debug.Log($"Start Post: \n {query}");

            response = await client.PostAsync(API_URL, new StringContent(query, Encoding.UTF8, "application/json"));

            try
            {
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.Log($"Post Fail: {e.Message}");
                throw new HttpRequestException(e.Message);
            }
        }
    }
}
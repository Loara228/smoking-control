using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace smoking_control.Api
{
    public class APIModule
    {
        public APIModule(APIClient client)
        {
            _client = client;
        }

        // example:
        // api.BuildQuery("auth", [("username", "test1"), ("password", "test2")]);
        // returns: http://192.168.0.148:8080/auth?username=test1&password=test2
        // 
        // also for example:
        // api.BuildQuery("users/get/1");
        // returns http://192.168.0.148:8080/users/get/1
        protected string BuildQuery(string route, (string pName, string pValue)[]? queries = default)
        {
            string result = $"{APIClient.PROTOCOL}://{APIClient.HOSTNAME}:{APIClient.PORT}/{route}?";

            if (queries is not null)
                foreach ((string k, string v) pair in queries)
                    result += $"{pair.k}={pair.v}&";

            return result.Substring(0, result.Length - 1);
        }

        protected async Task<(string content, HttpStatusCode code)> GetResponse(string request)
        {
            var response = await Client.HttpClient.GetAsync(request);
            return (await response.Content.ReadAsStringAsync(), response.StatusCode);
        }

        protected APIClient Client
        {
            get => _client;
        }

        private APIClient _client;
    }
}

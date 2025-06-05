using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace smoking_control
{
    public class API
    {
        public API()
        {
            this._client = new HttpClient();
        }

        public async Task<bool> Authorize(string username, string password)
        {
            var response = await _client.GetAsync(BuildQuery("auth", 
                [("username", username), ("password", password)]
            ));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ApiException(await response.Content.ReadAsStringAsync(), response.StatusCode);
                return false;
            }

            _token = await response.Content.ReadAsStringAsync();
            return true;
        }


        // example:
        // api.BuildQuery("auth", [("username", "test1"), ("password", "test2")]);
        // returns: http://192.168.0.148:8080/auth?username=test1&password=test2
        // 
        // also for example:
        // api.BuildQuery("users/get/1");
        // returns http://192.168.0.148:8080/users/get/1
        public string BuildQuery(string route, (string pName, string pValue)[]? queries = default)
        {
            string result = $"{PROTOCOL}://{Hostname}:{Port}/{route}?";
            if (queries is not null)
            {
                foreach ((string k, string v) pair in queries)
                {
                    result += $"{pair.k}={pair.v}&";
                }
            }

            return result.Substring(0, result.Length - 1);
        }

        public string Hostname
        {
            get => HOSTNAME;
        }

        public ushort Port
        {
            get => PORT;
        }

        public string Token
        {
            get => Token;
        }

        private HttpClient _client;
        private string? _token = null;

        // if http <application android:usesCleartextTraffic="true"></application>
        private const string PROTOCOL = "http";
        private const string HOSTNAME = "192.168.0.148";
        private const ushort PORT = 8080;
    }

    public class ApiException : Exception
    {
        public ApiException(string? message, HttpStatusCode code) : base(message)
        {
            this._code = code;
        }

        public override string ToString()
        {
            return $"code: {this._code} ({(int)this._code})\nmessage: {this.Message}\n\n" + base.ToString();
        }

        private HttpStatusCode _code;
    }
}

using smoking_control.Api.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace smoking_control.Api
{
    public class APIClient
    {
        public APIClient()
        {
            _httpClient = new HttpClient();

            AuthModule = new(this);

        }

        // Для обращения с любой части кода
        public static APIClient Current
        {
            get => _current;
        }

        public AuthenticationModule AuthModule { get; private set; }

        public string Token
        {
            get
            {
                if (_token is null)
                    throw new NullReferenceException("token is null");
                return _token;
            }
            internal set
            {
                _token = value;
            }

        }

        internal HttpClient HttpClient { get => _httpClient; }

        private HttpClient _httpClient;
        private string? _token = null;

        private static APIClient _current = new APIClient();

        // if http <application android:usesCleartextTraffic="true"></application>
        internal const string PROTOCOL = "http";
        internal const string HOSTNAME = "192.168.0.148";
        internal const ushort PORT = 8080;
    }

    public class ApiException : Exception
    {
        public ApiException(string? message, HttpStatusCode code) : base(message)
        {
            _code = code;
        }
        public ApiException((string? content, HttpStatusCode code) response) : base(response.content)
        {
            _code = response.code;
        }

        public override string ToString()
        {
            return $"code: {_code} ({(int)_code})\nmessage: {Message}\n\n" + base.ToString();
        }

        private HttpStatusCode _code;
    }
}

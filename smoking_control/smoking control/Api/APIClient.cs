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
            DataModule = new(this);
            LogsModule = new(this);
        }

        // Для обращения с любой части кода
        public static APIClient Current
        {
            get => _current;
        }

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

        public LogsModule LogsModule { get; private set; }
        public AuthenticationModule AuthModule { get; private set; }
        public DataModule DataModule { get; private set; }

        internal HttpClient HttpClient { get => _httpClient; }

        private HttpClient _httpClient;
        private string? _token = null;

        private static APIClient _current = new APIClient();

        // if http <application android:usesCleartextTraffic="true"></application>
        internal static readonly string PROTOCOL = CONSTANTS.PROTOCOL;
        internal static readonly string HOSTNAME = CONSTANTS.HOSTNAME;
        internal static readonly ushort PORT = CONSTANTS.PORT;
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

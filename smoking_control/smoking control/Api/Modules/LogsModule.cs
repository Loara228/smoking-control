using Newtonsoft.Json;
using smoking_control.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smoking_control.Api.Modules
{
    public class LogsModule : APIModule
    {
        public LogsModule(APIClient client) : base(client)
        {

        }

        /// <param name="utc_timestamp">0 - now</param>
        public async Task<long> AddLog(long utc_timestamp = 0)
        {
            var response = await GetResponse(BuildQuery("logs/add", [("token", Client.Token), ("timestamp", utc_timestamp.ToString())]));

            if (response.code != System.Net.HttpStatusCode.OK)
                throw new ApiException(response);

            return long.Parse(response.content);
        }

        public async Task GetLogs(int start, int count, List<UserLog>? in_list)
        {
            if (count > 50)
                throw new ArgumentException("GetLogs limit");

            var response = await GetResponse(BuildQuery("logs/get", [
                ("token", Client.Token),
                ("start", start.ToString()),
                ("count", count.ToString()),
            ]));

            if (response.code != System.Net.HttpStatusCode.OK)
                throw new ApiException(response);

            in_list = JsonConvert.DeserializeObject<List<UserLog>>(response.content);
        }
    }
}

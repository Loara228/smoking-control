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
        public async Task<UserLog> AddLog(long utc_timestamp = 0)
        {
            var response = await GetResponse(BuildQuery("logs/add", [("token", Client.Token), ("timestamp", utc_timestamp.ToString())]));

            if (response.code != System.Net.HttpStatusCode.OK)
                throw new ApiException(response);

            return JsonConvert.DeserializeObject<UserLog>(response.content)!;
        }

        public async Task DeleteLog(Int32 id)
        {
            var response = await GetResponse(BuildQuery("logs/delete", [("token", Client.Token), ("id", id.ToString())]));

            if (response.code != System.Net.HttpStatusCode.OK)
                throw new ApiException(response);
        }

        public async Task<List<UserLog>> GetLogs(int start, int count)
        {
            List<UserLog> result = new List<UserLog>();
            if (count > 50)
                throw new ArgumentException("GetLogs limit");

            var response = await GetResponse(BuildQuery("logs/get", [
                ("token", Client.Token),
                ("start", start.ToString()),
                ("count", count.ToString()),
            ]));

            if (response.code != System.Net.HttpStatusCode.OK)
                throw new ApiException(response);

            result = JsonConvert.DeserializeObject<List<UserLog>>(response.content)!;

            return result;
        }

        /// <param name="timeOffset">HOURS</param>
        /// <returns>today's logs</returns>
        public async Task<long> GetLogsToday(int timeOffset)
        {
            if (timeOffset < -12 || timeOffset > 12)
                throw new ArgumentOutOfRangeException(nameof(timeOffset));

            var response = await GetResponse(BuildQuery("logs/today", [("token", Client.Token), ("timezone", timeOffset.ToString())]));

            if (response.code != System.Net.HttpStatusCode.OK)
                throw new ApiException(response);

            return long.Parse(response.content);
        }
    }
}

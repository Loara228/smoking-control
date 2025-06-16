using Newtonsoft.Json;
using smoking_control.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace smoking_control.Api.Modules
{
    public class DataModule : APIModule
    {
        public DataModule(APIClient client) : base(client)
        {

        }

        public async Task<UserData?> GetData()
        {
            var response = await GetResponse(BuildQuery("users/data/get", [("token", Client.Token)]));
            if (response.code == HttpStatusCode.NotFound)
                return null;
            else if (response.code == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<UserData>(response.content);
            throw new ApiException(response);
        }

        public async Task SetData(UserData data)
        {
            var p = ParamsFrom(data);
            p.Insert(0, ("token", Client.Token));
            var response = await GetResponse(BuildQuery("users/data/set", p.ToArray()));

            if (response.code == HttpStatusCode.Unauthorized)
                throw new ApiException("token?", HttpStatusCode.Unauthorized);
            else if (response.code != HttpStatusCode.OK)
                throw new ApiException(response.content, response.code);
        }

        /// <returns>new interval = cur_interval + seconds</returns>
        public async Task<Int32> IncInterval(Int32 seconds)
        {
            var response = await GetResponse(BuildQuery("users/data/incinterval", [("token", Client.Token), ("interval", seconds.ToString())]));

            if (response.code == HttpStatusCode.OK)
                return Int32.Parse(response.content);
            throw new ApiException(response);
        }
    }
}

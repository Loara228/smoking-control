using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace smoking_control.Api.Modules
{
    public class AuthenticationModule : APIModule
    {
        public AuthenticationModule(APIClient client) : base(client)
        {

        }

        public async Task<bool> Auth(string username, string password)
        {
            var response = await GetResponse(BuildQuery("auth", [("username", username), ("password", password)]));

            if (response.code == HttpStatusCode.OK)
            {
                Client.Token = response.content;
                return true;
            }
            else if (response.code == HttpStatusCode.Unauthorized)
                return false;

            throw new ApiException(response);
        }

        public async Task<bool> VerifyToken(string token)
        {
            var response = await GetResponse(BuildQuery("verify", [("token", token)]));

            if (response.code == HttpStatusCode.OK)
                return true;
            else if (response.code == HttpStatusCode.Unauthorized)
                return false;
            throw new ApiException(response);
        }

        [Obsolete("todo: html registration")]
        public async Task<(bool result, string failMessage)> Register(string username, string password)
        {
            var response = await GetResponse(BuildQuery("users/create", [("username", username), ("password", password)]));

            if (response.code == HttpStatusCode.OK)
                return (true, string.Empty);
            else if (response.code == HttpStatusCode.Conflict) // If a user with that name already exists
                return (false, "Username is already taken");
            else if (response.code == HttpStatusCode.BadRequest)
                throw new ApiException("Invalid format exception", HttpStatusCode.BadRequest); // Invalid username or password format
            throw new ApiException(response);
        }
    }
}

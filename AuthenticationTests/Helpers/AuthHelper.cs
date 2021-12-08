using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationTests.Helpers
{
    public static class AuthHelper
    {
        public static async Task<TokenResponse> LoginAsync()
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5001");
            if (disco.IsError) throw new Exception(disco.Error);

            var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "roclient",
                ClientSecret = "secret",

                UserName = "bob",
                Password = "bob",

                Scope = "resource1.scope1 resource2.scope1",

                Parameters =
                {
                    { "acr_values", "tenant:custom_account_store1 foo bar quux" }
                }
            });

            if (response.IsError) throw new Exception(response.Error);
            return response;
        }

    }
}

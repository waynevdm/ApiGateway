using AccountModels;
using AuthenticationTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationTests.Integration
{
    [TestClass]
    public class GatewayApiTests
    {
        string apiBaseUrl = "http://localhost:5000";

        [TestMethod]
        public async Task ApiTest()
        {
            var tokenResp = await AuthHelper.LoginAsync();

            // Get no authorization
            var client = new RestClient(apiBaseUrl);
            var getAccountReq = new RestRequest("/account-service/account/35456");
            var accountResponse = client.Get<Account>(getAccountReq);
            Assert.IsNotNull(accountResponse);
            Assert.AreEqual(accountResponse.StatusCode, HttpStatusCode.OK);

            // Depost authorized
            var depositReq = new RestRequest("/account-service/Account/deposit");
            depositReq.RequestFormat = DataFormat.Json;

            var accountUpdate = new AccountUpdate
            {
                Number = "454576462",
                Amount = 50.00M
            };
            depositReq.Parameters.Clear();
            depositReq.AddParameter("application/json", SimpleJson.SerializeObject(accountUpdate), ParameterType.RequestBody);
            depositReq.AddHeader("Authorization", $"Bearer {tokenResp.AccessToken}");
            depositReq.AddHeader("content-type", "application/json");

            accountResponse = client.Post<Account>(depositReq);
            Assert.IsNotNull(accountResponse);
            Assert.AreEqual(accountResponse.StatusCode, HttpStatusCode.OK);

            // Withdraw authorized
            var withdrawReq = new RestRequest("/account-service/Account/withdraw");
            withdrawReq.RequestFormat = DataFormat.Json;

            accountUpdate = new AccountUpdate
            {
                Number = "454576462",
                Amount = 30.00M
            };
            withdrawReq.Parameters.Clear();
            withdrawReq.AddParameter("application/json", SimpleJson.SerializeObject(accountUpdate), ParameterType.RequestBody);
            withdrawReq.AddHeader("Authorization", $"Bearer {tokenResp.AccessToken}");
            withdrawReq.AddHeader("content-type", "application/json");

            accountResponse = client.Post<Account>(withdrawReq);
            Assert.IsNotNull(accountResponse);
            Assert.AreEqual(accountResponse.StatusCode, HttpStatusCode.OK);
        }

    }
}

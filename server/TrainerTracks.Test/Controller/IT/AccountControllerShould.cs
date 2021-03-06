﻿using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrainerTracks.Data.Model.DTO.Account;
using Xunit;

namespace TrainerTracks.Web.Test.Controller.IT
{
    // For a detailed setup, see https://fullstackmark.com/post/20/painless-integration-testing-with-aspnet-core-web-api
    public class AccountControllerShould : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient httpClient;

        public AccountControllerShould(CustomWebApplicationFactory<Startup> factory)
        {
            httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task ReturnSuccessWithClaimsAndCookieToken()
        {
            UserDTO user = new UserDTO
            {
                EmailAddress = "test@user.com",
                Password = "password1234"
            };
            string jsonInString = JsonConvert.SerializeObject(user);

            HttpResponseMessage response = await httpClient.PostAsync("/api/account/login", new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            string stringResponse = await response.Content.ReadAsStringAsync();
            UserClaimsDTO userClaims = JsonConvert.DeserializeObject<UserClaimsDTO>(stringResponse, new UserClaimsDtoJsonConverter());

            Assert.Equal(3, userClaims.Claims.Count);
            Assert.NotNull(userClaims.Token);
        }
    }
}

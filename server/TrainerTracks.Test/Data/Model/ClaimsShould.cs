using System.Collections.Generic;
using System.Security.Claims;
using TrainerTracks.Data.Enums;
using TrainerTracks.Data.Model.DTO.Account;
using TrainerTracks.Web.Data.Model.Entity;
using Xunit;

namespace TrainerTracks.Web.Test.Data.Model
{
    public class ClaimsShould
    {
        private const string JWT_KEY = "fc5a6707-634b-4776-ba70-6f6cc45fbcfc";

        /// <summary>
        /// GIVEN a Claims object contatining claims
        /// WHEN a UserClaimsDTO object is being generated
        /// THEN a UserClaimsDTO object with claims and a token is returned
        /// </summary>
        [Fact]
        public void ReturnUserClaimsDTOWithClaimsAndToken()
        {
            // GIVEN a Claims object contatining claims
            List<Claim> expectedClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, "test@user.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, UserRole.TRAINER.ToString())
            };
            Claims claims = new Claims(expectedClaims);

            // WHEN a UserClaimsDTO object is being generated
            UserClaimsDTO userClaims = claims.GenerateUserClaimsDTO(JWT_KEY);

            // THEN a UserClaimsDTO object with claims and a token is returned
            Assert.Equal(expectedClaims, userClaims.Claims);
            Assert.NotNull(userClaims.Token);
        }
    }
}

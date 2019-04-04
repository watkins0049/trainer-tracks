using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrainerTracks.Data.Model.DTO.Account;
using TrainerTracks.Data.Model.Entity;
using TrainerTracks.Data.Model;
using Microsoft.Extensions.Options;
using TrainerTracks.Web.Data.Context;

namespace TrainerTracks.Web.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly IOptions<TrainerTracksConfig> config;
        private readonly IAccountContext accountContext;

        public AccountServices(IAccountContext accountContext,
            IOptions<TrainerTracksConfig> config)
        {
            this.accountContext = accountContext;
            this.config = config;
        }

        public UserClaimsDTO SetupUserClaims(UserDTO user)
        {
            List<Claim> claims = GetTrainerClaims(user);
            UserClaimsDTO userClaims = new UserClaimsDTO
            {
                Claims = claims,
                Token = GenerateSecurityToken(claims)
            };
            return userClaims;
        }

        private List<Claim> GetTrainerClaims(UserDTO user)
        {
            TrainerCredentials trainerCredentials = accountContext.TrainerCredentials.Find(user.EmailAddress);

            if (trainerCredentials != null && trainerCredentials.IsTrainerAuthorized(user.Password))
            {
                return accountContext.Trainer.Find(user.EmailAddress).GenerateClaims();
            }
            throw new UnauthorizedAccessException("Username or password is incorrect.");
        }

        private string GenerateSecurityToken(List<Claim> claims)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "TrainerTracks");

            byte[] key = Encoding.ASCII.GetBytes(config.Value.JwtKey);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.CreateAndWriteToken(tokenDescriptor);
        }
    }

    internal static class JwtSecurityTokenHandlerExtensions
    {
        internal static string CreateAndWriteToken(this JwtSecurityTokenHandler handler, SecurityTokenDescriptor descriptor)
        {
            SecurityToken token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}

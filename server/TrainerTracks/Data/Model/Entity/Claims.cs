using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TrainerTracks.Data.Model.DTO.Account;

namespace TrainerTracks.Web.Data.Model.Entity
{
    public class Claims
    {
        private List<Claim> claims;

        public Claims(List<Claim> claims)
        {
            this.claims = claims;
        }

        public UserClaimsDTO GenerateUserClaimsDTO(string jwtKey)
        {
            return new UserClaimsDTO
            {
                Claims = claims,
                Token = GenerateSecurityToken(jwtKey)
            };
        }

        private string GenerateSecurityToken(string jwtKey)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "TrainerTracks");

            byte[] securityKey = Encoding.ASCII.GetBytes(jwtKey);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature)
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

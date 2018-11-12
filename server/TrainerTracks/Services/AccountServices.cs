using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrainerTracks.Data.Model.Entity;

namespace TrainerTracks.Services
{
    public class AccountServices
    {

        public static List<Claim> SetupClaims(Trainer user)
        {
            List<Claim> result = new List<Claim>();

            result.Add(new Claim(ClaimTypes.Email, user.EmailAddress));
            result.Add(new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName));
            result.Add(new Claim(ClaimTypes.Role, "Trainer"));
            result.Add(new Claim("TrainerId", user.TrainerId.ToString()));

            return result;
        }

        public static string GenerateSecurityToken(List<Claim> claims, string jwtEncoding)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "TrainerTracks");
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtEncoding);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

    }
}

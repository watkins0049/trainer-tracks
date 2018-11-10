using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrainerTracks.Data.Model.DTO;
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

            return result;
        }

        public static String GenerateSecurityToken(List<Claim> claims)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "TrainerTracks");
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("this is my custom Secret key for authnetication");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var test = tokenHandler.WriteToken(token);

            Thread.CurrentPrincipal = principal;

            return test;
        }

    }
}

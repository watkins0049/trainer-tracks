using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrainerTracks.Data.Dao;
using TrainerTracks.Data.Model.DTO.Account;
using TrainerTracks.Data.Model.Entity;

namespace TrainerTracks.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly UserDao userDao;

        public AccountServices(UserDao userDao)
        {
            this.userDao = userDao;
        }

        public UserClaimsDTO SetupUserClaims(UserDTO user)
        {
            UserClaimsDTO userClaims = new UserClaimsDTO
            {
                Claims = GetTrainerClaims(user),
                Token = ""
            };
            return userClaims;
        }

        private List<Claim> GetTrainerClaims(UserDTO user)
        {
            Trainer trainer = userDao.RetrieveUserInformation(user);
            return trainer.GenerateClaims();
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

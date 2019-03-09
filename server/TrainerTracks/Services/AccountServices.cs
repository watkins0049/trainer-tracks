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
            List<Claim> claims = GetTrainerClaims(user);
            UserClaimsDTO userClaims = new UserClaimsDTO
            {
                Claims = claims,
                Token = GenerateSecurityToken(claims, "fc5a6707-634b-4776-ba70-6f6cc45fbcfc")
            };
            return userClaims;
        }

        private List<Claim> GetTrainerClaims(UserDTO user)
        {
            Trainer trainer = userDao.RetrieveUserInformation(user);
            return trainer.GenerateClaims();
        }

        public string GenerateSecurityToken(List<Claim> claims, string jwtEncoding)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "TrainerTracks");
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(jwtEncoding);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}

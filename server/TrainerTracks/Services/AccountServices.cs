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
            if (userDao.IsUserAuthenticated(user))
            {
                Trainer trainer = userDao.RetrieveUserInformation(user);
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.emailAddress),
                    new Claim(ClaimTypes.Name, trainer.FirstName + " " + trainer.LastName),
                    new Claim(ClaimTypes.Role, "Trainer"),
                    new Claim("TrainerId", trainer.TrainerId.ToString())
                };

                UserClaimsDTO userClaims = new UserClaimsDTO()
                {
                    Claims = claims,
                    Token = ""
                };
                return userClaims;
            }

            return null;
        }

        //public static List<Claim> SetupClaims(Trainer user)
        //{
        //    List<Claim> result = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Email, user.EmailAddress),
        //        new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
        //        new Claim(ClaimTypes.Role, "Trainer"),
        //        new Claim("TrainerId", user.TrainerId.ToString())
        //    };

        //    return result;
        //}

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

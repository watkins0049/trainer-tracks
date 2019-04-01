﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrainerTracks.Data.Model.DTO.Account;
using TrainerTracks.Data.Model.Entity;
using TrainerTracks.Data.Repository;
using System.Security.Cryptography;

namespace TrainerTracks.Web.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly ITrainerRepository trainerRepository;
        private readonly ITrainerCredentialsRepository trainerCredentialsRepository;

        public AccountServices(ITrainerRepository trainerRepository, ITrainerCredentialsRepository trainerCredentialsRepository)
        {
            this.trainerRepository = trainerRepository;
            this.trainerCredentialsRepository = trainerCredentialsRepository;
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
            Trainer trainer = trainerRepository.GetTrainerByEmail(user.EmailAddress);
            TrainerCredentials trainerCredentials = trainerCredentialsRepository.GetById(trainer.TrainerId);

            string hashedPassword = HashStringSHA512(user.Password);
            if (BCrypt.Net.BCrypt.Verify(hashedPassword, trainerCredentials.Hash))
            {
                return trainer.GenerateClaims();
            }
            throw new UnauthorizedAccessException("Username or password is incorrect.");
        }

        // taken from https://stackoverflow.com/questions/11367727/how-can-i-sha512-a-string-in-c
        private string HashStringSHA512(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            using (SHA512 hash = SHA512.Create())
            {
                byte[] hashedInputBytes = hash.ComputeHash(bytes);
                return BitConverter.ToString(hashedInputBytes).Replace("-", "");
            }
        }

        private string GenerateSecurityToken(List<Claim> claims, string jwtEncoding)
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

            return tokenHandler.WriteToken(token);
        }
    }
}

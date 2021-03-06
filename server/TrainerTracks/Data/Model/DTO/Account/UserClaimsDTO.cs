﻿using System.Collections.Generic;
using System.Security.Claims;

namespace TrainerTracks.Data.Model.DTO.Account
{
    public class UserClaimsDTO
    {
        public List<Claim> Claims { get; set; }
        public string Token { get; set; }
    }
}

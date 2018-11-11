using System;
using System.Collections.Generic;
using System.Text;

namespace TrainerTracks.Data.Model.DTO.Account
{
    public class CredentialsDTO
    {
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}

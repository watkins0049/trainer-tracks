using System;
using System.ComponentModel.DataAnnotations;

namespace TrainerTracks.Data.Model.Entity
{
    public class TrainerCredentials
    {
        [Key]
        public string EmailAddress {get;set;}

        public string Hash { get; set; }
        public string Salt { get; set; }
    }
}

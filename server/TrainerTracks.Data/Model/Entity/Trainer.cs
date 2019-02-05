using System;
using System.ComponentModel.DataAnnotations;
using TrainerTracks.Data.Model.DTO.Account;

namespace TrainerTracks.Data.Model.Entity
{
    public class Trainer
    {
        public Trainer()
        {
        }

        [Key]
        public Int64 TrainerId { get; set; }

        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace TrainerTracks.Data.Model.Entity
{
    public class Trainer
    {
        [Key]
        public Int64 TrainerId { get; set; }

        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}

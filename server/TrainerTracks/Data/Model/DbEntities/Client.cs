using System;
using System.ComponentModel.DataAnnotations;

namespace TrainerTracks.Data.Model.Entity.DBEntities
{
    public class Client
    {
        [Key]
        public string EmailAddress { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Occupation { get; set; }
    }
}

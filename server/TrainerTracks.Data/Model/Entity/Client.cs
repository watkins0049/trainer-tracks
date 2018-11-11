using System;
using System.ComponentModel.DataAnnotations;

namespace TrainerTracks.Data.Model.Entity
{
    public class Client
    {
        [Key]
        public Int64 ClientId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Occupation { get; set; }
    }
}

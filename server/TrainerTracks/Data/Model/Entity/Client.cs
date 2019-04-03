using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainerTracks.Data.Model.Entity
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

        // Leave this commented out. This was causing an issue where trying to fetch the clients assigned to a particular trainer was
        // throwing an unexpected error in the client.
        //public List<TrainerClients> TrainerClients { get; set; }
    }
}

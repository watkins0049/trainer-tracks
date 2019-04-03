using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TrainerTracks.Data.Model.Entity
{
    public class TrainerClients
    {
        [Key]
        public string ClientEmailAddress { get; set; }
        public string TrainerEmailAddress { get; set; }

        [ForeignKey("EmailAddress")]
        public Client Client { get; set; }
        [ForeignKey("EmailAddress")]
        public Trainer Trainer { get; set; }

    }
}

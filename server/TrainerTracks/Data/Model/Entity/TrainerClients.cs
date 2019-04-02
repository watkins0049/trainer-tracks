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
        public Int64 ClientId { get; set; }
        public Int64 TrainerId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        [ForeignKey("TrainerId")]
        public Trainer Trainer { get; set; }

    }
}

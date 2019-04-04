using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainerTracks.Data.Model.Entity.DBEntities
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

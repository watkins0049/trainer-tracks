using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TrainerTracks.Data.Model
{
    public class Trainer
    {
        [Key]
        public string email_address { get; set; }
    }
}

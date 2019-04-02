using System;
namespace TrainerTracks.Data.Model
{
    public interface ITrainerTracksConfig
    {

        string JwtKey { get; set; }
        string BaseTrainerFormsDirectory { get; set; }
        string BaseClientFormsDirectory { get; set; }

    }
}

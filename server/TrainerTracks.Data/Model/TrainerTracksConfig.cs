
namespace TrainerTracks.Data.Model
{
    public class TrainerTracksConfig : ITrainerTracksConfig
    {
        public string JwtKey { get; set; }
        public string BaseTrainerFormsDirectory { get; set; }
        public string BaseClientFormsDirectory { get; set; }
    }
}
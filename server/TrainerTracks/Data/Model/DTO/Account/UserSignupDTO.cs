using System;
namespace TrainerTracks.Web.Data.Model.DTO.Account
{
    public class UserSignupDTO
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

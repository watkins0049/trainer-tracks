using System;
namespace TrainerTracks.Web.Exceptions
{
    public class UserSignupException : Exception
    {
        public UserSignupException(string message) : base(message) { }
    }
}

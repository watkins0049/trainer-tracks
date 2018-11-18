using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace TrainerTracks.Security
{
    public class TrainerAccount
    {
        public static ClaimsPrincipal GetClaimsPrincipal() { return (ClaimsPrincipal)Thread.CurrentPrincipal; }

        public static long GetTrainerId() {
            return Convert.ToInt64(GetClaimsPrincipal().Claims.Where(c => c.Type == "TrainerId"));
        }

    }
}

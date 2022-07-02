using System.Collections.Generic;

namespace MiniFW.WebHost
{
    public interface IIdentityHelper
    {
        string GetUserId();
        string GetUserName();
        string GetClaim(string type);
        List<string> GetClaims(string type);
    }
}

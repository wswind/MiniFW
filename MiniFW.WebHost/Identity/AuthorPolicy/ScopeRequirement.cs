// Author: Vincent Wang
// Email: ws_dev@163.com
// Date: 2021/12/1

using Microsoft.AspNetCore.Authorization;

namespace MiniFW.WebHost
{
    public class ScopeRequirement : IAuthorizationRequirement
    {
        public string ScopeName { get; set; }

        public ScopeRequirement(string scope)
        {
            ScopeName = scope;
        }
    }
}

// Author: Vincent Wang
// Email: ws_dev@163.com
// Date: 2021/12/1

using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace MiniFW.WebHost
{
    public class ScopeHandler : AuthorizationHandler<ScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "scope"))
                return Task.CompletedTask;
            var haveScope = context.User.HasClaim(c => c.Type == "scope" && c.Value.Equals(requirement.ScopeName));
            if (haveScope)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}

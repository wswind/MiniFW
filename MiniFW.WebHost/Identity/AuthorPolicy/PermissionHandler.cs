// Author: Vincent Wang
// Email: ws_dev@163.com
// Date: 2021/12/26

using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace MiniFW.WebHost
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "permission"))
            {
                return Task.CompletedTask;
            }
            bool hasPermission = context.User.HasClaim(c => c.Type == "permission" && c.Value.Equals(requirement.PermissionName));
            if (hasPermission)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}

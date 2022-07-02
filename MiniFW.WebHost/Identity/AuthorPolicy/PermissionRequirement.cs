// Author: Vincent Wang
// Email: ws_dev@163.com
// Date: 2021/12/26

using Microsoft.AspNetCore.Authorization;

namespace MiniFW.WebHost
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string PermissionName { get; set; }

        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }
    }
}

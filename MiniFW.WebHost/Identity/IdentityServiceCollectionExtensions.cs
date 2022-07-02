// Author: Vincent Wang
// Email: ws_dev@163.com
// Date: 2021/12/1

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace MiniFW.WebHost
{
    public static class IdentityServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, string identityUrl, string audience)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });
            return services;
        }

        public static void AddIdentityServerAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwtBearerAuthentication(configuration["IdentityServer:Url"], configuration["IdentityServer:Audience"]);
            services.AddIdentityHelper();
        }

        public static IServiceCollection AddIdentityHelper(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<IIdentityHelper, IdentityHelper>();
            return services;
        }

        public static IServiceCollection AddPermissionAndScopeAuthorization(this IServiceCollection services, IEnumerable<string> permissions)
        {
            if (permissions == null || permissions.Count() == 0)
                return services;

            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton<IAuthorizationHandler, ScopeHandler>();

            services.AddAuthorization(options =>
            {
                foreach (var permission in permissions)
                {
                    if (string.IsNullOrWhiteSpace(permission))
                        continue;
                    options.AddPolicy(permission, policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.Requirements.Add(new PermissionRequirement(permission));
                        policy.Requirements.Add(new ScopeRequirement(permission));
                    });
                }
            });
            return services;
        }
    }
}

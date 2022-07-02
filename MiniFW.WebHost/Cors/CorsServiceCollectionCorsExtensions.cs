﻿// Author: Vincent Wang
// Email: ws_dev@163.com
// Date: 2021/12/1

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MiniFW.WebHost
{
    public static class CorsServiceCollectionCorsExtensions
    {
        public static IServiceCollection AddAllowAnyCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(_defaultPolicyName, policy =>
                {
                    policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            return services;
        }
        private const string _defaultPolicyName = "AllowAny";

        public static IApplicationBuilder UseAllowAnyCors(this IApplicationBuilder app)
        {
            app.UseCors(_defaultPolicyName);
            return app;
        }
    }
}

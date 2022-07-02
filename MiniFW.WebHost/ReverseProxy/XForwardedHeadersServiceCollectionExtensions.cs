// Author: Vincent Wang
// Email: ws_dev@163.com
// Date: 2021/12/1
// https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer
// https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace MiniFW.WebHost
{
    public static class XForwardedHeadersServiceCollectionExtensions
    {
        public static IServiceCollection AddXForwardedHeaders(this IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
            return services;
        }
        public static IApplicationBuilder UseXForwardedHeaders(this IApplicationBuilder app)
        {
            app.UseForwardedHeaders();

            app.Use(async (ctx, next) =>
            {
                if (ctx.Request.Headers.TryGetValue("X-Forwarded-Prefix", out var prefixVals))
                {
                    if (!StringValues.IsNullOrEmpty(prefixVals))
                    {
                        string prefix = prefixVals.Last();
                        ctx.Request.PathBase = prefix;
                    }
                }

                await next();
            });
            return app;
        }
    }
}

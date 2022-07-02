// Author: Vincent Wang
// Email: ws_dev@163.com
// Date: 2021/12/29

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MiniFW.WebHost
{
    /// <summary>
    /// swagger extensions
    /// </summary>
    public static class SwaggerServiceCollectionExtensions
    {
        // from: https://github.com/domaindrivendev/Swashbuckle.AspNetCore#add-security-definitions-and-requirements-for-bearer-auth
        public static IServiceCollection AddSwaggerWithBearerAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var title = configuration["Swagger:Title"];
            if (string.IsNullOrEmpty(title))
                return services;
            var version = configuration["Swagger:Version"];
            var description = configuration["Swagger:Description"];
            var assemblyNames = configuration.GetSection("Swagger:AssemblyNames").Get<List<string>>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = title,
                    Version = version,
                    Description = description
                });

                c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                        },
                        new string[] {}
                    }
                });


                if (assemblyNames?.Count > 0)
                {
                    foreach (var assemblyName in assemblyNames)
                    {
                        if (string.IsNullOrEmpty(assemblyName))
                            continue;
                        var xmlFile = Path.Combine(AppContext.BaseDirectory, assemblyName + ".xml");
                        if (File.Exists(xmlFile))
                            c.IncludeXmlComments(xmlFile);
                    }
                }
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerWithBearerAuth(this IApplicationBuilder app, IConfiguration configuration)
        {
            var name = configuration["Swagger:Name"];
            var version = configuration["Swagger:Version"];

            app.UseSwagger(c =>
               c.PreSerializeFilters.Add((swagger, httpReq) =>
               {
                   string pathBase = httpReq.PathBase.Value;
                   if(!string.IsNullOrEmpty(pathBase))
                   {
                       if(!pathBase.Substring(1,1).Equals("/"))
                       {
                           pathBase = "/" + pathBase;
                       }
                   }
                   string url = $"{httpReq.Scheme}://{httpReq.Host.Value}{pathBase}";
                   swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = url } };
               }))
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint($"{version}/swagger.json", name);
               });
            return app;
        }
    }
}

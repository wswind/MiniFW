
## swagger

support nginx `X-Forwarded-Prefix`

nginx:

```bash
location /foo/bar/ {
            proxy_pass         http://192.168.190.10:8080/;
            proxy_http_version 1.1;
            proxy_set_header   Upgrade $http_upgrade;
            proxy_set_header   Connection keep-alive;
            proxy_set_header   Host $host;
            proxy_cache_bypass $http_upgrade;
            proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header   X-Forwarded-Proto $scheme;
            proxy_set_header   X-Forwarded-Prefix "foo/bar";
        } 
```

appsettings.json:


Bearer token:

```json
  "Swagger": {
    "Title": "xxapi",
    "Name": "xxapi",
    "Version": "v1",
    "Description": "xxapi",
    "AssemblyNames": [ "xxApi" ]
  },
  "IdentityServer": {
    "Url": "https://<IdentityServerUrl>",
    "Audience": "ApiResourceName"
  }
```

csproj gen xml file:
```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <NoWarn>$(NoWarn);1591</NoWarn>
</PropertyGroup>
```


use in asp.net core:
```csharp
services.AddSwaggerWithBearerAuth(config);
app.UseSwaggerWithBearerAuth(config);
```


## auth (IdentityServer)


appsettings.json:

```json
"IdentityServer": {
    "Url": "https://<IdentityServerUrl>",
    "Audience": "ApiResourceName"
}
```

IoC:

```csharp
services.AddIdentityServerAuth(Configuration);

services.AddAuthorization(options =>
{
    options.AddPolicy("PolicyName", policy =>
    policy.Requirements.Add(new ScopeRequirement("SomeScope")));
});
services.AddSingleton<IAuthorizationHandler, ScopeHandler>();

[Authorize(Policy = "PolicyName")]
```

identityserver template <https://github.com/wswind/IdentityServerAspNetIdentity>
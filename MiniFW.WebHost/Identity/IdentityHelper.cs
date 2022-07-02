// Author: Vincent Wang
// Email: ws_dev@163.com
// Date: 2021/12/1

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniFW.WebHost
{
    public class IdentityHelper : IIdentityHelper
    {
        private readonly IHttpContextAccessor _context;

        public IdentityHelper(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string GetUserId()
        {
            return _context.HttpContext.User.FindFirst("sub").Value;
        }

        public string GetUserName()
        {
            return _context.HttpContext.User.Identity.Name;
        }

        public string GetClaim(string type)
        {
            return _context.HttpContext.User.FindFirst(type)?.Value;
        }

        public List<string> GetClaims(string type)
        {
            return _context.HttpContext.User.FindAll(type).Select(x=>x.Value).ToList();
        }
    }
}

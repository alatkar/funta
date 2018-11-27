﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Funta.Core.Helper.Context
{
    public class Context : IContext
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public Context(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Uri Uri()
        {
            var request = GetHttpRequest();
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = request.Scheme;
            uriBuilder.Host = request.Host.Host;
            uriBuilder.Path = request.Path.ToString();
            uriBuilder.Query = request.QueryString.ToString();
            return uriBuilder.Uri;
        }

        public string GetHostDomain()
        {
            return $"{GetHttpRequest().Scheme}://{GetHttpRequest().Host}";
        }

        public HttpRequest GetHttpRequest()
        {
            return _httpContextAccessor.HttpContext.Request;
        }

        public Guid GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userId);
        }
    }
}

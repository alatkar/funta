using System;
using Microsoft.AspNetCore.Http;

namespace Funta.Core.Helper.Context
{
    public interface IContext
    {
        Uri Uri();
        string GetHostDomain();
        HttpRequest GetHttpRequest();
        Guid GetUserId();
    }
}

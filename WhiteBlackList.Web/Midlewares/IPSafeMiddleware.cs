using Microsoft.Extensions.Options;
using System.Net;

namespace WhiteBlackList.Web.Midlewares
{
    public class IPSafeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IPList _ipList;

        public IPSafeMiddleware(RequestDelegate next, IOptions<IPList> ıPList)
        {
            _next = next;
            _ipList = ıPList.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var reqIPAdress = context.Connection.RemoteIpAddress;
            var isWhiteList = _ipList.WhiteList.Where(x => IPAddress.Parse(x).Equals(reqIPAdress)).Any();

            if (!isWhiteList)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            await _next(context);
        }

    }
}

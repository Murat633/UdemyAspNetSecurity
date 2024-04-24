using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Net;
using WhiteBlackList.Web.Midlewares;

namespace WhiteBlackList.Web.Filters
{
    
    public class CheckWhiteList:ActionFilterAttribute
    {
        private readonly IPList _ipList;
        public CheckWhiteList(IOptions<IPList> iplist)
        {
            _ipList = iplist.Value;
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var reqIpAdress = context.HttpContext.Connection.RemoteIpAddress;
            var isWhiteList = _ipList.WhiteList.Where(x => IPAddress.Parse(x).Equals(reqIpAdress)).Any();
            if (!isWhiteList)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                return;
            }
            base.OnActionExecuted(context);
        }
    }
}

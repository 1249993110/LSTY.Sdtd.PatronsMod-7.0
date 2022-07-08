using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace LSTY.Sdtd.PatronsMod.SignalR
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ApiKeyAuthorizeAttribute : AuthorizeAttribute
    {
        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            var token = request.Headers.Get("access-token");
            if (token == AppSettings.AccessToken)
            {
                return true;
            }

            token = request.QueryString.Get("access-token");
            if (token == AppSettings.AccessToken)
            {
                return true;
            }

            return false;
        }
    }
}
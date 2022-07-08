using LSTY.Sdtd.PatronsMod;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using Owin;

namespace LSTY.Sdtd.PatronsMod.SignalR
{
    public class SignalRStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR(new HubConfiguration()
            {
                // 启用 JSONPJSONP 请求是不安全的，但一些较旧的浏览器（和一些 IE 版本）需要 JSONP 才能跨域工作
                EnableJSONP = false,
                EnableDetailedErrors = true,
                EnableJavaScriptProxies = false
            });

            GlobalHost.HubPipeline.AddModule(new GlobalExceptionModule());

            if (string.IsNullOrEmpty(AppSettings.AccessToken) == false)
            {
                var globalAuthorizer = new ApiKeyAuthorizeAttribute();
                // 该方法在处理第一个请求之前执行一次
                GlobalHost.HubPipeline.AddModule(new AuthorizeModule(globalAuthorizer, null));
            }

            GlobalHost.Configuration.MaxIncomingWebSocketMessageSize = null;
        }
    }
}
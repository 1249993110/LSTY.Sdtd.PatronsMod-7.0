using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace LSTY.Sdtd.PatronsMod
{
    /// <summary>
    /// 此类仅是声明，具体实现在<see cref="ModEventHook"/>
    /// </summary>
    [HubName(nameof(IModEventHub))]
    public class ModEventHub : Hub<IModEventHub>
    {
    }
}
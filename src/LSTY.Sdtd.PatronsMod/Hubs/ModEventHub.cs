using LSTY.Sdtd.Shared;
using LSTY.Sdtd.Shared.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using LSTY.Sdtd.PatronsMod;
using System.Threading.Tasks;

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

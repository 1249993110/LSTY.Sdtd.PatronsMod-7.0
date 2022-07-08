using Microsoft.AspNet.SignalR.Hubs;

namespace LSTY.Sdtd.PatronsMod.SignalR
{
    internal class GlobalExceptionModule : HubPipelineModule
    {
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            MethodDescriptor method = invokerContext.MethodDescriptor;
            CustomLogger.Error(exceptionContext.Error, $"Error in hub: {method.Hub.Name}.{method.Name}");
        }
    }
}
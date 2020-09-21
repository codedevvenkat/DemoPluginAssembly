using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginDemo
{
    public class PluginClass : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Write Your code.

            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.MessageName != "Update")
                return;
            if (context.Depth >1 )
             return;
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];
                if (entity.LogicalName != "account")
                    return;
                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    if (context.Stage == 40)
                    {
                      
                        Entity updateaccount = new Entity("account");
                        updateaccount.Id = entity.Id;
                        updateaccount.Attributes["description"] = "hELLO Create";
                       service.Update(updateaccount);
                        tracingService.Trace("Account name successfully changed");
                    }
                }
                catch (Exception exception)
                {
                    tracingService.Trace("Plugin: {0}", exception.ToString());
                    throw;
                }

            }
        }
    }
}

using System.ServiceModel;
using Microsoft.Xrm.Sdk;

namespace BasicPlugin
{
    public class FolloupPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Create an asynchronous plug-in registered on the Create message of the account table.
            // The plug-in will create a task activity that will remind the creator of the account to follow up one week later

            // Obtain the tracing service
            // ITracingService enables writing to the tracing log
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // IPluginExecutionContext provides access to the context for the event that executed the plugin
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request
            // Verify that the context InputParameters includes the expected parameters for the CreateRequest that this plug-in will be registered for
            // If the "Target" property is present, the "Entity" that was passed to the request will be available
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters
                Entity entity = (Entity)context.InputParameters["Target"];

                // Obtain the organisation service reference which you will need for web service calls]
                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    // Plug-in business logic goes in here
                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in FollowUpPlugin.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("FollowUpPlugin: {0}", ex.ToString());
                    throw;
;                }
            }


        }
    }
}
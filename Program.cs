using System;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using System.Collections.Generic;
using System.Linq;

namespace AzureRM
{
    class Program
    {
        static void Main(string[] args)
        {
            string subscriptionId = "";
            string clientId = "";
            string clientSecret = "";
            string tenantId = "";
            AzureEnvironment environment = AzureEnvironment.AzureGlobalCloud;

            Azure_instance instance = new Azure_instance(subscriptionId, clientId, clientSecret, tenantId, environment);
            var temp = instance.IsAzureInstanceValid();
            var ut = instance.GetFirstUserAndTimestamp(DateTime.Now, DateTime.Now, "rg");
            var a = ut.Caller;
        }
    }
}

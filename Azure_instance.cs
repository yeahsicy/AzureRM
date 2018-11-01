using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

//https://github.com/Azure/azure-libraries-for-net

namespace AzureRM
{
    class Azure_instance
    {
        IAzure azure;

        public Azure_instance(string subscriptionId, string clientId, string clientSecret, string tenantId, AzureEnvironment environment)
        {
            var c = SdkContext.AzureCredentialsFactory.FromServicePrincipal(clientId, clientSecret, tenantId, environment);
            azure = Azure.Authenticate(c).WithSubscription(subscriptionId);
        }

        public IEnumerable<string> GetResourceGroupNames()
        {
            var ResourceGroups = azure.ResourceGroups.List();
            return ResourceGroups.Select(r => r.Name);
        }

        /// <summary>
        /// Delete Resource Groups using the pattern
        /// </summary>
        /// <param name="pattern">For example, exact search "test"; start with "test*"; end with "*test"; contain "*test*"</param>
        public void DeleteResourceGroups(string pattern)
        {
            var names = GetResourceGroupNames();

            if (string.IsNullOrEmpty(pattern) || names.Count() == 0)
                return;
            if (pattern.First() == '*' && pattern.Last() == '*')
                names = names.Where(n => n.Contains(pattern.Substring(1, pattern.Length - 2)));
            else if (pattern.Last() == '*')
                names = names.Where(n => n.StartsWith(pattern.Substring(0, pattern.Length - 1)));
        }
    }
}

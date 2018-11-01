using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// Get Resource Group Names
        /// </summary>
        /// <param name="pattern">Use "*" to do the broad search</param>
        /// <returns>the group names</returns>
        public IEnumerable<string> GetResourceGroupNames(string pattern)
        {
            var names = GetResourceGroupNames();
            filter(ref names, pattern);
            return names;
        }

        void filter(ref IEnumerable<string> input, string pattern)
        {
            if (string.IsNullOrEmpty(pattern) || input.Count() == 0)
                return;

            if (pattern.First() == '*' && pattern.Last() == '*')
                input = input.Where(n => n.Contains(pattern.Substring(1, pattern.Length - 2)));
            else if (pattern.Last() == '*')
                input = input.Where(n => n.StartsWith(pattern.Substring(0, pattern.Length - 1)));
            else if (pattern.First() == '*')
                input = input.Where(n => n.EndsWith(pattern.Substring(1, pattern.Length - 1)));
            else
                input = input.Where(n => n == pattern);
        }

        /// <summary>
        /// Delete Resource Groups using the pattern
        /// </summary>
        /// <param name="pattern">Use "*" to do the broad search</param>
        /// <returns>The set of deleted Resource Groups</returns>
        public async Task<IEnumerable<string>> DeleteResourceGroups(string pattern)
        {
            var names = GetResourceGroupNames(pattern);

            List<Task> tasks = new List<Task>();
            foreach (var name in names)
                tasks.Add(azure.ResourceGroups.DeleteByNameAsync(name));
            await Task.WhenAll(tasks);
            return names;
        }
    }
}

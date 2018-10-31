using System;
using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace AzureRM
{
    class Program
    {
        static void Main(string[] args)
        {
            Azure_instance azure = new Azure_instance("", "", "", "", AzureEnvironment.AzureGlobalCloud);
        }
    }
}

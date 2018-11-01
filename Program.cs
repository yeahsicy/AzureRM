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
            Azure_instance instance = new Azure_instance("", "", "", "", AzureEnvironment.AzureGlobalCloud);

        }
    }
}

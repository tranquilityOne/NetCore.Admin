using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Core.Common
{
    public class ConfigurationManager
    {
        private static IConfigurationRoot configurationRoot;

        private ConfigurationManager() { }


        public static string GetJsonValue(string key)
        {
            if (configurationRoot == null)
            {
                var builder = new ConfigurationBuilder().AddJsonFile("app.json")
               .AddInMemoryCollection(new[]
               {
                           KeyValuePair.Create("the-key", "the-value"),
               });
               configurationRoot = builder.Build();
            }
            return configurationRoot[key];
        }
    }
}

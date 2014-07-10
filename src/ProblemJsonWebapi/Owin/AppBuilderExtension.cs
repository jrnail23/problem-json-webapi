using System;
using Microsoft.Owin.BuilderProperties;
using Owin;

namespace ProblemJsonWebapi.Owin
{
    static public class AppBuilderExtension
    {
        private const string HostAppModeKey = "host.AppMode";
        private const string DeveloperMode = "development";

        static public IAppBuilder UseProblemJsonExceptionFilter(this IAppBuilder appBuilder)
        {
            var applicationMode = new AppProperties(appBuilder.Properties).Get<string>(HostAppModeKey);

            var developerMode = string.Equals(DeveloperMode, applicationMode, StringComparison.Ordinal);

            return appBuilder.Use<ProblemJsonExceptionFilter>(developerMode);
        }

    }
}

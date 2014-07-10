using System.Net.Http.Formatting;

// ReSharper disable once CheckNamespace
namespace System.Web.Http
{
    public static class HttpConfigurationExtension
    {
        public static void UseProblemJsonErrorFormatter(this HttpConfiguration configuration)
        {
            configuration.Formatters.Add(new ProblemJsonMediaTypeFormatter());
        }
    }
}

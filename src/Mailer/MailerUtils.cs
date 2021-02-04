using RazorLight;
using Tayra.Mailer.MailerTemplateModels;

namespace Tayra.Mailer
{
    public class MailerUtils
    {

        public static string BuildTemplateForEmail<T>(string folderPath, T model,string fileName )
        {
            var engine = new RazorLightEngineBuilder().UseFileSystemProject(folderPath)
                .UseMemoryCachingProvider().Build();
            return engine.CompileRenderAsync(fileName, model).GetAwaiter().GetResult();
        }

        public static string BuildTemplateForSlack<T>(T model,string templateJson,string templateKey)
        {
            string template = $"[{templateJson}]";
            var engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(T))
                .UseMemoryCachingProvider()
                .Build();
            return engine.CompileRenderStringAsync(templateKey, template, model).GetAwaiter().GetResult();
        }
    }
}

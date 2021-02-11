using System.IO;
using PhantomJs.NetCore;
using RazorLight;
using Tayra.Mailer.MailerTemplateModels;

namespace Tayra.Mailer
{
    public class MailerUtils
    {

        public static string BuildTemplateForEmail<T>(T model,string fileName )
        {
            var folderPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                $@"Mailer{Path.DirectorySeparatorChar}TemplatesFiles{Path.DirectorySeparatorChar}","EmailTemplates");
            var engine = new RazorLightEngineBuilder().UseFileSystemProject(folderPath)
                .UseMemoryCachingProvider().Build();
            return engine.CompileRenderAsync(fileName, model).GetAwaiter().GetResult();
        }

        public static string BuildTemplateForSlack<T>(T model,string templateKey,string fileName)
        {
            var path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                $@"Mailer{Path.DirectorySeparatorChar}TemplatesFiles{Path.DirectorySeparatorChar}","SlackTemplates",fileName);
            var templateJson = File.ReadAllText(path);
            string template = $"[{templateJson}]";
            var engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(T))
                .UseMemoryCachingProvider()
                .Build();
            return engine.CompileRenderStringAsync(templateKey, template, model).GetAwaiter().GetResult();
        }

        public static string GeneratePdfFromHtml(string htmlToConvert)
        {
            var generator = new PdfGenerator();
            var pathOfGeneratedPdf = generator.GeneratePdf(htmlToConvert, Directory.GetCurrentDirectory());
            return pathOfGeneratedPdf;
        }
    }
}

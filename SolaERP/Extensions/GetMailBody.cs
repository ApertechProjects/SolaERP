using RazorLight;
using System.Net.Mail;

namespace SolaERP.API.Extensions
{
    public static class GetMailBody<T> where T : class
    {

        public async static Task<string> GetBody(T viewModel, string templateName, string imageName = null)
        {
            var fileRootPath = Path.GetFullPath(@"wwwroot/sources/templates");
            var imageRootPath = Path.GetFullPath(@"wwwroot/sources/images");

            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(fileRootPath)
                .EnableEncoding()
                .UseMemoryCachingProvider()
                .Build();

            string renderedHtml = await engine.CompileRenderAsync(templateName, viewModel);
            var processedBody = PreMailer.Net.PreMailer.MoveCssInline(renderedHtml, true).Html;

            if (!string.IsNullOrEmpty(imageName))
            {
                // Attach the image file
                var imageAttachment = new Attachment(Path.Combine(imageRootPath, imageName));
                imageAttachment.ContentId = "image1";

                // Include the image reference in the HTML body
                processedBody = processedBody.Replace("cid:image1", $"cid:{imageAttachment.ContentId}");
            }
            return processedBody;
        }
    }
}

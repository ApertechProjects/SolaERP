using System.Drawing;
using System.Net.Mail;
using System.Net.Mime;

namespace SolaERP.Application.Utils
{
    public static class ImageAppender
    {
        private static void AddImageToEmail(MailMessage mail, System.Drawing.Image image)
        {
            var imageStream = GetImageStream(image);

            var imageResource = new LinkedResource(imageStream, "image/png") { ContentId = "added-image-id" };
            var alternateView = AlternateView.CreateAlternateViewFromString(mail.Body, mail.BodyEncoding, MediaTypeNames.Text.Html);

            alternateView.LinkedResources.Add(imageResource);
            mail.AlternateViews.Add(alternateView);
        }

        private static Stream GetImageStream(System.Drawing.Image image)
        {
            var imageConverter = new ImageConverter();
            var imgaBytes = (byte[])imageConverter.ConvertTo(image, typeof(byte[]));
            var memoryStream = new MemoryStream(imgaBytes);

            return memoryStream;
        }
    }
}

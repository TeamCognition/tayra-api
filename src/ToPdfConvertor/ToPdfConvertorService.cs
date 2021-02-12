using System;
using DinkToPdf;

namespace ToPdfConvertor
{
    public class ToPdfConvertorService
    {
        public static byte[] ConvertHtmlToPdf(string htmlToConvert)
        {
            var convertor = new SynchronizedConverter(new PdfTools());
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Landscape,
                    PaperSize = PaperKind.A4Plus,
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = htmlToConvert
                    }
                }
            };
            return convertor.Convert(doc);
        }
    }
}
using DinkToPdf;

namespace ToPdfConvertor
{
    public static class ToPdfConvertorService
    {
        public static byte[] ConvertHtmlToPdf(string html)
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
                        HtmlContent = html
                    }
                }
            };
            return convertor.Convert(doc);
        }
    }
}
using System.Drawing;
using Tesseract;

namespace WPFUserInterface.Helpers
{
    // there is a world where it makes more sense to just have all of these be static methods
    internal class OcrUtils
    {

        // this should later be either a readonly collection or a struct of options?
        // TODO: support more languages 
        private const string Language = "eng";
        public Logger Logger { get; set; }

        public OcrUtils(Logger logger)
        {
            Logger = logger;
        }

        internal Bitmap DarkModeImageToLightMode(Bitmap image)
        {
            for (int y = 0; (y <= (image.Height - 1)); y++)
            {
                for (int x = 0; (x <= (image.Width - 1)); x++)
                {
                    // this method of color swapping is suuuuuper expensive
                    Color inv = image.GetPixel(x, y);
                    inv = Color.FromArgb(255, (255 - inv.R), (255 - inv.G), (255 - inv.B));
                    image.SetPixel(x, y, inv);
                }
            }

            return image;
        }


        internal string GetTextFromBitmapImage(Bitmap imgSource)
        {
            string ocrText = string.Empty;

            // does it make sense to retun nothing if the image is too small to ever even contain text?
            if (imgSource == null || imgSource.Height == 0 || imgSource.Width == 0)
            {
                return ocrText;
            }

            // im not sure how expensive this process is to run on a thread
            using (var tessEng = new TesseractEngine(@"./tessdata", Language, EngineMode.Default))
            {
                using (var img = PixConverter.ToPix(imgSource))
                {
                    using (var page = tessEng.Process(img))
                    {
                        ocrText = page.GetText();
                        Logger.Info($"Confidence: {page.GetMeanConfidence()}");
                    }
                }
            }

            return ocrText;
        }

        internal string GetTextFromBitmapImageWithFormatting(Bitmap imgSource)
        {
            string hOcrText = string.Empty;

            // does it make sense to retun nothing if the image is too small to ever even contain text?
            if (imgSource == null || imgSource.Height == 0 || imgSource.Width == 0)
            {
                return hOcrText;
            }

            // im not sure how expensive this process is to run on a thread
            using (var tessEng = new TesseractEngine(@"./tessdata", Language, EngineMode.Default))
            {
                using (var img = PixConverter.ToPix(imgSource))
                {
                    using (var page = tessEng.Process(img))
                    {
                        hOcrText = page.GetHOCRText(0).Replace('\n', ' ');
                    }
                }
            }

            var test = new HtmlAgilityPack.HtmlDocument();
            test.LoadHtml(hOcrText);

            int breakpont = 4;
            // this gets us into the main div section which has the individual lines and bbox data
            var x = test.DocumentNode.SelectSingleNode("//body").SelectSingleNode("//div");
            breakpont = 3;
            // the annoying part is that this is going to need to be generic because there will always be a different amount of data in here
            // has to be able to traverse downward and never just die somewhere
            // this entire OCR flow needs to go into a helper class
            var y = x;

            return hOcrText;
        }
    }
}

using PdfSharp.Pdf;
using System.Windows.Media.Imaging;

namespace WPFUserInterface.Models
{
    class FilePreview
    {
        public string FileName { get; set; }
        public BitmapImage PreviewIcon { get; set; }
        public PdfDocument PdfDocument { get; set; }
    }
}

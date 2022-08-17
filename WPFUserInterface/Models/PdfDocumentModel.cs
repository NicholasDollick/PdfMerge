using PdfSharp.Pdf;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace WPFUserInterface.Models
{
    public class PdfDocumentModel
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public BitmapImage PreviewIcon { get; set; }
        public PdfDocument PdfDocument { get; set; }

        public PdfDocumentModel(PdfDocument pdfDocument)
        {
            FilePath = pdfDocument.FullPath;
            FileName = Path.GetFileName(FilePath);
            PdfDocument = pdfDocument;
            PreviewIcon = new BitmapImage(new Uri("https://img.icons8.com/nolan/452/pdf.png"));
        }
    }
}

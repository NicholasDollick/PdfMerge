using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using WPFUserInterface.Helpers;

namespace WPFUserInterface.ViewModels
{
    public class PDFEditViewModel : BaseViewModel
    {
        public ICommand OpenFileButtonClick { get; set; }
        public ICommand OnFileDropCommand { get; set; }

        public PDFEditViewModel()
        {
            OpenFileButtonClick = new RelayCommand(ImportPDFs, param => true);
            OnFileDropCommand = new RelayCommand(Test, param => true);
        }

        private void Test(object obj)
        {
            throw new NotImplementedException();
        }

        void ImportPDFs(object param)
        {
            OpenFileDialog ofd = new OpenFileDialog {
                InitialDirectory = "",
                Filter = "Pdf Files|*.pdf",
                Title = "Select PDFs",
                RestoreDirectory = true,
                Multiselect = true
            };

            bool? res = ofd.ShowDialog();

            if (res == true)
            {
                List<PdfDocument> pdfs = new List<PdfDocument>();
                foreach (string filename in ofd.FileNames)
                {
                    using (var file = PdfReader.Open(filename, PdfDocumentOpenMode.Import)) {
                        pdfs.Add(file);
                    }
                }

                // assemble the pages of the pdfs into one file
                using (PdfDocument saveToDoc = new PdfDocument())
                {
                    foreach(PdfDocument doc in pdfs)
                    {
                        TransferPages(doc, saveToDoc);
                    }

                    saveToDoc.Save("combinedPdfs.pdf");
                }
            }
        }

        // these are bad var names
        void TransferPages(PdfDocument from, PdfDocument to)
        {
            foreach(PdfPage page in from.Pages)
            {
                to.AddPage(page);
            }
        }
    }
}

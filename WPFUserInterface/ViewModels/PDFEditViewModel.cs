using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFUserInterface.Helpers;

namespace WPFUserInterface.ViewModels
{
    public class PDFEditViewModel : BaseViewModel
    {
        public ICommand OpenFileButtonClick { get; set; }
        public ICommand OnFileDropCommand { get; set; }

        // this might need to be a custom list....
        public ObservableCollection<PdfDocument> Pdfs { get; set; }

        public PDFEditViewModel()
        {
            OpenFileButtonClick = new RelayCommand(ImportPDFs, param => true);
            OnFileDropCommand = new RelayCommand(Test, param => true);

            Pdfs = new ObservableCollection<PdfDocument>();
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
                Pdfs.Clear();
                foreach (string filename in ofd.FileNames)
                {
                    using (PdfDocument file = PdfReader.Open(filename, PdfDocumentOpenMode.Import)) {
                        Pdfs.Add(file);
                    }
                }

                // assemble the pages of the pdfs into one file
                using (PdfDocument saveToDoc = new PdfDocument())
                {
                    foreach(PdfDocument doc in Pdfs)
                    {
                        TransferPages(doc, saveToDoc);
                    }

                    // this should take a name field somewhere?
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

﻿using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using WPFUserInterface.Helpers;
using WPFUserInterface.Models;

namespace WPFUserInterface.ViewModels
{
    public class PDFEditViewModel : BaseViewModel
    {
        public ICommand OpenFileButtonClick { get; set; }
        public ICommand OnFileDropCommand { get; set; }

        public ObservableCollection<PdfDocumentModel> _pdfs;

        public PDFEditViewModel()
        {
            OpenFileButtonClick = new RelayCommand(ImportPDFs, param => true);
            OnFileDropCommand = new RelayCommand(Test, param => true);

            Pdfs = new ObservableCollection<PdfDocumentModel>();
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
                    // its possible for this section to...crash?
                    try{
                        using (PdfDocument file = PdfReader.Open(filename, PdfDocumentOpenMode.Import))
                        {
                            Pdfs.Add(new PdfDocumentModel(file));
                        }
                    }
                    catch (Exception e)
                    {
                        // this should much better error handling
                        System.Windows.MessageBox.Show(e.Message);
                    }
                }

                // assemble the pages of the pdfs into one file
                //using (PdfDocument saveToDoc = new PdfDocument())
                //{
                //    foreach(PdfDocumentModel doc in Pdfs)
                //    {
                //        TransferPages(doc.PdfDocument, saveToDoc);
                //    }

                //    // this should take a name field somewhere?
                //    saveToDoc.Save("combinedPdfs.pdf");
                //}
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


        public ObservableCollection<PdfDocumentModel> Pdfs
        {
            get => this._pdfs;
            set
            {
                SetField(ref this._pdfs, value, "Pdfs");
                if (value == null)
                    return;

                this._pdfs.CollectionChanged += ((object sender, NotifyCollectionChangedEventArgs args) =>
                {
                    OnPropertyChanged("Pdfs");
                });
            }
        }
    }
}

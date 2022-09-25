﻿using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Tesseract;
using WPFUserInterface.Helpers;
using WPFUserInterface.Models;

namespace WPFUserInterface.ViewModels
{
    public class PDFEditViewModel : BaseViewModel
    {
        public ICommand OpenFileButtonClick { get; set; }
        public ICommand MergeAndSaveCommand { get; set; }
        public ICommand ClearListCommand { get; set; }
        public ICommand OpenSettingsCommand { get; set; }

        public ObservableCollection<PdfDocumentModel> _pdfs;

        public string DisplayFilePreviewArea { get; set; } = "Hidden";
        public string DisplayImportButtonControls { get; set; } = "Visible";

        // this might be better served in the appviewmodel
        private PopupWindowFactory PopupWindowFactory { get; set; }

        public Logger Logger { get; set; }

        // this should later be either a readonly collection or a struct of options?
        // TODO: support more languages 
        private const string Language = "eng";

        private const int MaxVerticalPageSize = 750;

        public PDFEditViewModel(Logger logger)
        {
            OpenFileButtonClick = new RelayCommand(ImportPDFs, param => true);
            MergeAndSaveCommand = new RelayCommand(MergePDFs, param => true);
            ClearListCommand = new RelayCommand(ClearList, param => true);
            OpenSettingsCommand = new RelayCommand(OpenSettings, param => true);

            Logger = logger;
            PopupWindowFactory = new PopupWindowFactory();
            Pdfs = new ObservableCollection<PdfDocumentModel>();

            // DEBUG: test OCR on load
            Task.Run(() => { TestOcr(); });
        }

        private void TestOcr()
        {
            var darkModeOcr = GetTextFromBitmapImage(new Bitmap(@"C:\Users\Nullbytes\Pictures\darkmode.png"));

            var darkPic = new Bitmap(@"C:\Users\Nullbytes\Pictures\darkmode.png");
            // invert the colors of the darkmode image first
            for (int y = 0; (y <= (darkPic.Height - 1)); y++)
            {
                for (int x = 0; (x <= (darkPic.Width - 1)); x++)
                {
                    // this method of color swapping is suuuuuper expensive
                    Color inv = darkPic.GetPixel(x, y);
                    inv = Color.FromArgb(255, (255 - inv.R), (255 - inv.G), (255 - inv.B));
                    darkPic.SetPixel(x, y, inv);
                }
            }

            var darkModeOcrCorrected = GetTextFromBitmapImage(darkPic);


            var lightModeOcr = GetTextFromBitmapImage(new Bitmap(@"C:\Users\Nullbytes\Pictures\lightmode.png"));

            List<string> textToPrintToPDF = new List<string>();
            textToPrintToPDF.Add(darkModeOcr);
            textToPrintToPDF.Add(darkModeOcrCorrected);
            textToPrintToPDF.Add(lightModeOcr);


            // assemble the pages of the pdfs into one file
            using (PdfDocument saveToDoc = new PdfDocument())
            {
                saveToDoc.Info.Title = "OCR Testing";
                
                var page = saveToDoc.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                
                // Create a font
                XFont font = new XFont("Verdana", 12, XFontStyle.Regular);
                
                int yOffset = 0;
                // oh my god I forgot how much I hate pdfs
                foreach (var ocrText in textToPrintToPDF)
                {
                    foreach (var line in ocrText.Split('\n'))
                    {
                        // if currently off the page or if printing the current block would create ugly formatting, add a new page
                        if(yOffset > 750 || yOffset + 50 > 750)
                        {
                            // add a new page here
                            page = saveToDoc.AddPage();
                            gfx = XGraphics.FromPdfPage(page);
                            yOffset = 0;
                        }
                        gfx.DrawString(line, font, XBrushes.Black, new XRect(XUnit.FromCentimeter(1), yOffset, 10, 40), XStringFormats.CenterLeft);
                        yOffset += 15;
                    }
                    yOffset += 30;
                }

                saveToDoc.Save(Path.Combine(DefaultOutputPath, $"ocrTest.pdf"));
            }
        }

        // it may be helpful to move this into a helper class with a bunch of overloads to support text addition
        private void AddTextToPDF(string text, XFont font)
        {

        }

        private async void OpenSettings(object obj)
        {
            await PopupWindowFactory.CreateNewSettingsPopup();
        }

        private void ClearList(object obj)
        {
            Pdfs.Clear();

            // this entire paradigm is super hacky
            DisplayImportButtonControls = "Visible";
            OnPropertyChanged("DisplayImportButtonControls");
            DisplayFilePreviewArea = "Hidden";
            OnPropertyChanged("DisplayFilePreviewArea");
        }

        private void MergePDFs(object obj)
        {
            if (Pdfs.Count == 0)
                return;

            // assemble the pages of the pdfs into one file
            using (PdfDocument saveToDoc = new PdfDocument())
            {
                foreach (PdfDocumentModel doc in Pdfs)
                {
                    TransferPages(doc.PdfDocument, saveToDoc);
                }

                // this could be done inline but saving to this lets it be edited later?
                MergedFileName = Path.GetFileName(Pdfs[0].FileName);

                // this should take a custom name field somewhere?
                saveToDoc.Save(Path.Combine(DefaultOutputPath, $"{MergedFileName}-merged.pdf"));
                //Process.Start(saveToDoc.FullPath);
            }
        }

        private void ImportPDFs(object param)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = "",
                Filter = "Pdf Files|*.pdf",
                Title = "Select PDFs",
                RestoreDirectory = true,
                Multiselect = true
            };

            bool? res = ofd.ShowDialog();

            if (res == true)
            {
                AddPdfsToCollection(ofd.FileNames);
            }
        }

        internal void AddPdfsToCollection(string[] files)
        {
            // this block suuuuuuuuuuucks
            DisplayImportButtonControls = "Collapsed";
            OnPropertyChanged("DisplayImportButtonControls");
            DisplayFilePreviewArea = "Visible";
            OnPropertyChanged("DisplayFilePreviewArea");

            foreach (string filename in files)
            {
                try
                {
                    using (PdfDocument file = PdfReader.Open(filename, PdfDocumentOpenMode.Import))
                    {
                        Pdfs.Add(new PdfDocumentModel(file));
                    }
                }
                catch (Exception e)
                {
                    // this should much better error handling
                    Logger.Error($"Error parsing {Path.GetFileName(filename)}: {e.Message}");
                }
            }
        }

        internal void UpdatePDFListOrder(PdfDocumentModel droppedData, int targetIndex)
        {
            Pdfs.Remove(droppedData);
            Pdfs.Insert(targetIndex, droppedData);
        }

        // these are bad var names
        void TransferPages(PdfDocument from, PdfDocument to)
        {
            foreach (PdfPage page in from.Pages)
            {
                to.AddPage(page);
            }
        }

        private string GetTextFromBitmapImage(Bitmap imgSource) {
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
                    }
                }
            }
            
            return ocrText;
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

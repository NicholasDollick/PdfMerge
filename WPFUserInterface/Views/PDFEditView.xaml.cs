using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFUserInterface.Models;
using WPFUserInterface.ViewModels;

namespace WPFUserInterface.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class PDFEditView : UserControl
    {

        public PDFEditView()
        {
            InitializeComponent();
        }

        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                ((PDFEditViewModel)this.DataContext).AddPdfsToCollection(files);
            }
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            // this style of handing may not be the best.
            // my thoughts here are being able to wrap each section in 
            PdfDocumentModel droppedData = null;
            try
            {
                droppedData = (e.Data.GetData(typeof(StackPanel)) as StackPanel).DataContext as PdfDocumentModel;
            }
            catch (Exception ex)
            {
                // this should go to the logger
                Console.WriteLine();
            }

            var target = (sender as StackPanel).DataContext;
            int targetIndex = PdfListBox.Items.IndexOf(target);

            if (droppedData != null)
            {
                ((PDFEditViewModel)this.DataContext).UpdatePDFListOrder(droppedData, targetIndex);
            }
        }

        private void StackPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    DragDrop.DoDragDrop(sender as StackPanel, sender as StackPanel, DragDropEffects.Move);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message} || {ex.StackTrace}");
                }
            }
        }
    }
}

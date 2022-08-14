using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;
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

        public static readonly DependencyProperty FileDropCommandProperty = 
                    DependencyProperty.Register("FileDropCommandProperty", typeof(ICommand), typeof(PDFEditView), new PropertyMetadata(null));

        // TODO: this isnt MVVM and should be pulled out into a command
        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach(var file in files)
                {
                    MessageBox.Show(Path.GetFileName(file));
                }
            }
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            StackPanel droppedData = e.Data.GetData(typeof(StackPanel)) as StackPanel;

            // this correctly id's the item being dragged
            var target = (sender as StackPanel).DataContext;
            int targetIndex = PdfListBox.Items.IndexOf(target);

            ((PDFEditViewModel)this.DataContext).UpdatePDFListOrder((droppedData.DataContext) as PdfDocumentModel, targetIndex);
        }

        private void StackPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                // this wrapping is a little hacky 
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

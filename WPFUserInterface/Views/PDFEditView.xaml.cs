using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFUserInterface.Models;

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

        private void ListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem)
            {
                var draggedItem = sender as ListBoxItem;
                //var data = draggedItem.DataContext as PdfDocumentModel;

                draggedItem.IsSelected = true;
                //CreateDragDropWindow(card);
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
            }
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {

        }

        private void ListBox_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            // update the position of the visual feedback item
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);

            //this._dragdropWindow.Left = w32Mouse.X;
            //this._dragdropWindow = w32Mouse.Y;
        }


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };
    }
}

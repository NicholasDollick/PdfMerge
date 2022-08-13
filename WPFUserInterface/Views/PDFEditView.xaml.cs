using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            bool? res = openFile.ShowDialog();

            // TODO: this should filter to only allow for PDFS
            if(res == true)
            {
                string path = openFile.FileName;

                MessageBox.Show(path);
            }
        }
    }
}

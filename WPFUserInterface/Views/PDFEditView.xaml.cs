using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
    }
}

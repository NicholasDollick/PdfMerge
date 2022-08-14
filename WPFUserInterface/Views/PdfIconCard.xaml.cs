using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFUserInterface.Views
{
    /// <summary>
    /// Interaction logic for PdfIconCard.xaml
    /// </summary>
    public partial class PdfIconCard : UserControl
    {
        public string TestName { get; set; } = "Test File Name.pdf";
        public Uri TestUrl { get; set; } = new Uri("https://img.icons8.com/nolan/452/pdf.png");
        public PdfIconCard()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}

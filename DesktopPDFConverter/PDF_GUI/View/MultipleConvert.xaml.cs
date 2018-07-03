using System.Windows;
using PDF_GUI.ViewModel;

namespace PDF_GUI.View
{
    /// <summary>
    /// Interaction logic for MultipleConvert.xaml
    /// </summary>
    public partial class MultipleConvert : Window
    {
        public MultipleConvert()
        {
            DataContext = new MultipleConvertViewModel();
            InitializeComponent();
        }
    }
}

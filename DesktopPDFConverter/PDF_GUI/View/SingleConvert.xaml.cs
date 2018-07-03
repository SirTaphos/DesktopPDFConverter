using System.Windows;
using PDF_GUI.ViewModel;

namespace PDF_GUI.View
{
    /// <summary>
    /// Interaction logic for SingleConvert.xaml
    /// </summary>
    public partial class SingleConvert : Window
    {
        public SingleConvert()
        {
            DataContext = new SingleConvertViewModel();
            InitializeComponent();
        }
    }
}

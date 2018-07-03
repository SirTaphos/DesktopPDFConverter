using System.ComponentModel;
using System.Windows.Input;
using PDF_GUI.Helpers;

namespace PDF_GUI.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        readonly WindowHandler _windowHandler = new WindowHandler();
        private ICommand _goToSingleConvert;
        private ICommand _goToMultipleDownload;
        private ICommand _closeApplication;

        public ICommand GoToSingleConvert
        {
            get { return _goToSingleConvert ?? (_goToSingleConvert = new RelayCommand(param => _windowHandler.Navigate(1))); }
        }
        
        public ICommand GoToMultipleConvert
        {
            get { return _goToMultipleDownload ?? (_goToMultipleDownload = new RelayCommand(param => _windowHandler.Navigate(2))); }
        }

        public ICommand CloseApplication
        {
            get { return _closeApplication ?? (_closeApplication = new RelayCommand(param => _windowHandler.CloseApp())); }
        }        

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

    }
}

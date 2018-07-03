using System.ComponentModel;
using System.Windows.Input;
using PDF_GUI.Helpers;
using PDF_GUI.PDF_Engine;

namespace PDF_GUI.ViewModel
{
    class MultipleConvertViewModel : INotifyPropertyChanged
    {
        private readonly DialogHandler _dialogHandler = new DialogHandler();
        private readonly PdfHandler _pdfHandler = new PdfHandler();
        private readonly WindowHandler _windowHandler = new WindowHandler();

        private string _sourcePath;
        private string _targetPath;
        private ICommand _getPathFrom;
        private ICommand _getPathTo;
        private ICommand _convertToPdf;
        private ICommand _closeApplication;
        private ICommand _goToMainMenu;

        public string SourcePath
        {
            get => _sourcePath;
            set
            {
                _sourcePath = value;
                NotifyPropertyChanged("SourcePath");
            }
        }

        public string TargetPath
        {
            get => _targetPath;
            set
            {
                _targetPath = value;
                NotifyPropertyChanged("TargetPath");
            }
        }

        public ICommand GetPathFrom
        {
            get { return _getPathFrom ?? (_getPathFrom = new RelayCommand(param => RetrieveSourcePath())); }
        }


        public ICommand GetPathTo
        {
            get { return _getPathTo ?? (_getPathTo = new RelayCommand(param => RetrieveTargetPath())); }
        }


        public ICommand ConvertToPdf
        {
            get { return _convertToPdf ?? (_convertToPdf = new RelayCommand(param => _pdfHandler.ConvertFilesToPdf(SourcePath, TargetPath, ClearTextFields))); }
        }

        public ICommand CloseApplication
        {
            get { return _closeApplication ?? (_closeApplication = new RelayCommand(param => _windowHandler.CloseApp())); }
        }

        public ICommand GoToMainMenu
        {
            get { return _goToMainMenu ?? (_goToMainMenu = new RelayCommand(param => _windowHandler.Navigate(0))); }
        }

        private void RetrieveSourcePath()
        {
            SourcePath = _dialogHandler.GetFolder();
        }

        private void RetrieveTargetPath()
        {
            TargetPath = _dialogHandler.GetFolder();
        }

        private void ClearTextFields()
        {
            SourcePath = "";
            TargetPath = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}

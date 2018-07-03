using System.Linq;
using System.Windows;
using PDF_GUI.View;

namespace PDF_GUI.Helpers
{
    public class WindowHandler
    {
        public void Navigate(int navigator)
        {
            switch (navigator)
            {
                case 0:
                    ManageWindows(new MainWindow());
                    break;
                case 1:
                    ManageWindows(new SingleConvert());
                    break;
                case 2:
                    ManageWindows(new MultipleConvert());
                    break;
            }
        }

        private static void ManageWindows(Window window)
        {
            GetActiveWindow().Close();
            window.Show();
        }

        private static Window GetActiveWindow()
        {
            return Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
        }

        public void CloseApp()
        {
            var msgBox = MessageBox.Show("Are you sure you want to exit?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgBox == MessageBoxResult.Yes) Application.Current.Shutdown();
        }
    }
}

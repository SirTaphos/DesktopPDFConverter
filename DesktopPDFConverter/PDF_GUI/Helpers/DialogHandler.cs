using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace PDF_GUI.Helpers
{
    internal class DialogHandler
    {
        public string GetFile()
        {
            string filePath = null;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Vælg en fil",
                Filter = "All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true) filePath = openFileDialog.FileName;
            return filePath;
        }

        public string GetFolder()
        {
            string folderPath = null;
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) folderPath = folderBrowserDialog.SelectedPath;
            return folderPath;
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Windows;
using PDF_GUI.Helpers;

namespace PDF_GUI.PDF_Engine
{
    class PdfHandler
    {
        private readonly HelperMethods _helperMethods = new HelperMethods();
        private readonly PdfConvertHandler _pdfConvertHandler = new PdfConvertHandler();
        private readonly LogHandler _loghandler = new LogHandler();

        public void ConvertFilesToPdf(string sourcePath, string targetPath, Action action)
        {
            if (!string.IsNullOrEmpty(sourcePath) && !string.IsNullOrEmpty(targetPath))
            {
                try
                {
                    var listOfFiles = Directory.GetFiles(sourcePath);
                    foreach (var file in listOfFiles)
                    {
                        var ext = _helperMethods.GetExtension(file);
                        string pathTo = targetPath + "/" + ext.First() + ".pdf";
                        if(!File.Exists(pathTo)) _pdfConvertHandler.ReturnPdfFilePath(ext.Last(), file, pathTo);
                    }
                    action.Invoke();
                    _loghandler.WriteToLog(listOfFiles.Length);
                    MessageBox.Show("Filerne er blevet konverteret!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Programmet fejlede med fejlkode: " + Environment.NewLine + ex);
                }
            }
            else MessageBox.Show("Både kilde og destination skal være udfyldt!");
        }

        public void ConvertFileToPdf(string sourcePath, string targetPath, Action action)
        {
            if (!string.IsNullOrEmpty(sourcePath) && !string.IsNullOrEmpty(targetPath))
            {
                try
                {
                    var ext = _helperMethods.GetExtension(sourcePath);
                    string pathTo = targetPath + "/" + ext.First() + ".pdf";
                    _pdfConvertHandler.ReturnPdfFilePath(ext.Last(), sourcePath, pathTo);
                    action.Invoke();
                    _loghandler.WriteToLog(1);
                    MessageBox.Show("Filen er blevet konverteret!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Programmet fejlede med fejlkode: " + Environment.NewLine + ex);
                }
            }
            else MessageBox.Show("Både kilde og destination skal være udfyldt!");
        }
    }
}

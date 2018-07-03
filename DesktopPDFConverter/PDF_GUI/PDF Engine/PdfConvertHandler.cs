using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Interop.Word;
using PDF_GUI.Helpers;
using Application = Microsoft.Office.Interop.Word.Application;
using Document = Microsoft.Office.Interop.Word.Document;
using Exception = System.Exception;

namespace PDF_GUI.PDF_Engine
{
    class PdfConvertHandler
    {
        readonly HelperMethods _helperMethodMethods = new HelperMethods();

        public string ReturnPdfFilePath(string extension, string tempPath, string outputPath)
        {
            switch (extension.ToLower())
            {
                case "jpg":
                case "png":
                case "bmp":
                case "tiff":
                case "gif":
                case "jpeg":
                    if (ConvertImageToPdf(tempPath, outputPath))
                    {
                        return outputPath;
                    }
                    break;

                case "xlsx":
                case "xls":
                case "xlt":
                case "ods":
                case "xltx":
                    if (ConvertExcelToPdf(tempPath, outputPath))
                    {
                        return outputPath;
                    }
                    break;

                case "doc":
                case "docx":
                case "docm":
                case "rtf":
                case "dot":
                case "dotx":
                case "htm":
                case "odt":
                case "txt":
                    if (ConvertDocToPdf(tempPath, outputPath))
                    {
                        return outputPath;
                    }
                    break;

                case "xps":
                    if (ConvertXpsToPdf(tempPath, outputPath))
                    {
                        return outputPath;
                    }
                    break;

                case "xml":
                    if (ConvertXmlToPdf(tempPath, outputPath))
                    {
                        return outputPath;
                    }
                    break;

                case "html":
                    if (ConvertHtmlToPdf(tempPath, outputPath))
                    {
                        return outputPath;
                    }
                    break;

                case "pdf":
                    File.Copy(tempPath, outputPath);
                    return outputPath;

                case "msg":
                    if (ConvertMsgToPdf(tempPath, outputPath))
                        return outputPath;
                    break;
                case "note":
                    var text = File.ReadAllText(tempPath);
                    if (ConvertNoteToPdf(text, outputPath))
                        return outputPath;
                    break;
                default:
                    try
                    {
                        if (ConvertImageToPdf(tempPath, outputPath))
                        {
                            return outputPath;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    return outputPath;
            }
            return outputPath;
        }

        private bool ConvertImageToPdf(string tempPath, string output)
        {
            if (string.IsNullOrEmpty(tempPath) || string.IsNullOrEmpty(output))
            {
                return false;
            }
            try
            {
                using (FileStream fs = new FileStream(output, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (iTextSharp.text.Document doc = new iTextSharp.text.Document())
                    {
                        using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
                        {
                            doc.Open();
                            Image image = Image.GetInstance(tempPath);

                            image.SetAbsolutePosition(0, 0);
                            doc.SetPageSize(new iTextSharp.text.Rectangle(0, 0, image.Width, image.Height, 0));
                            doc.NewPage();

                            writer.DirectContent.AddImage(image, false);

                            doc.Close();

                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private bool ConvertExcelToPdf(string tempPath, string outputPath)
        {
            // If either required string is null or empty, stop and bail out
            if (string.IsNullOrEmpty(tempPath) || string.IsNullOrEmpty(outputPath))
            {
                return false;
            }

            // Create COM Objects

            // Create new instance of Excel
            var excelApplication = new Microsoft.Office.Interop.Excel.Application
            {
                ScreenUpdating = false,
                DisplayAlerts = false
            };

            // Make the process invisible to the user

            // Make the process silent

            // Open the workbook that you wish to export to PDF
            var excelWorkbook = excelApplication.Workbooks.Open(tempPath);

            // If the workbook failed to open, stop, clean up, and bail out
            if (excelWorkbook == null)
            {
                excelApplication.Quit();
                return false;
            }

            var exportSuccessful = true;
            try
            {
                // Call Excel's native export function (valid in Office 2007 and Office 2010, AFAIK)
                excelWorkbook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // Mark the export as failed for the return value...
                exportSuccessful = false;
            }
            finally
            {
                // Close the workbook, quit the Excel, and clean up regardless of the results...
                excelWorkbook.Close();
                excelApplication.Quit();
            }


            return exportSuccessful;
        }

        private bool ConvertDocToPdf(string tempPath, string outputPath)
        {
            // For opening ext: DOC and similar word extensions
            Application appWord = null;
            Document wordDocument = null;
            if (string.IsNullOrEmpty(tempPath) || string.IsNullOrEmpty(outputPath))
            {
                return false;
            }
            try
            {
                appWord = new Application();
                wordDocument = appWord.Documents.Open(tempPath);
                wordDocument.ExportAsFixedFormat(outputPath, WdExportFormat.wdExportFormatPDF);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            finally
            {
                if (wordDocument != null)
                    wordDocument.Close(false);
                if (appWord != null)
                {
                    appWord.NormalTemplate.Saved = true;
                    appWord.Quit(false);
                }
            }
        }

        private bool ConvertXpsToPdf(string tempPath, string outputPath)
        {
            // For opening ext: XPS
            try
            {
                using (PdfSharp.Xps.XpsModel.XpsDocument pdfXpsDoc = PdfSharp.Xps.XpsModel.XpsDocument.Open(tempPath))
                {
                    PdfSharp.Xps.XpsConverter.Convert(pdfXpsDoc, outputPath, 0);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private bool ConvertXmlToPdf(string tempPath, string outPutPath)
        {
            string text = null;
            try
            {
                foreach (var line in File.ReadAllLines(tempPath, Encoding.GetEncoding(874)))
                {
                    text = line;
                }
                string cleanXmlText = _helperMethodMethods.CleanInvalidXmlChars(text);
                File.WriteAllText(tempPath, cleanXmlText, Encoding.Default);
                Application app = new Application();
                Document doc2 = app.Documents.Open(tempPath);
                doc2.ExportAsFixedFormat(outPutPath, WdExportFormat.wdExportFormatPDF);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        
        private bool ConvertHtmlToPdf(string tempPath, string outPutPath)
        {
            try
            {
                var input = new WebClient().DownloadString(tempPath);
                var start = input.IndexOf("<body", StringComparison.Ordinal);
                var finish = input.IndexOf("</body", StringComparison.Ordinal);
                var totalLength = finish - start + 1;
                var htmlBody = input.Substring(start, totalLength);
                var cleanHtmlBody = _helperMethodMethods.StripHtmlTagsRegex(htmlBody);
                File.WriteAllText(outPutPath, cleanHtmlBody);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ConvertNoteToPdf(string xmlText, string outputPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlText);

            try
            {
                // read xml notes, write them to word document and write that to pdf
                XmlNode notes = xmlDoc.SelectSingleNode("descendant::node()");

                if (notes != null)
                {
                    string note = notes.OuterXml.Replace("&#xD;&#xA;", Environment.NewLine);
                    Application noteApp = new Application();
                    var noteDoc = noteApp.Documents.Add();
                    noteDoc.Application.Selection.Text = note;
                    noteDoc.ExportAsFixedFormat(outputPath, WdExportFormat.wdExportFormatPDF);
                    noteApp.NormalTemplate.Saved = true;
                    noteApp.Quit(WdSaveOptions.wdDoNotSaveChanges);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private bool ConvertMsgToPdf(string tempPath, string outPut)
        {
            try
            {
                Microsoft.Office.Interop.Outlook.Application app = new Microsoft.Office.Interop.Outlook.Application();
                var email = app.Application.Session.OpenSharedItem(tempPath) as MailItem;
                string noteXmlText = null;
                // EMAIL BODY TO PDF
                if (email == null) return true;
                email.BodyFormat = OlBodyFormat.olFormatRichText;
                string bodyText = email.Body;
                // Convert body text to pdf
                var application =
                    new Application();
                var doc = application.Documents.Add();
                if (!string.IsNullOrEmpty(bodyText))
                {
                    noteXmlText = _helperMethodMethods.CleanInvalidXmlChars(bodyText);
                }

                doc.Application.Selection.Text = noteXmlText;
                doc.ExportAsFixedFormat(outPut, WdExportFormat.wdExportFormatPDF);
                // Make sure that the document closes without saving itself or a template to normal.dotm
                doc.Close(false);
                application.NormalTemplate.Saved = true;
                application.Quit(false);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
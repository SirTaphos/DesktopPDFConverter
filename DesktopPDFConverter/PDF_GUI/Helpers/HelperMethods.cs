using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PDF_GUI.Helpers
{
    public class HelperMethods
    {
        public string CleanInvalidXmlChars(string text)
        {
            string re = @"[^\x09\x0A\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF]";
            return Regex.Replace(text, re, "");
        }

        public string CleanSpecialChars(string text)
        {
            var text1 = text.Replace("Ã¸", "ø");
            var text2 = text1.Replace("Ã¦", "æ");
            var text3 = text2.Replace("Ã¥", "å");
            var text4 = text3.Replace("Â", "");

            return text4;
        }

        public void Base64Decode(string base64EncodedData, string path)
        {
            var bytes = Convert.FromBase64String(base64EncodedData);
            File.WriteAllBytes(path, bytes);
        }

        public bool DetermineFileSource(string word)
        {
            var newword = word.Replace("\\", "");
            return newword.StartsWith("Mail fra") || newword.StartsWith("Re") ||
                   newword.StartsWith("SV") || newword.StartsWith("VS");
        }

        public string StripHtmlTagsRegex(string source)
        {
            var source2 = Regex.Replace(source, "<.*?>", string.Empty);
            return Regex.Replace(source2, @"<[^>]+> |&nbsp;", "").Trim();
        }

        public string[] GetExtension(string value)
        {
            char[] seperator = {'\\'};
            char[] seperator2 = { '.' };
            var ext = value.Split(seperator);
            var ext2 = ext.Last().Split(seperator2);
            return ext2;
        }
    }
}

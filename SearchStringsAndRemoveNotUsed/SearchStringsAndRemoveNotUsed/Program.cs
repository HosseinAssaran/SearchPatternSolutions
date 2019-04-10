using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchStringsAndRemoveNotUsed
{
    class Program
    {
        static void Main(string[] args)
        {
            string searchFolderPath = @"D:\frameworkPublic\Source\";
            DirectoryInfo dir = new DirectoryInfo(searchFolderPath);
            IEnumerable<FileInfo> fileList = dir.GetFiles("*.*", SearchOption.AllDirectories);  

            string patternLCD = @"\bLCD_[a-zA-Z0-9_]*\b";
            string patternPRN = @"\bPRN_[a-zA-Z0-9_]*\b";

            string refrencefilePath = @"D:\frameworkPublic\Include\FNV_stringResources.h";
            string text = File.ReadAllText(refrencefilePath, Encoding.GetEncoding(1256));


            SearchFilesAndRemoveStringsInTextIfNotUsed(text, patternLCD, fileList);
            SearchFilesAndRemoveStringsInTextIfNotUsed(text, patternPRN, fileList);

            File.WriteAllText(refrencefilePath, text, Encoding.GetEncoding(1256));
        }

        static void SearchFilesAndRemoveStringsInTextIfNotUsed(string text, string pattern, IEnumerable<FileInfo>  fileList)
        {
            MatchCollection matches = Regex.Matches(text, pattern);

            foreach (Match match in matches)
            {
                string searchTerm = @"\b" + match.Value + @"\b";

                var queryMatchingFiles =
                    from file in fileList
                    where file.Extension == ".c"
                    let fileText = GetFileText(file.FullName)
                    where Regex.IsMatch(fileText, searchTerm) //fileText.Contains(searchTerm)
                    select file.FullName;

                if (queryMatchingFiles.Count() == 0)
                {
                    Console.WriteLine("The term \"{0}\" was not found and so deleted.", searchTerm);
                    string removePattern = @"(#define[' ']*" + match.Value + @"\b)(.*\s)";
                    text = Regex.Replace(text, removePattern, "");
                }
            }
        }
        
        static string GetFileText(string name)
        {
            string fileContents = String.Empty;
            if (File.Exists(name))
            {
                fileContents = File.ReadAllText(name);
            }
            return fileContents;
        } 
    }
}

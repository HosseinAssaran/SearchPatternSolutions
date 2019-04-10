using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchAndReplaceInFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            string searchFolderPath = @"D:\H.Assaran_Projects\CastlesV5s\frameWorkFanava\public";
            DirectoryInfo dir = new DirectoryInfo(searchFolderPath);
            IEnumerable<FileInfo> fileList = dir.GetFiles("*.*", SearchOption.AllDirectories);
            string excelFilePath = @"C:\Users\h.asaran\Desktop\workOnStringResources\EngRes.xlsx";
            FileInfo excelFile = new FileInfo(excelFilePath);
            using (ExcelPackage xlPackage = new ExcelPackage(excelFile))
            {
                var excelWorksheet = xlPackage.Workbook.Worksheets[1];
                var firstColumn = excelWorksheet.Cells["A:A"];
                int j = 0;
                int i = 1;
                foreach (var col in firstColumn)
                {
                    string searchTerm = @"""" + col.Value.ToString() + @"""";
                    string replaceTerm = excelWorksheet.Cells[i, 2].Value.ToString();
                    i++;

                    var queryMatchingFiles =
                    from file in fileList
                    where file.Extension == ".c"
                    let fileText = GetFileText(file.FullName)
                    where Regex.IsMatch(fileText, searchTerm) 
                    select file.FullName;

                    Console.WriteLine(replaceTerm);
                    foreach (var file in queryMatchingFiles)
                    {
                        Console.WriteLine(file.ToString());
                        string text = File.ReadAllText(file.ToString());
                        text = text.Replace(searchTerm, replaceTerm);
                        File.WriteAllText(file.ToString(), text);
                    }
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

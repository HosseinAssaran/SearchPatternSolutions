using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchAndReplacePattern
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * This program get matches search pattern and matches replace pattern both from file1 
             * and replace them all matches search pattern in file2 with  matches replace pattern 
             */
            string searchForPattern = @"\bLCD_[a-zA-Z0-9_]*\b";
            string replacPattern = @""".*""";
            string pathFile1 = @"C:\Users\h.asaran\Desktop\textStrinUTF8.txt";
            string pathfile2 = @"C:\Users\h.asaran\Desktop\textStrinUTF8-2.txt";
            string textFile1 = File.ReadAllText(pathFile1);
            string textfile2 = File.ReadAllText(pathfile2);
            MatchCollection matchesInTextFile1 = Regex.Matches(textFile1, searchForPattern);
            MatchCollection matchesInTextFile2 = Regex.Matches(textfile2, searchForPattern);
            Console.WriteLine("textFile1 :{0} matchesInTextFile1", matchesInTextFile1.Count);
            Console.WriteLine("textfile2 :{0} matchesInTextFile2", matchesInTextFile2.Count);
            foreach (Match matchSearch in matchesInTextFile1)
            {
                Match matchesReplacePatternInTextFile1 = Regex.Match(textFile1.Substring(matchSearch.Index + matchSearch.Length), replacPattern);
                Console.WriteLine("Value is {0}", matchSearch.Value);
                textfile2 = Regex.Replace(textfile2, @"\b" + matchSearch.Value + @"\b", matchesReplacePatternInTextFile1.Value);
            }
            File.WriteAllText(pathfile2, textfile2);
        }
    }
}

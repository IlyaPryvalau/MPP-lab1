using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileGeneratorLib
{
    public class BigFileGenerator
    {
        private static int fileSize = 1000000000,
            minStringSize = 80,
            maxStringSize = 100,
            progressIndex = 0;

        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random rand = new Random();

        public static void Generate(string EncodingName)
        {
            Encoding enc = Encoding.GetEncoding(EncodingName);
            using (var writer = new StreamWriter("bigFile.txt", false, enc))
            {
                string generatedString;
                Console.WriteLine("Generating...");
                while (writer.BaseStream.Length <= fileSize)
                {
                    int temp = progressIndex;
                    progressIndex = (int)((double)writer.BaseStream.Length / fileSize * 100.0);
                    if (progressIndex != temp)
                        Console.Write("\r{0}%", progressIndex);
                    generatedString = GetRandomString(rand.Next(minStringSize, maxStringSize));
                    writer.WriteLine(generatedString);
                }
                Console.WriteLine("100%");
                Console.WriteLine("File generated.");
            }
        }

        private static string GetRandomString(int size) =>
            new string(Enumerable.Repeat(chars, size).Select(
                    s => s[rand.Next(s.Length)]).ToArray()
                );
    }
}

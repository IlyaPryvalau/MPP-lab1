using System;
using FileGeneratorLib;
using SortLib;

namespace lab1_MPP_console
{
    public class Program
    {
        public static void Main()
        {
            //Console.Write("Choose encoding: ");
            //string EncodingName = Console.ReadLine();
            //var generator = new FileGeneratorLib.BigFileGenerator();
            //BigFileGenerator.Generate(EncodingName);
            var s = new FileSorter("utf-8");
            s.MergeFiles();
        }
    }
}

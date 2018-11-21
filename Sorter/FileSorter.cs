using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SortLib
{
    public class FileSorter
    {
        public FileSorter(string encodingName)
        {
            enc = Encoding.GetEncoding(encodingName);
        }

        private Encoding enc;
        public Encoding FileEncoding { set { enc = value; } }
        private int chunkCount;

        public void Sort()
        {
            SplitFile();
            MergeFiles();
        }

        public void SplitFile()
        {
            var buffer = new List<string>();
            int stringBufferCapacity = 250000;
            chunkCount = 0;
            using (var sr = new StreamReader("bigFile.txt", enc))
            {
                while (sr.Peek() != -1)
                {
                    int lineCount = 0;
                    for (int i = 0; i < stringBufferCapacity && sr.Peek() != -1; i++)
                    {
                        buffer.Add(sr.ReadLine());
                        lineCount++;
                    }
                    buffer.Sort();
                    using (var sw = new StreamWriter("Chunks/" + "chunk".Insert(5, chunkCount.ToString())
                        + ".txt", false, enc))
                    {
                        string[] strArr = buffer.ToArray();
                        buffer.Clear();
                        for (int i = 0; i < lineCount; i++)
                            sw.WriteLine(strArr[i]);
                    }
                    chunkCount++;
                    Console.WriteLine("Chunk {0} is written.", chunkCount);
                }
            }
        }

        public void MergeFiles()
        {
            string[] paths = Directory.GetFiles("Chunks/", "chunk*.txt");
            chunkCount = paths.Length;
            int queueSize = 50000;

            var readers = new StreamReader[chunkCount];
            var queues = new Queue<string>[chunkCount];

            for (int i = 0; i < chunkCount; i++)
            {
                readers[i] = new StreamReader(paths[i], enc);
                queues[i] = new Queue<string>(queueSize);
                FillQueue(queues[i], readers[i], queueSize);
            }

            var sw = new StreamWriter("bigFileSorted.txt", false, enc);

            int minId, emptyQueuesCount;
            string min;
            bool mergeFinished = false;
            while (!mergeFinished)
            {
                minId = 0;
                emptyQueuesCount = 0;
                min = "";
                for (int i = 0; i < chunkCount; i++)
                {
                    if (queues[i] != null)
                    {
                        if (String.CompareOrdinal(queues[i].Peek(), min) < 0)
                        {
                            min = queues[i].Peek();
                            minId = i;
                        }
                    }
                    else
                        emptyQueuesCount++;
                }

                if (emptyQueuesCount == chunkCount)
                    mergeFinished = true;

                sw.WriteLine(min);
                queues[minId].Dequeue();
                if (queues[minId].Count == 0)
                {
                    FillQueue(queues[minId], readers[minId], queueSize);
                    if (queues[minId].Count == 0)
                        queues[minId] = null;
                }               
            }
            sw.Close();
            for (int i = 0; i < chunkCount; i++)
            {
                readers[i].Close();
                //File.Delete(paths[i]);
            }
        }

        static void FillQueue(Queue<string> queue, StreamReader r, int lines)
        {
            for (int i = 0; i < lines && r.Peek() != -1; i++)
                queue.Enqueue(r.ReadLine());
        }
    }
}

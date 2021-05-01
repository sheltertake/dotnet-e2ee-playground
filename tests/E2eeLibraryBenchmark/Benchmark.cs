using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using E2eeLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace E2eeLibraryBenchmark
{
    //[DryJob]
    [ShortRunJob]
    [MemoryDiagnoser]
    //[RankColumn, MarkdownExporterAttribute.StackOverflow]
    public class Benchmark
    {
        const string KEY = "fullstack";
        static readonly string[] words = new[]
        {
            "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
            "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
            "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"
        };

        // borrowed from greg (https://stackoverflow.com/questions/4286487/is-there-any-lorem-ipsum-generator-in-c)
        static IEnumerable<string> LoremIpsum(Random random, int minWords, int maxWords, int minSentences, int maxSentences, int numLines)
        {
            var line = new StringBuilder();
            for (int l = 0; l < numLines; l++)
            {
                line.Clear();
                var numSentences = random.Next(maxSentences - minSentences) + minSentences + 1;
                for (int s = 0; s < numSentences; s++)
                {
                    var numWords = random.Next(maxWords - minWords) + minWords + 1;
                    line.Append(words[random.Next(words.Length)]);
                    for (int w = 1; w < numWords; w++)
                    {
                        line.Append(" ");
                        line.Append(words[random.Next(words.Length)]);
                    }
                    line.Append(". ");
                }
                yield return line.ToString();
            }
        }

        string input;
        //private static readonly string s_data = string.Concat(LoremIpsum(new Random(), 6, 8, 2, 3, 1000));
        //public static ReadOnlySpan<char> input => s_data; // perfectly valid conversion

        [Params(0, 100, 1000)] // 1_000_000
        //[Params(1000)]
        public int N;

        [GlobalSetup]
        public void GlobalSetup()
        {
            if (N == 0)
                input = "simplestring";
            else
                input = string.Concat(LoremIpsum(new Random(), 6, 8, 2, 3, N));
        }

        [Benchmark(Baseline = true)]
        public bool EncryptDecryptV1Baseline()
        {
            return BaseLine.EncryptDecryptV1.Encrypt(input, KEY) == BaseLine.EncryptDecryptV1.Decrypt(input, KEY);
        }
        [Benchmark]
        public bool EncryptDecryptV2()
        {
            return BaseLine.EncryptDecryptV2.Encrypt(input, KEY) == BaseLine.EncryptDecryptV2.Decrypt(input, KEY);
        }

        [Benchmark]
        public bool EncryptDecryptV3()
        {
            return BaseLine.EncryptDecryptV3.Encrypt(input, KEY) == BaseLine.EncryptDecryptV3.Decrypt(input, KEY);
        }

        [Benchmark]
        public bool EncryptDecryptV4()
        {
            return BaseLine.EncryptDecryptV4.Encrypt(input, KEY) == BaseLine.EncryptDecryptV4.Decrypt(input, KEY);
        }

        [Benchmark]
        public bool EncryptDecrypt()
        {
            return input.Encrypt(KEY) == input.Decrypt(KEY);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmark>();
        }
    }
}

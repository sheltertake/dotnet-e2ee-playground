using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using E2eeLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace E2eeLibraryBenchmark
{
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

        [Params(1000, 1_000_000)]
        public int N;

        [GlobalSetup]
        public void GlobalSetup()
        {
            input = string.Concat(LoremIpsum(new Random(), 6, 8, 2, 3, 1_000_000));
        }

        [Benchmark(Baseline = true)]
        public bool EncryptDecryptBaseline()
        {
            return BaseLine.EncryptDecrypt.Encrypt(input, KEY) == BaseLine.EncryptDecrypt.Decrypt(input, KEY);
        }
        [Benchmark()]
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

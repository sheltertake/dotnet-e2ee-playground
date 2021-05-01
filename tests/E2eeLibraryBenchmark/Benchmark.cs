using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Text;
using E2eeLibraryBenchmark.Baseline;

namespace E2eeLibraryBenchmark
{
    //[DryJob]
    [SimpleJob]
    [MemoryDiagnoser]
    [AllStatisticsColumn, RankColumn, MarkdownExporterAttribute.StackOverflow]
    public class BenchmarkSimple : BenchmarkBase
    {
        [Params(0)]
        public int N;

        [GlobalSetup]
        public void GlobalSetup()
        {
            input = "simplestring";
            bytes = Encoding.ASCII.GetBytes(input);
        }
    }

    [SimpleJob]
    [MemoryDiagnoser]
    [AllStatisticsColumn, RankColumn, MarkdownExporterAttribute.StackOverflow]
    public class BenchmarksLineText : BenchmarkBase
    {
        [Params(1)]
        public int N;

        [GlobalSetup]
        public void GlobalSetup()
        {
            input = string.Concat(LoremIpsum(new Random(), 6, 8, 2, 3, N));
            bytes = Encoding.ASCII.GetBytes(input);
        }
    }

    [SimpleJob]
    [MemoryDiagnoser]
    [AllStatisticsColumn, RankColumn, MarkdownExporterAttribute.StackOverflow]
    public class Benchmarks100LineText : BenchmarkBase
    {
        [Params(100)]
        public int N;

        [GlobalSetup]
        public void GlobalSetup()
        {
            input = string.Concat(LoremIpsum(new Random(), 6, 8, 2, 3, N));
            bytes = Encoding.ASCII.GetBytes(input);
        }
    }

    [SimpleJob]
    [MemoryDiagnoser]
    [AllStatisticsColumn, RankColumn, MarkdownExporterAttribute.StackOverflow]
    public class Benchmarks1000LineText : BenchmarkBase
    {
        [Params(1000)]
        public int N;

        [GlobalSetup]
        public void GlobalSetup()
        {
            input = string.Concat(LoremIpsum(new Random(), 6, 8, 2, 3, N));
            bytes = Encoding.ASCII.GetBytes(input);
        }
    }

    [SimpleJob]
    [MemoryDiagnoser]
    [AllStatisticsColumn, RankColumn, MarkdownExporterAttribute.StackOverflow]
    public class Benchmarks1MillionLineText : BenchmarkBase
    {
        [Params(1_000_000)]
        public int N;

        [GlobalSetup]
        public void GlobalSetup()
        {
            input = string.Concat(LoremIpsum(new Random(), 6, 8, 2, 3, N));
            bytes = Encoding.ASCII.GetBytes(input);
        }
    }

    public abstract class BenchmarkBase
    {
        const string KEY = "fullstack";
        static readonly string[] words = new[]
        {
            "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
            "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
            "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"
        };

        // borrowed from greg (https://stackoverflow.com/questions/4286487/is-there-any-lorem-ipsum-generator-in-c)
        protected static IEnumerable<string> LoremIpsum(Random random, int minWords, int maxWords, int minSentences, int maxSentences, int numLines)
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


        protected string input;
        protected byte[] bytes;

        [Benchmark(Baseline = true)]
        public bool BEncryptDecryptV1BaselineString()
        {
            return EncryptDecryptV1.Encrypt(input, KEY) == EncryptDecryptV1.Decrypt(input, KEY);
        }
        [Benchmark]
        public bool BEncryptDecryptV2String()
        {
            return EncryptDecryptV2.Encrypt(input, KEY) == EncryptDecryptV2.Decrypt(input, KEY);
        }

        [Benchmark]
        public bool BEncryptDecryptV3String()
        {
            return EncryptDecryptV3.Encrypt(input, KEY) == EncryptDecryptV3.Decrypt(input, KEY);
        }

        [Benchmark]
        public bool BEncryptDecryptV4String()
        {
            return EncryptDecryptV4.Encrypt(input, KEY) == EncryptDecryptV4.Decrypt(input, KEY);
        }
        [Benchmark]
        public bool BEncryptDecryptV5StringUnsafe()
        {
            return EncryptDecryptV5.Encrypt(input, KEY) == EncryptDecryptV5.Decrypt(input, KEY);
        }
        [Benchmark]
        public bool BEncryptDecryptV6String()
        {
            return EncryptDecryptV6.Encrypt(input, KEY) == EncryptDecryptV6.Decrypt(input, KEY);
        }
        [Benchmark]
        public bool BEncryptDecryptV6()
        {
            return EncryptDecryptV6.Encrypt(bytes, KEY) == EncryptDecryptV6.Decrypt(bytes, KEY);
        }
        [Benchmark]
        public bool BEncryptDecryptV7String()
        {
            return EncryptDecryptV7.Encrypt(input, KEY) == EncryptDecryptV7.Decrypt(input, KEY);
        }
        [Benchmark]
        public bool BEncryptDecryptV7()
        {
            return EncryptDecryptV7.Encrypt(bytes, KEY) == EncryptDecryptV7.Decrypt(bytes, KEY);
        }
        [Benchmark]
        public bool BEncryptDecryptString()
        {
            return E2eeLibrary.EncryptDecrypt.Encrypt(input, KEY) == E2eeLibrary.EncryptDecrypt.Decrypt(input, KEY);
        }
        [Benchmark]
        public bool BEncryptDecryptBytes()
        {
            return E2eeLibrary.EncryptDecrypt.Encrypt(bytes, KEY) == E2eeLibrary.EncryptDecrypt.Decrypt(bytes, KEY);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {            
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}

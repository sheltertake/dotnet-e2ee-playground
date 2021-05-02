using E2eeLibraryBenchmark.Baseline;
using FluentAssertions;
using NUnit.Framework;

namespace E2eePlayegroundUnitTests
{
    public class EncryptDecryptReverseAndChunksEffectsTest
    {
        private const string KEY = "fullstack";

        //Some examples:
        //titanic-> 3(3 - ("
        //minecraft-> , (-$"1 %3
        //javascript-> ) 5 2"1(/3
        [TestCase("titanic", "3(3 -(\"")]
        [TestCase("minecraft", ",(-$\"1 %3")]
        [TestCase("javascript", ") 5 2\"1(/3")]
        public void EncryptDecryptTest(string input, string expected)
        {
            EncryptDecryptV1ChunksReverse.REVERSE_TWICE = false;
            EncryptDecryptV1ChunksReverse.USE_CHUNKS = false;

            var enc = EncryptDecryptV1ChunksReverse.Encrypt(input, KEY);
            enc.Should().Be(expected);

            EncryptDecryptV1ChunksReverse.REVERSE_TWICE = true;
            EncryptDecryptV1ChunksReverse.USE_CHUNKS = true;

            var enc2 = EncryptDecryptV1ChunksReverse.Encrypt(input, KEY);
            enc2.Should().Be(expected);

            enc.Should().Be(enc2);
        }
    }
}
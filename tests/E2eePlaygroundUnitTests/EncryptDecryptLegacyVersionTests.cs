using FluentAssertions;
using NUnit.Framework;
using E2eeLibraryBenchmark.Baseline;

namespace E2eePlayegroundUnitTests
{
    public class EncryptDecryptLegacyVersionTests
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
            //input.Encrypt(KEY).Should().Be(expected);
            //input.Encrypt(KEY).Decrypt(KEY).Should().Be(input);

            var enc = EncryptDecryptV1.Encrypt(input, KEY);
            enc.Should().Be(expected);
            EncryptDecryptV1.Decrypt(enc, KEY).Should().Be(input);

            enc = EncryptDecryptV2.Encrypt(input, KEY);
            enc.Should().Be(expected);
            EncryptDecryptV2.Decrypt(enc, KEY).Should().Be(input);

            enc = EncryptDecryptV3.Encrypt(input, KEY);
            enc.Should().Be(expected);
            EncryptDecryptV3.Decrypt(enc, KEY).Should().Be(input);

            enc = EncryptDecryptV4.Encrypt(input, KEY);
            enc.Should().Be(expected);
            EncryptDecryptV4.Decrypt(enc, KEY).Should().Be(input);

            enc = EncryptDecryptV5.Encrypt(input, KEY);
            enc.Should().Be(expected);
            EncryptDecryptV5.Decrypt(enc, KEY).Should().Be(input);

            enc = EncryptDecryptV6.Encrypt(input, KEY);
            enc.Should().Be(expected);
            EncryptDecryptV6.Decrypt(enc, KEY).Should().Be(input);

            enc = EncryptDecryptV7.Encrypt(input, KEY);
            enc.Should().Be(expected);
            EncryptDecryptV7.Decrypt(enc, KEY).Should().Be(input);

        }

        [TestCase("titanic")]
        [TestCase("minecraft")]
        [TestCase("javascript")]
        [TestCase("fullstackfullstack")]// 2 chunks
        [TestCase("fullstackfullstackfull")]// 3 chunks - 3rd chunk not full
        public void EncryptDecryptBytesTest(string input)
        {
            EncryptDecryptV1.Decrypt(EncryptDecryptV1.Encrypt(input, KEY), KEY)
                .Should().Be(input);
            EncryptDecryptV2.Decrypt(EncryptDecryptV2.Encrypt(input, KEY), KEY)
                .Should().Be(input);
            EncryptDecryptV3.Decrypt(EncryptDecryptV3.Encrypt(input, KEY), KEY)
                .Should().Be(input);
            EncryptDecryptV4.Decrypt(EncryptDecryptV4.Encrypt(input, KEY), KEY)
                .Should().Be(input);
            EncryptDecryptV5.Decrypt(EncryptDecryptV5.Encrypt(input, KEY), KEY)
                .Should().Be(input);
            EncryptDecryptV6.Decrypt(EncryptDecryptV6.Encrypt(input, KEY), KEY)
                .Should().Be(input);
            EncryptDecryptV7.Decrypt(EncryptDecryptV7.Encrypt(input, KEY), KEY)
                .Should().Be(input);
        }

    }
}
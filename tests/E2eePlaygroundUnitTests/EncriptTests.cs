using E2eeLibrary;
using FluentAssertions;
using NUnit.Framework;

namespace E2eePlayegroundUnitTests
{
    public class EncryptDecryptTests
    {
        private const string KEY = "fullstack";

        //Some examples:
        //titanic-> 3(3 - ("
        //minecraft-> , (-$"1 %3
        //javascript-> ) 5 2"1(/3
        [TestCase("titanic", "3(3 -(\"")]
        [TestCase("minecraft", ",(-$\"1 %3")]
        [TestCase("javascript", ") 5 2\"1(/3")]
        public void EncryptTest(string input, string expected)
        {

            // arrange
            // act
            var sut = input.Encrypt(KEY);

            // assert
            sut.Should().Be(expected);
        }

        //Some examples:
        //titanic-> 3(3 - ("
        //minecraft-> , (-$"1 %3
        //javascript-> ) 5 2"1(/3
        [TestCase("titanic", "3(3 -(\"")]
        [TestCase("minecraft", ",(-$\"1 %3")]
        [TestCase("javascript", ") 5 2\"1(/3")]
        public void DecryptTest(string expected, string input)
        {

            // arrange
            // act
            var sut = input.Decrypt(KEY);

            // assert
            sut.Should().Be(expected);
        }
        [TestCase("fullstackfullstack")]// 2 chunks
        [TestCase("fullstackfullstackfull")]// 3 chunks - 3rd chunk not full
        public void EncryptDecryptTest(string input)
        {
            // arrange
            // act
            var sut = input.Encrypt(KEY).Decrypt(KEY);

            // assert
            input.Should().Be(sut);
        }

    }
}
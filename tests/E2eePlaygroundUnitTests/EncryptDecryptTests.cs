using System;
using System.IO;
using System.Text;
using E2eeLibrary;
using E2eePlayegroundUnitTests.Helpers;
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

        
        [Test]
        public void EncryptDecryptStackTest()
        {
            // arrange
            var input = string.Concat(StringHelper.LoremIpsum(new Random(), 6, 8, 2, 3, 5));
            var decrypted = FindThrow(input);
            // assert
            input.Should().Be(decrypted);
        }

        [Test]
        public void EncryptDecryptCharAllowedTest()
        {
            // arrange
            for (int i = 32; i <= 125; i++)
            {
                var chr = ((char) i).ToString();
                var decrypted = FindThrow(chr);

                // assert
                chr.Should().Be(decrypted);
            }
           
        }

        [Test]
        public void CountBytesTest()
        {
            var my = string.Concat(StringHelper.LoremIpsum(new Random(), 6, 8, 2, 3, 1000));
            var byteCount = Encoding.ASCII.GetByteCount(my);
            Console.WriteLine(byteCount); // 152_761
            byteCount.Should().BeGreaterThan(0);
        }
        //[Test]
        //public void HugeStringTest()
        //{
            //var input = string.Concat(StringHelper.LoremIpsum(new Random(), 6, 8, 2, 3, 1_000_000));
            
            //var byteCount = Encoding.ASCII.GetByteCount(input);
            //Console.WriteLine(byteCount); // 152_631_783

            //File.WriteAllText("test.txt", my);

            //byteCount.Should().BeGreaterThan(0);
            //var sut = input.Encrypt(KEY).Decrypt(KEY);

            // assert
            //input.Should().Be(sut); // 23 seconds
        //}

        private static string FindThrow(string input)
        {
            foreach (var c in input)
            {
                var pos = (byte) c;
                if (pos < 32 || pos > 125)
                {
                    Console.WriteLine("input {0} {1}", c, pos);
                    throw new Exception("not ASCII allowed");
                }
            }

            // act
            var encrypted = input.Encrypt(KEY);
            foreach (var c in encrypted)
            {
                var pos = (byte) c;
                if (pos < 32 || pos > 125)
                {
                    Console.WriteLine("encrypted {0} {1} input {2} char {3}", c, pos, input, (byte)input[0]);
                    throw new Exception("not ASCII allowed");
                }
            }

            var decrypted = encrypted.Decrypt(KEY);
            foreach (var c in decrypted)
            {
                var pos = (byte) c;
                if (pos < 32 || pos > 125)
                {
                    Console.WriteLine("decrypted {0} {1}", c, pos);
                    throw new Exception("not ASCII allowed");
                }
            }

            return decrypted;
        }
    }
}
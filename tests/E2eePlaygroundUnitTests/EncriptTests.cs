using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E2eePlayegroundUnitTests
{
    public class EncryptDecryptTests
    {

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
            var sut = Encrypt("fullstack", input);

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
            var sut = Decrypt("fullstack", input);

            // assert
            sut.Should().Be(expected);
        }
        [TestCase("fullstackfullstack")]// 2 chunks
        [TestCase("fullstackfullstackfull")]// 3 chunks - 3rd chunk not full
        public void EncryptDecryptTest(string input)
        {
            // arrange
            var key = "fullstack";
            // act
            var sut = Decrypt(key, Encrypt(key, input));

            // assert
            input.Should().Be(sut);
        }

        /*
         * 
            The algorithm used to encrypt the text in the design was a block cipher with the following rules: The
            alphabet is restricted between ASCII codes 32 (" ") and 125 ("}") included. If you are not familiar with ASCII
            give a look at http://www.asciitable.com/ and https://en.wikipedia.org/wiki/ASCII The algorithm's encryption
            and decryption processes expect a key in the form of a string and a message (also a string). In pseudocode:
         
            Here are the encryption steps:
            . The message is split in chunks of length key.size

            Each chunk is reversed (eg. asdfg --> gfdsa)
            . For each chunk:
            . Each character is shifted upwards by n positions where n is the sum of the ASCII decimal codes of the encryption key
            . If the selected code exceeds the alphabet's length it must be reassigned from the alphabet start (e.g using the modulo operator)
            . After the operation, each chunk has to be reversed back again

            The given encrypted message was generated following the above steps using fullstack as key. Now it is
            your time to write the decryption algorithm to reverse the process!
         */
        private static string Encrypt(string key, string message)
        {
            // TRAP 2 - IGNORED - The message is split in chunks of length key.size
            // TRAP 1 - IGNORED - Each chunk is reversed (eg. asdfg --> gfdsa)

            //  n is the sum of the ASCII decimal codes of the encryption key
            var sumKeyToAsciBytes = key.Select(x => (int)x).Sum();
            var numCharAllowed = 125 - 32 + 1;
            var restToSum = sumKeyToAsciBytes % numCharAllowed;
            var retChunks = new List<string>();

            var shiftedChars = new List<char>();
            foreach (var c in message)
            {
                var asciiCharPos = (int)c;
                var newAsciiCharPos = asciiCharPos + restToSum;

                if (newAsciiCharPos > 125)
                    newAsciiCharPos = 31 + (newAsciiCharPos - 125);

                asciiCharPos = (char)newAsciiCharPos;

                shiftedChars.Add((char)asciiCharPos);
            }
            // TRAP 1 - IGNORED - After the operation, each chunk has to be reversed back again
            retChunks.Add(string.Concat(shiftedChars));

            return string.Concat(retChunks);
        }

        public static IEnumerable<string> SplitBy(string str, int chunkLength)
        {
            for (int i = 0; i < str.Length; i += chunkLength)
            {
                if (chunkLength + i > str.Length)
                    chunkLength = str.Length - i;

                yield return str.Substring(i, chunkLength);
            }
        }
        private static string Decrypt(string key, string message)
        {
            // TRAP 2 - IGNORED - The message is split in chunks of length key.size
            // TRAP 1 - IGNORED - Each chunk is reversed (eg. asdfg --> gfdsa)

            //  n is the sum of the ASCII decimal codes of the encryption key
            var sumKeyToAsciBytes = key.Select(x => (int)x).Sum();
            var numCharAllowed = 125 - 32 + 1;
            var restToSum = sumKeyToAsciBytes % numCharAllowed;
            var retChunks = new List<string>();

            var shiftedChars = new List<char>();
            foreach (var c in message)
            {
                var asciiCharPos = (int)c;
                var newAsciiCharPos = asciiCharPos - restToSum;
                if (newAsciiCharPos < 32)
                {
                    newAsciiCharPos = 125 - (31 - newAsciiCharPos);
                }
                asciiCharPos = (char)newAsciiCharPos;

                shiftedChars.Add((char)asciiCharPos);
            }
            // TRAP 1 - IGNORED - After the operation, each chunk has to be reversed back again
            retChunks.Add(string.Concat(shiftedChars));

            return string.Concat(retChunks);
        }

    }
}
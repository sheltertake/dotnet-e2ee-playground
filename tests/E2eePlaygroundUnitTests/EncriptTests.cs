using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E2eePlayegroundUnitTests
{
    public class EncriptTests
    {

        //Some examples:
        //titanic-> 3(3 - ("
        //minecraft-> , (-$"1 %3
        //javascript-> ) 5 2"1(/3
        [TestCase("titanic", "3(3 - (\"")]
        [TestCase("minecraft", ", (-$\"1 %3")]
        [TestCase("javascript", ") 5 2\"1(/3")]
        public void TestAlgo(string input, string expected)
        {

            // arrange
            // act
            var sut = Encript("fullstack", input);

            // assert
            sut.Should().Be(expected);
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
        private static string Encript(string key, string message)
        {
            // The message is split in chunks of length key.size
            // In cryptography, key size or key length is the size (measured in bits or bytes) 
            var keySize = System.Text.ASCIIEncoding.ASCII.GetByteCount(key);
            var chunks = SplitBy(message, keySize);
            //  Each chunk is reversed (eg. asdfg --> gfdsa)
            var reversedChunks = chunks.Select(x => Reverse(x)).ToList();
            //  n is the sum of the ASCII decimal codes of the encryption key
            var sumKeyToAsciBytes = key.Select(x => (int)x).Sum();//ToASCIBytes(key)

            var numCharAllowed = 125 - 32;
            var restToSum = sumKeyToAsciBytes % numCharAllowed;
            //  Each character is shifted upwards by n positions where 
            for (int i = 0; i < reversedChunks.Count; i++)
            {
                var shiftedChars = new List<char>();
                foreach (var c in reversedChunks[i])
                {
                    var asciiCharPos = (int)c;

                    var newAsciiCharPos = asciiCharPos + restToSum;

                    if (newAsciiCharPos > 125)
                        newAsciiCharPos = 31 + (newAsciiCharPos - 125);

                    // The alphabet is restricted between ASCII codes 32(" ") and 125("}") included
                    if (newAsciiCharPos < 32 || newAsciiCharPos > 125)
                        throw new Exception("not ASCII allowed");

                    //var newStringChar = new string((char)x, 1));
                    var shiftedChar = (char)newAsciiCharPos;
                    shiftedChars.Add(shiftedChar);
                }
                // After the operation, each chunk has to be reversed back again
                reversedChunks[i] = Reverse(string.Concat(shiftedChars));
            }

            return string.Concat(reversedChunks);
        }
        public static int[] ShiftBy(int[] datas, int n)
        {
            int[] temp = new int[datas.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = datas[(i + n) % temp.Length];
            }
            return temp;
        }
        private static string ShiftUpwardsEachChar(string input, int upward)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);

            for (int i = 0; i < bytes.Length; i++)
            {
                //bytes[i] = bytes[i] + upward;
            }

            return input;
        }

        private static byte[] ToASCIBytes(string input)
        {
            // Convert the string into a byte[].
            byte[] asciiBytes = Encoding.ASCII.GetBytes(input);
            return asciiBytes;
        }

        // https://stackoverflow.com/questions/1450774/splitting-a-string-into-chunks-of-a-certain-size
        private static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        // https://stackoverflow.com/questions/228038/best-way-to-reverse-a-string
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
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
            return message;
        }
    }
}
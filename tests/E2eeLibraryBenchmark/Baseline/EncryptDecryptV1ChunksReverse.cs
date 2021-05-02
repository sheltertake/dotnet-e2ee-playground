using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E2eeLibraryBenchmark.Baseline
{
    public static class EncryptDecryptV1ChunksReverse
    {
        public static bool USE_CHUNKS = false;
        public static bool REVERSE_TWICE = false;
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
        public static string Encrypt(this string message, string key)
        {
            // The message is split in chunks of length key.size
            // In cryptography, key size or key length is the size (measured in bits or bytes) 
            var keySize = key.Length;
            
            // chunks or not?
            var chunks = USE_CHUNKS ?
                SplitBy(message, keySize) :
                new List<string>() { message };

            //  Each chunk is reversed (eg. asdfg --> gfdsa)
            var reversedChunks = REVERSE_TWICE ?
                chunks.Select(Reverse).ToList() :
                chunks.ToList();

            //  n is the sum of the ASCII decimal codes of the encryption key
            var sumKeyToAsciBytes = key.Select(x => (int)x).Sum();
            var numCharAllowed = 125 - 32 + 1;
            var restToSum = sumKeyToAsciBytes % numCharAllowed;
            var retChunks = new List<string>();

            foreach (var chunk in reversedChunks)
            {
                var shiftedChars = new List<char>();
                foreach (var c in chunk)
                {
                    var asciiCharPos = (int)c;
                    var newAsciiCharPos = asciiCharPos + restToSum;

                    if (newAsciiCharPos > 125)
                        newAsciiCharPos = 31 + (newAsciiCharPos - 125);

                    asciiCharPos = (char)newAsciiCharPos;

                    shiftedChars.Add((char)asciiCharPos);
                }
                // After the operation, each chunk has to be reversed back again
                var toAdd = REVERSE_TWICE ? 
                    Reverse(string.Concat(shiftedChars)) : 
                    string.Concat(shiftedChars);

                retChunks.Add(toAdd);
            }
            return string.Concat(retChunks);
        }
        public static string Decrypt(this ReadOnlySpan<char> message, string key)
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

        public static IEnumerable<string> SplitBy(string str, int chunkLength)
        {
            for (int i = 0; i < str.Length; i += chunkLength)
            {
                if (chunkLength + i > str.Length)
                    chunkLength = str.Length - i;

                yield return str.Substring(i, chunkLength);
            }
        }

        // https://stackoverflow.com/questions/228038/best-way-to-reverse-a-string
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

    }
}

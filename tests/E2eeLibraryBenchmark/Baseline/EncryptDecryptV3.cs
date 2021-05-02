using System;
using System.Data;
using System.Linq;

namespace E2eeLibraryBenchmark.Baseline
{
    public static class EncryptDecryptV3
    {
        private const byte MAX_CHAR_CODE = 125;
        private const byte MIN_CHAR_CODE = 32;
        private const byte LIMIT_CHAR_CODE = MIN_CHAR_CODE - 1;
        private const byte MAX_CHAR_ALLOWED = MAX_CHAR_CODE - MIN_CHAR_CODE + 1;
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
            return EncryptDecrypt(message, key);
        }
        public static string Encrypt(this ReadOnlySpan<char> message, string key)
        {
            return EncryptDecrypt(message, key);
        }
        public static string Decrypt(this string message, string key)
        {
            return EncryptDecrypt(message, key, false);
        }
        public static string Decrypt(this ReadOnlySpan<char> message, string key)
        {
            return EncryptDecrypt(message, key, false);
        }

        private static string EncryptDecrypt(ReadOnlySpan<char> message, string key, bool leftDirection = true)
        {
            // TRAP 2 - IGNORED - The message is split in chunks of length key.size
            // TRAP 1 - IGNORED - Each chunk is reversed (eg. asdfg --> gfdsa)

            //  n is the sum of the ASCII decimal codes of the encryption key
            int sumKeyToAsciBytes = key.Select(x => (int)x).Sum();
            int restToSum = sumKeyToAsciBytes % MAX_CHAR_ALLOWED;
            int limit = message.Length;

            char[] shiftedChars = new char[message.Length];
            for (int i = 0; i < limit; i++)
            {
                int asciiCharPos = message[i];
                int newAsciiCharPos = leftDirection ?
                    asciiCharPos + restToSum :
                    asciiCharPos - restToSum;


                if (leftDirection && newAsciiCharPos > MAX_CHAR_CODE)
                {
                    newAsciiCharPos = LIMIT_CHAR_CODE + (newAsciiCharPos - MAX_CHAR_CODE);
                }
                else if (!leftDirection && newAsciiCharPos < MIN_CHAR_CODE)
                {
                    newAsciiCharPos = MAX_CHAR_CODE - (LIMIT_CHAR_CODE - newAsciiCharPos);
                }

                asciiCharPos = (char)newAsciiCharPos;

                shiftedChars[i] = (char)asciiCharPos;
            }
            // TRAP 1 - IGNORED - After the operation, each chunk has to be reversed back again

            return new string(shiftedChars);
        }
    }
}

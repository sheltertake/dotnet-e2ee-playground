using System.Linq;
using System.Text;

namespace E2eeLibrary
{
    public static class EncryptDecrypt
    {
        private const byte MAX_CHAR_CODE_125 = 125;
        private const byte MIN_CHAR_CODE_32 = 32;
        private const byte NUM_CHAR_ALLOWED_94 = MAX_CHAR_CODE_125 - MIN_CHAR_CODE_32 + 1;
        private const byte LOWER_SEGMENT_BOUND_31 = MIN_CHAR_CODE_32 - 1;
        private const byte UPPER_SEGMENT_BOUND_126 = MAX_CHAR_CODE_125 + 1;
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
            return EncryptDecryptToString(message, key);
        }
        public static byte[] Encrypt(this byte[] message, string key)
        {
            return EncryptDecript(message, key);
        }
        public static string Decrypt(this string message, string key)
        {
            return EncryptDecryptToString(message, key, false);
        }
        public static byte[] Decrypt(this byte[] message, string key)
        {
            return EncryptDecript(message, key, false);
        }
        private static string EncryptDecryptToString(string message, string key, bool leftDirection = true)
        {
            var bytes = Encoding.ASCII.GetBytes(message);
            var decrypted = EncryptDecript(bytes, key, leftDirection);
            //return Encoding.ASCII.GetString(decrypted);
            string result = string.Create(decrypted.Length, decrypted, (chars, buf) =>
            {
                var limit = chars.Length;
                for (int i = 0; i < limit; i++) chars[i] = (char)buf[i];
            });
            return result;
        }
        private static byte[] EncryptDecript(byte[] bytes, string key, bool leftDirection = true)
        {
            int sumKeyToAsciBytes = key.Select(x => (int)x).Sum();
            int restToSum = sumKeyToAsciBytes % NUM_CHAR_ALLOWED_94;
            int limit = bytes.Length;

            for (int i = 0; i < limit; i++)
            {
                if (leftDirection)
                {
                    bytes[i] = (byte)(bytes[i] + restToSum);
                    if (bytes[i] > MAX_CHAR_CODE_125)
                        bytes[i] = (byte)(LOWER_SEGMENT_BOUND_31 + (bytes[i] - MAX_CHAR_CODE_125));

                }
                else
                {
                    bytes[i] = (byte)(bytes[i] - restToSum);
                    if (bytes[i] < MIN_CHAR_CODE_32)
                        bytes[i] = (byte)(UPPER_SEGMENT_BOUND_126 - (MIN_CHAR_CODE_32 - bytes[i]));
                }

            }
            // TRAP 1 - IGNORED - After the operation, each chunk has to be reversed back again

            return bytes;
        }
    }
}

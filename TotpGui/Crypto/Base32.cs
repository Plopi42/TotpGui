using System;
using System.Text;
using System.Text.RegularExpressions;

namespace TotpGui.Crypto
{
    static class Base32
    {
        public static bool isBase32(string encoded)
        {
            return !Regex.IsMatch(encoded, @"[^ABCDEFGHIJKLMNOPQRSTUVWXYZ234567\s=-]");
        }

        public static byte[] decode(string encoded)
        {
            int    byteCount;
            byte[] decoded;
            byte   curByte    = 0;
            byte   leftOvers  = 8;
            int    mask       = 0;
            int    arrayIndex = 0;

            if (string.IsNullOrEmpty(encoded))
                return null;

            if (!isBase32(encoded))
                return null;

            encoded = Regex.Replace(encoded, @"[\s=-]+", "");

            byteCount = encoded.Length * 5 / 8;
            decoded   = new byte[byteCount];


            foreach (char c in encoded)
            {
                int v = char2Val(c);

                if (leftOvers > 5)
                {
                    mask = v << (leftOvers - 5);
                    curByte = (byte)(curByte | mask);
                    leftOvers -= 5;
                }
                else
                {
                    mask = v >> (5 - leftOvers);
                    curByte = (byte)(curByte | mask);
                    decoded[arrayIndex++] = curByte;
                    curByte = (byte)(v << (3 + leftOvers));
                    leftOvers += 3;
                }
            }

            //if we didn't end with a full byte
            if (arrayIndex != byteCount)
            {
                decoded[arrayIndex] = curByte;
            }

            return decoded;
        }

        public static string encode(string src)
        {
            return encode(getBytes(src));
        }

        public static string encode(byte[] src)
        {
            int    encodedLen;
            int    offset    = 0;
            char[] encoded;
            byte   nextChar  = 0;
            byte   leftOvers = 5;

            if (src == null || src.Length == 0)
                return null;

            encodedLen = (int)Math.Ceiling(src.Length / 5d) * 8;
            encoded    = new char[encodedLen];

            foreach (byte b in src)
            {
                nextChar = (byte)(nextChar | (b >> (8 - leftOvers)));
                encoded[offset++] = val2Char(nextChar);

                if (leftOvers < 4)
                {
                    nextChar = (byte)((b >> (3 - leftOvers)) & 31);
                    encoded[offset++] = val2Char(nextChar);
                    leftOvers += 5;
                }

                leftOvers -= 3;
                nextChar = (byte)((b << leftOvers) & 31);
            }

            // Padding
            if (offset != encodedLen)
            {
                encoded[offset++] = val2Char(nextChar);
                while (offset != encodedLen)
                    encoded[offset++] = '=';
            }

            return new string(encoded);
        }

        private static int char2Val(char c)
        {
            int value = (int)c;

            //65-90 == uppercase letters
            if (value < 91 && value > 64)
            {
                return value - 65;
            }
            //50-55 == numbers 2-7
            if (value < 56 && value > 49)
            {
                return value - 24;
            }
            //97-122 == lowercase letters
            if (value < 123 && value > 96)
            {
                return value - 97;
            }

            throw new ArgumentException("Invalid character for Base32.", "c");
        }

        private static char val2Char(byte b)
        {
            if (b < 26)
            {
                return (char)(b + 65);
            }

            if (b < 32)
            {
                return (char)(b + 24);
            }

            throw new ArgumentException("Invalid byte for Base32.", "b");
        }

        public static byte[] getBytes(string str)
        {
            //Encoding.ASCII.GetBytes(input);
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string getString(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
            //char[] chars = new char[bytes.Length / sizeof(char)];
            //System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            //return new string(chars);
        }
    }
}

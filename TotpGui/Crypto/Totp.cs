using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TotpGui.Crypto
{
    class Totp
    {
        private const int CODE_DIGITS = 6;

        public static string now(string base32Secret)
        {
            byte[] secret = Base32.decode(base32Secret);

            if (secret == null)
                return null;

            return now(secret);
        }

        public static string now(byte[] secret)
        {
            HMACSHA1 hmac = new HMACSHA1(secret);
            byte[]   mac;
            byte[]   time = BitConverter.GetBytes(timeInterval());

            if (BitConverter.IsLittleEndian)
                Array.Reverse(time);

            hmac.Initialize();
            mac = hmac.ComputeHash(time);

            return extractCode(mac);
        }

        private static string extractCode(byte[] hmac)
        {
            int start = hmac[hmac.Length - 1] & 0x0f;

            // extract those 4 bytes
            byte[] bytes = new byte[4];
            Array.Copy(hmac, start, bytes, 0, 4);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            uint fullcode = BitConverter.ToUInt32(bytes, 0) & 0x7fffffff;

            // we use the last 8 digits of this code in radix 10
            uint   codemask = (uint)Math.Pow(10, CODE_DIGITS);
            string format   = new string('0', CODE_DIGITS);
            string code     = (fullcode % codemask).ToString(format);

            return code;
        }

        public static long getIntervalPercent()
        {
            long time = getTime() % 30000L;

            return (time * 100) / 30000L;
        }

        private static long timeInterval()
        {
            return getTime() / 30000L;
        }

        private static long getTime()
        {
            long time = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds);

            return time + serverDiff();
        }

        private static long serverDiff()
        {
            // TODO : Check google time
            return 0;
        }
    }
}

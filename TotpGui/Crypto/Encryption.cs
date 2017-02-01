using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TotpGui.Crypto
{
    class Encryption
    {
        private static byte[] ENTROPY = { 9, 7, 7, 6, 4 };
        private const  string IV_TEXT = @"Totp_16_bits_IV.";
        private static byte[] IV      = Encoding.ASCII.GetBytes(IV_TEXT);

        // AES 256 (IV: 16bits, Block: 128bits, Key: 256bits)
        // ProtectedData : solution explorer >> add reference >> select the .Net tab >> select System.Security
        public static string encrypt(string password, string data)
        {
            byte[] hash = sha256(password, 3);

            return encrypt(hash, data);
        }

        public static string encrypt(byte[] hash, string data)
        {
            byte[] step1 = EncryptStringToBytes_Aes(data, hash, IV);
            byte[] step2 = ProtectedData.Protect(step1, ENTROPY, DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(step2);
        }

        public static string decrypt(string password, string data)
        {
            byte[] hash = sha256(password, 3);

            return decrypt(hash, data);
        }

        public static string decrypt(byte[] hash, string data)
        {
            try
            {
                byte[] step2 = Convert.FromBase64String(data);
                byte[] step1 = ProtectedData.Unprotect(step2, ENTROPY, DataProtectionScope.CurrentUser);

                return DecryptStringFromBytes_Aes(step1, hash, IV);
            }
            catch
            {
                return null;
            }
        }

        // Based on MSDN example at https://msdn.microsoft.com/en-us/library/system.security.cryptography.aesmanaged(v=vs.110).aspx
        private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create an AesManaged object with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.BlockSize = 128;
                aesAlg.KeySize   = 256;
                aesAlg.Key       = Key;
                aesAlg.IV        = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        // Based on MSDN example at https://msdn.microsoft.com/en-us/library/system.security.cryptography.aesmanaged(v=vs.110).aspx
        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Declare the string used to hold the decrypted text.
            string plaintext = null;

            // Create an AesManaged object with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.BlockSize = 128;
                aesAlg.KeySize   = 256;
                aesAlg.Key       = Key;
                aesAlg.IV        = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }

        public static byte[] sha256(string data, int rounds)
        {
            SHA256Managed shaAlg = new SHA256Managed();
            byte[] res = Encoding.Unicode.GetBytes(data);

            for (int i = 0; i < rounds; ++i)
                res = shaAlg.ComputeHash(res);

            return res;
        }
    }
}

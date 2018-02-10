using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace BlockchainWallet.Services
{
    public class CryptoManager
    {
        private const string DefaultInvalidHexMsg = "Invalid HEX!";

        public byte[] HexToByte(string hexHash)
        {
            if (hexHash.Length % 2 != 0)
            {
                throw new ArgumentException(DefaultInvalidHexMsg);
            }

            var result = new byte[hexHash.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = byte.Parse(hexHash.Substring(i * 2, 2),
                    NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return result;
        }

        public string ByteToHex(byte[] bytes)
        {
            var hex = BitConverter.ToString(bytes);
            return hex;
        }

        public byte[] SHA256(byte[] array)
        {
            var sha256 = new SHA256Managed();
            var result = sha256.ComputeHash(array);
            
            return result;
        }

        public byte[] CreateSignature(string[] parameters )
        {

            return null;
        }

        public byte[] MergeArrays(byte[] first, byte[] second)
        {
            var extended = new byte[first.Length + second.Length];

            Array.Copy(first, extended, first.Length);
            Array.Copy(second, 0, extended, first.Length, second.Length);
            
            return extended;
        }

        public byte[] GetBytes(string input)
        {
            byte[] asBytes = Encoding.Unicode.GetBytes(input);

            return asBytes;
        }

        //private static byte[] GetSignature(byte[] privatekey, byte[] msg)
        //{
        //    ECDsaCng ecsdKey;
        //    if (ecsdKey == null)
        //    {
        //        ecsdKey = new ECDsaCng(CngKey.Import(privatekey, CngKeyBlobFormat.EccPrivateBlob));
        //        ecsdKey.HashAlgorithm = CngAlgorithm.Sha512;
        //    }
        //    byte[] signature = ecsdKey.SignData(msg);
        //    if (ecsdKey.VerifyData(msg, signature))
        //        return signature;
        //    else
        //        throw new Exception("Data Verify Failed!");
        //}
    }
}

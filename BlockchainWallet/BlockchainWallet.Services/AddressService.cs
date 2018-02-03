namespace BlockchainWallet.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Models.Dto;
    using Org.BouncyCastle.Asn1.Sec;
    using Org.BouncyCastle.Math;

    public class AddressService
    {
        private static readonly Random Rand = new Random();

        public AddressDto CreateAddress()
        {
            var words = new List<string>();
            for (int i = 0; i < 12; i++)
            {
                var wordId = Rand.Next(0, MnemonicWords.Words.Count - 1);
                words.Add(MnemonicWords.Words[wordId]);
            }

            var mnemonic = string.Join(" ", words);
            return this.CreateAddress(mnemonic);
        }

        public AddressDto CreateAddress(string mnemonic)
        {
            var privateKeyData = this.Sha(mnemonic);
            var publicKeyData = this.GetPublicKey(privateKeyData);

            var privateKey = this.ByteToHex(privateKeyData);
            var publicKey = this.ByteToHex(publicKeyData.key) + (publicKeyData.isEven ? "1" : "0");

            var addressRipe = this.HexToRipe(publicKey);
            var address = this.ByteToHex(addressRipe);

            return new AddressDto
            {
                Mnemonic = mnemonic,
                PrivateKey = privateKey,
                PublicKey = publicKey,
                Address = address
            };
        }

        public (byte[] key, bool isEven) GetPublicKey(byte[] privateKey)
        {
            BigInteger privKeyInt = new BigInteger(+1, privateKey);

            var parameters = SecNamedCurves.GetByName("secp256k1");
            var qa = parameters.G.Multiply(privKeyInt);

            var xCoord = qa.XCoord.ToBigInteger();
            byte[] pubKeyX = xCoord.ToByteArrayUnsigned();
            var isEven = xCoord.Remainder(new BigInteger("2")).IntValue == 0;

            return (pubKeyX, isEven);
        }

        private byte[] Sha(string data)
        {
            using (var sha256 = new SHA256Managed())
            {
                var hash = sha256.ComputeHash(Encoding.Unicode.GetBytes(data));
                return hash;
            }
        }

        private string ByteToHex(byte[] data)
        {
            return string.Join("", data.Select(h => h.ToString("x2")));
        }

        private byte[] HexToRipe(string data)
        {
            using (var ripe = new RIPEMD160Managed())
            {
                return ripe.ComputeHash(Encoding.Unicode.GetBytes(data));
            }
        }
    }
}

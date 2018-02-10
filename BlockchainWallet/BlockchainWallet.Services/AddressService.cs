namespace BlockchainWallet.Services
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Models.Dto;
    using Org.BouncyCastle.Asn1.Sec;
    using Org.BouncyCastle.Asn1.X9;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security;

    public class AddressService
    {
        private static readonly X9ECParameters Curve = SecNamedCurves.GetByName("secp256k1");
        private static readonly ECDomainParameters Domain = new ECDomainParameters(Curve.Curve, Curve.G, Curve.N, Curve.H);

        public AddressDto CreateAddress(string mnemonic)
        {
            var privateKeyBytes = this.Sha(mnemonic);
            var publicKeyParameters = this.ToPublicKey(privateKeyBytes);
            var publicKeyData = publicKeyParameters.Q.GetEncoded();

            var privateKey = this.ByteToHex(privateKeyBytes);
            var publicKey = this.ByteToHex(publicKeyData);

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

        public byte[] GetBytes(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            return bytes;
        }

        public ECPublicKeyParameters ToPublicKey(string privateKey)
        {
            return this.ToPublicKey(this.GetBytes(privateKey));
        }

        public ECPublicKeyParameters ToPublicKey(byte[] privateKey)
        {
            BigInteger d = new BigInteger(privateKey);
            var q = Domain.G.Multiply(d);

            var publicParams = new ECPublicKeyParameters(q, Domain);
            return publicParams;
        }

        public string GetPublicKey(ECPublicKeyParameters publicKeyParameters)
        {
            var publicKeyData = publicKeyParameters.Q.GetEncoded();
            var publicKey = this.ByteToHex(publicKeyData);

            return publicKey;
        }

        private byte[] Sha(string data)
        {
            using (var sha256 = new SHA256Managed())
            {
                var hash = sha256.ComputeHash(Encoding.Unicode.GetBytes(data));
                return hash;
            }
        }

        public string ByteToHex(byte[] data)
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

        public byte[] SignData(string msg, string privateKey)
        {
            return this.SignData(msg, this.GetBytes(privateKey));
        }

        public byte[] SignData(string msg, byte[] privateKey)
        {
            BigInteger privateKeyInt = new BigInteger(privateKey);
            ECPrivateKeyParameters privateKeyParameters = new ECPrivateKeyParameters(privateKeyInt, Domain);
            byte[] msgBytes = Encoding.UTF8.GetBytes(msg);

            ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
            signer.Init(true, privateKeyParameters);
            signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
            byte[] sigBytes = signer.GenerateSignature();

            return sigBytes;
        }

        public bool VerifySignature(ECPublicKeyParameters pubKey, byte[] signature, string msg)
        {
            byte[] msgBytes = Encoding.UTF8.GetBytes(msg);

            ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
            signer.Init(false, pubKey);
            signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
            return signer.VerifySignature(signature);
        }
    }
}
using System;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities.Encoders;
using System.Security.Cryptography.X509Certificates;
using ApplePayDecrypter.Definition;

namespace ApplePayDecrypter
{
    public class ApplePayDecrypt
    {
        private readonly ApplePayRequest applePayRequest;
        private readonly byte[] symmetricIv;

        public ApplePayDecrypt(ApplePayRequest applePayRequest)
        {
            this.applePayRequest = applePayRequest;
            symmetricIv = Hex.Decode("00000000000000000000000000000000");
        }

        public string Decrypt()
        {
            var privateKey = GetMerchantPrivateKey(applePayRequest.PrivateKeyBytes);
            var ephemeralPublicKey = Base64.Decode(applePayRequest.ApplePayHeader.EphemeralPublicKey);
            var publicKey = GetPublicKeyParameters(ephemeralPublicKey);

            var sharedSecretBytes = GenerateSharedSecret(privateKey, publicKey);
            var encryptionKeyBytes = RestoreSymmetricKey(sharedSecretBytes);

            var decryptedBytes = DoDecrypt(Base64.Decode(applePayRequest.Data), encryptionKeyBytes);
            var decryptedString = Encoding.Default.GetString(decryptedBytes);

            return decryptedString;
        }

        public ECPublicKeyParameters GetPublicKeyParameters(byte[] ephemeralPublicKeyBytes)
        {
            return (ECPublicKeyParameters)PublicKeyFactory.CreateKey(ephemeralPublicKeyBytes);
        }

        public static ECPrivateKeyParameters GetMerchantPrivateKey(byte[] privateKeyBite)
        {
            var akp = PrivateKeyFactory.CreateKey(privateKeyBite);
            return (ECPrivateKeyParameters)akp;
        }

        private byte[] GenerateSharedSecret(ECPrivateKeyParameters privateKey, ECPublicKeyParameters publicKeys)
        {
            var keyParams = privateKey;
            var agree = AgreementUtilities.GetBasicAgreement("ECDH");
            agree.Init(keyParams);
            var sharedSecret = agree.CalculateAgreement(publicKeys);
            return sharedSecret.ToByteArrayUnsigned();
        }

        protected byte[] RestoreSymmetricKey(byte[] sharedSecretBytes)
        {
            var merchantIdentifier = ExtractMIdentifier();

            var generator = new ConcatenationKdfGenerator(new Sha256Digest());
            var algorithmIdBytes = Encoding.UTF8.GetBytes((char)0x0d + "id-aes256-GCM");
            var partyUInfoBytes = Encoding.UTF8.GetBytes("Apple");
            var partyVInfoBytes = merchantIdentifier;
            var otherInfoBytes = Combine(Combine(algorithmIdBytes, partyUInfoBytes), partyVInfoBytes);

            generator.Init(new KdfParameters(sharedSecretBytes, otherInfoBytes));
            var encryptionKeyBytes = new byte[32];
            generator.GenerateBytes(encryptionKeyBytes, 0, encryptionKeyBytes.Length);
            return encryptionKeyBytes;
        }

        private byte[] ExtractMIdentifier()
        {
            var merchantCertificate = new X509Certificate2(applePayRequest.ApplePayP12Path);
            var merchantIdentifierTlv = merchantCertificate.Extensions["1.2.840.113635.100.6.32"].RawData;
            var merchantIdentifier = new byte[64];

            Buffer.BlockCopy(merchantIdentifierTlv, 2, merchantIdentifier, 0, 64);

            return Hex.Decode(Encoding.ASCII.GetString(merchantIdentifier));
        }

        private byte[] DoDecrypt(byte[] cipherData, byte[] encryptionKeyBytes)
        {
            var keyParam = ParameterUtilities.CreateKeyParameter("AES", encryptionKeyBytes);
            var parameters = new ParametersWithIV(keyParam, symmetricIv);
            var cipher = GetCipher();
            cipher.Init(false, parameters);
            var output = cipher.DoFinal(cipherData);

            return output;
        }

        public IBufferedCipher GetCipher()
        {
            return CipherUtilities.GetCipher("AES/GCM/NoPadding");
        }

        private static byte[] GetHashSha256Bytes(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            var hashString = new SHA256Managed();
            var hash = hashString.ComputeHash(bytes);
            return hash;
        }

        protected static byte[] Combine(byte[] first, byte[] second)
        {
            var ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

    }
}

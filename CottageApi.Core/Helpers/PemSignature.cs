using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace CottageApi.Core.Helpers
{
    public static class PemSignature
    {
        public static string SignStringWithPrivatePem(string message)
        {
            string privateKeyPath = Path.Combine(Directory.GetCurrentDirectory(), "7702999.pem");

            var privateRsa = RsaProviderFromPrivateKeyInPemFile(privateKeyPath);

            var signedData = privateRsa.SignData(Encoding.UTF8.GetBytes(message), CryptoConfig.MapNameToOID("SHA1"));

            return Convert.ToBase64String(signedData);
        }

        public static string SignStringWithRS256(string message)
        {
            string privateKeyPath = Path.Combine(Directory.GetCurrentDirectory(), "7702999.pem");
            string keyName = "7702999";

            /*CspParameters cspp = new CspParameters();
            cspp.Flags = CspProviderFlags.UseMachineKeyStore;
            cspp.KeyContainerName = keyName;*/

            try
            {
                using (RSACryptoServiceProvider rsa = RsaProviderFromPrivateKeyInPemFile(privateKeyPath, null))
                {
                    byte[] hash;
                    using (var sha256 = SHA256.Create())
                    {
                        byte[] data = Encoding.UTF8.GetBytes(message);
                        hash = sha256.ComputeHash(data);
                    }

                    RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);

                    rsaFormatter.SetHashAlgorithm("SHA256");

                    byte[] signedHash = rsaFormatter.CreateSignature(hash);

                    string base64 = Base64UrlEncoder.Encode(signedHash);

                    return base64;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private static RSACryptoServiceProvider RsaProviderFromPrivateKeyInPemFile(string privateKeyPath, CspParameters cspp = null)
        {
            using (TextReader privateKeyTextReader = new StringReader(File.ReadAllText(privateKeyPath)))
            {
                PemReader pr = new PemReader(privateKeyTextReader);
                AsymmetricCipherKeyPair keyPair = (AsymmetricCipherKeyPair)pr.ReadObject();
                RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)keyPair.Private);

                RSACryptoServiceProvider csp = new RSACryptoServiceProvider(cspp);
                csp.ImportParameters(rsaParams);
                return csp;
            }
        }
    }
}

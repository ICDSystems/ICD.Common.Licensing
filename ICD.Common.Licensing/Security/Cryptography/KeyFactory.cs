using System;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace ICD.Common.Licensing.Security.Cryptography
{
	internal static class KeyFactory
	{
		private static readonly string s_KeyEncryptionAlgorithm = PkcsObjectIdentifiers.PbeWithShaAnd3KeyTripleDesCbc.Id;

		/// <summary>
		/// Encrypts and encodes the private key.
		/// </summary>
		/// <param name="key">The private key.</param>
		/// <param name="passPhrase">The pass phrase to encrypt the private key.</param>
		/// <returns>The encrypted private key.</returns>
		public static string ToEncryptedPrivateKeyString(AsymmetricKeyParameter key, string passPhrase)
		{
			var salt = new byte[16];
			var secureRandom = SecureRandom.GetInstance("SHA256PRNG");
			secureRandom.SetSeed(SecureRandom.GetSeed(16)); //See Bug #135
			secureRandom.NextBytes(salt);

			return
				Convert.ToBase64String(PrivateKeyFactory.EncryptKey(s_KeyEncryptionAlgorithm, passPhrase.ToCharArray(), salt, 10, key));
		}

		/// <summary>
		/// Decrypts the provided private key.
		/// </summary>
		/// <param name="privateKey">The encrypted private key.</param>
		/// <param name="passPhrase">The pass phrase to decrypt the private key.</param>
		/// <returns>The private key.</returns>
		public static AsymmetricKeyParameter FromEncryptedPrivateKeyString(string privateKey, string passPhrase)
		{
			return PrivateKeyFactory.DecryptKey(passPhrase.ToCharArray(), Convert.FromBase64String(privateKey));
		}

		/// <summary>
		/// Encodes the public key into DER encoding.
		/// </summary>
		/// <param name="key">The public key.</param>
		/// <returns>The encoded public key.</returns>
		public static string ToPublicKeyString(AsymmetricKeyParameter key)
		{
			return
				Convert.ToBase64String(SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(key).ToAsn1Object().GetDerEncoded());
		}

		/// <summary>
		/// Decoded the public key from DER encoding.
		/// </summary>
		/// <param name="publicKey">The encoded public key.</param>
		/// <returns>The public key.</returns>
		public static AsymmetricKeyParameter FromPublicKeyString(string publicKey)
		{
			return PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
		}
	}
}

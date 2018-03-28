using Org.BouncyCastle.Crypto;

namespace ICD.Common.Licensing.Security.Cryptography
{
	/// <summary>
	/// Represents a private/public encryption key pair.
	/// </summary>
	public sealed class KeyPair
	{
		private readonly AsymmetricCipherKeyPair m_KeyPair;

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyPair"/> class
		/// with the provided asymmetric key pair.
		/// </summary>
		/// <param name="keyPair"></param>
		internal KeyPair(AsymmetricCipherKeyPair keyPair)
		{
			m_KeyPair = keyPair;
		}

		/// <summary>
		/// Gets the encrypted and DER encoded private key.
		/// </summary>
		/// <param name="passPhrase">The pass phrase to encrypt the private key.</param>
		/// <returns>The encrypted private key.</returns>
		public string ToEncryptedPrivateKeyString(string passPhrase)
		{
			return KeyFactory.ToEncryptedPrivateKeyString(m_KeyPair.Private, passPhrase);
		}

		/// <summary>
		/// Gets the DER encoded public key.
		/// </summary>
		/// <returns>The public key.</returns>
		public string ToPublicKeyString()
		{
			return KeyFactory.ToPublicKeyString(m_KeyPair.Public);
		}
	}
}

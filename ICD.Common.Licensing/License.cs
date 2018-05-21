//
// Copyright © 2012 - 2013 Nauck IT KG     http://www.nauck-it.de
// Copyright © 2017 Nivloc Enterprises Ltd
// Copyright © 2018 ICD Systems
//
// Author:
//  Daniel Nauck        <d.nauck(at)nauck-it.de>
//  Neil Colvin
//  Chris Cameron
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#if SIMPLSHARP
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharp.CrestronXml;
using Crestron.SimplSharp.CrestronXmlLinq;
#else
using System.IO;
using System.Xml;
using System.Xml.Linq;
#endif
using System;
using System.Globalization;
using System.Text;
using ICD.Common.Licensing.Security.Cryptography;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Security;

namespace ICD.Common.Licensing
{
	/// <summary>
	/// A software license
	/// </summary>
	public sealed class License
	{
		private readonly XElement m_XmlData;
		private readonly string m_SignatureAlgorithm = X9ObjectIdentifiers.ECDsaWithSha512.Id;

		/// <summary>
		/// Initializes a new instance of the <see cref="License"/> class.
		/// </summary>
		internal License()
		{
			m_XmlData = new XElement("License");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="License"/> class
		/// with the specified content.
		/// </summary>
		/// <remarks>This constructor is only used for loading from XML.</remarks>
		/// <param name="xmlData">The initial content of this <see cref="License"/>.</param>
		internal License(XElement xmlData)
		{
			m_XmlData = xmlData;
		}

		/// <summary>
		/// Gets or sets the unique identifier of this <see cref="License"/>.
		/// </summary>
		public Guid Id
		{
			get { return new Guid(GetTag("Id") ?? Guid.Empty.ToString()); }
			set
			{
				if (!IsSigned)
					SetTag("Id", value.ToString());
			}
		}

		/// <summary>
		/// Gets or set the <see cref="eLicenseType"/> or this <see cref="License"/>.
		/// </summary>
		public eLicenseType Type
		{
			get
			{
				return (eLicenseType)Enum.Parse(typeof(eLicenseType), GetTag("Type") ?? eLicenseType.Trial.ToString(), false);
			}
			set
			{
				if (!IsSigned)
					SetTag("Type", value.ToString());
			}
		}

		/// <summary>
		/// Get or sets the quantity of this license.
		/// E.g. the count of per-developer-licenses.
		/// </summary>
		public int Quantity
		{
			get { return int.Parse(GetTag("Quantity") ?? "0"); }
			set
			{
				if (!IsSigned)
					SetTag("Quantity", value.ToString());
			}
		}

		/// <summary>
		/// Gets or sets the product features of this <see cref="License"/>.
		/// </summary>
		public LicenseAttributes ProductFeatures
		{
			get
			{
				var xmlElement = m_XmlData.Element("ProductFeatures");

				if (!IsSigned && xmlElement == null)
				{
					m_XmlData.Add(new XElement("ProductFeatures"));
					xmlElement = m_XmlData.Element("ProductFeatures");
				}
				else if (IsSigned && xmlElement == null)
					return null;

				return new LicenseAttributes(xmlElement, "Feature");
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="Customer"/> of this <see cref="License"/>.
		/// </summary>
		public Customer Customer
		{
			get
			{
				var xmlElement = m_XmlData.Element("Customer");

				if (!IsSigned && xmlElement == null)
				{
					m_XmlData.Add(new XElement("Customer"));
					xmlElement = m_XmlData.Element("Customer");
				}
				else if (IsSigned && xmlElement == null)
					return null;

				return new Customer(xmlElement);
			}
		}

		/// <summary>
		/// Gets or sets the additional attributes of this <see cref="License"/>.
		/// </summary>
		public LicenseAttributes AdditionalAttributes
		{
			get
			{
				var xmlElement = m_XmlData.Element("LicenseAttributes");

				if (!IsSigned && xmlElement == null)
				{
					m_XmlData.Add(new XElement("LicenseAttributes"));
					xmlElement = m_XmlData.Element("LicenseAttributes");
				}
				else if (IsSigned && xmlElement == null)
					return null;

				return new LicenseAttributes(xmlElement, "Attribute");
			}
		}

		/// <summary>
		/// Gets or sets the expiration date of this <see cref="License"/>.
		/// Use this property to set the expiration date for a trial license
		/// or the expiration of support & subscription updates for a standard license.
		/// </summary>
		public DateTime Expiration
		{
			get
			{
				return
					DateTime.ParseExact(
					                    GetTag("Expiration") ??
					                    DateTime.MaxValue.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture), "r",
					                    CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
			}
			set
			{
				if (!IsSigned)
					SetTag("Expiration", value.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture));
			}
		}

		/// <summary>
		/// Gets the digital signature of this license.
		/// </summary>
		/// <remarks>Use the <see cref="License.Sign"/> method to compute a signature.</remarks>
		public string Signature { get { return GetTag("Signature"); } }

		/// <summary>
		/// Compute a signature and sign this <see cref="License"/> with the provided key.
		/// </summary>
		/// <param name="privateKey">The private key in xml string format to compute the signature.</param>
		/// <param name="passPhrase">The pass phrase to decrypt the private key.</param>
		public void Sign(string privateKey, string passPhrase)
		{
			var signTag = m_XmlData.Element("Signature") ?? new XElement("Signature");

			try
			{
				if (signTag.Parent != null)
					signTag.Remove();

				var privKey = KeyFactory.FromEncryptedPrivateKeyString(privateKey, passPhrase);

				var documentToSign = Encoding.UTF8.GetBytes(m_XmlData.ToString(SaveOptions.DisableFormatting));
				var signer = SignerUtilities.GetSigner(m_SignatureAlgorithm);
				signer.Init(true, privKey);
				signer.BlockUpdate(documentToSign, 0, documentToSign.Length);
				var signature = signer.GenerateSignature();
				signTag.Value = Convert.ToBase64String(signature);
			}
			finally
			{
				m_XmlData.Add(signTag);
			}
		}

		/// <summary>
		/// Determines whether the <see cref="License.Signature"/> property verifies for the specified key.
		/// </summary>
		/// <param name="publicKey">The public key in xml string format to verify the <see cref="License.Signature"/>.</param>
		/// <returns>true if the <see cref="License.Signature"/> verifies; otherwise false.</returns>
		public bool VerifySignature(string publicKey)
		{
			var signTag = m_XmlData.Element("Signature");

			if (signTag == null)
				return false;

			try
			{
				signTag.Remove();

				var pubKey = KeyFactory.FromPublicKeyString(publicKey);

				var documentToSign = Encoding.UTF8.GetBytes(m_XmlData.ToString(SaveOptions.DisableFormatting));
				var signer = SignerUtilities.GetSigner(m_SignatureAlgorithm);
				signer.Init(false, pubKey);
				signer.BlockUpdate(documentToSign, 0, documentToSign.Length);

				return signer.VerifySignature(Convert.FromBase64String(signTag.Value));
			}
			finally
			{
				m_XmlData.Add(signTag);
			}
		}

		/// <summary>
		/// Create a new <see cref="License"/> using the <see cref="ILicenseBuilder"/>
		/// fluent api.
		/// </summary>
		/// <returns>An instance of the <see cref="ILicenseBuilder"/> class.</returns>
		public static ILicenseBuilder New()
		{
			return new LicenseBuilder();
		}

		/// <summary>
		/// Loads a <see cref="License"/> from a string that contains XML.
		/// </summary>
		/// <param name="xmlString">A <see cref="string"/> that contains XML.</param>
		/// <returns>A <see cref="License"/> populated from the <see cref="string"/> that contains XML.</returns>
		public static License Load(string xmlString)
		{
			return new License(XElement.Parse(xmlString, LoadOptions.None));
		}

		/// <summary>
		/// Loads a <see cref="License"/> by using the specified <see cref="Stream"/>
		/// that contains the XML.
		/// </summary>
		/// <param name="stream">A <see cref="Stream"/> that contains the XML.</param>
		/// <returns>A <see cref="License"/> populated from the <see cref="Stream"/> that contains XML.</returns>
		public static License Load(Stream stream)
		{
#if SIMPLSHARP
			return new License(XElement.Load(new XmlReader(stream), LoadOptions.None));
#else
			return new License (XElement.Load (stream, LoadOptions.None));
#endif
		}

		/// <summary>
		/// Loads a <see cref="License"/> by using the specified <see cref="TextReader"/>
		/// that contains the XML.
		/// </summary>
		/// <param name="reader">A <see cref="TextReader"/> that contains the XML.</param>
		/// <returns>A <see cref="License"/> populated from the <see cref="TextReader"/> that contains XML.</returns>
		public static License Load(TextReader reader)
		{
#if SIMPLSHARP
			return new License(XElement.Load(new XmlReader(reader), LoadOptions.None));
#else
         return new License (XElement.Load(reader, LoadOptions.None));
#endif
		}

		/// <summary>
		/// Loads a <see cref="License"/> by using the specified <see cref="XmlReader"/>
		/// that contains the XML.
		/// </summary>
		/// <param name="reader">A <see cref="XmlReader"/> that contains the XML.</param>
		/// <returns>A <see cref="License"/> populated from the <see cref="TextReader"/> that contains XML.</returns>
		public static License Load(XmlReader reader)
		{
			return new License(XElement.Load(reader, LoadOptions.None));
		}
        
		/// <summary>
		/// Serialize this <see cref="License"/> to a <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">A <see cref="Stream"/> that the 
		/// <see cref="License"/> will be written to.</param>
#if RO
		internal
#else
		public
#endif
			void Save(Stream stream)
		{
#if SIMPLSHARP
			m_XmlData.Save(new XmlWriter(stream));
#else
			m_XmlData.Save(stream);
#endif
		}

		/// <summary>
		/// Serialize this <see cref="License"/> to a <see cref="TextWriter"/>.
		/// </summary>
		/// <param name="textWriter">A <see cref="TextWriter"/> that the 
		/// <see cref="License"/> will be written to.</param>
#if RO
		internal
#else
		public
#endif
			void Save(TextWriter textWriter)
		{
#if SIMPLSHARP
			m_XmlData.Save(new XmlWriter(textWriter));
#else
			m_XmlData.Save(textWriter);
#endif
		}

		/// <summary>
		/// Serialize this <see cref="License"/> to a <see cref="XmlWriter"/>.
		/// </summary>
		/// <param name="xmlWriter">A <see cref="XmlWriter"/> that the 
		/// <see cref="License"/> will be written to.</param>
#if RO
		internal
#else
		public
#endif
			void Save(XmlWriter xmlWriter)
		{
			m_XmlData.Save(xmlWriter);
		}

		/// <summary>
		/// Returns the indented XML for this <see cref="License"/>.
		/// </summary>
		/// <returns>A string containing the indented XML.</returns>
		public override string ToString()
		{
			return m_XmlData.ToString();
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="License"/> is already signed.
		/// </summary>
		private bool IsSigned { get { return (!string.IsNullOrEmpty(Signature)); } }

		private void SetTag(string name, string value)
		{
			var element = m_XmlData.Element(name);

			if (element == null)
			{
				element = new XElement(name);
				m_XmlData.Add(element);
			}

			if (value != null)
				element.Value = value;
		}

		private string GetTag(string name)
		{
			var element = m_XmlData.Element(name);
			return element != null ? element.Value : null;
		}
	}
}

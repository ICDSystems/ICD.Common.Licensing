﻿//
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

using System;
using System.Collections.Generic;

namespace ICD.Common.Licensing
{
	/// <summary>
	/// Implementation of the <see cref="ILicenseBuilder"/>, a fluent api
	/// to create new licenses.
	/// </summary>
	internal sealed class LicenseBuilder : ILicenseBuilder
	{
		private readonly License m_License;

		/// <summary>
		/// Initializes a new instance of the <see cref="LicenseBuilder"/> class.
		/// </summary>
		public LicenseBuilder()
		{
			m_License = new License();
		}

		/// <summary>
		/// Sets the unique identifier of the <see cref="License"/>.
		/// </summary>
		/// <param name="id">The unique identifier of the <see cref="License"/>.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		public ILicenseBuilder WithUniqueIdentifier(Guid id)
		{
			m_License.Id = id;
			return this;
		}

		/// <summary>
		/// Sets the <see cref="eLicenseType"/> of the <see cref="License"/>.
		/// </summary>
		/// <param name="type">The <see cref="eLicenseType"/> of the <see cref="License"/>.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		public ILicenseBuilder As(eLicenseType type)
		{
			m_License.Type = type;
			return this;
		}

		/// <summary>
		/// Sets the expiration date of the <see cref="License"/>.
		/// </summary>
		/// <param name="date">The expiration date of the <see cref="License"/>.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		public ILicenseBuilder ExpiresAt(DateTime date)
		{
			m_License.Expiration = date.ToUniversalTime();
			return this;
		}

		/// <summary>
		/// Sets the maximum utilization of the <see cref="License"/>.
		/// This can be the quantity of developers for a "per-developer-license".
		/// </summary>
		/// <param name="utilization">The maximum utilization of the <see cref="License"/>.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		public ILicenseBuilder WithMaximumUtilization(int utilization)
		{
			m_License.Quantity = utilization;
			return this;
		}

		/// <summary>
		/// Sets the <see cref="Customer">license holder</see> of the <see cref="License"/>.
		/// </summary>
		/// <param name="name">The name of the license holder.</param>
		/// <param name="email">The email of the license holder.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		public ILicenseBuilder LicensedTo(string name, string email)
		{
			m_License.Customer.Name = name;
			m_License.Customer.Email = email;
			return this;
		}

		/// <summary>
		/// Sets the <see cref="Customer">license holder</see> of the <see cref="License"/>.
		/// </summary>
		/// <param name="name">The name of the license holder.</param>
		/// <param name="email">The email of the license holder.</param>
		/// <param name="configureCustomer">A delegate to configure the license holder.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		public ILicenseBuilder LicensedTo(string name, string email, Action<Customer> configureCustomer)
		{
			m_License.Customer.Name = name;
			m_License.Customer.Email = email;
			configureCustomer(m_License.Customer);
			return this;
		}

		/// <summary>
		/// Sets the <see cref="Customer">license holder</see> of the <see cref="License"/>.
		/// </summary>
		/// <param name="configureCustomer">A delegate to configure the license holder.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		public ILicenseBuilder LicensedTo(Action<Customer> configureCustomer)
		{
			configureCustomer(m_License.Customer);
			return this;
		}

		/// <summary>
		/// Sets the licensed product features of the <see cref="License"/>.
		/// </summary>
		/// <param name="productFeatures">The licensed product features of the <see cref="License"/>.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		public ILicenseBuilder WithProductFeatures(IDictionary<string, string> productFeatures)
		{
			m_License.ProductFeatures.AddAll(productFeatures);
			return this;
		}

		/// <summary>
		/// Sets the licensed product features of the <see cref="License"/>.
		/// </summary>
		/// <param name="configureProductFeatures">A delegate to configure the product features.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		public ILicenseBuilder WithProductFeatures(Action<LicenseAttributes> configureProductFeatures)
		{
			configureProductFeatures(m_License.ProductFeatures);
			return this;
		}

		/// <summary>
		/// Sets the licensed additional attributes of the <see cref="License"/>.
		/// </summary>
		/// <param name="additionalAttributes">The additional attributes of the <see cref="License"/>.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		public ILicenseBuilder WithAdditionalAttributes(IDictionary<string, string> additionalAttributes)
		{
			m_License.AdditionalAttributes.AddAll(additionalAttributes);
			return this;
		}

		/// <summary>
		/// Sets the licensed additional attributes of the <see cref="License"/>.
		/// </summary>
		/// <param name="configureAdditionalAttributes">A delegate to configure the additional attributes.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		public ILicenseBuilder WithAdditionalAttributes(Action<LicenseAttributes> configureAdditionalAttributes)
		{
			configureAdditionalAttributes(m_License.AdditionalAttributes);
			return this;
		}

		/// <summary>
		/// Create and sign a new <see cref="License"/> with the specified
		/// private encryption key.
		/// </summary>
		/// <param name="privateKey">The private encryption key for the signature.</param>
		/// <param name="passPhrase">The pass phrase to decrypt the private key.</param>
		/// <returns>The signed <see cref="License"/>.</returns>
		public License CreateAndSignWithPrivateKey(string privateKey, string passPhrase)
		{
			m_License.Sign(privateKey, passPhrase);
			return m_License;
		}
	}
}

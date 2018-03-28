using System;
using System.Collections.Generic;

namespace ICD.Common.Licensing
{
	/// <summary>
	/// Fluent api to create and sign a new <see cref="License"/>. 
	/// </summary>
	public interface ILicenseBuilder : IFluentInterface
	{
		/// <summary>
		/// Sets the unique identifier of the <see cref="License"/>.
		/// </summary>
		/// <param name="id">The unique identifier of the <see cref="License"/>.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		ILicenseBuilder WithUniqueIdentifier(Guid id);

		/// <summary>
		/// Sets the <see cref="eLicenseType"/> of the <see cref="License"/>.
		/// </summary>
		/// <param name="type">The <see cref="eLicenseType"/> of the <see cref="License"/>.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		ILicenseBuilder As(eLicenseType type);

		/// <summary>
		/// Sets the expiration date of the <see cref="License"/>.
		/// </summary>
		/// <param name="date">The expiration date of the <see cref="License"/>.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		ILicenseBuilder ExpiresAt(DateTime date);

		/// <summary>
		/// Sets the maximum utilization of the <see cref="License"/>.
		/// This can be the quantity of developers for a "per-developer-license".
		/// </summary>
		/// <param name="utilization">The maximum utilization of the <see cref="License"/>.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		ILicenseBuilder WithMaximumUtilization(int utilization);

		/// <summary>
		/// Sets the <see cref="Customer">license holder</see> of the <see cref="License"/>.
		/// </summary>
		/// <param name="name">The name of the license holder.</param>
		/// <param name="email">The email of the license holder.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		ILicenseBuilder LicensedTo(string name, string email);

		/// <summary>
		/// Sets the <see cref="Customer">license holder</see> of the <see cref="License"/>.
		/// </summary>
		/// <param name="name">The name of the license holder.</param>
		/// <param name="email">The email of the license holder.</param>
		/// <param name="configureCustomer">A delegate to configure the license holder.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		ILicenseBuilder LicensedTo(string name, string email, Action<Customer> configureCustomer);

		/// <summary>
		/// Sets the <see cref="Customer">license holder</see> of the <see cref="License"/>.
		/// </summary>
		/// <param name="configureCustomer">A delegate to configure the license holder.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		ILicenseBuilder LicensedTo(Action<Customer> configureCustomer);

		/// <summary>
		/// Sets the licensed product features of the <see cref="License"/>.
		/// </summary>
		/// <param name="productFeatures">The licensed product features of the <see cref="License"/>.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		ILicenseBuilder WithProductFeatures(IDictionary<string, string> productFeatures);

		/// <summary>
		/// Sets the licensed product features of the <see cref="License"/>.
		/// </summary>
		/// <param name="configureProductFeatures">A delegate to configure the product features.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		ILicenseBuilder WithProductFeatures(Action<LicenseAttributes> configureProductFeatures);

		/// <summary>
		/// Sets the licensed additional attributes of the <see cref="License"/>.
		/// </summary>
		/// <param name="additionalAttributes">The additional attributes of the <see cref="License"/>.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		ILicenseBuilder WithAdditionalAttributes(IDictionary<string, string> additionalAttributes);

		/// <summary>
		/// Sets the licensed additional attributes of the <see cref="License"/>.
		/// </summary>
		/// <param name="configureAdditionalAttributes">A delegate to configure the additional attributes.</param>
		/// <returns>The <see cref="ILicenseBuilder"/>.</returns>
		ILicenseBuilder WithAdditionalAttributes(Action<LicenseAttributes> configureAdditionalAttributes);

		/// <summary>
		/// Create and sign a new <see cref="License"/> with the specified
		/// private encryption key.
		/// </summary>
		/// <param name="privateKey">The private encryption key for the signature.</param>
		/// <param name="passPhrase">The pass phrase to decrypt the private key.</param>
		/// <returns>The signed <see cref="License"/>.</returns>
		License CreateAndSignWithPrivateKey(string privateKey, string passPhrase);
	}
}

namespace ICD.Common.Licensing.Validation
{
	/// <summary>
	/// Represents a <see cref="License"/> expired failure of a <see cref="ILicenseValidator"/>.
	/// </summary>
	public sealed class LicenseExpiredValidationFailure : IValidationFailure
	{
		/// <summary>
		/// Gets or sets a message that describes the validation failure.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets a message that describes how to recover from the validation failure.
		/// </summary>
		public string HowToResolve { get; set; }
	}
}

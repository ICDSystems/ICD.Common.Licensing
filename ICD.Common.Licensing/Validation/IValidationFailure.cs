namespace ICD.Common.Licensing.Validation
{
	/// <summary>
	/// Represents a failure of a <see cref="ILicenseValidator"/>.
	/// </summary>
	public interface IValidationFailure
	{
		/// <summary>
		/// Gets or sets a message that describes the validation failure.
		/// </summary>
		string Message { get; set; }

		/// <summary>
		/// Gets or sets a message that describes how to recover from the validation failure.
		/// </summary>
		string HowToResolve { get; set; }
	}
}

namespace ICD.Common.Licensing.Validation
{
	/// <summary>
	/// Represents a failure when the <see cref="License.Signature"/> is invalid.
	/// </summary>
	public sealed class InvalidSignatureValidationFailure : IValidationFailure
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

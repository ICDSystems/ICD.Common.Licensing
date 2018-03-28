namespace ICD.Common.Licensing.Validation
{
	/// <summary>
	/// Represents a general validation failure.
	/// </summary>
	public sealed class GeneralValidationFailure : IValidationFailure
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

using System;

namespace ICD.Common.Licensing.Validation
{
	/// <summary>
	/// Represents a <see cref="License"/> validator.
	/// </summary>
	internal sealed class LicenseValidator : ILicenseValidator
	{
		/// <summary>
		/// Gets or sets the predicate to determine if the <see cref="License"/>
		/// is valid.
		/// </summary>
		public Predicate<License> Validate { get; set; }

		/// <summary>
		/// Gets or sets the predicate to determine if the <see cref="ILicenseValidator"/>
		/// should be executed.
		/// </summary>
		public Predicate<License> ValidateWhen { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="IValidationFailure"/> result. The <see cref="IValidationFailure"/>
		/// will be returned to the application when the <see cref="ILicenseValidator"/> fails.
		/// </summary>
		public IValidationFailure FailureResult { get; set; }
	}
}
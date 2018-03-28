using System;

namespace ICD.Common.Licensing.Validation
{
	/// <summary>
	/// Represents a <see cref="License"/> validator.
	/// </summary>
	public interface ILicenseValidator
	{
		/// <summary>
		/// Gets or sets the predicate to determine if the <see cref="License"/>
		/// is valid.
		/// </summary>
		Predicate<License> Validate { get; set; }

		/// <summary>
		/// Gets or sets the predicate to determine if the <see cref="ILicenseValidator"/>
		/// should be executed.
		/// </summary>
		Predicate<License> ValidateWhen { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="IValidationFailure"/> result. The <see cref="IValidationFailure"/>
		/// will be returned to the application when the <see cref="ILicenseValidator"/> fails.
		/// </summary>
		IValidationFailure FailureResult { get; set; }
	}
}
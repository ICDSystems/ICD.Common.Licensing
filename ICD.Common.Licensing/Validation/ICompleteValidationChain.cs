namespace ICD.Common.Licensing.Validation
{
	/// <summary>
	/// Interface for the fluent validation syntax.
	/// This interface is used to complete a validation chain.
	/// </summary>
	public interface ICompleteValidationChain : IAddAdditionalValidationChain, IAssertValidation
	{
	}
}

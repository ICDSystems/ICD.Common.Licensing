namespace ICD.Common.Licensing.Validation
{
	/// <summary>
	/// Interface for the fluent validation syntax.
	/// </summary>
	public interface IAddAdditionalValidationChain : IFluentInterface
	{
		/// <summary>
		/// Adds an additional validation chain.
		/// </summary>
		/// <returns>An instance of <see cref="IStartValidationChain"/>.</returns>
		IStartValidationChain And();
	}
}

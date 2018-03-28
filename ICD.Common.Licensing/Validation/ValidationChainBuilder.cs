using System;
using System.Collections.Generic;

namespace ICD.Common.Licensing.Validation
{
	internal sealed class ValidationChainBuilder : IStartValidationChain, IValidationChain
	{
		private readonly Queue<ILicenseValidator> m_Validators;
		private ILicenseValidator m_CurrentValidatorChain;
		private readonly License m_License;

		public ValidationChainBuilder(License license)
		{
			m_License = license;
			m_Validators = new Queue<ILicenseValidator>();
		}

		public ILicenseValidator StartValidatorChain()
		{
			return m_CurrentValidatorChain = new LicenseValidator();
		}

		public void CompleteValidatorChain()
		{
			if (m_CurrentValidatorChain == null)
				return;

			m_Validators.Enqueue(m_CurrentValidatorChain);
			m_CurrentValidatorChain = null;
		}

		public ICompleteValidationChain When(Predicate<License> predicate)
		{
			m_CurrentValidatorChain.ValidateWhen = predicate;
			return this;
		}

		public IStartValidationChain And()
		{
			CompleteValidatorChain();
			return this;
		}

		public IEnumerable<IValidationFailure> AssertValidLicense()
		{
			CompleteValidatorChain();

			while (m_Validators.Count > 0)
			{
				var validator = m_Validators.Dequeue();
				if (validator.ValidateWhen != null && !validator.ValidateWhen(m_License))
					continue;

				if (!validator.Validate(m_License))
					yield return validator.FailureResult ?? new GeneralValidationFailure {Message = "License validation failed!"};
			}
		}
	}
}

//
// Copyright © 2012 - 2013 Nauck IT KG     http://www.nauck-it.de
// Copyright © 2017 Nivloc Enterprises Ltd
// Copyright © 2018 ICD Systems
//
// Author:
//  Daniel Nauck        <d.nauck(at)nauck-it.de>
//  Neil Colvin
//  Chris Cameron
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

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

﻿//
// Copyright © 2012 - 2013 Nauck IT KG     http://www.nauck-it.de
// Copyright © 2017 Nivloc Enterprises Ltd
//
// Author:
//  Daniel Nauck        <d.nauck(at)nauck-it.de>
//  Neil Colvin
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
using System.Globalization;
#if SIMPLSHARP
using Crestron.SimplSharp.Reflection;

#endif

namespace Portable.Licensing
	{
	/// <summary>
	/// Defines assembly build date information for an assembly manifest.
	/// </summary>
	[AttributeUsage (AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyBuildDateAttribute : Attribute
		{
		private readonly DateTime buildDate;

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyBuildDateAttribute"/> class
		/// with the specified build date.
		/// </summary>
		/// <param name="buildDate">The build date of the assembly.</param>
		public AssemblyBuildDateAttribute (DateTime buildDate)
			{
			this.buildDate = buildDate;
			}

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyBuildDateAttribute"/> class
		/// with the specified build date string.
		/// </summary>
		/// <param name="buildDateString">The build date of the assembly.</param>
		public AssemblyBuildDateAttribute (string buildDateString)
			{
			buildDate = DateTime.Parse (buildDateString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
			}

		/// <summary>
		/// Gets the assembly build date.
		/// </summary>
		public DateTime BuildDate
			{
			get { return buildDate; }
			}
		}
	}

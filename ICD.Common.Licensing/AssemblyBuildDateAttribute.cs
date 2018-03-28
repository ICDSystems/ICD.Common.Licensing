using System;
using System.Globalization;

namespace ICD.Common.Licensing
{
	/// <summary>
	/// Defines assembly build date information for an assembly manifest.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyBuildDateAttribute : Attribute
	{
		private readonly DateTime m_BuildDate;

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyBuildDateAttribute"/> class
		/// with the specified build date.
		/// </summary>
		/// <param name="buildDate">The build date of the assembly.</param>
		public AssemblyBuildDateAttribute(DateTime buildDate)
		{
			m_BuildDate = buildDate;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyBuildDateAttribute"/> class
		/// with the specified build date string.
		/// </summary>
		/// <param name="buildDateString">The build date of the assembly.</param>
		public AssemblyBuildDateAttribute(string buildDateString)
		{
			m_BuildDate = DateTime.Parse(buildDateString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
		}

		/// <summary>
		/// Gets the assembly build date.
		/// </summary>
		public DateTime BuildDate { get { return m_BuildDate; } }
	}
}

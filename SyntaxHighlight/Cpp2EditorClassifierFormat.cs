using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Windows.Media;

namespace SyntaxHighlight
{
	/// <summary>
	/// Defines an editor format for the Cpp2EditorClassifier type that has a purple background
	/// and is underlined.
	/// </summary>
	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = "Cpp2EditorClassifier")]
	[Name("Cpp2EditorClassifier")]
	[UserVisible(true)] // This should be visible to the end user
	[Order(Before = Priority.Default)] // Set the priority to be after the default classifiers
	internal sealed class Cpp2EditorClassifierFormat : ClassificationFormatDefinition
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Cpp2EditorClassifierFormat"/> class.
		/// </summary>
		public Cpp2EditorClassifierFormat()
		{
			this.DisplayName = "Cpp2EditorClassifier"; // Human readable version of the name
		}
	}
}

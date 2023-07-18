using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace SyntaxHighlight
{
	/// <summary>
	/// Classification type definition export for Cpp2EditorClassifier
	/// </summary>
	internal static class Cpp2EditorClassifierClassificationDefinition
	{
		// This disables "The field is never used" compiler's warning. Justification: the field is used by MEF.
#pragma warning disable 169

		/// <summary>
		/// Defines the "Cpp2EditorClassifier" classification type.
		/// </summary>
		[Export(typeof(ClassificationTypeDefinition))]
		[Name("Cpp2EditorClassifier")]
		private static ClassificationTypeDefinition _typeDefinition;

#pragma warning restore 169
	}
}

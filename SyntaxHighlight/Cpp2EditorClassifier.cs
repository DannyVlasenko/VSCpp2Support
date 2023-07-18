using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Language.StandardClassification;

namespace SyntaxHighlight
{
	/// <summary>
	/// Classifier that classifies all text as an instance of the "Cpp2EditorClassifier" classification type.
	/// </summary>
	internal class Cpp2EditorClassifier : IClassifier
	{
		/// <summary>
		/// Classification type.
		/// </summary>
		private readonly IClassificationType _classificationType;

		private IStandardClassificationService _classifications;

		/// <summary>
		/// Initializes a new instance of the <see cref="Cpp2EditorClassifier"/> class.
		/// </summary>
		/// <param name="registry">Classification registry.</param>
		internal Cpp2EditorClassifier(IClassificationTypeRegistryService registry, IStandardClassificationService classifications)
		{
			_classificationType = registry.GetClassificationType("Cpp2EditorClassifier"); 
			_classifications = classifications;
		}

		#region IClassifier

#pragma warning disable 67

		/// <summary>
		/// An event that occurs when the classification of a span of text has changed.
		/// </summary>
		/// <remarks>
		/// This event gets raised if a non-text change would affect the classification in some way,
		/// for example typing /* would cause the classification to change in C# without directly
		/// affecting the span.
		/// </remarks>
		public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

#pragma warning restore 67

		/// <summary>
		/// Gets all the <see cref="ClassificationSpan"/> objects that intersect with the given range of text.
		/// </summary>
		/// <remarks>
		/// This method scans the given SnapshotSpan for potential matches for this classification.
		/// In this instance, it classifies everything and returns each span as a new ClassificationSpan.
		/// </remarks>
		/// <param name="span">The span currently being classified.</param>
		/// <returns>A list of ClassificationSpans that represent spans identified to be of this classification.</returns>
		public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
		{
			var result = new List<ClassificationSpan>()
			{
				new ClassificationSpan(new SnapshotSpan(span.Snapshot, new Span(span.Start, span.Length)), _classifications.Comment)
			};

			return result;
		}

		#endregion
	}
}

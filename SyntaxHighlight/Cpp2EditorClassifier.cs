using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using System;
using System.Collections.Generic;
using System.Linq;
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
			using (var parser = new SyntaxParser(span.Snapshot.GetText()))
			{
				var line = span.Start.GetContainingLineNumber();
				return parser.TokensForLine(1)
					.Where(token=>token.Line == line+1)
					.Select(token =>
						new ClassificationSpan(new SnapshotSpan(span.Snapshot, new Span(span.Start.Position + token.Column-1, token.Count)),
							LexemeToClassification(token.Lexeme)))
					.ToList();
			}
		}

		private IClassificationType LexemeToClassification(Lexeme lexeme)
		{
			switch (lexeme)
			{
				case Lexeme.SlashEq:
				case Lexeme.Slash:
				case Lexeme.LeftShiftEq:
				case Lexeme.LeftShift:
				case Lexeme.Spaceship:
				case Lexeme.LessEq:
				case Lexeme.Less:
				case Lexeme.RightShiftEq:
				case Lexeme.RightShift:
				case Lexeme.GreaterEq:
				case Lexeme.Greater:
				case Lexeme.PlusPlus:
				case Lexeme.PlusEq:
				case Lexeme.Plus:
				case Lexeme.MinusMinus:
				case Lexeme.MinusEq:
				case Lexeme.Arrow:
				case Lexeme.Minus:
				case Lexeme.LogicalOrEq:
				case Lexeme.LogicalOr:
				case Lexeme.PipeEq:
				case Lexeme.Pipe:
				case Lexeme.LogicalAndEq:
				case Lexeme.LogicalAnd:
				case Lexeme.MultiplyEq:
				case Lexeme.Multiply:
				case Lexeme.ModuloEq:
				case Lexeme.Modulo:
				case Lexeme.AmpersandEq:
				case Lexeme.Ampersand:
				case Lexeme.CaretEq:
				case Lexeme.Caret:
				case Lexeme.TildeEq:
				case Lexeme.Tilde:
				case Lexeme.EqualComparison:
				case Lexeme.Assignment:
				case Lexeme.NotEqualComparison:
				case Lexeme.Not:
				case Lexeme.LeftBrace:
				case Lexeme.RightBrace:
				case Lexeme.LeftParen:
				case Lexeme.RightParen:
				case Lexeme.LeftBracket:
				case Lexeme.RightBracket:
				case Lexeme.Scope:
				case Lexeme.Colon:
				case Lexeme.Semicolon:
				case Lexeme.Comma:
				case Lexeme.Dot:
				case Lexeme.Ellipsis:
				case Lexeme.QuestionMark:
				case Lexeme.At:
				case Lexeme.Dollar:
					return _classifications.Operator;
				case Lexeme.FloatLiteral:
				case Lexeme.BinaryLiteral:
				case Lexeme.DecimalLiteral:
				case Lexeme.HexadecimalLiteral:
					return _classifications.NumberLiteral;
				case Lexeme.StringLiteral:
					return _classifications.StringLiteral;
				case Lexeme.CharacterLiteral:
					return _classifications.CharacterLiteral;
				case Lexeme.UserDefinedLiteralSuffix:
					return _classifications.FormalLanguage;
				case Lexeme.Keyword:
				case Lexeme.Cpp1MultiKeyword:
					return _classifications.Keyword;
				case Lexeme.Cpp2FixedType:
					return _classifications.SymbolDefinition;
				case Lexeme.Identifier:
					return _classifications.Identifier;
				case Lexeme.None:
					return _classifications.WhiteSpace;
				default:
					throw new ArgumentOutOfRangeException(nameof(lexeme), lexeme, null);
			}
		}

		#endregion
	}
}

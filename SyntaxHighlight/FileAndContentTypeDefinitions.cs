using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace SyntaxHighlight
{
	internal static class FileAndContentTypeDefinitions
	{
		[Export]
		[Name("cpp2")]
		[BaseDefinition("code")]
		internal static ContentTypeDefinition Cpp2ContentTypeDefinition;

		[Export]
		[FileExtension(".cpp2")]
		[ContentType("cpp2")]
		internal static FileExtensionToContentTypeDefinition Cpp2FileExtensionDefinition;
	}
}

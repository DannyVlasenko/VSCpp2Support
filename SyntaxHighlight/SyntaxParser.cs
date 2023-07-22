using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxHighlight
{
	enum Lexeme : sbyte
	{
		SlashEq,
		Slash,
		LeftShiftEq,
		LeftShift,
		Spaceship,
		LessEq,
		Less,
		RightShiftEq,
		RightShift,
		GreaterEq,
		Greater,
		PlusPlus,
		PlusEq,
		Plus,
		MinusMinus,
		MinusEq,
		Arrow,
		Minus,
		LogicalOrEq,
		LogicalOr,
		PipeEq,
		Pipe,
		LogicalAndEq,
		LogicalAnd,
		MultiplyEq,
		Multiply,
		ModuloEq,
		Modulo,
		AmpersandEq,
		Ampersand,
		CaretEq,
		Caret,
		TildeEq,
		Tilde,
		EqualComparison,
		Assignment,
		NotEqualComparison,
		Not,
		LeftBrace,
		RightBrace,
		LeftParen,
		RightParen,
		LeftBracket,
		RightBracket,
		Scope,
		Colon,
		Semicolon,
		Comma,
		Dot,
		Ellipsis,
		QuestionMark,
		At,
		Dollar,
		FloatLiteral,
		BinaryLiteral,
		DecimalLiteral,
		HexadecimalLiteral,
		StringLiteral,
		CharacterLiteral,
		UserDefinedLiteralSuffix,
		Keyword,
		Cpp1MultiKeyword,
		Cpp2FixedType,
		Identifier,
		None = 127
	}

	struct Token
	{
		public int Line;
		public int Column;
		public int Count;
		public Lexeme Lexeme;
	};

	internal class SyntaxParser : IDisposable
	{
		private IntPtr _parser;

		internal SyntaxParser(string text)
		{
			_parser = parse_source(text, text.Length);
			if (_parser == IntPtr.Zero)
			{
				throw new Exception("Cannot create Cpp2 syntax parser.");
			}
		}

		private void ReleaseUnmanagedResources()
		{
			delete_parser(_parser);
			_parser = IntPtr.Zero;
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}

		public List<Token> TokensForLine(int line)
		{
			Token[] tokens = new Token[count_tokens_for_line(_parser, line).ToUInt64()];
			var count = tokens_for_line(_parser, line, tokens, (UIntPtr)tokens.Length).ToUInt32();
			if (count != tokens.Length)
			{
				throw new Exception("Broken syntax parser: token count mismatch.");
			}
			return new List<Token>(tokens);
		}

		~SyntaxParser()
		{
			ReleaseUnmanagedResources();
		}

		[DllImport("CppFrontLib.dll")]
		private static extern IntPtr parse_source(string text, int len);

		[DllImport("CppFrontLib.dll")]
		private static extern void delete_parser(IntPtr parser);

		[DllImport("CppFrontLib.dll")]
		private static extern UIntPtr tokens_for_line(IntPtr parser, int line, [Out]Token[] tokens, UIntPtr token_buf_size);

		[DllImport("CppFrontLib.dll")] 
		private static extern UIntPtr count_tokens_for_line(IntPtr parser, int line);
	}
}

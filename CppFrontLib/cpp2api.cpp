#include "cppfront/source/lex.h"

namespace cpp2 { auto cmdline_processor::print(std::string_view s, int width) -> void {} }

namespace 
{
struct syntax_highlight_parser{
    std::vector<cpp2::error_entry> errors;
	cpp2::source src{errors};
	cpp2::tokens tokens{errors};
};

struct token
{
	int32_t line;
	int32_t column;
	int32_t count;
	cpp2::lexeme lexeme;
};

struct membuf: std::streambuf {
    membuf(char const* base, size_t size) {
        char* p(const_cast<char*>(base));
        this->setg(p, p, p + size);
    }
};

struct imemstream: virtual membuf, std::istream {
    imemstream(char const* base, size_t size)
        : membuf(base, size)
        , std::istream(static_cast<std::streambuf*>(this)) {
    }
};

}

extern "C"
{
__declspec(dllexport)
syntax_highlight_parser* parse_source(const char *text, size_t len)
{
	auto* parser = new syntax_highlight_parser;
	imemstream src_stream{text, len};
	if (!parser->src.load(src_stream))
	{
		delete parser;
		return nullptr;
	}
	parser->tokens.lex(parser->src.get_lines());
	return parser;
}

__declspec(dllexport)
void delete_parser(syntax_highlight_parser* parser)
{
	delete parser;
}

__declspec(dllexport)
size_t count_tokens_for_line(syntax_highlight_parser* parser, int32_t line)
{
	try {
		return parser->tokens.get_map().at(line).size();
	} catch(...)
	{
		return 0;
	}
}

__declspec(dllexport)
size_t tokens_for_line(syntax_highlight_parser* parser, int32_t line, token out_tokens_buf[], size_t token_buf_size)
{
	try {
		const auto tokens = parser->tokens.get_map().at(line);
		size_t count = 0;
		for(;count < token_buf_size && count < tokens.size(); ++count)
		{
			out_tokens_buf[count] = 
				token{
					.line = tokens[count].position().lineno,
					.column = tokens[count].position().colno,
					.count = tokens[count].length(),
					.lexeme = tokens[count].type() };
		}
		return count;
	} catch(...)
	{
		return 0;
	}
}

}
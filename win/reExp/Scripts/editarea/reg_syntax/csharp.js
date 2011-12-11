/* This syntax highlighting file is automagically generated from
	geshi language definition file by _fr_geshiLangToEditAreaLang.php generator.
	Copyright (c) 2009 by Vladislav "FractalizeR" Rastrusny.
	 */
//And Modified by Tim Speed on March, 24, 2010
	 
    editAreaLoader.load_syntax["csharp"] = {
    'COMMENT_SINGLE' : {1: "//"}, 
    'COMMENT_MULTI' : {"/*": "*/"},
    'QUOTEMARKS': { 0: "\'", 1: "\"" }, 
    'KEYWORD_CASE_SENSITIVE' : true,
    'KEYWORDS' : {
        'keywordgroup1': ["var", "as", "auto", "base", "break", "case", "catch", "const", "continue", "default", "do", "else", "event", "explicit", "extern", "false", "finally", "fixed", "for", "foreach", "goto", "if", "implicit", "in", "internal", "lock", "namespace", "null", "operator", "out", "override", "params", "partial", "private", "protected", "public", "readonly", "ref", "return", "sealed", "stackalloc", "static", "switch", "this", "throw", "true", "try", "unsafe", "using", "virtual", "void", "while"],
        'keywordgroup2': ["#elif", "#endif", "#endregion", "#else", "#error", "#define", "#if", "#line", "#region", "#undef", "#warning"],
        'keywordgroup3': ["checked", "is", "new", "sizeof", "typeof", "unchecked", "from", "select", "class", "interface", "struct"],
        'keywordgroup4': ["bool", "byte", "char", "decimal", "delegate", "double", "enum", "float", "int", "long", "object", "sbyte", "short", "string", "uint", "ulong", "ushort"]
    },
    'OPERATORS' : ["+", "-", "*", "?", "=", "/", "%", "&", ">", "<", "^", "!", ":", ";", "|"], 
    'DELIMITERS' : [ '(', ')', '[', ']', '{', '}' ], 
    'STYLES' : { 
        'COMMENTS' : 'color: #379F4B;',
        'QUOTESMARKS' : 'color: #CC0000;',
        'KEYWORDS' : { 
            'keywordgroup1': 'color: #0000FF;',
            'keywordgroup2': 'color: #0000FF;',
            'keywordgroup3': 'color: #0000FF;',
            'keywordgroup4': 'color: #0000FF;'
        },
        'OPERATORS' : 'color: #000000;',
        'DELIMITERS' : 'color: #000000;'
    } 
}; 

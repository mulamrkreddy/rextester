editAreaLoader.load_syntax["vb1"] = {
    'DISPLAY_NAME': 'Visual Basic'
	, 'COMMENT_SINGLE': { 1: "'" }
	, 'COMMENT_MULTI': {}
	, 'QUOTEMARKS': { 1: '"' }
	, 'KEYWORD_CASE_SENSITIVE': false
	, 'KEYWORDS': {
	    'statements': [
	        'if', 'then', 'for each', 'for', 'while', 'do', 'loop',
            'else', 'elseif', 'select', 'case', 'end select', 'with', 'end with',
            'until', 'next', 'step', 'to', 'in', 'end if', 'from', 'using', 'end using'
		]
		, 'keywords': [
            'empty', 'isempty', 'nothing', 'null', 'isnull', 'true', 'false',
            'set', 'call', 'imports', 'inherits', 'implements',
            'sub', 'end sub', 'function', 'end function', 'exit', 'exit function',
            'dim', 'Mod', 'In', 'private', 'public', 'protected', 'overrides', 'shared', 'const', 'from',
            'module', 'end module', 'class', 'end class', 'me', 'as', 'new', 'mybase', 'myclass',
            'namespace', 'end namespace', 'structure', 'end structure'
        ]

		, 'functions': [
			'CDate', 'Date', 'DateAdd', 'DateDiff', 'DatePart', 'DateSerial', 'DateValue', 'Day', 'FormatDateTime',
            'Hour', 'IsDate', 'Minute', 'Month',
            'MonthName', 'Now', 'Second', 'Time', 'Timer', 'TimeSerial', 'TimeValue', 'Weekday', 'WeekdayName ', 'Year',
            'Asc', 'CBool', 'CByte', 'CCur', 'CDate', 'CDbl', 'Chr', 'CInt', 'CLng', 'CSng', 'CStr', 'Hex', 'Oct', 'FormatCurrency',
            'FormatDateTime', 'FormatNumber', 'FormatPercent', 'Abs', 'Atn', 'Cos', 'Exp', 'Hex', 'Int', 'Fix', 'Log', 'Oct',
            'Rnd', 'Sgn', 'Sin', 'Sqr', 'Tan',
            'Array', 'Filter', 'IsArray', 'Join', 'LBound', 'Split', 'UBound',
            'InStr', 'InStrRev', 'LCase', 'Left', 'Len', 'LTrim', 'RTrim', 'Trim', 'Mid', 'Replace', 'Right', 'Space', 'StrComp',
            'String', 'StrReverse', 'UCase',
            'CreateObject', 'Eval', 'GetLocale', 'GetObject', 'GetRef', 'InputBox', 'IsEmpty', 'IsNull', 'IsNumeric',
            'IsObject', 'LoadPicture', 'MsgBox', 'RGB', 'Round', 'ScriptEngine', 'ScriptEngineBuildVersion', 'ScriptEngineMajorVersion',
            'ScriptEngineMinorVersion', 'SetLocale', 'TypeName', 'VarType'
		]
	}
	, 'OPERATORS': [
		'+', '-', '/', '*', '=', '<', '>', '!', '&'
	]
	, 'DELIMITERS': [
		'(', ')', '[', ']', '{', '}'
	]
	, 'STYLES': {
	    'COMMENTS': 'color: #379F4B;'
		, 'QUOTESMARKS': 'color: #CC0000;'
		, 'KEYWORDS': {
		    'keywords': 'color: #0000FF;'
			, 'functions': 'color: #0000FF;'
			, 'statements': 'color: #0000FF;'
		}
		, 'OPERATORS': 'color: #000000;'
		, 'DELIMITERS': 'color: #000000;'

	}
};

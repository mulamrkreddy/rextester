using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Threading;

namespace reExp.Controllers.regex
{
    public class RegexData
    {
        public bool IsReplace
        {
            get;
            set;
        }
        public bool IsResult
        {
            get;
            set;
        }
        public string Result
        {
            get;
            set;
        }
        public string Pattern
        {
            get;
            set;
        }
        public string Text
        {
            get;
            set;
        }
        public string Substitution
        {
            get;
            set;
        }
        List<bool> _options = new List<bool> { false, false, false, false, false, false, false, false };
        public List<bool> Options
        {
            get
            {
                return _options;
            }
            set
            {
                _options = value;
            }
        }
        public List<OptionDescribtion> Describtions
        {
            get;
            set;
        }
        public bool ShouldSave
        {
            get;
            set;
        }
        public string SavedUrl
        {
            get;
            set;
        }
        public string SavedOutput
        {
            get;
            set;
        }
    }

    public class OptionDescribtion
    {
        public string Title
        {
            get;
            set;
        }
        public string Comment
        {
            get;
            set;
        }
        public RegexOptions RegexOption
        {
            get;
            set;
        }
        public List<OptionDescribtion> GetDescribtions()
        {
            return new List<OptionDescribtion>()
            {
                new OptionDescribtion(){Title = "CultureInvariant", Comment = "Culture of the server is " + Thread.CurrentThread.CurrentCulture.ToString() + ". Mark this option to use InvariantCulture instead."/*+ ". Specifies that cultural differences in language is ignored. See Performing Culture-Insensitive Operations in the RegularExpressions Namespace for more information."*/, RegexOption = RegexOptions.CultureInvariant }, 
                new OptionDescribtion(){Title = "IgnoreCase", Comment = "Specifies case-insensitive matching.", RegexOption = RegexOptions.IgnoreCase},
                new OptionDescribtion(){Title = "Multiline", Comment = "Multiline mode. Changes the meaning of ^ and $ so they match at the beginning and end, respectively, of any line, and not just the beginning and end of the entire string.", RegexOption = RegexOptions.Multiline},
                new OptionDescribtion(){Title = "Singleline", Comment = "Specifies single-line mode. Changes the meaning of the dot (.) so it matches every character (instead of every character except \\n).", RegexOption = RegexOptions.Singleline},
                new OptionDescribtion(){Title = "RightToLeft", Comment = "Specifies that the search will be from right to left instead of from left to right.", RegexOption = RegexOptions.RightToLeft},
                new OptionDescribtion(){Title = "IgnorePatternWhitespace", Comment = "Eliminates unescaped white space from the pattern and enables comments marked with #. However, the IgnorePatternWhitespace value does not affect or eliminate white space in character classes. ", RegexOption = RegexOptions.IgnorePatternWhitespace},
                new OptionDescribtion(){Title = "ExplicitCapture", Comment = "Specifies that the only valid captures are explicitly named or numbered groups of the form (?<name>…). This allows unnamed parentheses to act as noncapturing groups without the syntactic clumsiness of the expression (?:…).", RegexOption = RegexOptions.ExplicitCapture},
                new OptionDescribtion(){Title = "ECMAScript", Comment = "Enables ECMAScript-compliant behavior for the expression. This value can be used only in conjunction with the IgnoreCase and Multiline values. The use of this value with any other values results in an exception.", RegexOption = RegexOptions.ECMAScript},
            };
        }
    }

}
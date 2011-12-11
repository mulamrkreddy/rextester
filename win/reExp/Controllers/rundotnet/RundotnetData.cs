using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reExp.Utils;

namespace reExp.Controllers.rundotnet
{

    public class RundotnetData
    {
        public string LanguageChoice
        {
            get;
            set;
        }
        public string EditorChoice
        {
            get;
            set;
        }
        public List<SelectListItem> Editor1
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Text = "CodeMirror",
                        Value = "1"
                    },
                    new SelectListItem()
                    {
                        Text = "EditArea  ",
                        Value = "2"
                    },
                    new SelectListItem()
                    {
                        Text = "Simple    ",
                        Value = "3"
                    }
                };
            }
        }
        public List<SelectListItem> Editor2
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Text = "EditArea",
                        Value = "2"
                    },
                    new SelectListItem()
                    {
                        Text = "Simple",
                        Value = "3"
                    }
                };
            }
        }
        public bool ShowCodeMirror
        {
            get
            {
                return this.LanguageChoice == "1" || this.LanguageChoice == "4" || this.LanguageChoice == "5" || this.LanguageChoice == "6" || this.LanguageChoice == "7" ? true : false;
            }
        }
        public bool IsInterpreted
        {
            get
            {
                return this.LanguageChoice == "5" ? true : false;
            }
        }
        public List<SelectListItem> Languages
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Text = "C#",
                        Value = "1"
                    },
                    new SelectListItem()
                    {
                        Text = "Visual Basic",
                        Value = "2"
                    },
                    new SelectListItem()
                    {
                        Text = "F#",
                        Value = "3"
                    },
                     new SelectListItem()
                    {
                        Text = "Java",
                        Value = "4"
                    },
                    new SelectListItem()
                    {
                        Text = "Python",
                        Value = "5"
                    },
                    new SelectListItem()
                    {
                        Text = "C",
                        Value = "6"
                    },
                    new SelectListItem()
                    {
                        Text = "C++",
                        Value = "7"
                    }
                };
            }
        }
        public string Program
        {
            get;
            set;
        }
        public List<string> Errors
        {
            get;
            set;
        }

        public string WholeError
        {
            get;
            set;
        }

        public bool ShowWarnings
        {
            get;
            set;
        }

        public List<string> Warnings
        {
            get;
            set;
        }

        public string WholeWarning
        {
            get;
            set;
        }

        public GlobalConst.RundotnetStatus Status
        {
            get;
            set;
        }

        public string Output
        {
            get;
            set;
        }

        public string GetInitialCode(string language, string editor)
        {
            switch (language)
            {
                case "1":
                    return
@"//C# 'Hello, world'
//Rextester.Program.Main is the entry point for your code. Don't change it.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rextester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Your code goes here
            Console.WriteLine(""Hello, world!"");
        }
    }
}";

                case "2":
                    return
@"'VB 'Hello, world'
'Rextester.Program.Main is the entry point for your code. Don't change it.

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text.RegularExpressions

Namespace Rextester
    Public Module Program
        Public Sub Main(args() As string)
            'Your code goes here
            Console.WriteLine(""Hello, world!"")
        End Sub
    End Module
End Namespace";
                case "3":
                    return
@"//F# 'Hello, world'
//Rextester.Program.Main is the entry point for your code. Don't change it.

namespace Rextester
module Program =
    open System
    let Main(args : string[]) =
        //Your code goes here
        printfn ""Hello, world!""";
                case "4":
                    return
@"//Java 'Hello, world'
//Given 'Rextester' class with default access modifier 
//together with given 'main' method is the entry point for your code. Don't change it.

class Rextester
{  
    public static void main(String args[])
    {
        System.out.println(""Hello, World!"");
    }
}";
                case "5":
                    return
@"#Python (2.7) 'Hello, world'

print ""Hello, world!""
";
                case "6":
                    return
@"//C 'Hello, world'

#include  <stdio.h>

int main(void)
{
    printf(""Hello, world!\n"");
    return 0;
}";
                case "7":
                    return
@"//C++ 'Hello, world!'

#include <iostream>
using namespace std;

int main()
{
    cout << ""Hello, world!\n"";
}";
                default:
                    return @"";
            }
        }

        public string RunStats
        {
            get;
            set;
        }
        public string CodeGuid
        {
            get;
            set;
        }
        public string SavedOutput
        {
            get;
            set;
        }
        public string StatsToSave
        {
            get;
            set;
        }
    }
}
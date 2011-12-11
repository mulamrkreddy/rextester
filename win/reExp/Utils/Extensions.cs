using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reExp.Utils
{
    public static class MyExtensions
    {
        public static string ToLanguage(this int number)
        {
            switch (number)
            { 
                case 1:
                    return "C#";
                case 2:
                    return "VB";
                case 3:
                    return "F#";
                case 4:
                    return "Java";
                case 5:
                    return "Python";
                case 6:
                    return "C";
                case 7:
                    return "C++";
                default:
                    return "Unknown";
            }
        }

        public static string ToEditor(this int number)
        {
            switch (number)
            {
                case 1:
                    return "Fancy";
                case 2:
                    return "Simple";
                default:
                    return "Unknown";
            }
        }

        public static GlobalConst.RundotnetStatus ToRundotnetStatus(this int number)
        {
            switch (number)
            { 
                case 0:
                    return GlobalConst.RundotnetStatus.Error;
                case 1:
                    return GlobalConst.RundotnetStatus.OK;
                case 2:
                    return GlobalConst.RundotnetStatus.Unknown;
                default:
                    return GlobalConst.RundotnetStatus.Unknown;
            }
        }

        public static string FirstLine(this string text)
        {
            if (string.IsNullOrEmpty(text) || !text.Contains("\n"))
                return text;

            string[] lines = text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            return lines[0];
        }
    } 
}
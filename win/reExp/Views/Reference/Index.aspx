<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Regex reference
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>regex reference</h2>
    Here is a short and general regular expressions reference found <a href="http://networking.ringofsaturn.com/Web/regex.php">
        here</a>.<br/><br/>
    <table style="width: 70%" align="center" border="1" cellpadding="5" cellspacing="1">
        <tbody>        
            <tr valign="top">
                <td>
                    <b>Character</b>
                </td>
                <td>
                    <b>Definition</b>
                </td>
                <td>
                    <b>Example</b>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <p align="center">
                        <span>Most characters</span></p>
                </td>
                <td>
                    Characters other than&nbsp;&nbsp;. $ ^ { [ ( | ) * + ? \&nbsp;&nbsp;match themselves. If you need to match these special characters - precede them by \
                </td>
                <td>
                    <span>abc matches abc<br/>\? matches ?</span>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <p align="center">
                        <span>^</span></p>
                </td>
                <td>
                    The pattern has to appear at the beginning of a string.
                </td>
                <td>
                    <span>^cat</span> matches any string that begins with <span>cat</span>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <p align="center">
                        <span>$</span></p>
                </td>
                <td>
                    The pattern has to appear at the end of a string.
                </td>
                <td>
                    <span>cat$</span> matches any string that ends with <span>cat</span>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <p align="center">
                        <span>.</span></p>
                </td>
                <td>
                    Matches any character.
                </td>
                <td>
                    <span>cat.</span> matches <span>catT</span> and <span>cat2</span> but not <span>catty</span>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <p align="center">
                        <span>[]</span></p>
                </td>
                <td>
                    Bracket expression. Matches one of any characters enclosed.
                </td>
                <td>
                    <span>gr[ae]y</span> matches <span>gray</span> or <span>grey</span>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <p align="center">
                        <span>[^]</span></p>
                </td>
                <td>
                    Negates a bracket expression. Matches one of any characters EXCEPT those enclosed.
                </td>
                <td>
                    <span>1[^02]</span> matches <span>13</span> but not <span>10</span> or <span>12</span>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <p align="center">
                        <span>[-]</span></p>
                </td>
                <td>
                    Range. Matches any characters within the range.
                </td>
                <td>
                    <span>[1-9]</span> matches any single digit EXCEPT <span>0</span>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <p align="center">
                        <span>?</span></p>
                </td>
                <td>
                    Preceeding item must match one or zero times.
                </td>
                <td>
                    <span>colou?r</span> matches <span>color</span> or <span>colour</span> but not <span>
                        colouur</span>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <p align="center">
                        <span>+</span></p>
                </td>
                <td>
                    Preceeding item must match one or more times.
                </td>
                <td>
                    <span>be+</span> matches <span>be</span> or <span>bee</span> but not <span>b</span>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <p align="center">
                        <span>*</span></p>
                </td>
                <td>
                    Preceeding item must match zero or more times.
                </td>
                <td>
                    <span>be*</span> matches <span>b</span> or <span>be</span> or <span>beeeeeeeeee</span>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <p align="center">
                        <span>()</span></p>
                </td>
                <td>
                    Parentheses. Creates a substring or item that metacharacters can be applied to
                </td>
                <td>
                    <span>a(bee)?t</span> matches <span>at</span> or <span>abeet</span> but not <span>abet</span>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <p align="center">
                        <span>{n}</span></p>
                </td>
                <td>
                    Bound. Specifies exact number of times for the preceeding item to match.
                </td>
                <td>
                    <span>[0-9]{3}</span> matches any three digits
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <p align="center">
                        <span>{n,}</span></p>
                </td>
                <td>
                    Bound. Specifies minimum number of times for the preceeding item to match.
                </td>
                <td>
                    <span>[0-9]{3,}</span> matches any three or more digits
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <p align="center">
                        <span>{n,m}</span></p>
                </td>
                <td>
                    Bound. Specifies minimum and maximum number of times for the preceeding item to
                    match.
                </td>
                <td>
                    <span>[0-9]{3,5}</span> matches any three, four, or five digits
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <p align="center">
                        <span>|</span></p>
                </td>
                <td>
                    Alternation. One of the alternatives has to match.
                </td>
                <td>
                    <span>July (first|1st|1)</span> will match <span>July 1st</span> but not <span>July
                        2</span>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <p align="center">
                        <span>\n</span></p>
                </td>
                <td>
                    The newline character. (ASCII 10)
                </td>
                <td>
                    <span>\n matches a newline</span>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <p align="center">
                        <span>\s</span></p>
                </td>
                <td>
                    A single whitespace character.
                </td>
                <td>
                    <span>a\sb matches a b but not ab</span>
                </td>
            </tr>
             <tr valign="top">
                <td>
                    <p align="center">
                        <span>\d</span></p>
                </td>
                <td>
                    A single digit character.
                </td>
                <td>
                    <span>a\db matches a2b but not acb</span>
                </td>
            </tr>
        </tbody>
    </table>
    <br/>For more complete and .net-specific reference see, for example, <a href="http://regexhero.net/reference/">Regex hero's reference</a>.
    <br/>For more complex but also even more complete reference - see the primary source - <a href="http://msdn.microsoft.com/en-us/library/az24scfc.aspx">Regular Expression Language Elements</a>.
    <br/>For information about regular expressions in general, their theoretical meaning - read <a href="http://en.wikipedia.org/wiki/Regular_expression">Wikipedia</a>!
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
    <meta name="Keywords" content="regex reference" />
    <meta name="Description" content="short regex reference" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

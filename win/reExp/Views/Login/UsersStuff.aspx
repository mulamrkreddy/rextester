<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.login.UserData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	User's stuff
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%int maxDisplayLength = 50; %>
    <%if (!Model.IsError)
      { %>
        <h2><%:Model.UserName%>'s stuff</h2>
        <div align="center">Code:</div>
        <%if (Model.Snippets.Count > 0)
          {%>            
            <table style="width: 70%" align="center" border="1" cellpadding="5" cellspacing="1">
                <thead>
                    <tr>
                        <th>
                            Date (utc)
                        </th>
                         <th>
                            Language
                        </th>
                         <th>
                            First line
                        </th>
                         <th>
                            Status
                        </th>
                    </tr>
                </thead>
                <tbody>
                <%foreach (var snippet in Model.Snippets.OrderByDescending(f => f.Date))
                    {%>
                    <tr>
                        <td style="width:10em;">
                            <a href="../<%:snippet.Guid%>"><%:snippet.Date.ToString(System.Globalization.CultureInfo.InvariantCulture)%></a>
                        </td>
                        <td>
                            <%:snippet.Lang.ToLanguage()%>
                        </td>
                        <td>
                            <%var firstLine = snippet.Program.FirstLine();%>
                            <%:firstLine != null && firstLine.Length > maxDisplayLength ? firstLine.Substring(0, maxDisplayLength) + "..." : firstLine%>
                        </td>
                        <td>
                            <%if (snippet.Status == GlobalConst.RundotnetStatus.Error)
                              { %>
                                <span style="color:red">Error</span>
                            <%}
                              else if (snippet.Status == GlobalConst.RundotnetStatus.OK)
                              {%>
                                <span style="color:green">OK</span>
                              <%}
                              else
                              { %>
                                Unknown
                              <%} %>
                        </td>                        
                    </tr> 
                <%} %>
                </tbody>
            </table>
        <%}
          else
          { %>
            <div align="center">None.</div>
        <%} %>

        <br/><br/>
        <div align="center">Regexes:</div>
        <%if (Model.Regexes.Count > 0)
          {%>
           
            <table style="width: 70%" align="center" border="1" cellpadding="5" cellspacing="1">
                <thead>
                    <tr>
                        <th>
                            Date (utc)
                        </th>
                         <th>
                            Regex
                        </th>
                         <th>
                            Text
                        </th>
                         <%--<th>
                            Output
                        </th>--%>
                    </tr>
                </thead>
                <tbody>
                <%foreach (var regex in Model.Regexes.OrderByDescending(f => f.Date))
                  {%>
                   <tr>
                        <td style="width:10em;">
                            <a href="../tester?code=<%:regex.Guid%>"><%:regex.Date.ToString(System.Globalization.CultureInfo.InvariantCulture)%></a>
                        </td>
                        <td>
                            <%:regex.Pattern != null && regex.Pattern.Length > maxDisplayLength ? regex.Pattern.Substring(0, maxDisplayLength) + "..." : regex.Pattern%>
                        </td>
                        <td>
                            <%:regex.Text != null && regex.Text.Length > maxDisplayLength ? regex.Text.Substring(0, maxDisplayLength) + "..." : regex.Text%>
                        </td>
                        <%--<td>
                            <%:regex.Output != null && regex.Output.Length > maxDisplayLength ? regex.Output.Substring(0, maxDisplayLength) + "..." : regex.Output%>
                        </td>--%>
                   </tr> 
                <%} %>
                </tbody>
            </table>
        <%}
          else
          { %>
            <div align="center">None.</div>
          <%} %>

        <br/><br/>
        <div align="center">Regex replacements:</div>
         <%if (Model.Replaces.Count > 0)
           {%>
            <table style="width: 70%" align="center" border="1" cellpadding="5" cellspacing="1">
                <thead>
                    <tr>
                        <th>
                            Date (utc)
                        </th>
                         <th>
                            Regex
                        </th>
                         <th>
                            Replacement
                        </th>
                        <th>
                            Text
                        </th>
                         <%--<th>
                            Output
                        </th>--%>
                    </tr>
                </thead>
                <tbody>
                <%foreach (var replace in Model.Replaces.OrderByDescending(f => f.Date))
                  {%>
                   <tr>
                        <td style="width:10em;">
                            <a href="../replace?code=<%:replace.Guid%>"><%:replace.Date.ToString(System.Globalization.CultureInfo.InvariantCulture)%></a>
                        </td>
                        <td>
                            <%:replace.Pattern != null && replace.Pattern.Length > maxDisplayLength ? replace.Pattern.Substring(0, maxDisplayLength) + "..." : replace.Pattern%>
                        </td>
                        <td>
                            <%:replace.Replacement != null && replace.Replacement.Length > maxDisplayLength ? replace.Replacement.Substring(0, maxDisplayLength) + "..." : replace.Replacement%>
                        </td>
                        <td>
                            <%:replace.Text != null && replace.Text.Length > maxDisplayLength ? replace.Text.Substring(0, maxDisplayLength) + "..." : replace.Text%>
                        </td>
                        <%--<td>
                            <%:replace.Output != null && replace.Output.Length > maxDisplayLength ? replace.Output.Substring(0, maxDisplayLength) + "..." : replace.Output%>
                        </td>--%>
                   </tr> 
                <%} %>
                </tbody>
            </table>
        <%}
           else
           { %>
            <div align="center">None.</div>
        <%} %>
    <%}
      else
      { %>
        <br/><br/><br/>
        <b><%:Model.Error%></b>
      <%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

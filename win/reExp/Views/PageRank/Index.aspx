<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.google.GoogleData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Page position at Google
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>What is your page's position at Google?</h2>
    <% using (Html.BeginForm())
       {%>
       <div class="formcontent">
            <div>
                Query:<br/>
                <%: Html.TextBoxFor(model => model.Query, new { size = 70, spellcheck = "false" })%>
            </div>
            <div style = "margin-top: 5px;">
                Your page address (no "http://www."):<br/>
                <%: Html.TextBoxFor(model => model.SearchFor, new { size = 70, spellcheck = "false" })%>
            </div>
            <input type="submit" value="Check" style="margin-top: 5px;" />
        </div>
    <% } %>
    <pre class="resultarea" id="result"><%
        if (Model.isResult)
        {      
            if(Model.Result.Count == 0)
            {
                %>No occurrences within first 1000 entries.<%
            }
            else
                foreach (KeyValuePair<int, string> pair in Model.Result)
                {
                    if (pair.Key != -1)
                    {
                        %>Found at approximately<b> <%: pair.Key%> </b> position - <%: pair.Value%><br/><%
                    }
                    else
                    {
                        %><span style="color: red"><%: pair.Value%></span><br/><%
                    }
                }
        }
    %></pre>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
    <meta name="Keywords" content="page position at Google" />
    <meta name="Description" content="Determine your page's position at google." />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
<%--    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.5.2/jquery.min.js" type="text/javascript">        
    </script> --%>
    <script type="text/javascript">
    // <![CDATA[
            $(document).ready(function () {
                $("#Query").focus();
            });
    // ]]>
    </script>
</asp:Content>

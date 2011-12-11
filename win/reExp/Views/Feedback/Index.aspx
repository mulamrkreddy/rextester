<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.feedback.Feedback>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Feedback
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
    <meta name="Keywords" content="rextester feedback" />
    <meta name="Description" content="rextester feedback." />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
<%--    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.5.2/jquery.min.js" type="text/javascript">        
    </script> --%>
    <script type="text/javascript">
    // <![CDATA[
            $(document).ready(function () {
                $("#Message").focus();
            });
    // ]]>
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <% if (Model.IsResult && Model.Succeeded)
       { %>
         <h2>Thank you for your feedback!</h2>
    <%}
       else
       {%>
        <h2>Feedback</h2>
        <%using (Html.BeginForm())
        {%>
            <div class="formcontent">
                <table>
                    <tr>
                        <td>
                            <%: Html.TextAreaFor(model => model.Message, 10, 60, null)%>
                        </td>
                    </tr>
                   <%-- <tr>
                        <td>
                            Please enter anything that matches given regular expression<br/>
                            (this should prevent automated posting):<br/>
                            <pre><%:Model.CaptchaRegex%>  <input id="UserAnswer" name="UserAnswer" type="text" value="<%:Model.UserAnswer%>" />  <a href="<%:Utils.GetUrl(Utils.PagesEnum.Reference)%>">Having trouble?</a></pre>
                        </td>
                    </tr>--%>
                    <tr align="right">
                        <td>
                            <input id="Button" type="submit" value="Submit"/>
                        </td>
                    </tr>
                </table>
                <%--<input id="InfoString" name="InfoString" type="hidden" value="<%:Model.InfoString %>" />           
                <input id="CaptchaRegex" name="CaptchaRegex" type="hidden" value="<%: Model.CaptchaRegex %>" />--%>
            </div>
       <%}%>
       <%if (Model.IsResult)
        {%>
            <pre class="resultarea" id="Result"><span style="color:red">Error:</span><br/><%:Model.ErrorMessage%></pre>
        <%} %>
       <%} %>

</asp:Content>

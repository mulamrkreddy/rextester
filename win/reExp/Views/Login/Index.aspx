<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.login.LoginData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Login or register
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Login or register</h2>
    <% using (Html.BeginForm())
       {%>
       <div class="formcontent">
           <table>
               <tr>
                    <td>
                        Login:<br/>
                        <%:Html.TextBoxFor(f => f.Name, new { spellcheck = "false" })%><br/>
                        <%:Html.PasswordFor(f => f.Password, new { style = "margin-top:5px;"})%><br/>
                        <input id="Button" type="submit" value="Login"/>
                    </td>
                    <td>
                        <div style="padding-left:5em; padding-right:5em;">Or</div>
                    </td>
                    <td>
                         Register:<br/>
                        <%:Html.TextBoxFor(f => f.RegName, new { spellcheck = "false" })%><br/>
                        <%:Html.PasswordFor(f => f.RegPassword, new { style = "margin-top:5px;"})%><br/>
                        <input id="Submit1" type="submit" value="Register"/>
                    </td>
               </tr>
           </table>   
        </div>    
    <%} %>
    <pre class="resultarea" id="Result"><%
        if (Model.IsError)
        { 
            %><span style="color:red">Error:</span><br/><%:Model.Error %><%
        } %>
    </pre>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
<%--    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.5.2/jquery.min.js" type="text/javascript">        
    </script> --%>
    <script type="text/javascript">
    // <![CDATA[
        $(document).ready(function () {
            $("#Name").focus();
        });
    // ]]>
    </script>
</asp:Content>

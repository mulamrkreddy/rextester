<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.regex.RegexData>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
Regex tester
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MetaContent" runat="server">    
    <meta name="Keywords" content="online .net regex tester" />
    <meta name="Description" content="online .net regex tester" />    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
<%--    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.5.2/jquery.min.js" type="text/javascript">        
    </script>  --%>  
    <script type="text/javascript">
    // <![CDATA[
        var previousText = '';
        var previousPattern = '';
        $(document).ready(function () {

            $("#Save").click(function () {
                $("#ShouldSave").val(true);
                $("#SavedOutput").val($("#Result").html());
            });

            previousText = $("#Text").val();
            previousPattern = $("#Pattern").val();

            $("#Pattern").focusout(function () {
                EnableButtons();
                if ($("#Pattern").val() !== previousPattern) {
                    previousPattern = $("#Pattern").val();
                    Match();
                }
            });
            $("#Text").focusout(function () {
                EnableButtons();
                if ($("#Text").val() !== previousText) {
                    previousText = $("#Text").val();
                    Match();
                }
            });
            $("#Pattern").focusin(function () {
                DisableButtons();
            });
            $("#Text").focusin(function () {
                DisableButtons();
            });
            $("input[type=checkbox]").click(function () { Match(); });

            $.ajaxSetup({
                timeout: 30000,
                error: function (request, status, err) {
                    $("#NonResultMessage").replaceWith("<pre class=\"resultarea\" id=\"NonResultMessage\">Error occurred. Try again later.</pre>");
                }
            });

            var Match = function () {
                var maxChar = 500000;
                if ($("#Text").val().length > maxChar) {
                    $("#Result").replaceWith("<pre class=\"resultarea\" id=\"Result\"></pre>");
                    $("#NonResultMessage").replaceWith("<pre class=\"resultarea\" id=\"NonResultMessage\">Text too long (max " + maxChar + " characters)</pre>");
                    return;
                }
                var maxAjaxChar = 10000;
                if ($("#Text").val().length > maxAjaxChar) {
                    $("#Result").replaceWith("<pre class=\"resultarea\" id=\"Result\"></pre>");
                    $("#NonResultMessage").replaceWith("<pre class=\"resultarea\" id=\"NonResultMessage\"><b>Make non-ajax post to see results.</b></pre>");
                    return;
                }
                $("#NonResultMessage").replaceWith("<pre class=\"resultarea\" id=\"NonResultMessage\">Loading...</pre>");
                $("#Result").replaceWith("<pre class=\"resultarea\" id=\"Result\"></pre>");
                var serializedData = $("form").serialize();
                $.post('/Main/TakeText', serializedData,
                                    function (data) {
                                        $("#NonResultMessage").replaceWith("<pre class=\"resultarea\" id=\"NonResultMessage\"></pre>");
                                        $("#Result").replaceWith("<pre class=\"resultarea\" id=\"Result\">" + data + "</pre>");
                                    }, 'html');
            };

            var DisableButtons = function () {
                $("#Test").attr("disabled", true);
                $("#Save").attr("disabled", true);
            };
            var EnableButtons = function () {
                $("#Test").attr("disabled", false);
                $("#Save").attr("disabled", false);
            };

            $("#Pattern").focus();
        });
    // ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>.net regex tester</h2>
    <% using (Html.BeginForm("Index", "tester"))
       {%>
       <div class="formcontent">
            <table style="width: 85%;">
                <tr>
                    <td>
                        Pattern:<br />
                        <%: Html.TextAreaFor(model => model.Pattern, new { style = "width: 96%", rows = 10, cols = 1000, spellcheck = "false" })%>
                    </td>
                    <td>
                        Text:<br />
                        <%: Html.TextAreaFor(model => model.Text, new { style = "width: 100%", rows = 10, cols = 1000, spellcheck = "false" })%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left">
                        <%for (int i = 0; i < Model.Options.Count; i++)
                          {%>
                                <span class="options">
                                <%: Html.CheckBoxFor(model => model.Options[i], new { title = Model.Describtions[i].Comment, id = Model.Describtions[i].Title })%>
                                <%: Html.Label(Model.Describtions[i].Title)%>
                                </span>
                        <%} %>
                    </td>
                </tr>
            </table>
            <div style="margin-top:1em;">
                <input style="margin-left:0.5em" id="Test" type="submit" value="Test it!" />
                <input style="margin-left:1em" id="Save" type="submit" value="Save it" />
            </div>
            <%:Html.HiddenFor(model => Model.IsReplace)%>
            <input id="ShouldSave" name="ShouldSave" type="hidden" value="<%=Model.ShouldSave%>" />
            <input id="SavedOutput" name="SavedOutput" type="hidden" value="<%=Model.SavedOutput%>" />
        </div>
    <%} %>
    <pre class="resultarea" id="NonResultMessage"><%
        if (!string.IsNullOrEmpty(Model.SavedUrl))
        {
            %>Permanent url: <%:Model.SavedUrl%><%
        }
    %></pre>
    <pre class="resultarea" id="Result"><% 
        if (Model.IsResult)
        {%>
            <%:Html.DisplayTextFor(f=>f.Result)%>
        <%}        
    %></pre>
</asp:Content>

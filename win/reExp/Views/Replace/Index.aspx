<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.regex.RegexData>" %>


<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
Regex replacement
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MetaContent" runat="server">    
    <meta name="Keywords" content=".net regex replacement" />
    <meta name="Description" content=".net regex replacement" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
<%--    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.5.2/jquery.min.js" type="text/javascript">        
    </script>--%>
     <script src="../../Scripts/ZeroClipboard.min.js" type="text/javascript">
     </script>
    <script type="text/javascript">
    // <![CDATA[
        function prepareCopyText() {
            var res = '';
            var interm = $("#ResultText").html().replace(new RegExp("<br/?>", "gi"), "<br/><div/>");
            var arr = interm.split(new RegExp("<br/?>", "i"));
            var i;
            var l;
            for (i = 0, l = arr.length; i < l; i++) {
                res += $('<div/>').html(arr[i].replace(/<.*?>/g, '')).text();
                if (i < l - 1) {
                    res += "\r\n";
                }
            }
            return res;
        }
        var previousText = '';
        var previousPattern = '';
        var previousSubstitution = '';

        $(document).ready(function () {
            $("#Save").click(function () {
                $("#ShouldSave").val(true);
                $("#SavedOutput").val($("#Result").html());
            });

            ZeroClipboard.setMoviePath('../../Content/ZeroClipboard.swf');
            var clip = new ZeroClipboard.Client();
            clip.setHandCursor(true);
            if ($("#ResultText").length > 0) {
                clip.setText(prepareCopyText());
            }
            else {
                clip.setText(' ');
            }
            clip.glue('d_clip_button', 'd_clip_container');

            previousText = $("#Text").val();
            previousPattern = $("#Pattern").val();
            previousSubstitution = $("#Substitution").val();

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
            $("#Substitution").focusout(function () {
                EnableButtons();
                if ($("#Substitution").val() !== previousSubstitution) {
                    previousSubstitution = $("#Substitution").val();
                    Match();
                }
            });
             $("#Pattern").focusin(function () {
                DisableButtons();
            });
            $("#Text").focusin(function () {
                DisableButtons();
            });
            $("#Substitution").focusin(function () {
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
                                        if ($("#ResultText").length > 0) {
                                            clip.setText(prepareCopyText());
                                        }
                                        else {
                                            clip.setText(' ');
                                        }
                                    }, 'html');
            };

            var DisableButtons = function () {
                $("#Replace").attr("disabled", true);
                $("#Save").attr("disabled", true);
            };
            var EnableButtons = function () {
                $("#Replace").attr("disabled", false);
                $("#Save").attr("disabled", false);
            };
            $("#Pattern").focus();

        });
    // ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">    
    <h2>.net regex replacement</h2>    
    <% using (Html.BeginForm("Index", "replace"))
       {%>
       <div class="formcontent">
            <table style="width: 85%">
                <tr>
                    <td valign="top">
                        Pattern:<br />
                        <%: Html.TextAreaFor(model => model.Pattern, new { style = "width: 96%", rows = 4, cols = 1000, tabindex = 1, spellcheck = "false" })%>
                    </td>
                    <td rowspan="2">
                        Text:<br />
                        <%: Html.TextAreaFor(model => model.Text, new { style = "width: 100%", rows = 11, cols = 1000, tabindex = 3, spellcheck = "false" })%>
                    </td>
                </tr>
                <tr>
                    <td valign="bottom">
                        Substitution:<br />
                        <%: Html.TextAreaFor(model => model.Substitution, new { style = "width: 96%", rows = 4, cols = 1000, tabindex = 2, spellcheck = "false" })%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left">
                        <%for (int i = 0; i < Model.Options.Count; i++)
                          {%>
                            <span class="options">
                                <%: Html.CheckBoxFor(model => model.Options[i], new {tabindex=4+i, title = Model.Describtions[i].Comment, id = Model.Describtions[i].Title })%>
                                <%: Html.Label(Model.Describtions[i].Title)%>     
                            </span>              
                        <%} %>
                    </td>
                </tr>
            </table>
            <table style="width: 85%;margin-top:1em;">
                <tr>
                    <td align="left">
                        <input tabindex="<%:Model.Options.Count + 4%>" id="Replace" type="submit" value="Replace it!"style="margin-left:0.5em" />
                        <input style="margin-left:1em" id="Save" type="submit" value="Save it" />
                    </td>
                    <td align="right">
                        <div id="d_clip_container" style="position:relative">
                           <div id="d_clip_button" style="text-decoration: underline;color:Gray;">Copy result</div>
                        </div>
                    </td>
                </tr>
            </table>
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

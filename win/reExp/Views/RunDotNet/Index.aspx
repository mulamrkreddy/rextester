<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<reExp.Controllers.rundotnet.RundotnetData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Compile and run code snippet
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>run code</h2>
    <%using (Html.BeginForm("Index", "rundotnet"))
      {%>
        <div class="formcontent" style="padding-top: 0.5em; padding-bottom: 0.5em">
            <span style="margin: 0 0.5em 0 0">Language:</span><%:Html.DropDownListFor(f => f.LanguageChoice, Model.Languages)%>
            <span style="margin: 0 0.5em 0 0.5em">Editor:</span><%:Model.ShowCodeMirror ? Html.DropDownListFor(f => f.EditorChoice, Model.Editor1) : Html.DropDownListFor(f => f.EditorChoice, Model.Editor2)%>
                <div  style="width: 85%; margin-top:1em;">
                    <textarea class="editor" spellcheck="false" cols="1000" id="Program" name="Program" rows="30" style="width: 100%;"><%=Model.Program%></textarea>
                </div>
            <table style="width: 85%; margin-top:1em;">
                <tr>
                    <td align="left">
                        <input id="Run" type="button" value="Run it<%:Model.EditorChoice == "1" ? " (F8)" : "" %>" />
                        <input style="margin-left:1em" id="Save" type="button" value="Save it" />    
                        <%if(!Model.IsInterpreted) 
                          {
                              %><%: Html.CheckBoxFor(model => model.ShowWarnings, new { style = "vertical-align: middle; margin-left: 1.5em;" })%>
                                <label for="ShowWarnings" style="margin-left: 0.2em;font-size: 0.85em;vertical-align: middle;">Show compiler warnings</label><%
                        }%>
                    </td>                    
                </tr>
                <tr>
                    <td>
                        <div style="margin-top: 0.8em;font-size: 0.85em;" id="Stats"><%:Model.RunStats%></div>
                    </td>
                </tr>
            </table>

            <input id="SavedOutput" name="SavedOutput" type="hidden" value="<%=Model.SavedOutput%>" />
            <input id="WholeError" name="WholeError" type="hidden" value="<%=Model.WholeError%>" />
            <input id="WholeWarning" name="WholeWarning" type="hidden" value="<%=Model.WholeWarning%>" />
            <input id="StatsToSave" name="StatsToSave" type="hidden" value="<%=Model.StatsToSave%>" />
        </div>
    <%} %>
    <pre id="Link" class="resultarea"></pre>

    <pre id="Warning" class="resultarea"><%
        if (Model.ShowWarnings && !string.IsNullOrEmpty(Model.WholeWarning))
        { 
            %><span style="color: Orange">Warning(s):</span><br/><span id="WarningSpan"><%:Model.WholeWarning%></span><%   
        }
    %></pre>

    <pre id="Error" class="resultarea"><%
        if (!string.IsNullOrEmpty(Model.WholeError))
        {
            %><span style="color: Red">Error(s)<%:Model.IsInterpreted ? ", warning(s)" : ""%>:</span><br/><span id="ErrorSpan"><%:Model.WholeError%></span><%  
        }                   
    %></pre> 
    <pre id="Result" class="resultarea"><%:Model.Output%></pre>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
    <meta name="Keywords" content="online compiler for .net languages" />
    <meta name="Description" content="run .net code online" />
    <%if (Model.EditorChoice == "1")
     {
             %><link rel="stylesheet" href="../../Scripts/codemirror2/lib/codemirror.css"/><%
            if (Model.LanguageChoice == "1")
            { 
                %><link rel="stylesheet" href="../../Scripts/codemirror2/theme/csharp.css"/><%
            }
            else if (Model.LanguageChoice == "4")
            { 
                %><link rel="stylesheet" href="../../Scripts/codemirror2/theme/java.css"/><%
            }
            else
            {
                %><link rel="stylesheet" href="../../Scripts/codemirror2/theme/default.css"/><%
            }
    }%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
    <%if (Model.EditorChoice == "1")
      {
            %><script src="../../Scripts/codemirror2/lib/codemirror.min.js" type="text/javascript"></script><%
    }%>
    <%if (Model.EditorChoice == "2")
    {
        %><script src="../../Scripts/editarea/edit_area_full.js" type="text/javascript"></script><%
    }%>
    <script type="text/javascript">
        //<![CDATA[
        <%if(Model.EditorChoice == "1")
        {
            %>var GlobalEditor;<%
        }%>
        $(document).ready(function () {

            $("#Save").click(function () {
                $("#SavedOutput").val($("#Result").text());
                $("#WholeError").val($("#ErrorSpan").text());
                $("#WholeWarning").val($("#WarningSpan").text());
                $("#StatsToSave").val($("#Stats").text());
                Save();
            });
            $("#Run").click(function () {
               Run();
            });
            $("#LanguageChoice").change(function () {
                Reload();
            });
            $("#EditorChoice").change(function () {
                Reload();
            });            
            var Reload = function () {
                $("#SavedOutput").val('');
                $("#WholeError").val('');
                $("#WholeWarning").val('');
                $("#StatsToSave").val('');
                $("form").submit();
            };

            $.ajaxSetup({
                timeout: 30000,
                error: function (request, status, err) {
                    $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\">Error occurred. Try again later.</pre>");
                }
            });

        });

        function Save () {
            $('html, body').animate({ scrollTop: $("#Link").offset().top }, 500);

            $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\">Saving...</pre>");
            $("#Warning").replaceWith("<pre id=\"Warning\" class=\"resultarea\"></pre>");
            $("#Error").replaceWith("<pre id=\"Error\" class=\"resultarea\"></pre>");
            $("#Result").replaceWith("<pre id=\"Result\" class=\"resultarea\"></pre>");

            <%if(Model.EditorChoice == "1") 
            {
                %>$("#Program").val(GlobalEditor.getValue());<%
            }
            else if(Model.EditorChoice == "2")
            {
                %>$("#Program").val(editAreaLoader.getValue("Program"));<%
            }%>

            var serializedData = $("form").serialize();
            $.post('/rundotnet/save', serializedData,
                    function (data) {
                        var obj = jQuery.parseJSON(data);
                        $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\">Permanent link: "+obj.Url+"</pre>");
                    }, 'text');
        };

        function Run () {
            $('html, body').animate({ scrollTop: $("#Link").offset().top }, 500);

            $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\">Working...</pre>");
            $("#Warning").replaceWith("<pre id=\"Warning\" class=\"resultarea\"></pre>");
            $("#Error").replaceWith("<pre id=\"Error\" class=\"resultarea\"></pre>");
            $("#Result").replaceWith("<pre id=\"Result\" class=\"resultarea\"></pre>");

            <%if(Model.EditorChoice == "1") 
            {
                %>$("#Program").val(GlobalEditor.getValue());<%
            }
            else if(Model.EditorChoice == "2")
            {
                %>$("#Program").val(editAreaLoader.getValue("Program"));<%
            }%>
            
            var serializedData = $("form").serialize();
            $.post('/rundotnet/run', serializedData,
                    function (data) {
                        $("#Link").replaceWith("<pre id=\"Link\" class=\"resultarea\"></pre>");                        
                        var obj = jQuery.parseJSON(data);
                        if(obj.Warnings != null)
                        {
                            $("#Warning").replaceWith("<pre id=\"Warning\" class=\"resultarea\"><span style=\"color: Orange\">Warning(s):</span><br/><span id=\"WarningSpan\"></span></pre>");
                            $("#WarningSpan").text(obj.Warnings);
                        }
                        if(obj.Errors != null)
                        {
                            $("#Error").replaceWith("<pre id=\"Error\" class=\"resultarea\"><span style=\"color: Red\">Error(s)"+'<%:Model.IsInterpreted ? ", warning(s)" : ""%>'+":</span><br/><span id=\"ErrorSpan\"></span></pre>");
                            $("#ErrorSpan").text(obj.Errors);
                        }
                        if(obj.Result != null)
                            $("#Result").text(obj.Result);
                        
                         $("#Stats").text(obj.Stats);

                         $('html, body').animate({ scrollTop: $("#Stats").offset().top }, 500);
                    }, 'text');
        };

        function keyEvent(cm, e) {
            // Hook into F8
            if ((e.keyCode == 119) && e.type == 'keydown') {
                e.stop();                
                $("#Program").val(cm.getValue());
                Run();
            }
        }
        //]]>
    </script>
    <%if (Model.LanguageChoice == "1" && Model.EditorChoice == "1")
      {
         %><script type="text/javascript" src="../../Scripts/codemirror2/mode/clike/clike.js"></script>
            <script type="text/javascript">
              //<![CDATA[
                $(document).ready(function () {
                    var Editor = CodeMirror.fromTextArea(document.getElementById("Program"), {
                        mode: "text/x-csharp",
                        lineNumbers: true,
                        indentUnit: 4,
                        tabMode: "shift",
                        matchBrackets: true,
                        theme: "csharp",
                        onKeyEvent: keyEvent
                    });
                    GlobalEditor = Editor;
                });
            //]]>
        </script><%
        }
        else if (Model.LanguageChoice == "1" && Model.EditorChoice == "2")
      {
        %><script type="text/javascript">
            //<![CDATA[
                $(document).ready(function () {
                    editAreaLoader.init({
                        id: "Program"		        // textarea id
	                    , syntax: "csharp"			// syntax to be uses for highgliting
	                    , start_highlight: true	    // to display with highlight mode on start-up
                        , allow_toggle: false
                        , replace_tab_by_spaces: 4
                        , show_line_colors: true
                    });
                });
            //]]>
        </script><%
      }
      else if (Model.LanguageChoice == "2" && Model.EditorChoice == "2")
      {
        %><script type="text/javascript">
            //<![CDATA[
                $(document).ready(function () {
                    editAreaLoader.init({
                        id: "Program"		        // textarea id
	                    , syntax: "vb1"			    // syntax to be uses for highgliting
	                    , start_highlight: true	    // to display with highlight mode on start-up
                        , allow_toggle: false
                        , replace_tab_by_spaces: 4
                        , show_line_colors: true
                    });
                });
            //]]>
        </script><%
      }
      else if (Model.LanguageChoice == "3" && Model.EditorChoice == "2")
      {
        %><script type="text/javascript">
            //<![CDATA[
              $(document).ready(function () {
                  editAreaLoader.init({
                      id: "Program"		            // textarea id
	                    , syntax: "fsharp"			// syntax to be uses for highgliting
	                    , start_highlight: true	    // to display with highlight mode on start-up
                        , allow_toggle: false
                        , replace_tab_by_spaces: 4
                        , show_line_colors: true
                  });
              });
            //]]>
        </script>
    <%}
    else if (Model.LanguageChoice == "4" && Model.EditorChoice == "1")
      {
         %><script type="text/javascript" src="../../Scripts/codemirror2/mode/clike/clike.js"></script>
            <script type="text/javascript">
              //<![CDATA[
                $(document).ready(function () {
                    var Editor = CodeMirror.fromTextArea(document.getElementById("Program"), {
                        mode: "text/x-java",
                        lineNumbers: true,
                        indentUnit: 4,
                        tabMode: "shift",
                        matchBrackets: true,
                        theme: "java",
                        onKeyEvent: keyEvent
                    });
                    GlobalEditor = Editor;
                });
            //]]>
        </script><%
        }
    else if (Model.LanguageChoice == "4" && Model.EditorChoice == "2")
      {
        %><script type="text/javascript">
            //<![CDATA[
              $(document).ready(function () {
                  editAreaLoader.init({
                      id: "Program"		            // textarea id
	                    , syntax: "java"			// syntax to be uses for highgliting
	                    , start_highlight: true	    // to display with highlight mode on start-up
                        , allow_toggle: false
                        , replace_tab_by_spaces: 4
                        , show_line_colors: true
                  });
              });
            //]]>
        </script>
    <%}
    else if (Model.LanguageChoice == "5" && Model.EditorChoice == "1")
      {
        %><script type="text/javascript" src="../../Scripts/codemirror2/mode/python/python.js"></script>
        <script type="text/javascript">
          //<![CDATA[
            $(document).ready(function () {
                var Editor = CodeMirror.fromTextArea(document.getElementById("Program"), {
                    mode:
                    {
                        name: "python",
                        version: 2,
                        singleLineStringErrors: false
                    },
                    lineNumbers: true,
                    indentUnit: 4,
                    tabMode: "shift",
                    matchBrackets: true,
                    onKeyEvent: keyEvent
                });
                GlobalEditor = Editor;
            });
        //]]>
        </script><%
    }
    else if (Model.LanguageChoice == "5" && Model.EditorChoice == "2")
      {
        %><script type="text/javascript">
            //<![CDATA[
              $(document).ready(function () {
                  editAreaLoader.init({
                      id: "Program"		            // textarea id
	                    , syntax: "python"			// syntax to be uses for highgliting
	                    , start_highlight: true	    // to display with highlight mode on start-up
                        , allow_toggle: false
                        , replace_tab_by_spaces: 4
                        , show_line_colors: true
                  });
              });
            //]]>
        </script>
    <%}
    else if (Model.LanguageChoice == "6" && Model.EditorChoice == "1")
      {
        %><script type="text/javascript" src="../../Scripts/codemirror2/mode/clike/clike.js"></script>
        <script type="text/javascript">
          //<![CDATA[
            $(document).ready(function () {
                var Editor = CodeMirror.fromTextArea(document.getElementById("Program"), {
                    mode: "text/x-csrc",
                    lineNumbers: true,
                    indentUnit: 4,
                    tabMode: "shift",
                    matchBrackets: true,
                    onKeyEvent: keyEvent
                });
                GlobalEditor = Editor;
            });
        //]]>
        </script><%
    } 
    else if (Model.LanguageChoice == "6" && Model.EditorChoice == "2")
      {
        %><script type="text/javascript">
            //<![CDATA[
              $(document).ready(function () {
                  editAreaLoader.init({
                      id: "Program"		            // textarea id
	                    , syntax: "c"			// syntax to be uses for highgliting
	                    , start_highlight: true	    // to display with highlight mode on start-up
                        , allow_toggle: false
                        , replace_tab_by_spaces: 4
                        , show_line_colors: true
                  });
              });
            //]]>
        </script>
    <%}
    else if (Model.LanguageChoice == "7" && Model.EditorChoice == "1")
      {
        %><script type="text/javascript" src="../../Scripts/codemirror2/mode/clike/clike.js"></script>
        <script type="text/javascript">
          //<![CDATA[
            $(document).ready(function () {
                var Editor = CodeMirror.fromTextArea(document.getElementById("Program"), {
                    mode: "text/x-c++src",
                    lineNumbers: true,
                    indentUnit: 4,
                    tabMode: "shift",
                    matchBrackets: true,
                    onKeyEvent: keyEvent
                });
                GlobalEditor = Editor;
            });
        //]]>
        </script><%
    } 
    else if (Model.LanguageChoice == "7" && Model.EditorChoice == "2")
      {
        %><script type="text/javascript">
            //<![CDATA[
              $(document).ready(function () {
                  editAreaLoader.init({
                      id: "Program"		            // textarea id
	                    , syntax: "cpp"			// syntax to be uses for highgliting
	                    , start_highlight: true	    // to display with highlight mode on start-up
                        , allow_toggle: false
                        , replace_tab_by_spaces: 4
                        , show_line_colors: true
                  });
              });
            //]]>
        </script>
    <%}%>

</asp:Content>

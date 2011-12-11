<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>About</h2>
    <div style="width: 95%;">
        Rextester - some online tools for anyone who finds them useful. It was started as online .net regex tester.
        <br/>
        <div style="padding-left: 2em">
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Tester)%>">Regex tester</a> - .net regex tester. <br />
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Replace)%>">Regex replace</a> - .net regex replacement.<br />
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Reference)%>">Regex reference</a> - short regex reference.<br />
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Pagerank)%>">Page rank</a> - determine your page's position to given google query.<br />            
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Rundotnet)%>">Run code</a> - online compiling and execution for some languages.
            <br/><br/><b style="color:Gray">C#, Visual basic, F#</b><br/>
            .Net framework v. 4 is used. Your code will be given max 5 sec of cpu time and limited memory (~100 Mb). Also your code will run in an appdomain with basic execution privilege only.<br/>
            The entry point for you code is given Main method in type Program in namespace Rextester. <b>This entry point shouldn't be changed.</b>
            Types from following assemblies are available:
             <ul>
                <li>System.dll</li>
                <li>System.Core.dll</li>
                <li>System.Data.dll</li>
                <li>System.Data.DataSetExtensions.dll</li>
                <li>System.Xml.dll</li>
                <li>System.Xml.Linq.dll</li>
                <li>Microsoft.CSharp.dll - when C# is used</li>
                <li>Microsoft.VisualBasic.dll - when Visual Basic is used</li>
            </ul>
            F# program can use types from FSharp.Core.dll (version 4.0.0.0) and System.Numerics.dll. Compilation is slow by our fault (F# compiler is not precompiled to native code by ngen).<br/>
            If you found security breaches and can break something in some way - we would appreciate your feedback on this.
            <br/><br/><b style="color:Gray">Java, Python, C, C++</b><br/>
            These languages run on linux. Here are compiler versions:
            <ul>
                <li>Java - Sun's implementation of java, compiler version 1.6.0_26.</li>
                <li>Python - 2.7</li>
                <li>C - gcc 4.5.2</li>
                <li>C++ - g++ 4.5.2</li>
            </ul> 
            Your code will be run on behalf user 'nobody' and group 'nogroup'. Also your code will be executed from Python wrapper which sets various limits to the process. It does so
            by using 'setrlimit' system call. You'll have max 5 sec of cpu time, limited memory (~100 Mb) and other restrictions will apply (like no network access and no writing permissions). Also your process and all its children will be run in a
            newly created process group which will be terminated after 10 seconds from start if still running.<br/>
            <br/>We don't claim that this is secure. In many senses you'll have the power of 'nobody' user. On a bright side, this has some <a href="http://rextester.com/runcode?code=KAKN22727">useful</a> side-effects. The reason why, at least for now, 
            we leave so many potential security breaches is because it's <b>hard</b> to make it really secure. What are the options? 
            <ul>
                <li><a href="http://codepad.org/">Codepad</a> seems to <a href="http://rextester.com/runcode?code=JAJ71205">'ptrace'</a>
                    everything and disallow many system calls, for example, 'fork()'. That's pretty restrictive.</li>
                <li>Apparmor - out of everything we tried, this did the job pretty well and it was rather easy to configure. But it doesn't work with OpenVZ virtualization that is used by our server provider.</li>
                <li>SELinux - stopped reading documentation in the middle of it. Too (unnecessarily?) complex.</li>
                <li>Native security mechanisms - like chroot and file permissions. Hard to make it secure this way without breaking the service and the system.</li>
            </ul>
            So, if you can take the system down - then be it, but please report how you did this. Your advice on this topic is always <a href="http://rextester.com/feedback/">welcome</a>.
            <br/><br/><b style="color:Gray">Credit</b><br/>
            Special thanks goes to people behind <a href="http://codemirror.net/">CodeMirror</a> and <a href="http://www.cdolivet.com/editarea/">Edit area</a>.<br/><br />
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Feedback)%>">Feedback</a> - give us feedback.<br />
            <a href="<%:Utils.GetUrl(Utils.PagesEnum.Login)%>">Login</a> - to register is as simple as to login and once logged in you'll be able to track your saved snippets. <br />
        </div>  
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MetaContent" runat="server">
    <meta name="Keywords" content=".net regex tester, online c#, vb, f#, java, python, c, c++ code compiler, online code execution, rundotnet, runcode" />
    <meta name="Description" content="rextester - test .net regexes, run code online." />    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

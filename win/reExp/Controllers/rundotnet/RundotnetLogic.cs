using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using reExp.Utils;
using Service;
namespace reExp.Controllers.rundotnet
{
    public class RundotnetLogic
    {
        public static RundotnetData RunProgram(RundotnetData data)
        {
            List<string> dotNet = new List<string>(){"1", "2", "3"};
            if (dotNet.Contains(data.LanguageChoice))
            {
                return RunDotNet(data);
            }
            else
            {
                return RunLinux(data);
            }
        }

        static RundotnetData RunDotNet(RundotnetData data)
        {
            string language = "";
            switch (data.LanguageChoice)
            {
                case "1":
                    language = "CSharp";
                    break;
                case "2":
                    language = "VisualBasic";
                    break;
                case "3":
                    language = "FSharp";
                    break;
            }

            int compilationTimeInMs;
            CompilerParameters cp = new CompilerParameters();
            cp.GenerateExecutable = false;
            Random rg = new Random();
            string folder = reExp.Utils.Utils.RootFolder + @"\executables\usercode\";
            string assemblyName = "userAssembly_" + rg.Next(0, 10000000);
            string path = folder + assemblyName + ".dll";
            cp.OutputAssembly = path;
            // Save the assembly as a physical file.
            cp.GenerateInMemory = false;
            // Set whether to treat all warnings as errors.
            cp.TreatWarningsAsErrors = false;
            cp.WarningLevel = 4;
            cp.IncludeDebugInformation = false;


            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("System.Core.dll");
            if (language != "FSharp")
            {
                cp.ReferencedAssemblies.Add("System.Data.dll");
                cp.ReferencedAssemblies.Add("System.Data.DataSetExtensions.dll");
                cp.ReferencedAssemblies.Add("System.Xml.dll");
                cp.ReferencedAssemblies.Add("System.Xml.Linq.dll");
                if (language == "CSharp")
                    cp.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
                else if (language == "VisualBasic")
                    cp.ReferencedAssemblies.Add("Microsoft.VisualBasic.dll");
            }
            else
            {

                cp.ReferencedAssemblies.Add("System.Numerics.dll");
                //string fspath = reExp.Utils.Utils.RootFolder + @"executables\usercode\";
                //cp.ReferencedAssemblies.Add(fspath + @"FSharp.Core.dll");
                //cp.ReferencedAssemblies.Add(fspath + @"FSharp.Powerpack.dll");
                //cp.ReferencedAssemblies.Add(fspath + @"FSharp.PowerPack.Compatibility.dll");
                //cp.ReferencedAssemblies.Add(fspath + @"FSharp.PowerPack.Linq.dll");
                //cp.ReferencedAssemblies.Add(fspath + @"FSharp.PowerPack.Metadata.dll");
                //cp.ReferencedAssemblies.Add(fspath + @"FSharp.PowerPack.Parallel.Seq.dll");
            }

            CompilerResults cr = null;
            if (language == "FSharp")
            {

                using (CodeDomProvider provider = new Microsoft.FSharp.Compiler.CodeDom.FSharpCodeProvider())
                {
                    DateTime comp_start = DateTime.Now;
                    // Invoke compilation of the source file.
                    cr = provider.CompileAssemblyFromSource(cp, new string[] { data.Program });
                    compilationTimeInMs = (int)(DateTime.Now - comp_start).TotalMilliseconds;
                }
            }
            else
            {
                using (CodeDomProvider provider = CodeDomProvider.CreateProvider(language))
                {
                    DateTime comp_start = DateTime.Now;
                    // Invoke compilation of the source file.
                    cr = provider.CompileAssemblyFromSource(cp, new string[] { data.Program });
                    compilationTimeInMs = (int)(DateTime.Now - comp_start).TotalMilliseconds;
                }
            }
            var messages = cr.Errors.Cast<CompilerError>();
            var warnings = messages.Where(f => f.IsWarning == true);
            var errors = messages.Where(f => f.IsWarning == false);

            if (warnings.Count() != 0)
            {
                foreach (var warn in warnings)
                    data.Warnings.Add(string.Format("({0}:{1}) {2}", warn.Line, warn.Column, warn.ErrorText));
            }
            if (errors.Count() != 0)
            {
                foreach (var ce in errors)
                {
                    data.Errors.Add(string.Format("({0}:{1}) {2}", ce.Line, ce.Column, ce.ErrorText));
                }
                Utils.Log.LogCodeToDB(data.Program, "Compilation errors");
                data.RunStats = string.Format("Compilation time: {0} s", Math.Round((compilationTimeInMs / (double)1000), 2));
                return data;
            }
            else
            {
                using (Process process = new Process())
                {
                    try
                    {
                        double TotalMemoryInBytes = 0;
                        double TotalThreadCount = 0;
                        int samplesCount = 0;

                        process.StartInfo.FileName = reExp.Utils.Utils.RootFolder + "executables/SpawnedProcess.exe";
                        process.StartInfo.Arguments = folder.Replace(" ", "|_|") + " " + assemblyName + " Rextester|Program|Main";
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;

                        DateTime start = DateTime.Now;
                        process.Start();
                        //try
                        //{
                        //    process.PriorityClass = ProcessPriorityClass.BelowNormal;
                        //}
                        //catch (Exception)
                        //{ }

                        OutputReader output = new OutputReader(process.StandardOutput);
                        Thread outputReader = new Thread(new ThreadStart(output.ReadOutput));
                        outputReader.Start();
                        OutputReader error = new OutputReader(process.StandardError);
                        Thread errorReader = new Thread(new ThreadStart(error.ReadOutput));
                        errorReader.Start();


                        do
                        {
                            // Refresh the current process property values.
                            process.Refresh();
                            if (!process.HasExited)
                            {
                                try
                                {
                                    var proc = process.TotalProcessorTime;
                                    // Update the values for the overall peak memory statistics.
                                    var mem1 = process.PagedMemorySize64;
                                    var mem2 = process.PrivateMemorySize64;

                                    //update stats
                                    TotalMemoryInBytes += (mem1 + mem2);
                                    TotalThreadCount += (process.Threads.Count);
                                    samplesCount++;

                                    if (proc.TotalSeconds > 5 || mem1 + mem2 > 100000000 || process.Threads.Count > 100 || start + TimeSpan.FromSeconds(10) < DateTime.Now)
                                    {
                                        var time = proc.TotalSeconds;
                                        var mem = mem1 + mem2;
                                        process.Kill();
                                        var res = string.Format("Process killed because it exceeded given resources.\nCpu time used {0} sec, absolute running time {1} sec, memory used {2} Mb, nr of threads {3}", time, (int)(DateTime.Now - start).TotalSeconds, (int)(mem / 1048576), process.Threads.Count);
                                        data.Errors.Add(res);
                                        string partialResult = output.Builder.ToString();
                                        data.Output = partialResult;
                                        Utils.Log.LogCodeToDB(data.Program, res);
                                        data.RunStats = string.Format("Compilation time: {0} sec, absolute running time: {1} sec, cpu time {2} sec, average memory usage: {3} Mb, average nr of threads: {4}",
                                            Math.Round((compilationTimeInMs / (double)1000), 2),
                                            Math.Round((DateTime.Now - start).TotalSeconds, 2),
                                            Math.Round(proc.TotalSeconds, 2),
                                            samplesCount != 0 ? (int?)((TotalMemoryInBytes / samplesCount) / 1048576) : null,
                                            samplesCount != 0 ? (int?)(TotalThreadCount / samplesCount) : null);
                                        return data;
                                    }
                                }
                                catch (InvalidOperationException)
                                {
                                    break;
                                }
                            }
                        }
                        while (!process.WaitForExit(10));
                        process.WaitForExit();

                        data.RunStats = string.Format("Compilation time: {0} sec, absolute running time: {1} sec, cpu time {2} sec, average memory usage: {3} Mb, average nr of threads: {4}",
                                            Math.Round((compilationTimeInMs / (double)1000), 2),
                                            Math.Round((process.ExitTime - process.StartTime).TotalSeconds, 2),
                                            Math.Round(process.TotalProcessorTime.TotalSeconds, 2),
                                            samplesCount != 0 ? (int?)((TotalMemoryInBytes / samplesCount) / 1048576) : null,
                                            samplesCount != 0 ? (int?)(TotalThreadCount / samplesCount) : null);

                        errorReader.Join(5000);
                        outputReader.Join(5000);
                        if (!string.IsNullOrEmpty(error.Output))
                        {
                            data.Output = output.Builder.ToString();
                            data.Errors.Add(error.Output);
                            Utils.Log.LogCodeToDB(data.Program, error.Output);
                            return data;
                        }
                        data.Output = output.Output;
                        Utils.Log.LogCodeToDB(data.Program, "OK");
                        return data;
                    }
                    catch (Exception e)
                    {
                        if (!process.HasExited)
                        {
                            reExp.Utils.Log.LogInfo("Process left running " + e.Message, "RunDotNet");
                        }
                        throw;
                    }
                    finally
                    {
                        reExp.Utils.CleanUp.DeleteFile(path);
                    }
                }
            }
        }

        static RundotnetData RunLinux(RundotnetData data)
        {
            LinuxService service = new LinuxService();
            Service.linux.Languages lang = Service.linux.Languages.Java;
            switch(data.LanguageChoice)
            {
                case "4":
                    lang = Service.linux.Languages.Java;
                    break;
                case "5":
                    lang = Service.linux.Languages.Python;
                    break;
                case "6":
                    lang = Service.linux.Languages.C;
                    break;
                case "7":
                    lang = Service.linux.Languages.CPP;
                    break;
                default:
                    break;
            }
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var res = service.DoWork(data.Program, lang);
            watch.Stop();
            if (res != null)
            {
                if (string.IsNullOrEmpty(res.Stats))
                    res.Stats = "";
                else
                    res.Stats += ", ";
                res.Stats += string.Format("absolute service time: {0}", Math.Round((double)watch.ElapsedMilliseconds/(double)1000, 2));
                data.RunStats = res.Stats;
            }
            bool logged = false;
            if (!string.IsNullOrEmpty(res.System_Error))
            {
                reExp.Utils.Log.LogInfo("Linux " + res.System_Error, "RunDotNet");
                data.Errors.Add(res.System_Error);
                Utils.Log.LogCodeToDB(data.Program, "Linux: system error");
                return data;
            }
            if (!string.IsNullOrEmpty(res.Errors))
            {
                data.Errors.Add(res.Errors);
                if (!logged)
                {
                    Utils.Log.LogCodeToDB(data.Program, "Linux: error");
                    logged = true;
                }
            }
            if (res.Exit_Code < 0)
            {
                data.Errors.Add(res.Exit_Status);
                if (!logged)
                {
                    Utils.Log.LogCodeToDB(data.Program, "Linux: negative exit code");
                    logged = true;
                }
            }
            if (!string.IsNullOrEmpty(res.Warnings))
            {
                data.Warnings.Add(res.Warnings);
                if (!logged)
                {
                    Utils.Log.LogCodeToDB(data.Program, "Linux: warnings");
                    logged = true;
                }
            }
            data.Output = res.Output;
            if (!logged)
            {
                Utils.Log.LogCodeToDB(data.Program, "Linux: ok");
                logged = true;
            }
            return data;
        }
    }

    class OutputReader
    {
        StreamReader reader;
        public string Output
        {
            get;
            set;
        }
        StringBuilder sb = new StringBuilder();
        public StringBuilder Builder
        {
            get
            {
                return sb;
            }
        }
        public OutputReader(StreamReader reader)
        {
            this.reader = reader;
        }

        public void ReadOutput()
        {
            try
            {                
                int bufferSize = 40000;
                byte[] buffer = new byte[bufferSize];
                int outputLimit = 200000;
                int count;
                bool addMore = true;
                while (true)
                {
                    Thread.Sleep(10);
                    count = reader.BaseStream.Read(buffer, 0, bufferSize);
                    if (count != 0)
                    {
                        if (addMore)
                        {
                            sb.Append(Encoding.UTF8.GetString(buffer, 0, count));
                            if (sb.Length > outputLimit)
                            {
                                sb.Append("\n\n...");
                                addMore = false;
                            }
                        }
                    }
                    else
                        break;
                }
                Output = sb.ToString();
            }
            catch (Exception e)
            {
                Output = string.Format("Error while reading output: {0}", e.Message);
            }
        }
    }
}
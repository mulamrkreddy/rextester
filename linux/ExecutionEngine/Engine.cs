using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.IO.Compression;

namespace ExecutionEngine
{
	public class InputData
	{
		public string Program
		{
			get;
			set;
		}
		
		public Languages Lang
		{
			get;
			set;
		}		
	}
	
	public class OutputData
	{
		public string Output
		{
			get;
			set;			
		}
		public string Errors
		{
			get;
			set;			
		}
		public string Warnings
		{
			get;
			set;
		}
		public string Stats
		{
			get;
			set;
		}	
		public string Exit_Status
		{
			get;
			set;
		}
		public int ExitCode
		{
			get;
			set;
		}
		public string System_Error
		{
			get;
			set;
		}
	}
	public enum Languages
	{
		Java,
		Python,
		C,
		CPP			
	}	
	
	class CompilerData
	{
		public bool Success
		{
			get;
			set;
		}
		public string Warning
		{
			get;
			set;			
		}
		public string Error
		{
			get;
			set;
		}
		public string Executor
		{
			get;
			set;			
		}			
		public string ExecuteThis
		{
			get;
			set;
		}
		public string CleanThis
		{
			get;
			set;			
		}
	}
	
	public class Engine
	{
		public Engine ()
		{}
		
		public string RootPath
		{
			get
			{
				return @"/var/www/service/usercode/";				
			}
		}
		
		string ParentRootPath
		{
			get
			{
				return @"/var/www/service/";				
			}
		}
		
		public OutputData DoWork(InputData idata)
		{
			CompilerData cdata = null;
			try
			{				
				OutputData odata = new OutputData();	
				cdata = CreateExecutable(idata);				
				if(!cdata.Success)
				{
					odata.Errors = cdata.Error;
					odata.Warnings = cdata.Warning;
					odata.Stats = string.Format("Compilation time: {0} sec", Math.Round((double)CompileTimeMs/(double)1000), 2);
					return odata;
				}
				if(!string.IsNullOrEmpty(cdata.Warning))
				{
					odata.Warnings = cdata.Warning;
				}
				
				Stopwatch watch = new Stopwatch();
				watch.Start();
				using(Process process = new Process())
				{
					process.StartInfo.FileName = ParentRootPath+"parent.py";
					process.StartInfo.Arguments = cdata.Executor+(string.IsNullOrEmpty(cdata.Executor) ? "" : " ")+cdata.ExecuteThis;
					process.StartInfo.UseShellExecute = false;
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.RedirectStandardError = true;
					process.StartInfo.RedirectStandardOutput = true;
	
					
					process.Start();
					
					OutputReader output = new OutputReader(process.StandardOutput);
	                Thread outputReader = new Thread(new ThreadStart(output.ReadOutput));
	                outputReader.Start();
	                OutputReader error = new OutputReader(process.StandardError);
	                Thread errorReader = new Thread(new ThreadStart(error.ReadOutput));
	                errorReader.Start();
				
					process.WaitForExit();

					errorReader.Join(5000);
	                outputReader.Join(5000);
					
					if(!string.IsNullOrEmpty(error.Output))
					{
						int index = error.Output.LastIndexOf('\n');
						int exitcode;
						if(index != -1 && index+1 < error.Output.Length && Int32.TryParse(error.Output.Substring(index+1), out exitcode))
				 	    {
							odata.ExitCode = exitcode;
							switch(exitcode)
							{
								case -8:
									odata.Exit_Status = "Floating point exception (SIGFPE)";
									break;
								case -9:
									odata.Exit_Status = "Kill signal (SIGKILL)";
									break;
								case -11:
									odata.Exit_Status = "Invalid memory reference (SIGSEGV)";
									break;
								case -6:
									odata.Exit_Status = "Abort signal from abort(3) (SIGABRT)";
									break;
								case -4:
									odata.Exit_Status = "Illegal instruction (SIGILL)";
									break;
								case -13:
									odata.Exit_Status = "Broken pipe: write to pipe with no readers (SIGPIPE)";
									break;
								case -14:
									odata.Exit_Status = "Timer signal from alarm(2) (SIGALRM)";
									break;	
								case -15:
									odata.Exit_Status = "Termination signal (SIGTERM)";
									break;
								case -19:
									odata.Exit_Status = "Stop process (SIGSTOP)";
									break;	
								case -17:
									odata.Exit_Status = "Child stopped or terminated (SIGCHLD)";
									break;
								default:
									odata.Exit_Status = string.Format("Exit code: {0} (see 'man 7 signal' for explanation)", exitcode);
									break;
							}
							error.Output = error.Output.Substring(0, index);
						}					
					}
					odata.Errors = error.Output;
					odata.Output = output.Output;	
				}
				watch.Stop();

				if(idata.Lang != Languages.Python)
				{
					odata.Stats = string.Format("Compilation time: {0} sec, absolute running time: {1} sec", Math.Round((double)CompileTimeMs / (double)1000, 2), Math.Round((double)watch.ElapsedMilliseconds / (double)1000, 2));
				}
				else
				{
					odata.Stats = string.Format("Absolute running time: {0} sec", Math.Round((double)watch.ElapsedMilliseconds / (double)1000, 2));
				}
				return odata;
			}
			catch(Exception ex)
			{
				return new OutputData()
					{
						System_Error = ex.Message
					};
			}
			finally
			{
				if(cdata != null)
					Cleanup(cdata.CleanThis);
			}
		}
		
	
		private void Cleanup(string dir)
		{
			
			try
			{
				//cleanup
				Directory.Delete(dir, true);
			}
			catch(Exception){}
		}
		CompilerData CreateExecutable(InputData input)
		{
			string ext = "";
			string compiler = "";
			string args = "";
			string rand = RandomString();
			string dir = rand + "/";
			switch(input.Lang)
			{
				case Languages.Java:
					ext = ".java";
					break;
				case Languages.Python:
					ext = ".py";
					break;
				case Languages.C:
					ext = ".c";
					break;	
				case Languages.CPP:
					ext = ".cpp";
					break;
				default:
					ext = ".unknown";
					break;
			}
			string PathToSource = RootPath+dir+rand+ext;
			Directory.CreateDirectory(RootPath+dir);
			using(TextWriter sw = new StreamWriter(PathToSource))
			{
				sw.Write(input.Program);				
			}
			CompilerData cdata = new CompilerData();
			cdata.CleanThis = RootPath+dir;
			List<string> res = new List<string>();
			switch(input.Lang)
			{
				case Languages.Java:					
					compiler = "javac";
					args = " -Xlint -encoding UTF-8 " + PathToSource;
					res = CallCompiler(compiler, args);
					if(!File.Exists(RootPath+dir+"Rextester.class"))
					{
						if(res.Count > 1)
						{
							if(string.IsNullOrEmpty(res[0]) && string.IsNullOrEmpty(res[1]))
								cdata.Error = "Entry class 'Rextester' not found.";
							else
								cdata.Error = ConcatenateString(res[0], res[1]);
						}
						cdata.Success = false;
						return cdata;		
					}
					if(res.Count > 1 && (!string.IsNullOrEmpty(res[0]) || !string.IsNullOrEmpty(res[1])))
						cdata.Warning = ConcatenateString(res[0], res[1]);
					cdata.ExecuteThis = "-Dfile.encoding=UTF-8 -classpath " +RootPath+dir+" Rextester";		
					cdata.Executor = "java";					
					cdata.Success = true;
					return cdata;
				case Languages.Python:
					cdata.ExecuteThis = PathToSource;
					cdata.Executor = "python";
					cdata.Success = true;
					return cdata;
				case Languages.C:
					compiler = "gcc";
					args = "-Wall -o " + RootPath + dir + "a.out " + PathToSource;
					res = CallCompiler(compiler, args);
					if(!File.Exists(RootPath+dir+"a.out"))
					{
						if(res.Count > 1)
						{
							cdata.Error = ConcatenateString(res[0], res[1]);
							if(cdata.Error != null)
							{
								string[] ew = cdata.Error.Split(new string[]{"\n"}, StringSplitOptions.RemoveEmptyEntries);
								string error = "";
								string warning = "";
								foreach(var a in ew)
									if(a.Contains("error: "))
										error+=a+"\n";
									else if(a.Contains("warning: "))
										warning+=a+"\n";
								cdata.Error = error;
								cdata.Warning = warning;
							}
						}	
						cdata.Success = false;
						return cdata;
					}
					if(res.Count > 1 && (!string.IsNullOrEmpty(res[0]) || !string.IsNullOrEmpty(res[1])))
						cdata.Warning = ConcatenateString(res[0], res[1]);
					cdata.ExecuteThis = RootPath+dir+"a.out";
					cdata.Executor = "";
					cdata.Success = true;
					return cdata;	
				case Languages.CPP:
					compiler = "g++";
					args = "-Wall -o " + RootPath + dir + "a.out " + PathToSource;
					res = CallCompiler(compiler, args);
					if(!File.Exists(RootPath+dir+"a.out"))
					{
						if(res.Count > 1)
						{
							cdata.Error = ConcatenateString(res[0], res[1]);
							if(cdata.Error != null)
							{
								string[] ew = cdata.Error.Split(new string[]{"\n"}, StringSplitOptions.RemoveEmptyEntries);
								string error = "";
								string warning = "";
								foreach(var a in ew)
									if(a.Contains("error: "))
										error+=a+"\n";
									else if(a.Contains("warning: "))
										warning+=a+"\n";
								cdata.Error = error;
								cdata.Warning = warning;
							}
						}
						cdata.Success = false;
						return cdata;
					}
					if(res.Count > 1 && (!string.IsNullOrEmpty(res[0]) || !string.IsNullOrEmpty(res[1])))
						cdata.Warning = ConcatenateString(res[0], res[1]);
					cdata.ExecuteThis = RootPath+dir+"a.out";
					cdata.Executor = "";
					cdata.Success = true;
					return cdata;
				default:
					
					break;
			}
			cdata.Success = false;
			return cdata;			
		}
		
		string ConcatenateString(string a, string b)
		{
			if(string.IsNullOrEmpty(a))
				return b;
			if(string.IsNullOrEmpty(b))
				return a;
			
			return a+"\n\n"+b;
		}
		long CompileTimeMs
		{
			get;
			set;
		}
		List<string> CallCompiler(string compiler, string args)
		{
			Stopwatch watch = new Stopwatch();			
			using(Process process = new Process())
			{
				process.StartInfo.FileName = compiler;
				process.StartInfo.Arguments = args;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.RedirectStandardError = true;
				process.StartInfo.RedirectStandardOutput = true;				
				
				watch.Start();
				process.Start();
				
				OutputReader output = new OutputReader(process.StandardOutput, 100);
                Thread outputReader = new Thread(new ThreadStart(output.ReadOutput));
                outputReader.Start();
                OutputReader error = new OutputReader(process.StandardError, 100);
                Thread errorReader = new Thread(new ThreadStart(error.ReadOutput));
                errorReader.Start();
				
				process.WaitForExit();				
				watch.Stop();

				CompileTimeMs = watch.ElapsedMilliseconds;
				errorReader.Join(5000);
                outputReader.Join(5000);
				
				List<string> compOutput = new List<string>();
				compOutput.Add(output.Output);
				compOutput.Add(error.Output);
				return compOutput;			
			}
		}
		
		string RandomString()
		{
			Random rg = new Random();
			return rg.Next(1, 1000000).ToString();
		}
		
		public Languages String2Lang(string lang)
		{
			switch(lang)
			{
				case "Java":
					return Languages.Java;
				case "Python":
					return Languages.Python;
				case "C":
					return Languages.C;
				case "CPP":
					return Languages.CPP;
				default:
					return Languages.Java;				
			}			
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
        public OutputReader(StreamReader reader, int interval = 10)
        {
            this.reader = reader;
			this.CheckInterval = interval;
        }

		int CheckInterval
		{
			get;
			set;
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
                    Thread.Sleep(CheckInterval);
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






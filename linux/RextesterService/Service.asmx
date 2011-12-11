<%@ WebService Language="C#" Class="RextesterService.Service" %>

using System;
using System.Web.Services;
using System.Collections.Generic;
using ExecutionEngine;

namespace RextesterService
{
	[WebService (Namespace = "http://rextester.com/")]
	public class Service : WebService
	{
		[WebMethod]
		public Result DoWork(string Program, Languages Language, string user, string pass, bool bytes = false)
		{
			if(user != "test" || pass != "test")
			{
				return new Result()
				{
					Errors = null,
					Warnings = null,
					Output = null,
					Stats = null,
					Exit_Status = null,
					Exit_Code = null,
					System_Error = "Not authorized."			
				};
			}
			Engine engine = new Engine();
			InputData idata = new InputData()
			{
				Program = Program,
				Lang = Language
			};

			var odata = engine.DoWork(idata);		
			
			var res = new Result()
				{
					Errors = !string.IsNullOrEmpty(odata.Errors) ? odata.Errors.Replace(engine.RootPath, "") : odata.Errors, 
					Warnings = !string.IsNullOrEmpty(odata.Warnings) ? odata.Warnings.Replace(engine.RootPath, "") : odata.Warnings,
					Output = odata.Output,
					Stats = odata.Stats,
					Exit_Status = odata.Exit_Status,
					Exit_Code = odata.ExitCode,
					System_Error = odata.System_Error			
				};
				if(bytes)
				{
					if(!string.IsNullOrEmpty(res.Errors))
					{
						res.Errors_Bytes =  System.Text.Encoding.Unicode.GetBytes(res.Errors);
						res.Errors = null;
					}
					if(!string.IsNullOrEmpty(res.Warnings))
					{
						res.Warnings_Bytes =  System.Text.Encoding.Unicode.GetBytes(res.Warnings);
						res.Warnings = null;
					}
					if(!string.IsNullOrEmpty(res.Output))
					{
						res.Output_Bytes =  System.Text.Encoding.Unicode.GetBytes(res.Output);
						res.Output = null;
					}
				}
			return res;			
		}
		
		[WebMethod]
		public int Sum(int a, int b)
		{
			return a+b;
		}
	}
	
	
	public class Result
	{
		public string Errors
		{
			get;
			set;
		}
		
		public byte[] Errors_Bytes
		{
			get;
			set;
		}
		
		public string Warnings
		{
			get;
			set;		
		}
		
		public byte[] Warnings_Bytes
		{
			get;
			set;
		}
		
		public string Output
		{
			get;
			set;		
		}
		
		public byte[] Output_Bytes
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
		public int? Exit_Code
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
}




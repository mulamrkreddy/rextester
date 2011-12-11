using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class LinuxService
    {
        public linux.Result DoWork(string Program, linux.Languages Language)
        {
            using (var service = new linux.Service())
            {
                try
                {
                    bool bytes = true;
                    var res = service.DoWork(Program, Language, "test", "test", bytes);

                    if (bytes)
                    {
                        if (res.Errors_Bytes != null)
                            res.Errors = System.Text.Encoding.Unicode.GetString(res.Errors_Bytes);
                        if (res.Warnings_Bytes != null)
                            res.Warnings = System.Text.Encoding.Unicode.GetString(res.Warnings_Bytes);
                        if (res.Output_Bytes != null)
                            res.Output = System.Text.Encoding.Unicode.GetString(res.Output_Bytes);
                    }
                    return res;
                }
                catch (Exception ex)
                {
                    return new linux.Result()
                    {
                        System_Error = string.Format("Error while calling service: {0}", ex.Message)
                    };
                }
            }
        }
    }
}

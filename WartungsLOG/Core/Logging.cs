using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace WartungsLOG.Core
{
    public static class Logging
    {
        public static void logMe(string log, [CallerMemberName] string cmn = "", [CallerLineNumber] int cln = 0, [CallerFilePath] string cfp = "")
        {
            
            cfp = cfp.Replace( @"C:\Users\abc\source\repos\WartungsLOG\WartungsLOG\" ,"" );

            Debug.WriteLine($"XXXX: {cmn} {cfp}@{cln}: {log}");
        }
    }
}

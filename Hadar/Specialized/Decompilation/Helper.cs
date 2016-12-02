//
//  File Name: Helper.cs
//  Project Name: Hadar
//
//  Created by Alexandro Luongo on 2/12/2016.
//  Copyright © 2012-2016 Alexandro Luongo. All rights reserved.
//


using System.Diagnostics;
using System.IO;

namespace Hadar.Decompilation
{
    /// <summary>
    /// Helper: Utilities used to decompile/disassemble Sockwave flash files.
    /// </summary>
    internal static class Helper
    {
        private static readonly string DIRECTORY = Program.DIRECTORY;

        /// <summary>
        /// Decompile a Shockwave Flash file using RABCDasm. 
        /// </summary>
        /// <param name="SWF">Name of the Sockwave Flash file to decompile.</param>
        /// <returns>Returns a boolean value representing the result of the operation.</returns>
        internal static bool Decompile(string SWF)
        {
            var Source = SWF.Substring(0, SWF.LastIndexOf('.'));

            Execute("Decompile.bat", Source);

            return File.Exists(Path.Combine(DIRECTORY, string.Format("{0}-0.abc", Source)));
        }

        /// <summary>
        /// Execute a Batch file with specified arguments.
        /// </summary>
        /// <param name="BAT">Name of the Batch file.</param>
        /// <param name="Arguments">Arguments to pass to the Batch file.</param>
        private static void Execute(string BAT, string Arguments)
        {
            if (File.Exists(Path.Combine(DIRECTORY, BAT)))
            {
                var Details = new ProcessStartInfo(BAT, Arguments);
                Details.WorkingDirectory = DIRECTORY;

                Process Process = Process.Start(Details);
                Process.Start(Details);

                Process.WaitForExit();
            }
        }
    }
}

//
//  File Name: Helper.cs
//  Project Name: Hadar
//
//  Created by Alexandro Luongo on 2/12/2016.
//  Copyright © 2012-2016 Alexandro Luongo. All rights reserved.
//


using System;
using System.Diagnostics;
using System.IO;

namespace Hadar.Decompilation
{
	internal static class Helper
	{
        private static readonly string DIRECTORY = Program.DIRECTORY;

        internal static bool Decompile(string SWF)
		{
            var Source = SWF.Substring(0, SWF.LastIndexOf('.'));

            Execute("Decompile.bat", Source);

            return File.Exists(Path.Combine(DIRECTORY, string.Format("{0}-0.abc", Source)));
        }

		private static void Execute(string BAT, string Argument)
		{
			if (File.Exists(Path.Combine(DIRECTORY, BAT)))
			{
                var Details = new ProcessStartInfo(BAT, Argument);
				Details.WorkingDirectory = DIRECTORY;

				Process Process = Process.Start(Details);
				Process.Start(Details);

				Process.WaitForExit();
			}
		}
	}
}

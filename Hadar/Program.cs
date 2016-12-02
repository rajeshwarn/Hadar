//
//  File Name: program.cs
//  Project Name: Hadar
//
//  Created by Alexandro Luongo on 2/12/2016.
//  Copyright © 2012-2016 Alexandro Luongo. All rights reserved.
//


using Hadar.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hadar
{
	internal class Program
	{
		internal static readonly string DIRECTORY = "Hacking";

		private static void Init()
		{
			Console.Title = "Hadar - DarkOrbit Cracking Utility";

			Console.WriteLine("Hadar - DarkOrbit Cracking Utility");
			Console.WriteLine("Copyright (C) 2012/2016 - W00dL3cs (Alexandro Luongo)");
			Console.WriteLine();

			Directory.CreateDirectory(DIRECTORY);
		}

		static void Main(string[] args)
		{
			try
			{
				Init();

				var SWF = GetSWF();

				var Session = Decompile(SWF);

                var Build = Handle(Session);

                Export(Build, "Protocol.xml");
			}
			catch (Exception e)
			{
				Console.WriteLine("Fatal exception occurred!");
				Console.WriteLine(string.Format("Error message: {0}", e.Message));
				Console.WriteLine();
			}
			finally
			{
				Terminate();
			}
		}

		private static FileInfo GetSWF()
		{
			Console.Write("Name of the SWF to hack: ");

			string Source = Console.ReadLine();

			Console.WriteLine();
			Console.WriteLine("Searching for SWF...");

			if (!File.Exists(Path.Combine(DIRECTORY, Source)))
			{
				throw new FileNotFoundException(string.Format("Cannot find file \"{0}\" inside \"{1}\" folder!", Source, DIRECTORY));
			}

            Console.WriteLine("SWF found!");
            Console.WriteLine();

			return new FileInfo(string.Format(@"{0}/{1}", DIRECTORY, Source));
		}

		private static Decompilation.Session Decompile(FileInfo SWF)
		{
			var Session = new Decompilation.Session(SWF);

            Console.WriteLine("Performing decompilation...");

			if (!Session.Decompile())
            {
                throw new Exception(string.Format("Cannot decompile file \"{0}\" !", SWF.Name));
            }

            Console.WriteLine(string.Format("SWF decompiled: {0} classes found!", Session.Classes.Count));
            Console.WriteLine();

            return Session;
		}

        private static Build Handle(Decompilation.Session Session)
        {
            Game.Commands.Manager Commands = null;
            Game.Handlers.Manager Handlers = null;

            List<Game.Command> Events = new List<Command>();

            Console.WriteLine("Parsing commands...");

            foreach (var Command in Session.Classes.Where(x => x.Directory.Name == "netbigpointdarkorbitnetnettycommands"))
            {
                var Interface = Session.ParseCommand(Command);

                if (Interface is Game.Commands.Manager)
                {
                    Commands = Interface as Game.Commands.Manager;
                }
                else
                {
                    Events.Add(Interface as Game.Command);
                }
            }

            Commands.SetCommands(Events);

            Console.WriteLine("Parsing handlers...");

            foreach (var Handler in Session.Classes.Where(x => x.Directory.Name == "netbigpointdarkorbitnetnetty"))
            {
                var Interface = Session.ParseHandler(Handler);

                if (Interface is Game.Handlers.Manager)
                {
                    Handlers = Interface as Game.Handlers.Manager;

                    break;
                }
            }

            Console.WriteLine(string.Format("Successfully loaded: {0} commands and {1} handlers!", Commands.Commands.Count, Handlers.Handlers.Count));
            Console.WriteLine();

            return new Build(Commands, Handlers);
        }

        private static void Export(Build Build, string Destination)
        {
            Console.WriteLine(string.Format("Exporting game messages to \"{0}\"...", Destination));

            Build.Save(Destination);
        }

		private static void Terminate()
		{
			Console.WriteLine("Press a key to terminate.");
			Console.ReadKey();

			Environment.Exit(0);
		}
	}
}

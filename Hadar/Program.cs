//
//  File Name: Program.cs
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
        /// <summary>
        /// Session: A decompilation session.
        /// </summary>
        internal static Decompilation.Session Session;

        /// <summary>
        /// DIRECTORY: Hadar hacking directory.
        /// </summary>
        internal static readonly string DIRECTORY = "Hacking";

        /// <summary>
        /// Print informations, create working directory.
        /// </summary>
        private static void Init()
        {
            Console.Title = "Hadar - DarkOrbit Cracking Utility";

            Console.WriteLine("Hadar - DarkOrbit Cracking Utility");
            Console.WriteLine("Copyright (C) 2012/2016 - W00dL3cs (Alexandro Luongo)");
            Console.WriteLine();

            Directory.CreateDirectory(DIRECTORY);
        }

        /// <summary>
        /// Entry point of the application.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                Init();

                var SWF = GetSWF();

                Session = Decompile(SWF);

                var Build = Handle(Session);

                Export(Build, "Protocol.xml");

                Clear(Session);
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

        /// <summary>
        /// Prompt the user for the Shockwave Flash file
        /// which will be decompiled.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Decompiles a Sockwave Flash file.
        /// </summary>
        /// <param name="SWF">The SWF to decompile.</param>
        /// <returns>Returns a decompilation session.</returns>
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

        /// <summary>
        /// Handle message informations contained in a decompilation session.
        /// </summary>
        /// <param name="Session">The decompilation session containing the informations.</param>
        /// <returns>Returns a Game.Build</returns>
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

        /// <summary>
        /// Export game messages contained in a game build to
        /// human-readable format.
        /// </summary>
        /// <param name="Build">The game build containing the informations.</param>
        /// <param name="Destination">The name of the destination file.</param>
        private static void Export(Build Build, string Destination)
        {
            Console.WriteLine(string.Format("Exporting game messages to \"{0}\"...", Destination));

            Build.Save(Destination);
        }

        /// <summary>
        /// Clean temp and useless files 
        /// generated by a decompilation session.
        /// </summary>
        /// <param name="Session">The specified decompilation session.</param>
        private static void Clear(Decompilation.Session Session)
        {
            Console.WriteLine("Cleaning trash...");

            //Session.Clear();

            Console.WriteLine();
        }

        /// <summary>
        /// Safely terminate the execution.
        /// </summary>
        private static void Terminate()
        {
            Console.WriteLine("Press a key to terminate.");
            Console.ReadKey();

            Environment.Exit(0);
        }
    }
}

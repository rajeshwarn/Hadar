//
//  File Name: Session.cs
//  Project Name: Hadar
//
//  Created by Alexandro Luongo on 2/12/2016.
//  Copyright © 2012-2016 Alexandro Luongo. All rights reserved.
//


using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hadar.Decompilation
{
    /// <summary>
    /// Session: Represents a Shockwave Flash decompilation session.
    /// </summary>
    internal class Session
    {
        /// <summary>
        /// SWF: Shockwave Flash file used in this session. 
        /// </summary>
        private FileInfo SWF;

        /// <summary>
        /// Source: Name used from decompilation utilities.
        /// </summary>
        internal string Source;

        /// <summary>
        /// Classes: Collection of disassembled classes.
        /// </summary>
        internal List<FileInfo> Classes;

        internal Session(FileInfo SWF)
        {
            this.SWF = SWF;

            this.Source = SWF.Name.Substring(0, SWF.Name.LastIndexOf('.'));
        }

        /// <summary>
        /// Perform decompilation.
        /// 
        /// Side note: it's more "disassembling" than "decompiling".
        /// </summary>
        /// <returns></returns>
        internal bool Decompile()
        {
            var Result = Helper.Decompile(SWF.Name);

            Classes = SWF.Directory.GetFiles("*.class.asasm", SearchOption.AllDirectories).ToList();

            return Result;
        }

        /// <summary>
        /// Analyse a commands-related disassembled class.
        /// </summary>
        /// <param name="Class">Disassembled class to analyse.</param>
        /// <returns>Returns a Game.Interface which represents the class.</returns>
        internal Game.Interface ParseCommand(FileInfo Class)
        {
            foreach (var Line in File.ReadAllLines(Class.FullName))
            {
                if (Line.Contains("implements") && Line.Contains("net.bigpoint.darkorbit.net.netty"))
                {
                    return new Game.Command(Class);
                }
            }

            return new Game.Commands.Manager(Class);
        }

        /// <summary>
        /// Analyse an handlers-related disassembled class.
        /// </summary>
        /// <param name="Class">Disassembled class to analyse.</param>
        /// <returns>Returns a Game.Interface which represents the class.</returns>
        internal Game.Interface ParseHandler(FileInfo Class)
        {
            foreach (var Line in File.ReadAllLines(Class.FullName))
            {
                if (Line.Contains("HandlerLookup"))
                {
                    return new Game.Handlers.Manager(Class);
                }
            }

            return new Game.Handler(Class);
        }

        /// <summary>
        /// Clean temp and useless files.
        /// </summary>
        internal void Clear()
        {
            foreach (var ABC in Directory.GetFiles(Program.DIRECTORY, "*.abc"))
            {
                File.Delete(ABC);
            }

            foreach (var Folder in Directory.GetDirectories(Program.DIRECTORY, string.Format("{0}-*", Source)))
            {
                Directory.Delete(Folder, true);
            }
        }
    }
}

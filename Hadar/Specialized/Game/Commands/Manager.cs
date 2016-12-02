//
//  File Name: Manager.cs
//  Project Name: Hadar
//
//  Created by Alexandro Luongo on 2/12/2016.
//  Copyright © 2012-2016 Alexandro Luongo. All rights reserved.
//


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hadar.Game.Commands
{
    internal class Manager : Interface
    {
        private Dictionary<int, string> Events;
        internal Dictionary<int, Command> Commands { get; private set; }
        
        public Manager(FileInfo Class) : base(Class)
        {
            Events = new Dictionary<int, string>();
            Commands = new Dictionary<int, Command>();

            for (int i = 0; i < Lines.Length; i++)
            {
                var Line = Lines[i];

                if (Line.Contains("getproperty") && Line.Contains("commandLookup"))
                {
                    var ID = Lines[++i];
                    var Command = Lines[++i];

                    if ((ID.Contains("pushbyte") || ID.Contains("pushshort")) && Command.Contains("getlex"))
                    {
                        ID = ID.Substring(ID.LastIndexOf(" ") + 1);
                        Command = Command.Split('"')[3];

                        Events.Add(int.Parse(ID), Command);
                    }
                }
            }
        }

        internal void SetCommands(List<Command> Commands)
        {
            foreach (var Event in Events)
            {
                this.Commands.Add(Event.Key, Commands.FirstOrDefault(x => x.Class == Event.Value));
            }
        }
    }
}

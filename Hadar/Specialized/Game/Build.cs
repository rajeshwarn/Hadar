//
//  File Name: Build.cs
//  Project Name: Hadar
//
//  Created by Alexandro Luongo on 2/12/2016.
//  Copyright © 2012-2016 Alexandro Luongo. All rights reserved.
//


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Hadar.Game
{
    internal class Build
    {
        private Commands.Manager Commands;
        private Handlers.Manager Handlers;

        internal Build(Commands.Manager Commands, Handlers.Manager Handlers)
        {
            this.Commands = Commands;
            this.Handlers = Handlers;
        }

        internal void Save(string Destination)
        {
            using (var Writer = new XmlTextWriter(Destination, Encoding.Default))
            {
                Writer.WriteStartDocument();
                Writer.WriteWhitespace("\n");

                Writer.WriteStartElement("Messages");
                Writer.WriteWhitespace("\n");

                foreach (var Command in Commands.Commands.OrderBy(x => x.Key))
                {
                    Writer.WriteWhitespace("	");

                    Writer.WriteStartElement("incoming");

                    Writer.WriteAttributeString("id", Command.Key.ToString());
                    Writer.WriteAttributeString("class", Command.Value.OPCode);

                    Writer.WriteEndElement();
                    Writer.WriteWhitespace("\n");
                }

                Writer.WriteWhitespace("\n");

                foreach (var Handler in Handlers.Handlers.OrderBy(x => x.Key))
                {
                    var Invoker = Commands.Commands.FirstOrDefault(x => x.Value.Class == Handler.Key);

                    Writer.WriteWhitespace("	");

                    Writer.WriteStartElement("outgoing");

                    Writer.WriteAttributeString("id", Invoker.Key.ToString());
                    Writer.WriteAttributeString("class", Handler.Value.OPCode);

                    Writer.WriteEndElement();
                    Writer.WriteWhitespace("\n");
                }

                Writer.WriteEndElement();
            }
        }
    }
}

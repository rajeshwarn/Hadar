//
//  File Name: Build.cs
//  Project Name: Hadar
//
//  Created by Alexandro Luongo on 2/12/2016.
//  Copyright © 2012-2016 Alexandro Luongo. All rights reserved.
//


using System.Linq;
using System.Text;
using System.Xml;

namespace Hadar.Game
{
    /// <summary>
    /// Build: Represents a DarkOrbit game build.
    /// </summary>
    internal class Build
    {
        /// <summary>
        /// Commands: Entity which manages game commands.
        /// </summary>
        private Commands.Manager Commands;

        /// <summary>
        /// Handlers: Entity which manages game handlers.
        /// </summary>
        private Handlers.Manager Handlers;

        internal Build(Commands.Manager Commands, Handlers.Manager Handlers)
        {
            this.Commands = Commands;
            this.Handlers = Handlers;
        }

        /// <summary>
        /// Save an XML-representation of current game messages
        /// inside a specified destination file.
        /// </summary>
        /// <param name="Destination">The name of the destination file.</param>
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

                foreach (var Handler in Handlers.Handlers.ToDictionary(x => Commands.Commands.FirstOrDefault(y => y.Value.Class == x.Key).Key, x => x.Value).OrderBy(x => x.Key))
                {
                    Writer.WriteWhitespace("	");

                    Writer.WriteStartElement("outgoing");

                    Writer.WriteAttributeString("id", Handler.Key.ToString());
                    Writer.WriteAttributeString("class", Handler.Value.OPCode);

                    Writer.WriteEndElement();
                    Writer.WriteWhitespace("\n");
                }

                Writer.WriteEndElement();
            }
        }
    }
}

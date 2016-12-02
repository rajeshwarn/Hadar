//
//  File Name: Interface.cs
//  Project Name: Hadar
//
//  Created by Alexandro Luongo on 2/12/2016.
//  Copyright © 2012-2016 Alexandro Luongo. All rights reserved.
//


using System.IO;

namespace Hadar.Game
{
    /// <summary>
    /// Interface: Represents any message-related entity inside a DarkOrbit client.
    /// </summary>
    internal class Interface
    {
        /// <summary>
        /// Class: Name of the decompiled class this entity refers to.
        /// </summary>
        internal string Class;

        /// <summary>
        /// OPCode: Name of the class this entity refers to.
        /// </summary>
        internal string OPCode;

        /// <summary>
        /// Lines: Content of the decompiled class.
        /// </summary>
        internal string[] Lines;

        internal Interface(FileInfo Class)
        {
            try
            {
                this.Class = Class.Name.Split('.')[0];

                this.Lines = File.ReadAllLines(Class.FullName);

                foreach (var Line in Lines)
                {
                    if (Line.Contains("instance"))
                    {
                        this.Class = Line.Split('"')[3];
                    }
                    else if (Line.Contains("protectedns") && Line.Contains(":"))
                    {
                        this.OPCode = Line.Split('"')[1].Split(':')[1];

                        break;
                    }
                }
            }
            catch { }
        }
    }
}

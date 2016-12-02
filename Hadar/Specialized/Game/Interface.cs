//
//  File Name: Interface.cs
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

namespace Hadar.Game
{
    internal class Interface
    {
        internal string Class;
        internal string OPCode;

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

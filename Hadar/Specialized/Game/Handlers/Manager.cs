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

namespace Hadar.Game.Handlers
{
    internal class Manager : Interface
    {
        internal Dictionary<string, Handler> Handlers;

        public Manager(FileInfo Class) : base(Class)
        {
            Handlers = new Dictionary<string, Handler>();

            for (int i = 0; i < Lines.Length; i++)
            {
                var Line = Lines[i];

                if (Line.Contains("getlex") && Line.Contains("handlerLookup"))
                {
                    var References = Lines[++i];
                    var Junk = Lines[++i];
                    var Name = Lines[++i];

                    if (References.Contains("getlex") && Name.Contains("getlex"))
                    {
                        var Reference = References.Split('"')[3];

                        try
                        {
                            var Data = Name.Split('"');

                            Name = Data[3];

                            Handlers.Add(Reference, new Handler(Path.Combine(Program.DIRECTORY, string.Format("{0}-1", Program.Session.Source), string.Format("{0}", Data[1].Replace(".", "")), string.Format("{0}.class.asasm", Name))));
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
            }
        }
    }
}

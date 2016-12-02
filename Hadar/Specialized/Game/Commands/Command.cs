//
//  File Name: Command.cs
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
    internal class Command : Interface
    {
        private int Invoker;

        private string ReadFunction;
        private string WriteFunction;

        private List<string> OnRead;
        private List<string> OnWrite;

        internal Command(FileInfo Class) : base(Class)
        {
            OnRead = new List<string>();
            OnWrite = new List<string>();

            for (int i = 0; i < Lines.Length; i++)
            {
                var Line = Lines[i];

                if (Line.Contains("findproperty") && Line.Contains("\"ID\""))
                {
                    var Next = Lines[++i];

                    if (Next.Contains("pushbyte") || Next.Contains("pushshort"))
                    {
                        Invoker = int.Parse(Next.Substring(Next.LastIndexOf(" ") + 1));
                    }
                }

                if (Line.Contains("trait method"))
                {
                    var Name = Line.Split('"')[3];

                    i += 3;

                    Line = Lines[i];

                    if (Line.Contains("param") && (Line.Contains("IDataInput") || Line.Contains("IDataOutput")))
                    {
                        var Return = Lines[++i];

                        if (Return.Contains("void"))
                        {
                            for (int j = i + 1; j < Lines.Length; j++, i = j)
                            {
                                Line = Lines[j];

                                if ((Line.Contains("callproperty") || Line.Contains("callpropvoid")))
                                {
                                    if (Line.Contains("IDataInput"))
                                    {
                                        if (ReadFunction == null)
                                        {
                                            ReadFunction = Name;

                                            continue;
                                        }

                                        var Function = Line.Split('"')[3];

                                        if (Function == "readInt")
                                        {
                                            Line = Lines[++j];

                                            if (Line.Contains("readBytes"))
                                            {
                                                Function = Line.Split('"')[3];
                                            }
                                        }

                                        OnRead.Add(Function);
                                    }
                                    else if (Line.Contains("IDataOutput"))
                                    {
                                        if (WriteFunction == null)
                                        {
                                            WriteFunction = Name;

                                            continue;
                                        }

                                        var Function = Line.Split('"')[3];

                                        OnWrite.Add(Function);
                                    }
                                }

                                if (Line.Contains("callpropvoid") && Line.Contains(Name))
                                {
                                    // Reference to another command!

                                    Line = Lines[j - 2];

                                    if (Line.Contains("getproperty"))
                                    {
                                        var Object = Line.Split('"')[3];

                                        for (int x = 0; x < Lines.Length; x++)
                                        {
                                            Line = Lines[x];

                                            if (Line.Contains("initproperty") && Line.Contains(Object))
                                            {
                                                Line = Lines[--x];

                                                if (Line.Contains("constructprop"))
                                                {
                                                    string Other = Line.Split('"')[3];

                                                    if (Name == ReadFunction)
                                                    {
                                                        OnRead.Add("Read(" + Other + ")");
                                                    }
                                                    else if (Name == WriteFunction)
                                                    {
                                                        OnWrite.Add("Write(" + Other + ")");
                                                    }

                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (Line.Contains("returnvoid"))
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

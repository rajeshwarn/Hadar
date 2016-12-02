//
//  File Name: Handler.cs
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
    internal class Handler : Interface
    {
        internal Handler(FileInfo Class) : base(Class)
        {
        }

        internal Handler(string Path) : this(new FileInfo(Path))
        {

        }
    }
}

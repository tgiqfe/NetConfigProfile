﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetConfigProfile.Serialize
{
    /// <summary>
    /// データタイプ判別用enum
    /// </summary>
    internal enum DataType
    {
        None = 0,
        Json = 1,
        Xml = 2,
        Yml = 4,
        Yaml = 4
    }
}

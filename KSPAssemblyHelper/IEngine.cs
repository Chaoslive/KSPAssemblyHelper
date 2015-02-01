using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPAssemblyHelper
{
    interface IEngine
    {
         double Isp { get; set; }
         double Thrust { get; set; }
         bool Enable { get; set; }
    }
}

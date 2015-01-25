using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPAssemblyHelper
{
    class Engine:Part,IStageing
    {
        public int Stage { get; set; }
        public Engine()
        {
            this.Name = "Engine";
        }
    }
}

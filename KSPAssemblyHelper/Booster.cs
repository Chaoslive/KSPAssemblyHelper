using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPAssemblyHelper
{
    class Booster:Part,IStageing
    {
        public int Stage { get; set; }
        public Booster()
        {
            this.Name = "Booster";
        }
    }
}

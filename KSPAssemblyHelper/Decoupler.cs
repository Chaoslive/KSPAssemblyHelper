using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPAssemblyHelper
{
    class Decoupler:Composite,IStageing
    {
        public int Stage { get; set; }
        public Decoupler()
        {
            this.Name = "Decoupler";
        }
    }
}

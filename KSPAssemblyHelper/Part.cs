using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPAssemblyHelper
{
    class Part:Composite
    {
        protected PartSize Size { get; set; }
        protected Part()
        {
            this.Size = PartSize.Regular;
        }
        protected Part(PartSize Size)
        {
            this.Size = Size;
        }
    }
}

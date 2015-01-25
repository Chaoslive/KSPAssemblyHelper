using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPAssemblyHelper
{
    class Part:Composite
    {
        protected PartSize Size { get; set; }
        /// <summary>
        /// 対象配置の数
        /// </summary>
        protected int Count { get; set; }
        protected Part()
        {
            this.Size = PartSize.Small;
        }
        protected Part(PartSize Size)
        {
            this.Size = Size;
        }
    }
}

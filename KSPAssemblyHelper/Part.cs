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
        //public int Count { get; set; }
        public double Mass { get; set; }
        public int Cost { get; set; }
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

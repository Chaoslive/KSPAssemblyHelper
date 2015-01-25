using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace KSPAssemblyHelper
{
    class FuelTank:Part
    {
        public FuelTank()
        {
            this.Name = "FuelTank";
        }
        void PrintSize()
        {
            Debug.Print(Size.ToString());
        }
    }

    
}

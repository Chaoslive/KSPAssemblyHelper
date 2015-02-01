using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPAssemblyHelper
{
    
    class Decoupler:Part,IStageing
    {
        public override double GetMassNow()
        {
            return this.Mass;
        }

        public Decoupler(string Name, PartSize Size, int Cost, double Mass, int Stage,int Count)
        {
            this.Name = Name;
            this.Size = Size;
            this.Cost = Cost;
            this.Mass = Mass;
            this.Stage = Stage;
            this.Count = Count;
        }

        public Decoupler Clone()
        {
            //var r = (Booster)MemberwiseClone();
            //r.Children.Clear();
            return new Decoupler(this.Name, this.Size, this.Cost, this.Mass,this.Stage,this.Count);
        }
    }
}

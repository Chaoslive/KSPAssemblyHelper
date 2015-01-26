using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPAssemblyHelper
{
    class Booster:Part,IStageing
    {
        public double WetMass;
        public double DryMass;
        public double Thrust;
        public double Isp;

        public int Stage { get; set; }
        public Booster()
        {
            this.Name = "Booster";
        }

        public Booster(string Name, PartSize Size, int Cost, double WetMass, double DryMass, double Thrust, double Isp, int Stage, int Count)
        {
            // TODO: Complete member initialization
            this.Name = Name;
            this.Size = Size;
            this.Cost = Cost;
            this.WetMass = WetMass;
            this.DryMass = DryMass;
            this.Thrust = Thrust;
            this.Isp = Isp;
            this.Stage = Stage;
            this.Count = Count;
        }


        public Booster Clone()
        {
            //var r = (Booster)MemberwiseClone();
            //r.Children.Clear();
            return new Booster(this.Name,this.Size,this.Cost,this.WetMass,this.DryMass,this.Thrust,this.Isp,this.Stage,this.Count);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace KSPAssemblyHelper
{
    class Booster : Part, IStageing, IEngine
    {
        public double WetMass;
        public double DryMass;
        public double Thrust { get; set; }
        public double Isp { get; set; }
        public double FuelMassNow { get; set; }
        public bool Enable { get; set; }
        public override double GetMassNow()
        {
            //Debug.Print(this.Name + "Mass:" + (DryMass + (WetMass - DryMass) * FuelRate).ToString());
            return (DryMass + FuelMassNow);
        }

        public Booster(string Name, PartSize Size, int Cost, double WetMass, double DryMass, double Thrust, double Isp, int Stage)
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
            this.Count = 1;
            this.FuelMassNow = WetMass - DryMass;
            this.Enable = true;
        }

        public Booster Clone()
        {
            //var r = (Booster)MemberwiseClone();
            //r.Children.Clear();
            return new Booster(this.Name, this.Size, this.Cost, this.WetMass, this.DryMass, this.Thrust, this.Isp, this.Stage);
        }









    }
}

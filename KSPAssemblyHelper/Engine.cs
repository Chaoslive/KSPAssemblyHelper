using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPAssemblyHelper
{
    class Engine:Part,IStageing
    {

        public int Stage { get; set; }
        public double Isp { get; set; }
        public double Thrust { get; set; }
        
        

        public Engine(string Name,PartSize Size,int Cost,double Mass,double Thrust,double Isp,int Stage)
        {
            this.Name = Name;
            this.Size = Size;
            this.Cost = Cost;
            this.Mass = Mass;
            this.Thrust = Thrust;
            this.Isp = Isp;
            this.Stage = Stage;
        }



      
        
        public Engine Clone()
        {
            //Engine r = (Engine)MemberwiseClone();
            //r.Children.Clear();
            return new Engine(this.Name, this.Size, this.Cost, this.Mass, this.Thrust, this.Isp, this.Stage);
        }
    }
}

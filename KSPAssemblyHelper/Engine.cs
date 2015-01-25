﻿using System;
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
        public double Mass { get; set; }
        public int Cost{get;set;}
        public Engine()
        {
            this.Name = "Engine";
        }

        public Engine(string Name,PartSize Size,int Cost,double Mass,double Thrust,double Isp,int Stage,int Count)
        {
            this.Name = Name;
            this.Size = Size;
            this.Cost = Cost;
            this.Mass = Mass;
            this.Thrust = Thrust;
            this.Isp = Isp;
            this.Stage = Stage;
            this.Count = Count;
        }



      
        
        public Engine Clone()
        {
            return (Engine)MemberwiseClone();
        }
    }
}
